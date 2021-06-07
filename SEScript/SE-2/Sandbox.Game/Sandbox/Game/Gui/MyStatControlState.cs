using System;

namespace Sandbox.Game.GUI
{
	[Flags]
	public enum MyStatControlState
	{
		FadingOut = 0x1,
		FadingIn = 0x2,
		Visible = 0x4,
		Invisible = 0x8
	}
}
