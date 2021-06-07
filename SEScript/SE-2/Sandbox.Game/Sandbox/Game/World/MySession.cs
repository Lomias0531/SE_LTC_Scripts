#define VRAGE
using EmptyKeys.UserInterface;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.ContextHandling;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.Factions.Definitions;
using VRage.Game.GUI;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.SessionComponents;
using VRage.Game.Voxels;
using VRage.GameServices;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Scripting;
using VRage.Serialization;
using VRage.UserInterface;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
	[StaticEventOwner]
	public sealed class MySession : IMyNetObject, IMyEventOwner, IMySession
	{
		private class ComponentComparer : IComparer<MySessionComponentBase>
		{
			public int Compare(MySessionComponentBase x, MySessionComponentBase y)
			{
				int num = x.Priority.CompareTo(y.Priority);
				if (num == 0)
				{
					return string.Compare(x.GetType().FullName, y.GetType().FullName, StringComparison.Ordinal);
				}
				return num;
			}
		}

		public enum LimitResult
		{
			Passed,
			MaxGridSize,
			NoFaction,
			BlockTypeLimit,
			MaxBlocksPerPlayer,
			PCU
		}

		protected sealed class OnCreativeToolsEnabled_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool value, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnCreativeToolsEnabled(value);
			}
		}

		protected sealed class OnPromoteLevelSet_003C_003ESystem_UInt64_0023VRage_Game_ModAPI_MyPromoteLevel : ICallSite<IMyEventOwner, ulong, MyPromoteLevel, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong steamId, in MyPromoteLevel level, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPromoteLevelSet(steamId, level);
			}
		}

		protected sealed class OnServerSaving_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool saveStarted, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnServerSaving(saveStarted);
			}
		}

		protected sealed class OnServerPerformanceWarning_003C_003ESystem_String_0023VRage_MySimpleProfiler_003C_003EProfilingBlockType : ICallSite<IMyEventOwner, string, MySimpleProfiler.ProfilingBlockType, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string key, in MySimpleProfiler.ProfilingBlockType type, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnServerPerformanceWarning(key, type);
			}
		}

		protected sealed class SetSpectatorPositionFromServer_003C_003EVRageMath_Vector3D : ICallSite<IMyEventOwner, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D position, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetSpectatorPositionFromServer(position);
			}
		}

		protected sealed class OnRequestVicinityInformation_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRequestVicinityInformation(entityId);
			}
		}

		protected sealed class OnVicinityInformation_003C_003ESystem_Collections_Generic_List_00601_003CSystem_String_003E_0023System_Collections_Generic_List_00601_003CSystem_String_003E_0023System_Collections_Generic_List_00601_003CSystem_String_003E : ICallSite<IMyEventOwner, List<string>, List<string>, List<string>, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<string> voxels, in List<string> models, in List<string> armorModels, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnVicinityInformation(voxels, models, armorModels);
			}
		}

		private static readonly ComponentComparer SessionComparer;

		private readonly CachingDictionary<Type, MySessionComponentBase> m_sessionComponents = new CachingDictionary<Type, MySessionComponentBase>();

		private readonly Dictionary<int, SortedSet<MySessionComponentBase>> m_sessionComponentsForUpdate = new Dictionary<int, SortedSet<MySessionComponentBase>>();

		private HashSet<string> m_componentsToLoad;

		public HashSet<string> SessionComponentEnabled = new HashSet<string>();

		public HashSet<string> SessionComponentDisabled = new HashSet<string>();

		private const string SAVING_FOLDER = ".new";

		public const int MIN_NAME_LENGTH = 5;

		public const int MAX_NAME_LENGTH = 128;

		public const int MAX_DESCRIPTION_LENGTH = 7999;

		internal MySpectatorCameraController Spectator = new MySpectatorCameraController();

		internal MyTimeSpan m_timeOfSave;

		internal DateTime m_lastTimeMemoryLogged;

		private Dictionary<string, short> EmptyBlockTypeLimitDictionary = new Dictionary<string, short>();

		private static MySession m_static;

		public int RequiresDX = 9;

		public MyObjectBuilder_SessionSettings Settings;

		private bool? m_saveOnUnloadOverride;

		private MyObjectBuilder_SessionSettings.ExperimentalReason m_experimentalReason;

		private bool m_experimentalReasonInited;

		public MyScriptManager ScriptManager;

		public List<Tuple<string, MyBlueprintItemInfo>> BattleBlueprints;

		public Dictionary<ulong, MyPromoteLevel> PromotedUsers = new Dictionary<ulong, MyPromoteLevel>();

		public MyScenarioDefinition Scenario;

		public BoundingBoxD? WorldBoundaries;

		public readonly MyVoxelMaps VoxelMaps = new MyVoxelMaps();

		public readonly MyFactionCollection Factions = new MyFactionCollection();

		public MyPlayerCollection Players = new MyPlayerCollection();

		public MyPerPlayerData PerPlayerData = new MyPerPlayerData();

		public readonly MyToolBarCollection Toolbars = new MyToolBarCollection();

		internal MyVirtualClients VirtualClients = new MyVirtualClients();

		internal MyCameraCollection Cameras = new MyCameraCollection();

		public MyGpsCollection Gpss = new MyGpsCollection();

		public MyBlockLimits GlobalBlockLimits;

		public MyBlockLimits PirateBlockLimits;

		public bool ServerSaving;

		private AdminSettingsEnum m_adminSettings;

		private Dictionary<ulong, AdminSettingsEnum> m_remoteAdminSettings = new Dictionary<ulong, AdminSettingsEnum>();

		private bool m_largeStreamingInProgress;

		private bool m_smallStreamingInProgress;

		private static bool m_showMotD;

		public Dictionary<string, MyFixedPoint> AmountMined = new Dictionary<string, MyFixedPoint>();

		public Action OnLocalPlayerSkinOrColorChanged;

		private bool m_cameraAwaitingEntity;

		private IMyCameraController m_cameraController = MySpectatorCameraController.Static;

		public ulong WorldSizeInBytes;

		private int m_gameplayFrameCounter;

		private const int FRAMES_TO_CONSIDER_READY = 10;

		private int m_framesToReady;

		private HashSet<ulong> m_creativeTools = new HashSet<ulong>();

		private bool m_updateAllowed;

		private MyHudNotification m_aliveNotification;

		private List<MySessionComponentBase> m_loadOrder = new List<MySessionComponentBase>();

		private static int m_profilerDumpDelay;

		private int m_currentDumpNumber;

		private MyObjectBuilder_SessionSettings _settings;

		public const float ADAPTIVE_LOAD_THRESHOLD = 90f;

		private MyOxygenProviderSystemHelper m_oxygenHelper = new MyOxygenProviderSystemHelper();

		public static string GameServiceName => MyGameService.Service.ServiceName;

		public static string WorkshopServiceName => MyGameService.WorkshopService.ServiceName;

		public static string PlatformLinkAgreement => MyGameService.WorkshopService.LegalUrl;

		public static MySession Static
		{
			get
			{
				return m_static;
			}
			set
			{
				m_static = value;
				MyVRage.Platform.SessionReady = (value != null);
			}
		}

		public DateTime GameDateTime
		{
			get
			{
				return new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc) + ElapsedGameTime;
			}
			set
			{
				ElapsedGameTime = value - new DateTime(2081, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			}
		}

		public TimeSpan ElapsedGameTime
		{
			get;
			set;
		}

		public DateTime InGameTime
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public ulong? WorkshopId
		{
			get;
			private set;
		}

		public string CurrentPath
		{
			get;
			private set;
		}

		public string Briefing
		{
			get;
			set;
		}

		public string BriefingVideo
		{
			get;
			set;
		}

		public float SessionSimSpeedPlayer
		{
			get;
			private set;
		}

		public float SessionSimSpeedServer
		{
			get;
			private set;
		}

		public bool CameraOnCharacter
		{
			get;
			set;
		}

		public uint AutoSaveInMinutes
		{
			get
			{
				if (MyFakes.ENABLE_AUTOSAVE && Settings != null)
				{
					return Settings.AutoSaveInMinutes;
				}
				return 0u;
			}
		}

		public bool? SaveOnUnloadOverride => m_saveOnUnloadOverride;

		public bool IsAdminMenuEnabled => IsUserModerator(Sync.MyId);

		public bool CreativeMode => Settings.GameMode == MyGameModeEnum.Creative;

		public bool SurvivalMode => Settings.GameMode == MyGameModeEnum.Survival;

		public bool InfiniteAmmo
		{
			get
			{
				if (!Settings.InfiniteAmmo)
				{
					return Settings.GameMode == MyGameModeEnum.Creative;
				}
				return true;
			}
		}

		public bool EnableContainerDrops
		{
			get
			{
				if (Settings.EnableContainerDrops)
				{
					return Settings.GameMode == MyGameModeEnum.Survival;
				}
				return false;
			}
		}

		public int MinDropContainerRespawnTime => Settings.MinDropContainerRespawnTime * 60;

		public int MaxDropContainerRespawnTime => Settings.MaxDropContainerRespawnTime * 60;

		public bool AutoHealing => Settings.AutoHealing;

		public bool ThrusterDamage => Settings.ThrusterDamage;

		public bool WeaponsEnabled => Settings.WeaponsEnabled;

		public bool CargoShipsEnabled => Settings.CargoShipsEnabled;

		public bool DestructibleBlocks => Settings.DestructibleBlocks;

		public bool EnableIngameScripts
		{
			get
			{
				if (Settings.EnableIngameScripts)
				{
					return MyVRage.Platform.IsScriptCompilationSupported;
				}
				return false;
			}
		}

		public bool Enable3RdPersonView => Settings.Enable3rdPersonView;

		public bool EnableToolShake => Settings.EnableToolShake;

		public bool ShowPlayerNamesOnHud => Settings.ShowPlayerNamesOnHud;

		public bool EnableConvertToStation => Settings.EnableConvertToStation;

		public short MaxPlayers => Settings.MaxPlayers;

		public short MaxFloatingObjects => Settings.MaxFloatingObjects;

		public short MaxBackupSaves => Settings.MaxBackupSaves;

		public int MaxGridSize => Settings.MaxGridSize;

		public int MaxBlocksPerPlayer => Settings.MaxBlocksPerPlayer;

		public Dictionary<string, short> BlockTypeLimits
		{
			get
			{
				if (Settings.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
				{
					return EmptyBlockTypeLimitDictionary;
				}
				return Settings.BlockTypeLimits.Dictionary;
			}
		}

		public bool EnableRemoteBlockRemoval => Settings.EnableRemoteBlockRemoval;

		public float InventoryMultiplier => Settings.InventorySizeMultiplier;

		public float CharactersInventoryMultiplier => Settings.InventorySizeMultiplier;

		public float BlocksInventorySizeMultiplier => Settings.BlocksInventorySizeMultiplier;

		public float RefinerySpeedMultiplier => Settings.RefinerySpeedMultiplier;

		public float AssemblerSpeedMultiplier => Settings.AssemblerSpeedMultiplier;

		public float AssemblerEfficiencyMultiplier => Settings.AssemblerEfficiencyMultiplier;

		public float WelderSpeedMultiplier => Settings.WelderSpeedMultiplier;

		public float GrinderSpeedMultiplier => Settings.GrinderSpeedMultiplier;

		public float HackSpeedMultiplier => Settings.HackSpeedMultiplier;

		public MyOnlineModeEnum OnlineMode => Settings.OnlineMode;

		public MyEnvironmentHostilityEnum EnvironmentHostility => Settings.EnvironmentHostility;

		public bool StartInRespawnScreen => Settings.StartInRespawnScreen;

		public bool EnableVoxelDestruction => Settings.EnableVoxelDestruction;

		public MyBlockLimitsEnabledEnum BlockLimitsEnabled => Settings.BlockLimitsEnabled;

		public int TotalPCU => Settings.TotalPCU;

		public int PiratePCU => Settings.PiratePCU;

		public int MaxFactionsCount
		{
			get
			{
				if (BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION)
				{
					return Settings.MaxFactionsCount;
				}
				return Math.Max(1, Settings.MaxFactionsCount);
			}
		}

		public bool ResearchEnabled
		{
			get
			{
				if (Settings.EnableResearch)
				{
					return !CreativeMode;
				}
				return false;
			}
		}

		public string CustomLoadingScreenImage
		{
			get;
			set;
		}

		public string CustomLoadingScreenText
		{
			get;
			set;
		}

		public string CustomSkybox
		{
			get;
			set;
		}

		public ulong SharedToolbar
		{
			get;
			set;
		}

		public bool EnableSpiders => Settings.EnableSpiders;

		public bool EnableWolfs => Settings.EnableWolfs;

		public bool EnableScripterRole => Settings.EnableScripterRole;

		public bool IsScenario => Settings.Scenario;

		public bool LoadedAsMission
		{
			get;
			private set;
		}

		public bool PersistentEditMode
		{
			get;
			private set;
		}

		public List<MyObjectBuilder_Checkpoint.ModItem> Mods
		{
			get;
			set;
		}

		BoundingBoxD IMySession.WorldBoundaries
		{
			get
			{
				if (!WorldBoundaries.HasValue)
				{
					return BoundingBoxD.CreateInvalid();
				}
				return WorldBoundaries.Value;
			}
		}

		public MySyncLayer SyncLayer
		{
			get;
			private set;
		}

		public MyChatSystem ChatSystem => GetComponent<MyChatSystem>();

		public MyChatBot ChatBot => GetComponent<MyChatBot>();

		public static bool ShowMotD
		{
			get
			{
				return m_showMotD;
			}
			set
			{
				m_showMotD = value;
			}
		}

		public TimeSpan ElapsedPlayTime
		{
			get;
			private set;
		}

		public TimeSpan TimeOnFoot
		{
			get;
			private set;
		}

		public TimeSpan TimeOnJetpack
		{
			get;
			private set;
		}

		public TimeSpan TimePilotingSmallShip
		{
			get;
			private set;
		}

		public TimeSpan TimePilotingBigShip
		{
			get;
			private set;
		}

		public TimeSpan TimeOnStation
		{
			get;
			private set;
		}

		public TimeSpan TimeOnShips
		{
			get;
			private set;
		}

		public TimeSpan TimeOnAsteroids
		{
			get;
			private set;
		}

		public TimeSpan TimeOnPlanets
		{
			get;
			private set;
		}

		public TimeSpan TimeInBuilderMode
		{
			get;
			private set;
		}

		public float PositiveIntegrityTotal
		{
			get;
			set;
		}

		public float NegativeIntegrityTotal
		{
			get;
			set;
		}

		public ulong VoxelHandVolumeChanged
		{
			get;
			set;
		}

		public uint TotalDamageDealt
		{
			get;
			set;
		}

		public uint TotalBlocksCreated
		{
			get;
			set;
		}

		public uint TotalBlocksCreatedFromShips
		{
			get;
			set;
		}

		public uint ToolbarPageSwitches
		{
			get;
			set;
		}

		public MyPlayer LocalHumanPlayer
		{
			get
			{
				if (Sync.Clients != null && Sync.Clients.LocalClient != null)
				{
					return Sync.Clients.LocalClient.FirstPlayer;
				}
				return null;
			}
		}

		IMyPlayer IMySession.LocalHumanPlayer => LocalHumanPlayer;

		public MyEntity TopMostControlledEntity
		{
			get
			{
				MyEntity myEntity = (ControlledEntity != null) ? ControlledEntity.Entity : null;
				MyEntity myEntity2 = myEntity?.GetTopMostParent();
				if (myEntity2 == null || Sync.Players.GetControllingPlayer(myEntity) != Sync.Players.GetControllingPlayer(myEntity2))
				{
					return myEntity;
				}
				return myEntity2;
			}
		}

		public Sandbox.Game.Entities.IMyControllableEntity ControlledEntity
		{
			get
			{
				if (LocalHumanPlayer != null)
				{
					return LocalHumanPlayer.Controller.ControlledEntity;
				}
				return null;
			}
		}

		public MyCharacter LocalCharacter
		{
			get
			{
				if (LocalHumanPlayer != null)
				{
					return LocalHumanPlayer.Character;
				}
				return null;
			}
		}

		public long LocalCharacterEntityId
		{
			get
			{
				if (LocalCharacter != null)
				{
					return LocalCharacter.EntityId;
				}
				return 0L;
			}
		}

		public long LocalPlayerId
		{
			get
			{
				if (LocalHumanPlayer != null)
				{
					return LocalHumanPlayer.Identity.IdentityId;
				}
				return 0L;
			}
		}

		public bool IsCameraAwaitingEntity
		{
			get
			{
				return m_cameraAwaitingEntity;
			}
			set
			{
				m_cameraAwaitingEntity = value;
			}
		}

		public IMyCameraController CameraController
		{
			get
			{
				return m_cameraController;
			}
			private set
			{
				if (m_cameraController == value)
				{
					return;
				}
				IMyCameraController cameraController = m_cameraController;
				m_cameraController = value;
				if (Static == null)
				{
					return;
				}
				if (this.CameraAttachedToChanged != null)
				{
					this.CameraAttachedToChanged(cameraController, m_cameraController);
				}
				if (cameraController != null)
				{
					cameraController.OnReleaseControl(m_cameraController);
					if (cameraController.Entity != null)
					{
						cameraController.Entity.OnClosing -= OnCameraEntityClosing;
					}
				}
				m_cameraController.OnAssumeControl(cameraController);
				if (m_cameraController.Entity != null)
				{
					m_cameraController.Entity.OnClosing += OnCameraEntityClosing;
				}
				m_cameraController.ForceFirstPersonCamera = false;
			}
		}

		public bool IsValid => true;

		public int GameplayFrameCounter => m_gameplayFrameCounter;

		public bool Ready
		{
			get;
			private set;
		}

		public MyEnvironmentHostilityEnum? PreviousEnvironmentHostility
		{
			get;
			set;
		}

		public bool HasCreativeRights => HasPlayerCreativeRights(Sync.MyId);

		public bool IsCopyPastingEnabled
		{
			get
			{
				if (!CreativeToolsEnabled(Sync.MyId) || !HasCreativeRights)
				{
					if (CreativeMode)
					{
						return Settings.EnableCopyPaste;
					}
					return false;
				}
				return true;
			}
		}

		public MyGameFocusManager GameFocusManager
		{
			get;
			private set;
		}

		public AdminSettingsEnum AdminSettings
		{
			get
			{
				return m_adminSettings;
			}
			set
			{
				m_adminSettings = value;
			}
		}

		public Dictionary<ulong, AdminSettingsEnum> RemoteAdminSettings
		{
			get
			{
				return m_remoteAdminSettings;
			}
			set
			{
				m_remoteAdminSettings = value;
			}
		}

		public bool LargeStreamingInProgress
		{
			get
			{
				return m_largeStreamingInProgress;
			}
			set
			{
				if (m_largeStreamingInProgress != value)
				{
					m_largeStreamingInProgress = value;
					if (m_largeStreamingInProgress)
					{
						MyHud.PushRotatingWheelVisible();
						MyHud.RotatingWheelText = MyTexts.Get(MySpaceTexts.LoadingWheel_Streaming);
					}
					else
					{
						MyHud.PopRotatingWheelVisible();
						MyHud.RotatingWheelText = MyHud.Empty;
					}
				}
			}
		}

		public bool SmallStreamingInProgress
		{
			get
			{
				return m_smallStreamingInProgress;
			}
			set
			{
				if (m_smallStreamingInProgress != value)
				{
					m_smallStreamingInProgress = value;
				}
			}
		}

		public bool IsUnloadSaveInProgress
		{
			get;
			set;
		}

		public bool IsServer
		{
			get
			{
				if (!Sync.IsServer)
				{
					return MyMultiplayer.Static == null;
				}
				return true;
			}
		}

		public MyGameDefinition GameDefinition
		{
			get;
			set;
		}

		public int AppVersionFromSave
		{
			get;
			private set;
		}

		public string ThumbPath => Path.Combine(CurrentPath, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);

		public bool MultiplayerAlive
		{
			get;
			set;
		}

		public bool MultiplayerDirect
		{
			get;
			set;
		}

		public double MultiplayerLastMsg
		{
			get;
			set;
		}

		public MyTimeSpan MultiplayerPing
		{
			get;
			set;
		}

		public bool HighSimulationQuality
		{
			get
			{
				if (Settings.AdaptiveSimulationQuality)
				{
					return !(MySandboxGame.Static.CPULoadSmooth > 90f);
				}
				return true;
			}
		}

		public bool LowMemoryState => MyVRage.Platform.RemainingAvailableMemory < 100;

		public bool HighSimulationQualityNotification
		{
			get
			{
				if (Settings.AdaptiveSimulationQuality && (!Sync.IsServer || MySandboxGame.Static.CPULoadSmooth > 90f))
				{
					if (!Sync.IsServer)
					{
						return !(Sync.ServerCPULoadSmooth > 90f);
					}
					return false;
				}
				return true;
			}
		}

		IMyVoxelMaps IMySession.VoxelMaps => VoxelMaps;

		IMyCameraController IMySession.CameraController => CameraController;

		float IMySession.AssemblerEfficiencyMultiplier => AssemblerEfficiencyMultiplier;

		float IMySession.AssemblerSpeedMultiplier => AssemblerSpeedMultiplier;

		bool IMySession.AutoHealing => AutoHealing;

		uint IMySession.AutoSaveInMinutes => AutoSaveInMinutes;

		bool IMySession.CargoShipsEnabled => CargoShipsEnabled;

		bool IMySession.ClientCanSave => false;

		bool IMySession.CreativeMode => CreativeMode;

		string IMySession.CurrentPath => CurrentPath;

		string IMySession.Description
		{
			get
			{
				return Description;
			}
			set
			{
				Description = value;
			}
		}

		TimeSpan IMySession.ElapsedPlayTime => ElapsedPlayTime;

		bool IMySession.EnableCopyPaste => IsCopyPastingEnabled;

		MyEnvironmentHostilityEnum IMySession.EnvironmentHostility => EnvironmentHostility;

		DateTime IMySession.GameDateTime
		{
			get
			{
				return GameDateTime;
			}
			set
			{
				GameDateTime = value;
			}
		}

		float IMySession.GrinderSpeedMultiplier => GrinderSpeedMultiplier;

		float IMySession.HackSpeedMultiplier => HackSpeedMultiplier;

		float IMySession.InventoryMultiplier => InventoryMultiplier;

		float IMySession.CharactersInventoryMultiplier => CharactersInventoryMultiplier;

		float IMySession.BlocksInventorySizeMultiplier => BlocksInventorySizeMultiplier;

		bool IMySession.IsCameraAwaitingEntity
		{
			get
			{
				return IsCameraAwaitingEntity;
			}
			set
			{
				IsCameraAwaitingEntity = value;
			}
		}

		bool IMySession.IsCameraControlledObject => IsCameraControlledObject();

		bool IMySession.IsCameraUserControlledSpectator => IsCameraUserControlledSpectator();

		short IMySession.MaxFloatingObjects => MaxFloatingObjects;

		short IMySession.MaxBackupSaves => MaxBackupSaves;

		short IMySession.MaxPlayers => MaxPlayers;

		bool IMySession.MultiplayerAlive
		{
			get
			{
				return MultiplayerAlive;
			}
			set
			{
				MultiplayerAlive = value;
			}
		}

		bool IMySession.MultiplayerDirect
		{
			get
			{
				return MultiplayerDirect;
			}
			set
			{
				MultiplayerDirect = value;
			}
		}

		double IMySession.MultiplayerLastMsg
		{
			get
			{
				return MultiplayerLastMsg;
			}
			set
			{
				MultiplayerLastMsg = value;
			}
		}

		string IMySession.Name
		{
			get
			{
				return Name;
			}
			set
			{
				Name = value;
			}
		}

		float IMySession.NegativeIntegrityTotal
		{
			get
			{
				return NegativeIntegrityTotal;
			}
			set
			{
				NegativeIntegrityTotal = value;
			}
		}

		MyOnlineModeEnum IMySession.OnlineMode => OnlineMode;

		string IMySession.Password
		{
			get
			{
				return Password;
			}
			set
			{
				Password = value;
			}
		}

		float IMySession.PositiveIntegrityTotal
		{
			get
			{
				return PositiveIntegrityTotal;
			}
			set
			{
				PositiveIntegrityTotal = value;
			}
		}

		float IMySession.RefinerySpeedMultiplier => RefinerySpeedMultiplier;

		bool IMySession.ShowPlayerNamesOnHud => ShowPlayerNamesOnHud;

		bool IMySession.SurvivalMode => SurvivalMode;

		bool IMySession.ThrusterDamage => ThrusterDamage;

		string IMySession.ThumbPath => ThumbPath;

		TimeSpan IMySession.TimeOnBigShip => TimePilotingBigShip;

		TimeSpan IMySession.TimeOnFoot => TimeOnFoot;

		TimeSpan IMySession.TimeOnJetpack => TimeOnJetpack;

		TimeSpan IMySession.TimeOnSmallShip => TimePilotingSmallShip;

		bool IMySession.WeaponsEnabled => WeaponsEnabled;

		float IMySession.WelderSpeedMultiplier => WelderSpeedMultiplier;

		ulong? IMySession.WorkshopId => WorkshopId;

		IMyPlayer IMySession.Player => LocalHumanPlayer;

		VRage.Game.ModAPI.Interfaces.IMyControllableEntity IMySession.ControlledObject => ControlledEntity;

		MyObjectBuilder_SessionSettings IMySession.SessionSettings => Settings;

		IMyFactionCollection IMySession.Factions => Factions;

		IMyCamera IMySession.Camera => MySector.MainCamera;

		double IMySession.CameraTargetDistance
		{
			get
			{
				return GetCameraTargetDistance();
			}
			set
			{
				SetCameraTargetDistance(value);
			}
		}

		public IMyConfig Config => MySandboxGame.Config;

		IMyDamageSystem IMySession.DamageSystem => MyDamageSystem.Static;

		IMyGpsCollection IMySession.GPS => Static.Gpss;

		[Obsolete("Use HasCreativeRights")]
		bool IMySession.HasAdminPrivileges => HasCreativeRights;

		MyPromoteLevel IMySession.PromoteLevel => GetUserPromoteLevel(Sync.MyId);

		bool IMySession.HasCreativeRights => HasCreativeRights;

		Version IMySession.Version => MyFinalBuildConstants.APP_VERSION;

		IMyOxygenProviderSystem IMySession.OxygenProviderSystem => m_oxygenHelper;

		public event Action<ulong, MyPromoteLevel> OnUserPromoteLevelChanged;

		public event Action<IMyCameraController, IMyCameraController> CameraAttachedToChanged;

		public static event Action OnLoading;

		public static event Action OnUnloading;

		public static event Action AfterLoading;

		public static event Action BeforeLoading;

		public static event Action OnUnloaded;

		public event Action OnReady;

		public event Action<MyObjectBuilder_Checkpoint> OnSavingCheckpoint;

		event Action IMySession.OnSessionReady
		{
			add
			{
				Static.OnReady += value;
			}
			remove
			{
				Static.OnReady -= value;
			}
		}

		event Action IMySession.OnSessionLoading
		{
			add
			{
				OnLoading += value;
			}
			remove
			{
				OnLoading -= value;
			}
		}

		private void PrepareBaseSession(List<MyObjectBuilder_Checkpoint.ModItem> mods, MyScenarioDefinition definition = null)
		{
			MyGeneralStats.Static.LoadData();
			ScriptManager.Init(null);
			MyDefinitionManager.Static.LoadData(mods);
			LoadGameDefinition(definition?.GameDefinition ?? MyGameDefinition.Default);
			Scenario = definition;
			if (definition != null)
			{
				WorldBoundaries = definition.WorldBoundaries;
				MySector.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>(definition.Environment);
			}
			MySector.InitEnvironmentSettings();
			MyModAPIHelper.Initialize();
			LoadDataComponents();
			InitDataComponents();
			MyModAPIHelper.Initialize();
		}

		private void PrepareBaseSession(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
		{
			MyGeneralStats.Static.LoadData();
			if (MyVRage.Platform.IsScriptCompilationSupported)
			{
				MyScriptCompiler.Static.Compile(MyApiTarget.Ingame, "Dummy", MyScriptCompiler.Static.GetIngameScript("", "Program", typeof(MyGridProgram).Name), new List<MyScriptCompiler.Message>(), "");
			}
			MyGuiTextures.Static.Reload();
			ScriptManager.Init(checkpoint.ScriptManagerData);
			MyDefinitionManager.Static.LoadData(checkpoint.Mods);
			if (MyFakes.PRIORITIZED_VICINITY_ASSETS_LOADING && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				PreloadVicinityCache(checkpoint.VicinityVoxelCache, checkpoint.VicinityModelsCache, checkpoint.VicinityArmorModelsCache);
				foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
				{
					MyGuiScreenLoading myGuiScreenLoading = screen as MyGuiScreenLoading;
					if (myGuiScreenLoading != null)
					{
						myGuiScreenLoading.DrawLoading();
						break;
					}
				}
			}
			VirtualClients.Init();
			LoadGameDefinition(checkpoint);
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyGuiManager.InitFonts();
			}
			MyDefinitionManager.Static.TryGetDefinition(checkpoint.Scenario, out Scenario);
			WorldBoundaries = checkpoint.WorldBoundaries;
			FixIncorrectSettings(Settings);
			if (!WorldBoundaries.HasValue && Scenario != null)
			{
				WorldBoundaries = Scenario.WorldBoundaries;
			}
			MySector.InitEnvironmentSettings(sector.Environment);
			MyModAPIHelper.Initialize();
			LoadDataComponents();
			LoadObjectBuildersComponents(checkpoint.SessionComponents);
			MyModAPIHelper.Initialize();
			if (Sync.IsDedicated && MySessionComponentAnimationSystem.Static != null)
			{
				MySessionComponentAnimationSystem.Static.SetUpdateOrder(MyUpdateOrder.NoUpdate);
			}
		}

		private void RegisterComponentsFromAssemblies()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			AssemblyName[] array = (from x in executingAssembly.GetReferencedAssemblies()
				group x by x.Name into y
				select y.First()).ToArray();
			m_componentsToLoad = new HashSet<string>();
			m_componentsToLoad.UnionWith(GameDefinition.SessionComponents.Keys);
			m_componentsToLoad.RemoveWhere((string x) => SessionComponentDisabled.Contains(x));
			m_componentsToLoad.UnionWith(SessionComponentEnabled);
			AssemblyName[] array2 = array;
			foreach (AssemblyName assemblyName in array2)
			{
				try
				{
					if (assemblyName.Name.Contains("Sandbox") || assemblyName.Name.Equals("VRage.Game"))
					{
						Assembly assembly = Assembly.Load(assemblyName);
						object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), inherit: false);
						if (customAttributes.Length != 0)
						{
							AssemblyProductAttribute assemblyProductAttribute = customAttributes[0] as AssemblyProductAttribute;
							if (assemblyProductAttribute.Product == "Sandbox" || assemblyProductAttribute.Product == "VRage.Game")
							{
								RegisterComponentsFromAssembly(assembly);
							}
						}
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLine("Error while resolving session components assemblies");
					MyLog.Default.WriteLine(ex.ToString());
				}
			}
			try
			{
				foreach (KeyValuePair<MyModContext, HashSet<MyStringId>> item in ScriptManager.ScriptsPerMod)
				{
					MyStringId key = item.Value.First();
					RegisterComponentsFromAssembly(ScriptManager.Scripts[key], modAssembly: true, item.Key);
				}
			}
			catch (Exception ex2)
			{
				MyLog.Default.WriteLine("Error while loading modded session components");
				MyLog.Default.WriteLine(ex2.ToString());
			}
			try
			{
				foreach (IPlugin plugin in MyPlugins.Plugins)
				{
					RegisterComponentsFromAssembly(plugin.GetType().Assembly, modAssembly: true);
				}
			}
			catch (Exception)
			{
			}
			try
			{
				RegisterComponentsFromAssembly(MyPlugins.GameAssembly);
			}
			catch (Exception ex4)
			{
				MyLog.Default.WriteLine("Error while resolving session components MOD assemblies");
				MyLog.Default.WriteLine(ex4.ToString());
			}
			try
			{
				RegisterComponentsFromAssembly(MyPlugins.UserAssemblies);
			}
			catch (Exception ex5)
			{
				MyLog.Default.WriteLine("Error while resolving session components MOD assemblies");
				MyLog.Default.WriteLine(ex5.ToString());
			}
			RegisterComponentsFromAssembly(executingAssembly);
		}

		public T GetComponent<T>() where T : MySessionComponentBase
		{
			m_sessionComponents.TryGetValue(typeof(T), out MySessionComponentBase value);
			return value as T;
		}

		public void RegisterComponent(MySessionComponentBase component, MyUpdateOrder updateOrder, int priority)
		{
			m_sessionComponents[component.ComponentType] = component;
			component.Session = this;
			AddComponentForUpdate(updateOrder, component);
			m_sessionComponents.ApplyChanges();
		}

		public void UnregisterComponent(MySessionComponentBase component)
		{
			component.Session = null;
			m_sessionComponents.Remove(component.ComponentType);
		}

		public void RegisterComponentsFromAssembly(Assembly[] assemblies, bool modAssembly = false, MyModContext context = null)
		{
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					RegisterComponentsFromAssembly(assembly, modAssembly, context);
				}
			}
		}

		public void RegisterComponentsFromAssembly(Assembly assembly, bool modAssembly = false, MyModContext context = null)
		{
			if (assembly == null)
			{
				return;
			}
			MySandboxGame.Log.WriteLine("Registered modules from: " + assembly.FullName);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (Attribute.IsDefined(type, typeof(MySessionComponentDescriptor)))
				{
					TryRegisterSessionComponent(type, modAssembly, context);
				}
			}
		}

		private void TryRegisterSessionComponent(Type type, bool modAssembly, MyModContext context)
		{
			try
			{
				MyDefinitionId? definition = null;
				MySessionComponentBase mySessionComponentBase = (MySessionComponentBase)Activator.CreateInstance(type);
				if (mySessionComponentBase.IsRequiredByGame || modAssembly || GetComponentInfo(type, out definition))
				{
					RegisterComponent(mySessionComponentBase, mySessionComponentBase.UpdateOrder, mySessionComponentBase.Priority);
					GetComponentInfo(type, out definition);
					mySessionComponentBase.Definition = definition;
					mySessionComponentBase.ModContext = context;
				}
			}
			catch (Exception)
			{
				MySandboxGame.Log.WriteLine("Exception during loading of type : " + type.Name);
			}
		}

		private bool GetComponentInfo(Type type, out MyDefinitionId? definition)
		{
			string text = null;
			if (m_componentsToLoad.Contains(type.Name))
			{
				text = type.Name;
			}
			else if (m_componentsToLoad.Contains(type.FullName))
			{
				text = type.FullName;
			}
			if (text != null)
			{
				GameDefinition.SessionComponents.TryGetValue(text, out definition);
				return true;
			}
			definition = null;
			return false;
		}

		public void AddComponentForUpdate(MyUpdateOrder updateOrder, MySessionComponentBase component)
		{
			for (int i = 0; i <= 2; i++)
			{
				if (((int)updateOrder & (1 << i)) != 0)
				{
					SortedSet<MySessionComponentBase> value = null;
					if (!m_sessionComponentsForUpdate.TryGetValue(1 << i, out value))
					{
						m_sessionComponentsForUpdate.Add(1 << i, value = new SortedSet<MySessionComponentBase>(SessionComparer));
					}
					value.Add(component);
				}
			}
		}

		public void SetComponentUpdateOrder(MySessionComponentBase component, MyUpdateOrder order)
		{
			for (int i = 0; i <= 2; i++)
			{
				SortedSet<MySessionComponentBase> value = null;
				if (((int)order & (1 << i)) != 0)
				{
					if (!m_sessionComponentsForUpdate.TryGetValue(1 << i, out value))
					{
						value = new SortedSet<MySessionComponentBase>();
						m_sessionComponentsForUpdate.Add(i, value);
					}
					value.Add(component);
				}
				else if (m_sessionComponentsForUpdate.TryGetValue(1 << i, out value))
				{
					value.Remove(component);
				}
			}
		}

		public void LoadObjectBuildersComponents(List<MyObjectBuilder_SessionComponent> objectBuilderData)
		{
			foreach (MyObjectBuilder_SessionComponent objectBuilderDatum in objectBuilderData)
			{
				Type key;
				if ((key = MySessionComponentMapping.TryGetMappedSessionComponentType(objectBuilderDatum.GetType())) != null && m_sessionComponents.TryGetValue(key, out MySessionComponentBase value))
				{
					value.Init(objectBuilderDatum);
				}
			}
			InitDataComponents();
		}

		private void InitDataComponents()
		{
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				if (!value.Initialized)
				{
					MyObjectBuilder_SessionComponent sessionComponent = null;
					if (value.ObjectBuilderType != MyObjectBuilderType.Invalid)
					{
						sessionComponent = (MyObjectBuilder_SessionComponent)Activator.CreateInstance(value.ObjectBuilderType);
					}
					value.Init(sessionComponent);
				}
			}
		}

		public void LoadDataComponents()
		{
			MyTimeOfDayHelper.Reset();
			RaiseOnLoading();
			Sync.Clients.SetLocalSteamId(Sync.MyId, !(MyMultiplayer.Static is MyMultiplayerClient));
			Sync.Players.RegisterEvents();
			SetAsNotReady();
			HashSet<MySessionComponentBase> hashSet = new HashSet<MySessionComponentBase>();
			do
			{
				m_sessionComponents.ApplyChanges();
				foreach (MySessionComponentBase value in m_sessionComponents.Values)
				{
					if (!hashSet.Contains(value))
					{
						LoadComponent(value);
						hashSet.Add(value);
					}
				}
			}
			while (m_sessionComponents.HasChanges());
		}

		private void LoadComponent(MySessionComponentBase component)
		{
			if (component.Loaded)
			{
				return;
			}
			Type[] dependencies = component.Dependencies;
			foreach (Type key in dependencies)
			{
				m_sessionComponents.TryGetValue(key, out MySessionComponentBase value);
				if (value != null)
				{
					LoadComponent(value);
				}
			}
			if (!m_loadOrder.Contains(component))
			{
				m_loadOrder.Add(component);
				component.LoadData();
				component.AfterLoadData();
				return;
			}
			string text = $"Circular dependency: {component.DebugName}";
			MySandboxGame.Log.WriteLine(text);
			throw new Exception(text);
		}

		public void UnloadDataComponents(bool beforeLoadWorld = false)
		{
			MySessionComponentBase mySessionComponentBase = null;
			try
			{
				for (int num = m_loadOrder.Count - 1; num >= 0; num--)
				{
					mySessionComponentBase = m_loadOrder[num];
					mySessionComponentBase.UnloadDataConditional();
				}
			}
			catch (Exception innerException)
			{
				IMyModContext modContext = mySessionComponentBase.ModContext;
				if (modContext != null && !modContext.IsBaseGame)
				{
					throw new ModCrashedException(innerException, modContext);
				}
				throw;
			}
			MySessionComponentMapping.Clear();
			m_sessionComponents.Clear();
			m_loadOrder.Clear();
			foreach (SortedSet<MySessionComponentBase> value in m_sessionComponentsForUpdate.Values)
			{
				value.Clear();
			}
			if (!beforeLoadWorld)
			{
				Sync.Players.UnregisterEvents();
				Sync.Clients.Clear();
				MyNetworkReader.Clear();
			}
			Ready = false;
		}

		public void BeforeStartComponents()
		{
			TotalDamageDealt = 0u;
			TotalBlocksCreated = 0u;
			ToolbarPageSwitches = 0u;
			ElapsedPlayTime = default(TimeSpan);
			m_timeOfSave = MySandboxGame.Static.TotalTime;
			MyFpsManager.Reset();
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				value.BeforeStart();
			}
			if (MySpaceAnalytics.Instance != null)
			{
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MySpaceAnalytics.Instance.StartSessionAndIdentifyPlayer(Guid.NewGuid().ToString(), "Dedicated Server", firstTimeRun: true);
				}
				MySpaceAnalytics.Instance.ReportGameplayStart(Settings);
				MyAnalyticsHelper.ReportEvent(MyAnalyticsProgressionStatus.ConfigData, new Dictionary<string, object>
				{
					{
						"UIOpacity",
						MySandboxGame.Config.UIOpacity
					},
					{
						"UIBkOpacity",
						MySandboxGame.Config.UIBkOpacity
					},
					{
						"HUDBkOpacity",
						MySandboxGame.Config.HUDBkOpacity
					}
				});
			}
		}

		public void UpdateComponents()
		{
			SortedSet<MySessionComponentBase> value = null;
			if (m_sessionComponentsForUpdate.TryGetValue(1, out value))
			{
				foreach (MySessionComponentBase item in value)
				{
					if (item.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
					{
						item.UpdateBeforeSimulation();
					}
				}
			}
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ReplicationLayer.Simulate();
			}
			if (m_sessionComponentsForUpdate.TryGetValue(2, out value))
			{
				foreach (MySessionComponentBase item2 in value)
				{
					if (item2.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
					{
						item2.Simulate();
					}
				}
			}
			if (m_sessionComponentsForUpdate.TryGetValue(4, out value))
			{
				foreach (MySessionComponentBase item3 in value)
				{
					if (item3.UpdatedBeforeInit() || MySandboxGame.IsGameReady)
					{
						item3.UpdateAfterSimulation();
					}
				}
			}
		}

		public void UpdateComponentsWhilePaused()
		{
			SortedSet<MySessionComponentBase> value = null;
			if (m_sessionComponentsForUpdate.TryGetValue(1, out value))
			{
				foreach (MySessionComponentBase item in value)
				{
					if (item.UpdateOnPause)
					{
						item.UpdateBeforeSimulation();
					}
				}
			}
			if (m_sessionComponentsForUpdate.TryGetValue(2, out value))
			{
				foreach (MySessionComponentBase item2 in value)
				{
					if (item2.UpdateOnPause)
					{
						item2.Simulate();
					}
				}
			}
			if (m_sessionComponentsForUpdate.TryGetValue(4, out value))
			{
				foreach (MySessionComponentBase item3 in value)
				{
					if (item3.UpdateOnPause)
					{
						item3.UpdateAfterSimulation();
					}
				}
			}
		}

		public static float GetPlayerDistance(MyEntity entity, ICollection<MyPlayer> players)
		{
			Vector3D translation = entity.WorldMatrix.Translation;
			float num = float.MaxValue;
			foreach (MyPlayer player in players)
			{
				Sandbox.Game.Entities.IMyControllableEntity controlledEntity = player.Controller.ControlledEntity;
				if (controlledEntity != null)
				{
					float num2 = Vector3.DistanceSquared(controlledEntity.Entity.WorldMatrix.Translation, translation);
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return (float)Math.Sqrt(num);
		}

		public static float GetOwnerLoginTimeSeconds(MyCubeGrid grid)
		{
			if (grid == null)
			{
				return 0f;
			}
			if (grid.BigOwners.Count == 0)
			{
				return 0f;
			}
			return GetIdentityLoginTimeSeconds(grid.BigOwners[0]);
		}

		public static float GetIdentityLoginTimeSeconds(long identityId)
		{
			MyIdentity myIdentity = Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null)
			{
				return 0f;
			}
			return (int)(DateTime.Now - myIdentity.LastLoginTime).TotalSeconds;
		}

		public static float GetOwnerLogoutTimeSeconds(MyCubeGrid grid)
		{
			if (grid == null)
			{
				return 0f;
			}
			if (grid.BigOwners.Count == 0)
			{
				return 0f;
			}
			if (grid.BigOwners.Count == 1)
			{
				return GetIdentityLogoutTimeSeconds(grid.BigOwners[0]);
			}
			float num = float.MaxValue;
			foreach (long bigOwner in grid.BigOwners)
			{
				float identityLogoutTimeSeconds = GetIdentityLogoutTimeSeconds(bigOwner);
				if (identityLogoutTimeSeconds < num)
				{
					num = identityLogoutTimeSeconds;
				}
			}
			return num;
		}

		public static float GetIdentityLogoutTimeSeconds(long identityId)
		{
			if (Static.Players.TryGetPlayerId(identityId, out MyPlayer.PlayerId result) && Static.Players.GetPlayerById(result) != null)
			{
				return 0f;
			}
			MyIdentity myIdentity = Static.Players.TryGetIdentity(identityId);
			if (myIdentity == null)
			{
				return 0f;
			}
			if (Static.Players.IdentityIsNpc(identityId))
			{
				return 0f;
			}
			return (int)(DateTime.Now - myIdentity.LastLogoutTime).TotalSeconds;
		}

		public bool SetSaveOnUnloadOverride_Dedicated(bool? save)
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_saveOnUnloadOverride = save;
				return true;
			}
			return false;
		}

		public bool IsSettingsExperimental()
		{
			if (GetSettingsExperimentalReason() != (MyObjectBuilder_SessionSettings.ExperimentalReason)0L)
			{
				return !MyCampaignManager.Static.IsCampaignRunning;
			}
			return false;
		}

		public MyObjectBuilder_SessionSettings.ExperimentalReason GetSettingsExperimentalReason()
		{
			if (!m_experimentalReasonInited)
			{
				m_experimentalReasonInited = true;
				m_experimentalReason = Settings.GetExperimentalReason(update: false);
				if (Sync.IsServer && !Sync.IsDedicated && OnlineMode != 0 && TotalPCU > 50000)
				{
					m_experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.TotalPCU;
				}
				if (MySandboxGame.Config.ExperimentalMode && (MyMultiplayer.Static == null || MyMultiplayer.Static.IsServerExperimental))
				{
					m_experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.ExperimentalTurnedOnInConfiguration;
				}
				if (MySandboxGame.InsufficientHardware)
				{
					m_experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.InsufficientHardware;
				}
				if (Mods.Count > 0)
				{
					m_experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.Mods;
				}
				if (MySandboxGame.ConfigDedicated != null && MySandboxGame.ConfigDedicated.Plugins != null && MySandboxGame.ConfigDedicated.Plugins.Count != 0)
				{
					m_experimentalReason |= MyObjectBuilder_SessionSettings.ExperimentalReason.Plugins;
				}
			}
			return m_experimentalReason;
		}

		public MyPromoteLevel GetUserPromoteLevel(ulong steamId)
		{
			if (Static.OnlineMode == MyOnlineModeEnum.OFFLINE)
			{
				return MyPromoteLevel.Owner;
			}
			if (Static.OnlineMode != 0 && steamId == Sync.ServerId)
			{
				return MyPromoteLevel.Owner;
			}
			Static.PromotedUsers.TryGetValue(steamId, out MyPromoteLevel value);
			return value;
		}

		public bool IsUserScripter(ulong steamId)
		{
			if (!EnableScripterRole)
			{
				return true;
			}
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.Scripter;
		}

		public bool IsUserModerator(ulong steamId)
		{
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.Moderator;
		}

		public bool IsUserSpaceMaster(ulong steamId)
		{
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.SpaceMaster;
		}

		public bool IsUserAdmin(ulong steamId)
		{
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.Admin;
		}

		public bool IsUserOwner(ulong steamId)
		{
			return GetUserPromoteLevel(steamId) >= MyPromoteLevel.Owner;
		}

		public bool CreativeToolsEnabled(ulong user)
		{
			if (m_creativeTools.Contains(user))
			{
				return HasPlayerCreativeRights(user);
			}
			return false;
		}

		public void EnableCreativeTools(ulong user, bool value)
		{
			if (value && HasCreativeRights)
			{
				m_creativeTools.Add(user);
			}
			else
			{
				m_creativeTools.Remove(user);
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnCreativeToolsEnabled, value);
		}

		[Event(null, 714)]
		[Reliable]
		[Server]
		private static void OnCreativeToolsEnabled(bool value)
		{
			ulong value2 = MyEventContext.Current.Sender.Value;
			if (value && Static.HasPlayerCreativeRights(value2))
			{
				Static.m_creativeTools.Add(value2);
			}
			else
			{
				Static.m_creativeTools.Remove(value2);
			}
		}

		public bool IsCopyPastingEnabledForUser(ulong user)
		{
			if (!CreativeToolsEnabled(user) || !HasPlayerCreativeRights(user))
			{
				if (CreativeMode)
				{
					return Settings.EnableCopyPaste;
				}
				return false;
			}
			return true;
		}

		public bool SetUserPromoteLevel(ulong steamId, MyPromoteLevel level)
		{
			if (level < MyPromoteLevel.None || level > MyPromoteLevel.Admin)
			{
				throw new ArgumentOutOfRangeException("level", level, null);
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnPromoteLevelSet, steamId, level);
			return true;
		}

		[Event(null, 807)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void OnPromoteLevelSet(ulong steamId, MyPromoteLevel level)
		{
			if (level == MyPromoteLevel.None)
			{
				Static.PromotedUsers.Remove(steamId);
			}
			else
			{
				Static.PromotedUsers[steamId] = level;
			}
			if (Static.RemoteAdminSettings.TryGetValue(steamId, out AdminSettingsEnum value))
			{
				if (!Static.IsUserAdmin(steamId))
				{
					Static.RemoteAdminSettings[steamId] = (value & ~(AdminSettingsEnum.Invulnerable | AdminSettingsEnum.UseTerminals | AdminSettingsEnum.Untargetable | AdminSettingsEnum.IgnoreSafeZones | AdminSettingsEnum.IgnorePcu));
					if (steamId == Sync.MyId)
					{
						Static.AdminSettings = Static.RemoteAdminSettings[steamId];
					}
				}
				else if (!Static.IsUserModerator(steamId))
				{
					Static.RemoteAdminSettings[steamId] = AdminSettingsEnum.None;
					if (steamId == Sync.MyId)
					{
						Static.AdminSettings = Static.RemoteAdminSettings[steamId];
					}
				}
			}
			if (Static.OnUserPromoteLevelChanged != null)
			{
				Static.OnUserPromoteLevelChanged(steamId, level);
			}
		}

		public bool CanPromoteUser(ulong requester, ulong target)
		{
			MyPromoteLevel userPromoteLevel = GetUserPromoteLevel(requester);
			MyPromoteLevel userPromoteLevel2 = GetUserPromoteLevel(target);
			if (userPromoteLevel2 < MyPromoteLevel.Admin)
			{
				if (userPromoteLevel >= userPromoteLevel2)
				{
					return userPromoteLevel >= MyPromoteLevel.Admin;
				}
				return false;
			}
			return false;
		}

		public bool CanDemoteUser(ulong requester, ulong target)
		{
			MyPromoteLevel userPromoteLevel = GetUserPromoteLevel(requester);
			MyPromoteLevel userPromoteLevel2 = GetUserPromoteLevel(target);
			if (userPromoteLevel2 > MyPromoteLevel.None && userPromoteLevel2 < MyPromoteLevel.Owner)
			{
				if (userPromoteLevel >= userPromoteLevel2)
				{
					return userPromoteLevel >= MyPromoteLevel.Admin;
				}
				return false;
			}
			return false;
		}

		public void SetAsNotReady()
		{
			m_framesToReady = 10;
			Ready = false;
		}

		public bool HasPlayerCreativeRights(ulong steamId)
		{
			if (MyMultiplayer.Static != null && !IsUserSpaceMaster(steamId))
			{
				return CreativeMode;
			}
			return true;
		}

		public bool HasPlayerSpectatorRights(ulong steamId)
		{
			if (!CreativeMode && !Settings.EnableSpectator && !IsUserAdmin(steamId))
			{
				if (IsUserModerator(steamId))
				{
					return CreativeToolsEnabled(steamId);
				}
				return false;
			}
			return true;
		}

		private void RaiseOnLoading()
		{
			MySession.OnLoading?.Invoke();
		}

		[Event(null, 875)]
		[Reliable]
		[Broadcast]
		private static void OnServerSaving(bool saveStarted)
		{
			Static.ServerSaving = saveStarted;
		}

		[Event(null, 884)]
		[Reliable]
		[Broadcast]
		private static void OnServerPerformanceWarning(string key, MySimpleProfiler.ProfilingBlockType type)
		{
			MySimpleProfiler.ShowServerPerformanceWarning(key, type);
		}

		private void PerformanceWarning(MySimpleProfiler.MySimpleProfilingBlock block)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnServerPerformanceWarning, block.Name, block.Type);
		}

		private MySession(MySyncLayer syncLayer, bool registerComponents = true)
		{
			if (syncLayer == null)
			{
				MyLog.Default.WriteLine("MySession.Static.MySession() - sync layer is null");
			}
			SyncLayer = syncLayer;
			ElapsedGameTime = default(TimeSpan);
			Spectator.Reset();
			MyCubeGrid.ResetInfoGizmos();
			m_timeOfSave = MyTimeSpan.Zero;
			ElapsedGameTime = default(TimeSpan);
			Ready = false;
			MultiplayerLastMsg = 0.0;
			MultiplayerAlive = true;
			MultiplayerDirect = true;
			AppVersionFromSave = MyFinalBuildConstants.APP_VERSION;
			Factions.FactionStateChanged += OnFactionsStateChanged;
			ScriptManager = new MyScriptManager();
			GC.Collect(2, GCCollectionMode.Forced);
			MySandboxGame.Log.WriteLine(string.Format("GC Memory: {0} B", GC.GetTotalMemory(forceFullCollection: false).ToString("##,#")));
			MySandboxGame.Log.WriteLine(string.Format("Process Memory: {0} B", Process.GetCurrentProcess().PrivateMemorySize64.ToString("##,#")));
			GameFocusManager = new MyGameFocusManager();
		}

		private MySession()
			: this(Sandbox.Engine.Platform.Game.IsDedicated ? MyMultiplayer.Static.SyncLayer : new MySyncLayer(new MyTransportLayer(2)))
		{
		}

		static MySession()
		{
			SessionComparer = new ComponentComparer();
			m_showMotD = false;
			if (MyAPIGatewayShortcuts.GetMainCamera == null)
			{
				MyAPIGatewayShortcuts.GetMainCamera = GetMainCamera;
			}
			if (MyAPIGatewayShortcuts.GetWorldBoundaries == null)
			{
				MyAPIGatewayShortcuts.GetWorldBoundaries = GetWorldBoundaries;
			}
			if (MyAPIGatewayShortcuts.GetLocalPlayerPosition == null)
			{
				MyAPIGatewayShortcuts.GetLocalPlayerPosition = GetLocalPlayerPosition;
			}
		}

		internal void StartServer(MyMultiplayerBase multiplayer)
		{
			multiplayer.WorldName = Name;
			multiplayer.GameMode = Settings.GameMode;
			multiplayer.WorldSize = WorldSizeInBytes;
			multiplayer.AppVersion = MyFinalBuildConstants.APP_VERSION;
			multiplayer.DataHash = MyDataIntegrityChecker.GetHashBase64();
			multiplayer.InventoryMultiplier = Settings.InventorySizeMultiplier;
			multiplayer.BlocksInventoryMultiplier = Settings.BlocksInventorySizeMultiplier;
			multiplayer.AssemblerMultiplier = Settings.AssemblerEfficiencyMultiplier;
			multiplayer.RefineryMultiplier = Settings.RefinerySpeedMultiplier;
			multiplayer.WelderMultiplier = Settings.WelderSpeedMultiplier;
			multiplayer.GrinderMultiplier = Settings.GrinderSpeedMultiplier;
			multiplayer.MemberLimit = Settings.MaxPlayers;
			multiplayer.Mods = Mods;
			multiplayer.ViewDistance = Settings.ViewDistance;
			multiplayer.SyncDistance = Settings.SyncDistance;
			multiplayer.Scenario = IsScenario;
			multiplayer.ExperimentalMode = IsSettingsExperimental();
			MyCachedServerItem.SendSettingsToSteam();
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				(multiplayer as MyDedicatedServerBase).SendGameTagsToSteam();
				MySimpleProfiler.ShowPerformanceWarning += PerformanceWarning;
			}
			if (multiplayer is MyMultiplayerLobby)
			{
				((MyMultiplayerLobby)multiplayer).HostSteamId = MyMultiplayer.Static.ServerId;
			}
			Static.Gpss.RegisterChat(multiplayer);
		}

		private void DisconnectMultiplayer()
		{
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ReplicationLayer.Disconnect();
			}
		}

		private void UnloadMultiplayer()
		{
			if (MyMultiplayer.Static != null)
			{
				Static.Gpss.UnregisterChat(MyMultiplayer.Static);
				MyMultiplayer.Static.Dispose();
				SyncLayer = null;
			}
		}

		private void LoadGameDefinition(MyDefinitionId? gameDef = null)
		{
			if (!gameDef.HasValue)
			{
				gameDef = MyGameDefinition.Default;
			}
			Static.GameDefinition = MyDefinitionManager.Static.GetDefinition<MyGameDefinition>(gameDef.Value);
			if (Static.GameDefinition == null)
			{
				Static.GameDefinition = MyGameDefinition.DefaultDefinition;
			}
			RegisterComponentsFromAssemblies();
		}

		private void LoadGameDefinition(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (checkpoint.GameDefinition.IsNull())
			{
				LoadGameDefinition();
				return;
			}
			Static.GameDefinition = MyDefinitionManager.Static.GetDefinition<MyGameDefinition>(checkpoint.GameDefinition);
			SessionComponentDisabled = checkpoint.SessionComponentDisabled;
			SessionComponentEnabled = checkpoint.SessionComponentEnabled;
			RegisterComponentsFromAssemblies();
			ShowMotD = true;
		}

		private void CheckUpdate()
		{
			bool flag = true;
			if (IsPausable())
			{
				flag = (!MySandboxGame.IsPaused && MySandboxGame.Static.IsActive);
			}
			if (m_updateAllowed == flag)
			{
				return;
			}
			m_updateAllowed = flag;
			if (!m_updateAllowed)
			{
				MyLog.Default.WriteLine("Updating stopped.");
				SortedSet<MySessionComponentBase> value = null;
				if (m_sessionComponentsForUpdate.TryGetValue(4, out value))
				{
					foreach (MySessionComponentBase item in value)
					{
						item.UpdatingStopped();
					}
				}
			}
			else
			{
				MyLog.Default.WriteLine("Updating continues.");
			}
		}

		public void Update(MyTimeSpan updateTime)
		{
			if (m_updateAllowed && MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ReplicationLayer.UpdateClientStateGroups();
			}
			CheckUpdate();
			CheckProfilerDump();
			if (MySandboxGame.Config.SyncRendering)
			{
				Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromMilliseconds(-1.0));
			}
			Parallel.RunCallbacks();
			TimeSpan elapsedTimespan = new TimeSpan(0, 0, 0, 0, 16);
			if (m_updateAllowed || Sandbox.Engine.Platform.Game.IsDedicated)
			{
				if (MySandboxGame.IsPaused)
				{
					return;
				}
				if (MyMultiplayer.Static != null)
				{
					MyMultiplayer.Static.ReplicationLayer.UpdateBefore();
				}
				UpdateComponents();
				MyParticleEffectsSoundManager.UpdateEffects();
				if (MyMultiplayer.Static != null)
				{
					MyMultiplayer.Static.ReplicationLayer.UpdateAfter();
					MyMultiplayer.Static.Tick();
				}
				if ((CameraController == null || !CameraController.IsInFirstPersonView) && MyThirdPersonSpectator.Static != null)
				{
					MyThirdPersonSpectator.Static.Update();
				}
				if (IsServer)
				{
					Players.SendDirtyBlockLimits();
				}
				ElapsedGameTime += (MyRandom.EnableDeterminism ? TimeSpan.FromMilliseconds(16.0) : elapsedTimespan);
				if (m_lastTimeMemoryLogged + TimeSpan.FromSeconds(30.0) < DateTime.UtcNow)
				{
					MySandboxGame.Log.WriteLine(string.Format("GC Memory: {0} B", GC.GetTotalMemory(forceFullCollection: false).ToString("##,#")));
					m_lastTimeMemoryLogged = DateTime.UtcNow;
				}
				if (AutoSaveInMinutes != 0 && MySandboxGame.IsGameReady && updateTime.TimeSpan - m_timeOfSave.TimeSpan > TimeSpan.FromMinutes(AutoSaveInMinutes))
				{
					MySandboxGame.Log.WriteLine("Autosave initiated");
					MyCharacter localCharacter = LocalCharacter;
					bool flag = (localCharacter != null && !localCharacter.IsDead) || localCharacter == null;
					MySandboxGame.Log.WriteLine("Character state: " + flag);
					flag &= Sync.IsServer;
					MySandboxGame.Log.WriteLine("IsServer: " + Sync.IsServer);
					flag &= !MyAsyncSaving.InProgress;
					MySandboxGame.Log.WriteLine("MyAsyncSaving.InProgress: " + MyAsyncSaving.InProgress);
					if (flag)
					{
						MySandboxGame.Log.WriteLineAndConsole("Autosave");
						MyAsyncSaving.Start(delegate
						{
							MySector.ResetEyeAdaptation = true;
						});
					}
					m_timeOfSave = updateTime;
				}
				if (MySandboxGame.IsGameReady && m_framesToReady > 0)
				{
					m_framesToReady--;
					if (m_framesToReady == 0)
					{
						Ready = true;
						MyAudio.Static.PlayMusic(new MyMusicTrack
						{
							TransitionCategory = MyStringId.GetOrCompute("Default")
						});
						if (this.OnReady != null)
						{
							this.OnReady();
						}
						MySimpleProfiler.Reset(resetFrameCounter: true);
						if (this.OnReady != null)
						{
							Delegate[] invocationList = this.OnReady.GetInvocationList();
							foreach (Delegate @delegate in invocationList)
							{
								OnReady -= (Action)@delegate;
							}
						}
						if (Sandbox.Engine.Platform.Game.IsDedicated)
						{
							if (!Console.IsInputRedirected && MySandboxGame.IsConsoleVisible)
							{
								MyLog.Default.WriteLineAndConsole("Game ready... Press Ctrl+C to exit");
							}
							else
							{
								MyLog.Default.WriteLineAndConsole("Game ready... ");
							}
						}
					}
				}
				if (Sync.MultiplayerActive && !Sync.IsServer)
				{
					CheckMultiplayerStatus();
				}
				m_gameplayFrameCounter++;
			}
			else if (MySandboxGame.IsPaused && Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				UpdateComponentsWhilePaused();
			}
			UpdateStatistics(ref elapsedTimespan);
			DebugDraw();
		}

		private static void CheckProfilerDump()
		{
			m_profilerDumpDelay--;
			if (m_profilerDumpDelay == 0)
			{
				MyRenderProxy.GetRenderProfiler().Dump();
				VRage.Profiler.MyRenderProfiler.SetLevel(0);
			}
			else if (m_profilerDumpDelay < 0)
			{
				m_profilerDumpDelay = -1;
			}
		}

		private void DebugDraw()
		{
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				_ = MyDebugDrawSettings.DEBUG_DRAW_CONTROLLED_ENTITIES;
				if (MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_COMPOSITION)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_DataProvider);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_ACCESS)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_Access);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_FULLCELLS)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.FullCells);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MicroNodes);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES_SCALED)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MicroNodesScaled);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRONODES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MacroNodes);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACROLEAVES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MacroLeaves);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRO_SCALED)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Content_MacroScaled);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACRONODES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Materials_MacroNodes);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACROLEAVES)
				{
					VoxelMaps.DebugDraw(MyVoxelDebugDrawMode.Materials_MacroLeaves);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_ENCOUNTERS)
				{
					MyEncounterGenerator.Static.DebugDraw();
				}
			}
		}

		private void CheckMultiplayerStatus()
		{
			MultiplayerAlive = MyMultiplayer.Static.IsConnectionAlive;
			MultiplayerDirect = MyMultiplayer.Static.IsConnectionDirect;
			if (Sync.IsServer)
			{
				MultiplayerLastMsg = 0.0;
				return;
			}
			MultiplayerLastMsg = (DateTime.UtcNow - MyMultiplayer.Static.LastMessageReceived).TotalSeconds;
			MyReplicationClient myReplicationClient = MyMultiplayer.ReplicationLayer as MyReplicationClient;
			if (myReplicationClient != null)
			{
				MultiplayerPing = myReplicationClient.Ping;
				LargeStreamingInProgress = (myReplicationClient.PendingStreamingRelicablesCount > 1);
				SmallStreamingInProgress = (myReplicationClient.PendingStreamingRelicablesCount > 0);
			}
		}

		public bool IsPausable()
		{
			return !Sync.MultiplayerActive;
		}

		private void UpdateStatistics(ref TimeSpan elapsedTimespan)
		{
			ElapsedPlayTime += (MyRandom.EnableDeterminism ? TimeSpan.FromMilliseconds(16.0) : elapsedTimespan);
			SessionSimSpeedPlayer += (float)((double)MyPhysics.SimulationRatio * elapsedTimespan.TotalSeconds);
			SessionSimSpeedServer += (float)((double)Sync.ServerSimulationRatio * elapsedTimespan.TotalSeconds);
			if (LocalHumanPlayer == null || LocalHumanPlayer.Character == null)
			{
				return;
			}
			if (ControlledEntity is MyCharacter)
			{
				if (((MyCharacter)ControlledEntity).GetCurrentMovementState() == MyCharacterMovementEnum.Flying)
				{
					TimeOnJetpack += elapsedTimespan;
				}
				else
				{
					TimeOnFoot += elapsedTimespan;
				}
				MyCharacterSoundComponent soundComp = ((MyCharacter)ControlledEntity).SoundComp;
				if (soundComp == null)
				{
					return;
				}
				if (soundComp.StandingOnGrid != null)
				{
					if (soundComp.StandingOnGrid.IsStatic)
					{
						TimeOnStation += elapsedTimespan;
					}
					else
					{
						TimeOnShips += elapsedTimespan;
					}
				}
				if (soundComp.StandingOnVoxel != null)
				{
					if (soundComp.StandingOnVoxel is MyVoxelPhysics && ((MyVoxelPhysics)soundComp.StandingOnVoxel).RootVoxel is MyPlanet)
					{
						TimeOnPlanets += elapsedTimespan;
					}
					else
					{
						TimeOnAsteroids += elapsedTimespan;
					}
				}
			}
			else if (ControlledEntity is MyCockpit)
			{
				if (((MyCockpit)ControlledEntity).IsLargeShip())
				{
					TimePilotingBigShip += elapsedTimespan;
				}
				else
				{
					TimePilotingSmallShip += elapsedTimespan;
				}
				if (((MyCockpit)ControlledEntity).BuildingMode)
				{
					TimeInBuilderMode += elapsedTimespan;
				}
			}
		}

		public void HandleInput()
		{
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				value.HandleInput();
			}
		}

		public void Draw()
		{
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				value.Draw();
			}
		}

		public static bool IsCompatibleVersion(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (checkpoint == null)
			{
				return false;
			}
			return checkpoint.AppVersion <= (int)MyFinalBuildConstants.APP_VERSION;
		}

		public static void Start(string name, string description, string password, MyObjectBuilder_SessionSettings settings, List<MyObjectBuilder_Checkpoint.ModItem> mods, MyWorldGenerator.Args generationArgs)
		{
			MyLog.Default.WriteLineAndConsole("Starting world " + name);
			MyEntityContainerEventExtensions.InitEntityEvents();
			Static = new MySession();
			Static.Name = name;
			Static.Mods = mods;
			Static.Description = description;
			Static.Password = password;
			Static.Settings = settings;
			Static.Scenario = generationArgs.Scenario;
			FixIncorrectSettings(Static.Settings);
			double num = settings.WorldSizeKm * 500;
			if (num > 0.0)
			{
				Static.WorldBoundaries = new BoundingBoxD(new Vector3D(0.0 - num, 0.0 - num, 0.0 - num), new Vector3D(num, num, num));
			}
			MyVisualScriptLogicProvider.Init();
			Static.InGameTime = generationArgs.Scenario.GameDate;
			Static.RequiresDX = (generationArgs.Scenario.HasPlanets ? 11 : 9);
			if (Static.OnlineMode != 0 && !StartServerRequest(out MyLobbyStatusCode statusCode))
			{
				Static.ShowLoadingError(lobbyFailed: true, statusCode);
				return;
			}
			Static.IsCameraAwaitingEntity = true;
			string text = MyUtils.StripInvalidChars(name);
			Static.CurrentPath = MyLocalCache.GetSessionSavesPath(text, contentFolder: false, createIfNotExists: false);
			while (Directory.Exists(Static.CurrentPath))
			{
				Static.CurrentPath = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
			}
			Static.PrepareBaseSession(mods, generationArgs.Scenario);
			MySector.EnvironmentDefinition = MyDefinitionManager.Static.GetDefinition<MyEnvironmentDefinition>(generationArgs.Scenario.Environment);
			MyWorldGenerator.GenerateWorld(generationArgs);
			if (Sync.IsServer)
			{
				Static.InitializeFactions();
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyToolBarCollection.RequestCreateToolbar(new MyPlayer.PlayerId(Sync.MyId, 0));
			}
			string scenario = generationArgs.Scenario.DisplayNameText.ToString();
			Static.LogSettings(scenario, generationArgs.AsteroidAmount);
			if (generationArgs.Scenario.SunDirection.IsValid())
			{
				MySector.SunProperties.SunDirectionNormalized = Vector3.Normalize(generationArgs.Scenario.SunDirection);
				MySector.SunProperties.BaseSunDirectionNormalized = Vector3.Normalize(generationArgs.Scenario.SunDirection);
			}
			MyPrefabManager.FinishedProcessingGrids.Reset();
			if (MyPrefabManager.PendingGrids > 0)
			{
				MyPrefabManager.FinishedProcessingGrids.WaitOne();
			}
			Parallel.RunCallbacks();
			MyEntities.UpdateOnceBeforeFrame();
			Static.BeforeStartComponents();
			Static.Save();
			Static.SessionSimSpeedPlayer = 0f;
			Static.SessionSimSpeedServer = 0f;
			MySpectatorCameraController.Static.InitLight(isLightOn: false);
		}

		internal static void LoadMultiplayer(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			if (MyFakes.ENABLE_PRELOAD_CHARACTER_ANIMATIONS)
			{
				PreloadAnimations("Models\\Characters\\Animations");
			}
			Static = new MySession(multiplayerSession.SyncLayer);
			Static.Mods = world.Checkpoint.Mods;
			Static.Settings = world.Checkpoint.Settings;
			Static.CurrentPath = MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(world.Checkpoint.SessionName), contentFolder: false, createIfNotExists: false);
			if (!MyDefinitionManager.Static.TryGetDefinition(world.Checkpoint.Scenario, out Static.Scenario))
			{
				Static.Scenario = MyDefinitionManager.Static.GetScenarioDefinitions().FirstOrDefault();
			}
			FixIncorrectSettings(Static.Settings);
			Static.WorldBoundaries = world.Checkpoint.WorldBoundaries;
			Static.InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
			Static.LoadMembersFromWorld(world, multiplayerSession);
			MySandboxGame.Static.SessionCompatHelper.FixSessionComponentObjectBuilders(world.Checkpoint, world.Sector);
			Static.PrepareBaseSession(world.Checkpoint, world.Sector);
			if (MyFakes.MP_SYNC_CLUSTERTREE)
			{
				MyPhysics.DeserializeClusters(world.Clusters);
			}
			foreach (MyObjectBuilder_Planet planet in world.Planets)
			{
				MyPlanet myPlanet = new MyPlanet();
				MyPlanetStorageProvider myPlanetStorageProvider = new MyPlanetStorageProvider();
				myPlanetStorageProvider.Init(generator: MyDefinitionManager.Static.GetDefinition<MyPlanetGeneratorDefinition>(MyStringHash.GetOrCompute(planet.PlanetGenerator)), seed: planet.Seed, radius: planet.Radius);
				VRage.Game.Voxels.IMyStorage storage = new MyOctreeStorage(myPlanetStorageProvider, myPlanetStorageProvider.StorageSize);
				myPlanet.Init(planet, storage);
				MyEntities.Add(myPlanet);
			}
			_ = world.Checkpoint.ControlledObject;
			world.Checkpoint.ControlledObject = -1L;
			if (multiplayerSession != null)
			{
				Static.Gpss.RegisterChat(multiplayerSession);
			}
			Static.CameraController = MySpectatorCameraController.Static;
			Static.LoadWorld(world.Checkpoint, world.Sector);
			if (Sync.IsServer)
			{
				Static.InitializeFactions();
			}
			Static.Settings.AutoSaveInMinutes = 0u;
			Static.IsCameraAwaitingEntity = true;
			MyGeneralStats.Clear();
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				CacheGuiTexturePaths();
			}
			Static.BeforeStartComponents();
		}

		public static void LoadMission(string sessionPath, MyObjectBuilder_Checkpoint checkpoint, ulong checkpointSizeInBytes, bool persistentEditMode)
		{
			LoadMission(sessionPath, checkpoint, checkpointSizeInBytes, checkpoint.SessionName, checkpoint.Description);
			Static.PersistentEditMode = persistentEditMode;
			Static.LoadedAsMission = true;
		}

		public static void LoadMission(string sessionPath, MyObjectBuilder_Checkpoint checkpoint, ulong checkpointSizeInBytes, string name, string description)
		{
			MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Load);
			Load(sessionPath, checkpoint, checkpointSizeInBytes);
			Static.Name = name;
			Static.Description = description;
			string text = MyUtils.StripInvalidChars(checkpoint.SessionName);
			Static.CurrentPath = MyLocalCache.GetSessionSavesPath(text, contentFolder: false, createIfNotExists: false);
			while (Directory.Exists(Static.CurrentPath))
			{
				Static.CurrentPath = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
			}
		}

		public static void Load(string sessionPath, MyObjectBuilder_Checkpoint checkpoint, ulong checkpointSizeInBytes, bool saveLastStates = true, bool allowXml = true)
		{
			MyLog.Default.WriteLineAndConsole("Loading session: " + sessionPath);
			MyEntityIdentifier.Reset();
			MyEntityIdentifier.SetSingleThreadClearWarnings(MySandboxGame.Config.SyncRendering);
			bool needsXml = false;
			ulong sizeInBytes;
			MyObjectBuilder_Sector myObjectBuilder_Sector = MyLocalCache.LoadSector(sessionPath, checkpoint.CurrentSector, allowXml, out sizeInBytes, out needsXml);
			if (myObjectBuilder_Sector == null)
			{
				if (!allowXml && needsXml)
				{
					throw new MyLoadingNeedXMLException();
				}
				throw new ApplicationException("Sector could not be loaded");
			}
			ulong voxelsSizeInBytes = GetVoxelsSizeInBytes(sessionPath);
			MyCubeGrid.Preload();
			Static = new MySession();
			Static.Name = MyStatControlText.SubstituteTexts(checkpoint.SessionName);
			Static.Description = checkpoint.Description;
			Static.Mods = checkpoint.Mods;
			Static.Settings = checkpoint.Settings;
			Static.CurrentPath = sessionPath;
			Static.WorldSizeInBytes = checkpointSizeInBytes + sizeInBytes + voxelsSizeInBytes;
			MyLog.Default.WriteLineAndConsole("Experimental mode: " + (Static.IsSettingsExperimental() ? "Yes" : "No"));
			MyLog.Default.WriteLineAndConsole("Experimental mode reason: " + Static.GetSettingsExperimentalReason());
			if (!Sandbox.Engine.Platform.Game.IsDedicated && Static.OnlineMode != 0 && !StartServerRequest(out MyLobbyStatusCode statusCode))
			{
				Static.ShowLoadingError(lobbyFailed: true, statusCode);
				return;
			}
			if (MySession.BeforeLoading != null)
			{
				MySession.BeforeLoading();
			}
			MySandboxGame.Static.SessionCompatHelper.FixSessionComponentObjectBuilders(checkpoint, myObjectBuilder_Sector);
			Static.PrepareBaseSession(checkpoint, myObjectBuilder_Sector);
			MyVisualScriptLogicProvider.Init();
			Static.LoadWorld(checkpoint, myObjectBuilder_Sector);
			if (Sync.IsServer)
			{
				Static.InitializeFactions();
			}
			if (saveLastStates)
			{
				MyLocalCache.SaveLastSessionInfo(sessionPath, isOnline: false, isLobby: false, Static.Name, null, 0);
			}
			Static.LogSettings();
			MyHud.Notifications.Get(MyNotificationSingletons.WorldLoaded).SetTextFormatArguments(Static.Name);
			MyHud.Notifications.Add(MyNotificationSingletons.WorldLoaded);
			if (!MyFakes.LOAD_UNCONTROLLED_CHARACTERS && !MySessionComponentReplay.Static.HasAnyData)
			{
				Static.RemoveUncontrolledCharacters();
			}
			MyGeneralStats.Clear();
			MyHudChat.ResetChatSettings();
			Static.BeforeStartComponents();
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				CacheGuiTexturePaths();
			}
			RaiseAfterLoading();
			if (!Sandbox.Engine.Platform.Game.IsDedicated && Static.LocalCharacter != null)
			{
				MyLocalCache.LoadInventoryConfig(Static.LocalCharacter);
			}
			MyLog.Default.WriteLineAndConsole("Session loaded");
		}

		private static void CacheGuiTexturePaths()
		{
			foreach (MyFactionIconsDefinition allDefinition in MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>())
			{
				if (allDefinition.Icons != null && allDefinition.Icons.Length != 0)
				{
					for (int i = 0; i < allDefinition.Icons.Length; i++)
					{
						if (!string.IsNullOrEmpty(allDefinition.Icons[i]))
						{
							ImageManager.Instance.AddImage(allDefinition.Icons[i]);
						}
					}
				}
			}
			foreach (MyPhysicalItemDefinition physicalItemDefinition in MyDefinitionManager.Static.GetPhysicalItemDefinitions())
			{
				if (physicalItemDefinition.Icons != null && physicalItemDefinition.Icons.Length != 0 && !string.IsNullOrEmpty(physicalItemDefinition.Icons[0]))
				{
					ImageManager.Instance.AddImage(physicalItemDefinition.Icons[0]);
				}
			}
			foreach (MyPrefabDefinition value in MyDefinitionManager.Static.GetPrefabDefinitions().Values)
			{
				if (!value.Context.IsBaseGame)
				{
					string modPath = value.Context.ModPath;
					if (value.Icons != null && value.Icons.Length == 1 && !string.IsNullOrEmpty(value.Icons[0]))
					{
						value.Icons[0] = Path.Combine(modPath, value.Icons[0]);
						ImageManager.Instance.AddImage(value.Icons[0]);
					}
					if (!string.IsNullOrEmpty(value.TooltipImage))
					{
						value.TooltipImage = Path.Combine(modPath, value.TooltipImage);
						ImageManager.Instance.AddImage(value.TooltipImage);
					}
				}
			}
		}

		private static void PreloadAnimations(string relativeDirectory)
		{
			IEnumerable<string> files = MyFileSystem.GetFiles(Path.Combine(MyFileSystem.ContentPath, relativeDirectory), "*.mwm", MySearchOption.AllDirectories);
			if (files != null && files.Any())
			{
				foreach (string item in files)
				{
					MyModels.GetModelOnlyAnimationData(item.Replace(MyFileSystem.ContentPath, string.Empty).TrimStart(new char[1]
					{
						Path.DirectorySeparatorChar
					}));
				}
			}
		}

		internal static void CreateWithEmptyWorld(MyMultiplayerBase multiplayerSession)
		{
			Static = new MySession(multiplayerSession.SyncLayer, registerComponents: false);
			Static.InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
			Static.Gpss.RegisterChat(multiplayerSession);
			Static.CameraController = MySpectatorCameraController.Static;
			Static.Settings = new MyObjectBuilder_SessionSettings();
			Static.Settings.AutoSaveInMinutes = 0u;
			Static.IsCameraAwaitingEntity = true;
			Static.PrepareBaseSession(new List<MyObjectBuilder_Checkpoint.ModItem>());
			multiplayerSession.StartProcessingClientMessagesWithEmptyWorld();
			if (Sync.IsServer)
			{
				Static.InitializeFactions();
			}
			MyLocalCache.ClearLastSessionInfo();
			if (!Sandbox.Engine.Platform.Game.IsDedicated && Static.LocalHumanPlayer == null)
			{
				Sync.Players.RequestNewPlayer(0, MyGameService.UserName, null, realPlayer: true, initialPlayer: true);
			}
			MyGeneralStats.Clear();
		}

		internal void LoadMultiplayerWorld(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			Static.UnloadDataComponents(beforeLoadWorld: true);
			MyDefinitionManager.Static.UnloadData();
			Static.Mods = world.Checkpoint.Mods;
			Static.Settings = world.Checkpoint.Settings;
			Static.CurrentPath = MyLocalCache.GetSessionSavesPath(MyUtils.StripInvalidChars(world.Checkpoint.SessionName), contentFolder: false, createIfNotExists: false);
			if (!MyDefinitionManager.Static.TryGetDefinition(world.Checkpoint.Scenario, out Static.Scenario))
			{
				Static.Scenario = MyDefinitionManager.Static.GetScenarioDefinitions().FirstOrDefault();
			}
			FixIncorrectSettings(Static.Settings);
			Static.InGameTime = MyObjectBuilder_Checkpoint.DEFAULT_DATE;
			MySandboxGame.Static.SessionCompatHelper.FixSessionComponentObjectBuilders(world.Checkpoint, world.Sector);
			Static.PrepareBaseSession(world.Checkpoint, world.Sector);
			_ = world.Checkpoint.ControlledObject;
			world.Checkpoint.ControlledObject = -1L;
			Static.Gpss.RegisterChat(multiplayerSession);
			Static.CameraController = MySpectatorCameraController.Static;
			Static.LoadWorld(world.Checkpoint, world.Sector);
			if (Sync.IsServer)
			{
				Static.InitializeFactions();
			}
			Static.Settings.AutoSaveInMinutes = 0u;
			Static.IsCameraAwaitingEntity = true;
			MyLocalCache.ClearLastSessionInfo();
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				CacheGuiTexturePaths();
			}
			Static.BeforeStartComponents();
		}

		private void LoadMembersFromWorld(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			if (multiplayerSession is MyMultiplayerClient)
			{
				(multiplayerSession as MyMultiplayerClient).LoadMembersFromWorld(world.Checkpoint.Clients);
			}
		}

		private void RemoveUncontrolledCharacters()
		{
			if (Sync.IsServer)
			{
				foreach (MyCharacter item in MyEntities.GetEntities().OfType<MyCharacter>())
				{
					if (item.ControllerInfo.Controller == null || (item.ControllerInfo.IsRemotelyControlled() && item.GetCurrentMovementState() != MyCharacterMovementEnum.Died))
					{
						MyLargeTurretBase myLargeTurretBase = ControlledEntity as MyLargeTurretBase;
						if (myLargeTurretBase == null || myLargeTurretBase.Pilot != item)
						{
							MyRemoteControl myRemoteControl = ControlledEntity as MyRemoteControl;
							if (myRemoteControl == null || myRemoteControl.Pilot != item)
							{
								item.Close();
							}
						}
					}
				}
				foreach (MyCubeGrid item2 in MyEntities.GetEntities().OfType<MyCubeGrid>())
				{
					foreach (MySlimBlock block in item2.GetBlocks())
					{
						MyCockpit myCockpit = block.FatBlock as MyCockpit;
						if (myCockpit != null && !(myCockpit is MyCryoChamber) && myCockpit.Pilot != null && myCockpit.Pilot != LocalCharacter)
						{
							myCockpit.Pilot.Close();
							myCockpit.ClearSavedpilot();
						}
					}
				}
			}
		}

		private static bool StartServerRequest(out MyLobbyStatusCode statusCode)
		{
			if (MyGameService.IsOnline)
			{
				Static.UnloadMultiplayer();
				MyNetworkMonitor.StartSession();
				MyMultiplayerHostResult myMultiplayerHostResult = MyMultiplayer.HostLobby(GetLobbyType(Static.OnlineMode), Static.MaxPlayers, Static.SyncLayer);
				myMultiplayerHostResult.Done += OnMultiplayerHost;
				myMultiplayerHostResult.Wait();
				statusCode = myMultiplayerHostResult.StatusCode;
				return myMultiplayerHostResult.Success;
			}
			statusCode = MyLobbyStatusCode.NoUser;
			return false;
		}

		private static MyLobbyType GetLobbyType(MyOnlineModeEnum onlineMode)
		{
			switch (onlineMode)
			{
			case MyOnlineModeEnum.FRIENDS:
				return MyLobbyType.FriendsOnly;
			case MyOnlineModeEnum.PUBLIC:
				return MyLobbyType.Public;
			case MyOnlineModeEnum.PRIVATE:
				return MyLobbyType.Private;
			default:
				return MyLobbyType.Private;
			}
		}

		private static void OnMultiplayerHost(bool success, MyLobbyStatusCode reason, MyMultiplayerBase multiplayer)
		{
			if (success)
			{
				Static.StartServer(multiplayer);
			}
		}

		private void LoadWorld(MyObjectBuilder_Checkpoint checkpoint, MyObjectBuilder_Sector sector)
		{
			MySandboxGame.Static.SessionCompatHelper.FixSessionObjectBuilders(checkpoint, sector);
			MyEntities.MemoryLimitAddFailureReset();
			ElapsedGameTime = new TimeSpan(checkpoint.ElapsedGameTime);
			InGameTime = checkpoint.InGameTime;
			Name = MyStatControlText.SubstituteTexts(checkpoint.SessionName);
			Description = checkpoint.Description;
			if (checkpoint.PromotedUsers != null)
			{
				PromotedUsers = checkpoint.PromotedUsers.Dictionary;
			}
			else
			{
				PromotedUsers = new Dictionary<ulong, MyPromoteLevel>();
			}
			m_remoteAdminSettings.Clear();
			foreach (KeyValuePair<ulong, int> item in checkpoint.RemoteAdminSettings.Dictionary)
			{
				m_remoteAdminSettings[item.Key] = (AdminSettingsEnum)item.Value;
				if (!Sync.IsDedicated && item.Key == Sync.MyId)
				{
					m_adminSettings = (AdminSettingsEnum)item.Value;
				}
			}
			if (checkpoint.CreativeTools != null)
			{
				m_creativeTools = checkpoint.CreativeTools;
			}
			else
			{
				m_creativeTools = new HashSet<ulong>();
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				foreach (KeyValuePair<ulong, MyPromoteLevel> item2 in PromotedUsers.Where((KeyValuePair<ulong, MyPromoteLevel> e) => e.Value == MyPromoteLevel.Owner).ToList())
				{
					PromotedUsers.Remove(item2.Key);
				}
				foreach (string administrator in MySandboxGame.ConfigDedicated.Administrators)
				{
					if (ulong.TryParse(administrator, out ulong result))
					{
						PromotedUsers[result] = MyPromoteLevel.Owner;
					}
				}
			}
			Briefing = checkpoint.Briefing;
			BriefingVideo = checkpoint.BriefingVideo;
			WorkshopId = checkpoint.WorkshopId;
			Password = checkpoint.Password;
			PreviousEnvironmentHostility = checkpoint.PreviousEnvironmentHostility;
			RequiresDX = checkpoint.RequiresDX;
			CustomLoadingScreenImage = checkpoint.CustomLoadingScreenImage;
			CustomLoadingScreenText = checkpoint.CustomLoadingScreenText;
			CustomSkybox = checkpoint.CustomSkybox;
			FixIncorrectSettings(Settings);
			AppVersionFromSave = checkpoint.AppVersion;
			MyToolbarComponent.InitCharacterToolbar(checkpoint.CharacterToolbar);
			LoadCameraControllerSettings(checkpoint);
			Sync.Players.RespawnComponent.InitFromCheckpoint(checkpoint);
			MyPlayer.PlayerId playerId = default(MyPlayer.PlayerId);
			MyPlayer.PlayerId? savingPlayerId = null;
			if (TryFindSavingPlayerId(checkpoint.ControlledEntities, checkpoint.ControlledObject, out playerId) && (!IsScenario || Static.OnlineMode == MyOnlineModeEnum.OFFLINE))
			{
				savingPlayerId = playerId;
			}
			if (Sync.IsServer || (!IsScenario && MyPerGameSettings.Game == GameEnum.SE_GAME))
			{
				Sync.Players.LoadIdentities(checkpoint, savingPlayerId);
			}
			GlobalBlockLimits = new MyBlockLimits(Static.TotalPCU, 0);
			PirateBlockLimits = new MyBlockLimits(Static.PiratePCU, 0);
			Toolbars.LoadToolbars(checkpoint);
			if (checkpoint.Factions != null && (Sync.IsServer || (!IsScenario && MyPerGameSettings.Game == GameEnum.SE_GAME)))
			{
				Static.Factions.Init(checkpoint.Factions);
			}
			if (!MyEntities.Load(sector.SectorObjects))
			{
				ShowLoadingError();
				return;
			}
			Parallel.RunCallbacks();
			MySandboxGame.Static.SessionCompatHelper.AfterEntitiesLoad(sector.AppVersion);
			MyGlobalEvents.LoadEvents(sector.SectorEvents);
			MySpectatorCameraController.Static.InitLight(checkpoint.SpectatorIsLightOn);
			if (Sync.IsServer)
			{
				MySpectatorCameraController.Static.SetViewMatrix(MatrixD.Invert(checkpoint.SpectatorPosition.GetMatrix()));
				MySpectatorCameraController.Static.SpeedModeLinear = checkpoint.SpectatorSpeed.X;
				MySpectatorCameraController.Static.SpeedModeAngular = checkpoint.SpectatorSpeed.Y;
			}
			if (!IsScenario || !Static.Settings.StartInRespawnScreen)
			{
				Sync.Players.LoadConnectedPlayers(checkpoint, savingPlayerId);
				Sync.Players.LoadControlledEntities(checkpoint.ControlledEntities, checkpoint.ControlledObject, savingPlayerId);
			}
			else
			{
				Static.Settings.StartInRespawnScreen = false;
			}
			LoadCamera(checkpoint);
			if (CreativeMode && !Sandbox.Engine.Platform.Game.IsDedicated && LocalHumanPlayer != null && LocalHumanPlayer.Character != null && LocalHumanPlayer.Character.IsDead)
			{
				MyPlayerCollection.RequestLocalRespawn();
			}
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.OnSessionReady();
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated && LocalHumanPlayer == null)
			{
				Sync.Players.RequestNewPlayer(0, MyGameService.UserName, null, realPlayer: true, initialPlayer: true);
			}
			else if (ControlledEntity == null && Sync.IsServer && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyLog.Default.WriteLine("ControlledObject was null, respawning character");
				m_cameraAwaitingEntity = true;
				MyPlayerCollection.RequestLocalRespawn();
			}
			SharedToolbar = checkpoint.SharedToolbar;
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyPlayer.PlayerId pid = new MyPlayer.PlayerId(Sync.MyId, 0);
				MyToolbar myToolbar = Toolbars.TryGetPlayerToolbar(pid);
				if (checkpoint.SharedToolbar != 0L)
				{
					MyPlayer.PlayerId pid2 = new MyPlayer.PlayerId(checkpoint.SharedToolbar, 0);
					MyToolbar myToolbar2 = Toolbars.TryGetPlayerToolbar(pid2);
					if (myToolbar2 != null)
					{
						myToolbar = myToolbar2;
					}
				}
				if (myToolbar == null)
				{
					MyToolBarCollection.RequestCreateToolbar(pid);
					MyToolbarComponent.InitCharacterToolbar(Scenario.DefaultToolbar);
				}
				else
				{
					MyToolbarComponent.InitCharacterToolbar(myToolbar.GetObjectBuilder());
				}
				GetComponent<MyRadialMenuComponent>().InitDefaultLastUsed(Scenario.DefaultToolbar);
			}
			Gpss.LoadGpss(checkpoint);
			if (MyFakes.ENABLE_MISSION_TRIGGERS)
			{
				MySessionComponentMissionTriggers.Static.Load(checkpoint.MissionTriggers);
			}
			MyRenderProxy.RebuildCullingStructure();
			Settings.ResetOwnership = false;
			if (!CreativeMode)
			{
				MyDebugDrawSettings.DEBUG_DRAW_PHYSICS = false;
			}
			MyRenderProxy.CollectGarbage();
		}

		private bool TryFindSavingPlayerId(SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> controlledEntities, long controlledObject, out MyPlayer.PlayerId playerId)
		{
			playerId = default(MyPlayer.PlayerId);
			if (!MyFakes.REUSE_OLD_PLAYER_IDENTITY)
			{
				return false;
			}
			if (!Sync.IsServer || Sync.Clients.Count != 1)
			{
				return false;
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return false;
			}
			if (controlledEntities == null)
			{
				return false;
			}
			bool flag = false;
			foreach (KeyValuePair<long, MyObjectBuilder_Checkpoint.PlayerId> item in controlledEntities.Dictionary)
			{
				if (item.Key == controlledObject)
				{
					playerId = new MyPlayer.PlayerId(item.Value.ClientId, item.Value.SerialId);
				}
				if (item.Value.ClientId == Sync.MyId && item.Value.SerialId == 0)
				{
					flag = true;
				}
			}
			return !flag;
		}

		private void LoadCamera(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (checkpoint.SpectatorDistance > 0f)
			{
				MyThirdPersonSpectator.Static.UpdateAfterSimulation();
				MyThirdPersonSpectator.Static.ResetViewerDistance(checkpoint.SpectatorDistance);
			}
			MySandboxGame.Log.WriteLine("Checkpoint.CameraAttachedTo: " + checkpoint.CameraEntity);
			IMyEntity myEntity = null;
			MyCameraControllerEnum myCameraControllerEnum = checkpoint.CameraController;
			if (!Static.Enable3RdPersonView && myCameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator)
			{
				myCameraControllerEnum = (checkpoint.CameraController = MyCameraControllerEnum.Entity);
			}
			if (checkpoint.CameraEntity == 0L && ControlledEntity != null)
			{
				myEntity = (ControlledEntity as MyEntity);
				if (myEntity != null)
				{
					MyRemoteControl myRemoteControl = ControlledEntity as MyRemoteControl;
					if (myRemoteControl != null)
					{
						myEntity = myRemoteControl.Pilot;
					}
					else if (!(ControlledEntity is IMyCameraController))
					{
						myEntity = null;
						myCameraControllerEnum = MyCameraControllerEnum.Spectator;
					}
				}
			}
			else if (!MyEntities.EntityExists(checkpoint.CameraEntity))
			{
				myEntity = (ControlledEntity as MyEntity);
				if (myEntity != null)
				{
					myCameraControllerEnum = MyCameraControllerEnum.Entity;
					if (!(ControlledEntity is IMyCameraController))
					{
						myEntity = null;
						myCameraControllerEnum = MyCameraControllerEnum.Spectator;
					}
				}
				else
				{
					MyLog.Default.WriteLine("ERROR: Camera entity from checkpoint does not exists!");
					myCameraControllerEnum = MyCameraControllerEnum.Spectator;
				}
			}
			else
			{
				myEntity = MyEntities.GetEntityById(checkpoint.CameraEntity);
			}
			if (myCameraControllerEnum == MyCameraControllerEnum.Spectator && myEntity != null)
			{
				myCameraControllerEnum = MyCameraControllerEnum.Entity;
			}
			MyEntityCameraSettings cameraSettings = null;
			bool flag = false;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && (myCameraControllerEnum == MyCameraControllerEnum.Entity || myCameraControllerEnum == MyCameraControllerEnum.ThirdPersonSpectator) && myEntity != null)
			{
				MyPlayer.PlayerId pid = (LocalHumanPlayer == null) ? new MyPlayer.PlayerId(Sync.MyId, 0) : LocalHumanPlayer.Id;
				if (Static.Cameras.TryGetCameraSettings(pid, myEntity.EntityId, myEntity is MyCharacter && LocalCharacter == myEntity, out cameraSettings) && !cameraSettings.IsFirstPerson)
				{
					myCameraControllerEnum = MyCameraControllerEnum.ThirdPersonSpectator;
					flag = true;
				}
			}
			Static.IsCameraAwaitingEntity = false;
			SetCameraController(myCameraControllerEnum, myEntity);
			if (flag)
			{
				MyThirdPersonSpectator.Static.ResetViewerAngle(cameraSettings.HeadAngle);
				MyThirdPersonSpectator.Static.ResetViewerDistance(cameraSettings.Distance);
			}
		}

		private void LoadCameraControllerSettings(MyObjectBuilder_Checkpoint checkpoint)
		{
			Cameras.LoadCameraCollection(checkpoint);
		}

		internal static void FixIncorrectSettings(MyObjectBuilder_SessionSettings settings)
		{
			MyObjectBuilder_SessionSettings myObjectBuilder_SessionSettings = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();
			_ = settings.EnableWolfs;
			_ = settings.EnableSpiders;
			if (settings.RefinerySpeedMultiplier <= 0f)
			{
				settings.RefinerySpeedMultiplier = myObjectBuilder_SessionSettings.RefinerySpeedMultiplier;
			}
			if (settings.AssemblerSpeedMultiplier <= 0f)
			{
				settings.AssemblerSpeedMultiplier = myObjectBuilder_SessionSettings.AssemblerSpeedMultiplier;
			}
			if (settings.AssemblerEfficiencyMultiplier <= 0f)
			{
				settings.AssemblerEfficiencyMultiplier = myObjectBuilder_SessionSettings.AssemblerEfficiencyMultiplier;
			}
			if (settings.InventorySizeMultiplier <= 0f)
			{
				settings.InventorySizeMultiplier = myObjectBuilder_SessionSettings.InventorySizeMultiplier;
			}
			if (settings.WelderSpeedMultiplier <= 0f)
			{
				settings.WelderSpeedMultiplier = myObjectBuilder_SessionSettings.WelderSpeedMultiplier;
			}
			if (settings.GrinderSpeedMultiplier <= 0f)
			{
				settings.GrinderSpeedMultiplier = myObjectBuilder_SessionSettings.GrinderSpeedMultiplier;
			}
			if (settings.HackSpeedMultiplier <= 0f)
			{
				settings.HackSpeedMultiplier = myObjectBuilder_SessionSettings.HackSpeedMultiplier;
			}
			if (!settings.PermanentDeath.HasValue)
			{
				settings.PermanentDeath = true;
			}
			settings.ViewDistance = MathHelper.Clamp(settings.ViewDistance, 1000, 50000);
			settings.SyncDistance = MathHelper.Clamp(settings.SyncDistance, 1000, 20000);
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				settings.Scenario = false;
				settings.ScenarioEditMode = false;
			}
			if (Static != null && Static.Scenario != null)
			{
				settings.WorldSizeKm = ((!Static.Scenario.HasPlanets) ? settings.WorldSizeKm : 0);
			}
			if (Static != null && !Static.WorldBoundaries.HasValue && settings.WorldSizeKm > 0)
			{
				double num = settings.WorldSizeKm * 500;
				if (num > 0.0)
				{
					Static.WorldBoundaries = new BoundingBoxD(new Vector3D(0.0 - num, 0.0 - num, 0.0 - num), new Vector3D(num, num, num));
				}
			}
		}

		private void ShowLoadingError(bool lobbyFailed = false, MyLobbyStatusCode statusCode = MyLobbyStatusCode.Error)
		{
			StringBuilder stringBuilder;
			if (lobbyFailed)
			{
				_ = MyCommonTexts.MessageBoxCaptionError;
				stringBuilder = MyJoinGameHelper.GetErrorMessage(statusCode);
			}
			else if (MyEntities.MemoryLimitAddFailure)
			{
				_ = MyCommonTexts.MessageBoxCaptionWarning;
				stringBuilder = MyTexts.Get(MyCommonTexts.MessageBoxTextMemoryLimitReachedDuringLoad);
			}
			else
			{
				_ = MyCommonTexts.MessageBoxCaptionError;
				stringBuilder = MyTexts.Get(MyCommonTexts.MessageBoxTextErrorLoadingEntities);
			}
			throw new MyLoadingException(stringBuilder.ToString());
		}

		internal void FixMissingCharacter()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				bool flag = ControlledEntity != null && ControlledEntity is MyCockpit;
				bool flag2 = MyEntities.GetEntities().OfType<MyCharacter>().Any();
				bool flag3 = ControlledEntity != null && ControlledEntity is MyRemoteControl && (ControlledEntity as MyRemoteControl).WasControllingCockpitWhenSaved();
				bool flag4 = ControlledEntity != null && ControlledEntity is MyLargeTurretBase && (ControlledEntity as MyLargeTurretBase).WasControllingCockpitWhenSaved();
				if (!MyInput.Static.ENABLE_DEVELOPER_KEYS && !flag && !flag2 && !flag3 && !flag4)
				{
					MyPlayerCollection.RequestLocalRespawn();
				}
			}
		}

		public MyCameraControllerEnum GetCameraControllerEnum()
		{
			if (CameraController == MySpectatorCameraController.Static)
			{
				switch (MySpectatorCameraController.Static.SpectatorCameraMovement)
				{
				case MySpectatorCameraMovementEnum.UserControlled:
					return MyCameraControllerEnum.Spectator;
				case MySpectatorCameraMovementEnum.ConstantDelta:
					return MyCameraControllerEnum.SpectatorDelta;
				case MySpectatorCameraMovementEnum.None:
					return MyCameraControllerEnum.SpectatorFixed;
				case MySpectatorCameraMovementEnum.Orbit:
					return MyCameraControllerEnum.SpectatorOrbit;
				}
			}
			else
			{
				if (CameraController == MyThirdPersonSpectator.Static)
				{
					return MyCameraControllerEnum.ThirdPersonSpectator;
				}
				if (CameraController is MyEntity || CameraController is MyEntityRespawnComponentBase)
				{
					if ((!CameraController.IsInFirstPersonView && !CameraController.ForceFirstPersonCamera) || !CameraController.EnableFirstPersonView)
					{
						return MyCameraControllerEnum.ThirdPersonSpectator;
					}
					return MyCameraControllerEnum.Entity;
				}
			}
			return MyCameraControllerEnum.Spectator;
		}

		[Event(null, 2489)]
		[Client]
		[Reliable]
		public static void SetSpectatorPositionFromServer(Vector3D position)
		{
			Static.SetCameraController(MyCameraControllerEnum.Spectator, null, position);
		}

		public void SetCameraController(MyCameraControllerEnum cameraControllerEnum, IMyEntity cameraEntity = null, Vector3D? position = null)
		{
			if (cameraEntity != null && Spectator.Position == Vector3.Zero)
			{
				Spectator.Position = cameraEntity.GetPosition() + cameraEntity.WorldMatrix.Forward * 4.0 + cameraEntity.WorldMatrix.Up * 2.0;
				Spectator.SetTarget(cameraEntity.GetPosition(), cameraEntity.PositionComp.WorldMatrix.Up);
				Spectator.Initialized = true;
			}
			CameraOnCharacter = (cameraEntity is MyCharacter);
			switch (cameraControllerEnum)
			{
			case MyCameraControllerEnum.Entity:
			{
				MyEntityRespawnComponentBase component;
				if (cameraEntity is IMyCameraController)
				{
					Static.CameraController = (IMyCameraController)cameraEntity;
				}
				else if (cameraEntity.Components.TryGet(out component))
				{
					Static.CameraController = component;
				}
				else
				{
					Static.CameraController = LocalCharacter;
				}
				break;
			}
			case MyCameraControllerEnum.Spectator:
				Static.CameraController = MySpectatorCameraController.Static;
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.UserControlled;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			case MyCameraControllerEnum.SpectatorFixed:
				Static.CameraController = MySpectatorCameraController.Static;
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.None;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			case MyCameraControllerEnum.SpectatorDelta:
				Static.CameraController = MySpectatorCameraController.Static;
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.ConstantDelta;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			case MyCameraControllerEnum.SpectatorFreeMouse:
				Static.CameraController = MySpectatorCameraController.Static;
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.FreeMouse;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			case MyCameraControllerEnum.ThirdPersonSpectator:
				if (cameraEntity != null)
				{
					Static.CameraController = (IMyCameraController)cameraEntity;
				}
				Static.CameraController.IsInFirstPersonView = false;
				break;
			case MyCameraControllerEnum.SpectatorOrbit:
				Static.CameraController = MySpectatorCameraController.Static;
				MySpectatorCameraController.Static.SpectatorCameraMovement = MySpectatorCameraMovementEnum.Orbit;
				if (position.HasValue)
				{
					MySpectatorCameraController.Static.Position = position.Value;
				}
				break;
			}
		}

		public void SetEntityCameraPosition(MyPlayer.PlayerId pid, IMyEntity cameraEntity)
		{
			if (LocalHumanPlayer == null || LocalHumanPlayer.Id != pid)
			{
				return;
			}
			if (Cameras.TryGetCameraSettings(pid, cameraEntity.EntityId, cameraEntity is MyCharacter && LocalCharacter == cameraEntity, out MyEntityCameraSettings cameraSettings))
			{
				if (!cameraSettings.IsFirstPerson)
				{
					SetCameraController(MyCameraControllerEnum.ThirdPersonSpectator, cameraEntity);
					MyThirdPersonSpectator.Static.ResetViewerAngle(cameraSettings.HeadAngle);
					MyThirdPersonSpectator.Static.ResetViewerDistance(cameraSettings.Distance);
				}
			}
			else if (GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator)
			{
				MyThirdPersonSpectator.Static.RecalibrateCameraPosition(cameraEntity is MyCharacter);
				MyThirdPersonSpectator.Static.ResetSpring();
				MyThirdPersonSpectator.Static.UpdateZoom();
			}
		}

		public bool IsCameraControlledObject()
		{
			return ControlledEntity == Static.CameraController;
		}

		public bool IsCameraUserControlledSpectator()
		{
			if (MySpectatorCameraController.Static != null)
			{
				if (Static.CameraController == MySpectatorCameraController.Static)
				{
					if (MySpectatorCameraController.Static.SpectatorCameraMovement != 0 && MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.Orbit)
					{
						return MySpectatorCameraController.Static.SpectatorCameraMovement == MySpectatorCameraMovementEnum.FreeMouse;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		public bool IsCameraUserAnySpectator()
		{
			if (MySpectatorCameraController.Static != null)
			{
				if (Static.CameraController == MySpectatorCameraController.Static)
				{
					return MySpectatorCameraController.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.None;
				}
				return false;
			}
			return true;
		}

		public float GetCameraTargetDistance()
		{
			return (float)MyThirdPersonSpectator.Static.GetViewerDistance();
		}

		public void SetCameraTargetDistance(double distance)
		{
			MyThirdPersonSpectator.Static.ResetViewerDistance((distance == 0.0) ? null : new double?(distance));
		}

		public void SaveControlledEntityCameraSettings(bool isFirstPerson)
		{
			if (ControlledEntity != null && LocalHumanPlayer != null)
			{
				MyCharacter myCharacter = ControlledEntity as MyCharacter;
				if (myCharacter == null || !myCharacter.IsDead)
				{
					Cameras.SaveEntityCameraSettings(LocalHumanPlayer.Id, ControlledEntity.Entity.EntityId, isFirstPerson, MyThirdPersonSpectator.Static.GetViewerDistance(), myCharacter != null && LocalCharacter == ControlledEntity, ControlledEntity.HeadLocalXAngle, ControlledEntity.HeadLocalYAngle);
				}
			}
		}

		public bool Save(string customSaveName = null)
		{
			if (!Save(out MySessionSnapshot snapshot, customSaveName))
			{
				return false;
			}
			bool num = snapshot.Save(null);
			if (num)
			{
				WorldSizeInBytes = snapshot.SavedSizeInBytes;
			}
			return num;
		}

		public bool Save(out MySessionSnapshot snapshot, string customSaveName = null)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnServerSaving, arg2: true);
			}
			snapshot = new MySessionSnapshot();
			MySandboxGame.Log.WriteLine("Saving world - START");
			using (MySandboxGame.Log.IndentUsing())
			{
				string saveName = customSaveName ?? Name;
				if (customSaveName != null)
				{
					if (!Path.IsPathRooted(customSaveName))
					{
						string directoryName = Path.GetDirectoryName(CurrentPath);
						if (Directory.Exists(directoryName))
						{
							CurrentPath = Path.Combine(directoryName, customSaveName);
						}
						else
						{
							CurrentPath = MyLocalCache.GetSessionSavesPath(customSaveName, contentFolder: false);
						}
					}
					else
					{
						CurrentPath = customSaveName;
						saveName = Path.GetFileName(customSaveName);
					}
				}
				snapshot.TargetDir = CurrentPath;
				snapshot.SavingDir = GetTempSavingFolder();
				try
				{
					MySandboxGame.Log.WriteLine("Making world state snapshot.");
					LogMemoryUsage("Before snapshot.");
					snapshot.CheckpointSnapshot = GetCheckpoint(saveName);
					snapshot.SectorSnapshot = GetSector();
					snapshot.CompressedVoxelSnapshots = VoxelMaps.GetVoxelMapsData(includeChanged: true, cached: true);
					Dictionary<string, VRage.Game.Voxels.IMyStorage> voxelStorageNameCache = new Dictionary<string, VRage.Game.Voxels.IMyStorage>();
					snapshot.VoxelSnapshots = VoxelMaps.GetVoxelMapsData(includeChanged: true, cached: false, voxelStorageNameCache);
					snapshot.VoxelStorageNameCache = voxelStorageNameCache;
					LogMemoryUsage("After snapshot.");
					SaveDataComponents();
				}
				catch (Exception ex)
				{
					MySandboxGame.Log.WriteLine(ex);
					return false;
				}
				finally
				{
					SaveEnded();
				}
				LogMemoryUsage("Directory cleanup");
			}
			MySandboxGame.Log.WriteLine("Saving world - END");
			return true;
		}

		public void SaveEnded()
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnServerSaving, arg2: false);
			}
		}

		public void SaveDataComponents()
		{
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				SaveComponent(value);
			}
		}

		private void SaveComponent(MySessionComponentBase component)
		{
			component.SaveData();
		}

		public MyObjectBuilder_World GetWorld(bool includeEntities = true)
		{
			return new MyObjectBuilder_World
			{
				Checkpoint = GetCheckpoint(Name),
				Sector = GetSector(includeEntities),
				VoxelMaps = (includeEntities ? new SerializableDictionary<string, byte[]>(Static.GetVoxelMapsArray(includeChanged: false)) : new SerializableDictionary<string, byte[]>())
			};
		}

		public MyObjectBuilder_Sector GetSector(bool includeEntities = true)
		{
			MyObjectBuilder_Sector myObjectBuilder_Sector = null;
			myObjectBuilder_Sector = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Sector>();
			if (includeEntities)
			{
				myObjectBuilder_Sector.SectorObjects = MyEntities.Save();
			}
			myObjectBuilder_Sector.SectorEvents = MyGlobalEvents.GetObjectBuilder();
			myObjectBuilder_Sector.Environment = MySector.GetEnvironmentSettings();
			myObjectBuilder_Sector.AppVersion = MyFinalBuildConstants.APP_VERSION;
			return myObjectBuilder_Sector;
		}

		public MyObjectBuilder_Checkpoint GetCheckpoint(string saveName)
		{
			MatrixD matrix = MatrixD.Invert(MySpectatorCameraController.Static.GetViewMatrix());
			MyCameraControllerEnum cameraControllerEnum = GetCameraControllerEnum();
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Checkpoint>();
			MyObjectBuilder_SessionSettings myObjectBuilder_SessionSettings = MyObjectBuilderSerializer.Clone(Settings) as MyObjectBuilder_SessionSettings;
			myObjectBuilder_SessionSettings.ScenarioEditMode = (myObjectBuilder_SessionSettings.ScenarioEditMode || PersistentEditMode);
			myObjectBuilder_Checkpoint.SessionName = saveName;
			myObjectBuilder_Checkpoint.Description = Description;
			myObjectBuilder_Checkpoint.PromotedUsers = new SerializableDictionary<ulong, MyPromoteLevel>(PromotedUsers);
			myObjectBuilder_Checkpoint.RemoteAdminSettings.Dictionary.Clear();
			foreach (KeyValuePair<ulong, AdminSettingsEnum> remoteAdminSetting in m_remoteAdminSettings)
			{
				myObjectBuilder_Checkpoint.RemoteAdminSettings[remoteAdminSetting.Key] = (int)remoteAdminSetting.Value;
			}
			myObjectBuilder_Checkpoint.CreativeTools = m_creativeTools;
			myObjectBuilder_Checkpoint.Briefing = Briefing;
			myObjectBuilder_Checkpoint.BriefingVideo = BriefingVideo;
			myObjectBuilder_Checkpoint.Password = Password;
			myObjectBuilder_Checkpoint.LastSaveTime = DateTime.Now;
			myObjectBuilder_Checkpoint.WorkshopId = WorkshopId;
			myObjectBuilder_Checkpoint.ElapsedGameTime = ElapsedGameTime.Ticks;
			myObjectBuilder_Checkpoint.InGameTime = InGameTime;
			myObjectBuilder_Checkpoint.Settings = myObjectBuilder_SessionSettings;
			myObjectBuilder_Checkpoint.Mods = Mods;
			myObjectBuilder_Checkpoint.CharacterToolbar = MyToolbarComponent.CharacterToolbar.GetObjectBuilder();
			myObjectBuilder_Checkpoint.Scenario = Scenario.Id;
			myObjectBuilder_Checkpoint.WorldBoundaries = WorldBoundaries;
			myObjectBuilder_Checkpoint.PreviousEnvironmentHostility = PreviousEnvironmentHostility;
			myObjectBuilder_Checkpoint.RequiresDX = RequiresDX;
			myObjectBuilder_Checkpoint.CustomLoadingScreenImage = CustomLoadingScreenImage;
			myObjectBuilder_Checkpoint.CustomLoadingScreenText = CustomLoadingScreenText;
			myObjectBuilder_Checkpoint.CustomSkybox = CustomSkybox;
			myObjectBuilder_Checkpoint.GameDefinition = GameDefinition.Id;
			myObjectBuilder_Checkpoint.SessionComponentDisabled = SessionComponentDisabled;
			myObjectBuilder_Checkpoint.SessionComponentEnabled = SessionComponentEnabled;
			myObjectBuilder_Checkpoint.SharedToolbar = SharedToolbar;
			Sync.Players.SavePlayers(myObjectBuilder_Checkpoint);
			Toolbars.SaveToolbars(myObjectBuilder_Checkpoint);
			Cameras.SaveCameraCollection(myObjectBuilder_Checkpoint);
			Gpss.SaveGpss(myObjectBuilder_Checkpoint);
			if (MyFakes.ENABLE_MISSION_TRIGGERS)
			{
				myObjectBuilder_Checkpoint.MissionTriggers = MySessionComponentMissionTriggers.Static.GetObjectBuilder();
			}
			if (MyFakes.SHOW_FACTIONS_GUI)
			{
				myObjectBuilder_Checkpoint.Factions = Factions.GetObjectBuilder();
			}
			else
			{
				myObjectBuilder_Checkpoint.Factions = null;
			}
			myObjectBuilder_Checkpoint.Identities = Sync.Players.SaveIdentities();
			myObjectBuilder_Checkpoint.RespawnCooldowns = new List<MyObjectBuilder_Checkpoint.RespawnCooldownItem>();
			Sync.Players.RespawnComponent.SaveToCheckpoint(myObjectBuilder_Checkpoint);
			myObjectBuilder_Checkpoint.ControlledEntities = Sync.Players.SerializeControlledEntities();
			myObjectBuilder_Checkpoint.SpectatorPosition = new MyPositionAndOrientation(ref matrix);
			myObjectBuilder_Checkpoint.SpectatorSpeed = new SerializableVector2(MySpectatorCameraController.Static.SpeedModeLinear, MySpectatorCameraController.Static.SpeedModeAngular);
			myObjectBuilder_Checkpoint.SpectatorIsLightOn = MySpectatorCameraController.Static.IsLightOn;
			myObjectBuilder_Checkpoint.SpectatorDistance = (float)MyThirdPersonSpectator.Static.GetViewerDistance();
			myObjectBuilder_Checkpoint.CameraController = cameraControllerEnum;
			if (cameraControllerEnum == MyCameraControllerEnum.Entity)
			{
				myObjectBuilder_Checkpoint.CameraEntity = ((MyEntity)CameraController).EntityId;
			}
			if (ControlledEntity != null)
			{
				myObjectBuilder_Checkpoint.ControlledObject = ControlledEntity.Entity.EntityId;
				if (!(ControlledEntity is MyCharacter))
				{
				}
			}
			else
			{
				myObjectBuilder_Checkpoint.ControlledObject = -1L;
			}
			myObjectBuilder_Checkpoint.AppVersion = MyFinalBuildConstants.APP_VERSION;
			myObjectBuilder_Checkpoint.Clients = SaveMembers();
			myObjectBuilder_Checkpoint.NonPlayerIdentities = Sync.Players.SaveNpcIdentities();
			SaveSessionComponentObjectBuilders(myObjectBuilder_Checkpoint);
			myObjectBuilder_Checkpoint.ScriptManagerData = ScriptManager.GetObjectBuilder();
			GatherVicinityInformation(myObjectBuilder_Checkpoint);
			if (this.OnSavingCheckpoint != null)
			{
				this.OnSavingCheckpoint(myObjectBuilder_Checkpoint);
			}
			return myObjectBuilder_Checkpoint;
		}

		public static void RequestVicinityCache(long entityId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRequestVicinityInformation, entityId);
		}

		[Event(null, 2947)]
		[Reliable]
		[Server]
		private static void OnRequestVicinityInformation(long entityId)
		{
			SendVicinityInformation(entityId, MyEventContext.Current.Sender);
		}

		public static void SendVicinityInformation(long entityId, EndpointId client)
		{
			MyEntity entityById = MyEntities.GetEntityById(entityId);
			if (entityById != null)
			{
				BoundingSphereD bs = new BoundingSphereD(entityById.PositionComp.WorldMatrix.Translation, MyFakes.PRIORITIZED_CUBE_VICINITY_RADIUS);
				HashSet<string> hashSet = new HashSet<string>();
				HashSet<string> hashSet2 = new HashSet<string>();
				HashSet<string> hashSet3 = new HashSet<string>();
				Static.GatherVicinityInformation(ref bs, hashSet, hashSet2, hashSet3);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnVicinityInformation, hashSet.ToList(), hashSet2.ToList(), hashSet3.ToList(), client);
			}
		}

		[Event(null, 2968)]
		[Reliable]
		[Client]
		private static void OnVicinityInformation(List<string> voxels, List<string> models, List<string> armorModels)
		{
			PreloadVicinityCache(voxels, models, armorModels);
		}

		private static void PreloadVicinityCache(List<string> voxels, List<string> models, List<string> armorModels)
		{
			if (voxels != null && voxels.Count > 0)
			{
				byte[] array = new byte[voxels.Count];
				int num = 0;
				foreach (string voxel in voxels)
				{
					MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(voxel);
					if (voxelMaterialDefinition != null)
					{
						array[num++] = voxelMaterialDefinition.Index;
					}
					else
					{
						array[num++] = 0;
					}
				}
				MyRenderProxy.PreloadVoxelMaterials(array);
			}
			if (models != null && models.Count > 0)
			{
				MyRenderProxy.PreloadModels(models, forInstancedComponent: true);
			}
			if (armorModels != null && armorModels.Count > 0)
			{
				MyRenderProxy.PreloadModels(armorModels, forInstancedComponent: false);
			}
		}

		private void GatherVicinityInformation(MyObjectBuilder_Checkpoint checkpoint)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MyFakes.PRIORITIZED_VICINITY_ASSETS_LOADING)
			{
				if (checkpoint.VicinityArmorModelsCache == null)
				{
					checkpoint.VicinityArmorModelsCache = new List<string>();
				}
				else
				{
					checkpoint.VicinityArmorModelsCache.Clear();
				}
				if (checkpoint.VicinityModelsCache == null)
				{
					checkpoint.VicinityModelsCache = new List<string>();
				}
				else
				{
					checkpoint.VicinityModelsCache.Clear();
				}
				if (checkpoint.VicinityVoxelCache == null)
				{
					checkpoint.VicinityVoxelCache = new List<string>();
				}
				else
				{
					checkpoint.VicinityVoxelCache.Clear();
				}
				if (LocalCharacter != null)
				{
					BoundingSphereD bs = new BoundingSphereD(LocalCharacter.WorldMatrix.Translation, MyFakes.PRIORITIZED_CUBE_VICINITY_RADIUS);
					HashSet<string> hashSet = new HashSet<string>();
					HashSet<string> hashSet2 = new HashSet<string>();
					HashSet<string> hashSet3 = new HashSet<string>();
					GatherVicinityInformation(ref bs, hashSet, hashSet2, hashSet3);
					checkpoint.VicinityArmorModelsCache.AddRange(hashSet3);
					checkpoint.VicinityModelsCache.AddRange(hashSet2);
					checkpoint.VicinityVoxelCache.AddRange(hashSet);
				}
			}
		}

		public void GatherVicinityInformation(ref BoundingSphereD bs, HashSet<string> voxelMaterials, HashSet<string> models, HashSet<string> armorModels)
		{
			List<MyEntity> list = new List<MyEntity>();
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref bs, list);
			foreach (MyEntity item in list)
			{
				MyCubeGrid myCubeGrid = item as MyCubeGrid;
				if (myCubeGrid != null)
				{
					if (myCubeGrid.RenderData != null && myCubeGrid.RenderData.Cells != null)
					{
						foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in myCubeGrid.Render.RenderData.Cells)
						{
							if (cell.Value.CubeParts != null)
							{
								foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cell.Value.CubeParts)
								{
									AddAllModels(cubePart.Key.Model, armorModels);
								}
							}
						}
					}
					foreach (MySlimBlock cubeBlock in myCubeGrid.CubeBlocks)
					{
						if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.Model != null)
						{
							AddAllModels(cubeBlock.FatBlock.Model, models);
						}
					}
				}
				else
				{
					MyVoxelBase myVoxelBase = item as MyVoxelBase;
					if (myVoxelBase != null && !(myVoxelBase is MyVoxelPhysics))
					{
						GetVoxelMaterials(voxelMaterials, myVoxelBase, 7, bs.Center, (float)MyFakes.PRIORITIZED_VOXEL_VICINITY_RADIUS_FAR);
						GetVoxelMaterials(voxelMaterials, myVoxelBase, 1, bs.Center, (float)MyFakes.PRIORITIZED_VOXEL_VICINITY_RADIUS_CLOSE);
					}
				}
			}
		}

		private void GetVoxelMaterials(HashSet<string> voxelMaterials, MyVoxelBase voxel, int lod, Vector3D center, float radius)
		{
			MyShapeSphere shape = new MyShapeSphere
			{
				Center = center,
				Radius = radius
			};
			foreach (byte item in voxel.GetMaterialsInShape(shape, lod))
			{
				MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(item);
				if (voxelMaterialDefinition != null)
				{
					voxelMaterials.Add(voxelMaterialDefinition.Id.SubtypeName);
				}
			}
		}

		private void AddAllModels(MyModel model, HashSet<string> models)
		{
			if (!string.IsNullOrEmpty(model.AssetName))
			{
				models.Add(model.AssetName);
			}
		}

		private void SaveSessionComponentObjectBuilders(MyObjectBuilder_Checkpoint checkpoint)
		{
			checkpoint.SessionComponents = new List<MyObjectBuilder_SessionComponent>();
			foreach (MySessionComponentBase value in m_sessionComponents.Values)
			{
				MyObjectBuilder_SessionComponent objectBuilder = value.GetObjectBuilder();
				if (objectBuilder != null)
				{
					checkpoint.SessionComponents.Add(objectBuilder);
				}
			}
		}

		private string GetTempSavingFolder()
		{
			return Path.Combine(CurrentPath, ".new");
		}

		public Dictionary<string, byte[]> GetVoxelMapsArray(bool includeChanged)
		{
			return VoxelMaps.GetVoxelMapsArray(includeChanged);
		}

		public List<MyObjectBuilder_Planet> GetPlanetObjectBuilders()
		{
			List<MyObjectBuilder_Planet> list = new List<MyObjectBuilder_Planet>();
			foreach (MyVoxelBase instance in VoxelMaps.Instances)
			{
				MyPlanet myPlanet = instance as MyPlanet;
				if (myPlanet != null)
				{
					list.Add(myPlanet.GetObjectBuilder() as MyObjectBuilder_Planet);
				}
			}
			return list;
		}

		internal List<MyObjectBuilder_Client> SaveMembers(bool forceSave = false)
		{
			if (MyMultiplayer.Static == null)
			{
				return null;
			}
			if (!forceSave && MyMultiplayer.Static.Members.Count() == 1)
			{
				using (IEnumerator<ulong> enumerator = MyMultiplayer.Static.Members.GetEnumerator())
				{
					enumerator.MoveNext();
					if (enumerator.Current == Sync.MyId)
					{
						return null;
					}
				}
			}
			List<MyObjectBuilder_Client> list = new List<MyObjectBuilder_Client>();
			foreach (ulong member in MyMultiplayer.Static.Members)
			{
				MyObjectBuilder_Client myObjectBuilder_Client = new MyObjectBuilder_Client();
				myObjectBuilder_Client.SteamId = member;
				myObjectBuilder_Client.Name = MyMultiplayer.Static.GetMemberName(member);
				myObjectBuilder_Client.IsAdmin = Static.IsUserAdmin(member);
				list.Add(myObjectBuilder_Client);
			}
			return list;
		}

		public void GameOver()
		{
			GameOver(MyCommonTexts.MP_YouHaveBeenKilled);
		}

		public void GameOver(MyStringId? customMessage)
		{
		}

		public void Unload()
		{
			if (MySession.OnUnloading != null)
			{
				MySession.OnUnloading();
			}
			Parallel.Scheduler.WaitForTasksToFinish(new TimeSpan(-1L));
			Parallel.RunCallbacks();
			MySandboxGame.IsPaused = false;
			if (MyHud.RotatingWheelVisible)
			{
				MyHud.PopRotatingWheelVisible();
			}
			Sandbox.Engine.Platform.Game.EnableSimSpeedLocking = false;
			MySpectatorCameraController.Static.CleanLight();
			if (MySpaceAnalytics.Instance != null)
			{
				MySpaceAnalytics.Instance.ReportGameplayEnd();
			}
			MySandboxGame.Log.WriteLine("MySession::Unload START");
			MySessionSnapshot.WaitForSaving();
			MySandboxGame.Log.WriteLine("AutoSaveInMinutes: " + AutoSaveInMinutes);
			MySandboxGame.Log.WriteLine("MySandboxGame.IsDedicated: " + Sandbox.Engine.Platform.Game.IsDedicated);
			MySandboxGame.Log.WriteLine("IsServer: " + Sync.IsServer);
			if ((SaveOnUnloadOverride.HasValue && SaveOnUnloadOverride.Value) || (!SaveOnUnloadOverride.HasValue && AutoSaveInMinutes != 0 && Sandbox.Engine.Platform.Game.IsDedicated))
			{
				MySandboxGame.Log.WriteLineAndConsole("Autosave in unload");
				IsUnloadSaveInProgress = true;
				Save();
				IsUnloadSaveInProgress = false;
			}
			MySandboxGame.Static.ClearInvokeQueue();
			MyDroneAIDataStatic.Reset();
			MyAudio.Static.StopUpdatingAll3DCues();
			MyAudio.Static.Mute = true;
			MyAudio.Static.StopMusic();
			MyAudio.Static.ChangeGlobalVolume(1f, 0f);
			MyAudio.ReloadData(MyAudioExtensions.GetSoundDataFromDefinitions(), MyAudioExtensions.GetEffectData());
			MyEntity3DSoundEmitter.LastTimePlaying.Clear();
			MyParticlesLibrary.Close();
			Ready = false;
			VoxelMaps.Clear();
			MySandboxGame.Config.Save();
			if (LocalHumanPlayer != null && LocalHumanPlayer.Controller != null)
			{
				LocalHumanPlayer.Controller.SaveCamera();
			}
			DisconnectMultiplayer();
			UnloadDataComponents();
			UnloadMultiplayer();
			MyTerminalControlFactory.Unload();
			MyDefinitionManager.Static.UnloadData();
			MyDefinitionManager.Static.PreloadDefinitions();
			MyInput.Static.ClearBlacklist();
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				(EmptyKeys.UserInterface.Engine.Instance.AssetManager as MyAssetManager)?.UnloadGeneratedTextures();
			}
			MyDefinitionErrors.Clear();
			MyRenderProxy.UnloadData();
			MyHud.Questlog.CleanDetails();
			MyHud.Questlog.Visible = false;
			MyAPIGateway.Clean();
			MyOxygenProviderSystem.ClearOxygenGenerators();
			MyDynamicAABBTree.Dispose();
			MyDynamicAABBTreeD.Dispose();
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
			MySandboxGame.Log.WriteLine("MySession::Unload END");
			if (MyCubeBuilder.AllPlayersColors != null)
			{
				MyCubeBuilder.AllPlayersColors.Clear();
			}
			if (MySession.OnUnloaded != null)
			{
				MySession.OnUnloaded();
			}
			Parallel.Scheduler.WaitForTasksToFinish(new TimeSpan(-1L));
			Parallel.Clean();
		}

		private void InitializeFactions()
		{
			Factions.CreateDefaultFactions();
		}

		public static void InitiateDump()
		{
			VRage.Profiler.MyRenderProfiler.SetLevel(-1);
			m_profilerDumpDelay = 60;
		}

		private static ulong GetVoxelsSizeInBytes(string sessionPath)
		{
			ulong num = 0uL;
			string[] files = Directory.GetFiles(sessionPath, "*.vx2", SearchOption.TopDirectoryOnly);
			for (int i = 0; i < files.Length; i++)
			{
				using (Stream stream = MyFileSystem.OpenRead(files[i]))
				{
					num = (ulong)((long)num + stream.Length);
				}
			}
			return num;
		}

		private void LogMemoryUsage(string msg)
		{
			MySandboxGame.Log.WriteMemoryUsage(msg);
		}

		private void LogSettings(string scenario = null, int asteroidAmount = 0)
		{
			MyLog log = MySandboxGame.Log;
			log.WriteLine("MySession.Static.LogSettings - START", LoggingOptions.SESSION_SETTINGS);
			using (log.IndentUsing(LoggingOptions.SESSION_SETTINGS))
			{
				log.WriteLine("Name = " + Name, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("Description = " + Description, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("GameDateTime = " + GameDateTime, LoggingOptions.SESSION_SETTINGS);
				if (scenario != null)
				{
					log.WriteLine("Scenario = " + scenario, LoggingOptions.SESSION_SETTINGS);
					log.WriteLine("AsteroidAmount = " + asteroidAmount, LoggingOptions.SESSION_SETTINGS);
				}
				log.WriteLine("Password = " + Password, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("CurrentPath = " + CurrentPath, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("WorkshopId = " + WorkshopId, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("CameraController = " + CameraController, LoggingOptions.SESSION_SETTINGS);
				log.WriteLine("ThumbPath = " + ThumbPath, LoggingOptions.SESSION_SETTINGS);
				Settings.LogMembers(log, LoggingOptions.SESSION_SETTINGS);
			}
			log.WriteLine("MySession.Static.LogSettings - END", LoggingOptions.SESSION_SETTINGS);
		}

		public bool GetVoxelHandAvailable(MyCharacter character)
		{
			MyPlayer playerFromCharacter = MyPlayer.GetPlayerFromCharacter(character);
			if (playerFromCharacter == null)
			{
				return false;
			}
			return GetVoxelHandAvailable(playerFromCharacter.Client.SteamUserId);
		}

		public bool GetVoxelHandAvailable(ulong user)
		{
			if (Settings.EnableVoxelHand)
			{
				if (SurvivalMode)
				{
					return CreativeToolsEnabled(user);
				}
				return true;
			}
			return false;
		}

		private void OnFactionsStateChanged(MyFactionStateChange change, long fromFactionId, long toFactionId, long playerId, long sender)
		{
			string text = null;
			if (change == MyFactionStateChange.FactionMemberKick && sender != playerId && LocalPlayerId == playerId)
			{
				text = MyTexts.GetString(MyCommonTexts.MessageBoxTextYouHaveBeenKickedFromFaction);
			}
			else if (change == MyFactionStateChange.FactionMemberAcceptJoin && sender != playerId && LocalPlayerId == playerId)
			{
				text = MyTexts.GetString(MyCommonTexts.MessageBoxTextYouHaveBeenAcceptedToFaction);
			}
			else if (change == MyFactionStateChange.FactionMemberNotPossibleJoin && sender != playerId && LocalPlayerId == playerId)
			{
				text = MyTexts.GetString(MyCommonTexts.MessageBoxTextYouCannotJoinToFaction);
			}
			else if (change == MyFactionStateChange.FactionMemberNotPossibleJoin && LocalPlayerId == sender)
			{
				text = MyTexts.GetString(MyCommonTexts.MessageBoxTextApplicantCannotJoinToFaction);
			}
			else if (change == MyFactionStateChange.FactionMemberAcceptJoin && (Static.Factions[toFactionId].IsFounder(LocalPlayerId) || Static.Factions[toFactionId].IsLeader(LocalPlayerId)) && playerId != 0L)
			{
				MyIdentity myIdentity = Sync.Players.TryGetIdentity(playerId);
				if (myIdentity != null)
				{
					string displayName = myIdentity.DisplayName;
					text = string.Format(MyTexts.GetString(MyCommonTexts.Faction_PlayerJoined), displayName);
				}
			}
			else if (change == MyFactionStateChange.FactionMemberLeave && (Static.Factions[toFactionId].IsFounder(LocalPlayerId) || Static.Factions[toFactionId].IsLeader(LocalPlayerId)) && playerId != 0L)
			{
				MyIdentity myIdentity2 = Sync.Players.TryGetIdentity(playerId);
				if (myIdentity2 != null)
				{
					string displayName2 = myIdentity2.DisplayName;
					text = string.Format(MyTexts.GetString(MyCommonTexts.Faction_PlayerLeft), displayName2);
				}
			}
			else if (change == MyFactionStateChange.FactionMemberSendJoin && (Static.Factions[toFactionId].IsFounder(LocalPlayerId) || Static.Factions[toFactionId].IsLeader(LocalPlayerId)) && playerId != 0L)
			{
				MyIdentity myIdentity3 = Sync.Players.TryGetIdentity(playerId);
				if (myIdentity3 != null)
				{
					string displayName3 = myIdentity3.DisplayName;
					text = string.Format(MyTexts.GetString(MyCommonTexts.Faction_PlayerApplied), displayName3);
				}
			}
			if (text != null)
			{
				MyHud.Chat.ShowMessage(MyTexts.GetString(MySpaceTexts.ChatBotName), text);
			}
		}

		private static IMyCamera GetMainCamera()
		{
			return MySector.MainCamera;
		}

		private static BoundingBoxD GetWorldBoundaries()
		{
			if (Static == null || !Static.WorldBoundaries.HasValue)
			{
				return default(BoundingBoxD);
			}
			return Static.WorldBoundaries.Value;
		}

		private static Vector3D GetLocalPlayerPosition()
		{
			if (Static != null && Static.LocalHumanPlayer != null)
			{
				return Static.LocalHumanPlayer.GetPosition();
			}
			return default(Vector3D);
		}

		public short GetBlockTypeLimit(string blockType)
		{
			int num = 1;
			switch (BlockLimitsEnabled)
			{
			case MyBlockLimitsEnabledEnum.NONE:
				return 0;
			case MyBlockLimitsEnabledEnum.GLOBALLY:
				num = 1;
				break;
			case MyBlockLimitsEnabledEnum.PER_PLAYER:
				num = 1;
				break;
			case MyBlockLimitsEnabledEnum.PER_FACTION:
				num = ((MaxFactionsCount != 0) ? 1 : 1);
				break;
			}
			if (!BlockTypeLimits.TryGetValue(blockType, out short value))
			{
				return 0;
			}
			if (value > 0 && value / num == 0)
			{
				return 1;
			}
			return (short)(value / num);
		}

		private static void RaiseAfterLoading()
		{
			MySession.AfterLoading?.Invoke();
		}

		public LimitResult IsWithinWorldLimits(out string failedBlockType, long ownerID, string blockName, int pcuToBuild, int blocksToBuild = 0, int blocksCount = 0, Dictionary<string, int> blocksPerType = null)
		{
			failedBlockType = null;
			if (BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
			{
				return LimitResult.Passed;
			}
			ulong num = Players.TryGetSteamId(ownerID);
			if (num != 0L && Static.IsUserAdmin(num))
			{
				AdminSettingsEnum? adminSettingsEnum = null;
				if (num == Sync.MyId)
				{
					adminSettingsEnum = Static.AdminSettings;
				}
				else if (Static.RemoteAdminSettings.ContainsKey(num))
				{
					adminSettingsEnum = Static.RemoteAdminSettings[num];
				}
				if (((int?)adminSettingsEnum & 0x40) != 0)
				{
					return LimitResult.Passed;
				}
			}
			MyIdentity myIdentity = Players.TryGetIdentity(ownerID);
			if (MaxGridSize != 0 && blocksCount + blocksToBuild > MaxGridSize)
			{
				return LimitResult.MaxGridSize;
			}
			if (myIdentity != null)
			{
				MyBlockLimits blockLimits = myIdentity.BlockLimits;
				if (BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && Factions.GetPlayerFaction(myIdentity.IdentityId) == null)
				{
					return LimitResult.NoFaction;
				}
				if (blockLimits != null)
				{
					if (MaxBlocksPerPlayer != 0 && blockLimits.BlocksBuilt + blocksToBuild > blockLimits.MaxBlocks)
					{
						return LimitResult.MaxBlocksPerPlayer;
					}
					if (TotalPCU != 0 && pcuToBuild > blockLimits.PCU)
					{
						return LimitResult.PCU;
					}
					if (blocksPerType != null)
					{
						foreach (KeyValuePair<string, short> blockTypeLimit2 in BlockTypeLimits)
						{
							if (blocksPerType.ContainsKey(blockTypeLimit2.Key))
							{
								int num2 = blocksPerType[blockTypeLimit2.Key];
								if (blockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit2.Key, out MyBlockLimits.MyTypeLimitData value))
								{
									num2 += value.BlocksBuilt;
								}
								if (num2 > GetBlockTypeLimit(blockTypeLimit2.Key))
								{
									return LimitResult.BlockTypeLimit;
								}
							}
						}
					}
					else
					{
						short blockTypeLimit = GetBlockTypeLimit(blockName);
						if (blockTypeLimit > 0)
						{
							if (blockLimits.BlockTypeBuilt.TryGetValue(blockName, out MyBlockLimits.MyTypeLimitData value2))
							{
								blocksToBuild += value2.BlocksBuilt;
							}
							if (blocksToBuild > blockTypeLimit)
							{
								return LimitResult.BlockTypeLimit;
							}
						}
					}
				}
			}
			return LimitResult.Passed;
		}

		public bool CheckLimitsAndNotify(long ownerID, string blockName, int pcuToBuild, int blocksToBuild = 0, int blocksCount = 0, Dictionary<string, int> blocksPerType = null)
		{
			string failedBlockType;
			LimitResult limitResult = IsWithinWorldLimits(out failedBlockType, ownerID, blockName, pcuToBuild, blocksToBuild, blocksCount, blocksPerType);
			if (limitResult != 0)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUnable);
				MyHud.Notifications.Add(GetNotificationForLimitResult(limitResult));
				return false;
			}
			return true;
		}

		public static MyNotificationSingletons GetNotificationForLimitResult(LimitResult result)
		{
			switch (result)
			{
			case LimitResult.MaxGridSize:
				return MyNotificationSingletons.LimitsGridSize;
			case LimitResult.NoFaction:
				return MyNotificationSingletons.LimitsNoFaction;
			case LimitResult.BlockTypeLimit:
				return MyNotificationSingletons.LimitsPerBlockType;
			case LimitResult.MaxBlocksPerPlayer:
				return MyNotificationSingletons.LimitsPlayer;
			case LimitResult.PCU:
				return MyNotificationSingletons.LimitsPCU;
			default:
				return MyNotificationSingletons.LimitsPCU;
			}
		}

		public bool CheckResearchAndNotify(long identityId, MyDefinitionId id)
		{
			if (Static.Settings.EnableResearch && !MySessionComponentResearch.Static.CanUse(identityId, id) && !Static.CreativeMode && !Static.CreativeToolsEnabled(Static.Players.TryGetSteamId(identityId)))
			{
				if (Static.LocalCharacter != null && identityId == Static.LocalCharacter.GetPlayerIdentityId())
				{
					MyHud.Notifications.Add(MyNotificationSingletons.BlockNotResearched);
				}
				return false;
			}
			return true;
		}

		public bool CheckDLCAndNotify(MyDefinitionBase definition)
		{
			MyHudNotificationBase myHudNotificationBase = MyHud.Notifications.Get(MyNotificationSingletons.MissingDLC);
			MyDLCs.MyDLC firstMissingDefinitionDLC = GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
			if (firstMissingDefinitionDLC == null)
			{
				return true;
			}
			myHudNotificationBase.SetTextFormatArguments(MyTexts.Get(firstMissingDefinitionDLC.DisplayName));
			MyHud.Notifications.Add(myHudNotificationBase);
			return false;
		}

		private void OnCameraEntityClosing(MyEntity entity)
		{
			SetCameraController(MyCameraControllerEnum.Spectator);
		}

		void IMySession.BeforeStartComponents()
		{
			BeforeStartComponents();
		}

		void IMySession.Draw()
		{
			Draw();
		}

		void IMySession.GameOver()
		{
			GameOver();
		}

		void IMySession.GameOver(MyStringId? customMessage)
		{
			GameOver(customMessage);
		}

		MyObjectBuilder_Checkpoint IMySession.GetCheckpoint(string saveName)
		{
			return GetCheckpoint(saveName);
		}

		MyObjectBuilder_Sector IMySession.GetSector()
		{
			return GetSector();
		}

		Dictionary<string, byte[]> IMySession.GetVoxelMapsArray()
		{
			return GetVoxelMapsArray(includeChanged: true);
		}

		MyObjectBuilder_World IMySession.GetWorld()
		{
			return GetWorld();
		}

		bool IMySession.IsPausable()
		{
			return IsPausable();
		}

		void IMySession.RegisterComponent(MySessionComponentBase component, MyUpdateOrder updateOrder, int priority)
		{
			RegisterComponent(component, updateOrder, priority);
		}

		bool IMySession.Save(string customSaveName)
		{
			return Save(customSaveName);
		}

		void IMySession.SetAsNotReady()
		{
			SetAsNotReady();
		}

		void IMySession.SetCameraController(MyCameraControllerEnum cameraControllerEnum, IMyEntity cameraEntity, Vector3D? position)
		{
			SetCameraController(cameraControllerEnum, cameraEntity, position);
		}

		void IMySession.Unload()
		{
			Unload();
		}

		void IMySession.UnloadDataComponents()
		{
			UnloadDataComponents();
		}

		void IMySession.UnloadMultiplayer()
		{
			UnloadMultiplayer();
		}

		void IMySession.UnregisterComponent(MySessionComponentBase component)
		{
			UnregisterComponent(component);
		}

		void IMySession.Update(MyTimeSpan time)
		{
			Update(time);
		}

		void IMySession.UpdateComponents()
		{
			UpdateComponents();
		}

		bool IMySession.IsUserAdmin(ulong steamId)
		{
			return Static.IsUserAdmin(steamId);
		}

		[Obsolete("Use GetUserPromoteLevel")]
		bool IMySession.IsUserPromoted(ulong steamId)
		{
			return Static.IsUserSpaceMaster(steamId);
		}

		MyPromoteLevel IMySession.GetUserPromoteLevel(ulong steamId)
		{
			return GetUserPromoteLevel(steamId);
		}
	}
}
