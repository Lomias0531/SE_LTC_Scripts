using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Pathfinding;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.AI.Navigation
{
	public class MyPathSteering : MyTargetSteering
	{
		private IMyPath m_path;

		private float m_weight;

		private const float END_RADIUS = 0.5f;

		private const float DISTANCE_FOR_FINAL_APPROACH = 2f;

		public bool PathFinished
		{
			get;
			private set;
		}

		public MyPathSteering(MyBotNavigation navigation)
			: base(navigation)
		{
		}

		public override string GetName()
		{
			return "Path steering";
		}

		public void SetPath(IMyPath path, float weight = 1f)
		{
			if (path == null || !path.IsValid)
			{
				UnsetPath();
				return;
			}
			if (m_path != null)
			{
				m_path.Invalidate();
			}
			m_path = path;
			m_weight = weight;
			PathFinished = false;
			SetNextTarget();
		}

		public void UnsetPath()
		{
			if (m_path != null)
			{
				m_path.Invalidate();
			}
			m_path = null;
			UnsetTarget();
			PathFinished = true;
		}

		private void SetNextTarget()
		{
			Vector3D? targetWorld = base.TargetWorld;
			if (m_path == null || !m_path.IsValid)
			{
				UnsetTarget();
				return;
			}
			Vector3D target = m_path.Destination.GetClosestPoint(CapsuleCenter());
			double num = TargetDistanceSq(ref target);
			if (num > 0.25)
			{
				Vector3D translation = base.Parent.PositionAndOrientation.Translation;
				if (m_path.PathCompleted)
				{
					if (num < 4.0)
					{
						MyEntity relativeEntity = m_path.EndEntity as MyEntity;
						UnsetPath();
						SetTarget(target, 0.5f, relativeEntity, m_weight);
						return;
					}
					if (targetWorld.HasValue)
					{
						m_path.Reinit(targetWorld.Value);
					}
					else
					{
						m_path.Reinit(translation);
					}
				}
				Vector3D target2;
				float targetRadius;
				IMyEntity relativeEntity2;
				bool nextTarget = m_path.GetNextTarget(base.Parent.PositionAndOrientation.Translation, out target2, out targetRadius, out relativeEntity2);
				MyEntity relativeEntity3 = relativeEntity2 as MyEntity;
				if (nextTarget)
				{
					SetTarget(target2, targetRadius, relativeEntity3, m_weight);
					return;
				}
			}
			UnsetPath();
		}

		public override void Update()
		{
			if (m_path == null)
			{
				base.Update();
			}
			else if (!m_path.IsValid)
			{
				UnsetPath();
			}
			else if (TargetReached())
			{
				SetNextTarget();
			}
		}

		public override void Cleanup()
		{
			base.Cleanup();
			if (m_path != null && m_path.IsValid)
			{
				m_path.Invalidate();
			}
		}

		public override void DebugDraw()
		{
			if (m_path != null && m_path.IsValid && MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyFakes.DEBUG_DRAW_FOUND_PATH)
			{
				m_path.DebugDraw();
			}
		}
	}
}
