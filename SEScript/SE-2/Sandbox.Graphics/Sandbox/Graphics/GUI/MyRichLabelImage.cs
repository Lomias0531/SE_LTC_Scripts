using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyRichLabelImage : MyRichLabelPart
	{
		private string m_texture;

		private Vector4 m_color;

		private Vector2 m_size;

		public string Texture
		{
			get
			{
				return m_texture;
			}
			set
			{
				m_texture = value;
			}
		}

		public Vector4 Color
		{
			get
			{
				return m_color;
			}
			set
			{
				m_color = value;
			}
		}

		public new Vector2 Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		public MyRichLabelImage(string texture, Vector2 size, Vector4 color)
		{
			m_texture = texture;
			m_size = size;
			m_color = color;
		}

		public override bool Draw(Vector2 position, float alphamask, ref int charactersLeft)
		{
			Vector4 color = m_color;
			color *= alphamask;
			MyGuiManager.DrawSpriteBatch(m_texture, position, m_size, new Color(color), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			charactersLeft--;
			return true;
		}

		public override bool HandleInput(Vector2 position)
		{
			return false;
		}
	}
}
