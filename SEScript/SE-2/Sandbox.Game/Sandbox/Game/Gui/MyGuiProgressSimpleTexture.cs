using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
	public class MyGuiProgressSimpleTexture
	{
		private bool m_dirty;

		private MyObjectBuilder_GuiTexture m_backgroundTexture;

		private MyObjectBuilder_GuiTexture m_progressBarTexture;

		private Vector2I m_size;

		private Vector2I m_barSize;

		private Vector2I m_progressBarTextureOffset;

		private Vector2 m_zeroOrigin = Vector2.Zero;

		public MyObjectBuilder_GuiTexture BackgroundTexture
		{
			get
			{
				return m_backgroundTexture;
			}
			set
			{
				m_backgroundTexture = value;
				m_dirty = true;
			}
		}

		public MyObjectBuilder_GuiTexture ProgressBarTexture
		{
			get
			{
				return m_progressBarTexture;
			}
			set
			{
				m_progressBarTexture = value;
				m_dirty = true;
			}
		}

		public Vector2I Size
		{
			get
			{
				return m_size;
			}
			set
			{
				m_size = value;
				m_dirty = true;
			}
		}

		public Vector2I ProgressBarTextureOffset
		{
			get
			{
				return m_progressBarTextureOffset;
			}
			set
			{
				m_progressBarTextureOffset = value;
				m_dirty = true;
			}
		}

		public Vector4 BackgroundColorMask
		{
			get;
			set;
		}

		public Vector4 ProgressBarColorMask
		{
			get;
			set;
		}

		public Vector2I Position
		{
			get;
			set;
		}

		public bool Inverted
		{
			get;
			set;
		}

		public void Draw(float progression, Color backgroundColorMask, Color progressColorMask)
		{
			Rectangle? sourceRectangle = null;
			RectangleF destination = default(RectangleF);
			progression = MyMath.Clamp(progression, 0f, 1f);
			if (m_dirty)
			{
				RecalculateInternals();
			}
			destination.X = Position.X;
			destination.Y = Position.Y;
			destination.Width = m_size.X;
			destination.Height = m_size.Y;
			MyRenderProxy.DrawSprite(m_backgroundTexture.Path, ref destination, sourceRectangle, backgroundColorMask, 0f);
			Vector2I vector2I = Position + m_progressBarTextureOffset;
			if (Inverted)
			{
				destination.X = (float)vector2I.X + (float)m_barSize.X * (1f - progression);
				destination.Y = vector2I.Y;
				destination.Width = (int)((float)m_barSize.X * progression);
				destination.Height = m_barSize.Y;
				sourceRectangle = new Rectangle((int)((float)m_progressBarTexture.SizePx.X * (1f - progression)), 0, (int)((float)m_progressBarTexture.SizePx.X * progression), m_progressBarTexture.SizePx.Y);
			}
			else
			{
				destination.X = vector2I.X;
				destination.Y = vector2I.Y;
				destination.Width = (int)((float)m_barSize.X * progression);
				destination.Height = m_barSize.Y;
				sourceRectangle = new Rectangle(0, 0, (int)((float)m_progressBarTexture.SizePx.X * progression), m_progressBarTexture.SizePx.Y);
			}
			MyRenderProxy.DrawSprite(m_progressBarTexture.Path, ref destination, sourceRectangle, progressColorMask, 0f);
		}

		private void RecalculateInternals()
		{
			Vector2 value = new Vector2((float)m_size.X / (float)m_backgroundTexture.SizePx.X, (float)m_size.Y / (float)m_backgroundTexture.SizePx.Y);
			m_barSize = new Vector2I(m_progressBarTexture.SizePx * value);
			m_progressBarTextureOffset = new Vector2I(m_progressBarTextureOffset * value);
			m_dirty = false;
		}
	}
}
