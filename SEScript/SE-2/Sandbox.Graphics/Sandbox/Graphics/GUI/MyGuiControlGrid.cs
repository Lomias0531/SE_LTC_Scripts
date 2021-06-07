using System;
using System.Collections.Generic;
using System.Text;
using VRage.Audio;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlGrid))]
	public class MyGuiControlGrid : MyGuiControlBase
	{
		public struct EventArgs
		{
			public int RowIndex;

			public int ColumnIndex;

			public int ItemIndex;

			public MySharedButtonsEnum Button;
		}

		private static MyGuiStyleDefinition[] m_styles;

		public const int INVALID_INDEX = -1;

		public Vector4 ItemBackgroundColorMask = Vector4.One;

		private Vector2 m_doubleClickFirstPosition;

		private int? m_doubleClickStarted;

		private bool m_isItemDraggingLeft;

		private bool m_isItemDraggingRight;

		private Vector2 m_mouseDragStartPosition;

		protected RectangleF m_itemsRectangle;

		protected Vector2 m_itemStep;

		private readonly List<MyGuiGridItem> m_items;

		private MyToolTips m_emptyItemToolTip;

		private EventArgs? m_singleClickEvents;

		private EventArgs? m_itemClicked;

		private int? m_lastClick;

		public Dictionary<int, Color> ModalItems;

		private int m_columnsCount;

		private int m_rowsCount;

		private int m_maxItemCount = int.MaxValue;

		private int m_mouseOverIndex;

		private int? m_selectedIndex;

		private MyGuiControlGridStyleEnum m_visualStyle;

		protected MyGuiStyleDefinition m_styleDef;

		private float m_itemTextScale;

		private float m_itemTextScaleWithLanguage;

		public string EmptyItemIcon;

		public bool SelectionEnabled = true;

		public bool ShowEmptySlots = true;

		private Vector2 m_entryPoint;

		private MyDirection m_entryDirection;

		public bool EnableSelectEmptyCell
		{
			get;
			set;
		}

		public List<MyGuiGridItem> Items => m_items;

		public Vector2 ItemStep => m_itemStep;

		public int ColumnsCount
		{
			get
			{
				return m_columnsCount;
			}
			set
			{
				if (m_columnsCount != value)
				{
					m_columnsCount = value;
					RefreshInternals();
				}
			}
		}

		public int RowsCount
		{
			get
			{
				return m_rowsCount;
			}
			set
			{
				if (m_rowsCount != value)
				{
					m_rowsCount = value;
					RefreshInternals();
				}
			}
		}

		public int MaxItemCount
		{
			get
			{
				return m_maxItemCount;
			}
			set
			{
				if (m_maxItemCount != value)
				{
					m_maxItemCount = value;
					RefreshInternals();
				}
			}
		}

		public Vector2 ItemSize
		{
			get;
			private set;
		}

		public int MouseOverIndex
		{
			get
			{
				return m_mouseOverIndex;
			}
			private set
			{
				if (value != m_mouseOverIndex)
				{
					m_mouseOverIndex = value;
					if (this.MouseOverIndexChanged != null)
					{
						EventArgs args = default(EventArgs);
						PrepareEventArgs(ref args, value);
						this.MouseOverIndexChanged(this, args);
					}
				}
			}
		}

		public MyGuiGridItem MouseOverItem => TryGetItemAt(MouseOverIndex);

		public int? SelectedIndex
		{
			get
			{
				return m_selectedIndex;
			}
			set
			{
				try
				{
					if (m_selectedIndex != value)
					{
						m_selectedIndex = value;
						if (value.HasValue && this.ItemSelected != null)
						{
							MakeEventArgs(out EventArgs args, value.Value, MySharedButtonsEnum.None);
							this.ItemSelected(this, args);
						}
					}
				}
				finally
				{
				}
			}
		}

		public MyGuiGridItem SelectedItem
		{
			get
			{
				if (!SelectedIndex.HasValue)
				{
					return null;
				}
				return TryGetItemAt(SelectedIndex.Value);
			}
		}

		public MyGuiControlGridStyleEnum VisualStyle
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

		public float ItemTextScale
		{
			get
			{
				return m_itemTextScale;
			}
			private set
			{
				m_itemTextScale = value;
				ItemTextScaleWithLanguage = value * MyGuiManager.LanguageTextScale;
			}
		}

		public float ItemTextScaleWithLanguage
		{
			get
			{
				return m_itemTextScaleWithLanguage;
			}
			private set
			{
				m_itemTextScaleWithLanguage = value;
			}
		}

		public override RectangleF FocusRectangle
		{
			get
			{
				if (!SelectedIndex.HasValue)
				{
					return base.Rectangle;
				}
				return GetItemRectangle(SelectedIndex.Value);
			}
		}

		public event Action<MyGuiControlGrid, EventArgs> ItemChanged;

		public event Action<MyGuiControlGrid, EventArgs> ItemClicked;

		public event Action<MyGuiControlGrid, EventArgs> ItemReleased;

		public event Action<MyGuiControlGrid> ReleasedWithoutItem;

		public event Action<MyGuiControlGrid, EventArgs> ItemClickedWithoutDoubleClick;

		public event Action<MyGuiControlGrid, EventArgs> ItemDoubleClicked;

		public event Action<MyGuiControlGrid, EventArgs> ItemDragged;

		public event Action<MyGuiControlGrid, EventArgs> ItemSelected;

		public event Action<MyGuiControlGrid, EventArgs> MouseOverIndexChanged;

		public event Action<MyGuiControlGrid, EventArgs> ItemAccepted;

		static MyGuiControlGrid()
		{
			MyGuiBorderThickness itemPadding = new MyGuiBorderThickness(4f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			MyGuiBorderThickness itemMargin = new MyGuiBorderThickness(2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			m_styles = new MyGuiStyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlGridStyleEnum>() + 1];
			m_styles[0] = new MyGuiStyleDefinition
			{
				BackgroundTexture = new MyGuiCompositeTexture
				{
					LeftTop = new MyGuiSizedTexture(MyGuiConstants.TEXTURE_SCREEN_BACKGROUND)
				},
				BackgroundPaddingSize = MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.PaddingSizeGui,
				ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM,
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemPadding = itemPadding
			};
			m_styles[1] = new MyGuiStyleDefinition
			{
				ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM,
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				SizeOverride = MyGuiConstants.TEXTURE_GRID_ITEM.SizeGui * new Vector2(10f, 1f),
				ItemMargin = itemMargin,
				ItemPadding = itemPadding,
				ItemTextScale = 0.6f,
				FitSizeToItems = true
			};
			m_styles[2] = new MyGuiStyleDefinition
			{
				ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM_SMALL,
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				SizeOverride = MyGuiConstants.TEXTURE_GRID_ITEM_SMALL.SizeGui * new Vector2(10f, 1f),
				ItemMargin = itemMargin,
				ItemPadding = itemPadding,
				ItemTextScale = 0.6f,
				FitSizeToItems = true
			};
			m_styles[3] = new MyGuiStyleDefinition
			{
				BackgroundTexture = new MyGuiCompositeTexture
				{
					Center = new MyGuiSizedTexture(MyGuiConstants.TEXTURE_SCREEN_TOOLS_BACKGROUND_BLOCKS)
				},
				BackgroundPaddingSize = MyGuiConstants.TEXTURE_SCREEN_TOOLS_BACKGROUND_BLOCKS.PaddingSizeGui,
				ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM,
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemMargin = itemMargin,
				ItemPadding = itemPadding
			};
			m_styles[4] = new MyGuiStyleDefinition
			{
				BackgroundTexture = new MyGuiCompositeTexture
				{
					LeftTop = new MyGuiSizedTexture(MyGuiConstants.TEXTURE_SCREEN_TOOLS_BACKGROUND_WEAPONS)
				},
				BackgroundPaddingSize = MyGuiConstants.TEXTURE_SCREEN_TOOLS_BACKGROUND_WEAPONS.PaddingSizeGui,
				ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM,
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemMargin = itemMargin,
				ItemPadding = itemPadding,
				FitSizeToItems = true
			};
			m_styles[5] = new MyGuiStyleDefinition
			{
				ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM,
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemMargin = itemMargin,
				ItemPadding = itemPadding,
				SizeOverride = new Vector2(593f, 91f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				ItemTextScale = 0.640000045f,
				BorderEnabled = true,
				BorderColor = new Vector4(0.37f, 0.58f, 0.68f, 1f),
				FitSizeToItems = true,
				ContentPadding = new MyGuiBorderThickness(1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y)
			};
			m_styles[6] = new MyGuiStyleDefinition
			{
				ItemTexture = MyGuiConstants.TEXTURE_GRID_ITEM_TINY,
				ItemFontNormal = "Blue",
				ItemFontHighlight = "White",
				ItemMargin = itemMargin,
				ItemPadding = itemPadding,
				SizeOverride = new Vector2(593f, 91f) / MyGuiConstants.GUI_OPTIMAL_SIZE,
				ItemTextScale = 0.640000045f,
				FitSizeToItems = false,
				ContentPadding = new MyGuiBorderThickness(1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y)
			};
		}

		public static MyGuiStyleDefinition GetVisualStyle(MyGuiControlGridStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public MyGuiControlGrid()
			: base(Vector2.Zero, new Vector2(0.05f, 0.05f), MyGuiConstants.LISTBOX_BACKGROUND_COLOR, null, null, isActiveControl: true, canHaveFocus: true)
		{
			m_items = new List<MyGuiGridItem>();
			RefreshVisualStyle();
			RowsCount = 1;
			ColumnsCount = 1;
			base.Name = "Grid";
			base.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			EnableSelectEmptyCell = true;
			ItemBackgroundColorMask = base.ColorMask;
			base.CanFocusChildren = true;
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlGrid myObjectBuilder_GuiControlGrid = (MyObjectBuilder_GuiControlGrid)objectBuilder;
			VisualStyle = myObjectBuilder_GuiControlGrid.VisualStyle;
			RowsCount = myObjectBuilder_GuiControlGrid.DisplayRowsCount;
			ColumnsCount = myObjectBuilder_GuiControlGrid.DisplayColumnsCount;
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlGrid obj = (MyObjectBuilder_GuiControlGrid)base.GetObjectBuilder();
			obj.VisualStyle = VisualStyle;
			obj.DisplayRowsCount = RowsCount;
			obj.DisplayColumnsCount = ColumnsCount;
			return obj;
		}

		public void Add(MyGuiGridItem item, int startingRow = 0)
		{
			if (!TryFindEmptyIndex(out int emptyIdx, startingRow))
			{
				emptyIdx = m_items.Count;
				m_items.Add(null);
			}
			m_items[emptyIdx] = item;
			if (this.ItemChanged != null)
			{
				EventArgs args = default(EventArgs);
				PrepareEventArgs(ref args, emptyIdx);
				this.ItemChanged(this, args);
			}
			float num = (float)(emptyIdx / m_columnsCount) + 1f;
			RowsCount = Math.Max(RowsCount, (int)num);
		}

		public MyGuiGridItem GetItemAt(int index)
		{
			if (m_items == null || m_items.Count == 0)
			{
				return null;
			}
			if (!IsValidIndex(index))
			{
				index = MathHelper.Clamp(index, 0, m_items.Count - 1);
			}
			return m_items[index];
		}

		public bool IsValidIndex(int row, int col)
		{
			return IsValidIndex(ComputeIndex(row, col));
		}

		public bool IsValidIndex(int index)
		{
			if (ModalItems != null && ModalItems.Count > 0 && !ModalItems.ContainsKey(index))
			{
				return false;
			}
			if (0 <= index && index < m_items.Count)
			{
				return index < m_maxItemCount;
			}
			return false;
		}

		public MyGuiGridItem GetItemAt(int rowIdx, int colIdx)
		{
			if (m_items == null || m_items.Count == 0)
			{
				return null;
			}
			return m_items[ComputeIndex(rowIdx, colIdx)];
		}

		public void SetItemAt(int index, MyGuiGridItem item)
		{
			m_items[index] = item;
			if (this.ItemChanged != null)
			{
				EventArgs args = default(EventArgs);
				PrepareEventArgs(ref args, index);
				this.ItemChanged(this, args);
			}
			float num = index / m_columnsCount + 1;
			RowsCount = Math.Max(RowsCount, (int)num);
		}

		public void SetItemAt(int rowIdx, int colIdx, MyGuiGridItem item)
		{
			int num = ComputeIndex(rowIdx, colIdx);
			if (num >= 0 && num < m_items.Count)
			{
				m_items[num] = item;
				if (this.ItemChanged != null)
				{
					EventArgs args = default(EventArgs);
					PrepareEventArgs(ref args, num, rowIdx, colIdx);
					this.ItemChanged(this, args);
				}
				RowsCount = Math.Max(RowsCount, rowIdx + 1);
			}
		}

		public void blinkSlot(int? slot)
		{
			if (slot.HasValue)
			{
				m_items[slot.Value].startBlinking();
			}
		}

		public void SetItemsToDefault()
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				m_items[i] = null;
			}
			RowsCount = 0;
		}

		public new virtual void Clear()
		{
			m_items.Clear();
			m_selectedIndex = null;
			RowsCount = 0;
		}

		public void TrimEmptyItems()
		{
			int num = m_items.Count - 1;
			while (m_items.Count > 0 && m_items[num] == null)
			{
				m_items.RemoveAt(num);
				num--;
			}
			if (SelectedIndex.HasValue && !IsValidIndex(SelectedIndex.Value))
			{
				SelectedIndex = null;
			}
			float num2 = num / m_columnsCount + 1;
			RowsCount = Math.Max(RowsCount, (int)num2);
		}

		public MyGuiGridItem TryGetItemAt(int rowIdx, int colIdx)
		{
			return TryGetItemAt(ComputeIndex(rowIdx, colIdx));
		}

		public MyGuiGridItem TryGetItemAt(int itemIdx)
		{
			if (!m_items.IsValidIndex(itemIdx))
			{
				return null;
			}
			return m_items[itemIdx];
		}

		public void SelectLastItem()
		{
			SelectedIndex = ((m_items.Count > 0) ? new int?(m_items.Count - 1) : null);
		}

		public void AddRows(int numberOfRows)
		{
			if (numberOfRows <= 0 || ColumnsCount <= 0)
			{
				return;
			}
			while (m_items.Count % ColumnsCount != 0)
			{
				m_items.Add(null);
			}
			for (int i = 0; i < numberOfRows; i++)
			{
				for (int j = 0; j < ColumnsCount; j++)
				{
					m_items.Add(null);
				}
			}
			RecalculateRowsCount();
		}

		public void RecalculateRowsCount()
		{
			float num = m_items.Count / m_columnsCount;
			RowsCount = Math.Max(RowsCount, (int)num);
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			RefreshItemsRectangle();
			DrawItemBackgrounds(backgroundTransitionAlpha);
			DrawItems(transitionAlpha);
			DrawItemTexts(transitionAlpha);
		}

		public void SetCustomStyleDefinition(MyGuiStyleDefinition styleDef)
		{
			m_styleDef = styleDef;
			ItemSize = m_styleDef.ItemTexture.SizeGui;
			m_itemStep = ItemSize + m_styleDef.ItemMargin.MarginStep;
			ItemTextScale = m_styleDef.ItemTextScale;
			BackgroundTexture = m_styleDef.BackgroundTexture;
			BorderEnabled = m_styleDef.BorderEnabled;
			BorderColor = m_styleDef.BorderColor;
			if (!m_styleDef.FitSizeToItems)
			{
				base.Size = (m_styleDef.SizeOverride ?? BackgroundTexture.MinSizeGui);
			}
			RefreshInternals();
		}

		internal override void OnFocusChanged(bool focus)
		{
			if (focus)
			{
				int col = MathHelper.Clamp((int)((m_entryPoint.X - m_itemsRectangle.Position.X) / m_itemStep.X), 0, m_columnsCount - 1);
				int row = MathHelper.Clamp((int)((m_entryPoint.Y - m_itemsRectangle.Position.Y) / m_itemStep.Y), 0, m_rowsCount - 1);
				switch (m_entryDirection)
				{
				case MyDirection.None:
					if (!SelectedIndex.HasValue)
					{
						SelectedIndex = 0;
					}
					break;
				case MyDirection.Down:
					SelectedIndex = ComputeIndex(0, col);
					break;
				case MyDirection.Up:
					SelectedIndex = ComputeIndex(m_rowsCount - 1, col);
					break;
				case MyDirection.Right:
					SelectedIndex = ComputeIndex(row, 0);
					break;
				case MyDirection.Left:
					SelectedIndex = ComputeIndex(row, m_columnsCount - 1);
					break;
				}
				if (SelectedIndex >= Items.Count)
				{
					SelectedIndex = Items.Count - 1;
				}
				ShowToolTip();
			}
			else
			{
				m_entryDirection = MyDirection.None;
			}
			base.OnFocusChanged(focus);
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			if (currentFocusControl == this)
			{
				if (page)
				{
					return null;
				}
				if (!SelectedIndex.HasValue)
				{
					SelectedIndex = 0;
				}
				int num = ComputeColumn(SelectedIndex.Value);
				int num2 = ComputeRow(SelectedIndex.Value);
				switch (direction)
				{
				case MyDirection.Down:
					num2++;
					break;
				case MyDirection.Up:
					num2--;
					break;
				case MyDirection.Right:
					num++;
					break;
				case MyDirection.Left:
					num--;
					break;
				}
				if (num2 < 0 || num2 >= m_rowsCount || num < 0 || num >= m_columnsCount)
				{
					return null;
				}
				SelectedIndex = ComputeIndex(num2, num);
				if (SelectedIndex.HasValue && SelectedIndex >= Items.Count)
				{
					SelectedIndex = null;
					return null;
				}
				ShowToolTip();
				base.Owner?.OnFocusChanged(this, focus: true);
			}
			else
			{
				m_entryPoint = currentFocusControl.FocusRectangle.Center;
				m_entryDirection = direction;
			}
			return this;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase captureInput = base.HandleInput();
			if (captureInput != null)
			{
				MouseOverIndex = -1;
				return captureInput;
			}
			if (!base.Enabled)
			{
				return captureInput;
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT))
			{
				AcceptItem();
			}
			if (!base.IsMouseOver)
			{
				TryTriggerSingleClickEvent();
				return captureInput;
			}
			int mouseOverIndex = MouseOverIndex;
			MouseOverIndex = (base.IsMouseOver ? ComputeIndex(MyGuiManager.MouseCursorPosition) : (-1));
			if (mouseOverIndex != MouseOverIndex && base.Enabled && MouseOverIndex != -1)
			{
				MyGuiSoundManager.PlaySound(GuiSounds.MouseOver);
			}
			HandleNewMousePress(ref captureInput);
			HandleMouseDrag(ref captureInput, MySharedButtonsEnum.Primary, ref m_isItemDraggingLeft);
			HandleMouseDrag(ref captureInput, MySharedButtonsEnum.Secondary, ref m_isItemDraggingRight);
			if (m_singleClickEvents.HasValue && m_singleClickEvents.Value.Button == MySharedButtonsEnum.Secondary)
			{
				TryTriggerSingleClickEvent();
			}
			if (m_doubleClickStarted.HasValue && (float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) >= 500f)
			{
				m_doubleClickStarted = null;
				TryTriggerSingleClickEvent();
			}
			return captureInput;
		}

		private void TryTriggerSingleClickEvent()
		{
			if (m_singleClickEvents.HasValue)
			{
				if (this.ItemClickedWithoutDoubleClick != null)
				{
					this.ItemClickedWithoutDoubleClick(this, m_singleClickEvents.Value);
				}
				m_singleClickEvents = null;
			}
		}

		public override void Update()
		{
			base.Update();
			if (!base.IsMouseOver)
			{
				MouseOverIndex = -1;
			}
		}

		public override void ShowToolTip()
		{
			MyToolTips toolTip = m_toolTip;
			int num = ComputeIndex(MyGuiManager.MouseCursorPosition);
			if (num == -1)
			{
				num = (SelectedIndex.HasValue ? SelectedIndex.Value : (-1));
			}
			if (num != -1)
			{
				MyGuiGridItem myGuiGridItem = TryGetItemAt(num);
				if (myGuiGridItem != null)
				{
					m_toolTip = myGuiGridItem.ToolTip;
				}
				else
				{
					m_toolTip = m_emptyItemToolTip;
				}
			}
			base.ShowToolTip();
			m_toolTip = toolTip;
		}

		public int ComputeIndex(int row, int col)
		{
			return row * ColumnsCount + col;
		}

		public void SetEmptyItemToolTip(string toolTip)
		{
			if (toolTip == null)
			{
				m_emptyItemToolTip = null;
			}
			else
			{
				m_emptyItemToolTip = new MyToolTips(toolTip);
			}
		}

		private int ComputeColumn(int itemIndex)
		{
			return itemIndex % ColumnsCount;
		}

		private int ComputeIndex(Vector2 normalizedPosition)
		{
			if (!m_itemsRectangle.Contains(normalizedPosition))
			{
				return -1;
			}
			Vector2I vector2I = default(Vector2I);
			vector2I.X = (int)((normalizedPosition.X - m_itemsRectangle.Position.X) / m_itemStep.X);
			vector2I.Y = (int)((normalizedPosition.Y - m_itemsRectangle.Position.Y) / m_itemStep.Y);
			int num = vector2I.Y * ColumnsCount + vector2I.X;
			if (!IsValidCellIndex(num))
			{
				return -1;
			}
			return num;
		}

		private int ComputeRow(int itemIndex)
		{
			return itemIndex / ColumnsCount;
		}

		private bool IsValidCellIndex(int itemIndex)
		{
			if (0 <= itemIndex)
			{
				return itemIndex < m_maxItemCount;
			}
			return false;
		}

		protected override void OnColorMaskChanged()
		{
			base.OnColorMaskChanged();
			ItemBackgroundColorMask = base.ColorMask;
		}

		private RectangleF GetItemRectangle(int index)
		{
			_ = m_styleDef.ItemPadding;
			int num = ComputeColumn(index);
			int num2 = ComputeRow(index);
			return new RectangleF(m_itemsRectangle.Position + m_itemStep * new Vector2(num, num2), ItemSize);
		}

		private void DebugDraw()
		{
			MyGuiManager.DrawBorders(new Vector2(m_itemsRectangle.X, m_itemsRectangle.Y), new Vector2(m_itemsRectangle.Width, m_itemsRectangle.Height), Color.White, 1);
			if (IsValidIndex(MouseOverIndex))
			{
				RectangleF itemRectangle = GetItemRectangle(MouseOverIndex);
				MyGuiBorderThickness itemPadding = m_styleDef.ItemPadding;
				itemRectangle.Position += itemPadding.TopLeftOffset;
				itemRectangle.Size -= itemPadding.SizeChange;
				MyGuiManager.DrawBorders(itemRectangle.Position, itemRectangle.Size, Color.White, 1);
			}
		}

		private void DrawItemBackgrounds(float transitionAlpha)
		{
			string normal = m_styleDef.ItemTexture.Normal;
			string highlight = m_styleDef.ItemTexture.Highlight;
			int num = Math.Min(m_maxItemCount, RowsCount * ColumnsCount);
			for (int i = 0; i < num; i++)
			{
				int num2 = i / ColumnsCount;
				int num3 = i % ColumnsCount;
				Vector2 vector = m_itemsRectangle.Position + m_itemStep * new Vector2(num3, num2);
				MyGuiGridItem myGuiGridItem = TryGetItemAt(i);
				bool flag = base.Enabled && (myGuiGridItem?.Enabled ?? true);
				bool flag2 = false;
				float num4 = 1f;
				if (myGuiGridItem != null)
				{
					flag2 = (MyGuiManager.TotalTimeInMilliseconds - myGuiGridItem.blinkCount <= 400);
					if (flag2)
					{
						num4 = myGuiGridItem.blinkingTransparency();
					}
				}
				bool flag3 = flag && (((MouseOverIndex == -1 || IsValidIndex(MouseOverIndex)) && (i == MouseOverIndex || flag2)) || i == SelectedIndex);
				Vector4 sourceColorMask = myGuiGridItem?.BackgroundColor ?? ItemBackgroundColorMask;
				if (ModalItems != null && ModalItems.Count > 0)
				{
					if (!ModalItems.ContainsKey(i))
					{
						continue;
					}
					sourceColorMask = ModalItems[i];
					MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.Draw(vector, ItemSize, Color.Yellow);
				}
				if (ShowEmptySlots)
				{
					MyGuiManager.DrawSpriteBatch(flag3 ? highlight : normal, vector, ItemSize, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, flag, transitionAlpha * num4), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				}
				else if (myGuiGridItem != null)
				{
					MyGuiManager.DrawSpriteBatch(flag3 ? highlight : normal, vector, ItemSize, MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, flag, transitionAlpha * num4), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				}
			}
		}

		private void DrawItems(float transitionAlpha)
		{
			int num = Math.Min(m_maxItemCount, RowsCount * ColumnsCount);
			for (int i = 0; i < num; i++)
			{
				int num2 = i / ColumnsCount;
				int num3 = i % ColumnsCount;
				MyGuiGridItem myGuiGridItem = TryGetItemAt(i);
				Vector2 vector = m_itemsRectangle.Position + m_itemStep * new Vector2(num3, num2);
				Vector4 colorMask = base.ColorMask;
				bool flag = true;
				if (ModalItems != null && ModalItems.Count > 0 && !ModalItems.ContainsKey(i))
				{
					continue;
				}
				Vector2 value = (myGuiGridItem != null && myGuiGridItem.SubIconOffset.HasValue) ? myGuiGridItem.SubIconOffset.Value : new Vector2(8f / 9f, 4f / 9f);
				Vector2 normalizedCoord = m_itemsRectangle.Position + m_itemStep * (new Vector2(num3, num2) + value);
				Vector2 normalizedCoord2 = m_itemsRectangle.Position + m_itemStep * (new Vector2(num3, num2) + Vector2.One);
				if (myGuiGridItem != null && myGuiGridItem.Icons != null)
				{
					bool enabled = base.Enabled && myGuiGridItem.Enabled && flag;
					for (int j = 0; j < myGuiGridItem.Icons.Length; j++)
					{
						MyGuiManager.DrawSpriteBatch(myGuiGridItem.Icons[j], vector + 0.5f * ItemSize, ItemSize * myGuiGridItem.IconScale, MyGuiControlBase.ApplyColorMaskModifiers(colorMask * myGuiGridItem.IconColorMask * myGuiGridItem.MainIconColorMask, enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, useFullClientArea: false, waitTillLoaded: false);
					}
					if (!string.IsNullOrWhiteSpace(myGuiGridItem.SubIcon))
					{
						MyGuiManager.DrawSpriteBatch(myGuiGridItem.SubIcon, normalizedCoord, ItemSize / 3f, MyGuiControlBase.ApplyColorMaskModifiers(colorMask * myGuiGridItem.IconColorMask, enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, useFullClientArea: false, waitTillLoaded: false);
					}
					if (!string.IsNullOrWhiteSpace(myGuiGridItem.SubIcon2))
					{
						MyGuiManager.DrawSpriteBatch(myGuiGridItem.SubIcon2, normalizedCoord2, ItemSize / 3.5f, MyGuiControlBase.ApplyColorMaskModifiers(colorMask * myGuiGridItem.IconColorMask, enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, useFullClientArea: false, waitTillLoaded: false);
					}
					if (myGuiGridItem.OverlayPercent != 0f)
					{
						MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", vector, ItemSize * new Vector2(myGuiGridItem.OverlayPercent, 1f), MyGuiControlBase.ApplyColorMaskModifiers(colorMask * myGuiGridItem.OverlayColorMask, enabled, transitionAlpha * 0.5f), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false);
					}
				}
				else if (EmptyItemIcon != null)
				{
					bool enabled2 = base.Enabled && flag;
					MyGuiManager.DrawSpriteBatch(EmptyItemIcon, vector, ItemSize, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, enabled2, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				}
				if (myGuiGridItem != null)
				{
					foreach (KeyValuePair<MyGuiDrawAlignEnum, ColoredIcon> item in myGuiGridItem.IconsByAlign)
					{
						if (!string.IsNullOrEmpty(item.Value.Icon))
						{
							MyGuiManager.DrawSpriteBatch(item.Value.Icon, vector + m_itemStep * new Vector2(0.055555556f, 0.111111112f), ItemSize / 3f, MyGuiControlBase.ApplyColorMaskModifiers(item.Value.Color, enabled: true, transitionAlpha), item.Key);
						}
					}
				}
			}
		}

		private void DrawItemTexts(float transitionAlpha)
		{
			MyGuiBorderThickness itemPadding = m_styleDef.ItemPadding;
			string itemFontNormal = m_styleDef.ItemFontNormal;
			string itemFontHighlight = m_styleDef.ItemFontHighlight;
			int num = Math.Min(m_maxItemCount, RowsCount * ColumnsCount);
			for (int i = 0; i < num; i++)
			{
				int num2 = i / ColumnsCount;
				int num3 = i % ColumnsCount;
				MyGuiGridItem myGuiGridItem = TryGetItemAt(i);
				if (myGuiGridItem != null)
				{
					foreach (KeyValuePair<MyGuiDrawAlignEnum, StringBuilder> item in myGuiGridItem.TextsByAlign)
					{
						Vector2 value = m_itemsRectangle.Position + m_itemStep * new Vector2(num3, num2);
						RectangleF rect = new RectangleF(value + itemPadding.TopLeftOffset, ItemSize - itemPadding.SizeChange);
						Vector2 coordAlignedFromRectangle = MyUtils.GetCoordAlignedFromRectangle(ref rect, item.Key);
						bool enabled = base.Enabled && myGuiGridItem.Enabled;
						MyGuiManager.DrawString((i == MouseOverIndex || i == SelectedIndex) ? itemFontHighlight : itemFontNormal, item.Value, coordAlignedFromRectangle, ItemTextScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, enabled, transitionAlpha), item.Key, useFullClientArea: false, rect.Size.X);
					}
				}
			}
		}

		private void HandleMouseDrag(ref MyGuiControlBase captureInput, MySharedButtonsEnum button, ref bool isDragging)
		{
			if (MyInput.Static.IsNewButtonPressed(button))
			{
				isDragging = true;
				m_mouseDragStartPosition = MyGuiManager.MouseCursorPosition;
			}
			else if (MyInput.Static.IsButtonPressed(button))
			{
				if (!isDragging || SelectedItem == null)
				{
					return;
				}
				if ((MyGuiManager.MouseCursorPosition - m_mouseDragStartPosition).Length() != 0f)
				{
					if (this.ItemDragged != null)
					{
						int num = ComputeIndex(MyGuiManager.MouseCursorPosition);
						if (IsValidIndex(num) && GetItemAt(num) != null)
						{
							MakeEventArgs(out EventArgs args, num, button);
							this.ItemDragged(this, args);
						}
					}
					isDragging = false;
				}
				captureInput = this;
			}
			else
			{
				isDragging = false;
			}
		}

		private void DoubleClickItem()
		{
			if (SelectedIndex.HasValue && TryGetItemAt(SelectedIndex.Value) != null && this.ItemDoubleClicked != null)
			{
				m_singleClickEvents = null;
				MakeEventArgs(out EventArgs args, SelectedIndex.Value, MySharedButtonsEnum.Primary);
				this.ItemDoubleClicked(this, args);
				MyGuiSoundManager.PlaySound(GuiSounds.Item);
			}
		}

		private void AcceptItem()
		{
			if (base.HasFocus && SelectedIndex.HasValue && TryGetItemAt(SelectedIndex.Value) != null && this.ItemAccepted != null)
			{
				m_singleClickEvents = null;
				MakeEventArgs(out EventArgs args, SelectedIndex.Value, MySharedButtonsEnum.Primary);
				this.ItemAccepted(this, args);
				MyGuiSoundManager.PlaySound(GuiSounds.Item);
			}
		}

		private void HandleNewMousePress(ref MyGuiControlBase captureInput)
		{
			bool flag = m_itemsRectangle.Contains(MyGuiManager.MouseCursorPosition);
			if (MyInput.Static.IsNewPrimaryButtonReleased() || MyInput.Static.IsNewSecondaryButtonReleased())
			{
				if (flag)
				{
					int? mouseOverIndex = ComputeIndex(MyGuiManager.MouseCursorPosition);
					if (!IsValidIndex(mouseOverIndex.Value))
					{
						mouseOverIndex = null;
					}
					SelectMouseOverItem(mouseOverIndex);
					if (SelectedIndex.HasValue && m_itemClicked.HasValue && m_lastClick.HasValue && mouseOverIndex.HasValue)
					{
						if ((float)(MyGuiManager.TotalTimeInMilliseconds - m_lastClick.Value) < 500f && m_itemClicked.Value.ItemIndex == mouseOverIndex.Value)
						{
							captureInput = this;
							MySharedButtonsEnum button = MySharedButtonsEnum.None;
							if (MyInput.Static.IsNewPrimaryButtonReleased())
							{
								button = MySharedButtonsEnum.Primary;
							}
							else if (MyInput.Static.IsNewSecondaryButtonReleased())
							{
								button = MySharedButtonsEnum.Secondary;
							}
							MakeEventArgs(out EventArgs args, SelectedIndex.Value, button);
							this.ItemReleased.InvokeIfNotNull(this, args);
						}
					}
					else
					{
						captureInput = this;
						this.ReleasedWithoutItem.InvokeIfNotNull(this);
					}
				}
				m_itemClicked = null;
				m_lastClick = null;
			}
			if (MyInput.Static.IsAnyNewMouseOrJoystickPressed() && flag)
			{
				m_lastClick = MyGuiManager.TotalTimeInMilliseconds;
				int? mouseOverIndex2 = ComputeIndex(MyGuiManager.MouseCursorPosition);
				if (!IsValidIndex(mouseOverIndex2.Value))
				{
					mouseOverIndex2 = null;
				}
				SelectMouseOverItem(mouseOverIndex2);
				captureInput = this;
				if (SelectedIndex.HasValue && (this.ItemClicked != null || this.ItemClickedWithoutDoubleClick != null))
				{
					MySharedButtonsEnum button2 = MySharedButtonsEnum.None;
					if (MyInput.Static.IsNewPrimaryButtonPressed())
					{
						button2 = MySharedButtonsEnum.Primary;
					}
					else if (MyInput.Static.IsNewSecondaryButtonPressed())
					{
						button2 = MySharedButtonsEnum.Secondary;
					}
					else if (MyInput.Static.IsNewMiddleMousePressed())
					{
						button2 = MySharedButtonsEnum.Ternary;
					}
					MakeEventArgs(out EventArgs args2, SelectedIndex.Value, button2);
					this.ItemClicked?.Invoke(this, args2);
					m_singleClickEvents = args2;
					m_itemClicked = args2;
					if (MyInput.Static.IsAnyCtrlKeyPressed() || MyInput.Static.IsAnyShiftKeyPressed())
					{
						MyGuiSoundManager.PlaySound(GuiSounds.Item);
					}
				}
			}
			if (MyInput.Static.IsNewPrimaryButtonPressed() && flag)
			{
				if (!m_doubleClickStarted.HasValue)
				{
					m_doubleClickStarted = MyGuiManager.TotalTimeInMilliseconds;
					m_doubleClickFirstPosition = MyGuiManager.MouseCursorPosition;
				}
				else if ((float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) <= 500f && (m_doubleClickFirstPosition - MyGuiManager.MouseCursorPosition).Length() <= 0.005f)
				{
					DoubleClickItem();
					m_doubleClickStarted = null;
					captureInput = this;
				}
			}
		}

		private void SelectMouseOverItem(int? mouseOverIndex)
		{
			if (SelectionEnabled && mouseOverIndex.HasValue)
			{
				if (EnableSelectEmptyCell)
				{
					SelectedIndex = mouseOverIndex.Value;
				}
				else if (TryGetItemAt(mouseOverIndex.Value) != null)
				{
					SelectedIndex = mouseOverIndex.Value;
				}
			}
			else
			{
				SelectedIndex = null;
			}
		}

		private void MakeEventArgs(out EventArgs args, int itemIndex, MySharedButtonsEnum button)
		{
			args.ItemIndex = itemIndex;
			args.RowIndex = ComputeRow(itemIndex);
			args.ColumnIndex = ComputeColumn(itemIndex);
			args.Button = button;
		}

		private void RefreshInternals()
		{
			if (m_styleDef.FitSizeToItems)
			{
				base.Size = m_styleDef.ContentPadding.SizeChange + m_styleDef.ItemMargin.TopLeftOffset + m_itemStep * new Vector2(ColumnsCount, RowsCount);
			}
			int num = Math.Min(m_maxItemCount, RowsCount * ColumnsCount);
			while (m_items.Count < num)
			{
				m_items.Add(null);
			}
			RefreshItemsRectangle();
		}

		private void RefreshItemsRectangle()
		{
			m_itemsRectangle.Position = GetPositionAbsoluteTopLeft() + m_styleDef.BackgroundPaddingSize + m_styleDef.ContentPadding.TopLeftOffset + m_styleDef.ItemMargin.TopLeftOffset;
			m_itemsRectangle.Size = m_itemStep * new Vector2(ColumnsCount, RowsCount);
		}

		private void RefreshVisualStyle()
		{
			if (VisualStyle != MyGuiControlGridStyleEnum.Custom)
			{
				m_styleDef = GetVisualStyle(VisualStyle);
			}
			BackgroundTexture = m_styleDef.BackgroundTexture;
			ItemSize = m_styleDef.ItemTexture.SizeGui;
			m_itemStep = ItemSize + m_styleDef.ItemMargin.MarginStep;
			ItemTextScale = m_styleDef.ItemTextScale;
			BorderEnabled = m_styleDef.BorderEnabled;
			BorderColor = m_styleDef.BorderColor;
			if (!m_styleDef.FitSizeToItems)
			{
				base.Size = (m_styleDef.SizeOverride ?? BackgroundTexture.MinSizeGui);
			}
			RefreshInternals();
		}

		private void PrepareEventArgs(ref EventArgs args, int itemIndex, int? rowIdx = null, int? columnIdx = null)
		{
			args.ItemIndex = itemIndex;
			args.ColumnIndex = (columnIdx ?? ComputeColumn(itemIndex));
			args.RowIndex = (rowIdx ?? ComputeRow(itemIndex));
		}

		private bool TryFindEmptyIndex(out int emptyIdx, int startingRow)
		{
			for (int i = startingRow * m_columnsCount; i < m_items.Count; i++)
			{
				if (m_items[i] == null)
				{
					emptyIdx = i;
					return true;
				}
			}
			emptyIdx = 0;
			return false;
		}

		public int GetItemsCount()
		{
			return m_items.Count;
		}
	}
}
