using System;
using System.IO;

namespace VRage.FileSystem
{
	public class MyNullVerifier : IFileVerifier
	{
		public event Action<IFileVerifier, string> ChecksumNotFound
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<string, string> ChecksumFailed
		{
			add
			{
			}
			remove
			{
			}
		}

		public Stream Verify(string filename, Stream stream)
		{
			return stream;
		}
	}
}
