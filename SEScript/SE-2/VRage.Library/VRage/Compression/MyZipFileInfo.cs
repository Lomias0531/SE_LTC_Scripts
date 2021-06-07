using System.IO;
using System.IO.Compression;

namespace VRage.Compression
{
	public struct MyZipFileInfo
	{
		private readonly ZipArchiveEntry m_fileInfo;

		public string Name => m_fileInfo.Name;

		public long Length => m_fileInfo.Length;

		internal MyZipFileInfo(ZipArchiveEntry fileInfo)
		{
			m_fileInfo = fileInfo;
		}

		public Stream GetStream()
		{
			return m_fileInfo.Open();
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
