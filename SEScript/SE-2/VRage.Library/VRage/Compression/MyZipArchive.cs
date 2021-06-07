using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;

namespace VRage.Compression
{
	/// <summary>
	/// Class based on http://www.codeproject.com/Articles/209731/Csharp-use-Zip-archives-without-external-libraries.
	/// </summary>
	public class MyZipArchive : IDisposable
	{
		private readonly ZipArchive m_zip;

		private readonly Dictionary<string, string> m_mixedCaseHelper = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

		public string ZipPath
		{
			get;
			private set;
		}

		public IEnumerable<string> FileNames => from p in Files
			select p.FullName into p
			orderby p
			select p;

		public ReadOnlyCollection<ZipArchiveEntry> Files => m_zip.Entries;

		private MyZipArchive(ZipArchive zipObject, string path = null)
		{
			m_zip = zipObject;
			ZipPath = path;
			if (m_zip.Mode != ZipArchiveMode.Create)
			{
				foreach (ZipArchiveEntry file in Files)
				{
					m_mixedCaseHelper[file.FullName.Replace('/', '\\')] = file.FullName;
				}
			}
		}

		private static void FixName(ref string name)
		{
			name = name.Replace('/', '\\');
		}

		public static MyZipArchive OpenOnFile(string path, ZipArchiveMode mode = ZipArchiveMode.Read)
		{
			return new MyZipArchive(ZipFile.Open(path, mode), path);
		}

		public MyZipFileInfo AddFile(string path, CompressionLevel level)
		{
			return new MyZipFileInfo(m_zip.CreateEntry(path, level));
		}

		public MyZipFileInfo GetFile(string name)
		{
			FixName(ref name);
			return new MyZipFileInfo(m_zip.GetEntry(m_mixedCaseHelper[name]));
		}

		public bool FileExists(string name)
		{
			FixName(ref name);
			return m_mixedCaseHelper.ContainsKey(name);
		}

		public bool DirectoryExists(string name)
		{
			FixName(ref name);
			foreach (string key in m_mixedCaseHelper.Keys)
			{
				if (key.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public void Dispose()
		{
			m_zip.Dispose();
		}

		public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
		{
			ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
		}
	}
}
