#define VRAGE
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Replication;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenTerminal : MyGuiScreenBase
	{
		private MyGuiControlTabControl m_terminalTabs;

		private MyGuiControlParent m_propertiesTopMenuParent;

		private MyGuiControlParent m_propertiesTableParent;

		private MyTerminalControlPanel m_controllerControlPanel;

		private MyTerminalInventoryController m_controllerInventory;

		private MyTerminalProductionController m_controllerProduction;

		private MyTerminalInfoController m_controllerInfo;

		private MyTerminalFactionController m_controllerFactions;

		private MyTerminalPropertiesController m_controllerProperties;

		private MyTerminalChatController m_controllerChat;

		private MyTerminalGpsController m_controllerGps;

		private MyGridColorHelper m_colorHelper;

		private MyGuiControlLabel m_terminalNotConnected;

		private bool m_autoSelectControlSearch = true;

		private MyCharacter m_user;

		private static MyEntity m_interactedEntity;

		private static MyEntity m_openInventoryInteractedEntity;

		private MyTerminalPageEnum m_initialPage;

		private Dictionary<long, Action<long>> m_requestedEntities = new Dictionary<long, Action<long>>();

		private static Action<MyEntity> m_closeHandler;

		private Dictionary<MyTerminalPageEnum, MyGuiControlBase> m_defaultFocusedControl = new Dictionary<MyTerminalPageEnum, MyGuiControlBase>();

		private Dictionary<MyTerminalPageEnum, MyTerminalController> m_terminalControllers = new Dictionary<MyTerminalPageEnum, MyTerminalController>();

		public static bool IsRemote;

		private static bool m_screenOpen;

		private bool m_connected = true;

		private static MyGuiScreenTerminal m_instance;

		public static MyEntity InteractedEntity
		{
			get
			{
				return m_interactedEntity;
			}
			set
			{
				if (m_interactedEntity != null)
				{
					m_interactedEntity.OnClose -= m_closeHandler;
				}
				if (m_instance.m_controllerControlPanel != null)
				{
					m_instance.m_controllerControlPanel.ClearBlockList();
				}
				m_interactedEntity = value;
				if (m_interactedEntity != null)
				{
					m_interactedEntity.OnClose += m_closeHandler;
					if (m_interactedEntity != m_openInventoryInteractedEntity)
					{
						m_instance.m_initialPage = MyTerminalPageEnum.ControlPanel;
					}
				}
				if (m_screenOpen)
				{
					m_instance.RecreateTabs();
				}
			}
		}

		internal static bool IsOpen => m_screenOpen;

		public static event ScreenHandler ClosedCallback
		{
			add
			{
				m_instance.Closed += value;
			}
			remove
			{
				m_instance.Closed -= value;
			}
		}

		private MyGuiScreenTerminal()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(1.0157f, 0.9172f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			m_closeHandler = OnInteractedClose;
			m_colorHelper = new MyGridColorHelper();
		}

		private void OnInteractedClose(MyEntity entity)
		{
			if (m_interactedEntity != null)
			{
				m_interactedEntity.OnClose -= m_closeHandler;
			}
			Hide();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTerminal";
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override bool CloseScreen()
		{
			if (base.CloseScreen())
			{
				if (m_interactedEntity != null)
				{
					m_interactedEntity.OnClose -= m_closeHandler;
				}
				return true;
			}
			return false;
		}

		private void CreateFixedTerminalElements()
		{
			m_terminalNotConnected = CreateErrorLabel(MySpaceTexts.ScreenTerminalError_ShipHasBeenDisconnected, "DisconnectedMessage");
			m_terminalNotConnected.Visible = false;
			Controls.Add(m_terminalNotConnected);
			AddCaption(MySpaceTexts.Terminal, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.447f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.894f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.447f, m_size.Value.Y / 2f - 0.1435f), m_size.Value.X * 0.894f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.447f, (0f - m_size.Value.Y) / 2f + 0.048f), m_size.Value.X * 0.894f);
			Controls.Add(myGuiControlSeparatorList);
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				m_propertiesTopMenuParent = new MyGuiControlParent
				{
					Position = new Vector2(-0.855f, -0.514f),
					Size = new Vector2(0.8f, 0.15f),
					Name = "PropertiesPanel",
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
				};
				m_propertiesTableParent = new MyGuiControlParent
				{
					Position = new Vector2(-0.02f, -0.37f),
					Size = new Vector2(0.93f, 0.78f),
					Name = "PropertiesTable",
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
				};
				CreatePropertiesPageControls(m_propertiesTopMenuParent, m_propertiesTableParent);
				if (m_controllerProperties == null)
				{
					m_controllerProperties = new MyTerminalPropertiesController();
				}
				else
				{
					m_controllerProperties.Close();
				}
				m_controllerProperties.ButtonClicked += PropertiesButtonClicked;
				Controls.Add(m_propertiesTableParent);
				Controls.Add(m_propertiesTopMenuParent);
			}
		}

		private void CreateTabs()
		{
			m_terminalTabs = new MyGuiControlTabControl
			{
				Position = new Vector2(-0.001f, -0.367f),
				Size = new Vector2(0.907f, 0.78f),
				Name = "TerminalTabs",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			if (MyFakes.ENABLE_COMMUNICATION)
			{
				m_terminalTabs.TabButtonScale = 0.875f;
			}
			MyGuiControlTabPage tabSubControl = m_terminalTabs.GetTabSubControl(0);
			MyGuiControlTabPage tabSubControl2 = m_terminalTabs.GetTabSubControl(1);
			MyGuiControlTabPage tabSubControl3 = m_terminalTabs.GetTabSubControl(2);
			MyGuiControlTabPage tabSubControl4 = m_terminalTabs.GetTabSubControl(3);
			MyGuiControlTabPage tabSubControl5 = m_terminalTabs.GetTabSubControl(4);
			MyGuiControlTabPage myGuiControlTabPage = null;
			if (MyFakes.ENABLE_COMMUNICATION)
			{
				myGuiControlTabPage = m_terminalTabs.GetTabSubControl(5);
			}
			MyGuiControlTabPage myGuiControlTabPage2 = null;
			if (MyFakes.ENABLE_GPS)
			{
				myGuiControlTabPage2 = m_terminalTabs.GetTabSubControl(6);
				m_terminalTabs.TabButtonScale = 0.75f;
			}
			CreateInventoryPageControls(tabSubControl);
			CreateControlPanelPageControls(tabSubControl2);
			CreateProductionPageControls(tabSubControl3);
			CreateInfoPageControls(tabSubControl4);
			CreateFactionsPageControls(tabSubControl5);
			if (MyFakes.ENABLE_GPS)
			{
				CreateGpsPageControls(myGuiControlTabPage2);
			}
			if (MyFakes.ENABLE_COMMUNICATION)
			{
				CreateChatPageControls(myGuiControlTabPage);
			}
			MyCubeGrid myCubeGrid = (InteractedEntity != null) ? (InteractedEntity.Parent as MyCubeGrid) : null;
			m_colorHelper.Init(myCubeGrid);
			if (m_controllerInventory == null)
			{
				m_controllerInventory = new MyTerminalInventoryController();
				m_terminalControllers.Add(MyTerminalPageEnum.Inventory, m_controllerInventory);
			}
			else
			{
				m_controllerInventory.Close();
			}
			if (m_controllerControlPanel == null)
			{
				m_controllerControlPanel = new MyTerminalControlPanel();
				m_terminalControllers.Add(MyTerminalPageEnum.ControlPanel, m_controllerControlPanel);
			}
			else
			{
				m_controllerControlPanel.Close();
			}
			if (m_controllerProduction == null)
			{
				m_controllerProduction = new MyTerminalProductionController();
				m_terminalControllers.Add(MyTerminalPageEnum.Production, m_controllerProduction);
			}
			else
			{
				m_controllerProduction.Close();
			}
			if (m_controllerInfo == null)
			{
				m_controllerInfo = new MyTerminalInfoController();
				m_terminalControllers.Add(MyTerminalPageEnum.Info, m_controllerInfo);
			}
			else
			{
				m_controllerInfo.Close();
			}
			if (m_controllerFactions == null)
			{
				m_controllerFactions = new MyTerminalFactionController();
				m_terminalControllers.Add(MyTerminalPageEnum.Factions, m_controllerFactions);
			}
			else
			{
				m_controllerFactions.Close();
			}
			if (MyFakes.ENABLE_GPS)
			{
				if (m_controllerGps == null)
				{
					m_controllerGps = new MyTerminalGpsController();
					m_terminalControllers.Add(MyTerminalPageEnum.Gps, m_controllerGps);
				}
				else
				{
					m_controllerGps.Close();
				}
			}
			if (MyFakes.ENABLE_COMMUNICATION)
			{
				if (m_controllerChat == null)
				{
					m_controllerChat = new MyTerminalChatController();
					m_terminalControllers.Add(MyTerminalPageEnum.Comms, m_controllerChat);
				}
				else
				{
					m_controllerChat.Close();
				}
			}
			m_controllerInventory.Init(tabSubControl, m_user, InteractedEntity, m_colorHelper);
			m_controllerControlPanel.Init(tabSubControl2, MySession.Static.LocalHumanPlayer, myCubeGrid, InteractedEntity as MyTerminalBlock, m_colorHelper);
			m_controllerProduction.Init(tabSubControl3, myCubeGrid, InteractedEntity as MyCubeBlock);
			m_controllerInfo.Init(tabSubControl4, (InteractedEntity != null) ? (InteractedEntity.Parent as MyCubeGrid) : null);
			m_controllerFactions.Init(tabSubControl5);
			if (MyFakes.ENABLE_GPS)
			{
				m_controllerGps.Init(myGuiControlTabPage2);
			}
			if (MyFakes.ENABLE_COMMUNICATION)
			{
				m_controllerChat.Init(myGuiControlTabPage);
			}
			m_terminalTabs.SelectedPage = (int)m_initialPage;
			if (m_terminalTabs.SelectedPage != -1 && !m_terminalTabs.GetTabSubControl(m_terminalTabs.SelectedPage).Enabled)
			{
				m_terminalTabs.SelectedPage = m_terminalTabs.Controls.IndexOf(tabSubControl2);
			}
			base.CloseButtonEnabled = true;
			SetDefaultCloseButtonOffset();
			Controls.Add(m_terminalTabs);
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				m_terminalTabs.OnPageChanged += tabs_OnPageChanged;
			}
		}

		private void CreateProperties()
		{
			if (m_controllerProperties == null)
			{
				m_controllerProperties = new MyTerminalPropertiesController();
			}
			else
			{
				m_controllerProperties.Close();
			}
			m_controllerProperties.Init(m_propertiesTopMenuParent, m_propertiesTableParent, InteractedEntity, m_openInventoryInteractedEntity, IsRemote);
			if (m_propertiesTableParent != null)
			{
				m_propertiesTableParent.Visible = (m_initialPage == MyTerminalPageEnum.Properties);
			}
		}

		private void RecreateTabs()
		{
			Controls.RemoveControlByName("TerminalTabs");
			CreateTabs();
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				CreateProperties();
			}
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			CreateFixedTerminalElements();
			CreateTabs();
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				CreateProperties();
			}
			tabs_OnPageChanged();
		}

		private void CreateInventoryPageControls(MyGuiControlTabPage page)
		{
			page.Name = "PageInventory";
			page.TextEnum = MySpaceTexts.Inventory;
			page.TextScale = 0.7005405f;
			float num = -0.008f;
			float num2 = 0.1254f;
			MyGuiControlRadioButton control = new MyGuiControlRadioButton
			{
				Position = new Vector2(-0.4565f + num, -0.338f),
				Size = new Vector2(0.056875f, 0.0575f),
				Name = "LeftSuitButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterCharacter
			};
			MyGuiControlRadioButton control2 = new MyGuiControlRadioButton
			{
				Position = new Vector2(-0.41f + num, -0.338f),
				Size = new Vector2(0.056875f, 0.0575f),
				Name = "LeftGridButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterGrid
			};
			MyGuiControlRadioButton control3 = new MyGuiControlRadioButton
			{
				Position = new Vector2(-0.3215f + num + num2, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "LeftFilterShipButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterShip
			};
			MyGuiControlRadioButton control4 = new MyGuiControlRadioButton
			{
				Position = new Vector2(-0.275f + num + num2, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "LeftFilterStorageButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterStorage
			};
			MyGuiControlRadioButton control5 = new MyGuiControlRadioButton
			{
				Position = new Vector2(-0.2285f + num + num2, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "LeftFilterSystemButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterSystem
			};
			MyGuiControlRadioButton control6 = new MyGuiControlRadioButton
			{
				Position = new Vector2(-0.182f + num + num2, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "LeftFilterEnergyButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterEnergy
			};
			MyGuiControlRadioButton control7 = new MyGuiControlRadioButton
			{
				Position = new Vector2(-0.1355f + num + num2, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "LeftFilterAllButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterAll
			};
			MyGuiControlCheckbox control8 = new MyGuiControlCheckbox
			{
				Position = new Vector2(-0.0075f + num, -0.255f),
				Name = "CheckboxHideEmptyLeft",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
			};
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
			{
				Position = new Vector2(-0.055f + num, -0.255f),
				Name = "LabelHideEmptyLeft",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				TextEnum = MySpaceTexts.HideEmpty
			};
			MyGuiControlSearchBox control9 = new MyGuiControlSearchBox(new Vector2(-0.452f + num, -0.26f), new Vector2(0.361f - myGuiControlLabel.Size.X, 0.052f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
			{
				Name = "BlockSearchLeft"
			};
			MyGuiControlList myGuiControlList = new MyGuiControlList
			{
				Position = new Vector2(-0.465f, -0.26f),
				Size = new Vector2(0.44f, 0.616f),
				Name = "LeftInventory",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			page.Controls.Add(control);
			page.Controls.Add(control2);
			page.Controls.Add(control3);
			page.Controls.Add(control4);
			page.Controls.Add(control5);
			page.Controls.Add(control6);
			page.Controls.Add(control7);
			page.Controls.Add(control9);
			page.Controls.Add(control8);
			page.Controls.Add(myGuiControlLabel);
			page.Controls.Add(myGuiControlList);
			num = 0.004f;
			MyGuiControlRadioButton control10 = new MyGuiControlRadioButton
			{
				Position = new Vector2(0.0145f + num, -0.338f),
				Size = new Vector2(0.056875f, 0.0575f),
				Name = "RightSuitButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterCharacter
			};
			MyGuiControlRadioButton control11 = new MyGuiControlRadioButton
			{
				Position = new Vector2(0.061f + num, -0.338f),
				Size = new Vector2(0.056875f, 0.0575f),
				Name = "RightGridButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterGrid
			};
			MyGuiControlRadioButton control12 = new MyGuiControlRadioButton
			{
				Position = new Vector2(0.275f + num, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "RightFilterShipButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterShip
			};
			MyGuiControlRadioButton control13 = new MyGuiControlRadioButton
			{
				Position = new Vector2(0.321f + num, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "RightFilterStorageButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterStorage
			};
			MyGuiControlRadioButton control14 = new MyGuiControlRadioButton
			{
				Position = new Vector2(0.3675f + num, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "RightFilterSystemButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterSystem
			};
			MyGuiControlRadioButton control15 = new MyGuiControlRadioButton
			{
				Position = new Vector2(0.414f + num, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "RightFilterEnergyButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterEnergy
			};
			MyGuiControlRadioButton control16 = new MyGuiControlRadioButton
			{
				Position = new Vector2(0.4605f + num, -0.338f),
				Size = new Vector2(0.045f, 17f / 300f),
				Name = "RightFilterAllButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Key = 0,
				VisualStyle = MyGuiControlRadioButtonStyleEnum.FilterAll
			};
			MyGuiControlCheckbox control17 = new MyGuiControlCheckbox
			{
				Position = new Vector2(0.463f + num, -0.255f),
				Name = "CheckboxHideEmptyRight",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
			};
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel
			{
				Position = new Vector2(0.415f + num, -0.255f),
				Name = "LabelHideEmptyRight",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				TextEnum = MySpaceTexts.HideEmpty
			};
			MyGuiControlSearchBox control18 = new MyGuiControlSearchBox(new Vector2(0.0185f + num, -0.26f), new Vector2(0.361f - myGuiControlLabel2.Size.X, 0.052f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
			{
				Name = "BlockSearchRight"
			};
			MyGuiControlList control19 = new MyGuiControlList
			{
				Position = new Vector2(0.465f, -0.295f),
				Size = new Vector2(0.44f, 0.65f),
				Name = "RightInventory",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP
			};
			page.Controls.Add(control10);
			page.Controls.Add(control11);
			page.Controls.Add(control12);
			page.Controls.Add(control13);
			page.Controls.Add(control14);
			page.Controls.Add(control15);
			page.Controls.Add(control16);
			page.Controls.Add(control18);
			page.Controls.Add(control17);
			page.Controls.Add(myGuiControlLabel2);
			page.Controls.Add(control19);
			float num3 = -0.225f;
			float num4 = 0.1f;
			MyGuiControlButton control20 = new MyGuiControlButton
			{
				Position = new Vector2(0f, num3),
				Size = new Vector2(0.044375f, 41f / 300f),
				Name = "ThrowOutButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
				TextEnum = MySpaceTexts.Afterburner,
				TextScale = 0f,
				TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				DrawCrossTextureWhenDisabled = true,
				VisualStyle = MyGuiControlButtonStyleEnum.InventoryTrash,
				ActivateOnMouseRelease = true
			};
			MyGuiControlButton control21 = new MyGuiControlButton
			{
				Position = new Vector2(0f, num3 += num4),
				Size = new Vector2(0.044375f, 41f / 300f),
				Name = "WithdrawButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
				TextEnum = MySpaceTexts.Afterburner,
				TextScale = 0f,
				TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				DrawCrossTextureWhenDisabled = true,
				VisualStyle = MyGuiControlButtonStyleEnum.Withdraw,
				ActivateOnMouseRelease = true
			};
			MyGuiControlButton control22 = new MyGuiControlButton
			{
				Position = new Vector2(0f, num3 += num4),
				Size = new Vector2(0.044375f, 41f / 300f),
				Name = "DepositAllButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
				TextEnum = MySpaceTexts.Afterburner,
				TextScale = 0f,
				TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				DrawCrossTextureWhenDisabled = true,
				VisualStyle = MyGuiControlButtonStyleEnum.DepositAll,
				ActivateOnMouseRelease = true
			};
			MyGuiControlButton control23 = new MyGuiControlButton
			{
				Position = new Vector2(0f, num3 += num4),
				Size = new Vector2(0.044375f, 41f / 300f),
				Name = "AddToProductionButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
				TextEnum = MySpaceTexts.Afterburner,
				TextScale = 0f,
				TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				DrawCrossTextureWhenDisabled = true,
				VisualStyle = MyGuiControlButtonStyleEnum.AddToProduction,
				ActivateOnMouseRelease = true
			};
			MyGuiControlButton control24 = new MyGuiControlButton
			{
				Position = new Vector2(0f, num3 += num4),
				Size = new Vector2(0.044375f, 41f / 300f),
				Name = "SelectedToProductionButton",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
				TextEnum = MySpaceTexts.Afterburner,
				TextScale = 0f,
				TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				DrawCrossTextureWhenDisabled = true,
				VisualStyle = MyGuiControlButtonStyleEnum.SelectedToProduction,
				ActivateOnMouseRelease = true
			};
			page.Controls.Add(control20);
			page.Controls.Add(control21);
			page.Controls.Add(control22);
			page.Controls.Add(control23);
			page.Controls.Add(control24);
			m_defaultFocusedControl[MyTerminalPageEnum.Inventory] = myGuiControlList;
		}

		private void CreateControlPanelPageControls(MyGuiControlTabPage page)
		{
			page.Name = "PageControlPanel";
			page.TextEnum = MySpaceTexts.ControlPanel;
			page.TextScale = 0.7005405f;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddVertical(new Vector2(0.145f, -0.333f), 0.676f, 0.002f);
			myGuiControlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 0.002f);
			page.Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSearchBox myGuiControlSearchBox = new MyGuiControlSearchBox(new Vector2(-0.452f, -0.342f), new Vector2(0.255f, 0.052f))
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Name = "FunctionalBlockSearch"
			};
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(-0.442f, -0.271f),
				Name = "ControlLabel",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.ControlScreen_GridBlocksLabel)
			};
			MyGuiControlPanel control2 = new MyGuiControlPanel(new Vector2(-0.452f, -0.276f), new Vector2(0.29f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			page.Controls.Add(control2);
			page.Controls.Add(control);
			MyGuiControlListbox control3 = new MyGuiControlListbox
			{
				Position = new Vector2(-0.452f, -0.2426f),
				Size = new Vector2(0.29f, 0.5f),
				Name = "FunctionalBlockListbox",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				VisualStyle = MyGuiControlListboxStyleEnum.ChatScreen,
				VisibleRowsCount = 17
			};
			new MyGuiControlCompositePanel
			{
				Position = new Vector2(-0.1525f, 0f),
				Size = new Vector2(0.615f, 0.7125f),
				Name = "Control",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				InnerHeight = 0.685f,
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK
			};
			new MyGuiControlPanel
			{
				Position = new Vector2(-0.1425f, -0.32f),
				Size = new Vector2(0.595f, 0.035f),
				Name = "SelectedBlockNamePanel",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK
			};
			MyGuiControlLabel control4 = new MyGuiControlLabel
			{
				Position = new Vector2(-0.1325f, -0.322f),
				Size = new Vector2(0.0470270254f, 0.0266666654f),
				Name = "BlockNameLabel",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Visible = false,
				TextEnum = MySpaceTexts.Afterburner
			};
			MyGuiControlLabel control5 = new MyGuiControlLabel
			{
				Position = new Vector2(0.165f, -0.32f),
				Size = new Vector2(0.0470270254f, 0.0266666654f),
				Name = "GroupTitleLabel",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				TextEnum = MySpaceTexts.Terminal_GroupTitle
			};
			MyGuiControlTextbox control6 = new MyGuiControlTextbox
			{
				Position = new Vector2(0.165f, -0.283f),
				Size = new Vector2(0.29f, 0.052f),
				Name = "GroupName",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(0.167f, -0.228f), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(222f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.TerminalButton_GroupSave))
			{
				Name = "GroupSave"
			};
			MyGuiControlButton control7 = new MyGuiControlButton(myGuiControlButton.Position + new Vector2(myGuiControlButton.Size.X + 0.013f, 0f), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(222f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Delete))
			{
				Name = "GroupDelete"
			};
			MyGuiControlButton control8 = new MyGuiControlButton(new Vector2(-0.19f, -0.332f), MyGuiControlButtonStyleEnum.SquareSmall, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, null, GuiSounds.MouseClick, 0.5f)
			{
				Name = "ShowAll"
			};
			page.Controls.Add(myGuiControlSearchBox);
			page.Controls.Add(control3);
			page.Controls.Add(control4);
			page.Controls.Add(control5);
			page.Controls.Add(control6);
			page.Controls.Add(myGuiControlButton);
			page.Controls.Add(control8);
			page.Controls.Add(control7);
			m_defaultFocusedControl[MyTerminalPageEnum.ControlPanel] = myGuiControlSearchBox.GetTextbox();
		}

		private void CreateFactionsPageControls(MyGuiControlTabPage page)
		{
			page.Name = "PageFactions";
			page.TextEnum = MySpaceTexts.TerminalTab_Factions;
			page.TextScale = 0.7005405f;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.343f), 0.696f, 0.002f);
			page.Controls.Add(myGuiControlSeparatorList);
			float num = -0.462f;
			float num2 = -0.34f;
			float num3 = 0.0045f;
			float num4 = 0.01f;
			new Vector2(0.29f, 0.052f);
			Vector2 value = new Vector2(0.13f, 0.04f);
			num += num3;
			num2 += num4;
			MyGuiControlCombobox myGuiControlCombobox = new MyGuiControlCombobox
			{
				Position = new Vector2(-0.452f, num2),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Size = new Vector2(0.29f, base.Size.Value.Y),
				Name = "FactionFilters"
			};
			num2 += myGuiControlCombobox.Size.Y + num4;
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2(-0.452f, num2), new Vector2(0.29f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = myGuiControlPanel.Position + new Vector2(0.01f, 0.005f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.TerminalTab_FactionsTableLabel)
			};
			num2 += myGuiControlPanel.Size.Y - 0.001f;
			MyGuiControlTable myGuiControlTable = new MyGuiControlTable
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(num + 0.0055f, num2),
				Size = new Vector2(0.29f, 0.15f),
				Name = "FactionsTable",
				ColumnsCount = 3,
				VisibleRowsCount = 13
			};
			myGuiControlTable.SetCustomColumnWidths(new float[3]
			{
				0.23f,
				0.64f,
				0.13f
			});
			myGuiControlTable.SetColumnName(0, MyTexts.Get(MyCommonTexts.Tag));
			myGuiControlTable.SetHeaderColumnMargin(0, new Thickness(0.01f, 0f, 0f, 0f));
			myGuiControlTable.SetColumnName(1, MyTexts.Get(MyCommonTexts.Name));
			myGuiControlTable.SetHeaderColumnMargin(1, new Thickness(0.005f, 0f, 0.005f, 0f));
			myGuiControlTable.SetHeaderColumnMargin(2, new Thickness(0f));
			num2 += myGuiControlTable.Size.Y + num4;
			MyGuiControlButton control2 = new MyGuiControlButton(size: new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, position: new Vector2(-0.449f, 0.305f), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonJoin",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control3 = new MyGuiControlButton(size: new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, position: new Vector2(-0.449f, 0.305f), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonCancelJoin",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control4 = new MyGuiControlButton(size: new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, position: new Vector2(-0.449f, 0.305f), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonLeave",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control5 = new MyGuiControlButton(size: new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, position: new Vector2(-0.16f, 0.305f), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				Name = "buttonEnemy",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control6 = new MyGuiControlButton(size: new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, position: new Vector2(-0.449f, 0.255f), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonCreate",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control7 = new MyGuiControlButton(size: new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, position: new Vector2(-0.16f, 0.255f), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				Name = "buttonSendPeace",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control8 = new MyGuiControlButton(size: new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, position: new Vector2(-0.16f, 0.255f), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				Name = "buttonCancelPeace",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control9 = new MyGuiControlButton(size: new Vector2(225f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, position: new Vector2(-0.16f, 0.255f), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				Name = "buttonAcceptPeace",
				ShowTooltipWhenDisabled = true
			};
			page.Controls.Add(myGuiControlCombobox);
			page.Controls.Add(myGuiControlPanel);
			page.Controls.Add(control);
			page.Controls.Add(myGuiControlTable);
			page.Controls.Add(control6);
			page.Controls.Add(control2);
			page.Controls.Add(control3);
			page.Controls.Add(control4);
			page.Controls.Add(control7);
			page.Controls.Add(control8);
			page.Controls.Add(control9);
			page.Controls.Add(control5);
			num = -0.0475f;
			num2 = -0.34f;
			new MyGuiControlCompositePanel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(-0.05f, num2),
				Size = new Vector2(0.5f, 0.69f),
				Name = "compositeFaction"
			};
			num += num3;
			num2 += num4;
			Vector2 vector = new Vector2(0.58f, 0.04f);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(-0.124f, num2), new Vector2(0.4f, 0.035f), null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "labelFactionDesc"
			};
			num2 += myGuiControlLabel.Size.Y + num4;
			Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
			float num5 = (float)safeGuiRectangle.Height / (float)safeGuiRectangle.Width;
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(new Vector2(-0.125f, num2), textPadding: new MyGuiBorderThickness(0.002f, 0f, 0f, 0f), size: new Vector2(0.58f - 0.1f * num5 - num3 * 2f, 0.1f), backgroundColor: null, font: "Blue", textScale: 0.7f, textAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, contents: null, drawScrollbarV: true, drawScrollbarH: true, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Name = "textFactionDesc"
			};
			MyGuiControlImage control10 = new MyGuiControlImage
			{
				Position = new Vector2(myGuiControlMultilineText.PositionX + myGuiControlMultilineText.Size.X + num3 * 2f, num2),
				Size = new Vector2(myGuiControlMultilineText.Size.Y * num5, myGuiControlMultilineText.Size.Y),
				Name = "factionIcon",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER,
				Padding = new MyGuiBorderThickness(1f)
			};
			num2 += myGuiControlMultilineText.Size.Y + 2f * num4;
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(-0.124f, num2), vector - new Vector2(0.01f, 0.01f), null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "labelReputation"
			};
			myGuiControlLabel2.Visible = false;
			MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2(0.166f, num2), vector - new Vector2(0.01f, 0.01f), null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP)
			{
				Name = "textReputation"
			};
			myGuiControlLabel3.Visible = false;
			MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(new Vector2(-0.124f, num2), vector - new Vector2(0.01f, 0.01f), null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "labelFactionPrivate"
			};
			num2 += myGuiControlLabel4.Size.Y + num4;
			MyGuiReputationProgressBar myGuiReputationProgressBar = new MyGuiReputationProgressBar(new Vector2(-0.124f, num2), new Vector2(0.58f, 0.06f), null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "progressReputation"
			};
			myGuiReputationProgressBar.Visible = false;
			if (MySession.Static != null)
			{
				MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
				if (component != null)
				{
					myGuiReputationProgressBar.SetBorderValues(component.GetHostileMax(), component.GetFriendlyMax(), component.GetNeutralMin(), component.GetFriendlyMin());
				}
			}
			MyGuiControlMultilineText control11 = new MyGuiControlMultilineText(new Vector2(-0.125f, num2), textPadding: new MyGuiBorderThickness(0.002f, 0f, 0f, 0f), size: new Vector2(0.58f, 0.1f), backgroundColor: null, font: "Blue", textScale: 0.7f, textAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, contents: null, drawScrollbarV: true, drawScrollbarH: true, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Name = "textFactionPrivate"
			};
			num2 += myGuiControlMultilineText.Size.Y + 0.0275f;
			MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(new Vector2(-0.125f, num2), vector * 0.5f, MyTexts.Get(MySpaceTexts.TerminalTab_Factions_AutoAccept).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "labelFactionMembersAcceptEveryone"
			};
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(new Vector2(myGuiControlLabel5.PositionX + myGuiControlLabel5.Size.X + 0.02f, myGuiControlLabel5.PositionY + 0.012f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
			{
				Name = "checkFactionMembersAcceptEveryone"
			};
			MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel(new Vector2(myGuiControlLabel5.PositionX + myGuiControlLabel5.Size.X + 0.08f, myGuiControlLabel5.PositionY), vector * 0.5f, MyTexts.Get(MySpaceTexts.TerminalTab_Factions_AutoAcceptRequest).ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "labelFactionMembersAcceptPeace"
			};
			MyGuiControlCheckbox control12 = new MyGuiControlCheckbox(new Vector2(myGuiControlLabel6.PositionX + myGuiControlLabel6.Size.X + 0.02f, myGuiControlLabel6.PositionY + 0.012f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
			{
				Name = "checkFactionMembersAcceptPeace"
			};
			num2 += myGuiControlLabel5.Size.Y + 2f * num4;
			MyGuiControlPanel myGuiControlPanel2 = new MyGuiControlPanel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(-0.125f, num2),
				Size = new Vector2(0.44f, 0.04f),
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER,
				Name = "panelFactionMembersNamePanel"
			};
			num2 += 0.007f;
			MyGuiControlLabel control13 = new MyGuiControlLabel(new Vector2(-0.114f, num2), vector - new Vector2(0.01f, 0.01f), null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "labelFactionMembers"
			};
			num2 += -0.007f + myGuiControlPanel2.Size.Y - MyGuiManager.GetHudNormalizedSizeFromPixelSize(new Vector2(1f)).Y;
			Vector2 vector2 = vector - new Vector2(0.14f, 0.01f);
			MyGuiControlTable myGuiControlTable2 = new MyGuiControlTable
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(-0.125f, num2),
				Size = new Vector2(vector2.X, 0.15f),
				Name = "tableMembers",
				ColumnsCount = 2,
				VisibleRowsCount = 6,
				HeaderVisible = false
			};
			myGuiControlTable2.SetCustomColumnWidths(new float[2]
			{
				0.5f,
				0.5f
			});
			myGuiControlTable2.SetColumnName(0, MyTexts.Get(MyCommonTexts.Name));
			myGuiControlTable2.SetColumnName(1, MyTexts.Get(MyCommonTexts.Status));
			float num6 = value.Y + num4 - 0.0027f;
			MyGuiControlButton control14 = new MyGuiControlButton(size: value, position: new Vector2(num + myGuiControlTable2.Size.X + num4 - 0.081f, myGuiControlCheckbox.PositionY - 0.017f), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonEdit",
				ShowTooltipWhenDisabled = true
			};
			float num7 = myGuiControlPanel2.PositionY + 0.002f;
			MyGuiControlButton control15 = new MyGuiControlButton(size: value, position: new Vector2(num + myGuiControlTable2.Size.X + num4 - 0.081f, num7), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonPromote",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control16 = new MyGuiControlButton(size: value, position: new Vector2(num + myGuiControlTable2.Size.X + num4 - 0.081f, num7 + num6), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonKick",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control17 = new MyGuiControlButton(size: value, position: new Vector2(num + myGuiControlTable2.Size.X + num4 - 0.081f, num7 + 2f * num6), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonAcceptJoin",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control18 = new MyGuiControlButton(size: value, position: new Vector2(num + myGuiControlTable2.Size.X + num4 - 0.081f, num7 + 3f * num6), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonDemote",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control19 = new MyGuiControlButton(size: value, position: new Vector2(num + myGuiControlTable2.Size.X + num4 - 0.081f, num7 + 4f * num6), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonShareProgress",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton control20 = new MyGuiControlButton(size: value, position: new Vector2(num + myGuiControlTable2.Size.X + num4 - 0.081f, num7 + 5f * num6), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "buttonAddNpc",
				ShowTooltipWhenDisabled = true
			};
			float y = num7 + 6f * num6 + 0.005f;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(size: value, position: new Vector2(num + myGuiControlTable2.Size.X + num4 - 0.081f, y), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "withdrawBtn",
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(size: value, position: new Vector2(myGuiControlButton.PositionX - num4, y), visualStyle: MyGuiControlButtonStyleEnum.Rectangular, colorMask: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				Name = "depositBtn",
				ShowTooltipWhenDisabled = true
			};
			num2 += myGuiControlTable2.Size.Y + 0.018f;
			MyGuiControlLabel control21 = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(-0.125f, num2),
				Size = new Vector2(0.3f, 0.08f),
				TextEnum = MySpaceTexts.Currency_Default_Account_Label,
				Name = "labelBalance"
			};
			num5 = (float)safeGuiRectangle.Width / (float)safeGuiRectangle.Height;
			Vector2 vector3 = new Vector2(0.018f, num5 * 0.018f);
			MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				Position = new Vector2(myGuiControlButton2.PositionX - myGuiControlButton2.Size.X - 0.01f - vector3.X, num2),
				Size = new Vector2(0.3f, 0.08f),
				Text = "N/A",
				Name = "labelBalanceValue"
			};
			MyGuiControlImage control22 = new MyGuiControlImage
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = myGuiControlLabel7.Position + new Vector2(0.005f, 0f),
				Name = "imageCurrency",
				Size = new Vector2(0.018f, num5 * 0.018f),
				Visible = false
			};
			page.Controls.Add(myGuiControlPanel2);
			page.Controls.Add(myGuiControlLabel);
			page.Controls.Add(myGuiControlMultilineText);
			page.Controls.Add(control10);
			page.Controls.Add(myGuiReputationProgressBar);
			page.Controls.Add(myGuiControlLabel4);
			page.Controls.Add(myGuiControlLabel2);
			page.Controls.Add(myGuiControlLabel3);
			page.Controls.Add(control11);
			page.Controls.Add(control13);
			page.Controls.Add(myGuiControlLabel5);
			page.Controls.Add(myGuiControlLabel6);
			page.Controls.Add(myGuiControlCheckbox);
			page.Controls.Add(control12);
			page.Controls.Add(myGuiControlTable2);
			page.Controls.Add(control14);
			page.Controls.Add(control15);
			page.Controls.Add(control16);
			page.Controls.Add(control18);
			page.Controls.Add(control17);
			page.Controls.Add(control19);
			page.Controls.Add(control20);
			page.Controls.Add(myGuiControlButton);
			page.Controls.Add(myGuiControlButton2);
			page.Controls.Add(control21);
			page.Controls.Add(myGuiControlLabel7);
			page.Controls.Add(control22);
			m_defaultFocusedControl[MyTerminalPageEnum.Factions] = myGuiControlTable;
		}

		private void CreateChatPageControls(MyGuiControlTabPage chatPage)
		{
			chatPage.Name = "PageComms";
			chatPage.TextEnum = MySpaceTexts.TerminalTab_Chat;
			chatPage.TextScale = 0.7005405f;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 0.002f);
			chatPage.Controls.Add(myGuiControlSeparatorList);
			float num = -0.452f;
			float num2 = 0f - num;
			float num3 = -0.332f;
			int num4 = 11;
			float x = 0.35f;
			float num5 = 0f;
			float num6 = 0.02f;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
			{
				Position = new Vector2(num + 0.01f, num3 + 0.005f),
				Name = "PlayerLabel",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.TerminalTab_PlayersTableLabel)
			};
			MyGuiControlPanel control = new MyGuiControlPanel(new Vector2(-0.452f, -0.332f), new Vector2(0.29f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			chatPage.Controls.Add(control);
			chatPage.Controls.Add(myGuiControlLabel);
			num3 += myGuiControlLabel.GetTextSize().Y + 0.012f;
			MyGuiControlListbox myGuiControlListbox = new MyGuiControlListbox
			{
				Position = new Vector2(num, num3),
				Size = new Vector2(x, 0f),
				Name = "PlayerListbox",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				VisibleRowsCount = num4,
				VisualStyle = MyGuiControlListboxStyleEnum.ChatScreen
			};
			chatPage.Controls.Add(myGuiControlListbox);
			num5 = myGuiControlListbox.ItemSize.Y * (float)num4;
			num3 += num5 + num6 + 0.004f;
			num4 = 6;
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = new Vector2(num + 0.01f, num3 + 0.003f),
				Name = "PlayerLabel",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.TerminalTab_FactionsTableLabel)
			};
			MyGuiControlPanel control3 = new MyGuiControlPanel(new Vector2(-0.452f, 0.097f), new Vector2(0.29f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			chatPage.Controls.Add(control3);
			chatPage.Controls.Add(control2);
			num3 += myGuiControlLabel.GetTextSize().Y + 0.01f;
			MyGuiControlListbox control4 = new MyGuiControlListbox
			{
				Position = new Vector2(num, num3),
				Size = new Vector2(x, 0f),
				Name = "FactionListbox",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				VisibleRowsCount = num4,
				VisualStyle = MyGuiControlListboxStyleEnum.ChatScreen
			};
			chatPage.Controls.Add(control4);
			num3 = -0.34f;
			x = 0.6f;
			num5 = 0.515f;
			num6 = 0.038f;
			MyGuiControlPanel control5 = new MyGuiControlPanel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(-0.125f, num3 + 0.008f),
				Size = new Vector2(x - 0.019f, num5 + 0.1f),
				BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST
			};
			chatPage.Controls.Add(control5);
			MyGuiControlMultilineText myGuiControlMultilineText = new MyGuiControlMultilineText(new Vector2(num2 + 0.003f, num3 + 0.02f), new Vector2(x - 0.033f, num5 + 0.08f), null, "Blue", 684f / 925f);
			myGuiControlMultilineText.Name = "ChatHistory";
			myGuiControlMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			myGuiControlMultilineText.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			chatPage.Controls.Add(myGuiControlMultilineText);
			num3 += num5 + num6;
			num5 = 0.05f;
			MyGuiControlTextbox control6 = new MyGuiControlTextbox
			{
				Position = new Vector2(num2 - 0.5765f, num3 + 0.104f),
				Size = new Vector2(x - 0.165f, num5),
				Name = "Chatbox",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
			};
			chatPage.Controls.Add(control6);
			x = 0.75f;
			num3 += num5 + num6;
			num5 = 0.05f;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton
			{
				Position = new Vector2(num2 + 0.007f, num3 + 0.023f),
				Text = MyTexts.Get(MyCommonTexts.SendMessage).ToString(),
				Name = "SendButton",
				Size = new Vector2(x, num5),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
			};
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.ComboBoxButton;
			chatPage.Controls.Add(myGuiControlButton);
			m_defaultFocusedControl[MyTerminalPageEnum.Comms] = myGuiControlListbox;
		}

		private void CreateInfoPageControls(MyGuiControlTabPage infoPage)
		{
			infoPage.Name = "PageInfo";
			infoPage.TextEnum = MySpaceTexts.TerminalTab_Info;
			infoPage.TextScale = 0.7005405f;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddVertical(new Vector2(0.145f, -0.333f), 0.676f, 0.002f);
			myGuiControlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 0.002f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.168f, 0.148f), 0.27f, 0.001f);
			infoPage.Controls.Add(myGuiControlSeparatorList);
			MyGuiControlPanel control = new MyGuiControlPanel
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(-0.452f, -0.332f),
				Size = new Vector2(0.29f, 0.035f),
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel
			{
				Position = new Vector2(-0.442f, -0.327f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.TerminalTab_Info_GridInfoLabel)
			};
			myGuiControlLabel.Name = "Infolabel";
			infoPage.Controls.Add(control);
			infoPage.Controls.Add(myGuiControlLabel);
			MyGuiControlList myGuiControlList = new MyGuiControlList(new Vector2(-0.452f, -0.299f), new Vector2(0.29f, 0.6405f));
			myGuiControlList.Name = "InfoList";
			myGuiControlList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			infoPage.Controls.Add(myGuiControlList);
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel
			{
				Position = new Vector2(0.168f, 0.05f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShipName)
			};
			myGuiControlLabel2.Name = "RenameShipLabel";
			infoPage.Controls.Add(myGuiControlLabel2);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton();
			myGuiControlButton.Position = new Vector2(0.31f, 0.225f);
			myGuiControlButton.TextEnum = MySpaceTexts.TerminalTab_Info_ConvertButton;
			myGuiControlButton.SetToolTip(MySpaceTexts.TerminalTab_Info_ConvertButton_TT);
			myGuiControlButton.ShowTooltipWhenDisabled = true;
			myGuiControlButton.Name = "ConvertBtn";
			infoPage.Controls.Add(myGuiControlButton);
			MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton();
			myGuiControlButton2.Position = new Vector2(0.31f, 0.285f);
			myGuiControlButton2.TextEnum = MySpaceTexts.TerminalTab_Info_ConvertToStationButton;
			myGuiControlButton2.SetToolTip(MySpaceTexts.TerminalTab_Info_ConvertToStationButton_TT);
			myGuiControlButton2.ShowTooltipWhenDisabled = true;
			myGuiControlButton2.Name = "ConvertToStationBtn";
			myGuiControlButton2.Visible = MySession.Static.EnableConvertToStation;
			infoPage.Controls.Add(myGuiControlButton2);
			MyGuiControlMultilineText control2 = new MyGuiControlMultilineText(null, null, null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, selectable: false, showTextShadow: false, null, null)
			{
				Name = "InfoHelpMultilineText",
				Position = new Vector2(0.167f, -0.3345f),
				Size = new Vector2(0.297f, 0.36f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.Get(MySpaceTexts.TerminalTab_Info_Description)
			};
			infoPage.Controls.Add(control2);
			if (MyFakes.ENABLE_CENTER_OF_MASS)
			{
				MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2(-0.123f, -0.313f), null, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowMassCenter));
				myGuiControlLabel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				infoPage.Controls.Add(myGuiControlLabel3);
				MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(new Vector2(0.135f, myGuiControlLabel3.Position.Y));
				myGuiControlCheckbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				myGuiControlCheckbox.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowMassCenter_ToolTip));
				myGuiControlCheckbox.Name = "CenterBtn";
				infoPage.Controls.Add(myGuiControlCheckbox);
				m_defaultFocusedControl[MyTerminalPageEnum.Info] = myGuiControlCheckbox;
			}
			MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(new Vector2(-0.123f, -0.263f), null, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowGravityGizmo));
			myGuiControlLabel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			infoPage.Controls.Add(myGuiControlLabel4);
			MyGuiControlCheckbox myGuiControlCheckbox2 = new MyGuiControlCheckbox(new Vector2(0.135f, myGuiControlLabel4.Position.Y));
			myGuiControlCheckbox2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlCheckbox2.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowGravityGizmo_ToolTip));
			myGuiControlCheckbox2.Name = "ShowGravityGizmo";
			infoPage.Controls.Add(myGuiControlCheckbox2);
			MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(new Vector2(-0.123f, -0.213f), null, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowSenzorGizmo));
			myGuiControlLabel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			infoPage.Controls.Add(myGuiControlLabel5);
			MyGuiControlCheckbox myGuiControlCheckbox3 = new MyGuiControlCheckbox(new Vector2(0.135f, myGuiControlLabel5.Position.Y));
			myGuiControlCheckbox3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlCheckbox3.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowSenzorGizmo_ToolTip));
			myGuiControlCheckbox3.Name = "ShowSenzorGizmo";
			infoPage.Controls.Add(myGuiControlCheckbox3);
			MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel(new Vector2(-0.123f, -0.163f), null, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowAntenaGizmo));
			myGuiControlLabel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			infoPage.Controls.Add(myGuiControlLabel6);
			MyGuiControlCheckbox myGuiControlCheckbox4 = new MyGuiControlCheckbox(new Vector2(0.135f, myGuiControlLabel6.Position.Y));
			myGuiControlCheckbox4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlCheckbox4.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_ShowAntenaGizmo_ToolTip));
			myGuiControlCheckbox4.Name = "ShowAntenaGizmo";
			infoPage.Controls.Add(myGuiControlCheckbox4);
			CreateAntennaSlider(infoPage, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_FriendlyAntennaRange), "FriendAntennaRange", -0.05f);
			CreateAntennaSlider(infoPage, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_EnemyAntennaRange), "EnemyAntennaRange", 0.09f);
			CreateAntennaSlider(infoPage, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_OwnedAntennaRange), "OwnedAntennaRange", 0.23f);
			MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(new Vector2(-0.123f, -0.113f), null, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_PivotBtn));
			myGuiControlLabel7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			infoPage.Controls.Add(myGuiControlLabel7);
			MyGuiControlCheckbox myGuiControlCheckbox5 = new MyGuiControlCheckbox(new Vector2(0.135f, myGuiControlLabel7.Position.Y));
			myGuiControlCheckbox5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlCheckbox5.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Info_PivotBtn_ToolTip));
			myGuiControlCheckbox5.Name = "PivotBtn";
			infoPage.Controls.Add(myGuiControlCheckbox5);
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				MyGuiControlTextbox myGuiControlTextbox = new MyGuiControlTextbox
				{
					Name = "RenameShipText",
					Position = new Vector2(0.168f, myGuiControlLabel2.PositionY + 0.048f),
					Size = new Vector2(0.225f, 0.005f),
					OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER
				};
				MyGuiControlButton myGuiControlButton3 = new MyGuiControlButton
				{
					Name = "RenameShipButton",
					Position = new Vector2(myGuiControlTextbox.PositionX + myGuiControlTextbox.Size.X + 0.025f, myGuiControlTextbox.PositionY + 0.006f),
					Text = MyTexts.Get(MyCommonTexts.Ok).ToString(),
					VisualStyle = MyGuiControlButtonStyleEnum.Rectangular,
					Size = new Vector2(0.036f, 0.0392f)
				};
				myGuiControlButton3.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
				infoPage.Controls.Add(myGuiControlButton3);
				myGuiControlTextbox.SetTooltip(MyTexts.Get(MySpaceTexts.TerminalName).ToString());
				myGuiControlTextbox.ShowTooltipWhenDisabled = true;
				infoPage.Controls.Add(myGuiControlTextbox);
			}
			MyGuiControlLabel myGuiControlLabel8 = new MyGuiControlLabel(new Vector2(-0.123f, 0.28f), null, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_DestructibleBlocks));
			myGuiControlLabel8.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlLabel8.Visible = (MySession.Static.Settings.ScenarioEditMode || MySession.Static.IsScenario);
			infoPage.Controls.Add(myGuiControlLabel8);
			MyGuiControlCheckbox myGuiControlCheckbox6 = new MyGuiControlCheckbox(new Vector2(0.135f, myGuiControlLabel8.Position.Y), null, MyTexts.GetString(MySpaceTexts.TerminalTab_Info_DestructibleBlocks_Tooltip));
			myGuiControlCheckbox6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlCheckbox6.Name = "SetDestructibleBlocks";
			infoPage.Controls.Add(myGuiControlCheckbox6);
		}

		private static bool OnAntennaSliderClicked(MyGuiControlSlider arg)
		{
			if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				float num = MyHudMarkerRender.Denormalize(0f);
				float max = MyHudMarkerRender.Denormalize(1f);
				float value = MyHudMarkerRender.Denormalize(arg.Value);
				bool flag = true;
				if (flag && Math.Abs(num) < 1f)
				{
					num = 0f;
				}
				MyGuiScreenDialogAmount myGuiScreenDialogAmount = new MyGuiScreenDialogAmount(num, max, MyCommonTexts.DialogAmount_SetValueCaption, 3, flag, value, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity);
				myGuiScreenDialogAmount.OnConfirmed += delegate(float v)
				{
					arg.Value = MyHudMarkerRender.Normalize(v);
				};
				MyGuiSandbox.AddScreen(myGuiScreenDialogAmount);
				return true;
			}
			return false;
		}

		private static void CreateAntennaSlider(MyGuiControlTabPage infoPage, string labelText, string name, float startY)
		{
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(-0.123f, startY), null, labelText);
			myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			infoPage.Controls.Add(myGuiControlLabel);
			MyGuiControlLabel friendAntennaRangeValueLabel = new MyGuiControlLabel(new Vector2(-0.123f, startY + 0.09f));
			friendAntennaRangeValueLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			infoPage.Controls.Add(friendAntennaRangeValueLabel);
			MyGuiControlSlider myGuiControlSlider = new MyGuiControlSlider(new Vector2(0.126f, startY + 0.05f));
			myGuiControlSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
			myGuiControlSlider.Name = name;
			myGuiControlSlider.Size = new Vector2(0.25f, 1f);
			myGuiControlSlider.MinValue = 0f;
			myGuiControlSlider.MaxValue = 1f;
			myGuiControlSlider.DefaultValue = myGuiControlSlider.MaxValue;
			myGuiControlSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(myGuiControlSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider s)
			{
				friendAntennaRangeValueLabel.Text = MyValueFormatter.GetFormatedFloat(MyHudMarkerRender.Denormalize(s.Value), 0) + "m";
			});
			myGuiControlSlider.SliderClicked = OnAntennaSliderClicked;
			infoPage.Controls.Add(myGuiControlSlider);
		}

		private void CreateProductionPageControls(MyGuiControlTabPage productionPage)
		{
			productionPage.Name = "PageProduction";
			productionPage.TextEnum = MySpaceTexts.TerminalTab_Production;
			productionPage.TextScale = 0.7005405f;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddVertical(new Vector2(0.145f, -0.333f), 0.676f, 0.002f);
			myGuiControlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 0.002f);
			productionPage.Controls.Add(myGuiControlSeparatorList);
			float num = 0.03f;
			float num2 = 0.01f;
			float num3 = 0.05f;
			float num4 = 0.08f;
			MyGuiControlCombobox myGuiControlCombobox = new MyGuiControlCombobox(-0.5f * productionPage.Size + new Vector2(0.001f, num2 + 0.174f))
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Name = "AssemblersCombobox"
			};
			MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(-0.5f * productionPage.Size + new Vector2(0f, num2 + 0.028f) + new Vector2(0.001f, myGuiControlCombobox.Size.Y + num2 - 0.001f - 0.048f), new Vector2(1f, num4), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER,
				Name = "BlueprintsBackgroundPanel"
			};
			MyGuiControlPanel control = new MyGuiControlPanel(new Vector2(-0.452f, -0.332000017f), new Vector2(0.29f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			MyGuiControlLabel control2 = new MyGuiControlLabel(myGuiControlPanel.Position + new Vector2(num2, num2 - 0.005f), null, MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_Blueprints), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "BlueprintsLabel"
			};
			MyGuiControlSearchBox myGuiControlSearchBox = new MyGuiControlSearchBox(myGuiControlPanel.Position + new Vector2(0f, num4 + num2), myGuiControlCombobox.Size, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "BlueprintsSearchBox"
			};
			MyGuiControlGrid myGuiControlGrid = new MyGuiControlGrid
			{
				VisualStyle = MyGuiControlGridStyleEnum.Blueprints,
				RowsCount = MyTerminalProductionController.BLUEPRINT_GRID_ROWS,
				ColumnsCount = 5,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(myGuiControlGrid)
			{
				Name = "BlueprintsScrollableArea",
				ScrollbarVEnabled = true,
				Position = myGuiControlCombobox.Position + new Vector2(0f, myGuiControlCombobox.Size.Y + num2),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				Size = new Vector2(myGuiControlPanel.Size.X, 0.5f),
				ScrolledAreaPadding = new MyGuiBorderThickness(0.005f),
				DrawScrollBarSeparator = true
			};
			myGuiControlScrollablePanel.FitSizeToScrolledControl();
			myGuiControlCombobox.Size = new Vector2(myGuiControlScrollablePanel.Size.X, myGuiControlCombobox.Size.Y);
			myGuiControlSearchBox.Size = myGuiControlCombobox.Size;
			myGuiControlPanel.Size = new Vector2(myGuiControlScrollablePanel.Size.X, num4);
			myGuiControlGrid.RowsCount = 20;
			productionPage.Controls.Add(myGuiControlCombobox);
			productionPage.Controls.Add(myGuiControlPanel);
			productionPage.Controls.Add(control);
			productionPage.Controls.Add(control2);
			productionPage.Controls.Add(myGuiControlSearchBox);
			productionPage.Controls.Add(myGuiControlScrollablePanel);
			Vector2 value = myGuiControlPanel.Position + new Vector2(myGuiControlPanel.Size.X + num + 0.05f, 0f);
			MyGuiControlLabel control3 = new MyGuiControlLabel(value + new Vector2(num2, num2) + new Vector2(-0.05f, 0.002f), null, MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_StoredMaterials), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			productionPage.Controls.Add(control3);
			MyGuiControlLabel control4 = new MyGuiControlLabel(value + new Vector2(num2, num2) + new Vector2(-0.05f, 0.028f), null, MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_MaterialType), null, 0.704f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			productionPage.Controls.Add(control4);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(value + new Vector2(num2, num2) + new Vector2(0.2f, 0.028f), null, null, null, 0.704f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			myGuiControlLabel.Name = "RequiredLabel";
			productionPage.Controls.Add(myGuiControlLabel);
			MyGuiControlComponentList control5 = new MyGuiControlComponentList
			{
				Position = value + new Vector2(-0.062f, num3 - 0.002f),
				Size = new Vector2(0.29f, num3 + myGuiControlScrollablePanel.Size.Y - num3),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = null,
				Name = "MaterialsList"
			};
			productionPage.Controls.Add(control5);
			MyGuiControlRadioButton myGuiControlRadioButton = new MyGuiControlRadioButton(value + new Vector2(myGuiControlPanel.Size.X + num - 0.071f, 0f), new Vector2(210f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Icon = MyGuiConstants.TEXTURE_BUTTON_ICON_COMPONENT,
				IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				Text = MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_AssemblingButton),
				Name = "AssemblingButton"
			};
			myGuiControlRadioButton.SetToolTip(MySpaceTexts.ToolTipTerminalProduction_AssemblingMode);
			MyGuiControlRadioButton myGuiControlRadioButton2 = new MyGuiControlRadioButton(myGuiControlRadioButton.Position + new Vector2(myGuiControlRadioButton.Size.X + num2, 0f), new Vector2(238f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Icon = MyGuiConstants.TEXTURE_BUTTON_ICON_DISASSEMBLY,
				IconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				Text = MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_DisassemblingButton),
				Name = "DisassemblingButton"
			};
			myGuiControlRadioButton2.SetToolTip(MySpaceTexts.ToolTipTerminalProduction_DisassemblingMode);
			MyGuiControlCompositePanel myGuiControlCompositePanel = new MyGuiControlCompositePanel
			{
				Position = myGuiControlRadioButton.Position + new Vector2(0f, myGuiControlRadioButton.Size.Y + num2),
				Size = new Vector2(0.4f, num4),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			MyGuiControlLabel control6 = new MyGuiControlLabel(myGuiControlCompositePanel.Position + new Vector2(num2, num2), null, MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_ProductionQueue), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			MyGuiControlGrid obj = new MyGuiControlGrid
			{
				VisualStyle = MyGuiControlGridStyleEnum.Blueprints,
				RowsCount = 2,
				ColumnsCount = 5,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlScrollablePanel myGuiControlScrollablePanel2 = new MyGuiControlScrollablePanel(obj)
			{
				Name = "QueueScrollableArea",
				ScrollbarVEnabled = true,
				Position = myGuiControlCompositePanel.Position + new Vector2(0f, myGuiControlCompositePanel.Size.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ScrolledAreaPadding = new MyGuiBorderThickness(0.005f),
				DrawScrollBarSeparator = true
			};
			myGuiControlScrollablePanel2.FitSizeToScrolledControl();
			obj.RowsCount = 10;
			myGuiControlCompositePanel.Size = new Vector2(myGuiControlScrollablePanel2.Size.X, myGuiControlCompositePanel.Size.Y);
			MyGuiControlCheckbox control7 = new MyGuiControlCheckbox(myGuiControlCompositePanel.Position + new Vector2(myGuiControlCompositePanel.Size.X - num2, num2), null, MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_RepeatMode), isChecked: false, MyGuiControlCheckboxStyleEnum.Repeat, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				Name = "RepeatCheckbox"
			};
			MyGuiControlCheckbox control8 = new MyGuiControlCheckbox(myGuiControlCompositePanel.Position + new Vector2(myGuiControlCompositePanel.Size.X - 0.1f - num2, num2), null, MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_SlaveMode), isChecked: false, MyGuiControlCheckboxStyleEnum.Slave, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				Name = "SlaveCheckbox"
			};
			MyGuiControlCompositePanel myGuiControlCompositePanel2 = new MyGuiControlCompositePanel
			{
				Position = myGuiControlScrollablePanel2.Position + new Vector2(0f, myGuiControlScrollablePanel2.Size.Y + num2),
				Size = new Vector2(0.4f, num4),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			MyGuiControlLabel control9 = new MyGuiControlLabel(myGuiControlCompositePanel2.Position + new Vector2(num2, num2), null, MyTexts.GetString(MySpaceTexts.ScreenTerminalProduction_Inventory), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			MyGuiControlGrid obj2 = new MyGuiControlGrid
			{
				VisualStyle = MyGuiControlGridStyleEnum.Blueprints,
				RowsCount = 3,
				ColumnsCount = 5,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				ShowTooltipWhenDisabled = true
			};
			MyGuiControlScrollablePanel myGuiControlScrollablePanel3 = new MyGuiControlScrollablePanel(obj2)
			{
				Name = "InventoryScrollableArea",
				ScrollbarVEnabled = true,
				Position = myGuiControlCompositePanel2.Position + new Vector2(0f, myGuiControlCompositePanel2.Size.Y),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ScrolledAreaPadding = new MyGuiBorderThickness(0.005f),
				DrawScrollBarSeparator = true
			};
			myGuiControlScrollablePanel3.FitSizeToScrolledControl();
			obj2.RowsCount = 10;
			myGuiControlCompositePanel2.Size = new Vector2(myGuiControlScrollablePanel3.Size.X, myGuiControlCompositePanel2.Size.Y);
			MyGuiControlButton control10 = new MyGuiControlButton(myGuiControlCompositePanel2.Position + new Vector2(myGuiControlCompositePanel2.Size.X - num2, num2), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(220f, 40f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, text: MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_DisassembleAllButton), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipTerminalProduction_DisassembleAll))
			{
				Name = "DisassembleAllButton"
			};
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(myGuiControlScrollablePanel3.Position + new Vector2(0.002f, myGuiControlScrollablePanel3.Size.Y + num2 + 0.048f), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(224f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_InventoryButton))
			{
				Name = "InventoryButton"
			};
			MyGuiControlButton control11 = new MyGuiControlButton(myGuiControlButton.Position + new Vector2(myGuiControlButton.Size.X + num2, 0f), MyGuiControlButtonStyleEnum.Rectangular, myGuiControlButton.Size, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MySpaceTexts.ScreenTerminalProduction_ControlPanelButton))
			{
				Name = "ControlPanelButton"
			};
			productionPage.Controls.Add(myGuiControlRadioButton);
			productionPage.Controls.Add(myGuiControlRadioButton2);
			productionPage.Controls.Add(myGuiControlCompositePanel);
			productionPage.Controls.Add(control6);
			productionPage.Controls.Add(control7);
			productionPage.Controls.Add(control8);
			productionPage.Controls.Add(myGuiControlScrollablePanel2);
			productionPage.Controls.Add(myGuiControlCompositePanel2);
			productionPage.Controls.Add(control9);
			productionPage.Controls.Add(control10);
			productionPage.Controls.Add(myGuiControlScrollablePanel3);
			productionPage.Controls.Add(myGuiControlButton);
			productionPage.Controls.Add(control11);
			m_defaultFocusedControl[MyTerminalPageEnum.Production] = myGuiControlGrid;
		}

		private void CreateGpsPageControls(MyGuiControlTabPage gpsPage)
		{
			gpsPage.Name = "PageIns";
			gpsPage.TextEnum = MySpaceTexts.TerminalTab_GPS;
			gpsPage.TextScale = 0.7005405f;
			float num = 0.01f;
			float num2 = 0.01f;
			new Vector2(0.29f, 0.052f);
			new Vector2(0.13f, 0.04f);
			float num3 = -0.4625f;
			float num4 = -0.325f;
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddVertical(new Vector2(-0.1435f, -0.333f), 0.676f, 0.002f);
			gpsPage.Controls.Add(myGuiControlSeparatorList);
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = new Vector2(-0.442f, -0.267f),
				Name = "GpsLabel",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MySpaceTexts.GpsScreen_GpsListLabel)
			};
			MyGuiControlPanel control2 = new MyGuiControlPanel(new Vector2(-0.452f, -0.272f), new Vector2(0.29f, 0.035f), null, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			gpsPage.Controls.Add(control2);
			gpsPage.Controls.Add(control);
			MyGuiControlSearchBox myGuiControlSearchBox = new MyGuiControlSearchBox(new Vector2(-0.452f, num4), new Vector2(0.29f, 0.02f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			myGuiControlSearchBox.Name = "SearchIns";
			gpsPage.Controls.Add(myGuiControlSearchBox);
			num4 += myGuiControlSearchBox.Size.Y + 0.01f + num2;
			MyGuiControlTable myGuiControlTable = new MyGuiControlTable
			{
				Position = new Vector2(num3 + 0.0105f, num4 + 0.044f),
				Size = new Vector2(0.29f, 0.5f),
				Name = "TableINS",
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				ColumnsCount = 1,
				VisibleRowsCount = 13,
				HeaderVisible = false
			};
			myGuiControlTable.SetCustomColumnWidths(new float[1]
			{
				1f
			});
			num4 += myGuiControlTable.Size.Y + num2 + 0.055f;
			num3 += 0.013f;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(new Vector2(num3, num4), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(140f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Add), toolTip: MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Add_ToolTip))
			{
				Name = "buttonAdd"
			};
			MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(new Vector2(num3, num4 + myGuiControlButton.Size.Y + num2), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(140f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Delete), toolTip: MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Delete_ToolTip))
			{
				Name = "buttonDelete"
			};
			myGuiControlButton2.ShowTooltipWhenDisabled = true;
			MyGuiControlButton myGuiControlButton3 = new MyGuiControlButton(new Vector2(num3 + myGuiControlButton.Size.X + num, num4), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(310f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromCurrent), toolTip: MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewFromCurrent_ToolTip))
			{
				Name = "buttonFromCurrent"
			};
			MyGuiControlButton control3 = new MyGuiControlButton(new Vector2(num3 + myGuiControlButton.Size.X + num, num4 + myGuiControlButton.Size.Y + num2), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(310f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, text: MyTexts.Get(MySpaceTexts.TerminalTab_GPS_NewFromClipboard), toolTip: MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewFromClipboard_ToolTip))
			{
				Name = "buttonFromClipboard"
			};
			gpsPage.Controls.Add(myGuiControlTable);
			gpsPage.Controls.Add(myGuiControlButton);
			gpsPage.Controls.Add(myGuiControlButton2);
			gpsPage.Controls.Add(myGuiControlButton3);
			gpsPage.Controls.Add(control3);
			num3 = -0.15f;
			num4 = -0.345f;
			num3 += num;
			num4 += num2;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(-0.125f, num4), new Vector2(0.4f, 0.035f), null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "labelInsName",
				Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Name).ToString()
			};
			num4 += myGuiControlLabel.Size.Y + num2;
			MyGuiControlTextbox myGuiControlTextbox = new MyGuiControlTextbox(null, null, 32)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = new Vector2(-0.125f, num4),
				Size = new Vector2(0.58f, 0.035f),
				Name = "panelInsName"
			};
			myGuiControlTextbox.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewCoord_Name_ToolTip));
			num4 += myGuiControlTextbox.Size.Y + 2f * num2;
			_ = myGuiControlTextbox.Size - new Vector2(0.14f, 0.01f);
			MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(new Vector2(-0.125f, num4), new Vector2(0.288000017f, 0.035f), null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "labelInsDesc",
				Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Description).ToString()
			};
			num4 += myGuiControlLabel2.Size.Y * 0.5f + 2f * num2;
			MyGuiControlMultilineEditableText myGuiControlMultilineEditableText = new MyGuiControlMultilineEditableText
			{
				Position = new Vector2(-0.125f, num4),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Name = "textInsDesc",
				Size = new Vector2(0.58f, 0.356f),
				BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_BUTTON_BORDER,
				TextPadding = new MyGuiBorderThickness(0.006f)
			};
			myGuiControlMultilineEditableText.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_NewCoord_Desc_ToolTip));
			num4 += myGuiControlMultilineEditableText.Size.Y + num2;
			MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(new Vector2(-0.125f, num4), new Vector2(0.4f, 0.035f), null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				Name = "labelInsCoordinates",
				Text = MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Coordinates)
			};
			num4 += myGuiControlLabel3.Size.Y + 3f * num2;
			MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(new Vector2(num3 + 0.017f, num4), new Vector2(0.01f, 0.035f), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_X).ToString())
			{
				Name = "labelInsX"
			};
			num3 += myGuiControlLabel4.Size.X + num;
			MyGuiControlTextbox myGuiControlTextbox2 = new MyGuiControlTextbox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(num3 + 0.017f, num4),
				Size = new Vector2((0.598f - num) / 3f - 2f * num - myGuiControlLabel4.Size.X, 0.035f),
				Name = "textInsX"
			};
			myGuiControlTextbox2.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_X_ToolTip));
			num3 += myGuiControlTextbox2.Size.X + num;
			MyGuiControlLabel control4 = new MyGuiControlLabel(new Vector2(num3 + 0.017f, num4), new Vector2(0.586f, 0.035f), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Y).ToString())
			{
				Name = "labelInsY"
			};
			num3 += myGuiControlLabel4.Size.X + num;
			MyGuiControlTextbox myGuiControlTextbox3 = new MyGuiControlTextbox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(num3 + 0.017f, num4),
				Size = new Vector2((0.598f - num) / 3f - 2f * num - myGuiControlLabel4.Size.X, 0.035f),
				Name = "textInsY"
			};
			myGuiControlTextbox3.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Y_ToolTip));
			num3 += myGuiControlTextbox3.Size.X + num;
			MyGuiControlLabel control5 = new MyGuiControlLabel(new Vector2(num3 + 0.017f, num4), new Vector2(0.01f, 0.035f), MyTexts.Get(MySpaceTexts.TerminalTab_GPS_Z).ToString())
			{
				Name = "labelInsZ"
			};
			num3 += myGuiControlLabel4.Size.X + num;
			MyGuiControlTextbox control6 = new MyGuiControlTextbox
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				Position = new Vector2(num3 + 0.017f, num4),
				Size = new Vector2((0.598f - num) / 3f - 2f * num - myGuiControlLabel4.Size.X, 0.035f),
				Name = "textInsZ"
			};
			control6.SetTooltip(MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_Z_ToolTip));
			num4 += myGuiControlTextbox.Size.Y + 2f * num2 + 0.032f;
			num3 = num - 0.135f;
			MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(new Vector2(num3, num4), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
			{
				Name = "checkInsShowOnHud"
			};
			MyGuiControlLabel control7 = new MyGuiControlLabel(new Vector2(num3 + myGuiControlCheckbox.Size.X + num, num4), myGuiControlCheckbox.Size - new Vector2(0.01f, 0.01f))
			{
				Name = "labelInsShowOnHud",
				Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_ShowOnHud).ToString()
			};
			MyGuiControlButton myGuiControlButton4 = new MyGuiControlButton(new Vector2(0.456f, num4 + 0.006f), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(300f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MySpaceTexts.TerminalTab_GPS_CopyToClipboard), toolTip: MyTexts.GetString(MySpaceTexts.TerminalTab_GPS_CopyToClipboard_ToolTip))
			{
				Name = "buttonToClipboard"
			};
			num4 += myGuiControlButton4.Size.Y * 1.1f;
			MyGuiControlCheckbox myGuiControlCheckbox2 = new MyGuiControlCheckbox(new Vector2(num3, num4), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
			{
				Name = "checkInsAlwaysVisible"
			};
			myGuiControlCheckbox2.SetToolTip(MySpaceTexts.TerminalTab_GPS_AlwaysVisible_Tooltip);
			MyGuiControlLabel control8 = new MyGuiControlLabel(new Vector2(num3 + myGuiControlCheckbox.Size.X + num, num4), myGuiControlCheckbox.Size - new Vector2(0.01f, 0.01f))
			{
				Name = "labelInsAlwaysVisible",
				Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_AlwaysVisible).ToString()
			};
			num4 += myGuiControlCheckbox.Size.Y;
			MyGuiControlLabel control9 = new MyGuiControlLabel(new Vector2(num3 + num, num4), new Vector2(0.288000017f, 0.035f))
			{
				Name = "TerminalTab_GPS_SaveWarning",
				Text = MyTexts.Get(MySpaceTexts.TerminalTab_GPS_SaveWarning).ToString(),
				ColorMask = Color.Red.ToVector4()
			};
			gpsPage.Controls.Add(myGuiControlTextbox);
			gpsPage.Controls.Add(myGuiControlLabel);
			gpsPage.Controls.Add(myGuiControlLabel2);
			gpsPage.Controls.Add(myGuiControlMultilineEditableText);
			gpsPage.Controls.Add(myGuiControlLabel3);
			gpsPage.Controls.Add(myGuiControlLabel4);
			gpsPage.Controls.Add(myGuiControlTextbox2);
			gpsPage.Controls.Add(control4);
			gpsPage.Controls.Add(myGuiControlTextbox3);
			gpsPage.Controls.Add(control5);
			gpsPage.Controls.Add(control6);
			gpsPage.Controls.Add(myGuiControlButton4);
			gpsPage.Controls.Add(myGuiControlCheckbox);
			gpsPage.Controls.Add(control7);
			gpsPage.Controls.Add(control9);
			gpsPage.Controls.Add(myGuiControlCheckbox2);
			gpsPage.Controls.Add(control8);
			m_defaultFocusedControl[MyTerminalPageEnum.Gps] = myGuiControlButton3;
		}

		private void CreatePropertiesPageControls(MyGuiControlParent menuParent, MyGuiControlParent panelParent)
		{
			m_propertiesTableParent.Name = "PropertiesTable";
			m_propertiesTopMenuParent.Name = "PropertiesTopMenu";
			MyGuiControlCombobox myGuiControlCombobox = new MyGuiControlCombobox
			{
				Position = new Vector2(0f, 0f),
				Size = new Vector2(0.25f, 0.1f),
				Name = "ShipsInRange",
				Visible = false,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			myGuiControlCombobox.SetToolTip(MySpaceTexts.ScreenTerminal_ShipCombobox);
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton
			{
				Position = new Vector2(0.258f, 0.004f),
				Size = new Vector2(0.2f, 0.05f),
				Name = "SelectShip",
				Text = MyTexts.GetString(MySpaceTexts.Terminal_RemoteControl_Button),
				TextScale = 0.7005405f,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				VisualStyle = MyGuiControlButtonStyleEnum.Small
			};
			myGuiControlButton.SetToolTip(MySpaceTexts.ScreenTerminal_ShipList);
			menuParent.Controls.Add(myGuiControlCombobox);
			menuParent.Controls.Add(myGuiControlButton);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddVertical(new Vector2(0.164f, -0.31f), 0.675f, 0.002f);
			panelParent.Controls.Add(myGuiControlSeparatorList);
			MyGuiControlMultilineText control = new MyGuiControlMultilineText(null, null, null, "Blue", 0.8f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, selectable: false, showTextShadow: false, null, null)
			{
				Name = "InfoHelpMultilineText",
				Position = new Vector2(0.186f, -0.31f),
				Size = new Vector2(0.3f, 0.68f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = new StringBuilder(MyTexts.GetString(MySpaceTexts.RemoteAccess_Description))
			};
			panelParent.Controls.Add(control);
			MyGuiControlTable myGuiControlTable = new MyGuiControlTable
			{
				Position = new Vector2(-0.142f, -0.31f),
				Size = new Vector2(0.582f, 0.88f),
				Name = "ShipsData",
				ColumnsCount = 5,
				VisibleRowsCount = 19,
				HeaderVisible = true,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
			};
			myGuiControlTable.SetCustomColumnWidths(new float[5]
			{
				0.267f,
				0.15f,
				0.15f,
				0.22f,
				0.22f
			});
			myGuiControlTable.SetColumnName(0, MyTexts.Get(MySpaceTexts.TerminalName));
			myGuiControlTable.SetColumnName(3, MyTexts.Get(MySpaceTexts.TerminalControl));
			myGuiControlTable.SetColumnName(1, MyTexts.Get(MySpaceTexts.TerminalDistance));
			myGuiControlTable.SetColumnName(2, MyTexts.Get(MySpaceTexts.TerminalStatus));
			myGuiControlTable.SetColumnName(4, MyTexts.Get(MySpaceTexts.TerminalAccess));
			myGuiControlTable.SetColumnComparison(0, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => a.Text.CompareTo(b.Text));
			myGuiControlTable.SetColumnComparison(1, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => ((double)a.UserData).CompareTo((double)b.UserData));
			panelParent.Controls.Add(myGuiControlTable);
			panelParent.Visible = false;
			IsRemote = false;
			m_defaultFocusedControl[MyTerminalPageEnum.Properties] = null;
		}

		public override bool Draw()
		{
			if (m_controllerInfo != null)
			{
				m_controllerInfo.UpdateBeforeDraw();
			}
			if (m_controllerInventory != null)
			{
				m_controllerInventory.UpdateBeforeDraw();
			}
			return base.Draw();
		}

		protected override void OnClosed()
		{
			if (m_instance != null)
			{
				MyGuiScreenGamePlay.ActiveGameplayScreen = null;
				m_interactedEntity = null;
				MyAnalyticsHelper.ReportActivityEnd(m_instance.m_user, "show_terminal");
				if (MyFakes.ENABLE_GPS)
				{
					m_controllerGps.Close();
				}
				m_controllerControlPanel.Close();
				if (m_controllerInventory != null)
				{
					m_controllerInventory.Close();
				}
				m_controllerProduction.Close();
				m_controllerInfo.Close();
				Controls.Clear();
				m_terminalTabs = null;
				m_controllerInventory = null;
				if (MyFakes.SHOW_FACTIONS_GUI)
				{
					m_controllerFactions.Close();
				}
				if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
				{
					m_controllerProperties.Close();
					m_controllerProperties.ButtonClicked -= PropertiesButtonClicked;
					m_propertiesTableParent = null;
					m_propertiesTopMenuParent = null;
				}
				if (MyFakes.ENABLE_COMMUNICATION)
				{
					m_controllerChat.Close();
				}
				if (m_requestedEntities.Count > 0)
				{
					MyMultiplayer.GetReplicationClient().OnReplicableReady -= InvokeWhenLoaded;
				}
				foreach (KeyValuePair<long, Action<long>> requestedEntity in m_requestedEntities)
				{
					MyMultiplayer.GetReplicationClient()?.RequestReplicable(requestedEntity.Key, 0, add: false);
				}
				m_requestedEntities.Clear();
				m_instance = null;
				m_screenOpen = false;
				base.OnClosed();
			}
		}

		private void InfoButton_OnButtonClick(MyGuiControlButton sender)
		{
			MyGuiSandbox.OpenUrlWithFallback(MySteamConstants.URL_HELP_TERMINAL_SCREEN, "Steam Guide");
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			bool num = base.FocusedControl is MyGuiControlTextbox;
			if (!num && (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TERMINAL) || MyInput.Static.IsNewGameControlPressed(MyControlsSpace.USE)))
			{
				MyGuiSoundManager.PlaySound(m_closingCueEnum.HasValue ? m_closingCueEnum.Value : GuiSounds.MouseClick);
				CloseScreen();
			}
			if (!num && MyInput.Static.IsNewGameControlPressed(MyControlsSpace.INVENTORY))
			{
				if (m_terminalTabs.SelectedPage == 0)
				{
					MyGuiSoundManager.PlaySound(m_closingCueEnum.HasValue ? m_closingCueEnum.Value : GuiSounds.MouseClick);
					CloseScreen();
				}
				else
				{
					SwitchToInventory();
				}
			}
			if (!num && MyInput.Static.IsNewGameControlPressed(MyControlsSpace.PAUSE_GAME))
			{
				MySandboxGame.PauseToggle();
			}
			if (!num && MyInput.Static.IsAnyCtrlKeyPressed() && MyInput.Static.IsKeyPress(MyKeys.A) && m_instance.m_terminalTabs.SelectedPage == 1)
			{
				m_controllerControlPanel.SelectAllBlocks();
			}
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
		}

		public void PropertiesButtonClicked()
		{
			m_terminalTabs.SelectedPage = -1;
			m_controllerProperties.Refresh();
			m_propertiesTableParent.Visible = true;
		}

		public void Info_ShipRenamed()
		{
			m_controllerProperties.Refresh();
		}

		public void tabs_OnPageChanged()
		{
			if (m_instance != null)
			{
				if (MyVRage.Platform.ImeProcessor != null)
				{
					MyVRage.Platform.ImeProcessor.Deactivate();
				}
				if (m_propertiesTableParent.Visible && m_instance.m_terminalTabs.SelectedPage != -1)
				{
					m_propertiesTableParent.Visible = false;
				}
				if (m_instance.m_terminalTabs.SelectedPage == 0 && m_instance.m_controllerInventory != null)
				{
					m_instance.m_controllerInventory.Refresh();
				}
				if (m_instance.m_terminalTabs.SelectedPage == 3)
				{
					m_controllerInfo?.MarkControlsDirty();
				}
				if (m_instance.m_terminalTabs.SelectedPage != 0)
				{
					base.FocusedControl = m_defaultFocusedControl[(MyTerminalPageEnum)m_terminalTabs.SelectedPage];
				}
				else
				{
					base.FocusedControl = m_controllerInventory.GetDefaultFocus();
				}
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (m_terminalControllers.TryGetValue((MyTerminalPageEnum)m_terminalTabs.SelectedPage, out MyTerminalController value))
			{
				value.HandleInput();
			}
			if (m_terminalTabs.SelectedPage == -1)
			{
				m_controllerProperties.HandleInput();
			}
			base.HandleInput(receivedFocusInThisUpdate);
		}

		public static void Show(MyTerminalPageEnum page, MyCharacter user, MyEntity interactedEntity)
		{
			if (!MyPerGameSettings.TerminalEnabled || !MyPerGameSettings.GUI.EnableTerminalScreen)
			{
				return;
			}
			bool flag = MyInput.Static.IsAnyShiftKeyPressed();
			if (m_instance == null)
			{
				m_instance = new MyGuiScreenTerminal();
				m_instance.m_user = user;
				m_openInventoryInteractedEntity = interactedEntity;
				if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
				{
					m_instance.m_initialPage = (flag ? MyTerminalPageEnum.Properties : page);
				}
				else
				{
					m_instance.m_initialPage = page;
				}
				InteractedEntity = interactedEntity;
				m_instance.RecreateControls(constructor: true);
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_instance);
				m_screenOpen = true;
				string activityFocus = (interactedEntity != null) ? interactedEntity.GetType().Name : "";
				MyAnalyticsHelper.ReportActivityStart(user, "show_terminal", activityFocus, "gui", string.Empty);
			}
		}

		internal static void Hide()
		{
			if (m_instance != null)
			{
				m_instance.CloseScreen();
			}
		}

		public static void ChangeInteractedEntity(MyEntity interactedEntity, bool isRemote)
		{
			IsRemote = isRemote;
			InteractedEntity = interactedEntity;
		}

		public static MyGuiControlLabel CreateErrorLabel(MyStringId text, string name)
		{
			return new MyGuiControlLabel(null, null, MyTexts.GetString(text), null, 1.2f, "Red", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER)
			{
				Name = name,
				Visible = true
			};
		}

		public static void SwitchToControlPanelBlock(MyTerminalBlock block)
		{
			m_instance.m_terminalTabs.SelectedPage = 1;
			m_instance.m_controllerControlPanel.SelectBlocks(new MyTerminalBlock[1]
			{
				block
			});
		}

		public static void SwitchToInventory(MyTerminalBlock block = null)
		{
			m_instance.m_terminalTabs.SelectedPage = 0;
			if (m_instance.m_controllerInventory != null && m_interactedEntity != block && block != null)
			{
				m_instance.m_controllerInventory.SetSearch(block.DisplayNameText);
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (MyFakes.ENABLE_TERMINAL_PROPERTIES)
			{
				if (m_connected && m_terminalTabs.SelectedPage != -1 && !m_controllerProperties.TestConnection())
				{
					m_connected = false;
					ShowDisconnectScreen();
				}
				else if (!m_connected && m_controllerProperties.TestConnection())
				{
					m_connected = true;
					ShowConnectScreen();
				}
				m_controllerProperties.Update();
				if (MyFakes.ENABLE_COMMUNICATION)
				{
					m_controllerChat.Update();
				}
			}
			MyCubeGrid myCubeGrid = (InteractedEntity != null && !InteractedEntity.Closed) ? (InteractedEntity.Parent as MyCubeGrid) : null;
			if (myCubeGrid != null && myCubeGrid.GridSystems.TerminalSystem != m_controllerControlPanel.TerminalSystem)
			{
				if (m_controllerControlPanel != null)
				{
					m_controllerControlPanel.Close();
					MyGuiControlTabPage controlsParent = (MyGuiControlTabPage)m_terminalTabs.Controls.GetControlByName("PageControlPanel");
					m_controllerControlPanel.Init(controlsParent, MySession.Static.LocalHumanPlayer, myCubeGrid, InteractedEntity as MyTerminalBlock, m_colorHelper);
				}
				if (m_controllerProduction != null)
				{
					m_controllerProduction.Close();
					MyGuiControlTabPage tabSubControl = m_terminalTabs.GetTabSubControl(2);
					m_controllerProduction.Init(tabSubControl, myCubeGrid, InteractedEntity as MyCubeBlock);
				}
				if (m_controllerInventory != null)
				{
					m_controllerInventory.Close();
					MyGuiControlTabPage controlsParent2 = (MyGuiControlTabPage)m_terminalTabs.Controls.GetControlByName("PageInventory");
					m_controllerInventory.Init(controlsParent2, m_user, InteractedEntity, m_colorHelper);
				}
			}
			m_controllerFactions.Update();
			return base.Update(hasFocus);
		}

		public void ShowDisconnectScreen()
		{
			m_terminalTabs.Visible = false;
			m_propertiesTableParent.Visible = false;
			m_terminalNotConnected.Visible = true;
		}

		public void ShowConnectScreen()
		{
			m_terminalTabs.Visible = true;
			m_propertiesTableParent.Visible = (m_terminalTabs.SelectedPage == -1);
			m_terminalNotConnected.Visible = false;
		}

		private void InvokeWhenLoaded(IMyReplicable replicable)
		{
			MyCubeGridReplicable myCubeGridReplicable = replicable as MyCubeGridReplicable;
			MyTerminalReplicable myTerminalReplicable = replicable as MyTerminalReplicable;
			long num = 0L;
			if (myCubeGridReplicable != null)
			{
				num = myCubeGridReplicable.Instance.EntityId;
			}
			else
			{
				if (myTerminalReplicable == null)
				{
					return;
				}
				num = myTerminalReplicable.Instance.EntityId;
			}
			foreach (KeyValuePair<long, Action<long>> requestedEntity in m_requestedEntities)
			{
				if (requestedEntity.Value != null && requestedEntity.Key == num)
				{
					requestedEntity.Value(num);
				}
			}
		}

		public static void RequestReplicable(long requestedId, long waitForId, Action<long> loadAction)
		{
			MyReplicationClient replicationClient = MyMultiplayer.GetReplicationClient();
			if (replicationClient != null && m_instance != null && !m_instance.m_requestedEntities.ContainsKey(requestedId))
			{
				replicationClient.RequestReplicable(requestedId, 0, add: true);
				if (m_instance.m_requestedEntities.Count == 0)
				{
					MyMultiplayer.GetReplicationClient().OnReplicableReady += m_instance.InvokeWhenLoaded;
				}
				m_instance.m_requestedEntities.Add(requestedId, (requestedId == waitForId) ? loadAction : null);
				if (requestedId != waitForId && !m_instance.m_requestedEntities.ContainsKey(waitForId))
				{
					m_instance.m_requestedEntities.Add(waitForId, loadAction);
				}
			}
		}

		public static MyTerminalPageEnum GetCurrentScreen()
		{
			if (IsOpen)
			{
				return (MyTerminalPageEnum)m_instance.m_terminalTabs.SelectedPage;
			}
			return MyTerminalPageEnum.None;
		}
	}
}
