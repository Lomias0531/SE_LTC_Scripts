using System;
using System.Linq;

namespace VRage.GameServices
{
	public class MyModMetadata
	{
		/// <summary>
		/// Version of the mod
		/// </summary>
		public Version ModVersion;

		/// <summary>
		/// Minimum supported version of the game
		/// </summary>
		public Version MinGameVersion;

		/// <summary>
		/// Maximum supported version of the game
		/// </summary>
		public Version MaxGameVersion;

		public override string ToString()
		{
			return string.Format("ModVersion: {0}, MinGameVersion: {1}, MaxGameVersion: {2}", (ModVersion != null) ? ModVersion.ToString() : "N/A", (MinGameVersion != null) ? MinGameVersion.ToString() : "N/A", (MaxGameVersion != null) ? MaxGameVersion.ToString() : "N/A");
		}

		public static implicit operator MyModMetadata(ModMetadataFile file)
		{
			if (file == null)
			{
				return null;
			}
			MyModMetadata myModMetadata = new MyModMetadata();
			Version.TryParse(file.ModVersion, out myModMetadata.ModVersion);
			if (file.MinGameVersion != null)
			{
				string[] source = file.MinGameVersion.Split(new char[1]
				{
					'.'
				});
				file.MinGameVersion = string.Join(".", source.Take(3));
				Version.TryParse(file.MinGameVersion, out myModMetadata.MinGameVersion);
			}
			else
			{
				myModMetadata.MinGameVersion = null;
			}
			if (file.MaxGameVersion != null)
			{
				string[] source2 = file.MaxGameVersion.Split(new char[1]
				{
					'.'
				});
				file.MaxGameVersion = string.Join(".", source2.Take(3));
				Version.TryParse(file.MaxGameVersion, out myModMetadata.MaxGameVersion);
			}
			else
			{
				myModMetadata.MaxGameVersion = null;
			}
			return myModMetadata;
		}

		public static implicit operator ModMetadataFile(MyModMetadata metadata)
		{
			if (metadata == null)
			{
				return null;
			}
			ModMetadataFile modMetadataFile = new ModMetadataFile();
			if (metadata.ModVersion != null)
			{
				modMetadataFile.ModVersion = metadata.ModVersion.ToString();
			}
			if (metadata.MinGameVersion != null)
			{
				modMetadataFile.MinGameVersion = metadata.MinGameVersion.ToString();
			}
			if (metadata.MaxGameVersion != null)
			{
				modMetadataFile.MaxGameVersion = metadata.MaxGameVersion.ToString();
			}
			return modMetadataFile;
		}
	}
}
