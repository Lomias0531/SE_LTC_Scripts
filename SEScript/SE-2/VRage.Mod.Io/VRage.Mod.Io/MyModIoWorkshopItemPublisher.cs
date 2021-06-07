using System.Collections.Generic;
using VRage.GameServices;
using VRage.Mod.Io.Data;
using VRage.Utils;

namespace VRage.Mod.Io
{
	internal class MyModIoWorkshopItemPublisher : MyWorkshopItemPublisher
	{
		public MyModIoWorkshopItemPublisher()
		{
		}

		public MyModIoWorkshopItemPublisher(MyWorkshopItem item)
		{
			base.Id = item.Id;
			base.Title = item.Title;
			base.Description = item.Description;
			base.Metadata = item.Metadata;
			base.Visibility = item.Visibility;
			Tags = new List<string>(item.Tags);
			DLCs = new HashSet<uint>(item.DLCs);
			Dependencies = new List<ulong>(item.Dependencies);
		}

		public override void Publish()
		{
			base.Publish();
			MyModIo.AddOrEditMod(base.Id, base.Title, base.Description, base.Visibility, Tags, MyModMetadataLoader.Serialize(base.Metadata), base.Thumbnail, base.Folder, OnPublished);
		}

		private void OnPublished(Modfile modFile, MyGameServiceCallResult result)
		{
			MyLog.Default.Info("Workshop item with id {0} update finished. Result: {1}", modFile?.mod_id ?? 0, result);
			if (result == MyGameServiceCallResult.OK)
			{
				base.Id = (ulong)modFile.mod_id;
				MyModIo.Subscribe(base.Id);
			}
			OnItemPublished(result, base.Id);
		}
	}
}
