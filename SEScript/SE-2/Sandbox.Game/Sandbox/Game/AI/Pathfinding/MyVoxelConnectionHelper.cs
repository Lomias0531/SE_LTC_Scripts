using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding
{
	public class MyVoxelConnectionHelper
	{
		private struct InnerEdgeIndex : IEquatable<InnerEdgeIndex>
		{
			public ushort V0;

			public ushort V1;

			public InnerEdgeIndex(ushort vert0, ushort vert1)
			{
				V0 = vert0;
				V1 = vert1;
			}

			public override int GetHashCode()
			{
				return V0 + V1 << 16;
			}

			public override bool Equals(object obj)
			{
				if (!(obj is InnerEdgeIndex))
				{
					return false;
				}
				return Equals((InnerEdgeIndex)obj);
			}

			public override string ToString()
			{
				return "{" + V0 + ", " + V1 + "}";
			}

			public bool Equals(InnerEdgeIndex other)
			{
				if (other.V0 == V0)
				{
					return other.V1 == V1;
				}
				return false;
			}
		}

		public struct OuterEdgePoint
		{
			public int EdgeIndex;

			public bool FirstPoint;

			public OuterEdgePoint(int edgeIndex, bool firstPoint)
			{
				EdgeIndex = edgeIndex;
				FirstPoint = firstPoint;
			}

			public override string ToString()
			{
				return "{" + EdgeIndex + (FirstPoint ? " O--->" : " <---O") + "}";
			}
		}

		private Dictionary<InnerEdgeIndex, int> m_innerEdges = new Dictionary<InnerEdgeIndex, int>();

		private MyVector3Grid<OuterEdgePoint> m_outerEdgePoints = new MyVector3Grid<OuterEdgePoint>(1f);

		private Dictionary<int, int> m_innerMultiedges = new Dictionary<int, int>();

		private Dictionary<InnerEdgeIndex, int> m_edgeClassifier = new Dictionary<InnerEdgeIndex, int>();

		private List<OuterEdgePoint> m_tmpOuterEdgePointList = new List<OuterEdgePoint>();

		public static float OUTER_EDGE_EPSILON = 0.05f;

		public static float OUTER_EDGE_EPSILON_SQ = OUTER_EDGE_EPSILON * OUTER_EDGE_EPSILON;

		public void ClearCell()
		{
			m_innerEdges.Clear();
			m_innerMultiedges.Clear();
			m_edgeClassifier.Clear();
		}

		public void PreprocessInnerEdge(ushort a, ushort b)
		{
			InnerEdgeIndex key = new InnerEdgeIndex(a, b);
			InnerEdgeIndex key2 = new InnerEdgeIndex(b, a);
			int value = (!m_edgeClassifier.TryGetValue(key, out value)) ? 1 : (value + 1);
			m_edgeClassifier[key] = value;
			value = (m_edgeClassifier.TryGetValue(key2, out value) ? (value - 1) : (-1));
			m_edgeClassifier[key2] = value;
		}

		public bool IsInnerEdge(ushort v0, ushort v1)
		{
			return IsInnerEdge(new InnerEdgeIndex(v0, v1));
		}

		private bool IsInnerEdge(InnerEdgeIndex edgeIndex)
		{
			return m_edgeClassifier[edgeIndex] == 0;
		}

		public int TryGetAndRemoveEdgeIndex(ushort iv0, ushort iv1, ref Vector3 posv0, ref Vector3 posv1)
		{
			int value = -1;
			InnerEdgeIndex innerEdgeIndex = new InnerEdgeIndex(iv0, iv1);
			if (IsInnerEdge(new InnerEdgeIndex(iv1, iv0)))
			{
				if (!m_innerEdges.TryGetValue(innerEdgeIndex, out value))
				{
					value = -1;
				}
				else
				{
					RemoveInnerEdge(value, innerEdgeIndex);
				}
			}
			else
			{
				TryRemoveOuterEdge(ref posv0, ref posv1, ref value);
			}
			return value;
		}

		public void AddEdgeIndex(ushort iv0, ushort iv1, ref Vector3 posv0, ref Vector3 posv1, int edgeIndex)
		{
			InnerEdgeIndex innerEdgeIndex = new InnerEdgeIndex(iv0, iv1);
			if (IsInnerEdge(innerEdgeIndex))
			{
				if (m_innerEdges.TryGetValue(innerEdgeIndex, out int value))
				{
					m_innerMultiedges.Add(edgeIndex, value);
					m_innerEdges[innerEdgeIndex] = edgeIndex;
				}
				else
				{
					m_innerEdges.Add(innerEdgeIndex, edgeIndex);
				}
			}
			else
			{
				AddOuterEdgeIndex(ref posv0, ref posv1, edgeIndex);
			}
		}

		public void AddOuterEdgeIndex(ref Vector3 posv0, ref Vector3 posv1, int edgeIndex)
		{
			m_outerEdgePoints.AddPoint(ref posv0, new OuterEdgePoint(edgeIndex, firstPoint: true));
			m_outerEdgePoints.AddPoint(ref posv1, new OuterEdgePoint(edgeIndex, firstPoint: false));
		}

		public void FixOuterEdge(int edgeIndex, bool firstPoint, Vector3 currentPosition)
		{
			new OuterEdgePoint(edgeIndex, firstPoint);
			MyVector3Grid<OuterEdgePoint>.SphereQuery sphereQuery = m_outerEdgePoints.QueryPointsSphere(ref currentPosition, OUTER_EDGE_EPSILON * 3f);
			while (sphereQuery.MoveNext())
			{
				if (sphereQuery.Current.EdgeIndex == edgeIndex && sphereQuery.Current.FirstPoint == firstPoint)
				{
					m_outerEdgePoints.MovePoint(sphereQuery.StorageIndex, ref currentPosition);
				}
			}
		}

		private InnerEdgeIndex RemoveInnerEdge(int formerEdgeIndex, InnerEdgeIndex innerIndex)
		{
			if (m_innerMultiedges.TryGetValue(formerEdgeIndex, out int value))
			{
				m_innerMultiedges.Remove(formerEdgeIndex);
				m_innerEdges[innerIndex] = value;
			}
			else
			{
				m_innerEdges.Remove(innerIndex);
			}
			return innerIndex;
		}

		public bool TryRemoveOuterEdge(ref Vector3 posv0, ref Vector3 posv1, ref int edgeIndex)
		{
			if (edgeIndex == -1)
			{
				MyVector3Grid<OuterEdgePoint>.SphereQuery en = m_outerEdgePoints.QueryPointsSphere(ref posv0, OUTER_EDGE_EPSILON);
				while (en.MoveNext())
				{
					MyVector3Grid<OuterEdgePoint>.SphereQuery en2 = m_outerEdgePoints.QueryPointsSphere(ref posv1, OUTER_EDGE_EPSILON);
					while (en2.MoveNext())
					{
						OuterEdgePoint current = en.Current;
						OuterEdgePoint current2 = en2.Current;
						if (current.EdgeIndex == current2.EdgeIndex && current.FirstPoint && !current2.FirstPoint)
						{
							edgeIndex = current.EdgeIndex;
							m_outerEdgePoints.RemoveTwo(ref en, ref en2);
							return true;
						}
					}
				}
				edgeIndex = -1;
			}
			else
			{
				int num = 0;
				MyVector3Grid<OuterEdgePoint>.SphereQuery en3 = m_outerEdgePoints.QueryPointsSphere(ref posv0, OUTER_EDGE_EPSILON);
				while (en3.MoveNext())
				{
					if (en3.Current.EdgeIndex == edgeIndex && en3.Current.FirstPoint)
					{
						num++;
						break;
					}
				}
				MyVector3Grid<OuterEdgePoint>.SphereQuery en4 = m_outerEdgePoints.QueryPointsSphere(ref posv1, OUTER_EDGE_EPSILON);
				while (en4.MoveNext())
				{
					if (en4.Current.EdgeIndex == edgeIndex && !en4.Current.FirstPoint)
					{
						num++;
						break;
					}
				}
				if (num == 2)
				{
					m_outerEdgePoints.RemoveTwo(ref en3, ref en4);
					return true;
				}
				edgeIndex = -1;
			}
			return false;
		}

		public void DebugDraw(ref Matrix drawMatrix, MyWingedEdgeMesh mesh)
		{
			Dictionary<Vector3I, int>.Enumerator enumerator = m_outerEdgePoints.EnumerateBins();
			int num = 0;
			BoundingBoxD aabb = default(BoundingBoxD);
			while (enumerator.MoveNext())
			{
				int binIndex = MyCestmirDebugInputComponent.BinIndex;
				if (binIndex == m_outerEdgePoints.InvalidIndex || num == binIndex)
				{
					Vector3I binPosition = enumerator.Current.Key;
					int num2 = enumerator.Current.Value;
					m_outerEdgePoints.GetLocalBinBB(ref binPosition, out BoundingBoxD output);
					aabb.Min = Vector3D.Transform(output.Min, drawMatrix);
					aabb.Max = Vector3D.Transform(output.Max, drawMatrix);
					while (num2 != m_outerEdgePoints.InvalidIndex)
					{
						Vector3 point = m_outerEdgePoints.GetPoint(num2);
						MyWingedEdgeMesh.Edge edge = mesh.GetEdge(m_outerEdgePoints.GetData(num2).EdgeIndex);
						Vector3 vertexPosition = mesh.GetVertexPosition(edge.Vertex1);
						Vector3 vertexPosition2 = mesh.GetVertexPosition(edge.Vertex2);
						Vector3D pointFrom = Vector3D.Transform((Vector3D)((vertexPosition + vertexPosition2) * 0.5f), drawMatrix);
						Vector3D pointTo = Vector3D.Transform((Vector3D)point, drawMatrix);
						MyRenderProxy.DebugDrawArrow3D(pointFrom, pointTo, Color.Yellow, Color.Yellow);
						num2 = m_outerEdgePoints.GetNextBinIndex(num2);
					}
					MyRenderProxy.DebugDrawAABB(aabb, Color.PowderBlue, 1f, 1f, depthRead: false);
				}
				num++;
			}
		}

		[Conditional("DEBUG")]
		public void CollectOuterEdges(List<MyTuple<OuterEdgePoint, Vector3>> output)
		{
			Dictionary<Vector3I, int>.Enumerator enumerator = m_outerEdgePoints.EnumerateBins();
			while (enumerator.MoveNext())
			{
				for (int num = enumerator.Current.Value; num != -1; num = m_outerEdgePoints.GetNextBinIndex(num))
				{
					output.Add(new MyTuple<OuterEdgePoint, Vector3>(m_outerEdgePoints.GetData(num), m_outerEdgePoints.GetPoint(num)));
				}
			}
		}
	}
}
