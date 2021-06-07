using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlList))]
	public class MyGuiControlList : MyGuiControlParent
	{
		public class StyleDefinition
		{
			public MyGuiCompositeTexture Texture = new MyGuiCompositeTexture();

			public MyGuiBorderThickness ScrollbarMargin;

			public MyGuiBorderThickness ItemMargin;

			public bool ScrollbarEnabled;
		}

		private static StyleDefinition[] m_styles;

		private MyScrollbar m_scrollBar;

		private Vector2 m_realSize;

		private bool m_showScrollbar;

		private RectangleF m_itemsRectangle;

		private MyGuiBorderThickness m_itemMargin;

		private MyGuiControlBase m_lastCheckedControl;

		private bool m_isChildFocused;

		public bool FocusScrollingEnabled = true;

		private MyGuiControlListStyleEnum m_visualStyle;

		private StyleDefinition m_styleDef;

		public MyGuiControlListStyleEnum VisualStyle
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

		public event Action<MyGuiControlList> ItemMouseOver;

		static MyGuiControlList()
		{
			m_styles = new StyleDefinition[MyUtils.GetMaxValueFromEnum<MyGuiControlListStyleEnum>() + 1];
			StyleDefinition[] styles = m_styles;
			StyleDefinition obj = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST
			};
			MyGuiBorderThickness scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj.ScrollbarMargin = scrollbarMargin;
			obj.ItemMargin = new MyGuiBorderThickness(12f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 12f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			obj.ScrollbarEnabled = true;
			styles[0] = obj;
			StyleDefinition[] styles2 = m_styles;
			StyleDefinition obj2 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj2.ScrollbarMargin = scrollbarMargin;
			obj2.ItemMargin = new MyGuiBorderThickness(12f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 12f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			obj2.ScrollbarEnabled = true;
			styles2[1] = obj2;
			m_styles[2] = new StyleDefinition
			{
				ScrollbarEnabled = true
			};
			StyleDefinition[] styles3 = m_styles;
			StyleDefinition obj3 = new StyleDefinition
			{
				Texture = MyGuiConstants.TEXTURE_SCROLLABLE_WBORDER_LIST
			};
			scrollbarMargin = new MyGuiBorderThickness
			{
				Left = 2f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Right = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X,
				Top = 3f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y,
				Bottom = 1f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y
			};
			obj3.ScrollbarMargin = scrollbarMargin;
			obj3.ItemMargin = new MyGuiBorderThickness(12f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 12f / MyGuiConstants.GUI_OPTIMAL_SIZE.Y);
			obj3.ScrollbarEnabled = true;
			styles3[3] = obj3;
		}

		public static StyleDefinition GetVisualStyle(MyGuiControlListStyleEnum style)
		{
			return m_styles[(int)style];
		}

		public MyGuiControlList()
			: this(null, null, null, null, MyGuiControlListStyleEnum.Default)
		{
		}

		public MyGuiControlList(Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, string toolTip = null, MyGuiControlListStyleEnum visualStyle = MyGuiControlListStyleEnum.Default)
			: base(position, size, backgroundColor, toolTip)
		{
			base.Name = "ControlList";
			m_realSize = (size ?? Vector2.One);
			m_scrollBar = new MyVScrollbar(this);
			m_scrollBar.ValueChanged += ValueChanged;
			VisualStyle = visualStyle;
			RecalculateScrollbar();
			base.Controls.CollectionChanged += OnVisibleControlsChanged;
			base.Controls.CollectionMembersVisibleChanged += OnVisibleControlsChanged;
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GuiControlList myObjectBuilder_GuiControlList = builder as MyObjectBuilder_GuiControlList;
			VisualStyle = myObjectBuilder_GuiControlList.VisualStyle;
		}

		public void InitControls(IEnumerable<MyGuiControlBase> controls)
		{
			base.Controls.CollectionMembersVisibleChanged -= OnVisibleControlsChanged;
			base.Controls.CollectionChanged -= OnVisibleControlsChanged;
			base.Controls.Clear();
			foreach (MyGuiControlBase control in controls)
			{
				if (control != null)
				{
					base.Controls.Add(control);
				}
			}
			base.Controls.CollectionChanged += OnVisibleControlsChanged;
			base.Controls.CollectionMembersVisibleChanged += OnVisibleControlsChanged;
			Recalculate();
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlList obj = base.GetObjectBuilder() as MyObjectBuilder_GuiControlList;
			obj.VisualStyle = VisualStyle;
			return obj;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			DrawBorder(transitionAlpha);
			Vector2 positionAbsoluteTopLeft = GetPositionAbsoluteTopLeft();
			m_styleDef.Texture.Draw(positionAbsoluteTopLeft, base.Size, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, backgroundTransitionAlpha));
			RectangleF normalizedRectangle = m_itemsRectangle;
			normalizedRectangle.Position += positionAbsoluteTopLeft;
			using (MyGuiManager.UsingScissorRectangle(ref normalizedRectangle))
			{
				base.Draw(transitionAlpha, backgroundTransitionAlpha);
			}
			if (m_showScrollbar)
			{
				m_scrollBar.Draw(MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha));
			}
			Vector2 positionAbsoluteTopRight = GetPositionAbsoluteTopRight();
			positionAbsoluteTopRight.X -= m_styleDef.ScrollbarMargin.HorizontalSum + m_scrollBar.Size.X;
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Controls\\scrollable_list_line.dds", positionAbsoluteTopRight, new Vector2(0.001f, base.Size.Y), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
		}

		private void DebugDraw()
		{
			MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft() + m_itemsRectangle.Position, m_itemsRectangle.Size, Color.White, 1);
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (m_showScrollbar && m_scrollBar.Visible && CheckMouseOver() && myGuiControlBase == null)
			{
				if (m_scrollBar.HandleInput())
				{
					return this;
				}
				return myGuiControlBase;
			}
			MyGuiControlBase myGuiControlBase2 = GetTopMostOwnerScreen()?.FocusedControl;
			if (myGuiControlBase2 != null && m_lastCheckedControl != myGuiControlBase2 && m_scrollBar != null)
			{
				m_lastCheckedControl = myGuiControlBase2;
				m_isChildFocused = false;
				while (myGuiControlBase2.Owner != null)
				{
					if (myGuiControlBase2.Owner == this)
					{
						m_isChildFocused = true;
						break;
					}
					myGuiControlBase2 = (myGuiControlBase2.Owner as MyGuiControlBase);
					if (myGuiControlBase2 == null)
					{
						break;
					}
				}
			}
			if (m_isChildFocused)
			{
				if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_UP, MyControlStateType.PRESSED))
				{
					m_scrollBar.ScrollUp();
				}
				else if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_DOWN, MyControlStateType.PRESSED))
				{
					m_scrollBar.ScrollDown();
				}
			}
			return myGuiControlBase;
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			RecalculateScrollbar();
			CalculateNewPositionsForControls((m_scrollBar != null) ? m_scrollBar.Value : 0f);
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
		}

		public void Recalculate()
		{
			_ = m_realSize;
			CalculateRealSize();
			m_itemsRectangle.Position = m_itemMargin.TopLeftOffset;
			m_itemsRectangle.Size = base.Size - (m_itemMargin.SizeChange + new Vector2(m_styleDef.ScrollbarMargin.HorizontalSum + (m_showScrollbar ? m_scrollBar.Size.X : 0f), 0f));
			RecalculateScrollbar();
			CalculateNewPositionsForControls((m_scrollBar != null) ? m_scrollBar.Value : 0f);
		}

		public MyGuiBorderThickness GetItemMargins()
		{
			return m_itemMargin;
		}

		private void RecalculateScrollbar()
		{
			if (m_showScrollbar)
			{
				m_scrollBar.Visible = (base.Size.Y < m_realSize.Y);
				m_scrollBar.Init(m_realSize.Y, m_itemsRectangle.Size.Y);
				Vector2 vector = base.Size * new Vector2(0.5f, -0.5f);
				MyGuiBorderThickness scrollbarMargin = m_styleDef.ScrollbarMargin;
				Vector2 position = new Vector2(vector.X - (scrollbarMargin.Right + m_scrollBar.Size.X), vector.Y + scrollbarMargin.Top);
				m_scrollBar.Layout(position, base.Size.Y - scrollbarMargin.VerticalSum);
			}
		}

		private void ValueChanged(MyScrollbar scrollbar)
		{
			CalculateNewPositionsForControls(scrollbar.Value);
		}

		private void CalculateNewPositionsForControls(float offset)
		{
			Vector2 marginStep = m_itemMargin.MarginStep;
			Vector2 topLeft = -0.5f * base.Size + 0.001f + m_itemMargin.TopLeftOffset - new Vector2(-1f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, offset);
			foreach (MyGuiControlBase visibleControl in base.Controls.GetVisibleControls())
			{
				Vector2 size = visibleControl.Size;
				size.X = m_itemsRectangle.Size.X;
				visibleControl.Position = MyUtils.GetCoordAlignedFromTopLeft(topLeft, size, visibleControl.OriginAlign);
				topLeft.Y += size.Y + marginStep.Y;
			}
		}

		private void CalculateRealSize()
		{
			Vector2 zero = Vector2.Zero;
			float y = m_itemMargin.MarginStep.Y;
			foreach (MyGuiControlBase visibleControl in base.Controls.GetVisibleControls())
			{
				Vector2 value = visibleControl.GetSize().Value;
				zero.Y += value.Y + y;
				zero.X = Math.Max(zero.X, value.X);
			}
			zero.Y -= y;
			m_realSize.X = Math.Max(base.Size.X, zero.X);
			m_realSize.Y = Math.Max(base.Size.Y, zero.Y);
		}

		private void RefreshVisualStyle()
		{
			m_styleDef = GetVisualStyle(VisualStyle);
			m_itemMargin = m_styleDef.ItemMargin;
			m_showScrollbar = m_styleDef.ScrollbarEnabled;
			base.MinSize = m_styleDef.Texture.MinSizeGui;
			base.MaxSize = m_styleDef.Texture.MaxSizeGui;
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			Recalculate();
		}

		private void OnVisibleControlsChanged(MyGuiControls sender)
		{
			Recalculate();
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			m_scrollBar.HasHighlight = base.HasHighlight;
		}

		public void SetScrollBarPage(float page = 0f)
		{
			m_scrollBar.SetPage(page);
		}

		public MyScrollbar GetScrollBar()
		{
			return m_scrollBar;
		}

		public override void OnFocusChanged(MyGuiControlBase control, bool focus)
		{
			if (focus && (FocusScrollingEnabled || MyInput.Static.IsJoystickLastUsed))
			{
				RectangleF focusRectangle = control.FocusRectangle;
				RectangleF itemsRectangle = m_itemsRectangle;
				itemsRectangle.Position += GetPositionAbsoluteTopLeft();
				if (focusRectangle.Y < itemsRectangle.Y)
				{
					m_scrollBar.Value += focusRectangle.Y - itemsRectangle.Y;
				}
				else if (focusRectangle.Y + focusRectangle.Size.Y > itemsRectangle.Y + itemsRectangle.Height)
				{
					m_scrollBar.Value += focusRectangle.Y + focusRectangle.Size.Y - itemsRectangle.Y - itemsRectangle.Height;
				}
			}
		}
	}
}
