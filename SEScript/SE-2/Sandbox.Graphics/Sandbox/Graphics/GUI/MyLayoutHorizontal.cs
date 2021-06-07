using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public struct MyLayoutHorizontal
	{
		private IMyGuiControlsParent m_parent;

		private Vector2 m_parentSize;

		private float m_currentPosX;

		private float m_verticalPadding;

		public float CurrentX => m_currentPosX;

		public float VerticalPadding => m_verticalPadding;

		public MyLayoutHorizontal(IMyGuiControlsParent parent, float verticalPaddingPx)
		{
			m_parent = parent;
			m_parentSize = (parent.GetSize() ?? Vector2.One);
			m_currentPosX = m_parentSize.X * -0.5f;
			m_verticalPadding = verticalPaddingPx / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
		}

		public void Add(MyGuiControlBase control, MyAlignV align, bool advance = true)
		{
			AddInternal(control, MyAlignH.Left, align, advance, control.Size.X);
		}

		public void Add(MyGuiControlBase control, float preferredHeightPx, float preferredWidthPx, MyAlignV align)
		{
			control.Size = new Vector2(preferredWidthPx, preferredHeightPx) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Add(control, align);
		}

		public void Advance(float advanceAmountPx)
		{
			m_currentPosX += advanceAmountPx / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
		}

		private void AddInternal(MyGuiControlBase control, MyAlignH alignH, MyAlignV alignV, bool advance, float horizontalSize)
		{
			control.OriginAlign = (MyGuiDrawAlignEnum)(3 * (int)alignH + alignV);
			int num = (int)(-1 + alignV);
			float num2 = horizontalSize * 0.5f * (float)alignH;
			control.Position = new Vector2(m_currentPosX + num2, (float)num * (0.5f * m_parentSize.Y - m_verticalPadding));
			m_currentPosX += (advance ? (horizontalSize - num2) : 0f);
			m_parent.Controls.Add(control);
		}
	}
}
