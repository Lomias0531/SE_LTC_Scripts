using System;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public abstract class MyScrollbar
	{
		protected enum StateEnum
		{
			Ready,
			Drag,
			Click
		}

		public const float DEFAULT_STEP = 0.015f;

		private bool m_hasHighlight;

		private float m_value;

		private MyGuiCompositeTexture m_normalTexture;

		private MyGuiCompositeTexture m_highlightTexture;

		protected MyGuiCompositeTexture m_backgroundTexture;

		protected MyGuiControlBase OwnerControl;

		protected Vector2 Position;

		protected Vector2 CaretSize;

		protected Vector2 CaretPageSize;

		protected float Max;

		protected float Page;

		protected StateEnum State;

		protected MyGuiCompositeTexture Texture;

		public float ScrollBarScale = 1f;

		public bool Visible;

		public Vector2 Size
		{
			get;
			protected set;
		}

		public bool HasHighlight
		{
			get
			{
				return m_hasHighlight;
			}
			set
			{
				if (m_hasHighlight != value)
				{
					m_hasHighlight = value;
					RefreshInternals();
				}
			}
		}

		public float MaxSize => Max;

		public float PageSize => Page;

		public float Value
		{
			get
			{
				return m_value;
			}
			set
			{
				value = MathHelper.Clamp(value, 0f, Max - Page);
				if (m_value != value)
				{
					m_value = value;
					if (this.ValueChanged != null)
					{
						this.ValueChanged(this);
					}
				}
			}
		}

		public bool IsOverCaret
		{
			get;
			protected set;
		}

		public bool IsInDomainCaret
		{
			get;
			protected set;
		}

		public event Action<MyScrollbar> ValueChanged;

		protected MyScrollbar(MyGuiControlBase control, MyGuiCompositeTexture normalTexture, MyGuiCompositeTexture highlightTexture, MyGuiCompositeTexture backgroundTexture)
		{
			OwnerControl = control;
			m_normalTexture = normalTexture;
			m_highlightTexture = highlightTexture;
			m_backgroundTexture = backgroundTexture;
			RefreshInternals();
		}

		protected bool CanScroll()
		{
			if (Max > 0f)
			{
				return Max > Page;
			}
			return false;
		}

		public void Init(float max, float page)
		{
			Max = max;
			Page = page;
		}

		public void ChangeValue(float amount)
		{
			Value += amount;
		}

		public void PageDown()
		{
			ChangeValue(Page);
		}

		public void PageUp()
		{
			ChangeValue(0f - Page);
		}

		public void ScrollDown(float step = 0.015f)
		{
			ChangeValue(step);
		}

		public void ScrollUp(float step = 0.015f)
		{
			ChangeValue(0f - step);
		}

		public void SetPage(float pageNumber)
		{
			Value = pageNumber * Page;
		}

		public float GetPage()
		{
			return Value / Page;
		}

		public abstract void Layout(Vector2 position, float length);

		public abstract void Draw(Color colorMask);

		public void DebugDraw()
		{
			MyGuiManager.DrawBorders(OwnerControl.GetPositionAbsoluteCenter() + Position, Size, Color.White, 1);
		}

		public abstract bool HandleInput();

		protected virtual void RefreshInternals()
		{
			Texture = (HasHighlight ? m_highlightTexture : m_normalTexture);
			if (HasHighlight)
			{
				Texture = m_highlightTexture;
			}
			else
			{
				Texture = m_normalTexture;
			}
		}
	}
}
