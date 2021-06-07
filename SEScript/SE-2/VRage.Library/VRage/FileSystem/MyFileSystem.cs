using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VRage.Library.Filesystem;

namespace VRage.FileSystem
{
	public static class MyFileSystem
	{
		public static readonly Assembly MainAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

		public static readonly string MainAssemblyName = MainAssembly.GetName().Name;

		public static string ExePath = new FileInfo(MainAssembly.Location).DirectoryName;

		public static string RootPath = new FileInfo(ExePath).Directory?.FullName ?? Path.GetFullPath(ExePath);

		private static string m_shadersBasePath;

		private static string m_contentPath;

		private static string m_modsPath;

		private static string m_cachePath;

		private static string m_userDataPath;

		private static string m_savesPath;

		public static IFileVerifier FileVerifier = new MyNullVerifier();

		private static MyFileProviderAggregator m_fileProvider = new MyFileProviderAggregator(new MyClassicFileProvider(), new MyZipFileProvider());

		public static string ShadersBasePath
		{
			get
			{
				CheckInitialized();
				return m_shadersBasePath;
			}
		}

		public static string ContentPath
		{
			get
			{
				CheckInitialized();
				return m_contentPath;
			}
		}

		public static string ModsPath
		{
			get
			{
				CheckInitialized();
				return m_modsPath;
			}
		}

		public static string UserDataPath
		{
			get
			{
				CheckInitialized();
				return m_userDataPath;
			}
		}

		public static string SavesPath
		{
			get
			{
				CheckUserSpecificInitialized();
				return m_savesPath;
			}
		}

		public static string CachePath
		{
			get
			{
				CheckUserSpecificInitialized();
				return m_cachePath;
			}
		}

		public static bool IsInitialized => m_contentPath != null;

		private static void CheckInitialized()
		{
			if (!IsInitialized)
			{
				throw new InvalidOperationException("Paths are not initialized, call 'Init'");
			}
		}

		private static void CheckUserSpecificInitialized()
		{
			if (m_userDataPath == null)
			{
				throw new InvalidOperationException("User specific path not initialized, call 'InitUserSpecific'");
			}
		}

		public static void Init(string contentPath, string userData, string modDirName = "Mods", string shadersBasePath = null)
		{
			if (m_contentPath != null)
			{
				throw new InvalidOperationException("Paths already initialized");
			}
			m_contentPath = new DirectoryInfo(contentPath).FullName;
			m_contentPath = UnTerminatePath(m_contentPath);
			m_shadersBasePath = (string.IsNullOrEmpty(shadersBasePath) ? m_contentPath : Path.GetFullPath(shadersBasePath));
			m_userDataPath = Path.GetFullPath(userData);
			m_modsPath = Path.Combine(m_userDataPath, modDirName);
			m_cachePath = Path.Combine(m_userDataPath, "cache");
			Directory.CreateDirectory(m_modsPath);
			Directory.CreateDirectory(m_cachePath);
			string text = Path.Combine(contentPath, "Content.index");
			if (File.Exists(text))
			{
				ContentIndex.Load(text);
			}
		}

		public static void InitUserSpecific(string userSpecificName, string saveDirName = "Saves")
		{
			CheckInitialized();
			if (m_savesPath != null)
			{
				throw new InvalidOperationException("User specific paths already initialized");
			}
			m_savesPath = Path.Combine(m_userDataPath, saveDirName, userSpecificName ?? string.Empty);
			Directory.CreateDirectory(m_savesPath);
		}

		public static void Reset()
		{
			m_contentPath = (m_shadersBasePath = (m_modsPath = (m_userDataPath = (m_savesPath = null))));
		}

		/// <summary>
		/// Replace one of the built-in file providers for MyFileSystem.
		/// </summary>
		/// <typeparam name="TReplaced"></typeparam>
		/// <param name="instance"></param>
		public static void ReplaceFileProvider<TReplaced>(IFileProvider instance)
		{
			IFileProvider fileProvider = null;
			foreach (IFileProvider provider in m_fileProvider.Providers)
			{
				if (provider is TReplaced)
				{
					fileProvider = provider;
					break;
				}
			}
			if (fileProvider != null)
			{
				m_fileProvider.RemoveProvider(fileProvider);
			}
			m_fileProvider.AddProvider(instance);
		}

		public static Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			bool flag = mode == FileMode.Open && access != FileAccess.Write;
			Stream stream = m_fileProvider.Open(path, mode, access, share);
			if (!flag || stream == null)
			{
				return stream;
			}
			return FileVerifier.Verify(path, stream);
		}

		/// <summary>
		/// Opens file for reading
		/// </summary>
		public static Stream OpenRead(string path)
		{
			return Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		/// <summary>
		/// Opens file for reading, convenient method with two paths to combine
		/// </summary>
		public static Stream OpenRead(string path, string subpath)
		{
			return OpenRead(Path.Combine(path, subpath));
		}

		/// <summary>
		/// Creates or overwrites existing file
		/// </summary>
		public static Stream OpenWrite(string path, FileMode mode = FileMode.Create)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			return File.Open(path, mode, FileAccess.Write, FileShare.Read);
		}

		/// <summary>
		/// Creates or overwrites existing file, convenient method with two paths to combine
		/// </summary>
		public static Stream OpenWrite(string path, string subpath, FileMode mode = FileMode.Create)
		{
			return OpenWrite(Path.Combine(path, subpath), mode);
		}

		/// <summary>
		/// Checks write access for file
		/// </summary>
		public static bool CheckFileWriteAccess(string path)
		{
			try
			{
				using (OpenWrite(path, FileMode.Append))
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public static bool FileExists(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			if (IsInitialized && ContentIndex.IsLoaded)
			{
				string fullPath = Path.GetFullPath(path);
				if (fullPath.StartsWith(ContentPath, StringComparison.InvariantCultureIgnoreCase))
				{
					if (fullPath.Length == m_contentPath.Length)
					{
						return false;
					}
					return ContentIndex.FileExists(fullPath.Substring(m_contentPath.Length + 1));
				}
			}
			return m_fileProvider.FileExists(path);
		}

		public static bool DirectoryExists(string path)
		{
			return m_fileProvider.DirectoryExists(path);
		}

		public static IEnumerable<string> GetFiles(string path)
		{
			return m_fileProvider.GetFiles(path, "*", MySearchOption.AllDirectories);
		}

		public static IEnumerable<string> GetFiles(string path, string filter)
		{
			return m_fileProvider.GetFiles(path, filter, MySearchOption.AllDirectories);
		}

		public static IEnumerable<string> GetFiles(string path, string filter, MySearchOption searchOption)
		{
			return m_fileProvider.GetFiles(path, filter, searchOption);
		}

		/// <summary>
		/// Creates a relative path from one file or folder to another.
		/// </summary>
		public static string MakeRelativePath(string fromPath, string toPath)
		{
			if (string.IsNullOrEmpty(fromPath))
			{
				throw new ArgumentNullException("fromPath");
			}
			if (string.IsNullOrEmpty(toPath))
			{
				throw new ArgumentNullException("toPath");
			}
			Uri uri = new Uri(fromPath);
			Uri uri2 = new Uri(toPath);
			if (uri.Scheme != uri2.Scheme)
			{
				return toPath;
			}
			Uri uri3 = uri.MakeRelativeUri(uri2);
			string text = Uri.UnescapeDataString(uri3.ToString());
			if (uri2.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
			{
				text = text.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			}
			return text;
		}

		public static string TerminatePath(string path)
		{
			if (!string.IsNullOrEmpty(path) && path[path.Length - 1] == Path.DirectorySeparatorChar)
			{
				return path;
			}
			return path + Path.DirectorySeparatorChar;
		}

		/// <summary>
		/// Return the trailing directory separator of the provided path if any.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string UnTerminatePath(string path)
		{
			if (path.Length > 1 && path[m_contentPath.Length - 1] == Path.DirectorySeparatorChar)
			{
				return path.Substring(0, path.Length - 1);
			}
			return path;
		}

		/// <summary>
		/// Copy all files and folders from a source directory to the target directory.
		/// </summary>
		/// <param name="source">the source path</param>
		/// <param name="target">the target path</param>        
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

		/// <summary>
		/// Copy all files and folders from a source directory to the target directory, excluding any paths that do not match the provided condition predicate.
		/// </summary>
		/// <param name="source">the source path</param>
		/// <param name="target">the target path</param>        
		public static void CopyAll(string source, string target, Predicate<string> condition)
		{
			EnsureDirectoryExists(target);
			FileInfo[] files = new DirectoryInfo(source).GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				if (condition(fileInfo.FullName))
				{
					fileInfo.CopyTo(Path.Combine(target, fileInfo.Name), overwrite: true);
				}
			}
			DirectoryInfo[] directories = new DirectoryInfo(source).GetDirectories();
			foreach (DirectoryInfo directoryInfo in directories)
			{
				if (condition(directoryInfo.FullName))
				{
					DirectoryInfo directoryInfo2 = Directory.CreateDirectory(Path.Combine(target, directoryInfo.Name));
					CopyAll(directoryInfo.FullName, directoryInfo2.FullName, condition);
				}
			}
		}

		/// <summary>
		/// Ensure that the directory identified by the specified path does exist.
		/// </summary>
		/// <param name="path"></param>
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

		/// <summary>
		/// Checks if the path is directory
		/// </summary>
		/// <param name="path">the path</param>
		/// <returns></returns>
		public static bool IsDirectory(string path)
		{
			if (!DirectoryExists(path))
			{
				return false;
			}
			FileAttributes attributes = File.GetAttributes(path);
			return attributes.HasFlag(FileAttributes.Directory);
		}

		public static void CreateDirectoryRecursive(string path)
		{
			if (!string.IsNullOrEmpty(path) && !DirectoryExists(path))
			{
				string directoryName = Path.GetDirectoryName(path);
				CreateDirectoryRecursive(directoryName);
				Directory.CreateDirectory(path);
			}
		}
	}
}
