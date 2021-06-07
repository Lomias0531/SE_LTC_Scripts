using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents.Renders
{
	public class MyRenderComponentWheel : MyRenderComponentCubeBlock
	{
		private class Sandbox_Game_EntityComponents_Renders_MyRenderComponentWheel_003C_003EActor : IActivator, IActivator<MyRenderComponentWheel>
		{
			private sealed override object CreateInstance()
			{
				return new MyRenderComponentWheel();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRenderComponentWheel CreateInstance()
			{
				return new MyRenderComponentWheel();
			}

			MyRenderComponentWheel IActivator<MyRenderComponentWheel>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyParticleEffect m_dustParticleEffect;

		private string m_dustParticleName = string.Empty;

		private Vector3 m_relativePosition = Vector3.Zero;

		private int m_timer;

		private MyWheel m_wheel;

		private const int PARTICLE_GENERATION_TIMEOUT = 20;

		public bool UpdateNeeded => m_timer > 0;

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			m_wheel = (base.Entity as MyWheel);
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			if (m_dustParticleEffect != null)
			{
				m_dustParticleEffect.Stop(instant: false);
			}
			m_wheel = null;
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
			if (m_dustParticleEffect != null)
			{
				m_dustParticleEffect.Stop(instant: false);
			}
		}

		public bool TrySpawnParticle(string particleName, ref Vector3D position, ref Vector3 normal)
		{
			if (!MyFakes.ENABLE_DRIVING_PARTICLES)
			{
				return false;
			}
			if (m_dustParticleEffect == null || !particleName.Equals(m_dustParticleName))
			{
				if (m_dustParticleEffect != null)
				{
					m_dustParticleEffect.Stop(instant: false);
					m_dustParticleEffect = null;
				}
				if (m_wheel.GetTopMostParent().Physics.LinearVelocity.LengthSquared() > 0.1f)
				{
					m_dustParticleName = particleName;
					MyParticlesManager.TryCreateParticleEffect(m_dustParticleName, GetParticleMatrix(ref position, ref normal), out m_dustParticleEffect);
					m_timer = 20;
				}
			}
			return true;
		}

		private MatrixD GetParticleMatrix(ref Vector3D position, ref Vector3 normal)
		{
			Vector3 vector = Vector3.Cross(normal, m_wheel.GetTopMostParent().Physics.LinearVelocity);
			Vector3 up = Vector3.Cross(normal, vector);
			return MatrixD.CreateWorld(position, normal, up);
		}

		public void UpdateParticle(ref Vector3D position, ref Vector3 normal)
		{
			if (m_dustParticleEffect != null)
			{
				float num = m_wheel.GetTopMostParent().Physics.LinearVelocity.LengthSquared();
				if (num < 0.1f)
				{
					m_dustParticleEffect.Stop(instant: false);
					m_dustParticleEffect = null;
					return;
				}
				float num2 = num / (MyGridPhysics.ShipMaxLinearVelocity() * MyGridPhysics.ShipMaxLinearVelocity());
				m_relativePosition = position - m_wheel.WorldMatrix.Translation;
				m_dustParticleEffect.WorldMatrix = GetParticleMatrix(ref position, ref normal);
				float userScale = 1f + num2 * 2f;
				m_dustParticleEffect.UserScale = userScale;
				m_timer = 20;
			}
		}

		public void UpdatePosition()
		{
			if (m_dustParticleEffect != null)
			{
				m_timer--;
				if (m_timer <= 0)
				{
					m_dustParticleEffect.Stop(instant: false);
					m_dustParticleEffect = null;
				}
				else
				{
					MatrixD worldMatrix = m_dustParticleEffect.WorldMatrix;
					worldMatrix.Translation = m_wheel.WorldMatrix.Translation + m_relativePosition;
					m_dustParticleEffect.WorldMatrix = worldMatrix;
				}
			}
		}
	}
}
