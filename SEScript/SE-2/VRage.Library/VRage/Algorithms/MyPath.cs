using System.Collections;
using System.Collections.Generic;

namespace VRage.Algorithms
{
	public class MyPath<V> : IEnumerable<MyPath<V>.PathNode>, IEnumerable where V : class, IMyPathVertex<V>, IEnumerable<IMyPathEdge<V>>
	{
		public struct PathNode
		{
			public IMyPathVertex<V> Vertex;

			public int nextVertex;
		}

		private List<PathNode> m_vertices;

		public int Count => m_vertices.Count;

		public PathNode this[int position]
		{
			get
			{
				return m_vertices[position];
			}
			set
			{
				m_vertices[position] = value;
			}
		}

		internal MyPath(int size)
		{
			m_vertices = new List<PathNode>(size);
		}

		internal void Add(IMyPathVertex<V> vertex, IMyPathVertex<V> nextVertex)
		{
			PathNode item = default(PathNode);
			item.Vertex = vertex;
			if (nextVertex == null)
			{
				m_vertices.Add(item);
				return;
			}
			int neighborCount = vertex.GetNeighborCount();
			int num = 0;
			while (true)
			{
				if (num < neighborCount)
				{
					IMyPathVertex<V> neighbor = vertex.GetNeighbor(num);
					if (neighbor == nextVertex)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			item.nextVertex = num;
			m_vertices.Add(item);
		}

		public IEnumerator<PathNode> GetEnumerator()
		{
			return m_vertices.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_vertices.GetEnumerator();
		}
	}
}
