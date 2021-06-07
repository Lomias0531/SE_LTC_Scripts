using ParallelTasks;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using VRage.Collections;
using VRage.Library.Memory;
using VRage.Library.Utils;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Stream which writes data based on bits.
	/// When writing, buffer must be reset to zero to write values correctly, this is done by ResetWrite() methods or SetPositionAndClearForward()
	/// </summary>
	public class BitStream : IDisposable
	{
		private static MyConcurrentBufferPool<NativeArray> BufferPool = new MyConcurrentBufferPool<NativeArray>("BitStreamBuffers", new NativeArrayAllocator(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("BitStreamBuffers")));

		private unsafe ulong* m_buffer;

		private GCHandle m_bufferHandle;

		private NativeArray m_ownedBuffer;

		private readonly int m_defaultByteSize;

		public const int TERMINATOR_SIZE = 2;

		private const ushort TERMINATOR = 51385;

		private const long Int64Msb = long.MinValue;

		private const int Int32Msb = int.MinValue;

		/// <summary>
		/// Read/write bit position in stream.
		/// </summary>
		public int BitPosition
		{
			get;
			private set;
		}

		/// <summary>
		/// Length of valid data (when reading) or buffer (when writing) in bits.
		/// </summary>
		public int BitLength
		{
			get;
			private set;
		}

		/// <summary>
		/// Position in stream round up to whole bytes.
		/// </summary>
		public int BytePosition => MyLibraryUtils.GetDivisionCeil(BitPosition, 8);

		/// <summary>
		/// Length of valid data (when reading) or buffer (when writing) round up to whole bytes
		/// </summary>
		public int ByteLength => MyLibraryUtils.GetDivisionCeil(BitLength, 8);

		/// <summary>
		/// Returns true when owns buffers, always true when writing.
		/// May or may not own buffer when reading.
		/// </summary>
		private bool OwnsBuffer => m_ownedBuffer != null;

		public bool Reading => !Writing;

		/// <summary>
		/// True when stream is for writing.
		/// </summary>
		public bool Writing
		{
			get;
			private set;
		}

		public unsafe IntPtr DataPointer => (IntPtr)(void*)m_buffer;

		public BitStream(int defaultByteSize = 1536)
		{
			m_defaultByteSize = Math.Max(16, MyLibraryUtils.GetDivisionCeil(defaultByteSize, 8) * 8);
		}

		public void Dispose()
		{
			ReleaseInternalBuffer();
			GC.SuppressFinalize(this);
		}

		~BitStream()
		{
			ReleaseInternalBuffer();
		}

		private void EnsureSize(int bitCount)
		{
			if (BitLength < bitCount)
			{
				Resize(bitCount);
			}
		}

		private unsafe void Resize(int bitSize)
		{
			if (!OwnsBuffer)
			{
				throw new BitStreamException("BitStream cannot write more data. Buffer is full and it's not owned by BitStream", new EndOfStreamException());
			}
			int bytePosition = BytePosition;
			int num = Math.Max(BitLength * 2, bitSize);
			int num2 = MyLibraryUtils.GetDivisionCeil(num, 64) * 8;
			NativeArray nativeArray = BufferPool.Get(num2);
			Buffer.MemoryCopy(m_buffer, nativeArray.Ptr.ToPointer(), num2, bytePosition);
			Unsafe.InitBlockUnaligned((nativeArray.Ptr + bytePosition).ToPointer(), 0, (uint)(num2 - bytePosition));
			BufferPool.Return(m_ownedBuffer);
			m_ownedBuffer = nativeArray;
			m_buffer = (ulong*)(void*)nativeArray.Ptr;
			BitLength = num;
		}

		private unsafe void ReleaseInternalBuffer()
		{
			FreeNotOwnedBuffer();
			if (OwnsBuffer)
			{
				m_buffer = null;
				BitLength = 0;
				BufferPool.Return(m_ownedBuffer);
				m_ownedBuffer = null;
			}
		}

		/// <summary>
		/// Resets stream for reading (reads what was written so far).
		/// </summary>
		public unsafe void ResetRead()
		{
			FreeNotOwnedBuffer();
			BitLength = BitPosition;
			m_buffer = (ulong*)(void*)(m_ownedBuffer?.Ptr).Value;
			Writing = false;
			BitPosition = 0;
		}

		/// <summary>
		/// Resets stream for reading and copies data.
		/// </summary>
		public unsafe void ResetRead(byte[] data, int byteOffset, int bitLength, bool copy = true)
		{
			fixed (byte* value = &data[byteOffset])
			{
				ResetRead((IntPtr)(void*)value, bitLength, copy);
				if (!copy)
				{
					m_bufferHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
				}
			}
		}

		public void ResetRead(BitStream source, bool copy = true)
		{
			ResetRead(source.DataPointer + source.BytePosition, source.BitLength - source.BytePosition * 8, copy);
		}

		/// <summary>
		/// Resets stream for reading.
		/// </summary>
		public unsafe void ResetRead(IntPtr buffer, int bitLength, bool copy)
		{
			FreeNotOwnedBuffer();
			if (copy)
			{
				int divisionCeil = MyLibraryUtils.GetDivisionCeil(bitLength, 8);
				int bucketId = Math.Max(divisionCeil, m_defaultByteSize);
				if (m_ownedBuffer == null || m_ownedBuffer.Size < bitLength)
				{
					if (m_ownedBuffer != null)
					{
						BufferPool.Return(m_ownedBuffer);
					}
					m_ownedBuffer = BufferPool.Get(bucketId);
				}
				Buffer.MemoryCopy(buffer.ToPointer(), m_ownedBuffer.Ptr.ToPointer(), divisionCeil, divisionCeil);
				m_buffer = (ulong*)(void*)m_ownedBuffer.Ptr;
				BitLength = bitLength;
				BitPosition = 0;
				Writing = false;
			}
			else
			{
				m_buffer = (ulong*)(void*)buffer;
				BitLength = bitLength;
				BitPosition = 0;
				Writing = false;
			}
		}

		private unsafe void FreeNotOwnedBuffer()
		{
			if (!OwnsBuffer && m_buffer != null && m_bufferHandle.IsAllocated)
			{
				m_bufferHandle.Free();
			}
		}

		/// <summary>
		/// Resets stream for writing.
		/// Uses internal buffer for writing, it's available as DataPointer.
		/// </summary>
		public unsafe void ResetWrite()
		{
			FreeNotOwnedBuffer();
			if (m_ownedBuffer == null)
			{
				m_ownedBuffer = BufferPool.Get(m_defaultByteSize);
			}
			m_buffer = (ulong*)(void*)m_ownedBuffer.Ptr;
			BitLength = m_ownedBuffer.Size * 8;
			BitPosition = 0;
			*m_buffer = 0uL;
			Writing = true;
		}

		public void Serialize(ref double value)
		{
			if (Writing)
			{
				WriteDouble(value);
			}
			else
			{
				value = ReadDouble();
			}
		}

		public void Serialize(ref float value)
		{
			if (Writing)
			{
				WriteFloat(value);
			}
			else
			{
				value = ReadFloat();
			}
		}

		public void Serialize(ref decimal value)
		{
			if (Writing)
			{
				WriteDecimal(value);
			}
			else
			{
				value = ReadDecimal();
			}
		}

		public void Serialize(ref bool value)
		{
			if (Writing)
			{
				WriteBool(value);
			}
			else
			{
				value = ReadBool();
			}
		}

		public void Serialize(ref sbyte value, int bitCount = 8)
		{
			if (Writing)
			{
				WriteSByte(value, bitCount);
			}
			else
			{
				value = ReadSByte(bitCount);
			}
		}

		public void Serialize(ref short value, int bitCount = 16)
		{
			if (Writing)
			{
				WriteInt16(value, bitCount);
			}
			else
			{
				value = ReadInt16(bitCount);
			}
		}

		public void Serialize(ref int value, int bitCount = 32)
		{
			if (Writing)
			{
				WriteInt32(value, bitCount);
			}
			else
			{
				value = ReadInt32(bitCount);
			}
		}

		public void Serialize(ref long value, int bitCount = 64)
		{
			if (Writing)
			{
				WriteInt64(value, bitCount);
			}
			else
			{
				value = ReadInt64(bitCount);
			}
		}

		public void Serialize(ref byte value, int bitCount = 8)
		{
			if (Writing)
			{
				WriteByte(value, bitCount);
			}
			else
			{
				value = ReadByte(bitCount);
			}
		}

		public void Serialize(ref ushort value, int bitCount = 16)
		{
			if (Writing)
			{
				WriteUInt16(value, bitCount);
			}
			else
			{
				value = ReadUInt16(bitCount);
			}
		}

		public void Serialize(ref uint value, int bitCount = 32)
		{
			if (Writing)
			{
				WriteUInt32(value, bitCount);
			}
			else
			{
				value = ReadUInt32(bitCount);
			}
		}

		public void Serialize(ref ulong value, int bitCount = 64)
		{
			if (Writing)
			{
				WriteUInt64(value, bitCount);
			}
			else
			{
				value = ReadUInt64(bitCount);
			}
		}

		/// <summary>
		/// Efficiently serializes small integers. Closer to zero, less bytes.
		/// From -64 to 63 (inclusive), 8 bits.
		/// From -8 192 to 8 191 (inclusive), 16 bits.
		/// From -1 048 576 to 1 048 575, 24 bits.
		/// From -134 217 728 to 134 217 727, 32 bits.
		/// Otherwise 40 bits.
		/// </summary>
		public void SerializeVariant(ref int value)
		{
			if (Writing)
			{
				WriteVariantSigned(value);
			}
			else
			{
				value = ReadInt32Variant();
			}
		}

		/// <summary>
		/// Efficiently serializes small integers. Closer to zero, less bytes.
		/// From -64 to 63 (inclusive), 8 bits.
		/// From -8192 to 8191 (inclusive), 16 bits.
		/// From -1048576 to 1048575, 24 bits.
		/// From -134217728 to 134217727, 32 bits.
		/// Etc...
		/// </summary>
		public void SerializeVariant(ref long value)
		{
			if (Writing)
			{
				WriteVariantSigned(value);
			}
			else
			{
				value = ReadInt64Variant();
			}
		}

		/// <summary>
		/// Efficiently serializes small integers. Closer to zero, less bytes.
		/// 0 - 127, 8 bits.
		/// 128 - 16383, 16 bits.
		/// 16384 - 2097151, 24 bits.
		/// 2097152 - 268435455, 32 bits.
		/// Otherwise 40 bits.
		/// </summary>
		public void SerializeVariant(ref uint value)
		{
			if (Writing)
			{
				WriteVariant(value);
			}
			else
			{
				value = ReadUInt32Variant();
			}
		}

		/// <summary>
		/// Efficiently serializes small integers. Closer to zero, less bytes.
		/// 0 - 127, 8 bits.
		/// 128 - 16383, 16 bits.
		/// 16384 - 2097151, 24 bits.
		/// 2097152 - 268435455, 32 bits.
		/// Etc...
		/// </summary>
		public void SerializeVariant(ref ulong value)
		{
			if (Writing)
			{
				WriteVariant(value);
			}
			else
			{
				value = ReadUInt64Variant();
			}
		}

		/// <summary>
		/// Writes char as UTF16, 2-byte value.
		/// For ASCII or different encoding, use SerializeByte() or SerializeBytes().
		/// </summary>
		public void Serialize(ref char value)
		{
			if (Writing)
			{
				WriteChar(value);
			}
			else
			{
				value = ReadChar();
			}
		}

		public void Serialize(StringBuilder value, ref char[] tmpArray, Encoding encoding)
		{
			if (Writing)
			{
				if (value.Length > tmpArray.Length)
				{
					tmpArray = new char[Math.Max(value.Length, tmpArray.Length * 2)];
				}
				value.CopyTo(0, tmpArray, 0, value.Length);
				WritePrefixLengthString(tmpArray, 0, value.Length, encoding);
			}
			else
			{
				value.Clear();
				int charCount = ReadPrefixLengthString(ref tmpArray, encoding);
				value.Append(tmpArray, 0, charCount);
			}
		}

		/// <summary>
		/// Serializes fixed size memory region.
		/// </summary>
		public void SerializeMemory(IntPtr ptr, int bitSize)
		{
			if (Writing)
			{
				WriteMemory(ptr, bitSize);
			}
			else
			{
				ReadMemory(ptr, bitSize);
			}
		}

		/// <summary>
		/// Serializes fixed size memory region.
		/// </summary>
		public unsafe void SerializeMemory(void* ptr, int bitSize)
		{
			if (Writing)
			{
				WriteMemory(ptr, bitSize);
			}
			else
			{
				ReadMemory(ptr, bitSize);
			}
		}

		/// <summary>
		/// Serializes string length (as UInt32 variant) and string itself in defined encoding.
		/// </summary>
		public void SerializePrefixString(ref string str, Encoding encoding)
		{
			if (Writing)
			{
				WritePrefixLengthString(str, 0, str.Length, encoding);
			}
			else
			{
				str = ReadPrefixLengthString(encoding);
			}
		}

		/// <summary>
		/// Serializes string length (as UInt32 variant) and string itself encoded with ASCII encoding.
		/// </summary>
		public void SerializePrefixStringAscii(ref string str)
		{
			SerializePrefixString(ref str, Encoding.ASCII);
		}

		/// <summary>
		/// Serializes string length (as UInt32 variant) and string itself encoded with UTF8 encoding.
		/// </summary>
		/// <param name="str"></param>
		public void SerializePrefixStringUtf8(ref string str)
		{
			SerializePrefixString(ref str, Encoding.UTF8);
		}

		/// <summary>
		/// Serializes byte array length (as UInt32 variant) and bytes.
		/// </summary>
		public void SerializePrefixBytes(ref byte[] bytes)
		{
			if (Writing)
			{
				WriteVariant((uint)bytes.Length);
				WriteBytes(bytes, 0, bytes.Length);
			}
			else
			{
				int num = (int)ReadUInt32Variant();
				bytes = new byte[num];
				ReadBytes(bytes, 0, num);
			}
		}

		/// <summary>
		/// Serializes fixed-size byte array or it's part (length is NOT serialized).
		/// </summary>
		public void SerializeBytes(ref byte[] bytes, int start, int count)
		{
			if (Writing)
			{
				WriteBytes(bytes, start, count);
			}
			else
			{
				ReadBytes(bytes, start, count);
			}
		}

		public void Terminate()
		{
			WriteUInt16(51385);
		}

		public bool CheckTerminator()
		{
			ushort num = ReadUInt16();
			return num == 51385;
		}

		public Type ReadDynamicType(Type baseType, DynamicSerializerDelegate typeResolver)
		{
			Type obj = null;
			typeResolver(this, baseType, ref obj);
			return obj;
		}

		public void WriteDynamicType(Type baseType, Type obj, DynamicSerializerDelegate typeResolver)
		{
			typeResolver(this, baseType, ref obj);
		}

		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		private unsafe ulong ReadInternal(int bitSize)
		{
			int num = BitPosition >> 6;
			int num2 = BitPosition + bitSize - 1 >> 6;
			ulong num3 = ulong.MaxValue >> 64 - bitSize;
			int num4 = BitPosition & 0x3F;
			ulong num5 = m_buffer[num] >> num4;
			if (num2 != num)
			{
				num5 |= m_buffer[num2] << 64 - num4;
			}
			BitPosition += bitSize;
			return num5 & num3;
		}

		public void SetBitPositionRead(int newReadBitPosition)
		{
			BitPosition = newReadBitPosition;
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

		/// <summary>
		/// Reads uniform-spaced float within -1,1 range with specified number of bits.
		/// </summary>
		public float ReadNormalizedSignedFloat(int bits)
		{
			return MyLibraryUtils.DenormalizeFloatCenter(ReadUInt32(bits), -1f, 1f, bits);
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

		public string ReadString()
		{
			return ReadPrefixLengthString(Encoding.UTF8);
		}

		public unsafe string ReadPrefixLengthString(Encoding encoding)
		{
			int num = (int)ReadUInt32Variant();
			if (num == 0)
			{
				return string.Empty;
			}
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

		/// <summary>
		/// Reads prefixed length string, returns nubmer of characters read.
		/// Passed array is automatically resized when needed.
		/// </summary>
		/// <returns>Nubmer of characters read.</returns>
		public unsafe int ReadPrefixLengthString(ref char[] value, Encoding encoding)
		{
			int num = (int)ReadUInt32Variant();
			if (num == 0)
			{
				return 0;
			}
			if (num <= 1024)
			{
				byte* tmpBuffer = stackalloc byte[(int)(uint)num];
				return ReadChars(tmpBuffer, num, ref value, encoding);
			}
			fixed (byte* tmpBuffer2 = new byte[num])
			{
				return ReadChars(tmpBuffer2, num, ref value, encoding);
			}
		}

		private unsafe int ReadChars(byte* tmpBuffer, int byteCount, ref char[] outputArray, Encoding encoding)
		{
			ReadMemory(tmpBuffer, byteCount * 8);
			int charCount = encoding.GetCharCount(tmpBuffer, byteCount);
			if (charCount > outputArray.Length)
			{
				outputArray = new char[Math.Max(charCount, outputArray.Length * 2)];
			}
			fixed (char* chars = &outputArray[0])
			{
				encoding.GetChars(tmpBuffer, byteCount, chars, charCount);
			}
			return charCount;
		}

		public unsafe void ReadBytes(byte[] bytes, int start, int count)
		{
			fixed (byte* ptr = bytes)
			{
				ReadMemory(ptr + start, count * 8);
			}
		}

		public byte[] ReadPrefixBytes()
		{
			int num = (int)ReadUInt32Variant();
			byte[] array = new byte[num];
			ReadBytes(array, 0, num);
			return array;
		}

		private unsafe void WriteInternal(ulong value, int bitSize)
		{
			if (bitSize != 0)
			{
				EnsureSize(BitPosition + bitSize);
				int num = BitPosition >> 6;
				int num2 = BitPosition + bitSize - 1 >> 6;
				ulong num3 = ulong.MaxValue >> 64 - bitSize;
				int num4 = BitPosition & 0x3F;
				value &= num3;
				m_buffer[num] &= ~(num3 << num4);
				m_buffer[num] |= value << num4;
				if (num2 != num)
				{
					m_buffer[num2] &= ~(num3 >> 64 - num4);
					m_buffer[num2] |= value >> 64 - num4;
				}
				BitPosition += bitSize;
			}
		}

		private unsafe void Clear(int fromPosition)
		{
			int num = fromPosition >> 6;
			int num2 = fromPosition & 0x3F;
			m_buffer[num] &= (ulong)(~(-1L << num2));
			int divisionCeil = MyLibraryUtils.GetDivisionCeil(BitPosition, 64);
			int num3 = num + 1;
			for (int i = num3; i < divisionCeil; i++)
			{
				m_buffer[i] = 0uL;
			}
		}

		/// <summary>
		/// Use when you need to overwrite part of the data.
		/// Sets new bit position and clears everything from min(old position, new position) to end of stream.
		/// </summary>
		public void SetBitPositionWrite(int newBitPosition)
		{
			BitPosition = newBitPosition;
		}

		public unsafe void WriteDouble(double value)
		{
			WriteInternal((ulong)(*(long*)(&value)), 64);
		}

		public unsafe void WriteFloat(float value)
		{
			WriteInternal(*(uint*)(&value), 32);
		}

		/// <summary>
		/// Writes uniform-spaced float within -1,1 range with specified number of bits.
		/// </summary>
		public void WriteNormalizedSignedFloat(float value, int bits)
		{
			WriteUInt32(MyLibraryUtils.NormalizeFloatCenter(value, -1f, 1f, bits), bits);
		}

		public unsafe void WriteDecimal(decimal value)
		{
			WriteInternal((ulong)(*(long*)(&value)), 64);
			WriteInternal((ulong)(*(long*)((byte*)(&value) + 8)), 64);
		}

		public void WriteBool(bool value)
		{
			WriteInternal((ulong)(value ? (-1) : 0), 1);
		}

		public void WriteSByte(sbyte value, int bitCount = 8)
		{
			WriteInternal((ulong)value, bitCount);
		}

		public void WriteInt16(short value, int bitCount = 16)
		{
			WriteInternal((ulong)value, bitCount);
		}

		public void WriteInt32(int value, int bitCount = 32)
		{
			WriteInternal((ulong)value, bitCount);
		}

		public void WriteInt64(long value, int bitCount = 64)
		{
			WriteInternal((ulong)value, bitCount);
		}

		public void WriteByte(byte value, int bitCount = 8)
		{
			WriteInternal(value, bitCount);
		}

		public void WriteUInt16(ushort value, int bitCount = 16)
		{
			WriteInternal(value, bitCount);
		}

		public void WriteUInt32(uint value, int bitCount = 32)
		{
			WriteInternal(value, bitCount);
		}

		public void WriteUInt64(ulong value, int bitCount = 64)
		{
			WriteInternal(value, bitCount);
		}

		private static uint Zig(int value)
		{
			return (uint)((value << 1) ^ (value >> 31));
		}

		private static ulong Zig(long value)
		{
			return (ulong)((value << 1) ^ (value >> 63));
		}

		/// <summary>
		/// Efficiently writes small integers. Closer to zero, less bytes.
		/// From -64 to 63 (inclusive), 8 bits.
		/// From -8 192 to 8 191 (inclusive), 16 bits.
		/// From -1 048 576 to 1 048 575, 24 bits.
		/// From -134 217 728 to 134 217 727, 32 bits.
		/// Otherwise 40 bits.
		/// </summary>
		public void WriteVariantSigned(int value)
		{
			WriteVariant(Zig(value));
		}

		/// <summary>
		/// Efficiently writes small integers. Closer to zero, less bytes.
		/// From -64 to 63 (inclusive), 8 bits.
		/// From -8192 to 8191 (inclusive), 16 bits.
		/// From -1048576 to 1048575, 24 bits.
		/// From -134217728 to 134217727, 32 bits.
		/// Etc...
		/// </summary>
		public void WriteVariantSigned(long value)
		{
			WriteVariant(Zig(value));
		}

		/// <summary>
		/// Efficiently writes small integers. Closer to zero, less bytes.
		/// 0 - 127, 8 bits.
		/// 128 - 16383, 16 bits.
		/// 16384 - 2097151, 24 bits.
		/// 2097152 - 268435455, 32 bits.
		/// Otherwise 40 bits.
		/// </summary>
		public unsafe void WriteVariant(uint value)
		{
			ulong value2 = default(ulong);
			byte* ptr = (byte*)(&value2);
			int num = 0;
			int num2 = 0;
			do
			{
				ptr[num2++] = (byte)(value | 0x80);
				num++;
			}
			while ((value >>= 7) != 0);
			byte* intPtr = ptr + (num2 - 1);
			*intPtr = (byte)(*intPtr & 0x7F);
			WriteInternal(value2, num * 8);
		}

		/// <summary>
		/// Efficiently writes small integers. Closer to zero, less bytes.
		/// 0 - 127, 8 bits.
		/// 128 - 16383, 16 bits.
		/// 16384 - 2097151, 24 bits.
		/// 2097152 - 268435455, 32 bits.
		/// Etc...
		/// </summary>
		public unsafe void WriteVariant(ulong value)
		{
			byte* ptr = stackalloc byte[16];
			int num = 0;
			int num2 = 0;
			do
			{
				ptr[num2++] = (byte)((value & 0x7F) | 0x80);
				num++;
			}
			while ((value >>= 7) != 0L);
			byte* intPtr = ptr + (num2 - 1);
			*intPtr = (byte)(*intPtr & 0x7F);
			if (num > 8)
			{
				WriteInternal((ulong)(*(long*)ptr), 64);
				WriteInternal((ulong)(*(long*)(ptr + 8)), (num - 8) * 8);
			}
			else
			{
				WriteInternal((ulong)(*(long*)ptr), num * 8);
			}
		}

		public void WriteChar(char value, int bitCount = 16)
		{
			WriteInternal(value, bitCount);
		}

		public void WriteBitStream(BitStream readStream)
		{
			int num = readStream.BitLength - readStream.BitPosition;
			while (num > 0)
			{
				int num2 = Math.Min(64, num);
				ulong value = readStream.ReadUInt64(num2);
				WriteUInt64(value, num2);
				num -= num2;
			}
		}

		public unsafe void WriteMemory(IntPtr ptr, int bitSize)
		{
			WriteMemory((void*)ptr, bitSize);
		}

		public unsafe void WriteMemory(void* ptr, int bitSize)
		{
			int num = bitSize / 8 / 8;
			for (int i = 0; i < num; i++)
			{
				WriteUInt64((ulong)(*(long*)((byte*)ptr + (long)i * 8L)));
			}
			int num2 = bitSize - num * 8 * 8;
			byte* ptr2 = (byte*)ptr + (long)num * 8L;
			while (num2 > 0)
			{
				int num3 = Math.Min(num2, 8);
				WriteByte(*ptr2, num3);
				num2 -= num3;
				ptr2++;
			}
		}

		public void WriteString(string str)
		{
			WritePrefixLengthString(str, 0, str.Length, Encoding.UTF8);
		}

		public unsafe void WritePrefixLengthString(string str, int characterStart, int characterCount, Encoding encoding)
		{
			fixed (char* ptr = str)
			{
				WritePrefixLengthString(characterStart, characterCount, encoding, ptr);
			}
		}

		public unsafe void WritePrefixLengthString(char[] str, int characterStart, int characterCount, Encoding encoding)
		{
			fixed (char* ptr = str)
			{
				WritePrefixLengthString(characterStart, characterCount, encoding, ptr);
			}
		}

		private unsafe void WritePrefixLengthString(int characterStart, int characterCount, Encoding encoding, char* ptr)
		{
			char* ptr2 = ptr + characterStart;
			int byteCount = encoding.GetByteCount(ptr2, characterCount);
			WriteVariant((uint)byteCount);
			byte* ptr3 = stackalloc byte[256];
			int val = 256 / encoding.GetMaxByteCount(1);
			while (characterCount > 0)
			{
				int num = Math.Min(val, characterCount);
				int bytes = encoding.GetBytes(ptr2, num, ptr3, 256);
				WriteMemory(ptr3, bytes * 8);
				ptr2 += num;
				characterCount -= num;
			}
		}

		public unsafe void WriteBytes(byte[] bytes, int start, int count)
		{
			fixed (byte* ptr = bytes)
			{
				WriteMemory(ptr + start, count * 8);
			}
		}

		public void WritePrefixBytes(byte[] bytes, int start, int count)
		{
			WriteVariant((uint)count);
			WriteBytes(bytes, start, count);
		}
	}
}
