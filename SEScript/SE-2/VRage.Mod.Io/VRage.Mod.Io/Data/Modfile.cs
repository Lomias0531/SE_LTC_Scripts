using System;

namespace VRage.Mod.Io.Data
{
	[Serializable]
	internal class Modfile
	{
		public const int NULL_ID = 0;

		public int id;

		public int mod_id;

		public int date_added;

		public string filename;

		public long filesize;

		public FileHash filehash;

		public string version;

		public string changelog;

		public string metadata_blob;

		public int date_scanned;

		public int virus_status;

		public int virus_positive;

		public string virustotal_hash;

		public ModfileLocator download;
	}
}
