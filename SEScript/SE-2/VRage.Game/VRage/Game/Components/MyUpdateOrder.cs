using System;

namespace VRage.Game.Components
{
	[Flags]
	public enum MyUpdateOrder
	{
		BeforeSimulation = 0x1,
		Simulation = 0x2,
		AfterSimulation = 0x4,
		NoUpdate = 0x0
	}
}
