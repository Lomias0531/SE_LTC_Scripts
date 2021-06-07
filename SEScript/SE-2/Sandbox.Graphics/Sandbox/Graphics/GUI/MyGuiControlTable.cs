using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlTable : MyGuiControlBase
	{
		public class StyleDefinition
		{
			public string HeaderFontHighlight;

			public string HeaderFontNormal;

			public string HeaderTextureHighlight;

			public MyGuiBorderThickness Padding;

			public string RowFontHighlight;

			public string RowFontNormal;

			public float RowHeight;

			public string RowTextureHighlight;

			public float TextScale;

			public MyGuiBorderThickness ScrollbarMargin;

			public MyGuiCompositeTexture Texture;
		}

		public delegate bool EqualUserData(object first, object second);

		public struct EventArgs
		{
			public int RowIndex;

			public MyMouseButtonsEnum MouseButton;
		}

		public class Cell
		{
			public readonly StringBuilder Text;

			public readonly object UserData;

			public readonly MyToolTips ToolTip;

			public MyGuiHighlightTexture? Icon;

			public readonly MyGuiDrawAlignEnum IconOriginAlign;

			public Color? TextColor;

			public Thickness Margin;

			public MyGuiControlBase Control;

			public Row Row;

			private StringBuilder text;

			private StringBuilder toolTip;

			public Cell(string text = null, object userData = null, string toolTip = null, Color? textColor = null, MyGuiHighlightTexture? icon = null, MyGuiDrawAlignEnum iconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				if (text != null)
				{
					Text = new StringBuilder().Append(text);
				}
				if (toolTip != null)
				{
					ToolTip = new MyToolTips(toolTip);
				}
				UserData = userData;
				Icon = icon;
				IconOriginAlign = iconOriginAlign;
				TextColor = textColor;
				Margin = new Thickness(0f);
			}

			public Cell(StringBuilder text, object userData = null, string toolTip = null, Color? textColor = null, MyGuiHighlightTexture? icon = null, MyGuiDrawAlignEnum iconOriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)
			{
				if (text != null)
				{
					Text = new StringBuilder().AppendStringBuilder(text);
				}
				if (toolTip != null)
				{
					ToolTip = new MyToolTips(toolTip);
				}
				UserData = userData;
				Icon = icon;
				IconOriginAlign = iconOriginAlign;
				TextColor = textColor;
				Margin = new Thickness(0f);
			}

			public virtual void Update()
			{
			}
		}

		public class Row
		{
			internal readonly List<Cell> Cells;

			public readonly object UserData;

			public bool IsGlobalSortEnabled
			{
				get;
				set;
			}

			public Row(object userData = null)
			{
				IsGlobalSortEnabled = true;
				UserData = userData;
				Cells = new List<Cell>();
			}

			public void AddCell(Cell cell)
			{
				Cells.Add(cell);
				cell.Row = this;
			}

			public Cell GetCell(int cell)
			{
				return Cells[cell];
			}

			public void Update()
			{
				foreach (Cell cell in Cells)
				{
					cell.Update();
				}
			}
		}

		public enum SortStateEnum
		{
			Unsorted,
			Ascending,
			Descending
		}

		private class ColumnMetaData
		{
			public StringBuilder Name;

			public float Width;

			public Comparison<Cell> AscendingComparison;

			public SortStateEnum SortState;

			public MyGuiDrawAlignEnum TextAlign;

			public MyGuiDrawAlignEnum HeaderTextAlign;

			public Thickness Margin;

			public ColumnMetaData()
			{
				Name = new StringBuilder();
				SortState = SortStateEnum.Unsorted;
				TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				HeaderTextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				Margin = new Thickness(0.01f);
			}
		}

		private static StyleDefinition[] m_styles;

		private MyGuiControls m_controls;

		private List<ColumnMetaData> m_columnsMetaData;

		private List<Row> m_rows;

		private Vector2 m_doubleClickFirstPosition;

		private int? m_doubleClickStarted;

		private bool m_mouseOverHeader;

		private int? m_mouseOverColumnIndex;

		private int? m_mouseOverRowIndex;

		private RectangleF m_headerArea;

		private RectangleF m_rowsArea;

		private StyleDefinition m_styleDef;

		private MyVScrollbar m_scrollBar;

		protected int m_visibleRowIndexOffset;

		private int m_lastSortedColumnIdx;

		private float m_textScale;

		private float m_textScaleWithLanguage;

		private int m_sortColumn = -1;

		private SortStateEnum? m_sortColumnState;

		private bool m_headerVisible = true;

		private int m_columnsCount = 1;

		private int? m_selectedRowIndex;

		private int m_visibleRows = 1;

		private MyGuiControlTableStyleEnum m_visualStyle;

		private Vector2 m_entryPoint;

		private MyDirection m_entryDirection;

		public MyGuiControls Controls => m_controls;

		public MyVScrollbar ScrollBar => m_scrollBar;

		public bool HeaderVisible
		{
			get
			{
				return m_headerVisible;
			}
			set
			{
				m_headerVisible = value;
				RefreshInternals();
			}
		}

		public bool ColumnLinesVisible
		{
			get;
			set;
		}

		public bool RowLinesVisible
		{
			get;
			set;
		}

		public int ColumnsCount
		{
			get
			{
				return m_columnsCount;
			}
			set
			{
				m_columnsCount = value;
				RefreshInternals();
			}
		}

		public int? SelectedRowIndex
		{
			get
			{
				return m_selectedRowIndex;
			}
			set
			{
				m_selectedRowIndex = value;
			}
		}

		public Row SelectedRow
		{
			get
			{
				if (IsValidRowIndex(SelectedRowIndex))
				{
					return m_rows[SelectedRowIndex.Value];
				}
				return null;
			}
			set
			{
				int num = m_rows.IndexOf(value);
				if (num >= 0)
				{
					m_selectedRowIndex = num;
				}
			}
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

		public bool IgnoreFirstRowForSort
		{
			get;
			set;
		}

		public float RowHeight
		{
			get;
			private set;
		}

		public MyGuiControlTableStyleEnum VisualStyle
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

		public float TextScale
		{
			get
			{
				return m_textScale;
			}
			private set
			{
				m_textScale = value;
				TextScaleWithLanguage = value * MyGuiManager.LanguageTextScale;
			}
		}

		public float TextScaleWithLanguage
		{
			get
			{
				return m_textScaleWithLanguage;
			}
			private set
			{
				m_textScaleWithLanguage = value;
			}
		}

		public int RowsCount => m_rows.Count;

		public override RectangleF FocusRectangle => GetRowRectangle(SelectedRowIndex);

		public event Action<MyGuiControlTable, EventArgs> ItemDoubleClicked;

		public event Action<MyGuiControlTable, EventArgs> ItemRightClicked;

		public event Action<MyGuiControlTable, EventArgs> ItemSelected;

		public event Action<MyGuiControlTable, EventArgs> ItemConfirmed;

		public event Action<MyGuiControlTable, int> ColumnClicked;

		public event Action<Row> ItemMouseOver;

		static MyGuiControlTable()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlTableStyleEnum>() + 1];
			StyleDefinition[] styles = m_styles;
			StyleDefinition obj = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				RowTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				HeaderTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_light.dds",
				RowFontNormal = "Blue",
				RowFontHighlight = "White",
				HeaderFontNormal = "White",
				HeaderFontHighlight = "White",
				TextScale = 0.8f,
				RowHeight = 40f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			MyGuiBorderThickness myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj.Padding = myGuiBorderThickness;
			myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj.ScrollbarMargin = myGuiBorderThickness;
			styles[0] = obj;
			StyleDefinition[] styles2 = m_styles;
			StyleDefinition obj2 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST,
				RowTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_dark.dds",
				HeaderTextureHighlight = "Textures\\GUI\\Controls\\item_highlight_light.dds",
				RowFontNormal = "White",
				RowFontHighlight = "White",
				HeaderFontNormal = "White",
				HeaderFontHighlight = "White",
				TextScale = 0.8f,
				RowHeight = 40f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj2.Padding = myGuiBorderThickness;
			myGuiBorderThickness = new MyGuiBorderThickness
			{
				Left = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 5f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj2.ScrollbarMargin = myGuiBorderThickness;
			styles2[1] = obj2;
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlTableStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public MyGuiControlTable()
			: base(null, null, null, null, null, isActiveControl: true, canHaveFocus: true)
		{
			m_scrollBar = new MyVScrollbar(this);
			m_scrollBar.ValueChanged += verticalScrollBar_ValueChanged;
			m_rows = new List<Row>();
			m_columnsMetaData = new List<ColumnMetaData>();
			VisualStyle = MyGuiControlTableStyleEnum.Default;
			m_controls = new MyGuiControls(null);
			base.Name = "Table";
			base.CanFocusChildren = true;
			BorderHighlightEnabled = true;
		}

		public void Add(Row row)
		{
			m_rows.Add(row);
			RefreshScrollbar();
		}

		public void Insert(int index, Row row)
		{
			m_rows.Insert(index, row);
			RefreshScrollbar();
		}

		public new void Clear()
		{
			foreach (Row row in m_rows)
			{
				foreach (Cell cell in row.Cells)
				{
					if (cell.Control != null)
					{
						cell.Control.OnRemoving();
						cell.Control.Clear();
					}
				}
			}
			m_rows.Clear();
			SelectedRowIndex = null;
			RefreshScrollbar();
		}

		public Row GetRow(int index)
		{
			return m_rows[index];
		}

		public Row Find(Predicate<Row> match)
		{
			return m_rows.Find(match);
		}

		public int FindIndex(Predicate<Row> match)
		{
			return m_rows.FindIndex(match);
		}

		public int FindIndexByUserData(ref object data, EqualUserData equals)
		{
			int num = -1;
			foreach (Row row in m_rows)
			{
				num++;
				if (row.UserData == null)
				{
					if (data == null)
					{
						return num;
					}
				}
				else if (equals == null)
				{
					if (row.UserData == data)
					{
						return num;
					}
				}
				else if (equals(row.UserData, data))
				{
					return num;
				}
			}
			return -1;
		}

		public void Remove(Row row)
		{
			int num = m_rows.IndexOf(row);
			if (num != -1)
			{
				m_rows.RemoveAt(num);
				if (SelectedRowIndex.HasValue && SelectedRowIndex.Value != num && SelectedRowIndex.Value > num)
				{
					SelectedRowIndex = SelectedRowIndex.Value - 1;
				}
			}
		}

		public void Remove(Predicate<Row> match)
		{
			int num = m_rows.FindIndex(match);
			if (num != -1)
			{
				m_rows.RemoveAt(num);
				if (SelectedRowIndex.HasValue && SelectedRowIndex.Value != num && SelectedRowIndex.Value > num)
				{
					SelectedRowIndex = SelectedRowIndex.Value - 1;
				}
			}
		}

		public void RemoveSelectedRow()
		{
			if (SelectedRowIndex.HasValue && SelectedRowIndex.Value < m_rows.Count)
			{
				m_rows.RemoveAt(SelectedRowIndex.Value);
				if (!IsValidRowIndex(SelectedRowIndex.Value))
				{
					SelectedRowIndex = null;
				}
				RefreshScrollbar();
			}
		}

		public void MoveSelectedRowUp()
		{
			if (SelectedRow != null && IsValidRowIndex(SelectedRowIndex - 1))
			{
				Row value = m_rows[SelectedRowIndex.Value - 1];
				m_rows[SelectedRowIndex.Value - 1] = m_rows[SelectedRowIndex.Value];
				m_rows[SelectedRowIndex.Value] = value;
				SelectedRowIndex--;
			}
		}

		public void MoveSelectedRowDown()
		{
			if (SelectedRow != null && IsValidRowIndex(SelectedRowIndex + 1))
			{
				Row value = m_rows[SelectedRowIndex.Value + 1];
				m_rows[SelectedRowIndex.Value + 1] = m_rows[SelectedRowIndex.Value];
				m_rows[SelectedRowIndex.Value] = value;
				SelectedRowIndex++;
			}
		}

		public void MoveSelectedRowTop()
		{
			if (SelectedRow != null)
			{
				Row selectedRow = SelectedRow;
				RemoveSelectedRow();
				m_rows.Insert(0, selectedRow);
				SelectedRowIndex = 0;
			}
		}

		public void MoveSelectedRowBottom()
		{
			if (SelectedRow != null)
			{
				Row selectedRow = SelectedRow;
				RemoveSelectedRow();
				m_rows.Add(selectedRow);
				SelectedRowIndex = RowsCount - 1;
			}
		}

		public void MoveToNextRow()
		{
			if (m_rows.Count == 0)
			{
				return;
			}
			if (!SelectedRowIndex.HasValue)
			{
				SelectedRowIndex = 0;
				return;
			}
			int val = SelectedRowIndex.Value + 1;
			val = Math.Min(val, m_rows.Count - 1);
			if (val != SelectedRowIndex.Value)
			{
				SelectedRowIndex = val;
				this.ItemSelected.InvokeIfNotNull(this, new EventArgs
				{
					RowIndex = SelectedRowIndex.Value,
					MouseButton = MyMouseButtonsEnum.Left
				});
				ScrollToSelection();
			}
		}

		public void MoveToPreviousRow()
		{
			if (m_rows.Count == 0)
			{
				return;
			}
			if (!SelectedRowIndex.HasValue)
			{
				SelectedRowIndex = 0;
				return;
			}
			int val = SelectedRowIndex.Value - 1;
			val = Math.Max(val, 0);
			if (val != SelectedRowIndex.Value)
			{
				SelectedRowIndex = val;
				this.ItemSelected.InvokeIfNotNull(this, new EventArgs
				{
					RowIndex = SelectedRowIndex.Value,
					MouseButton = MyMouseButtonsEnum.Left
				});
				ScrollToSelection();
			}
		}

		public void SetColumnName(int colIdx, StringBuilder name)
		{
			m_columnsMetaData[colIdx].Name.Clear().AppendStringBuilder(name);
		}

		public void SetColumnComparison(int colIdx, Comparison<Cell> ascendingComparison)
		{
			m_columnsMetaData[colIdx].AscendingComparison = ascendingComparison;
		}

		public void SetCustomColumnWidths(float[] p)
		{
			for (int i = 0; i < ColumnsCount; i++)
			{
				m_columnsMetaData[i].Width = p[i];
			}
		}

		public void SetColumnAlign(int colIdx, MyGuiDrawAlignEnum align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
		{
			m_columnsMetaData[colIdx].TextAlign = align;
		}

		public void SetHeaderColumnAlign(int colIdx, MyGuiDrawAlignEnum align = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER)
		{
			m_columnsMetaData[colIdx].HeaderTextAlign = align;
		}

		public void SetHeaderColumnMargin(int colIdx, Thickness margin)
		{
			m_columnsMetaData[colIdx].Margin = margin;
		}

		public void ScrollToSelection()
		{
			if (SelectedRow == null)
			{
				m_visibleRowIndexOffset = 0;
				return;
			}
			int value = SelectedRowIndex.Value;
			if (value > m_visibleRowIndexOffset + VisibleRowsCount - 1)
			{
				m_scrollBar.Value = value - VisibleRowsCount + 1;
			}
			if (value < m_visibleRowIndexOffset)
			{
				m_scrollBar.Value = value;
			}
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			_ = RowHeight;
			_ = VisibleRowsCount;
			m_styleDef.Texture.Draw(positionAbsoluteTopLeft, base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, backgroundTransitionAlpha));
			if (HeaderVisible)
			{
				DrawHeader(transitionAlpha);
			}
			DrawRows(transitionAlpha);
			DrawGridLines(transitionAlpha);
			m_scrollBar.Draw(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
			Vector2 positionAbsoluteTopRight = GetPositionAbsoluteTopRight();
			positionAbsoluteTopRight.X -= m_styleDef.ScrollbarMargin.HorizontalSum + m_scrollBar.Size.X;
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Controls\\scrollable_list_line.dds", positionAbsoluteTopRight, new Vector2(0.0012f, base.Size.Y), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
		}

		private void DrawGridLines(float alpha)
		{
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			if (ColumnLinesVisible)
			{
				Vector2 value = positionAbsoluteTopLeft + m_headerArea.Position;
				for (int i = 0; i < m_columnsMetaData.Count - 1; i++)
				{
					ColumnMetaData columnMetaData = m_columnsMetaData[i];
					Vector2 normalizedSize = new Vector2(columnMetaData.Width * m_rowsArea.Size.X, m_headerArea.Height);
					Vector2 screenCoordinateFromNormalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(value + new Vector2(normalizedSize.X, 0f));
					Vector2 screenSizeFromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(normalizedSize);
					MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", (int)Math.Round(screenCoordinateFromNormalizedCoordinate.X), (int)Math.Round(screenCoordinateFromNormalizedCoordinate.Y), 1, (int)(screenSizeFromNormalizedSize.Y * (float)(VisibleRowsCount + 1)), new Color(0.5f, 0.5f, 0.5f, 0.2f * alpha));
					value.X += columnMetaData.Width * m_headerArea.Width;
				}
			}
			if (RowLinesVisible)
			{
				Vector2 value2 = positionAbsoluteTopLeft;
				value2.Y += RowHeight;
				Vector2 value3 = new Vector2(0f, RowHeight);
				Vector2 screenSizeFromNormalizedSize2 = MyGuiManager.GetScreenSizeFromNormalizedSize(m_rowsArea.Size);
				for (int j = 1; j < VisibleRowsCount; j++)
				{
					Vector2 screenCoordinateFromNormalizedCoordinate2 = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(value2 + value3);
					MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", (int)Math.Round(screenCoordinateFromNormalizedCoordinate2.X), (int)Math.Round(screenCoordinateFromNormalizedCoordinate2.Y), (int)screenSizeFromNormalizedSize2.X, 1, new Color(0.5f, 0.5f, 0.5f, 0.2f * alpha));
					value2.Y += RowHeight;
				}
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase captureInput = base.HandleInput();
			if (captureInput != null)
			{
				return captureInput;
			}
			if (!base.Enabled)
			{
				return null;
			}
			if (m_scrollBar != null && m_scrollBar.HandleInput())
			{
				captureInput = this;
			}
			HandleMouseOver();
			HandleNewMousePress(ref captureInput);
			using (List<MyGuiControlBase>.Enumerator enumerator = Controls.GetVisibleControls().GetEnumerator())
			{
				while (enumerator.MoveNext() && enumerator.Current.HandleInput() == null)
				{
				}
			}
			if (m_doubleClickStarted.HasValue && (float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) >= 500f)
			{
				m_doubleClickStarted = null;
			}
			if (!base.HasFocus)
			{
				return captureInput;
			}
			if (SelectedRowIndex.HasValue && (MyInput.Static.IsNewKeyPressed(MyKeys.Enter) || MyInput.Static.IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01)) && this.ItemConfirmed != null)
			{
				captureInput = this;
				this.ItemConfirmed(this, new EventArgs
				{
					RowIndex = SelectedRowIndex.Value
				});
			}
			return captureInput;
		}

		public override void Update()
		{
			base.Update();
			if (!base.IsMouseOver)
			{
				m_mouseOverColumnIndex = null;
				m_mouseOverRowIndex = null;
				m_mouseOverHeader = false;
			}
		}

		protected override void OnOriginAlignChanged()
		{
			base.OnOriginAlignChanged();
			RefreshInternals();
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			RefreshInternals();
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			m_scrollBar.HasHighlight = base.HasHighlight;
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
		}

		public override void ShowToolTip()
		{
			MyToolTips toolTip = m_toolTip;
			if (m_mouseOverRowIndex.HasValue && m_rows.IsValidIndex(m_mouseOverRowIndex.Value))
			{
				Row row = m_rows[m_mouseOverRowIndex.Value];
				if (row.Cells.IsValidIndex(m_mouseOverColumnIndex.Value))
				{
					Cell cell = row.Cells[m_mouseOverColumnIndex.Value];
					if (cell.ToolTip != null)
					{
						m_toolTip = cell.ToolTip;
					}
				}
				if (this.ItemMouseOver != null)
				{
					this.ItemMouseOver(row);
				}
			}
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				visibleControl.ShowToolTip();
			}
			base.ShowToolTip();
			m_toolTip = toolTip;
		}

		private int ComputeColumnIndexFromPosition(Vector2 normalizedPosition)
		{
			normalizedPosition -= GetPositionAbsoluteTopLeft();
			float num = (normalizedPosition.X - m_rowsArea.Position.X) / m_rowsArea.Size.X;
			int i;
			for (i = 0; i < m_columnsMetaData.Count && !(num < m_columnsMetaData[i].Width); i++)
			{
				num -= m_columnsMetaData[i].Width;
			}
			return i;
		}

		private int ComputeRowIndexFromPosition(Vector2 normalizedPosition)
		{
			normalizedPosition -= GetPositionAbsoluteTopLeft();
			return (int)((normalizedPosition.Y - m_rowsArea.Position.Y) / RowHeight) + m_visibleRowIndexOffset;
		}

		private void DebugDraw()
		{
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_headerArea.Position, m_headerArea.Size, Color.Cyan, 1);
			MyGuiManager.DrawBorders(positionAbsoluteTopLeft + m_rowsArea.Position, m_rowsArea.Size, Color.White, 1);
			Vector2 topLeftPosition = positionAbsoluteTopLeft + m_headerArea.Position;
			for (int i = 0; i < m_columnsMetaData.Count; i++)
			{
				ColumnMetaData columnMetaData = m_columnsMetaData[i];
				MyGuiManager.DrawBorders(size: new Vector2(columnMetaData.Width * m_rowsArea.Size.X, m_headerArea.Height), topLeftPosition: topLeftPosition, color: Color.Yellow, borderSize: 1);
				topLeftPosition.X += columnMetaData.Width * m_headerArea.Width;
			}
			m_scrollBar.DebugDraw();
		}

		private void DrawHeader(float transitionAlpha)
		{
			Vector2 value = GetPositionAbsoluteTopLeft() + m_headerArea.Position;
			MyGuiManager.DrawSpriteBatch(m_styleDef.HeaderTextureHighlight, new Vector2(value.X + 0.001f, value.Y), new Vector2(m_headerArea.Size.X - 0.001f, m_headerArea.Size.Y), Color.White, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			for (int i = 0; i < m_columnsMetaData.Count; i++)
			{
				ColumnMetaData columnMetaData = m_columnsMetaData[i];
				string font = m_styleDef.HeaderFontNormal;
				if (m_mouseOverColumnIndex.HasValue && m_mouseOverColumnIndex.Value == i)
				{
					font = m_styleDef.HeaderFontHighlight;
				}
				Vector2 vector = new Vector2(columnMetaData.Width * m_rowsArea.Size.X, m_headerArea.Height);
				Vector2 coordAlignedFromCenter = MyUtils.GetCoordAlignedFromCenter(value + 0.5f * vector + new Vector2(columnMetaData.Margin.Left, 0f), vector, columnMetaData.HeaderTextAlign);
				MyGuiManager.DrawString(font, columnMetaData.Name, coordAlignedFromCenter, TextScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), columnMetaData.HeaderTextAlign, useFullClientArea: false, vector.X - columnMetaData.Margin.Left - columnMetaData.Margin.Right);
				value.X += columnMetaData.Width * m_headerArea.Width;
			}
		}

		private void DrawRows(float transitionAlpha)
		{
			Vector2 vector = GetPositionAbsoluteTopLeft() + m_rowsArea.Position;
			Vector2 normalizedSizeFromScreenSize = MyGuiManager.GetNormalizedSizeFromScreenSize(new Vector2(1f, 1f));
			for (int i = 0; i < VisibleRowsCount; i++)
			{
				int num = i + m_visibleRowIndexOffset;
				if (num >= m_rows.Count)
				{
					break;
				}
				if (num < 0)
				{
					continue;
				}
				bool num2 = (m_mouseOverRowIndex.HasValue && m_mouseOverRowIndex.Value == num) || (SelectedRowIndex.HasValue && SelectedRowIndex.Value == num);
				string font = m_styleDef.RowFontNormal;
				if (num2)
				{
					Vector2 normalizedCoord = vector;
					normalizedCoord.X += normalizedSizeFromScreenSize.X;
					MyGuiManager.DrawSpriteBatch(m_styleDef.RowTextureHighlight, normalizedCoord, new Vector2(m_rowsArea.Size.X - normalizedSizeFromScreenSize.X, RowHeight), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					font = m_styleDef.RowFontHighlight;
				}
				Row row = m_rows[num];
				if (row != null)
				{
					Vector2 vector2 = vector;
					for (int j = 0; j < ColumnsCount && j < row.Cells.Count; j++)
					{
						Cell cell = row.Cells[j];
						ColumnMetaData columnMetaData = m_columnsMetaData[j];
						Vector2 vector3 = new Vector2(columnMetaData.Width * m_rowsArea.Size.X, RowHeight);
						if (cell != null && cell.Control != null)
						{
							MyUtils.GetCoordAlignedFromTopLeft(vector2, vector3, cell.IconOriginAlign);
							cell.Control.Position = vector2 + vector3 * 0.5f;
							cell.Control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
							cell.Control.Draw(transitionAlpha, transitionAlpha);
						}
						else if (cell != null && cell.Text != null)
						{
							float num3 = 0f;
							float num4 = columnMetaData.Margin.Left + cell.Margin.Left;
							if (cell.Icon.HasValue)
							{
								Vector2 coordAlignedFromTopLeft = MyUtils.GetCoordAlignedFromTopLeft(vector2, vector3, cell.IconOriginAlign);
								MyGuiHighlightTexture value = cell.Icon.Value;
								Vector2 vector4 = Vector2.Min(value.SizeGui, vector3) / value.SizeGui;
								float num5 = Math.Min(vector4.X, vector4.Y);
								num3 = value.SizeGui.X;
								MyGuiManager.DrawSpriteBatch(base.HasHighlight ? value.Highlight : value.Normal, coordAlignedFromTopLeft + new Vector2(num4, 0f), value.SizeGui * num5, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), cell.IconOriginAlign);
								if (num5.IsValid())
								{
									num4 *= 2f;
								}
							}
							Vector2 coordAlignedFromCenter = MyUtils.GetCoordAlignedFromCenter(vector2 + 0.5f * vector3 + new Vector2(num4, 0f), vector3, columnMetaData.TextAlign);
							coordAlignedFromCenter.X += num3;
							MyGuiManager.DrawString(font, cell.Text, coordAlignedFromCenter, TextScaleWithLanguage, (!cell.TextColor.HasValue) ? new Color?(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha)) : (cell.TextColor * transitionAlpha), columnMetaData.TextAlign, useFullClientArea: false, vector3.X - num4 - columnMetaData.Margin.Right - cell.Margin.Right);
						}
						vector2.X += vector3.X;
					}
				}
				vector.Y += RowHeight;
			}
		}

		private void HandleMouseOver()
		{
			if (m_rowsArea.Contains(MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft()))
			{
				m_mouseOverRowIndex = ComputeRowIndexFromPosition(MyGuiManager.MouseCursorPosition);
				m_mouseOverColumnIndex = ComputeColumnIndexFromPosition(MyGuiManager.MouseCursorPosition);
				m_mouseOverHeader = false;
			}
			else if (m_headerArea.Contains(MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft()))
			{
				m_mouseOverRowIndex = null;
				m_mouseOverColumnIndex = ComputeColumnIndexFromPosition(MyGuiManager.MouseCursorPosition);
				m_mouseOverHeader = true;
			}
			else
			{
				m_mouseOverRowIndex = null;
				m_mouseOverColumnIndex = null;
				m_mouseOverHeader = false;
			}
		}

		public MyGuiControlBase GetInnerControlsFromCurrentCell(int cellIndex)
		{
			if (!m_selectedRowIndex.HasValue || m_selectedRowIndex.Value < 0 || m_selectedRowIndex.Value > m_rows.Count)
			{
				return null;
			}
			Row row = m_rows[m_selectedRowIndex.Value];
			if (cellIndex < 0 || cellIndex >= row.Cells.Count)
			{
				return null;
			}
			return row.Cells[cellIndex].Control;
		}

		private void HandleNewMousePress(ref MyGuiControlBase captureInput)
		{
			bool flag = m_rowsArea.Contains(MyGuiManager.MouseCursorPosition - GetPositionAbsoluteTopLeft());
			MyMouseButtonsEnum mouseButton = MyMouseButtonsEnum.None;
			if (MyInput.Static.IsNewPrimaryButtonPressed())
			{
				mouseButton = MyMouseButtonsEnum.Left;
			}
			else if (MyInput.Static.IsNewSecondaryButtonPressed())
			{
				mouseButton = MyMouseButtonsEnum.Right;
			}
			else if (MyInput.Static.IsNewMiddleMousePressed())
			{
				mouseButton = MyMouseButtonsEnum.Middle;
			}
			else if (MyInput.Static.IsNewXButton1MousePressed())
			{
				mouseButton = MyMouseButtonsEnum.XButton1;
			}
			else if (MyInput.Static.IsNewXButton2MousePressed())
			{
				mouseButton = MyMouseButtonsEnum.XButton2;
			}
			EventArgs arg;
			if (MyInput.Static.IsAnyNewMouseOrJoystickPressed() && flag)
			{
				SelectedRowIndex = ComputeRowIndexFromPosition(MyGuiManager.MouseCursorPosition);
				captureInput = this;
				if (this.ItemSelected != null)
				{
					Action<MyGuiControlTable, EventArgs> itemSelected = this.ItemSelected;
					arg = new EventArgs
					{
						RowIndex = SelectedRowIndex.Value,
						MouseButton = mouseButton
					};
					itemSelected.InvokeIfNotNull(this, arg);
					MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
				}
			}
			if (!MyInput.Static.IsNewPrimaryButtonPressed())
			{
				return;
			}
			if (m_mouseOverHeader)
			{
				SortByColumn(m_mouseOverColumnIndex.Value);
				if (this.ColumnClicked != null)
				{
					this.ColumnClicked(this, m_mouseOverColumnIndex.Value);
				}
			}
			else
			{
				if (!flag)
				{
					return;
				}
				if (!m_doubleClickStarted.HasValue)
				{
					m_doubleClickStarted = MyGuiManager.TotalTimeInMilliseconds;
					m_doubleClickFirstPosition = MyGuiManager.MouseCursorPosition;
				}
				else if ((float)(MyGuiManager.TotalTimeInMilliseconds - m_doubleClickStarted.Value) <= 500f && (m_doubleClickFirstPosition - MyGuiManager.MouseCursorPosition).Length() <= 0.005f)
				{
					if (this.ItemDoubleClicked != null && SelectedRowIndex.HasValue)
					{
						Action<MyGuiControlTable, EventArgs> itemDoubleClicked = this.ItemDoubleClicked;
						arg = new EventArgs
						{
							RowIndex = SelectedRowIndex.Value,
							MouseButton = mouseButton
						};
						itemDoubleClicked(this, arg);
					}
					m_doubleClickStarted = null;
					captureInput = this;
				}
			}
		}

		public void Sort(bool switchSort = true)
		{
			if (m_sortColumn != -1)
			{
				SortByColumn(m_sortColumn, null, switchSort);
			}
		}

		public void SortByColumn(int columnIdx, SortStateEnum? sortState = null, bool switchSort = true)
		{
			columnIdx = MathHelper.Clamp(columnIdx, 0, m_columnsMetaData.Count - 1);
			m_sortColumn = columnIdx;
			m_sortColumnState = (sortState.HasValue ? new SortStateEnum?(sortState.Value) : m_sortColumnState);
			ColumnMetaData columnMetaData = m_columnsMetaData[columnIdx];
			SortStateEnum sortState2 = columnMetaData.SortState;
			m_columnsMetaData[m_lastSortedColumnIdx].SortState = SortStateEnum.Unsorted;
			Comparison<Cell> comparison = columnMetaData.AscendingComparison;
			if (comparison != null)
			{
				SortStateEnum sortStateEnum = sortState2;
				if (switchSort)
				{
					sortStateEnum = ((sortState2 != SortStateEnum.Ascending) ? SortStateEnum.Ascending : SortStateEnum.Descending);
				}
				if (sortState.HasValue)
				{
					sortStateEnum = sortState.Value;
				}
				else if (m_sortColumnState.HasValue)
				{
					sortStateEnum = m_sortColumnState.Value;
				}
				Row row = null;
				if (IgnoreFirstRowForSort && m_rows.Count > 0)
				{
					row = m_rows[0];
					m_rows.RemoveAt(0);
				}
				List<Row> list = m_rows.Where((Row r) => !r.IsGlobalSortEnabled).ToList();
				foreach (Row item in list)
				{
					m_rows.Remove(item);
				}
				if (sortStateEnum == SortStateEnum.Ascending)
				{
					m_rows.Sort((Row a, Row b) => Compare(columnIdx, comparison, a, b));
					list.Sort((Row a, Row b) => Compare(columnIdx, comparison, a, b));
				}
				else
				{
					m_rows.Sort((Row a, Row b) => Compare(columnIdx, comparison, b, a));
					list.Sort((Row a, Row b) => Compare(columnIdx, comparison, b, a));
				}
				if (row != null)
				{
					m_rows.Insert(0, row);
				}
				m_rows.InsertRange(0, list);
				m_lastSortedColumnIdx = columnIdx;
				columnMetaData.SortState = sortStateEnum;
				SelectedRowIndex = null;
			}
		}

		private static int Compare(int columnIdx, Comparison<Cell> comparison, Row a, Row b)
		{
			Cell x = (a.Cells.Count > columnIdx) ? a.Cells[columnIdx] : null;
			Cell y = (b.Cells.Count > columnIdx) ? b.Cells[columnIdx] : null;
			return comparison(x, y);
		}

		public int FindRow(Row row)
		{
			return m_rows.IndexOf(row);
		}

		private bool IsValidRowIndex(int? index)
		{
			if (index.HasValue && 0 <= index.Value)
			{
				return index.Value < m_rows.Count;
			}
			return false;
		}

		private void RefreshInternals()
		{
			while (m_columnsMetaData.Count < ColumnsCount)
			{
				m_columnsMetaData.Add(new ColumnMetaData());
			}
			Vector2 minSizeGui = m_styleDef.Texture.MinSizeGui;
			Vector2 maxSizeGui = m_styleDef.Texture.MaxSizeGui;
			base.Size = Vector2.Clamp(new Vector2(base.Size.X, RowHeight * (float)(VisibleRowsCount + 1) + minSizeGui.Y), minSizeGui, maxSizeGui);
			m_headerArea.Position = new Vector2(m_styleDef.Padding.Left, m_styleDef.Padding.Top);
			m_headerArea.Size = new Vector2(base.Size.X - (m_styleDef.Padding.Left + m_styleDef.ScrollbarMargin.HorizontalSum + m_scrollBar.Size.X), RowHeight);
			m_rowsArea.Position = m_headerArea.Position + (HeaderVisible ? new Vector2(0f, RowHeight) : Vector2.Zero);
			m_rowsArea.Size = new Vector2(m_headerArea.Size.X, RowHeight * (float)VisibleRowsCount);
			RefreshScrollbar();
		}

		private void RefreshScrollbar()
		{
			m_scrollBar.Visible = (m_rows.Count > VisibleRowsCount);
			m_scrollBar.Init(m_rows.Count, VisibleRowsCount);
			Vector2 vector = base.Size * new Vector2(0.5f, -0.5f);
			MyGuiBorderThickness scrollbarMargin = m_styleDef.ScrollbarMargin;
			Vector2 position = new Vector2(vector.X - (scrollbarMargin.Right + m_scrollBar.Size.X), vector.Y + scrollbarMargin.Top);
			m_scrollBar.Layout(position, base.Size.Y - (scrollbarMargin.Top + scrollbarMargin.Bottom));
			m_scrollBar.ChangeValue(0f);
		}

		private void RefreshVisualStyle()
		{
			m_styleDef = GetVisualStyle(VisualStyle);
			RowHeight = m_styleDef.RowHeight;
			TextScale = m_styleDef.TextScale;
			RefreshInternals();
		}

		private void verticalScrollBar_ValueChanged(MyScrollbar scrollbar)
		{
			m_visibleRowIndexOffset = (int)scrollbar.Value;
		}

		private RectangleF GetRowRectangle(int? row)
		{
			if (row.HasValue)
			{
				return new RectangleF(GetPositionAbsoluteTopLeft() + new Vector2(0f, m_rowsArea.Position.Y + (float)(row.Value - m_visibleRowIndexOffset) * RowHeight), new Vector2(base.Size.X, RowHeight));
			}
			return base.Rectangle;
		}

		internal override void OnFocusChanged(bool focus)
		{
			if (focus && !SelectedRowIndex.HasValue)
			{
				int num = MathHelper.Clamp((int)((m_entryPoint.Y - m_rowsArea.Y) / RowHeight), 0, VisibleRowsCount);
				num += m_visibleRowIndexOffset;
				if (num >= m_rows.Count)
				{
					num = m_rows.Count - 1;
				}
				if (SelectedRowIndex != num)
				{
					SelectedRowIndex = num;
					this.ItemSelected.InvokeIfNotNull(this, new EventArgs
					{
						RowIndex = SelectedRowIndex.Value,
						MouseButton = MyMouseButtonsEnum.Left
					});
					ScrollToSelection();
				}
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
				int num = 0;
				if (SelectedRowIndex.HasValue)
				{
					num = SelectedRowIndex.Value;
				}
				switch (direction)
				{
				case MyDirection.Down:
					num++;
					break;
				case MyDirection.Up:
					num--;
					break;
				case MyDirection.Right:
					return null;
				case MyDirection.Left:
					return null;
				}
				if (num < 0 || num >= m_rows.Count)
				{
					return null;
				}
				if (SelectedRowIndex != num)
				{
					SelectedRowIndex = num;
					this.ItemSelected.InvokeIfNotNull(this, new EventArgs
					{
						RowIndex = SelectedRowIndex.Value,
						MouseButton = MyMouseButtonsEnum.Left
					});
					if (base.Owner == null)
					{
						return null;
					}
					ScrollToSelection();
				}
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
