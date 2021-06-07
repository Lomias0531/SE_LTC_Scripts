using System.Collections.Generic;
using VRage.GameServices;
using VRage.Mod.Io.Data;
using VRage.Utils;

namespace VRage.Mod.Io
{
	internal class MyModIoWorkshopItem : MyWorkshopItem
	{
		private ModProfile m_profile;

		private ulong m_bytesDownloaded;

		private ulong m_bytesTotal;

		public override ulong BytesDownloaded
		{
			get
			{
				UpdateDownloadProgress();
				return m_bytesDownloaded;
			}
		}

		public override ulong BytesTotal
		{
			get
			{
				UpdateDownloadProgress();
				return m_bytesTotal;
			}
		}

		public override float DownloadProgress
		{
			get
			{
				UpdateDownloadProgress();
				if (m_bytesTotal == 0L)
				{
					return 0f;
				}
				return (float)m_bytesDownloaded / (float)m_bytesTotal;
			}
		}

		public MyModIoWorkshopItem()
		{
		}

		public MyModIoWorkshopItem(ModProfile profile)
		{
			m_profile = profile;
			base.Id = (ulong)profile.id;
			base.OwnerId = (ulong)profile.submitted_by.id;
			base.Title = profile.name;
			base.Description = (profile.description_plaintext ?? profile.summary);
			base.Size = (ulong)profile.modfile.filesize;
			base.ItemType = MyWorkshopItemType.Item;
			base.Visibility = ((profile.visible != 1) ? MyPublishedFileVisibility.Private : MyPublishedFileVisibility.Public);
			m_tags = new List<string>();
			ModTag[] tags = profile.tags;
			foreach (ModTag modTag in tags)
			{
				m_tags.Add(modTag.name);
			}
			base.TimeUpdated = ((uint)profile.date_updated).ToDateTimeFromUnixTimestamp();
			base.TimeCreated = ((uint)profile.date_added).ToDateTimeFromUnixTimestamp();
			base.Score = profile.stats.ratings_percentage_positive;
			MyModIo.GetModDependencies(base.Id, DependenciesResponse);
		}

		private void DependenciesResponse(RequestPage<ModDependency> dependency, MyGameServiceCallResult result)
		{
			if (result != MyGameServiceCallResult.OK || dependency == null)
			{
				return;
			}
			if (m_dependencies != null)
			{
				m_dependencies.Clear();
			}
			else
			{
				m_dependencies = new List<ulong>(dependency.result_total);
			}
			for (int i = 0; i < dependency.data.Length; i++)
			{
				if (dependency.data[i].mod_id != 0)
				{
					m_dependencies.Add((ulong)dependency.data[i].mod_id);
				}
			}
		}

		private void UpdateDownloadProgress()
		{
			MyModIoCache.GetItemDownloadInfo(base.Id, out m_bytesDownloaded, out m_bytesTotal);
		}

		public override MyWorkshopItemPublisher GetPublisher()
		{
			return new MyModIoWorkshopItemPublisher(this);
		}

		public override void Download()
		{
			base.Download();
			if (base.Id == 0L)
			{
				OnItemDownloaded(MyGameServiceCallResult.Fail, base.Id);
				return;
			}
			base.State = MyModIoCache.GetItemState(m_profile.modfile);
			if (base.State.HasFlag(MyWorkshopItemState.Downloading))
			{
				OnItemDownloaded(MyGameServiceCallResult.Pending, base.Id);
			}
			else if (base.State != MyWorkshopItemState.Installed)
			{
				MyModIoCache.DownloadItem(m_profile.modfile, DownloadItemResponse);
			}
			else
			{
				DownloadItemResponse(MyGameServiceCallResult.OK);
			}
		}

		private void DownloadItemResponse(MyGameServiceCallResult result)
		{
			MyLog.Default.WriteLineAndConsole($"Workshop item with id {base.Id} download finished. Result: {result}");
			base.State = MyModIoCache.GetItemState(m_profile.modfile);
			if (result == MyGameServiceCallResult.OK)
			{
				UpdateInstalledItem();
			}
			OnItemDownloaded(result, base.Id);
		}

		private void UpdateInstalledItem()
		{
			if (MyModIoCache.GetItemInstallInfo(m_profile.modfile, out ulong size, out string folder, out uint timeStamp))
			{
				if (!base.State.HasFlag(MyWorkshopItemState.LegacyItem))
				{
					base.Size = size;
				}
				base.Folder = folder;
				base.LocalTimeUpdated = timeStamp.ToDateTimeFromUnixTimestamp();
			}
			else
			{
				base.State |= MyWorkshopItemState.NeedsUpdate;
			}
		}

		public override void UpdateState()
		{
			base.UpdateState();
			if (base.Id != 0L)
			{
				base.LocalTimeUpdated = DateTimeExtensions.Epoch;
				base.State = MyModIoCache.GetItemState(m_profile.modfile);
				if (base.State.HasFlag(MyWorkshopItemState.Installed))
				{
					UpdateInstalledItem();
				}
			}
		}

		public override void Subscribe()
		{
			MyModIo.Subscribe(base.Id);
		}

		public override bool IsUpToDate()
		{
			return (MyModIoCache.GetItemState(m_profile.modfile) & (MyWorkshopItemState.Installed | MyWorkshopItemState.NeedsUpdate)) == MyWorkshopItemState.Installed;
		}
	}
}
