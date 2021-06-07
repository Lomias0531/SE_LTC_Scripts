using Sandbox.Engine.Platform;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.GameServices;

namespace Sandbox.Engine.Networking
{
	public static class MyGameService
	{
		private class MyNullAchievement : IMyAchievement
		{
			public bool IsUnlocked => false;

			public int StatValueInt
			{
				get;
				set;
			}

			public float StatValueFloat
			{
				get;
				set;
			}

			public int StatValueConditionBitField
			{
				get;
				set;
			}

			public event Action OnStatValueChanged;

			public void Unlock()
			{
			}

			public void IndicateProgress(uint value)
			{
			}
		}

		private static IMyGameService m_gameServiceCache;

		private static IMyUGCService m_ugcServiceCache;

		private static MyNullPeer2Peer m_nullPeer2Peer;

		public static IMyGameService Service
		{
			get
			{
				EnsureGameService();
				return m_gameServiceCache;
			}
		}

		public static IMyGameServer GameServer
		{
			get
			{
				if (!EnsureGameService())
				{
					return null;
				}
				return m_gameServiceCache.GameServer;
			}
		}

		public static IMyPeer2Peer Peer2Peer
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.Peer2Peer;
				}
				return m_nullPeer2Peer ?? (m_nullPeer2Peer = new MyNullPeer2Peer());
			}
		}

		public static uint AppId
		{
			get
			{
				if (!EnsureGameService())
				{
					return 0u;
				}
				return m_gameServiceCache.AppId;
			}
		}

		public static bool IsActive
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.IsActive;
				}
				return false;
			}
		}

		public static bool IsOnline
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.IsOnline;
				}
				return false;
			}
		}

		public static bool IsOverlayEnabled
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.IsOverlayEnabled;
				}
				return false;
			}
		}

		public static bool OwnsGame
		{
			get
			{
				if (EnsureGameService() && IsActive)
				{
					return m_gameServiceCache.OwnsGame;
				}
				return false;
			}
		}

		public static ulong UserId
		{
			get
			{
				if (!EnsureGameService())
				{
					return ulong.MaxValue;
				}
				return m_gameServiceCache.UserId;
			}
			set
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.UserId = value;
				}
			}
		}

		public static string UserName
		{
			get
			{
				string text = null;
				if (EnsureGameService())
				{
					text = m_gameServiceCache.UserName;
				}
				return text ?? string.Empty;
			}
		}

		public static MyGameServiceUniverse UserUniverse
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.UserUniverse;
				}
				return MyGameServiceUniverse.Invalid;
			}
		}

		public static string BranchName
		{
			get
			{
				if (Sandbox.Engine.Platform.Game.IsDedicated)
				{
					return "DedicatedServer";
				}
				if (!IsActive)
				{
					return "SVN";
				}
				if (!EnsureGameService() || string.IsNullOrEmpty(m_gameServiceCache.BranchName))
				{
					return "";
				}
				return m_gameServiceCache.BranchName;
			}
		}

		public static int RecycleTokens
		{
			get
			{
				if (EnsureGameService())
				{
					return m_gameServiceCache.RecycleTokens;
				}
				return 0;
			}
		}

		public static string BranchNameFriendly
		{
			get
			{
				if (string.IsNullOrEmpty(BranchName))
				{
					return "default";
				}
				return BranchName;
			}
		}

		public static ICollection<MyGameInventoryItem> InventoryItems
		{
			get
			{
				if (!EnsureGameService())
				{
					return null;
				}
				return m_gameServiceCache.InventoryItems;
			}
		}

		public static IDictionary<int, MyGameInventoryItemDefinition> Definitions
		{
			get
			{
				if (!EnsureGameService())
				{
					return null;
				}
				return m_gameServiceCache.Definitions;
			}
		}

		public static bool HasGameServer
		{
			get
			{
				if (!EnsureGameService())
				{
					return false;
				}
				return m_gameServiceCache.HasGameServer;
			}
		}

		public static IMyUGCService WorkshopService
		{
			get
			{
				EnsureUGCService();
				return m_ugcServiceCache;
			}
		}

		public static event MyLobbyJoinRequested LobbyJoinRequested
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnJoinLobbyRequested += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnJoinLobbyRequested -= value;
				}
			}
		}

		public static event MyLobbyServerChangeRequested ServerChangeRequested
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnServerChangeRequested += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnServerChangeRequested -= value;
				}
			}
		}

		public static event EventHandler<MyGameServerItem> OnPingServerResponded
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnPingServerResponded += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnPingServerResponded -= value;
				}
			}
		}

		public static event EventHandler OnPingServerFailedToRespond
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnPingServerFailedToRespond += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnPingServerFailedToRespond -= value;
				}
			}
		}

		public static event EventHandler<int> OnFavoritesServerListResponded
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnFavoritesServerListResponded += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnFavoritesServerListResponded -= value;
				}
			}
		}

		public static event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnFavoritesServersCompleteResponse += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnFavoritesServersCompleteResponse -= value;
				}
			}
		}

		public static event EventHandler<int> OnHistoryServerListResponded
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnHistoryServerListResponded += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnHistoryServerListResponded -= value;
				}
			}
		}

		public static event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnHistoryServersCompleteResponse += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnHistoryServersCompleteResponse -= value;
				}
			}
		}

		public static event EventHandler<int> OnLANServerListResponded
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnLANServerListResponded += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnLANServerListResponded -= value;
				}
			}
		}

		public static event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnLANServersCompleteResponse += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnLANServersCompleteResponse -= value;
				}
			}
		}

		public static event EventHandler<int> OnFriendsServerListResponded
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnFriendsServerListResponded += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnFriendsServerListResponded -= value;
				}
			}
		}

		public static event EventHandler<MyMatchMakingServerResponse> OnFriendsServersCompleteResponse
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnFriendsServersCompleteResponse += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnFriendsServersCompleteResponse -= value;
				}
			}
		}

		public static event EventHandler<int> OnDedicatedServerListResponded
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnDedicatedServerListResponded += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnDedicatedServerListResponded -= value;
				}
			}
		}

		public static event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnDedicatedServersCompleteResponse += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnDedicatedServersCompleteResponse -= value;
				}
			}
		}

		public static event EventHandler InventoryRefreshed
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.InventoryRefreshed += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.InventoryRefreshed -= value;
				}
			}
		}

		public static event EventHandler<MyGameItemsEventArgs> CheckItemDataReady
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.CheckItemDataReady += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.CheckItemDataReady -= value;
				}
			}
		}

		public static event EventHandler<MyGameItemsEventArgs> ItemsAdded
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.ItemsAdded += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.ItemsAdded -= value;
				}
			}
		}

		public static event EventHandler NoItemsRecieved
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.NoItemsRecieved += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.NoItemsRecieved -= value;
				}
			}
		}

		public static event Action<bool> OnOverlayActivated
		{
			add
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnOverlayActivated += value;
				}
			}
			remove
			{
				if (EnsureGameService())
				{
					m_gameServiceCache.OnOverlayActivated -= value;
				}
			}
		}

		public static event Action<uint> OnDLCInstalled
		{
			add
			{
				if (EnsureGameService() && m_gameServiceCache.IsActive)
				{
					m_gameServiceCache.OnDLCInstalled += value;
				}
			}
			remove
			{
				if (EnsureGameService() && m_gameServiceCache.IsActive)
				{
					m_gameServiceCache.OnDLCInstalled -= value;
				}
			}
		}

		public static event Action OnUserChanged
		{
			add
			{
				if (EnsureGameService() && m_gameServiceCache.IsActive)
				{
					m_gameServiceCache.OnUserChanged += value;
				}
			}
			remove
			{
				if (EnsureGameService() && m_gameServiceCache.IsActive)
				{
					m_gameServiceCache.OnUserChanged -= value;
				}
			}
		}

		public static void ClearCache()
		{
			m_gameServiceCache = null;
		}

		private static bool EnsureGameService()
		{
			if (m_gameServiceCache == null)
			{
				m_gameServiceCache = MyServiceManager.Instance.GetService<IMyGameService>();
			}
			return m_gameServiceCache != null;
		}

		private static bool EnsureUGCService()
		{
			if (m_ugcServiceCache == null)
			{
				m_ugcServiceCache = MyServiceManager.Instance.GetService<IMyUGCService>();
			}
			return m_ugcServiceCache != null;
		}

		public static IEnumerable<MyGameInventoryItemDefinition> GetDefinitionsForSlot(MyGameInventoryItemSlot slot)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetDefinitionsForSlot(slot);
		}

		public static void OpenOverlayUrl(string url)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.OpenOverlayUrl(url);
			}
		}

		public static void OpenInviteOverlay()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.OpenInviteOverlay();
			}
		}

		internal static void SetNotificationPosition(NotificationPosition notificationPosition)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.SetNotificationPosition(notificationPosition);
			}
		}

		public static void ShutDown()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.ShutDown();
			}
		}

		public static IMyAchievement GetAchievement(string achievementName, string statName, float statMaxValue)
		{
			if (!EnsureGameService())
			{
				return new MyNullAchievement();
			}
			return m_gameServiceCache.GetAchievement(achievementName, statName, statMaxValue);
		}

		public static bool IsAppInstalled(uint appId)
		{
			if (!EnsureGameService() || !m_gameServiceCache.IsActive)
			{
				return false;
			}
			return m_gameServiceCache.IsAppInstalled(appId);
		}

		public static bool IsDlcInstalled(uint dlcId)
		{
			if (!EnsureGameService() || !m_gameServiceCache.IsActive)
			{
				return false;
			}
			return m_gameServiceCache.IsDlcInstalled(dlcId);
		}

		public static int GetDLCCount()
		{
			if (!EnsureGameService() || !m_gameServiceCache.IsActive)
			{
				return 0;
			}
			return m_gameServiceCache.GetDLCCount();
		}

		public static bool GetDLCDataByIndex(int index, out uint dlcId, out bool available, out string name, int nameBufferSize)
		{
			if (!EnsureGameService())
			{
				dlcId = 0u;
				available = false;
				name = null;
				return false;
			}
			return m_gameServiceCache.GetDLCDataByIndex(index, out dlcId, out available, out name, nameBufferSize);
		}

		public static void OpenOverlayUser(ulong id)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.OpenOverlayUser(id);
			}
		}

		public static bool GetAuthSessionTicket(out uint ticketHandle, byte[] buffer, out uint length)
		{
			length = 0u;
			ticketHandle = 0u;
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.GetAuthSessionTicket(out ticketHandle, buffer, out length);
		}

		public static void PingServer(uint ip, ushort port)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.PingServer(ip, port);
			}
		}

		public static void OnThreadpoolInitialized()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.OnThreadpoolInitialized();
			}
		}

		public static void LoadStats()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.LoadStats();
			}
		}

		public static void ResetAllStats(bool achievementsToo)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.ResetAllStats(achievementsToo);
			}
		}

		public static void StoreStats()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.StoreStats();
			}
		}

		public static void GetServerRules(uint ip, ushort port, ServerRulesResponse completedAction, Action failedAction)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.GetServerRules(ip, port, completedAction, failedAction);
			}
		}

		public static string GetPersonaName(ulong userId)
		{
			if (!EnsureGameService())
			{
				return string.Empty;
			}
			return m_gameServiceCache.GetPersonaName(userId);
		}

		public static bool HasFriend(ulong userId)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.HasFriend(userId);
		}

		public static string GetClanName(ulong groupId)
		{
			if (!EnsureGameService())
			{
				return string.Empty;
			}
			return m_gameServiceCache.GetClanName(groupId);
		}

		public static void GetPlayerDetails(uint ip, ushort port, PlayerDetailsResponse completedAction, Action failedAction)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.GetPlayerDetails(ip, port, completedAction, failedAction);
			}
		}

		public static void AddFavoriteGame(uint ip, ushort connPort, ushort queryPort)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.AddFavoriteGame(ip, connPort, queryPort);
			}
		}

		public static void RemoveFavoriteGame(uint ip, ushort connPort, ushort queryPort)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.RemoveFavoriteGame(ip, connPort, queryPort);
			}
		}

		public static void AddHistoryGame(uint ip, ushort connPort, ushort queryPort)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.AddHistoryGame(ip, connPort, queryPort);
			}
		}

		public static void ServerUpdate()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.ServerUpdate();
			}
		}

		public static void Update()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.Update();
			}
			if (EnsureUGCService())
			{
				m_ugcServiceCache.Update();
			}
		}

		public static bool IsUserInGroup(ulong groupId)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.IsUserInGroup(groupId);
		}

		public static bool GetRemoteStorageQuota(out ulong totalBytes, out ulong availableBytes)
		{
			totalBytes = (availableBytes = 0uL);
			if (!EnsureGameService())
			{
				return true;
			}
			return m_gameServiceCache.GetRemoteStorageQuota(out totalBytes, out availableBytes);
		}

		public static int GetRemoteStorageFileCount()
		{
			if (!EnsureGameService())
			{
				return 0;
			}
			return m_gameServiceCache.GetRemoteStorageFileCount();
		}

		public static string GetRemoteStorageFileNameAndSize(int fileIndex, out int fileSizeInBytes)
		{
			fileSizeInBytes = 0;
			if (!EnsureGameService())
			{
				return string.Empty;
			}
			return m_gameServiceCache.GetRemoteStorageFileNameAndSize(fileIndex, out fileSizeInBytes);
		}

		public static bool IsRemoteStorageFilePersisted(string file)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.IsRemoteStorageFilePersisted(file);
		}

		public static bool RemoteStorageFileForget(string file)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.RemoteStorageFileForget(file);
		}

		internal static void RequestFavoritesServerList(string filterOps)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.RequestFavoritesServerList(filterOps);
			}
		}

		internal static void CancelFavoritesServersRequest()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.CancelFavoritesServersRequest();
			}
		}

		internal static MyGameServerItem GetFavoritesServerDetails(int server)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetFavoritesServerDetails(server);
		}

		internal static void RequestInternetServerList(string filterOps)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.RequestInternetServerList(filterOps);
			}
		}

		internal static void CancelInternetServersRequest()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.CancelInternetServersRequest();
			}
		}

		internal static MyGameServerItem GetDedicatedServerDetails(int server)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetDedicatedServerDetails(server);
		}

		internal static void RequestHistoryServerList(string filterOps)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.RequestHistoryServerList(filterOps);
			}
		}

		internal static MyGameServerItem GetHistoryServerDetails(int server)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetHistoryServerDetails(server);
		}

		internal static void CancelHistoryServersRequest()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.CancelHistoryServersRequest();
			}
		}

		public static void RequestLANServerList()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.RequestLANServerList();
			}
		}

		public static MyGameServerItem GetLANServerDetails(int server)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetLANServerDetails(server);
		}

		public static void CancelLANServersRequest()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.CancelLANServersRequest();
			}
		}

		internal static MyGameServerItem GetFriendsServerDetails(int server)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetFriendsServerDetails(server);
		}

		internal static ulong CreatePublishedFileUpdateRequest(ulong publishedFileId)
		{
			if (!EnsureGameService())
			{
				return 0uL;
			}
			return m_gameServiceCache.CreatePublishedFileUpdateRequest(publishedFileId);
		}

		internal static void UpdatePublishedFileTags(ulong updateHandle, string[] tags)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.UpdatePublishedFileTags(updateHandle, tags);
			}
		}

		internal static void UpdatePublishedFileFile(ulong updateHandle, string steamItemFileName)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.UpdatePublishedFileFile(updateHandle, steamItemFileName);
			}
		}

		internal static void UpdatePublishedFilePreviewFile(ulong updateHandle, string steamPreviewFileName)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.UpdatePublishedFilePreviewFile(updateHandle, steamPreviewFileName);
			}
		}

		internal static void CommitPublishedFileUpdate(ulong updateHandle, Action<bool, MyRemoteStorageUpdatePublishedFileResult> onCallResult)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.CommitPublishedFileUpdate(updateHandle, onCallResult);
			}
		}

		internal static void FileDelete(string steamItemFileName)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.FileDelete(steamItemFileName);
			}
		}

		internal static void PublishWorkshopFile(string file, string previewFile, string title, string description, string longDescription, MyPublishedFileVisibility visibility, string[] tags, Action<bool, MyRemoteStoragePublishFileResult> onCallResult)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.PublishWorkshopFile(file, previewFile, title, description, longDescription, visibility, tags, onCallResult);
			}
		}

		internal static void SubscribePublishedFile(ulong publishedFileId, Action<bool, MyRemoteStorageSubscribePublishedFileResult> onCallResult)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.SubscribePublishedFile(publishedFileId, onCallResult);
			}
		}

		internal static bool FileExists(string fileName)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.FileExists(fileName);
		}

		internal static int GetFileSize(string fileName)
		{
			if (!EnsureGameService())
			{
				return 0;
			}
			return m_gameServiceCache.GetFileSize(fileName);
		}

		internal static ulong FileWriteStreamOpen(string fileName)
		{
			if (!EnsureGameService())
			{
				return 0uL;
			}
			return m_gameServiceCache.FileWriteStreamOpen(fileName);
		}

		internal static void FileWriteStreamWriteChunk(ulong handle, byte[] buffer, int size)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.FileWriteStreamWriteChunk(handle, buffer, size);
			}
		}

		internal static void FileWriteStreamClose(ulong handle)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.FileWriteStreamClose(handle);
			}
		}

		internal static void FileShare(string file, Action<bool, MyRemoteStorageFileShareResult> onCallResult)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.FileShare(file, onCallResult);
			}
		}

		internal static void GetAllInventoryItems()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.GetAllInventoryItems();
			}
		}

		internal static MyGameInventoryItemDefinition GetInventoryItemDefinition(string assetModifierId)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetInventoryItemDefinition(assetModifierId);
		}

		internal static bool HasInventoryItemWithDefinitionId(int id)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.HasInventoryItemWithDefinitionId(id);
		}

		internal static bool HasInventoryItem(ulong id)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.HasInventoryItem(id);
		}

		internal static bool HasInventoryItem(string assetModifierId)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.HasInventoryItem(assetModifierId);
		}

		internal static void TriggerPersonalContainer()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.TriggerPersonalContainer();
			}
		}

		internal static void TriggerCompetitiveContainer()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.TriggerCompetitiveContainer();
			}
		}

		internal static void GetItemCheckData(MyGameInventoryItem item, Action<byte[]> completedAction)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.GetItemCheckData(item, completedAction);
			}
		}

		internal static void GetItemsCheckData(List<MyGameInventoryItem> items, Action<byte[]> completedAction)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.GetItemsCheckData(items, completedAction);
			}
		}

		internal static List<MyGameInventoryItem> CheckItemData(byte[] checkData, out bool checkResult)
		{
			if (!EnsureGameService())
			{
				checkResult = false;
				return null;
			}
			return m_gameServiceCache.CheckItemData(checkData, out checkResult);
		}

		internal static void ConsumeItem(MyGameInventoryItem item)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.ConsumeItem(item);
			}
		}

		internal static void JoinLobby(ulong lobbyId, MyJoinResponseDelegate reponseDelegate)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.JoinLobby(lobbyId, reponseDelegate);
			}
		}

		internal static IMyLobby CreateLobby(ulong lobbyId)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.CreateLobby(lobbyId);
		}

		internal static void CreateLobby(MyLobbyType type, uint maxPlayers, MyLobbyCreated createdResponse)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.CreateLobby(type, maxPlayers, createdResponse);
			}
		}

		internal static void AddFriendLobbies(List<IMyLobby> lobbies)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.AddFriendLobbies(lobbies);
			}
		}

		internal static void AddPublicLobbies(List<IMyLobby> lobbies)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.AddPublicLobbies(lobbies);
			}
		}

		internal static void RequestLobbyList(Action<bool> completed)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.RequestLobbyList(completed);
			}
		}

		internal static void AddLobbyFilter(string key, string value)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.AddLobbyFilter(key, value);
			}
		}

		internal static int GetChatMaxMessageSize()
		{
			if (!EnsureGameService())
			{
				return 0;
			}
			return m_gameServiceCache.GetChatMaxMessageSize();
		}

		internal static MyGameServiceAccountType GetServerAccountType(ulong steamId)
		{
			if (!EnsureGameService())
			{
				return MyGameServiceAccountType.Invalid;
			}
			return m_gameServiceCache.GetServerAccountType(steamId);
		}

		internal static void SetServerModTemporaryDirectory()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.SetServerModTemporaryDirectory();
			}
		}

		internal static void InitializeVoiceRecording()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.InitializeVoiceRecording();
			}
		}

		internal static void DisposeVoiceRecording()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.DisposeVoiceRecording();
			}
		}

		internal static void StartVoiceRecording()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.StartVoiceRecording();
			}
		}

		internal static void StopVoiceRecording()
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.StopVoiceRecording();
			}
		}

		internal static int GetVoiceSampleRate()
		{
			if (!EnsureGameService())
			{
				return 0;
			}
			return m_gameServiceCache.SampleRate;
		}

		internal static MyVoiceResult DecompressVoice(byte[] compressedBuffer, uint size, byte[] uncompressedBuffer, out uint writtenBytes)
		{
			if (!EnsureGameService())
			{
				writtenBytes = 0u;
				return MyVoiceResult.NotInitialized;
			}
			return m_gameServiceCache.DecompressVoice(compressedBuffer, size, uncompressedBuffer, out writtenBytes);
		}

		internal static MyVoiceResult GetAvailableVoice(out uint size)
		{
			if (!EnsureGameService())
			{
				size = 0u;
				return MyVoiceResult.NotInitialized;
			}
			return m_gameServiceCache.GetAvailableVoice(out size);
		}

		internal static MyVoiceResult GetVoice(byte[] buffer, out uint bytesWritten)
		{
			if (!EnsureGameService())
			{
				bytesWritten = 0u;
				return MyVoiceResult.NotInitialized;
			}
			return m_gameServiceCache.GetVoice(buffer, out bytesWritten);
		}

		internal static MyPlayerVoiceChatState GetPlayerVoiceChatState(ulong playerId)
		{
			if (!EnsureGameService())
			{
				return MyPlayerVoiceChatState.Silent;
			}
			return m_gameServiceCache.GetPlayerVoiceChatState(playerId);
		}

		internal static void SetPlayerMuted(ulong playerId, bool muted)
		{
			if (EnsureGameService())
			{
				m_gameServiceCache.SetPlayerMuted(playerId, muted);
			}
		}

		internal static bool RecycleItem(MyGameInventoryItem item)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.RecycleItem(item);
		}

		internal static bool CraftSkin(MyGameInventoryItemQuality quality)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.CraftSkin(quality);
		}

		internal static uint GetCraftingCost(MyGameInventoryItemQuality quality)
		{
			if (!EnsureGameService())
			{
				return 0u;
			}
			return m_gameServiceCache.GetCraftingCost(quality);
		}

		internal static uint GetRecyclingReward(MyGameInventoryItemQuality quality)
		{
			if (!EnsureGameService())
			{
				return 0u;
			}
			return m_gameServiceCache.GetRecyclingReward(quality);
		}

		public static int GetFriendsCount()
		{
			if (!EnsureGameService())
			{
				return -1;
			}
			return m_gameServiceCache.GetFriendsCount();
		}

		public static ulong GetFriendIdByIndex(int index)
		{
			if (!EnsureGameService())
			{
				return 0uL;
			}
			return m_gameServiceCache.GetFriendIdByIndex(index);
		}

		public static string GetFriendNameByIndex(int index)
		{
			if (!EnsureGameService())
			{
				return string.Empty;
			}
			return m_gameServiceCache.GetFriendNameByIndex(index);
		}

		public static bool SaveToCloudAsync(string fileName, byte[] buffer, Action<bool> completedAction)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.SaveToCloudAsync(fileName, buffer, completedAction);
		}

		public static bool SaveToCloud(string fileName, byte[] buffer)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.SaveToCloud(fileName, buffer);
		}

		public static bool LoadFromCloudAsync(string fileName, Action<bool> completedAction)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.LoadFromCloudAsync(fileName, completedAction);
		}

		public static byte[] GetFileBufferFromCloud(string fileName)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetFileBufferFromCloud(fileName);
		}

		public static List<MyCloudFileInfo> GetCloudFiles(string directoryFilter)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.GetCloudFiles(directoryFilter);
		}

		public static byte[] LoadFromCloud(string fileName)
		{
			if (!EnsureGameService())
			{
				return null;
			}
			return m_gameServiceCache.LoadFromCloud(fileName);
		}

		public static bool DeleteFromCloud(string fileName)
		{
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.DeleteFromCloud(fileName);
		}

		public static bool IsUpdateAvailable()
		{
			return false;
		}

		public static MyWorkshopItem CreateWorkshopItem()
		{
			if (!EnsureUGCService())
			{
				return null;
			}
			return m_ugcServiceCache.CreateWorkshopItem();
		}

		public static MyWorkshopItemPublisher CreateWorkshopPublisher()
		{
			if (!EnsureUGCService())
			{
				return null;
			}
			return m_ugcServiceCache.CreateWorkshopPublisher();
		}

		public static MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item)
		{
			if (!EnsureUGCService())
			{
				return null;
			}
			return m_ugcServiceCache.CreateWorkshopPublisher(item);
		}

		public static MyWorkshopQuery CreateWorkshopQuery()
		{
			if (!EnsureUGCService())
			{
				return null;
			}
			return m_ugcServiceCache.CreateWorkshopQuery();
		}

		public static bool IsProductOwned(uint productId, out DateTime? purchaseTime)
		{
			purchaseTime = null;
			if (!EnsureGameService())
			{
				return false;
			}
			return m_gameServiceCache.IsProductOwned(productId, out purchaseTime);
		}

		public static void SuspendWorkshopDownloads()
		{
			if (EnsureUGCService())
			{
				m_ugcServiceCache.SuspendWorkshopDownloads();
			}
		}

		public static void ResumeWorkshopDownloads()
		{
			if (EnsureUGCService())
			{
				m_ugcServiceCache.ResumeWorkshopDownloads();
			}
		}

		public static void RequestWorkshopSecurityCode(string email, Action<MyGameServiceCallResult> onCompleted)
		{
			if (EnsureUGCService())
			{
				m_ugcServiceCache.RequestSecurityCode(email, onCompleted);
			}
		}

		public static void AuthenticateWorkshopWithSecurityCode(string code, Action<MyGameServiceCallResult> onCompleted)
		{
			if (EnsureUGCService())
			{
				m_ugcServiceCache.AuthenticateWithSecurityCode(code, onCompleted);
			}
		}

		public static void AddUnownedItems()
		{
			if (EnsureGameService() && !Sync.IsDedicated)
			{
				m_gameServiceCache.AddUnownedItems();
			}
		}
	}
}
