using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlCombobox))]
	public class MyGuiControlCombobox : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public string ItemFontHighlight;

			public string ItemFontNormal;

			public string ItemTextureHighlight;

			public Vector2 SelectedItemOffset;

			public MyGuiCompositeTexture DropDownTexture;

			public MyGuiCompositeTexture ComboboxTextureNormal;

			public MyGuiCompositeTexture ComboboxTextureHighlight;

			public float TextScale;

			public float DropDownHighlightExtraWidth;

			public MyGuiBorderThickness ScrollbarMargin;
		}

		public class Item : IComparable
		{
			public readonly long Key;

			public readonly int SortOrder;

			public readonly StringBuilder Value;

			public MyToolTips ToolTip;

			public Item(long key, StringBuilder value, int sortOrder, string toolTip = null)
			{
				Key = key;
				SortOrder = sortOrder;
				if (value != null)
				{
					Value = new StringBuilder(value.Length).AppendStringBuilder(value);
				}
				else
				{
					Value = new StringBuilder();
				}
				if (toolTip != null)
				{
					ToolTip = new MyToolTips(toolTip);
				}
			}

			public Item(long key, string value, int sortOrder, string toolTip = null)
			{
				Key = key;
				SortOrder = sortOrder;
				if (value != null)
				{
					Value = new StringBuilder(value.Length).Append(value);
				}
				else
				{
					Value = new StringBuilder();
				}
				if (toolTip != null)
				{
					ToolTip = new MyToolTips(toolTip);
				}
			}

			public int CompareTo(object compareToObject)
			{
				Item item = (Item)compareToObject;
				return SortOrder.CompareTo(item.SortOrder);
			}
		}

		public delegate void ItemSelectedDelegate();

		private const float ITEM_HEIGHT = 0.03f;

		private static StyleDefinition[] m_styles;

		private bool m_isOpen;

		private bool m_scrollBarDragging;

		private List<Item> m_items;

		private Item m_selected;

		private Item m_preselectedMouseOver;

		private Item m_preselectedMouseOverPrevious;

		private int? m_preselectedKeyboardIndex;

		private int? m_preselectedKeyboardIndexPrevious;

		private int m_openAreaItemsCount;

		private int m_middleIndex;

		private bool m_showScrollBar;

		private float m_scrollBarCurrentPosition;

		private float m_scrollBarCurrentNonadjustedPosition;

		private float m_mouseOldPosition;

		private bool m_mousePositionReinit;

		private float m_maxScrollBarPosition;

		private float m_scrollBarEndPositionRelative;

		private int m_displayItemsStartIndex;

		private int m_displayItemsEndIndex;

		private int m_scrollBarItemOffSet;

		private float m_scrollBarHeight;

		private float m_scrollBarWidth;

		private float m_comboboxItemDeltaHeight;

		private float m_scrollRatio;

		private Vector2 m_dropDownItemSize;

		private const float ITEM_DRAW_DELTA = 0.0001f;

		private bool m_useScrollBarOffset;

		private MyGuiControlComboboxStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		private RectangleF m_selectedItemArea;

		private RectangleF m_openedArea;

		private RectangleF m_openedItemArea;

		private string m_selectedItemFont;

		private MyGuiCompositeTexture m_scrollbarTexture;

		private Vector4 m_textColor;

		private float m_textScaleWithLanguage;

		private bool m_isFlipped;

		public bool IsOpen => m_isOpen;

		public MyGuiControlComboboxStyleEnum VisualStyle
		{
			get
			{
				return m_visualStyle;
			}
			set
			{
				m_visualStyle = value;
				RefreshVisualStyle();
			}
		}

		public bool IsFlipped
		{
			get
			{
				return m_isFlipped;
			}
			set
			{
				if (m_isFlipped != value)
				{
					m_isFlipped = value;
					RefreshInternals();
				}
			}
		}

		public event ItemSelectedDelegate ItemSelected;

		static MyGuiControlCombobox()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlComboboxStyleEnum>() + 1];
			StyleDefinition[] styles = m_styles;
			StyleDefinition obj = new StyleDefinition
			{
				DropDownTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST_BORDER,
				ComboboxTextureNormal = MyGuiConstants.TEXTURE_COMBOBOX_NORMAL,
				ComboboxTextureHighlight = MyGuiConstants.TEXTURE_COMBOBOX_HIGHLIGHT,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				SelectedItemOffset = new Vector2(0.01f, 0.005f),
				TextScale = 0.719999969f,
				DropDownHighlightExtraWidth = 0.007f
			};
			MyGuiBorderThickness scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj.ScrollbarMargin = scrollbarMargin;
			styles[0] = obj;
			StyleDefinition[] styles2 = m_styles;
			StyleDefinition obj2 = new StyleDefinition
			{
				DropDownTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ComboboxTextureNormal = MyGuiConstants.TEXTURE_COMBOBOX_NORMAL,
				ComboboxTextureHighlight = MyGuiConstants.TEXTURE_COMBOBOX_HIGHLIGHT,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Debug",
				ItemFontHighlight = "White",
				SelectedItemOffset = new Vector2(0.01f, 0.005f),
				TextScale = 0.719999969f,
				DropDownHighlightExtraWidth = 0.007f
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj2.ScrollbarMargin = scrollbarMargin;
			styles2[1] = obj2;
			StyleDefinition[] styles3 = m_styles;
			StyleDefinition obj3 = new StyleDefinition
			{
				DropDownTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ComboboxTextureNormal = MyGuiConstants.TEXTURE_COMBOBOX_NORMAL,
				ComboboxTextureHighlight = MyGuiConstants.TEXTURE_COMBOBOX_HIGHLIGHT,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				SelectedItemOffset = new Vector2(0.01f, 0.005f),
				TextScale = 0.719999969f,
				DropDownHighlightExtraWidth = 0.007f
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj3.ScrollbarMargin = scrollbarMargin;
			styles3[2] = obj3;
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlComboboxStyleEnum style)
		{
			return m_styles[(int)style];
		}

		private void RefreshVisualStyle()
		{
			m_styleDef = GetVisualStyle(VisualStyle);
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			if (base.HasHighlight)
			{
				BackgroundTexture = m_styleDef.ComboboxTextureHighlight;
				m_selectedItemFont = m_styleDef.ItemFontHighlight;
			}
			else
			{
				BackgroundTexture = m_styleDef.ComboboxTextureNormal;
				m_selectedItemFont = m_styleDef.ItemFontNormal;
			}
			base.MinSize = BackgroundTexture.MinSizeGui;
			base.MaxSize = BackgroundTexture.MaxSizeGui;
			m_scrollbarTexture = (base.HasHighlight ? MyGuiConstants.TEXTURE_SCROLLBAR_V_THUMB_HIGHLIGHT : MyGuiConstants.TEXTURE_SCROLLBAR_V_THUMB);
			m_selectedItemArea.Position = m_styleDef.SelectedItemOffset;
			m_selectedItemArea.Size = new Vector2(base.Size.X - (m_scrollbarTexture.MinSizeGui.X + m_styleDef.ScrollbarMargin.HorizontalSum + m_styleDef.SelectedItemOffset.X), 0.03f);
			MyRectangle2D openedArea = GetOpenedArea();
			m_openedArea.Position = openedArea.LeftTop;
			m_openedArea.Size = openedArea.Size;
			m_openedItemArea.Position = m_openedArea.Position + new Vector2(m_styleDef.SelectedItemOffset.X, m_styleDef.DropDownTexture.LeftTop.SizeGui.Y);
			m_openedItemArea.Size = new Vector2(m_selectedItemArea.Size.X, (float)(m_showScrollBar ? m_openAreaItemsCount : m_items.Count) * m_selectedItemArea.Size.Y);
			m_textScaleWithLanguage = m_styleDef.TextScale * MyGuiManager.LanguageTextScale;
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			RefreshInternals();
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			RefreshInternals();
		}

		protected override void OnOriginAlignChanged()
		{
			RefreshInternals();
			base.OnOriginAlignChanged();
		}

		public MyGuiControlCombobox()
			: this(null, null, null, null, 10, null, useScrollBarOffset: false, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null)
		{
		}

		public MyGuiControlCombobox(Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, Vector2? textOffset = null, int openAreaItemsCount = 10, Vector2? iconSize = null, bool useScrollBarOffset = false, string toolTip = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, Vector4? textColor = null)
			: base(position, size ?? (new Vector2(455f, 48f) / MyGuiConstants.GUI_OPTIMAL_SIZE), backgroundColor, toolTip, null, isActiveControl: true, canHaveFocus: true, MyGuiControlHighlightType.WHEN_ACTIVE, originAlign)
		{
			base.Name = "Combobox";
			HighlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER;
			m_items = new List<Item>();
			m_isOpen = false;
			m_openAreaItemsCount = openAreaItemsCount;
			m_middleIndex = Math.Max(m_openAreaItemsCount / 2 - 1, 0);
			m_textColor = (textColor.HasValue ? textColor.Value : Vector4.One);
			m_dropDownItemSize = GetItemSize();
			m_comboboxItemDeltaHeight = m_dropDownItemSize.Y;
			m_mousePositionReinit = true;
			RefreshVisualStyle();
			InitializeScrollBarParameters();
			m_showToolTip = true;
			m_useScrollBarOffset = useScrollBarOffset;
		}

		public void ClearItems()
		{
			m_items.Clear();
			m_selected = null;
			m_preselectedKeyboardIndex = null;
			m_preselectedKeyboardIndexPrevious = null;
			m_preselectedMouseOver = null;
			m_preselectedMouseOverPrevious = null;
			InitializeScrollBarParameters();
		}

		public void AddItem(long key, MyStringId value, int? sortOrder = null, MyStringId? toolTip = null)
		{
			AddItem(key, MyTexts.Get(value), sortOrder, toolTip.HasValue ? MyTexts.GetString(toolTip.Value) : null);
		}

		public void AddItem(long key, StringBuilder value, int? sortOrder = null, string toolTip = null)
		{
			sortOrder = (sortOrder ?? m_items.Count);
			m_items.Add(new Item(key, value, sortOrder.Value, toolTip));
			m_items.Sort();
			AdjustScrollBarParameters();
			RefreshInternals();
		}

		public void AddItem(long key, string value, int? sortOrder = null, string toolTip = null)
		{
			sortOrder = (sortOrder ?? m_items.Count);
			m_items.Add(new Item(key, value, sortOrder.Value, toolTip));
			m_items.Sort();
			AdjustScrollBarParameters();
			RefreshInternals();
		}

		public void RemoveItem(long key)
		{
			Item item = m_items.Find((Item x) => x.Key == key);
			RemoveItem(item);
		}

		public void RemoveItemByIndex(int index)
		{
			if (index < 0 || index >= m_items.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			RemoveItem(m_items[index]);
		}

		public Item GetItemByIndex(int index)
		{
			if (index < 0 || index >= m_items.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return m_items[index];
		}

		private void RemoveItem(Item item)
		{
			if (item != null)
			{
				m_items.Remove(item);
				if (m_selected == item)
				{
					m_selected = null;
				}
			}
		}

		public Item TryGetItemByKey(long key)
		{
			foreach (Item item in m_items)
			{
				if (item.Key == key)
				{
					return item;
				}
			}
			return null;
		}

		public int GetItemsCount()
		{
			return m_items.Count;
		}

		public void SortItemsByValueText()
		{
			if (m_items != null)
			{
				m_items.Sort((Item item1, Item item2) => item1.Value.ToString().CompareTo(item2.Value.ToString()));
			}
		}

		public void CustomSortItems(Comparison<Item> comparison)
		{
			if (m_items != null)
			{
				m_items.Sort(comparison);
			}
		}

		public override MyGuiControlBase GetExclusiveInputHandler()
		{
			if (!m_isOpen)
			{
				return null;
			}
			return this;
		}

		public void SelectItemByIndex(int index)
		{
			if (!m_items.IsValidIndex(index))
			{
				m_selected = null;
				return;
			}
			m_selected = m_items[index];
			SetScrollBarPositionByIndex(index);
			if (this.ItemSelected != null)
			{
				this.ItemSelected();
			}
		}

		public void SelectItemByKey(long key, bool sendEvent = true)
		{
			int num = 0;
			Item item;
			while (true)
			{
				if (num < m_items.Count)
				{
					item = m_items[num];
					if (item.Key.Equals(key) && m_selected != item)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			m_selected = item;
			m_preselectedKeyboardIndex = num;
			SetScrollBarPositionByIndex(num);
			if (sendEvent && this.ItemSelected != null)
			{
				this.ItemSelected();
			}
		}

		public long GetSelectedKey()
		{
			if (m_selected == null)
			{
				return -1L;
			}
			return m_selected.Key;
		}

		public int GetSelectedIndex()
		{
			if (m_selected == null)
			{
				return -1;
			}
			return m_items.IndexOf(m_selected);
		}

		public StringBuilder GetSelectedValue()
		{
			if (m_selected == null)
			{
				return null;
			}
			return m_selected.Value;
		}

		private void Assert()
		{
		}

		private void SwitchComboboxMode()
		{
			if (m_scrollBarDragging)
			{
				return;
			}
			m_isOpen = !m_isOpen;
			if (!m_isOpen)
			{
				return;
			}
			if (IsFlipped)
			{
				MyRectangle2D openedArea = GetOpenedArea();
				if (GetPositionAbsoluteTopRight().Y - openedArea.LeftTop.Y < 1f)
				{
					IsFlipped = false;
				}
			}
			else
			{
				MyRectangle2D openedArea2 = GetOpenedArea();
				if (GetPositionAbsoluteBottomRight().Y + openedArea2.Size.Y > 1f)
				{
					IsFlipped = true;
				}
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null && base.Enabled)
			{
				if (base.IsMouseOver && MyInput.Static.IsNewPrimaryButtonPressed() && !m_isOpen && !m_scrollBarDragging)
				{
					return this;
				}
				if (MyInput.Static.IsNewPrimaryButtonReleased() && !m_scrollBarDragging && ((base.IsMouseOver && !m_isOpen) || (IsMouseOverSelectedItem() && m_isOpen)))
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
					SwitchComboboxMode();
					myGuiControlBase = this;
				}
				if (base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsNewKeyPressed(MyKeys.Space) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01)))
				{
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
					if (m_preselectedKeyboardIndex.HasValue && m_preselectedKeyboardIndex.Value < m_items.Count)
					{
						if (!m_isOpen)
						{
							SetScrollBarPositionByIndex(m_selected.Key);
						}
						else
						{
							SelectItemByKey(m_items[m_preselectedKeyboardIndex.Value].Key);
						}
					}
					SwitchComboboxMode();
					myGuiControlBase = this;
				}
				if (m_isOpen)
				{
					if (m_showScrollBar && MyInput.Static.IsPrimaryButtonPressed())
					{
						Vector2 positionAbsoluteCenterLeft = GetPositionAbsoluteCenterLeft();
						MyRectangle2D openedArea = GetOpenedArea();
						openedArea.LeftTop += GetPositionAbsoluteTopLeft();
						float num = positionAbsoluteCenterLeft.X + base.Size.X - m_scrollBarWidth;
						float num2 = positionAbsoluteCenterLeft.X + base.Size.X;
						float num3;
						float num4;
						if (IsFlipped)
						{
							num3 = openedArea.LeftTop.Y - base.Size.Y / 2f;
							num4 = num3 + openedArea.Size.Y;
						}
						else
						{
							num3 = positionAbsoluteCenterLeft.Y + base.Size.Y / 2f;
							num4 = num3 + openedArea.Size.Y;
						}
						if (m_scrollBarDragging)
						{
							num = float.NegativeInfinity;
							num2 = float.PositiveInfinity;
							num3 = float.NegativeInfinity;
							num4 = float.PositiveInfinity;
						}
						if (MyGuiManager.MouseCursorPosition.X >= num && MyGuiManager.MouseCursorPosition.X <= num2 && MyGuiManager.MouseCursorPosition.Y >= num3 && MyGuiManager.MouseCursorPosition.Y <= num4)
						{
							float num5 = m_scrollBarCurrentPosition + openedArea.LeftTop.Y;
							if (MyGuiManager.MouseCursorPosition.Y > num5 && MyGuiManager.MouseCursorPosition.Y < num5 + m_scrollBarHeight)
							{
								if (m_mousePositionReinit)
								{
									m_mouseOldPosition = MyGuiManager.MouseCursorPosition.Y;
									m_mousePositionReinit = false;
								}
								float num6 = MyGuiManager.MouseCursorPosition.Y - m_mouseOldPosition;
								if (num6 > float.Epsilon || num6 < float.Epsilon)
								{
									SetScrollBarPosition(m_scrollBarCurrentNonadjustedPosition + num6);
								}
								m_mouseOldPosition = MyGuiManager.MouseCursorPosition.Y;
							}
							else
							{
								float value = MyGuiManager.MouseCursorPosition.Y - openedArea.LeftTop.Y - m_scrollBarHeight / 2f;
								SetScrollBarPosition(value);
							}
							m_scrollBarDragging = true;
						}
					}
					if (MyInput.Static.IsNewPrimaryButtonReleased())
					{
						m_mouseOldPosition = MyGuiManager.MouseCursorPosition.Y;
						m_mousePositionReinit = true;
					}
					if ((base.HasFocus && (MyInput.Static.IsNewKeyPressed(MyKeys.Escape) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J02))) || (!IsMouseOverOnOpenedArea() && !base.IsMouseOver && MyInput.Static.IsNewLeftMousePressed()))
					{
						MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
						m_isOpen = false;
					}
					myGuiControlBase = this;
					if (!m_scrollBarDragging)
					{
						m_preselectedMouseOverPrevious = m_preselectedMouseOver;
						m_preselectedMouseOver = null;
						int num7 = 0;
						int num8 = m_items.Count;
						float num9 = 0f;
						if (m_showScrollBar)
						{
							num7 = m_displayItemsStartIndex;
							num8 = m_displayItemsEndIndex;
							num9 = 0.025f;
						}
						for (int i = num7; i < num8; i++)
						{
							Vector2 openItemPosition = GetOpenItemPosition(i - m_displayItemsStartIndex);
							MyRectangle2D openedArea2 = GetOpenedArea();
							Vector2 value2 = new Vector2(openItemPosition.X, Math.Max(openedArea2.LeftTop.Y, openItemPosition.Y));
							Vector2 vector = value2 + new Vector2(base.Size.X - num9, m_comboboxItemDeltaHeight);
							Vector2 vector2 = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
							if (vector2.X >= value2.X && vector2.X <= vector.X && vector2.Y >= value2.Y && vector2.Y <= vector.Y)
							{
								m_preselectedMouseOver = m_items[i];
							}
						}
						if (m_preselectedMouseOver != null && m_preselectedMouseOver != m_preselectedMouseOverPrevious)
						{
							MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
						}
						if (MyInput.Static.IsNewPrimaryButtonReleased() && m_preselectedMouseOver != null)
						{
							SelectItemByKey(m_preselectedMouseOver.Key);
							MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
							m_isOpen = false;
							myGuiControlBase = this;
						}
						if (base.HasFocus || IsMouseOverOnOpenedArea())
						{
							if (MyInput.Static.DeltaMouseScrollWheelValue() < 0)
							{
								HandleItemMovement(forwardMovement: true);
								myGuiControlBase = this;
							}
							else if (MyInput.Static.DeltaMouseScrollWheelValue() > 0)
							{
								HandleItemMovement(forwardMovement: false);
								myGuiControlBase = this;
							}
							if (MyInput.Static.IsNewKeyPressed(MyKeys.Down) || MyInput.Static.IsNewGamepadKeyDownPressed())
							{
								HandleItemMovement(forwardMovement: true);
								myGuiControlBase = this;
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.Up) || MyInput.Static.IsNewGamepadKeyUpPressed())
							{
								HandleItemMovement(forwardMovement: false);
								myGuiControlBase = this;
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.PageDown))
							{
								HandleItemMovement(forwardMovement: true, page: true);
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.PageUp))
							{
								HandleItemMovement(forwardMovement: false, page: true);
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.Home))
							{
								HandleItemMovement(forwardMovement: true, page: false, list: true);
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.End))
							{
								HandleItemMovement(forwardMovement: false, page: false, list: true);
							}
							else if (MyInput.Static.IsNewKeyPressed(MyKeys.Tab))
							{
								if (m_isOpen)
								{
									SwitchComboboxMode();
								}
								myGuiControlBase = null;
							}
						}
					}
					else
					{
						if (MyInput.Static.IsNewPrimaryButtonReleased())
						{
							m_scrollBarDragging = false;
						}
						myGuiControlBase = this;
					}
				}
			}
			return myGuiControlBase;
		}

		private void HandleItemMovement(bool forwardMovement, bool page = false, bool list = false)
		{
			m_preselectedKeyboardIndexPrevious = m_preselectedKeyboardIndex;
			int num = 0;
			if (list && forwardMovement)
			{
				m_preselectedKeyboardIndex = 0;
			}
			else if (!list || forwardMovement)
			{
				num = ((page && forwardMovement) ? ((m_openAreaItemsCount <= m_items.Count) ? (m_openAreaItemsCount - 1) : (m_items.Count - 1)) : ((page && !forwardMovement) ? ((m_openAreaItemsCount <= m_items.Count) ? (-m_openAreaItemsCount + 1) : (-(m_items.Count - 1))) : ((!page && !list && forwardMovement) ? 1 : (-1))));
			}
			else
			{
				m_preselectedKeyboardIndex = m_items.Count - 1;
			}
			if (!m_preselectedKeyboardIndex.HasValue)
			{
				m_preselectedKeyboardIndex = ((!forwardMovement) ? (m_items.Count - 1) : 0);
			}
			else
			{
				m_preselectedKeyboardIndex += num;
				if (m_preselectedKeyboardIndex > m_items.Count - 1)
				{
					m_preselectedKeyboardIndex = m_items.Count - 1;
				}
				if (m_preselectedKeyboardIndex < 0)
				{
					m_preselectedKeyboardIndex = 0;
				}
			}
			if (m_preselectedKeyboardIndex != m_preselectedKeyboardIndexPrevious)
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
			}
			SetScrollBarPositionByIndex(m_preselectedKeyboardIndex.Value);
		}

		private void SetScrollBarPositionByIndex(long index)
		{
			if (m_showScrollBar)
			{
				m_scrollRatio = 0f;
				if (m_preselectedKeyboardIndex >= m_displayItemsEndIndex)
				{
					m_displayItemsEndIndex = Math.Max(m_openAreaItemsCount, m_preselectedKeyboardIndex.Value + 1);
					m_displayItemsStartIndex = Math.Max(0, m_displayItemsEndIndex - m_openAreaItemsCount);
					SetScrollBarPosition((float)m_preselectedKeyboardIndex.Value * m_maxScrollBarPosition / (float)(m_items.Count - 1), calculateItemIndexes: false);
				}
				else if (m_preselectedKeyboardIndex < m_displayItemsStartIndex)
				{
					m_displayItemsStartIndex = Math.Max(0, m_preselectedKeyboardIndex.Value);
					m_displayItemsEndIndex = Math.Max(m_openAreaItemsCount, m_displayItemsStartIndex + m_openAreaItemsCount);
					SetScrollBarPosition((float)m_preselectedKeyboardIndex.Value * m_maxScrollBarPosition / (float)(m_items.Count - 1), calculateItemIndexes: false);
				}
				else if (m_preselectedKeyboardIndex.HasValue)
				{
					SetScrollBarPosition((float)m_preselectedKeyboardIndex.Value * m_maxScrollBarPosition / (float)(m_items.Count - 1), calculateItemIndexes: false);
				}
			}
		}

		private bool IsMouseOverOnOpenedArea()
		{
			MyRectangle2D openedArea = GetOpenedArea();
			openedArea.Size.Y += m_dropDownItemSize.Y;
			Vector2 leftTop = openedArea.LeftTop;
			Vector2 vector = openedArea.LeftTop + openedArea.Size;
			Vector2 vector2 = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
			if (vector2.X >= leftTop.X && vector2.X <= vector.X && vector2.Y >= leftTop.Y)
			{
				return vector2.Y <= vector.Y;
			}
			return false;
		}

		private MyRectangle2D GetOpenedArea()
		{
			MyRectangle2D result = default(MyRectangle2D);
			if (IsFlipped)
			{
				if (m_showScrollBar)
				{
					result.LeftTop = new Vector2(0f, (float)(-m_openAreaItemsCount) * m_comboboxItemDeltaHeight);
				}
				else
				{
					result.LeftTop = new Vector2(0f, (float)(-m_items.Count) * m_comboboxItemDeltaHeight);
				}
				if (m_showScrollBar)
				{
					result.Size = new Vector2(m_dropDownItemSize.X, (float)m_openAreaItemsCount * m_comboboxItemDeltaHeight);
				}
				else
				{
					result.Size = new Vector2(m_dropDownItemSize.X, (float)m_items.Count * m_comboboxItemDeltaHeight);
				}
			}
			else
			{
				result.LeftTop = new Vector2(0f, base.Size.Y);
				if (m_showScrollBar)
				{
					result.Size = new Vector2(m_dropDownItemSize.X, (float)m_openAreaItemsCount * m_comboboxItemDeltaHeight);
				}
				else
				{
					result.Size = new Vector2(m_dropDownItemSize.X, (float)m_items.Count * m_comboboxItemDeltaHeight);
				}
			}
			return result;
		}

		private Vector2 GetOpenItemPosition(int index)
		{
			float num = m_dropDownItemSize.Y / 2f;
			num += m_comboboxItemDeltaHeight * 0.5f;
			if (IsFlipped)
			{
				return new Vector2(0f, -0.5f * base.Size.Y) + new Vector2(0f, num - (float)(Math.Min(m_openAreaItemsCount, m_items.Count) - index) * m_comboboxItemDeltaHeight);
			}
			return new Vector2(0f, 0.5f * base.Size.Y) + new Vector2(0f, num + (float)index * m_comboboxItemDeltaHeight);
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, transitionAlpha);
			if (m_selected != null)
			{
				DrawSelectedItemText(transitionAlpha);
			}
			float scrollbarInnerTexturePositionX = GetPositionAbsoluteCenterLeft().X + base.Size.X - m_scrollBarWidth / 2f;
			int startIndex = 0;
			int endIndex = m_items.Count;
			if (m_showScrollBar)
			{
				startIndex = m_displayItemsStartIndex;
				endIndex = m_displayItemsEndIndex;
			}
			if (m_isOpen)
			{
				MyRectangle2D openedArea = GetOpenedArea();
				DrawOpenedAreaItems(startIndex, endIndex, transitionAlpha);
				if (m_showScrollBar)
				{
					DrawOpenedAreaScrollbar(scrollbarInnerTexturePositionX, openedArea, transitionAlpha);
				}
			}
		}

		private void DebugDraw()
		{
			BorderEnabled = true;
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_selectedItemArea.Position, m_selectedItemArea.Size, Color.Cyan, 1);
			if (m_isOpen)
			{
				MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_openedArea.Position, m_openedArea.Size, Color.GreenYellow, 1);
				MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_openedItemArea.Position, m_openedItemArea.Size, Color.Red, 1);
			}
		}

		private void DrawOpenedAreaScrollbar(float scrollbarInnerTexturePositionX, MyRectangle2D openedArea, float transitionAlpha)
		{
			MyGuiBorderThickness scrollbarMargin = m_styleDef.ScrollbarMargin;
			Vector2 positionTopLeft;
			if (IsFlipped)
			{
				positionTopLeft = GetPositionAbsoluteTopRight();
				positionTopLeft += new Vector2(0f - (scrollbarMargin.Right + m_scrollbarTexture.MinSizeGui.X), 0f - openedArea.Size.Y + scrollbarMargin.Top + m_scrollBarCurrentPosition);
			}
			else
			{
				positionTopLeft = GetPositionAbsoluteBottomRight();
				positionTopLeft += new Vector2(0f - (scrollbarMargin.Right + m_scrollbarTexture.MinSizeGui.X), scrollbarMargin.Top + m_scrollBarCurrentPosition);
			}
			m_scrollbarTexture.Draw(positionTopLeft, m_scrollBarHeight - m_scrollbarTexture.MinSizeGui.Y, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
		}

		private void DrawOpenedAreaItems(int startIndex, int endIndex, float transitionAlpha)
		{
			float num = (float)(endIndex - startIndex) * (m_comboboxItemDeltaHeight + 0.0001f);
			Vector2 minSizeGui = m_styleDef.DropDownTexture.MinSizeGui;
			Vector2 maxSizeGui = m_styleDef.DropDownTexture.MaxSizeGui;
			Vector2 size = Vector2.Clamp(new Vector2(base.Size.X, num + minSizeGui.Y), minSizeGui, maxSizeGui);
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			m_styleDef.DropDownTexture.Draw(positionAbsoluteTopLeft + m_openedArea.Position, size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
			RectangleF normalizedRectangle = m_openedItemArea;
			normalizedRectangle.Position += positionAbsoluteTopLeft;
			normalizedRectangle.Position.X -= m_styleDef.DropDownHighlightExtraWidth;
			normalizedRectangle.Size.X += m_styleDef.DropDownHighlightExtraWidth;
			using (MyGuiManager.UsingScissorRectangle(ref normalizedRectangle))
			{
				Vector2 value = positionAbsoluteTopLeft + m_openedItemArea.Position;
				for (int i = startIndex; i < endIndex && i < m_items.Count; i++)
				{
					Item item = m_items[i];
					string font = m_styleDef.ItemFontNormal;
					if (item == m_preselectedMouseOver || (m_preselectedKeyboardIndex.HasValue && m_preselectedKeyboardIndex == i))
					{
						MyGuiManager.DrawSpriteBatchRoundUp(m_styleDef.ItemTextureHighlight, value - new Vector2(m_styleDef.DropDownHighlightExtraWidth, 0f), m_selectedItemArea.Size + new Vector2(m_styleDef.DropDownHighlightExtraWidth, 0f), MyGuiControlBase.ApplyColorMaskModifiers(Vector4.One, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
						font = m_styleDef.ItemFontHighlight;
					}
					MyGuiManager.DrawString(font, item.Value, new Vector2(value.X, value.Y + m_styleDef.DropDownHighlightExtraWidth / 2f), m_textScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(m_textColor, base.Enabled, transitionAlpha));
					value.Y += 0.03f;
				}
			}
		}

		private void DrawSelectedItemText(float transitionAlpha)
		{
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			RectangleF normalizedRectangle = m_selectedItemArea;
			normalizedRectangle.Position += positionAbsoluteTopLeft;
			using (MyGuiManager.UsingScissorRectangle(ref normalizedRectangle))
			{
				Vector2 normalizedCoord = positionAbsoluteTopLeft + m_selectedItemArea.Position + new Vector2(0f, m_selectedItemArea.Size.Y * 0.5f);
				MyGuiManager.DrawString(m_selectedItemFont, m_selected.Value, normalizedCoord, m_textScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(m_textColor, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			}
		}

		private void InitializeScrollBarParameters()
		{
			m_showScrollBar = false;
			Vector2 cOMBOBOX_VSCROLLBAR_SIZE = MyGuiConstants.COMBOBOX_VSCROLLBAR_SIZE;
			m_scrollBarWidth = cOMBOBOX_VSCROLLBAR_SIZE.X;
			m_scrollBarHeight = cOMBOBOX_VSCROLLBAR_SIZE.Y;
			m_scrollBarCurrentPosition = 0f;
			m_scrollBarEndPositionRelative = (float)m_openAreaItemsCount * m_comboboxItemDeltaHeight + m_styleDef.DropDownTexture.LeftBottom.SizeGui.Y;
			m_displayItemsStartIndex = 0;
			m_displayItemsEndIndex = m_openAreaItemsCount;
		}

		private void AdjustScrollBarParameters()
		{
			m_showScrollBar = (m_items.Count > m_openAreaItemsCount);
			if (m_showScrollBar)
			{
				m_maxScrollBarPosition = m_scrollBarEndPositionRelative - m_scrollBarHeight;
				m_scrollBarItemOffSet = m_items.Count - m_openAreaItemsCount;
			}
		}

		private void CalculateStartAndEndDisplayItemsIndex()
		{
			m_scrollRatio = ((m_scrollBarCurrentPosition == 0f) ? 0f : (m_scrollBarCurrentPosition * (float)m_scrollBarItemOffSet / m_maxScrollBarPosition));
			m_displayItemsStartIndex = Math.Max(0, (int)Math.Floor((double)m_scrollRatio + 0.5));
			m_displayItemsEndIndex = Math.Min(m_items.Count, m_displayItemsStartIndex + m_openAreaItemsCount);
		}

		public void ScrollToPreSelectedItem()
		{
			if (m_preselectedKeyboardIndex.HasValue)
			{
				m_displayItemsStartIndex = ((m_preselectedKeyboardIndex.Value > m_middleIndex) ? (m_preselectedKeyboardIndex.Value - m_middleIndex) : 0);
				m_displayItemsEndIndex = m_displayItemsStartIndex + m_openAreaItemsCount;
				if (m_displayItemsEndIndex > m_items.Count)
				{
					m_displayItemsEndIndex = m_items.Count;
					m_displayItemsStartIndex = m_displayItemsEndIndex - m_openAreaItemsCount;
				}
				SetScrollBarPosition((float)m_displayItemsStartIndex * m_maxScrollBarPosition / (float)m_scrollBarItemOffSet);
			}
		}

		private void SetScrollBarPosition(float value, bool calculateItemIndexes = true)
		{
			value = MathHelper.Clamp(value, 0f, m_maxScrollBarPosition);
			if (m_scrollBarCurrentPosition != value)
			{
				m_scrollBarCurrentNonadjustedPosition = value;
				m_scrollBarCurrentPosition = value;
				if (calculateItemIndexes)
				{
					CalculateStartAndEndDisplayItemsIndex();
				}
			}
		}

		protected Vector2 GetItemSize()
		{
			return MyGuiConstants.COMBOBOX_MEDIUM_ELEMENT_SIZE;
		}

		public override bool CheckMouseOver()
		{
			if (m_isOpen)
			{
				int num = m_showScrollBar ? m_openAreaItemsCount : m_items.Count;
				for (int i = 0; i < num; i++)
				{
					Vector2 openItemPosition = GetOpenItemPosition(i);
					MyRectangle2D openedArea = GetOpenedArea();
					Vector2 value = new Vector2(openItemPosition.X, Math.Max(openedArea.LeftTop.Y, openItemPosition.Y));
					Vector2 vector = value + new Vector2(base.Size.X, m_comboboxItemDeltaHeight);
					Vector2 vector2 = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
					if (vector2.X >= value.X && vector2.X <= vector.X && vector2.Y >= value.Y && vector2.Y <= vector.Y)
					{
						return true;
					}
				}
			}
			if (m_scrollBarDragging)
			{
				return false;
			}
			return MyGuiControlBase.CheckMouseOver(base.Size, GetPositionAbsolute(), base.OriginAlign);
		}

		private void SnapCursorToControl(int controlIndex)
		{
			Vector2 openItemPosition = GetOpenItemPosition(controlIndex);
			Vector2 openItemPosition2 = GetOpenItemPosition(m_displayItemsStartIndex);
			Vector2 vector = openItemPosition - openItemPosition2;
			MyRectangle2D openedArea = GetOpenedArea();
			Vector2 positionAbsoluteCenter = GetPositionAbsoluteCenter();
			positionAbsoluteCenter.Y += openedArea.LeftTop.Y;
			positionAbsoluteCenter.Y += vector.Y;
			Vector2 screenCoordinateFromNormalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(positionAbsoluteCenter);
			m_preselectedMouseOver = m_items[controlIndex];
			MyInput.Static.SetMousePosition((int)screenCoordinateFromNormalizedCoordinate.X, (int)screenCoordinateFromNormalizedCoordinate.Y);
		}

		private bool IsMouseOverSelectedItem()
		{
			Vector2 value = GetPositionAbsoluteCenterLeft() - new Vector2(0f, base.Size.Y / 2f);
			Vector2 vector = value + base.Size;
			if (MyGuiManager.MouseCursorPosition.X >= value.X && MyGuiManager.MouseCursorPosition.X <= vector.X && MyGuiManager.MouseCursorPosition.Y >= value.Y)
			{
				return MyGuiManager.MouseCursorPosition.Y <= vector.Y;
			}
			return false;
		}

		public override void ShowToolTip()
		{
			MyToolTips toolTip = m_toolTip;
			if (m_isOpen && IsMouseOverOnOpenedArea() && m_preselectedMouseOver != null && m_preselectedMouseOver.ToolTip != null)
			{
				m_toolTip = m_preselectedMouseOver.ToolTip;
			}
			base.ShowToolTip();
			m_toolTip = toolTip;
		}

		public void ApplyStyle(StyleDefinition style)
		{
			if (style != null)
			{
				m_styleDef = style;
				RefreshInternals();
			}
		}
	}
}
