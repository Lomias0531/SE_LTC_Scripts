using System;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlScrollablePanel : MyGuiControlBase, IMyGuiControlsParent, IMyGuiControlsOwner
	{
		private MyGuiControls m_controls;

		private MyVScrollbar m_scrollbarV;

		private MyHScrollbar m_scrollbarH;

		private MyGuiControlBase m_scrolledControl;

		private RectangleF m_scrolledArea;

		private MyGuiBorderThickness m_scrolledAreaPadding;

		public Vector2 ScrollBarOffset = Vector2.Zero;

		public float ScrollBarHScale = 1f;

		public float ScrollBarVScale = 1f;

		public Vector2 ContentOffset = Vector2.Zero;

		public bool DrawScrollBarSeparator;

		public float ScrollbarHSizeX
		{
			get
			{
				if (m_scrollbarH == null)
				{
					return 0f;
				}
				return m_scrollbarH.Size.X;
			}
		}

		public float ScrollbarHSizeY
		{
			get
			{
				if (m_scrollbarH == null)
				{
					return 0f;
				}
				return m_scrollbarH.Size.Y;
			}
		}

		public float ScrollbarVSizeX
		{
			get
			{
				if (m_scrollbarV == null)
				{
					return 0f;
				}
				return m_scrollbarV.Size.X;
			}
		}

		public float ScrollbarVSizeY
		{
			get
			{
				if (m_scrollbarV == null)
				{
					return 0f;
				}
				return m_scrollbarV.Size.Y;
			}
		}

		public bool ScrollbarHWheel
		{
			get
			{
				if (m_scrollbarH != null)
				{
					return m_scrollbarH.EnableWheelScroll;
				}
				return false;
			}
			set
			{
				if (m_scrollbarH != null)
				{
					m_scrollbarH.EnableWheelScroll = value;
				}
			}
		}

		public bool ScrollbarHEnabled
		{
			get
			{
				return m_scrollbarH != null;
			}
			set
			{
				if (value && m_scrollbarH == null)
				{
					m_scrollbarH = new MyHScrollbar(this);
					m_scrollbarH.ValueChanged += scrollbar_ValueChanged;
				}
				else if (!value)
				{
					m_scrollbarH = null;
				}
			}
		}

		public bool ScrollbarVEnabled
		{
			get
			{
				return m_scrollbarV != null;
			}
			set
			{
				if (value && m_scrollbarV == null)
				{
					m_scrollbarV = new MyVScrollbar(this);
					m_scrollbarV.ValueChanged += scrollbar_ValueChanged;
				}
				else if (!value)
				{
					m_scrollbarV = null;
				}
			}
		}

		public MyGuiControlBase ScrolledControl
		{
			get
			{
				return m_scrolledControl;
			}
			set
			{
				if (m_scrolledControl != value)
				{
					if (m_scrolledControl != null)
					{
						Elements.Remove(m_scrolledControl);
						m_scrolledControl.SizeChanged -= scrolledControl_SizeChanged;
					}
					m_scrolledControl = value;
					if (m_scrolledControl != null)
					{
						Elements.Add(m_scrolledControl);
						m_scrolledControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
						m_scrolledControl.SizeChanged += scrolledControl_SizeChanged;
					}
					RefreshScrollbar();
					RefreshScrolledControlPosition();
				}
			}
		}

		public Vector2 ScrolledAreaSize => m_scrolledArea.Size;

		public MyGuiBorderThickness ScrolledAreaPadding
		{
			get
			{
				return m_scrolledAreaPadding;
			}
			set
			{
				m_scrolledAreaPadding = value;
				RefreshInternals();
			}
		}

		public float ScrollbarVPosition
		{
			get
			{
				if (m_scrollbarV != null)
				{
					return m_scrollbarV.Value;
				}
				return 0f;
			}
			set
			{
				if (m_scrollbarV != null)
				{
					m_scrollbarV.Value = value;
				}
			}
		}

		public MyGuiControls Controls => m_controls;

		public event Action<MyGuiControlScrollablePanel> PanelScrolled;

		public MyGuiControlScrollablePanel(MyGuiControlBase scrolledControl)
		{
			base.Name = "ScrollablePanel";
			ScrolledControl = scrolledControl;
			m_controls = new MyGuiControls(this);
			if (scrolledControl != null)
			{
				m_controls.Add(ScrolledControl);
			}
			base.CanFocusChildren = true;
			base.CanHaveFocus = true;
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			RectangleF scrolledArea = m_scrolledArea;
			scrolledArea.Position += GetPositionAbsoluteTopLeft();
			myGuiControlBase = HandleInputElements();
			if (m_scrollbarV != null && m_scrollbarV.HandleInput())
			{
				myGuiControlBase = (myGuiControlBase ?? this);
			}
			if (m_scrollbarH != null && m_scrollbarH.HandleInput())
			{
				myGuiControlBase = (myGuiControlBase ?? this);
			}
			return myGuiControlBase;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			Color colorMask = MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha);
			if (m_scrollbarV != null)
			{
				m_scrollbarV.ScrollBarScale = ScrollBarVScale;
				m_scrollbarV.Draw(colorMask);
				if (DrawScrollBarSeparator)
				{
					Vector2 positionAbsoluteTopRight = GetPositionAbsoluteTopRight();
					positionAbsoluteTopRight.X -= m_scrollbarV.Size.X + 0.0021f;
					MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Controls\\scrollable_list_line.dds", positionAbsoluteTopRight, new Vector2(0.0012f, base.Size.Y), MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				}
			}
			if (m_scrollbarH != null)
			{
				m_scrollbarH.ScrollBarScale = ScrollBarHScale;
				m_scrollbarH.Draw(colorMask);
			}
		}

		protected override void DrawElements(float transitionAlpha, float backgroundTransitionAlpha)
		{
			RectangleF normalizedRectangle = m_scrolledArea;
			normalizedRectangle.Position += GetPositionAbsoluteTopLeft();
			using (MyGuiManager.UsingScissorRectangle(ref normalizedRectangle))
			{
				base.DrawElements(transitionAlpha, backgroundTransitionAlpha);
			}
		}

		private void DebugDraw()
		{
			MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft(), base.Size, Color.White, 2);
			MyGuiManager.DrawBorders(GetPositionAbsoluteTopLeft() + m_scrolledArea.Position, m_scrolledArea.Size, Color.Cyan, 1);
		}

		public void FitSizeToScrolledControl()
		{
			if (ScrolledControl != null)
			{
				m_scrolledArea.Size = ScrolledControl.Size;
				Vector2 size = ScrolledControl.Size + m_scrolledAreaPadding.SizeChange;
				if (m_scrollbarV != null)
				{
					size.X += m_scrollbarV.Size.X;
				}
				if (m_scrollbarH != null)
				{
					size.Y += m_scrollbarH.Size.Y;
				}
				base.Size = size;
			}
		}

		public void SetPageVertical(float pageNumber)
		{
			m_scrollbarV.SetPage(pageNumber);
		}

		protected override void OnPositionChanged()
		{
			base.OnPositionChanged();
			RefreshScrollbar();
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshScrolledArea();
			RefreshScrollbar();
		}

		protected override void OnHasHighlightChanged()
		{
			base.OnHasHighlightChanged();
			if (m_scrollbarV != null)
			{
				m_scrollbarV.HasHighlight = base.HasHighlight;
			}
			if (m_scrollbarH != null)
			{
				m_scrollbarH.HasHighlight = base.HasHighlight;
			}
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
		}

		public override void ShowToolTip()
		{
			base.ShowToolTip();
		}

		private void RefreshScrolledArea()
		{
			m_scrolledArea = new RectangleF(m_scrolledAreaPadding.TopLeftOffset, base.Size - m_scrolledAreaPadding.SizeChange);
			if (m_scrollbarV != null)
			{
				m_scrolledArea.Size.X -= m_scrollbarV.Size.X;
			}
			if (m_scrollbarH != null)
			{
				m_scrolledArea.Size.Y -= m_scrollbarH.Size.Y;
			}
			if (this.PanelScrolled != null)
			{
				this.PanelScrolled(this);
			}
		}

		private void RefreshScrollbar()
		{
			if (ScrolledControl != null)
			{
				if (m_scrollbarV != null)
				{
					m_scrollbarV.Visible = (m_scrolledArea.Size.Y < ScrolledControl.Size.Y);
					if (m_scrollbarV.Visible)
					{
						m_scrollbarV.Init(ScrolledControl.Size.Y, m_scrolledArea.Size.Y);
						Vector2 vector = base.Size * new Vector2(0.5f, -0.5f);
						Vector2 value = new Vector2(vector.X - m_scrollbarV.Size.X + ScrollBarOffset.X, vector.Y);
						m_scrollbarV.Layout(value + new Vector2(0f, m_scrolledAreaPadding.Top), m_scrolledArea.Size.Y);
					}
					else
					{
						m_scrollbarV.Value = 0f;
					}
				}
				if (m_scrollbarH != null)
				{
					m_scrollbarH.Visible = (m_scrolledArea.Size.X < ScrolledControl.Size.X);
					if (m_scrollbarH.Visible)
					{
						m_scrollbarH.Init(ScrolledControl.Size.X, m_scrolledArea.Size.X);
						Vector2 vector2 = base.Size * new Vector2(-0.5f, 0.5f);
						Vector2 value2 = new Vector2(vector2.X, vector2.Y - m_scrollbarH.Size.Y + ScrollBarOffset.Y);
						m_scrollbarH.Layout(value2 + new Vector2(m_scrolledAreaPadding.Left), m_scrolledArea.Size.X);
					}
					else
					{
						m_scrollbarH.Value = 0f;
					}
				}
			}
			else
			{
				if (m_scrollbarV != null)
				{
					m_scrollbarV.Visible = false;
				}
				if (m_scrollbarH != null)
				{
					m_scrollbarH.Visible = false;
				}
			}
			RefreshScrolledControlPosition();
		}

		private void scrollbar_ValueChanged(MyScrollbar scrollbar)
		{
			RefreshScrolledControlPosition();
		}

		private void scrolledControl_SizeChanged(MyGuiControlBase control)
		{
			RefreshInternals();
		}

		public void RefreshInternals()
		{
			RefreshScrolledArea();
			RefreshScrollbar();
		}

		private void RefreshScrolledControlPosition()
		{
			Vector2 position = -0.5f * base.Size + m_scrolledAreaPadding.TopLeftOffset + ContentOffset;
			if (m_scrollbarH != null)
			{
				position.X -= m_scrollbarH.Value;
			}
			if (m_scrollbarV != null)
			{
				position.Y -= m_scrollbarV.Value;
			}
			if (ScrolledControl != null)
			{
				ScrolledControl.Position = position;
			}
			if (this.PanelScrolled != null)
			{
				this.PanelScrolled(this);
			}
		}

		public void SetVerticalScrollbarValue(float value)
		{
			if (m_scrollbarV != null)
			{
				m_scrollbarV.Value = value;
			}
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			return m_scrolledControl.GetNextFocusControl(currentFocusControl, direction, page);
		}

		public override void OnFocusChanged(MyGuiControlBase control, bool focus)
		{
			if (focus)
			{
				RectangleF focusRectangle = control.FocusRectangle;
				RectangleF scrolledArea = m_scrolledArea;
				scrolledArea.Position += GetPositionAbsoluteTopLeft();
				if (focusRectangle.Y < scrolledArea.Y)
				{
					m_scrollbarV.Value += focusRectangle.Y - scrolledArea.Y;
				}
				else if (focusRectangle.Y + focusRectangle.Size.Y > scrolledArea.Y + scrolledArea.Height)
				{
					m_scrollbarV.Value += focusRectangle.Y + focusRectangle.Size.Y - scrolledArea.Y - scrolledArea.Height;
				}
			}
		}
	}
}
