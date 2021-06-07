#define VRAGE
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Platform;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.World.Triggers;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MySessionComponentMissionTriggers : MySessionComponentBase
	{
		protected bool m_someoneWon;

		private int m_updateCount;

		public static MySessionComponentMissionTriggers Static
		{
			get;
			private set;
		}

		public Dictionary<MyPlayer.PlayerId, MyMissionTriggers> MissionTriggers
		{
			get;
			private set;
		}

		public override void UpdateBeforeSimulation()
		{
			if ((!MySession.Static.IsScenario && !MySession.Static.Settings.ScenarioEditMode) || MyScenarioSystem.Static == null || MyScenarioSystem.Static.GameState < MyScenarioSystem.MyState.Running)
			{
				return;
			}
			m_updateCount++;
			if (m_updateCount % 10 != 0)
			{
				return;
			}
			UpdateLocal();
			if (Sync.IsServer)
			{
				int num = 0;
				int num2 = 0;
				foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
				{
					MyEntity entity = null;
					if (onlinePlayer.Controller != null && onlinePlayer.Controller.ControlledEntity != null && onlinePlayer.Controller.ControlledEntity.Entity != null)
					{
						entity = onlinePlayer.Controller.ControlledEntity.Entity;
					}
					if (Update(onlinePlayer, entity))
					{
						num++;
					}
					num2++;
				}
				if (num + 1 == num2 && num > 0)
				{
					foreach (MyPlayer onlinePlayer2 in MySession.Static.Players.GetOnlinePlayers())
					{
						RaiseSignal(onlinePlayer2, Signal.ALL_OTHERS_LOST);
					}
				}
				if (m_someoneWon)
				{
					foreach (KeyValuePair<MyPlayer.PlayerId, MyMissionTriggers> missionTrigger in MissionTriggers)
					{
						missionTrigger.Value.RaiseSignal(missionTrigger.Key, Signal.OTHER_WON);
					}
				}
			}
		}

		public bool Update(MyPlayer player, MyEntity entity)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			if (!MissionTriggers.TryGetValue(player.Id, out MyMissionTriggers value))
			{
				value = TryCreateFromDefault(player.Id);
			}
			value.UpdateWin(player, entity);
			if (!value.Won)
			{
				value.UpdateLose(player, entity);
			}
			else
			{
				m_someoneWon = true;
				MySpaceAnalytics.Instance.ReportTutorialEnd();
			}
			return value.Lost;
		}

		public static void PlayerDied(MyPlayer player)
		{
			RaiseSignal(player, Signal.PLAYER_DIED);
		}

		public static void RaiseSignal(MyPlayer player, Signal signal)
		{
			if (!Static.MissionTriggers.TryGetValue(player.Id, out MyMissionTriggers value))
			{
				value = Static.TryCreateFromDefault(player.Id);
			}
			value.RaiseSignal(player.Id, signal);
			if (Static.IsLocal(player.Id))
			{
				Static.UpdateLocal(player);
			}
		}

		public static bool CanRespawn(MyPlayer.PlayerId Id)
		{
			if (MySession.Static.Settings.ScenarioEditMode)
			{
				return true;
			}
			if (!Static.MissionTriggers.TryGetValue(Id, out MyMissionTriggers value))
			{
				return true;
			}
			return !value.Lost;
		}

		private void UpdateLocal()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalHumanPlayer != null)
			{
				UpdateLocal(MySession.Static.LocalHumanPlayer);
			}
		}

		private void UpdateLocal(MyPlayer player)
		{
			MyEntity me = null;
			if (player.Controller != null && player.Controller.ControlledEntity != null && player.Controller.ControlledEntity.Entity != null)
			{
				me = player.Controller.ControlledEntity.Entity;
			}
			UpdateLocal(player, me);
		}

		private void UpdateLocal(MyPlayer player, MyEntity me)
		{
			if (!MissionTriggers.TryGetValue(player.Id, out MyMissionTriggers value))
			{
				value = TryCreateFromDefault(player.Id);
				return;
			}
			value.DisplayMsg();
			value.DisplayHints(player, me);
		}

		public static StringBuilder GetProgress(MyPlayer player)
		{
			if (!Static.MissionTriggers.TryGetValue(player.Id, out MyMissionTriggers value))
			{
				value = Static.TryCreateFromDefault(player.Id);
			}
			return value.GetProgress();
		}

		public void SetWon(MyPlayer.PlayerId Id, int index)
		{
			if (MissionTriggers.TryGetValue(Id, out MyMissionTriggers value))
			{
				value.SetWon(index);
			}
		}

		public void SetLost(MyPlayer.PlayerId Id, int index)
		{
			if (MissionTriggers.TryGetValue(Id, out MyMissionTriggers value))
			{
				value.SetLost(index);
			}
		}

		private bool IsLocal(MyPlayer.PlayerId Id)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.LocalHumanPlayer != null && Id == MySession.Static.LocalHumanPlayer.Id)
			{
				return true;
			}
			return false;
		}

		public MyMissionTriggers TryCreateFromDefault(MyPlayer.PlayerId newId, bool overwrite = false)
		{
			MyMissionTriggers value;
			if (overwrite)
			{
				MissionTriggers.Remove(newId);
			}
			else if (MissionTriggers.TryGetValue(newId, out value))
			{
				return value;
			}
			MyMissionTriggers myMissionTriggers = new MyMissionTriggers();
			MissionTriggers.Add(newId, myMissionTriggers);
			MissionTriggers.TryGetValue(MyMissionTriggers.DefaultPlayerId, out value);
			if (value == null)
			{
				value = new MyMissionTriggers();
				Static.MissionTriggers.Add(MyMissionTriggers.DefaultPlayerId, value);
			}
			myMissionTriggers.CopyTriggersFrom(value);
			m_someoneWon = false;
			return myMissionTriggers;
		}

		public MySessionComponentMissionTriggers()
		{
			MissionTriggers = new Dictionary<MyPlayer.PlayerId, MyMissionTriggers>();
			Static = this;
		}

		public void Load(MyObjectBuilder_SessionComponentMission obj)
		{
			MissionTriggers.Clear();
			if (obj != null && obj.Triggers != null)
			{
				foreach (KeyValuePair<MyObjectBuilder_SessionComponentMission.pair, MyObjectBuilder_MissionTriggers> item in obj.Triggers.Dictionary)
				{
					MyPlayer.PlayerId key = new MyPlayer.PlayerId(item.Key.stm, item.Key.ser);
					MyMissionTriggers value = new MyMissionTriggers(item.Value);
					MissionTriggers.Add(key, value);
				}
			}
		}

		public new MyObjectBuilder_SessionComponentMission GetObjectBuilder()
		{
			MyObjectBuilder_SessionComponentMission myObjectBuilder_SessionComponentMission = new MyObjectBuilder_SessionComponentMission();
			if (MissionTriggers != null)
			{
				foreach (KeyValuePair<MyPlayer.PlayerId, MyMissionTriggers> missionTrigger in MissionTriggers)
				{
					myObjectBuilder_SessionComponentMission.Triggers.Dictionary.Add(new MyObjectBuilder_SessionComponentMission.pair(missionTrigger.Key.SteamId, missionTrigger.Key.SerialId), missionTrigger.Value.GetObjectBuilder());
				}
				return myObjectBuilder_SessionComponentMission;
			}
			return myObjectBuilder_SessionComponentMission;
		}
	}
}
