using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public struct MyGuiSizedTexture
	{
		private Vector2 m_sizePx;

		private Vector2 m_sizeGui;

		public string Texture;

		public Vector2 SizePx
		{
			get
			{
				return m_sizePx;
			}
			set
			{
				m_sizePx = value;
				SizeGui = m_sizePx / MyGuiConstants.GUI_OPTIMAL_SIZE;
			}
		}

		public Vector2 SizeGui
		{
			get
			{
				return m_sizeGui;
			}
			private set
			{
				m_sizeGui = value;
			}
		}

		public MyGuiSizedTexture(MyGuiPaddedTexture original)
		{
			Texture = original.Texture;
			m_sizePx = original.SizePx;
			m_sizeGui = original.SizeGui;
		}
	}
}
