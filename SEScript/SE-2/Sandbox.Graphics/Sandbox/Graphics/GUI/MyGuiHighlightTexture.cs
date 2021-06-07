using VRageMath;
using VRageRender;

namespace Sandbox.Graphics.GUI
{
	public struct MyGuiHighlightTexture
	{
		private string m_highlight;

		private string m_normal;

		private Vector2 m_sizePx;

		public Vector2 HighlightSize;

		public Vector2 NormalSize;

		public string Normal
		{
			get
			{
				return m_highlight;
			}
			set
			{
				m_highlight = value;
				HighlightSize = MyRenderProxy.GetTextureSize(m_highlight);
			}
		}

		public string Highlight
		{
			get
			{
				return m_normal;
			}
			set
			{
				m_normal = value;
				NormalSize = MyRenderProxy.GetTextureSize(m_normal);
			}
		}

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
			get;
			private set;
		}
	}
}
