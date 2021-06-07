using System;

namespace VRage.GameServices
{
	public interface IMyUGCService
	{
		string ServiceName
		{
			get;
		}

		string LegalUrl
		{
			get;
		}

		bool SupportsLinkAccount
		{
			get;
		}

		event Action<UGCAuthenticationFailReason, Action> OnAuthenticationFailed;

		MyWorkshopItem CreateWorkshopItem();

		MyWorkshopItemPublisher CreateWorkshopPublisher();

		MyWorkshopItemPublisher CreateWorkshopPublisher(MyWorkshopItem item);

		MyWorkshopQuery CreateWorkshopQuery();

		void SuspendWorkshopDownloads();

		void ResumeWorkshopDownloads();

		void LinkAccount(string email, Action<MyGameServiceCallResult> onDone);

		void RequestSecurityCode(string email, Action<MyGameServiceCallResult> onCompleted);

		void AuthenticateWithSecurityCode(string code, Action<MyGameServiceCallResult> onCompleted);

		string GetItemUrl(ulong itemId);

		string GetItemListUrl(string requiredTag);

		void Update();
	}
}
