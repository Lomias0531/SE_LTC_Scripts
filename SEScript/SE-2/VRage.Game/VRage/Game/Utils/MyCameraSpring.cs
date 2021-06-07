using VRageMath;

namespace VRage.Game.Utils
{
	/// <summary>
	/// Camera spring 
	/// </summary>
	public class MyCameraSpring
	{
		/// <summary>
		/// Is the sprint enabled?
		/// </summary>
		public bool Enabled = true;

		private Vector3 m_springCenterLinearVelocity;

		private Vector3 m_springCenterLinearVelocityLast;

		private Vector3 m_springBodyVelocity;

		private Vector3 m_springBodyPosition;

		private float m_stiffness;

		private float m_weight;

		private float m_dampening;

		private float m_maxVelocityChange;

		private static float m_springMaxLength;

		/// <summary>
		/// Stiffness of the spring.
		/// </summary>
		public float SpringStiffness
		{
			get
			{
				return m_stiffness;
			}
			set
			{
				m_stiffness = MathHelper.Clamp(value, 0f, 50f);
			}
		}

		/// <summary>
		/// Spring velocity dampening.
		/// </summary>
		public float SpringDampening
		{
			get
			{
				return m_dampening;
			}
			set
			{
				m_dampening = MathHelper.Clamp(value, 0f, 1f);
			}
		}

		/// <summary>
		/// Maximum speed of spring center.
		/// </summary>
		public float SpringMaxVelocity
		{
			get
			{
				return m_maxVelocityChange;
			}
			set
			{
				m_maxVelocityChange = MathHelper.Clamp(value, 0f, 10f);
			}
		}

		/// <summary>
		/// Final spring length is transformed using calculation:
		/// springTransformedLength = SpringMaxLength * springLength / (springLength + 2)
		/// </summary>
		public float SpringMaxLength
		{
			get
			{
				return m_springMaxLength;
			}
			set
			{
				m_springMaxLength = MathHelper.Clamp(value, 0f, 2f);
			}
		}

		public MyCameraSpring()
		{
			Reset(resetSpringSettings: true);
		}

		public void Reset(bool resetSpringSettings)
		{
			m_springCenterLinearVelocity = Vector3.Zero;
			m_springCenterLinearVelocityLast = Vector3.Zero;
			m_springBodyVelocity = Vector3.Zero;
			m_springBodyPosition = Vector3.Zero;
			if (resetSpringSettings)
			{
				m_stiffness = 20f;
				m_weight = 1f;
				m_dampening = 0.7f;
				m_maxVelocityChange = 2f;
				m_springMaxLength = 0.5f;
			}
		}

		public void SetCurrentCameraControllerVelocity(Vector3 velocity)
		{
			m_springCenterLinearVelocity = velocity;
		}

		public void AddCurrentCameraControllerVelocity(Vector3 velocity)
		{
			m_springCenterLinearVelocity += velocity;
		}

		/// <summary>
		/// Update camera spring.
		/// </summary>
		/// <param name="timeStep">Time passed.</param>
		/// <param name="newCameraLocalOffset">Resulting local camera position.</param>
		public bool Update(float timeStep, out Vector3 newCameraLocalOffset)
		{
			if (!Enabled)
			{
				newCameraLocalOffset = Vector3.Zero;
				m_springCenterLinearVelocity = Vector3.Zero;
				return false;
			}
			Vector3 value = m_springCenterLinearVelocity - m_springCenterLinearVelocityLast;
			if (value.LengthSquared() > m_maxVelocityChange * m_maxVelocityChange)
			{
				value.Normalize();
				value *= m_maxVelocityChange;
			}
			m_springCenterLinearVelocityLast = m_springCenterLinearVelocity;
			m_springBodyPosition += value * timeStep;
			Vector3 value2 = -m_springBodyPosition * m_stiffness / m_weight;
			m_springBodyVelocity += value2 * timeStep;
			m_springBodyPosition += m_springBodyVelocity * timeStep;
			m_springBodyVelocity *= m_dampening;
			newCameraLocalOffset = TransformLocalOffset(m_springBodyPosition);
			return true;
		}

		private static Vector3 TransformLocalOffset(Vector3 springBodyPosition)
		{
			float num = springBodyPosition.Length();
			if (num <= 1E-05f)
			{
				return springBodyPosition;
			}
			return m_springMaxLength * num / (num + 2f) * springBodyPosition / num;
		}
	}
}
