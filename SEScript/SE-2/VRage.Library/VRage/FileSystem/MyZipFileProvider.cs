using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VRage.Compression;

namespace VRage.FileSystem
{
	public class MyZipFileProvider : IFileProvider
	{
		public readonly char[] Separators = new char[2]
		{
			Path.AltDirectorySeparatorChar,
			Path.DirectorySeparatorChar
		};

		/// <summary>
		/// FileShare is ignored
		/// Usage: C:\Users\Data\Archive.zip\InnerFolder\file.txt
		/// </summary>
		public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			if (mode != FileMode.Open || access != FileAccess.Read)
			{
				return null;
			}
			return TryDoZipAction(path, TryOpen, null);
		}

		private T TryDoZipAction<T>(string path, Func<string, string, T> action, T defaultValue)
		{
			for (int num = path.Length; num >= 0; num = path.LastIndexOfAny(Separators, num - 1))
			{
				string text = path.Substring(0, num);
				if (File.Exists(text))
				{
					return action(text, path.Substring(Math.Min(path.Length, num + 1)));
				}
			}
			return defaultValue;
		}

		private Stream TryOpen(string zipFile, string subpath)
		{
			MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(zipFile);
			try
			{
				if (myZipArchive.FileExists(subpath))
				{
					MyZipFileInfo file = myZipArchive.GetFile(subpath);
					return new MyStreamWrapper(file, myZipArchive, file.Length);
				}
				return null;
			}
			catch
			{
				myZipArchive.Dispose();
				return null;
			}
		}

		public bool DirectoryExists(string path)
		{
			return TryDoZipAction(path, DirectoryExistsInZip, defaultValue: false);
		}

		private bool DirectoryExistsInZip(string zipFile, string subpath)
		{
			MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(zipFile);
			try
			{
				return subpath == string.Empty || myZipArchive.DirectoryExists(subpath + "/");
			}
			finally
			{
				myZipArchive.Dispose();
			}
		}

		private MyZipArchive TryGetZipArchive(string zipFile, string subpath)
		{
			MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(zipFile);
			try
			{
				return myZipArchive;
			}
			catch
			{
				myZipArchive.Dispose();
				return null;
			}
		}

		private string TryGetSubpath(string zipFile, string subpath)
		{
			return subpath;
		}

		public IEnumerable<string> GetFiles(string path, string filter, MySearchOption searchOption)
		{
			MyZipArchive zipFile = TryDoZipAction(path, TryGetZipArchive, null);
			string subpath = "";
			if (searchOption == MySearchOption.TopDirectoryOnly)
			{
				subpath = TryDoZipAction(path, TryGetSubpath, null);
			}
			if (zipFile != null)
			{
				string pattern2 = Regex.Escape(filter).Replace("\\*", ".*").Replace("\\?", ".");
				pattern2 += "$";
				foreach (string fileName in zipFile.FileNames)
				{
					if ((searchOption != 0 || fileName.Count((char x) => x == '\\') == subpath.Count((char x) => x == '\\') + 1) && Regex.IsMatch(fileName, pattern2, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
					{
						yield return Path.Combine(zipFile.ZipPath, fileName);
					}
				}
				zipFile.Dispose();
			}
		}

		public bool FileExists(string path)
		{
			return TryDoZipAction(path, FileExistsInZip, defaultValue: false);
		}

		private bool FileExistsInZip(string zipFile, string subpath)
		{
			MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(zipFile);
			try
			{
				return myZipArchive.FileExists(subpath);
			}
			finally
			{
				myZipArchive.Dispose();
			}
		}

		public static bool IsZipFile(string path)
		{
			return !Directory.Exists(path);
		}
	}
}
