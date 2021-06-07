using System;
using System.IO;

namespace VRage
{
	public static class DirectoryExtensions
	{
		public static void CopyAll(string source, string target)
		{
			EnsureDirectoryExists(target);
			FileInfo[] files = new DirectoryInfo(source).GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				fileInfo.CopyTo(Path.Combine(target, fileInfo.Name), overwrite: true);
			}
			DirectoryInfo[] directories = new DirectoryInfo(source).GetDirectories();
			foreach (DirectoryInfo directoryInfo in directories)
			{
				DirectoryInfo directoryInfo2 = Directory.CreateDirectory(Path.Combine(target, directoryInfo.Name));
				CopyAll(directoryInfo.FullName, directoryInfo2.FullName);
			}
		}

		public static void EnsureDirectoryExists(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (directoryInfo.Parent != null)
			{
				EnsureDirectoryExists(directoryInfo.Parent.FullName);
			}
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
		}

		public static bool IsParentOf(this DirectoryInfo dir, string absPath)
		{
			string value = dir.FullName.TrimEnd(new char[1]
			{
				Path.DirectorySeparatorChar
			});
			DirectoryInfo directoryInfo = new DirectoryInfo(absPath);
			while (directoryInfo.Exists)
			{
				if (directoryInfo.FullName.TrimEnd(new char[1]
				{
					Path.DirectorySeparatorChar
				}).Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				if (!directoryInfo.FullName.TrimEnd(new char[1]
				{
					Path.DirectorySeparatorChar
				}).StartsWith(value))
				{
					return false;
				}
				if (directoryInfo.Parent == null)
				{
					return false;
				}
				directoryInfo = directoryInfo.Parent;
			}
			return false;
		}
	}
}
