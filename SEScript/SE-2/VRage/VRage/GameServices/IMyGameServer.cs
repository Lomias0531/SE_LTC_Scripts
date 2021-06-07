using System;
using System.Net;
using VRage.Network;

namespace VRage.GameServices
{
	public interface IMyGameServer
	{
		string GameDescription
		{
			get;
			set;
		}

		string ProductName
		{
			get;
			set;
		}

		ulong ServerId
		{
			get;
		}

		bool Running
		{
			get;
		}

		event Action PlatformConnected;

		event Action<string> PlatformDisconnected;

		event Action<string> PlatformConnectionFailed;

		event Action<ulong, JoinResult, ulong> ValidateAuthTicketResponse;

		event Action<ulong, ulong, bool, bool> UserGroupStatusResponse;

		event Action<sbyte> PolicyResponse;

		bool Start(IPEndPoint serverEndpoint, ushort steamPort, string versionString);

		uint GetPublicIP();

		void Update();

		void SetKeyValue(string key, string value);

		void ClearAllKeyValues();

		void SetGameTags(string tags);

		void SetGameData(string data);

		void SetModDir(string directory);

		void SetDedicated(bool isDedicated);

		void SetMapName(string mapName);

		void SetServerName(string serverName);

		void SetMaxPlayerCount(int count);

		void SetBotPlayerCount(int count);

		void SetPasswordProtected(bool passwdProtected);

		void LogOnAnonymous();

		void LogOff();

		void Shutdown();

		bool BeginAuthSession(ulong userId, byte[] token);

		void EndAuthSession(ulong userId);

		void SendUserDisconnect(ulong userId);

		void EnableHeartbeats(bool enable);

		void BrowserUpdateUserData(ulong userId, string playerName, int score);

		bool RequestGroupStatus(ulong userId, ulong groupId);

		bool UserHasLicenseForApp(ulong steamId, uint appId);
	}
}
