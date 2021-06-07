using System.Collections.Generic;

namespace VRage.GameServices
{
	public interface IMyLobby
	{
		ulong LobbyId
		{
			get;
		}

		bool IsValid
		{
			get;
		}

		ulong OwnerId
		{
			get;
		}

		MyLobbyType LobbyType
		{
			get;
			set;
		}

		ConnectionStrategy ConnectionStrategy
		{
			get;
		}

		int MemberCount
		{
			get;
		}

		int MemberLimit
		{
			get;
			set;
		}

		IEnumerable<ulong> MemberList
		{
			get;
		}

		bool IsChatAvailable
		{
			get;
		}

		event KickedDelegate OnKicked;

		event MyLobbyDataUpdated OnDataReceived;

		event MessageReceivedDelegate OnChatReceived;

		event MyLobbyChatUpdated OnChatUpdated;

		void Leave();

		bool RequestData();

		string GetData(string key);

		bool SetData(string key, string value);

		bool DeleteData(string key);

		bool SendChatMessage(string text, byte channel, long targetId = 0L, string customAuthor = null);
	}
}
