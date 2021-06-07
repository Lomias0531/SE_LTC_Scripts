using Sandbox.RenderDirect.ActorComponents;
using VRage.Network;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.EntityComponents.Renders
{
	public class MyRenderComponentShipGrinder : MyRenderComponentCubeBlockWithParentedSubpart
	{
		public class MyRenderComponentShipGrinderBlade : MyParentedSubpartRenderComponent
		{
			private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentShipGrinder_003C_003EMyRenderComponentShipGrinderBlade_003C_003EActor : IActivator, IActivator<MyRenderComponentShipGrinderBlade>
			{
				private sealed override object CreateInstance()
				{
					return new MyRenderComponentShipGrinderBlade();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyRenderComponentShipGrinderBlade CreateInstance()
				{
					return new MyRenderComponentShipGrinderBlade();
				}

				MyRenderComponentShipGrinderBlade IActivator<MyRenderComponentShipGrinderBlade>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private float m_speed;

			public override void OnParented()
			{
				base.OnParented();
				m_speed = 0f;
				MyRenderProxy.UpdateRenderComponent(GetRenderObjectID(), null, delegate(MyRotationAnimatorInitData d, object _)
				{
					d.RotationAxis = MyRotationAnimator.RotationAxis.AxisX;
					d.SpinUpSpeed = 41.8879f;
					d.SpinDownSpeed = 20.94395f;
				});
			}

			public void UpdateBladeSpeed(float speed)
			{
				if (m_speed != speed)
				{
					m_speed = speed;
					uint renderObjectID = GetRenderObjectID();
					if (renderObjectID != uint.MaxValue)
					{
						FloatData.Update<MyRotationAnimator>(renderObjectID, m_speed);
					}
				}
			}
		}

		private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentShipGrinder_003C_003EActor : IActivator, IActivator<MyRenderComponentShipGrinder>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentShipGrinder();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentShipGrinder CreateInstance()
			{
				return new MyRenderComponentShipGrinder();
			}

			MyRenderComponentShipGrinder IActivator<MyRenderComponentShipGrinder>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
