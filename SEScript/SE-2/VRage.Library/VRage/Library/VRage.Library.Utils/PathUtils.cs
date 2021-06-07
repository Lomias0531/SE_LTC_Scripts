using System.Collections.Generic;
using System.IO;

namespace VRage.Library.Utils
{
	public static class PathUtils
	{
		public static string[] GetFilesRecursively(string path, string searchPath)
		{
			List<string> list = new List<string>();
			GetfGetFilesRecursively(path, searchPath, list);
			return list.ToArray();
		}

		public static void GetfGetFilesRecursively(string path, string searchPath, List<string> paths)
		{
			paths.AddRange(Directory.GetFiles(path, searchPath));
			string[] directories = Directory.GetDirectories(path);
			foreach (string path2 in directories)
			{
				GetfGetFilesRecursively(path2, searchPath, paths);
			}
		}
	}
}
