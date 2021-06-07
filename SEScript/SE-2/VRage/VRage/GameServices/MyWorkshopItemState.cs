using System;

namespace VRage.GameServices
{
	[Flags]
	public enum MyWorkshopItemState
	{
		None = 0x0,
		Subscribed = 0x1,
		LegacyItem = 0x2,
		Installed = 0x4,
		NeedsUpdate = 0x8,
		Downloading = 0x10,
		DownloadPending = 0x20
	}
}
