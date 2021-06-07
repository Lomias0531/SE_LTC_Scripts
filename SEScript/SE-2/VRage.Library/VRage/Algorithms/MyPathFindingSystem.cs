using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;

namespace VRage.Algorithms
{
	public class MyPathFindingSystem<V> : IEnumerable<V>, IEnumerable where V : class, IMyPathVertex<V>
	{
		public class Enumerator : IEnumerator<V>, IEnumerator, IDisposable
		{
			private V m_currentVertex;

			private MyPathFindingSystem<V> m_parent;

			private Predicate<V> m_vertexFilter;

			private Predicate<V> m_vertexTraversable;

			private Predicate<IMyPathEdge<V>> m_edgeTraversable;

			public V Current => m_currentVertex;

			object IEnumerator.Current => m_currentVertex;

			public void Init(MyPathFindingSystem<V> parent, V startingVertex, Predicate<V> vertexFilter = null, Predicate<V> vertexTraversable = null, Predicate<IMyPathEdge<V>> edgeTraversable = null)
			{
				m_parent = parent;
				m_vertexFilter = vertexFilter;
				m_vertexTraversable = vertexTraversable;
				m_edgeTraversable = edgeTraversable;
				m_parent.CalculateNextTimestamp();
				m_parent.m_enumerating = true;
				m_parent.m_bfsQueue.Enqueue(startingVertex);
				startingVertex.PathfindingData.Timestamp = m_parent.m_timestamp;
			}

			public void Dispose()
			{
				m_vertexFilter = null;
				m_currentVertex = null;
				m_edgeTraversable = null;
				m_vertexTraversable = null;
				m_parent.m_enumerating = false;
				m_parent.m_bfsQueue.Clear();
				m_parent = null;
			}

			public bool MoveNext()
			{
				while (m_parent.m_bfsQueue.Count() != 0)
				{
					m_currentVertex = m_parent.m_bfsQueue.Dequeue();
					V val = null;
					for (int i = 0; i < m_currentVertex.GetNeighborCount(); i++)
					{
						if (m_edgeTraversable == null)
						{
							val = (V)m_currentVertex.GetNeighbor(i);
							if (val == null)
							{
								continue;
							}
						}
						else
						{
							IMyPathEdge<V> edge = m_currentVertex.GetEdge(i);
							if (!m_edgeTraversable(edge))
							{
								continue;
							}
							val = edge.GetOtherVertex(m_currentVertex);
							if (val == null)
							{
								continue;
							}
						}
						if (val.PathfindingData.Timestamp != m_parent.m_timestamp && (m_vertexTraversable == null || m_vertexTraversable(val)))
						{
							m_parent.m_bfsQueue.Enqueue(val);
							val.PathfindingData.Timestamp = m_parent.m_timestamp;
						}
					}
					if (m_vertexFilter == null || m_vertexFilter(m_currentVertex))
					{
						return true;
					}
				}
				return false;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		private long m_timestamp;

		private Func<long> m_timestampFunction;

		private Queue<V> m_bfsQueue;

		private List<V> m_reachableList;

		private MyBinaryHeap<float, MyPathfindingData> m_openVertices;

		private Enumerator m_enumerator;

		private bool m_enumerating;

		public MyPathFindingSystem(int queueInitSize = 128, Func<long> timestampFunction = null)
		{
			m_bfsQueue = new Queue<V>(queueInitSize);
			m_reachableList = new List<V>(128);
			m_openVertices = new MyBinaryHeap<float, MyPathfindingData>(128);
			m_timestamp = 0L;
			m_timestampFunction = timestampFunction;
			m_enumerating = false;
			m_enumerator = new Enumerator();
		}

		protected void CalculateNextTimestamp()
		{
			if (m_timestampFunction != null)
			{
				m_timestamp = m_timestampFunction();
			}
			else
			{
				m_timestamp++;
			}
		}

		public MyPath<V> FindPath(V start, V end, Predicate<V> vertexTraversable = null, Predicate<IMyPathEdge<V>> edgeTraversable = null)
		{
			CalculateNextTimestamp();
			MyPathfindingData pathfindingData = start.PathfindingData;
			Visit(pathfindingData);
			pathfindingData.Predecessor = null;
			pathfindingData.PathLength = 0f;
			IMyPathVertex<V> myPathVertex = null;
			float num = float.PositiveInfinity;
			m_openVertices.Insert(start.PathfindingData, start.EstimateDistanceTo(end));
			while (m_openVertices.Count > 0)
			{
				MyPathfindingData myPathfindingData = m_openVertices.RemoveMin();
				V val = myPathfindingData.Parent as V;
				float pathLength = myPathfindingData.PathLength;
				if (myPathVertex != null && pathLength >= num)
				{
					break;
				}
				for (int i = 0; i < val.GetNeighborCount(); i++)
				{
					IMyPathEdge<V> edge = val.GetEdge(i);
					if (edge == null || (edgeTraversable != null && !edgeTraversable(edge)))
					{
						continue;
					}
					V otherVertex = edge.GetOtherVertex(val);
					if (otherVertex == null || (vertexTraversable != null && !vertexTraversable(otherVertex)))
					{
						continue;
					}
					float num2 = myPathfindingData.PathLength + edge.GetWeight();
					MyPathfindingData pathfindingData2 = otherVertex.PathfindingData;
					if (otherVertex == end && num2 < num)
					{
						myPathVertex = otherVertex;
						num = num2;
					}
					if (Visited(pathfindingData2))
					{
						if (num2 < pathfindingData2.PathLength)
						{
							pathfindingData2.PathLength = num2;
							pathfindingData2.Predecessor = myPathfindingData;
							m_openVertices.ModifyUp(pathfindingData2, num2 + otherVertex.EstimateDistanceTo(end));
						}
					}
					else
					{
						Visit(pathfindingData2);
						pathfindingData2.PathLength = num2;
						pathfindingData2.Predecessor = myPathfindingData;
						m_openVertices.Insert(pathfindingData2, num2 + otherVertex.EstimateDistanceTo(end));
					}
				}
			}
			m_openVertices.Clear();
			if (myPathVertex == null)
			{
				return null;
			}
			return ReturnPath(myPathVertex.PathfindingData, null, 0);
		}

		public MyPath<V> FindPath(V start, Func<V, float> heuristic, Func<V, float> terminationCriterion, Predicate<V> vertexTraversable = null, bool returnClosest = true)
		{
			CalculateNextTimestamp();
			MyPathfindingData pathfindingData = start.PathfindingData;
			Visit(pathfindingData);
			pathfindingData.Predecessor = null;
			pathfindingData.PathLength = 0f;
			IMyPathVertex<V> myPathVertex = null;
			float num = float.PositiveInfinity;
			IMyPathVertex<V> myPathVertex2 = null;
			float num2 = float.PositiveInfinity;
			float num3 = terminationCriterion(start);
			if (num3 != float.PositiveInfinity)
			{
				myPathVertex = start;
				num = heuristic(start) + num3;
			}
			m_openVertices.Insert(start.PathfindingData, heuristic(start));
			while (m_openVertices.Count > 0)
			{
				MyPathfindingData myPathfindingData = m_openVertices.RemoveMin();
				V val = myPathfindingData.Parent as V;
				float pathLength = myPathfindingData.PathLength;
				if (myPathVertex != null && pathLength + heuristic(val) >= num)
				{
					break;
				}
				for (int i = 0; i < val.GetNeighborCount(); i++)
				{
					IMyPathEdge<V> edge = val.GetEdge(i);
					if (edge == null)
					{
						continue;
					}
					V otherVertex = edge.GetOtherVertex(val);
					if (otherVertex == null || (vertexTraversable != null && !vertexTraversable(otherVertex)))
					{
						continue;
					}
					float num4 = myPathfindingData.PathLength + edge.GetWeight();
					MyPathfindingData pathfindingData2 = otherVertex.PathfindingData;
					float num5 = num4 + heuristic(otherVertex);
					if (num5 < num2)
					{
						myPathVertex2 = otherVertex;
						num2 = num5;
					}
					num3 = terminationCriterion(otherVertex);
					if (num5 + num3 < num)
					{
						myPathVertex = otherVertex;
						num = num5 + num3;
					}
					if (Visited(pathfindingData2))
					{
						if (num4 < pathfindingData2.PathLength)
						{
							pathfindingData2.PathLength = num4;
							pathfindingData2.Predecessor = myPathfindingData;
							m_openVertices.ModifyUp(pathfindingData2, num5);
						}
					}
					else
					{
						Visit(pathfindingData2);
						pathfindingData2.PathLength = num4;
						pathfindingData2.Predecessor = myPathfindingData;
						m_openVertices.Insert(pathfindingData2, num5);
					}
				}
			}
			m_openVertices.Clear();
			if (myPathVertex == null)
			{
				if (!returnClosest || myPathVertex2 == null)
				{
					return null;
				}
				return ReturnPath(myPathVertex2.PathfindingData, null, 0);
			}
			return ReturnPath(myPathVertex.PathfindingData, null, 0);
		}

		private MyPath<V> ReturnPath(MyPathfindingData vertexData, MyPathfindingData successor, int remainingVertices)
		{
			if (vertexData.Predecessor == null)
			{
				MyPath<V> myPath = new MyPath<V>(remainingVertices + 1);
				myPath.Add(vertexData.Parent as V, (successor != null) ? (successor.Parent as V) : null);
				return myPath;
			}
			MyPath<V> myPath2 = ReturnPath(vertexData.Predecessor, vertexData, remainingVertices + 1);
			myPath2.Add(vertexData.Parent as V, (successor != null) ? (successor.Parent as V) : null);
			return myPath2;
		}

		public bool Reachable(V from, V to, Predicate<V> vertexFilter = null, Predicate<V> vertexTraversable = null, Predicate<IMyPathEdge<V>> edgeTraversable = null)
		{
			PrepareTraversal(from, vertexFilter, vertexTraversable, edgeTraversable);
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					V current = enumerator.Current;
					if (current.Equals(to))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void FindReachable(IEnumerable<V> fromSet, List<V> reachableVertices, Predicate<V> vertexFilter = null, Predicate<V> vertexTraversable = null, Predicate<IMyPathEdge<V>> edgeTraversable = null)
		{
			CalculateNextTimestamp();
			foreach (V item in fromSet)
			{
				if (!Visited(item))
				{
					FindReachableInternal(item, reachableVertices, vertexFilter, vertexTraversable, edgeTraversable);
				}
			}
		}

		public void FindReachable(V from, List<V> reachableVertices, Predicate<V> vertexFilter = null, Predicate<V> vertexTraversable = null, Predicate<IMyPathEdge<V>> edgeTraversable = null)
		{
			FindReachableInternal(from, reachableVertices, vertexFilter, vertexTraversable, edgeTraversable);
		}

		public long GetCurrentTimestamp()
		{
			return m_timestamp;
		}

		public bool VisitedBetween(V vertex, long start, long end)
		{
			if (vertex.PathfindingData.Timestamp >= start)
			{
				return vertex.PathfindingData.Timestamp <= end;
			}
			return false;
		}

		private void FindReachableInternal(V from, List<V> reachableVertices, Predicate<V> vertexFilter = null, Predicate<V> vertexTraversable = null, Predicate<IMyPathEdge<V>> edgeTraversable = null)
		{
			PrepareTraversal(from, vertexFilter, vertexTraversable, edgeTraversable);
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					V current = enumerator.Current;
					reachableVertices.Add(current);
				}
			}
		}

		private void Visit(V vertex)
		{
			vertex.PathfindingData.Timestamp = m_timestamp;
		}

		private void Visit(MyPathfindingData vertexData)
		{
			vertexData.Timestamp = m_timestamp;
		}

		private bool Visited(V vertex)
		{
			return vertex.PathfindingData.Timestamp == m_timestamp;
		}

		private bool Visited(MyPathfindingData vertexData)
		{
			return vertexData.Timestamp == m_timestamp;
		}

		/// <summary>
		/// Has to be called before any traversal of the pathfinding system using enumerators.
		///
		/// Several predicates can be supplied to the system that change the behavior of the traversal.
		/// </summary>
		/// <param name="startingVertex">The vertex from which the traversal starts</param>
		/// <param name="vertexFilter">If set, this predicate is applied to the output vertices so that we only get those that we are interested in.</param>
		/// <param name="vertexTraversable">
		///     This predicate allows to make vertices of the graph untraversable, blocking the paths through them.
		///     It is guaranteed to be called only once on every vertex when enumerating the graph or finding reachable vertices, but
		///     for pathfinding functions, this guarantee is no longer valid.
		/// </param>
		/// <param name="edgeTraversable">This predicate allows to make edges untraversable, blocking the paths through them.</param>
		public void PrepareTraversal(V startingVertex, Predicate<V> vertexFilter = null, Predicate<V> vertexTraversable = null, Predicate<IMyPathEdge<V>> edgeTraversable = null)
		{
			m_enumerator.Init(this, startingVertex, vertexFilter, vertexTraversable, edgeTraversable);
		}

		public void PerformTraversal()
		{
			while (m_enumerator.MoveNext())
			{
			}
			m_enumerator.Dispose();
		}

		private Enumerator GetEnumeratorInternal()
		{
			return m_enumerator;
		}

		public Enumerator GetEnumerator()
		{
			return GetEnumeratorInternal();
		}

		IEnumerator<V> IEnumerable<V>.GetEnumerator()
		{
			return GetEnumeratorInternal();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumeratorInternal();
		}
	}
}
