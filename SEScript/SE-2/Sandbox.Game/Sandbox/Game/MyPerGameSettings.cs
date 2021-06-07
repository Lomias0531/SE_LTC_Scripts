using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRage.Voxels.DualContouring;
using VRageMath;
using VRageRender;

namespace Sandbox.Game
{
	public static class MyPerGameSettings
	{
		public static MyBasicGameInfo BasicGameInfo = default(MyBasicGameInfo);

		public static GameEnum Game = GameEnum.UNKNOWN_GAME;

		public static string GameWebUrl = "www.SpaceEngineersGame.com";

		public static string DeluxeEditionUrl = "https://store.steampowered.com/app/573160/Space_Engineers_Deluxe/";

		public static string SkinSaleUrl = "http://store.steampowered.com/itemstore/244850/browse/?filter=all";

		public static uint DeluxeEditionDlcId = 573160u;

		public static string LocalizationWebUrl = "http://www.spaceengineersgame.com/localization.html";

		public static string ChangeLogUrl = "https://mirror.keenswh.com/news/SpaceEngineersChangelog.xml";

		public static string EventsUrl = "https://mirror.keenswh.com/news/SpaceEngineersActiveEvents.txt";

		public static string ChangeLogUrlDevelop = "https://mirror.keenswh.com/news/SpaceEngineersChangelogDevelop.xml";

		public static string RankedServersUrl = "https://mirror.keenswh.com/xml/rankedServers.xml";

		public static string EShopUrl = "https://shop.keenswh.com/";

		public static bool RequiresDX11 = false;

		public static string GameIcon;

		public static bool EnableGlobalGravity;

		public static bool ZoomRequiresLookAroundPressed = true;

		public static bool EnablePregeneratedAsteroidHack = false;

		public static bool SendLogToKeen = false;

		public static string GA_Public_GameKey = string.Empty;

		public static string GA_Public_SecretKey = string.Empty;

		public static string GA_Dev_GameKey = string.Empty;

		public static string GA_Dev_SecretKey = string.Empty;

		public static string GA_Pirate_GameKey = string.Empty;

		public static string GA_Pirate_SecretKey = string.Empty;

		public static string GA_Other_GameKey = string.Empty;

		public static string GA_Other_SecretKey = string.Empty;

		public static string GameModAssembly;

		public static string GameModObjBuildersAssembly;

		public static string GameModBaseObjBuildersAssembly;

		public static string SandboxAssembly = "Sandbox.Common.dll";

		public static string SandboxGameAssembly = "Sandbox.Game.dll";

		public static bool OffsetVoxelMapByHalfVoxel = false;

		public static bool UseVolumeLimiter = false;

		public static bool UseMusicController = false;

		public static bool UseReverbEffect = false;

		public static bool UseSameSoundLimiter = false;

		public static bool UseNewDamageEffects = false;

		public static bool RestrictSpectatorFlyMode = false;

		public static float MaxFrameRate = 120f;

		private static Type m_isoMesherType = typeof(MyDualContouringMesher);

		public static double MinimumLargeShipCollidableMass = 1000.0;

		public static float? ConstantVoxelAmbient;

		private const float DefaultMaxWalkSpeed = 6f;

		private const float DefaultMaxCrouchWalkSpeed = 4f;

		public static MyCharacterMovementSettings CharacterMovement = new MyCharacterMovementSettings
		{
			WalkAcceleration = 50f,
			WalkDecceleration = 10f,
			SprintAcceleration = 100f,
			SprintDecceleration = 20f
		};

		public static MyCollisionParticleSettings CollisionParticle = new MyCollisionParticleSettings
		{
			LargeGridClose = "Collision_Sparks_LargeGrid_Close",
			LargeGridDistant = "Collision_Sparks_LargeGrid_Distant",
			SmallGridClose = "Collision_Sparks",
			SmallGridDistant = "Collision_Sparks",
			CloseDistanceSq = 400f,
			Scale = 1f
		};

		public static MyDestructionParticleSettings DestructionParticle = new MyDestructionParticleSettings
		{
			DestructionSmokeLarge = "Dummy",
			DestructionHit = "Dummy",
			CloseDistanceSq = 400f,
			Scale = 1f
		};

		public static bool UseGridSegmenter = true;

		public static bool Destruction = false;

		public static float PhysicsConvexRadius = 0.05f;

		public static bool PhysicsNoCollisionLayerWithDefault = false;

		public static float DefaultLinearDamping = 0f;

		public static float DefaultAngularDamping = 0.1f;

		public static bool BallFriendlyPhysics = false;

		public static bool DoubleKinematicForLargeGrids = true;

		public static bool CharacterStartsOnVoxel = false;

		public static bool LimitedWorld = false;

		public static bool EnableCollisionSparksEffect = true;

		private static bool m_useAnimationInsteadOfIK = false;

		public static bool WorkshopUseUGCEnumerate = true;

		public static string SteamGameServerGameDir = "Space Engineers";

		public static string SteamGameServerProductName = "Space Engineers";

		public static string SteamGameServerDescription = "Space Engineers";

		public static bool TerminalEnabled = true;

		public static MyGUISettings GUI = new MyGUISettings
		{
			EnableTerminalScreen = true,
			EnableToolbarConfigScreen = true,
			MultipleSpinningWheels = true,
			LoadingScreenIndexRange = new Vector2I(1, 16),
			HUDScreen = typeof(MyGuiScreenHudSpace),
			ToolbarConfigScreen = typeof(MyGuiScreenCubeBuilder),
			ToolbarControl = typeof(MyGuiControlToolbar),
			CustomWorldScreen = typeof(MyGuiScreenWorldSettings),
			ScenarioScreen = typeof(MyGuiScreenScenario),
			EditWorldSettingsScreen = typeof(MyGuiScreenWorldSettings),
			HelpScreen = typeof(MyGuiScreenHelpSpace),
			VoxelMapEditingScreen = typeof(MyGuiScreenDebugSpawnMenu),
			ScenarioLobbyClientScreen = typeof(MyGuiScreenScenarioMpClient),
			AdminMenuScreen = typeof(MyGuiScreenAdminMenu),
			CreateFactionScreen = typeof(MyGuiScreenCreateOrEditFaction),
			PlayersScreen = typeof(MyGuiScreenPlayers)
		};

		public static Type PathfindingType = null;

		public static Type BotFactoryType = null;

		public static bool EnableAi = false;

		public static bool EnablePathfinding = false;

		public static bool NavmeshPresumesDownwardGravity = false;

		public static Type ControlMenuInitializerType = null;

		public static Type CompatHelperType = typeof(MySessionCompatHelper);

		public static MyCredits Credits = new MyCredits();

		public static MyMusicTrack? MainMenuTrack = null;

		public static bool EnableObjectExport = true;

		public static bool TryConvertGridToDynamicAfterSplit = false;

		public static bool AnimateOnlyVisibleCharacters = false;

		public static float CharacterDamageMinVelocity = 12f;

		public static float CharacterDamageDeadlyDamageVelocity = 16f;

		public static float CharacterDamageMediumDamageVelocity = 13f;

		public static float CharacterDamageHitObjectMinMass = 10f;

		public static float CharacterDamageHitObjectMinVelocity = 8.5f;

		public static float CharacterDamageHitObjectMediumEnergy = 100f;

		public static float CharacterDamageHitObjectSmallEnergy = 80f;

		public static float CharacterDamageHitObjectCriticalEnergy = 200f;

		public static float CharacterDamageHitObjectDeadlyEnergy = 500f;

		public static float CharacterSmallDamage = 10f;

		public static float CharacterMediumDamage = 30f;

		public static float CharacterCriticalDamage = 70f;

		public static float CharacterDeadlyDamage = 100f;

		public static float CharacterSqueezeDamageDelay = 1f;

		public static float CharacterSqueezeMinMass = 200f;

		public static float CharacterSqueezeMediumDamageMass = 1000f;

		public static float CharacterSqueezeCriticalDamageMass = 3000f;

		public static float CharacterSqueezeDeadlyDamageMass = 5000f;

		public static bool CharacterSuicideEnabled = true;

		public static Func<bool> ConstrainInventory = () => MySession.Static.SurvivalMode;

		public static bool SwitchToSpectatorCameraAfterDeath = false;

		public static bool SimplePlayerNames = false;

		public static Type CharacterDetectionComponent;

		public static string BugReportUrl = "http://forum.keenswh.com/forums/bug-reports.326950";

		public static bool EnableScenarios = false;

		public static bool EnableRagdollModels = true;

		public static bool ShowObfuscationStatus = true;

		public static bool EnableRagdollInJetpack = false;

		public static bool InventoryMass = false;

		public static bool EnableCharacterCollisionDamage = false;

		public static MyStringId DefaultGraphicsRenderer;

		public static bool EnableWelderAutoswitch = false;

		public static Type VoiceChatLogic = null;

		public static bool VoiceChatEnabled = false;

		public static bool EnableMutePlayer = false;

		public static bool EnableJumpDrive = false;

		public static bool EnableShipSoundSystem = false;

		public static bool EnableFloatingObjectsActiveSync = false;

		public static bool DisableAnimationsOnDS = true;

		public static float CharacterGravityMultiplier = 1f;

		public static bool BlockForVoxels = false;

		public static bool AlwaysShowAvailableBlocksOnHud = false;

		public static float MaxAntennaDrawDistance = 500000f;

		public static bool EnableResearch = true;

		public static MyRenderDeviceSettings? DefaultRenderDeviceSettings;

		public static MyRelationsBetweenFactions DefaultFactionRelationship = MyRelationsBetweenFactions.Enemies;

		private static int m_defaultFactionReputation = -1000;

		public static bool MultiplayerEnabled = true;

		public static Type ClientStateType = typeof(MyClientState);

		public static RigidBodyFlag LargeGridRBFlag = MyFakes.ENABLE_DOUBLED_KINEMATIC ? RigidBodyFlag.RBF_DOUBLED_KINEMATIC : RigidBodyFlag.RBF_DEFAULT;

		public static readonly string MotDServerNameVariable = "{server_name}";

		public static readonly string MotDWorldNameVariable = "{world_name}";

		public static readonly string MotDServerDescriptionVariable = "{server_description}";

		public static readonly string MotDPlayerCountVariable = "{player_count}";

		public static readonly string MotDCurrentPlayerVariable = "{current_player}";

		public static string GameName => BasicGameInfo.GameName;

		public static string GameNameSafe => BasicGameInfo.GameNameSafe;

		public static string MinimumRequirementsPage => BasicGameInfo.MinimumRequirementsWeb;

		public static Type IsoMesherType
		{
			get
			{
				return m_isoMesherType;
			}
			set
			{
				m_isoMesherType = value;
			}
		}

		public static int DefaultFactionReputation
		{
			get
			{
				if (MySession.Static == null)
				{
					return m_defaultFactionReputation;
				}
				return MySession.Static.GetComponent<MySessionComponentEconomy>()?.GetDefaultReputationForRelation(DefaultFactionRelationship) ?? m_defaultFactionReputation;
			}
		}

		public static Tuple<MyRelationsBetweenFactions, int> DefaultFactionRelationshipAndReputation => new Tuple<MyRelationsBetweenFactions, int>(DefaultFactionRelationship, DefaultFactionReputation);
	}
}
