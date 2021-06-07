using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlListbox))]
	public class MyGuiControlListbox : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public string ItemFontHighlight;

			public string ItemFontNormal;

			public Vector2 ItemSize;

			public string ItemTextureHighlight;

			public Vector2 ItemsOffset;

			public float TextOffset;

			public bool DrawScroll;

			public bool PriorityCaptureInput;

			public bool XSizeVariable;

			public float TextScale;

			public MyGuiCompositeTexture Texture;

			public MyGuiBorderThickness ScrollbarMargin;

			public StyleDefinition CloneShallow()
			{
				return new StyleDefinition
				{
					ItemFontHighlight = ItemFontHighlight,
					ItemFontNormal = ItemFontNormal,
					ItemSize = new Vector2(ItemSize.X, ItemSize.Y),
					ItemTextureHighlight = ItemTextureHighlight,
					ItemsOffset = new Vector2(ItemsOffset.X, ItemsOffset.Y),
					TextOffset = TextOffset,
					DrawScroll = DrawScroll,
					PriorityCaptureInput = PriorityCaptureInput,
					XSizeVariable = XSizeVariable,
					TextScale = TextScale,
					Texture = Texture,
					ScrollbarMargin = ScrollbarMargin
				};
			}
		}

		public class Item
		{
			private bool m_visible;

			public readonly string Icon;

			public readonly MyToolTips ToolTip;

			public readonly object UserData;

			public string FontOverride;

			public Vector4 ColorMask = Vector4.One;

			public StringBuilder Text
			{
				get;
				set;
			}

			public bool Visible
			{
				get
				{
					return m_visible;
				}
				set
				{
					if (m_visible != value)
					{
						m_visible = value;
						if (this.OnVisibleChanged != null)
						{
							this.OnVisibleChanged();
						}
					}
				}
			}

			public event Action OnVisibleChanged;

			public Item(StringBuilder text = null, string toolTip = null, string icon = null, object userData = null, string fontOverride = null)
			{
				Text = new StringBuilder((text != null) ? text.ToString() : "");
				ToolTip = ((toolTip != null) ? new MyToolTips(toolTip) : null);
				Icon = icon;
				UserData = userData;
				FontOverride = fontOverride;
				Visible = true;
			}

			public Item(ref StringBuilder text, string toolTip = null, string icon = null, object userData = null, string fontOverride = null)
			{
				Text = text;
				ToolTip = ((toolTip != null) ? new MyToolTips(toolTip) : null);
				Icon = icon;
				UserData = userData;
				FontOverride = fontOverride;
				Visible = true;
			}
		}

		private static StyleDefinition[] m_styles;

		private Vector2 m_doubleClickFirstPosition;

		private int? m_doubleClickStarted;

		private RectangleF m_itemsRectangle;

		private Item m_mouseOverItem;

		private StyleDefinition m_styleDef;

		private StyleDefinition m_customStyle;

		private bool m_useCustomStyle;

		private MyVScrollbar m_scrollBar;

		private int m_visibleRowIndexOffset;

		private int m_visibleRows;

		public readonly ObservableCollection<Item> Items;

		public List<Item> SelectedItems = new List<Item>();

		private MyGuiControlListboxStyleEnum m_visualStyle;

		public bool MultiSelect;

		private List<Item> m_StoredSelectedItems = new List<Item>();

		private int m_StoredTopmostSelectedPosition;

		private Item m_StoredTopmostSelectedItem;

		private Item m_StoredMouseOverItem;

		private int m_StoredMouseOverPosition;

		private Item m_StoredItemOnTop;

		private float m_StoredScrollbarValue;

		private Vector2 m_entryPoint;

		private MyDirection m_entryDirection;

		public Item MouseOverItem => m_mouseOverItem;

		public Vector2 ItemSize
		{
			get;
			set;
		}

		public float TextScale
		{
			get;
			set;
		}

		public int VisibleRowsCount
		{
			get
			{
				return m_visibleRows;
			}
			set
			{
				m_visibleRows = value;
				RefreshInternals();
			}
		}

		public int FirstVisibleRow
		{
			get
			{
				return m_visibleRowIndexOffset;
			}
			set
			{
				m_scrollBar.Value = value;
			}
		}

		public MyGuiControlListboxStyleEnum VisualStyle
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

		public Item SelectedItem
		{
			get
			{
				if (SelectedItems == null)
				{
					SelectedItems = new List<Item>();
				}
				if (SelectedItems.Count == 0)
				{
					return null;
				}
				return SelectedItems[SelectedItems.Count - 1];
			}
			set
			{
				if (SelectedItems == null)
				{
					SelectedItems = new List<Item>();
				}
				SelectedItems.Clear();
				SelectedItems.Add(value);
			}
		}

		public override RectangleF FocusRectangle => GetRowRectangle(SelectedItems);

		public event Action<MyGuiControlListbox> ItemClicked;

		public event Action<MyGuiControlListbox> ItemDoubleClicked;

		public event Action<MyGuiControlListbox> ItemsSelected;

		public event Action<MyGuiControlListbox> ItemMouseOver;

		static MyGuiControlListbox()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlListboxStyleEnum>() + 1];
			SetupStyles();
		}

		private static void SetupStyles()
		{
			StyleDefinition[] styles = m_styles;
			StyleDefinition obj = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.25f, 0.034f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
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
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.2535f, 0.034f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
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
				Texture = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.25f, 0.035f),
				TextScale = 0.8f,
				TextOffset = 0.004f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = true,
				XSizeVariable = true
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 0f,
				Right = 0f,
				Top = 0f,
				Bottom = 0f
			};
			obj3.ScrollbarMargin = scrollbarMargin;
			styles3[2] = obj3;
			StyleDefinition[] styles4 = m_styles;
			StyleDefinition obj4 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.25f, 0.035f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj4.ScrollbarMargin = scrollbarMargin;
			styles4[3] = obj4;
			StyleDefinition[] styles5 = m_styles;
			StyleDefinition obj5 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST_TOOLS_BLOCKS,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.15f, 0.0272f),
				TextScale = 0.78f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 6f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj5.ScrollbarMargin = scrollbarMargin;
			styles5[4] = obj5;
			StyleDefinition[] styles6 = m_styles;
			StyleDefinition obj6 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.21f, 0.025f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj6.ScrollbarMargin = scrollbarMargin;
			styles6[5] = obj6;
			StyleDefinition[] styles7 = m_styles;
			StyleDefinition obj7 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				ItemTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemSize = new Vector2(0.231f, 0.035f),
				TextScale = 0.8f,
				TextOffset = 0.006f,
				ItemsOffset = new Vector2(6f, 2f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				DrawScroll = true,
				PriorityCaptureInput = false,
				XSizeVariable = false
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj7.ScrollbarMargin = scrollbarMargin;
			styles7[6] = obj7;
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlListboxStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public MyGuiControlListbox()
			: this(null, MyGuiControlListboxStyleEnum.Default)
		{
		}

		public MyGuiControlListbox(Vector2? position = null, MyGuiControlListboxStyleEnum visualStyle = MyGuiControlListboxStyleEnum.Default)
			: base(position, null, null, null, null, isActiveControl: true, canHaveFocus: true)
		{
			SetupStyles();
			m_scrollBar = new MyVScrollbar(this);
			m_scrollBar.ValueChanged += verticalScrollBar_ValueChanged;
			Items = new ObservableCollection<Item>();
			Items.CollectionChanged += Items_CollectionChanged;
			VisualStyle = visualStyle;
			base.Name = "Listbox";
			MultiSelect = true;
			base.CanFocusChildren = true;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlListbox myObjectBuilder_GuiControlListbox = (MyObjectBuilder_GuiControlListbox)objectBuilder;
			VisibleRowsCount = myObjectBuilder_GuiControlListbox.VisibleRows;
			VisualStyle = myObjectBuilder_GuiControlListbox.VisualStyle;
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlListbox obj = (MyObjectBuilder_GuiControlListbox)base.GetObjectBuilder();
			obj.VisibleRows = VisibleRowsCount;
			obj.VisualStyle = VisualStyle;
			return obj;
		}

		public bool SelectByUserData(object data)
		{
			bool result = false;
			if (SelectedItems == null)
			{
				SelectedItems = new List<Item>();
			}
			foreach (Item item in Items)
			{
				if (item.UserData == data)
				{
					result = true;
					SelectedItems.Add(item);
				}
			}
			return result;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase captureInput = base.HandleInput();
			if (captureInput != null)
			{
				return captureInput;
			}
			if (!base.Enabled || !base.IsMouseOver)
			{
				return null;
			}
			if (m_scrollBar != null && m_scrollBar.HandleInput())
			{
				return this;
			}
			HandleNewMousePress(ref captureInput);
			Vector2 vector = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
			if (m_itemsRectangle.Contains(vector))
			{
				int num = ComputeIndexFromPosition(vector);
				m_mouseOverItem = (IsValidIndex(num) ? Items[num] : null);
				if (this.ItemMouseOver != null)
				{
					this.ItemMouseOver(this);
				}
				if (m_styleDef.PriorityCaptureInput)
				{
					captureInput = this;
				}
			}
			else
			{
				m_mouseOverItem = null;
			}
			if (m_doubleClickStarted.HasValue && (float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) >= 500f)
			{
				m_doubleClickStarted = null;
			}
			return captureInput;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			m_styleDef.Texture.Draw(positionAbsoluteTopLeft, base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, backgroundTransitionAlpha));
			Vector2 vector = positionAbsoluteTopLeft + new Vector2(m_itemsRectangle.X, m_itemsRectangle.Y);
			int i = m_visibleRowIndexOffset;
			Vector2 normalizedSize = Vector2.Zero;
			Vector2 value = Vector2.Zero;
			if (ShouldDrawIconSpacing())
			{
				normalizedSize = MyGuiConstants.LISTBOX_ICON_SIZE;
				value = MyGuiConstants.LISTBOX_ICON_OFFSET;
			}
			for (int j = 0; j < VisibleRowsCount; j++)
			{
				int num = j + m_visibleRowIndexOffset;
				if (num >= Items.Count)
				{
					break;
				}
				if (num < 0)
				{
					continue;
				}
				for (; i < Items.Count && !Items[i].Visible; i++)
				{
				}
				if (i >= Items.Count)
				{
					break;
				}
				Item item = Items[i];
				i++;
				if (item != null)
				{
					Color color = MyGuiControlBase.ApplyColorMaskModifiers(item.ColorMask * base.ColorMask, base.Enabled, transitionAlpha);
					bool flag = SelectedItems.Contains(item) || item == m_mouseOverItem;
					string font = item.FontOverride ?? (flag ? m_styleDef.ItemFontHighlight : m_styleDef.ItemFontNormal);
					if (flag)
					{
						Vector2 itemSize = ItemSize;
						MyGuiManager.DrawSpriteBatch(m_styleDef.ItemTextureHighlight, vector, itemSize, color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
						MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", vector, new Vector2(0.003f, itemSize.Y), new Color(225, 230, 236), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					}
					if (!string.IsNullOrEmpty(item.Icon))
					{
						MyGuiManager.DrawSpriteBatch(item.Icon, vector + value, normalizedSize, color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					}
					MyGuiManager.DrawString(font, item.Text, vector + new Vector2(normalizedSize.X + 2f * value.X, 0.5f * ItemSize.Y) + new Vector2(m_styleDef.TextOffset, 0f), TextScale, color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, useFullClientArea: false, ItemSize.X - normalizedSize.X - 5f * MyGuiConstants.LISTBOX_ICON_OFFSET.X);
				}
				vector.Y += ItemSize.Y;
			}
			if (m_styleDef.DrawScroll)
			{
				m_scrollBar.Draw(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
				Vector2 positionAbsoluteTopRight = GetPositionAbsoluteTopRight();
				positionAbsoluteTopRight.X -= m_styleDef.ScrollbarMargin.HorizontalSum + m_scrollBar.Size.X + 0.0005f;
				MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Controls\\scrollable_list_line.dds", positionAbsoluteTopRight, new Vector2(0.0012f, base.Size.Y), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
		}

		private bool ShouldDrawIconSpacing()
		{
			int i = m_visibleRowIndexOffset;
			for (int j = 0; j < VisibleRowsCount; j++)
			{
				int num = j + m_visibleRowIndexOffset;
				if (num >= Items.Count)
				{
					break;
				}
				if (num >= 0)
				{
					for (; i < Items.Count && !Items[i].Visible; i++)
					{
					}
					if (i >= Items.Count)
					{
						break;
					}
					Item item = Items[i];
					i++;
					if (item != null && !string.IsNullOrEmpty(item.Icon))
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void ShowToolTip()
		{
			if (m_mouseOverItem != null && m_mouseOverItem.ToolTip != null && m_mouseOverItem.ToolTip.ToolTips.Count > 0)
			{
				m_toolTip = m_mouseOverItem.ToolTip;
			}
			else
			{
				m_toolTip = null;
			}
			base.ShowToolTip();
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			RefreshInternals();
		}

		protected override void OnOriginAlignChanged()
		{
			base.OnOriginAlignChanged();
			RefreshInternals();
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			m_scrollBar.HasHighlight = base.HasHighlight;
			m_mouseOverItem = null;
		}

		public override void OnRemoving()
		{
			Items.CollectionChanged -= Items_CollectionChanged;
			Items.Clear();
			this.ItemClicked = null;
			this.ItemDoubleClicked = null;
			this.ItemsSelected = null;
			base.OnRemoving();
		}

		public void Remove(Predicate<Item> match)
		{
			int num = Items.FindIndex(match);
			if (num != -1)
			{
				Items.RemoveAt(num);
			}
		}

		public void Add(Item item, int? position = null)
		{
			item.OnVisibleChanged += item_OnVisibleChanged;
			if (position.HasValue)
			{
				Items.Insert(position.Value, item);
			}
			else
			{
				Items.Add(item);
			}
		}

		private void item_OnVisibleChanged()
		{
			RefreshScrollBar();
		}

		private int ComputeIndexFromPosition(Vector2 position)
		{
			int num = (int)((position.Y - m_itemsRectangle.Position.Y) / ItemSize.Y);
			num++;
			int num2 = 0;
			for (int i = m_visibleRowIndexOffset; i < Items.Count; i++)
			{
				if (Items[i].Visible)
				{
					num2++;
				}
				if (num2 == num)
				{
					return i;
				}
			}
			return -1;
		}

		private void DebugDraw()
		{
			MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft() + m_itemsRectangle.Position, m_itemsRectangle.Size, Color.White, 1);
			m_scrollBar.DebugDraw();
		}

		private void HandleNewMousePress(ref MyGuiControlBase captureInput)
		{
			Vector2 vector = MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft();
			bool flag = m_itemsRectangle.Contains(vector);
			if (MyInput.Static.IsAnyNewMouseOrJoystickPressed() && flag)
			{
				int num = ComputeIndexFromPosition(vector);
				if (IsValidIndex(num) && Items[num].Visible)
				{
					if (MultiSelect && MyInput.Static.IsAnyCtrlKeyPressed())
					{
						if (SelectedItems.Contains(Items[num]))
						{
							SelectedItems.Remove(Items[num]);
						}
						else
						{
							SelectedItems.Add(Items[num]);
						}
					}
					else if (MultiSelect && MyInput.Static.IsAnyShiftKeyPressed())
					{
						int num2 = 0;
						if (SelectedItems.Count > 0)
						{
							num2 = Items.IndexOf(SelectedItems[SelectedItems.Count - 1]);
						}
						do
						{
							num2 += ((num2 <= num) ? 1 : (-1));
							if (!IsValidIndex(num2))
							{
								break;
							}
							if (Items[num2].Visible)
							{
								if (SelectedItems.Contains(Items[num2]))
								{
									SelectedItems.Remove(Items[num2]);
								}
								else
								{
									SelectedItems.Add(Items[num2]);
								}
							}
						}
						while (num2 != num);
					}
					else
					{
						SelectedItems.Clear();
						SelectedItems.Add(Items[num]);
					}
					if (this.ItemsSelected != null)
					{
						this.ItemsSelected(this);
					}
					captureInput = this;
					if (this.ItemClicked != null)
					{
						this.ItemClicked(this);
						MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
					}
				}
			}
			if (!(MyInput.Static.IsNewPrimaryButtonPressed() && flag))
			{
				return;
			}
			if (!m_doubleClickStarted.HasValue)
			{
				int num3 = ComputeIndexFromPosition(vector);
				if (IsValidIndex(num3) && Items[num3].Visible)
				{
					m_doubleClickStarted = MyGuiManager.TotalTimeInMilliseconds;
					m_doubleClickFirstPosition = MyGuiManager.MouseCursorPosition;
				}
			}
			else if ((float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) <= 500f && (m_doubleClickFirstPosition - MyGuiManager.MouseCursorPosition).Length() <= 0.005f)
			{
				if (this.ItemDoubleClicked != null)
				{
					this.ItemDoubleClicked(this);
				}
				m_doubleClickStarted = null;
				captureInput = this;
			}
		}

		private void RefreshVisualStyle()
		{
			if (m_useCustomStyle)
			{
				m_styleDef = m_customStyle;
			}
			else
			{
				m_styleDef = GetVisualStyle(VisualStyle);
			}
			ItemSize = m_styleDef.ItemSize;
			TextScale = m_styleDef.TextScale;
			RefreshInternals();
		}

		private float ComputeVariableItemWidth()
		{
			float num = 0.015f;
			int num2 = 0;
			foreach (Item item in Items)
			{
				if (item.Text.Length > num2)
				{
					num2 = item.Text.Length;
				}
			}
			return (float)num2 * num;
		}

		public bool IsOverScrollBar()
		{
			return m_scrollBar.IsOverCaret;
		}

		private void RefreshInternals()
		{
			Vector2 minSizeGui = m_styleDef.Texture.MinSizeGui;
			Vector2 maxSizeGui = m_styleDef.Texture.MaxSizeGui;
			if (m_styleDef.XSizeVariable)
			{
				ItemSize = new Vector2(ComputeVariableItemWidth(), ItemSize.Y);
			}
			if (m_styleDef.DrawScroll && !m_styleDef.XSizeVariable)
			{
				base.Size = Vector2.Clamp(new Vector2(m_styleDef.TextOffset + m_styleDef.ScrollbarMargin.HorizontalSum + m_styleDef.ItemSize.X + m_scrollBar.Size.X, minSizeGui.Y + m_styleDef.ItemSize.Y * (float)VisibleRowsCount), minSizeGui, maxSizeGui);
			}
			else
			{
				base.Size = Vector2.Clamp(new Vector2(m_styleDef.TextOffset + ItemSize.X, minSizeGui.Y + ItemSize.Y * (float)VisibleRowsCount), minSizeGui, maxSizeGui);
			}
			RefreshScrollBar();
			m_itemsRectangle.X = m_styleDef.ItemsOffset.X;
			m_itemsRectangle.Y = m_styleDef.ItemsOffset.Y + m_styleDef.Texture.LeftTop.SizeGui.Y;
			m_itemsRectangle.Width = ItemSize.X;
			m_itemsRectangle.Height = ItemSize.Y * (float)VisibleRowsCount;
		}

		private void RefreshScrollBar()
		{
			int num = 0;
			foreach (Item item in Items)
			{
				if (item.Visible)
				{
					num++;
				}
			}
			m_scrollBar.Visible = (num > VisibleRowsCount);
			m_scrollBar.Init(num, VisibleRowsCount);
			Vector2 vector = base.Size * new Vector2(0.5f, -0.5f);
			MyGuiBorderThickness scrollbarMargin = m_styleDef.ScrollbarMargin;
			Vector2 position = new Vector2(vector.X - (scrollbarMargin.Right + m_scrollBar.Size.X), vector.Y + scrollbarMargin.Top);
			m_scrollBar.Layout(position, base.Size.Y - scrollbarMargin.VerticalSum);
		}

		public void ScrollToolbarToTop()
		{
			m_scrollBar.SetPage(0f);
		}

		public void ScrollToFirstSelection()
		{
			if (Items.Count == 0 || SelectedItems.Count == 0)
			{
				return;
			}
			Item item = SelectedItems[0];
			int num = -1;
			for (int i = 0; i < Items.Count; i++)
			{
				Item item2 = Items[i];
				if (item == item2)
				{
					num = i;
					break;
				}
			}
			if (m_visibleRowIndexOffset > num || num >= m_visibleRowIndexOffset + m_visibleRows)
			{
				SetScrollPosition(num);
			}
		}

		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
			{
				foreach (object oldItem in e.OldItems)
				{
					if (SelectedItems.Contains((Item)oldItem))
					{
						SelectedItems.Remove((Item)oldItem);
					}
				}
				if (this.ItemsSelected != null)
				{
					this.ItemsSelected(this);
				}
			}
			RefreshScrollBar();
		}

		private void verticalScrollBar_ValueChanged(MyScrollbar scrollbar)
		{
			int num = (int)scrollbar.Value;
			int num2 = -1;
			for (int i = 0; i < Items.Count; i++)
			{
				if (Items[i].Visible)
				{
					num2++;
				}
				if (num2 == num)
				{
					num = i;
					break;
				}
			}
			m_visibleRowIndexOffset = num;
		}

		private bool IsValidIndex(int idx)
		{
			if (0 <= idx)
			{
				return idx < Items.Count;
			}
			return false;
		}

		public void SelectAllVisible()
		{
			SelectedItems.Clear();
			foreach (Item item in Items)
			{
				if (item.Visible)
				{
					SelectedItems.Add(item);
				}
			}
			if (this.ItemsSelected != null)
			{
				this.ItemsSelected(this);
			}
		}

		public void ChangeSelection(List<bool> states)
		{
			SelectedItems.Clear();
			int num = 0;
			foreach (Item item in Items)
			{
				if (num >= states.Count)
				{
					break;
				}
				if (states[num])
				{
					SelectedItems.Add(item);
				}
				num++;
			}
			if (this.ItemsSelected != null)
			{
				this.ItemsSelected(this);
			}
		}

		public void ClearItems()
		{
			Items.Clear();
		}

		public float GetScrollPosition()
		{
			return m_scrollBar.Value;
		}

		public void SetScrollPosition(float position)
		{
			m_scrollBar.Value = position;
		}

		public void StoreSituation()
		{
			m_StoredSelectedItems.Clear();
			m_StoredTopmostSelectedItem = null;
			m_StoredMouseOverItem = null;
			m_StoredItemOnTop = null;
			m_StoredTopmostSelectedPosition = m_visibleRows;
			foreach (Item selectedItem in SelectedItems)
			{
				m_StoredSelectedItems.Add(selectedItem);
				int num = Items.IndexOf(SelectedItems[0]);
				if (num < m_StoredTopmostSelectedPosition && num >= m_visibleRowIndexOffset)
				{
					m_StoredTopmostSelectedPosition = num;
					m_StoredTopmostSelectedItem = selectedItem;
				}
			}
			m_StoredMouseOverItem = m_mouseOverItem;
			int num2 = 0;
			if (m_mouseOverItem != null)
			{
				foreach (Item item in Items)
				{
					if (m_mouseOverItem == item)
					{
						m_StoredMouseOverPosition = num2;
						break;
					}
					num2++;
				}
			}
			if (FirstVisibleRow < Items.Count)
			{
				m_StoredItemOnTop = Items[FirstVisibleRow];
			}
			m_StoredScrollbarValue = m_scrollBar.Value;
		}

		private bool CompareItems(Item item1, Item item2, bool compareUserData, bool compareText)
		{
			if (compareUserData && compareText)
			{
				if (item1.UserData == item2.UserData && item1.Text.CompareTo(item2.Text) == 0)
				{
					return true;
				}
				return false;
			}
			if (compareUserData && item1.UserData == item2.UserData)
			{
				return true;
			}
			if (compareText && item1.Text.CompareTo(item2.Text) == 0)
			{
				return true;
			}
			return false;
		}

		public void RestoreSituation(bool compareUserData, bool compareText)
		{
			SelectedItems.Clear();
			foreach (Item storedSelectedItem in m_StoredSelectedItems)
			{
				foreach (Item item in Items)
				{
					if (CompareItems(storedSelectedItem, item, compareUserData, compareText))
					{
						SelectedItems.Add(item);
						break;
					}
				}
			}
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			foreach (Item item2 in Items)
			{
				if (m_StoredMouseOverItem != null && CompareItems(item2, m_StoredMouseOverItem, compareUserData, compareText))
				{
					m_scrollBar.Value = m_StoredScrollbarValue + (float)num3 - (float)m_StoredMouseOverPosition;
					return;
				}
				if (m_StoredTopmostSelectedItem != null && CompareItems(item2, m_StoredTopmostSelectedItem, compareUserData, compareText))
				{
					num = num3;
				}
				if (m_StoredItemOnTop != null && CompareItems(item2, m_StoredItemOnTop, compareUserData, compareText))
				{
					num2 = num3;
				}
				num3++;
			}
			if (m_StoredTopmostSelectedPosition != m_visibleRows)
			{
				m_scrollBar.Value = m_StoredScrollbarValue + (float)num - (float)m_StoredTopmostSelectedPosition;
			}
			else if (num2 != -1)
			{
				m_scrollBar.Value = num2;
			}
			else
			{
				m_scrollBar.Value = m_StoredScrollbarValue;
			}
		}

		public void ApplyStyle(StyleDefinition style)
		{
			m_useCustomStyle = true;
			m_customStyle = style;
			RefreshVisualStyle();
		}

		private RectangleF GetRowRectangle(List<Item> selectedItems)
		{
			if (selectedItems.Count > 0)
			{
				int num = Items.FindIndex((Item x) => x == selectedItems[0]);
				int num2 = 0;
				for (int i = m_visibleRowIndexOffset; i < num; i++)
				{
					if (Items[i].Visible)
					{
						num2++;
					}
				}
				return new RectangleF(GetPositionAbsoluteTopLeft() + new Vector2(0f, (float)num2 * ItemSize.Y), new Vector2(base.Size.X, ItemSize.Y));
			}
			return base.Rectangle;
		}

		internal override void OnFocusChanged(bool focus)
		{
			if (focus && SelectedItem == null && Items.Count > 0)
			{
				int num = (int)((m_entryPoint.Y - m_itemsRectangle.Position.Y) / ItemSize.Y);
				num++;
				int num2 = 0;
				for (int i = m_visibleRowIndexOffset; i < Items.Count; i++)
				{
					if (Items[i].Visible)
					{
						num2++;
					}
					if (num2 == num)
					{
						break;
					}
				}
				if (num >= Items.Count)
				{
					num = Items.Count - 1;
				}
				SelectedItem = Items[num];
				this.ItemsSelected?.Invoke(this);
				ScrollToFirstSelection();
			}
			base.OnFocusChanged(focus);
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			if (currentFocusControl == this)
			{
				if (page || Items.Count == 0)
				{
					return null;
				}
				int i = 0;
				int num = 0;
				int num2 = int.MaxValue;
				if (SelectedItems.Count > 0)
				{
					foreach (Item item in SelectedItems)
					{
						int num3 = Items.FindIndex((Item x) => x == item);
						if (num3 > num)
						{
							num = num3;
						}
						if (num3 < num2)
						{
							num2 = num3;
						}
					}
				}
				else
				{
					num = -1;
					num2 = Items.Count;
				}
				switch (direction)
				{
				case MyDirection.Down:
					for (i = num + 1; i < Items.Count && !Items[i].Visible; i++)
					{
					}
					break;
				case MyDirection.Up:
					i = num2 - 1;
					while (i > 0 && !Items[i].Visible)
					{
						i--;
					}
					break;
				case MyDirection.Right:
					return null;
				case MyDirection.Left:
					return null;
				}
				if (i < 0 || i >= Items.Count)
				{
					return null;
				}
				SelectedItem = Items[i];
				this.ItemsSelected?.Invoke(this);
				this.ItemClicked?.Invoke(this);
				ScrollToFirstSelection();
			}
			else
			{
				m_entryPoint = currentFocusControl.FocusRectangle.Center;
				m_entryDirection = direction;
			}
			return this;
		}
	}
}
