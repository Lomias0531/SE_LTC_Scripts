using System.Collections.Generic;
using System.Net;

namespace VRage.GameServices
{
	public class MyGameServerItem : IMyMultiplayerGame
	{
		public uint AppID
		{
			get;
			set;
		}

		public int BotPlayers
		{
			get;
			set;
		}

		public bool DoNotRefresh
		{
			get;
			set;
		}

		public bool Experimental
		{
			get;
			set;
		}

		public string GameDescription
		{
			get;
			set;
		}

		public string GameDir
		{
			get;
			set;
		}

		public List<string> GameTagList
		{
			get;
			set;
		}

		public string GameTags
		{
			get;
			set;
		}

		public bool HadSuccessfulResponse
		{
			get;
			set;
		}

		public string Map
		{
			get;
			set;
		}

		public int MaxPlayers
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public IPEndPoint NetAdr
		{
			get;
			set;
		}

		public bool Password
		{
			get;
			set;
		}

		public int Ping
		{
			get;
			set;
		}

		public int Players
		{
			get;
			set;
		}

		public bool Secure
		{
			get;
			set;
		}

		public int ServerVersion
		{
			get;
			set;
		}

		public ulong GameID => SteamID;

		public ulong SteamID
		{
			get;
			set;
		}

		public uint TimeLastPlayed
		{
			get;
			set;
		}

		public bool IsRanked
		{
			get;
			set;
		}

		public MyGameServerItem()
		{
			GameTagList = new List<string>();
		}

		public string GetGameTagByPrefix(string prefix)
		{
			foreach (string gameTag in GameTagList)
			{
				if (gameTag.StartsWith(prefix))
				{
					return gameTag.Substring(prefix.Length, gameTag.Length - prefix.Length);
				}
			}
			return string.Empty;
		}

		public ulong GetGameTagByPrefixUlong(string prefix)
		{
			string gameTagByPrefix = GetGameTagByPrefix(prefix);
			if (string.IsNullOrEmpty(gameTagByPrefix))
			{
				return 0uL;
			}
			ulong.TryParse(gameTagByPrefix, out ulong result);
			return result;
		}
	}
}
