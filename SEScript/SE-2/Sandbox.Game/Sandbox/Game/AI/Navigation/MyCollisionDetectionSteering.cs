using Sandbox.Engine.Physics;
using System.Collections.Generic;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Navigation
{
	public class MyCollisionDetectionSteering : MySteeringBase
	{
		private bool m_hitLeft;

		private bool m_hitRight;

		private float m_hitLeftFraction;

		private float m_hitRightFraction;

		public MyCollisionDetectionSteering(MyBotNavigation parent)
			: base(parent, 1f)
		{
		}

		public override string GetName()
		{
			return "Collision detection steering";
		}

		public override void Update()
		{
			base.Update();
		}

		public override void AccumulateCorrection(ref Vector3 correction, ref float weight)
		{
			m_hitLeft = false;
			m_hitRight = false;
			MatrixD positionAndOrientation = base.Parent.PositionAndOrientation;
			Vector3 forwardVector = base.Parent.ForwardVector;
			Vector3 vector = Vector3.Cross(positionAndOrientation.Up, forwardVector);
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			MyPhysics.CastRay(positionAndOrientation.Translation + positionAndOrientation.Up, positionAndOrientation.Translation + positionAndOrientation.Up + forwardVector * 0.1f + vector * 1.3f, list);
			if (list.Count > 0)
			{
				m_hitLeft = true;
				m_hitLeftFraction = list[0].HkHitInfo.HitFraction;
			}
			list.Clear();
			MyPhysics.CastRay(positionAndOrientation.Translation + positionAndOrientation.Up, positionAndOrientation.Translation + positionAndOrientation.Up + forwardVector * 0.1f - vector * 1.3f, list);
			if (list.Count > 0)
			{
				m_hitRight = true;
				m_hitRightFraction = list[0].HkHitInfo.HitFraction;
			}
			list.Clear();
			float num = base.Weight * 0.01f * (1f - m_hitLeftFraction);
			float num2 = base.Weight * 0.01f * (1f - m_hitRightFraction);
			if (m_hitLeft)
			{
				correction -= vector * num;
				weight += num;
			}
			if (m_hitRight)
			{
				correction += vector * num2;
				weight += num2;
			}
			if (m_hitLeft && m_hitRight)
			{
				correction -= vector;
				weight += num;
			}
		}

		public override void DebugDraw()
		{
			MatrixD positionAndOrientation = base.Parent.PositionAndOrientation;
			Vector3 forwardVector = base.Parent.ForwardVector;
			Vector3 value = Vector3.Cross(positionAndOrientation.Up, forwardVector);
			Color color = m_hitLeft ? Color.Orange : Color.Green;
			MyRenderProxy.DebugDrawLine3D(positionAndOrientation.Translation + positionAndOrientation.Up, positionAndOrientation.Translation + positionAndOrientation.Up + forwardVector * 0.1f + value * 1.3f, color, color, depthRead: true);
			MyRenderProxy.DebugDrawText3D(positionAndOrientation.Translation + positionAndOrientation.Up * 3.0, "Hit LT: " + m_hitLeftFraction, color, 0.7f, depthRead: false);
			color = (m_hitRight ? Color.Orange : Color.Green);
			MyRenderProxy.DebugDrawLine3D(positionAndOrientation.Translation + positionAndOrientation.Up, positionAndOrientation.Translation + positionAndOrientation.Up + forwardVector * 0.1f - value * 1.3f, color, color, depthRead: true);
			MyRenderProxy.DebugDrawText3D(positionAndOrientation.Translation + positionAndOrientation.Up * 3.2000000476837158, "Hit RT: " + m_hitRightFraction, color, 0.7f, depthRead: false);
		}
	}
}
