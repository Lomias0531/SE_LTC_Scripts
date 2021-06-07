using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace VRage
{
	public class ByteStream : Stream
	{
		private byte[] m_baseArray;

		private int m_position;

		private int m_length;

		public readonly bool Expandable;

		public readonly bool Resetable;

		public byte[] Data => m_baseArray;

		public override bool CanRead => true;

		public override bool CanSeek => true;

		public override bool CanWrite => true;

		public override long Length => m_length;

		public override long Position
		{
			get
			{
				return m_position;
			}
			set
			{
				m_position = (int)value;
			}
		}

		/// <summary>
		/// Create non-resetable Stream, optionally expandable
		/// </summary>
		public ByteStream(int capacity, bool expandable = true)
		{
			Expandable = expandable;
			Resetable = false;
			m_baseArray = new byte[capacity];
			m_length = m_baseArray.Length;
		}

		/// <summary>
		/// Creates resetable Stream
		/// </summary>
		public ByteStream()
		{
			Resetable = true;
			Expandable = false;
		}

		/// <summary>
		/// Creates and initializes resetable Stream
		/// </summary>
		public ByteStream(byte[] newBaseArray, int length)
			: this()
		{
			Reset(newBaseArray, length);
		}

		public override void Flush()
		{
		}

		public void Reset(byte[] newBaseArray, int length)
		{
			if (!Resetable)
			{
				throw new InvalidOperationException("Stream is not created as resetable");
			}
			if (newBaseArray.Length < length)
			{
				throw new ArgumentException("Length must be >= newBaseArray.Length");
			}
			m_baseArray = newBaseArray;
			m_length = length;
			m_position = 0;
		}

		/// <summary>
		/// Original C# implementation
		/// </summary>
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = m_length - m_position;
			if (num > count)
			{
				num = count;
			}
			if (num <= 0)
			{
				return 0;
			}
			if (num <= 8)
			{
				int num2 = num;
				while (--num2 >= 0)
				{
					buffer[offset + num2] = m_baseArray[m_position + num2];
				}
			}
			else
			{
				Buffer.BlockCopy(m_baseArray, m_position, buffer, offset, num);
			}
			m_position += num;
			return num;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				m_position = (int)offset;
				break;
			case SeekOrigin.Current:
				m_position += (int)offset;
				break;
			case SeekOrigin.End:
				m_position = m_length + (int)offset;
				break;
			default:
				throw new ArgumentException("Invalid seek origin");
			}
			return m_position;
		}

		public void EnsureCapacity(long minimumSize)
		{
			if (m_length < minimumSize)
			{
				if (!Expandable)
				{
					throw new EndOfStreamException("ByteSteam is not large enough and is not expandable");
				}
				if (minimumSize < 256)
				{
					minimumSize = 256L;
				}
				if (minimumSize < m_length * 2)
				{
					minimumSize = m_length * 2;
				}
				Resize(minimumSize);
			}
		}

		public void CheckCapacity(long minimumSize)
		{
			if (m_length < minimumSize)
			{
				throw new EndOfStreamException("Stream does not have enough size");
			}
		}

		private void Resize(long size)
		{
			Array.Resize(ref m_baseArray, (int)size);
			m_length = m_baseArray.Length;
		}

		public override void SetLength(long value)
		{
			if (Expandable)
			{
				Resize((int)value);
				return;
			}
			throw new InvalidOperationException("ByteStream is not expandable");
		}

		public new byte ReadByte()
		{
			CheckCapacity(m_position + 1);
			byte result = m_baseArray[m_position];
			m_position++;
			return result;
		}

		public new void WriteByte(byte value)
		{
			EnsureCapacity(m_position + 1);
			m_baseArray[m_position] = value;
			m_position++;
		}

		public unsafe ushort ReadUShort()
		{
			CheckCapacity(m_position + 2);
			fixed (byte* ptr = &m_baseArray[m_position])
			{
				m_position += 2;
				return *(ushort*)ptr;
			}
		}

		public unsafe void WriteUShort(ushort value)
		{
			EnsureCapacity(m_position + 2);
			fixed (byte* ptr = &m_baseArray[m_position])
			{
				*(ushort*)ptr = value;
			}
			m_position += 2;
		}

		/// <summary>
		/// Original C# implementation
		/// </summary>
		public override void Write(byte[] buffer, int offset, int count)
		{
			EnsureCapacity(m_position + count);
			int position = m_position + count;
			if (count <= 128 && buffer != m_baseArray)
			{
				int num = count;
				while (--num >= 0)
				{
					m_baseArray[m_position + num] = buffer[offset + num];
				}
			}
			else
			{
				Buffer.BlockCopy(buffer, offset, m_baseArray, m_position, count);
			}
			m_position = position;
		}

		internal unsafe void Write(IntPtr srcPtr, int offset, int count)
		{
			EnsureCapacity(m_position + count);
			fixed (byte* destination = &m_baseArray[m_position])
			{
				Unsafe.CopyBlockUnaligned(destination, (srcPtr + offset).ToPointer(), (uint)count);
			}
			m_position += count;
		}
	}
}
