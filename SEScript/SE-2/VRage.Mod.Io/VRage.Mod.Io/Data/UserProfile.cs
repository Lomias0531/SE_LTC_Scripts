using System;

namespace VRage.Mod.Io.Data
{
	[Serializable]
	internal class UserProfile
	{
		public int id;

		public string name_id;

		public string username;

		public AvatarImageLocator avatar;

		public int date_online;

		public string timezone;

		public string language;

		public string profile_url;
	}
}
