using System.Globalization;

namespace System.Text
{
	public static class StringBuilderExtensions_Format
	{
		public static StringBuilder AppendStringBuilder(this StringBuilder stringBuilder, StringBuilder otherStringBuilder)
		{
			stringBuilder.EnsureCapacity(stringBuilder.Length + otherStringBuilder.Length);
			for (int i = 0; i < otherStringBuilder.Length; i++)
			{
				stringBuilder.Append(otherStringBuilder[i]);
			}
			return stringBuilder;
		}

		public static StringBuilder AppendSubstring(this StringBuilder stringBuilder, StringBuilder append, int start, int count)
		{
			stringBuilder.EnsureCapacity(stringBuilder.Length + count);
			for (int i = 0; i < count; i++)
			{
				stringBuilder.Append(append[start + i]);
			}
			return stringBuilder;
		}

		public static StringBuilder ConcatFormat<A>(this StringBuilder string_builder, string format_string, A arg1, NumberFormatInfo numberFormat = null) where A : IConvertible
		{
			return string_builder.ConcatFormat(format_string, arg1, 0, 0, 0, 0, numberFormat);
		}

		public static StringBuilder ConcatFormat<A, B>(this StringBuilder string_builder, string format_string, A arg1, B arg2, NumberFormatInfo numberFormat = null) where A : IConvertible where B : IConvertible
		{
			return string_builder.ConcatFormat(format_string, arg1, arg2, 0, 0, 0, numberFormat);
		}

		public static StringBuilder ConcatFormat<A, B, C>(this StringBuilder string_builder, string format_string, A arg1, B arg2, C arg3, NumberFormatInfo numberFormat = null) where A : IConvertible where B : IConvertible where C : IConvertible
		{
			return string_builder.ConcatFormat(format_string, arg1, arg2, arg3, 0, 0, numberFormat);
		}

		public static StringBuilder ConcatFormat<A, B, C, D>(this StringBuilder string_builder, string format_string, A arg1, B arg2, C arg3, D arg4, NumberFormatInfo numberFormat = null) where A : IConvertible where B : IConvertible where C : IConvertible where D : IConvertible
		{
			return string_builder.ConcatFormat(format_string, arg1, arg2, arg3, arg4, 0, numberFormat);
		}

		public static StringBuilder ConcatFormat<A, B, C, D, E>(this StringBuilder string_builder, string format_string, A arg1, B arg2, C arg3, D arg4, E arg5, NumberFormatInfo numberFormat = null) where A : IConvertible where B : IConvertible where C : IConvertible where D : IConvertible where E : IConvertible
		{
			int num = 0;
			numberFormat = (numberFormat ?? CultureInfo.InvariantCulture.NumberFormat);
			for (int i = 0; i < format_string.Length; i++)
			{
				if (format_string[i] != '{')
				{
					continue;
				}
				if (num < i)
				{
					string_builder.Append(format_string, num, i - num);
				}
				uint base_value = 10u;
				uint num2 = 0u;
				uint num3 = (uint)numberFormat.NumberDecimalDigits;
				bool thousandSeparation = !string.IsNullOrEmpty(numberFormat.NumberGroupSeparator);
				i++;
				char c = format_string[i];
				if (c == '{')
				{
					string_builder.Append('{');
					i++;
				}
				else
				{
					i++;
					if (format_string[i] == ':')
					{
						i++;
						while (format_string[i] == '0')
						{
							i++;
							num2++;
						}
						if (format_string[i] == 'X')
						{
							i++;
							base_value = 16u;
							if (format_string[i] >= '0' && format_string[i] <= '9')
							{
								num2 = (uint)(format_string[i] - 48);
								i++;
							}
						}
						else if (format_string[i] == '.')
						{
							i++;
							num3 = 0u;
							while (format_string[i] == '0')
							{
								i++;
								num3++;
							}
						}
					}
					for (; format_string[i] != '}'; i++)
					{
					}
					switch (c)
					{
					case '0':
						string_builder.ConcatFormatValue(arg1, num2, base_value, num3, thousandSeparation);
						break;
					case '1':
						string_builder.ConcatFormatValue(arg2, num2, base_value, num3, thousandSeparation);
						break;
					case '2':
						string_builder.ConcatFormatValue(arg3, num2, base_value, num3, thousandSeparation);
						break;
					case '3':
						string_builder.ConcatFormatValue(arg4, num2, base_value, num3, thousandSeparation);
						break;
					case '4':
						string_builder.ConcatFormatValue(arg5, num2, base_value, num3, thousandSeparation);
						break;
					}
				}
				num = i + 1;
			}
			if (num < format_string.Length)
			{
				string_builder.Append(format_string, num, format_string.Length - num);
			}
			return string_builder;
		}

		private static void ConcatFormatValue<T>(this StringBuilder string_builder, T arg, uint padding, uint base_value, uint decimal_places, bool thousandSeparation) where T : IConvertible
		{
			switch (arg.GetTypeCode())
			{
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.DateTime:
			case (TypeCode)17:
				break;
			case TypeCode.Boolean:
				if (arg.ToBoolean(CultureInfo.InvariantCulture))
				{
					string_builder.Append("true");
				}
				else
				{
					string_builder.Append("false");
				}
				break;
			case TypeCode.UInt32:
				string_builder.Concat(arg.ToUInt32(NumberFormatInfo.InvariantInfo), padding, '0', base_value, thousandSeparation);
				break;
			case TypeCode.Int32:
				string_builder.Concat(arg.ToInt32(NumberFormatInfo.InvariantInfo), padding, '0', base_value, thousandSeparation);
				break;
			case TypeCode.Int64:
				string_builder.Concat(arg.ToInt64(NumberFormatInfo.InvariantInfo), padding, '0', base_value, thousandSeparation);
				break;
			case TypeCode.UInt64:
				string_builder.Concat(arg.ToInt32(NumberFormatInfo.InvariantInfo), padding, '0', base_value, thousandSeparation);
				break;
			case TypeCode.Single:
				string_builder.Concat(arg.ToSingle(NumberFormatInfo.InvariantInfo), decimal_places, padding, '0', thousandSeparator: false);
				break;
			case TypeCode.Double:
				string_builder.Concat(arg.ToDouble(NumberFormatInfo.InvariantInfo), decimal_places, padding, '0', thousandSeparator: false);
				break;
			case TypeCode.Decimal:
				string_builder.Concat(arg.ToSingle(NumberFormatInfo.InvariantInfo), decimal_places, padding, '0', thousandSeparator: false);
				break;
			case TypeCode.String:
				string_builder.Append(arg.ToString());
				break;
			}
		}

		public static StringBuilder ToUpper(this StringBuilder self)
		{
			for (int i = 0; i < self.Length; i++)
			{
				self[i] = char.ToUpper(self[i]);
			}
			return self;
		}

		public static StringBuilder ToLower(this StringBuilder self)
		{
			for (int i = 0; i < self.Length; i++)
			{
				self[i] = char.ToLower(self[i]);
			}
			return self;
		}

		public static StringBuilder FirstLetterUpperCase(this StringBuilder self)
		{
			if (self.Length > 0)
			{
				self[0] = char.ToUpper(self[0]);
			}
			return self;
		}
	}
}
