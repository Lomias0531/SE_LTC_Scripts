#define VRAGE
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Data.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenLoadInventory : MyGuiScreenBase
	{
		private enum InventoryItemAction
		{
			Apply,
			Sell,
			Trade,
			Recycle,
			Delete,
			Buy
		}

		private enum TabState
		{
			Character,
			Tools
		}

		private enum LowerTabState
		{
			Coloring,
			Recycling
		}

		private struct CategoryButton
		{
			public MyStringId Tooltip;

			public MyGameInventoryItemSlot Slot;

			public string ImageName;

			public string ButtonText;

			public CategoryButton(MyStringId tooltip, MyGameInventoryItemSlot slot, string imageName = null, string buttonText = null)
			{
				Tooltip = tooltip;
				Slot = slot;
				ImageName = imageName;
				ButtonText = buttonText;
			}
		}

		private static readonly bool SKIN_STORE_FEATURES_ENABLED;

		private readonly string m_hueScaleTexture = "Textures\\GUI\\HueScale.png";

		private readonly string m_equipCheckbox = "equipCheckbox";

		private Vector2 m_itemsTableSize;

		private MyGuiControlButton m_viewDetailsButton;

		private MyGuiControlButton m_openStoreButton;

		private MyGuiControlButton m_refreshButton;

		private MyGuiControlButton m_browseItemsButton;

		private MyGuiControlButton m_characterButton;

		private MyGuiControlButton m_toolsButton;

		private MyGuiControlButton m_recyclingButton;

		private MyGuiControlButton m_currentButton;

		private bool m_inGame;

		private TabState m_activeTabState;

		private LowerTabState m_activeLowTabState;

		private string m_rotatingWheelTexture;

		private MyGuiControlRotatingWheel m_wheel;

		private MyEntityRemoteController m_entityController;

		private List<MyGuiControlCheckbox> m_itemCheckboxes;

		private bool m_itemCheckActive;

		private MyGuiControlCombobox m_modelPicker;

		private MyGuiControlSlider m_sliderHue;

		private MyGuiControlSlider m_sliderSaturation;

		private MyGuiControlSlider m_sliderValue;

		private MyGuiControlLabel m_labelHue;

		private MyGuiControlLabel m_labelSaturation;

		private MyGuiControlLabel m_labelValue;

		private string m_selectedModel;

		private Vector3 m_selectedHSV;

		private Dictionary<string, int> m_displayModels;

		private Dictionary<int, string> m_models;

		private string m_storedModel;

		private Vector3 m_storedHSV;

		private bool m_colorOrModelChanged;

		private Vector3D m_originalSpectatorPosition = Vector3D.Zero;

		private Vector3D m_targetSpectatorPosition = Vector3D.Zero;

		private const float ZOOM_SPEED = 2.5f;

		private bool m_zooming;

		private float m_zoomSpeed;

		private Vector3D m_zoomDirection = Vector3D.Forward;

		private const float ZOOM_ACCELERATION = 0.15f;

		private MyGameInventoryItemSlot m_filteredSlot;

		private MyGuiControlContextMenu m_contextMenu;

		private MyGuiControlImageButton m_contextMenuLastButton;

		private bool m_hideDuplicatesEnabled;

		private bool m_showOnlyDuplicatesEnabled;

		private MyGuiControlParent m_itemsTableParent;

		private List<MyGameInventoryItem> m_userItems;

		private List<MyPhysicalInventoryItem> m_allTools;

		private MyGuiControlCombobox m_toolPicker;

		private string m_selectedTool;

		private MyGuiControlButton m_OkButton;

		private MyGuiControlButton m_cancelButton;

		private MyGuiControlButton m_craftButton;

		private MyGuiControlCombobox m_rarityPicker;

		private MyGameInventoryItem m_lastCraftedItem;

		private MyGuiControlButton m_coloringButton;

		private bool m_audioSet;

		private bool? m_savedStateAnselEnabled;

		public static event MyLookChangeDelegate LookChanged;

		public MyGuiScreenLoadInventory()
			: base(new Vector2(0.32f, 0.05f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.65f, 0.9f), isTopMostScreen: true, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = false;
		}

		public MyGuiScreenLoadInventory(bool inGame = false, HashSet<string> customCharacterNames = null)
			: base(new Vector2(0.32f, 0.05f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.65f, 0.9f), isTopMostScreen: true, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = false;
			Initialize(inGame, customCharacterNames);
		}

		public void Initialize(bool inGame, HashSet<string> customCharacterNames)
		{
			m_inGame = inGame;
			m_audioSet = inGame;
			m_rotatingWheelTexture = "Textures\\GUI\\screens\\screen_loading_wheel_loading_screen.dds";
			base.Align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_filteredSlot = MyGameInventoryItemSlot.None;
			IsHitTestVisible = true;
			MyGameService.CheckItemDataReady += MyGameService_CheckItemDataReady;
			m_storedModel = ((MySession.Static.LocalCharacter != null) ? MySession.Static.LocalCharacter.ModelName : string.Empty);
			InitModels(customCharacterNames);
			m_entityController = new MyEntityRemoteController(MySession.Static.LocalCharacter);
			m_entityController.LockRotationAxis(GlobalAxis.Y | GlobalAxis.Z);
			m_allTools = m_entityController.GetInventoryTools();
			RecreateControls(constructor: true);
			UpdateSliderTooltips();
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenIntroVideo>()?.HideScreen();
			if (!inGame)
			{
				MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter);
			}
			EquipTool();
			UpdateCheckboxes();
			m_isTopMostScreen = false;
		}

		private void InitModels(HashSet<string> customCharacterNames)
		{
			m_displayModels = new Dictionary<string, int>();
			m_models = new Dictionary<int, string>();
			int value = 0;
			if (customCharacterNames == null)
			{
				foreach (MyCharacterDefinition character in MyDefinitionManager.Static.Characters)
				{
					if ((character.UsableByPlayer || (!MySession.Static.SurvivalMode && m_inGame && MySession.Static.Settings.IsSettingsExperimental())) && character.Public)
					{
						string displayName = GetDisplayName(character.Name);
						m_displayModels[displayName] = value;
						m_models[value++] = character.Name;
					}
				}
				return;
			}
			DictionaryValuesReader<string, MyCharacterDefinition> characters = MyDefinitionManager.Static.Characters;
			foreach (string customCharacterName in customCharacterNames)
			{
				if (characters.TryGetValue(customCharacterName, out MyCharacterDefinition result) && (!MySession.Static.SurvivalMode || result.UsableByPlayer) && result.Public)
				{
					string displayName2 = GetDisplayName(result.Name);
					m_displayModels[displayName2] = value;
					m_models[value++] = result.Name;
				}
			}
		}

		private string GetDisplayName(string name)
		{
			return MyTexts.GetString(name);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			if (m_sliderHue != null)
			{
				MyGuiControlSlider sliderHue = m_sliderHue;
				sliderHue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderHue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			}
			if (m_sliderSaturation != null)
			{
				MyGuiControlSlider sliderSaturation = m_sliderSaturation;
				sliderSaturation.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderSaturation.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			}
			if (m_sliderValue != null)
			{
				MyGuiControlSlider sliderValue = m_sliderValue;
				sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			}
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.072f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.147f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.228f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.548f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.629f), 0.5385f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0.056f, 0.778f), 0.5385f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlStackPanel myGuiControlStackPanel = new MyGuiControlStackPanel();
			myGuiControlStackPanel.BackgroundTexture = MyGuiConstants.TEXTURE_COMPOSITE_ROUND_ALL;
			myGuiControlStackPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			MyGuiControlStackPanel myGuiControlStackPanel2 = new MyGuiControlStackPanel();
			myGuiControlStackPanel2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlStackPanel2.Margin = new Thickness(0.016f, 0.009f, 0.015f, 0.015f);
			myGuiControlStackPanel2.Orientation = MyGuiOrientation.Horizontal;
			m_coloringButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryColoring, MyCommonTexts.ScreenLoadInventoryColoringFilterTooltip, OnViewTabColoring);
			m_coloringButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
			m_coloringButton.Margin = new Thickness(0.0415f, 0.0285f, 0.0025f, 0f);
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				m_coloringButton.HighlightType = MyGuiControlHighlightType.FORCED;
				m_coloringButton.HasHighlight = true;
				m_coloringButton.Selected = true;
			}
			myGuiControlStackPanel2.Add(m_coloringButton);
			Controls.Add(m_coloringButton);
			m_recyclingButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryRecycling, MyCommonTexts.ScreenLoadInventoryRecyclingFilterTooltip, OnViewTabRecycling);
			m_recyclingButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
			m_recyclingButton.Margin = new Thickness(0.0025f, 0.0285f, 0.0025f, 0f);
			if (m_activeLowTabState == LowerTabState.Recycling)
			{
				m_recyclingButton.HighlightType = MyGuiControlHighlightType.FORCED;
				m_recyclingButton.HasHighlight = true;
				m_recyclingButton.Selected = true;
			}
			myGuiControlStackPanel2.Add(m_recyclingButton);
			Controls.Add(m_recyclingButton);
			MyGuiControlStackPanel myGuiControlStackPanel3 = new MyGuiControlStackPanel();
			myGuiControlStackPanel3.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlStackPanel3.Margin = new Thickness(0.016f, 0f, 0.015f, 0.015f);
			myGuiControlStackPanel3.Orientation = MyGuiOrientation.Horizontal;
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_HIGHLIGHT
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_HIGHLIGHT
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_NORMAL
				}
			};
			Vector2 size = new Vector2(0.14f, 0.05f);
			MyGuiControlImageButton.StyleDefinition style2 = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_HIGHLIGHT
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_HIGHLIGHT
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = MyGuiConstants.TEXTURE_BUTTON_SKINS_NORMAL
				}
			};
			m_characterButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryCharacter, MyCommonTexts.ScreenLoadInventoryCharacterFilterTooltip, OnViewTabCharacter);
			m_characterButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
			m_characterButton.Margin = new Thickness(0.0415f, 0.0285f, 0.0025f, 0f);
			List<CategoryButton> list = new List<CategoryButton>();
			if (m_activeTabState == TabState.Character)
			{
				base.FocusedControl = m_characterButton;
				m_characterButton.HighlightType = MyGuiControlHighlightType.FORCED;
				m_characterButton.HasHighlight = true;
				m_characterButton.Selected = true;
				list.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryHelmetTooltip, MyGameInventoryItemSlot.Helmet, "Textures\\GUI\\Icons\\Skins\\Categories\\helmet.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryHelmet)));
				list.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventorySuitTooltip, MyGameInventoryItemSlot.Suit, "Textures\\GUI\\Icons\\Skins\\Categories\\suit.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventorySuit)));
				list.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryGlovesTooltip, MyGameInventoryItemSlot.Gloves, "Textures\\GUI\\Icons\\Skins\\Categories\\glove.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryGloves)));
				list.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryBootsTooltip, MyGameInventoryItemSlot.Boots, "Textures\\GUI\\Icons\\Skins\\Categories\\boot.png", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryBoots)));
			}
			myGuiControlStackPanel3.Add(m_characterButton);
			Controls.Add(m_characterButton);
			m_toolsButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryTools, MyCommonTexts.ScreenLoadInventoryToolsFilterTooltip, OnViewTools);
			m_toolsButton.VisualStyle = MyGuiControlButtonStyleEnum.ToolbarButton;
			m_toolsButton.Margin = new Thickness(0.0025f, 0.0285f, 0f, 0f);
			if (m_activeTabState == TabState.Tools)
			{
				base.FocusedControl = m_toolsButton;
				m_toolsButton.HighlightType = MyGuiControlHighlightType.FORCED;
				m_toolsButton.HasHighlight = true;
				m_toolsButton.Selected = true;
				list.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryWelderTooltip, MyGameInventoryItemSlot.Welder, "Textures\\GUI\\Icons\\WeaponWelder.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryWelder)));
				list.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryGrinderTooltip, MyGameInventoryItemSlot.Grinder, "Textures\\GUI\\Icons\\WeaponGrinder.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryGrinder)));
				list.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryDrillTooltip, MyGameInventoryItemSlot.Drill, "Textures\\GUI\\Icons\\WeaponDrill.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryDrill)));
				list.Add(new CategoryButton(MyCommonTexts.ScreenLoadInventoryRifleTooltip, MyGameInventoryItemSlot.Rifle, "Textures\\GUI\\Icons\\WeaponAutomaticRifle.dds", MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryRifle)));
			}
			myGuiControlStackPanel3.Add(m_toolsButton);
			Controls.Add(m_toolsButton);
			MyGuiControlStackPanel myGuiControlStackPanel4 = new MyGuiControlStackPanel();
			myGuiControlStackPanel4.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlStackPanel4.Margin = new Thickness(0.055f, 0.05f, 0f, 0f);
			myGuiControlStackPanel4.Orientation = MyGuiOrientation.Horizontal;
			foreach (CategoryButton item in list)
			{
				MyGuiControlImageButton myGuiControlImageButton = MakeImageButton(Vector2.Zero, size, style, item.Tooltip, OnCategoryClicked);
				myGuiControlImageButton.UserData = item.Slot;
				myGuiControlImageButton.Margin = new Thickness(0f, 0f, 0.004f, 0f);
				myGuiControlStackPanel4.Add(myGuiControlImageButton);
			}
			if (m_modelPicker != null)
			{
				m_modelPicker.ItemSelected -= OnItemSelected;
			}
			MyGuiControlStackPanel myGuiControlStackPanel5 = new MyGuiControlStackPanel();
			myGuiControlStackPanel5.Orientation = MyGuiOrientation.Horizontal;
			myGuiControlStackPanel5.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlStackPanel5.Margin = new Thickness(0.014f);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.PlayerCharacterModel));
			myGuiControlLabel.Margin = new Thickness(0.045f, -0.03f, 0.01f, 0f);
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				m_modelPicker = new MyGuiControlCombobox();
				m_modelPicker.Size = new Vector2(0.225f, 1f);
				m_modelPicker.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_modelPicker.Margin = new Thickness(0.015f, -0.03f, 0.01f, 0f);
				m_modelPicker.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipCharacterScreen_Model));
				foreach (KeyValuePair<string, int> displayModel in m_displayModels)
				{
					m_modelPicker.AddItem(displayModel.Value, new StringBuilder(displayModel.Key));
				}
				m_selectedModel = MySession.Static.LocalCharacter.ModelName;
				if (m_displayModels.ContainsKey(GetDisplayName(m_selectedModel)))
				{
					m_modelPicker.SelectItemByKey(m_displayModels[GetDisplayName(m_selectedModel)]);
				}
				else if (m_displayModels.Count > 0)
				{
					m_modelPicker.SelectItemByKey(m_displayModels.First().Value);
				}
				m_modelPicker.ItemSelected += OnItemSelected;
				if (m_activeTabState == TabState.Character || m_activeTabState == TabState.Tools)
				{
					myGuiControlStackPanel5.Add(myGuiControlLabel);
					myGuiControlStackPanel5.Add(m_modelPicker);
					Controls.Add(myGuiControlLabel);
					Controls.Add(m_modelPicker);
				}
			}
			if (m_activeTabState == TabState.Tools && m_filteredSlot != 0)
			{
				m_toolPicker = new MyGuiControlCombobox();
				m_toolPicker.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_toolPicker.Margin = new Thickness(0.015f, 0.01f, 0.01f, 0.01f);
				foreach (MyPhysicalInventoryItem allTool in m_allTools)
				{
					if (m_entityController.GetToolSlot(allTool.Content.SubtypeName) == m_filteredSlot)
					{
						MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(allTool.Content);
						if (physicalItemDefinition != null)
						{
							m_toolPicker.AddItem(allTool.ItemId, new StringBuilder(physicalItemDefinition.DisplayNameText));
						}
						else
						{
							m_toolPicker.AddItem(allTool.ItemId, new StringBuilder(allTool.Content.SubtypeName));
						}
					}
				}
				if (string.IsNullOrEmpty(m_selectedTool))
				{
					if (m_toolPicker.GetItemsCount() > 0)
					{
						m_toolPicker.SelectItemByIndex(0);
						uint key = (uint)m_toolPicker.GetSelectedKey();
						MyPhysicalInventoryItem myPhysicalInventoryItem = m_allTools.FirstOrDefault((MyPhysicalInventoryItem t) => t.ItemId == key);
						if (myPhysicalInventoryItem.Content != null)
						{
							m_selectedTool = myPhysicalInventoryItem.Content.SubtypeName;
						}
					}
				}
				else
				{
					MyPhysicalInventoryItem myPhysicalInventoryItem2 = m_allTools.FirstOrDefault((MyPhysicalInventoryItem t) => t.Content.SubtypeName == m_selectedTool);
					m_toolPicker.SelectItemByKey(myPhysicalInventoryItem2.ItemId);
				}
				m_toolPicker.ItemSelected += m_toolPicker_ItemSelected;
				new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryToolType)).Margin = new Thickness(0f, 0.01f, 0.01f, 0.01f);
			}
			m_itemsTableSize = new Vector2(0.582f, 0.29f);
			m_itemsTableParent = new MyGuiControlParent(null, new Vector2(m_itemsTableSize.X, 0.1f));
			m_itemsTableParent.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_itemsTableParent.SkipForMouseTest = true;
			MyGuiControlWrapPanel myGuiControlWrapPanel = CreateItemsTable(m_itemsTableParent);
			myGuiControlStackPanel.Add(myGuiControlWrapPanel);
			myGuiControlStackPanel.Add(myGuiControlStackPanel2);
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				myGuiControlCheckbox.IsChecked = m_hideDuplicatesEnabled;
				myGuiControlCheckbox.IsCheckedChanged = OnHideDuplicates;
				myGuiControlCheckbox.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipCharacterScreen_HideDuplicates));
				myGuiControlCheckbox.Margin = new Thickness(0.039f, -0.03f, 0.01f, 0.01f);
				myGuiControlStackPanel5.Add(myGuiControlCheckbox);
				Controls.Add(myGuiControlCheckbox);
				MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryHideDuplicates));
				myGuiControlLabel2.Margin = new Thickness(0.005f, -0.03f, 0.01f, 0.01f);
				myGuiControlStackPanel5.Add(myGuiControlLabel2);
				Controls.Add(myGuiControlLabel2);
			}
			else
			{
				MyGuiControlCheckbox myGuiControlCheckbox2 = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
				myGuiControlCheckbox2.IsChecked = m_showOnlyDuplicatesEnabled;
				myGuiControlCheckbox2.IsCheckedChanged = OnShowOnlyDuplicates;
				myGuiControlCheckbox2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipCharacterScreen_ShowOnlyDuplicates));
				myGuiControlCheckbox2.Margin = new Thickness(0.039f, -0.03f, 0.01f, 0.01f);
				myGuiControlStackPanel5.Add(myGuiControlCheckbox2);
				Controls.Add(myGuiControlCheckbox2);
				MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryShowOnlyDuplicates));
				myGuiControlLabel3.Margin = new Thickness(0.005f, -0.03f, 0.01f, 0.01f);
				myGuiControlStackPanel5.Add(myGuiControlLabel3);
				Controls.Add(myGuiControlLabel3);
				MyGuiControlLabel myGuiControlLabel4 = new MyGuiControlLabel(null, null, string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryCurrencyCurrent), MyGameService.RecycleTokens));
				myGuiControlLabel4.Margin = new Thickness(0.19f, -0.05f, 0.01f, 0.01f);
				myGuiControlStackPanel5.Add(myGuiControlLabel4);
				Controls.Add(myGuiControlLabel4);
			}
			MyGuiControlStackPanel myGuiControlStackPanel6 = new MyGuiControlStackPanel();
			myGuiControlStackPanel6.Orientation = MyGuiOrientation.Horizontal;
			myGuiControlStackPanel6.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlStackPanel6.Margin = new Thickness(0.018f);
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				bool enabled = m_OkButton == null || (m_OkButton != null && m_OkButton.Enabled);
				m_OkButton = MakeButton(Vector2.Zero, MyCommonTexts.Ok, MyCommonTexts.ScreenLoadInventoryOkTooltip, OnOkClick);
				m_OkButton.Enabled = enabled;
				m_OkButton.Margin = new Thickness(0.0395f, -0.029f, 0.0075f, 0f);
				myGuiControlStackPanel6.Add(m_OkButton);
			}
			else
			{
				m_craftButton = MakeButton(Vector2.Zero, MyCommonTexts.CraftButton, MyCommonTexts.ScreenLoadInventoryCraftTooltip, OnCraftClick);
				m_craftButton.Enabled = true;
				m_craftButton.Margin = new Thickness(0.0395f, -0.029f, 0.0075f, 0f);
				myGuiControlStackPanel6.Add(m_craftButton);
				uint craftingCost = MyGameService.GetCraftingCost(MyGameInventoryItemQuality.Common);
				m_craftButton.Text = string.Format(MyTexts.GetString(MyCommonTexts.CraftButton), craftingCost);
				m_craftButton.Enabled = (MyGameService.RecycleTokens >= craftingCost);
			}
			m_cancelButton = MakeButton(Vector2.Zero, MyCommonTexts.Cancel, MyCommonTexts.ScreenLoadInventoryCancelTooltip, OnCancelClick);
			m_cancelButton.Margin = new Thickness(0f, -0.029f, 0.0075f, 0.03f);
			myGuiControlStackPanel6.Add(m_cancelButton);
			if (SKIN_STORE_FEATURES_ENABLED)
			{
				m_openStoreButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadInventoryBrowseItems, MyCommonTexts.ScreenLoadInventoryBrowseItems, OnOpenStore);
				myGuiControlStackPanel6.Add(m_openStoreButton);
			}
			else
			{
				m_refreshButton = MakeButton(Vector2.Zero, MyCommonTexts.ScreenLoadSubscribedWorldRefresh, MyCommonTexts.ScreenLoadInventoryRefreshTooltip, OnRefreshClick);
				m_refreshButton.Margin = new Thickness(0f, -0.029f, 0f, 0f);
				myGuiControlStackPanel6.Add(m_refreshButton);
			}
			m_wheel = new MyGuiControlRotatingWheel(Vector2.Zero, MyGuiConstants.ROTATING_WHEEL_COLOR, 0.2f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, m_rotatingWheelTexture, manualRotationUpdate: false, MyPerGameSettings.GUI.MultipleSpinningWheels);
			m_wheel.ManualRotationUpdate = false;
			m_wheel.Margin = new Thickness(0.21f, 0.047f, 0f, 0f);
			Elements.Add(m_wheel);
			myGuiControlStackPanel3.Add(m_wheel);
			MyGuiControlStackPanel myGuiControlStackPanel7 = new MyGuiControlStackPanel();
			myGuiControlStackPanel7.Orientation = MyGuiOrientation.Horizontal;
			myGuiControlStackPanel7.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				m_sliderHue = new MyGuiControlSlider(null, 0f, 360f, 0.177f, null, null, null, 0, 0.8f, 0f, "White", string.Empty, MyGuiControlSliderStyleEnum.Hue, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
				m_sliderHue.Margin = new Thickness(0.055f, -0.0425f, 0f, 0f);
				m_sliderHue.Enabled = (m_activeTabState == TabState.Character);
				myGuiControlStackPanel7.Add(m_sliderHue);
				Controls.Add(m_sliderHue);
				m_sliderSaturation = new MyGuiControlSlider(null, 0f, 1f, 0.177f, 0f, null, null, 1, 0.8f, 0f, "White", string.Empty, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				m_sliderSaturation.Margin = new Thickness(0.0052f, -0.0425f, 0f, 0f);
				m_sliderSaturation.Enabled = (m_activeTabState == TabState.Character);
				myGuiControlStackPanel7.Add(m_sliderSaturation);
				Controls.Add(m_sliderSaturation);
				m_sliderValue = new MyGuiControlSlider(null, 0f, 1f, 0.177f, 0f, null, null, 1, 0.8f, 0f, "White", string.Empty, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				m_sliderValue.Margin = new Thickness(0.006f, -0.0425f, 0f, 0f);
				m_sliderValue.Enabled = (m_activeTabState == TabState.Character);
				myGuiControlStackPanel7.Add(m_sliderValue);
				Controls.Add(m_sliderValue);
			}
			else
			{
				CreateRecyclerUI(myGuiControlStackPanel7);
			}
			GridLength[] columns = new GridLength[1]
			{
				new GridLength(1f)
			};
			GridLength[] rows = new GridLength[7]
			{
				new GridLength(0.6f),
				new GridLength(0.5f),
				new GridLength(0.8f),
				new GridLength(4.6f),
				new GridLength(0.6f),
				new GridLength(0.6f),
				new GridLength(0.8f)
			};
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = new MyGuiControlLayoutGrid(columns, rows);
			myGuiControlLayoutGrid.Size = new Vector2(0.65f, 0.9f);
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel3, 0, 1);
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel4, 0, 2);
			if (MyGameService.InventoryItems != null && MyGameService.InventoryItems.Count == 0)
			{
				MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryNoItem));
				myGuiControlLabel5.Margin = new Thickness(0.015f);
				myGuiControlLayoutGrid.Add(myGuiControlLabel5, 0, 3);
				Elements.Add(myGuiControlLabel5);
			}
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel, 0, 3);
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel5, 0, 4);
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel7, 0, 5);
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel6, 0, 6);
			MyGuiControlLabel myGuiControlLabel6 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenCaptionInventory), Vector4.One, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
			myGuiControlLabel6.Name = "CaptionLabel";
			myGuiControlLabel6.Font = "ScreenCaption";
			Elements.Add(myGuiControlLabel6);
			myGuiControlLayoutGrid.Add(myGuiControlLabel6, 0, 0);
			GridLength[] columns2 = new GridLength[1]
			{
				new GridLength(1f)
			};
			GridLength[] rows2 = new GridLength[1]
			{
				new GridLength(1f)
			};
			MyGuiControlLayoutGrid myGuiControlLayoutGrid2 = new MyGuiControlLayoutGrid(columns2, rows2);
			myGuiControlLayoutGrid2.Size = new Vector2(1f, 1f);
			myGuiControlLayoutGrid2.Add(myGuiControlLayoutGrid, 0, 0);
			myGuiControlLayoutGrid2.UpdateMeasure();
			m_itemsTableParent.Size = new Vector2(m_itemsTableSize.X, myGuiControlWrapPanel.Size.Y);
			myGuiControlWrapPanel.Size = m_itemsTableSize;
			myGuiControlLayoutGrid2.UpdateArrange();
			m_itemsTableParent.Position = myGuiControlWrapPanel.Position;
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(m_itemsTableParent);
			myGuiControlScrollablePanel.ContentOffset = new Vector2(0f - m_itemsTableSize.X, 0f - m_itemsTableParent.Size.Y - 0.575f) / 2f;
			myGuiControlScrollablePanel.ScrollBarOffset = new Vector2(0.005f, 0f);
			myGuiControlScrollablePanel.ScrollbarVEnabled = true;
			myGuiControlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlScrollablePanel.Size = m_itemsTableSize;
			myGuiControlScrollablePanel.Position = myGuiControlWrapPanel.Position;
			Controls.Add(myGuiControlScrollablePanel);
			myGuiControlLabel6.Position = new Vector2(myGuiControlLabel6.Position.X + base.Size.Value.X / 2f, myGuiControlLabel6.Position.Y + MyGuiConstants.SCREEN_CAPTION_DELTA_Y / 3f + 0.023f);
			foreach (MyGuiControlBase control2 in myGuiControlStackPanel6.GetControls())
			{
				Controls.Add(control2);
			}
			List<MyGuiControlBase> controls = myGuiControlStackPanel4.GetControls();
			for (int i = 0; i < controls.Count; i++)
			{
				MyGuiControlImageButton myGuiControlImageButton2 = controls[i] as MyGuiControlImageButton;
				Controls.Add(myGuiControlImageButton2);
				if (m_filteredSlot != 0 && m_filteredSlot == (MyGameInventoryItemSlot)myGuiControlImageButton2.UserData)
				{
					myGuiControlImageButton2.ApplyStyle(style2);
					myGuiControlImageButton2.HighlightType = MyGuiControlHighlightType.FORCED;
					myGuiControlImageButton2.HasHighlight = true;
					myGuiControlImageButton2.Selected = true;
				}
				myGuiControlImageButton2.Size = new Vector2(0.1f, 0.1f);
				float num = myGuiControlImageButton2.Position.X;
				if (!string.IsNullOrEmpty(list[i].ImageName))
				{
					MyGuiControlImage myGuiControlImage = new MyGuiControlImage(size: new Vector2(0.03f, 0.04f), position: myGuiControlImageButton2.Position + new Vector2(0.005f, 0.001f), backgroundColor: null, backgroundTexture: null, textures: new string[1]
					{
						list[i].ImageName
					}, toolTip: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					Controls.Add(myGuiControlImage);
					num += myGuiControlImage.Size.X;
				}
				if (!string.IsNullOrEmpty(list[i].ButtonText))
				{
					MyGuiControlLabel control = new MyGuiControlLabel(size: myGuiControlImageButton2.Size, position: new Vector2((num + myGuiControlImageButton2.Position.X + myGuiControlImageButton2.Size.X) / 2f, myGuiControlImageButton2.Position.Y + myGuiControlImageButton2.Size.Y / 2.18f), text: list[i].ButtonText, colorMask: null, textScale: 0.8f, font: "Blue", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					Controls.Add(control);
				}
			}
			m_originalSpectatorPosition = MySpectatorCameraController.Static.Position;
			m_wheel.Visible = false;
			base.CloseButtonEnabled = true;
			m_storedHSV = MySession.Static.LocalCharacter.ColorMask;
			m_selectedHSV = m_storedHSV;
			m_sliderHue.Value = m_selectedHSV.X * 360f;
			m_sliderSaturation.Value = MathHelper.Clamp(m_selectedHSV.Y + MyColorPickerConstants.SATURATION_DELTA, 0f, 1f);
			m_sliderValue.Value = MathHelper.Clamp(m_selectedHSV.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0f, 1f);
			MyGuiControlSlider sliderHue2 = m_sliderHue;
			sliderHue2.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderHue2.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderSaturation2 = m_sliderSaturation;
			sliderSaturation2.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderSaturation2.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderValue2 = m_sliderValue;
			sliderValue2.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderValue2.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			m_contextMenu = new MyGuiControlContextMenu();
			m_contextMenu.CreateNewContextMenu();
			if (SKIN_STORE_FEATURES_ENABLED && MyGameService.IsOverlayEnabled)
			{
				StringBuilder stringBuilder = MyTexts.Get(MyCommonTexts.ScreenLoadInventoryBuyItem);
				m_contextMenu.AddItem(stringBuilder, stringBuilder.ToString(), "", InventoryItemAction.Buy);
			}
			StringBuilder stringBuilder2 = MyTexts.Get(MyCommonTexts.ScreenLoadInventorySellItem);
			if (MyGameService.IsOverlayEnabled)
			{
				m_contextMenu.AddItem(stringBuilder2, stringBuilder2.ToString(), "", InventoryItemAction.Sell);
			}
			StringBuilder stringBuilder3 = MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItem);
			string.Format(stringBuilder3.ToString(), 0);
			m_contextMenu.AddItem(stringBuilder3, string.Empty, "", InventoryItemAction.Recycle);
			m_contextMenu.ItemClicked += m_contextMenu_ItemClicked;
			Controls.Add(m_contextMenu);
			m_contextMenu.Deactivate();
			if (constructor)
			{
				m_colorOrModelChanged = false;
			}
			UpdateSliderTooltips();
		}

		private void OnViewTabColoring(MyGuiControlButton obj)
		{
			m_activeLowTabState = LowerTabState.Coloring;
			EquipTool();
			RecreateControls(constructor: false);
			UpdateCheckboxes();
		}

		private void CreateRecyclerUI(MyGuiControlStackPanel panel)
		{
			GridLength[] columns = new GridLength[3]
			{
				new GridLength(1.4f),
				new GridLength(0.6f),
				new GridLength(0.8f)
			};
			GridLength[] rows = new GridLength[1]
			{
				new GridLength(1f)
			};
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = new MyGuiControlLayoutGrid(columns, rows);
			myGuiControlLayoutGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlLayoutGrid.Margin = new Thickness(0.055f, -0.035f, 0f, 0f);
			myGuiControlLayoutGrid.Size = new Vector2(0.65f, 0.1f);
			MyGuiControlStackPanel myGuiControlStackPanel = new MyGuiControlStackPanel();
			myGuiControlStackPanel.Orientation = MyGuiOrientation.Horizontal;
			myGuiControlStackPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			myGuiControlStackPanel.Margin = new Thickness(0f);
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventorySelectRarity));
			myGuiControlLabel.Margin = new Thickness(0f, 0f, 0.01f, 0f);
			myGuiControlStackPanel.Add(myGuiControlLabel);
			Controls.Add(myGuiControlLabel);
			m_rarityPicker = new MyGuiControlCombobox();
			m_rarityPicker.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_rarityPicker.Size = new Vector2(0.15f, 0f);
			foreach (object value3 in Enum.GetValues(typeof(MyGameInventoryItemQuality)))
			{
				m_rarityPicker.AddItem((int)value3, MyTexts.GetString(MyStringId.GetOrCompute(value3.ToString())));
			}
			m_rarityPicker.SelectItemByIndex(0);
			m_rarityPicker.ItemSelected += m_rarityPicker_ItemSelected;
			myGuiControlStackPanel.Add(m_rarityPicker);
			Controls.Add(m_rarityPicker);
			if (m_lastCraftedItem != null)
			{
				Vector2 value = new Vector2(0.07f, 0.09f);
				MyGuiControlImage myGuiControlImage = null;
				if (string.IsNullOrEmpty(m_lastCraftedItem.ItemDefinition.BackgroundColor))
				{
					myGuiControlImage = new MyGuiControlImage(null, value, null, null, new string[1]
					{
						"Textures\\GUI\\Controls\\grid_item_highlight.dds"
					}, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
					Controls.Add(myGuiControlImage);
				}
				else
				{
					Vector4 value2 = string.IsNullOrEmpty(m_lastCraftedItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(m_lastCraftedItem.ItemDefinition.BackgroundColor);
					myGuiControlImage = new MyGuiControlImage(null, new Vector2(value.X - 0.004f, value.Y - 0.002f), value2, null, new string[1]
					{
						"Textures\\GUI\\blank.dds"
					});
					Controls.Add(myGuiControlImage);
				}
				string[] array = new string[1]
				{
					"Textures\\GUI\\Blank.dds"
				};
				if (!string.IsNullOrEmpty(m_lastCraftedItem.ItemDefinition.IconTexture))
				{
					array[0] = m_lastCraftedItem.ItemDefinition.IconTexture;
				}
				MyGuiControlImage control = new MyGuiControlImage(null, new Vector2(0.06f, 0.08f), null, null, array);
				Controls.Add(control);
				MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, m_lastCraftedItem.ItemDefinition.Name, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				Controls.Add(control2);
				MyGuiControlLabel myGuiControlLabel2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryCraftedLabel), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
				myGuiControlLabel2.Margin = new Thickness(0f, -0.035f, 0f, 0f);
				Controls.Add(myGuiControlLabel2);
				myGuiControlLayoutGrid.Add(myGuiControlImage, 1, 0);
				myGuiControlLayoutGrid.Add(control, 1, 0);
				myGuiControlLayoutGrid.Add(control2, 2, 0);
				myGuiControlLayoutGrid.Add(myGuiControlLabel2, 2, 0);
			}
			myGuiControlLayoutGrid.Add(myGuiControlStackPanel, 0, 0);
			panel.Add(myGuiControlLayoutGrid);
		}

		private void OnOkClick(MyGuiControlButton obj)
		{
			if (m_colorOrModelChanged && MyGuiScreenLoadInventory.LookChanged != null && MySession.Static != null)
			{
				MyGuiScreenLoadInventory.LookChanged();
			}
			if (MySession.Static.LocalCharacter.Definition.UsableByPlayer)
			{
				MyLocalCache.SaveInventoryConfig(MySession.Static.LocalCharacter);
			}
			CloseScreen();
		}

		private void OnCancelClick(MyGuiControlButton obj)
		{
			Cancel();
			CloseScreen();
		}

		private void OnCraftClick(MyGuiControlButton obj)
		{
			MyGameService.ItemsAdded -= MyGameService_ItemsAdded;
			MyGameService.ItemsAdded += MyGameService_ItemsAdded;
			if (MyGameService.CraftSkin((MyGameInventoryItemQuality)m_rarityPicker.GetSelectedKey()))
			{
				RotatingWheelShow();
			}
			else
			{
				MyGameService.ItemsAdded -= MyGameService_ItemsAdded;
			}
		}

		private void MyGameService_ItemsAdded(object sender, MyGameItemsEventArgs e)
		{
			if (e.NewItems != null && e.NewItems.Count > 0)
			{
				m_lastCraftedItem = e.NewItems[0];
				m_lastCraftedItem.IsNew = true;
				MyGameService.ItemsAdded -= MyGameService_ItemsAdded;
				RefreshUI();
			}
			RotatingWheelHide();
		}

		private static void Cancel()
		{
			if (MyGameService.InventoryItems != null)
			{
				foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
				{
					inventoryItem.IsInUse = false;
				}
			}
			MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter);
		}

		private void m_toolPicker_ItemSelected()
		{
			MyPhysicalInventoryItem myPhysicalInventoryItem = m_allTools.FirstOrDefault((MyPhysicalInventoryItem t) => t.ItemId == m_toolPicker.GetSelectedKey());
			if (myPhysicalInventoryItem.Content != null)
			{
				m_selectedTool = myPhysicalInventoryItem.Content.SubtypeName;
				EquipTool();
			}
		}

		private void m_rarityPicker_ItemSelected()
		{
			uint craftingCost = MyGameService.GetCraftingCost((MyGameInventoryItemQuality)m_rarityPicker.GetSelectedIndex());
			m_craftButton.Text = string.Format(MyTexts.GetString(MyCommonTexts.CraftButton), craftingCost);
			m_craftButton.Enabled = (MyGameService.RecycleTokens >= craftingCost);
		}

		private void OnHideDuplicates(MyGuiControlCheckbox obj)
		{
			m_hideDuplicatesEnabled = obj.IsChecked;
			RefreshUI();
		}

		private void OnShowOnlyDuplicates(MyGuiControlCheckbox obj)
		{
			m_showOnlyDuplicatesEnabled = obj.IsChecked;
			RefreshUI();
		}

		private void m_contextMenu_ItemClicked(MyGuiControlContextMenu contextMenu, MyGuiControlContextMenu.EventArgs selectedItem)
		{
			switch ((InventoryItemAction)selectedItem.UserData)
			{
			case InventoryItemAction.Trade:
				break;
			case InventoryItemAction.Apply:
				hiddenButton_ButtonClicked(m_contextMenuLastButton);
				break;
			case InventoryItemAction.Sell:
				OpenUserInventory();
				break;
			case InventoryItemAction.Recycle:
				RecycleItemRequest();
				break;
			case InventoryItemAction.Delete:
				DeleteItemRequest();
				break;
			case InventoryItemAction.Buy:
				OpenCurrentItemInStore();
				break;
			}
		}

		private void OnViewTools(MyGuiControlButton obj)
		{
			m_activeTabState = TabState.Tools;
			m_filteredSlot = MyGameInventoryItemSlot.Welder;
			m_selectedTool = string.Empty;
			RefreshUI();
		}

		private void OnViewTabCharacter(MyGuiControlButton obj)
		{
			m_activeTabState = TabState.Character;
			m_filteredSlot = MyGameInventoryItemSlot.None;
			m_selectedTool = string.Empty;
			EquipTool();
			RecreateControls(constructor: false);
			UpdateCheckboxes();
		}

		private void OnViewTabRecycling(MyGuiControlButton obj)
		{
			m_activeLowTabState = LowerTabState.Recycling;
			EquipTool();
			RecreateControls(constructor: false);
			UpdateCheckboxes();
		}

		private void OnItemSelected()
		{
			Cancel();
			int key = (int)m_modelPicker.GetSelectedKey();
			m_selectedModel = m_models[key];
			ChangeCharacter(m_selectedModel, m_selectedHSV);
			RecreateControls(constructor: false);
			MyLocalCache.ResetAllInventorySlots(MySession.Static.LocalCharacter);
			RefreshItems();
		}

		private void ChangeCharacter(string model, Vector3 colorMaskHSV, bool resetToDefault = true)
		{
			m_colorOrModelChanged = true;
			MySession.Static.LocalCharacter.ChangeModelAndColor(model, colorMaskHSV, resetToDefault, MySession.Static.LocalPlayerId);
		}

		public static void ResetOnFinish(string model, bool resetToDefault)
		{
			MyGuiScreenLoadInventory firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoadInventory>();
			if (firstScreenOfType != null && !(firstScreenOfType.m_selectedModel == MySession.Static.LocalCharacter.ModelName))
			{
				if (resetToDefault)
				{
					firstScreenOfType.ResetOnFinishInternal(model);
				}
				firstScreenOfType.RecreateControls(constructor: false);
				MyLocalCache.ResetAllInventorySlots(MySession.Static.LocalCharacter);
				firstScreenOfType.RefreshItems();
			}
		}

		private void ResetOnFinishInternal(string model)
		{
			if (model == "Default_Astronaut" || model == "Default_Astronaut_Female")
			{
				MyLocalCache.LoadInventoryConfig(MySession.Static.LocalCharacter, setModel: false);
				return;
			}
			int key = (int)m_modelPicker.GetSelectedKey();
			m_selectedModel = m_models[key];
			Cancel();
			RecreateControls(constructor: false);
			MyLocalCache.ResetAllInventorySlots(MySession.Static.LocalCharacter);
			RefreshItems();
		}

		private void OnValueChange(MyGuiControlSlider sender)
		{
			UpdateSliderTooltips();
			m_selectedHSV.X = m_sliderHue.Value / 360f;
			m_selectedHSV.Y = m_sliderSaturation.Value - MyColorPickerConstants.SATURATION_DELTA;
			m_selectedHSV.Z = m_sliderValue.Value - MyColorPickerConstants.VALUE_DELTA + MyColorPickerConstants.VALUE_COLORIZE_DELTA;
			ChangeCharacter(m_selectedModel, m_selectedHSV, resetToDefault: false);
		}

		private void UpdateSliderTooltips()
		{
			m_sliderHue.Tooltips.ToolTips.Clear();
			m_sliderHue.Tooltips.AddToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryHue), m_sliderHue.Value));
			m_sliderSaturation.Tooltips.ToolTips.Clear();
			m_sliderSaturation.Tooltips.AddToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventorySaturation), m_sliderSaturation.Value.ToString("P1")));
			m_sliderValue.Tooltips.ToolTips.Clear();
			m_sliderValue.Tooltips.AddToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryValue), m_sliderValue.Value.ToString("P1")));
		}

		private MyGuiControlWrapPanel CreateItemsTable(MyGuiControlParent parent)
		{
			Vector2 vector = new Vector2(0.07f, 0.09f);
			MyGuiControlWrapPanel myGuiControlWrapPanel = new MyGuiControlWrapPanel(vector);
			myGuiControlWrapPanel.Size = m_itemsTableSize;
			myGuiControlWrapPanel.Margin = new Thickness(0.018f, 0.044f, 0f, 0f);
			myGuiControlWrapPanel.InnerOffset = new Vector2(0.005f, 0.0065f);
			myGuiControlWrapPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			MyGuiControlImageButton.StyleDefinition style = new MyGuiControlImageButton.StyleDefinition
			{
				Highlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture("Textures\\Gui\\Screens\\screen_background_fade.dds")
				},
				ActiveHighlight = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture("Textures\\Gui\\Screens\\screen_background_fade.dds")
				},
				Normal = new MyGuiControlImageButton.StateDefinition
				{
					Texture = new MyGuiCompositeTexture("Textures\\Gui\\Screens\\screen_background_fade.dds")
				}
			};
			MyGuiControlCheckbox.StyleDefinition style2 = new MyGuiControlCheckbox.StyleDefinition
			{
				NormalCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_GREEN_CHECKED,
				NormalUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_BLANK,
				HighlightCheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_BLANK,
				HighlightUncheckedTexture = MyGuiConstants.TEXTURE_CHECKBOX_BLANK
			};
			m_itemCheckboxes = new List<MyGuiControlCheckbox>();
			m_userItems = GetInventoryItems();
			if (SKIN_STORE_FEATURES_ENABLED)
			{
				List<MyGameInventoryItem> storeItems = GetStoreItems(m_userItems);
				m_userItems.AddRange(storeItems);
			}
			for (int i = 0; i < m_userItems.Count; i++)
			{
				MyGameInventoryItem myGameInventoryItem = m_userItems[i];
				if (myGameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.None || myGameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Emote || myGameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Armor || (m_filteredSlot != 0 && m_filteredSlot != myGameInventoryItem.ItemDefinition.ItemSlot))
				{
					continue;
				}
				if (m_filteredSlot == MyGameInventoryItemSlot.None)
				{
					MyGameInventoryItemSlot itemSlot = myGameInventoryItem.ItemDefinition.ItemSlot;
					switch (m_activeTabState)
					{
					case TabState.Character:
						if (itemSlot == MyGameInventoryItemSlot.Grinder || itemSlot == MyGameInventoryItemSlot.Rifle || itemSlot == MyGameInventoryItemSlot.Welder || itemSlot == MyGameInventoryItemSlot.Drill)
						{
							continue;
						}
						break;
					case TabState.Tools:
						if (itemSlot == MyGameInventoryItemSlot.Helmet || itemSlot == MyGameInventoryItemSlot.Gloves || itemSlot == MyGameInventoryItemSlot.Suit || itemSlot == MyGameInventoryItemSlot.Boots)
						{
							continue;
						}
						break;
					}
				}
				GridLength[] columns = new GridLength[2]
				{
					new GridLength(1f),
					new GridLength(1f)
				};
				GridLength[] rows = new GridLength[2]
				{
					new GridLength(1f),
					new GridLength(0f)
				};
				MyGuiControlLayoutGrid myGuiControlLayoutGrid = new MyGuiControlLayoutGrid(columns, rows);
				myGuiControlLayoutGrid.Size = vector;
				myGuiControlLayoutGrid.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				MyGuiControlImageButton myGuiControlImageButton = new MyGuiControlImageButton("Button", myGuiControlLayoutGrid.Position, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, onButtonClick: hiddenButton_ButtonClicked, onButtonRightClick: hiddenButton_ButtonRightClicked, toolTip: myGameInventoryItem.ItemDefinition.Name);
				myGuiControlImageButton.Tooltips.AddToolTip(string.Empty);
				if (myGameInventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Helmet)
				{
					MyControl gameControl = MyInput.Static.GetGameControl(MyControlsSpace.HELMET);
					string toolTip = string.Format(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryToggleHelmet), gameControl.GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
					myGuiControlImageButton.Tooltips.AddToolTip(toolTip);
				}
				myGuiControlImageButton.Tooltips.AddToolTip(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryLeftClickTip));
				myGuiControlImageButton.Tooltips.AddToolTip(MyTexts.GetString(MyCommonTexts.ScreenLoadInventoryRightClickTip));
				myGuiControlImageButton.ApplyStyle(style);
				myGuiControlImageButton.Size = myGuiControlLayoutGrid.Size;
				parent.Controls.Add(myGuiControlImageButton);
				MyGuiControlImage myGuiControlImage = null;
				if (string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.BackgroundColor))
				{
					myGuiControlImage = new MyGuiControlImage(null, vector, null, null, new string[1]
					{
						"Textures\\GUI\\Controls\\grid_item_highlight.dds"
					}, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					parent.Controls.Add(myGuiControlImage);
				}
				else
				{
					Vector4 value = string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.BackgroundColor) ? Vector4.One : ColorExtensions.HexToVector4(myGameInventoryItem.ItemDefinition.BackgroundColor);
					myGuiControlImage = new MyGuiControlImage(null, new Vector2(vector.X - 0.004f, vector.Y - 0.002f), value, null, new string[1]
					{
						"Textures\\GUI\\blank.dds"
					}, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					parent.Controls.Add(myGuiControlImage);
					myGuiControlImage.Margin = new Thickness(0.0023f, 0.001f, 0f, 0f);
				}
				string[] array = new string[1]
				{
					"Textures\\GUI\\Blank.dds"
				};
				if (!string.IsNullOrEmpty(myGameInventoryItem.ItemDefinition.IconTexture))
				{
					array[0] = myGameInventoryItem.ItemDefinition.IconTexture;
				}
				MyGuiControlImage myGuiControlImage2 = new MyGuiControlImage(null, new Vector2(0.06f, 0.08f), null, null, array, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				myGuiControlImage2.Margin = new Thickness(0.005f, 0.005f, 0f, 0f);
				parent.Controls.Add(myGuiControlImage2);
				MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(null, null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				myGuiControlCheckbox.Name = m_equipCheckbox;
				myGuiControlCheckbox.ApplyStyle(style2);
				myGuiControlCheckbox.Margin = new Thickness(0.005f, 0.005f, 0.01f, 0.01f);
				myGuiControlCheckbox.IsHitTestVisible = false;
				parent.Controls.Add(myGuiControlCheckbox);
				myGuiControlCheckbox.UserData = myGameInventoryItem;
				myGuiControlImageButton.UserData = myGuiControlLayoutGrid;
				m_itemCheckboxes.Add(myGuiControlCheckbox);
				myGuiControlLayoutGrid.Add(myGuiControlImage, 0, 0);
				myGuiControlLayoutGrid.Add(myGuiControlImageButton, 0, 0);
				myGuiControlLayoutGrid.Add(myGuiControlImage2, 0, 0);
				myGuiControlLayoutGrid.Add(myGuiControlCheckbox, 1, 0);
				if (myGameInventoryItem.IsNew)
				{
					MyGuiControlImage myGuiControlImage3 = new MyGuiControlImage(null, new Vector2(0.0175f, 0.023f), null, null, new string[1]
					{
						"Textures\\GUI\\Icons\\HUD 2017\\Notification_badge.png"
					}, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					myGuiControlImage3.Margin = new Thickness(0.01f, -0.035f, 0f, 0f);
					parent.Controls.Add(myGuiControlImage3);
					myGuiControlLayoutGrid.Add(myGuiControlImage3, 1, 1);
				}
				myGuiControlWrapPanel.Add(myGuiControlLayoutGrid);
				parent.Controls.Add(myGuiControlLayoutGrid);
			}
			return myGuiControlWrapPanel;
		}

		private static List<MyGameInventoryItem> GetStoreItems(List<MyGameInventoryItem> userItems)
		{
			List<MyGameInventoryItemDefinition> list = new List<MyGameInventoryItemDefinition>(MyGameService.Definitions.Values);
			List<MyGameInventoryItem> list2 = new List<MyGameInventoryItem>();
			foreach (MyGameInventoryItemDefinition item in list)
			{
				if (item.DefinitionType == MyGameInventoryItemDefinitionType.item && item.ItemSlot != 0 && !item.Hidden && !item.IsStoreHidden && userItems.FirstOrDefault((MyGameInventoryItem i) => i.ItemDefinition.ID == item.ID) == null)
				{
					MyGameInventoryItem item2 = new MyGameInventoryItem
					{
						ID = 0uL,
						IsStoreFakeItem = true,
						ItemDefinition = item,
						Quantity = 1
					};
					list2.Add(item2);
				}
			}
			return list2.OrderBy((MyGameInventoryItem i) => i.ItemDefinition.Name).ToList();
		}

		private List<MyGameInventoryItem> GetInventoryItems()
		{
			List<MyGameInventoryItem> list = new List<MyGameInventoryItem>(MyGameService.InventoryItems);
			List<MyGameInventoryItem> list2 = null;
			if (m_activeLowTabState == LowerTabState.Coloring)
			{
				if (list.Count > 0 && m_hideDuplicatesEnabled)
				{
					list2 = new List<MyGameInventoryItem>();
					list2.AddRange(list.Where((MyGameInventoryItem i) => i.IsNew));
					list2.AddRange(list.Where((MyGameInventoryItem i) => i.IsInUse));
					foreach (MyGameInventoryItem item in list)
					{
						if (!item.IsInUse && list2.FirstOrDefault((MyGameInventoryItem i) => i.ItemDefinition.AssetModifierId == item.ItemDefinition.AssetModifierId) == null)
						{
							list2.Add(item);
						}
					}
					return (from i in list2
						orderby i.IsNew descending, i.IsInUse descending, i.ItemDefinition.Name
						select i).ToList();
				}
				return (from i in list
					orderby i.IsNew descending, i.IsInUse descending, i.ItemDefinition.Name
					select i).ToList();
			}
			if (list.Count > 0 && m_showOnlyDuplicatesEnabled)
			{
				HashSet<string> hashSet = new HashSet<string>();
				list2 = new List<MyGameInventoryItem>();
				foreach (MyGameInventoryItem item2 in list)
				{
					if (!item2.IsInUse)
					{
						if (!hashSet.Contains(item2.ItemDefinition.AssetModifierId))
						{
							hashSet.Add(item2.ItemDefinition.AssetModifierId);
						}
						else
						{
							list2.Add(item2);
						}
					}
				}
				return (from i in list2
					orderby i.IsNew descending, i.IsInUse descending, i.ItemDefinition.Name
					select i).ToList();
			}
			list2 = new List<MyGameInventoryItem>();
			list2.AddRange(list.Where((MyGameInventoryItem i) => !i.IsInUse));
			return (from i in list2
				orderby i.IsNew descending, i.IsInUse descending, i.ItemDefinition.Name
				select i).ToList();
		}

		private void hiddenButton_ButtonClicked(MyGuiControlImageButton obj)
		{
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = obj.UserData as MyGuiControlLayoutGrid;
			if (myGuiControlLayoutGrid != null)
			{
				MyGuiControlCheckbox myGuiControlCheckbox = myGuiControlLayoutGrid.GetAllControls().FirstOrDefault((MyGuiControlBase c) => c.Name.StartsWith(m_equipCheckbox)) as MyGuiControlCheckbox;
				if (myGuiControlCheckbox != null)
				{
					myGuiControlCheckbox.IsChecked = !myGuiControlCheckbox.IsChecked;
				}
			}
		}

		private void hiddenButton_ButtonRightClicked(MyGuiControlImageButton obj)
		{
			m_contextMenuLastButton = obj;
			MyGuiControlListbox.Item item = m_contextMenu.Items.FirstOrDefault((MyGuiControlListbox.Item i) => i.UserData != null && (InventoryItemAction)i.UserData == InventoryItemAction.Recycle);
			if (item != null)
			{
				MyGameInventoryItem currentItem = GetCurrentItem();
				if (currentItem != null)
				{
					string value = string.Format(MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItem).ToString(), MyGameService.GetRecyclingReward(currentItem.ItemDefinition.ItemQuality));
					item.Text = new StringBuilder(value);
				}
			}
			m_contextMenu.Activate();
			base.FocusedControl = m_contextMenu;
		}

		private void OnCategoryClicked(MyGuiControlImageButton obj)
		{
			if (obj.UserData == null)
			{
				return;
			}
			MyGameInventoryItemSlot myGameInventoryItemSlot = (MyGameInventoryItemSlot)obj.UserData;
			if (myGameInventoryItemSlot == m_filteredSlot)
			{
				if (m_activeTabState == TabState.Character)
				{
					m_filteredSlot = MyGameInventoryItemSlot.None;
				}
			}
			else
			{
				m_filteredSlot = myGameInventoryItemSlot;
			}
			m_selectedTool = string.Empty;
			RecreateControls(constructor: false);
			EquipTool();
			UpdateCheckboxes();
		}

		private void EquipTool()
		{
			if (m_filteredSlot != 0 && m_activeTabState == TabState.Tools)
			{
				long key = m_toolPicker.GetSelectedKey();
				MyPhysicalInventoryItem myPhysicalInventoryItem = m_allTools.FirstOrDefault((MyPhysicalInventoryItem t) => t.ItemId == key);
				if (myPhysicalInventoryItem.Content != null)
				{
					m_entityController.ActivateCharacterToolbarItem(new MyDefinitionId(typeof(MyObjectBuilder_PhysicalGunObject), myPhysicalInventoryItem.Content.SubtypeName));
				}
				foreach (MyGameInventoryItem userItem in m_userItems)
				{
					if (userItem.IsInUse && userItem.ItemDefinition.ItemSlot == m_filteredSlot)
					{
						m_itemCheckActive = true;
						MyGameService.GetItemCheckData(userItem, null);
						break;
					}
				}
			}
			else
			{
				m_entityController.ActivateCharacterToolbarItem(default(MyDefinitionId));
			}
		}

		private void OnItemCheckChanged(MyGuiControlCheckbox sender)
		{
			if (sender == null)
			{
				return;
			}
			MyGameInventoryItem myGameInventoryItem = sender.UserData as MyGameInventoryItem;
			if (myGameInventoryItem == null)
			{
				return;
			}
			if (sender.IsChecked)
			{
				m_itemCheckActive = true;
				MyGameService.GetItemCheckData(myGameInventoryItem, null);
				return;
			}
			m_itemCheckActive = false;
			myGameInventoryItem.IsInUse = false;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
			{
				return;
			}
			if (localCharacter != null)
			{
				MyAssetModifierComponent component;
				switch (myGameInventoryItem.ItemDefinition.ItemSlot)
				{
				case MyGameInventoryItemSlot.Face:
				case MyGameInventoryItemSlot.Helmet:
				case MyGameInventoryItemSlot.Gloves:
				case MyGameInventoryItemSlot.Boots:
				case MyGameInventoryItemSlot.Suit:
					if (localCharacter.Components.TryGet(out component))
					{
						component.ResetSlot(myGameInventoryItem.ItemDefinition.ItemSlot);
					}
					break;
				case MyGameInventoryItemSlot.Rifle:
				case MyGameInventoryItemSlot.Welder:
				case MyGameInventoryItemSlot.Grinder:
				case MyGameInventoryItemSlot.Drill:
				{
					MyEntity myEntity = localCharacter.CurrentWeapon as MyEntity;
					if (myEntity != null && myEntity.Components.TryGet(out component))
					{
						component.ResetSlot(myGameInventoryItem.ItemDefinition.ItemSlot);
					}
					break;
				}
				}
			}
			UpdateOKButton();
		}

		private void MyGameService_CheckItemDataReady(object sender, MyGameItemsEventArgs itemArgs)
		{
			if (itemArgs.NewItems != null && itemArgs.NewItems.Count != 0)
			{
				MyGameInventoryItem myGameInventoryItem = itemArgs.NewItems[0];
				UseItem(myGameInventoryItem, itemArgs.CheckData);
				foreach (MyGameInventoryItem item in new List<MyGameInventoryItem>(MyGameService.InventoryItems))
				{
					if (item != null && item.ID != myGameInventoryItem.ID && item.ItemDefinition.ItemSlot == myGameInventoryItem.ItemDefinition.ItemSlot)
					{
						item.IsInUse = false;
					}
				}
				UpdateCheckboxes();
				UpdateOKButton();
			}
		}

		private void UpdateOKButton()
		{
			bool flag = true;
			foreach (MyGameInventoryItem userItem in m_userItems)
			{
				if (userItem.IsInUse)
				{
					flag &= !userItem.IsStoreFakeItem;
				}
			}
			m_OkButton.Enabled = flag;
		}

		private void UpdateCheckboxes()
		{
			if (MySession.Static.LocalCharacter.Components.TryGet(out MyAssetModifierComponent _))
			{
				foreach (MyGuiControlCheckbox itemCheckbox in m_itemCheckboxes)
				{
					MyGameInventoryItem myGameInventoryItem = itemCheckbox.UserData as MyGameInventoryItem;
					if (myGameInventoryItem != null)
					{
						itemCheckbox.IsCheckedChanged = null;
						itemCheckbox.IsChecked = myGameInventoryItem.IsInUse;
						itemCheckbox.IsCheckedChanged = OnItemCheckChanged;
					}
				}
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenLoadInventory";
		}

		private void OpenCurrentItemInStore()
		{
			MyGameInventoryItem currentItem = GetCurrentItem();
			if (currentItem != null)
			{
				MyGuiSandbox.OpenUrlWithFallback(string.Format(MySteamConstants.URL_INVENTORY_BUY_ITEM_FORMAT, MyGameService.AppId, currentItem.ItemDefinition.ID), "Buy Item");
			}
		}

		private void OpenUserInventory()
		{
			MyGuiSandbox.OpenUrlWithFallback(string.Format(MySteamConstants.URL_INVENTORY, MyGameService.UserId, MyGameService.AppId), "User Inventory");
		}

		private void OnOpenStore(MyGuiControlButton obj)
		{
			MyGuiSandbox.OpenUrlWithFallback(string.Format(MySteamConstants.URL_INVENTORY_BROWSE_ALL_ITEMS, MyGameService.AppId), "Store");
		}

		private MyGameInventoryItem GetCurrentItem()
		{
			if (m_contextMenuLastButton == null)
			{
				return null;
			}
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = m_contextMenuLastButton.UserData as MyGuiControlLayoutGrid;
			if (myGuiControlLayoutGrid == null)
			{
				return null;
			}
			MyGuiControlCheckbox myGuiControlCheckbox = myGuiControlLayoutGrid.GetAllControls().FirstOrDefault((MyGuiControlBase c) => c.Name.StartsWith(m_equipCheckbox)) as MyGuiControlCheckbox;
			if (myGuiControlCheckbox == null)
			{
				return null;
			}
			return myGuiControlCheckbox.UserData as MyGameInventoryItem;
		}

		private void RecycleItemRequest()
		{
			if (GetCurrentItem() != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItemMessageTitle), messageText: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryRecycleItemMessageText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: OnRecycleItem));
			}
		}

		private void OnRecycleItem(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyGameInventoryItem currentItem = GetCurrentItem();
				if (currentItem != null && MyGameService.RecycleItem(currentItem))
				{
					RemoveItemFromUI(currentItem);
				}
			}
			base.State = MyGuiScreenState.OPENING;
		}

		private void DeleteItemRequest()
		{
			if (GetCurrentItem() != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryDeleteItemMessageTitle), messageText: MyTexts.Get(MyCommonTexts.ScreenLoadInventoryDeleteItemMessageText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: DeleteItemRequestMessageHandler));
			}
		}

		private void DeleteItemRequestMessageHandler(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyGameInventoryItem currentItem = GetCurrentItem();
				if (currentItem != null)
				{
					MyGameService.ConsumeItem(currentItem);
					RemoveItemFromUI(currentItem);
				}
			}
			base.State = MyGuiScreenState.OPENING;
		}

		private void RemoveItemFromUI(MyGameInventoryItem item)
		{
			MyGuiControlLayoutGrid myGuiControlLayoutGrid = m_contextMenuLastButton.UserData as MyGuiControlLayoutGrid;
			if (myGuiControlLayoutGrid != null)
			{
				foreach (MyGuiControlBase allControl in myGuiControlLayoutGrid.GetAllControls())
				{
					allControl.Visible = false;
					allControl.Enabled = false;
				}
			}
			m_contextMenuLastButton = null;
			if (MySession.Static.LocalCharacter != null && item.IsInUse && MySession.Static.LocalCharacter.Components.TryGet(out MyAssetModifierComponent component))
			{
				component.ResetSlot(item.ItemDefinition.ItemSlot);
			}
			MyLocalCache.SaveInventoryConfig(MySession.Static.LocalCharacter);
		}

		private void OnRefreshClick(MyGuiControlButton obj)
		{
			m_refreshButton.Enabled = false;
			RotatingWheelShow();
			RefreshItems();
		}

		private void UseItem(MyGameInventoryItem item, byte[] checkData)
		{
			if (MySession.Static.LocalCharacter == null)
			{
				return;
			}
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			item.IsNew = false;
			string assetModifierId = item.ItemDefinition.AssetModifierId;
			m_colorOrModelChanged = true;
			MyAssetModifierComponent component;
			switch (item.ItemDefinition.ItemSlot)
			{
			case MyGameInventoryItemSlot.Emote:
			case MyGameInventoryItemSlot.Armor:
				break;
			case MyGameInventoryItemSlot.Face:
			case MyGameInventoryItemSlot.Helmet:
			case MyGameInventoryItemSlot.Gloves:
			case MyGameInventoryItemSlot.Boots:
			case MyGameInventoryItemSlot.Suit:
				if (localCharacter.Components.TryGet(out component) && (MyFakes.OWN_ALL_ITEMS ? component.TryAddAssetModifier(assetModifierId) : component.TryAddAssetModifier(checkData)))
				{
					item.IsInUse = true;
					m_entityController.PlayRandomCharacterAnimation();
				}
				break;
			case MyGameInventoryItemSlot.Rifle:
			case MyGameInventoryItemSlot.Welder:
			case MyGameInventoryItemSlot.Grinder:
			case MyGameInventoryItemSlot.Drill:
			{
				MyEntity myEntity = localCharacter.CurrentWeapon as MyEntity;
				if (myEntity != null && myEntity.Components.TryGet(out component) && (MyFakes.OWN_ALL_ITEMS ? component.TryAddAssetModifier(assetModifierId) : component.TryAddAssetModifier(checkData)))
				{
					item.IsInUse = true;
				}
				break;
			}
			}
		}

		public override bool Update(bool hasFocus)
		{
			if (base.State != MyGuiScreenState.CLOSING && base.State != MyGuiScreenState.CLOSED)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator);
			}
			if (MyInput.Static.IsNewPrimaryButtonPressed() && m_contextMenu.IsActiveControl && !m_contextMenu.IsMouseOver)
			{
				m_contextMenu.Deactivate();
			}
			base.Update(hasFocus);
			if (!m_audioSet && MySandboxGame.IsGameReady && MyAudio.Static != null && MyRenderProxy.VisibleObjectsRead != null && MyRenderProxy.VisibleObjectsRead.Count > 0)
			{
				SetAudioVolumes();
				m_audioSet = true;
			}
			return true;
		}

		private static void SetAudioVolumes()
		{
			MyAudio.Static.StopMusic();
			MyAudio.Static.ChangeGlobalVolume(1f, 5f);
			if (MyPerGameSettings.UseMusicController && MyFakes.ENABLE_MUSIC_CONTROLLER && MySandboxGame.Config.EnableDynamicMusic && !Sandbox.Engine.Platform.Game.IsDedicated && MyMusicController.Static == null)
			{
				MyMusicController.Static = new MyMusicController(MyAudio.Static.GetAllMusicCues());
			}
			MyAudio.Static.MusicAllowed = (MyMusicController.Static == null);
			if (MyMusicController.Static != null)
			{
				MyMusicController.Static.Active = true;
			}
			else
			{
				MyAudio.Static.PlayMusic(new MyMusicTrack
				{
					TransitionCategory = MyStringId.GetOrCompute("Default")
				});
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (m_entityController != null)
			{
				bool isMouseOverAnyControl = IsMouseOver() || m_lastHandlingControl != null;
				m_entityController.Update(isMouseOverAnyControl);
			}
		}

		public override bool Draw()
		{
			bool result = base.Draw();
			DrawScene();
			return result;
		}

		private void DrawScene()
		{
			if (MySession.Static == null)
			{
				return;
			}
			if ((MySession.Static.CameraController == null || !MySession.Static.CameraController.IsInFirstPersonView) && MyThirdPersonSpectator.Static != null)
			{
				MyThirdPersonSpectator.Static.Update();
			}
			if (MySector.MainCamera != null)
			{
				MySession.Static.CameraController.ControlCamera(MySector.MainCamera);
				MySector.MainCamera.Update(0.0166666675f);
				MySector.MainCamera.UploadViewMatrixToRender();
			}
			MySector.UpdateSunLight();
			MyRenderProxy.UpdateGameplayFrame(MySession.Static.GameplayFrameCounter);
			MyRenderFogSettings myRenderFogSettings = default(MyRenderFogSettings);
			myRenderFogSettings.FogMultiplier = MySector.FogProperties.FogMultiplier;
			myRenderFogSettings.FogColor = MySector.FogProperties.FogColor;
			myRenderFogSettings.FogDensity = MySector.FogProperties.FogDensity / 100f;
			MyRenderFogSettings settings = myRenderFogSettings;
			MyRenderProxy.UpdateFogSettings(ref settings);
			MyRenderPlanetSettings myRenderPlanetSettings = default(MyRenderPlanetSettings);
			myRenderPlanetSettings.AtmosphereIntensityMultiplier = MySector.PlanetProperties.AtmosphereIntensityMultiplier;
			myRenderPlanetSettings.AtmosphereIntensityAmbientMultiplier = MySector.PlanetProperties.AtmosphereIntensityAmbientMultiplier;
			myRenderPlanetSettings.AtmosphereDesaturationFactorForward = MySector.PlanetProperties.AtmosphereDesaturationFactorForward;
			myRenderPlanetSettings.CloudsIntensityMultiplier = MySector.PlanetProperties.CloudsIntensityMultiplier;
			MyRenderPlanetSettings settings2 = myRenderPlanetSettings;
			MyRenderProxy.UpdatePlanetSettings(ref settings2);
			MyRenderProxy.UpdateSSAOSettings(ref MySector.SSAOSettings);
			MyRenderProxy.UpdateHBAOSettings(ref MySector.HBAOSettings);
			MyEnvironmentData data = MySector.SunProperties.EnvironmentData;
			data.Skybox = ((!string.IsNullOrEmpty(MySession.Static.CustomSkybox)) ? MySession.Static.CustomSkybox : MySector.EnvironmentDefinition.EnvironmentTexture);
			data.SkyboxOrientation = MySector.EnvironmentDefinition.EnvironmentOrientation.ToQuaternion();
			data.EnvironmentLight.SunLightDirection = -MySector.SunProperties.SunDirectionNormalized;
			MyRenderProxy.UpdateRenderEnvironment(ref data, MySector.ResetEyeAdaptation);
			MySector.ResetEyeAdaptation = false;
			MyRenderProxy.UpdateEnvironmentMap();
			if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled != MyPostprocessSettingsWrapper.AllEnabled || MyPostprocessSettingsWrapper.IsDirty)
			{
				if (MyVideoSettingsManager.CurrentGraphicsSettings.PostProcessingEnabled)
				{
					MyPostprocessSettingsWrapper.SetWardrobePostProcessing();
				}
				else
				{
					MyPostprocessSettingsWrapper.ReducePostProcessing();
				}
			}
			MyRenderProxy.SwitchPostprocessSettings(ref MyPostprocessSettingsWrapper.Settings);
			if (MyRenderProxy.SettingsDirty)
			{
				MyRenderProxy.SwitchRenderSettings(MyRenderProxy.Settings);
			}
			MyRenderProxy.Draw3DScene();
			using (Stats.Generic.Measure("GamePrepareDraw"))
			{
				if (MySession.Static != null)
				{
					MySession.Static.Draw();
				}
			}
		}

		protected override void Canceling()
		{
			Cancel();
			base.Canceling();
		}

		protected override void OnHide()
		{
			base.OnHide();
			DrawScene();
		}

		protected override void OnClosed()
		{
			if (MyGameService.IsActive)
			{
				MyGameService.InventoryRefreshed -= MySteamInventory_Refreshed;
			}
			MyGuiControlSlider sliderHue = m_sliderHue;
			sliderHue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderHue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderSaturation = m_sliderSaturation;
			sliderSaturation.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderSaturation.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderValue = m_sliderValue;
			sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGameService.CheckItemDataReady -= MyGameService_CheckItemDataReady;
			MyRenderProxy.UnloadTexture(m_rotatingWheelTexture);
			base.OnClosed();
			MyAnalyticsHelper.ReportActivityEnd(null, "show_inventory");
			if (!m_inGame)
			{
				MyVRage.Platform.Ansel.IsSessionEnabled = false;
			}
			else if (m_savedStateAnselEnabled.HasValue)
			{
				MyVRage.Platform.Ansel.IsSessionEnabled = m_savedStateAnselEnabled.Value;
				m_savedStateAnselEnabled = null;
			}
		}

		protected override void OnShow()
		{
			m_savedStateAnselEnabled = MyVRage.Platform.Ansel.IsSessionEnabled;
			MyVRage.Platform.Ansel.IsSessionEnabled = false;
			if (MySector.MainCamera != null && !m_inGame)
			{
				MySector.MainCamera.FieldOfViewDegrees = 55f;
			}
			if (MyGameService.IsActive)
			{
				MyGameService.InventoryRefreshed += MySteamInventory_Refreshed;
			}
			base.OnShow();
			MyAnalyticsHelper.ReportActivityStart(null, "show_inventory", string.Empty, "gui", string.Empty);
		}

		public override bool CloseScreen()
		{
			MyScreenManager.GetFirstScreenOfType<MyGuiScreenIntroVideo>()?.UnhideScreen();
			return base.CloseScreen();
		}

		private void RefreshItems()
		{
			if (MyGameService.IsActive)
			{
				MyGameService.GetAllInventoryItems();
			}
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, MyStringId toolTip, Action<MyGuiControlButton> onClick)
		{
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(text), toolTip: MyTexts.GetString(toolTip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: onClick)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			};
		}

		private MyGuiControlImageButton MakeImageButton(Vector2 position, Vector2 size, MyGuiControlImageButton.StyleDefinition style, MyStringId toolTip, Action<MyGuiControlImageButton> onClick)
		{
			MyGuiControlImageButton myGuiControlImageButton = new MyGuiControlImageButton("Button", position, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyTexts.GetString(toolTip), null, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			if (style != null)
			{
				myGuiControlImageButton.ApplyStyle(style);
			}
			myGuiControlImageButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlImageButton.Size = size;
			return myGuiControlImageButton;
		}

		private void MySteamInventory_Refreshed(object sender, EventArgs e)
		{
			if (m_itemCheckActive)
			{
				m_itemCheckActive = false;
			}
			else
			{
				RefreshUI();
			}
		}

		private void RefreshUI()
		{
			RecreateControls(constructor: false);
			EquipTool();
			UpdateCheckboxes();
		}

		private void RotatingWheelShow()
		{
			m_wheel.ManualRotationUpdate = true;
			m_wheel.Visible = true;
		}

		private void RotatingWheelHide()
		{
			m_wheel.ManualRotationUpdate = false;
			m_wheel.Visible = false;
		}
	}
}
