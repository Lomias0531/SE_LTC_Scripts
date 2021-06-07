using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_MotorRotor))]
	public class MyMotorRotor : MyAttachableTopBlockBase, Sandbox.ModAPI.IMyMotorRotor, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyMotorRotor
	{
		private class Sandbox_Game_Entities_Cube_MyMotorRotor_003C_003EActor : IActivator, IActivator<MyMotorRotor>
		{
			private sealed override object CreateInstance()
			{
				return new MyMotorRotor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMotorRotor CreateInstance()
			{
				return new MyMotorRotor();
			}

			MyMotorRotor IActivator<MyMotorRotor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[Obsolete("Use MyAttachableTopBlockBase.Base")]
		Sandbox.ModAPI.IMyMotorBase Sandbox.ModAPI.IMyMotorRotor.Stator => base.Stator as MyMotorStator;

		Sandbox.ModAPI.IMyMotorBase Sandbox.ModAPI.IMyMotorRotor.Base => (Sandbox.ModAPI.IMyMotorBase)base.Stator;
	}
}
