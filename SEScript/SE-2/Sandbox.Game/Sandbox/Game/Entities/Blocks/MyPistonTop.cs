using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_PistonTop))]
	public class MyPistonTop : MyAttachableTopBlockBase, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyPistonTop, Sandbox.ModAPI.IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyPistonTop
	{
		private class Sandbox_Game_Entities_Blocks_MyPistonTop_003C_003EActor : IActivator, IActivator<MyPistonTop>
		{
			private sealed override object CreateInstance()
			{
				return new MyPistonTop();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyPistonTop CreateInstance()
			{
				return new MyPistonTop();
			}

			MyPistonTop IActivator<MyPistonTop>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyPistonBase m_pistonBlock;

		private MyAttachableConveyorEndpoint m_conveyorEndpoint;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		bool Sandbox.ModAPI.Ingame.IMyAttachableTopBlock.IsAttached => m_pistonBlock != null;

		Sandbox.ModAPI.IMyMechanicalConnectionBlock Sandbox.ModAPI.IMyAttachableTopBlock.Base => m_pistonBlock;

		Sandbox.ModAPI.IMyPistonBase Sandbox.ModAPI.IMyPistonTop.Base => m_pistonBlock;

		Sandbox.ModAPI.IMyPistonBase Sandbox.ModAPI.IMyPistonTop.Piston => m_pistonBlock;

		public override void Attach(MyMechanicalConnectionBlockBase pistonBase)
		{
			base.Attach(pistonBase);
			m_pistonBlock = (pistonBase as MyPistonBase);
		}

		public override void ContactPointCallback(ref MyGridContactInfo value)
		{
			base.ContactPointCallback(ref value);
			if (m_pistonBlock != null && value.CollidingEntity == m_pistonBlock.Subpart3)
			{
				value.EnableDeformation = false;
				value.EnableParticles = false;
			}
		}

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
