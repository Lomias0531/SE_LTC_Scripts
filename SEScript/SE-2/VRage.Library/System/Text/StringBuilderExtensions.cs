using System.Globalization;

namespace System.Text
{
	public static class StringBuilderExtensions
	{
		private static readonly char[] ms_digits = new char[16]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};

		private static readonly uint ms_default_decimal_places = 5u;

		private static readonly char ms_default_pad_char = '0';

		public static StringBuilder Concat(this StringBuilder string_builder, ulong uint_val, uint pad_amount, char pad_char, uint base_val, bool thousandSeparation)
		{
			uint num = 0u;
			ulong num2 = uint_val;
			int num3 = 0;
			do
			{
				num3++;
				if (thousandSeparation && num3 % 4 == 0)
				{
					num++;
					continue;
				}
				num2 /= base_val;
				num++;
			}
			while (num2 != 0);
			string_builder.Append(pad_char, (int)Math.Max(pad_amount, num));
			int num4 = string_builder.Length;
			num3 = 0;
			while (num != 0)
			{
				num4--;
				num3++;
				if (thousandSeparation && num3 % 4 == 0)
				{
					num--;
					string_builder[num4] = NumberFormatInfo.InvariantInfo.NumberGroupSeparator[0];
				}
				else
				{
					string_builder[num4] = ms_digits[uint_val % base_val];
					uint_val /= base_val;
					num--;
				}
			}
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val)
		{
			string_builder.Concat(uint_val, 0u, ms_default_pad_char, 10u, thousandSeparation: true);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val, uint pad_amount)
		{
			string_builder.Concat(uint_val, pad_amount, ms_default_pad_char, 10u, thousandSeparation: true);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, uint uint_val, uint pad_amount, char pad_char)
		{
			string_builder.Concat(uint_val, pad_amount, pad_char, 10u, thousandSeparation: true);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, int int_val, uint pad_amount, char pad_char, uint base_val, bool thousandSeparation)
		{
			if (int_val < 0)
			{
				string_builder.Append('-');
				uint num = (uint)(-1 - int_val + 1);
				string_builder.Concat(num, pad_amount, pad_char, base_val, thousandSeparation);
			}
			else
			{
				string_builder.Concat((uint)int_val, pad_amount, pad_char, base_val, thousandSeparation);
			}
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, long long_val, uint pad_amount, char pad_char, uint base_val, bool thousandSeparation)
		{
			if (long_val < 0)
			{
				string_builder.Append('-');
				ulong uint_val = (ulong)(-1 - long_val + 1);
				string_builder.Concat(uint_val, pad_amount, pad_char, base_val, thousandSeparation);
			}
			else
			{
				string_builder.Concat((ulong)long_val, pad_amount, pad_char, base_val, thousandSeparation);
			}
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, int int_val)
		{
			string_builder.Concat(int_val, 0u, ms_default_pad_char, 10u, thousandSeparation: true);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, int int_val, uint pad_amount)
		{
			string_builder.Concat(int_val, pad_amount, ms_default_pad_char, 10u, thousandSeparation: true);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, int int_val, uint pad_amount, char pad_char)
		{
			string_builder.Concat(int_val, pad_amount, pad_char, 10u, thousandSeparation: true);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, float float_val, uint decimal_places, uint pad_amount, char pad_char, bool thousandSeparator)
		{
			if (decimal_places == 0)
			{
				long long_val = (!(float_val >= 0f)) ? ((long)(float_val - 0.5f)) : ((long)(float_val + 0.5f));
				string_builder.Concat(long_val, pad_amount, pad_char, 10u, thousandSeparator);
			}
			else
			{
				float num = 0.5f;
				for (int i = 0; i < decimal_places; i++)
				{
					num *= 0.1f;
				}
				float_val += ((float_val >= 0f) ? num : (0f - num));
				long num2 = (long)float_val;
				if (num2 == 0L && float_val < 0f)
				{
					string_builder.Append('-');
				}
				string_builder.Concat(num2, pad_amount, pad_char, 10u, thousandSeparator);
				string_builder.Append('.');
				float num3 = Math.Abs(float_val - (float)num2);
				uint num4 = decimal_places;
				do
				{
					num3 *= 10f;
					num4--;
				}
				while (num4 != 0);
				string_builder.Concat((uint)num3, decimal_places, '0', 10u, thousandSeparator);
			}
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, float float_val)
		{
			string_builder.Concat(float_val, ms_default_decimal_places, 0u, ms_default_pad_char, thousandSeparator: false);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, float float_val, uint decimal_places)
		{
			string_builder.Concat(float_val, decimal_places, 0u, ms_default_pad_char, thousandSeparator: false);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, float float_val, uint decimal_places, uint pad_amount)
		{
			string_builder.Concat(float_val, decimal_places, pad_amount, ms_default_pad_char, thousandSeparator: false);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, double double_val, uint decimal_places, uint pad_amount, char pad_char, bool thousandSeparator)
		{
			if (decimal_places == 0)
			{
				long long_val = (!(double_val >= 0.0)) ? ((long)(double_val - 0.5)) : ((long)(double_val + 0.5));
				string_builder.Concat(long_val, pad_amount, pad_char, 10u, thousandSeparator);
			}
			else
			{
				double num = 0.5;
				for (int i = 0; i < decimal_places; i++)
				{
					num *= 0.10000000149011612;
				}
				double_val += ((double_val >= 0.0) ? num : (0.0 - num));
				long num2 = (long)double_val;
				if (num2 == 0L && double_val < 0.0)
				{
					string_builder.Append('-');
				}
				string_builder.Concat(num2, pad_amount, pad_char, 10u, thousandSeparator);
				string_builder.Append('.');
				double num3 = Math.Abs(double_val - (double)num2);
				uint num4 = decimal_places;
				do
				{
					num3 *= 10.0;
					num4--;
				}
				while (num4 != 0);
				string_builder.Concat((uint)num3, decimal_places, '0', 10u, thousandSeparator);
			}
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, double double_val)
		{
			string_builder.Concat(double_val, ms_default_decimal_places, 0u, ms_default_pad_char, thousandSeparator: false);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, double double_val, uint decimal_places)
		{
			string_builder.Concat(double_val, decimal_places, 0u, ms_default_pad_char, thousandSeparator: false);
			return string_builder;
		}

		public static StringBuilder Concat(this StringBuilder string_builder, double double_val, uint decimal_places, uint pad_amount)
		{
			string_builder.Concat(double_val, decimal_places, pad_amount, ms_default_pad_char, thousandSeparator: false);
			return string_builder;
		}
	}
}
