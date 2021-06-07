using System.Collections.Generic;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyHudText
	{
		public class ComparerType : IComparer<MyHudText>
		{
			public int Compare(MyHudText x, MyHudText y)
			{
				return x.Font.CompareTo(y.Font);
			}
		}

		public static readonly ComparerType Comparer = new ComparerType();

		public string Font;

		public Vector2 Position;

		public Color Color;

		public float Scale;

		public MyGuiDrawAlignEnum Alignement;

		public bool Visible;

		private readonly StringBuilder m_text;

		public MyHudText()
		{
			m_text = new StringBuilder(256);
		}

		public MyHudText Start(string font, Vector2 position, Color color, float scale, MyGuiDrawAlignEnum alignement)
		{
			Font = font;
			Position = position;
			Color = color;
			Scale = scale;
			Alignement = alignement;
			m_text.Clear();
			return this;
		}

		public void Append(StringBuilder sb)
		{
			m_text.AppendStringBuilder(sb);
		}

		public void Append(string text)
		{
			m_text.Append(text);
		}

		public void AppendInt32(int number)
		{
			m_text.AppendInt32(number);
		}

		public void AppendLine()
		{
			m_text.AppendLine();
		}

		public StringBuilder GetStringBuilder()
		{
			return m_text;
		}
	}
}
