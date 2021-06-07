using System.IO;
using System.Linq;

namespace System
{
	public static class SteamHelpers
	{
		public static bool IsSteamPath(string path)
		{
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				return directoryInfo.Parent != null && directoryInfo.Parent.Parent != null && directoryInfo.Parent.Name.Equals("Common", StringComparison.InvariantCultureIgnoreCase) && directoryInfo.Parent.Parent.Name.Equals("SteamApps", StringComparison.InvariantCultureIgnoreCase);
			}
			catch
			{
				return false;
			}
		}

		public static bool IsAppManifestPresent(string path, uint appId)
		{
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				return IsSteamPath(path) && Directory.GetFiles(directoryInfo.Parent.Parent.FullName).Contains("AppManifest_" + appId + ".acf", StringComparer.InvariantCultureIgnoreCase);
			}
			catch
			{
				return false;
			}
		}
	}
}
