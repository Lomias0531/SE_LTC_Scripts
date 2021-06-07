using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.IntergridCommunication
{
	internal class MyIntergridCommunicationContext : IMyIntergridCommunicationSystem
	{
		private class ConnectionData
		{
			public static int ValidDurationFrames = 1;

			public int ValidTill;

			public TransmissionDistance ConnectionType;

			public WeakReference<MyDataBroadcaster> Broadcaster;

			public void ReleaseBroadcaster()
			{
				if (Broadcaster != null)
				{
					Broadcaster.SetTarget(null);
				}
			}
		}

		private HashSet<MyMessageListener> m_pendingCallbacks;

		private HashSet<BroadcastListener> m_broadcastListeners;

		private LRUCache<long, ConnectionData> m_connectionCache = new LRUCache<long, ConnectionData>(10);

		private static HashSet<MyDataBroadcaster> m_broadcasters;

		private static MyIGCSystemSessionComponent Context => MyIGCSystemSessionComponent.Static;

		public bool IsActive => ProgrammableBlock != null;

		public UnicastListener UnicastListener
		{
			get;
			private set;
		}

		public MyProgrammableBlock ProgrammableBlock
		{
			get;
			private set;
		}

		long IMyIntergridCommunicationSystem.Me => GetAddressOfThisContext();

		IMyUnicastListener IMyIntergridCommunicationSystem.UnicastListener => UnicastListener;

		public MyIntergridCommunicationContext(MyProgrammableBlock programmableBlock)
		{
			ProgrammableBlock = programmableBlock;
			ProgrammableBlock.OnClosing += ProgrammableBlock_OnClosing;
			UnicastListener = new UnicastListener(this);
		}

		public long GetAddressOfThisContext()
		{
			return ProgrammableBlock.EntityId;
		}

		public void InvokeSinglePendingCallback()
		{
			m_pendingCallbacks.FirstElement().InvokeCallback();
		}

		public void RegisterForCallback(MyMessageListener messageListener)
		{
			if (m_pendingCallbacks == null)
			{
				m_pendingCallbacks = new HashSet<MyMessageListener>();
			}
			if (m_pendingCallbacks.Count == 0)
			{
				Context.RegisterContextWithPendingCallbacks(this);
			}
			m_pendingCallbacks.Add(messageListener);
		}

		public void UnregisterFromCallback(MyMessageListener messageListener)
		{
			m_pendingCallbacks.Remove(messageListener);
			if (m_pendingCallbacks.Count == 0)
			{
				Context.UnregisterContextWithPendingCallbacks(this);
			}
		}

		public void DisposeBroadcastListener(BroadcastListener broadcastListener, bool keepIfHavingPendingMessages)
		{
			if (broadcastListener.IsActive)
			{
				broadcastListener.IsActive = false;
				broadcastListener.DisableMessageCallback();
				Context.UnregisterBroadcastListener(broadcastListener);
			}
			if (!keepIfHavingPendingMessages || !broadcastListener.HasPendingMessage)
			{
				m_broadcastListeners.Remove(broadcastListener);
			}
		}

		public bool IsConnectedTo(MyIntergridCommunicationContext targetContext, TransmissionDistance transmissionDistance)
		{
			long addressOfThisContext = targetContext.GetAddressOfThisContext();
			ConnectionData connectionData = m_connectionCache.Read(addressOfThisContext);
			if (connectionData == null)
			{
				connectionData = new ConnectionData();
				m_connectionCache.Write(addressOfThisContext, connectionData);
			}
			int gameplayFrameCounter = MySession.Static.GameplayFrameCounter;
			if (gameplayFrameCounter >= connectionData.ValidTill)
			{
				connectionData.ConnectionType = (EvaluateConnectionTo(targetContext, connectionData) ?? ((TransmissionDistance)(-1)));
				connectionData.ValidTill = gameplayFrameCounter + ConnectionData.ValidDurationFrames;
			}
			if (connectionData.ConnectionType != (TransmissionDistance)(-1))
			{
				return connectionData.ConnectionType <= transmissionDistance;
			}
			return false;
		}

		private TransmissionDistance? EvaluateConnectionTo(MyIntergridCommunicationContext targetContext, ConnectionData connectionData)
		{
			MyProgrammableBlock programmableBlock = ProgrammableBlock;
			MyProgrammableBlock programmableBlock2 = targetContext.ProgrammableBlock;
			if (!programmableBlock.GetUserRelationToOwner(programmableBlock2.OwnerId).IsFriendly())
			{
				connectionData.ReleaseBroadcaster();
				return null;
			}
			MyCubeGrid cubeGrid = programmableBlock.CubeGrid;
			MyCubeGrid cubeGrid2 = programmableBlock2.CubeGrid;
			if (cubeGrid == cubeGrid2 || MyCubeGridGroups.Static.Mechanical.HasSameGroup(cubeGrid, cubeGrid2))
			{
				return TransmissionDistance.CurrentConstruct;
			}
			if (MyCubeGridGroups.Static.Logical.HasSameGroup(cubeGrid, cubeGrid2))
			{
				return TransmissionDistance.ConnectedConstructs;
			}
			MyDataBroadcaster target = null;
			if (connectionData.Broadcaster != null && connectionData.Broadcaster.TryGetTarget(out target) && !target.Closed && Context.ConnectionProvider(programmableBlock2, target, programmableBlock.OwnerId))
			{
				return TransmissionDistance.AntennaRelay;
			}
			using (MyUtils.ReuseCollection(ref m_broadcasters))
			{
				Context.BroadcasterProvider(programmableBlock.CubeGrid, m_broadcasters, programmableBlock.OwnerId);
				foreach (MyDataBroadcaster broadcaster in m_broadcasters)
				{
					if (Context.ConnectionProvider(programmableBlock2, broadcaster, programmableBlock.OwnerId))
					{
						if (connectionData.Broadcaster == null)
						{
							connectionData.Broadcaster = new WeakReference<MyDataBroadcaster>(broadcaster);
						}
						else
						{
							connectionData.Broadcaster.SetTarget(broadcaster);
						}
						return TransmissionDistance.AntennaRelay;
					}
				}
			}
			connectionData.ReleaseBroadcaster();
			return null;
		}

		public void DisposeContext()
		{
			UnicastListener.DisableMessageCallback();
			if (m_broadcastListeners != null)
			{
				while (m_broadcastListeners.Count > 0)
				{
					DisposeBroadcastListener(m_broadcastListeners.FirstElement(), keepIfHavingPendingMessages: false);
				}
			}
			if (m_pendingCallbacks != null && m_pendingCallbacks.Count != 0)
			{
				while (m_pendingCallbacks.Count > 0)
				{
					UnregisterFromCallback(m_pendingCallbacks.FirstElement());
				}
			}
			ProgrammableBlock.OnClosing -= ProgrammableBlock_OnClosing;
			ProgrammableBlock = null;
		}

		private void ProgrammableBlock_OnClosing(MyEntity block)
		{
			MyIGCSystemSessionComponent.Static.EvictContextFor((MyProgrammableBlock)block);
		}

		bool IMyIntergridCommunicationSystem.IsEndpointReachable(long address, TransmissionDistance transmissionDistance)
		{
			MyIntergridCommunicationContext contextForPB = Context.GetContextForPB(address);
			if (contextForPB == null)
			{
				return false;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_IGC)
			{
				Vector3D from = ProgrammableBlock.WorldMatrix.Translation;
				Vector3D to = MyEntities.GetEntityById(address).WorldMatrix.Translation;
				MyIGCSystemSessionComponent.Static.AddDebugDraw(delegate
				{
					MyRenderProxy.DebugDrawArrow3D(from, to, Color.Red);
				});
			}
			return IsConnectedTo(contextForPB, transmissionDistance);
		}

		void IMyIntergridCommunicationSystem.SendBroadcastMessage<TData>(string tag, TData data, TransmissionDistance transmissionDistance)
		{
			MyIGCSystemSessionComponent.Message message = MyIGCSystemSessionComponent.Message.FromBroadcast(MyIGCSystemSessionComponent.BoxMessage(data), tag, transmissionDistance, this);
			Context.EnqueueMessage(message);
		}

		bool IMyIntergridCommunicationSystem.SendUnicastMessage<TData>(long addressee, string tag, TData data)
		{
			MyIntergridCommunicationContext contextForPB = Context.GetContextForPB(addressee);
			if (contextForPB == null || contextForPB == this)
			{
				return false;
			}
			if (!IsConnectedTo(contextForPB, TransmissionDistance.AntennaRelay))
			{
				return false;
			}
			object data2 = MyIGCSystemSessionComponent.BoxMessage(data);
			Context.EnqueueMessage(MyIGCSystemSessionComponent.Message.FromUnicast(data2, tag, this, contextForPB));
			return true;
		}

		IMyBroadcastListener IMyIntergridCommunicationSystem.RegisterBroadcastListener(string tag)
		{
			if (m_broadcastListeners == null)
			{
				m_broadcastListeners = new HashSet<BroadcastListener>();
			}
			BroadcastListener broadcastListener = null;
			foreach (BroadcastListener broadcastListener2 in m_broadcastListeners)
			{
				if (broadcastListener2.Tag == tag)
				{
					broadcastListener = broadcastListener2;
					break;
				}
			}
			if (broadcastListener == null)
			{
				broadcastListener = new BroadcastListener(this, tag);
				m_broadcastListeners.Add(broadcastListener);
			}
			if (!broadcastListener.IsActive)
			{
				broadcastListener.IsActive = true;
				Context.RegisterBroadcastListener(broadcastListener);
			}
			return broadcastListener;
		}

		void IMyIntergridCommunicationSystem.DisableBroadcastListener(IMyBroadcastListener broadcastListener)
		{
			BroadcastListener broadcastListener2 = broadcastListener as BroadcastListener;
			if (broadcastListener2 == null || broadcastListener2.Context != this)
			{
				throw new ArgumentException("broadcastListener");
			}
			if (m_broadcastListeners.Contains(broadcastListener2))
			{
				DisposeBroadcastListener(broadcastListener2, keepIfHavingPendingMessages: true);
			}
		}

		void IMyIntergridCommunicationSystem.GetBroadcastListeners(List<IMyBroadcastListener> broadcastListeners, Func<IMyBroadcastListener, bool> collect)
		{
			if (m_broadcastListeners != null)
			{
				foreach (BroadcastListener broadcastListener in m_broadcastListeners)
				{
					if (collect == null || collect(broadcastListener))
					{
						broadcastListeners.Add(broadcastListener);
					}
				}
			}
		}
	}
}
