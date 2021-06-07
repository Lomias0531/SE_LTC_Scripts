using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library;
using VRage.Network;

namespace Sandbox.Game.World.Triggers
{
	[StaticEventOwner]
	public class MyMissionTriggers
	{
		protected sealed class OnPlayerWon_003C_003ESandbox_Game_World_MyPlayer_003C_003EPlayerId_0023System_Int32 : ICallSite<IMyEventOwner, MyPlayer.PlayerId, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyPlayer.PlayerId id, in int triggerIndex, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPlayerWon(id, triggerIndex);
			}
		}

		protected sealed class OnPlayerLost_003C_003ESandbox_Game_World_MyPlayer_003C_003EPlayerId_0023System_Int32 : ICallSite<IMyEventOwner, MyPlayer.PlayerId, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyPlayer.PlayerId id, in int triggerIndex, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPlayerLost(id, triggerIndex);
			}
		}

		public static readonly MyPlayer.PlayerId DefaultPlayerId = new MyPlayer.PlayerId(0uL, 0);

		private List<MyTrigger> m_winTriggers = new List<MyTrigger>();

		private List<MyTrigger> m_loseTriggers = new List<MyTrigger>();

		private IMyHudNotification m_notification;

		private StringBuilder m_progress = new StringBuilder();

		public bool Won
		{
			get;
			protected set;
		}

		public bool Lost
		{
			get;
			protected set;
		}

		public string Message
		{
			get;
			protected set;
		}

		public bool IsMsgWinning
		{
			get;
			protected set;
		}

		public List<MyTrigger> WinTriggers => m_winTriggers;

		public List<MyTrigger> LoseTriggers => m_loseTriggers;

		public void SetWon(int triggerIndex)
		{
			Won = true;
			m_winTriggers[triggerIndex].SetTrue();
			if (Message == null)
			{
				Message = m_winTriggers[triggerIndex].Message;
				IsMsgWinning = true;
			}
			DoEnd();
		}

		public void SetLost(int triggerIndex)
		{
			Lost = true;
			m_loseTriggers[triggerIndex].SetTrue();
			if (Message == null)
			{
				Message = m_loseTriggers[triggerIndex].Message;
				IsMsgWinning = false;
			}
			DoEnd();
		}

		protected void DoEnd()
		{
			if (!MySession.Static.Settings.ScenarioEditMode)
			{
				Sync.Players.RespawnComponent.CloseRespawnScreen();
			}
			MyScenarioSystem.Static.GameState = MyScenarioSystem.MyState.Ending;
		}

		public bool DisplayMsg()
		{
			if (Message != null && m_notification == null)
			{
				m_notification = MyAPIGateway.Utilities.CreateNotification(Message, 0, IsMsgWinning ? "Green" : "Red");
				m_notification.Show();
				return true;
			}
			return false;
		}

		public bool UpdateWin(MyPlayer player, MyEntity me)
		{
			if (Won || Lost)
			{
				return true;
			}
			for (int i = 0; i < m_winTriggers.Count; i++)
			{
				MyTrigger myTrigger = m_winTriggers[i];
				if (myTrigger.IsTrue || myTrigger.Update(player, me))
				{
					SetPlayerWon(player.Id, i);
					return true;
				}
			}
			return false;
		}

		public bool UpdateLose(MyPlayer player, MyEntity me)
		{
			if (Won || Lost)
			{
				return true;
			}
			for (int i = 0; i < m_loseTriggers.Count; i++)
			{
				MyTrigger myTrigger = m_loseTriggers[i];
				if (myTrigger.IsTrue || myTrigger.Update(player, me))
				{
					SetPlayerLost(player.Id, i);
					return true;
				}
			}
			return false;
		}

		public bool RaiseSignal(MyPlayer.PlayerId Id, Signal signal)
		{
			if (Won || Lost)
			{
				return true;
			}
			if ((uint)(signal - 1) <= 2u)
			{
				for (int i = 0; i < m_winTriggers.Count; i++)
				{
					MyTrigger myTrigger = m_winTriggers[i];
					if (myTrigger.IsTrue || myTrigger.RaiseSignal(signal))
					{
						SetPlayerWon(Id, i);
						return true;
					}
				}
				for (int j = 0; j < m_loseTriggers.Count; j++)
				{
					MyTrigger myTrigger2 = m_loseTriggers[j];
					if (myTrigger2.IsTrue || myTrigger2.RaiseSignal(signal))
					{
						SetPlayerLost(Id, j);
						return true;
					}
				}
			}
			return false;
		}

		public void DisplayHints(MyPlayer player, MyEntity me)
		{
			for (int i = 0; i < m_winTriggers.Count; i++)
			{
				m_winTriggers[i].DisplayHints(player, me);
			}
			for (int j = 0; j < m_loseTriggers.Count; j++)
			{
				m_loseTriggers[j].DisplayHints(player, me);
			}
		}

		public StringBuilder GetProgress()
		{
			m_progress.Clear().Append((object)MyTexts.Get(MySpaceTexts.ScenarioProgressWinConditions)).Append(MyEnvironment.NewLine);
			for (int i = 0; i < m_winTriggers.Count; i++)
			{
				StringBuilder progress = m_winTriggers[i].GetProgress();
				if (progress != null)
				{
					m_progress.Append((object)progress).Append(MyEnvironment.NewLine);
				}
			}
			m_progress.Append(MyEnvironment.NewLine).Append((object)MyTexts.Get(MySpaceTexts.ScenarioProgressLoseConditions)).Append(MyEnvironment.NewLine);
			for (int j = 0; j < m_loseTriggers.Count; j++)
			{
				StringBuilder progress = m_loseTriggers[j].GetProgress();
				if (progress != null)
				{
					m_progress.Append((object)progress).Append(MyEnvironment.NewLine);
				}
			}
			return m_progress;
		}

		public MyMissionTriggers(MyObjectBuilder_MissionTriggers builder)
		{
			Init(builder);
		}

		public MyMissionTriggers()
		{
		}

		public void CopyTriggersFrom(MyMissionTriggers source)
		{
			m_winTriggers.Clear();
			foreach (MyTrigger winTrigger in source.m_winTriggers)
			{
				MyTrigger myTrigger = (MyTrigger)winTrigger.Clone();
				myTrigger.IsTrue = false;
				m_winTriggers.Add(myTrigger);
			}
			m_loseTriggers.Clear();
			foreach (MyTrigger loseTrigger in source.m_loseTriggers)
			{
				MyTrigger myTrigger2 = (MyTrigger)loseTrigger.Clone();
				myTrigger2.IsTrue = false;
				m_loseTriggers.Add(myTrigger2);
			}
			Won = false;
			Lost = false;
			Message = null;
			HideNotification();
		}

		public void Init(MyObjectBuilder_MissionTriggers builder)
		{
			foreach (MyObjectBuilder_Trigger winTrigger in builder.WinTriggers)
			{
				m_winTriggers.Add(TriggerFactory.CreateInstance(winTrigger));
			}
			foreach (MyObjectBuilder_Trigger loseTrigger in builder.LoseTriggers)
			{
				m_loseTriggers.Add(TriggerFactory.CreateInstance(loseTrigger));
			}
			Message = builder.message;
			Won = builder.Won;
			Lost = builder.Lost;
			if (Won)
			{
				IsMsgWinning = true;
			}
		}

		public virtual MyObjectBuilder_MissionTriggers GetObjectBuilder()
		{
			MyObjectBuilder_MissionTriggers myObjectBuilder_MissionTriggers = new MyObjectBuilder_MissionTriggers();
			foreach (MyTrigger winTrigger in m_winTriggers)
			{
				myObjectBuilder_MissionTriggers.WinTriggers.Add(winTrigger.GetObjectBuilder());
			}
			foreach (MyTrigger loseTrigger in m_loseTriggers)
			{
				myObjectBuilder_MissionTriggers.LoseTriggers.Add(loseTrigger.GetObjectBuilder());
			}
			myObjectBuilder_MissionTriggers.message = Message;
			myObjectBuilder_MissionTriggers.Won = Won;
			myObjectBuilder_MissionTriggers.Lost = Lost;
			return myObjectBuilder_MissionTriggers;
		}

		public void HideNotification()
		{
			if (m_notification != null)
			{
				m_notification.Hide();
				m_notification = null;
			}
		}

		private static void SetPlayerWon(MyPlayer.PlayerId id, int triggerIndex)
		{
			MySessionComponentMissionTriggers.Static.SetWon(id, triggerIndex);
			if (Sync.MultiplayerActive && MySession.Static.IsScenario)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPlayerWon, id, triggerIndex);
			}
		}

		private static void SetPlayerLost(MyPlayer.PlayerId id, int triggerIndex)
		{
			MySessionComponentMissionTriggers.Static.SetLost(id, triggerIndex);
			if (Sync.MultiplayerActive && MySession.Static.IsScenario)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPlayerLost, id, triggerIndex);
			}
		}

		[Event(null, 275)]
		[Reliable]
		[Broadcast]
		private static void OnPlayerWon(MyPlayer.PlayerId id, int triggerIndex)
		{
			MySessionComponentMissionTriggers.Static.SetWon(id, triggerIndex);
		}

		[Event(null, 281)]
		[Reliable]
		[Broadcast]
		private static void OnPlayerLost(MyPlayer.PlayerId id, int triggerIndex)
		{
			MySessionComponentMissionTriggers.Static.SetLost(id, triggerIndex);
		}
	}
}
