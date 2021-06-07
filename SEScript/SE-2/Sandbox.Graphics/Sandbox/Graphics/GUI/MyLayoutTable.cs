using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public struct MyLayoutTable
	{
		private IMyGuiControlsParent m_parent;

		private Vector2 m_parentTopLeft;

		private Vector2 m_size;

		private float[] m_prefixScanX;

		private float[] m_prefixScanY;

		private const float BORDER = 0.005f;

		public int LastRow
		{
			get
			{
				if (m_prefixScanY == null)
				{
					return 0;
				}
				return m_prefixScanY.Length - 2;
			}
		}

		public int LastColumn
		{
			get
			{
				if (m_prefixScanX == null)
				{
					return 0;
				}
				return m_prefixScanX.Length - 2;
			}
		}

		public Vector2 GetCellSize(int row, int col, int colSpan = 1, int rowSpan = 1)
		{
			Vector2 value = new Vector2(m_prefixScanX[col], m_prefixScanY[row]);
			Vector2 result = new Vector2(m_prefixScanX[col + colSpan], m_prefixScanY[row + rowSpan]) - value;
			result.X -= 0.01f;
			result.Y -= 0.01f;
			return result;
		}

		public MyLayoutTable(IMyGuiControlsParent parent)
		{
			m_parent = parent;
			m_size = (m_parent.GetSize() ?? Vector2.One);
			m_parentTopLeft = -0.5f * m_size;
			m_prefixScanX = null;
			m_prefixScanY = null;
		}

		public MyLayoutTable(IMyGuiControlsParent parent, Vector2 topLeft, Vector2 size)
		{
			m_parent = parent;
			m_parentTopLeft = topLeft;
			m_size = size;
			m_prefixScanX = null;
			m_prefixScanY = null;
		}

		public void SetColumnWidths(params float[] widthsPx)
		{
			m_prefixScanX = new float[widthsPx.Length + 1];
			m_prefixScanX[0] = m_parentTopLeft.X;
			float x = MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			for (int i = 0; i < widthsPx.Length; i++)
			{
				float num = widthsPx[i] / x;
				m_prefixScanX[i + 1] = m_prefixScanX[i] + num;
			}
		}

		public void SetColumnWidthsNormalized(params float[] widthsPx)
		{
			float x = m_size.X;
			float num = 0f;
			for (int i = 0; i < widthsPx.Length; i++)
			{
				num += widthsPx[i];
			}
			for (int j = 0; j < widthsPx.Length; j++)
			{
				widthsPx[j] *= MyGuiConstants.GUI_OPTIMAL_SIZE.X / num * x;
			}
			SetColumnWidths(widthsPx);
		}

		public void SetRowHeights(params float[] heightsPx)
		{
			m_prefixScanY = new float[heightsPx.Length + 1];
			m_prefixScanY[0] = m_parentTopLeft.Y;
			float y = MyGuiConstants.GUI_OPTIMAL_SIZE.Y;
			for (int i = 0; i < heightsPx.Length; i++)
			{
				float num = heightsPx[i] / y;
				m_prefixScanY[i + 1] = m_prefixScanY[i] + num;
			}
		}

		public void SetRowHeightsNormalized(params float[] heightsPx)
		{
			float y = m_size.Y;
			float num = 0f;
			for (int i = 0; i < heightsPx.Length; i++)
			{
				num += heightsPx[i];
			}
			for (int j = 0; j < heightsPx.Length; j++)
			{
				heightsPx[j] *= MyGuiConstants.GUI_OPTIMAL_SIZE.Y / num * y;
			}
			SetRowHeights(heightsPx);
		}

		public void Add(MyGuiControlBase control, MyAlignH alignH, MyAlignV alignV, int row, int col, int rowSpan = 1, int colSpan = 1)
		{
			Vector2 value = new Vector2(m_prefixScanX[col], m_prefixScanY[row]);
			Vector2 vector = new Vector2(m_prefixScanX[col + colSpan], m_prefixScanY[row + rowSpan]) - value;
			control.Position = new Vector2(value.X + vector.X * 0.5f * (float)alignH, value.Y + vector.Y * 0.5f * (float)alignV);
			control.OriginAlign = (MyGuiDrawAlignEnum)(3 * (int)alignH + alignV);
			m_parent.Controls.Add(control);
		}

		public void AddWithSize(MyGuiControlBase control, MyAlignH alignH, MyAlignV alignV, int row, int col, int rowSpan = 1, int colSpan = 1)
		{
			Vector2 value = new Vector2(m_prefixScanX[col], m_prefixScanY[row]);
			Vector2 size = new Vector2(m_prefixScanX[col + colSpan], m_prefixScanY[row + rowSpan]) - value;
			size.X -= 0.01f;
			size.Y -= 0.01f;
			control.Size = size;
			control.Position = new Vector2(value.X + size.X * 0.5f * (float)alignH + 0.005f, value.Y + size.Y * 0.5f * (float)alignV + 0.005f);
			control.OriginAlign = (MyGuiDrawAlignEnum)(3 * (int)alignH + alignV);
			m_parent.Controls.Add(control);
		}
	}
}
