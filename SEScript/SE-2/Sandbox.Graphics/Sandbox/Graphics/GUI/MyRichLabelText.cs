using System;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyRichLabelText : MyRichLabelPart
	{
		protected StringBuilder m_tmpText = new StringBuilder();

		private StringBuilder m_text;

		private string m_font;

		private Vector4 m_color;

		private float m_scale;

		private bool m_showTextShadow;

		public StringBuilder Text => m_text;

		public bool ShowTextShadow
		{
			get
			{
				return m_showTextShadow;
			}
			set
			{
				m_showTextShadow = value;
			}
		}

		public float Scale
		{
			get
			{
				return m_scale;
			}
			set
			{
				m_scale = value;
				RecalculateSize();
			}
		}

		public string Font
		{
			get
			{
				return m_font;
			}
			set
			{
				m_font = value;
				RecalculateSize();
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

		public string Tag
		{
			get;
			set;
		}

		public MyRichLabelText(StringBuilder text, string font, float scale, Vector4 color)
		{
			m_text = text;
			m_font = font;
			m_scale = scale;
			m_color = color;
			RecalculateSize();
		}

		public MyRichLabelText()
		{
			m_text = new StringBuilder(512);
			m_font = "Blue";
			m_scale = 0f;
			m_color = Vector4.Zero;
		}

		public void Init(string text, string font, float scale, Vector4 color)
		{
			m_text.Append(text);
			m_font = font;
			m_scale = scale;
			m_color = color;
			RecalculateSize();
		}

		public void Append(string text)
		{
			m_text.Append(text);
			RecalculateSize();
		}

		public override void AppendTextTo(StringBuilder builder)
		{
			builder.Append((object)m_text);
		}

		public override bool Draw(Vector2 position, float alphamask, ref int charactersLeft)
		{
			string value = m_text.ToString(0, Math.Min(m_text.Length, charactersLeft));
			charactersLeft -= m_text.Length;
			if (ShowTextShadow && !string.IsNullOrWhiteSpace(value))
			{
				Vector2 textSize = Size;
				MyGuiTextShadows.DrawShadow(ref position, ref textSize, null, alphamask, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
			Vector4 color = m_color;
			color *= alphamask;
			m_tmpText.Clear();
			m_tmpText.Append(value);
			MyGuiManager.DrawString(m_font, m_tmpText, position, m_scale, new Color(color));
			return true;
		}

		public override bool HandleInput(Vector2 position)
		{
			return false;
		}

		private void RecalculateSize()
		{
			Size = MyGuiManager.MeasureString(m_font, m_text, m_scale);
		}
	}
}
