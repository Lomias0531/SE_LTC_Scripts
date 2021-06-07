using Sandbox.Graphics;

namespace System.Text
{
	public static class StringBuilderExtensions
	{
		[ThreadStatic]
		private static StringBuilder m_tmp;

		public static void Autowrap(this StringBuilder sb, float width, string font, float textScale)
		{
			int num = 0;
			int num2 = 0;
			if (m_tmp == null)
			{
				m_tmp = new StringBuilder(sb.Length);
			}
			m_tmp.Clear();
			while (true)
			{
				int length = m_tmp.Length;
				int num3 = num;
				num = AppendWord(sb, m_tmp, num);
				if (num == num3)
				{
					break;
				}
				num2++;
				if (!(MyGuiManager.MeasureString(font, m_tmp, textScale).X <= width))
				{
					if (num2 == 1)
					{
						m_tmp.AppendLine();
						num = MoveSpaces(sb, num);
						num2 = 0;
						continue;
					}
					m_tmp.Length = length;
					num = num3;
					m_tmp.AppendLine();
					num = MoveSpaces(sb, num);
					num2 = 0;
					width = MyGuiManager.MeasureString(font, m_tmp, textScale).X;
				}
			}
			sb.Clear().AppendStringBuilder(m_tmp);
		}

		private static int MoveSpaces(StringBuilder from, int pos)
		{
			while (pos < from.Length && from[pos] == ' ')
			{
				pos++;
			}
			return pos;
		}

		private static int AppendWord(StringBuilder from, StringBuilder to, int wordPos)
		{
			int i = wordPos;
			bool flag = false;
			for (; i < from.Length; i++)
			{
				char c = from[i];
				bool flag2 = c == ' ' || c == '\r' || c == '\n';
				if (!flag2 && flag)
				{
					break;
				}
				flag = (flag || flag2);
				to.Append(c);
			}
			return i;
		}

		public static bool EqualsStrFast(this string str, StringBuilder sb)
		{
			return sb.EqualsStrFast(str);
		}

		public static bool EqualsStrFast(this StringBuilder sb, string str)
		{
			if (sb.Length != str.Length)
			{
				return false;
			}
			for (int i = 0; i < str.Length; i++)
			{
				if (sb[i] != str[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
