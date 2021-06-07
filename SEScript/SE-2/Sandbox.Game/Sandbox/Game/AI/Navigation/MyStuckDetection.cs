using System;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
	public class MyStuckDetection
	{
		private static readonly int STUCK_COUNTDOWN = 60;

		private static readonly int LONGTERM_COUNTDOWN = 300;

		private static readonly double LONGTERM_TOLERANCE = 0.025;

		private Vector3D m_translationStuckDetection;

		private Vector3D m_longTermTranslationStuckDetection;

		private Vector3 m_rotationStuckDetection;

		private float m_positionToleranceSq;

		private float m_rotationToleranceSq;

		private double m_previousDistanceFromTarget;

		private bool m_isRotating;

		private int m_counter;

		private int m_longTermCounter;

		private int m_tickCounter;

		private int m_stoppedTime;

		private BoundingBoxD m_boundingBox;

		public bool IsStuck
		{
			get;
			private set;
		}

		public MyStuckDetection(float positionTolerance, float rotationTolerance)
		{
			m_positionToleranceSq = positionTolerance * positionTolerance;
			m_rotationToleranceSq = rotationTolerance * rotationTolerance;
			Reset();
		}

		public MyStuckDetection(float positionTolerance, float rotationTolerance, BoundingBoxD box)
			: this(positionTolerance, rotationTolerance)
		{
			m_boundingBox = box;
		}

		public void SetRotating(bool rotating)
		{
			m_isRotating = rotating;
		}

		public void Update(Vector3D worldPosition, Vector3 rotation, Vector3D targetLocation = default(Vector3D))
		{
			m_translationStuckDetection = m_translationStuckDetection * 0.8 + worldPosition * 0.2;
			m_rotationStuckDetection = m_rotationStuckDetection * 0.95f + rotation * 0.05f;
			bool flag = (m_translationStuckDetection - worldPosition).LengthSquared() < (double)m_positionToleranceSq && (m_rotationStuckDetection - rotation).LengthSquared() < m_rotationToleranceSq && !m_isRotating;
			double num = (worldPosition - targetLocation).Length();
			if (targetLocation != Vector3D.Zero && !flag && num < 2.0 * m_boundingBox.Extents.Min())
			{
				if (Math.Abs(m_previousDistanceFromTarget - num) > 1.0)
				{
					m_previousDistanceFromTarget = num + 1.0;
				}
				m_previousDistanceFromTarget = m_previousDistanceFromTarget * 0.7 + num * 0.3;
				flag = (Math.Abs(num - m_previousDistanceFromTarget) < (double)m_positionToleranceSq);
			}
			if (m_counter <= 0)
			{
				if (flag)
				{
					IsStuck = true;
				}
				else
				{
					m_counter = STUCK_COUNTDOWN;
				}
			}
			else
			{
				if (m_counter == STUCK_COUNTDOWN && !flag)
				{
					IsStuck = false;
					return;
				}
				m_counter--;
			}
			if (m_longTermCounter <= 0)
			{
				if ((m_longTermTranslationStuckDetection - worldPosition).LengthSquared() < LONGTERM_TOLERANCE)
				{
					IsStuck = true;
					return;
				}
				m_longTermCounter = LONGTERM_COUNTDOWN;
				m_longTermTranslationStuckDetection = worldPosition;
			}
			else
			{
				m_longTermCounter--;
			}
		}

		public void Reset(bool force = false)
		{
			if (force || m_stoppedTime != m_tickCounter)
			{
				m_translationStuckDetection = Vector3D.Zero;
				m_rotationStuckDetection = Vector3.Zero;
				IsStuck = false;
				m_counter = STUCK_COUNTDOWN;
				m_longTermCounter = LONGTERM_COUNTDOWN;
				m_isRotating = false;
			}
		}

		public void Stop()
		{
			m_stoppedTime = m_tickCounter;
		}

		public void SetCurrentTicks(int behaviorTicks)
		{
			m_tickCounter = behaviorTicks;
		}
	}
}
