using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyColoredText
	{
		private float m_scale;

		public StringBuilder Text
		{
			get;
			private set;
		}

		public Color NormalColor
		{
			get;
			private set;
		}

		public Color HighlightColor
		{
			get;
			private set;
		}

		public string Font
		{
			get;
			private set;
		}

		public Vector2 Offset
		{
			get;
			private set;
		}

		public float ScaleWithLanguage
		{
			get;
			private set;
		}

		public Vector2 Size
		{
			get;
			private set;
		}

		public float Scale
		{
			get
			{
				return m_scale;
			}
			private set
			{
				m_scale = value;
				ScaleWithLanguage = value * MyGuiManager.LanguageTextScale;
				Size = MyGuiManager.MeasureString(Font, Text, ScaleWithLanguage);
			}
		}

		public MyColoredText(string text, Color? normalColor = null, Color? highlightColor = null, string font = "White", float textScale = 0.75f, Vector2? offset = null)
		{
			Text = new StringBuilder(text.Length).Append(text);
			NormalColor = (normalColor ?? MyGuiConstants.COLORED_TEXT_DEFAULT_COLOR);
			HighlightColor = (highlightColor ?? MyGuiConstants.COLORED_TEXT_DEFAULT_HIGHLIGHT_COLOR);
			Font = font;
			Scale = textScale;
			Offset = (offset ?? Vector2.Zero);
		}

		public void Draw(Vector2 normalizedPosition, MyGuiDrawAlignEnum drawAlign, float backgroundAlphaFade, bool isHighlight, float colorMultiplicator = 1f)
		{
			Vector4 vector = (isHighlight ? HighlightColor : NormalColor).ToVector4();
			vector.W *= backgroundAlphaFade;
			vector *= colorMultiplicator;
			MyGuiManager.DrawString(Font, Text, normalizedPosition + Offset, ScaleWithLanguage, new Color(vector), drawAlign);
		}

		public void Draw(Vector2 normalizedPosition, MyGuiDrawAlignEnum drawAlign, float backgroundAlphaFade, float colorMultiplicator = 1f)
		{
			Draw(normalizedPosition, drawAlign, backgroundAlphaFade, isHighlight: false, colorMultiplicator);
		}
	}
}
