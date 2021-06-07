using System;
using System.Collections.Generic;
using VRageMath;

namespace VRageRender.Utils
{
	public class MyPolygon
	{
		public struct Vertex
		{
			public Vector3 Coord;

			public int Prev;

			public int Next;
		}

		public struct LoopIterator
		{
			private List<Vertex> m_data;

			private int m_begin;

			private int m_currentIndex;

			private Vertex m_current;

			public Vector3 Current => m_current.Coord;

			public int CurrentIndex => m_currentIndex;

			public LoopIterator(MyPolygon poly, int loopBegin)
			{
				m_data = poly.m_vertices;
				m_begin = loopBegin;
				m_currentIndex = -1;
				m_current = default(Vertex);
				m_current.Next = m_begin;
			}

			public bool MoveNext()
			{
				if (m_currentIndex != -1 && m_current.Next == m_begin)
				{
					return false;
				}
				m_currentIndex = m_current.Next;
				m_current = m_data[m_currentIndex];
				return true;
			}
		}

		private List<Vertex> m_vertices;

		private List<int> m_loops;

		private Plane m_plane;

		public int VertexCount => m_vertices.Count;

		public int LoopCount => m_loops.Count;

		public Plane PolygonPlane => m_plane;

		public MyPolygon(Plane polygonPlane)
		{
			m_vertices = new List<Vertex>();
			m_loops = new List<int>();
			m_plane = polygonPlane;
		}

		public void Transform(ref Matrix transformationMatrix)
		{
			for (int i = 0; i < m_vertices.Count; i++)
			{
				Vertex value = m_vertices[i];
				Vector3.Transform(ref value.Coord, ref transformationMatrix, out value.Coord);
				m_vertices[i] = value;
			}
		}

		public LoopIterator GetLoopIterator(int loopIndex)
		{
			return new LoopIterator(this, m_loops[loopIndex]);
		}

		public int GetLoopStart(int loopIndex)
		{
			return m_loops[loopIndex];
		}

		public void GetVertex(int vertexIndex, out Vertex v)
		{
			v = m_vertices[vertexIndex];
		}

		public void GetXExtents(out float minX, out float maxX)
		{
			minX = float.PositiveInfinity;
			maxX = float.NegativeInfinity;
			for (int i = 0; i < m_vertices.Count; i++)
			{
				float x = m_vertices[i].Coord.X;
				minX = Math.Min(minX, x);
				maxX = Math.Max(maxX, x);
			}
		}

		public void AddLoop(List<Vector3> loop)
		{
			if (loop.Count >= 3)
			{
				for (int i = 0; i < loop.Count; i++)
				{
				}
				int count = m_vertices.Count;
				int prev = m_vertices.Count + loop.Count - 1;
				m_loops.Add(count);
				Vertex item;
				for (int j = 0; j < loop.Count - 1; j++)
				{
					List<Vertex> vertices = m_vertices;
					item = new Vertex
					{
						Coord = loop[j],
						Next = count + j + 1,
						Prev = prev
					};
					vertices.Add(item);
					prev = count + j;
				}
				List<Vertex> vertices2 = m_vertices;
				item = new Vertex
				{
					Coord = loop[loop.Count - 1],
					Next = count,
					Prev = prev
				};
				vertices2.Add(item);
			}
		}

		public void Clear()
		{
			m_vertices.Clear();
			m_loops.Clear();
		}

		public void DebugDraw(ref MatrixD drawMatrix)
		{
			for (int i = 0; i < m_vertices.Count; i++)
			{
				MyRenderProxy.DebugDrawLine3D(m_vertices[i].Coord, m_vertices[m_vertices[i].Next].Coord, Color.DarkRed, Color.DarkRed, depthRead: false);
				MyRenderProxy.DebugDrawPoint(m_vertices[i].Coord, Color.Red, depthRead: false);
				MyRenderProxy.DebugDrawText3D(m_vertices[i].Coord + Vector3.Right * 0.05f, i + "/" + m_vertices.Count, Color.Red, 0.45f, depthRead: false);
			}
		}
	}
}
