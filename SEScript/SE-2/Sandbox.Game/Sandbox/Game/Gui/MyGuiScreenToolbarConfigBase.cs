#define VRAGE
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.GameServices;
using VRage.Input;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenToolbarConfigBase : MyGuiScreenBase
	{
		public enum GroupModes
		{
			Default,
			HideEmpty,
			HideBlockGroups,
			HideAll
		}

		public class GridItemUserData
		{
			public Func<MyObjectBuilder_ToolbarItem> ItemData;

			public Action Action;
		}

		public static MyGuiScreenToolbarConfigBase Static;

		protected MyGuiControlSearchBox m_searchBox;

		protected MyGuiControlListbox m_categoriesListbox;

		protected MyGuiControlGrid m_gridBlocks;

		protected MyGuiControlBlockGroupInfo m_blockGroupInfo;

		protected MyGuiControlScrollablePanel m_gridBlocksPanel;

		protected MyGuiControlScrollablePanel m_researchPanel;

		protected MyGuiControlResearchGraph m_researchGraph;

		protected MyGuiControlLabel m_blocksLabel;

		protected MyGuiControlGridDragAndDrop m_dragAndDrop;

		protected MyGuiControlToolbar m_toolbarControl;

		protected MyGuiControlContextMenu m_contextMenu;

		protected MyGuiControlContextMenu m_onDropContextMenu;

		protected MyObjectBuilder_ToolbarControlVisualStyle m_toolbarStyle;

		protected MyGuiControlTabControl m_tabControl;

		private MyShipController m_shipController;

		protected MyCharacter m_character;

		protected MyCubeGrid m_screenCubeGrid;

		protected const string SHIP_GROUPS_NAME = "Groups";

		protected const string CHARACTER_ANIMATIONS_GROUP_NAME = "CharacterAnimations";

		protected MyStringHash manipulationToolId = MyStringHash.GetOrCompute("ManipulationTool");

		protected string[] m_forcedCategoryOrder = new string[8]
		{
			"ShipWeapons",
			"ShipTools",
			"Weapons",
			"Tools",
			"CharacterWeapons",
			"CharacterTools",
			"CharacterAnimations",
			"Groups"
		};

		protected MySearchByStringCondition m_nameSearchCondition = new MySearchByStringCondition();

		protected MySearchByCategoryCondition m_categorySearchCondition = new MySearchByCategoryCondition();

		protected SortedDictionary<string, MyGuiBlockCategoryDefinition> m_sortedCategories = new SortedDictionary<string, MyGuiBlockCategoryDefinition>();

		protected static List<MyGuiBlockCategoryDefinition> m_allSelectedCategories = new List<MyGuiBlockCategoryDefinition>();

		protected List<MyGuiBlockCategoryDefinition> m_searchInBlockCategories = new List<MyGuiBlockCategoryDefinition>();

		private HashSet<string> m_tmpUniqueStrings = new HashSet<string>();

		protected MyGuiBlockCategoryDefinition m_shipGroupsCategory = new MyGuiBlockCategoryDefinition();

		protected float m_scrollOffset;

		protected static float m_savedVPosition = 0f;

		protected int m_contextBlockX = -1;

		protected int m_contextBlockY = -1;

		protected int m_onDropContextMenuToolbarIndex = -1;

		protected MyToolbarItem m_onDropContextMenuItem;

		protected bool m_shipMode;

		private MyGuiControlLabel m_categoryHintLeft;

		private MyGuiControlLabel m_categoryHintRight;

		public static GroupModes GroupMode = GroupModes.HideEmpty;

		protected MyCubeBlock m_screenOwner;

		protected static bool m_ownerChanged = false;

		protected static MyEntity m_previousOwner = null;

		private int m_framesBeforeSearchEnabled = 5;

		private ConditionBase m_visibleCondition;

		protected MyGuiControlPcuBar m_PCUControl;

		private int m_frameCounterPCU;

		private readonly int PCU_UPDATE_EACH_N_FRAMES = 1;

		private readonly List<int> m_blockOffsets = new List<int>();

		protected int? m_gamepadSlot;

		private float m_minVerticalPosition;

		private bool m_researchItemFound;

		public MyGuiScreenToolbarConfigBase(MyObjectBuilder_ToolbarControlVisualStyle toolbarStyle, int scrollOffset = 0, MyCubeBlock owner = null, int? gamepadSlot = null)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, null, isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity, gamepadSlot)
		{
			MySandboxGame.Log.WriteLine("MyGuiScreenCubeBuilder.ctor START");
			Static = this;
			m_toolbarStyle = toolbarStyle;
			m_visibleCondition = m_toolbarStyle.VisibleCondition;
			m_toolbarStyle.VisibleCondition = null;
			m_scrollOffset = (float)scrollOffset / 6.5f;
			m_size = new Vector2(1f, 1f);
			m_canShareInput = true;
			m_drawEvenWithoutFocus = true;
			base.EnabledBackgroundFade = true;
			m_screenOwner = owner;
			GetType();
			if (typeof(MyGuiScreenToolbarConfigBase) == GetType())
			{
				RecreateControls(contructor: true);
			}
			m_framesBeforeSearchEnabled = 10;
			m_gamepadSlot = gamepadSlot;
			MySandboxGame.Log.WriteLine("MyGuiScreenCubeBuilder.ctor END");
		}

		protected override void OnClosed()
		{
			m_toolbarStyle.VisibleCondition = m_visibleCondition;
			Static = null;
			base.OnClosed();
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
		}

		public static void Reset()
		{
			m_allSelectedCategories.Clear();
		}

		public override bool RegisterClicks()
		{
			return true;
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT) && SelectedCategoryMove(m_categoriesListbox, -1))
			{
				categories_ItemClicked(m_categoriesListbox);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT) && SelectedCategoryMove(m_categoriesListbox))
			{
				categories_ItemClicked(m_categoriesListbox);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1) && m_gamepadSlot.HasValue)
			{
				MyToolbarComponent.CurrentToolbar.SetItemAtIndex(m_gamepadSlot.Value, null, gamepad: true);
				CloseScreen();
			}
			if (base.FocusedControl == null && MyInput.Static.IsKeyPress(MyKeys.Tab))
			{
				if (MyVRage.Platform.ImeProcessor != null)
				{
					MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
				}
				base.FocusedControl = m_searchBox.TextBox;
			}
			if (MyInput.Static.IsMouseReleased(MyMouseButtonsEnum.Right))
			{
				if (m_onDropContextMenu.Enabled)
				{
					m_onDropContextMenu.Enabled = false;
					m_contextMenu.Enabled = false;
					m_onDropContextMenu.Activate();
				}
				else if (m_contextMenu.Enabled && !m_onDropContextMenu.Visible)
				{
					m_contextMenu.Enabled = false;
					m_contextMenu.Activate();
					base.FocusedControl = m_contextMenu;
				}
			}
			if (!MyInput.Static.IsNewGameControlPressed(MyControlsSpace.BUILD_SCREEN))
			{
				return;
			}
			if (!m_searchBox.TextBox.HasFocus)
			{
				if (m_closingCueEnum.HasValue)
				{
					MyGuiSoundManager.PlaySound(m_closingCueEnum.Value);
				}
				else
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				}
				CloseScreen();
			}
			else if (MyInput.Static.IsNewGameControlJoystickOnlyPressed(MyControlsSpace.BUILD_SCREEN))
			{
				if (m_closingCueEnum.HasValue)
				{
					MyGuiSoundManager.PlaySound(m_closingCueEnum.Value);
				}
				else
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				}
				CloseScreen();
			}
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.PAUSE_GAME))
			{
				MySandboxGame.PauseToggle();
			}
		}

		public override bool CloseScreen()
		{
			m_savedVPosition = m_gridBlocksPanel.ScrollbarVPosition;
			Static = null;
			MyAnalyticsHelper.ReportActivityEnd(null, "show_toolbar_config");
			return base.CloseScreen();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenToolbarConfigBase";
		}

		public override void RecreateControls(bool contructor)
		{
			base.RecreateControls(contructor);
			MyAnalyticsHelper.ReportActivityStart(null, "show_toolbar_config", string.Empty, "gui", string.Empty);
			m_character = null;
			m_shipController = null;
			m_ownerChanged = (m_previousOwner != MyToolbarComponent.CurrentToolbar.Owner);
			m_previousOwner = MyToolbarComponent.CurrentToolbar.Owner;
			if (MyToolbarComponent.CurrentToolbar.Owner == null)
			{
				m_character = MySession.Static.LocalCharacter;
			}
			else
			{
				m_shipController = (MyToolbarComponent.CurrentToolbar.Owner as MyShipController);
			}
			m_screenCubeGrid = ((m_screenOwner == null) ? null : m_screenOwner.CubeGrid);
			bool flag = m_screenCubeGrid != null;
			string path = Path.Combine("Data", "Screens", "CubeBuilder.gsc");
			MyObjectBuilderSerializer.DeserializeXML(Path.Combine(MyFileSystem.ContentPath, path), out MyObjectBuilder_GuiScreen objectBuilder);
			Init(objectBuilder);
			m_tabControl = (Controls.GetControlByName("Tab") as MyGuiControlTabControl);
			m_tabControl.TabButtonScale = 0.5f;
			m_tabControl.ButtonsOffset = new Vector2(0f, -0.03f);
			MyGuiControlTabPage myGuiControlTabPage = m_tabControl.Controls.GetControlByName("BlocksPage") as MyGuiControlTabPage;
			m_gridBlocks = (MyGuiControlGrid)myGuiControlTabPage.Controls.GetControlByName("Grid");
			Vector4 colorMask = m_gridBlocks.ColorMask;
			m_gridBlocks.ColorMask = Vector4.One;
			m_gridBlocks.ItemBackgroundColorMask = colorMask;
			m_categoriesListbox = (MyGuiControlListbox)Controls.GetControlByName("CategorySelector");
			m_categoriesListbox.CanHaveFocus = false;
			m_categoriesListbox.VisualStyle = MyGuiControlListboxStyleEnum.ToolsBlocks;
			m_categoriesListbox.ItemClicked += categories_ItemClicked;
			MyGuiControlTextbox myGuiControlTextbox = (MyGuiControlTextbox)Controls.GetControlByName("SearchItemTextBox");
			MyGuiControlLabel control = (MyGuiControlLabel)Controls.GetControlByName("BlockSearchLabel");
			m_searchBox = new MyGuiControlSearchBox(myGuiControlTextbox.Position + new Vector2(-0.1f, 0f), myGuiControlTextbox.Size + new Vector2(0.2f, 0f))
			{
				CanHaveFocus = false
			};
			m_searchBox.OnTextChanged += searchItemTexbox_TextChanged;
			m_searchBox.Enabled = false;
			MyGuiControlTextbox textBox = m_searchBox.TextBox;
			MyGuiControlTextbox.MySkipCombination[] array = new MyGuiControlTextbox.MySkipCombination[2];
			MyGuiControlTextbox.MySkipCombination mySkipCombination = array[0] = new MyGuiControlTextbox.MySkipCombination
			{
				Ctrl = true,
				Keys = null
			};
			mySkipCombination = (array[1] = new MyGuiControlTextbox.MySkipCombination
			{
				Keys = new MyKeys[2]
				{
					MyKeys.Snapshot,
					MyKeys.Delete
				}
			});
			textBox.SkipCombinations = array;
			Controls.Add(m_searchBox);
			Controls.Remove(myGuiControlTextbox);
			Controls.Remove(control);
			myGuiControlTabPage.Controls.Remove(m_gridBlocks);
			m_gridBlocks.VisualStyle = MyGuiControlGridStyleEnum.Toolbar;
			m_gridBlocksPanel = new MyGuiControlScrollablePanel(null);
			MyGuiStyleDefinition visualStyle = MyGuiControlGrid.GetVisualStyle(MyGuiControlGridStyleEnum.ToolsBlocks);
			m_gridBlocksPanel.BackgroundTexture = visualStyle.BackgroundTexture;
			m_gridBlocksPanel.ColorMask = colorMask;
			m_gridBlocksPanel.ScrolledControl = m_gridBlocks;
			m_gridBlocksPanel.ScrollbarVEnabled = true;
			m_gridBlocksPanel.ScrolledAreaPadding = new MyGuiBorderThickness(10f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 10f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			m_gridBlocksPanel.FitSizeToScrolledControl();
			m_gridBlocksPanel.Size += new Vector2(0f, 0.032f);
			m_gridBlocksPanel.PanelScrolled += grid_PanelScrolled;
			m_gridBlocksPanel.Position = new Vector2(-0.216f, -0.044f);
			myGuiControlTabPage.Controls.Add(m_gridBlocksPanel);
			Vector2 value = new Vector2(-0.495f, -0.52f) + m_categoriesListbox.GetPositionAbsoluteTopLeft();
			Vector2 value2 = new Vector2(-0.505f, -0.52f) + m_categoriesListbox.GetPositionAbsoluteTopRight();
			m_categoryHintLeft = new MyGuiControlLabel(value, null, '\ue008'.ToString());
			m_categoryHintRight = new MyGuiControlLabel(value2, null, '\ue007'.ToString(), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			Controls.Add(m_categoryHintLeft);
			Controls.Add(m_categoryHintRight);
			if (m_scrollOffset != 0f)
			{
				m_gridBlocksPanel.SetPageVertical(m_scrollOffset);
			}
			else
			{
				m_gridBlocksPanel.ScrollbarVPosition = m_savedVPosition;
			}
			m_researchGraph = new MyGuiControlResearchGraph();
			m_researchGraph.ItemSize = new Vector2(41f / 800f, 0.06833334f) * 0.75f;
			m_researchGraph.NodePadding = m_researchGraph.ItemSize / 7f;
			m_researchGraph.NodeMargin = m_researchGraph.ItemSize / 7f;
			m_researchGraph.Size = new Vector2(0.52f, 0f);
			m_researchGraph.ItemClicked += m_researchGraph_ItemClicked;
			m_researchGraph.ItemDoubleClicked += m_researchGraph_ItemDoubleClicked;
			m_researchGraph.ItemDragged += m_researchGraph_ItemDragged;
			m_researchGraph.Nodes = CreateResearchGraph();
			m_researchGraph.SelectedItem = m_researchGraph.Nodes[0].Items[0];
			MyGuiControlTabPage myGuiControlTabPage2 = m_tabControl.Controls.GetControlByName("ResearchPage") as MyGuiControlTabPage;
			if (MySession.Static != null && MySession.Static.Settings != null && MySession.Static.Settings.EnableResearch)
			{
				myGuiControlTabPage2.SetToolTip((string)null);
				myGuiControlTabPage2.Enabled = true;
			}
			else
			{
				myGuiControlTabPage2.SetToolTip(MySpaceTexts.ToolbarConfig_ResearchTabDisabledTooltip);
				myGuiControlTabPage2.Enabled = false;
			}
			m_researchPanel = new MyGuiControlScrollablePanel(null);
			m_researchPanel.BackgroundTexture = visualStyle.BackgroundTexture;
			m_researchPanel.ColorMask = colorMask;
			m_researchPanel.ScrolledControl = m_researchGraph;
			m_researchPanel.ScrollbarVEnabled = true;
			m_researchPanel.ScrollbarHEnabled = true;
			m_researchPanel.ScrolledAreaPadding = new MyGuiBorderThickness(10f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 10f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			m_researchPanel.FitSizeToScrolledControl();
			m_researchPanel.Size = m_gridBlocksPanel.Size;
			m_researchPanel.Position = m_gridBlocksPanel.Position;
			myGuiControlTabPage2.Controls.Add(m_researchPanel);
			MyGuiControlLabel myGuiControlLabel = (MyGuiControlLabel)Controls.GetControlByName("LabelToolbar");
			myGuiControlLabel.Position = new Vector2(myGuiControlLabel.Position.X - 0.12f, myGuiControlLabel.Position.Y);
			m_toolbarControl = (MyGuiControlToolbar)Activator.CreateInstance(MyPerGameSettings.GUI.ToolbarControl, m_toolbarStyle);
			m_toolbarControl.Position = m_toolbarStyle.CenterPosition - new Vector2(0.62f, 0.5f);
			m_toolbarControl.OriginAlign = m_toolbarStyle.OriginAlign;
			Controls.Add(m_toolbarControl);
			if (m_gamepadSlot.HasValue)
			{
				myGuiControlLabel.Visible = false;
				m_toolbarControl.Visible = false;
			}
			m_onDropContextMenu = new MyGuiControlContextMenu();
			m_onDropContextMenu.Deactivate();
			m_onDropContextMenu.ItemClicked += onDropContextMenu_ItemClicked;
			m_onDropContextMenu.OnDeactivated += contextMenu_Deactivated;
			Controls.Add(m_onDropContextMenu);
			m_gridBlocks.SetItemsToDefault();
			m_gridBlocks.ItemDoubleClicked += grid_ItemDoubleClicked;
			m_gridBlocks.ItemClicked += grid_ItemClicked;
			m_gridBlocks.ItemDragged += grid_OnDrag;
			m_gridBlocks.ItemAccepted += grid_ItemDoubleClicked;
			m_dragAndDrop = new MyGuiControlGridDragAndDrop(MyGuiConstants.DRAG_AND_DROP_BACKGROUND_COLOR, MyGuiConstants.DRAG_AND_DROP_TEXT_COLOR, 0.7f, MyGuiConstants.DRAG_AND_DROP_TEXT_OFFSET, supportIcon: true);
			m_dragAndDrop.ItemDropped += dragAndDrop_OnDrop;
			m_dragAndDrop.DrawBackgroundTexture = false;
			Controls.Add(m_dragAndDrop);
			m_contextMenu = new MyGuiControlContextMenu();
			m_contextMenu.ItemClicked += contextMenu_ItemClicked;
			m_contextMenu.OnDeactivated += contextMenu_Deactivated;
			Controls.Add(m_contextMenu);
			m_contextMenu.Deactivate();
			m_PCUControl = new MyGuiControlPcuBar(new Vector2(0.153f, 0.4f))
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
			Controls.Add(m_PCUControl);
			m_PCUControl.UpdatePCU(GetIdentity(), performAnimation: false);
			SolveAspectRatio();
			AddCategoryToDisplayList(MyTexts.GetString(MySpaceTexts.DisplayName_Category_AllBlocks), null);
			Dictionary<string, MyGuiBlockCategoryDefinition> categories = MyDefinitionManager.Static.GetCategories();
			if (m_screenCubeGrid != null && (m_shipController == null || (m_shipController != null && !m_shipController.BuildingMode)))
			{
				if (!flag || m_shipController == null || m_shipController.EnableShipControl)
				{
					RecreateShipCategories(categories, m_sortedCategories, m_screenCubeGrid);
					AddShipGroupsIntoCategoryList(m_screenCubeGrid);
					AddShipBlocksDefinitions(m_screenCubeGrid, flag, null);
					AddShipGunsToCategories(categories, m_sortedCategories);
				}
				else
				{
					m_categoriesListbox.Items.Clear();
				}
				if (m_shipController != null && m_shipController.ToolbarType != MyToolbarType.None)
				{
					MyGuiBlockCategoryDefinition value3 = null;
					if (!m_sortedCategories.TryGetValue("CharacterAnimations", out value3) && categories.TryGetValue("CharacterAnimations", out value3))
					{
						m_sortedCategories.Add("CharacterAnimations", value3);
					}
				}
				m_researchGraph.Nodes.Clear();
				MyGuiControlTabPage tab = m_tabControl.GetTab(1);
				bool isTabVisible = m_tabControl.GetTab(1).Enabled = false;
				tab.IsTabVisible = isTabVisible;
				m_shipMode = true;
				m_PCUControl.Visible = false;
				m_PCUControl.Controls.Clear();
			}
			else if (m_character != null || (m_shipController != null && m_shipController.BuildingMode))
			{
				if (GroupMode != GroupModes.HideAll)
				{
					RecreateBlockCategories(categories, m_sortedCategories);
				}
				AddCubeDefinitionsToBlocks(m_categorySearchCondition);
				MyGuiControlTabPage tab2 = m_tabControl.GetTab(1);
				bool isTabVisible = m_tabControl.GetTab(1).Enabled = true;
				tab2.IsTabVisible = isTabVisible;
				m_shipMode = false;
				m_PCUControl.Visible = true;
			}
			if (MyFakes.ENABLE_SHIP_BLOCKS_TOOLBAR)
			{
				m_gridBlocks.Visible = true;
				m_gridBlocksPanel.ScrollbarVEnabled = true;
			}
			else
			{
				m_gridBlocksPanel.ScrollbarVEnabled = !flag;
				m_gridBlocks.Visible = !flag;
			}
			SortCategoriesToDisplayList();
			if (m_categoriesListbox.Items.Count > 0)
			{
				SelectCategories();
			}
			if (m_gamepadSlot.HasValue)
			{
				m_framesBeforeSearchEnabled = -1;
				base.FocusedControl = ((m_tabControl.SelectedPage == 0) ? ((MyGuiControlBase)m_gridBlocks) : ((MyGuiControlBase)m_researchGraph));
			}
			if (!m_gamepadSlot.HasValue)
			{
				return;
			}
			char c = ' ';
			switch (m_gamepadSlot % 4)
			{
			case 0:
				c = '\ue011';
				break;
			case 1:
				c = '\ue010';
				break;
			case 2:
				c = '\ue012';
				break;
			case 3:
				c = '\ue013';
				break;
			}
			string text = string.Format(MyTexts.GetString(MyCommonTexts.Gamepad_GScreen_Caption), c.ToString());
			Vector2 vector = new Vector2(0.265f, -0.29f);
			MyGuiControlLabel control2 = new MyGuiControlLabel(vector, null, text);
			Controls.Add(control2);
			new Vector2(0.085f, 0f);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(vector + new Vector2(0f, 0.035f), 0.22f);
			Controls.Add(myGuiControlSeparatorList);
			char c2 = '\ue00f';
			string codeForControl = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT);
			string codeForControl2 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACTION1);
			string text2 = string.Format(MyTexts.GetString(MyCommonTexts.Gamepad_GScreen_Hint), c2.ToString(), codeForControl, codeForControl2);
			Vector2 value4 = new Vector2(0f, 0.59f);
			MyGuiControlLabel control3 = new MyGuiControlLabel(vector + value4, null, text2, null, 0.67f);
			Controls.Add(control3);
			Vector2 vector2 = new Vector2(0.29f, -0.2f);
			new Vector2(0.027f, -0.26f);
			Vector2 vector3 = new Vector2(72f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(vector2, vector3);
			myGuiControlImage.SetTexture("Textures\\GUI\\Controls\\grid_item_highlight.dds");
			MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(vector2, vector3);
			myGuiControlImage2.SetTextures(MyToolbarComponent.CurrentToolbar.GetItemIconsGamepad(m_gamepadSlot.Value % 4));
			myGuiControlImage2.ColorMask = MyToolbarComponent.CurrentToolbar.GetItemIconsColormaskGamepad(m_gamepadSlot.Value % 4);
			float num = 0.3f;
			MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage(vector2 + new Vector2(num * vector3.X, (0f - num) * vector3.Y), vector3 / 3f);
			myGuiControlImage3.SetTexture(MyToolbarComponent.CurrentToolbar.GetItemSubiconGamepad(m_gamepadSlot.Value % 4));
			MyToolbarItem itemAtIndexGamepad = MyToolbarComponent.CurrentToolbar.GetItemAtIndexGamepad(m_gamepadSlot.Value % 4);
			MyToolbarItemTerminalBlock myToolbarItemTerminalBlock;
			string text3;
			string text4;
			if (itemAtIndexGamepad != null && (myToolbarItemTerminalBlock = (itemAtIndexGamepad as MyToolbarItemTerminalBlock)) != null)
			{
				text3 = myToolbarItemTerminalBlock.GetBlockName();
				text4 = myToolbarItemTerminalBlock.GetActionName();
			}
			else
			{
				string text5 = MyToolbarComponent.CurrentToolbar.GetItemNameGamepad(m_gamepadSlot.Value % 4);
				if (text5 == null)
				{
					text5 = string.Empty;
				}
				text3 = text5.Trim();
				text4 = string.Empty;
			}
			float num2 = 0.009f;
			Vector2 value5 = new Vector2(0.032f, 0f - num2 - 0.011f);
			Vector2 value6 = new Vector2(0.032f, num2 - 0.011f);
			MyGuiControlLabel control4 = new MyGuiControlLabel(vector2 + value5, null, text3);
			MyGuiControlLabel control5 = new MyGuiControlLabel(vector2 + value6, null, text4);
			Controls.Add(myGuiControlImage);
			Controls.Add(myGuiControlImage2);
			Controls.Add(myGuiControlImage3);
			Controls.Add(control4);
			Controls.Add(control5);
		}

		private void contextMenu_Deactivated()
		{
			base.FocusedControl = m_gridBlocks;
		}

		private List<MyGuiControlResearchGraph.GraphNode> CreateResearchGraph()
		{
			List<MyGuiControlResearchGraph.GraphNode> list = new List<MyGuiControlResearchGraph.GraphNode>();
			Dictionary<string, MyGuiControlResearchGraph.GraphNode> dictionary = new Dictionary<string, MyGuiControlResearchGraph.GraphNode>();
			List<MyGuiControlResearchGraph.GraphNode> list2 = new List<MyGuiControlResearchGraph.GraphNode>();
			HashSet<SerializableDefinitionId> hashSet = new HashSet<SerializableDefinitionId>();
			foreach (MyResearchGroupDefinition researchGroupDefinition in MyDefinitionManager.Static.GetResearchGroupDefinitions())
			{
				HashSet<string> hashSet2 = new HashSet<string>();
				List<MyCubeBlockDefinition> list3 = new List<MyCubeBlockDefinition>();
				if (researchGroupDefinition.Members != null)
				{
					SerializableDefinitionId[] members = researchGroupDefinition.Members;
					foreach (SerializableDefinitionId serializableDefinitionId in members)
					{
						MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(serializableDefinitionId);
						if (cubeBlockDefinition != null && (cubeBlockDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (cubeBlockDefinition.AvailableInSurvival || !MySession.Static.SurvivalMode))
						{
							MyResearchBlockDefinition researchBlock = MyDefinitionManager.Static.GetResearchBlock(serializableDefinitionId);
							if (researchBlock != null && researchBlock.UnlockedByGroups != null && researchBlock.UnlockedByGroups.Length != 0)
							{
								string[] unlockedByGroups = researchBlock.UnlockedByGroups;
								foreach (string item in unlockedByGroups)
								{
									hashSet2.Add(item);
								}
							}
							hashSet.Add(serializableDefinitionId);
							MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition.BlockPairName);
							if (definitionGroup != null && (definitionGroup.Large == null || definitionGroup.Small == null || !(definitionGroup.Small.Id == cubeBlockDefinition.Id)))
							{
								list3.Add(cubeBlockDefinition);
							}
						}
					}
					if (hashSet2.Count == 0)
					{
						MyGuiControlResearchGraph.GraphNode graphNode = CreateNode(researchGroupDefinition, list3);
						list.Add(graphNode);
						dictionary.Add(graphNode.Name, graphNode);
					}
					else
					{
						foreach (string item2 in hashSet2)
						{
							MyGuiControlResearchGraph.GraphNode graphNode2 = CreateNode(researchGroupDefinition, list3, item2);
							list2.Add(graphNode2);
							if (!dictionary.ContainsKey(graphNode2.Name))
							{
								dictionary.Add(graphNode2.Name, graphNode2);
							}
						}
					}
				}
			}
			Dictionary<string, MyGuiControlResearchGraph.GraphNode> dictionary2 = new Dictionary<string, MyGuiControlResearchGraph.GraphNode>();
			foreach (MyResearchBlockDefinition researchBlockDefinition in MyDefinitionManager.Static.GetResearchBlockDefinitions())
			{
				if (!hashSet.Contains(researchBlockDefinition.Id))
				{
					MyCubeBlockDefinition cubeBlockDefinition2 = MyDefinitionManager.Static.GetCubeBlockDefinition(researchBlockDefinition.Id);
					if (cubeBlockDefinition2 != null && (cubeBlockDefinition2.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (cubeBlockDefinition2.AvailableInSurvival || !MySession.Static.SurvivalMode) && researchBlockDefinition.UnlockedByGroups != null)
					{
						string[] unlockedByGroups = researchBlockDefinition.UnlockedByGroups;
						foreach (string text in unlockedByGroups)
						{
							MyGuiControlResearchGraph.GraphNode value = null;
							if (!dictionary.TryGetValue(text, out value))
							{
								MyLog.Default.WriteLine($"Research group {text} was not found for block {researchBlockDefinition.Id}.");
							}
							else
							{
								MyCubeBlockDefinitionGroup definitionGroup2 = MyDefinitionManager.Static.GetDefinitionGroup(cubeBlockDefinition2.BlockPairName);
								if (definitionGroup2 != null && (definitionGroup2.Large == null || definitionGroup2.Small == null || !(definitionGroup2.Small.Id == cubeBlockDefinition2.Id)))
								{
									MyGuiControlResearchGraph.GraphNode value2 = null;
									if (!dictionary2.TryGetValue(text, out value2))
									{
										value2 = new MyGuiControlResearchGraph.GraphNode();
										dictionary2.Add(text, value2);
										value2.Name = "Common_" + text;
										value2.UnlockedBy = text;
										value.Children.Add(value2);
										value2.Parent = value;
									}
									CreateNodeItem(value2, cubeBlockDefinition2);
								}
							}
						}
					}
				}
			}
			CreateGraph(dictionary, list2);
			return list;
		}

		private static void CreateGraph(Dictionary<string, MyGuiControlResearchGraph.GraphNode> nodesByName, List<MyGuiControlResearchGraph.GraphNode> children)
		{
			int num = 0;
			while (children.Count != num)
			{
				bool flag = false;
				foreach (MyGuiControlResearchGraph.GraphNode child in children)
				{
					if (!string.IsNullOrEmpty(child.UnlockedBy))
					{
						MyGuiControlResearchGraph.GraphNode value = null;
						if (nodesByName.TryGetValue(child.UnlockedBy, out value))
						{
							num++;
							value.Children.Add(child);
							child.Parent = value;
							flag = true;
						}
					}
				}
				if (!flag)
				{
					break;
				}
			}
		}

		private MyGuiControlResearchGraph.GraphNode CreateNode(MyResearchGroupDefinition group, List<MyCubeBlockDefinition> items, string unlockedBy = null)
		{
			MyGuiControlResearchGraph.GraphNode graphNode = new MyGuiControlResearchGraph.GraphNode();
			graphNode.Name = group.Id.SubtypeName;
			graphNode.UnlockedBy = unlockedBy;
			foreach (MyCubeBlockDefinition item in items)
			{
				CreateNodeItem(graphNode, item);
			}
			return graphNode;
		}

		private void CreateNodeItem(MyGuiControlResearchGraph.GraphNode node, MyCubeBlockDefinition definition)
		{
			bool flag = !MySession.Static.ResearchEnabled || MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySessionComponentResearch.Static.CanUse(m_character ?? ((m_shipController != null) ? m_shipController.Pilot : null), definition.Id);
			string subicon = null;
			if (definition.BlockStages != null && definition.BlockStages.Length != 0)
			{
				subicon = MyGuiTextures.Static.GetTexture(MyHud.HudDefinition.Toolbar.ItemStyle.VariantTexture).Path;
			}
			string subIcon = null;
			bool flag2 = true;
			if (definition.DLCs != null && definition.DLCs.Length != 0)
			{
				MyDLCs.MyDLC firstMissingDefinitionDLC = MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
				MyDLCs.MyDLC dlc;
				if (firstMissingDefinitionDLC != null)
				{
					subIcon = firstMissingDefinitionDLC.Icon;
					flag2 = false;
				}
				else if (MyDLCs.TryGetDLC(definition.DLCs[0], out dlc))
				{
					subIcon = dlc.Icon;
				}
			}
			bool flag3 = MyToolbarComponent.GlobalBuilding || MySession.Static.ControlledEntity is MyCharacter || (MySession.Static.ControlledEntity is MyCockpit && (MySession.Static.ControlledEntity as MyCockpit).BuildingMode);
			flag3 = (flag3 && flag);
			flag3 = (flag3 && flag2);
			string[] icons = definition.Icons;
			string definitionTooltip = GetDefinitionTooltip(definition, flag);
			MyGuiGridItem myGuiGridItem = new MyGuiGridItem(icons, subicon, definitionTooltip, new GridItemUserData
			{
				ItemData = (() => MyToolbarItemFactory.ObjectBuilderFromDefinition(definition))
			}, flag3);
			myGuiGridItem.SubIcon2 = subIcon;
			myGuiGridItem.ItemDefinition = definition;
			myGuiGridItem.OverlayColorMask = new Vector4(0f, 1f, 0f, 0.25f);
			node.Items.Add(myGuiGridItem);
			m_researchGraph.ItemToNode.Add(myGuiGridItem, node);
		}

		private MyIdentity GetIdentity()
		{
			MyPlayer myPlayer = null;
			if (m_character != null)
			{
				myPlayer = MyPlayer.GetPlayerFromCharacter(m_character);
			}
			else if (m_shipController != null)
			{
				if (m_shipController.Pilot != null && m_shipController.ControllerInfo.Controller != null)
				{
					myPlayer = m_shipController.ControllerInfo.Controller.Player;
				}
			}
			else if (MySession.Static.LocalCharacter != null)
			{
				myPlayer = MyPlayer.GetPlayerFromCharacter(MySession.Static.LocalCharacter);
			}
			return myPlayer?.Identity;
		}

		private void SolveAspectRatio()
		{
			Rectangle fullscreenRectangle = MyGuiManager.GetFullscreenRectangle();
			switch (MyVideoSettingsManager.GetClosestAspectRatio((float)fullscreenRectangle.Width / (float)fullscreenRectangle.Height))
			{
			case MyAspectRatioEnum.Normal_4_3:
			case MyAspectRatioEnum.Unsupported_5_4:
				m_gridBlocks.ColumnsCount = 8;
				m_gridBlocksPanel.Size *= new Vector2(0.82f, 1f);
				m_researchPanel.Size = m_gridBlocksPanel.Size;
				m_researchGraph.Size = new Vector2(0.4f, 0f);
				m_researchGraph.InvalidateItemsLayout();
				m_categoriesListbox.PositionX *= 0.9f;
				Controls.GetControlByName("BlockInfoPanel").PositionX *= 0.78f;
				((MyGuiControlLabel)Controls.GetControlByName("CaptionLabel2")).PositionX *= 0.9f;
				((MyGuiControlLabel)Controls.GetControlByName("LabelSubtitle")).PositionX *= 0.9f;
				m_searchBox.PositionX *= 0.68f;
				break;
			}
			CalculateBlockOffsets();
		}

		private void CalculateBlockOffsets()
		{
			m_blockOffsets.Clear();
			foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
			{
				if (MyFakes.ENABLE_MULTIBLOCKS_IN_SURVIVAL || !MySession.Static.SurvivalMode || !definitionPairName.EndsWith("MultiBlock"))
				{
					MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
					Vector2I cubeBlockScreenPosition = MyDefinitionManager.Static.GetCubeBlockScreenPosition(definitionPairName);
					if (IsValidItem(definitionGroup) && (!MyFakes.ENABLE_GUI_HIDDEN_CUBEBLOCKS || definitionGroup.Any.GuiVisible) && definitionGroup.AnyPublic != null)
					{
						if (m_blockOffsets.Count <= cubeBlockScreenPosition.Y)
						{
							for (int i = m_blockOffsets.Count - 1; i < cubeBlockScreenPosition.Y; i++)
							{
								m_blockOffsets.Add(0);
							}
						}
						if (cubeBlockScreenPosition.Y >= 0)
						{
							m_blockOffsets[cubeBlockScreenPosition.Y]++;
						}
					}
				}
			}
			int num = 0;
			for (int j = 0; j < m_blockOffsets.Count; j++)
			{
				int num2 = (m_blockOffsets[j] - 1) / m_gridBlocks.ColumnsCount;
				num += num2;
				m_blockOffsets[j] = num;
			}
		}

		private void OnItemDragged(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			StartDragging(MyDropHandleType.MouseRelease, sender, ref eventArgs);
		}

		protected void SelectCategories()
		{
			List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
			if (m_allSelectedCategories.Count == 0 || m_ownerChanged)
			{
				list.Add(m_categoriesListbox.Items[0]);
			}
			else
			{
				foreach (MyGuiControlListbox.Item item in m_categoriesListbox.Items)
				{
					if (m_allSelectedCategories.Exists((MyGuiBlockCategoryDefinition x) => x == item.UserData))
					{
						list.Add(item);
					}
				}
			}
			m_allSelectedCategories.Clear();
			m_categoriesListbox.SelectedItems = list;
			categories_ItemClicked(m_categoriesListbox);
		}

		protected void SortCategoriesToDisplayList()
		{
			string[] forcedCategoryOrder = m_forcedCategoryOrder;
			foreach (string key in forcedCategoryOrder)
			{
				MyGuiBlockCategoryDefinition value = null;
				if (m_sortedCategories.TryGetValue(key, out value))
				{
					AddCategoryToDisplayList(value.DisplayNameText, value);
				}
			}
			foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> sortedCategory in m_sortedCategories)
			{
				if (!m_forcedCategoryOrder.Contains(sortedCategory.Key))
				{
					AddCategoryToDisplayList(sortedCategory.Value.DisplayNameText, sortedCategory.Value);
				}
			}
		}

		public void RecreateBlockCategories(Dictionary<string, MyGuiBlockCategoryDefinition> loadedCategories, SortedDictionary<string, MyGuiBlockCategoryDefinition> categories)
		{
			categories.Clear();
			foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> loadedCategory in loadedCategories)
			{
				loadedCategory.Value.ValidItems = 0;
			}
			if (MySession.Static.ResearchEnabled && !MySession.Static.CreativeToolsEnabled(Sync.MyId) && MySessionComponentResearch.Static.m_requiredResearch.Count > 0)
			{
				foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
				{
					MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
					if (IsValidItem(definitionGroup) && definitionGroup.AnyPublic != null && MySessionComponentResearch.Static.CanUse(m_character, definitionGroup.AnyPublic.Id))
					{
						foreach (MyGuiBlockCategoryDefinition value in loadedCategories.Values)
						{
							if (value.HasItem(definitionGroup.AnyPublic.Id.ToString()))
							{
								value.ValidItems++;
							}
						}
					}
				}
			}
			MyPlayer myPlayer = null;
			if (m_shipController != null && m_shipController.BuildingMode)
			{
				if (m_shipController.Pilot != null)
				{
					myPlayer = m_shipController.ControllerInfo.Controller.Player;
				}
			}
			else
			{
				myPlayer = MyPlayer.GetPlayerFromCharacter(m_character);
			}
			if (myPlayer != null)
			{
				foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> loadedCategory2 in loadedCategories)
				{
					if ((!MySession.Static.SurvivalMode || loadedCategory2.Value.AvailableInSurvival || MySession.Static.IsUserAdmin(myPlayer.Client.SteamUserId)) && (!MySession.Static.CreativeMode || loadedCategory2.Value.ShowInCreative) && ((m_character != null && MySession.Static.GetVoxelHandAvailable(m_character)) || loadedCategory2.Key.CompareTo("VoxelHands") != 0) && (GroupMode != GroupModes.HideBlockGroups || loadedCategory2.Value.IsAnimationCategory || loadedCategory2.Value.IsToolCategory) && (GroupMode != GroupModes.HideEmpty || loadedCategory2.Value.IsAnimationCategory || loadedCategory2.Value.IsToolCategory || (loadedCategory2.Value.ItemIds.Count != 0 && (!MySession.Static.ResearchEnabled || MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySessionComponentResearch.Static.m_requiredResearch.Count <= 0 || loadedCategory2.Value.ValidItems != 0))) && loadedCategory2.Value.IsBlockCategory)
					{
						categories.Add(loadedCategory2.Value.Name, loadedCategory2.Value);
					}
				}
			}
		}

		private void AddCategoryToDisplayList(string displayName, MyGuiBlockCategoryDefinition categoryID)
		{
			MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(displayName), displayName, null, categoryID);
			m_categoriesListbox.Add(item);
		}

		private void AddShipGunsToCategories(Dictionary<string, MyGuiBlockCategoryDefinition> loadedCategories, SortedDictionary<string, MyGuiBlockCategoryDefinition> categories)
		{
			if (m_shipController != null)
			{
				foreach (KeyValuePair<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> gunSet in m_shipController.CubeGrid.GridSystems.WeaponSystem.GetGunSets())
				{
					MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(gunSet.Key);
					foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> loadedCategory in loadedCategories)
					{
						if (loadedCategory.Value.IsShipCategory && loadedCategory.Value.HasItem(definition.Id.ToString()))
						{
							MyGuiBlockCategoryDefinition value = null;
							if (!categories.TryGetValue(loadedCategory.Value.Name, out value))
							{
								categories.Add(loadedCategory.Value.Name, loadedCategory.Value);
							}
						}
					}
				}
			}
		}

		private void RecreateShipCategories(Dictionary<string, MyGuiBlockCategoryDefinition> loadedCategories, SortedDictionary<string, MyGuiBlockCategoryDefinition> categories, MyCubeGrid grid)
		{
			if (grid == null || grid.GridSystems.TerminalSystem == null || grid.GridSystems.TerminalSystem.BlockGroups == null)
			{
				return;
			}
			categories.Clear();
			MyTerminalBlock[] array = grid.GridSystems.TerminalSystem.Blocks.ToArray();
			Array.Sort(array, MyTerminalComparer.Static);
			List<string> list = new List<string>();
			MyTerminalBlock[] array2 = array;
			foreach (MyTerminalBlock myTerminalBlock in array2)
			{
				if (myTerminalBlock != null)
				{
					string item = myTerminalBlock.BlockDefinition.Id.ToString();
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			foreach (string item2 in list)
			{
				foreach (KeyValuePair<string, MyGuiBlockCategoryDefinition> loadedCategory in loadedCategories)
				{
					if (loadedCategory.Value.IsShipCategory && loadedCategory.Value.HasItem(item2) && loadedCategory.Value.SearchBlocks)
					{
						MyGuiBlockCategoryDefinition value = null;
						if (!categories.TryGetValue(loadedCategory.Value.Name, out value))
						{
							categories.Add(loadedCategory.Value.Name, loadedCategory.Value);
						}
					}
				}
			}
		}

		private void AddShipGroupsIntoCategoryList(MyCubeGrid grid)
		{
			if (grid == null || grid.GridSystems.TerminalSystem == null || grid.GridSystems.TerminalSystem.BlockGroups == null)
			{
				return;
			}
			MyBlockGroup[] array = grid.GridSystems.TerminalSystem.BlockGroups.ToArray();
			Array.Sort(array, MyTerminalComparer.Static);
			List<string> list = new List<string>();
			MyBlockGroup[] array2 = array;
			foreach (MyBlockGroup myBlockGroup in array2)
			{
				if (myBlockGroup != null)
				{
					list.Add(myBlockGroup.Name.ToString());
				}
			}
			if (list.Count > 0)
			{
				m_shipGroupsCategory.DisplayNameString = MyTexts.GetString(MySpaceTexts.DisplayName_Category_ShipGroups);
				m_shipGroupsCategory.ItemIds = new HashSet<string>(list);
				m_shipGroupsCategory.SearchBlocks = false;
				m_shipGroupsCategory.Name = "Groups";
				m_sortedCategories.Add(m_shipGroupsCategory.Name, m_shipGroupsCategory);
			}
		}

		public virtual bool AllowToolbarKeys()
		{
			return !m_searchBox.TextBox.HasFocus;
		}

		protected virtual void UpdateGridBlocksBySearchCondition(IMySearchCondition searchCondition)
		{
			searchCondition?.CleanDefinitionGroups();
			if (m_shipController == null || m_shipController.EnableShipControl)
			{
				if (m_character != null || (m_shipController != null && m_shipController.BuildingMode))
				{
					AddCubeDefinitionsToBlocks(searchCondition);
				}
				else if (m_screenCubeGrid != null)
				{
					AddShipBlocksDefinitions(m_screenCubeGrid, isShip: true, searchCondition);
				}
			}
			m_gridBlocks.SelectedIndex = 0;
			m_gridBlocksPanel.ScrollbarVPosition = 0f;
		}

		protected virtual void AddToolsAndAnimations(IMySearchCondition searchCondition)
		{
			if (m_character != null)
			{
				MyCharacter character = m_character;
				foreach (MyPhysicalItemDefinition weaponDefinition in MyDefinitionManager.Static.GetWeaponDefinitions())
				{
					if ((searchCondition == null || searchCondition.MatchesCondition(weaponDefinition)) && weaponDefinition.Public)
					{
						MyInventory inventory = character.GetInventory();
						bool flag = weaponDefinition.Id.SubtypeId == manipulationToolId || (inventory?.ContainItems(1, weaponDefinition.Id) ?? false);
						flag |= MySession.Static.CreativeMode;
						if (flag || MyPerGameSettings.Game == GameEnum.SE_GAME)
						{
							AddWeaponDefinition(m_gridBlocks, weaponDefinition, flag);
						}
					}
				}
				foreach (MyPhysicalItemDefinition consumableDefinition in MyDefinitionManager.Static.GetConsumableDefinitions())
				{
					if ((searchCondition == null || searchCondition.MatchesCondition(consumableDefinition)) && consumableDefinition.Public)
					{
						MyInventory inventory2 = character.GetInventory();
						bool flag2 = consumableDefinition.Id.SubtypeId == manipulationToolId || (inventory2?.ContainItems(1, consumableDefinition.Id) ?? false);
						flag2 |= MySession.Static.CreativeMode;
						if (flag2 || MyPerGameSettings.Game == GameEnum.SE_GAME)
						{
							AddConsumableDefinition(m_gridBlocks, consumableDefinition, flag2);
						}
					}
				}
				if (MyPerGameSettings.EnableAi && MyFakes.ENABLE_BARBARIANS)
				{
					AddAiCommandDefinitions(searchCondition);
					AddBotDefinitions(searchCondition);
				}
				if (MySession.Static.GetVoxelHandAvailable(character))
				{
					AddVoxelHands(searchCondition);
				}
				if (MyFakes.ENABLE_PREFAB_THROWER)
				{
					AddPrefabThrowers(searchCondition);
				}
				AddAnimations(shipController: false, searchCondition);
				AddEmotes(shipController: false, searchCondition);
				AddGridCreators(searchCondition);
			}
			else
			{
				if (m_screenOwner == null)
				{
					return;
				}
				long entityId = m_screenOwner.EntityId;
				AddTerminalGroupsToGridBlocks(m_screenCubeGrid, entityId, searchCondition);
				if (m_shipController != null)
				{
					if (m_shipController.EnableShipControl)
					{
						AddTools(m_shipController, searchCondition);
					}
					AddAnimations(shipController: true, searchCondition);
					AddEmotes(shipController: true, searchCondition);
				}
			}
		}

		private bool IsValidItem(MyCubeBlockDefinitionGroup item)
		{
			if (IsBlockPairResearched(item))
			{
				return true;
			}
			MyBlockVariantGroup myBlockVariantGroup;
			if ((myBlockVariantGroup = item.AnyPublic?.BlockVariantsGroup) != null)
			{
				MyCubeBlockDefinitionGroup[] blockGroups = myBlockVariantGroup.BlockGroups;
				foreach (MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup in blockGroups)
				{
					if (myCubeBlockDefinitionGroup != item && IsBlockPairResearched(myCubeBlockDefinitionGroup))
					{
						return true;
					}
				}
			}
			return false;
			bool IsBlockPairResearched(MyCubeBlockDefinitionGroup group)
			{
				for (int j = 0; j < group.SizeCount; j++)
				{
					MyCubeBlockDefinition myCubeBlockDefinition = group[(MyCubeSize)j];
					if ((MyFakes.ENABLE_NON_PUBLIC_BLOCKS || (myCubeBlockDefinition != null && myCubeBlockDefinition.Public && myCubeBlockDefinition.Enabled)) && IsBlockResearched(myCubeBlockDefinition))
					{
						return true;
					}
				}
				return false;
			}
		}

		private bool IsBlockResearched(MyCubeBlockDefinition block)
		{
			if (MySession.Static.ResearchEnabled && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
			{
				return MySessionComponentResearch.Static.CanUse(m_character ?? m_shipController?.Pilot, block.Id);
			}
			return true;
		}

		private bool HasDLCs(MyDefinitionBase definition)
		{
			MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
			MyCubeBlockDefinition myCubeBlockDefinition;
			if ((myCubeBlockDefinition = (definition as MyCubeBlockDefinition)) != null && myCubeBlockDefinition.BlockVariantsGroup != null && myCubeBlockDefinition.BlockStages != null)
			{
				MyCubeBlockDefinition[] blocks = myCubeBlockDefinition.BlockVariantsGroup.Blocks;
				foreach (MyCubeBlockDefinition myCubeBlockDefinition2 in blocks)
				{
					if (component.GetFirstMissingDefinitionDLC(myCubeBlockDefinition2, Sync.MyId) == null && IsBlockResearched(myCubeBlockDefinition2))
					{
						return true;
					}
				}
				return false;
			}
			return component.GetFirstMissingDefinitionDLC(definition, Sync.MyId) == null;
		}

		protected void AddDefinition(MyGuiControlGrid grid, MyObjectBuilder_ToolbarItem data, MyDefinitionBase definition, bool enabled = true)
		{
			if ((!definition.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS) || (!definition.AvailableInSurvival && MySession.Static.SurvivalMode))
			{
				return;
			}
			bool flag = true;
			MyCubeBlockDefinition myCubeBlockDefinition;
			if ((myCubeBlockDefinition = (definition as MyCubeBlockDefinition)) != null)
			{
				MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(myCubeBlockDefinition.BlockPairName);
				flag = IsValidItem(definitionGroup);
			}
			enabled = (enabled && flag);
			enabled &= HasDLCs(definition);
			string subIcon = null;
			if (definition.DLCs != null && definition.DLCs.Length != 0)
			{
				MyDLCs.MyDLC firstMissingDefinitionDLC = MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
				MyDLCs.MyDLC dlc;
				if (firstMissingDefinitionDLC != null)
				{
					enabled = false;
					subIcon = firstMissingDefinitionDLC.Icon;
				}
				else if (MyDLCs.TryGetDLC(definition.DLCs[0], out dlc))
				{
					subIcon = dlc.Icon;
				}
			}
			MyGuiGridItem myGuiGridItem = new MyGuiGridItem(definition.Icons, null, GetDefinitionTooltip(definition, flag), enabled: enabled, userData: new GridItemUserData
			{
				ItemData = (() => data)
			});
			myGuiGridItem.SubIcon2 = subIcon;
			grid.Add(myGuiGridItem);
		}

		protected void AddDefinitionAtPosition(MyGuiControlGrid grid, MyCubeBlockDefinition block, Vector2I position, bool enabled = true, string subicon = null)
		{
			if (block != null && (block.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (block.AvailableInSurvival || !MySession.Static.SurvivalMode))
			{
				bool flag = IsValidItem(MyDefinitionManager.Static.GetDefinitionGroup(block.BlockPairName));
				enabled = (enabled && flag);
				enabled &= HasDLCs(block);
				string subIcon = null;
				if (!block.DLCs.IsNullOrEmpty() && MyDLCs.TryGetDLC(block.DLCs[0], out MyDLCs.MyDLC dlc))
				{
					subIcon = dlc.Icon;
				}
				string[] icons = block.Icons;
				string definitionTooltip = GetDefinitionTooltip(block, flag);
				MyGuiGridItem myGuiGridItem = new MyGuiGridItem(icons, subicon, definitionTooltip, new GridItemUserData
				{
					ItemData = (() => MyToolbarItemFactory.ObjectBuilderFromDefinition(block))
				}, enabled);
				myGuiGridItem.SubIcon2 = subIcon;
				int num = -position.Y - 1;
				if (position.Y < 0)
				{
					grid.Add(myGuiGridItem, num * 6);
				}
				else if (grid.IsValidIndex(position.Y, position.X))
				{
					SetOrReplaceItemOnPosition(grid, myGuiGridItem, position);
				}
				else if (grid.IsValidIndex(0, position.X))
				{
					grid.RecalculateRowsCount();
					grid.AddRows(position.Y - grid.RowsCount + 1);
					SetOrReplaceItemOnPosition(grid, myGuiGridItem, position);
				}
			}
		}

		private string GetDefinitionTooltip(MyDefinitionBase definition, bool researched)
		{
			StringBuilder stringBuilder = new StringBuilder(definition.DisplayNameText);
			if (!researched)
			{
				stringBuilder.Append("\n").Append(MyTexts.GetString(MyCommonTexts.ScreenCubeBuilderRequiresResearch)).Append(" ");
				MyCubeBlockDefinition myCubeBlockDefinition = definition as MyCubeBlockDefinition;
				if (myCubeBlockDefinition != null)
				{
					MyResearchBlockDefinition researchBlock = MyDefinitionManager.Static.GetResearchBlock(myCubeBlockDefinition.Id);
					if (researchBlock != null)
					{
						string[] unlockedByGroups = researchBlock.UnlockedByGroups;
						foreach (string subtype in unlockedByGroups)
						{
							MyResearchGroupDefinition researchGroup = MyDefinitionManager.Static.GetResearchGroup(subtype);
							if (researchGroup == null)
							{
								continue;
							}
							SerializableDefinitionId[] members = researchGroup.Members;
							foreach (SerializableDefinitionId v in members)
							{
								if (MyDefinitionManager.Static.TryGetDefinition(v, out MyDefinitionBase definition2) && !m_tmpUniqueStrings.Contains(definition2.DisplayNameText))
								{
									stringBuilder.Append("\n");
									stringBuilder.Append(definition2.DisplayNameText);
									m_tmpUniqueStrings.Add(definition2.DisplayNameText);
								}
							}
						}
					}
				}
				m_tmpUniqueStrings.Clear();
			}
			if (!definition.DLCs.IsNullOrEmpty() && MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId) != null)
			{
				stringBuilder.Append("\n");
				for (int k = 0; k < definition.DLCs.Length; k++)
				{
					stringBuilder.Append("\n");
					stringBuilder.Append(MyDLCs.GetRequiredDLCTooltip(definition.DLCs[k]));
				}
			}
			return stringBuilder.ToString();
		}

		private void SetOrReplaceItemOnPosition(MyGuiControlGrid grid, MyGuiGridItem gridItem, Vector2I position)
		{
			MyGuiGridItem myGuiGridItem = grid.TryGetItemAt(position.Y, position.X);
			grid.SetItemAt(position.Y, position.X, gridItem);
			if (myGuiGridItem != null)
			{
				grid.Add(myGuiGridItem);
			}
		}

		private void AddCubeDefinitionsToBlocks(IMySearchCondition searchCondition)
		{
			foreach (string definitionPairName in MyDefinitionManager.Static.GetDefinitionPairNames())
			{
				if (MyFakes.ENABLE_MULTIBLOCKS_IN_SURVIVAL || !MySession.Static.SurvivalMode || !definitionPairName.EndsWith("MultiBlock"))
				{
					MyCubeBlockDefinitionGroup definitionGroup = MyDefinitionManager.Static.GetDefinitionGroup(definitionPairName);
					Vector2I cubeBlockScreenPosition = MyDefinitionManager.Static.GetCubeBlockScreenPosition(definitionPairName);
					if (IsValidItem(definitionGroup))
					{
						if (searchCondition != null)
						{
							bool flag = false;
							for (int i = 0; i < definitionGroup.SizeCount; i++)
							{
								if (flag)
								{
									break;
								}
								MyCubeBlockDefinition myCubeBlockDefinition = definitionGroup[(MyCubeSize)i];
								if ((MyFakes.ENABLE_NON_PUBLIC_BLOCKS || (myCubeBlockDefinition != null && myCubeBlockDefinition.Public && myCubeBlockDefinition.Enabled)) && myCubeBlockDefinition != null)
								{
									bool flag2 = searchCondition.MatchesCondition(myCubeBlockDefinition);
									if (flag2 && (!MyFakes.ENABLE_GUI_HIDDEN_CUBEBLOCKS || myCubeBlockDefinition.GuiVisible || searchCondition is MySearchByStringCondition))
									{
										flag = true;
										break;
									}
									MySearchByCategoryCondition mySearchByCategoryCondition;
									if ((mySearchByCategoryCondition = (searchCondition as MySearchByCategoryCondition)) != null)
									{
										if (mySearchByCategoryCondition.StrictSearch)
										{
											if (flag2)
											{
												flag = true;
												break;
											}
										}
										else if (myCubeBlockDefinition.BlockStages != null && myCubeBlockDefinition.BlockStages.Length != 0)
										{
											for (int j = 0; j < myCubeBlockDefinition.BlockStages.Length; j++)
											{
												MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(myCubeBlockDefinition.BlockStages[j]);
												if (cubeBlockDefinition != null && searchCondition.MatchesCondition(cubeBlockDefinition))
												{
													flag = true;
													break;
												}
											}
										}
										else if (searchCondition.MatchesCondition(myCubeBlockDefinition))
										{
											flag = true;
										}
									}
								}
							}
							if (flag)
							{
								searchCondition.AddDefinitionGroup(definitionGroup);
							}
						}
						else if ((!MyFakes.ENABLE_GUI_HIDDEN_CUBEBLOCKS || definitionGroup.Any.GuiVisible) && definitionGroup.AnyPublic != null)
						{
							if (cubeBlockScreenPosition.Y > 0 && cubeBlockScreenPosition.Y < m_blockOffsets.Count)
							{
								cubeBlockScreenPosition.Y += m_blockOffsets[cubeBlockScreenPosition.Y - 1];
								cubeBlockScreenPosition.Y += cubeBlockScreenPosition.X / m_gridBlocks.ColumnsCount;
								cubeBlockScreenPosition.X %= m_gridBlocks.ColumnsCount;
							}
							AddCubeDefinition(m_gridBlocks, definitionGroup, cubeBlockScreenPosition);
						}
					}
				}
			}
			if (searchCondition != null)
			{
				HashSet<MyCubeBlockDefinitionGroup> sortedBlocks = searchCondition.GetSortedBlocks();
				int num = 0;
				foreach (MyCubeBlockDefinitionGroup item2 in sortedBlocks)
				{
					Vector2I position = default(Vector2I);
					position.X = num % m_gridBlocks.ColumnsCount;
					position.Y = (int)((float)num / (float)m_gridBlocks.ColumnsCount);
					MyCubeBlockDefinition myCubeBlockDefinition2 = MyFakes.ENABLE_NON_PUBLIC_BLOCKS ? item2.Any : item2.AnyPublic;
					if (IsBlockResearched(myCubeBlockDefinition2))
					{
						num++;
						AddCubeDefinition(m_gridBlocks, item2, position);
					}
					MySearchByCategoryCondition mySearchByCategoryCondition2;
					if (myCubeBlockDefinition2.BlockStages != null && myCubeBlockDefinition2.BlockStages.Length != 0 && (mySearchByCategoryCondition2 = (searchCondition as MySearchByCategoryCondition)) != null && !mySearchByCategoryCondition2.StrictSearch)
					{
						MyDefinitionId[] blockStages = myCubeBlockDefinition2.BlockStages;
						foreach (MyDefinitionId id in blockStages)
						{
							bool flag3 = true;
							MyCubeBlockDefinition cubeBlockDefinition2 = MyDefinitionManager.Static.GetCubeBlockDefinition(id);
							if (cubeBlockDefinition2 != null && IsBlockResearched(cubeBlockDefinition2))
							{
								foreach (MyCubeBlockDefinitionGroup item3 in sortedBlocks)
								{
									if (item2 != item3 && (item3.Small == cubeBlockDefinition2 || item3.Large == cubeBlockDefinition2))
									{
										flag3 = false;
										break;
									}
								}
								if (flag3)
								{
									position.X = num % m_gridBlocks.ColumnsCount;
									position.Y = (int)((float)num / (float)m_gridBlocks.ColumnsCount);
									num++;
									AddDefinitionAtPosition(m_gridBlocks, cubeBlockDefinition2, position);
								}
							}
						}
					}
				}
				return;
			}
			int num2 = 0;
			int num3 = int.MaxValue;
			for (int l = 0; l < m_gridBlocks.RowsCount; l++)
			{
				for (int m = 0; m < m_gridBlocks.ColumnsCount; m++)
				{
					MyGuiGridItem myGuiGridItem = m_gridBlocks.TryGetItemAt(l, m);
					if (myGuiGridItem != null && num3 != int.MaxValue)
					{
						m_gridBlocks.SetItemAt(l, m, null);
						m_gridBlocks.SetItemAt(l, num3, myGuiGridItem);
						num3++;
					}
					else if (myGuiGridItem == null && num3 > m)
					{
						num3 = m;
					}
				}
				if (num3 == 0)
				{
					num2++;
				}
				else if (num2 > 0)
				{
					for (int n = 0; n < m_gridBlocks.ColumnsCount; n++)
					{
						MyGuiGridItem item = m_gridBlocks.TryGetItemAt(l, n);
						m_gridBlocks.SetItemAt(l, n, null);
						m_gridBlocks.SetItemAt(l - num2, n, item);
					}
				}
				num3 = int.MaxValue;
			}
			if (num2 > 0)
			{
				int num4 = num2 * m_gridBlocks.ColumnsCount;
				m_gridBlocks.Items.RemoveRange(m_gridBlocks.Items.Count - num4, num4);
				m_gridBlocks.RowsCount -= num2;
			}
		}

		private void AddCubeDefinition(MyGuiControlGrid grid, MyCubeBlockDefinitionGroup group, Vector2I position)
		{
			MyCubeBlockDefinition myCubeBlockDefinition = MyFakes.ENABLE_NON_PUBLIC_BLOCKS ? group.Any : group.AnyPublic;
			if (MyFakes.ENABLE_MULTIBLOCK_CONSTRUCTION || !MySession.Static.SurvivalMode || myCubeBlockDefinition.MultiBlock == null)
			{
				MyBlockVariantGroup blockVariantsGroup = myCubeBlockDefinition.BlockVariantsGroup;
				bool flag = (blockVariantsGroup != null) ? (blockVariantsGroup.BlockGroups.Length > 1 && blockVariantsGroup.PrimaryGUIBlock == myCubeBlockDefinition) : (myCubeBlockDefinition.BlockStages != null && myCubeBlockDefinition.BlockStages.Length != 0);
				string subicon = null;
				if (flag)
				{
					subicon = MyGuiTextures.Static.GetTexture(MyHud.HudDefinition.Toolbar.ItemStyle.VariantTexture).Path;
				}
				MyCockpit myCockpit;
				bool enabled = MyToolbarComponent.GlobalBuilding || MySession.Static.ControlledEntity is MyCharacter || ((myCockpit = (MySession.Static.ControlledEntity as MyCockpit)) != null && myCockpit.BuildingMode);
				AddDefinitionAtPosition(grid, myCubeBlockDefinition, position, enabled, subicon);
			}
		}

		private void AddGridGun(MyShipController shipController, MyDefinitionId gunId, IMySearchCondition searchCondition)
		{
			MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(gunId);
			if (searchCondition == null || searchCondition.MatchesCondition(definition))
			{
				AddWeaponDefinition(m_gridBlocks, definition);
			}
		}

		private void AddTools(MyShipController shipController, IMySearchCondition searchCondition)
		{
			foreach (KeyValuePair<MyDefinitionId, HashSet<IMyGunObject<MyDeviceBase>>> gunSet in shipController.CubeGrid.GridSystems.WeaponSystem.GetGunSets())
			{
				AddGridGun(shipController, gunSet.Key, searchCondition);
			}
		}

		private void AddAnimations(bool shipController, IMySearchCondition searchCondition)
		{
			foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
			{
				if (animationDefinition.Public && (!shipController || (shipController && animationDefinition.AllowInCockpit)) && (searchCondition == null || searchCondition.MatchesCondition(animationDefinition)))
				{
					AddAnimationDefinition(m_gridBlocks, animationDefinition);
				}
			}
		}

		private void AddEmotes(bool shipController, IMySearchCondition searchCondition)
		{
			foreach (MyGameInventoryItemDefinition item in from e in MyGameService.GetDefinitionsForSlot(MyGameInventoryItemSlot.Emote)
				orderby e.Name
				select e)
			{
				MyEmoteDefinition definition = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(item.AssetModifierId);
				if (definition != null && definition.Public)
				{
					MyAnimationDefinition myAnimationDefinition = MyDefinitionManager.Static.TryGetAnimationDefinition(definition.AnimationId.SubtypeName);
					if (myAnimationDefinition != null && (!shipController || (shipController && myAnimationDefinition.AllowInCockpit)) && (searchCondition == null || searchCondition.MatchesCondition(definition)))
					{
						AddEmoteDefinition(m_gridBlocks, definition, MyGameService.HasInventoryItemWithDefinitionId(item.ID));
					}
				}
			}
		}

		private void AddVoxelHands(IMySearchCondition searchCondition)
		{
			foreach (MyVoxelHandDefinition voxelHandDefinition in MyDefinitionManager.Static.GetVoxelHandDefinitions())
			{
				if (voxelHandDefinition.Public && (searchCondition == null || searchCondition.MatchesCondition(voxelHandDefinition)))
				{
					AddVoxelHandDefinition(m_gridBlocks, voxelHandDefinition);
				}
			}
		}

		private void AddGridCreators(IMySearchCondition searchCondition)
		{
			foreach (MyGridCreateToolDefinition gridCreatorDefinition in MyDefinitionManager.Static.GetGridCreatorDefinitions())
			{
				if (gridCreatorDefinition.Public && (searchCondition == null || searchCondition.MatchesCondition(gridCreatorDefinition)))
				{
					AddGridCreatorDefinition(m_gridBlocks, gridCreatorDefinition);
				}
			}
		}

		private void AddPrefabThrowers(IMySearchCondition searchCondition)
		{
			foreach (MyPrefabThrowerDefinition prefabThrowerDefinition in MyDefinitionManager.Static.GetPrefabThrowerDefinitions())
			{
				if ((prefabThrowerDefinition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (searchCondition == null || searchCondition.MatchesCondition(prefabThrowerDefinition)))
				{
					AddPrefabThrowerDefinition(m_gridBlocks, prefabThrowerDefinition);
				}
			}
		}

		private void AddBotDefinitions(IMySearchCondition searchCondition)
		{
			foreach (MyBotDefinition item in MyDefinitionManager.Static.GetDefinitionsOfType<MyBotDefinition>())
			{
				if ((item.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (item.AvailableInSurvival || MySession.Static.CreativeMode) && (searchCondition == null || searchCondition.MatchesCondition(item)))
				{
					AddBotDefinition(m_gridBlocks, item);
				}
			}
		}

		private void AddAiCommandDefinitions(IMySearchCondition searchCondition)
		{
			foreach (MyAiCommandDefinition item in MyDefinitionManager.Static.GetDefinitionsOfType<MyAiCommandDefinition>())
			{
				if ((item.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (item.AvailableInSurvival || MySession.Static.CreativeMode) && (searchCondition == null || searchCondition.MatchesCondition(item)))
				{
					AddToolbarItemDefinition<MyObjectBuilder_ToolbarItemAiCommand>(m_gridBlocks, item);
				}
			}
		}

		private void AddWeaponDefinition(MyGuiControlGrid grid, MyDefinitionBase definition, bool enabled = true)
		{
			if ((definition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (definition.AvailableInSurvival || !MySession.Static.SurvivalMode))
			{
				MyObjectBuilder_ToolbarItemWeapon weaponData = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemWeapon>();
				weaponData.DefinitionId = definition.Id;
				MyGuiGridItem item = new MyGuiGridItem(definition.Icons, null, definition.DisplayNameText, new GridItemUserData
				{
					ItemData = (() => weaponData)
				}, enabled);
				grid.Add(item);
			}
		}

		private void AddConsumableDefinition(MyGuiControlGrid grid, MyDefinitionBase definition, bool enabled = true)
		{
			if ((definition.Public || MyFakes.ENABLE_NON_PUBLIC_BLOCKS) && (definition.AvailableInSurvival || !MySession.Static.SurvivalMode))
			{
				MyObjectBuilder_ToolbarItemConsumable consumableData = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemConsumable>();
				consumableData.DefinitionId = definition.Id;
				MyGuiGridItem item = new MyGuiGridItem(definition.Icons, null, definition.DisplayNameText, new GridItemUserData
				{
					ItemData = (() => consumableData)
				}, enabled);
				grid.Add(item);
			}
		}

		private void AddAnimationDefinition(MyGuiControlGrid grid, MyDefinitionBase definition)
		{
			MyObjectBuilder_ToolbarItemAnimation myObjectBuilder_ToolbarItemAnimation = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemAnimation>();
			myObjectBuilder_ToolbarItemAnimation.DefinitionId = definition.Id;
			AddDefinition(grid, myObjectBuilder_ToolbarItemAnimation, definition);
		}

		private void AddEmoteDefinition(MyGuiControlGrid grid, MyDefinitionBase definition, bool enabled = true)
		{
			MyObjectBuilder_ToolbarItemEmote myObjectBuilder_ToolbarItemEmote = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemEmote>();
			myObjectBuilder_ToolbarItemEmote.DefinitionId = definition.Id;
			AddDefinition(grid, myObjectBuilder_ToolbarItemEmote, definition, enabled);
		}

		private void AddVoxelHandDefinition(MyGuiControlGrid grid, MyDefinitionBase definition)
		{
			MyObjectBuilder_ToolbarItemVoxelHand myObjectBuilder_ToolbarItemVoxelHand = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemVoxelHand>();
			myObjectBuilder_ToolbarItemVoxelHand.DefinitionId = definition.Id;
			AddDefinition(grid, myObjectBuilder_ToolbarItemVoxelHand, definition);
		}

		private void AddGridCreatorDefinition(MyGuiControlGrid grid, MyDefinitionBase definition)
		{
			MyObjectBuilder_ToolbarItemCreateGrid myObjectBuilder_ToolbarItemCreateGrid = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemCreateGrid>();
			myObjectBuilder_ToolbarItemCreateGrid.DefinitionId = definition.Id;
			AddDefinition(grid, myObjectBuilder_ToolbarItemCreateGrid, definition);
		}

		private void AddPrefabThrowerDefinition(MyGuiControlGrid grid, MyPrefabThrowerDefinition definition)
		{
			MyObjectBuilder_ToolbarItemPrefabThrower myObjectBuilder_ToolbarItemPrefabThrower = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemPrefabThrower>();
			myObjectBuilder_ToolbarItemPrefabThrower.DefinitionId = definition.Id;
			AddDefinition(grid, myObjectBuilder_ToolbarItemPrefabThrower, definition);
		}

		private void AddBotDefinition(MyGuiControlGrid grid, MyBotDefinition definition)
		{
			MyObjectBuilder_ToolbarItemBot myObjectBuilder_ToolbarItemBot = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemBot>();
			myObjectBuilder_ToolbarItemBot.DefinitionId = definition.Id;
			AddDefinition(grid, myObjectBuilder_ToolbarItemBot, definition);
		}

		private void AddToolbarItemDefinition<T>(MyGuiControlGrid grid, MyDefinitionBase definition) where T : MyObjectBuilder_ToolbarItemDefinition, new()
		{
			T val = MyObjectBuilderSerializer.CreateNewObject<T>();
			val.DefinitionId = definition.Id;
			AddDefinition(grid, val, definition);
		}

		private void AddShipBlocksDefinitions(MyCubeGrid grid, bool isShip, IMySearchCondition searchCondition)
		{
			if ((!isShip || m_shipController == null || m_shipController.EnableShipControl) && MyFakes.ENABLE_SHIP_BLOCKS_TOOLBAR)
			{
				AddTerminalSingleBlocksToGridBlocks(grid, searchCondition);
			}
		}

		private void AddTerminalGroupsToGridBlocks(MyCubeGrid grid, long Owner, IMySearchCondition searchCondition)
		{
			if (grid == null || grid.GridSystems.TerminalSystem == null || grid.GridSystems.TerminalSystem.BlockGroups == null)
			{
				return;
			}
			int num = 0;
			int columnsCount = m_gridBlocks.ColumnsCount;
			MyBlockGroup[] array = grid.GridSystems.TerminalSystem.BlockGroups.ToArray();
			Array.Sort(array, MyTerminalComparer.Static);
			MyBlockGroup[] array2 = array;
			foreach (MyBlockGroup myBlockGroup in array2)
			{
				if (searchCondition == null || searchCondition.MatchesCondition(myBlockGroup.Name.ToString()))
				{
					MyObjectBuilder_ToolbarItemTerminalGroup groupData = MyToolbarItemFactory.TerminalGroupObjectBuilderFromGroup(myBlockGroup);
					bool enabled = false;
					foreach (MyTerminalBlock block in myBlockGroup.Blocks)
					{
						if (block.IsFunctional)
						{
							enabled = true;
							break;
						}
					}
					groupData.BlockEntityId = Owner;
					m_gridBlocks.Add(new MyGuiGridItem(MyToolbarItemFactory.GetIconForTerminalGroup(myBlockGroup), null, myBlockGroup.Name.ToString(), new GridItemUserData
					{
						ItemData = (() => groupData)
					}, enabled));
					num++;
				}
			}
			if (num <= 0)
			{
				return;
			}
			int num2 = num % columnsCount;
			if (num2 == 0)
			{
				num2 = columnsCount;
			}
			for (int j = 0; j < 2 * columnsCount - num2; j++)
			{
				if (num < m_gridBlocks.GetItemsCount())
				{
					m_gridBlocks.SetItemAt(num++, new MyGuiGridItem("", null, string.Empty, new GridItemUserData
					{
						ItemData = (() => MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemEmpty>())
					}, enabled: false));
				}
				else
				{
					m_gridBlocks.Add(new MyGuiGridItem("", null, string.Empty, new GridItemUserData
					{
						ItemData = (() => MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemEmpty>())
					}, enabled: false));
				}
			}
		}

		private void AddTerminalSingleBlocksToGridBlocks(MyCubeGrid grid, IMySearchCondition searchCondition)
		{
			if (grid == null || grid.GridSystems.TerminalSystem == null)
			{
				return;
			}
			MyTerminalBlock[] array = grid.GridSystems.TerminalSystem.Blocks.ToArray();
			Array.Sort(array, MyTerminalComparer.Static);
			MyTerminalBlock[] array2 = array;
			foreach (MyTerminalBlock myTerminalBlock in array2)
			{
				if (myTerminalBlock != null && MyTerminalControlFactory.GetActions(myTerminalBlock.GetType()).Count > 0 && (searchCondition == null || searchCondition.MatchesCondition(myTerminalBlock.BlockDefinition) || searchCondition.MatchesCondition(myTerminalBlock.CustomName.ToString())) && myTerminalBlock.ShowInToolbarConfig && (myTerminalBlock.BlockDefinition.AvailableInSurvival || !MySession.Static.SurvivalMode))
				{
					MyObjectBuilder_ToolbarItemTerminalBlock blockData = MyToolbarItemFactory.TerminalBlockObjectBuilderFromBlock(myTerminalBlock);
					m_gridBlocks.Add(new MyGuiGridItem(myTerminalBlock.BlockDefinition.Icons, MyTerminalActionIcons.NONE, myTerminalBlock.CustomName.ToString(), new GridItemUserData
					{
						ItemData = (() => blockData)
					}, myTerminalBlock.IsFunctional));
				}
			}
		}

		private void categories_ItemClicked(MyGuiControlListbox sender)
		{
			m_gridBlocks.SetItemsToDefault();
			if (sender.SelectedItems.Count != 0)
			{
				m_allSelectedCategories.Clear();
				m_searchInBlockCategories.Clear();
				bool flag = true;
				bool flag2 = false;
				foreach (MyGuiControlListbox.Item selectedItem in sender.SelectedItems)
				{
					MyGuiBlockCategoryDefinition myGuiBlockCategoryDefinition = (MyGuiBlockCategoryDefinition)selectedItem.UserData;
					if (myGuiBlockCategoryDefinition == null)
					{
						flag2 = true;
					}
					else
					{
						if (myGuiBlockCategoryDefinition.SearchBlocks)
						{
							m_searchInBlockCategories.Add(myGuiBlockCategoryDefinition);
						}
						flag &= myGuiBlockCategoryDefinition.StrictSearch;
						m_allSelectedCategories.Add(myGuiBlockCategoryDefinition);
					}
				}
				m_categorySearchCondition.SelectedCategories = m_allSelectedCategories;
				AddToolsAndAnimations(m_categorySearchCondition);
				m_categorySearchCondition.StrictSearch = flag;
				m_categorySearchCondition.SelectedCategories = m_searchInBlockCategories;
				object obj;
				if (!flag2)
				{
					IMySearchCondition categorySearchCondition = m_categorySearchCondition;
					obj = categorySearchCondition;
				}
				else if (!m_nameSearchCondition.IsValid)
				{
					obj = null;
				}
				else
				{
					IMySearchCondition categorySearchCondition = m_nameSearchCondition;
					obj = categorySearchCondition;
				}
				IMySearchCondition searchCondition = (IMySearchCondition)obj;
				UpdateGridBlocksBySearchCondition(searchCondition);
				SearchResearch(searchCondition);
			}
		}

		private bool SelectedCategoryMove(MyGuiControlListbox target, int step = 1)
		{
			if (step == 0)
			{
				return false;
			}
			int count = target.Items.Count;
			int num = 0;
			num = ((step >= 0) ? step : (count + step % count));
			List<MyGuiControlListbox.Item> selectedItems = target.SelectedItems;
			int num2 = 0;
			if (selectedItems.Count <= 0)
			{
				num2 = ((step <= 0) ? (target.Items.Count - 1) : 0);
			}
			else
			{
				int num3 = 0;
				MyGuiControlListbox.Item item = (step <= 0) ? selectedItems[0] : selectedItems[selectedItems.Count - 1];
				foreach (MyGuiControlListbox.Item item2 in target.Items)
				{
					if (item == item2)
					{
						break;
					}
					num3++;
				}
				if (num3 >= count)
				{
					return false;
				}
				num2 = num3 + num;
			}
			int index = num2 % count;
			List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
			list.Add(target.Items[index]);
			target.SelectedItems = list;
			return true;
		}

		protected override bool HandleKeyboardActiveIndex(MyDirection direction, bool page, bool loop)
		{
			MyGuiControlBase myGuiControlBase = base.FocusedControl;
			if (base.FocusedControl == null)
			{
				myGuiControlBase = GetFirstFocusableControl();
				if (myGuiControlBase == null)
				{
					return false;
				}
			}
			MyGuiControlBase focusControl = myGuiControlBase.GetFocusControl(direction, page, loop);
			if (focusControl != null)
			{
				base.FocusedControl = focusControl;
			}
			return true;
		}

		private void m_researchGraph_ItemClicked(object sender, MySharedButtonsEnum button)
		{
			MyGuiGridItem selectedItem = (sender as MyGuiControlResearchGraph).SelectedItem;
			if (selectedItem == null || !selectedItem.Enabled)
			{
				return;
			}
			switch (button)
			{
			case MySharedButtonsEnum.Primary:
				MyToolbarItemFactory.CreateToolbarItem(((GridItemUserData)selectedItem.UserData).ItemData());
				break;
			case MySharedButtonsEnum.Secondary:
			{
				GridItemUserData gridItemUserData = (GridItemUserData)selectedItem.UserData;
				MyToolbarItemActions myToolbarItemActions = MyToolbarItemFactory.CreateToolbarItem(gridItemUserData.ItemData()) as MyToolbarItemActions;
				if (myToolbarItemActions != null)
				{
					if (!UpdateContextMenu(ref m_contextMenu, myToolbarItemActions, gridItemUserData))
					{
						m_researchGraph_ItemDoubleClicked(sender, EventArgs.Empty);
					}
				}
				else
				{
					m_researchGraph_ItemDoubleClicked(sender, EventArgs.Empty);
				}
				break;
			}
			case MySharedButtonsEnum.Ternary:
				if (m_blockGroupInfo.SelectedDefinition != null && MySession.Static.LocalCharacter.AddToBuildPlanner(m_blockGroupInfo.SelectedDefinition))
				{
					m_blockGroupInfo.UpdateBuildPlanner();
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				}
				break;
			default:
				if (MyInput.Static.IsAnyShiftKeyPressed())
				{
					m_researchGraph_ItemDoubleClicked(sender, EventArgs.Empty);
				}
				break;
			}
		}

		private void m_researchGraph_ItemDoubleClicked(object sender, EventArgs e)
		{
			MyGuiGridItem selectedItem = (sender as MyGuiControlResearchGraph).SelectedItem;
			if (selectedItem != null && selectedItem.Enabled)
			{
				MyObjectBuilder_ToolbarItem myObjectBuilder_ToolbarItem = ((GridItemUserData)selectedItem.UserData).ItemData();
				if (!(myObjectBuilder_ToolbarItem is MyObjectBuilder_ToolbarItemEmpty))
				{
					AddGridItemToToolbar(myObjectBuilder_ToolbarItem);
				}
			}
		}

		private void grid_ItemClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			if (eventArgs.Button == MySharedButtonsEnum.Primary)
			{
				MyGuiGridItem myGuiGridItem = sender.TryGetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
				if (myGuiGridItem != null && myGuiGridItem.Enabled)
				{
					MyToolbarItemFactory.CreateToolbarItem(((GridItemUserData)myGuiGridItem.UserData).ItemData());
				}
			}
			else if (eventArgs.Button == MySharedButtonsEnum.Secondary)
			{
				MyGuiGridItem myGuiGridItem2 = sender.TryGetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
				if (myGuiGridItem2 == null || !myGuiGridItem2.Enabled)
				{
					return;
				}
				GridItemUserData gridItemUserData = (GridItemUserData)myGuiGridItem2.UserData;
				MyToolbarItem myToolbarItem = MyToolbarItemFactory.CreateToolbarItem(gridItemUserData.ItemData());
				if (myToolbarItem is MyToolbarItemActions)
				{
					m_contextBlockX = eventArgs.RowIndex;
					m_contextBlockY = eventArgs.ColumnIndex;
					if (!UpdateContextMenu(ref m_contextMenu, myToolbarItem as MyToolbarItemActions, gridItemUserData))
					{
						grid_ItemDoubleClicked(sender, eventArgs);
					}
				}
				else
				{
					grid_ItemDoubleClicked(sender, eventArgs);
				}
			}
			else if (eventArgs.Button == MySharedButtonsEnum.Ternary)
			{
				if (m_blockGroupInfo.SelectedDefinition != null && MySession.Static.LocalCharacter.AddToBuildPlanner(m_blockGroupInfo.SelectedDefinition))
				{
					m_blockGroupInfo.UpdateBuildPlanner();
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				}
			}
			else if (MyInput.Static.IsAnyShiftKeyPressed())
			{
				grid_ItemShiftClicked(sender, eventArgs);
			}
		}

		private void grid_ItemShiftClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			if (eventArgs.Button != MySharedButtonsEnum.Primary)
			{
				return;
			}
			MyGuiGridItem myGuiGridItem = sender.TryGetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
			if (myGuiGridItem != null && myGuiGridItem.Enabled)
			{
				MyToolbarItem myToolbarItem = MyToolbarItemFactory.CreateToolbarItem(((GridItemUserData)myGuiGridItem.UserData).ItemData());
				if (!myToolbarItem.WantsToBeActivated)
				{
					myToolbarItem.Activate();
				}
			}
		}

		private void grid_ItemDoubleClicked(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			try
			{
				MyGuiGridItem myGuiGridItem = sender.TryGetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
				if (myGuiGridItem != null && myGuiGridItem.Enabled)
				{
					GridItemUserData gridItemUserData = (GridItemUserData)myGuiGridItem.UserData;
					MyObjectBuilder_ToolbarItem myObjectBuilder_ToolbarItem = gridItemUserData.ItemData();
					if (!(myObjectBuilder_ToolbarItem is MyObjectBuilder_ToolbarItemEmpty))
					{
						if (MyToolbarItemFactory.CreateToolbarItem(gridItemUserData.ItemData()) is MyToolbarItemActions && MyInput.Static.IsJoystickLastUsed)
						{
							MyGuiControlGrid.EventArgs eventArgs2 = default(MyGuiControlGrid.EventArgs);
							eventArgs2.Button = MySharedButtonsEnum.Secondary;
							eventArgs2.ColumnIndex = eventArgs.ColumnIndex;
							eventArgs2.ItemIndex = eventArgs.ItemIndex;
							eventArgs2.RowIndex = eventArgs.RowIndex;
							MyGuiControlGrid.EventArgs eventArgs3 = eventArgs2;
							grid_ItemClicked(sender, eventArgs3);
						}
						else
						{
							AddGridItemToToolbar(myObjectBuilder_ToolbarItem);
						}
					}
				}
			}
			finally
			{
			}
		}

		private void grid_PanelScrolled(MyGuiControlScrollablePanel panel)
		{
			if (m_contextMenu != null && m_contextMenu.IsActiveControl)
			{
				m_contextMenu.Deactivate();
			}
		}

		protected void grid_OnDrag(MyGuiControlGrid sender, MyGuiControlGrid.EventArgs eventArgs)
		{
			StartDragging(MyDropHandleType.MouseRelease, sender, ref eventArgs);
		}

		private void m_researchGraph_ItemDragged(object sender, MyGuiGridItem item)
		{
			StartDragging(MyDropHandleType.MouseRelease, sender as MyGuiControlResearchGraph, item);
		}

		private void dragAndDrop_OnDrop(object sender, MyDragAndDropEventArgs eventArgs)
		{
			if (eventArgs.DropTo != null && !m_toolbarControl.IsToolbarGrid(eventArgs.DragFrom.Grid) && m_toolbarControl.IsToolbarGrid(eventArgs.DropTo.Grid))
			{
				GridItemUserData gridItemUserData = (GridItemUserData)eventArgs.Item.UserData;
				MyObjectBuilder_ToolbarItem myObjectBuilder_ToolbarItem = gridItemUserData.ItemData();
				if (myObjectBuilder_ToolbarItem is MyObjectBuilder_ToolbarItemEmpty)
				{
					return;
				}
				if (eventArgs.DropTo.ItemIndex >= 0 && eventArgs.DropTo.ItemIndex < 9)
				{
					MyToolbarItem myToolbarItem = MyToolbarItemFactory.CreateToolbarItem(myObjectBuilder_ToolbarItem);
					if (myToolbarItem is MyToolbarItemActions)
					{
						if (UpdateContextMenu(ref m_onDropContextMenu, myToolbarItem as MyToolbarItemActions, gridItemUserData))
						{
							m_onDropContextMenuToolbarIndex = eventArgs.DropTo.ItemIndex;
							m_onDropContextMenu.Enabled = true;
							m_onDropContextMenuItem = myToolbarItem;
						}
						else
						{
							DropGridItemToToolbar(myToolbarItem, eventArgs.DropTo.ItemIndex);
						}
					}
					else
					{
						DropGridItemToToolbar(myToolbarItem, eventArgs.DropTo.ItemIndex);
						if (myToolbarItem.WantsToBeActivated)
						{
							MyToolbarComponent.CurrentToolbar.ActivateItemAtSlot(eventArgs.DropTo.ItemIndex, checkIfWantsToBeActivated: false, playActivationSound: false, userActivated: false);
						}
					}
				}
			}
			else if (eventArgs.DropTo != null && !m_toolbarControl.IsToolbarGrid(eventArgs.DragFrom.Grid) && m_blockGroupInfo.IsBuildPlannerGrid(eventArgs.DropTo.Grid))
			{
				if (((GridItemUserData)eventArgs.Item.UserData).ItemData() is MyObjectBuilder_ToolbarItemEmpty)
				{
					return;
				}
				if (m_blockGroupInfo.SelectedDefinition != null && MySession.Static.LocalCharacter.AddToBuildPlanner(m_blockGroupInfo.SelectedDefinition, eventArgs.DropTo.ItemIndex))
				{
					m_blockGroupInfo.UpdateBuildPlanner();
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
				}
			}
			m_toolbarControl.HandleDragAndDrop(sender, eventArgs);
		}

		private void searchItemTexbox_TextChanged(string text)
		{
			if (m_framesBeforeSearchEnabled > 0)
			{
				return;
			}
			m_gridBlocks.SetItemsToDefault();
			if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text))
			{
				if (m_character != null || (m_shipController != null && m_shipController.BuildingMode))
				{
					AddCubeDefinitionsToBlocks(null);
				}
				else
				{
					AddShipBlocksDefinitions(m_screenCubeGrid, isShip: true, null);
				}
				m_nameSearchCondition.Clean();
				SearchResearch(null);
				return;
			}
			m_nameSearchCondition.SearchName = text;
			if (m_shipController == null || m_shipController.EnableShipControl)
			{
				AddToolsAndAnimations(m_nameSearchCondition);
				UpdateGridBlocksBySearchCondition(m_nameSearchCondition);
				SearchResearch(m_nameSearchCondition);
			}
			else
			{
				AddAnimations(shipController: true, m_nameSearchCondition);
				AddEmotes(shipController: true, m_nameSearchCondition);
			}
		}

		private void SearchResearch(IMySearchCondition searchCondition)
		{
			m_minVerticalPosition = float.MaxValue;
			m_researchItemFound = false;
			if (m_researchGraph != null && m_researchGraph.Nodes != null)
			{
				foreach (MyGuiControlResearchGraph.GraphNode node in m_researchGraph.Nodes)
				{
					SearchNode(node, searchCondition);
				}
			}
			if (m_researchItemFound)
			{
				m_researchPanel.SetVerticalScrollbarValue(m_minVerticalPosition);
			}
		}

		private void SearchNode(MyGuiControlResearchGraph.GraphNode node, IMySearchCondition searchCondition)
		{
			foreach (MyGuiGridItem item in node.Items)
			{
				bool flag = searchCondition?.MatchesCondition(item.ItemDefinition) ?? false;
				item.OverlayPercent = (flag ? 1f : 0f);
				if (flag && m_minVerticalPosition > node.Position.Y)
				{
					m_minVerticalPosition = node.Position.Y;
					m_researchItemFound = true;
				}
			}
			foreach (MyGuiControlResearchGraph.GraphNode child in node.Children)
			{
				SearchNode(child, searchCondition);
			}
		}

		protected void UpdateGridControl()
		{
			categories_ItemClicked(m_categoriesListbox);
		}

		private void contextMenu_ItemClicked(MyGuiControlContextMenu sender, MyGuiControlContextMenu.EventArgs args)
		{
			if (m_contextBlockX >= 0 && m_contextBlockX < m_gridBlocks.RowsCount && m_contextBlockY >= 0 && m_contextBlockY < m_gridBlocks.ColumnsCount)
			{
				MyGuiGridItem myGuiGridItem = m_gridBlocks.TryGetItemAt(m_contextBlockX, m_contextBlockY);
				if (myGuiGridItem != null)
				{
					MyObjectBuilder_ToolbarItemTerminal myObjectBuilder_ToolbarItemTerminal = (MyObjectBuilder_ToolbarItemTerminal)(myGuiGridItem.UserData as GridItemUserData).ItemData();
					myObjectBuilder_ToolbarItemTerminal._Action = (string)args.UserData;
					AddGridItemToToolbar(myObjectBuilder_ToolbarItemTerminal);
					myObjectBuilder_ToolbarItemTerminal._Action = null;
					base.FocusedControl = m_gridBlocks;
				}
			}
		}

		private void onDropContextMenu_ItemClicked(MyGuiControlContextMenu sender, MyGuiControlContextMenu.EventArgs args)
		{
			int onDropContextMenuToolbarIndex = m_onDropContextMenuToolbarIndex;
			if (onDropContextMenuToolbarIndex >= 0 && onDropContextMenuToolbarIndex < MyToolbarComponent.CurrentToolbar.SlotCount)
			{
				MyToolbarItem onDropContextMenuItem = m_onDropContextMenuItem;
				if (onDropContextMenuItem is MyToolbarItemActions)
				{
					(onDropContextMenuItem as MyToolbarItemActions).ActionId = (string)args.UserData;
					DropGridItemToToolbar(onDropContextMenuItem, onDropContextMenuToolbarIndex);
				}
			}
		}

		private void AddGridItemToToolbar(MyObjectBuilder_ToolbarItem data)
		{
			if (m_gamepadSlot.HasValue)
			{
				MyToolbarItem myToolbarItem = MyToolbarItemFactory.CreateToolbarItem(data);
				if (myToolbarItem != null)
				{
					MyToolbarComponent.CurrentToolbar.SetItemAtIndex(m_gamepadSlot.Value, myToolbarItem, gamepad: true);
					CloseScreen();
				}
				return;
			}
			MyToolbar currentToolbar = MyToolbarComponent.CurrentToolbar;
			int slotCount = currentToolbar.SlotCount;
			MyToolbarItem newItem = MyToolbarItemFactory.CreateToolbarItem(data);
			if (newItem != null)
			{
				RequestItemParameters(newItem, delegate
				{
					bool flag = false;
					int num = 0;
					for (int i = 0; i < slotCount; i++)
					{
						MyToolbarItem slotItem = currentToolbar.GetSlotItem(i);
						if (slotItem != null && slotItem.Equals(newItem))
						{
							if (slotItem.WantsToBeActivated)
							{
								currentToolbar.ActivateItemAtSlot(i, checkIfWantsToBeActivated: false, playActivationSound: false, userActivated: false);
							}
							num = i;
							flag = true;
							break;
						}
					}
					for (int j = 0; j < slotCount; j++)
					{
						if (!flag && currentToolbar.GetSlotItem(j) == null)
						{
							currentToolbar.SetItemAtSlot(j, newItem);
							if (newItem.WantsToBeActivated)
							{
								currentToolbar.ActivateItemAtSlot(j, checkIfWantsToBeActivated: false, playActivationSound: false, userActivated: false);
							}
							num = j;
							flag = true;
						}
						else if (j != num && currentToolbar.GetSlotItem(j) != null && currentToolbar.GetSlotItem(j).Equals(newItem))
						{
							currentToolbar.SetItemAtSlot(j, null);
						}
					}
					if (!flag)
					{
						int slot = currentToolbar.SelectedSlot ?? 0;
						currentToolbar.SetItemAtSlot(slot, newItem);
						if (newItem.WantsToBeActivated)
						{
							currentToolbar.ActivateItemAtSlot(slot, checkIfWantsToBeActivated: false, playActivationSound: false, userActivated: false);
						}
					}
				});
			}
		}

		public static void RequestItemParameters(MyToolbarItem item, Action<bool> callback)
		{
			MyToolbarItemTerminalBlock myToolbarItemTerminalBlock = item as MyToolbarItemTerminalBlock;
			if (myToolbarItemTerminalBlock != null)
			{
				ITerminalAction actionOrNull = myToolbarItemTerminalBlock.GetActionOrNull(myToolbarItemTerminalBlock.ActionId);
				if (actionOrNull != null && actionOrNull.GetParameterDefinitions().Count > 0)
				{
					actionOrNull.RequestParameterCollection(myToolbarItemTerminalBlock.Parameters, callback);
					return;
				}
			}
			callback(obj: true);
		}

		public static void DropGridItemToToolbar(MyToolbarItem item, int slot)
		{
			RequestItemParameters(item, delegate(bool success)
			{
				if (success)
				{
					MyToolbar currentToolbar = MyToolbarComponent.CurrentToolbar;
					for (int i = 0; i < currentToolbar.SlotCount; i++)
					{
						if (currentToolbar.GetSlotItem(i) != null && currentToolbar.GetSlotItem(i).Equals(item))
						{
							currentToolbar.SetItemAtSlot(i, null);
						}
					}
					MyGuiAudio.PlaySound(MyGuiSounds.HudItem);
					MyToolbarComponent.CurrentToolbar.SetItemAtSlot(slot, item);
				}
			});
		}

		public static void ReinitializeBlockScrollbarPosition()
		{
			m_savedVPosition = 0f;
		}

		private bool CanDropItem(MyPhysicalInventoryItem item, MyGuiControlGrid dropFrom, MyGuiControlGrid dropTo)
		{
			return dropTo != dropFrom;
		}

		private void StartDragging(MyDropHandleType dropHandlingType, MyGuiControlResearchGraph graph, MyGuiGridItem draggingItem)
		{
			if (draggingItem.Enabled)
			{
				MyDragAndDropInfo draggingFrom = new MyDragAndDropInfo();
				m_dragAndDrop.StartDragging(dropHandlingType, MySharedButtonsEnum.Primary, draggingItem, draggingFrom, includeTooltip: false);
				graph.HideToolTip();
			}
		}

		private void StartDragging(MyDropHandleType dropHandlingType, MyGuiControlGrid grid, ref MyGuiControlGrid.EventArgs args)
		{
			MyDragAndDropInfo myDragAndDropInfo = new MyDragAndDropInfo();
			myDragAndDropInfo.Grid = grid;
			myDragAndDropInfo.ItemIndex = args.ItemIndex;
			MyGuiGridItem itemAt = grid.GetItemAt(args.ItemIndex);
			if (itemAt.Enabled)
			{
				m_dragAndDrop.StartDragging(dropHandlingType, args.Button, itemAt, myDragAndDropInfo, includeTooltip: false);
				grid.HideToolTip();
			}
		}

		private bool UpdateContextMenu(ref MyGuiControlContextMenu currentContextMenu, MyToolbarItemActions item, GridItemUserData data)
		{
			ListReader<ITerminalAction> listReader = item.PossibleActions(m_toolbarControl.ShownToolbar.ToolbarType);
			if (listReader.Count > 0)
			{
				currentContextMenu.Enabled = true;
				currentContextMenu.CreateNewContextMenu();
				foreach (ITerminalAction item2 in listReader)
				{
					currentContextMenu.AddItem(item2.Name, "", item2.Icon, item2.Id);
				}
				return true;
			}
			return false;
		}

		public override bool Update(bool hasFocus)
		{
			if (m_framesBeforeSearchEnabled > 0)
			{
				m_framesBeforeSearchEnabled--;
			}
			if (m_framesBeforeSearchEnabled == 0)
			{
				m_searchBox.Enabled = true;
				m_searchBox.TextBox.CanHaveFocus = true;
				if (MyVRage.Platform.ImeProcessor != null)
				{
					MyVRage.Platform.ImeProcessor.RegisterActiveScreen(this);
				}
				base.FocusedControl = m_searchBox.TextBox;
				m_framesBeforeSearchEnabled--;
			}
			if (m_frameCounterPCU >= PCU_UPDATE_EACH_N_FRAMES && m_PCUControl.Visible)
			{
				m_PCUControl.UpdatePCU(GetIdentity(), performAnimation: true);
				m_frameCounterPCU = 0;
			}
			else
			{
				m_frameCounterPCU++;
			}
			bool isJoystickLastUsed = MyInput.Static.IsJoystickLastUsed;
			m_categoryHintLeft.Visible = isJoystickLastUsed;
			m_categoryHintRight.Visible = isJoystickLastUsed;
			return base.Update(hasFocus);
		}
	}
}
