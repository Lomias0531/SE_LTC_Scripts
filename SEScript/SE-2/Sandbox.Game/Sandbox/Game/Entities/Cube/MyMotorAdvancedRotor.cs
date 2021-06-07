using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;

namespace Sandbox.Game.Entities.Cube
{
	[MyCubeBlockType(typeof(MyObjectBuilder_MotorAdvancedRotor))]
	public class MyMotorAdvancedRotor : MyMotorRotor, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyMotorAdvancedRotor, Sandbox.ModAPI.IMyMotorRotor, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyMotorRotor, Sandbox.ModAPI.Ingame.IMyMotorAdvancedRotor
	{
		private class Sandbox_Game_Entities_Cube_MyMotorAdvancedRotor_003C_003EActor : IActivator, IActivator<MyMotorAdvancedRotor>
		{
			private sealed override object CreateInstance()
			{
				return new MyMotorAdvancedRotor();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMotorAdvancedRotor CreateInstance()
			{
				return new MyMotorAdvancedRotor();
			}

			MyMotorAdvancedRotor IActivator<MyMotorAdvancedRotor>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyAttachableConveyorEndpoint m_conveyorEndpoint;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyAttachableConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
		}

		public PullInformation GetPullInformation()
		{
			return null;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
