using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyGuiScreenColorPicker : MyGuiScreenBase
	{
		private class MyGameInventoryItemDefinitionComparer : IEqualityComparer<MyGameInventoryItem>
		{
			public static MyGameInventoryItemDefinitionComparer Comparer = new MyGameInventoryItemDefinitionComparer();

			public bool Equals(MyGameInventoryItem x, MyGameInventoryItem y)
			{
				if (x == null && y == null)
				{
					return true;
				}
				if (x == null && y != null)
				{
					return false;
				}
				if (x != null && y == null)
				{
					return false;
				}
				return GetHashCode(x) == GetHashCode(y);
			}

			public int GetHashCode(MyGameInventoryItem obj)
			{
				return obj.ItemDefinition.ID;
			}
		}

		private static readonly Vector2 SCREEN_SIZE = new Vector2(0.37f, 1.2f);

		private static readonly float HIDDEN_PART_RIGHT = 0.04f;

		private float m_textScale = 0.8f;

		private Vector2 m_controlPadding = new Vector2(0.02f, 0.02f);

		private MyGuiControlButton m_closeButton;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_defaultsButton;

		private MyGuiControlSlider m_hueSlider;

		private MyGuiControlSlider m_saturationSlider;

		private MyGuiControlSlider m_valueSlider;

		private MyGuiControlTextbox m_hueTextbox;

		private MyGuiControlTextbox m_saturationTextbox;

		private MyGuiControlTextbox m_valueTextbox;

		private MyGuiControlTextbox m_hexTextbox;

		private MyGuiControlPanel m_colorVariantPanel;

		private MyGuiControlPanel m_highlightControlPanel;

		private List<MyGuiControlPanel> m_colorPaletteControlsList = new List<MyGuiControlPanel>();

		private List<MyGameInventoryItemDefinition> m_userItems;

		private MyGuiControlGrid m_itemGrid;

		private List<Vector3> m_oldPaletteList;

		private MyStringHash m_oldArmorSkin;

		private int m_oldColorSlot;

		private bool m_oldApplyColor;

		private bool m_oldApplySkin;

		private MyGuiStyleDefinition m_armorGridStyle = new MyGuiStyleDefinition
		{
			BackgroundTexture = null,
			BackgroundPaddingSize = Vector2.Zero,
			ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM_WHITE,
			ItemFontNormal = "Blue",
			ItemFontHighlight = "White",
			ItemPadding = new MyGuiBorderThickness(4f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
			ItemMargin = new MyGuiBorderThickness(4f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y),
			FitSizeToItems = true
		};

		private static readonly Regex HEX_REGEX = new Regex("^(#{0,1})([0-9A-Fa-f]{6})$");

		private readonly StringBuilder m_hexSb = new StringBuilder();

		private MySessionComponentGameInventory m_gameInventoryComp;

		private MySessionComponentDLC m_dlcComp;

		private MyGuiControlCheckbox m_applyColorCheckbox;

		private MyGuiControlCheckbox m_applySkinCheckbox;

		private const int x = -170;

		private const int y = -250;

		private const int defColLine = -230;

		private const int defColCol = -42;

		private const string m_hueScaleTexture = "Textures\\GUI\\HueScale.png";

		public static bool ApplyColor
		{
			get;
			private set;
		} = true;


		public static bool ApplySkin
		{
			get;
			private set;
		} = true;


		public MyGuiScreenColorPicker()
			: base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - SCREEN_SIZE.X * 0.5f + HIDDEN_PART_RIGHT, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR * MySandboxGame.Config.UIBkOpacity, SCREEN_SIZE)
		{
			base.CanHideOthers = false;
			m_dlcComp = MySession.Static?.GetComponent<MySessionComponentDLC>();
			m_gameInventoryComp = MySession.Static?.GetComponent<MySessionComponentGameInventory>();
			RecreateControls(constructor: true);
			m_oldPaletteList = new List<Vector3>();
			foreach (Vector3 buildColorSlot in MySession.Static.LocalHumanPlayer.BuildColorSlots)
			{
				m_oldPaletteList.Add(buildColorSlot);
			}
			m_oldArmorSkin = MyStringHash.GetOrCompute(MySession.Static.LocalHumanPlayer.BuildArmorSkin);
			m_oldColorSlot = MySession.Static.LocalHumanPlayer.SelectedBuildColorSlot;
			m_oldApplySkin = ApplySkin;
			m_oldApplyColor = ApplyColor;
			UpdateSliders(MyPlayer.SelectedColor);
			UpdateLabels();
			if (MyGuiScreenHudSpace.Static != null)
			{
				MyGuiScreenHudSpace.Static.HideScreen();
			}
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			float num = (SCREEN_SIZE.Y - 1f) / 2f;
			AddCaption(MyTexts.Get(MyCommonTexts.ColorPicker).ToString(), Color.White.ToVector4(), m_controlPadding + new Vector2(0f - HIDDEN_PART_RIGHT, num - 0.03f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.44f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.316f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, 0.055f), m_size.Value.X * 0.73f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, -0.362f), m_size.Value.X * 0.73f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlLabel control = new MyGuiControlLabel(new Vector2((0f - m_size.Value.X) * 0.83f / 2f, -5f / 12f), null, MyTexts.Get(MyCommonTexts.ApplyColor).ToString());
			Controls.Add(control);
			m_applyColorCheckbox = new MyGuiControlCheckbox(new Vector2((0f - m_size.Value.X) * 0.83f / 2f + m_size.Value.X * 0.73f, -13f / 30f), null, null, ApplyColor, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				IsCheckedChanged = OnColorCheckedChanged
			};
			m_applyColorCheckbox.SetTooltip(MyTexts.Get(MyCommonTexts.ApplyColorTooltip).ToString());
			Controls.Add(m_applyColorCheckbox);
			Color white = Color.White;
			int num2 = 0;
			m_highlightControlPanel = new MyGuiControlPanel(size: new Vector2(0.04f, 0.035f), position: new Vector2(-0.1325f + (float)(MyPlayer.SelectedColorSlot % 7) * 0.038f, -0.375f + (float)(MyPlayer.SelectedColorSlot / 7) * 0.035f));
			m_highlightControlPanel.ColorMask = white.ToVector4();
			m_highlightControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
			Controls.Add(m_highlightControlPanel);
			int num3 = 0;
			while (num3 < 14)
			{
				MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(size: new Vector2(0.034f, 0.03f), position: new Vector2(-0.1325f + (float)(num3 % 7) * 0.038f, -0.375f + (float)num2 * 0.035f));
				myGuiControlPanel.ColorMask = prev(MyPlayer.ColorSlots.ItemAt(num3)).HSVtoColor().ToVector4();
				myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_GUI_BLANK;
				myGuiControlPanel.CanHaveFocus = true;
				myGuiControlPanel.BorderHighlightEnabled = true;
				myGuiControlPanel.BorderMargin = new Vector2(0.034f, 0.031f);
				myGuiControlPanel.BorderSize = 2;
				myGuiControlPanel.HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
				m_colorPaletteControlsList.Add(myGuiControlPanel);
				Controls.Add(myGuiControlPanel);
				num3++;
				if (num3 % 7 == 0)
				{
					num2++;
				}
			}
			float num4 = -49f / 320f;
			float num5 = -109f / 800f;
			float num6 = 91f / 800f;
			Controls.Add(new MyGuiControlLabel(new Vector2(num4, -0.275f), null, "H:"));
			Controls.Add(new MyGuiControlLabel(new Vector2(num6, -0.275f), null, "°", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER));
			m_hueTextbox = new MyGuiControlTextbox
			{
				Position = new Vector2(num6 - 0.0125f, -167f / 600f),
				Size = new Vector2(0.053f, 11f / 240f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				Type = MyGuiControlTextboxType.DigitsOnly,
				TruncateDecimalDigits = false
			};
			m_hueSlider = new MyGuiControlSlider(new Vector2(num5, -0.275f), 0f, 360f, 0.18f, null, null, string.Empty, 0, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Hue, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			MyGuiControlSlider hueSlider = m_hueSlider;
			hueSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(hueSlider.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			Controls.Add(m_hueSlider);
			m_hueTextbox.MaxLength = 5;
			m_hueTextbox.EnterPressed += HueTextboxOnEnterPressed;
			m_hueTextbox.FocusChanged += HueTextboxOnFocusChanged;
			Controls.Add(m_hueTextbox);
			Controls.Add(new MyGuiControlLabel(new Vector2(num4, -13f / 60f), null, "S:"));
			Controls.Add(new MyGuiControlLabel(new Vector2(num6, -13f / 60f), null, "%", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER));
			m_saturationTextbox = new MyGuiControlTextbox
			{
				Position = new Vector2(num6 - 0.0125f, -0.22f),
				Size = new Vector2(0.053f, 0.232f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				Type = MyGuiControlTextboxType.DigitsOnly,
				TruncateDecimalDigits = false
			};
			m_saturationSlider = new MyGuiControlSlider(new Vector2(num5, -13f / 60f), 0f, 1f, 0.18f, 0f, null, string.Empty, 1, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			MyGuiControlSlider saturationSlider = m_saturationSlider;
			saturationSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(saturationSlider.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			Controls.Add(m_saturationSlider);
			m_saturationTextbox.MaxLength = 5;
			m_saturationTextbox.EnterPressed += SaturationTextboxOnEnterPressed;
			m_saturationTextbox.FocusChanged += SaturationTextboxOnFocusChanged;
			Controls.Add(m_saturationTextbox);
			Controls.Add(new MyGuiControlLabel(new Vector2(num4, -19f / 120f), null, "V:"));
			Controls.Add(new MyGuiControlLabel(new Vector2(num6, -19f / 120f), null, "%", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER));
			m_valueTextbox = new MyGuiControlTextbox
			{
				Position = new Vector2(num6 - 0.0125f, -97f / 600f),
				Size = new Vector2(0.053f, 0.232f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER,
				Type = MyGuiControlTextboxType.DigitsOnly,
				TruncateDecimalDigits = false
			};
			m_valueSlider = new MyGuiControlSlider(new Vector2(num5, -19f / 120f), 0f, 1f, 0.18f, 0f, null, string.Empty, 1, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			MyGuiControlSlider valueSlider = m_valueSlider;
			valueSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(valueSlider.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			Controls.Add(m_valueSlider);
			m_valueTextbox.MaxLength = 5;
			m_valueTextbox.EnterPressed += ValueTextboxOnEnterPressed;
			m_valueTextbox.FocusChanged += ValueTextboxOnFocusChanged;
			Controls.Add(m_valueTextbox);
			Controls.Add(new MyGuiControlLabel(new Vector2(num4, -0.1f), null, "Hex:"));
			m_hexTextbox = new MyGuiControlTextbox
			{
				Position = new Vector2(num6 - 0.0125f, -0.1f),
				Size = new Vector2(0.222f, 0.232f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER
			};
			m_hexTextbox.MaxLength = 7;
			Controls.Add(m_hexTextbox);
			m_hexSb.Clear();
			m_hexTextbox.SetText(m_hexSb);
			m_hexTextbox.EnterPressed += HexTextboxOnEnterPressed;
			m_hexTextbox.FocusChanged += HexTextboxOnFocusChanged;
			m_userItems = GetInventoryItems();
			m_itemGrid = new MyGuiControlGrid
			{
				ColumnsCount = 5,
				RowsCount = 5
			};
			m_itemGrid.SetCustomStyleDefinition(m_armorGridStyle);
			m_itemGrid.ItemClicked += ItemGridOnItemClicked;
			m_itemGrid.ItemAccepted += ItemGridOnItemClicked;
			MyGuiControlLabel control2 = new MyGuiControlLabel(new Vector2(-0.15f, -7f / 240f), null, MyTexts.Get(MyCommonTexts.ApplySkin).ToString());
			Controls.Add(control2);
			m_applySkinCheckbox = new MyGuiControlCheckbox(new Vector2(0.125f, -29f / 600f), null, null, ApplySkin, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				IsCheckedChanged = OnSkinCheckedChanged
			};
			m_applySkinCheckbox.SetTooltip(MyTexts.Get(MyCommonTexts.ApplySkinTooltip).ToString());
			Controls.Add(m_applySkinCheckbox);
			MyGuiControlScrollablePanel myGuiControlScrollablePanel = new MyGuiControlScrollablePanel(m_itemGrid);
			myGuiControlScrollablePanel.Position = new Vector2(-0.15f, -0.006666667f);
			myGuiControlScrollablePanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			myGuiControlScrollablePanel.DrawScrollBarSeparator = true;
			myGuiControlScrollablePanel.FitSizeToScrolledControl();
			Controls.Add(myGuiControlScrollablePanel);
			m_itemGrid.Add(new MyGuiGridItem("Textures\\Sprites\\Cross.dds", null, "None", string.Empty));
			int value = 0;
			int num7 = 0;
			for (int i = 0; i < m_userItems.Count; i++)
			{
				MyGameInventoryItemDefinition myGameInventoryItemDefinition = m_userItems[i];
				MyAssetModifierDefinition assetModifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), myGameInventoryItemDefinition.AssetModifierId));
				if (assetModifierDefinition != null)
				{
					MyGuiGridItem item = CreateArmorGridItem(myGameInventoryItemDefinition, assetModifierDefinition);
					m_itemGrid.Add(item);
					num7++;
					if (myGameInventoryItemDefinition.AssetModifierId == MyPlayer.SelectedArmorSkin)
					{
						value = num7;
					}
				}
			}
			m_itemGrid.SelectedIndex = value;
			Vector2 value2 = new Vector2(-0.083f, 0.36f);
			Vector2 value3 = new Vector2(0.134f, 0.038f);
			float num8 = 0.265f;
			m_defaultsButton = CreateButton(num8, MyTexts.Get(MyCommonTexts.Defaults), OnDefaultsClick, enabled: true, textScale: m_textScale, tooltip: MySpaceTexts.ToolTipOptionsControls_Defaults);
			m_defaultsButton.Position = value2 + new Vector2(0f, 1f) * value3;
			m_defaultsButton.PositionX += value3.X / 2f;
			Controls.Add(m_defaultsButton);
			m_okButton = CreateButton(num8, MyTexts.Get(MyCommonTexts.Ok), OnOkClick, enabled: true, textScale: m_textScale, tooltip: MySpaceTexts.ToolTipOptionsSpace_Ok);
			m_okButton.Position = value2 + new Vector2(value3.X / 2f, value3.Y * 2f);
			m_okButton.ShowTooltipWhenDisabled = true;
			Controls.Add(m_okButton);
		}

		private void HueTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
		{
			MyGuiControlTextbox obj2;
			if (!state && (obj2 = (obj as MyGuiControlTextbox)) != null)
			{
				HueTextboxOnEnterPressed(obj2);
			}
		}

		private MyGuiGridItem CreateArmorGridItem(MyGameInventoryItemDefinition def, MyAssetModifierDefinition definition)
		{
			StringBuilder stringBuilder = new StringBuilder(def.Name);
			string icon = def.IconTexture ?? MyGuiConstants.TEXTURE_ICON_FAKE.Texture;
			if (!definition.Icons.IsNullOrEmpty())
			{
				icon = definition.Icons[0];
			}
			bool flag = m_gameInventoryComp != null && m_gameInventoryComp.HasArmor(MyStringHash.GetOrCompute(def.AssetModifierId), Sync.MyId);
			string subIcon = null;
			if (!definition.DLCs.IsNullOrEmpty())
			{
				MyDLCs.MyDLC firstMissingDefinitionDLC = m_dlcComp.GetFirstMissingDefinitionDLC(definition, Sync.MyId);
				MyDLCs.MyDLC dlc;
				if (firstMissingDefinitionDLC != null)
				{
					flag = false;
					subIcon = firstMissingDefinitionDLC.Icon;
					stringBuilder.Append("\n");
					for (int i = 0; i < definition.DLCs.Length; i++)
					{
						stringBuilder.Append("\n");
						stringBuilder.Append(MyDLCs.GetRequiredDLCTooltip(definition.DLCs[i]));
					}
				}
				else if (MyDLCs.TryGetDLC(definition.DLCs[0], out dlc))
				{
					subIcon = dlc.Icon;
				}
			}
			else if (!flag)
			{
				subIcon = MyGuiConstants.TEXTURE_ICON_FAKE.Texture;
				stringBuilder.Append("\n");
				stringBuilder.Append("\n");
				stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.RequiresGameInventoryItem), def.Name);
			}
			Vector4 one = Vector4.One;
			if (!flag)
			{
				one.W = 0.5f;
			}
			return new MyGuiGridItem(icon, null, stringBuilder.ToString(), def.AssetModifierId, flag, 0.85f)
			{
				SubIcon2 = subIcon,
				MainIconColorMask = one
			};
		}

		private void HueTextboxOnEnterPressed(MyGuiControlTextbox obj)
		{
			if (float.TryParse(obj.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
			{
				m_hueSlider.Value = result;
			}
			UpdateLabels();
		}

		private void SaturationTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
		{
			MyGuiControlTextbox obj2;
			if (!state && (obj2 = (obj as MyGuiControlTextbox)) != null)
			{
				SaturationTextboxOnEnterPressed(obj2);
			}
		}

		private void SaturationTextboxOnEnterPressed(MyGuiControlTextbox obj)
		{
			if (float.TryParse(obj.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
			{
				m_saturationSlider.Value = result * 0.01f;
			}
			UpdateLabels();
		}

		private void ValueTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
		{
			MyGuiControlTextbox obj2;
			if (!state && (obj2 = (obj as MyGuiControlTextbox)) != null)
			{
				ValueTextboxOnEnterPressed(obj2);
			}
		}

		private void ValueTextboxOnEnterPressed(MyGuiControlTextbox obj)
		{
			if (float.TryParse(obj.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
			{
				m_valueSlider.Value = result * 0.01f;
			}
			UpdateLabels();
		}

		private void HexTextboxOnFocusChanged(MyGuiControlBase obj, bool state)
		{
			MyGuiControlTextbox obj2;
			if (!state && (obj2 = (obj as MyGuiControlTextbox)) != null)
			{
				HexTextboxOnEnterPressed(obj2);
			}
		}

		private void HexTextboxOnEnterPressed(MyGuiControlTextbox obj)
		{
			if (MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			m_hexSb.Clear();
			obj.GetText(m_hexSb);
			Match match = HEX_REGEX.Match(m_hexSb.ToString());
			if (!match.Success || match.Length == 0)
			{
				Color currentColor = GetCurrentColor();
				m_hexSb.Clear();
				m_hexSb.AppendFormat("#{0:X2}{1:X2}{2:X2}", currentColor.R, currentColor.G, currentColor.B);
				m_hexTextbox.SetText(m_hexSb);
				return;
			}
			string text = match.Value;
			if (text.Length > 6)
			{
				text = text.Substring(1);
			}
			byte r = byte.Parse(text.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(text.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(text.Substring(4, 2), NumberStyles.HexNumber);
			Vector3 vector = new Color(r, g, b).ColorToHSV();
			m_hueSlider.Value = vector.X * 360f;
			m_saturationSlider.Value = vector.Y;
			m_valueSlider.Value = vector.Z;
			UpdateLabels();
		}

		private void OnColorCheckedChanged(MyGuiControlCheckbox obj)
		{
			ApplyColor = obj.IsChecked;
		}

		private void OnSkinCheckedChanged(MyGuiControlCheckbox obj)
		{
			ApplySkin = obj.IsChecked;
		}

		private void ItemGridOnItemClicked(MyGuiControlGrid grid, MyGuiControlGrid.EventArgs eventArgs)
		{
			MyGuiGridItem itemAt = grid.GetItemAt(eventArgs.RowIndex, eventArgs.ColumnIndex);
			if (itemAt == null || MySession.Static == null || MySession.Static.LocalHumanPlayer == null)
			{
				return;
			}
			MyStringHash orCompute = MyStringHash.GetOrCompute((string)itemAt.UserData);
			if (orCompute != MyStringHash.NullOrEmpty)
			{
				MyAssetModifierDefinition assetModifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), orCompute));
				if (!assetModifierDefinition.DLCs.IsNullOrEmpty())
				{
					ShowDLCStorePage(assetModifierDefinition);
				}
				if (m_gameInventoryComp == null || !m_gameInventoryComp.HasArmor(orCompute, Sync.MyId))
				{
					return;
				}
			}
			MySession.Static.LocalHumanPlayer.BuildArmorSkin = (string)itemAt.UserData;
		}

		private MyGuiControlButton CreateButton(float usableWidth, StringBuilder text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, float textScale = 1f)
		{
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Rectangular, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, text, textScale, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, onClick);
			myGuiControlButton.Size = new Vector2(usableWidth, 0.034f);
			myGuiControlButton.Position += new Vector2(-0.02f, 0f);
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			return myGuiControlButton;
		}

		protected override void OnShow()
		{
			base.OnShow();
			OnSetVisible(visible: true);
		}

		protected override void OnClosed()
		{
			base.OnClosed();
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
			OnSetVisible(visible: false);
		}

		protected override void OnHide()
		{
			base.OnHide();
			OnSetVisible(visible: false);
		}

		private void OnSetVisible(bool visible)
		{
			if (MyCubeBuilder.Static != null)
			{
				MyCubeBuilder.Static.UseTransparency = !visible;
			}
		}

		private void UpdateLabels()
		{
			m_hexSb.Clear();
			m_hexSb.Append($"{m_hueSlider.Value:F1}");
			m_hueTextbox.SetText(m_hexSb);
			m_hexSb.Clear();
			m_hexSb.Append($"{m_saturationSlider.Value * 100f:F1}");
			m_saturationTextbox.SetText(m_hexSb);
			m_hexSb.Clear();
			m_hexSb.Append($"{m_valueSlider.Value * 100f:F1}");
			m_valueTextbox.SetText(m_hexSb);
		}

		private void UpdateSliders(Vector3 colorValue)
		{
			m_hueSlider.Value = colorValue.X * 360f;
			m_saturationSlider.Value = MathHelper.Clamp(colorValue.Y + MyColorPickerConstants.SATURATION_DELTA, 0f, 1f);
			m_valueSlider.Value = MathHelper.Clamp(colorValue.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0f, 1f);
		}

		private Vector3 prev(Vector3 HSV)
		{
			return new Vector3(HSV.X, MathHelper.Clamp(HSV.Y + MyColorPickerConstants.SATURATION_DELTA, 0f, 1f), MathHelper.Clamp(HSV.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0f, 1f));
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.LANDING_GEAR))
			{
				CloseScreenNow();
			}
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			if (localHumanPlayer != null && (MyInput.Static.IsNewLeftMousePressed() || MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.ACCEPT)))
			{
				for (int i = 0; i < m_colorPaletteControlsList.Count; i++)
				{
					if (m_colorPaletteControlsList[i].IsMouseOver)
					{
						base.FocusedControl = m_colorPaletteControlsList[i];
						break;
					}
				}
				for (int j = 0; j < m_colorPaletteControlsList.Count; j++)
				{
					if (m_colorPaletteControlsList[j].HasFocus)
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
						localHumanPlayer.SelectedBuildColorSlot = j;
						m_highlightControlPanel.Position = new Vector2(-0.1325f + (float)(localHumanPlayer.SelectedBuildColorSlot % 7) * 0.038f, -0.375f + (float)(localHumanPlayer.SelectedBuildColorSlot / 7) * 0.035f);
						UpdateSliders(localHumanPlayer.SelectedBuildColor);
					}
				}
			}
			base.HandleInput(receivedFocusInThisUpdate);
		}

		private void OnValueChange(MyGuiControlSlider sender)
		{
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			if (localHumanPlayer != null)
			{
				UpdateLabels();
				float value = m_saturationSlider.Value;
				float value2 = m_valueSlider.Value;
				Color currentColor = GetCurrentColor();
				m_colorPaletteControlsList[localHumanPlayer.SelectedBuildColorSlot].ColorMask = currentColor.ToVector4();
				float num = value - MyColorPickerConstants.SATURATION_DELTA;
				float z = value2 - MyColorPickerConstants.VALUE_DELTA + MyColorPickerConstants.VALUE_COLORIZE_DELTA;
				localHumanPlayer.SelectedBuildColor = new Vector3(m_hueSlider.Value / 360f, num, z);
				m_hexSb.Clear();
				m_hexSb.AppendFormat("#{0:X2}{1:X2}{2:X2}", currentColor.R, currentColor.G, currentColor.B);
				m_hexTextbox.SetText(m_hexSb);
			}
		}

		private Color GetCurrentColor()
		{
			return new Vector3(m_hueSlider.Value / 360f, m_saturationSlider.Value, m_valueSlider.Value).HSVtoColor();
		}

		private void OnDefaultsClick(MyGuiControlButton sender)
		{
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			if (localHumanPlayer != null)
			{
				localHumanPlayer.SetDefaultColors();
				_ = Color.White;
				for (int i = 0; i < 14; i++)
				{
					m_colorPaletteControlsList[i].ColorMask = prev(localHumanPlayer.BuildColorSlots[i]).HSVtoColor().ToVector4();
				}
				localHumanPlayer.SelectedBuildColorSlot = 0;
				m_highlightControlPanel.Position = new Vector2(-0.1325f + (float)(localHumanPlayer.SelectedBuildColorSlot % 7) * 0.038f, -0.375f + (float)(localHumanPlayer.SelectedBuildColorSlot / 7) * 0.035f);
				UpdateSliders(localHumanPlayer.SelectedBuildColor);
				localHumanPlayer.BuildArmorSkin = string.Empty;
				m_itemGrid.SelectedIndex = 0;
				m_applyColorCheckbox.IsChecked = true;
				m_applySkinCheckbox.IsChecked = true;
			}
		}

		public override bool CloseScreen()
		{
			OnOkClick(m_okButton);
			return base.CloseScreen();
		}

		private void OnOkClick(MyGuiControlButton sender)
		{
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			if (localHumanPlayer != null)
			{
				bool flag = false;
				int num = 0;
				foreach (Vector3 buildColorSlot in localHumanPlayer.BuildColorSlots)
				{
					if (m_oldPaletteList[num] != buildColorSlot)
					{
						flag = true;
						m_oldPaletteList[num] = buildColorSlot;
					}
					num++;
				}
				if (flag)
				{
					Sync.Players.RequestPlayerColorsChanged(localHumanPlayer.Id.SerialId, m_oldPaletteList);
				}
			}
			MyCubeBuilder.Static.ColorPickerOk();
			MyGuiGridItem selectedItem = m_itemGrid.SelectedItem;
			if (ApplySkin && selectedItem != null && MySession.Static != null && MySession.Static.LocalHumanPlayer != null)
			{
				MyStringHash orCompute = MyStringHash.GetOrCompute((string)selectedItem.UserData);
				if (orCompute != MyStringHash.NullOrEmpty && (m_gameInventoryComp == null || !m_gameInventoryComp.HasArmor(orCompute, Sync.MyId)))
				{
					CloseScreenNow();
					return;
				}
				if (m_gameInventoryComp != null)
				{
					m_gameInventoryComp.HasArmor(orCompute, Sync.MyId);
				}
				else
					_ = 0;
				MyAssetModifierDefinition assetModifierDefinition = MyDefinitionManager.Static.GetAssetModifierDefinition(new MyDefinitionId(typeof(MyObjectBuilder_AssetModifierDefinition), orCompute));
				if (assetModifierDefinition == null)
				{
					CloseScreenNow();
					return;
				}
				if (!assetModifierDefinition.DLCs.IsNullOrEmpty())
				{
					ShowDLCStorePage(assetModifierDefinition);
				}
			}
			CloseScreenNow();
		}

		private void ShowDLCStorePage(MyAssetModifierDefinition assetModDef)
		{
			MyDLCs.MyDLC missing = m_dlcComp.GetFirstMissingDefinitionDLC(assetModDef, Sync.MyId);
			if (missing != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MySpaceTexts.SkinNotOwned), MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
				{
					if (result == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						MyGameService.OpenOverlayUrl(missing.URL);
					}
				}));
			}
		}

		private void OnCancelClick(MyGuiControlButton sender)
		{
			MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
			if (localHumanPlayer != null)
			{
				localHumanPlayer.SetBuildColorSlots(m_oldPaletteList);
				localHumanPlayer.BuildArmorSkin = m_oldArmorSkin.String;
				localHumanPlayer.SelectedBuildColorSlot = m_oldColorSlot;
			}
			ApplySkin = m_oldApplySkin;
			ApplyColor = m_oldApplyColor;
			MyCubeBuilder.Static.ColorPickerCancel();
			CloseScreenNow();
		}

		public override string GetFriendlyName()
		{
			return "ColorPick";
		}

		private List<MyGameInventoryItemDefinition> GetInventoryItems()
		{
			return (from e in MyGameService.GetDefinitionsForSlot(MyGameInventoryItemSlot.Armor)
				orderby e.Name
				select e).ToList();
		}
	}
}
