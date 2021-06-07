using System.Linq;

namespace VRage
{
	public static class Partition
	{
		private static readonly string[] m_letters = (from s in Enumerable.Range(65, 26)
			select new string((char)s, 1)).ToArray();

		public static T Select<T>(int num, T a, T b)
		{
			if (num % 2 != 0)
			{
				return b;
			}
			return a;
		}

		public static T Select<T>(int num, T a, T b, T c)
		{
			switch ((uint)num % 3u)
			{
			default:
				return c;
			case 1u:
				return b;
			case 0u:
				return a;
			}
		}

		public static T Select<T>(int num, T a, T b, T c, T d)
		{
			switch ((uint)num % 4u)
			{
			default:
				return d;
			case 2u:
				return c;
			case 1u:
				return b;
			case 0u:
				return a;
			}
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e)
		{
			switch ((uint)num % 5u)
			{
			default:
				return e;
			case 3u:
				return d;
			case 2u:
				return c;
			case 1u:
				return b;
			case 0u:
				return a;
			}
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e, T f)
		{
			switch ((uint)num % 6u)
			{
			case 0u:
				return a;
			case 1u:
				return b;
			case 2u:
				return c;
			case 3u:
				return d;
			case 4u:
				return e;
			default:
				return f;
			}
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e, T f, T g)
		{
			switch ((uint)num % 7u)
			{
			case 0u:
				return a;
			case 1u:
				return b;
			case 2u:
				return c;
			case 3u:
				return d;
			case 4u:
				return e;
			case 5u:
				return f;
			default:
				return g;
			}
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e, T f, T g, T h)
		{
			switch ((uint)num % 8u)
			{
			case 0u:
				return a;
			case 1u:
				return b;
			case 2u:
				return c;
			case 3u:
				return d;
			case 4u:
				return e;
			case 5u:
				return f;
			case 6u:
				return g;
			default:
				return h;
			}
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e, T f, T g, T h, T i)
		{
			switch ((uint)num % 9u)
			{
			case 0u:
				return a;
			case 1u:
				return b;
			case 2u:
				return c;
			case 3u:
				return d;
			case 4u:
				return e;
			case 5u:
				return f;
			case 6u:
				return g;
			case 7u:
				return h;
			default:
				return i;
			}
		}

		public static string SelectStringByLetter(char c)
		{
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
			{
				c = char.ToUpperInvariant(c);
				return m_letters[c - 65];
			}
			if (c >= '0' && c <= '9')
			{
				return "0-9";
			}
			return "Non-letter";
		}

		public static string SelectStringGroupOfTenByLetter(char c)
		{
			c = char.ToUpperInvariant(c);
			if (c >= '0' && c <= '9')
			{
				return "0-9";
			}
			switch (c)
			{
			case 'A':
			case 'B':
			case 'C':
				return "A-C";
			case 'D':
			case 'E':
			case 'F':
				return "D-F";
			case 'G':
			case 'H':
			case 'I':
				return "G-I";
			case 'J':
			case 'K':
			case 'L':
				return "J-L";
			case 'M':
			case 'N':
			case 'O':
				return "M-O";
			case 'P':
			case 'Q':
			case 'R':
				return "P-R";
			case 'S':
			case 'T':
			case 'U':
			case 'V':
				return "S-V";
			case 'W':
			case 'X':
			case 'Y':
			case 'Z':
				return "W-Z";
			default:
				return "Non-letter";
			}
		}
	}
}
