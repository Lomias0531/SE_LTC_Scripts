using System;

namespace VRage.ObjectBuilders
{
	[Flags]
	public enum MyPersistentEntityFlags2
	{
		None = 0x0,
		Enabled = 0x2,
		CastShadows = 0x4,
		InScene = 0x10
	}
}
