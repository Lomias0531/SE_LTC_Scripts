using Sandbox.RenderDirect.ActorComponents;
using System;
using VRage.Network;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.EntityComponents.Renders
{
	public class MyShipDrillRenderComponent : MyRenderComponentCubeBlockWithParentedSubpart
	{
		public class MyDrillHeadRenderComponent : MyParentedSubpartRenderComponent
		{
			private class Sandbox_Game_EntityComponents_Renders_MyShipDrillRenderComponent_003C_003EMyDrillHeadRenderComponent_003C_003EActor : IActivator, IActivator<MyDrillHeadRenderComponent>
			{
				private sealed override object CreateInstance()
				{
					return new MyDrillHeadRenderComponent();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyDrillHeadRenderComponent CreateInstance()
				{
					return new MyDrillHeadRenderComponent();
				}

				MyDrillHeadRenderComponent IActivator<MyDrillHeadRenderComponent>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			private float m_speed;

			public override void OnParented()
			{
				base.OnParented();
				uint renderObjectID = GetRenderObjectID();
				float context = MathF.PI * 8f;
				MyRenderProxy.UpdateRenderComponent(renderObjectID, context, delegate(MyRotationAnimatorInitData d, float s)
				{
					d.SpinUpSpeed = s;
					d.SpinDownSpeed = s;
					d.RotationAxis = MyRotationAnimator.RotationAxis.AxisZ;
				});
			}

			public void UpdateSpeed(float speed)
			{
				if (m_speed != speed)
				{
					m_speed = speed;
					FloatData.Update<MyRotationAnimator>(GetRenderObjectID(), speed);
				}
			}
		}

		private class Sandbox_Game_EntityComponents_Renders_MyShipDrillRenderComponent_003C_003EActor : IActivator, IActivator<MyShipDrillRenderComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipDrillRenderComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipDrillRenderComponent CreateInstance()
			{
				return new MyShipDrillRenderComponent();
			}

			MyShipDrillRenderComponent IActivator<MyShipDrillRenderComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
