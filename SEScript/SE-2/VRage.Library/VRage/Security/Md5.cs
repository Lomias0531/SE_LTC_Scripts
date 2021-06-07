namespace VRage.Security
{
	public static class Md5
	{
		/// <summary>
		/// Represent digest with ABCD
		/// </summary>
		public class Hash
		{
			public uint A;

			public uint B;

			public uint C;

			public uint D;

			public override string ToString()
			{
				return ReverseByte(A).ToString("X8") + ReverseByte(B).ToString("X8") + ReverseByte(C).ToString("X8") + ReverseByte(D).ToString("X8");
			}

			public string ToLowerString()
			{
				return ReverseByte(A).ToString("x8") + ReverseByte(B).ToString("x8") + ReverseByte(C).ToString("x8") + ReverseByte(D).ToString("x8");
			}

			/// <summary>
			/// perform a ByteReversal on a number
			/// </summary>
			/// <param name="uiNumber">value to be reversed</param>
			/// <returns>reversed value</returns>
			public static uint ReverseByte(uint uiNumber)
			{
				return ((uiNumber & 0xFF) << 24) | (uiNumber >> 24) | ((uiNumber & 0xFF0000) >> 8) | ((uiNumber & 0xFF00) << 8);
			}
		}

		/// <summary>
		/// lookup table 4294967296*sin(i)
		/// </summary>
		public static readonly uint[] T = new uint[64]
		{
			3614090360u,
			3905402710u,
			606105819u,
			3250441966u,
			4118548399u,
			1200080426u,
			2821735955u,
			4249261313u,
			1770035416u,
			2336552879u,
			4294925233u,
			2304563134u,
			1804603682u,
			4254626195u,
			2792965006u,
			1236535329u,
			4129170786u,
			3225465664u,
			643717713u,
			3921069994u,
			3593408605u,
			38016083u,
			3634488961u,
			3889429448u,
			568446438u,
			3275163606u,
			4107603335u,
			1163531501u,
			2850285829u,
			4243563512u,
			1735328473u,
			2368359562u,
			4294588738u,
			2272392833u,
			1839030562u,
			4259657740u,
			2763975236u,
			1272893353u,
			4139469664u,
			3200236656u,
			681279174u,
			3936430074u,
			3572445317u,
			76029189u,
			3654602809u,
			3873151461u,
			530742520u,
			3299628645u,
			4096336452u,
			1126891415u,
			2878612391u,
			4237533241u,
			1700485571u,
			2399980690u,
			4293915773u,
			2240044497u,
			1873313359u,
			4264355552u,
			2734768916u,
			1309151649u,
			4149444226u,
			3174756917u,
			718787259u,
			3951481745u
		};

		/// <summary>
		/// Left rotates the input word
		/// </summary>
		/// <param name="uiNumber">a value to be rotated</param>
		/// <param name="shift">no of bits to be rotated</param>
		/// <returns>the rotated value</returns>
		private static uint RotateLeft(uint uiNumber, ushort shift)
		{
			return (uiNumber >> 32 - shift) | (uiNumber << (int)shift);
		}

		/// <summary>
		/// calculat md5 signature of the string in Input
		/// </summary>
		/// <returns> Digest: the finger print of msg</returns>
		public static Hash ComputeHash(byte[] input)
		{
			Hash hash = new Hash();
			ComputeHash(input, hash);
			return hash;
		}

		/// <summary>
		/// calculat md5 signature of the string in Input
		/// </summary>
		/// <returns> Digest: the finger print of msg</returns>
		public unsafe static void ComputeHash(byte[] input, Hash dg)
		{
			uint* ptr = stackalloc uint[16];
			dg.A = 1732584193u;
			dg.B = 4023233417u;
			dg.C = 2562383102u;
			dg.D = 271733878u;
			uint num = (uint)(input.Length * 8) / 32u;
			for (uint num2 = 0u; num2 < num / 16u; num2++)
			{
				CopyBlock(input, num2, ptr);
				PerformTransformation(ref dg.A, ref dg.B, ref dg.C, ref dg.D, ptr);
			}
			if (input.Length % 64 >= 56)
			{
				CopyLastBlock(input, ptr);
				PerformTransformation(ref dg.A, ref dg.B, ref dg.C, ref dg.D, ptr);
				for (int i = 0; i < 16; i++)
				{
					ptr[i] = 0u;
				}
				*(long*)((byte*)ptr + 7L * 8L) = (long)input.Length * 8L;
				PerformTransformation(ref dg.A, ref dg.B, ref dg.C, ref dg.D, ptr);
			}
			else
			{
				CopyLastBlock(input, ptr);
				*(long*)((byte*)ptr + 7L * 8L) = (long)input.Length * 8L;
				PerformTransformation(ref dg.A, ref dg.B, ref dg.C, ref dg.D, ptr);
			}
		}

		private unsafe static void TransF(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i, uint* X)
		{
			a = b + RotateLeft(a + ((b & c) | (~b & d)) + X[k] + T[i - 1], s);
		}

		private unsafe static void TransG(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i, uint* X)
		{
			a = b + RotateLeft(a + ((b & d) | (c & ~d)) + X[k] + T[i - 1], s);
		}

		/// <summary>
		/// perform transformatio using h(b^c^d)
		/// </summary>
		private unsafe static void TransH(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i, uint* X)
		{
			a = b + RotateLeft(a + (b ^ c ^ d) + X[k] + T[i - 1], s);
		}

		/// <summary>
		/// perform transformatio using i (c^(b|~d))
		/// </summary>
		private unsafe static void TransI(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i, uint* X)
		{
			a = b + RotateLeft(a + (c ^ (b | ~d)) + X[k] + T[i - 1], s);
		}

		/// <summary>
		/// Perform All the transformation on the data
		/// </summary>
		/// <param name="A">A</param>
		/// <param name="B">B </param>
		/// <param name="C">C</param>
		/// <param name="D">D</param>
		private unsafe static void PerformTransformation(ref uint A, ref uint B, ref uint C, ref uint D, uint* X)
		{
			uint num = A;
			uint num2 = B;
			uint num3 = C;
			uint num4 = D;
			TransF(ref A, B, C, D, 0u, 7, 1u, X);
			TransF(ref D, A, B, C, 1u, 12, 2u, X);
			TransF(ref C, D, A, B, 2u, 17, 3u, X);
			TransF(ref B, C, D, A, 3u, 22, 4u, X);
			TransF(ref A, B, C, D, 4u, 7, 5u, X);
			TransF(ref D, A, B, C, 5u, 12, 6u, X);
			TransF(ref C, D, A, B, 6u, 17, 7u, X);
			TransF(ref B, C, D, A, 7u, 22, 8u, X);
			TransF(ref A, B, C, D, 8u, 7, 9u, X);
			TransF(ref D, A, B, C, 9u, 12, 10u, X);
			TransF(ref C, D, A, B, 10u, 17, 11u, X);
			TransF(ref B, C, D, A, 11u, 22, 12u, X);
			TransF(ref A, B, C, D, 12u, 7, 13u, X);
			TransF(ref D, A, B, C, 13u, 12, 14u, X);
			TransF(ref C, D, A, B, 14u, 17, 15u, X);
			TransF(ref B, C, D, A, 15u, 22, 16u, X);
			TransG(ref A, B, C, D, 1u, 5, 17u, X);
			TransG(ref D, A, B, C, 6u, 9, 18u, X);
			TransG(ref C, D, A, B, 11u, 14, 19u, X);
			TransG(ref B, C, D, A, 0u, 20, 20u, X);
			TransG(ref A, B, C, D, 5u, 5, 21u, X);
			TransG(ref D, A, B, C, 10u, 9, 22u, X);
			TransG(ref C, D, A, B, 15u, 14, 23u, X);
			TransG(ref B, C, D, A, 4u, 20, 24u, X);
			TransG(ref A, B, C, D, 9u, 5, 25u, X);
			TransG(ref D, A, B, C, 14u, 9, 26u, X);
			TransG(ref C, D, A, B, 3u, 14, 27u, X);
			TransG(ref B, C, D, A, 8u, 20, 28u, X);
			TransG(ref A, B, C, D, 13u, 5, 29u, X);
			TransG(ref D, A, B, C, 2u, 9, 30u, X);
			TransG(ref C, D, A, B, 7u, 14, 31u, X);
			TransG(ref B, C, D, A, 12u, 20, 32u, X);
			TransH(ref A, B, C, D, 5u, 4, 33u, X);
			TransH(ref D, A, B, C, 8u, 11, 34u, X);
			TransH(ref C, D, A, B, 11u, 16, 35u, X);
			TransH(ref B, C, D, A, 14u, 23, 36u, X);
			TransH(ref A, B, C, D, 1u, 4, 37u, X);
			TransH(ref D, A, B, C, 4u, 11, 38u, X);
			TransH(ref C, D, A, B, 7u, 16, 39u, X);
			TransH(ref B, C, D, A, 10u, 23, 40u, X);
			TransH(ref A, B, C, D, 13u, 4, 41u, X);
			TransH(ref D, A, B, C, 0u, 11, 42u, X);
			TransH(ref C, D, A, B, 3u, 16, 43u, X);
			TransH(ref B, C, D, A, 6u, 23, 44u, X);
			TransH(ref A, B, C, D, 9u, 4, 45u, X);
			TransH(ref D, A, B, C, 12u, 11, 46u, X);
			TransH(ref C, D, A, B, 15u, 16, 47u, X);
			TransH(ref B, C, D, A, 2u, 23, 48u, X);
			TransI(ref A, B, C, D, 0u, 6, 49u, X);
			TransI(ref D, A, B, C, 7u, 10, 50u, X);
			TransI(ref C, D, A, B, 14u, 15, 51u, X);
			TransI(ref B, C, D, A, 5u, 21, 52u, X);
			TransI(ref A, B, C, D, 12u, 6, 53u, X);
			TransI(ref D, A, B, C, 3u, 10, 54u, X);
			TransI(ref C, D, A, B, 10u, 15, 55u, X);
			TransI(ref B, C, D, A, 1u, 21, 56u, X);
			TransI(ref A, B, C, D, 8u, 6, 57u, X);
			TransI(ref D, A, B, C, 15u, 10, 58u, X);
			TransI(ref C, D, A, B, 6u, 15, 59u, X);
			TransI(ref B, C, D, A, 13u, 21, 60u, X);
			TransI(ref A, B, C, D, 4u, 6, 61u, X);
			TransI(ref D, A, B, C, 11u, 10, 62u, X);
			TransI(ref C, D, A, B, 2u, 15, 63u, X);
			TransI(ref B, C, D, A, 9u, 21, 64u, X);
			A += num;
			B += num2;
			C += num3;
			D += num4;
		}

		/// <summary>
		/// Copies a 512 bit block into X as 16 32 bit words
		/// </summary>
		/// <param name="bMsg"> source buffer</param>
		/// <param name="block">no of block to copy starting from 0</param>
		private unsafe static void CopyBlock(byte[] bMsg, uint block, uint* X)
		{
			block <<= 6;
			for (uint num = 0u; num < 61; num += 4)
			{
				X[num >> 2] = (uint)((bMsg[block + (num + 3)] << 24) | (bMsg[block + (num + 2)] << 16) | (bMsg[block + (num + 1)] << 8) | bMsg[block + num]);
			}
		}

		private unsafe static void CopyLastBlock(byte[] bMsg, uint* X)
		{
			long num = bMsg.LongLength / 64 * 64;
			int i;
			for (i = 0; i < bMsg.Length - num; i++)
			{
				((byte*)X)[i] = bMsg[num + i];
			}
			((byte*)X)[i] = 128;
			for (i++; i < 64; i++)
			{
				((byte*)X)[i] = 0;
			}
		}
	}
}
