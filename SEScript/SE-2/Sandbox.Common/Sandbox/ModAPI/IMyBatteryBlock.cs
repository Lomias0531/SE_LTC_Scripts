using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	public interface IMyBatteryBlock : IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyBatteryBlock
	{
	}
}
