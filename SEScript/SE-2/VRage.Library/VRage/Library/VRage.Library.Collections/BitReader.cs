using System;
using System.IO;
using System.Text;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Lightweight bit reader which works on native pointer.
	/// Stores bit length and current position.
	/// </summary>
	public struct BitReader
	{
		private unsafe ulong* m_buffer;

		private int m_bitLength;

		public int BitPosition;

		private const long Int64Msb = long.MinValue;

		private const int Int32Msb = int.MinValue;

		public unsafe BitReader(IntPtr data, int bitLength)
		{
			m_buffer = (ulong*)(void*)data;
			m_bitLength = bitLength;
			BitPosition = 0;
		}

		public unsafe void Reset(IntPtr data, int bitLength)
		{
			m_buffer = (ulong*)(void*)data;
			m_bitLength = bitLength;
			BitPosition = 0;
		}

		private unsafe ulong ReadInternal(int bitSize)
		{
			if (m_bitLength < BitPosition + bitSize)
			{
				throw new BitStreamException(new EndOfStreamException("Cannot read from bit stream, end of steam"));
			}
			int num = BitPosition >> 6;
			int num2 = BitPosition + bitSize - 1 >> 6;
			ulong num3 = ulong.MaxValue >> 64 - bitSize;
			int num4 = BitPosition & -65;
			ulong num5 = m_buffer[num] >> num4;
			if (num2 != num)
			{
				num5 |= m_buffer[num2] << 64 - num4;
			}
			BitPosition += bitSize;
			return num5 & num3;
		}

		public unsafe double ReadDouble()
		{
			ulong num = ReadInternal(64);
			return *(double*)(&num);
		}

		public unsafe float ReadFloat()
		{
			ulong num = ReadInternal(32);
			return *(float*)(&num);
		}

		public unsafe decimal ReadDecimal()
		{
			decimal result = default(decimal);
			*(ulong*)(&result) = ReadInternal(64);
			*(ulong*)((byte*)(&result) + 8) = ReadInternal(64);
			return result;
		}

		public bool ReadBool()
		{
			return ReadInternal(1) != 0;
		}

		public sbyte ReadSByte(int bitCount = 8)
		{
			return (sbyte)ReadInternal(bitCount);
		}

		public short ReadInt16(int bitCount = 16)
		{
			return (short)ReadInternal(bitCount);
		}

		public int ReadInt32(int bitCount = 32)
		{
			return (int)ReadInternal(bitCount);
		}

		public long ReadInt64(int bitCount = 64)
		{
			return (long)ReadInternal(bitCount);
		}

		public byte ReadByte(int bitCount = 8)
		{
			return (byte)ReadInternal(bitCount);
		}

		public ushort ReadUInt16(int bitCount = 16)
		{
			return (ushort)ReadInternal(bitCount);
		}

		public uint ReadUInt32(int bitCount = 32)
		{
			return (uint)ReadInternal(bitCount);
		}

		public ulong ReadUInt64(int bitCount = 64)
		{
			return ReadInternal(bitCount);
		}

		private static int Zag(uint ziggedValue)
		{
			return (int)(0 - (ziggedValue & 1)) ^ (((int)ziggedValue >> 1) & int.MaxValue);
		}

		private static long Zag(ulong ziggedValue)
		{
			return (long)(0L - (ziggedValue & 1)) ^ (((long)ziggedValue >> 1) & long.MaxValue);
		}

		public int ReadInt32Variant()
		{
			return Zag(ReadUInt32Variant());
		}

		public long ReadInt64Variant()
		{
			return Zag(ReadUInt64Variant());
		}

		public uint ReadUInt32Variant()
		{
			uint num = ReadByte();
			if ((num & 0x80) == 0)
			{
				return num;
			}
			num &= 0x7F;
			uint num2 = ReadByte();
			num |= (num2 & 0x7F) << 7;
			if ((num2 & 0x80) == 0)
			{
				return num;
			}
			num2 = ReadByte();
			num |= (num2 & 0x7F) << 14;
			if ((num2 & 0x80) == 0)
			{
				return num;
			}
			num2 = ReadByte();
			num |= (num2 & 0x7F) << 21;
			if ((num2 & 0x80) == 0)
			{
				return num;
			}
			num2 = ReadByte();
			num |= num2 << 28;
			if ((num2 & 0xF0) == 0)
			{
				return num;
			}
			throw new BitStreamException(new OverflowException("Error when deserializing variant uint32"));
		}

		public ulong ReadUInt64Variant()
		{
			ulong num = ReadByte();
			if ((num & 0x80) == 0L)
			{
				return num;
			}
			num &= 0x7F;
			ulong num2 = ReadByte();
			num |= (num2 & 0x7F) << 7;
			if ((num2 & 0x80) == 0L)
			{
				return num;
			}
			num2 = ReadByte();
			num |= (num2 & 0x7F) << 14;
			if ((num2 & 0x80) == 0L)
			{
				return num;
			}
			num2 = ReadByte();
			num |= (num2 & 0x7F) << 21;
			if ((num2 & 0x80) == 0L)
			{
				return num;
			}
			num2 = ReadByte();
			num |= (num2 & 0x7F) << 28;
			if ((num2 & 0x80) == 0L)
			{
				return num;
			}
			num2 = ReadByte();
			num |= (num2 & 0x7F) << 35;
			if ((num2 & 0x80) == 0L)
			{
				return num;
			}
			num2 = ReadByte();
			num |= (num2 & 0x7F) << 42;
			if ((num2 & 0x80) == 0L)
			{
				return num;
			}
			num2 = ReadByte();
			num |= (num2 & 0x7F) << 49;
			if ((num2 & 0x80) == 0L)
			{
				return num;
			}
			num2 = ReadByte();
			num |= (num2 & 0x7F) << 56;
			if ((num2 & 0x80) == 0L)
			{
				return num;
			}
			num2 = ReadByte();
			num |= num2 << 63;
			if (((long)num2 & -2L) != 0L)
			{
				throw new BitStreamException(new OverflowException("Error when deserializing variant uint64"));
			}
			return num;
		}

		public char ReadChar(int bitCount = 16)
		{
			return (char)ReadInternal(bitCount);
		}

		public unsafe void ReadMemory(IntPtr ptr, int bitSize)
		{
			ReadMemory((void*)ptr, bitSize);
		}

		public unsafe void ReadMemory(void* ptr, int bitSize)
		{
			int num = bitSize / 8 / 8;
			for (int i = 0; i < num; i++)
			{
				*(ulong*)((byte*)ptr + (long)i * 8L) = ReadUInt64();
			}
			int num2 = bitSize - num * 8 * 8;
			byte* ptr2 = (byte*)ptr + (long)num * 8L;
			while (num2 > 0)
			{
				int num3 = Math.Min(num2, 8);
				*ptr2 = ReadByte(num3);
				num2 -= num3;
				ptr2++;
			}
		}

		public unsafe string ReadPrefixLengthString(Encoding encoding)
		{
			int num = (int)ReadUInt32Variant();
			if (num <= 1024)
			{
				byte* ptr = stackalloc byte[(int)(uint)num];
				ReadMemory(ptr, num * 8);
				int charCount = encoding.GetCharCount(ptr, num);
				char* ptr2 = stackalloc char[charCount];
				encoding.GetChars(ptr, num, ptr2, charCount);
				return new string(ptr2, 0, charCount);
			}
			byte[] array = new byte[num];
			fixed (byte* ptr3 = array)
			{
				ReadMemory(ptr3, num * 8);
			}
			return new string(encoding.GetChars(array));
		}

		public unsafe void ReadBytes(byte[] bytes, int start, int count)
		{
			fixed (byte* ptr = bytes)
			{
				ReadMemory(ptr + start, count * 8);
			}
		}
	}
}
