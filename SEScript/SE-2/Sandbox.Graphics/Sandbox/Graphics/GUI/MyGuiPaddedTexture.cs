using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public struct MyGuiPaddedTexture
	{
		public string Texture;

		private Vector2 m_sizePx;

		private Vector2 m_paddingSizePx;

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

		public Vector2 PaddingSizePx
		{
			get
			{
				return m_paddingSizePx;
			}
			set
			{
				m_paddingSizePx = value;
				PaddingSizeGui = m_paddingSizePx / MyGuiConstants.GUI_OPTIMAL_SIZE;
			}
		}

		public Vector2 SizeGui
		{
			get;
			private set;
		}

		public Vector2 PaddingSizeGui
		{
			get;
			private set;
		}
	}
}
