using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyHScrollbar : MyScrollbar
	{
		private Vector2 m_dragClick;

		public bool EnableWheelScroll;

		public MyHScrollbar(MyGuiControlBase control)
			: base(control, MyGuiConstants.TEXTURE_SCROLLBAR_H_THUMB, MyGuiConstants.TEXTURE_SCROLLBAR_H_THUMB_HIGHLIGHT, MyGuiConstants.TEXTURE_SCROLLBAR_V_BACKGROUND)
		{
		}

		private Vector2 GetCarretPosition()
		{
			return new Vector2(base.Value * (base.Size.X - CaretSize.X) / (Max - Page), 0f);
		}

		public override void Layout(Vector2 positionTopLeft, float length)
		{
			Position = positionTopLeft;
			base.Size = new Vector2(length, Texture.MinSizeGui.Y);
			if (CanScroll())
			{
				CaretSize = new Vector2(MathHelper.Clamp(Page / Max * length, Texture.MinSizeGui.X, Texture.MaxSizeGui.X), Texture.MinSizeGui.Y);
				if (base.Value > Max - Page)
				{
					base.Value = Max - Page;
				}
			}
		}

		public override void Draw(Color colorMask)
		{
			if (Visible)
			{
				Vector2 vector = OwnerControl.GetPositionAbsoluteCenter() + Position;
				m_backgroundTexture.Draw(vector, base.Size, colorMask);
				if (CanScroll())
				{
					Vector2 carretPosition = GetCarretPosition();
					Texture.Draw(vector + carretPosition, CaretSize, colorMask, ScrollBarScale);
				}
			}
		}

		public override bool HandleInput()
		{
			bool result = false;
			if (!Visible || !CanScroll())
			{
				return result;
			}
			Vector2 vector = OwnerControl.GetPositionAbsoluteCenter() + Position;
			base.IsOverCaret = MyGuiControlBase.CheckMouseOver(CaretSize, vector + GetCarretPosition(), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			switch (State)
			{
			case StateEnum.Ready:
				if (MyInput.Static.IsNewLeftMousePressed() && base.IsOverCaret)
				{
					result = true;
					State = StateEnum.Drag;
					m_dragClick = MyGuiManager.MouseCursorPosition;
				}
				break;
			case StateEnum.Drag:
				if (!MyInput.Static.IsLeftMousePressed())
				{
					State = StateEnum.Ready;
				}
				else
				{
					ChangeValue((MyGuiManager.MouseCursorPosition.X - m_dragClick.X) * (Max - Page) / (base.Size.X - CaretSize.X));
					m_dragClick = MyGuiManager.MouseCursorPosition;
				}
				result = true;
				break;
			}
			if (EnableWheelScroll)
			{
				bool num = MyGuiControlBase.CheckMouseOver(base.Size, vector, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				bool isMouseOver = OwnerControl.IsMouseOver;
				if (num || isMouseOver)
				{
					base.Value += (float)MyInput.Static.DeltaMouseScrollWheelValue() / -120f * Page * 0.25f;
				}
			}
			return result;
		}

		protected override void RefreshInternals()
		{
			base.RefreshInternals();
			base.Size = new Vector2(base.Size.X, Texture.MinSizeGui.Y);
		}
	}
}
