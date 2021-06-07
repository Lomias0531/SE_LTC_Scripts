using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ExtendedPistonBase))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyExtendedPistonBase),
		typeof(Sandbox.ModAPI.Ingame.IMyExtendedPistonBase)
	})]
	public class MyExtendedPistonBase : MyPistonBase, Sandbox.ModAPI.IMyExtendedPistonBase, Sandbox.ModAPI.IMyPistonBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyPistonBase, Sandbox.ModAPI.Ingame.IMyExtendedPistonBase
	{
		private class Sandbox_Game_Entities_Blocks_MyExtendedPistonBase_003C_003EActor : IActivator, IActivator<MyExtendedPistonBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyExtendedPistonBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyExtendedPistonBase CreateInstance()
			{
				return new MyExtendedPistonBase();
			}

			MyExtendedPistonBase IActivator<MyExtendedPistonBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
