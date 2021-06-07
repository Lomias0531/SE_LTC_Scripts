using System;
using System.IO;
using System.IO.Compression;

namespace VRage
{
	public class MyCompressionStreamLoad : IMyCompressionLoad
	{
		private static byte[] m_intBytesBuffer = new byte[4];

		private MemoryStream m_input;

		private GZipStream m_gz;

		private BufferedStream m_buffer;

		public MyCompressionStreamLoad(byte[] compressedData)
		{
			m_input = new MemoryStream(compressedData);
			m_input.Read(m_intBytesBuffer, 0, 4);
			m_gz = new GZipStream(m_input, CompressionMode.Decompress);
			m_buffer = new BufferedStream(m_gz, 16384);
		}

		public int GetInt32()
		{
			m_buffer.Read(m_intBytesBuffer, 0, 4);
			return BitConverter.ToInt32(m_intBytesBuffer, 0);
		}

		public byte GetByte()
		{
			return (byte)m_buffer.ReadByte();
		}

		public int GetBytes(int bytes, byte[] output)
		{
			return m_buffer.Read(output, 0, bytes);
		}

		public bool EndOfFile()
		{
			return m_input.Position == m_input.Length;
		}
	}
}
