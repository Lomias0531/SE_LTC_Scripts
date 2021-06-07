using System;
using System.Collections.Generic;
using VRage.Algorithms;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MySmartGoal : IMyHighLevelPrimitiveObserver
	{
		private MyNavigationPrimitive m_end;

		private MyHighLevelPrimitive m_hlEnd;

		private MyEntity m_endEntity;

		private bool m_hlEndIsApproximate;

		private IMyDestinationShape m_destination;

		private Vector3D m_destinationCenter;

		private Func<MyNavigationPrimitive, float> m_pathfindingHeuristic;

		private Func<MyNavigationPrimitive, float> m_terminationCriterion;

		private static Func<MyNavigationPrimitive, float> m_hlPathfindingHeuristic = HlHeuristic;

		private static Func<MyNavigationPrimitive, float> m_hlTerminationCriterion = HlCriterion;

		private static MySmartGoal m_pathfindingStatic = null;

		private HashSet<MyHighLevelPrimitive> m_ignoredPrimitives;

		public IMyDestinationShape Destination => m_destination;

		public MyEntity EndEntity => m_endEntity;

		public Func<MyNavigationPrimitive, float> PathfindingHeuristic => m_pathfindingHeuristic;

		public Func<MyNavigationPrimitive, float> TerminationCriterion => m_terminationCriterion;

		public bool IsValid
		{
			get;
			private set;
		}

		public MySmartGoal(IMyDestinationShape goal, MyEntity entity = null)
		{
			m_destination = goal;
			m_destinationCenter = goal.GetDestination();
			m_endEntity = entity;
			if (m_endEntity != null)
			{
				m_destination.SetRelativeTransform(m_endEntity.PositionComp.WorldMatrixNormalizedInv);
				m_endEntity.OnClosing += m_endEntity_OnClosing;
			}
			m_pathfindingHeuristic = Heuristic;
			m_terminationCriterion = Criterion;
			m_ignoredPrimitives = new HashSet<MyHighLevelPrimitive>();
			IsValid = true;
		}

		public void Invalidate()
		{
			if (m_endEntity != null)
			{
				m_endEntity.OnClosing -= m_endEntity_OnClosing;
				m_endEntity = null;
			}
			foreach (MyHighLevelPrimitive ignoredPrimitive in m_ignoredPrimitives)
			{
				ignoredPrimitive.Parent.StopObservingPrimitive(ignoredPrimitive, this);
			}
			m_ignoredPrimitives.Clear();
			IsValid = false;
		}

		public bool ShouldReinitPath()
		{
			return TargetMoved();
		}

		public void Reinit()
		{
			if (m_endEntity != null)
			{
				m_destination.UpdateWorldTransform(m_endEntity.WorldMatrix);
				m_destinationCenter = m_destination.GetDestination();
			}
		}

		public MyPath<MyNavigationPrimitive> FindHighLevelPath(MyPathfinding pathfinding, MyHighLevelPrimitive startPrimitive)
		{
			m_pathfindingStatic = this;
			MyPath<MyNavigationPrimitive> result = pathfinding.FindPath(startPrimitive, m_hlPathfindingHeuristic, m_hlTerminationCriterion, null, returnClosest: false);
			pathfinding.LastHighLevelTimestamp = pathfinding.GetCurrentTimestamp();
			m_pathfindingStatic = null;
			return result;
		}

		public MyPath<MyNavigationPrimitive> FindPath(MyPathfinding pathfinding, MyNavigationPrimitive startPrimitive)
		{
			throw new NotImplementedException();
		}

		public void IgnoreHighLevel(MyHighLevelPrimitive primitive)
		{
			if (!m_ignoredPrimitives.Contains(primitive))
			{
				primitive.Parent.ObservePrimitive(primitive, this);
				m_ignoredPrimitives.Add(primitive);
			}
		}

		private bool TargetMoved()
		{
			return Vector3D.DistanceSquared(m_destinationCenter, m_destination.GetDestination()) > 4.0;
		}

		private void m_endEntity_OnClosing(MyEntity obj)
		{
			m_endEntity = null;
			IsValid = false;
		}

		private float Heuristic(MyNavigationPrimitive primitive)
		{
			return (float)Vector3D.Distance(primitive.WorldPosition, m_destinationCenter);
		}

		private float Criterion(MyNavigationPrimitive primitive)
		{
			return m_destination.PointAdmissibility(primitive.WorldPosition, 2f);
		}

		private static float HlHeuristic(MyNavigationPrimitive primitive)
		{
			return (float)Vector3D.RectangularDistance(primitive.WorldPosition, m_pathfindingStatic.m_destinationCenter) * 2f;
		}

		private static float HlCriterion(MyNavigationPrimitive primitive)
		{
			MyHighLevelPrimitive myHighLevelPrimitive = primitive as MyHighLevelPrimitive;
			if (myHighLevelPrimitive == null || m_pathfindingStatic.m_ignoredPrimitives.Contains(myHighLevelPrimitive))
			{
				return float.PositiveInfinity;
			}
			float num = m_pathfindingStatic.m_destination.PointAdmissibility(primitive.WorldPosition, 8.7f);
			if (num < float.PositiveInfinity)
			{
				return num * 4f;
			}
			IMyHighLevelComponent component = myHighLevelPrimitive.GetComponent();
			if (component == null)
			{
				return float.PositiveInfinity;
			}
			if (!component.FullyExplored)
			{
				return (float)Vector3D.RectangularDistance(primitive.WorldPosition, m_pathfindingStatic.m_destinationCenter) * 8f;
			}
			return float.PositiveInfinity;
		}

		public void DebugDraw()
		{
			m_destination.DebugDraw();
			foreach (MyHighLevelPrimitive ignoredPrimitive in m_ignoredPrimitives)
			{
				MyRenderProxy.DebugDrawSphere(ignoredPrimitive.WorldPosition, 0.5f, Color.Red, 1f, depthRead: false);
			}
		}
	}
}
