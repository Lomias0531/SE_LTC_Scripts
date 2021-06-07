using Sandbox.Definitions;
using Sandbox.Definitions.GUI;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, Priority = 10)]
	public class MyHud : MySessionComponentBase
	{
		private static readonly MyStringHash m_defaultDefinitionId = MyStringHash.GetOrCompute("Default");

		public static readonly StringBuilder Empty = new StringBuilder();

		private static MyHud m_Static;

		private static int m_rotatingWheelVisibleCounter;

		private static bool m_buildMode = false;

		private static MyHudDefinition m_definition;

		public static MyHudScreenEffects ScreenEffects = new MyHudScreenEffects();

		public static MyHudVoiceChat VoiceChat = new MyHudVoiceChat();

		public static MyHudSelectedObject SelectedObjectHighlight = new MyHudSelectedObject();

		public static MyHudBlockInfo BlockInfo = new MyHudBlockInfo();

		public static MyHudGravityIndicator GravityIndicator = new MyHudGravityIndicator();

		public static MyHudOreMarkers OreMarkers = new MyHudOreMarkers();

		public static MyHudLargeTurretTargets LargeTurretTargets = new MyHudLargeTurretTargets();

		public static MyHudQuestlog Questlog = new MyHudQuestlog();

		public static MyHudLocationMarkers LocationMarkers = new MyHudLocationMarkers();

		public static MyHudNotifications Notifications;

		public static MyHudGpsMarkers GpsMarkers = new MyHudGpsMarkers();

		private static int m_hudState;

		private readonly MyHudCrosshair m_Crosshair = new MyHudCrosshair();

		private readonly MyHudShipInfo m_ShipInfo = new MyHudShipInfo();

		private readonly MyHudScenarioInfo m_ScenarioInfo = new MyHudScenarioInfo();

		private readonly MyHudSinkGroupInfo m_SinkGroupInfo = new MyHudSinkGroupInfo();

		private readonly MyHudGpsMarkers m_ButtonPanelMarkers = new MyHudGpsMarkers();

		private readonly MyHudChat m_Chat = new MyHudChat();

		private readonly MyHudWorldBorderChecker m_WorldBorderChecker = new MyHudWorldBorderChecker();

		private readonly MyHudHackingMarkers m_HackingMarkers = new MyHudHackingMarkers();

		private readonly MyHudCameraInfo m_CameraInfo = new MyHudCameraInfo();

		private readonly MyHudObjectiveLine m_ObjectiveLine = new MyHudObjectiveLine();

		private readonly MyHudChangedInventoryItems m_ChangedInventoryItems = new MyHudChangedInventoryItems();

		private readonly MyHudText m_BlocksLeft = new MyHudText();

		private MyHudStatManager m_Stats = new MyHudStatManager();

		public static MyHudCrosshair Crosshair => Static.m_Crosshair;

		public static MyHudShipInfo ShipInfo => Static.m_ShipInfo;

		public static MyHudScenarioInfo ScenarioInfo => Static.m_ScenarioInfo;

		public static MyHudSinkGroupInfo SinkGroupInfo => Static.m_SinkGroupInfo;

		public static MyHudGpsMarkers ButtonPanelMarkers => Static.m_ButtonPanelMarkers;

		public static MyHudChat Chat => Static.m_Chat;

		public static MyHudWorldBorderChecker WorldBorderChecker => Static.m_WorldBorderChecker;

		public static MyHudHackingMarkers HackingMarkers => Static.m_HackingMarkers;

		public static MyHudCameraInfo CameraInfo => Static.m_CameraInfo;

		public static MyHudObjectiveLine ObjectiveLine => Static.m_ObjectiveLine;

		public static MyHudChangedInventoryItems ChangedInventoryItems => Static.m_ChangedInventoryItems;

		public static MyHudText BlocksLeft => Static.m_BlocksLeft;

		public static MyHudStatManager Stats => Static.m_Stats;

		private static MyHud Static => m_Static;

		public static MyHudDefinition HudDefinition
		{
			get
			{
				if (m_definition == null)
				{
					m_definition = MyDefinitionManagerBase.Static.GetDefinition<MyHudDefinition>(m_defaultDefinitionId);
				}
				return m_definition;
			}
		}

		public static float HudElementsScaleMultiplier
		{
			get
			{
				float num = (float)m_definition.OptimalScreenRatio.Value.X / (float)m_definition.OptimalScreenRatio.Value.Y;
				return MyMath.Clamp((float)MySandboxGame.ScreenSize.X / (float)MySandboxGame.ScreenSize.Y / num, 0f, 1f);
			}
		}

		public static bool RotatingWheelVisible => m_rotatingWheelVisibleCounter > 0;

		public static StringBuilder RotatingWheelText
		{
			get;
			set;
		}

		public static int HudState
		{
			get
			{
				return m_hudState;
			}
			set
			{
				if (m_hudState != value)
				{
					m_hudState = value;
					MySandboxGame.Config.HudState = value;
				}
			}
		}

		public static bool IsHudMinimal => m_hudState == 0;

		public static bool MinimalHud
		{
			get;
			set;
		}

		public static bool IsVisible
		{
			get;
			set;
		}

		public static bool CutsceneHud
		{
			get;
			set;
		}

		public static bool IsBuildMode
		{
			get
			{
				return m_buildMode;
			}
			set
			{
				m_buildMode = value;
			}
		}

		public static void SetHudState(int state)
		{
			HudState = state % 3;
			MinimalHud = IsHudMinimal;
		}

		public static void ToggleGamepadHud()
		{
			if (IsHudMinimal)
			{
				SetHudState(1);
			}
			else
			{
				SetHudState(0);
			}
		}

		public static void SetHudDefinition(string definition)
		{
			MyHudDefinition myHudDefinition = null;
			if (!string.IsNullOrEmpty(definition))
			{
				myHudDefinition = MyDefinitionManager.Static.GetDefinition<MyHudDefinition>(MyStringHash.GetOrCompute(definition));
			}
			if (myHudDefinition == null)
			{
				myHudDefinition = MyDefinitionManager.Static.GetDefinition<MyHudDefinition>(MyStringHash.GetOrCompute("Default"));
			}
			if (HudDefinition != myHudDefinition)
			{
				m_definition = myHudDefinition;
				if (MyGuiScreenHudSpace.Static != null && m_definition != null)
				{
					MyGuiScreenHudSpace.Static.RecreateControls(constructor: false);
				}
			}
		}

		public static bool CheckShowPlayerNamesOnHud()
		{
			return MySession.Static.ShowPlayerNamesOnHud;
		}

		public static void ReloadTexts()
		{
			Notifications.ReloadTexts();
			ShipInfo.Reload();
			SinkGroupInfo.Reload();
			ScenarioInfo.Reload();
		}

		public static void PushRotatingWheelVisible()
		{
			m_rotatingWheelVisibleCounter++;
		}

		public static void PopRotatingWheelVisible()
		{
			m_rotatingWheelVisibleCounter--;
		}

		internal static void HideAll()
		{
			Crosshair.HideDefaultSprite();
			ShipInfo.Hide();
			BlockInfo.Visible = false;
			GravityIndicator.Hide();
			SinkGroupInfo.Visible = false;
			LargeTurretTargets.Visible = false;
		}

		public override void LoadData()
		{
			m_Static = this;
			base.LoadData();
			Notifications = new MyHudNotifications();
			m_Stats = new MyHudStatManager();
			HudState = MySandboxGame.Config.HudState;
			m_Chat.RegisterChat(MyMultiplayer.Static);
		}

		public override void BeforeStart()
		{
			Questlog.Init();
		}

		public override void SaveData()
		{
			if (MyCampaignManager.Static != null && MyCampaignManager.Static.IsCampaignRunning)
			{
				Questlog.Save();
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Notifications.Clear();
			OreMarkers.Clear();
			LocationMarkers.Clear();
			GpsMarkers.Clear();
			m_HackingMarkers.Clear();
			m_ObjectiveLine.Clear();
			m_ChangedInventoryItems.Clear();
			GravityIndicator.Clean();
			SelectedObjectHighlight.Clean();
			MyGuiScreenToolbarConfigBase.Reset();
			m_Stats = null;
			m_Chat.UnregisterChat(MyMultiplayer.Static);
			m_Static = null;
			IsVisible = false;
		}

		public override void UpdateBeforeSimulation()
		{
			IsVisible = (MySession.Static.LocalCharacter != null && !MySession.Static.LocalCharacter.IsDead);
			Notifications.UpdateBeforeSimulation();
			m_Chat.Update();
			m_WorldBorderChecker.Update();
			ScreenEffects.Update();
			m_Stats.Update();
			base.UpdateBeforeSimulation();
		}
	}
}
