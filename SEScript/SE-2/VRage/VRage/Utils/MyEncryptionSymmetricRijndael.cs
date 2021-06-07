using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VRage.Utils
{
	public static class MyEncryptionSymmetricRijndael
	{
		public static string EncryptString(string inputText, string password)
		{
			if (inputText.Length <= 0)
			{
				return "";
			}
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			byte[] bytes = Encoding.Unicode.GetBytes(inputText);
			byte[] bytes2 = Encoding.ASCII.GetBytes(password.Length.ToString());
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(password, bytes2);
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] inArray = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(inArray);
		}

		public static string DecryptString(string inputText, string password)
		{
			if (inputText.Length <= 0)
			{
				return "";
			}
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			byte[] array = Convert.FromBase64String(inputText);
			byte[] bytes = Encoding.ASCII.GetBytes(password.Length.ToString());
			PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(password, bytes);
			ICryptoTransform transform = rijndaelManaged.CreateDecryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
			MemoryStream memoryStream = new MemoryStream(array);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
			byte[] array2 = new byte[array.Length];
			int count = cryptoStream.Read(array2, 0, array2.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.Unicode.GetString(array2, 0, count);
		}
	}
}
