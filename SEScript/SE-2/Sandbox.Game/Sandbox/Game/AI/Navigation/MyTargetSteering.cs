using System;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Navigation
{
	public class MyTargetSteering : MySteeringBase
	{
		protected Vector3D? m_target;

		protected MyEntity m_entity;

		private const float m_slowdownRadius = 4f;

		private const float m_maxSpeed = 1f;

		private float m_capsuleRadiusSq = 1f;

		private float m_capsuleHeight = 0.5f;

		private float m_capsuleOffset = -0.8f;

		public bool TargetSet => m_target.HasValue;

		public bool Flying
		{
			get;
			private set;
		}

		public Vector3D? TargetWorld
		{
			get
			{
				if (m_entity == null || m_entity.MarkedForClose)
				{
					return m_target;
				}
				if (m_target.HasValue)
				{
					return Vector3D.Transform(m_target.Value, m_entity.WorldMatrix);
				}
				return null;
			}
		}

		public MyTargetSteering(MyBotNavigation navigation)
			: base(navigation, 1f)
		{
			m_target = null;
		}

		public override string GetName()
		{
			return "Target steering";
		}

		public void SetTarget(Vector3D target, float radius = 1f, MyEntity relativeEntity = null, float weight = 1f, bool fly = false)
		{
			if (relativeEntity == null || relativeEntity.MarkedForClose)
			{
				m_entity = null;
				m_target = target;
			}
			else
			{
				m_entity = relativeEntity;
				m_target = Vector3D.Transform(target, m_entity.PositionComp.WorldMatrixNormalizedInv);
			}
			m_capsuleRadiusSq = radius * radius;
			base.Weight = weight;
			Flying = fly;
		}

		public void UnsetTarget()
		{
			m_target = null;
		}

		public bool TargetReached()
		{
			if (!TargetWorld.HasValue)
			{
				return false;
			}
			Vector3D target = TargetWorld.Value;
			return TargetReached(ref target, m_capsuleRadiusSq);
		}

		protected Vector3D CapsuleCenter()
		{
			Vector3D up = base.Parent.PositionAndOrientation.Up;
			return base.Parent.PositionAndOrientation.Translation + up * (m_capsuleOffset + m_capsuleHeight) * 0.5;
		}

		public double TargetDistanceSq(ref Vector3D target)
		{
			Vector3D vector = base.Parent.PositionAndOrientation.Up;
			Vector3D vector2 = base.Parent.PositionAndOrientation.Translation + vector * m_capsuleOffset;
			Vector3D.Dot(ref vector2, ref vector, out double result);
			Vector3D.Dot(ref target, ref vector, out double result2);
			result2 -= result;
			if (result2 >= (double)m_capsuleHeight)
			{
				vector2 += vector;
			}
			else if (result2 >= 0.0)
			{
				vector2 += vector * result2;
			}
			Vector3D.DistanceSquared(ref target, ref vector2, out double result3);
			return result3;
		}

		public bool TargetReached(ref Vector3D target, float radiusSq)
		{
			return TargetDistanceSq(ref target) < (double)radiusSq;
		}

		public override void AccumulateCorrection(ref Vector3 correctionHint, ref float weight)
		{
			if (m_entity != null && m_entity.MarkedForClose)
			{
				m_entity = null;
			}
			GetMovements(out Vector3 currentMovement, out Vector3 wantedMovement);
			correctionHint += (wantedMovement - currentMovement) * base.Weight;
			weight += base.Weight;
		}

		public override void Update()
		{
			base.Update();
			if (TargetReached())
			{
				UnsetTarget();
			}
		}

		private void GetMovements(out Vector3 currentMovement, out Vector3 wantedMovement)
		{
			Vector3? vector = TargetWorld;
			currentMovement = base.Parent.ForwardVector * base.Parent.Speed;
			if (vector.HasValue)
			{
				wantedMovement = vector.Value - base.Parent.PositionAndOrientation.Translation;
				float num = wantedMovement.Length();
				if (num > 4f)
				{
					wantedMovement = wantedMovement * 1f / num;
				}
				else
				{
					wantedMovement = wantedMovement * 1f / 4f;
				}
			}
			else
			{
				wantedMovement = Vector3.Zero;
			}
		}

		public override void DebugDraw()
		{
			Vector3D vector3D = base.Parent.PositionAndOrientation.Translation + base.Parent.PositionAndOrientation.Up * m_capsuleOffset;
			Vector3D vector3D2 = vector3D + base.Parent.PositionAndOrientation.Up * m_capsuleHeight;
			Vector3D pointFrom = (vector3D + vector3D2) * 0.5;
			GetMovements(out Vector3 currentMovement, out Vector3 wantedMovement);
			Vector3D? targetWorld = TargetWorld;
			if (targetWorld.HasValue)
			{
				MyRenderProxy.DebugDrawLine3D(pointFrom, targetWorld.Value, Color.White, Color.White, depthRead: true);
				MyRenderProxy.DebugDrawSphere(targetWorld.Value, 0.05f, Color.White.ToVector3(), 1f, depthRead: false);
				MyRenderProxy.DebugDrawCapsule(vector3D, vector3D2, (float)Math.Sqrt(m_capsuleRadiusSq), Color.Yellow, depthRead: false);
			}
			MyRenderProxy.DebugDrawLine3D(vector3D2, vector3D2 + wantedMovement, Color.Red, Color.Red, depthRead: false);
			MyRenderProxy.DebugDrawLine3D(vector3D2, vector3D2 + currentMovement, Color.Green, Color.Green, depthRead: false);
		}
	}
}
