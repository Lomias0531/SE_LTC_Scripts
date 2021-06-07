using System.IO;
using System.IO.Compression;
using System.Text;

namespace System
{
	public static class StreamExtensions
	{
		[ThreadStatic]
		private static byte[] m_buffer;

		private static byte[] Buffer
		{
			get
			{
				if (m_buffer == null)
				{
					m_buffer = new byte[256];
				}
				return m_buffer;
			}
		}

		public static bool CheckGZipHeader(this Stream stream)
		{
			if (!stream.CanSeek)
			{
				return false;
			}
			long position = stream.Position;
			byte[] array = new byte[2];
			stream.Seek(0L, SeekOrigin.Begin);
			stream.Read(array, 0, 2);
			if (array[0] == 31 && array[1] == 139)
			{
				stream.Seek(position, SeekOrigin.Begin);
				return true;
			}
			stream.Seek(position, SeekOrigin.Begin);
			return false;
		}

		/// <summary>
		/// Checks for GZip header and if found, returns decompressed Stream, otherwise original Stream
		/// </summary>
		public static Stream UnwrapGZip(this Stream stream)
		{
			if (!stream.CheckGZipHeader())
			{
				return stream;
			}
			return new GZipStream(stream, CompressionMode.Decompress, leaveOpen: false);
		}

		/// <summary>
		/// Wraps stream into GZip compression stream resulting in writing compressed stream
		/// </summary>
		public static Stream WrapGZip(this Stream stream, bool buffered = true)
		{
			GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: false);
			if (!buffered)
			{
				return gZipStream;
			}
			return new BufferedStream(gZipStream, 32768);
		}

		public static int Read7BitEncodedInt(this Stream stream)
		{
			byte[] buffer = Buffer;
			int num = 0;
			int num2 = 0;
			while (num2 != 35)
			{
				if (stream.Read(buffer, 0, 1) == 0)
				{
					throw new EndOfStreamException();
				}
				byte b = buffer[0];
				num |= (b & 0x7F) << num2;
				num2 += 7;
				if ((b & 0x80) == 0)
				{
					return num;
				}
			}
			throw new FormatException("Bad string length. 7bit Int32 format");
		}

		public static void Write7BitEncodedInt(this Stream stream, int value)
		{
			byte[] buffer = Buffer;
			int num = 0;
			uint num2 = (uint)value;
			while (num2 >= 128)
			{
				buffer[num++] = (byte)(num2 | 0x80);
				num2 >>= 7;
				if (num == buffer.Length)
				{
					stream.Write(buffer, 0, num);
					num = 0;
				}
			}
			buffer[num++] = (byte)num2;
			stream.Write(buffer, 0, num);
		}

		public static byte ReadByteNoAlloc(this Stream stream)
		{
			byte[] buffer = Buffer;
			if (stream.Read(buffer, 0, 1) == 0)
			{
				throw new EndOfStreamException();
			}
			return buffer[0];
		}

		public unsafe static void WriteNoAlloc(this Stream stream, byte* bytes, int offset, int count)
		{
			byte[] buffer = Buffer;
			int num = 0;
			int num2 = offset;
			int num3 = offset + count;
			while (num2 != num3)
			{
				buffer[num++] = bytes[num2++];
				if (num == buffer.Length)
				{
					stream.Write(buffer, 0, num);
					num = 0;
				}
			}
			if (num != 0)
			{
				stream.Write(buffer, 0, num);
			}
		}

		public unsafe static void ReadNoAlloc(this Stream stream, byte* bytes, int offset, int count)
		{
			byte[] buffer = Buffer;
			int num = 0;
			int num2 = offset;
			int num3 = offset + count;
			while (num2 != num3)
			{
				num = Math.Min(count, buffer.Length);
				stream.Read(buffer, 0, num);
				count -= num;
				for (int i = 0; i < num; i++)
				{
					bytes[num2++] = buffer[i];
				}
			}
		}

		/// <summary>
		/// Writes byte count prefixed encoded text into the file. Byte count is written as 7-bit encoded 32-bit int.
		/// If no encoding is specified, UTF-8 will be used. Byte count prefix specifies number of bytes taken up by
		/// the string, not length of the string itself.
		/// Note that this method may allocate if the size of encoded string exceeds size of prepared buffer.
		/// </summary>
		public static void WriteNoAlloc(this Stream stream, string text, Encoding encoding = null)
		{
			encoding = (encoding ?? Encoding.UTF8);
			int byteCount = encoding.GetByteCount(text);
			stream.Write7BitEncodedInt(byteCount);
			byte[] array = Buffer;
			if (byteCount > array.Length)
			{
				array = new byte[byteCount];
			}
			int bytes = encoding.GetBytes(text, 0, text.Length, array, 0);
			stream.Write(array, 0, bytes);
		}

		public static string ReadString(this Stream stream, Encoding encoding = null)
		{
			encoding = (encoding ?? Encoding.UTF8);
			int num = stream.Read7BitEncodedInt();
			byte[] array = Buffer;
			if (num > array.Length)
			{
				array = new byte[num];
			}
			stream.Read(array, 0, num);
			return encoding.GetString(array, 0, num);
		}

		public static void WriteNoAlloc(this Stream stream, byte value)
		{
			byte[] buffer = Buffer;
			buffer[0] = value;
			stream.Write(buffer, 0, 1);
		}

		public unsafe static void WriteNoAlloc(this Stream stream, short v)
		{
			stream.WriteNoAlloc((byte*)(&v), 0, 2);
		}

		public unsafe static void WriteNoAlloc(this Stream stream, int v)
		{
			stream.WriteNoAlloc((byte*)(&v), 0, 4);
		}

		public unsafe static void WriteNoAlloc(this Stream stream, long v)
		{
			stream.WriteNoAlloc((byte*)(&v), 0, 8);
		}

		public unsafe static void WriteNoAlloc(this Stream stream, ushort v)
		{
			stream.WriteNoAlloc((byte*)(&v), 0, 2);
		}

		public unsafe static void WriteNoAlloc(this Stream stream, uint v)
		{
			stream.WriteNoAlloc((byte*)(&v), 0, 4);
		}

		public unsafe static void WriteNoAlloc(this Stream stream, ulong v)
		{
			stream.WriteNoAlloc((byte*)(&v), 0, 8);
		}

		public unsafe static void WriteNoAlloc(this Stream stream, float v)
		{
			stream.WriteNoAlloc((byte*)(&v), 0, 4);
		}

		public unsafe static void WriteNoAlloc(this Stream stream, double v)
		{
			stream.WriteNoAlloc((byte*)(&v), 0, 8);
		}

		public unsafe static void WriteNoAlloc(this Stream stream, decimal v)
		{
			stream.WriteNoAlloc((byte*)(&v), 0, 16);
		}

		public unsafe static short ReadInt16(this Stream stream)
		{
			short result = default(short);
			stream.ReadNoAlloc((byte*)(&result), 0, 2);
			return result;
		}

		public unsafe static int ReadInt32(this Stream stream)
		{
			int result = default(int);
			stream.ReadNoAlloc((byte*)(&result), 0, 4);
			return result;
		}

		public unsafe static long ReadInt64(this Stream stream)
		{
			long result = default(long);
			stream.ReadNoAlloc((byte*)(&result), 0, 8);
			return result;
		}

		public unsafe static ushort ReadUInt16(this Stream stream)
		{
			ushort result = default(ushort);
			stream.ReadNoAlloc((byte*)(&result), 0, 2);
			return result;
		}

		public unsafe static uint ReadUInt32(this Stream stream)
		{
			uint result = default(uint);
			stream.ReadNoAlloc((byte*)(&result), 0, 4);
			return result;
		}

		public unsafe static ulong ReadUInt64(this Stream stream)
		{
			ulong result = default(ulong);
			stream.ReadNoAlloc((byte*)(&result), 0, 8);
			return result;
		}

		public unsafe static float ReadFloat(this Stream stream)
		{
			float result = default(float);
			stream.ReadNoAlloc((byte*)(&result), 0, 4);
			return result;
		}

		public unsafe static double ReadDouble(this Stream stream)
		{
			double result = default(double);
			stream.ReadNoAlloc((byte*)(&result), 0, 8);
			return result;
		}

		public unsafe static decimal ReadDecimal(this Stream stream)
		{
			decimal result = default(decimal);
			stream.ReadNoAlloc((byte*)(&result), 0, 16);
			return result;
		}

		public static void SkipBytes(this Stream stream, int byteCount)
		{
			byte[] buffer = Buffer;
			while (byteCount > 0)
			{
				int num = (byteCount > buffer.Length) ? buffer.Length : byteCount;
				stream.Read(buffer, 0, num);
				byteCount -= num;
			}
		}
	}
}
