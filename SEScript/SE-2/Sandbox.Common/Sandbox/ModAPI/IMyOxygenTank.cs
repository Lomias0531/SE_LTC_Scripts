using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	[Obsolete("Use IMyGasTank")]
	public interface IMyOxygenTank : IMyGasTank, IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyGasTank, Sandbox.ModAPI.Ingame.IMyOxygenTank
	{
	}
}
