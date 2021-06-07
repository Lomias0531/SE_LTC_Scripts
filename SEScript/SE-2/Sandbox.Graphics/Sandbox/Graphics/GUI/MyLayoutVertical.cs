using System;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public struct MyLayoutVertical
	{
		private IMyGuiControlsParent m_parent;

		private Vector2 m_parentSize;

		private float m_currentPosY;

		private float m_horizontalPadding;

		public float CurrentY => m_currentPosY;

		public float HorrizontalPadding => m_horizontalPadding;

		public MyLayoutVertical(IMyGuiControlsParent parent, float horizontalPaddingPx)
		{
			m_parent = parent;
			m_parentSize = (parent.GetSize() ?? Vector2.One);
			m_currentPosY = m_parentSize.Y * -0.5f;
			m_horizontalPadding = horizontalPaddingPx / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
		}

		public void Add(MyGuiControlBase control, MyAlignH align, bool advance = true)
		{
			AddInternal(control, align, MyAlignV.Top, advance, control.Size.Y);
		}

		public void Add(MyGuiControlBase control, float preferredWidthPx, float preferredHeightPx, MyAlignH align)
		{
			control.Size = new Vector2(preferredWidthPx, preferredHeightPx) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Add(control, align);
		}

		public void Add(MyGuiControlBase leftControl, MyGuiControlBase rightControl)
		{
			float num = Math.Max(leftControl.Size.Y, rightControl.Size.Y);
			AddInternal(leftControl, MyAlignH.Left, MyAlignV.Center, advance: false, num);
			AddInternal(rightControl, MyAlignH.Right, MyAlignV.Center, advance: false, num);
			m_currentPosY += num;
		}

		public void Add(MyGuiControlBase leftControl, MyGuiControlBase centerControl, MyGuiControlBase rightControl)
		{
			float num = MathHelper.Max(leftControl.Size.Y, centerControl.Size.Y, rightControl.Size.Y);
			AddInternal(leftControl, MyAlignH.Left, MyAlignV.Center, advance: false, num);
			AddInternal(centerControl, MyAlignH.Center, MyAlignV.Center, advance: false, num);
			AddInternal(rightControl, MyAlignH.Right, MyAlignV.Center, advance: false, num);
			m_currentPosY += num;
		}

		public void Advance(float advanceAmountPx)
		{
			m_currentPosY += advanceAmountPx / MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
		}

		private void AddInternal(MyGuiControlBase control, MyAlignH alignH, MyAlignV alignV, bool advance, float verticalSize)
		{
			control.OriginAlign = (MyGuiDrawAlignEnum)(3 * (int)alignH + alignV);
			int num = (int)(-1 + alignH);
			float num2 = verticalSize * 0.5f * (float)alignV;
			control.Position = new Vector2((float)num * (0.5f * m_parentSize.X - m_horizontalPadding), m_currentPosY + num2);
			m_currentPosY += (advance ? (verticalSize - num2) : 0f);
			m_parent.Controls.Add(control);
		}
	}
}
