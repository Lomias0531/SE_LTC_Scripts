using System;
using VRage.FileSystem;

namespace VRage.Filesystem
{
	internal class MyFileChecksumWatcher : IDisposable
	{
		public bool ChecksumFound
		{
			get;
			private set;
		}

		public bool ChecksumValid
		{
			get;
			private set;
		}

		public MyFileChecksumWatcher()
		{
			ChecksumFound = true;
			ChecksumValid = true;
			MyFileSystem.FileVerifier.ChecksumFailed += FileVerifier_ChecksumFailed;
			MyFileSystem.FileVerifier.ChecksumNotFound += FileVerifier_ChecksumNotFound;
		}

		public void Reset()
		{
			ChecksumValid = true;
			ChecksumFound = true;
		}

		private void FileVerifier_ChecksumNotFound(IFileVerifier arg1, string arg2)
		{
			ChecksumFound = false;
			ChecksumValid = false;
		}

		private void FileVerifier_ChecksumFailed(string arg1, string arg2)
		{
			ChecksumFound = true;
			ChecksumValid = false;
		}

		void IDisposable.Dispose()
		{
			MyFileSystem.FileVerifier.ChecksumFailed -= FileVerifier_ChecksumFailed;
			MyFileSystem.FileVerifier.ChecksumNotFound -= FileVerifier_ChecksumNotFound;
		}
	}
}
