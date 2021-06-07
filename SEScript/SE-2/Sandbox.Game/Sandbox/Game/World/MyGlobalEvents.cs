using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MyGlobalEvents : MySessionComponentBase
	{
		private static SortedSet<MyGlobalEventBase> m_globalEvents = new SortedSet<MyGlobalEventBase>();

		private int m_elapsedTimeInMilliseconds;

		private int m_previousTime;

		private static readonly int GLOBAL_EVENT_UPDATE_RATIO_IN_MS = 2000;

		private static Predicate<MyGlobalEventBase> m_removalPredicate = RemovalPredicate;

		private static MyDefinitionId m_defIdToRemove;

		public static bool EventsEmpty => m_globalEvents.Count == 0;

		public override void LoadData()
		{
			m_globalEvents.Clear();
			base.LoadData();
		}

		protected override void UnloadData()
		{
			m_globalEvents.Clear();
			base.UnloadData();
		}

		public void Init(MyObjectBuilder_GlobalEvents objectBuilder)
		{
			foreach (MyObjectBuilder_GlobalEventBase @event in objectBuilder.Events)
			{
				m_globalEvents.Add(MyGlobalEventFactory.CreateEvent(@event));
			}
		}

		public new static MyObjectBuilder_GlobalEvents GetObjectBuilder()
		{
			MyObjectBuilder_GlobalEvents myObjectBuilder_GlobalEvents = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GlobalEvents>();
			foreach (MyGlobalEventBase globalEvent in m_globalEvents)
			{
				myObjectBuilder_GlobalEvents.Events.Add(globalEvent.GetObjectBuilder());
			}
			return myObjectBuilder_GlobalEvents;
		}

		public override void BeforeStart()
		{
			m_previousTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public override void UpdateBeforeSimulation()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			m_elapsedTimeInMilliseconds += MySandboxGame.TotalGamePlayTimeInMilliseconds - m_previousTime;
			m_previousTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (m_elapsedTimeInMilliseconds < GLOBAL_EVENT_UPDATE_RATIO_IN_MS)
			{
				return;
			}
			foreach (MyGlobalEventBase globalEvent in m_globalEvents)
			{
				globalEvent.SetActivationTime(TimeSpan.FromTicks(globalEvent.ActivationTime.Ticks - (long)m_elapsedTimeInMilliseconds * 10000L));
			}
			MyGlobalEventBase myGlobalEventBase = m_globalEvents.FirstOrDefault();
			while (myGlobalEventBase != null && myGlobalEventBase.IsInPast)
			{
				m_globalEvents.Remove(myGlobalEventBase);
				if (myGlobalEventBase.Enabled)
				{
					StartGlobalEvent(myGlobalEventBase);
				}
				if (myGlobalEventBase.IsPeriodic)
				{
					if (myGlobalEventBase.RemoveAfterHandlerExit)
					{
						m_globalEvents.Remove(myGlobalEventBase);
					}
					else if (!m_globalEvents.Contains(myGlobalEventBase))
					{
						myGlobalEventBase.RecalculateActivationTime();
						AddGlobalEvent(myGlobalEventBase);
					}
				}
				myGlobalEventBase = m_globalEvents.FirstOrDefault();
			}
			m_elapsedTimeInMilliseconds = 0;
		}

		public override void Draw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_EVENTS)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, 500f), "Upcoming events:", Color.White, 1f);
				StringBuilder stringBuilder = new StringBuilder();
				float num = 530f;
				foreach (MyGlobalEventBase globalEvent in m_globalEvents)
				{
					int num2 = (int)globalEvent.ActivationTime.TotalHours;
					int minutes = globalEvent.ActivationTime.Minutes;
					int seconds = globalEvent.ActivationTime.Seconds;
					stringBuilder.Clear();
					stringBuilder.AppendFormat("{0}:{1:D2}:{2:D2}", num2, minutes, seconds);
					stringBuilder.AppendFormat(" {0}: {1}", globalEvent.Enabled ? "ENABLED" : "--OFF--", globalEvent.Definition.DisplayNameString ?? globalEvent.Definition.Id.SubtypeName);
					MyRenderProxy.DebugDrawText2D(new Vector2(0f, num), stringBuilder.ToString(), globalEvent.Enabled ? Color.White : Color.Gray, 0.8f);
					num += 20f;
				}
			}
		}

		public static MyGlobalEventBase GetEventById(MyDefinitionId defId)
		{
			foreach (MyGlobalEventBase globalEvent in m_globalEvents)
			{
				if (globalEvent.Definition.Id == defId)
				{
					return globalEvent;
				}
			}
			return null;
		}

		private static bool RemovalPredicate(MyGlobalEventBase globalEvent)
		{
			return globalEvent.Definition.Id == m_defIdToRemove;
		}

		public static void RemoveEventsById(MyDefinitionId defIdToRemove)
		{
			m_defIdToRemove = defIdToRemove;
			m_globalEvents.RemoveWhere(m_removalPredicate);
		}

		public static void AddGlobalEvent(MyGlobalEventBase globalEvent)
		{
			m_globalEvents.Add(globalEvent);
		}

		public static void RemoveGlobalEvent(MyGlobalEventBase globalEvent)
		{
			m_globalEvents.Remove(globalEvent);
		}

		public static void RescheduleEvent(MyGlobalEventBase globalEvent, TimeSpan time)
		{
			m_globalEvents.Remove(globalEvent);
			globalEvent.SetActivationTime(time);
			m_globalEvents.Add(globalEvent);
		}

		public static void LoadEvents(MyObjectBuilder_GlobalEvents eventsBuilder)
		{
			if (eventsBuilder != null)
			{
				foreach (MyObjectBuilder_GlobalEventBase @event in eventsBuilder.Events)
				{
					MyGlobalEventBase myGlobalEventBase = MyGlobalEventFactory.CreateEvent(@event);
					if (myGlobalEventBase != null && myGlobalEventBase.IsHandlerValid)
					{
						m_globalEvents.Add(myGlobalEventBase);
					}
				}
			}
		}

		private void StartGlobalEvent(MyGlobalEventBase globalEvent)
		{
			AddGlobalEventToEventLog(globalEvent);
			if (globalEvent.IsHandlerValid)
			{
				globalEvent.Action.Invoke(this, new object[1]
				{
					globalEvent
				});
			}
		}

		private void AddGlobalEventToEventLog(MyGlobalEventBase globalEvent)
		{
			MySandboxGame.Log.WriteLine("MyGlobalEvents.StartGlobalEvent: " + globalEvent.Definition.Id.ToString());
		}

		public static void EnableEvents()
		{
			foreach (MyGlobalEventBase globalEvent in m_globalEvents)
			{
				globalEvent.Enabled = true;
			}
		}

		internal static void DisableEvents()
		{
			foreach (MyGlobalEventBase globalEvent in m_globalEvents)
			{
				globalEvent.Enabled = false;
			}
		}
	}
}
