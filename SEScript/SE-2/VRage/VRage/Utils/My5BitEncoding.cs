using System;
using System.Collections.Generic;
using System.Text;

namespace VRage.Utils
{
	public class My5BitEncoding
	{
		private static My5BitEncoding m_default;

		private char[] m_encodeTable;

		private Dictionary<char, byte> m_decodeTable;

		public static My5BitEncoding Default
		{
			get
			{
				if (m_default == null)
				{
					m_default = new My5BitEncoding();
				}
				return m_default;
			}
		}

		/// <summary>
		/// Initializes a new instance of the Encoding5Bit class.
		/// Uses characters 0-9 and A-Z except (0,O,1,I).
		/// </summary>
		public My5BitEncoding()
			: this(new char[32]
			{
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9',
				'A',
				'B',
				'C',
				'D',
				'E',
				'F',
				'G',
				'H',
				'J',
				'K',
				'L',
				'M',
				'N',
				'P',
				'Q',
				'R',
				'S',
				'T',
				'U',
				'V',
				'W',
				'X',
				'Y',
				'Z'
			})
		{
		}

		/// <summary>
		/// Initializes a new instance of the Encoding5Bit class.
		/// </summary>
		/// <param name="characters">32 characters which will be used when encoding bytes to string.</param>
		public My5BitEncoding(char[] characters)
		{
			if (characters.Length != 32)
			{
				throw new ArgumentException("Characters array must have 32 characters!");
			}
			m_encodeTable = new char[32];
			characters.CopyTo(m_encodeTable, 0);
			m_decodeTable = CreateDecodeDict();
		}

		private Dictionary<char, byte> CreateDecodeDict()
		{
			Dictionary<char, byte> dictionary = new Dictionary<char, byte>(m_encodeTable.Length);
			for (byte b = 0; b < (byte)m_encodeTable.Length; b = (byte)(b + 1))
			{
				dictionary.Add(m_encodeTable[b], b);
			}
			return dictionary;
		}

		/// <summary>
		/// Encodes data as 5bit string.
		/// </summary>
		public char[] Encode(byte[] data)
		{
			StringBuilder stringBuilder = new StringBuilder(data.Length * 8 / 5);
			int num = 0;
			int num2 = 0;
			foreach (byte b in data)
			{
				num += b << num2;
				num2 += 8;
				while (num2 >= 5)
				{
					int num3 = num & 0x1F;
					num >>= 5;
					num2 -= 5;
					stringBuilder.Append(m_encodeTable[num3]);
				}
			}
			if (num2 > 0)
			{
				stringBuilder.Append(m_encodeTable[num]);
			}
			return stringBuilder.ToString().ToCharArray();
		}

		/// <summary>
		/// Decodes 5bit string as bytes, not alligned characters may be lost.
		/// Only decode strings encoded with Encode.
		/// </summary>
		/// <param name="encoded5BitText"></param>
		/// <returns></returns>
		public byte[] Decode(char[] encoded5BitText)
		{
			List<byte> list = new List<byte>();
			int num = 0;
			int num2 = 0;
			foreach (char key in encoded5BitText)
			{
				if (!m_decodeTable.TryGetValue(key, out byte value))
				{
					throw new ArgumentException("Encoded text is not valid for this encoding!");
				}
				num += value << num2;
				num2 += 5;
				while (num2 >= 8)
				{
					int num3 = num & 0xFF;
					num >>= 8;
					num2 -= 8;
					list.Add((byte)num3);
				}
			}
			return list.ToArray();
		}
	}
}
