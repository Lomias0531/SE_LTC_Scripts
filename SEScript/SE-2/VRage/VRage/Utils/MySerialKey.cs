using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VRage.Utils
{
	public static class MySerialKey
	{
		private static int m_dataSize = 14;

		private static int m_hashSize = 4;

		public static string[] Generate(short productTypeId, short distributorId, int keyCount)
		{
			byte[] bytes = BitConverter.GetBytes(productTypeId);
			byte[] bytes2 = BitConverter.GetBytes(distributorId);
			using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
			{
				using (SHA1Managed sHA1Managed = new SHA1Managed())
				{
					List<string> list = new List<string>(keyCount);
					byte[] array = new byte[m_dataSize + m_hashSize];
					for (int i = 0; i < keyCount; i++)
					{
						rNGCryptoServiceProvider.GetBytes(array);
						array[0] = bytes[0];
						array[1] = bytes[1];
						array[2] = bytes2[0];
						array[3] = bytes2[1];
						for (int j = 0; j < 4; j++)
						{
							array[j] = (byte)(array[j] ^ array[j + 4]);
						}
						byte[] array2 = sHA1Managed.ComputeHash(array, 0, m_dataSize);
						for (int k = 0; k < m_hashSize; k++)
						{
							array[m_dataSize + k] = array2[k];
						}
						list.Add(new string(My5BitEncoding.Default.Encode(array.ToArray())) + "X");
					}
					return list.ToArray();
				}
			}
		}

		public static string AddDashes(string key)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < key.Length; i++)
			{
				if (i % 5 == 0 && i > 0)
				{
					stringBuilder.Append('-');
				}
				stringBuilder.Append(key[i]);
			}
			return stringBuilder.ToString();
		}

		public static string RemoveDashes(string key)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < key.Length; i++)
			{
				if ((i + 1) % 6 != 0)
				{
					stringBuilder.Append(key[i]);
				}
			}
			return stringBuilder.ToString();
		}

		public static bool ValidateSerial(string serialKey, out int productTypeId, out int distributorId)
		{
			using (new RNGCryptoServiceProvider())
			{
				using (SHA1 sHA = SHA1.Create())
				{
					if (serialKey.EndsWith("X"))
					{
						byte[] array = My5BitEncoding.Default.Decode(serialKey.Take(serialKey.Length - 1).ToArray());
						byte[] array2 = array.Take(array.Length - m_hashSize).ToArray();
						byte[] source = sHA.ComputeHash(array2);
						if (array.Skip(array2.Length).Take(m_hashSize).SequenceEqual(source.Take(m_hashSize)))
						{
							for (int i = 0; i < 4; i++)
							{
								array2[i] = (byte)(array2[i] ^ array2[i + 4]);
							}
							productTypeId = BitConverter.ToInt16(array2, 0);
							distributorId = BitConverter.ToInt16(array2, 2);
							return true;
						}
					}
					productTypeId = 0;
					distributorId = 0;
					return false;
				}
			}
		}
	}
}
