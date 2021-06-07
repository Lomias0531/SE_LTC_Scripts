using System;
using System.IO;

namespace VRage.Compression
{
	/// <summary>
	/// Stream wrapper which will close both stream and other IDisposable object
	/// </summary>
	public class MyStreamWrapper : Stream
	{
		private readonly IDisposable m_obj;

		private Stream m_innerStream;

		private readonly long m_length;

		private readonly MyZipFileInfo m_file;

		private long m_position;

		public override bool CanRead => m_innerStream.CanRead;

		public override bool CanSeek => m_innerStream.CanSeek;

		public override bool CanWrite => m_innerStream.CanWrite;

		public override long Length => m_length;

		public override long Position
		{
			get
			{
				if (CanSeek)
				{
					return m_innerStream.Position;
				}
				return m_position;
			}
			set
			{
				Seek(value, SeekOrigin.Begin);
			}
		}

		public MyStreamWrapper(MyZipFileInfo file, IDisposable objectToClose, long length)
		{
			m_file = file;
			m_innerStream = file.GetStream();
			m_obj = objectToClose;
			m_length = length;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && m_obj != null)
			{
				m_obj.Dispose();
			}
			base.Dispose(disposing);
		}

		public override void Flush()
		{
			m_innerStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = m_innerStream.Read(buffer, offset, count);
			m_position += num;
			return num;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if (m_innerStream.CanSeek)
			{
				m_position = m_innerStream.Seek(offset, origin);
				return m_position;
			}
			switch (origin)
			{
			case SeekOrigin.Begin:
				if (m_position <= offset)
				{
					m_innerStream.SkipBytes((int)(offset - m_position));
					m_position = offset;
					return m_position;
				}
				break;
			case SeekOrigin.Current:
				if (offset >= 0)
				{
					m_innerStream.SkipBytes((int)offset);
					m_position += offset;
					return m_position;
				}
				break;
			case SeekOrigin.End:
				if (m_position <= Length - offset)
				{
					m_innerStream.SkipBytes((int)(Length - offset - m_position));
					m_position += Length - offset;
					return m_position;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			m_innerStream = m_file.GetStream();
			switch (origin)
			{
			case SeekOrigin.Begin:
				m_innerStream.SkipBytes((int)offset);
				m_position = offset;
				return m_position;
			case SeekOrigin.Current:
				m_position += offset;
				m_innerStream.SkipBytes((int)m_position);
				return m_position;
			case SeekOrigin.End:
				m_innerStream.SkipBytes((int)(Length - offset));
				m_position = Length - offset;
				return m_position;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public override void SetLength(long value)
		{
			m_innerStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			m_position += count;
			m_innerStream.Write(buffer, offset, count);
		}
	}
}
