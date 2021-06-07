using System;
using System.IO;

namespace VRage
{
	public class ResetableMemoryStream : Stream
	{
		private byte[] m_baseArray;

		private int m_position;

		private int m_length;

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

		public ResetableMemoryStream()
		{
		}

		public ResetableMemoryStream(byte[] baseArray, int length)
		{
			Reset(baseArray, length);
		}

		public void Reset(byte[] newBaseArray, int length)
		{
			if (newBaseArray.Length < length)
			{
				throw new ArgumentException("Length must be >= newBaseArray.Length");
			}
			m_baseArray = newBaseArray;
			m_length = length;
			m_position = 0;
		}

		public byte[] GetInternalBuffer()
		{
			return m_baseArray;
		}

		public override void Flush()
		{
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

		public override void SetLength(long value)
		{
			throw new InvalidOperationException("Operation not supported");
		}

		/// <summary>
		/// Original C# implementation
		/// </summary>
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (m_length < m_position + count)
			{
				throw new EndOfStreamException();
			}
			int position = m_position + count;
			if (count <= 8 && buffer != m_baseArray)
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
	}
}
