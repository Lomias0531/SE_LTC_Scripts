using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.Multiplayer;
using Sandbox.RenderDirect.ActorComponents;
using System;
using VRage.Network;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Components
{
	public class MyRenderComponentThrust : MyRenderComponentCubeBlockWithParentedSubpart
	{
		public class MyPropellerRenderComponent : MyParentedSubpartRenderComponent
		{
			private class Sandbox_Game_Components_MyRenderComponentThrust_003C_003EMyPropellerRenderComponent_003C_003EActor : IActivator, IActivator<MyPropellerRenderComponent>
			{
				private sealed override object CreateInstance()
				{
					return new MyPropellerRenderComponent();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyPropellerRenderComponent CreateInstance()
				{
					return new MyPropellerRenderComponent();
				}

				MyPropellerRenderComponent IActivator<MyPropellerRenderComponent>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public override void OnParented()
			{
				base.OnParented();
				MyThrust myThrust = (MyThrust)base.Entity.Parent;
				MyRenderProxy.UpdateRenderComponent(GetRenderObjectID(), myThrust, delegate(MyRotationAnimatorInitData d, MyThrust t)
				{
					MyThrustDefinition blockDefinition = t.BlockDefinition;
					float num = blockDefinition.PropellerFullSpeed * (MathF.PI * 2f);
					d.SpinUpSpeed = num / blockDefinition.PropellerAcceleration;
					d.SpinDownSpeed = num / blockDefinition.PropellerDeceleration;
					d.RotationAxis = MyRotationAnimator.RotationAxis.AxisZ;
				});
				SendPropellerSpeed(myThrust.Render.m_propellerSpeed);
			}

			public void SendPropellerSpeed(float speed)
			{
				FloatData.Update<MyRotationAnimator>(GetRenderObjectID(), speed);
			}
		}

		private class Sandbox_Game_Components_MyRenderComponentThrust_003C_003EActor : IActivator, IActivator<MyRenderComponentThrust>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentThrust();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentThrust CreateInstance()
			{
				return new MyRenderComponentThrust();
			}

			MyRenderComponentThrust IActivator<MyRenderComponentThrust>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private float m_strength;

		private bool m_flamesEnabled;

		private float m_propellerSpeed;

		private MyThrust m_thrust;

		private bool m_flameAnimatorInitialized;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_thrust = (base.Container.Entity as MyThrust);
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			float strength = m_strength;
			bool flamesEnabled = m_flamesEnabled;
			m_strength = 0f;
			m_propellerSpeed = 0f;
			m_flamesEnabled = false;
			m_flameAnimatorInitialized = false;
			UpdateFlameAnimatorData();
			if (m_flameAnimatorInitialized)
			{
				UpdateFlameProperties(flamesEnabled, strength);
			}
		}

		public void UpdateFlameProperties(bool enabled, float strength)
		{
			if (m_thrust.CubeGrid.Physics != null && m_flameAnimatorInitialized)
			{
				bool flag = false;
				if (m_strength != strength)
				{
					flag = true;
					m_strength = strength;
				}
				if (m_flamesEnabled != enabled)
				{
					flag = true;
					m_flamesEnabled = enabled;
				}
				if (flag)
				{
					FloatData.Update<MyThrustFlameAnimator>(GetRenderObjectID(), m_flamesEnabled ? m_strength : (-1f));
				}
			}
		}

		public void UpdateFlameAnimatorData()
		{
			if (m_thrust.CubeGrid.Physics == null || Sync.IsDedicated)
			{
				return;
			}
			uint renderObjectID = GetRenderObjectID();
			if (renderObjectID == uint.MaxValue)
			{
				return;
			}
			if (m_thrust.Flames.Count == 0)
			{
				if (m_flameAnimatorInitialized)
				{
					UpdateFlameProperties(enabled: false, 0f);
					m_flameAnimatorInitialized = false;
					MyRenderProxy.RemoveRenderComponent<MyThrustFlameAnimator>(renderObjectID);
				}
			}
			else
			{
				m_flameAnimatorInitialized = true;
				MyRenderProxy.UpdateRenderComponent(renderObjectID, m_thrust, delegate(FlameData d, MyThrust t)
				{
					MatrixD matrix = t.PositionComp.LocalMatrix;
					d.LightPosition = Vector3D.TransformNormal(t.Flames[0].Position, matrix) + matrix.Translation;
					d.Flames = t.Flames;
					d.FlareSize = t.Flares.Size;
					d.Glares = t.Flares.SubGlares;
					d.GridScale = t.CubeGrid.GridScale;
					d.FlareIntensity = t.Flares.Intensity;
					d.FlamePointMaterial = t.FlamePointMaterial;
					d.FlameLengthMaterial = t.FlameLengthMaterial;
					d.GlareQuerySize = t.CubeGrid.GridSize / 2.5f;
					d.IdleColor = t.BlockDefinition.FlameIdleColor;
					d.FullColor = t.BlockDefinition.FlameFullColor;
					d.FlameLengthScale = t.BlockDefinition.FlameLengthScale;
				});
			}
		}

		public void UpdatePropellerSpeed(float propellerSpeed)
		{
			if (m_propellerSpeed != propellerSpeed)
			{
				m_propellerSpeed = propellerSpeed;
				((MyPropellerRenderComponent)m_thrust.Propeller.Render).SendPropellerSpeed(propellerSpeed);
			}
		}
	}
}
