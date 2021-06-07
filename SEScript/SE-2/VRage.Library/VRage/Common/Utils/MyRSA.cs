using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using VRage.Cryptography;

namespace VRage.Common.Utils
{
	public class MyRSA
	{
		private HashAlgorithm m_hasher;

		public HashAlgorithm HashObject => m_hasher;

		public MyRSA()
		{
			m_hasher = MySHA256.Create();
			m_hasher.Initialize();
		}

		public void GenerateKeys(string publicKeyFileName, string privateKeyFileName)
		{
			GenerateKeys(out byte[] publicKey, out byte[] privateKey);
			if (publicKey != null && privateKey != null)
			{
				File.WriteAllText(publicKeyFileName, Convert.ToBase64String(publicKey));
				File.WriteAllText(privateKeyFileName, Convert.ToBase64String(privateKey));
			}
		}

		/// <summary>
		/// Generate keys into specified files.
		/// </summary>
		/// <param name="publicKeyFileName">Name of the file that will contain public key</param>
		/// <param name="privateKeyFileName">Name of the file that will contain private key</param>
		public void GenerateKeys(out byte[] publicKey, out byte[] privateKey)
		{
			CspParameters cspParameters = null;
			RSACryptoServiceProvider rSACryptoServiceProvider = null;
			try
			{
				cspParameters = new CspParameters
				{
					ProviderType = 1,
					Flags = CspProviderFlags.UseArchivableKey,
					KeyNumber = 1
				};
				rSACryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
				rSACryptoServiceProvider.PersistKeyInCsp = false;
				publicKey = rSACryptoServiceProvider.ExportCspBlob(includePrivateParameters: false);
				privateKey = rSACryptoServiceProvider.ExportCspBlob(includePrivateParameters: true);
			}
			catch (Exception)
			{
				publicKey = null;
				privateKey = null;
			}
			finally
			{
				if (rSACryptoServiceProvider != null)
				{
					rSACryptoServiceProvider.PersistKeyInCsp = false;
				}
			}
		}

		/// <summary>
		/// Signs given data with provided key.
		/// </summary>
		/// <param name="data">data to sign (in base64 form)</param>
		/// <param name="privateKey">private key (in base64 form)</param>
		/// <returns>Signed data (string in base64 form)</returns>
		public string SignData(string data, string privateKey)
		{
			byte[] inArray;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.PersistKeyInCsp = false;
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				byte[] bytes = uTF8Encoding.GetBytes(data);
				try
				{
					rSACryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(privateKey));
					inArray = rSACryptoServiceProvider.SignData(bytes, m_hasher);
				}
				catch (CryptographicException)
				{
					return null;
				}
				finally
				{
					rSACryptoServiceProvider.PersistKeyInCsp = false;
				}
			}
			return Convert.ToBase64String(inArray);
		}

		/// <summary>
		/// Signs given hash with provided key.
		/// </summary>
		/// <param name="hash">hash to sign</param>
		/// <param name="privateKey">private key</param>
		/// <returns>Signed hash (string in base64 form)</returns>
		public string SignHash(byte[] hash, byte[] privateKey)
		{
			byte[] inArray;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.PersistKeyInCsp = false;
				try
				{
					rSACryptoServiceProvider.ImportCspBlob(privateKey);
					inArray = rSACryptoServiceProvider.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));
				}
				catch (CryptographicException)
				{
					return null;
				}
				finally
				{
					rSACryptoServiceProvider.PersistKeyInCsp = false;
				}
			}
			return Convert.ToBase64String(inArray);
		}

		/// <summary>
		/// Signs given hash with provided key.
		/// </summary>
		/// <param name="hash">hash to sign (in base64 form)</param>
		/// <param name="privateKey">private key (in base64 form)</param>
		/// <returns>Signed hash (string in base64 form)</returns>
		public string SignHash(string hash, string privateKey)
		{
			byte[] inArray;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.PersistKeyInCsp = false;
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				byte[] bytes = uTF8Encoding.GetBytes(hash);
				try
				{
					rSACryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(privateKey));
					inArray = rSACryptoServiceProvider.SignHash(bytes, CryptoConfig.MapNameToOID("SHA256"));
				}
				catch (CryptographicException)
				{
					return null;
				}
				finally
				{
					rSACryptoServiceProvider.PersistKeyInCsp = false;
				}
			}
			return Convert.ToBase64String(inArray);
		}

		/// <summary>
		/// Verifies that a digital signature is valid by determining the hash value
		/// in the signature using the provided public key and comparing it to the provided hash value.
		/// </summary>
		/// <param name="hash">hash to test</param>
		/// <param name="signedHash">already signed hash</param>
		/// <param name="publicKey">signature</param>
		/// <returns>true if the signature is valid; otherwise, false.</returns>
		public bool VerifyHash(byte[] hash, byte[] signedHash, byte[] publicKey)
		{
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				try
				{
					rSACryptoServiceProvider.ImportCspBlob(publicKey);
					return rSACryptoServiceProvider.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), signedHash);
				}
				catch (CryptographicException)
				{
					return false;
				}
				finally
				{
					rSACryptoServiceProvider.PersistKeyInCsp = false;
				}
			}
		}

		/// <summary>
		/// Verifies that a digital signature is valid by determining the hash value
		/// in the signature using the provided public key and comparing it to the provided hash value.
		/// </summary>
		/// <param name="hash">hash to test</param>
		/// <param name="signedHash">already signed hash (in base64 form)</param>
		/// <param name="publicKey">signature (in base64 form)</param>
		/// <returns>true if the signature is valid; otherwise, false.</returns>
		public bool VerifyHash(string hash, string signedHash, string publicKey)
		{
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				byte[] bytes = uTF8Encoding.GetBytes(hash);
				byte[] rgbSignature = Convert.FromBase64String(signedHash);
				try
				{
					rSACryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(publicKey));
					return rSACryptoServiceProvider.VerifyHash(bytes, CryptoConfig.MapNameToOID("SHA256"), rgbSignature);
				}
				catch (CryptographicException)
				{
					return false;
				}
				finally
				{
					rSACryptoServiceProvider.PersistKeyInCsp = false;
				}
			}
		}

		/// <summary>
		/// Verifies that a digital signature is valid by determining the hash value
		/// in the signature using the provided public key and comparing it to the hash value of the provided data.
		/// </summary>
		/// <param name="originalMessage">original data</param>
		/// <param name="signedMessage">signed message (in base64 form)</param>
		/// <param name="publicKey">signature (in base64 form)</param>
		/// <returns>true if the signature is valid; otherwise, false.</returns>
		public bool VerifyData(string originalMessage, string signedMessage, string publicKey)
		{
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				byte[] bytes = uTF8Encoding.GetBytes(originalMessage);
				byte[] signature = Convert.FromBase64String(signedMessage);
				try
				{
					rSACryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(publicKey));
					return rSACryptoServiceProvider.VerifyData(bytes, m_hasher, signature);
				}
				catch (CryptographicException)
				{
					return false;
				}
				finally
				{
					rSACryptoServiceProvider.PersistKeyInCsp = false;
				}
			}
		}
	}
}
