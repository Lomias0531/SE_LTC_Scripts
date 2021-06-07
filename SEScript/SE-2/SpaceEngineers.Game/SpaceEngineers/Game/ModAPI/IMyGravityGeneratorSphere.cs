using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace SpaceEngineers.Game.ModAPI
{
	public interface IMyGravityGeneratorSphere : IMyGravityGeneratorBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase, IMyGravityProvider, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorSphere
	{
		/// <summary>
		/// Radius of the gravity field, in meters
		/// </summary>
		/// <remarks>This is not clamped like the Ingame one is.</remarks>
		new float Radius
		{
			get;
			set;
		}
	}
}
