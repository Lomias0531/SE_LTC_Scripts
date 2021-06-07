using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	public interface IMyAssembler : IMyProductionBlock, IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyProductionBlock, Sandbox.ModAPI.Ingame.IMyAssembler
	{
		/// <summary>
		/// Called when the current item build progress changes.
		/// </summary>
		event Action<IMyAssembler> CurrentProgressChanged;

		/// <summary>
		/// Called when the state of the assembler changes.
		/// </summary>
		event Action<IMyAssembler> CurrentStateChanged;

		/// <summary>
		/// Called when the build mode changes.
		/// </summary>
		event Action<IMyAssembler> CurrentModeChanged;
	}
}
