using System.Runtime.InteropServices;

namespace Sandbox.Game.EntityComponents
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct MyEffectConstants
	{
		public static float HealthTick = 0.004166667f;

		public static float HealthInterval = 1f;

		public static float MinOxygenLevelForHealthRegeneration = 0.75f;

		public static float GenericHeal = -0.075f;

		public static float MedRoomHeal = 5f * GenericHeal;
	}
}
