using System;

namespace Sandbox.Graphics.GUI
{
	[Flags]
	public enum UrlOpenMode
	{
		SteamOverlay = 0x1,
		ExternalBrowser = 0x2,
		ConfirmExternal = 0x4,
		SteamOrExternal = 0x3,
		SteamOrExternalWithConfirm = 0x7
	}
}
