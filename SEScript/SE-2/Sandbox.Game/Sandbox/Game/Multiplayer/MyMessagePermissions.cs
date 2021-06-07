using System;

namespace Sandbox.Game.Multiplayer
{
	[Flags]
	public enum MyMessagePermissions
	{
		FromServer = 0x1,
		ToServer = 0x2
	}
}
