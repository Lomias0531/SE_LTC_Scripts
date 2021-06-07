using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	public struct StringSegment
	{
		private static readonly char[] NEWLINE_CHARS = new char[2]
		{
			'\r',
			'\n'
		};

		public readonly string Text;

		public readonly int Start;

		public readonly int Length;

		private string m_cache;

		public bool IsEmpty => Text == null;

		public bool IsCached => m_cache != null;

		public char this[int i]
		{
			get
			{
				if (i < 0 || i >= Length)
				{
					return '\0';
				}
				return Text[Start + i];
			}
		}

		public StringSegment(string text)
			: this(text, 0, text.Length)
		{
			m_cache = text;
		}

		public StringSegment(string text, int start, int length)
		{
			Text = text;
			Start = start;
			Length = Math.Max(0, length);
			m_cache = null;
		}

		public int IndexOf(char ch)
		{
			if (Length == 0)
			{
				return -1;
			}
			return Text.IndexOf(ch, Start, Length);
		}

		public int IndexOf(char ch, int start)
		{
			if (Length == 0)
			{
				return -1;
			}
			return Text.IndexOf(ch, Start + start, Length);
		}

		public int IndexOfAny(char[] chars)
		{
			if (Length == 0)
			{
				return -1;
			}
			return Text.IndexOfAny(chars, Start, Length);
		}

		public bool EqualsIgnoreCase(StringSegment other)
		{
			if (Length != other.Length)
			{
				return false;
			}
			string text = Text;
			int num = Start;
			string text2 = other.Text;
			int num2 = other.Start;
			for (int i = 0; i < Length; i++)
			{
				if (char.ToUpperInvariant(text[num]) != char.ToUpperInvariant(text2[num2]))
				{
					return false;
				}
				num++;
				num2++;
			}
			return true;
		}

		public override string ToString()
		{
			if (Length == 0)
			{
				return "";
			}
			if (m_cache == null)
			{
				m_cache = Text.Substring(Start, Length);
			}
			return m_cache;
		}

		public void GetLines(List<string> lines)
		{
			if (Length == 0)
			{
				return;
			}
			string text = Text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			int num = Start;
			int num2 = num + Length;
			lines.Clear();
			while (true)
			{
				if (num < num2)
				{
					int num3 = text.IndexOfAny(NEWLINE_CHARS, num, num2 - num);
					if (num3 < 0)
					{
						break;
					}
					lines.Add(text.Substring(num, num3 - num));
					num = num3;
					if (num < text.Length && text[num] == '\r')
					{
						num++;
					}
					if (num < text.Length && text[num] == '\n')
					{
						num++;
					}
					continue;
				}
				return;
			}
			lines.Add(text.Substring(num, text.Length - num));
		}
	}
}
