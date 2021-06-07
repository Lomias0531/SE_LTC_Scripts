using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace SpaceEngineers.Game.ModAPI
{
	public interface IMySolarPanel : Sandbox.ModAPI.IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, SpaceEngineers.Game.ModAPI.Ingame.IMySolarPanel
	{
		/// <summary>
		/// Resource (power) Source
		/// </summary>
		MyResourceSourceComponent SourceComp
		{
			get;
			set;
		}
	}
}
