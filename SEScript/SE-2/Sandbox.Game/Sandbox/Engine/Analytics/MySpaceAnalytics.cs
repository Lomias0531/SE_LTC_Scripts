#define VRAGE
using Sandbox.Engine.Platform;
using Sandbox.Game;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.Game;
using VRage.Game.SessionComponents;
using VRage.Game.VisualScripting.Missions;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Engine.Analytics
{
	public sealed class MySpaceAnalytics : MyAnalyticsHelper
	{
		private static readonly object m_singletonGuard = new object();

		private static MyAnalyticsHelper m_instance = null;

		private int m_lastCampaignProgressionTime;

		private float m_lastMinuteUpdate;

		private bool m_registeredEventsInVisualScripting = true;

		private string m_startedTutorialName = string.Empty;

		public static MySpaceAnalytics Instance
		{
			get
			{
				lock (m_singletonGuard)
				{
					if (m_instance == null)
					{
						m_instance = new MySpaceAnalytics();
					}
					else if (!(m_instance is MySpaceAnalytics))
					{
						return null;
					}
				}
				return m_instance as MySpaceAnalytics;
			}
		}

		private MySpaceAnalytics()
		{
		}

		protected override void RegisterEventsInVisualScripting()
		{
			MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			if (component != null && component.SMManager != null)
			{
				component.SMManager.StateMachineStarted += SMManager_StateMachineStarted;
				foreach (MyVSStateMachine runningMachine in component.SMManager.RunningMachines)
				{
					SMManager_StateMachineStarted(runningMachine);
				}
			}
		}

		private void SMManager_StateMachineStarted(MyVSStateMachine obj)
		{
			obj.CursorStateChanged += obj_CursorStateChanged;
		}

		private void obj_CursorStateChanged(MyVSStateMachineNode arg1, MyVSStateMachineNode arg2)
		{
			if (!arg1.PassThrough)
			{
				ReportCampaignProgression(arg1.Name);
			}
		}

		public override void ReportToolbarSwitch(int page)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				try
				{
					MyAnalyticsManager.Instance.ReportToolbarPageSwitch(page);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
		}

		[Conditional("VRAGE")]
		public void ReportCampaignProgression(string completedState)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null && MyCampaignManager.Static != null && MyCampaignManager.Static.IsCampaignRunning && MyCampaignManager.Static.ActiveCampaign != null && MyCampaignManager.Static.ActiveCampaign.IsVanilla)
			{
				try
				{
					int timeFromLastCompletion = (int)Math.Round(MySession.Static.ElapsedPlayTime.TotalSeconds) - m_lastCampaignProgressionTime;
					m_lastCampaignProgressionTime = (int)Math.Round(MySession.Static.ElapsedPlayTime.TotalSeconds);
					MyAnalyticsManager.Instance.ReportCampaignProgression(MyCampaignManager.Static.ActiveCampaignName, MySession.Static.Name, completedState, timeFromLastCompletion);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
		}

		protected override Dictionary<string, object> GetSessionData()
		{
			Dictionary<string, object> sessionData = base.GetSessionData();
			sessionData["audio_hud_warnings"] = MySandboxGame.Config.HudWarnings;
			sessionData["speed_based_ship_sounds"] = MySandboxGame.Config.ShipSoundsAreBasedOnSpeed;
			return sessionData;
		}

		protected override Dictionary<string, object> GetGameplayStartData(MyGameEntryEnum entry, MyObjectBuilder_SessionSettings settings)
		{
			Dictionary<string, object> gameplayStartData = base.GetGameplayStartData(entry, settings);
			MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
			bool flag = MyCampaignManager.Static != null && MyCampaignManager.Static.ActiveCampaign != null;
			gameplayStartData["is_campaign_mission"] = flag;
			gameplayStartData["is_official_campaign"] = (flag && MyCampaignManager.Static.ActiveCampaign != null && MyCampaignManager.Static.ActiveCampaign.IsVanilla);
			gameplayStartData["level_script_count"] = ((component != null && component.RunningLevelScriptNames != null) ? component.RunningLevelScriptNames.Count() : 0);
			gameplayStartData["state_machine_count"] = ((component != null && component.SMManager != null && component.SMManager.MachineDefinitions != null) ? component.SMManager.MachineDefinitions.Count : 0);
			m_lastCampaignProgressionTime = 0;
			gameplayStartData["voxel_support"] = settings.StationVoxelSupport;
			gameplayStartData["destructible_blocks"] = settings.DestructibleBlocks;
			gameplayStartData["destructible_voxels"] = settings.EnableVoxelDestruction;
			gameplayStartData["jetpack"] = settings.EnableJetpack;
			gameplayStartData["hostility"] = settings.EnvironmentHostility.ToString();
			gameplayStartData["drones"] = settings.EnableDrones;
			gameplayStartData["wolfs"] = settings.EnableWolfs;
			gameplayStartData["spiders"] = settings.EnableSpiders;
			gameplayStartData["encounters"] = settings.EnableEncounters;
			gameplayStartData["oxygen"] = settings.EnableOxygen;
			gameplayStartData["pressurization"] = settings.EnableOxygenPressurization;
			gameplayStartData["realistic_sounds"] = settings.RealisticSound;
			gameplayStartData["tool_shake"] = settings.EnableToolShake;
			gameplayStartData["multiplier_inventory"] = settings.InventorySizeMultiplier;
			gameplayStartData["multiplier_welding_speed"] = settings.WelderSpeedMultiplier;
			gameplayStartData["multiplier_grinding_speed"] = settings.GrinderSpeedMultiplier;
			gameplayStartData["multiplier_refinery_speed"] = settings.RefinerySpeedMultiplier;
			gameplayStartData["multiplier_assembler_speed"] = settings.AssemblerSpeedMultiplier;
			gameplayStartData["multiplier_assembler_efficiency"] = settings.AssemblerEfficiencyMultiplier;
			gameplayStartData["max_floating_objects"] = settings.MaxFloatingObjects;
			return gameplayStartData;
		}

		protected override Dictionary<string, object> GetGameplayEndData()
		{
			Dictionary<string, object> gameplayEndData = base.GetGameplayEndData();
			bool flag = MyCampaignManager.Static != null && MyCampaignManager.Static.ActiveCampaign != null;
			gameplayEndData["is_campaign_mission"] = flag;
			gameplayEndData["is_official_campaign"] = (flag && MyCampaignManager.Static.ActiveCampaign != null && MyCampaignManager.Static.ActiveCampaign.IsVanilla);
			int num = 0;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter != null && localCharacter.Toolbar != null)
			{
				for (int i = 0; i < localCharacter.Toolbar.ItemCount; i++)
				{
					if (localCharacter.Toolbar.GetItemAtIndex(i) != null)
					{
						num++;
					}
				}
			}
			gameplayEndData["toolbar_used_slots"] = (uint)num;
			gameplayEndData["toolbar_page_switches"] = MySession.Static.ToolbarPageSwitches;
			gameplayEndData["total_blocks_created"] = MySession.Static.TotalBlocksCreated;
			gameplayEndData["total_damage_dealt"] = MySession.Static.TotalDamageDealt;
			gameplayEndData["total_amount_mined"] = MySession.Static.AmountMined;
			gameplayEndData["time_piloting_big_ships"] = (uint)MySession.Static.TimePilotingBigShip.TotalSeconds;
			gameplayEndData["time_piloting_small_ships"] = (uint)MySession.Static.TimePilotingSmallShip.TotalSeconds;
			gameplayEndData["time_on_foot_all"] = (uint)MySession.Static.TimeOnFoot.TotalSeconds;
			gameplayEndData["time_using_jetpack"] = (uint)MySession.Static.TimeOnJetpack.TotalSeconds;
			gameplayEndData["time_on_foot_stations"] = (uint)MySession.Static.TimeOnStation.TotalSeconds;
			gameplayEndData["time_on_foot_ships"] = (uint)MySession.Static.TimeOnShips.TotalSeconds;
			gameplayEndData["time_on_foot_asteroids"] = (uint)MySession.Static.TimeOnAsteroids.TotalSeconds;
			gameplayEndData["time_on_foot_planets"] = (uint)MySession.Static.TimeOnPlanets.TotalSeconds;
			gameplayEndData["time_in_ship_builder_mode"] = (uint)MySession.Static.TimeInBuilderMode.TotalSeconds;
			gameplayEndData["total_blocks_created_from_ship"] = MySession.Static.TotalBlocksCreatedFromShips;
			return gameplayEndData;
		}

		[Conditional("VRAGE")]
		public void ReportTutorialStart(string tutorialName)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_startedTutorialName = tutorialName;
				Dictionary<string, object> dictionary = CopyDefaultSessionData();
				dictionary["name"] = m_startedTutorialName;
				MyAnalyticsHelper.ReportEvent(MyAnalyticsProgressionStatus.TutorialStart, dictionary);
			}
		}

		[Conditional("VRAGE")]
		public void ReportTutorialStep(string stepName, int stepNumber)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				Dictionary<string, object> dictionary = CopyDefaultSessionData();
				dictionary["name"] = m_startedTutorialName;
				dictionary["step_name"] = stepName;
				dictionary["step_number"] = stepNumber;
				MyAnalyticsHelper.ReportEvent(MyAnalyticsProgressionStatus.TutorialStep, dictionary);
			}
		}

		[Conditional("VRAGE")]
		public void ReportTutorialEnd()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				Dictionary<string, object> dictionary = CopyDefaultSessionData();
				dictionary["name"] = m_startedTutorialName;
				MyAnalyticsHelper.ReportEvent(MyAnalyticsProgressionStatus.TutorialEnd, dictionary);
			}
		}

		[Conditional("VRAGE")]
		public void ReportTutorialScreen(string initiatedFrom)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				Dictionary<string, object> dictionary = CopyDefaultSessionData();
				dictionary["name"] = m_startedTutorialName;
				dictionary["source"] = initiatedFrom;
				MyAnalyticsHelper.ReportEvent(MyAnalyticsProgressionStatus.TutorialClick, dictionary);
			}
		}

		[Conditional("VRAGE")]
		public void ReportScenarioStart(string scenarioName)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				Dictionary<string, object> dictionary = CopyDefaultSessionData();
				dictionary["name"] = scenarioName;
				MyAnalyticsHelper.ReportEvent(MyAnalyticsProgressionStatus.ScenarioStart, dictionary);
			}
		}

		public void Update(MyTimeSpan updateTime)
		{
			if (updateTime.Seconds - 60.0 >= (double)m_lastMinuteUpdate)
			{
				m_lastMinuteUpdate = (float)updateTime.Seconds;
				MyAnalyticsHelper.ReportServerStatus();
			}
			if (!m_registeredEventsInVisualScripting && Instance != null)
			{
				RegisterEventsInVisualScripting();
				m_registeredEventsInVisualScripting = true;
			}
		}

		[Conditional("VRAGE")]
		public void SetEntry(MyGameEntryEnum entry)
		{
			m_entry = entry;
		}
	}
}
