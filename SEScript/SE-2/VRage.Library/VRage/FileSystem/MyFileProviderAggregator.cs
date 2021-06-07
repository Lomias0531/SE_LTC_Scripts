using System.Collections.Generic;
using System.IO;
using VRage.Collections;

namespace VRage.FileSystem
{
	public class MyFileProviderAggregator : IFileProvider
	{
		private HashSet<IFileProvider> m_providers = new HashSet<IFileProvider>();

		public HashSetReader<IFileProvider> Providers => new HashSetReader<IFileProvider>(m_providers);

		public MyFileProviderAggregator(params IFileProvider[] providers)
		{
			foreach (IFileProvider provider in providers)
			{
				AddProvider(provider);
			}
		}

		public void AddProvider(IFileProvider provider)
		{
			m_providers.Add(provider);
		}

		public void RemoveProvider(IFileProvider provider)
		{
			m_providers.Remove(provider);
		}

		public Stream OpenRead(string path)
		{
			return Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public Stream OpenWrite(string path, FileMode mode = FileMode.OpenOrCreate)
		{
			return Open(path, mode, FileAccess.Write, FileShare.Read);
		}

		/// <summary>
		/// Opens file, returns null when file does not exists or cannot be opened
		/// </summary>
		public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			foreach (IFileProvider provider in m_providers)
			{
				try
				{
					Stream stream = provider.Open(path, mode, access, share);
					if (stream != null)
					{
						return stream;
					}
				}
				catch
				{
				}
			}
			return null;
		}

		public bool DirectoryExists(string path)
		{
			foreach (IFileProvider provider in m_providers)
			{
				try
				{
					if (provider.DirectoryExists(path))
					{
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}

		public IEnumerable<string> GetFiles(string path, string filter, MySearchOption searchOption)
		{
			foreach (IFileProvider provider in m_providers)
			{
				try
				{
					IEnumerable<string> files = provider.GetFiles(path, filter, searchOption);
					if (files != null)
					{
						return files;
					}
				}
				catch
				{
				}
			}
			return null;
		}

		public bool FileExists(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			foreach (IFileProvider provider in m_providers)
			{
				try
				{
					if (provider.FileExists(path))
					{
						return true;
					}
				}
				catch
				{
				}
			}
			return false;
		}
	}
}
