#define VRAGE
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Input;
using VRage.Utils;
using VRageRender.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MyToolbarComponent : MySessionComponentBase
	{
		private static readonly MyStringId[] m_slotControls = new MyStringId[10]
		{
			MyControlsSpace.SLOT1,
			MyControlsSpace.SLOT2,
			MyControlsSpace.SLOT3,
			MyControlsSpace.SLOT4,
			MyControlsSpace.SLOT5,
			MyControlsSpace.SLOT6,
			MyControlsSpace.SLOT7,
			MyControlsSpace.SLOT8,
			MyControlsSpace.SLOT9,
			MyControlsSpace.SLOT0
		};

		private static MyToolbarComponent m_instance;

		private MyToolbar m_currentToolbar;

		private MyToolbar m_universalCharacterToolbar;

		private bool m_toolbarControlIsShown;

		private static StringBuilder m_slotControlTextCache = new StringBuilder();

		public static bool IsToolbarControlShown
		{
			get
			{
				if (m_instance == null)
				{
					return false;
				}
				return m_instance.m_toolbarControlIsShown;
			}
			set
			{
				if (m_instance != null)
				{
					m_instance.m_toolbarControlIsShown = value;
				}
			}
		}

		public static MyToolbar CurrentToolbar
		{
			get
			{
				if (m_instance == null)
				{
					return null;
				}
				return m_instance.m_currentToolbar;
			}
			set
			{
				if (m_instance.m_currentToolbar != value)
				{
					m_instance.m_currentToolbar = value;
					if (MyToolbarComponent.CurrentToolbarChanged != null)
					{
						MyToolbarComponent.CurrentToolbarChanged();
					}
				}
			}
		}

		public static MyToolbar CharacterToolbar
		{
			get
			{
				if (m_instance == null)
				{
					return null;
				}
				return m_instance.m_universalCharacterToolbar;
			}
		}

		public static bool GlobalBuilding
		{
			get
			{
				_ = Sandbox.Engine.Platform.Game.IsDedicated;
				if (MySession.Static.IsCameraUserControlledSpectator())
				{
					return MyInput.Static.ENABLE_DEVELOPER_KEYS;
				}
				return false;
			}
		}

		public static bool CreativeModeEnabled
		{
			get
			{
				if (!MyFakes.UNLIMITED_CHARACTER_BUILDING)
				{
					return MySession.Static.CreativeMode;
				}
				return true;
			}
		}

		public static bool AutoUpdate
		{
			get;
			set;
		}

		public static event Action CurrentToolbarChanged;

		public static void UpdateCurrentToolbar()
		{
			if (AutoUpdate && MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Toolbar != null && m_instance.m_currentToolbar != MySession.Static.ControlledEntity.Toolbar)
			{
				m_instance.m_currentToolbar = MySession.Static.ControlledEntity.Toolbar;
				m_instance.m_currentToolbar.ShareToolbarItems();
				if (MyToolbarComponent.CurrentToolbarChanged != null)
				{
					MyToolbarComponent.CurrentToolbarChanged();
				}
			}
		}

		public MyToolbarComponent()
		{
			m_universalCharacterToolbar = new MyToolbar(MyToolbarType.Character);
			m_currentToolbar = m_universalCharacterToolbar;
			AutoUpdate = true;
		}

		public override void LoadData()
		{
			m_instance = this;
			base.LoadData();
		}

		public override void HandleInput()
		{
			try
			{
				MyStringId context = (MySession.Static.ControlledEntity != null) ? MySession.Static.ControlledEntity.ControlContext : MyStringId.NullOrEmpty;
				MyGuiScreenBase screenWithFocus = MyScreenManager.GetScreenWithFocus();
				if ((screenWithFocus == MyGuiScreenGamePlay.Static || IsToolbarControlShown) && CurrentToolbar != null && !MyGuiScreenGamePlay.DisableInput)
				{
					for (int i = 0; i < m_slotControls.Length; i++)
					{
						if (MyControllerHelper.IsControl(context, m_slotControls[i]))
						{
							if (!MyInput.Static.IsAnyCtrlKeyPressed())
							{
								if ((screenWithFocus is MyGuiScreenScriptingTools || screenWithFocus == MyGuiScreenGamePlay.Static || ((screenWithFocus is MyGuiScreenCubeBuilder || screenWithFocus is MyGuiScreenToolbarConfigBase) && ((MyGuiScreenToolbarConfigBase)screenWithFocus).AllowToolbarKeys())) && CurrentToolbar != null)
								{
									CurrentToolbar.ActivateItemAtSlot(i);
								}
							}
							else if (i < CurrentToolbar.PageCount)
							{
								MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
								CurrentToolbar.SwitchToPage(i);
								MySession.Static.ToolbarPageSwitches++;
								if (MySpaceAnalytics.Instance != null)
								{
									MySpaceAnalytics.Instance.ReportToolbarSwitch(CurrentToolbar.CurrentPage);
								}
							}
						}
					}
					if ((screenWithFocus == MyGuiScreenGamePlay.Static || ((screenWithFocus is MyGuiScreenCubeBuilder || screenWithFocus is MyGuiScreenToolbarConfigBase) && ((MyGuiScreenToolbarConfigBase)screenWithFocus).AllowToolbarKeys())) && CurrentToolbar != null)
					{
						if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_NEXT_ITEM))
						{
							CurrentToolbar.SelectNextSlot();
						}
						else if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_PREV_ITEM))
						{
							CurrentToolbar.SelectPreviousSlot();
						}
						if (MySpectator.Static.SpectatorCameraMovement != MySpectatorCameraMovementEnum.ConstantDelta)
						{
							if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_UP))
							{
								MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
								CurrentToolbar.PageUp();
								MySession.Static.ToolbarPageSwitches++;
								if (MySpaceAnalytics.Instance != null)
								{
									MySpaceAnalytics.Instance.ReportToolbarSwitch(CurrentToolbar.CurrentPage);
								}
							}
							if (MyControllerHelper.IsControl(context, MyControlsSpace.TOOLBAR_DOWN))
							{
								MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
								CurrentToolbar.PageDown();
								MySession.Static.ToolbarPageSwitches++;
								if (MySpaceAnalytics.Instance != null)
								{
									MySpaceAnalytics.Instance.ReportToolbarSwitch(CurrentToolbar.CurrentPage);
								}
							}
						}
					}
				}
			}
			finally
			{
			}
			base.HandleInput();
		}

		public override void UpdateBeforeSimulation()
		{
			try
			{
				using (Stats.Generic.Measure("Toolbar.Update()"))
				{
					UpdateCurrentToolbar();
					if (CurrentToolbar != null)
					{
						CurrentToolbar.Update();
					}
				}
			}
			finally
			{
			}
			base.UpdateBeforeSimulation();
		}

		protected override void UnloadData()
		{
			m_instance = null;
			base.UnloadData();
		}

		private static MyToolbar GetToolbar()
		{
			return m_instance.m_currentToolbar;
		}

		public static void InitCharacterToolbar(MyObjectBuilder_Toolbar characterToolbar)
		{
			m_instance.m_universalCharacterToolbar.Init(characterToolbar, null, skipAssert: true);
		}

		public static void InitToolbar(MyToolbarType type, MyObjectBuilder_Toolbar builder)
		{
			if (builder != null && builder.ToolbarType != type)
			{
				builder.ToolbarType = type;
			}
			m_instance.m_currentToolbar.Init(builder, null, skipAssert: true);
		}

		public static MyObjectBuilder_Toolbar GetObjectBuilder(MyToolbarType type)
		{
			MyObjectBuilder_Toolbar objectBuilder = m_instance.m_currentToolbar.GetObjectBuilder();
			objectBuilder.ToolbarType = type;
			return objectBuilder;
		}

		public static StringBuilder GetSlotControlText(int slotIndex)
		{
			if (!m_slotControls.IsValidIndex(slotIndex))
			{
				return null;
			}
			m_slotControlTextCache.Clear();
			MyInput.Static.GetGameControl(m_slotControls[slotIndex]).AppendBoundKeyJustOne(ref m_slotControlTextCache);
			return m_slotControlTextCache;
		}

		private MyToolbarType GetCurrentToolbarType()
		{
			if (MyBlockBuilderBase.SpectatorIsBuilding)
			{
				return MyToolbarType.Spectator;
			}
			if (MySession.Static.ControlledEntity == null)
			{
				return MyToolbarType.Spectator;
			}
			return MySession.Static.ControlledEntity.ToolbarType;
		}
	}
}
