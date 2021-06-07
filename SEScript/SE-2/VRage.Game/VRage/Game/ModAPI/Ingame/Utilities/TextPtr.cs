using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	[DebuggerDisplay("{ToDebugString(),nq}")]
	public struct TextPtr
	{
		public readonly string Content;

		public readonly int Index;

		public char Char
		{
			get
			{
				if (!IsOutOfBounds())
				{
					return Content[Index];
				}
				return '\0';
			}
		}

		public bool IsEmpty => Content != null;

		public static implicit operator int(TextPtr ptr)
		{
			return ptr.Index;
		}

		public static implicit operator string(TextPtr ptr)
		{
			return ptr.Content;
		}

		public static TextPtr operator +(TextPtr ptr, int offset)
		{
			return new TextPtr(ptr.Content, ptr.Index + offset);
		}

		public static TextPtr operator ++(TextPtr ptr)
		{
			return new TextPtr(ptr.Content, ptr.Index + 1);
		}

		public static TextPtr operator -(TextPtr ptr, int offset)
		{
			return new TextPtr(ptr.Content, ptr.Index - offset);
		}

		public static TextPtr operator --(TextPtr ptr)
		{
			return new TextPtr(ptr.Content, ptr.Index - 1);
		}

		public TextPtr(string content)
		{
			Content = content;
			Index = 0;
		}

		public TextPtr(string content, int index)
		{
			Content = content;
			Index = index;
		}

		public bool IsOutOfBounds()
		{
			if (Index >= 0)
			{
				return Index >= Content.Length;
			}
			return true;
		}

		public int FindLineNo()
		{
			string content = Content;
			int index = Index;
			int num = 1;
			for (int i = 0; i < index; i++)
			{
				if (content[i] == '\n')
				{
					num++;
				}
			}
			return num;
		}

		public TextPtr Find(string str)
		{
			if (IsOutOfBounds())
			{
				return this;
			}
			int num = Content.IndexOf(str, Index, StringComparison.InvariantCulture);
			if (num == -1)
			{
				return new TextPtr(Content, Content.Length);
			}
			return new TextPtr(Content, num);
		}

		public TextPtr Find(char ch)
		{
			if (IsOutOfBounds())
			{
				return this;
			}
			int num = Content.IndexOf(ch, Index);
			if (num == -1)
			{
				return new TextPtr(Content, Content.Length);
			}
			return new TextPtr(Content, num);
		}

		public TextPtr FindInLine(char ch)
		{
			if (IsOutOfBounds())
			{
				return this;
			}
			string content = Content;
			int length = Content.Length;
			for (int i = Index; i < length; i++)
			{
				char c = content[i];
				if (c == ch)
				{
					return new TextPtr(content, i);
				}
				if (c == '\r' || c == '\n')
				{
					break;
				}
			}
			return new TextPtr(Content, Content.Length);
		}

		public TextPtr FindEndOfLine(bool skipNewline = false)
		{
			int length = Content.Length;
			if (Index >= length)
			{
				return this;
			}
			TextPtr result = this;
			while (result.Index < length)
			{
				if (result.IsNewLine())
				{
					if (skipNewline)
					{
						if (result.Char == '\r')
						{
							++result;
						}
						++result;
					}
					break;
				}
				++result;
			}
			return result;
		}

		public bool StartsWithCaseInsensitive(string what)
		{
			TextPtr textPtr = this;
			foreach (char c in what)
			{
				if (char.ToUpper(textPtr.Char) != char.ToUpper(c))
				{
					return false;
				}
				++textPtr;
			}
			return true;
		}

		public bool StartsWith(string what)
		{
			TextPtr textPtr = this;
			foreach (char c in what)
			{
				if (textPtr.Char != c)
				{
					return false;
				}
				++textPtr;
			}
			return true;
		}

		public TextPtr SkipWhitespace(bool skipNewline = false)
		{
			TextPtr result = this;
			int length = result.Content.Length;
			if (skipNewline)
			{
				while (true)
				{
					char @char = result.Char;
					if (result.Index >= length || !char.IsWhiteSpace(@char))
					{
						break;
					}
					++result;
				}
				return result;
			}
			while (true)
			{
				char char2 = result.Char;
				if (result.Index >= length || result.IsNewLine() || !char.IsWhiteSpace(char2))
				{
					break;
				}
				++result;
			}
			return result;
		}

		public bool IsEndOfLine()
		{
			if (Index < Content.Length)
			{
				return IsNewLine();
			}
			return true;
		}

		public bool IsStartOfLine()
		{
			if (Index > 0)
			{
				return (this - 1).IsNewLine();
			}
			return true;
		}

		public bool IsNewLine()
		{
			switch (Char)
			{
			case '\r':
				if (Index < Content.Length - 1)
				{
					return Content[Index + 1] == '\n';
				}
				return false;
			case '\n':
				return true;
			default:
				return false;
			}
		}

		public TextPtr TrimStart()
		{
			string content = Content;
			int i = Index;
			for (int length = content.Length; i < length; i++)
			{
				char c = content[i];
				if (c != ' ' && c != '\t')
				{
					break;
				}
			}
			return new TextPtr(content, i);
		}

		public TextPtr TrimEnd()
		{
			string content = Content;
			int num;
			for (num = Index - 1; num >= 0; num--)
			{
				char c = content[num];
				if (c != ' ' && c != '\t')
				{
					break;
				}
			}
			return new TextPtr(content, num + 1);
		}

		private string ToDebugString()
		{
			if (Index < 0)
			{
				return "<before>";
			}
			if (Index >= Content.Length)
			{
				return "<after>";
			}
			int num = Index + 37;
			string input = (num <= Content.Length) ? (Content.Substring(Index, num - Index) + "...") : Content.Substring(Index, Content.Length - Index);
			return Regex.Replace(input, "[\\r\\t\\n]", delegate(Match match)
			{
				switch (match.Value)
				{
				case "\r":
					return "\\r";
				case "\t":
					return "\\t";
				case "\n":
					return "\\n";
				default:
					return match.Value;
				}
			});
		}
	}
}
