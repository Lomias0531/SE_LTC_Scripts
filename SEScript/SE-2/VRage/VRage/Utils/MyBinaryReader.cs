using System.IO;
using System.Security;
using System.Text;

namespace VRage.Utils
{
	public class MyBinaryReader : BinaryReader
	{
		private Decoder m_decoder;

		private int m_maxCharsSize;

		private byte[] m_charBytes;

		private char[] m_charBuffer;

		public MyBinaryReader(Stream stream)
			: this(stream, new UTF8Encoding())
		{
		}

		public MyBinaryReader(Stream stream, Encoding encoding)
			: base(stream, encoding)
		{
			m_decoder = encoding.GetDecoder();
			m_maxCharsSize = encoding.GetMaxCharCount(128);
			encoding.GetMaxByteCount(1);
			_ = 16;
		}

		public new int Read7BitEncodedInt()
		{
			int num = 0;
			int num2 = 0;
			byte b;
			do
			{
				if (num2 == 35)
				{
					return -1;
				}
				b = ReadByte();
				num |= (b & 0x7F) << num2;
				num2 += 7;
			}
			while ((b & 0x80) != 0);
			return num;
		}

		[SecuritySafeCritical]
		public string ReadStringIncomplete(out bool isComplete)
		{
			if (BaseStream == null)
			{
				isComplete = false;
				return string.Empty;
			}
			int num = 0;
			int num2 = Read7BitEncodedInt();
			if (num2 < 0)
			{
				isComplete = false;
				return string.Empty;
			}
			if (num2 == 0)
			{
				isComplete = true;
				return string.Empty;
			}
			if (m_charBytes == null)
			{
				m_charBytes = new byte[128];
			}
			if (m_charBuffer == null)
			{
				m_charBuffer = new char[m_maxCharsSize];
			}
			StringBuilder stringBuilder = null;
			do
			{
				int count = (num2 - num > 128) ? 128 : (num2 - num);
				int num3 = BaseStream.Read(m_charBytes, 0, count);
				if (num3 == 0)
				{
					isComplete = false;
					if (stringBuilder == null)
					{
						return string.Empty;
					}
					return stringBuilder.ToString();
				}
				int chars = m_decoder.GetChars(m_charBytes, 0, num3, m_charBuffer, 0);
				if (num == 0 && num3 == num2)
				{
					isComplete = true;
					return new string(m_charBuffer, 0, chars);
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(num2);
				}
				stringBuilder.Append(m_charBuffer, 0, chars);
				num += num3;
			}
			while (num < num2);
			isComplete = true;
			return stringBuilder.ToString();
		}
	}
}
