using System.Security.Cryptography;

namespace VRage.Cryptography
{
	public static class MySHA256
	{
		private static bool m_supportsFips = true;

		private static SHA256 CreateInternal()
		{
			if (m_supportsFips)
			{
				return new SHA256CryptoServiceProvider();
			}
			return new SHA256Managed();
		}

		/// <summary>
		/// Creates FIPS compliant crypto provider if available, otherwise pure managed implementation.
		/// </summary>
		public static SHA256 Create()
		{
			try
			{
				return CreateInternal();
			}
			catch
			{
				m_supportsFips = false;
				return CreateInternal();
			}
		}
	}
}
