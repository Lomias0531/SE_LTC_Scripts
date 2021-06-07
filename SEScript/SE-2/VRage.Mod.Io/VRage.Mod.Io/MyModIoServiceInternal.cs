using System;
using VRage.GameServices;

namespace VRage.Mod.Io
{
	internal class MyModIoServiceInternal : IDisposable, IMyUGCService
	{
		private IMyGameService m_service;

		public string ServiceName => "mod.io";

		public string LegalUrl => "https://mod.io/terms";

		public ulong UserId => m_service.UserId;

		public bool SupportsLinkAccount => true;

		public event Action<UGCAuthenticationFailReason, Action> OnAuthenticationFailed;

		public bool HasFriend(ulong userId)
		{
			return m_service.HasFriend(userId);
		}

		public MyModIoServiceInternal(bool isDedicated, IMyGameService service, string gameId, string gameName, string apiKey)
		{
			m_service = service;
			MyModIo.Init(m_service, this, gameId, gameName, apiKey);
		}

		public MyWorkshopItem CreateWorkshopItem()
		{
			return new MyModIoWorkshopItem();
		}

		public MyWorkshopItemPublisher CreateWorkshopPublisher()
		{
			return new MyModIoWorkshopItemPublisher();
		}

		public MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item)
		{
			return new MyModIoWorkshopItemPublisher(item);
		}

		public MyWorkshopQuery CreateWorkshopQuery()
		{
			return new MyModIoWorkshopQuery(this);
		}

		public void SuspendWorkshopDownloads()
		{
			MyModIo.SuspendDownloads(state: true);
		}

		public void ResumeWorkshopDownloads()
		{
			MyModIo.SuspendDownloads(state: false);
		}

		public void LinkAccount(string email, Action<MyGameServiceCallResult> onDone)
		{
			MyModIo.LinkAccount(email, onDone);
		}

		public void RequestSecurityCode(string email, Action<MyGameServiceCallResult> onCompleted)
		{
			MyModIo.EmailRequest(email, onCompleted);
		}

		public void AuthenticateWithSecurityCode(string code, Action<MyGameServiceCallResult> onCompleted)
		{
			MyModIo.EmailExchange(code, onCompleted);
		}

		public string GetItemUrl(ulong itemId)
		{
			return MyModIo.GetWebUrl() + itemId;
		}

		public string GetItemListUrl(string requiredTag)
		{
			return MyModIo.GetWebUrl() + "?filter=t&tag[]=" + requiredTag;
		}

		public void Dispose()
		{
		}

		public void Update()
		{
			MyModIo.Update();
		}

		public void AuthenticationFailed(UGCAuthenticationFailReason reason, Action onDone)
		{
			this.OnAuthenticationFailed.InvokeIfNotNull(reason, onDone);
		}
	}
}
