using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace SpaceEngineers.Game.ModAPI
{
	public interface IMySafeZoneBlock : Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMySafeZoneBlock
	{
		/// <summary>
		/// Enables(activates)/disables(deactivates) safe zone. Grid has to be static. If grid is not static this method will do nothing.
		/// </summary>
		/// <param name="turnOn">True to enable the safe zone. Otherwise false.</param>
		void EnableSafeZone(bool turnOn);

		/// <summary>
		/// Gets state of the safe zone. Enabled(active).
		/// </summary>
		/// <returns>True - All safe zone filters are active. Otherwise false.</returns>
		bool IsSafeZoneEnabled();
	}
}
