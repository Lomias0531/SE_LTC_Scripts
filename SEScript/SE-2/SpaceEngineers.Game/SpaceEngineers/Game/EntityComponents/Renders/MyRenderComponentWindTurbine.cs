using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.RenderDirect.ActorComponents;
using SpaceEngineers.Game.Entities.Blocks;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.EntityComponents.Renders
{
	public class MyRenderComponentWindTurbine : MyRenderComponentCubeBlockWithParentedSubpart
	{
		public class TurbineRenderComponent : MyParentedSubpartRenderComponent
		{
			private float m_speed;

			private Color m_color;

			private bool m_animatorInitialized;

			protected MyWindTurbineDefinition Definition => ((MyWindTurbine)base.Entity.Parent).BlockDefinition;

			public void SetSpeed(float speed)
			{
				if (m_speed != speed)
				{
					m_speed = speed;
					SendSpeed();
				}
			}

			public void SetColor(Color color)
			{
				if (m_color != color)
				{
					m_color = color;
					SendColor();
				}
			}

			private void SendSpeed()
			{
				if (m_animatorInitialized)
				{
					uint renderObjectID = GetRenderObjectID();
					if (renderObjectID != uint.MaxValue)
					{
						FloatData.Update<MyRotationAnimator>(renderObjectID, m_speed);
					}
				}
			}

			private void SendColor()
			{
				if (GetRenderObjectID() != uint.MaxValue)
				{
					base.Entity.SetEmissiveParts("Emissive", m_color, 1f);
				}
			}

			public override void OnParented()
			{
				if (!((MyCubeGrid)base.Entity.Parent.Parent).IsPreview)
				{
					MyRenderProxy.UpdateRenderComponent(GetRenderObjectID(), this, delegate(MyRotationAnimatorInitData message, TurbineRenderComponent thiz)
					{
						MyWindTurbineDefinition definition = Definition;
						message.SpinUpSpeed = definition.TurbineSpinUpSpeed;
						message.SpinDownSpeed = definition.TurbineSpinDownSpeed;
						message.RotationAxis = MyRotationAnimator.RotationAxis.AxisY;
					});
					base.OnParented();
					m_animatorInitialized = true;
					SendSpeed();
					SendColor();
				}
			}
		}
	}
}
