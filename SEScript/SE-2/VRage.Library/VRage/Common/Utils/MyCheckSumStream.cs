using System;
using System.IO;

namespace VRage.Common.Utils
{
	internal class MyCheckSumStream : Stream
	{
		private MyRSA m_verifier;

		private Stream m_stream;

		private string m_filename;

		private byte[] m_signedHash;

		private byte[] m_publicKey;

		private Action<string, string> m_failHandler;

		private long m_lastPosition;

		private byte[] m_tmpArray = new byte[1];

		public override bool CanRead => m_stream.CanRead;

		public override bool CanSeek => m_stream.CanSeek;

		public override bool CanWrite => m_stream.CanWrite;

		public override long Length => m_stream.Length;

		public override long Position
		{
			get
			{
				return m_stream.Position;
			}
			set
			{
				m_stream.Position = value;
			}
		}

		internal MyCheckSumStream(Stream stream, string filename, byte[] signedHash, byte[] publicKey, Action<string, string> failHandler)
		{
			m_stream = stream;
			m_verifier = new MyRSA();
			m_signedHash = signedHash;
			m_publicKey = publicKey;
			m_filename = filename;
			m_failHandler = failHandler;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				m_verifier.HashObject.TransformFinalBlock(new byte[0], 0, 0);
				if (!m_verifier.VerifyHash(m_verifier.HashObject.Hash, m_signedHash, m_publicKey))
				{
					m_failHandler(m_filename, Convert.ToBase64String(m_verifier.HashObject.Hash));
				}
				m_stream.Dispose();
			}
			base.Dispose(disposing);
		}

		public override int Read(byte[] array, int offset, int count)
		{
			int num = (int)(m_lastPosition - m_stream.Position);
			int num2 = m_stream.Read(array, offset, count);
			int num3 = offset + num;
			int num4 = num2 - num;
			if (num4 > 0 && num3 > 0)
			{
				m_verifier.HashObject.TransformBlock(array, offset + num, num2 - num, null, 0);
			}
			m_lastPosition = m_stream.Position;
			return num2;
		}

		public override void Flush()
		{
			m_stream.Flush();
		}

		public override int ReadByte()
		{
			if (Read(m_tmpArray, 0, 1) == 0)
			{
				return -1;
			}
			return m_tmpArray[0];
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return m_stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			m_stream.SetLength(value);
		}

		public override void Write(byte[] array, int offset, int count)
		{
			m_stream.Write(array, offset, count);
		}

		public override void WriteByte(byte value)
		{
			m_stream.WriteByte(value);
		}
	}
}
