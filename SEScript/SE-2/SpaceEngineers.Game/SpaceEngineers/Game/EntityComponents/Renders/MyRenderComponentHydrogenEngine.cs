using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.RenderDirect.ActorComponents;
using SpaceEngineers.Game.Entities.Blocks;
using VRage.Render.Scene.Components;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.EntityComponents.Renders
{
	public class MyRenderComponentHydrogenEngine : MyRenderComponentCubeBlockWithParentedSubpart
	{
		public class MyHydrogenEngineSubpartRenderComponent<TComponent> : MyParentedSubpartRenderComponent where TComponent : MyRenderDirectComponent
		{
			private float m_speed;

			private bool m_animatorInitialized;

			protected MyHydrogenEngineDefinition Definition => ((MyHydrogenEngine)base.Entity.Parent).BlockDefinition;

			public void SetSpeed(float speed)
			{
				if (speed != m_speed)
				{
					m_speed = speed;
					SendSpeed();
				}
			}

			private void SendSpeed()
			{
				if (m_animatorInitialized)
				{
					uint renderObjectID = GetRenderObjectID();
					if (renderObjectID != uint.MaxValue)
					{
						FloatData.Update<TComponent>(renderObjectID, m_speed);
					}
				}
			}

			public override void OnParented()
			{
				base.OnParented();
				m_animatorInitialized = true;
				SendSpeed();
			}

			protected void FillAnimationParams(MySpinupAnimatorInitData data)
			{
				MyHydrogenEngineDefinition definition = Definition;
				data.SpinUpSpeed = definition.AnimationSpinUpSpeed;
				data.SpinDownSpeed = definition.AnimationSpinDownSpeed;
			}
		}

		public class MyRotatingSubpartRenderComponent : MyHydrogenEngineSubpartRenderComponent<MyRotationAnimator>
		{
			public override void OnParented()
			{
				if (!((MyCubeGrid)base.Entity.Parent.Parent).IsPreview)
				{
					MyRenderProxy.UpdateRenderComponent(GetRenderObjectID(), this, delegate(MyRotationAnimatorInitData message, MyRotatingSubpartRenderComponent thiz)
					{
						thiz.FillAnimationParams(message);
						message.RotationAxis = MyRotationAnimator.RotationAxis.AxisZ;
					});
					base.OnParented();
				}
			}
		}

		public class MyPistonRenderComponent : MyHydrogenEngineSubpartRenderComponent<MyTranslationAnimator>
		{
			public float AnimationOffset
			{
				get;
				set;
			}

			public override void OnParented()
			{
				if (!((MyCubeGrid)base.Entity.Parent.Parent).IsPreview)
				{
					MyRenderProxy.UpdateRenderComponent(GetRenderObjectID(), this, delegate(MyTranslationAnimatorInitData message, MyPistonRenderComponent thiz)
					{
						thiz.FillAnimationParams(message);
						message.AnimationOffset = AnimationOffset;
						message.TranslationAxis = Base6Directions.Direction.Up;
						message.MinPosition = thiz.Definition.PistonAnimationMin;
						message.MaxPosition = thiz.Definition.PistonAnimationMax;
						thiz.GetCullObjectRelativeMatrix(out message.BaseRelativeTransform);
					});
					base.OnParented();
				}
			}
		}
	}
}
