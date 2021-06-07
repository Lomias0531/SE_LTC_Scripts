#define VRAGE
using EmptyKeys.UserInterface;
using Havok;
using ObjectBuilders.SafeZone;
using ParallelTasks;
using ProtoBuf;
using Sandbox.AppCode;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.Audio;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Cube.CubeBuilder;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Contracts;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using Sandbox.ModAPI.Weapons;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.GUI;
using VRage.Game.GUI.TextPanel;
using VRage.Game.Localization;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.News;
using VRage.Game.ObjectBuilder;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Game.SessionComponents;
using VRage.Game.VisualScripting;
using VRage.GameServices;
using VRage.Http;
using VRage.Input;
using VRage.Library;
using VRage.Library.Threading;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Scripting;
using VRage.Serialization;
using VRage.UserInterface.Media;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;
using VRageRender;
using VRageRender.ExternalApp;
using VRageRender.Lights;
using VRageRender.Messages;
using VRageRender.Utils;

namespace Sandbox
{
	public class MySandboxGame : Sandbox.Engine.Platform.Game, IDisposable
	{
		private struct MyInvokeData
		{
			public Action Action;

			public object Context;

			public Action<object> ContextualAction;

			public string Invoker;
		}

		public interface IGameCustomInitialization
		{
			void InitIlChecker();

			void InitIlCompiler();
		}

		private const int MilisToMin = 60000;

		public static readonly MyStringId DirectX11RendererKey = MyStringId.GetOrCompute("DirectX 11");

		public const string CONSOLE_OUTPUT_AUTORESTART = "AUTORESTART";

		public static Version BuildVersion = Assembly.GetExecutingAssembly().GetName().Version;

		public static DateTime BuildDateTime = new DateTime(2000, 1, 1).AddDays(BuildVersion.Build).AddSeconds(BuildVersion.Revision * 2);

		public static MySandboxGame Static;

		public static Vector2I ScreenSize;

		public static Vector2I ScreenSizeHalf;

		public static MyViewport ScreenViewport;

		private bool m_isPendingLobbyInvite;

		private IMyLobby m_invitingLobby;

		private static ParallelTasks.Task? m_currentPreloadingTask;

		private static bool m_reconfirmClipmaps = false;

		private static bool m_areClipmapsReady = true;

		private static bool m_renderTasksFinished = false;

		public static bool IsUpdateReady = true;

		private static EnumAutorestartStage m_autoRestartState = EnumAutorestartStage.NoRestart;

		private int m_lastRestartCheckInMilis;

		private DateTime m_lastUpdateCheckTime = DateTime.UtcNow;

		private int m_autoUpdateRestartTimeInMin = int.MaxValue;

		private bool m_isGoingToUpdate;

		public static bool IsRenderUpdateSyncEnabled = false;

		public static bool IsVideoRecordingEnabled = false;

		public static bool IsConsoleVisible = false;

		public static bool IsReloading = false;

		public static bool FatalErrorDuringInit = false;

		protected static ManualResetEvent m_windowCreatedEvent = new ManualResetEvent(initialState: false);

		public static MyLog Log = new MyLog(alwaysFlush: true);

		public static Action PerformNotInteractiveReport = null;

		private bool hasFocus = true;

		private static int m_pauseStartTimeInMilliseconds;

		private static int m_totalPauseTimeInMilliseconds = 0;

		private static long m_lastFrameTimeStamp = 0L;

		public static int NumberOfCores;

		public static uint CPUFrequency;

		public static bool InsufficientHardware;

		private bool m_dataLoadedDebug;

		private ulong? m_joinLobbyId;

		public static bool ShowIsBetterGCAvailableNotification = false;

		public static bool ShowGpuUnderMinimumNotification = false;

		private MyConcurrentQueue<MyInvokeData> m_invokeQueue = new MyConcurrentQueue<MyInvokeData>(32);

		private MyConcurrentQueue<MyInvokeData> m_invokeQueueExecuting = new MyConcurrentQueue<MyInvokeData>(32);

		public MyGameRenderComponent GameRenderComponent;

		public MySessionCompatHelper SessionCompatHelper;

		public static MyConfig Config;

		public static IMyConfigDedicated ConfigDedicated;

		private bool m_enableDamageEffects = true;

		private bool m_unpauseInput;

		private DateTime m_inputPauseTime;

		private const int INPUT_UNPAUSE_DELAY = 10;

		private static int m_timerTTHelper = 0;

		private static readonly int m_timerTTHelper_Max = 100;

		public Action<Vector2I> OnScreenSize;

		private static bool ShowWhitelistPopup = false;

		private static bool CanShowWhitelistPopup = false;

		private static bool ShowHotfixPopup = false;

		private static bool CanShowHotfixPopup = false;

		private static bool m_isPaused;

		private static int m_pauseStackCount = 0;

		private MyNews m_changelog;

		private XmlSerializer m_changelogSerializer;

		private int m_queueSize;

		private static IErrorConsumer m_errorConsumer = new MyGameErrorConsumer();

		private IVRageWindow form;

		private bool m_joystickLastUsed;

		public static bool IsDirectX11 => MyVideoSettingsManager.RunningGraphicsRenderer == DirectX11RendererKey;

		public static bool IsGameReady
		{
			get
			{
				if (IsUpdateReady)
				{
					if (!AreClipmapsReady || !RenderTasksFinished)
					{
						return MyMultiplayer.Static != null;
					}
					return true;
				}
				return false;
			}
		}

		public static bool AreClipmapsReady
		{
			get
			{
				if (!m_areClipmapsReady)
				{
					return !MyFakes.ENABLE_WAIT_UNTIL_CLIPMAPS_READY;
				}
				return true;
			}
			set
			{
				m_areClipmapsReady = (!m_reconfirmClipmaps && value);
				if (MySession.Static != null && (!value || m_reconfirmClipmaps))
				{
					foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
					{
						(instance.Render as MyRenderComponentVoxelMap)?.ResetLoading();
					}
				}
				m_reconfirmClipmaps = !value;
			}
		}

		public static bool RenderTasksFinished
		{
			get
			{
				return true;
			}
			set
			{
				m_renderTasksFinished = value;
			}
		}

		public static EnumAutorestartStage AutoRestartState => m_autoRestartState;

		public static bool IsAutoRestarting => m_autoRestartState == EnumAutorestartStage.Restarting;

		public bool IsGoingToUpdate => m_isGoingToUpdate;

		public bool IsRestartingForUpdate
		{
			get
			{
				if (IsGoingToUpdate)
				{
					return IsAutoRestarting;
				}
				return false;
			}
		}

		public bool SuppressLoadingDialogs
		{
			get;
			private set;
		}

		public static int TotalGamePlayTimeInMilliseconds => (IsPaused ? m_pauseStartTimeInMilliseconds : TotalSimulationTimeInMilliseconds) - m_totalPauseTimeInMilliseconds;

		public static int TotalTimeInMilliseconds => (int)Static.TotalTime.Milliseconds;

		public static int TotalTimeInTicks => (int)Static.TotalTime.Ticks;

		public static int TotalSimulationTimeInMilliseconds => (int)Static.SimulationTimeWithSpeed.Milliseconds;

		public static double SecondsSinceLastFrame
		{
			get;
			private set;
		}

		public bool EnableDamageEffects
		{
			get
			{
				return m_enableDamageEffects;
			}
			set
			{
				m_enableDamageEffects = value;
				UpdateDamageEffectsInScene();
			}
		}

		public static IGameCustomInitialization GameCustomInitialization
		{
			get;
			set;
		}

		public bool IsCursorVisible
		{
			get;
			private set;
		}

		public bool PauseInput
		{
			get;
			private set;
		}

		public static bool IsExitForced
		{
			get;
			set;
		}

		public static bool IsPaused
		{
			get
			{
				return m_isPaused;
			}
			set
			{
				if (!Sync.MultiplayerActive || !Sync.IsServer)
				{
					if (m_isPaused != value)
					{
						m_isPaused = value;
						if (IsPaused)
						{
							m_pauseStartTimeInMilliseconds = TotalSimulationTimeInMilliseconds;
						}
						else
						{
							m_totalPauseTimeInMilliseconds += TotalSimulationTimeInMilliseconds - m_pauseStartTimeInMilliseconds;
						}
					}
				}
				else
				{
					if (m_isPaused)
					{
						MyAudio.Static.ResumeGameSounds();
					}
					m_isPaused = false;
				}
				if (!m_isPaused)
				{
					m_pauseStackCount = 0;
				}
				MyParticlesManager.Paused = m_isPaused;
			}
		}

		public static IErrorConsumer ErrorConsumer
		{
			get
			{
				return m_errorConsumer;
			}
			set
			{
				m_errorConsumer = value;
			}
		}

		public uint IntroVideoId
		{
			get;
			protected set;
		}

		public event EventHandler OnGameLoaded;

		public event EventHandler OnScreenshotTaken;

		public MySandboxGame(string[] commandlineArgs, IntPtr windowHandle = default(IntPtr))
		{
			MyUtils.MainThread = Thread.CurrentThread;
			if (Config.SyncRendering)
			{
				MyRandom.EnableDeterminism = true;
				MyFakes.FORCE_NO_WORKER = true;
				MyFakes.ENABLE_WORKSHOP_MODS = false;
				MyFakes.ENABLE_HAVOK_MULTITHREADING = false;
				MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING = false;
				MyRenderProxy.SetFrameTimeStep(0.0166666675f);
				MyRenderProxy.Settings.IgnoreOcclusionQueries = !IsVideoRecordingEnabled;
				MyRenderProxy.SetSettingsDirty();
			}
			if (commandlineArgs != null)
			{
				if (commandlineArgs.Contains("-skipintro"))
				{
					MyFakes.ENABLE_LOGOS = false;
				}
				if (commandlineArgs.Contains("-suppressLoadingDialogs"))
				{
					SuppressLoadingDialogs = true;
				}
			}
			InitiliazeRender(windowHandle);
			MyGameService.WorkshopService.OnAuthenticationFailed += OnWorkshopAuthenticationFailed;
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				ConfigDedicated.Load();
				Console.CancelKeyPress += Console_CancelKeyPress;
			}
			RegisterAssemblies(commandlineArgs);
			Log.WriteLine("MySandboxGame.Constructor() - START");
			Log.IncreaseIndent();
			base.UpdateThread = Thread.CurrentThread;
			MyScreenManager.UpdateThread = base.UpdateThread;
			MyPrecalcComponent.UpdateThreadManagedId = base.UpdateThread.ManagedThreadId;
			Log.WriteLine("Game dir: " + MyFileSystem.ExePath);
			Log.WriteLine("Content dir: " + MyFileSystem.ContentPath);
			Static = this;
			InitNumberOfCores();
			MyLanguage.Init();
			MyGlobalTypeMetadata.Static.Init();
			Preallocate();
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				IPAddress address = MyDedicatedServerOverrides.IpAddress ?? IPAddressExtensions.ParseOrAny(ConfigDedicated.IP);
				ushort port = (ushort)(MyDedicatedServerOverrides.Port ?? ConfigDedicated.ServerPort);
				IPEndPoint iPEndPoint = new IPEndPoint(address, port);
				MyLog.Default.WriteLineAndConsole("Bind IP : " + iPEndPoint.ToString());
				FatalErrorDuringInit = !((MyDedicatedServerBase)(MyMultiplayer.Static = new MyDedicatedServer(iPEndPoint))).ServerStarted;
				if (FatalErrorDuringInit)
				{
					Exception ex = new Exception("Fatal error during dedicated server init see log for more information.");
					ex.Data["Silent"] = true;
					throw ex;
				}
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated && !FatalErrorDuringInit)
			{
				(MyMultiplayer.Static as MyDedicatedServerBase).SendGameTagsToSteam();
			}
			SessionCompatHelper = (Activator.CreateInstance(MyPerGameSettings.CompatHelperType) as MySessionCompatHelper);
			InitMultithreading();
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MyGameService.IsActive)
			{
				MyGameService.OnOverlayActivated += OverlayActivated;
			}
			MyCampaignManager.Static.Init();
			Log.DecreaseIndent();
			Log.WriteLine("MySandboxGame.Constructor() - END");
		}

		private void OnWorkshopAuthenticationFailed(UGCAuthenticationFailReason reason, Action onResume)
		{
			MyWorkshopAuthenticate.Start(onResume);
		}

		protected virtual void InitiliazeRender(IntPtr windowHandle)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				GameRenderComponent = new MyGameRenderComponent();
				MyRenderDeviceSettings? settingsToTry = MyVideoSettingsManager.Initialize();
				StartRenderComponent(settingsToTry, windowHandle);
				m_windowCreatedEvent.WaitOne();
			}
		}

		private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			ExitThreadSafe();
			Thread.Sleep(1000);
		}

		public void Run(bool customRenderLoop = false, Action disposeSplashScreen = null)
		{
			if (!FatalErrorDuringInit)
			{
				if (GameRenderComponent != null)
				{
					MyVideoSettingsManager.LogApplicationInformation();
					MyVRage.Platform.LogEnvironmentInformation();
				}
				Initialize();
				disposeSplashScreen?.Invoke();
				LoadData_UpdateThread();
				foreach (IPlugin plugin in MyPlugins.Plugins)
				{
					Log.WriteLineAndConsole("Plugin Init: " + plugin.GetType());
					plugin.Init(this);
				}
				if (MyPerGameSettings.Destruction && !HkBaseSystem.DestructionEnabled)
				{
					MyLog.Default.WriteLine("Havok Destruction is not availiable in this build. Exiting game.");
					ExitThreadSafe();
				}
				else if (!customRenderLoop)
				{
					RunLoop();
					EndLoop();
				}
			}
		}

		public void EndLoop()
		{
			MyLog.Default.WriteLineAndConsole("Exiting..");
			if (MySpaceAnalytics.Instance != null)
			{
				MySpaceAnalytics.Instance.EndSession();
			}
			MyAnalyticsManager.Instance.FlushAndDispose();
			MyNetworkMonitor.Done();
			UnloadData_UpdateThread();
		}

		public virtual void SwitchSettings(MyRenderDeviceSettings settings)
		{
			MyRenderProxy.SwitchDeviceSettings(settings);
		}

		protected virtual void InitInput()
		{
			MyLog.Default.WriteLine("MyGuiGameControlsHelpers()");
			MyGuiGameControlsHelpers.Add(MyControlsSpace.FORWARD, new MyGuiDescriptor(MyCommonTexts.ControlName_Forward));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.BACKWARD, new MyGuiDescriptor(MyCommonTexts.ControlName_Backward));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.STRAFE_LEFT, new MyGuiDescriptor(MyCommonTexts.ControlName_StrafeLeft));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.STRAFE_RIGHT, new MyGuiDescriptor(MyCommonTexts.ControlName_StrafeRight));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.ROLL_LEFT, new MyGuiDescriptor(MySpaceTexts.ControlName_RollLeft));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.ROLL_RIGHT, new MyGuiDescriptor(MySpaceTexts.ControlName_RollRight));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SPRINT, new MyGuiDescriptor(MyCommonTexts.ControlName_HoldToSprint));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.PRIMARY_TOOL_ACTION, new MyGuiDescriptor(MySpaceTexts.ControlName_FirePrimaryWeapon));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SECONDARY_TOOL_ACTION, new MyGuiDescriptor(MySpaceTexts.ControlName_FireSecondaryWeapon));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.JUMP, new MyGuiDescriptor(MyCommonTexts.ControlName_UpOrJump));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CROUCH, new MyGuiDescriptor(MyCommonTexts.ControlName_DownOrCrouch));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SWITCH_WALK, new MyGuiDescriptor(MyCommonTexts.ControlName_SwitchWalk));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.DAMPING, new MyGuiDescriptor(MySpaceTexts.ControlName_InertialDampenersOnOff));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.THRUSTS, new MyGuiDescriptor(MySpaceTexts.ControlName_Jetpack));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.BROADCASTING, new MyGuiDescriptor(MySpaceTexts.ControlName_Broadcasting));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.HELMET, new MyGuiDescriptor(MySpaceTexts.ControlName_Helmet));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.USE, new MyGuiDescriptor(MyCommonTexts.ControlName_UseOrInteract));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.TOGGLE_REACTORS, new MyGuiDescriptor(MySpaceTexts.ControlName_PowerSwitchOnOff));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.TERMINAL, new MyGuiDescriptor(MySpaceTexts.ControlName_TerminalOrInventory));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.INVENTORY, new MyGuiDescriptor(MySpaceTexts.Inventory));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.HELP_SCREEN, new MyGuiDescriptor(MyCommonTexts.ControlName_Help));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.ACTIVE_CONTRACT_SCREEN, new MyGuiDescriptor(MyCommonTexts.ControlName_ActiveContractScreen));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SUICIDE, new MyGuiDescriptor(MyCommonTexts.ControlName_Suicide));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.PAUSE_GAME, new MyGuiDescriptor(MyCommonTexts.ControlName_PauseGame, MyCommonTexts.ControlDescPauseGame));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.ROTATION_LEFT, new MyGuiDescriptor(MySpaceTexts.ControlName_RotationLeft));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.ROTATION_RIGHT, new MyGuiDescriptor(MySpaceTexts.ControlName_RotationRight));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.ROTATION_UP, new MyGuiDescriptor(MySpaceTexts.ControlName_RotationUp));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.ROTATION_DOWN, new MyGuiDescriptor(MySpaceTexts.ControlName_RotationDown));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CAMERA_MODE, new MyGuiDescriptor(MyCommonTexts.ControlName_FirstOrThirdPerson));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.HEADLIGHTS, new MyGuiDescriptor(MySpaceTexts.ControlName_ToggleHeadlights));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CHAT_SCREEN, new MyGuiDescriptor(MySpaceTexts.Chat_screen));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CONSOLE, new MyGuiDescriptor(MySpaceTexts.ControlName_Console));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SCREENSHOT, new MyGuiDescriptor(MyCommonTexts.ControlName_Screenshot));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.LOOKAROUND, new MyGuiDescriptor(MyCommonTexts.ControlName_HoldToLookAround));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.LANDING_GEAR, new MyGuiDescriptor(MySpaceTexts.ControlName_LandingGear));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SWITCH_LEFT, new MyGuiDescriptor(MyCommonTexts.ControlName_PreviousColor));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SWITCH_RIGHT, new MyGuiDescriptor(MyCommonTexts.ControlName_NextColor));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.BUILD_SCREEN, new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarConfig));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE, new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateVerticalPos));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE, new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateVerticalNeg));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE, new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateHorizontalPos));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE, new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateHorizontalNeg));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE, new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateRollPos));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE, new MyGuiDescriptor(MyCommonTexts.ControlName_CubeRotateRollNeg));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_COLOR_CHANGE, new MyGuiDescriptor(MyCommonTexts.ControlName_CubeColorChange));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.BUILD_PLANNER, new MyGuiDescriptor(MyCommonTexts.ControlName_BuildPlanner));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SYMMETRY_SWITCH, new MyGuiDescriptor(MySpaceTexts.ControlName_SymmetrySwitch));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.USE_SYMMETRY, new MyGuiDescriptor(MySpaceTexts.ControlName_UseSymmetry));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT, new MyGuiDescriptor(MySpaceTexts.ControlName_CubeDefaultMountpoint));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT1, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot1));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT2, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot2));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT3, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot3));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT4, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot4));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT5, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot5));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT6, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot6));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT7, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot7));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT8, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot8));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT9, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot9));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SLOT0, new MyGuiDescriptor(MyCommonTexts.ControlName_Slot0));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.TOOLBAR_DOWN, new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarDown));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.TOOLBAR_UP, new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarUp));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.TOOLBAR_NEXT_ITEM, new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarNextItem));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.TOOLBAR_PREV_ITEM, new MyGuiDescriptor(MyCommonTexts.ControlName_ToolbarPreviousItem));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_NONE, new MyGuiDescriptor(MyCommonTexts.SpectatorControls_None, MySpaceTexts.SpectatorControls_None_Desc));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_DELTA, new MyGuiDescriptor(MyCommonTexts.SpectatorControls_Delta, MyCommonTexts.SpectatorControls_Delta_Desc));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_FREE, new MyGuiDescriptor(MyCommonTexts.SpectatorControls_Free, MySpaceTexts.SpectatorControls_Free_Desc));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.SPECTATOR_STATIC, new MyGuiDescriptor(MyCommonTexts.SpectatorControls_Static, MySpaceTexts.SpectatorControls_Static_Desc));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.TOGGLE_HUD, new MyGuiDescriptor(MyCommonTexts.ControlName_HudOnOff));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.VOXEL_HAND_SETTINGS, new MyGuiDescriptor(MyCommonTexts.ControlName_VoxelHandSettings));
			MyGuiGameControlsHelpers.Add(MyControlsSpace.CONTROL_MENU, new MyGuiDescriptor(MyCommonTexts.ControlName_ControlMenu));
			if (MyFakes.ENABLE_MISSION_TRIGGERS)
			{
				MyGuiGameControlsHelpers.Add(MyControlsSpace.MISSION_SETTINGS, new MyGuiDescriptor(MySpaceTexts.ControlName_MissionSettings));
			}
			MyGuiGameControlsHelpers.Add(MyControlsSpace.FREE_ROTATION, new MyGuiDescriptor(MySpaceTexts.StationRotation_Static, MySpaceTexts.StationRotation_Static_Desc));
			if (MyPerGameSettings.VoiceChatEnabled)
			{
				MyGuiGameControlsHelpers.Add(MyControlsSpace.VOICE_CHAT, new MyGuiDescriptor(MyCommonTexts.ControlName_VoiceChat));
			}
			Dictionary<MyStringId, MyControl> dictionary = new Dictionary<MyStringId, MyControl>(MyStringId.Comparer);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.FORWARD, null, MyKeys.W);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.BACKWARD, null, MyKeys.S);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.STRAFE_LEFT, null, MyKeys.A);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.STRAFE_RIGHT, null, MyKeys.D);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROTATION_LEFT, null, MyKeys.Left);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROTATION_RIGHT, null, MyKeys.Right);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROTATION_UP, null, MyKeys.Up);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROTATION_DOWN, null, MyKeys.Down);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROLL_LEFT, null, MyKeys.Q);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.ROLL_RIGHT, null, MyKeys.E);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.SPRINT, null, MyKeys.Shift);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.SWITCH_WALK, null, MyKeys.CapsLock);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.JUMP, null, MyKeys.Space);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Navigation, MyControlsSpace.CROUCH, null, MyKeys.C);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.PRIMARY_TOOL_ACTION, MyMouseButtonsEnum.Left);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.SECONDARY_TOOL_ACTION, MyMouseButtonsEnum.Right);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.USE, null, MyKeys.F);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.HELMET, null, MyKeys.J);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.THRUSTS, null, MyKeys.X);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.DAMPING, null, MyKeys.Z);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.BROADCASTING, null, MyKeys.O);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.TOGGLE_REACTORS, null, MyKeys.Y);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.HEADLIGHTS, null, MyKeys.L);
			if (MyPerGameSettings.VoiceChatEnabled)
			{
				AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.VOICE_CHAT, null, MyKeys.U);
			}
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.CHAT_SCREEN, null, MyKeys.Enter);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.TERMINAL, null, MyKeys.K);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.INVENTORY, null, MyKeys.I);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems1, MyControlsSpace.SUICIDE, null, MyKeys.Back);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.BUILD_SCREEN, null, MyKeys.G);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE, null, MyKeys.PageDown);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE, null, MyKeys.Delete);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE, null, MyKeys.Home);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE, null, MyKeys.End);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE, null, MyKeys.Insert);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE, null, MyKeys.PageUp);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_COLOR_CHANGE, MyMouseButtonsEnum.Middle);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT, null, MyKeys.T);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.USE_SYMMETRY, null, MyKeys.N);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.SYMMETRY_SWITCH, null, MyKeys.M);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.FREE_ROTATION, null, MyKeys.B);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems2, MyControlsSpace.BUILD_PLANNER, MyMouseButtonsEnum.Middle);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.TOOLBAR_UP, null, MyKeys.OemPeriod);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.TOOLBAR_DOWN, null, MyKeys.OemComma);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.TOOLBAR_NEXT_ITEM);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.TOOLBAR_PREV_ITEM);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT1, null, MyKeys.D1);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT2, null, MyKeys.D2);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT3, null, MyKeys.D3);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT4, null, MyKeys.D4);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT5, null, MyKeys.D5);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT6, null, MyKeys.D6);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT7, null, MyKeys.D7);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT8, null, MyKeys.D8);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT9, null, MyKeys.D9);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Systems3, MyControlsSpace.SLOT0, null, MyKeys.D0, MyKeys.OemTilde);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.SWITCH_LEFT, null, MyKeys.OemOpenBrackets);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.SWITCH_RIGHT, null, MyKeys.OemCloseBrackets);
			if (MyFakes.ENABLE_MISSION_TRIGGERS)
			{
				AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.MISSION_SETTINGS, null, MyKeys.U);
			}
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.LANDING_GEAR, null, MyKeys.P);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.VOXEL_HAND_SETTINGS, null, MyKeys.K);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.CONTROL_MENU, null, MyKeys.OemMinus);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.PAUSE_GAME, null, MyKeys.Pause);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.CONSOLE, null, MyKeys.OemTilde);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.HELP_SCREEN, null, MyKeys.F1);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.ToolsOrWeapons, MyControlsSpace.ACTIVE_CONTRACT_SCREEN, null, MyKeys.OemSemicolon);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_NONE, null, MyKeys.F6);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_DELTA, null, MyKeys.F7);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_FREE, null, MyKeys.F8);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SPECTATOR_STATIC, null, MyKeys.F9);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.CAMERA_MODE, null, MyKeys.V);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.LOOKAROUND, null, MyKeys.Alt);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.SCREENSHOT, null, MyKeys.F4);
			AddDefaultGameControl(dictionary, MyGuiControlTypeEnum.Spectator, MyControlsSpace.TOGGLE_HUD, null, MyKeys.Tab);
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyInput.Initialize(new MyNullInput());
			}
			else
			{
				MyInput.Initialize(new MyVRageInput(MyVRage.Platform.Input, new MyKeysToString(), dictionary, enableDevKeys: false));
			}
			MySpaceBindingCreator.CreateBinding();
		}

		private void InitJoystick()
		{
			List<string> list = MyInput.Static.EnumerateJoystickNames();
			if (MyFakes.ENFORCE_CONTROLLER && list.Count > 0)
			{
				MyInput.Static.JoystickInstanceName = list[0];
			}
		}

		protected virtual void InitSteamWorkshop()
		{
			MyWorkshop.Category[] array = new MyWorkshop.Category[13];
			MyWorkshop.Category category = new MyWorkshop.Category
			{
				Id = "block",
				LocalizableName = MyCommonTexts.WorkshopTag_Block,
				IsVisibleForFilter = true
			};
			array[0] = category;
			category = new MyWorkshop.Category
			{
				Id = "skybox",
				LocalizableName = MyCommonTexts.WorkshopTag_Skybox,
				IsVisibleForFilter = true
			};
			array[1] = category;
			category = new MyWorkshop.Category
			{
				Id = "character",
				LocalizableName = MyCommonTexts.WorkshopTag_Character,
				IsVisibleForFilter = true
			};
			array[2] = category;
			category = new MyWorkshop.Category
			{
				Id = "animation",
				LocalizableName = MyCommonTexts.WorkshopTag_Animation,
				IsVisibleForFilter = true
			};
			array[3] = category;
			category = new MyWorkshop.Category
			{
				Id = "respawn ship",
				LocalizableName = MySpaceTexts.WorkshopTag_RespawnShip,
				IsVisibleForFilter = true
			};
			array[4] = category;
			category = new MyWorkshop.Category
			{
				Id = "production",
				LocalizableName = MySpaceTexts.WorkshopTag_Production,
				IsVisibleForFilter = true
			};
			array[5] = category;
			category = new MyWorkshop.Category
			{
				Id = "script",
				LocalizableName = MyCommonTexts.WorkshopTag_Script,
				IsVisibleForFilter = true
			};
			array[6] = category;
			category = new MyWorkshop.Category
			{
				Id = "modpack",
				LocalizableName = MyCommonTexts.WorkshopTag_ModPack,
				IsVisibleForFilter = true
			};
			array[7] = category;
			category = new MyWorkshop.Category
			{
				Id = "asteroid",
				LocalizableName = MySpaceTexts.WorkshopTag_Asteroid,
				IsVisibleForFilter = true
			};
			array[8] = category;
			category = new MyWorkshop.Category
			{
				Id = "planet",
				LocalizableName = MySpaceTexts.WorkshopTag_Planet,
				IsVisibleForFilter = true
			};
			array[9] = category;
			category = new MyWorkshop.Category
			{
				Id = "hud",
				LocalizableName = MySpaceTexts.WorkshopTag_Hud,
				IsVisibleForFilter = true
			};
			array[10] = category;
			category = new MyWorkshop.Category
			{
				Id = "other",
				LocalizableName = MyCommonTexts.WorkshopTag_Other,
				IsVisibleForFilter = true
			};
			array[11] = category;
			category = new MyWorkshop.Category
			{
				Id = "npc",
				LocalizableName = MyCommonTexts.WorkshopTag_Npc,
				IsVisibleForFilter = false
			};
			array[12] = category;
			MyWorkshop.Category[] array2 = new MyWorkshop.Category[1];
			category = new MyWorkshop.Category
			{
				Id = "exploration",
				LocalizableName = MySpaceTexts.WorkshopTag_Exploration
			};
			array2[0] = category;
			MyWorkshop.Category[] array3 = new MyWorkshop.Category[1];
			category = new MyWorkshop.Category
			{
				Id = "exploration",
				LocalizableName = MySpaceTexts.WorkshopTag_Exploration
			};
			array3[0] = category;
			MyWorkshop.Init(array, array2, array3, new MyWorkshop.Category[0]);
		}

		protected static void AddDefaultGameControl(Dictionary<MyStringId, MyControl> self, MyGuiControlTypeEnum controlTypeEnum, MyStringId controlId, MyMouseButtonsEnum? mouse = null, MyKeys? key = null, MyKeys? key2 = null)
		{
			MyDescriptor gameControlHelper = MyGuiGameControlsHelpers.GetGameControlHelper(controlId);
			self[controlId] = new MyControl(controlId, gameControlHelper.NameEnum, controlTypeEnum, mouse, key, null, key2, gameControlHelper.DescriptionEnum);
		}

		private void RegisterAssemblies(string[] args)
		{
			MyPlugins.RegisterGameAssemblyFile(MyPerGameSettings.GameModAssembly);
			if (MyPerGameSettings.GameModBaseObjBuildersAssembly != null)
			{
				MyPlugins.RegisterBaseGameObjectBuildersAssemblyFile(MyPerGameSettings.GameModBaseObjBuildersAssembly);
			}
			MyPlugins.RegisterGameObjectBuildersAssemblyFile(MyPerGameSettings.GameModObjBuildersAssembly);
			MyPlugins.RegisterSandboxAssemblyFile(MyPerGameSettings.SandboxAssembly);
			MyPlugins.RegisterSandboxGameAssemblyFile(MyPerGameSettings.SandboxGameAssembly);
			MyPlugins.RegisterFromArgs(args);
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyPlugins.RegisterUserAssemblyFiles(ConfigDedicated.Plugins);
			}
			MyPlugins.Load();
			if (args == null)
			{
				return;
			}
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "+connect_lobby" && args.Length > i + 1)
				{
					i++;
					if (ulong.TryParse(args[i], out ulong result))
					{
						m_joinLobbyId = result;
					}
				}
			}
		}

		public static void AfterLogos()
		{
			MyGuiSandbox.BackToMainMenu();
			if (MyFakes.ENABLE_GDPR_MESSAGE && (Config.GDPRConsentSent == false || !Config.GDPRConsentSent.HasValue))
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenGDPR());
			}
			if (Config.WelcomScreenCurrentStatus == MyConfig.WelcomeScreenStatus.NotSeen)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenWelcomeScreen());
			}
		}

		private void InitQuickLaunch()
		{
			if (MyVRage.Platform.Window != null)
			{
				MyVRage.Platform.Window.ShowAndFocus();
			}
			MyWorkshop.CancelToken cancelToken = new MyWorkshop.CancelToken();
			MyQuickLaunchType? myQuickLaunchType = null;
			if (m_joinLobbyId.HasValue)
			{
				IMyLobby myLobby = MyGameService.CreateLobby(m_joinLobbyId.Value);
				if (myLobby.IsValid)
				{
					MyJoinGameHelper.JoinGame(myLobby);
					return;
				}
			}
			if (myQuickLaunchType.HasValue && !Sandbox.Engine.Platform.Game.IsDedicated && Sandbox.Engine.Platform.Game.ConnectToServer == null)
			{
				MyQuickLaunchType value = myQuickLaunchType.Value;
				if (value > MyQuickLaunchType.LAST_SANDBOX)
				{
					throw new InvalidBranchException();
				}
				MyGuiSandbox.AddScreen(new MyGuiScreenStartQuickLaunch(myQuickLaunchType.Value, MyCommonTexts.StartGameInProgressPleaseWait));
			}
			else if (MyFakes.ENABLE_LOGOS)
			{
				MyGuiSandbox.BackToIntroLogos(AfterLogos);
				MyGuiScreenInitialLoading.CloseInstance();
			}
			else
			{
				AfterLogos();
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				bool flag = false;
				if (string.IsNullOrWhiteSpace(ConfigDedicated.WorldName))
				{
					string.Concat(MyTexts.Get(MyCommonTexts.DefaultSaveName), DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
				}
				else
				{
					ConfigDedicated.WorldName.Trim();
				}
				try
				{
					string lastSessionPath = MyLocalCache.GetLastSessionPath();
					if (!Sandbox.Engine.Platform.Game.IgnoreLastSession && !ConfigDedicated.IgnoreLastSession && lastSessionPath != null)
					{
						MyLog.Default.WriteLineAndConsole("Loading last session " + lastSessionPath);
						ulong sizeInBytes;
						MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(lastSessionPath, out sizeInBytes);
						if (!MySession.IsCompatibleVersion(myObjectBuilder_Checkpoint))
						{
							MyLog.Default.WriteLineAndConsole(MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersion).ToString());
							Static.Exit();
							return;
						}
						if (!MyWorkshop.DownloadWorldModsBlocking(myObjectBuilder_Checkpoint.Mods, cancelToken).Success)
						{
							MyLog.Default.WriteLineAndConsole("Unable to download mods");
							Static.Exit();
							return;
						}
						MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Load);
						MySession.Load(lastSessionPath, myObjectBuilder_Checkpoint, sizeInBytes);
						MySession.Static.StartServer(MyMultiplayer.Static);
					}
					else if (!string.IsNullOrEmpty(ConfigDedicated.LoadWorld))
					{
						string text = ConfigDedicated.LoadWorld;
						if (!Path.IsPathRooted(text))
						{
							text = Path.Combine(MyFileSystem.SavesPath, text);
						}
						if (Directory.Exists(text))
						{
							ulong sizeInBytes2;
							MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint2 = MyLocalCache.LoadCheckpoint(text, out sizeInBytes2);
							if (!MySession.IsCompatibleVersion(myObjectBuilder_Checkpoint2))
							{
								MyLog.Default.WriteLineAndConsole(MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersion).ToString());
								Static.Exit();
								return;
							}
							if (!MyWorkshop.DownloadWorldModsBlocking(myObjectBuilder_Checkpoint2.Mods, cancelToken).Success)
							{
								MyLog.Default.WriteLineAndConsole("Unable to download mods");
								Static.Exit();
								return;
							}
							MySpaceAnalytics.Instance.SetEntry(MyGameEntryEnum.Load);
							MySession.Load(text, myObjectBuilder_Checkpoint2, sizeInBytes2);
							MySession.Static.StartServer(MyMultiplayer.Static);
						}
						else
						{
							MyLog.Default.WriteLineAndConsole("World " + Path.GetFileName(ConfigDedicated.LoadWorld) + " not found.");
							MyLog.Default.WriteLineAndConsole("Creating new one with same name");
							flag = true;
							Path.GetFileName(ConfigDedicated.LoadWorld);
						}
					}
					else
					{
						flag = true;
					}
				}
				catch (Exception ex)
				{
					MyLog.Default.WriteLineAndConsole("Exception while loading world: " + ex.Message);
					MyLog.Default.WriteLine(ex.StackTrace);
					Static.Exit();
					return;
				}
				if (flag)
				{
					MyObjectBuilder_SessionSettings sessionSettings = ConfigDedicated.SessionSettings;
					if (!MyFileSystem.DirectoryExists(ConfigDedicated.PremadeCheckpointPath))
					{
						MyLog.Default.WriteLineAndConsole("Cannot start new world - Premade world not found " + ConfigDedicated.PremadeCheckpointPath);
						Static.Exit();
						return;
					}
					string premadeCheckpointPath = ConfigDedicated.PremadeCheckpointPath;
					ulong sizeInBytes3;
					MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint3 = MyLocalCache.LoadCheckpoint(premadeCheckpointPath, out sizeInBytes3);
					if (myObjectBuilder_Checkpoint3 == null)
					{
						MyLog.Default.WriteLineAndConsole("LoadCheckpoint failed.");
						Static.Exit();
						return;
					}
					myObjectBuilder_Checkpoint3.Settings = sessionSettings;
					myObjectBuilder_Checkpoint3.SessionName = ConfigDedicated.WorldName;
					if (!MyWorkshop.DownloadWorldModsBlocking(myObjectBuilder_Checkpoint3.Mods, cancelToken).Success)
					{
						MyLog.Default.WriteLineAndConsole("Unable to download mods");
						Static.Exit();
						return;
					}
					MySession.Load(premadeCheckpointPath, myObjectBuilder_Checkpoint3, sizeInBytes3);
					MySession.Static.Save(Path.Combine(MyFileSystem.SavesPath, myObjectBuilder_Checkpoint3.SessionName.Replace(':', '-')));
					MySession.Static.StartServer(MyMultiplayer.Static);
				}
			}
			if (Sandbox.Engine.Platform.Game.ConnectToServer != null && MyGameService.IsActive)
			{
				MyGameService.OnPingServerResponded += ServerResponded;
				MyGameService.OnPingServerFailedToRespond += ServerFailedToRespond;
				MyGameService.PingServer(Sandbox.Engine.Platform.Game.ConnectToServer.Address.ToIPv4NetworkOrder(), (ushort)Sandbox.Engine.Platform.Game.ConnectToServer.Port);
				Sandbox.Engine.Platform.Game.ConnectToServer = null;
			}
		}

		public void ServerResponded(object sender, MyGameServerItem serverItem)
		{
			MyLog.Default.WriteLineAndConsole("Server responded");
			CloseHandlers();
			MyJoinGameHelper.JoinGame(serverItem);
		}

		public void ServerFailedToRespond(object sender, object e)
		{
			MyLog.Default.WriteLineAndConsole("Server failed to respond");
			CloseHandlers();
		}

		private void CloseHandlers()
		{
			MyGameService.OnPingServerResponded -= ServerResponded;
			MyGameService.OnPingServerFailedToRespond -= ServerFailedToRespond;
		}

		private static void InitNumberOfCores()
		{
			NumberOfCores = MyEnvironment.ProcessorCount;
			Log.WriteLine("Found processor count: " + NumberOfCores);
			NumberOfCores = MyUtils.GetClampInt(NumberOfCores, 1, 16);
			Log.WriteLine("Using processor count: " + NumberOfCores);
		}

		private void InitMultithreading()
		{
			if (MyFakes.FORCE_SINGLE_WORKER)
			{
				ParallelTasks.Parallel.Scheduler = new FixedPriorityScheduler(1, ThreadPriority.Normal);
			}
			else if (MyFakes.FORCE_NO_WORKER)
			{
				ParallelTasks.Parallel.Scheduler = new FakeTaskScheduler();
			}
			else
			{
				ParallelTasks.Parallel.Scheduler = new PrioritizedScheduler(Math.Max(NumberOfCores / 2, 1), MyVRage.Platform.GetInfoCPU(out uint _).StartsWith("AMD"));
			}
			MyGameService.OnThreadpoolInitialized();
		}

		private void OnExit()
		{
			if (MySession.Static != null && MySpaceAnalytics.Instance != null)
			{
				MySpaceAnalytics.Instance.ReportGameplayEnd();
				MySpaceAnalytics.Instance.ReportGameQuit("Window closed");
			}
			ExitThreadSafe();
		}

		protected virtual IVRageWindow InitializeRenderThread()
		{
			base.DrawThread = Thread.CurrentThread;
			MyVRage.Platform.CreateWindow(MyPerGameSettings.GameName, MyPerGameSettings.GameIcon, (Config.Language == MyLanguagesEnum.ChineseChina) ? typeof(MyGuiControlContextMenu) : null);
			form = MyVRage.Platform.Window;
			MyVRage.Platform.Window.OnExit += OnExit;
			UpdateMouseCapture();
			if (Config.SyncRendering)
			{
				MyViewport viewport = new MyViewport(0f, 0f, Config.ScreenWidth.Value, Config.ScreenHeight.Value);
				RenderThread_SizeChanged((int)viewport.Width, (int)viewport.Height, viewport);
			}
			return form;
		}

		protected void RenderThread_SizeChanged(int width, int height, MyViewport viewport)
		{
			OnScreenSize?.Invoke(new Vector2I(width, height));
			Invoke(delegate
			{
				UpdateScreenSize(width, height, viewport);
			}, "MySandboxGame::UpdateScreenSize");
		}

		protected void RenderThread_BeforeDraw()
		{
			MyFpsManager.Update();
		}

		private void ShowUpdateDriverDialog(MyAdapterInfo adapter)
		{
			switch (MyErrorReporter.ReportOldDrivers(MyPerGameSettings.GameName, adapter.DeviceName, adapter.DriverUpdateLink))
			{
			case VRage.MessageBoxResult.Yes:
				ExitThreadSafe();
				MyVRage.Platform.OpenUrl(adapter.DriverUpdateLink);
				break;
			case VRage.MessageBoxResult.No:
				Config.DisableUpdateDriverNotification = true;
				Config.Save();
				break;
			default:
				_ = 2;
				break;
			}
		}

		protected virtual void CheckGraphicsCard(MyRenderMessageVideoAdaptersResponse msgVideoAdapters)
		{
			MyAdapterInfo adapter = msgVideoAdapters.Adapters[MyVideoSettingsManager.CurrentDeviceSettings.AdapterOrdinal];
			if (MyGpuIds.IsUnsupported(adapter.VendorId, adapter.DeviceId) || MyGpuIds.IsUnderMinimum(adapter.VendorId, adapter.DeviceId))
			{
				Log.WriteLine("Error: It seems that your graphics card is currently unsupported or it does not meet minimum requirements.");
				Log.WriteLine($"Graphics card name: {adapter.Name}, vendor id: 0x{adapter.VendorId:X}, device id: 0x{adapter.DeviceId:X}.");
				MyErrorReporter.ReportNotCompatibleGPU(MyPerGameSettings.GameName, Log.GetFilePath(), MyPerGameSettings.MinimumRequirementsPage);
			}
			if (Config.DisableUpdateDriverNotification)
			{
				return;
			}
			if (!adapter.IsNvidiaNotebookGpu)
			{
				if (adapter.DriverUpdateNecessary)
				{
					ShowUpdateDriverDialog(adapter);
				}
				return;
			}
			for (int i = 0; i < msgVideoAdapters.Adapters.Length; i++)
			{
				adapter = msgVideoAdapters.Adapters[i];
				if (adapter.DriverUpdateNecessary)
				{
					ShowUpdateDriverDialog(adapter);
				}
			}
		}

		protected virtual void Initialize()
		{
			Log.WriteLine("MySandboxGame.Initialize() - START");
			Log.IncreaseIndent();
			Log.WriteLine("Installed DLCs: ");
			Log.IncreaseIndent();
			if (!Sync.IsDedicated)
			{
				foreach (KeyValuePair<uint, MyDLCs.MyDLC> dLC in MyDLCs.DLCs)
				{
					if (MyGameService.IsDlcInstalled(dLC.Value.AppId))
					{
						Log.WriteLine($"{MyTexts.GetString(dLC.Value.DisplayName)} ({dLC.Value.AppId})");
					}
				}
			}
			Log.DecreaseIndent();
			if (MyGameService.Service != null)
			{
				MyNetworkMonitor.Init();
			}
			InitInput();
			InitSteamWorkshop();
			LoadData();
			MyVisualScriptingProxy.Init();
			MyVisualScriptingProxy.RegisterLogicProvider(typeof(VRage.Game.VisualScripting.MyVisualScriptLogicProvider));
			MyVisualScriptingProxy.RegisterLogicProvider(typeof(Sandbox.Game.MyVisualScriptLogicProvider));
			InitQuickLaunch();
			MyObjectBuilder_ProfilerSnapshot.SetDelegates();
			InitServices();
			Log.DecreaseIndent();
			Log.WriteLine("MySandboxGame.Initialize() - END");
			if (MyFakes.ENABLE_RESAVE_PROTOBUFFERS)
			{
				System.Threading.Tasks.Task.Run(delegate
				{
					ReSaveCustomWorlds();
					ReSaveScenarios();
					ReSaveInventoryScenes();
				});
			}
		}

		protected virtual void InitServices()
		{
		}

		private static void ReSaveInventoryScenes()
		{
			ReSaveSectors(Path.Combine(MyFileSystem.ContentPath, "InventoryScenes"));
		}

		private static void ReSaveScenarios()
		{
			ReSaveSectors(Path.Combine(MyFileSystem.ContentPath, "Scenarios"));
		}

		private static void ReSaveCustomWorlds()
		{
			ReSaveSectors(Path.Combine(MyFileSystem.ContentPath, "CustomWorlds"));
		}

		private static void ReSaveSectors(string topDirectory)
		{
			foreach (string file in MyFileSystem.GetFiles(topDirectory, "*.sbs", MySearchOption.AllDirectories))
			{
				if (!file.EndsWith("B5"))
				{
					string path = file + "B5";
					if (MyFileSystem.FileExists(path))
					{
						File.Delete(path);
					}
					ulong fileSize = 0uL;
					MyObjectBuilder_Sector objectBuilder = null;
					MyObjectBuilderSerializer.DeserializeXML(file, out objectBuilder, out fileSize);
					MyObjectBuilderSerializer.SerializePB(path, compress: false, objectBuilder);
				}
			}
		}

		protected virtual void StartRenderComponent(MyRenderDeviceSettings? settingsToTry, IntPtr windowHandle)
		{
			if (settingsToTry.HasValue)
			{
				MyRenderDeviceSettings value = settingsToTry.Value;
				value.DisableWindowedModeForOldDriver = Config.DisableUpdateDriverNotification;
				settingsToTry = value;
			}
			if (Config.SyncRendering)
			{
				GameRenderComponent.StartSync(m_gameTimer, InitializeRenderThread(), settingsToTry, MyRenderQualityEnum.NORMAL, MyPerGameSettings.MaxFrameRate);
			}
			else
			{
				GameRenderComponent.Start(m_gameTimer, InitializeRenderThread, settingsToTry, MyRenderQualityEnum.NORMAL, MyPerGameSettings.MaxFrameRate);
			}
			GameRenderComponent.RenderThread.SizeChanged += RenderThread_SizeChanged;
			GameRenderComponent.RenderThread.BeforeDraw += RenderThread_BeforeDraw;
		}

		public static void UpdateScreenSize(int width, int height, MyViewport viewport)
		{
			ScreenSize = new Vector2I(width, height);
			ScreenSizeHalf = new Vector2I(ScreenSize.X / 2, ScreenSize.Y / 2);
			ScreenViewport = viewport;
			if (MyGuiManager.UpdateScreenSize(ScreenSize, ScreenSizeHalf, MyVideoSettingsManager.IsTripleHead(ScreenSize)))
			{
				MyScreenManager.RecreateControls();
			}
			if (MySector.MainCamera != null)
			{
				MySector.MainCamera.UpdateScreenSize(ScreenViewport);
			}
			CanShowHotfixPopup = true;
			CanShowWhitelistPopup = true;
		}

		private static void Preallocate()
		{
			Log.WriteLine("Preallocate - START");
			Log.IncreaseIndent();
			Type[] types = new Type[6]
			{
				typeof(MyEntities),
				typeof(MyObjectBuilder_Base),
				typeof(MyTransparentGeometry),
				typeof(MyCubeGridDeformationTables),
				typeof(MyMath),
				typeof(MySimpleObjectDraw)
			};
			try
			{
				PreloadTypesFrom(MyPlugins.GameAssembly);
				PreloadTypesFrom(MyPlugins.SandboxAssembly);
				PreloadTypesFrom(MyPlugins.UserAssemblies);
				ForceStaticCtor(types);
				PreloadTypesFrom(typeof(MySandboxGame).Assembly);
			}
			catch (ReflectionTypeLoadException ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Exception[] loaderExceptions = ex.LoaderExceptions;
				foreach (Exception ex2 in loaderExceptions)
				{
					stringBuilder.AppendLine(ex2.Message);
					if (ex2 is FileNotFoundException)
					{
						FileNotFoundException ex3 = ex2 as FileNotFoundException;
						if (!string.IsNullOrEmpty(ex3.FusionLog))
						{
							stringBuilder.AppendLine("Fusion Log:");
							stringBuilder.AppendLine(ex3.FusionLog);
						}
					}
					stringBuilder.AppendLine();
				}
				stringBuilder.ToString();
			}
			Log.DecreaseIndent();
			Log.WriteLine("Preallocate - END");
		}

		private static void PreloadTypesFrom(Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					PreloadTypesFrom(assemblies[i]);
				}
			}
		}

		private static void PreloadTypesFrom(Assembly assembly)
		{
			if (assembly != null)
			{
				ForceStaticCtor((from type in assembly.GetTypes()
					where Attribute.IsDefined(type, typeof(PreloadRequiredAttribute))
					select type).ToArray());
			}
		}

		public static void ForceStaticCtor(Type[] types)
		{
			foreach (Type type in types)
			{
				Log.WriteLine(type.Name + " - START");
				RuntimeHelpers.RunClassConstructor(type.TypeHandle);
				Log.WriteLine(type.Name + " - END");
			}
		}

		private void LoadData()
		{
			if (MySession.Static != null)
			{
				MySession.Static.SetAsNotReady();
			}
			else if (MyAudio.Static != null)
			{
				MyAudio.Static.Mute = false;
			}
			if (MyInput.Static != null)
			{
				MyInput.Static.LoadContent();
			}
			HkBaseSystem.Init(16777216, LogWriter, deepProfiling: false);
			WriteHavokCodeToLog();
			ParallelTasks.Parallel.StartOnEachWorker(delegate
			{
				HkBaseSystem.InitThread(Thread.CurrentThread.Name);
			});
			MyPhysicsDebugDraw.DebugGeometry = new HkGeometry();
			Log.WriteLine("MySandboxGame.LoadData() - START");
			Log.IncreaseIndent();
			if (MyDefinitionManager.Static.GetScenarioDefinitions().Count == 0)
			{
				MyDefinitionManager.Static.LoadScenarios();
			}
			MyDefinitionManager.Static.PreloadDefinitions();
			MyAudioInitParams initParams = default(MyAudioInitParams);
			IMyAudio instance;
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				instance = MyVRage.Platform.Audio;
			}
			else
			{
				IMyAudio myAudio = new MyNullAudio();
				instance = myAudio;
			}
			initParams.Instance = instance;
			initParams.SimulateNoSoundCard = MyFakes.SIMULATE_NO_SOUND_CARD;
			initParams.DisablePooling = MyFakes.DISABLE_SOUND_POOLING;
			initParams.CacheLoaded = true;
			initParams.OnSoundError = MyAudioExtensions.OnSoundError;
			MyAudio.LoadData(initParams, MyAudioExtensions.GetSoundDataFromDefinitions(), MyAudioExtensions.GetEffectData());
			if (MyPerGameSettings.UseVolumeLimiter)
			{
				MyAudio.Static.UseVolumeLimiter = true;
			}
			if (MyPerGameSettings.UseSameSoundLimiter)
			{
				MyAudio.Static.UseSameSoundLimiter = true;
				MyAudio.Static.SetSameSoundLimiter();
			}
			if (MyPerGameSettings.UseReverbEffect && MyFakes.AUDIO_ENABLE_REVERB)
			{
				if (Config.EnableReverb && MyAudio.Static.SampleRate > MyAudio.MAX_SAMPLE_RATE)
				{
					Config.EnableReverb = false;
				}
				else
				{
					MyAudio.Static.EnableReverb = Config.EnableReverb;
				}
			}
			else
			{
				Config.EnableReverb = false;
				MyAudio.Static.EnableReverb = false;
			}
			MyAudio.Static.EnableDoppler = Config.EnableDoppler;
			MyAudio.Static.VolumeMusic = Config.MusicVolume;
			MyAudio.Static.VolumeGame = Config.GameVolume;
			MyAudio.Static.VolumeHud = Config.GameVolume;
			MyAudio.Static.VolumeVoiceChat = Config.VoiceChatVolume;
			MyAudio.Static.EnableVoiceChat = Config.EnableVoiceChat;
			MyGuiAudio.HudWarnings = Config.HudWarnings;
			MyGuiSoundManager.Audio = MyGuiAudio.Static;
			StartPreload();
			if (EmptyKeys.UserInterface.Engine.Instance != null)
			{
				MyAudioDevice myAudioDevice = EmptyKeys.UserInterface.Engine.Instance.AudioDevice as MyAudioDevice;
				if (myAudioDevice != null)
				{
					myAudioDevice.GuiAudio = MyGuiAudio.Static;
				}
			}
			MyLocalization.Initialize();
			MyGuiSandbox.LoadData(Sandbox.Engine.Platform.Game.IsDedicated);
			LoadGui();
			m_dataLoadedDebug = true;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MyGameService.IsActive)
			{
				MyGameService.LobbyJoinRequested += Matchmaking_LobbyJoinRequest;
				MyGameService.ServerChangeRequested += Matchmaking_ServerChangeRequest;
			}
			MyInput.Static.LoadData(Config.ControlsGeneral, Config.ControlsButtons);
			InitJoystick();
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyParticlesManager.Enabled = false;
			}
			MyParticlesManager.EnableCPUGenerations = MyFakes.ENABLE_CPU_PARTICLES;
			MyParticlesManager.CalculateGravityInPoint = MyGravityProviderSystem.CalculateTotalGravityInPoint;
			MyRenderProxy.SetGravityProvider(MyGravityProviderSystem.CalculateTotalGravityInPoint);
			Log.DecreaseIndent();
			Log.WriteLine("MySandboxGame.LoadData() - END");
			InitModAPI();
			MyPositionComponentBase.OnReportInvalidMatrix = (Action<VRage.ModAPI.IMyEntity>)Delegate.Combine(MyPositionComponentBase.OnReportInvalidMatrix, new Action<VRage.ModAPI.IMyEntity>(ReportInvalidMatrix));
			this.OnGameLoaded?.Invoke(this, null);
		}

		private void ReportInvalidMatrix(VRage.Game.ModAPI.Ingame.IMyEntity entity)
		{
			if (entity is MyEntity && Sandbox.Engine.Platform.Game.IsDedicated && MyPerGameSettings.SendLogToKeen)
			{
				if (MySession.Static.Players.GetEntityController((MyEntity)entity) == null)
				{
					Log.Error(entity.Name ?? string.Concat(entity, " with ID:", entity.EntityId, " has invalid world matrix! Deleted."));
					((MyEntity)entity).Close();
				}
				MyReportException ex = new MyReportException();
				MyLog.Default.WriteLineAndConsole("Exception with invalid matrix");
				MyLog.Default.WriteLine(ex.ToString());
				MyLog.Default.WriteLine(Environment.StackTrace);
				PerformNotInteractiveReport.InvokeIfNotNull();
			}
		}

		public static void StartPreload()
		{
			m_currentPreloadingTask?.WaitOrExecute();
			m_currentPreloadingTask = ParallelTasks.Parallel.Start(PerformPreloading);
		}

		public static void WaitForPreload()
		{
			m_currentPreloadingTask?.WaitOrExecute();
		}

		private static void PerformPreloading()
		{
			Log.WriteLine("MySandboxGame.PerformPreload() - START");
			ParallelTasks.Task? task = MyMultiplayer.InitOfflineReplicationLayer();
			MyMath.InitializeFastSin();
			List<Tuple<MyObjectBuilder_Definitions, string>> list = null;
			try
			{
				list = MyDefinitionManager.Static.GetSessionPreloadDefinitions();
			}
			catch (MyLoadingException ex)
			{
				string message = ex.Message;
				Log.WriteLineAndConsole(message);
				MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder(message), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), null, null, null, null, ClosePopup);
				Vector2 value = myGuiScreenMessageBox.Size.Value;
				value.Y *= 1.5f;
				myGuiScreenMessageBox.Size = value;
				myGuiScreenMessageBox.RecreateControls(constructor: false);
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
			}
			if (Static.Exiting)
			{
				return;
			}
			if (list != null)
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MyGuiTextures.Static.Reload();
				}
				Log.WriteLine("MySandboxGame.PerformPreload() - PRELOAD VANILLA SOUNDS AND VOXELS");
				List<MyObjectBuilder_AudioDefinition> sounds = new List<MyObjectBuilder_AudioDefinition>();
				List<MyObjectBuilder_VoxelMapStorageDefinition> voxels = new List<MyObjectBuilder_VoxelMapStorageDefinition>();
				foreach (Tuple<MyObjectBuilder_Definitions, string> item2 in list)
				{
					MyObjectBuilder_Definitions item = item2.Item1;
					if (item.Sounds != null && !Sandbox.Engine.Platform.Game.IsDedicated)
					{
						sounds.AddRange(item.Sounds);
					}
					if (item.VoxelMapStorages != null && MyFakes.ENABLE_ASTEROIDS)
					{
						voxels.AddRange(item.VoxelMapStorages);
					}
				}
				MyDefinitionManager.Static.ReloadVoxelMaterials();
				string defaultAsteroidGeneratorVersion = MyFakes.DEFAULT_PROCEDURAL_ASTEROID_GENERATOR.ToString();
				ParallelTasks.Parallel.For(0, voxels.Count + sounds.Count, delegate(int i)
				{
					if (!Static.Exiting)
					{
						if (i < voxels.Count)
						{
							MyObjectBuilder_VoxelMapStorageDefinition myObjectBuilder_VoxelMapStorageDefinition = voxels[i];
							if (!string.IsNullOrEmpty(myObjectBuilder_VoxelMapStorageDefinition.StorageFile))
							{
								string[] explicitProceduralGeneratorVersions = myObjectBuilder_VoxelMapStorageDefinition.ExplicitProceduralGeneratorVersions;
								if ((explicitProceduralGeneratorVersions == null || explicitProceduralGeneratorVersions.Contains(defaultAsteroidGeneratorVersion)) && (myObjectBuilder_VoxelMapStorageDefinition.UseAsPrimaryProceduralAdditionShape || myObjectBuilder_VoxelMapStorageDefinition.UseForProceduralAdditions || myObjectBuilder_VoxelMapStorageDefinition.UseForProceduralRemovals))
								{
									MyStorageBase.LoadFromFile(Path.Combine(MyFileSystem.ContentPath, myObjectBuilder_VoxelMapStorageDefinition.StorageFile));
								}
							}
						}
						else
						{
							MyObjectBuilder_AudioDefinition myObjectBuilder_AudioDefinition = sounds[i - voxels.Count];
							if (MyAudio.Static != null)
							{
								MyStringHash.GetOrCompute(myObjectBuilder_AudioDefinition.Id.SubtypeName);
								foreach (MyAudioWave wave in myObjectBuilder_AudioDefinition.Waves)
								{
									if (!string.IsNullOrEmpty(wave.Start))
									{
										MyAudio.Static.Preload(wave.Start);
									}
									if (!string.IsNullOrEmpty(wave.Loop))
									{
										MyAudio.Static.Preload(wave.Loop);
									}
									if (!string.IsNullOrEmpty(wave.End))
									{
										MyAudio.Static.Preload(wave.End);
									}
								}
							}
						}
					}
				}, WorkPriority.VeryLow);
				MyDefinitionManager.Static.GetAllSessionPreloadObjectBuilders();
				Log.WriteLine("MySandboxGame.PerformPreload() - END VANILLA SOUNDS AND VOXELS");
				task?.WaitOrExecute();
			}
			Log.WriteLine("MySandboxGame.PerformPreload() - END");
		}

		private BoundingFrustumD GetCameraFrustum()
		{
			if (MySector.MainCamera == null)
			{
				return new BoundingFrustumD(MatrixD.Identity);
			}
			return MySector.MainCamera.BoundingFrustumFar;
		}

		protected virtual void LoadGui()
		{
			MyGuiSandbox.LoadContent();
		}

		private void WriteHavokCodeToLog()
		{
			Log.WriteLine("HkGameName: " + HkBaseSystem.GameName);
			string[] keyCodes = HkBaseSystem.GetKeyCodes();
			foreach (string text in keyCodes)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					Log.WriteLine("HkCode: " + text);
				}
			}
		}

		private void InitModAPI()
		{
			try
			{
				if (MyVRage.Platform.IsScriptCompilationSupported)
				{
					InitIlCompiler();
					InitIlChecker();
				}
			}
			catch (MyWhitelistException ex)
			{
				Log.Error("Mod API Whitelist Integrity: {0}", ex.Message);
				ShowWhitelistPopup = true;
			}
			catch (Exception ex2)
			{
				Log.Error("Error during ModAPI initialization: {0}", ex2.Message);
				ShowHotfixPopup = true;
			}
		}

		private static void OnDotNetHotfixPopupClosed(MyGuiScreenMessageBox.ResultEnum result)
		{
			Process.Start("https://support.microsoft.com/kb/3120241");
			ClosePopup(result);
		}

		private static void OnWhitelistIntegrityPopupClosed(MyGuiScreenMessageBox.ResultEnum result)
		{
			ClosePopup(result);
		}

		private static void ClosePopup(MyGuiScreenMessageBox.ResultEnum result)
		{
			Process.GetCurrentProcess().Kill();
		}

		private void InitIlCompiler()
		{
			Log.IncreaseIndent();
			if (GameCustomInitialization != null)
			{
				GameCustomInitialization.InitIlCompiler();
			}
			Log.DecreaseIndent();
			if (MyFakes.ENABLE_SCRIPTS_PDB)
			{
				MyScriptCompiler.Static.EnableDebugInformation = true;
			}
		}

		internal static void InitIlChecker()
		{
			if (GameCustomInitialization != null)
			{
				GameCustomInitialization.InitIlChecker();
			}
			using (IMyWhitelistBatch myWhitelistBatch = MyScriptCompiler.Static.Whitelist.OpenBatch())
			{
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.ModApi, typeof(MyCubeBuilder).GetField("Static"), typeof(MyCubeBuilder).GetProperty("CubeBuilderState"), typeof(MyCubeBuilderState).GetProperty("CurrentBlockDefinition"), typeof(MyHud).GetField("BlockInfo"));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(MyHudBlockInfo), typeof(MyHudBlockInfo.ComponentInfo), typeof(MyObjectBuilder_CubeBuilderDefinition), typeof(MyPlacementSettings), typeof(MyGridPlacementSettings), typeof(SnapMode), typeof(VoxelPlacementMode), typeof(VoxelPlacementSettings));
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof(ListExtensions), typeof(VRage.Game.ModAPI.Ingame.IMyCubeBlock), typeof(MyIni), typeof(Sandbox.ModAPI.Ingame.IMyTerminalBlock), typeof(Vector3), typeof(MySprite));
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(MyAPIUtilities), typeof(Sandbox.ModAPI.Interfaces.ITerminalAction), typeof(IMyTerminalAction), typeof(VRage.Game.ModAPI.IMyCubeBlock), typeof(MyAPIGateway), typeof(IMyCameraController), typeof(VRage.ModAPI.IMyEntity), typeof(MyEntity), typeof(MyEntityExtensions), typeof(EnvironmentItemsEntry), typeof(MyObjectBuilder_GasProperties), typeof(MyObjectBuilder_AdvancedDoor), typeof(MyObjectBuilder_AdvancedDoorDefinition), typeof(MyObjectBuilder_ComponentBase), typeof(MyObjectBuilder_Base), typeof(MyIngameScript), typeof(MyResourceSourceComponent), typeof(MyCharacterOxygenComponent), typeof(IMyUseObject), typeof(IMyModelDummy), typeof(IMyTextSurfaceScript), typeof(MyObjectBuilder_SafeZoneBlock));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(MyBillboard.BlendTypeEnum));
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(MyObjectBuilder_EntityStatRegenEffect));
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(MyStatLogic), typeof(MyEntityStatComponent), typeof(MyEnvironmentSector), typeof(SerializableVector3), typeof(MyDefinitionManager), typeof(MyFixedPoint), typeof(ListReader<>), typeof(MyStorageData), typeof(MyEventArgs), typeof(MyStringId), typeof(MyGameTimer), typeof(MyLight), typeof(IMyAutomaticRifleGun), typeof(MyContractAcquisition));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.ModApi, typeof(MySpectatorCameraController).GetProperty("IsLightOn"));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof(TerminalActionExtensions), typeof(Sandbox.ModAPI.Interfaces.ITerminalAction), typeof(ITerminalProperty), typeof(ITerminalProperty<>), typeof(TerminalPropertyExtensions), typeof(MySpaceTexts), typeof(StringBuilderExtensions_Format), typeof(MyFixedPoint));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof(MyTuple), typeof(MyTuple<>), typeof(MyTuple<, >), typeof(MyTuple<, , >), typeof(MyTuple<, , , >), typeof(MyTuple<, , , , >), typeof(MyTuple<, , , , , >), typeof(MyTupleComparer<, >), typeof(MyTupleComparer<, , >));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof(MyTexts.MyLanguageDescription), typeof(MyLanguagesEnum));
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof(VRage.ModAPI.IMyInput));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(MyInputExtensions), typeof(MyKeys), typeof(MyJoystickAxesEnum), typeof(MyJoystickButtonsEnum), typeof(MyMouseButtonsEnum), typeof(MySharedButtonsEnum), typeof(MyGuiControlTypeEnum), typeof(MyGuiInputDeviceEnum));
				IEnumerable<MethodInfo> source = from method in typeof(MyComponentContainer).GetMethods()
					where method.Name == "TryGet" && method.ContainsGenericParameters && method.GetParameters().Length == 1
					select method;
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, source.FirstOrDefault(), typeof(MyComponentContainer).GetMethod("Has"), typeof(MyComponentContainer).GetMethod("Get"), typeof(MyComponentContainer).GetMethod("TryGet", new Type[2]
				{
					typeof(Type),
					typeof(MyComponentBase).MakeByRefType()
				}));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.Ingame, typeof(ListReader<>), typeof(MyDefinitionId), typeof(MyRelationsBetweenPlayerAndBlock), typeof(MyRelationsBetweenPlayerAndBlockExtensions), typeof(MyResourceSourceComponentBase), typeof(MyObjectBuilder_GasProperties), typeof(SerializableDefinitionId), typeof(MyCubeSize));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Ingame, typeof(MyComponentBase).GetMethod("GetAs"), typeof(MyComponentBase).GetProperty("ContainerBase"));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Ingame, typeof(MyObjectBuilder_Base).GetProperty("TypeId"), typeof(MyObjectBuilder_Base).GetProperty("SubtypeId"));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Ingame, typeof(MyResourceSourceComponent).GetProperty("CurrentOutput"), typeof(MyResourceSourceComponent).GetProperty("MaxOutput"), typeof(MyResourceSourceComponent).GetProperty("DefinedOutput"), typeof(MyResourceSourceComponent).GetProperty("ProductionEnabled"), typeof(MyResourceSourceComponent).GetProperty("RemainingCapacity"), typeof(MyResourceSourceComponent).GetProperty("HasCapacityRemaining"), typeof(MyResourceSourceComponent).GetProperty("ResourceTypes"), typeof(MyResourceSinkComponent).GetProperty("AcceptedResources"), typeof(MyResourceSinkComponent).GetProperty("RequiredInput"), typeof(MyResourceSinkComponent).GetProperty("SuppliedRatio"), typeof(MyResourceSinkComponent).GetProperty("CurrentInput"), typeof(MyResourceSinkComponent).GetProperty("IsPowered"), typeof(MyResourceSinkComponentBase).GetProperty("AcceptedResources"), typeof(MyResourceSinkComponentBase).GetMethod("CurrentInputByType"), typeof(MyResourceSinkComponentBase).GetMethod("IsPowerAvailable"), typeof(MyResourceSinkComponentBase).GetMethod("IsPoweredByType"), typeof(MyResourceSinkComponentBase).GetMethod("MaxRequiredInputByType"), typeof(MyResourceSinkComponentBase).GetMethod("RequiredInputByType"), typeof(MyResourceSinkComponentBase).GetMethod("SuppliedRatioByType"));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(MyPhysicsHelper), typeof(MyPhysics.CollisionLayers));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(MyLodTypeEnum), typeof(MyMaterialsSettings), typeof(MyShadowsSettings), typeof(MyPostprocessSettings), typeof(MyHBAOData), typeof(MySSAOSettings), typeof(MyEnvironmentLightData), typeof(MyEnvironmentData), typeof(MyPostprocessSettings.Layout), typeof(MySSAOSettings.Layout), typeof(MyShadowsSettings.Struct), typeof(MyShadowsSettings.Cascade), typeof(MyMaterialsSettings.Struct), typeof(MyMaterialsSettings.MyChangeableMaterial), typeof(MyGlareTypeEnum), typeof(SerializableDictionary<, >), typeof(MyToolBase), typeof(MyGunBase), typeof(MyDeviceBase), typeof(Stopwatch), typeof(ConditionalAttribute), typeof(Version), typeof(ObsoleteAttribute));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(IWork), typeof(ParallelTasks.Task), typeof(WorkOptions), typeof(VRage.Library.Threading.SpinLock), typeof(SpinLockRef), typeof(Monitor), typeof(AutoResetEvent), typeof(ManualResetEvent), typeof(Interlocked));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.ModApi, typeof(ProtoMemberAttribute), typeof(ProtoContractAttribute), typeof(ProtoIncludeAttribute), typeof(ProtoIgnoreAttribute), typeof(ProtoEnumAttribute), typeof(MemberSerializationOptions), typeof(DataFormat));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.ModApi, typeof(WorkData).GetMethod("FlagAsFailed"));
				myWhitelistBatch.AllowMembers(MyWhitelistTarget.Both, typeof(ArrayExtensions).GetMethod("Contains"));
				myWhitelistBatch.AllowTypes(MyWhitelistTarget.Both, typeof(MyPhysicalInventoryItemExtensions_ModAPI));
				myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof(ImmutableArray));
			}
		}

		private void Matchmaking_LobbyJoinRequest(IMyLobby lobby, ulong invitedBy)
		{
			if (!m_isPendingLobbyInvite && lobby.IsValid && (MySession.Static == null || MyMultiplayer.Static == null || MyMultiplayer.Static.LobbyId != lobby.LobbyId))
			{
				m_isPendingLobbyInvite = true;
				m_invitingLobby = lobby;
				if (invitedBy == MyGameService.UserId)
				{
					OnAcceptLobbyInvite(MyGuiScreenMessageBox.ResultEnum.YES);
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.InvitedToLobbyCaption), callback: OnAcceptLobbyInvite, messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.InvitedToLobby), MyGameService.GetPersonaName(invitedBy)))));
				}
			}
		}

		private void Matchmaking_ServerChangeRequest(string server, string password)
		{
			if (IPAddressExtensions.TryParseEndpoint(server, out IPEndPoint result))
			{
				MySessionLoader.UnloadAndExitToMenu();
				MyGameService.OnPingServerResponded += ServerResponded;
				MyGameService.OnPingServerFailedToRespond += ServerFailedToRespond;
				MyGameService.PingServer(result.Address.ToIPv4NetworkOrder(), (ushort)result.Port);
			}
		}

		public void OnAcceptLobbyInvite(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (!m_isPendingLobbyInvite)
			{
				return;
			}
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MySessionLoader.UnloadAndExitToMenu();
				if (m_invitingLobby.ConnectionStrategy == ConnectionStrategy.ConnectImmediately)
				{
					MyJoinGameHelper.JoinGame(m_invitingLobby.LobbyId);
				}
				else
				{
					MyJoinGameHelper.JoinGame(m_invitingLobby);
				}
			}
			m_isPendingLobbyInvite = false;
		}

		private void UnloadData()
		{
			Log.WriteLine("MySandboxGame.UnloadData() - START");
			Log.IncreaseIndent();
			UnloadAudio();
			UnloadInput();
			MyAudio.UnloadData();
			Log.DecreaseIndent();
			Log.WriteLine("MySandboxGame.UnloadData() - END");
			MyModels.UnloadData();
			MyGuiSandbox.UnloadContent();
		}

		private void UnloadAudio()
		{
			if (MyAudio.Static != null)
			{
				MyAudio.Static.Mute = true;
				MyEntity3DSoundEmitter.ClearEntityEmitters();
				MyAudio.Static.ClearSounds();
				MyHud.ScreenEffects.FadeScreen(1f);
			}
		}

		private void UnloadInput()
		{
			MyInput.UnloadData();
			MyGuiGameControlsHelpers.Reset();
		}

		public static void PausePush()
		{
			UpdatePauseState(++m_pauseStackCount);
		}

		public static void PausePop()
		{
			m_pauseStackCount--;
			if (m_pauseStackCount < 0)
			{
				m_pauseStackCount = 0;
			}
			UpdatePauseState(m_pauseStackCount);
		}

		public static void PauseToggle()
		{
			if (!Sync.MultiplayerActive)
			{
				if (IsPaused)
				{
					PausePop();
				}
				else
				{
					PausePush();
				}
			}
		}

		[Conditional("DEBUG")]
		public static void AssertUpdateThread()
		{
		}

		private static void UpdatePauseState(int pauseStackCount)
		{
			if (pauseStackCount > 0)
			{
				IsPaused = true;
			}
			else
			{
				IsPaused = false;
			}
		}

		protected override void Update()
		{
			if (IsRenderUpdateSyncEnabled && GameRenderComponent != null && GameRenderComponent.RenderThread != null)
			{
				if (GameRenderComponent.RenderThread.RenderUpdateSyncEvent != null)
				{
					GameRenderComponent.RenderThread.RenderUpdateSyncEvent.Set();
					GameRenderComponent.RenderThread.RenderUpdateSyncEvent.Reset();
				}
				else
				{
					GameRenderComponent.RenderThread.RenderUpdateSyncEvent = new ManualResetEvent(initialState: false);
				}
			}
			MyVRage.Platform.Update();
			bool isJoystickLastUsed = MyInput.Static.IsJoystickLastUsed;
			if (isJoystickLastUsed != m_joystickLastUsed)
			{
				m_joystickLastUsed = isJoystickLastUsed;
				MyAnalyticsHelper.ReportEvent(MyAnalyticsProgressionStatus.Gamepad, new Dictionary<string, object>
				{
					{
						"GamepadActive",
						isJoystickLastUsed
					}
				});
			}
			if (ShowHotfixPopup && CanShowHotfixPopup)
			{
				ShowHotfixPopup = false;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.ErrorPopup_Hotfix_Caption), messageText: MyTexts.Get(MyCommonTexts.ErrorPopup_Hotfix_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: OnDotNetHotfixPopupClosed, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
			}
			if (ShowWhitelistPopup && CanShowWhitelistPopup)
			{
				ShowHotfixPopup = false;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.ErrorPopup_Whitelist_Caption), messageText: MyTexts.Get(MyCommonTexts.ErrorPopup_Whitelist_Text), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: OnWhitelistIntegrityPopupClosed, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.NO));
			}
			long timestamp = Stopwatch.GetTimestamp();
			long num = timestamp - m_lastFrameTimeStamp;
			m_lastFrameTimeStamp = timestamp;
			SecondsSinceLastFrame = (MyRandom.EnableDeterminism ? 0.01666666753590107 : ((double)num / (double)Stopwatch.Frequency));
			if (ShowIsBetterGCAvailableNotification)
			{
				ShowIsBetterGCAvailableNotification = false;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.BetterGCIsAvailable), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
			}
			if (ShowGpuUnderMinimumNotification)
			{
				ShowGpuUnderMinimumNotification = false;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.GpuUnderMinimumNotification), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
			}
			form?.UpdateMainThread();
			using (Stats.Generic.Measure("InvokeQueue"))
			{
				ProcessInvoke();
			}
			MyGeneralStats.Static.Update();
			if (Config.SyncRendering)
			{
				if (IsVideoRecordingEnabled && MySession.Static != null && IsGameReady)
				{
					string pathToSave = Path.Combine(MyFileSystem.UserDataPath, "Recording", "img_" + base.SimulationFrameCounter.ToString("D8") + ".png");
					MyRenderProxy.TakeScreenshot(Vector2.One, pathToSave, debug: false, ignoreSprites: true, showNotification: false);
				}
				GameRenderComponent.RenderThread.TickSync();
			}
			using (Stats.Generic.Measure("RenderRequests"))
			{
				ProcessRenderOutput();
			}
			using (Stats.Generic.Measure("Network"))
			{
				if (Sync.Layer != null)
				{
					Sync.Layer.TransportLayer.Tick();
				}
				MyGameService.Update();
				try
				{
					MyNetworkReader.Process();
				}
				catch (MyIncompatibleDataException)
				{
					MyMultiplayer.Static.Dispose();
					MySessionLoader.UnloadAndExitToMenu();
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.IncompatibleDataNotification), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
			}
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ReportReplicatedObjects();
			}
			using (Stats.Generic.Measure("GuiUpdate"))
			{
				MyGuiSandbox.Update(16);
			}
			foreach (IHandleInputPlugin handleInputPlugin in MyPlugins.HandleInputPlugins)
			{
				handleInputPlugin.HandleInput();
			}
			using (Stats.Generic.Measure("Input"))
			{
				MyGuiSandbox.HandleInput();
				if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null)
				{
					MySession.Static.HandleInput();
				}
			}
			using (Stats.Generic.Measure("GameLogic"))
			{
				if (MySession.Static != null)
				{
					bool flag = true;
					if (Sandbox.Engine.Platform.Game.IsDedicated && ConfigDedicated.PauseGameWhenEmpty)
					{
						flag = (Sync.Clients.Count > 1 || !MySession.Static.Ready);
					}
					if (flag)
					{
						MySession.Static.Update(base.TotalTime);
					}
				}
			}
			using (Stats.Generic.Measure("InputAfter"))
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MyGuiSandbox.HandleInputAfterSimulation();
				}
			}
			if (MyFakes.SIMULATE_SLOW_UPDATE)
			{
				Thread.Sleep(40);
			}
			using (Stats.Generic.Measure("Audio"))
			{
				Vector3 listenerUp = Vector3.Up;
				Vector3 listenerFront = Vector3.Forward;
				if (MySector.MainCamera != null)
				{
					listenerUp = MySector.MainCamera.UpVector;
					listenerFront = -MySector.MainCamera.ForwardVector;
				}
				Vector3 velocity = Vector3.Zero;
				GetListenerVelocity(ref velocity);
				MyAudio.Static.Update(16, Vector3.Zero, listenerUp, listenerFront, velocity);
				if (MyMusicController.Static != null && MyMusicController.Static.Active)
				{
					MyMusicController.Static.Update();
				}
				if (Config.EnableMuteWhenNotInFocus && form != null)
				{
					if (!form.IsActive)
					{
						if (hasFocus)
						{
							MyAudio.Static.VolumeMusic = 0f;
							MyAudio.Static.VolumeGame = 0f;
							MyAudio.Static.VolumeHud = 0f;
							MyAudio.Static.VolumeVoiceChat = 0f;
							hasFocus = false;
						}
					}
					else if (!hasFocus)
					{
						MyAudio.Static.VolumeMusic = Config.MusicVolume;
						MyAudio.Static.VolumeGame = Config.GameVolume;
						MyAudio.Static.VolumeHud = Config.GameVolume;
						MyAudio.Static.VolumeVoiceChat = Config.VoiceChatVolume;
						hasFocus = true;
					}
				}
			}
			foreach (IPlugin plugin in MyPlugins.Plugins)
			{
				plugin.Update();
			}
			base.Update();
			MyGameStats.Static.Update();
			if (MySpaceAnalytics.Instance != null && MySession.Static != null && MySession.Static.Ready)
			{
				MySpaceAnalytics.Instance.Update(base.TotalTime);
			}
			if (m_unpauseInput && (DateTime.Now - Static.m_inputPauseTime).TotalMilliseconds >= 10.0)
			{
				Static.m_unpauseInput = false;
				Static.PauseInput = false;
			}
			if (MyMultiplayer.ReplicationLayer != null)
			{
				MyMultiplayer.ReplicationLayer.AdvanceSyncTime();
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null)
			{
				CheckAutoUpdateForDedicatedServer();
				CheckAutoRestartForDedicatedServer();
			}
			if (MyFakes.ENABLE_TESTING_TOOL_HEPER)
			{
				MyTestingToolHelper.Instance.Update();
			}
			MyStatsGraph.Commit();
		}

		private void CheckAutoRestartForDedicatedServer()
		{
			if (((!ConfigDedicated.AutoRestartEnabled || ConfigDedicated.AutoRestatTimeInMin <= 0) && !IsGoingToUpdate) || TotalTimeInMilliseconds <= m_lastRestartCheckInMilis)
			{
				return;
			}
			m_lastRestartCheckInMilis = TotalTimeInMilliseconds + 60000;
			int num = int.MaxValue;
			if (ConfigDedicated.AutoRestartEnabled && ConfigDedicated.AutoRestatTimeInMin > 0)
			{
				num = Math.Min(num, ConfigDedicated.AutoRestatTimeInMin);
			}
			if (IsGoingToUpdate)
			{
				num = Math.Min(num, m_autoUpdateRestartTimeInMin);
			}
			switch (AutoRestartState)
			{
			case EnumAutorestartStage.NotWarned:
				if (TotalTimeInMilliseconds >= (num - 10) * 60000)
				{
					AutoRestartWarning(10);
					m_autoRestartState = EnumAutorestartStage.Warned_10Min;
				}
				break;
			case EnumAutorestartStage.Warned_10Min:
				if (TotalTimeInMilliseconds >= (num - 5) * 60000)
				{
					AutoRestartWarning(5);
					m_autoRestartState = EnumAutorestartStage.Warned_5Min;
				}
				break;
			case EnumAutorestartStage.Warned_5Min:
				if (TotalTimeInMilliseconds >= (num - 1) * 60000)
				{
					AutoRestartWarning(1);
					m_autoRestartState = EnumAutorestartStage.Warned_1Min;
				}
				break;
			case EnumAutorestartStage.Warned_1Min:
				if (TotalTimeInMilliseconds >= num * 60000)
				{
					m_autoRestartState = EnumAutorestartStage.Restarting;
					m_lastRestartCheckInMilis = TotalTimeInMilliseconds;
				}
				break;
			case EnumAutorestartStage.Restarting:
				if (ConfigDedicated.AutoRestatTimeInMin > 60)
				{
					MyLog.Default.WriteLineAndConsole($"Automatic stop after {num / 60} hours and {num % 60} minutes");
				}
				else
				{
					MyLog.Default.WriteLineAndConsole($"Automatic stop after {num} minutes");
				}
				if (MySession.Static != null)
				{
					MySession.Static.SetSaveOnUnloadOverride_Dedicated(ConfigDedicated.AutoRestartSave);
				}
				ExitThreadSafe();
				break;
			case EnumAutorestartStage.NoRestart:
			{
				int num2 = num - TotalTimeInMilliseconds / 60000;
				if (num2 > 10)
				{
					m_autoRestartState = EnumAutorestartStage.NotWarned;
				}
				else if (num2 > 5)
				{
					m_autoRestartState = EnumAutorestartStage.Warned_10Min;
				}
				else if (num2 > 1)
				{
					m_autoRestartState = EnumAutorestartStage.Warned_5Min;
				}
				else
				{
					if (num2 <= 0)
					{
						break;
					}
					m_autoRestartState = EnumAutorestartStage.Warned_1Min;
				}
				if (IsGoingToUpdate)
				{
					AutoUpdateWarning(num2);
				}
				else
				{
					AutoRestartWarning(num2);
				}
				break;
			}
			}
		}

		private void CheckAutoUpdateForDedicatedServer()
		{
			if (ConfigDedicated.AutoUpdateEnabled && !IsGoingToUpdate && (DateTime.UtcNow - m_lastUpdateCheckTime).TotalMinutes >= (double)ConfigDedicated.AutoUpdateCheckIntervalInMin)
			{
				m_lastUpdateCheckTime = DateTime.UtcNow;
				System.Threading.Tasks.Task.Run(delegate
				{
					DownloadChangelog();
				}).ContinueWith(delegate
				{
					DownloadChangelogCompleted();
				});
				if (MyGameService.IsUpdateAvailable())
				{
					StartAutoUpdateCountdown();
				}
			}
		}

		private void DownloadChangelogCompleted()
		{
			if (m_changelog != null && m_changelog.Entry.Count > 0 && int.TryParse(m_changelog.Entry[0].Version, out int result) && result > (int)MyFinalBuildConstants.APP_VERSION)
			{
				StartAutoUpdateCountdown();
			}
		}

		private void DownloadChangelog()
		{
			if (m_changelogSerializer == null)
			{
				m_changelogSerializer = new XmlSerializer(typeof(MyNews));
			}
			try
			{
				if (MyVRage.Platform.Http.SendRequest(MyPerGameSettings.ChangeLogUrl, null, HttpMethod.GET, out string content) == HttpStatusCode.OK)
				{
					using (StringReader textReader = new StringReader(content))
					{
						m_changelog = (MyNews)m_changelogSerializer.Deserialize(textReader);
					}
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Error while downloading changelog: " + ex.ToString());
			}
		}

		private void StartAutoUpdateCountdown()
		{
			m_isGoingToUpdate = true;
			m_autoUpdateRestartTimeInMin = TotalTimeInMilliseconds / 60000 + ConfigDedicated.AutoUpdateRestartDelayInMin;
		}

		public static void WriteConsoleOutputs()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated && IsAutoRestarting)
			{
				MyLog.Default.WriteLineAndConsole("AUTORESTART");
			}
		}

		public static void AutoRestartWarning(int time)
		{
			MyLog.Default.WriteLineAndConsole(string.Format("Server will restart in {0} minute{1}", time, (time == 1) ? "" : "s"));
			MyMultiplayer.Static.SendChatMessage(string.Format(MyTexts.GetString(MyCommonTexts.Server_Restart_Warning), time, (time == 1) ? "" : "s"), ChatChannel.Global, 0L);
		}

		public static void AutoUpdateWarning(int time)
		{
			MyLog.Default.WriteLineAndConsole(string.Format("New version available. Server will restart in {0} minute{1}", time, (time == 1) ? "" : "s"));
			MyMultiplayer.Static.SendChatMessage(string.Format(MyTexts.GetString(MyCommonTexts.Server_Update_Warning), time, (time == 1) ? "" : "s"), ChatChannel.Global, 0L);
		}

		private void GetListenerVelocity(ref Vector3 velocity)
		{
			if (MySession.Static != null)
			{
				MySession.Static.ControlledEntity?.GetLinearVelocity(ref velocity, useRemoteControlVelocity: false);
			}
		}

		private void LogWriter(string text)
		{
			Log.WriteLine("Havok: " + text);
		}

		protected override void LoadData_UpdateThread()
		{
		}

		protected override void UnloadData_UpdateThread()
		{
			if (MySession.Static != null)
			{
				MySession.Static.Unload();
			}
			UnloadData();
			if (GameRenderComponent != null)
			{
				GameRenderComponent.Stop();
				GameRenderComponent.Dispose();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Havok memory statistics:");
			HkBaseSystem.GetMemoryStatistics(stringBuilder);
			MyLog.Default.WriteLine(stringBuilder.ToString());
			MyPhysicsDebugDraw.DebugGeometry.Dispose();
			ParallelTasks.Parallel.StartOnEachWorker(HkBaseSystem.QuitThread);
			if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
			{
				HkBaseSystem.Quit();
			}
			MySimpleProfiler.LogPerformanceTestResults();
		}

		protected override void PrepareForDraw()
		{
			using (Stats.Generic.Measure("GuiPrepareDraw"))
			{
				MyGuiSandbox.Draw();
			}
			using (Stats.Generic.Measure("DebugDraw"))
			{
				MyEntities.DebugDraw();
			}
			using (Stats.Generic.Measure("Hierarchy"))
			{
				if (MyGridPhysicalHierarchy.Static != null)
				{
					MyGridPhysicalHierarchy.Static.Draw();
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PARTICLES)
			{
				MyParticlesLibrary.DebugDraw();
			}
		}

		protected override void AfterDraw()
		{
			MyRenderProxy.AfterUpdate(base.TotalTime);
		}

		public void Invoke(Action action, string invokerName)
		{
			m_invokeQueue.Enqueue(new MyInvokeData
			{
				Action = action,
				Invoker = invokerName
			});
		}

		public void Invoke(string invokerName, object context, Action<object> action)
		{
			m_invokeQueue.Enqueue(new MyInvokeData
			{
				ContextualAction = action,
				Context = context,
				Invoker = invokerName
			});
		}

		public void ProcessInvoke()
		{
			MyUtils.Swap(ref m_invokeQueue, ref m_invokeQueueExecuting);
			MyInvokeData instance;
			while (m_invokeQueueExecuting.TryDequeue(out instance))
			{
				if (instance.Action != null)
				{
					instance.Action();
				}
				else
				{
					instance.ContextualAction(instance.Context);
				}
			}
			if (MyVRage.Platform.ImeProcessor != null)
			{
				MyVRage.Platform.ImeProcessor.ProcessInvoke();
			}
		}

		public void ClearInvokeQueue()
		{
			m_invokeQueue.Clear();
		}

		public void SetMouseVisible(bool visible)
		{
			if (!MyExternalAppBase.IsEditorActive)
			{
				IsCursorVisible = visible;
				MyVRage.Platform.Input.ShowCursor = visible;
			}
		}

		public static void ProcessRenderOutput()
		{
			MyRenderMessageBase obj;
			while (MyRenderProxy.OutputQueue.TryDequeue(out obj))
			{
				if (obj == null)
				{
					continue;
				}
				switch (obj.MessageType)
				{
				case MyRenderMessageEnum.Error:
				{
					MyRenderMessageError myRenderMessageError = (MyRenderMessageError)obj;
					ErrorConsumer.OnError("Renderer error", myRenderMessageError.Message, myRenderMessageError.Callstack);
					if (myRenderMessageError.ShouldTerminate)
					{
						ExitThreadSafe();
					}
					break;
				}
				case MyRenderMessageEnum.ScreenshotTaken:
				{
					if (MySession.Static == null)
					{
						break;
					}
					MyRenderMessageScreenshotTaken myRenderMessageScreenshotTaken = (MyRenderMessageScreenshotTaken)obj;
					if (myRenderMessageScreenshotTaken.ShowNotification)
					{
						MyHudNotification myHudNotification = new MyHudNotification(myRenderMessageScreenshotTaken.Success ? MyCommonTexts.ScreenshotSaved : MyCommonTexts.ScreenshotFailed, 2000);
						if (myRenderMessageScreenshotTaken.Success)
						{
							myHudNotification.SetTextFormatArguments(Path.GetFileName(myRenderMessageScreenshotTaken.Filename));
						}
						MyHud.Notifications.Add(myHudNotification);
					}
					if (Static != null && Static.OnScreenshotTaken != null)
					{
						Static.OnScreenshotTaken(Static, null);
					}
					MyGuiBlueprintScreen_Reworked.ScreenshotTaken(myRenderMessageScreenshotTaken.Success, myRenderMessageScreenshotTaken.Filename);
					break;
				}
				case MyRenderMessageEnum.ExportToObjComplete:
					_ = (MyRenderMessageExportToObjComplete)obj;
					break;
				case MyRenderMessageEnum.CreatedDeviceSettings:
					MyVideoSettingsManager.OnCreatedDeviceSettings((MyRenderMessageCreatedDeviceSettings)obj);
					break;
				case MyRenderMessageEnum.VideoAdaptersResponse:
				{
					MyRenderMessageVideoAdaptersResponse myRenderMessageVideoAdaptersResponse = (MyRenderMessageVideoAdaptersResponse)obj;
					MyVideoSettingsManager.OnVideoAdaptersResponse(myRenderMessageVideoAdaptersResponse);
					Static.CheckGraphicsCard(myRenderMessageVideoAdaptersResponse);
					bool firstTimeRun = Config.FirstTimeRun;
					if (firstTimeRun)
					{
						Config.FirstTimeRun = false;
						Config.ExperimentalMode = false;
						MyVideoSettingsManager.WriteCurrentSettingsToConfig();
						Config.Save();
					}
					if (firstTimeRun)
					{
						Config.FirstVTTimeRun = false;
					}
					if (Config.FirstVTTimeRun && !firstTimeRun && (MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality == MyRenderQualityEnum.HIGH || MyVideoSettingsManager.CurrentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality == MyRenderQualityEnum.EXTREME))
					{
						Config.SetToMediumQuality();
						Config.FirstVTTimeRun = false;
						Config.Save();
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: MyTexts.Get(MySpaceTexts.SwitchToNormalVT)));
					}
					if (MySpaceAnalytics.Instance != null)
					{
						string text = "";
						string text2 = "";
						Random random = new Random((int)MyGameService.UserId);
						byte[] array = new byte[16];
						random.NextBytes(array);
						text = new Guid(array).ToString();
						text2 = text;
						MySpaceAnalytics.Instance.StartSessionAndIdentifyPlayer(text, text2, firstTimeRun);
					}
					break;
				}
				case MyRenderMessageEnum.MainThreadCallback:
				{
					MyRenderMessageMainThreadCallback myRenderMessageMainThreadCallback = (MyRenderMessageMainThreadCallback)obj;
					if (myRenderMessageMainThreadCallback.Callback != null)
					{
						myRenderMessageMainThreadCallback.Callback();
					}
					myRenderMessageMainThreadCallback.Callback = null;
					break;
				}
				case MyRenderMessageEnum.ClipmapsReady:
					AreClipmapsReady = true;
					break;
				case MyRenderMessageEnum.TasksFinished:
					RenderTasksFinished = true;
					break;
				}
				obj.Dispose();
			}
		}

		public static void ExitThreadSafe()
		{
			Static.Invoke(delegate
			{
				Static.Exit();
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					Static.form.Hide();
				}
				ParallelTasks.Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromSeconds(10.0));
			}, "MySandboxGame::Exit");
			if (!Sandbox.Engine.Platform.Game.IsDedicated && !string.IsNullOrEmpty(MyFakes.EXIT_URL))
			{
				MyGuiSandbox.OpenUrl(MyFakes.EXIT_URL, UrlOpenMode.ExternalBrowser);
				MyFakes.EXIT_URL = "";
			}
		}

		public void Dispose()
		{
			if (MySessionComponentExtDebug.Static != null)
			{
				MySessionComponentExtDebug.Static.Dispose();
				MySessionComponentExtDebug.Static = null;
			}
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.Dispose();
			}
			if (GameRenderComponent != null)
			{
				GameRenderComponent.Dispose();
				GameRenderComponent = null;
			}
			MyPlugins.Unload();
			ParallelTasks.Parallel.Scheduler.WaitForTasksToFinish(TimeSpan.FromSeconds(10.0));
			m_windowCreatedEvent.Dispose();
			if (MyScriptCompiler.Static.Whitelist != null)
			{
				MyScriptCompiler.Static.Whitelist.Clear();
			}
			MyObjectBuilderType.UnregisterAssemblies();
			MyObjectBuilderSerializer.UnregisterAssembliesAndSerializers();
		}

		private void UpdateDamageEffectsInScene()
		{
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyCubeGrid myCubeGrid = entity as MyCubeGrid;
				if (myCubeGrid != null && myCubeGrid.Projector == null)
				{
					foreach (MySlimBlock block in myCubeGrid.GetBlocks())
					{
						if (m_enableDamageEffects)
						{
							block.ResumeDamageEffect();
						}
						else if (block.FatBlock != null)
						{
							block.FatBlock.StopDamageEffect(stopSound: false);
						}
					}
				}
			}
		}

		public static void ReloadDedicatedServerSession()
		{
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyLog.Default.WriteLineAndConsole("Reloading dedicated server");
				IsReloading = true;
				Static.Exit();
			}
		}

		internal void UpdateMouseCapture()
		{
			MyVRage.Platform.Input.MouseCapture = (Config.CaptureMouse && Config.WindowMode != MyWindowModeEnum.Fullscreen);
		}

		private void OverlayActivated(bool isActive)
		{
			if (isActive)
			{
				if (!Sync.MultiplayerActive)
				{
					PausePush();
				}
				Static.PauseInput = true;
				Static.m_unpauseInput = false;
			}
			else
			{
				if (!Sync.MultiplayerActive)
				{
					PausePop();
				}
				Static.m_unpauseInput = true;
				Static.m_inputPauseTime = DateTime.Now;
			}
		}
	}
}
