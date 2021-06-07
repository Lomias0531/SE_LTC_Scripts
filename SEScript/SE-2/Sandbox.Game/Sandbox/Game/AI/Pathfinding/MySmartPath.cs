using Sandbox.Engine.Utils;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Algorithms;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MySmartPath : IMyHighLevelPrimitiveObserver, IMyPath
	{
		private MyPathfinding m_pathfinding;

		private int m_lastInitTime;

		private bool m_usedWholePath;

		private bool m_valid;

		private List<MyHighLevelPrimitive> m_pathNodes;

		private List<Vector4D> m_expandedPath;

		private int m_pathNodePosition;

		private int m_expandedPathPosition;

		private MyNavigationPrimitive m_currentPrimitive;

		private MyHighLevelPrimitive m_hlBegin;

		private Vector3D m_startPoint;

		private MySmartGoal m_goal;

		private static MySmartPath m_pathfindingStatic;

		private const float TRANSITION_RADIUS = 1f;

		public IMyDestinationShape Destination => m_goal.Destination;

		public IMyEntity EndEntity => m_goal.EndEntity;

		public bool IsValid
		{
			get
			{
				if (!m_goal.IsValid)
				{
					if (m_valid)
					{
						Invalidate();
					}
					return false;
				}
				if (m_valid)
				{
					return true;
				}
				m_goal.Invalidate();
				return false;
			}
		}

		public bool PathCompleted => m_usedWholePath;

		public MySmartPath(MyPathfinding pathfinding)
		{
			m_pathfinding = pathfinding;
			m_pathNodes = new List<MyHighLevelPrimitive>();
			m_expandedPath = new List<Vector4D>();
		}

		public void Init(Vector3D start, MySmartGoal goal)
		{
			m_lastInitTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_startPoint = start;
			m_goal = goal;
			m_currentPrimitive = m_pathfinding.FindClosestPrimitive(start, highLevel: false);
			if (m_currentPrimitive != null)
			{
				m_hlBegin = m_currentPrimitive.GetHighLevelPrimitive();
				if (m_hlBegin != null && !m_pathNodes.Contains(m_hlBegin))
				{
					m_hlBegin.Parent.ObservePrimitive(m_hlBegin, this);
				}
			}
			if (m_currentPrimitive == null)
			{
				m_currentPrimitive = null;
				Invalidate();
				return;
			}
			m_pathNodePosition = 0;
			m_expandedPathPosition = 0;
			m_expandedPath.Clear();
			m_pathNodes.Clear();
			m_usedWholePath = false;
			m_valid = true;
		}

		public void Reinit(Vector3D newStart)
		{
			MySmartGoal goal = m_goal;
			_ = goal.EndEntity;
			ClearPathNodes();
			m_expandedPath.Clear();
			m_expandedPathPosition = 0;
			m_currentPrimitive = null;
			if (m_hlBegin != null)
			{
				m_hlBegin.Parent.StopObservingPrimitive(m_hlBegin, this);
			}
			m_hlBegin = null;
			m_valid = false;
			m_goal.Reinit();
			Init(newStart, goal);
		}

		public bool GetNextTarget(Vector3D currentPosition, out Vector3D targetWorld, out float radius, out IMyEntity relativeEntity)
		{
			bool flag = false;
			targetWorld = default(Vector3D);
			radius = 1f;
			relativeEntity = null;
			if (m_pathNodePosition > 1)
			{
				ClearFirstPathNode();
			}
			if (m_expandedPathPosition >= m_expandedPath.Count)
			{
				if (!m_usedWholePath)
				{
					flag = ShouldReinitPath();
				}
				if (flag)
				{
					Reinit(currentPosition);
				}
				if (!IsValid)
				{
					return false;
				}
				ExpandPath(currentPosition);
				if (m_expandedPath.Count == 0)
				{
					return false;
				}
			}
			if (m_expandedPathPosition < m_expandedPath.Count)
			{
				Vector4D xyz = m_expandedPath[m_expandedPathPosition];
				targetWorld = new Vector3D(xyz);
				radius = (float)xyz.W;
				m_expandedPathPosition++;
				if (m_expandedPathPosition == m_expandedPath.Count && m_pathNodePosition >= m_pathNodes.Count - 1)
				{
					m_usedWholePath = true;
				}
				relativeEntity = null;
				return true;
			}
			return false;
		}

		public void Invalidate()
		{
			if (m_valid)
			{
				ClearPathNodes();
				m_expandedPath.Clear();
				m_expandedPathPosition = 0;
				m_currentPrimitive = null;
				if (m_goal.IsValid)
				{
					m_goal.Invalidate();
				}
				if (m_hlBegin != null)
				{
					m_hlBegin.Parent.StopObservingPrimitive(m_hlBegin, this);
				}
				m_hlBegin = null;
				m_valid = false;
			}
		}

		private void ExpandPath(Vector3D currentPosition)
		{
			if (m_pathNodePosition >= m_pathNodes.Count - 1)
			{
				GenerateHighLevelPath();
			}
			if (m_pathNodePosition >= m_pathNodes.Count)
			{
				return;
			}
			MyPath<MyNavigationPrimitive> myPath = null;
			bool flag = false;
			m_expandedPath.Clear();
			if (m_pathNodePosition + 1 < m_pathNodes.Count)
			{
				if (m_pathNodes[m_pathNodePosition].IsExpanded && m_pathNodes[m_pathNodePosition + 1].IsExpanded)
				{
					IMyHighLevelComponent component = m_pathNodes[m_pathNodePosition].GetComponent();
					IMyHighLevelComponent otherComponent = m_pathNodes[m_pathNodePosition + 1].GetComponent();
					myPath = m_pathfinding.FindPath(m_currentPrimitive, m_goal.PathfindingHeuristic, (MyNavigationPrimitive prim) => (!otherComponent.Contains(prim)) ? float.PositiveInfinity : 0f, (MyNavigationPrimitive prim) => component.Contains(prim) || otherComponent.Contains(prim));
				}
			}
			else if (m_pathNodes[m_pathNodePosition].IsExpanded)
			{
				IMyHighLevelComponent component2 = m_pathNodes[m_pathNodePosition].GetComponent();
				myPath = m_pathfinding.FindPath(m_currentPrimitive, m_goal.PathfindingHeuristic, (MyNavigationPrimitive prim) => (!component2.Contains(prim)) ? 30f : m_goal.TerminationCriterion(prim), (MyNavigationPrimitive prim) => component2.Contains(prim));
				if (myPath != null)
				{
					if (myPath.Count != 0 && component2.Contains(myPath[myPath.Count - 1].Vertex as MyNavigationPrimitive))
					{
						flag = true;
					}
					else
					{
						m_goal.IgnoreHighLevel(m_pathNodes[m_pathNodePosition]);
					}
				}
			}
			if (myPath == null || myPath.Count == 0)
			{
				return;
			}
			Vector3D vector3D = default(Vector3D);
			MyNavigationPrimitive myNavigationPrimitive = myPath[myPath.Count - 1].Vertex as MyNavigationPrimitive;
			if (flag)
			{
				Vector3 v = m_goal.Destination.GetBestPoint(myNavigationPrimitive.WorldPosition);
				Vector3 point = myNavigationPrimitive.Group.GlobalToLocal(v);
				point = myNavigationPrimitive.ProjectLocalPoint(point);
				vector3D = myNavigationPrimitive.Group.LocalToGlobal(point);
			}
			else
			{
				vector3D = myNavigationPrimitive.WorldPosition;
			}
			RefineFoundPath(ref currentPosition, ref vector3D, myPath);
			if (m_pathNodes.Count <= 1 && flag && m_expandedPath.Count > 0 && myPath.Count <= 2 && !m_goal.ShouldReinitPath())
			{
				Vector4D vector4D = m_expandedPath[m_expandedPath.Count - 1];
				if (Vector3D.DistanceSquared(currentPosition, vector3D) < vector4D.W * vector4D.W / 256.0)
				{
					m_expandedPath.Clear();
				}
			}
		}

		private bool ShouldReinitPath()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastInitTime < 1000)
			{
				return false;
			}
			return m_goal.ShouldReinitPath();
		}

		private void GenerateHighLevelPath()
		{
			ClearPathNodes();
			if (m_hlBegin != null)
			{
				MyPath<MyNavigationPrimitive> myPath = m_goal.FindHighLevelPath(m_pathfinding, m_hlBegin);
				if (myPath != null)
				{
					foreach (MyPath<MyNavigationPrimitive>.PathNode item in myPath)
					{
						MyHighLevelPrimitive myHighLevelPrimitive = item.Vertex as MyHighLevelPrimitive;
						m_pathNodes.Add(myHighLevelPrimitive);
						if (myHighLevelPrimitive != m_hlBegin)
						{
							myHighLevelPrimitive.Parent.ObservePrimitive(myHighLevelPrimitive, this);
						}
					}
					m_pathNodePosition = 0;
				}
			}
		}

		private void RefineFoundPath(ref Vector3D begin, ref Vector3D end, MyPath<MyNavigationPrimitive> path)
		{
			if (!MyPerGameSettings.EnablePathfinding || path == null)
			{
				return;
			}
			m_currentPrimitive = (path[path.Count - 1].Vertex as MyNavigationPrimitive);
			if (m_hlBegin != null && !m_pathNodes.Contains(m_hlBegin))
			{
				m_hlBegin.Parent.StopObservingPrimitive(m_hlBegin, this);
			}
			m_hlBegin = m_currentPrimitive.GetHighLevelPrimitive();
			if (m_hlBegin != null && !m_pathNodes.Contains(m_hlBegin))
			{
				m_hlBegin.Parent.ObservePrimitive(m_hlBegin, this);
			}
			IMyNavigationGroup myNavigationGroup = null;
			int begin2 = 0;
			int num = 0;
			Vector3 startPoint = default(Vector3);
			Vector3 vector = default(Vector3);
			for (int i = 0; i < path.Count; i++)
			{
				MyNavigationPrimitive myNavigationPrimitive = path[i].Vertex as MyNavigationPrimitive;
				IMyNavigationGroup group = myNavigationPrimitive.Group;
				if (myNavigationGroup == null)
				{
					myNavigationGroup = group;
					startPoint = myNavigationGroup.GlobalToLocal(begin);
				}
				bool flag = i == path.Count - 1;
				if (group != myNavigationGroup)
				{
					num = i - 1;
					vector = myNavigationGroup.GlobalToLocal(myNavigationPrimitive.WorldPosition);
				}
				else
				{
					if (!flag)
					{
						continue;
					}
					num = i;
					vector = myNavigationGroup.GlobalToLocal(end);
				}
				int count = m_expandedPath.Count;
				myNavigationGroup.RefinePath(path, m_expandedPath, ref startPoint, ref vector, begin2, num);
				int count2 = m_expandedPath.Count;
				for (int j = count; j < count2; j++)
				{
					Vector3D v = new Vector3D(m_expandedPath[j]);
					v = myNavigationGroup.LocalToGlobal(v);
					m_expandedPath[j] = new Vector4D(v, m_expandedPath[j].W);
				}
				if (flag && group != myNavigationGroup)
				{
					m_expandedPath.Add(new Vector4D(myNavigationPrimitive.WorldPosition, m_expandedPath[count2 - 1].W));
				}
				myNavigationGroup = group;
				begin2 = i;
				if (m_expandedPath.Count != 0)
				{
					startPoint = group.GlobalToLocal(new Vector3D(m_expandedPath[m_expandedPath.Count - 1]));
				}
			}
			m_pathNodePosition++;
			m_expandedPathPosition = 0;
		}

		private void ClearPathNodes()
		{
			foreach (MyHighLevelPrimitive pathNode in m_pathNodes)
			{
				if (pathNode != m_hlBegin)
				{
					pathNode.Parent.StopObservingPrimitive(pathNode, this);
				}
			}
			m_pathNodes.Clear();
			m_pathNodePosition = 0;
		}

		private void ClearFirstPathNode()
		{
			using (List<MyHighLevelPrimitive>.Enumerator enumerator = m_pathNodes.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					MyHighLevelPrimitive current = enumerator.Current;
					if (current != m_hlBegin)
					{
						current.Parent.StopObservingPrimitive(current, this);
					}
				}
			}
			m_pathNodes.RemoveAt(0);
			m_pathNodePosition--;
		}

		public void DebugDraw()
		{
			MatrixD viewMatrix = MySector.MainCamera.ViewMatrix;
			Vector3D? vector3D = null;
			foreach (MyHighLevelPrimitive pathNode in m_pathNodes)
			{
				Vector3D value = MyGravityProviderSystem.CalculateTotalGravityInPoint(pathNode.WorldPosition);
				if (Vector3D.IsZero(value, 0.001))
				{
					value = Vector3D.Down;
				}
				value.Normalize();
				Vector3D vector3D2 = pathNode.WorldPosition + value * -10.0;
				MyRenderProxy.DebugDrawSphere(vector3D2, 1f, Color.IndianRed, 1f, depthRead: false);
				MyRenderProxy.DebugDrawLine3D(pathNode.WorldPosition, vector3D2, Color.IndianRed, Color.IndianRed, depthRead: false);
				if (vector3D.HasValue)
				{
					MyRenderProxy.DebugDrawLine3D(vector3D2, vector3D.Value, Color.IndianRed, Color.IndianRed, depthRead: false);
				}
				vector3D = vector3D2;
			}
			MyRenderProxy.DebugDrawSphere(m_startPoint, 0.5f, Color.HotPink, 1f, depthRead: false);
			if (m_goal != null)
			{
				m_goal.DebugDraw();
			}
			if (!MyFakes.DEBUG_DRAW_FOUND_PATH)
			{
				return;
			}
			Vector3D? vector3D3 = null;
			for (int i = 0; i < m_expandedPath.Count; i++)
			{
				Vector3D vector3D4 = new Vector3D(m_expandedPath[i]);
				float num = (float)m_expandedPath[i].W;
				Color color = (i == m_expandedPath.Count - 1) ? Color.OrangeRed : Color.Orange;
				MyRenderProxy.DebugDrawPoint(vector3D4, color, depthRead: false);
				MyRenderProxy.DebugDrawText3D(vector3D4 + viewMatrix.Right * 0.10000000149011612, num.ToString(), color, 0.7f, depthRead: false);
				if (vector3D3.HasValue)
				{
					MyRenderProxy.DebugDrawLine3D(vector3D3.Value, vector3D4, Color.Pink, Color.Pink, depthRead: false);
				}
				vector3D3 = vector3D4;
			}
		}
	}
}
