namespace VRage.Utils
{
	public class MySingleCrypto
	{
		private readonly byte[] m_password;

		private MySingleCrypto()
		{
		}

		public MySingleCrypto(byte[] password)
		{
			m_password = (byte[])password.Clone();
		}

		public void Encrypt(byte[] data, int length)
		{
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				data[i] = (byte)(data[i] + m_password[num]);
				num++;
				num %= m_password.Length;
			}
		}

		public void Decrypt(byte[] data, int length)
		{
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				data[i] = (byte)(data[i] - m_password[num]);
				num++;
				num %= m_password.Length;
			}
		}
	}
}
