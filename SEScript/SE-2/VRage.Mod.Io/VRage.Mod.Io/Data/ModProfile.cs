using System;

namespace VRage.Mod.Io.Data
{
	[Serializable]
	internal class ModProfile
	{
		public int id;

		public int game_id;

		public int status;

		public int visible;

		public UserProfile submitted_by;

		public int date_added;

		public int date_updated;

		public int date_live;

		public int maturity_option;

		public LogoImageLocator logo;

		public string homepage_url;

		public string name;

		public string name_id;

		public string summary;

		public string description;

		public string description_plaintext;

		public string metadata_blob;

		public string profile_url;

		public Modfile modfile;

		public ModMediaCollection media;

		public MetadataKVP[] metadata_kvp;

		public ModTag[] tags;

		public Statistics stats;
	}
}
