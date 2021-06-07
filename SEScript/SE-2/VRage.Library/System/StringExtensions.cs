namespace System
{
	public static class StringExtensions
	{
		public unsafe static bool Equals(this string text, char* compareTo, int length)
		{
			int num = Math.Min(length, text.Length);
			for (int i = 0; i < num; i++)
			{
				if (text[i] != compareTo[i])
				{
					return false;
				}
			}
			if (length > num)
			{
				return compareTo[num] == '\0';
			}
			return true;
		}

		public static bool Contains(this string text, string testSequence, StringComparison comparison)
		{
			return text.IndexOf(testSequence, comparison) != -1;
		}

		public unsafe static long GetHashCode64(this string self)
		{
			fixed (char* ptr = self)
			{
				int num = self.Length;
				long* ptr2 = (long*)ptr;
				long num2 = 1692801359929L;
				ushort* ptr3 = (ushort*)ptr;
				while (num >= 4)
				{
					num2 = (num2 << 5) + num2 + (num2 >> 59);
					num2 ^= *ptr2;
					ptr2++;
					ptr3 += 4;
					num -= 4;
				}
				if (num > 0)
				{
					long num3 = 0L;
					while (num > 0)
					{
						long num4 = num3 << 16;
						ushort* intPtr = ptr3;
						ptr3 = intPtr + 1;
						num3 = (num4 | *intPtr);
						num--;
					}
					num2 = (num2 << 5) + num2 + (num2 >> 59);
					num2 ^= num3;
				}
				return num2;
			}
		}
	}
}
