using System;

namespace VRage.ModAPI
{
	[Flags]
	public enum MyEntityUpdateEnum
	{
		NONE = 0x0,
		EACH_FRAME = 0x1,
		EACH_10TH_FRAME = 0x2,
		EACH_100TH_FRAME = 0x4,
		/// <summary>
		/// Separate update performed once before any other updates are called.
		/// </summary>
		BEFORE_NEXT_FRAME = 0x8,
		SIMULATE = 0x10
	}
}
