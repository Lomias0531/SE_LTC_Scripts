using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyRDPath : IMyPath
	{
		private MyRDPathfinding m_pathfinding;

		private IMyDestinationShape m_destination;

		private bool m_isValid;

		private bool m_pathCompleted;

		private List<Vector3D> m_pathPoints;

		private int m_currentPointIndex;

		private MyPlanet m_planet;

		public IMyDestinationShape Destination => m_destination;

		public IMyEntity EndEntity => null;

		public bool IsValid => m_isValid;

		public bool PathCompleted => m_pathCompleted;

		public MyRDPath(MyRDPathfinding pathfinding, Vector3D begin, IMyDestinationShape destination)
		{
			m_pathPoints = new List<Vector3D>();
			m_pathfinding = pathfinding;
			m_destination = destination;
			m_currentPointIndex = 0;
			m_planet = GetClosestPlanet(begin);
			m_isValid = (m_planet != null);
		}

		public void Invalidate()
		{
			m_isValid = false;
		}

		public bool GetNextTarget(Vector3D position, out Vector3D target, out float targetRadius, out IMyEntity relativeEntity)
		{
			target = Vector3D.Zero;
			relativeEntity = null;
			targetRadius = 0.8f;
			if (!m_isValid)
			{
				return false;
			}
			if (m_pathPoints.Count == 0 || m_pathCompleted || !m_isValid)
			{
				m_pathPoints = m_pathfinding.GetPath(m_planet, position, m_destination.GetDestination());
				if (m_pathPoints.Count < 2)
				{
					return false;
				}
				m_currentPointIndex = 1;
			}
			_ = m_currentPointIndex;
			_ = m_pathPoints.Count - 1;
			target = m_pathPoints[m_currentPointIndex];
			if (Math.Abs(Vector3.Distance(target, position)) < targetRadius)
			{
				if (m_currentPointIndex == m_pathPoints.Count - 1)
				{
					m_pathCompleted = true;
					return false;
				}
				m_currentPointIndex++;
				target = m_pathPoints[m_currentPointIndex];
			}
			return true;
		}

		public void Reinit(Vector3D position)
		{
		}

		public void DebugDraw()
		{
			if (m_pathPoints.Count > 0)
			{
				for (int i = 0; i < m_pathPoints.Count - 1; i++)
				{
					Vector3D pointFrom = m_pathPoints[i];
					Vector3D vector3D = m_pathPoints[i + 1];
					MyRenderProxy.DebugDrawLine3D(pointFrom, vector3D, Color.Blue, Color.Red, depthRead: true);
					MyRenderProxy.DebugDrawSphere(vector3D, 0.3f, Color.Yellow);
				}
			}
		}

		private MyPlanet GetClosestPlanet(Vector3D position)
		{
			int num = 200;
			BoundingBoxD box = new BoundingBoxD(position - (float)num * 0.5f, position + (float)num * 0.5f);
			return MyGamePruningStructure.GetClosestPlanet(ref box);
		}
	}
}
