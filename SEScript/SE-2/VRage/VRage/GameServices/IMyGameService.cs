using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public interface IMyGameService
	{
		uint AppId
		{
			get;
		}

		bool IsActive
		{
			get;
		}

		bool IsOnline
		{
			get;
		}

		bool IsOverlayEnabled
		{
			get;
		}

		bool OwnsGame
		{
			get;
		}

		ulong UserId
		{
			get;
			set;
		}

		string UserName
		{
			get;
		}

		MyGameServiceUniverse UserUniverse
		{
			get;
		}

		string BranchName
		{
			get;
		}

		string BranchNameFriendly
		{
			get;
		}

		IMyGameServer GameServer
		{
			get;
		}

		ICollection<MyGameInventoryItem> InventoryItems
		{
			get;
		}

		IDictionary<int, MyGameInventoryItemDefinition> Definitions
		{
			get;
		}

		IMyPeer2Peer Peer2Peer
		{
			get;
		}

		bool HasGameServer
		{
			get;
		}

		int RecycleTokens
		{
			get;
		}

		int SampleRate
		{
			get;
		}

		string ServiceName
		{
			get;
		}

		bool ContinueToLobbySupported
		{
			get;
		}

		event EventHandler InventoryRefreshed;

		event EventHandler<MyGameItemsEventArgs> CheckItemDataReady;

		event EventHandler<MyGameItemsEventArgs> ItemsAdded;

		event EventHandler NoItemsRecieved;

		event EventHandler<int> OnLANServerListResponded;

		event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse;

		event EventHandler<int> OnFriendsServerListResponded;

		event EventHandler<MyMatchMakingServerResponse> OnFriendsServersCompleteResponse;

		event EventHandler<int> OnDedicatedServerListResponded;

		event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse;

		event EventHandler<int> OnFavoritesServerListResponded;

		event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse;

		event EventHandler<int> OnHistoryServerListResponded;

		event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse;

		event EventHandler<MyGameServerItem> OnPingServerResponded;

		event EventHandler OnPingServerFailedToRespond;

		event Action<bool> OnOverlayActivated;

		event Action<uint> OnDLCInstalled;

		event Action OnUserChanged;

		event MyLobbyJoinRequested OnJoinLobbyRequested;

		event MyLobbyServerChangeRequested OnServerChangeRequested;

		void OpenOverlayUrl(string url);

		void SetNotificationPosition(NotificationPosition notificationPosition);

		void ShutDown();

		bool IsAppInstalled(uint appId);

		bool IsDlcInstalled(uint dlcId);

		int GetDLCCount();

		bool GetDLCDataByIndex(int index, out uint dlcId, out bool available, out string name, int nameBufferSize);

		void OpenOverlayUser(ulong id);

		void OpenInviteOverlay();

		bool GetAuthSessionTicket(out uint ticketHandle, byte[] buffer, out uint length);

		void PingServer(uint ip, ushort port);

		void LoadStats();

		IMyAchievement GetAchievement(string achievementName, string statName, float maxValue);

		void ResetAllStats(bool achievementsToo);

		void StoreStats();

		void GetServerRules(uint ip, ushort port, ServerRulesResponse completedAction, Action failedAction);

		void GetPlayerDetails(uint ip, ushort port, PlayerDetailsResponse completedAction, Action failedAction);

		string GetPersonaName(ulong userId);

		bool HasFriend(ulong userId);

		string GetClanName(ulong groupId);

		void AddFavoriteGame(uint ip, ushort connPort, ushort queryPort);

		void RemoveFavoriteGame(uint ip, ushort connPort, ushort queryPort);

		void AddHistoryGame(uint ip, ushort connPort, ushort queryPort);

		void ServerUpdate();

		void Update();

		bool IsUserInGroup(ulong groupId);

		bool GetRemoteStorageQuota(out ulong totalBytes, out ulong availableBytes);

		int GetRemoteStorageFileCount();

		string GetRemoteStorageFileNameAndSize(int fileIndex, out int fileSizeInBytes);

		bool IsRemoteStorageFilePersisted(string file);

		bool RemoteStorageFileForget(string file);

		MyGameServerItem GetFavoritesServerDetails(int server);

		void RequestFavoritesServerList(string filterOps);

		void CancelFavoritesServersRequest();

		MyGameServerItem GetDedicatedServerDetails(int server);

		void RequestInternetServerList(string filterOps);

		void CancelInternetServersRequest();

		MyGameServerItem GetHistoryServerDetails(int server);

		void RequestHistoryServerList(string filterOps);

		void CancelHistoryServersRequest();

		MyGameServerItem GetLANServerDetails(int server);

		void RequestLANServerList();

		void CancelLANServersRequest();

		MyGameServerItem GetFriendsServerDetails(int server);

		ulong CreatePublishedFileUpdateRequest(ulong publishedFileId);

		void UpdatePublishedFileTags(ulong updateHandle, string[] tags);

		void UpdatePublishedFileFile(ulong updateHandle, string steamItemFileName);

		void UpdatePublishedFilePreviewFile(ulong updateHandle, string steamPreviewFileName);

		void FileDelete(string steamItemFileName);

		bool FileExists(string fileName);

		int GetFileSize(string fileName);

		ulong FileWriteStreamOpen(string fileName);

		void FileWriteStreamWriteChunk(ulong handle, byte[] buffer, int size);

		void FileWriteStreamClose(ulong handle);

		void GetAllInventoryItems();

		MyGameInventoryItemDefinition GetInventoryItemDefinition(string assetModifierId);

		IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(MyGameInventoryItemSlot slot);

		bool HasInventoryItemWithDefinitionId(int id);

		bool HasInventoryItem(ulong id);

		bool HasInventoryItem(string assetModifierId);

		void TriggerPersonalContainer();

		void TriggerCompetitiveContainer();

		void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction);

		List<MyGameInventoryItem> CheckItemData(byte[] checkData, out bool checkResult);

		void GetItemsCheckData(List<MyGameInventoryItem> items, Action<byte[]> completedAction);

		void ConsumeItem(MyGameInventoryItem item);

		void JoinLobby(ulong lobbyId, MyJoinResponseDelegate responseDelegate);

		IMyLobby CreateLobby(ulong lobbyId);

		void AddPublicLobbies(List<IMyLobby> lobbyList);

		void AddFriendLobbies(List<IMyLobby> lobbyList);

		void RequestLobbyList(Action<bool> completed);

		void CreateLobby(MyLobbyType type, uint maxPlayers, MyLobbyCreated createdResponse);

		void AddLobbyFilter(string key, string value);

		int GetChatMaxMessageSize();

		MyGameServiceAccountType GetServerAccountType(ulong steamId);

		void CommitPublishedFileUpdate(ulong updateHandle, Action<bool, MyRemoteStorageUpdatePublishedFileResult> onCallResult);

		void PublishWorkshopFile(string file, string previewFile, string title, string description, string longDescription, MyPublishedFileVisibility visibility, string[] tags, Action<bool, MyRemoteStoragePublishFileResult> onCallResult);

		void SubscribePublishedFile(ulong publishedFileId, Action<bool, MyRemoteStorageSubscribePublishedFileResult> onCallResult);

		void FileShare(string file, Action<bool, MyRemoteStorageFileShareResult> onCallResult);

		void SetServerModTemporaryDirectory();

		void InitializeVoiceRecording();

		void DisposeVoiceRecording();

		void StartVoiceRecording();

		void StopVoiceRecording();

		MyVoiceResult DecompressVoice(byte[] compressedBuffer, uint size, byte[] uncompressedBuffer, out uint writtenBytes);

		MyVoiceResult GetAvailableVoice(out uint size);

		MyVoiceResult GetVoice(byte[] buffer, out uint bytesWritten);

		MyPlayerVoiceChatState GetPlayerVoiceChatState(ulong playerId);

		void SetPlayerMuted(ulong playerId, bool muted);

		bool RecycleItem(MyGameInventoryItem item);

		bool CraftSkin(MyGameInventoryItemQuality quality);

		uint GetCraftingCost(MyGameInventoryItemQuality quality);

		uint GetRecyclingReward(MyGameInventoryItemQuality quality);

		int GetFriendsCount();

		ulong GetFriendIdByIndex(int index);

		string GetFriendNameByIndex(int index);

		bool SaveToCloudAsync(string fileName, byte[] buffer, Action<bool> completedAction);

		bool SaveToCloud(string fileName, byte[] buffer);

		bool LoadFromCloudAsync(string fileName, Action<bool> completedAction);

		byte[] GetFileBufferFromCloud(string fileName);

		List<MyCloudFileInfo> GetCloudFiles(string directoryFilter);

		byte[] LoadFromCloud(string fileName);

		bool DeleteFromCloud(string fileName);

		bool IsProductOwned(uint productId, out DateTime? purchaseTime);

		void RequestEncryptedAppTicket(string url, Action<string> onDone);

		void AddUnownedItems();

		void RequestPermissions(Permissions permission, bool attemptResolution, Action<bool> onDone);

		void OnThreadpoolInitialized();
	}
}
