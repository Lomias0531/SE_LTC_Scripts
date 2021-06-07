#define VRAGE
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Scripting;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Analytics
{
	public abstract class MyAnalyticsHelper
	{
		public const string ANALYTICS_CONDITION_STRING = "VRAGE";

		private static MyDamageInformation m_lastDamageInformation = new MyDamageInformation
		{
			Type = MyStringHash.NullOrEmpty
		};

		private static bool m_scenarioFlag;

		private static bool m_loadingStarted = false;

		private static int ReportChecksActivityStart = 0;

		private static int ReportChecksActivityEnd = 0;

		private static int ReportChecksLastMinute = DateTime.UtcNow.Minute;

		private static ConcurrentDictionary<MyTuple<string, int>, byte> m_reportedBugs = new ConcurrentDictionary<MyTuple<string, int>, byte>();

		private DateTime m_gameplayStartTime = DateTime.UtcNow;

		private bool m_isSessionStarted;

		private bool m_isSessionEnded;

		private bool m_firstRun;

		protected MyGameEntryEnum m_entry;

		private DateTime m_loadingStartedAt;

		private Dictionary<string, object> m_defaultSessionData;

		private static bool SanityCheckAmountPerMinute(int reportCount, int limit)
		{
			if (DateTime.UtcNow.Minute != ReportChecksLastMinute)
			{
				ReportChecksLastMinute = DateTime.UtcNow.Minute;
				ReportChecksActivityStart = 0;
				ReportChecksActivityEnd = 0;
			}
			if (reportCount < limit)
			{
				return false;
			}
			return true;
		}

		private static bool SanityCheckOnePerMinute(ref int lastInstance)
		{
			int num = DateTime.UtcNow.Hour * 60 + DateTime.UtcNow.Minute;
			if (num != lastInstance)
			{
				lastInstance = num;
				return false;
			}
			return true;
		}

		private static bool IsReportedPlayer(MyEntity entity)
		{
			if (entity == null)
			{
				return true;
			}
			IMyControllableEntity myControllableEntity = entity as IMyControllableEntity;
			if (myControllableEntity != null && myControllableEntity.ControllerInfo.IsLocallyControlled())
			{
				return true;
			}
			if (entity.Parent != null)
			{
				return IsReportedPlayer(entity.Parent);
			}
			return false;
		}

		[Conditional("VRAGE")]
		public static void SetLastDamageInformation(MyDamageInformation lastDamageInformation)
		{
			try
			{
				if (!(lastDamageInformation.Type == default(MyStringHash)))
				{
					m_lastDamageInformation = lastDamageInformation;
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
			}
		}

		[Conditional("VRAGE")]
		public static void ReportServerStatus()
		{
			if (MyMultiplayer.Static != null && MyMultiplayer.Static.IsServer && MyMultiplayer.Static.MemberCount > 1)
			{
				try
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					MyConcurrentHashSet<MyEntity> entities = MyEntities.GetEntities();
					foreach (MyEntity item in entities)
					{
						if (item is MyCubeGrid)
						{
							num++;
							num2 += (item as MyCubeGrid).BlocksCount;
							if ((item as MyCubeGrid).Physics != null && (item as MyCubeGrid).Physics.LinearVelocity != Vector3.Zero)
							{
								num3++;
							}
						}
					}
					MyAnalyticsManager.Instance.ReportServerStatus(MyMultiplayer.Static.MemberCount, MyMultiplayer.Static.MemberLimit, Sync.ServerSimulationRatio, entities.Count, num, num2, num3, MyMultiplayer.Static.HostName, MySession.Static.Scenario.Id.SubtypeName, MySession.Static.Name, (uint)MySession.Static.ElapsedGameTime.TotalSeconds);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
		}

		protected virtual void RegisterEventsInVisualScripting()
		{
		}

		[Conditional("VRAGE")]
		public virtual void ReportToolbarSwitch(int page)
		{
		}

		[Conditional("VRAGE")]
		public static void ReportActivityStart(MyEntity sourceEntity, string activityName, string activityFocus, string activityType, string activityItemUsage, bool expectActivityEnd = true)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && !SanityCheckAmountPerMinute(ReportChecksActivityStart, 60))
			{
				try
				{
					if (IsReportedPlayer(sourceEntity))
					{
						if (MySession.Static != null && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.PositionComp != null)
						{
							MyPlanetNamesData planetNames = GetPlanetNames(MySession.Static.LocalCharacter.PositionComp.GetPosition());
							MyAnalyticsManager.Instance.ReportActivityStart(activityName, activityFocus, activityType, activityItemUsage, expectActivityEnd, planetNames.planetName, planetNames.planetType, MyPhysics.SimulationRatio, Sync.ServerSimulationRatio);
						}
						else
						{
							MyAnalyticsManager.Instance.ReportActivityStart(activityName, activityFocus, activityType, activityItemUsage, expectActivityEnd, "", "", MyPhysics.SimulationRatio, Sync.ServerSimulationRatio);
						}
						ReportChecksActivityStart++;
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
		}

		[Conditional("VRAGE")]
		public static void ReportActivityStartIf(bool condition, MyEntity sourceEntity, string activityName, string activityFocus, string activityType, string activityItemUsage, bool expectActivityEnd = true)
		{
			try
			{
				if (condition)
				{
					ReportActivityStart(sourceEntity, activityName, activityFocus, activityType, activityItemUsage, expectActivityEnd);
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
			}
		}

		[Conditional("VRAGE")]
		public static void ReportActivityEnd(MyEntity sourceEntity, string activityName)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && !SanityCheckAmountPerMinute(ReportChecksActivityEnd, 60))
			{
				try
				{
					if (IsReportedPlayer(sourceEntity))
					{
						if (MySession.Static != null && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.PositionComp != null)
						{
							MyPlanetNamesData planetNames = GetPlanetNames(MySession.Static.LocalCharacter.PositionComp.GetPosition());
							MyAnalyticsManager.Instance.ReportActivityEnd(activityName, planetNames.planetName, planetNames.planetType, MyPhysics.SimulationRatio, Sync.ServerSimulationRatio);
						}
						else
						{
							MyAnalyticsManager.Instance.ReportActivityEnd(activityName, "", "", MyPhysics.SimulationRatio, Sync.ServerSimulationRatio);
						}
						ReportChecksActivityEnd++;
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
		}

		[Conditional("VRAGE")]
		public static void ReportPlayerDeath(bool isLocallyControlled, ulong playerSteamId)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				try
				{
					if (isLocallyControlled)
					{
						string @string = m_lastDamageInformation.Type.String;
						bool flag = false;
						bool flag2 = false;
						if (m_lastDamageInformation.Type != MyStringHash.NullOrEmpty && m_lastDamageInformation.AttackerId != 0L)
						{
							if (m_lastDamageInformation.Type == MyDamageType.Suicide)
							{
								flag2 = true;
							}
							else
							{
								MyEntity entity = null;
								MyEntities.TryGetEntityById(m_lastDamageInformation.AttackerId, out entity);
								IMyControllableEntity myControllableEntity = entity as IMyControllableEntity;
								if (myControllableEntity != null)
								{
									MyEntityController controller = myControllableEntity.ControllerInfo.Controller;
									if (controller != null)
									{
										if (controller.Player.Id.SteamId != playerSteamId)
										{
											flag = true;
										}
										else
										{
											flag2 = true;
										}
									}
								}
								else if (entity is IMyGunBaseUser || entity is IMyHandheldGunObject<MyToolBase> || entity is IMyHandheldGunObject<MyGunBase>)
								{
									flag = true;
								}
							}
						}
						string deathType = flag ? "pvp" : (flag2 ? "self_inflicted" : ((!(m_lastDamageInformation.Type == MyDamageType.Destruction)) ? ((m_lastDamageInformation.Type == MyDamageType.Environment) ? "environment" : "unknown") : "cockpit_destruction"));
						MyPlanetNamesData planetNames = GetPlanetNames(MySession.Static.LocalCharacter.PositionComp.GetPosition());
						bool flag3 = MyCampaignManager.Static != null && MyCampaignManager.Static.IsCampaignRunning;
						bool official = flag3 && MyCampaignManager.Static.ActiveCampaign != null && MyCampaignManager.Static.ActiveCampaign.IsVanilla;
						MyAnalyticsManager.Instance.ReportPlayerDeath(deathType, @string, planetNames.planetName, planetNames.planetType, flag3, official, MySession.Static.Settings.GameMode.ToString(), GetModList(), MySession.Static.Mods.Count);
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
		}

		[Conditional("VRAGE")]
		public static void ReportBug(string data, string ticket = null, bool firstTimeOnly = true, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1)
		{
			if (!firstTimeOnly || m_reportedBugs.TryAdd(MyTuple.Create(file, line), 0))
			{
				string text = "[" + file + ":" + line + "]" + data;
				if (ticket != null)
				{
					text = "[" + ticket + "]" + text;
				}
				if (string.IsNullOrEmpty(file))
				{
					text = data;
				}
				MyLog.Default.WriteLine(text);
				ReportEvent(MyAnalyticsProgressionStatus.BugReport, new Dictionary<string, object>
				{
					{
						"data",
						text
					}
				});
			}
		}

		public static MyPlanetNamesData GetPlanetNames(Vector3D position)
		{
			MyPlanetNamesData result = default(MyPlanetNamesData);
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
			Vector3 vector = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
			if (closestPlanet != null && vector.LengthSquared() > 0f)
			{
				result.planetName = closestPlanet.StorageName;
				result.planetType = closestPlanet.Generator.FolderName;
			}
			else
			{
				result.planetName = "";
				result.planetType = "";
			}
			return result;
		}

		[Conditional("VRAGE")]
		public void StoreLoadingStartTime()
		{
			m_loadingStartedAt = DateTime.UtcNow;
		}

		private static string GetModList()
		{
			string text = string.Empty;
			foreach (MyObjectBuilder_Checkpoint.ModItem mod in MySession.Static.Mods)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ", ";
				}
				text += mod.FriendlyName.Replace(",", "");
			}
			return text;
		}

		[Conditional("VRAGE")]
		public static void ReportEvent(MyAnalyticsProgressionStatus status, Dictionary<string, object> eventData = null, double timestamp = 0.0)
		{
			try
			{
				MyAnalyticsManager.Instance.ReportEvent(status, eventData, timestamp);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
			}
		}

		protected Dictionary<string, object> CopyDefaultSessionData()
		{
			if (m_defaultSessionData == null || m_defaultSessionData.Count == 0)
			{
				return new Dictionary<string, object>();
			}
			return new Dictionary<string, object>(m_defaultSessionData);
		}

		[Conditional("VRAGE")]
		public void StartSessionAndIdentifyPlayer(string userId, string userName, bool firstTimeRun)
		{
			if (!m_isSessionStarted)
			{
				m_firstRun = firstTimeRun;
				try
				{
					m_defaultSessionData = GetSessionData();
					Dictionary<string, object> identificationData = CopyDefaultSessionData();
					if (Sandbox.Engine.Platform.Game.IsDedicated)
					{
						MyAnalyticsManager.Instance.IdentifyPlayer(userId, userName, isSteamOnline: false, identificationData);
					}
					else
					{
						MyAnalyticsManager.Instance.IdentifyPlayer(userId, userName, MyGameService.IsOnline, identificationData);
					}
					Dictionary<string, object> sessionData = CopyDefaultSessionData();
					MyAnalyticsManager.Instance.StartSession(sessionData);
					m_isSessionStarted = true;
					MyGuiScreenBase.MouseClickEvent += ReportMouseClick;
					MyLog.Default.WriteLine("Analytics helper process start reported");
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
		}

		public void ReportMouseClick(string screen, Vector2 position, uint seconds)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				try
				{
					MyAnalyticsManager.Instance.ReportScreenMouseClick(screen, position.X, position.Y, seconds);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
		}

		[Conditional("VRAGE")]
		public void EndSession()
		{
			if (!m_isSessionEnded)
			{
				try
				{
					Dictionary<string, object> sessionData = CopyDefaultSessionData();
					MyAnalyticsManager.Instance.EndSession(sessionData);
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine(ex);
				}
			}
		}

		[Conditional("VRAGE")]
		public static void ReportUsedScriptNamespaces()
		{
			try
			{
				MySandboxGame.Log.WriteLineAndConsole("Used namespaces in scripts:");
				foreach (KeyValuePair<string, int> usedNamespace in MyScriptCompiler.UsedNamespaces)
				{
					MyAnalyticsManager.Instance.ReportUsedNamespace(usedNamespace.Key, usedNamespace.Value);
				}
				MyScriptCompiler.UsedNamespaces.Clear();
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
			}
		}

		[Conditional("VRAGE")]
		public void ReportGameQuit(string reason)
		{
			if (m_isSessionStarted)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["reason"] = reason;
				dictionary["game_duration"] = (MySandboxGame.TotalTimeInMilliseconds / 1000).ToString();
				ReportEvent(MyAnalyticsProgressionStatus.GameQuit, dictionary);
			}
		}

		[Conditional("VRAGE")]
		public void ReportGameCrash(Exception exception)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(exception?.Message ?? "Native crash");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(exception?.StackTrace ?? Environment.StackTrace);
			ReportEvent(MyAnalyticsProgressionStatus.GameCrash, new Dictionary<string, object>
			{
				["exception"] = stringBuilder.ToString()
			});
		}

		[Conditional("VRAGE")]
		public void ReportGameplayStart(MyObjectBuilder_SessionSettings settings)
		{
			if (m_isSessionStarted)
			{
				Dictionary<string, object> gameplayStartData = GetGameplayStartData(m_entry, settings);
				m_gameplayStartTime = DateTime.UtcNow;
				ReportEvent(MyAnalyticsProgressionStatus.WorldStart, gameplayStartData);
				ReportMods();
			}
		}

		private void ReportMods()
		{
			try
			{
				for (int i = 0; i < MySession.Static.Mods.Count; i++)
				{
					MyAnalyticsManager.Instance.ReportModLoaded(MySession.Static.Mods[i].FriendlyName);
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
			}
		}

		[Conditional("VRAGE")]
		public void ReportGameplayEnd()
		{
			if (m_isSessionStarted)
			{
				ReportUsedScriptNamespaces();
				Dictionary<string, object> gameplayEndData = GetGameplayEndData();
				ReportEvent(MyAnalyticsProgressionStatus.WorldEnd, gameplayEndData);
			}
		}

		protected virtual Dictionary<string, object> GetSessionData()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			try
			{
				dictionary["game_version"] = MyPerGameSettings.BasicGameInfo.GameVersion.ToString();
				dictionary["game_branch"] = MyGameService.BranchNameFriendly;
				dictionary["cpu_info"] = MyVRage.Platform.GetInfoCPU(out uint _);
				dictionary["cpu_number_of_cores"] = Environment.ProcessorCount;
				dictionary["ram_size"] = MyVRage.Platform.GetTotalPhysicalMemory() / 1024uL / 1024uL;
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MyAdapterInfo myAdapterInfo = MyVideoSettingsManager.Adapters[MyVideoSettingsManager.CurrentDeviceSettings.AdapterOrdinal];
					dictionary["gpu_name"] = myAdapterInfo.Name;
					dictionary["gpu_memory"] = myAdapterInfo.VRAM / 1024uL / 1024uL;
					dictionary["gpu_driver_version"] = myAdapterInfo.DriverVersion;
				}
				dictionary["os_info"] = Environment.OSVersion.VersionString;
				dictionary["os_platform"] = (Environment.Is64BitOperatingSystem ? "64bit" : "32bit");
				dictionary["is_first_run"] = m_firstRun;
				dictionary["is_dedicated"] = Sandbox.Engine.Platform.Game.IsDedicated;
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					return dictionary;
				}
				dictionary["display_resolution"] = MySandboxGame.Config.ScreenWidth + " x " + MySandboxGame.Config.ScreenHeight;
				dictionary["display_window_mode"] = MyVideoSettingsManager.CurrentDeviceSettings.WindowMode.ToString();
				dictionary["graphics_anisotropic_filtering"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.AnisotropicFiltering.ToString();
				dictionary["graphics_antialiasing_mode"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.AntialiasingMode.ToString();
				dictionary["graphics_shadow_quality"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.ShadowQuality.ToString();
				dictionary["graphics_texture_quality"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.TextureQuality.ToString();
				dictionary["graphics_voxel_quality"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality.ToString();
				dictionary["graphics_grass_density_factor"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.GrassDensityFactor;
				dictionary["graphics_grass_draw_distance"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.GrassDrawDistance;
				dictionary["graphics_flares_intensity"] = MyVideoSettingsManager.CurrentGraphicsSettings.FlaresIntensity;
				dictionary["graphics_voxel_shader_quality"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelShaderQuality;
				dictionary["graphics_alphamasked_shader_quality"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.AlphaMaskedShaderQuality;
				dictionary["graphics_atmosphere_shader_quality"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.AtmosphereShaderQuality;
				dictionary["graphics_distance_fade"] = MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.DistanceFade;
				dictionary["audio_music_volume"] = MySandboxGame.Config.MusicVolume;
				dictionary["audio_sound_volume"] = MySandboxGame.Config.GameVolume;
				dictionary["audio_mute_when_not_in_focus"] = MySandboxGame.Config.EnableMuteWhenNotInFocus;
				return dictionary;
			}
			catch (Exception ex)
			{
				dictionary["failed_to_get_data"] = ex.Message + "\n" + ex.StackTrace;
				return dictionary;
			}
		}

		protected virtual Dictionary<string, object> GetGameplayStartData(MyGameEntryEnum entry, MyObjectBuilder_SessionSettings settings)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			try
			{
				dictionary["entry"] = entry.ToString();
				dictionary["game_mode"] = settings.GameMode.ToString();
				dictionary["online_mode"] = settings.OnlineMode.ToString();
				dictionary["world_type"] = MySession.Static.Scenario.Id.SubtypeName;
				dictionary["worldName"] = MySession.Static.Name;
				dictionary["server_is_dedicated"] = (MyMultiplayer.Static != null && MyMultiplayer.Static.HostName.Equals("Dedicated server"));
				dictionary["server_name"] = ((MyMultiplayer.Static != null) ? MyMultiplayer.Static.HostName : MySession.Static.LocalHumanPlayer.DisplayName);
				dictionary["server_max_number_of_players"] = ((MyMultiplayer.Static == null) ? 1 : MyMultiplayer.Static.MemberLimit);
				dictionary["server_current_number_of_players"] = ((MyMultiplayer.Static == null) ? 1 : MyMultiplayer.Static.MemberCount);
				dictionary["is_hosting_player"] = (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer);
				if (MyMultiplayer.Static != null)
				{
					if (MySession.Static != null && MySession.Static.LocalCharacter != null && MyMultiplayer.Static.HostName.Equals(MySession.Static.LocalCharacter.DisplayNameText))
					{
						dictionary["multiplayer_type"] = "Host";
					}
					else if (MyMultiplayer.Static.HostName.Equals("Dedicated server"))
					{
						dictionary["multiplayer_type"] = "Dedicated server";
					}
					else
					{
						dictionary["multiplayer_type"] = "Client";
					}
				}
				else
				{
					dictionary["multiplayer_type"] = "Off-line";
				}
				dictionary["active_mods"] = GetModList();
				dictionary["active_mods_count"] = MySession.Static.Mods.Count;
				long num = (long)Math.Ceiling((DateTime.UtcNow - m_loadingStartedAt).TotalSeconds);
				dictionary["loading_duration"] = num;
				return dictionary;
			}
			catch (Exception ex)
			{
				dictionary["failed_to_get_data"] = ex.Message + "\n" + ex.StackTrace;
				return dictionary;
			}
		}

		protected virtual Dictionary<string, object> GetGameplayEndData()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			try
			{
				MyFpsManager.PrepareMinMax();
				dictionary["game_duration"] = (uint)MySession.Static.ElapsedPlayTime.TotalSeconds;
				dictionary["entire_world_duration"] = (uint)MySession.Static.ElapsedGameTime.TotalSeconds;
				dictionary["fps_average"] = (uint)((double)MyFpsManager.GetSessionTotalFrames() / MySession.Static.ElapsedPlayTime.TotalSeconds);
				dictionary["fps_minimum"] = (uint)MyFpsManager.GetMinSessionFPS();
				dictionary["fps_maximum"] = (uint)MyFpsManager.GetMaxSessionFPS();
				dictionary["ups_average"] = (uint)((double)MyGameStats.Static.UpdateCount / MySession.Static.ElapsedPlayTime.TotalSeconds);
				dictionary["simspeed_client_average"] = (float)((double)MySession.Static.SessionSimSpeedPlayer / MySession.Static.ElapsedPlayTime.TotalSeconds);
				dictionary["simspeed_server_average"] = (float)((double)MySession.Static.SessionSimSpeedServer / MySession.Static.ElapsedPlayTime.TotalSeconds);
				return dictionary;
			}
			catch (Exception ex)
			{
				dictionary["failed_to_get_data"] = ex.Message + "\n" + ex.StackTrace;
				return dictionary;
			}
		}
	}
}
