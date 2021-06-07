using System;

namespace VRage.Network
{
	[Flags]
	public enum MyInvocationFlags
	{
		None = 0x0,
		Invoke = 0x1,
		Validate = 0x2
	}
}
