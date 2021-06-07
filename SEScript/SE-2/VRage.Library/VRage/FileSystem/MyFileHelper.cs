using System.IO;

namespace VRage.FileSystem
{
	public class MyFileHelper
	{
		public static bool CanWrite(string path)
		{
			if (!File.Exists(path))
			{
				return true;
			}
			try
			{
				using (File.Open(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
		}
	}
}
