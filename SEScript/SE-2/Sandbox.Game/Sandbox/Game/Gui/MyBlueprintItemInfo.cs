using System;
using VRage.GameServices;

namespace Sandbox.Game.Gui
{
	public class MyBlueprintItemInfo
	{
		public MyBlueprintTypeEnum Type
		{
			get;
			set;
		}

		public ulong? PublishedItemId
		{
			get;
			set;
		}

		public MyWorkshopItem Item
		{
			get;
			set;
		}

		public DateTime? TimeCreated
		{
			get;
			set;
		}

		public DateTime? TimeUpdated
		{
			get;
			set;
		}

		public string BlueprintName
		{
			get;
			set;
		}

		public string CloudPathXML
		{
			get;
			set;
		}

		public string CloudPathPB
		{
			get;
			set;
		}

		public bool IsDirectory
		{
			get;
			set;
		}

		public MyCloudFileInfo CloudInfo
		{
			get;
			set;
		}

		public AdditionalBlueprintData Data
		{
			get;
			set;
		}

		public MyBlueprintItemInfo(MyBlueprintTypeEnum type, ulong? id = null)
		{
			Type = type;
			PublishedItemId = id;
			Data = new AdditionalBlueprintData();
		}

		public void SetAdditionalBlueprintInformation(string name = null, string description = null, uint[] dlcs = null)
		{
			Data.Name = (name ?? string.Empty);
			Data.Description = (description ?? string.Empty);
			Data.CloudImagePath = string.Empty;
			Data.DLCs = dlcs;
		}

		public override bool Equals(object obj)
		{
			MyBlueprintItemInfo myBlueprintItemInfo = obj as MyBlueprintItemInfo;
			if (myBlueprintItemInfo == null)
			{
				return false;
			}
			if (BlueprintName.Equals(myBlueprintItemInfo.BlueprintName) && Type.Equals(myBlueprintItemInfo.Type) && Data.Name.Equals(myBlueprintItemInfo.Data.Name) && Data.Description.Equals(myBlueprintItemInfo.Data.Description) && Data.CloudImagePath.Equals(myBlueprintItemInfo.Data.CloudImagePath))
			{
				return true;
			}
			return false;
		}
	}
}
