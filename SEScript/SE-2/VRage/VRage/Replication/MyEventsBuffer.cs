using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace VRage.Replication
{
	public class MyEventsBuffer : IDisposable
	{
		public delegate void Handler(MyPacketDataBitStreamBase data, NetworkId objectInstance, NetworkId blockedNetId, uint eventId, EndpointId sender, Vector3D? position);

		public delegate bool IsBlockedHandler(NetworkId objectInstance, NetworkId blockedNetId);

		private class MyBufferedEvent
		{
			/// <summary>
			/// Stream with the event
			/// </summary>
			public MyPacketDataBitStreamBase Data;

			/// <summary>
			/// Target object net id of the event.
			/// </summary>
			public NetworkId TargetObjectId;

			/// <summary>
			/// Object network id that is blocking this event. If 'NetworkId.Invalid' than no blocking object.
			/// </summary>
			public NetworkId BlockingObjectId;

			public uint EventId;

			public EndpointId Sender;

			/// <summary>
			/// Indicates if this event is a barrier.
			/// </summary>
			public bool IsBarrier;

			public Vector3D? Position;
		}

		private struct MyObjectEventsBuffer
		{
			/// <summary>
			/// Events to be processed for network object.
			/// </summary>
			public Queue<MyBufferedEvent> Events;

			/// <summary>
			/// Indicates if events are currently processed.
			/// </summary>
			public bool IsProcessing;
		}

		private readonly Stack<MyBufferedEvent> m_eventPool;

		private readonly Stack<Queue<MyBufferedEvent>> m_listPool;

		private readonly Dictionary<NetworkId, MyObjectEventsBuffer> m_buffer = new Dictionary<NetworkId, MyObjectEventsBuffer>(16);

		private readonly Thread m_mainThread;

		public MyEventsBuffer(Thread mainThread, int eventCapacity = 32)
		{
			m_mainThread = mainThread;
			m_listPool = new Stack<Queue<MyBufferedEvent>>(16);
			for (int i = 0; i < 16; i++)
			{
				m_listPool.Push(new Queue<MyBufferedEvent>(16));
			}
			m_eventPool = new Stack<MyBufferedEvent>(eventCapacity);
			for (int j = 0; j < eventCapacity; j++)
			{
				m_eventPool.Push(new MyBufferedEvent());
			}
		}

		public void Dispose()
		{
			m_eventPool.Clear();
			foreach (KeyValuePair<NetworkId, MyObjectEventsBuffer> item in m_buffer)
			{
				foreach (MyBufferedEvent @event in item.Value.Events)
				{
					if (@event.Data != null)
					{
						@event.Data.Return();
					}
				}
			}
			m_buffer.Clear();
		}

		private MyBufferedEvent ObtainEvent()
		{
			if (m_eventPool.Count > 0)
			{
				return m_eventPool.Pop();
			}
			return new MyBufferedEvent();
		}

		private void ReturnEvent(MyBufferedEvent evnt)
		{
			if (evnt.Data != null)
			{
				evnt.Data.Return();
			}
			evnt.Data = null;
			m_eventPool.Push(evnt);
		}

		private Queue<MyBufferedEvent> ObtainList()
		{
			if (m_listPool.Count > 0)
			{
				return m_listPool.Pop();
			}
			return new Queue<MyBufferedEvent>(16);
		}

		private void ReturnList(Queue<MyBufferedEvent> list)
		{
			m_listPool.Push(list);
		}

		/// <summary>
		/// Enqueues event that have to be done on target object.
		/// </summary>
		/// <param name="stream">Stream with event data.</param>
		/// <param name="targetObjectId">Object id that is a target of the event.</param>
		/// <param name="blockingObjectId">Object id that is blocking target to be processed. 'NetworkId.Invalid' if none.</param>
		/// <param name="eventId">Event id.</param>
		/// <param name="sender">Endpoint.</param>
		public void EnqueueEvent(MyPacketDataBitStreamBase data, NetworkId targetObjectId, NetworkId blockingObjectId, uint eventId, EndpointId sender, Vector3D? position)
		{
			MyBufferedEvent myBufferedEvent = ObtainEvent();
			myBufferedEvent.Data = data;
			myBufferedEvent.TargetObjectId = targetObjectId;
			myBufferedEvent.BlockingObjectId = blockingObjectId;
			myBufferedEvent.EventId = eventId;
			myBufferedEvent.Sender = sender;
			myBufferedEvent.IsBarrier = false;
			myBufferedEvent.Position = position;
			if (!m_buffer.TryGetValue(targetObjectId, out MyObjectEventsBuffer value))
			{
				MyObjectEventsBuffer myObjectEventsBuffer = default(MyObjectEventsBuffer);
				myObjectEventsBuffer.Events = ObtainList();
				value = myObjectEventsBuffer;
				m_buffer.Add(targetObjectId, value);
			}
			value.IsProcessing = false;
			value.Events.Enqueue(myBufferedEvent);
		}

		/// <summary>
		/// Enqueues barrier for an entity that is targeting network object with blocking event. WARNING: Have to be in
		/// pair with blocking event!
		/// </summary>
		/// <param name="targetObjectId">Network object id that will get barrier event.</param>
		/// <param name="blockingObjectId">Network object that have blocking event.</param>
		public void EnqueueBarrier(NetworkId targetObjectId, NetworkId blockingObjectId)
		{
			MyBufferedEvent myBufferedEvent = ObtainEvent();
			myBufferedEvent.TargetObjectId = targetObjectId;
			myBufferedEvent.BlockingObjectId = blockingObjectId;
			myBufferedEvent.IsBarrier = true;
			if (!m_buffer.TryGetValue(targetObjectId, out MyObjectEventsBuffer value))
			{
				value = default(MyObjectEventsBuffer);
				value.Events = ObtainList();
				m_buffer.Add(targetObjectId, value);
			}
			value.IsProcessing = false;
			value.Events.Enqueue(myBufferedEvent);
		}

		/// <summary>
		/// Removes all events from target id.
		/// </summary>
		/// <param name="objectInstance">Target object network id.</param>
		public void RemoveEvents(NetworkId objectInstance)
		{
			if (m_buffer.TryGetValue(objectInstance, out MyObjectEventsBuffer value))
			{
				foreach (MyBufferedEvent @event in value.Events)
				{
					ReturnEvent(@event);
				}
				value.Events.Clear();
				ReturnList(value.Events);
				value.Events = null;
			}
			m_buffer.Remove(objectInstance);
		}

		/// <summary>
		/// Tries to lift barrier from target network object. If successfull, removes this barrier from
		/// target object events queue. Also barrier must be aiming target object id.
		/// </summary>
		/// <param name="targetObjectId">Target network object id.</param>
		/// <returns>True if barrier found on the top of target object events queue. Otherwise false.</returns>
		private bool TryLiftBarrier(NetworkId targetObjectId)
		{
			if (m_buffer.TryGetValue(targetObjectId, out MyObjectEventsBuffer value))
			{
				MyBufferedEvent myBufferedEvent = value.Events.Peek();
				if (myBufferedEvent.IsBarrier && myBufferedEvent.TargetObjectId.Equals(targetObjectId))
				{
					value.Events.Dequeue();
					ReturnEvent(myBufferedEvent);
					return true;
				}
			}
			return false;
		}

		public bool ContainsEvents(NetworkId netId)
		{
			if (m_buffer.TryGetValue(netId, out MyObjectEventsBuffer value))
			{
				return value.Events.Count > 0;
			}
			return false;
		}

		/// <summary>
		/// Tries to process events for prarticular object id (network id).
		/// </summary>
		/// <param name="targetObjectId">Target object network id.</param>
		/// <param name="eventHandler">Handler for processing events.</param>
		/// <param name="isBlockedHandler">Handler for checking if processing of events should be canceled.</param>
		/// <param name="caller">Parent Network id from which this is called. Set NetworkId.Invalid if no parent.</param>
		/// <returns>True if all sucessfull.</returns>
		public bool ProcessEvents(NetworkId targetObjectId, Handler eventHandler, IsBlockedHandler isBlockedHandler, NetworkId caller)
		{
			bool flag = false;
			Queue<NetworkId> postProcessQueue = new Queue<NetworkId>();
			if (!m_buffer.TryGetValue(targetObjectId, out MyObjectEventsBuffer value))
			{
				return false;
			}
			if (value.IsProcessing)
			{
				return false;
			}
			value.IsProcessing = true;
			bool num = ProcessEventsBuffer(value, targetObjectId, eventHandler, isBlockedHandler, caller, ref postProcessQueue);
			value.IsProcessing = false;
			if (!num)
			{
				return false;
			}
			if (value.Events.Count == 0)
			{
				ReturnList(value.Events);
				value.Events = null;
				flag = true;
			}
			if (flag)
			{
				m_buffer.Remove(targetObjectId);
			}
			while (postProcessQueue.Count > 0)
			{
				NetworkId targetObjectId2 = postProcessQueue.Dequeue();
				ProcessEvents(targetObjectId2, eventHandler, isBlockedHandler, targetObjectId);
			}
			return true;
		}

		private bool ProcessEventsBuffer(MyObjectEventsBuffer eventsBuffer, NetworkId targetObjectId, Handler eventHandler, IsBlockedHandler isBlockedHandler, NetworkId caller, ref Queue<NetworkId> postProcessQueue)
		{
			while (eventsBuffer.Events.Count > 0)
			{
				bool flag = true;
				MyBufferedEvent myBufferedEvent = eventsBuffer.Events.Peek();
				if (myBufferedEvent.Data != null)
				{
					_ = myBufferedEvent.Data.Stream.BitPosition;
				}
				if (myBufferedEvent.IsBarrier)
				{
					flag = ProcessBarrierEvent(targetObjectId, myBufferedEvent, eventHandler, isBlockedHandler);
				}
				else
				{
					if (myBufferedEvent.BlockingObjectId.IsValid)
					{
						flag = ProcessBlockingEvent(targetObjectId, myBufferedEvent, caller, eventHandler, isBlockedHandler, ref postProcessQueue);
					}
					else
					{
						eventHandler(myBufferedEvent.Data, myBufferedEvent.TargetObjectId, myBufferedEvent.BlockingObjectId, myBufferedEvent.EventId, myBufferedEvent.Sender, myBufferedEvent.Position);
						myBufferedEvent.Data = null;
					}
					if (flag)
					{
						eventsBuffer.Events.Dequeue();
						if (myBufferedEvent.Data != null && !myBufferedEvent.Data.Stream.CheckTerminator())
						{
							MyLog.Default.WriteLine("RPC: Invalid stream terminator");
						}
						ReturnEvent(myBufferedEvent);
					}
				}
				if (!flag)
				{
					eventsBuffer.IsProcessing = false;
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Process barrier event.
		/// </summary>
		/// <param name="targetObjectId">Target of the barrier event.</param>
		/// <param name="eventToProcess">Event to process.</param>
		/// <param name="eventHandler">Handler for processing event.</param>
		/// <param name="isBlockedHandler">Handler for checking if processing of events should be canceled.</param>
		/// <returns>True if success.</returns>
		private bool ProcessBarrierEvent(NetworkId targetObjectId, MyBufferedEvent eventToProcess, Handler eventHandler, IsBlockedHandler isBlockedHandler)
		{
			if (isBlockedHandler(eventToProcess.TargetObjectId, eventToProcess.BlockingObjectId))
			{
				return false;
			}
			return ProcessEvents(eventToProcess.BlockingObjectId, eventHandler, isBlockedHandler, targetObjectId);
		}

		/// <summary>
		/// Process blocking event.
		/// </summary>
		/// <param name="targetObjectId">Target object id for which to process.</param>
		/// <param name="eventToProcess">Event to be processed.</param>
		/// <param name="caller">Parent Network id from which this is called. Set NetworkId.Invalid if no parent.</param>
		/// <param name="eventHandler">Handler for processing event.</param>
		/// <param name="isBlockedHandler">Handler for checking if processing of events should be canceled.</param>
		/// <param name="postProcessQueue">Queue that should be post processed.</param>
		/// <returns>True if was success.</returns>
		private bool ProcessBlockingEvent(NetworkId targetObjectId, MyBufferedEvent eventToProcess, NetworkId caller, Handler eventHandler, IsBlockedHandler isBlockedHandler, ref Queue<NetworkId> postProcessQueue)
		{
			if (isBlockedHandler(eventToProcess.TargetObjectId, eventToProcess.BlockingObjectId))
			{
				return false;
			}
			if (TryLiftBarrier(eventToProcess.BlockingObjectId))
			{
				eventHandler(eventToProcess.Data, eventToProcess.TargetObjectId, eventToProcess.BlockingObjectId, eventToProcess.EventId, eventToProcess.Sender, eventToProcess.Position);
				eventToProcess.Data = null;
				if (eventToProcess.BlockingObjectId.IsValid && !eventToProcess.BlockingObjectId.Equals(caller))
				{
					postProcessQueue.Enqueue(eventToProcess.BlockingObjectId);
				}
				return true;
			}
			return ProcessEvents(eventToProcess.BlockingObjectId, eventHandler, isBlockedHandler, targetObjectId);
		}

		/// <summary>
		/// Gets events buffer statistics.
		/// </summary>
		/// <returns>Formatted events buffer statistics.</returns>
		public string GetEventsBufferStat()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Pending Events Buffer:");
			foreach (KeyValuePair<NetworkId, MyObjectEventsBuffer> item in m_buffer)
			{
				string value = string.Concat("    NetworkId: ", item.Key, ", EventsCount: ", item.Value.Events.Count);
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString();
		}

		[Conditional("DEBUG")]
		private void CheckThread()
		{
		}
	}
}
