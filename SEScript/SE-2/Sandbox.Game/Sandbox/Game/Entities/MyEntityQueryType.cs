using System;

namespace Sandbox.Game.Entities
{
	[Flags]
	public enum MyEntityQueryType : byte
	{
		Static = 0x1,
		Dynamic = 0x2,
		Both = 0x3
	}
}
