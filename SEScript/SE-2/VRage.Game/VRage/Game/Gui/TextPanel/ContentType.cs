using System;

namespace VRage.Game.GUI.TextPanel
{
	public enum ContentType : byte
	{
		NONE,
		TEXT_AND_IMAGE,
		[Obsolete("Use TEXT_AND_IMAGE instead.")]
		IMAGE,
		SCRIPT
	}
}
