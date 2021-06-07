using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Ship welder interface
	/// </summary>
	public interface IMyShipWelder : IMyShipToolBase, IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyShipToolBase, Sandbox.ModAPI.Ingame.IMyShipWelder
	{
		/// <summary>
		/// Determines whether the projected grid still fits within block limits set by server after a new block is added
		/// </summary>
		bool IsWithinWorldLimits(IMyProjector projector, string name, int pcuToBuild);
	}
}
