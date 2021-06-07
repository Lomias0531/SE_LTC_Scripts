using System;
using System.Collections.Generic;
using System.IO;
using VRage.Common.Utils;

namespace VRage.FileSystem
{
	public class MyChecksumVerifier : IFileVerifier
	{
		public readonly string BaseChecksumDir;

		public readonly byte[] PublicKey;

		private Dictionary<string, string> m_checksums;

		public event Action<IFileVerifier, string> ChecksumNotFound;

		public event Action<string, string> ChecksumFailed;

		public MyChecksumVerifier(MyChecksums checksums, string baseChecksumDir)
		{
			PublicKey = checksums.PublicKeyAsArray;
			BaseChecksumDir = baseChecksumDir;
			m_checksums = checksums.Items.Dictionary;
		}

		public Stream Verify(string filename, Stream stream)
		{
			Action<string, string> checksumFailed = this.ChecksumFailed;
			Action<IFileVerifier, string> checksumNotFound = this.ChecksumNotFound;
			if ((checksumFailed != null || checksumNotFound != null) && filename.StartsWith(BaseChecksumDir, StringComparison.InvariantCultureIgnoreCase))
			{
				string key = filename.Substring(BaseChecksumDir.Length + 1);
				if (m_checksums.TryGetValue(key, out string value))
				{
					if (checksumFailed != null)
					{
						return new MyCheckSumStream(stream, filename, Convert.FromBase64String(value), PublicKey, checksumFailed);
					}
				}
				else
				{
					checksumNotFound?.Invoke(this, filename);
				}
			}
			return stream;
		}
	}
}
