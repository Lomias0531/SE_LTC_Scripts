using System;
using System.IO;

namespace VRage.FileSystem
{
	public interface IFileVerifier
	{
		event Action<IFileVerifier, string> ChecksumNotFound;

		event Action<string, string> ChecksumFailed;

		Stream Verify(string filename, Stream stream);
	}
}
