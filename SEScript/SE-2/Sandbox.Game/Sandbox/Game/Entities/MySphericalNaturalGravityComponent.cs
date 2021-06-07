using System;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public class MySphericalNaturalGravityComponent : MyGravityProviderComponent
	{
		private class Sandbox_Game_Entities_MySphericalNaturalGravityComponent_003C_003EActor
		{
		}

		private const double GRAVITY_LIMIT_STRENGTH = 0.05;

		private readonly double m_minRadius;

		private readonly double m_maxRadius;

		private readonly double m_falloff;

		private readonly double m_intensity;

		private float m_gravityLimit;

		private float m_gravityLimitSq;

		public Vector3D Position
		{
			get;
			private set;
		}

		public override bool IsWorking => true;

		public float GravityLimit
		{
			get
			{
				return m_gravityLimit;
			}
			private set
			{
				m_gravityLimitSq = value * value;
				m_gravityLimit = value;
			}
		}

		public float GravityLimitSq
		{
			get
			{
				return m_gravityLimitSq;
			}
			private set
			{
				m_gravityLimitSq = value;
				m_gravityLimit = (float)Math.Sqrt(value);
			}
		}

		public override string ComponentTypeDebugString => GetType().Name;

		public MySphericalNaturalGravityComponent(double minRadius, double maxRadius, double falloff, double intensity)
		{
			m_minRadius = minRadius;
			m_maxRadius = maxRadius;
			m_falloff = falloff;
			m_intensity = intensity;
			double y = 1.0 / falloff;
			GravityLimit = (float)(maxRadius * Math.Pow(intensity / 0.05, y));
		}

		public override bool IsPositionInRange(Vector3D worldPoint)
		{
			return (Position - worldPoint).LengthSquared() <= (double)m_gravityLimitSq;
		}

		public override void GetProxyAABB(out BoundingBoxD aabb)
		{
			BoundingSphereD sphere = new BoundingSphereD(Position, GravityLimit);
			BoundingBoxD.CreateFromSphere(ref sphere, out aabb);
		}

		public override Vector3 GetWorldGravity(Vector3D worldPoint)
		{
			Vector3 worldGravityNormalized = GetWorldGravityNormalized(ref worldPoint);
			float gravityMultiplier = GetGravityMultiplier(worldPoint);
			return worldGravityNormalized * 9.81f * gravityMultiplier;
		}

		public override float GetGravityMultiplier(Vector3D worldPoint)
		{
			double num = (Position - worldPoint).Length();
			if (num > (double)m_gravityLimit)
			{
				return 0f;
			}
			float num2 = 1f;
			if (num > m_maxRadius)
			{
				num2 = (float)Math.Pow(num / m_maxRadius, 0.0 - m_falloff);
			}
			else if (num < m_minRadius)
			{
				num2 = (float)(num / m_minRadius);
				if (num2 < 0.01f)
				{
					num2 = 0.01f;
				}
			}
			return (float)((double)num2 * m_intensity);
		}

		public Vector3 GetWorldGravityNormalized(ref Vector3D worldPoint)
		{
			Vector3 result = Position - worldPoint;
			result.Normalize();
			return result;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
			Position = base.Entity.PositionComp.GetPosition();
		}
	}
}
