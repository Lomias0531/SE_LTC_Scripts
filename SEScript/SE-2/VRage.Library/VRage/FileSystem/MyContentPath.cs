using System.IO;

namespace VRage.FileSystem
{
	public struct MyContentPath
	{
		private const string DEFAULT = "";

		private string m_absolutePath;

		private string m_rootFolder;

		private string m_alternatePath;

		/// <summary>
		/// Relative path within the Mod/Content structure.
		/// Do not set directly. Public only for Object buidler purposes.
		/// </summary>
		public string Path;

		/// <summary>
		/// Name of the Mod.
		/// Do not set directly. Public only for Object buidler purposes.
		/// </summary>
		public string ModFolder;

		/// <summary>
		/// Returns respective absolute path.
		/// </summary>
		public string Absolute => m_absolutePath;

		/// <summary>
		/// Returns respective absolute path to root (Content/ModsFolder).
		/// </summary>
		public string RootFolder => m_rootFolder;

		/// <summary>
		/// Possible Content alternative of modded files.
		/// </summary>
		public string AlternatePath => m_alternatePath;

		public bool AbsoluteFileExists
		{
			get
			{
				if (m_absolutePath != null)
				{
					return MyFileSystem.FileExists(Absolute);
				}
				return false;
			}
		}

		public bool AbsoluteDirExists
		{
			get
			{
				if (m_absolutePath != null)
				{
					return MyFileSystem.DirectoryExists(Absolute);
				}
				return false;
			}
		}

		public bool AlternateFileExists
		{
			get
			{
				if (m_alternatePath != null)
				{
					return MyFileSystem.FileExists(AlternatePath);
				}
				return false;
			}
		}

		public bool AlternateDirExists
		{
			get
			{
				if (m_alternatePath != null)
				{
					return MyFileSystem.DirectoryExists(AlternatePath);
				}
				return false;
			}
		}

		public MyContentPath(string path = null, string possibleModPath = null)
		{
			Path = "";
			ModFolder = "";
			m_absolutePath = "";
			m_rootFolder = "";
			m_alternatePath = "";
			SetPath(path, possibleModPath);
		}

		/// <summary>
		/// Helper method.
		/// </summary>
		/// <returns>Valid file path to existing file.</returns>
		public string GetExitingFilePath()
		{
			if (AbsoluteFileExists)
			{
				return Absolute;
			}
			if (AlternateFileExists)
			{
				return AlternatePath;
			}
			return "";
		}

		/// <summary>
		/// Use this method to get the ModFolder automaticaly adjusted.
		/// </summary>
		/// <param name="path">Absolute or relative path within the content structure (Content/Mods).</param>
		public void SetPath(string path, string possibleModPath = null)
		{
			Path = path;
			ModFolder = "";
			m_absolutePath = "";
			m_rootFolder = "";
			m_alternatePath = "";
			if (!string.IsNullOrEmpty(path) && !System.IO.Path.IsPathRooted(path))
			{
				string text = "";
				string path2 = System.IO.Path.Combine(MyFileSystem.ContentPath, path);
				text = ((possibleModPath == null) ? System.IO.Path.Combine(MyFileSystem.ModsPath, path) : System.IO.Path.Combine(MyFileSystem.ModsPath, possibleModPath, path));
				if (MyFileSystem.FileExists(text))
				{
					Path = text;
					path = Path;
				}
				else if (MyFileSystem.FileExists(path2))
				{
					Path = path2;
				}
				else if (MyFileSystem.DirectoryExists(text))
				{
					Path = text;
				}
				else if (MyFileSystem.DirectoryExists(path2))
				{
					Path = path2;
				}
				else
				{
					Path = "";
				}
			}
			if (string.IsNullOrEmpty(Path))
			{
				return;
			}
			if (Path.StartsWith(MyFileSystem.ContentPath))
			{
				Path = ((MyFileSystem.ContentPath.Length == Path.Length) ? "" : Path.Remove(0, MyFileSystem.ContentPath.Length + 1));
			}
			else if (Path.StartsWith(MyFileSystem.ModsPath))
			{
				Path = Path.Remove(0, MyFileSystem.ModsPath.Length + 1);
				int num = Path.IndexOf('\\');
				if (num == -1)
				{
					ModFolder = Path;
					Path = "";
					SetupHelperPaths();
					return;
				}
				ModFolder = Path.Substring(0, num);
				Path = Path.Remove(0, num + 1);
			}
			else
			{
				Path = path;
			}
			SetupHelperPaths();
		}

		private void SetupHelperPaths()
		{
			m_absolutePath = (string.IsNullOrEmpty(ModFolder) ? System.IO.Path.Combine(MyFileSystem.ContentPath, Path) : System.IO.Path.Combine(MyFileSystem.ModsPath, ModFolder, Path));
			m_rootFolder = (string.IsNullOrEmpty(ModFolder) ? MyFileSystem.ContentPath : System.IO.Path.Combine(MyFileSystem.ModsPath, ModFolder));
			m_alternatePath = (string.IsNullOrEmpty(ModFolder) ? "" : System.IO.Path.Combine(MyFileSystem.ContentPath, Path));
		}

		/// <summary>
		/// Assignment operator.
		/// </summary>
		/// <param name="path">Absolute path to content</param>
		/// <returns></returns>
		public static implicit operator MyContentPath(string path)
		{
			return new MyContentPath(path);
		}
	}
}
