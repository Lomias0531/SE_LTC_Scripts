using System.Collections.Generic;
using VRage.GameServices;
using VRage.Mod.Io.Data;

namespace VRage.Mod.Io
{
	internal class MyModIoWorkshopQuery : MyWorkshopQuery
	{
		private MyModIoServiceInternal m_service;

		private bool m_queryActive;

		private int m_resultsReturned;

		private int m_page;

		public override uint ItemsPerPage => 50u;

		public override bool IsRunning => m_queryActive;

		public MyModIoWorkshopQuery(MyModIoServiceInternal service)
		{
			m_service = service;
		}

		public override void Run()
		{
			base.Run();
			if (!m_queryActive)
			{
				base.TotalResults = 0u;
				if (base.Items == null)
				{
					base.Items = new List<MyWorkshopItem>();
				}
				base.Items.Clear();
				RunQuery();
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			m_service = null;
			base.ItemIds?.Clear();
			base.RequiredTags?.Clear();
			base.ExcludedTags?.Clear();
			base.Items = null;
			m_queryActive = false;
		}

		private void QueryCleanup()
		{
			m_queryActive = false;
			m_page = 0;
		}

		private void RunQuery()
		{
			QueryCleanup();
			m_queryActive = true;
			if ((base.ItemIds != null && base.ItemIds.Count > 0) || base.UserId == 0L)
			{
				MyModIo.GetMods(Response, MyModIo.Sort.Subscribers, base.SearchString, base.ItemIds, base.RequiredTags, base.ExcludedTags, m_page, ItemsPerPage);
			}
			else
			{
				MyModIo.GetMySubscriptions(Response, MyModIo.Sort.Name, base.SearchString, base.ItemIds, base.RequiredTags, base.ExcludedTags, m_page, ItemsPerPage);
			}
		}

		private void Response(RequestPage<ModProfile> mods, MyGameServiceCallResult result)
		{
			if (!m_queryActive || result != MyGameServiceCallResult.OK)
			{
				QueryCleanup();
				OnQueryCompleted((!m_queryActive) ? MyGameServiceCallResult.Cancelled : result);
				return;
			}
			m_queryActive = false;
			base.TotalResults = (uint)mods.result_total;
			int num = mods.data.Length;
			m_resultsReturned += num;
			for (uint num2 = 0u; num2 < num; num2++)
			{
				MyModIoWorkshopItem myModIoWorkshopItem = new MyModIoWorkshopItem(mods.data[num2]);
				if ((myModIoWorkshopItem.Visibility != MyPublishedFileVisibility.Private || myModIoWorkshopItem.OwnerId == m_service.UserId) && (myModIoWorkshopItem.Visibility != MyPublishedFileVisibility.FriendsOnly || myModIoWorkshopItem.OwnerId == m_service.UserId || m_service.HasFriend(myModIoWorkshopItem.OwnerId)))
				{
					base.Items.Add(myModIoWorkshopItem);
				}
			}
			if (m_resultsReturned == base.TotalResults)
			{
				OnQueryCompleted(MyGameServiceCallResult.OK);
				return;
			}
			m_page++;
			RunQuery();
		}
	}
}
