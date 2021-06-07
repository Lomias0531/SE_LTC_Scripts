using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	public interface IMyCockpit : IMyShipController, IMyTerminalBlock, IMyCubeBlock, IMyEntity, IMyTextSurfaceProvider
	{
		/// <summary>
		/// Gets the maximum oxygen capacity of this cockpit.
		/// </summary>
		float OxygenCapacity
		{
			get;
		}

		/// <summary>
		/// Gets the current oxygen level of this cockpit, as a value between 0 (empty) and 1 (full).
		/// </summary>
		/// <returns></returns>
		float OxygenFilledRatio
		{
			get;
		}
	}
}
