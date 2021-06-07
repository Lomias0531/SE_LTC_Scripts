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
	[MyCubeBlockType(typeof(MyObjectBuilder_Decoy))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyDecoy),
		typeof(Sandbox.ModAPI.Ingame.IMyDecoy)
	})]
	public class MyDecoy : MyFunctionalBlock, Sandbox.ModAPI.IMyDecoy, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDecoy
	{
		private class Sandbox_Game_Entities_Blocks_MyDecoy_003C_003EActor : IActivator, IActivator<MyDecoy>
		{
			private sealed override object CreateInstance()
			{
				return new MyDecoy();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDecoy CreateInstance()
			{
				return new MyDecoy();
			}

			MyDecoy IActivator<MyDecoy>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.CubeGrid.RegisterDecoy(this);
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			base.CubeGrid.UnregisterDecoy(this);
		}
	}
}
