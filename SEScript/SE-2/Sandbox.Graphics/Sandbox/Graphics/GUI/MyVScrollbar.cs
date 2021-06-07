using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyVScrollbar : MyScrollbar
	{
		private Vector2 m_dragClick;

		public MyVScrollbar(MyGuiControlBase control)
			: base(control, MyGuiConstants.TEXTURE_SCROLLBAR_V_THUMB, MyGuiConstants.TEXTURE_SCROLLBAR_V_THUMB_HIGHLIGHT, MyGuiConstants.TEXTURE_SCROLLBAR_V_BACKGROUND)
		{
		}

		private Vector2 GetCarretPosition()
		{
			return new Vector2(0f, base.Value * (base.Size.Y - CaretSize.Y) / (Max - Page));
		}

		public override void Layout(Vector2 positionTopLeft, float length)
		{
			Position = positionTopLeft;
			base.Size = new Vector2(Texture.MinSizeGui.X, length);
			if (CanScroll())
			{
				CaretSize = new Vector2(Texture.MinSizeGui.X, MathHelper.Clamp(Page / Max * length, Texture.MinSizeGui.Y, Texture.MaxSizeGui.Y));
				CaretPageSize = new Vector2(Texture.MinSizeGui.X, MathHelper.Clamp(Page, Texture.MinSizeGui.Y, Texture.MaxSizeGui.Y));
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
					Texture.Draw(vector + carretPosition, CaretSize, colorMask);
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
			bool flag = MyGuiControlBase.CheckMouseOver(base.Size, vector, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			bool isMouseOver = OwnerControl.IsMouseOver;
			base.IsOverCaret = MyGuiControlBase.CheckMouseOver(CaretSize, vector + GetCarretPosition(), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			base.IsInDomainCaret = MyGuiControlBase.CheckMouseOver(base.Size, vector, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			base.HasHighlight = base.IsOverCaret;
			switch (State)
			{
			case StateEnum.Ready:
				if (MyInput.Static.IsNewPrimaryButtonPressed() && base.IsOverCaret)
				{
					result = true;
					State = StateEnum.Drag;
					m_dragClick = MyGuiManager.MouseCursorPosition;
				}
				else if (MyInput.Static.IsNewPrimaryButtonPressed() && base.IsInDomainCaret)
				{
					result = true;
					m_dragClick = MyGuiManager.MouseCursorPosition;
					State = StateEnum.Click;
				}
				break;
			case StateEnum.Drag:
				if (!MyInput.Static.IsPrimaryButtonPressed())
				{
					State = StateEnum.Ready;
				}
				else
				{
					ChangeValue((MyGuiManager.MouseCursorPosition.Y - m_dragClick.Y) * (Max - Page) / (base.Size.Y - CaretSize.Y));
					m_dragClick = MyGuiManager.MouseCursorPosition;
				}
				result = true;
				break;
			case StateEnum.Click:
			{
				m_dragClick = MyGuiManager.MouseCursorPosition;
				Vector2 vector2 = GetCarretPosition() + vector + CaretSize / 2f;
				float amount = (m_dragClick.Y - vector2.Y) * (Max - Page) / (base.Size.Y - CaretSize.Y);
				ChangeValue(amount);
				State = StateEnum.Ready;
				break;
			}
			}
			if (flag || isMouseOver)
			{
				int num = MyInput.Static.DeltaMouseScrollWheelValue();
				if (num != 0 && num != -MyInput.Static.PreviousMouseScrollWheelValue())
				{
					result = true;
					ChangeValue((float)num / -120f * Page * 0.25f);
				}
			}
			return result;
		}

		protected override void RefreshInternals()
		{
			base.RefreshInternals();
			base.Size = new Vector2(Texture.MinSizeGui.X, base.Size.Y);
		}
	}
}
