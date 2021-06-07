using System;
using System.Collections.Generic;
using VRage.Security;

namespace VRage.Input.Keyboard
{
	internal class MyKeyHasher
	{
		public readonly List<MyKeys> Keys = new List<MyKeys>(10);

		private readonly Md5.Hash m_hash = new Md5.Hash();

		private readonly byte[] m_tmpHashData = new byte[256];

		private void ComputeHash(string salt)
		{
			Keys.Sort(delegate(MyKeys x, MyKeys y)
			{
				byte b = (byte)x;
				return b.CompareTo((byte)y);
			});
			int num = 0;
			foreach (MyKeys key in Keys)
			{
				m_tmpHashData[num++] = (byte)key;
			}
			foreach (char c in salt)
			{
				m_tmpHashData[num++] = (byte)c;
				m_tmpHashData[num++] = (byte)((int)c >> 8);
			}
			Md5.ComputeHash(m_tmpHashData, m_hash);
		}

		private static byte HexToByte(char c)
		{
			if (c >= 'a')
			{
				return (byte)(10 + c - 97);
			}
			if (c >= 'A')
			{
				return (byte)(10 + c - 65);
			}
			return (byte)(c - 48);
		}

		private static byte HexToByte(char c1, char c2)
		{
			return (byte)(HexToByte(c1) * 16 + HexToByte(c2));
		}

		public unsafe bool TestHash(string hash, string salt)
		{
			uint* ptr = stackalloc uint[4];
			for (int i = 0; i < Math.Min(hash.Length, 32) / 2; i++)
			{
				((byte*)ptr)[i] = HexToByte(hash[i * 2], hash[i * 2 + 1]);
			}
			return TestHash(*ptr, ptr[1], ptr[2], ptr[3], salt);
		}

		public bool TestHash(uint h0, uint h1, uint h2, uint h3, string salt)
		{
			ComputeHash(salt);
			if (m_hash.A == h0 && m_hash.B == h1 && m_hash.C == h2)
			{
				return m_hash.D == h3;
			}
			return false;
		}
	}
}
