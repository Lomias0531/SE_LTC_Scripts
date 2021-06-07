using System;

namespace VRageRender
{
	/// <summary>
	/// Light type, flags, could be combined
	/// </summary>
	[Flags]
	public enum LightTypeEnum
	{
		None = 0x0,
		PointLight = 0x1,
		Spotlight = 0x2
	}
}
