using System;
using System.Text;

namespace VRage.Http
{
	public static class MyHttpTools
	{
		private static bool ByteArrayHasPrefix(byte[] prefix, byte[] byteArray)
		{
			if (prefix == null || byteArray == null || prefix.Length > byteArray.Length)
			{
				return false;
			}
			for (int i = 0; i < prefix.Length; i++)
			{
				if (prefix[i] != byteArray[i])
				{
					return false;
				}
			}
			return true;
		}

		public static string ConvertToString(byte[] data, string preferredEncodingName = null)
		{
			if (data != null && data.Length != 0)
			{
				Encoding encoding = null;
				int num = 0;
				if (!string.IsNullOrEmpty(preferredEncodingName))
				{
					try
					{
						encoding = Encoding.GetEncoding(preferredEncodingName);
						num = (ByteArrayHasPrefix(encoding.GetPreamble(), data) ? encoding.GetPreamble().Length : 0);
					}
					catch (Exception)
					{
						encoding = Encoding.UTF8;
					}
				}
				if (encoding == null)
				{
					Encoding[] array = new Encoding[4]
					{
						Encoding.UTF8,
						Encoding.UTF32,
						Encoding.Unicode,
						Encoding.BigEndianUnicode
					};
					for (int i = 0; i < array.Length; i++)
					{
						byte[] preamble = array[i].GetPreamble();
						if (ByteArrayHasPrefix(preamble, data))
						{
							encoding = array[i];
							num = preamble.Length;
							break;
						}
					}
				}
				if (encoding == null)
				{
					encoding = Encoding.UTF8;
					if (num == -1)
					{
						byte[] preamble2 = encoding.GetPreamble();
						num = (ByteArrayHasPrefix(preamble2, data) ? preamble2.Length : 0);
					}
				}
				return encoding.GetString(data, num, data.Length - num);
			}
			return null;
		}
	}
}
