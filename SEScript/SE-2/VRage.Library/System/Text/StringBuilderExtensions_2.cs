using System.Collections.Generic;
using System.Globalization;
using VRage;

namespace System.Text
{
	public static class StringBuilderExtensions_2
	{
		private static NumberFormatInfo m_numberFormatInfoHelper;

		static StringBuilderExtensions_2()
		{
			if (m_numberFormatInfoHelper == null)
			{
				m_numberFormatInfoHelper = (CultureInfo.InvariantCulture.NumberFormat.Clone() as NumberFormatInfo);
			}
		}

		/// <summary>
		/// Compares string builder with text,
		/// when it's different, Clears string builder and Appends text.
		/// Returns true when original string builder was modified.
		/// </summary>
		public static bool CompareUpdate(this StringBuilder sb, string text)
		{
			if (sb.CompareTo(text) != 0)
			{
				sb.Clear();
				sb.Append(text);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Compares string builder with text,
		/// when it's different, Clears string builder and Appends text.
		/// Returns true when original string builder was modified.
		/// </summary>
		public static bool CompareUpdate(this StringBuilder sb, StringBuilder text)
		{
			if (sb.CompareTo(text) != 0)
			{
				sb.Clear();
				sb.AppendStringBuilder(text);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the specified number of characters from the end.
		/// </summary>
		/// <param name="sb">The sb.</param>
		/// <param name="length">The length.</param>
		public static void TrimEnd(this StringBuilder sb, int length)
		{
			Exceptions.ThrowIf<ArgumentException>(length > sb.Length, "String builder contains less characters then requested number!");
			sb.Length -= length;
		}

		public static StringBuilder GetFormatedLong(this StringBuilder sb, string before, long value, string after = "")
		{
			sb.Clear();
			sb.ConcatFormat("{0}{1: #,0}{2}", before, value, after);
			return sb;
		}

		public static StringBuilder GetFormatedInt(this StringBuilder sb, string before, int value, string after = "")
		{
			sb.Clear();
			sb.ConcatFormat("{0}{1: #,0}{2}", before, value, after);
			return sb;
		}

		public static StringBuilder GetFormatedFloat(this StringBuilder sb, string before, float value, string after = "")
		{
			sb.Clear();
			sb.ConcatFormat("{0}{1: #,0}{2}", before, value, after);
			return sb;
		}

		public static StringBuilder GetFormatedBool(this StringBuilder sb, string before, bool value, string after = "")
		{
			sb.Clear();
			sb.ConcatFormat("{0}{1}{2}", before, value, after);
			return sb;
		}

		public static StringBuilder GetFormatedDateTimeOffset(this StringBuilder sb, string before, DateTimeOffset value, string after = "")
		{
			return sb.GetFormatedDateTimeOffset(before, value.DateTime, after);
		}

		public static StringBuilder GetFormatedDateTimeOffset(this StringBuilder sb, string before, DateTime value, string after = "")
		{
			sb.Clear();
			sb.Append(before);
			sb.Concat(value.Year, 4u, '0', 10u, thousandSeparation: false);
			sb.Append("-");
			sb.Concat(value.Month, 2u);
			sb.Append("-");
			sb.Concat(value.Day, 2u);
			sb.Append(" ");
			sb.Concat(value.Hour, 2u);
			sb.Append(":");
			sb.Concat(value.Minute, 2u);
			sb.Append(":");
			sb.Concat(value.Second, 2u);
			sb.Append(".");
			sb.Concat(value.Millisecond, 3u);
			sb.Append(after);
			return sb;
		}

		public static StringBuilder GetFormatedDateTime(this StringBuilder sb, DateTime value)
		{
			sb.Clear();
			sb.Concat(value.Day, 2u);
			sb.Append("/");
			sb.Concat(value.Month, 2u);
			sb.Append("/");
			sb.Concat(value.Year, 0u, '0', 10u, thousandSeparation: false);
			sb.Append(" ");
			sb.Concat(value.Hour, 2u);
			sb.Append(":");
			sb.Concat(value.Minute, 2u);
			sb.Append(":");
			sb.Concat(value.Second, 2u);
			return sb;
		}

		public static StringBuilder GetFormatedDateTimeForFilename(this StringBuilder sb, DateTime value)
		{
			sb.Clear();
			sb.Concat(value.Year, 0u, '0', 10u, thousandSeparation: false);
			sb.Concat(value.Month, 2u);
			sb.Concat(value.Day, 2u);
			sb.Append("_");
			sb.Concat(value.Hour, 2u);
			sb.Concat(value.Minute, 2u);
			sb.Concat(value.Second, 2u);
			return sb;
		}

		public static StringBuilder AppendFormatedDateTime(this StringBuilder sb, DateTime value)
		{
			sb.Concat(value.Day, 2u);
			sb.Append("/");
			sb.Concat(value.Month, 2u);
			sb.Append("/");
			sb.Concat(value.Year, 0u, '0', 10u, thousandSeparation: false);
			sb.Append(" ");
			sb.Concat(value.Hour, 2u);
			sb.Append(":");
			sb.Concat(value.Minute, 2u);
			sb.Append(":");
			sb.Concat(value.Second, 2u);
			return sb;
		}

		public static StringBuilder GetFormatedTimeSpan(this StringBuilder sb, string before, TimeSpan value, string after = "")
		{
			sb.Clear();
			sb.Clear();
			sb.Append(before);
			sb.Concat(value.Hours, 2u);
			sb.Append(":");
			sb.Concat(value.Minutes, 2u);
			sb.Append(":");
			sb.Concat(value.Seconds, 2u);
			sb.Append(".");
			sb.Concat(value.Milliseconds, 3u);
			sb.Append(after);
			return sb;
		}

		public static StringBuilder GetStrings(this StringBuilder sb, string before, string value = "", string after = "")
		{
			sb.Clear();
			sb.ConcatFormat("{0}{1}{2}", before, value, after);
			return sb;
		}

		public static StringBuilder AppendFormatedDecimal(this StringBuilder sb, string before, float value, int decimalDigits, string after = "")
		{
			sb.Clear();
			m_numberFormatInfoHelper.NumberDecimalDigits = decimalDigits;
			sb.ConcatFormat("{0}{1 }{2}", before, value, after, m_numberFormatInfoHelper);
			return sb;
		}

		public static StringBuilder AppendInt64(this StringBuilder sb, long number)
		{
			sb.ConcatFormat("{0}", number);
			return sb;
		}

		public static StringBuilder AppendInt32(this StringBuilder sb, int number)
		{
			sb.ConcatFormat("{0}", number);
			return sb;
		}

		public static int GetDecimalCount(float number, int validDigitCount)
		{
			int num = validDigitCount;
			while (number >= 1f && num > 0)
			{
				number /= 10f;
				num--;
			}
			return num;
		}

		public static int GetDecimalCount(double number, int validDigitCount)
		{
			int num = validDigitCount;
			while (number >= 1.0 && num > 0)
			{
				number /= 10.0;
				num--;
			}
			return num;
		}

		public static int GetDecimalCount(decimal number, int validDigitCount)
		{
			int num = validDigitCount;
			while (number >= 1m && num > 0)
			{
				number /= 10m;
				num--;
			}
			return num;
		}

		public static StringBuilder AppendDecimalDigit(this StringBuilder sb, float number, int validDigitCount)
		{
			return sb.AppendDecimal(number, GetDecimalCount(number, validDigitCount));
		}

		public static StringBuilder AppendDecimalDigit(this StringBuilder sb, double number, int validDigitCount)
		{
			return sb.AppendDecimal(number, GetDecimalCount(number, validDigitCount));
		}

		public static StringBuilder AppendDecimal(this StringBuilder sb, float number, int decimals)
		{
			m_numberFormatInfoHelper.NumberDecimalDigits = Math.Max(0, Math.Min(decimals, 99));
			sb.ConcatFormat("{0}", number, m_numberFormatInfoHelper);
			return sb;
		}

		public static StringBuilder AppendDecimal(this StringBuilder sb, double number, int decimals)
		{
			m_numberFormatInfoHelper.NumberDecimalDigits = Math.Max(0, Math.Min(decimals, 99));
			sb.ConcatFormat("{0}", number, m_numberFormatInfoHelper);
			return sb;
		}

		public static List<StringBuilder> Split(this StringBuilder sb, char separator)
		{
			List<StringBuilder> list = new List<StringBuilder>();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < sb.Length; i++)
			{
				if (sb[i] == separator)
				{
					list.Add(stringBuilder);
					stringBuilder = new StringBuilder();
				}
				else
				{
					stringBuilder.Append(sb[i]);
				}
			}
			if (stringBuilder.Length > 0)
			{
				list.Add(stringBuilder);
			}
			return list;
		}

		/// <summary>
		/// Removes whitespace from the end.
		/// </summary>
		public static StringBuilder TrimTrailingWhitespace(this StringBuilder sb)
		{
			int num = sb.Length;
			while (num > 0 && (sb[num - 1] == ' ' || sb[num - 1] == '\r' || sb[num - 1] == '\n'))
			{
				num--;
			}
			sb.Length = num;
			return sb;
		}

		public static int CompareTo(this StringBuilder self, StringBuilder other)
		{
			int num = 0;
			int num2;
			while (true)
			{
				bool flag = num < self.Length;
				bool flag2 = num < other.Length;
				if (!(flag || flag2))
				{
					return 0;
				}
				if (!flag)
				{
					return -1;
				}
				if (!flag2)
				{
					return 1;
				}
				num2 = self[num].CompareTo(other[num]);
				if (num2 != 0)
				{
					break;
				}
				num++;
			}
			return num2;
		}

		public static int CompareTo(this StringBuilder self, string other)
		{
			int num = 0;
			int num2;
			while (true)
			{
				bool flag = num < self.Length;
				bool flag2 = num < other.Length;
				if (!(flag || flag2))
				{
					return 0;
				}
				if (!flag)
				{
					return -1;
				}
				if (!flag2)
				{
					return 1;
				}
				num2 = self[num].CompareTo(other[num]);
				if (num2 != 0)
				{
					break;
				}
				num++;
			}
			return num2;
		}

		public static int CompareToIgnoreCase(this StringBuilder self, StringBuilder other)
		{
			int num = 0;
			int num2;
			while (true)
			{
				bool flag = num < self.Length;
				bool flag2 = num < other.Length;
				if (!(flag || flag2))
				{
					return 0;
				}
				if (!flag)
				{
					return -1;
				}
				if (!flag2)
				{
					return 1;
				}
				num2 = char.ToLowerInvariant(self[num]).CompareTo(char.ToLowerInvariant(other[num]));
				if (num2 != 0)
				{
					break;
				}
				num++;
			}
			return num2;
		}
	}
}
