using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_MotorAdvancedStator))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyMotorAdvancedStator),
		typeof(Sandbox.ModAPI.Ingame.IMyMotorAdvancedStator)
	})]
	public class MyMotorAdvancedStator : MyMotorStator, Sandbox.ModAPI.IMyMotorAdvancedStator, Sandbox.ModAPI.IMyMotorStator, Sandbox.ModAPI.Ingame.IMyMotorStator, Sandbox.ModAPI.Ingame.IMyMotorBase, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyMotorBase, Sandbox.ModAPI.IMyMechanicalConnectionBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMotorAdvancedStator
	{
		private class Sandbox_Game_Entities_Cube_MyMotorAdvancedStator_003C_003EActor : IActivator, IActivator<MyMotorAdvancedStator>
		{
			private sealed override object CreateInstance()
			{
				return new MyMotorAdvancedStator();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMotorAdvancedStator CreateInstance()
			{
				return new MyMotorAdvancedStator();
			}

			MyMotorAdvancedStator IActivator<MyMotorAdvancedStator>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override bool Attach(MyAttachableTopBlockBase rotor, bool updateGroup = true)
		{
			if (rotor is MyMotorRotor)
			{
				bool num = base.Attach(rotor, updateGroup);
				if (num && updateGroup && base.TopBlock is MyMotorAdvancedRotor)
				{
					m_conveyorEndpoint.Attach((base.TopBlock as MyMotorAdvancedRotor).ConveyorEndpoint as MyAttachableConveyorEndpoint);
				}
				return num;
			}
			return false;
		}

		protected override void Detach(MyCubeGrid topGrid, bool updateGroup = true)
		{
			if (base.TopBlock != null && updateGroup)
			{
				MyAttachableTopBlockBase topBlock = base.TopBlock;
				if (topBlock is MyMotorAdvancedRotor)
				{
					m_conveyorEndpoint.Detach((topBlock as MyMotorAdvancedRotor).ConveyorEndpoint as MyAttachableConveyorEndpoint);
				}
			}
			base.Detach(topGrid, updateGroup);
		}

		public override void ComputeTopQueryBox(out Vector3D pos, out Vector3 halfExtents, out Quaternion orientation)
		{
			base.ComputeTopQueryBox(out pos, out halfExtents, out orientation);
			if (base.CubeGrid.GridSizeEnum == MyCubeSize.Small)
			{
				halfExtents.Y *= 2f;
			}
		}

		public MyMotorAdvancedStator()
		{
			m_canBeDetached = true;
		}
	}
}
