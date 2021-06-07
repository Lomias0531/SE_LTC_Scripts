using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRageMath;

namespace VRageRender.Utils
{
	public class MyPolygonBoolOps
	{
		private enum ClassificationIndex : byte
		{
			LEFT_SUBJECT_AND_LEFT_SUBJECT,
			LEFT_SUBJECT_AND_LEFT_CLIP,
			LEFT_SUBJECT_AND_RIGHT_SUBJECT,
			LEFT_SUBJECT_AND_RIGHT_CLIP,
			LEFT_CLIP_AND_LEFT_SUBJECT,
			LEFT_CLIP_AND_LEFT_CLIP,
			LEFT_CLIP_AND_RIGHT_SUBJECT,
			LEFT_CLIP_AND_RIGHT_CLIP,
			RIGHT_SUBJECT_AND_LEFT_SUBJECT,
			RIGHT_SUBJECT_AND_LEFT_CLIP,
			RIGHT_SUBJECT_AND_RIGHT_SUBJECT,
			RIGHT_SUBJECT_AND_RIGHT_CLIP,
			RIGHT_CLIP_AND_LEFT_SUBJECT,
			RIGHT_CLIP_AND_LEFT_CLIP,
			RIGHT_CLIP_AND_RIGHT_SUBJECT,
			RIGHT_CLIP_AND_RIGHT_CLIP
		}

		private enum IntersectionClassification : byte
		{
			INVALID,
			LIKE_INTERSECTION,
			LOCAL_MINIMUM,
			LOCAL_MAXIMUM,
			LEFT_E1_INTERMEDIATE,
			RIGHT_E1_INTERMEDIATE,
			LEFT_E2_INTERMEDIATE,
			RIGHT_E2_INTERMEDIATE
		}

		private class Operation
		{
			private IntersectionClassification[] m_classificationTable;

			private bool m_sParityForContribution;

			private bool m_cParityForContribution;

			public bool SubjectInvert
			{
				get;
				private set;
			}

			public bool ClipInvert
			{
				get;
				private set;
			}

			public Operation(IntersectionClassification[] classificationTable, bool subjectContributingParity, bool clipContributingParity, bool invertSubjectSides, bool invertClipSides)
			{
				m_classificationTable = classificationTable;
				m_sParityForContribution = subjectContributingParity;
				m_cParityForContribution = clipContributingParity;
				SubjectInvert = invertSubjectSides;
				ClipInvert = invertClipSides;
			}

			public IntersectionClassification ClassifyIntersection(Side side1, PolygonType type1, Side side2, PolygonType type2)
			{
				return m_classificationTable[EncodeClassificationIndex(side1, type1, side2, type2)];
			}

			public bool InitializeContributing(bool parity, PolygonType type)
			{
				if (type == PolygonType.SUBJECT)
				{
					if (parity)
					{
						return m_sParityForContribution;
					}
					return !m_sParityForContribution;
				}
				if (parity)
				{
					return m_cParityForContribution;
				}
				return !m_cParityForContribution;
			}
		}

		private struct BoundPair
		{
			public MyPolygon Parent;

			public int Left;

			public int Minimum;

			public int Right;

			public bool RightIsPrecededByHorizontal;

			private float m_minimumCoordinate;

			public float MinimumCoordinate => m_minimumCoordinate;

			public BoundPair(MyPolygon parent, int left, int minimum, int right, bool rightHorizontal)
			{
				Parent = parent;
				Left = left;
				Minimum = minimum;
				Right = right;
				RightIsPrecededByHorizontal = rightHorizontal;
				m_minimumCoordinate = 0f;
			}

			public bool IsValid()
			{
				if (Parent != null && Left != -1 && Right != -1 && Minimum != -1 && Left != Minimum && Right != Minimum)
				{
					return !float.IsNaN(MinimumCoordinate);
				}
				return false;
			}

			public void CalculateMinimumCoordinate()
			{
				Parent.GetVertex(Minimum, out MyPolygon.Vertex v);
				m_minimumCoordinate = v.Coord.Y;
			}
		}

		private struct SortedEdgeEntry
		{
			public int Index;

			public float XCoord;

			public float DX;

			public PolygonType Kind;

			public float QNumerator;
		}

		private struct IntersectionListEntry
		{
			public int LIndex;

			public int RIndex;

			public float X;

			public float Y;
		}

		private enum PolygonType : byte
		{
			SUBJECT,
			CLIP
		}

		private enum Side : byte
		{
			LEFT = 0,
			RIGHT = 2
		}

		private class PartialPolygon
		{
			private List<Vector3> m_vertices = new List<Vector3>();

			public void Clear()
			{
				m_vertices.Clear();
			}

			public void Append(Vector3 newVertex)
			{
				int count = m_vertices.Count;
				if (count == 0 || Vector3.DistanceSquared(newVertex, m_vertices[count - 1]) > 1E-06f)
				{
					m_vertices.Add(newVertex);
				}
			}

			public void Prepend(Vector3 newVertex)
			{
				if (m_vertices.Count == 0 || Vector3.DistanceSquared(newVertex, m_vertices[0]) > 1E-06f)
				{
					m_vertices.Insert(0, newVertex);
				}
			}

			public void Reverse()
			{
				m_vertices.Reverse();
			}

			public void Add(PartialPolygon other)
			{
				if (other.m_vertices.Count != 0)
				{
					Append(other.m_vertices[0]);
					for (int i = 1; i < other.m_vertices.Count; i++)
					{
						m_vertices.Add(other.m_vertices[i]);
					}
				}
			}

			public List<Vector3> GetLoop()
			{
				return m_vertices;
			}

			public void Postprocess()
			{
				if (m_vertices.Count >= 3 && Vector3.DistanceSquared(m_vertices[m_vertices.Count - 1], m_vertices[0]) <= 1E-06f)
				{
					m_vertices.RemoveAt(m_vertices.Count - 1);
				}
				if (m_vertices.Count < 3)
				{
					m_vertices.Clear();
				}
			}
		}

		private struct Edge
		{
			public int BoundPairIndex;

			public Side BoundPairSide;

			public int TopVertexIndex;

			public float BottomX;

			public float TopY;

			public float DXdy;

			public PolygonType Kind;

			public Side OutputSide;

			public bool Contributing;

			public PartialPolygon AssociatedPolygon;

			public float CalculateX(float dy)
			{
				return BottomX + DXdy * dy;
			}

			public float CalculateQNumerator(float bottom, float top, float topX)
			{
				return BottomX * top - topX * bottom;
			}
		}

		private static Operation m_operationIntersection;

		private static Operation m_operationUnion;

		private static Operation m_operationDifference;

		private static MyPolygonBoolOps m_static;

		private MyPolygon m_polyA = new MyPolygon(new Plane(Vector3.Forward, 0f));

		private MyPolygon m_polyB = new MyPolygon(new Plane(Vector3.Forward, 0f));

		private Operation m_operation;

		private List<BoundPair> m_boundsA = new List<BoundPair>();

		private List<BoundPair> m_boundsB = new List<BoundPair>();

		private List<BoundPair> m_usedBoundPairs = new List<BoundPair>();

		private List<float> m_scanBeamList = new List<float>();

		private List<float> m_horizontalScanBeamList = new List<float>();

		private List<BoundPair> m_localMinimaList = new List<BoundPair>();

		private List<Edge> m_activeEdgeList = new List<Edge>();

		private List<Vector3> m_tmpList = new List<Vector3>();

		private List<SortedEdgeEntry> m_sortedEdgeList = new List<SortedEdgeEntry>();

		private List<IntersectionListEntry> m_intersectionList = new List<IntersectionListEntry>();

		private List<int> m_edgePositionInfo = new List<int>();

		private List<PartialPolygon> m_results = new List<PartialPolygon>();

		private Matrix m_projectionTransform;

		private Matrix m_invProjectionTransform;

		private Plane m_projectionPlane;

		public static MyPolygonBoolOps Static
		{
			get
			{
				if (m_static == null)
				{
					m_static = new MyPolygonBoolOps();
				}
				return m_static;
			}
		}

		[Conditional("DEBUG")]
		private static void CheckClassificationIndices()
		{
		}

		private static int EncodeClassificationIndex(Side side1, PolygonType type1, Side side2, PolygonType type2)
		{
			return ((int)side1 + (int)type1 << 2) + (int)side2 + (int)type2;
		}

		private static void InitializeOperations()
		{
			_ = new IntersectionClassification[16];
			IntersectionClassification[] array = new IntersectionClassification[16];
			InitClassificationTable(array);
			array[7] = IntersectionClassification.LIKE_INTERSECTION;
			array[13] = IntersectionClassification.LIKE_INTERSECTION;
			array[2] = IntersectionClassification.LIKE_INTERSECTION;
			array[8] = IntersectionClassification.LIKE_INTERSECTION;
			array[4] = IntersectionClassification.LEFT_E2_INTERMEDIATE;
			array[1] = IntersectionClassification.LEFT_E2_INTERMEDIATE;
			array[14] = IntersectionClassification.RIGHT_E1_INTERMEDIATE;
			array[11] = IntersectionClassification.RIGHT_E1_INTERMEDIATE;
			array[3] = IntersectionClassification.LOCAL_MAXIMUM;
			array[6] = IntersectionClassification.LOCAL_MAXIMUM;
			array[9] = IntersectionClassification.LOCAL_MINIMUM;
			array[12] = IntersectionClassification.LOCAL_MINIMUM;
			m_operationIntersection = new Operation(array, subjectContributingParity: true, clipContributingParity: true, invertSubjectSides: false, invertClipSides: false);
			IntersectionClassification[] array2 = new IntersectionClassification[16];
			InitClassificationTable(array2);
			array2[7] = IntersectionClassification.LIKE_INTERSECTION;
			array2[13] = IntersectionClassification.LIKE_INTERSECTION;
			array2[2] = IntersectionClassification.LIKE_INTERSECTION;
			array2[8] = IntersectionClassification.LIKE_INTERSECTION;
			array2[4] = IntersectionClassification.LEFT_E1_INTERMEDIATE;
			array2[1] = IntersectionClassification.LEFT_E1_INTERMEDIATE;
			array2[14] = IntersectionClassification.RIGHT_E2_INTERMEDIATE;
			array2[11] = IntersectionClassification.RIGHT_E2_INTERMEDIATE;
			array2[3] = IntersectionClassification.LOCAL_MINIMUM;
			array2[6] = IntersectionClassification.LOCAL_MINIMUM;
			array2[9] = IntersectionClassification.LOCAL_MAXIMUM;
			array2[12] = IntersectionClassification.LOCAL_MAXIMUM;
			m_operationUnion = new Operation(array2, subjectContributingParity: false, clipContributingParity: false, invertSubjectSides: false, invertClipSides: false);
			IntersectionClassification[] array3 = new IntersectionClassification[16];
			InitClassificationTable(array3);
			array3[7] = IntersectionClassification.LIKE_INTERSECTION;
			array3[13] = IntersectionClassification.LIKE_INTERSECTION;
			array3[2] = IntersectionClassification.LIKE_INTERSECTION;
			array3[8] = IntersectionClassification.LIKE_INTERSECTION;
			array3[4] = IntersectionClassification.LEFT_E2_INTERMEDIATE;
			array3[1] = IntersectionClassification.LEFT_E2_INTERMEDIATE;
			array3[14] = IntersectionClassification.RIGHT_E1_INTERMEDIATE;
			array3[11] = IntersectionClassification.RIGHT_E1_INTERMEDIATE;
			array3[3] = IntersectionClassification.LOCAL_MAXIMUM;
			array3[6] = IntersectionClassification.LOCAL_MAXIMUM;
			array3[9] = IntersectionClassification.LOCAL_MINIMUM;
			array3[12] = IntersectionClassification.LOCAL_MINIMUM;
			m_operationDifference = new Operation(array3, subjectContributingParity: false, clipContributingParity: true, invertSubjectSides: false, invertClipSides: true);
		}

		private static void InitClassificationTable(IntersectionClassification[] m_classificationTable)
		{
			for (int i = 0; i < 16; i++)
			{
				m_classificationTable[i] = IntersectionClassification.INVALID;
			}
		}

		private static Side OtherSide(Side side)
		{
			if (side == Side.LEFT)
			{
				return Side.RIGHT;
			}
			return Side.LEFT;
		}

		static MyPolygonBoolOps()
		{
			InitializeOperations();
		}

		public MyPolygon Intersection(MyPolygon polyA, MyPolygon polyB)
		{
			return PerformBooleanOperation(polyA, polyB, m_operationIntersection);
		}

		public MyPolygon Union(MyPolygon polyA, MyPolygon polyB)
		{
			return PerformBooleanOperation(polyA, polyB, m_operationUnion);
		}

		public MyPolygon Difference(MyPolygon polyA, MyPolygon polyB)
		{
			return PerformBooleanOperation(polyA, polyB, m_operationDifference);
		}

		private MyPolygon PerformBooleanOperation(MyPolygon polyA, MyPolygon polyB, Operation operation)
		{
			Clear();
			PrepareTransforms(polyA);
			ProjectPoly(polyA, m_polyA, ref m_projectionTransform);
			ProjectPoly(polyB, m_polyB, ref m_projectionTransform);
			m_operation = operation;
			PerformInPlane();
			m_operation = null;
			return UnprojectResult();
		}

		private void Clear()
		{
			m_polyA.Clear();
			m_polyB.Clear();
			m_boundsA.Clear();
			m_boundsB.Clear();
			m_usedBoundPairs.Clear();
			m_scanBeamList.Clear();
			m_horizontalScanBeamList.Clear();
			m_localMinimaList.Clear();
			m_activeEdgeList.Clear();
			m_results.Clear();
		}

		private void PrepareTransforms(MyPolygon polyA)
		{
			m_projectionPlane = polyA.PolygonPlane;
			Vector3 position = -polyA.PolygonPlane.Normal * polyA.PolygonPlane.D;
			Vector3 normal = polyA.PolygonPlane.Normal;
			Vector3 up = Vector3.Cross(Vector3.CalculatePerpendicularVector(normal), normal);
			m_invProjectionTransform = Matrix.CreateWorld(position, normal, up);
			Matrix.Invert(ref m_invProjectionTransform, out m_projectionTransform);
		}

		private void ProjectPoly(MyPolygon input, MyPolygon output, ref Matrix projection)
		{
			for (int i = 0; i < input.LoopCount; i++)
			{
				m_tmpList.Clear();
				MyPolygon.LoopIterator loopIterator = input.GetLoopIterator(i);
				while (loopIterator.MoveNext())
				{
					Vector3 item = Vector3.Transform(loopIterator.Current, projection);
					m_tmpList.Add(item);
				}
				output.AddLoop(m_tmpList);
			}
		}

		private void PerformInPlane()
		{
			ConstructBoundPairs(m_polyA, m_boundsA);
			ConstructBoundPairs(m_polyB, m_boundsB);
			AddBoundPairsToLists(m_boundsA);
			AddBoundPairsToLists(m_boundsB);
			float num = m_scanBeamList[m_scanBeamList.Count - 1];
			m_scanBeamList.RemoveAt(m_scanBeamList.Count - 1);
			do
			{
				AddBoundPairsToActiveEdges(num);
				float num2 = m_scanBeamList[m_scanBeamList.Count - 1];
				m_scanBeamList.RemoveAt(m_scanBeamList.Count - 1);
				if (num == num2)
				{
					ProcessHorizontalLine(num);
					if (m_scanBeamList.Count == 0)
					{
						break;
					}
					num2 = m_scanBeamList[m_scanBeamList.Count - 1];
					m_scanBeamList.RemoveAt(m_scanBeamList.Count - 1);
				}
				ProcessIntersections(num, num2);
				UpdateActiveEdges(num, num2);
				num = num2;
			}
			while (m_scanBeamList.Count > 0);
			m_scanBeamList.Clear();
		}

		private void ProcessHorizontalLine(float bottomY)
		{
			float endX = float.PositiveInfinity;
			PolygonType endType = PolygonType.SUBJECT;
			int from = 0;
			for (int i = 0; i < m_activeEdgeList.Count; i++)
			{
				int num = i;
				Edge e = m_activeEdgeList[num];
				while (endX < e.BottomX || (endX == e.BottomX && endType == PolygonType.CLIP && e.Kind == PolygonType.SUBJECT))
				{
					ProcessOldHorizontalEdges(bottomY, ref endX, ref endType, ref from, i);
				}
				if (e.TopY == bottomY)
				{
					if (e.DXdy == 0f)
					{
						for (int num2 = i - 1; num2 >= from; num2--)
						{
							Edge e2 = m_activeEdgeList[num2];
							if (e2.Kind == e.Kind && e2.TopVertexIndex == e.TopVertexIndex)
							{
								if (e2.Contributing)
								{
									AddLocalMaximum(num2, num, ref e2, ref e, new Vector3(e.BottomX, bottomY, 0f));
								}
								m_activeEdgeList.RemoveAt(num);
								m_activeEdgeList.RemoveAt(num2);
								i -= 2;
								break;
							}
							Vector3 intersectionPosition = new Vector3(e.BottomX, bottomY, 0f);
							IntersectionClassification intersectionClassification = m_operation.ClassifyIntersection(e2.OutputSide, e2.Kind, e.OutputSide, e.Kind);
							PerformIntersection(num2, num, ref e2, ref e, ref intersectionPosition, intersectionClassification);
							num = num2;
						}
					}
					else
					{
						float num3 = e.BottomX + e.DXdy;
						if (num3 < endX || (num3 == endX && e.Kind == PolygonType.CLIP && endType == PolygonType.SUBJECT))
						{
							endX = num3;
							endType = e.Kind;
						}
					}
				}
				else
				{
					for (int num4 = i - 1; num4 >= from; num4--)
					{
						Edge e3 = m_activeEdgeList[num4];
						Vector3 intersectionPosition2 = new Vector3(e.BottomX, bottomY, 0f);
						IntersectionClassification intersectionClassification2 = m_operation.ClassifyIntersection(e3.OutputSide, e3.Kind, e.OutputSide, e.Kind);
						PerformIntersection(num4, num, ref e3, ref e, ref intersectionPosition2, intersectionClassification2);
						num = num4;
					}
					from++;
				}
			}
			while (from < m_activeEdgeList.Count)
			{
				ProcessOldHorizontalEdges(bottomY, ref endX, ref endType, ref from, m_activeEdgeList.Count);
			}
		}

		private void ProcessOldHorizontalEdges(float bottomY, ref float endX, ref PolygonType endType, ref int from, int to)
		{
			float num = endX;
			PolygonType polygonType = endType;
			endX = float.PositiveInfinity;
			endType = PolygonType.SUBJECT;
			for (int i = from; i < to; i++)
			{
				Edge edge = m_activeEdgeList[i];
				if (edge.BottomX + edge.DXdy == num && edge.Kind == polygonType)
				{
					BoundPair boundPair = m_usedBoundPairs[edge.BoundPairIndex];
					boundPair.Parent.GetVertex(edge.TopVertexIndex, out MyPolygon.Vertex v);
					Vector3 coord = v.Coord;
					if (edge.Contributing)
					{
						if (edge.OutputSide == Side.LEFT)
						{
							edge.AssociatedPolygon.Append(coord);
						}
						else
						{
							edge.AssociatedPolygon.Prepend(coord);
						}
					}
					MyPolygon.Vertex v2;
					if (edge.BoundPairSide == Side.LEFT)
					{
						boundPair.Parent.GetVertex(v.Next, out v2);
					}
					else
					{
						boundPair.Parent.GetVertex(v.Prev, out v2);
					}
					RecalculateActiveEdge(ref edge, ref v, ref v2, edge.BoundPairSide);
					m_activeEdgeList[i] = edge;
					if (edge.TopY == bottomY && edge.DXdy != 0f)
					{
						float num2 = edge.BottomX + edge.DXdy;
						if (num2 < endX || (num2 == endX && edge.Kind == PolygonType.CLIP && endType == PolygonType.SUBJECT))
						{
							endX = num2;
							endType = edge.Kind;
						}
						continue;
					}
					int rightEdgeIndex = i;
					for (int num3 = i - 1; num3 >= from; num3--)
					{
						Edge e = m_activeEdgeList[num3];
						Vector3 intersectionPosition = new Vector3(edge.BottomX, bottomY, 0f);
						IntersectionClassification intersectionClassification = m_operation.ClassifyIntersection(e.OutputSide, e.Kind, edge.OutputSide, edge.Kind);
						PerformIntersection(num3, rightEdgeIndex, ref e, ref edge, ref intersectionPosition, intersectionClassification);
						rightEdgeIndex = num3;
					}
					from++;
				}
				else
				{
					float num4 = edge.BottomX + edge.DXdy;
					if (num4 < endX || (num4 == endX && edge.Kind == PolygonType.CLIP && endType == PolygonType.SUBJECT))
					{
						endX = num4;
						endType = edge.Kind;
					}
				}
			}
		}

		private void UpdateActiveEdges(float bottomY, float topY)
		{
			if (m_activeEdgeList.Count == 0)
			{
				return;
			}
			float dy = topY - bottomY;
			int num = 0;
			while (num < m_activeEdgeList.Count)
			{
				Edge edge = m_activeEdgeList[num];
				if (edge.TopY == topY)
				{
					BoundPair boundPair = m_usedBoundPairs[edge.BoundPairIndex];
					bool flag = false;
					boundPair.Parent.GetVertex(edge.TopVertexIndex, out MyPolygon.Vertex v);
					Vector3 coord = v.Coord;
					if (edge.TopVertexIndex == ((edge.BoundPairSide == Side.LEFT) ? boundPair.Left : boundPair.Right))
					{
						flag = true;
					}
					else
					{
						if (edge.Contributing)
						{
							if (edge.OutputSide == Side.LEFT)
							{
								edge.AssociatedPolygon.Append(coord);
							}
							else
							{
								edge.AssociatedPolygon.Prepend(coord);
							}
						}
						MyPolygon.Vertex v2;
						if (edge.BoundPairSide == Side.LEFT)
						{
							boundPair.Parent.GetVertex(v.Next, out v2);
						}
						else
						{
							boundPair.Parent.GetVertex(v.Prev, out v2);
						}
						RecalculateActiveEdge(ref edge, ref v, ref v2, edge.BoundPairSide);
					}
					if (flag)
					{
						if (edge.BoundPairSide != Side.RIGHT || !m_usedBoundPairs[edge.BoundPairIndex].RightIsPrecededByHorizontal)
						{
							int num2 = -1;
							Edge e = default(Edge);
							for (int i = num + 1; i < m_activeEdgeList.Count; i++)
							{
								e = m_activeEdgeList[i];
								if (e.Kind == edge.Kind && e.TopVertexIndex == edge.TopVertexIndex)
								{
									num2 = i;
									break;
								}
							}
							if (edge.Contributing && e.Contributing)
							{
								AddLocalMaximum(num, num2, ref edge, ref e, coord);
							}
							m_activeEdgeList.RemoveAt(num2);
							m_activeEdgeList.RemoveAt(num);
							continue;
						}
						edge.BottomX = edge.CalculateX(dy);
						edge.TopY = topY;
						edge.DXdy = 0f;
						m_activeEdgeList[num] = edge;
					}
					else
					{
						m_activeEdgeList[num] = edge;
					}
				}
				else
				{
					edge.BottomX = edge.CalculateX(dy);
					m_activeEdgeList[num] = edge;
				}
				num++;
			}
		}

		private void ProcessIntersections(float bottom, float top)
		{
			if (m_activeEdgeList.Count != 0)
			{
				BuildIntersectionList(bottom, top);
				ProcessIntersectionList();
			}
		}

		private void BuildIntersectionList(float bottom, float top)
		{
			float num = top - bottom;
			float num2 = 1f / num;
			SortedEdgeEntry entry = default(SortedEdgeEntry);
			GetSortedEdgeEntry(bottom, top, num, 0, ref entry);
			m_sortedEdgeList.Add(entry);
			for (int i = 1; i < m_activeEdgeList.Count; i++)
			{
				GetSortedEdgeEntry(bottom, top, num, i, ref entry);
				int num3;
				for (num3 = m_sortedEdgeList.Count - 1; num3 >= 0; num3--)
				{
					SortedEdgeEntry entry2 = m_sortedEdgeList[num3];
					if (CompareSortedEdgeEntries(ref entry2, ref entry) == -1)
					{
						break;
					}
					float num4 = 1f / (entry.DX - entry2.DX);
					float num5 = (entry2.QNumerator - entry.QNumerator) * num4;
					float x = entry.DX * num2 * num5 + entry.QNumerator * num2;
					IntersectionListEntry intersection = default(IntersectionListEntry);
					intersection.RIndex = entry.Index;
					intersection.LIndex = entry2.Index;
					intersection.X = x;
					intersection.Y = num5;
					InsertIntersection(ref intersection);
				}
				m_sortedEdgeList.Insert(num3 + 1, entry);
			}
			m_sortedEdgeList.Clear();
		}

		private SortedEdgeEntry GetSortedEdgeEntry(float bottom, float top, float dy, int i, ref SortedEdgeEntry entry)
		{
			Edge edge = m_activeEdgeList[i];
			entry.Index = i;
			if (edge.TopY == top)
			{
				MyPolygon.Vertex v;
				if (edge.Kind == PolygonType.SUBJECT)
				{
					m_polyA.GetVertex(edge.TopVertexIndex, out v);
				}
				else
				{
					m_polyB.GetVertex(edge.TopVertexIndex, out v);
				}
				entry.XCoord = v.Coord.X;
			}
			else
			{
				entry.XCoord = edge.CalculateX(dy);
			}
			entry.DX = entry.XCoord - edge.BottomX;
			entry.Kind = edge.Kind;
			entry.QNumerator = edge.CalculateQNumerator(bottom, top, entry.XCoord);
			return entry;
		}

		private void InsertIntersection(ref IntersectionListEntry intersection)
		{
			for (int i = 0; i < m_intersectionList.Count; i++)
			{
				if (m_intersectionList[i].Y > intersection.Y)
				{
					m_intersectionList.Insert(i, intersection);
					return;
				}
			}
			m_intersectionList.Add(intersection);
		}

		private void ProcessIntersectionList()
		{
			InitializeEdgePositions();
			for (int i = 0; i < m_intersectionList.Count; i++)
			{
				IntersectionListEntry intersectionListEntry = m_intersectionList[i];
				int num = m_edgePositionInfo[intersectionListEntry.LIndex];
				int num2 = m_edgePositionInfo[intersectionListEntry.RIndex];
				Edge e = m_activeEdgeList[num];
				Edge e2 = m_activeEdgeList[num2];
				Vector3 intersectionPosition = new Vector3(intersectionListEntry.X, intersectionListEntry.Y, 0f);
				IntersectionClassification intersectionClassification = m_operation.ClassifyIntersection(e.OutputSide, e.Kind, e2.OutputSide, e2.Kind);
				PerformIntersection(num, num2, ref e, ref e2, ref intersectionPosition, intersectionClassification);
				SwapEdgePositions(intersectionListEntry.LIndex, intersectionListEntry.RIndex);
			}
			m_intersectionList.Clear();
			m_edgePositionInfo.Clear();
		}

		private void PerformIntersection(int leftEdgeIndex, int rightEdgeIndex, ref Edge e1, ref Edge e2, ref Vector3 intersectionPosition, IntersectionClassification intersectionClassification)
		{
			switch (intersectionClassification)
			{
			case IntersectionClassification.LIKE_INTERSECTION:
			{
				Side outputSide = e1.OutputSide;
				e1.OutputSide = e2.OutputSide;
				e2.OutputSide = outputSide;
				if (e1.Contributing)
				{
					if (e1.OutputSide == Side.RIGHT)
					{
						e1.AssociatedPolygon.Append(intersectionPosition);
						e2.AssociatedPolygon.Prepend(intersectionPosition);
					}
					else
					{
						e1.AssociatedPolygon.Prepend(intersectionPosition);
						e2.AssociatedPolygon.Append(intersectionPosition);
					}
				}
				break;
			}
			case IntersectionClassification.LEFT_E1_INTERMEDIATE:
				e1.AssociatedPolygon.Append(intersectionPosition);
				break;
			case IntersectionClassification.RIGHT_E1_INTERMEDIATE:
				e1.AssociatedPolygon.Prepend(intersectionPosition);
				break;
			case IntersectionClassification.LEFT_E2_INTERMEDIATE:
				e2.AssociatedPolygon.Append(intersectionPosition);
				break;
			case IntersectionClassification.RIGHT_E2_INTERMEDIATE:
				e2.AssociatedPolygon.Prepend(intersectionPosition);
				break;
			case IntersectionClassification.LOCAL_MINIMUM:
			{
				PartialPolygon partialPolygon = new PartialPolygon();
				partialPolygon.Append(intersectionPosition);
				e1.AssociatedPolygon = partialPolygon;
				e2.AssociatedPolygon = partialPolygon;
				break;
			}
			case IntersectionClassification.LOCAL_MAXIMUM:
				AddLocalMaximum(leftEdgeIndex, rightEdgeIndex, ref e1, ref e2, intersectionPosition);
				break;
			}
			PartialPolygon associatedPolygon = e1.AssociatedPolygon;
			e1.AssociatedPolygon = e2.AssociatedPolygon;
			e2.AssociatedPolygon = associatedPolygon;
			if (intersectionClassification != IntersectionClassification.LIKE_INTERSECTION)
			{
				e1.Contributing = !e1.Contributing;
				e2.Contributing = !e2.Contributing;
			}
			m_activeEdgeList[leftEdgeIndex] = e2;
			m_activeEdgeList[rightEdgeIndex] = e1;
		}

		private void AddLocalMaximum(int leftEdgeIndex, int rightEdgeIndex, ref Edge e1, ref Edge e2, Vector3 maximumPosition)
		{
			if (e1.OutputSide == Side.LEFT)
			{
				e1.AssociatedPolygon.Append(maximumPosition);
			}
			else
			{
				e1.AssociatedPolygon.Prepend(maximumPosition);
			}
			int otherEdgeIndex;
			if (e1.AssociatedPolygon == e2.AssociatedPolygon)
			{
				m_results.Add(e1.AssociatedPolygon);
			}
			else if (e1.OutputSide == Side.LEFT)
			{
				Edge value = FindOtherPolygonEdge(e2.AssociatedPolygon, rightEdgeIndex, out otherEdgeIndex);
				e1.AssociatedPolygon.Add(e2.AssociatedPolygon);
				e2.AssociatedPolygon = e1.AssociatedPolygon;
				value.AssociatedPolygon = e2.AssociatedPolygon;
				m_activeEdgeList[otherEdgeIndex] = value;
			}
			else
			{
				Edge value2 = FindOtherPolygonEdge(e1.AssociatedPolygon, leftEdgeIndex, out otherEdgeIndex);
				e2.AssociatedPolygon.Add(e1.AssociatedPolygon);
				e1.AssociatedPolygon = e2.AssociatedPolygon;
				value2.AssociatedPolygon = e2.AssociatedPolygon;
				m_activeEdgeList[otherEdgeIndex] = value2;
			}
			e1.AssociatedPolygon = null;
			e2.AssociatedPolygon = null;
		}

		private Edge FindOtherPolygonEdge(PartialPolygon polygon, int thisEdgeIndex, out int otherEdgeIndex)
		{
			for (int i = 0; i < m_activeEdgeList.Count; i++)
			{
				if (i != thisEdgeIndex && m_activeEdgeList[i].AssociatedPolygon == polygon)
				{
					otherEdgeIndex = i;
					return m_activeEdgeList[i];
				}
			}
			otherEdgeIndex = -1;
			return default(Edge);
		}

		private void InitializeEdgePositions()
		{
			m_edgePositionInfo.Capacity = Math.Max(m_edgePositionInfo.Capacity, m_intersectionList.Count);
			for (int i = 0; i < m_edgePositionInfo.Count; i++)
			{
				m_edgePositionInfo[i] = i;
			}
			for (int j = m_edgePositionInfo.Count; j < m_activeEdgeList.Count; j++)
			{
				m_edgePositionInfo.Add(j);
			}
		}

		private void SwapEdgePositions(int leftEdge, int rightEdge)
		{
			int value = m_edgePositionInfo[leftEdge];
			int value2 = m_edgePositionInfo[rightEdge];
			m_edgePositionInfo[leftEdge] = value2;
			m_edgePositionInfo[rightEdge] = value;
		}

		private void AddBoundPairsToLists(List<BoundPair> boundsList)
		{
			foreach (BoundPair bounds in boundsList)
			{
				bounds.CalculateMinimumCoordinate();
				int num;
				for (num = m_localMinimaList.Count - 1; num >= 0; num--)
				{
					if (m_localMinimaList[num].MinimumCoordinate >= bounds.MinimumCoordinate)
					{
						m_localMinimaList.Insert(num + 1, bounds);
						break;
					}
				}
				if (num == -1)
				{
					m_localMinimaList.Insert(0, bounds);
				}
				InsertScanBeamDivide(bounds.MinimumCoordinate);
				bounds.Parent.GetVertex(bounds.Minimum, out MyPolygon.Vertex v);
				bounds.Parent.GetVertex(v.Next, out MyPolygon.Vertex v2);
				InsertScanBeamDivide(v2.Coord.Y);
				bounds.Parent.GetVertex(v.Prev, out v2);
				while (v2.Coord.Y == v.Coord.Y)
				{
					bounds.Parent.GetVertex(v2.Prev, out v2);
				}
				InsertScanBeamDivide(v2.Coord.Y);
			}
		}

		private void AddBoundPairsToActiveEdges(float bottomY)
		{
			int num = m_localMinimaList.Count - 1;
			while (num >= 0 && m_localMinimaList[num].MinimumCoordinate == bottomY)
			{
				int count = m_usedBoundPairs.Count;
				m_usedBoundPairs.Add(m_localMinimaList[num]);
				MyPolygon parent = m_localMinimaList[num].Parent;
				parent.GetVertex(m_localMinimaList[num].Minimum, out MyPolygon.Vertex v);
				parent.GetVertex(v.Prev, out MyPolygon.Vertex v2);
				parent.GetVertex(v.Next, out MyPolygon.Vertex v3);
				PolygonType polygonType = (parent != m_polyA) ? PolygonType.CLIP : PolygonType.SUBJECT;
				Edge leftEdge = PrepareActiveEdge(count, ref v, ref v3, polygonType, Side.LEFT);
				Edge rightEdge = PrepareActiveEdge(count, ref v, ref v2, polygonType, Side.RIGHT);
				int num2 = SortInMinimum(ref leftEdge, ref rightEdge, polygonType);
				if (leftEdge.Contributing)
				{
					PartialPolygon partialPolygon = new PartialPolygon();
					partialPolygon.Append(v.Coord);
					leftEdge.AssociatedPolygon = partialPolygon;
					rightEdge.AssociatedPolygon = partialPolygon;
				}
				if (leftEdge.DXdy > rightEdge.DXdy)
				{
					m_activeEdgeList.Insert(num2, rightEdge);
					m_activeEdgeList.Insert(num2 + 1, leftEdge);
				}
				else
				{
					m_activeEdgeList.Insert(num2, leftEdge);
					m_activeEdgeList.Insert(num2 + 1, rightEdge);
				}
				num--;
			}
			int count2 = m_localMinimaList.Count - 1 - num;
			m_localMinimaList.RemoveRange(num + 1, count2);
		}

		private Edge PrepareActiveEdge(int boundPairIndex, ref MyPolygon.Vertex lowerVertex, ref MyPolygon.Vertex upperVertex, PolygonType polyType, Side side)
		{
			Edge edge = default(Edge);
			edge.BoundPairIndex = boundPairIndex;
			edge.BoundPairSide = side;
			edge.Kind = polyType;
			if (polyType == PolygonType.CLIP)
			{
				edge.OutputSide = (m_operation.ClipInvert ? OtherSide(side) : side);
			}
			else
			{
				edge.OutputSide = (m_operation.SubjectInvert ? OtherSide(side) : side);
			}
			RecalculateActiveEdge(ref edge, ref lowerVertex, ref upperVertex, side);
			return edge;
		}

		private void RecalculateActiveEdge(ref Edge edge, ref MyPolygon.Vertex lowerVertex, ref MyPolygon.Vertex upperVertex, Side boundPairSide)
		{
			float num = upperVertex.Coord.Y - lowerVertex.Coord.Y;
			float num2 = upperVertex.Coord.X - lowerVertex.Coord.X;
			edge.TopVertexIndex = ((boundPairSide == Side.LEFT) ? lowerVertex.Next : lowerVertex.Prev);
			edge.BottomX = lowerVertex.Coord.X;
			edge.TopY = upperVertex.Coord.Y;
			edge.DXdy = ((num == 0f) ? num2 : (num2 / num));
			InsertScanBeamDivide(upperVertex.Coord.Y);
		}

		private int SortInMinimum(ref Edge leftEdge, ref Edge rightEdge, PolygonType type)
		{
			bool flag = false;
			int i;
			for (i = 0; i < m_activeEdgeList.Count; i++)
			{
				Edge edge = m_activeEdgeList[i];
				if (CompareEdges(ref leftEdge, ref edge) == -1)
				{
					break;
				}
				if (edge.Kind != type)
				{
					flag = !flag;
				}
			}
			rightEdge.Contributing = (leftEdge.Contributing = m_operation.InitializeContributing(flag, type));
			return i;
		}

		private void InsertScanBeamDivide(float value)
		{
			for (int i = 0; i < m_scanBeamList.Count; i++)
			{
				if (!(m_scanBeamList[i] > value))
				{
					if (m_scanBeamList[i] != value)
					{
						m_scanBeamList.Insert(i, value);
					}
					return;
				}
			}
			m_scanBeamList.Add(value);
		}

		private static int CompareEdges(ref Edge edge1, ref Edge edge2)
		{
			if (edge1.BottomX < edge2.BottomX)
			{
				return -1;
			}
			if (edge1.BottomX == edge2.BottomX)
			{
				if (edge1.Kind == edge2.Kind)
				{
					return 1;
				}
				if (edge1.Kind == PolygonType.CLIP)
				{
					return -1;
				}
			}
			return 1;
		}

		private static int CompareSortedEdgeEntries(ref SortedEdgeEntry entry1, ref SortedEdgeEntry entry2)
		{
			if (entry1.XCoord < entry2.XCoord)
			{
				return -1;
			}
			if (entry1.XCoord == entry2.XCoord)
			{
				if (entry1.Kind == entry2.Kind)
				{
					return 1;
				}
				if (entry1.Kind == PolygonType.CLIP)
				{
					return -1;
				}
			}
			return 1;
		}

		private static int CompareCoords(Vector3 coord1, Vector3 coord2)
		{
			if (coord1.Y > coord2.Y)
			{
				return 1;
			}
			if (coord1.Y < coord2.Y)
			{
				return -1;
			}
			if (coord1.X > coord2.X)
			{
				return 1;
			}
			if (coord1.X < coord2.X)
			{
				return -1;
			}
			return 0;
		}

		private static void ConstructBoundPairs(MyPolygon poly, List<BoundPair> boundList)
		{
			for (int i = 0; i < poly.LoopCount; i++)
			{
				int num = FindLoopLocalMaximum(poly, i);
				poly.GetVertex(num, out MyPolygon.Vertex v);
				int num2 = num;
				poly.GetVertex(v.Prev, out MyPolygon.Vertex v2);
				BoundPair item = new BoundPair(poly, -1, -1, num, v2.Coord.Y == v.Coord.Y);
				bool flag = true;
				int num3 = -1;
				do
				{
					Vector3 coord = v.Coord;
					int num4 = num2;
					num2 = v.Next;
					poly.GetVertex(num2, out v);
					int num5 = num3;
					num3 = CompareCoords(v.Coord, coord);
					if (flag)
					{
						if (num3 > 0)
						{
							item.Minimum = num4;
							flag = false;
						}
					}
					else if (num3 < 0)
					{
						item.Left = num4;
						boundList.Add(item);
						item = new BoundPair(poly, -1, -1, num4, num5 == 0);
						flag = true;
					}
				}
				while (num2 != num);
				item.Left = num2;
				boundList.Add(item);
			}
		}

		private static int FindLoopLocalMaximum(MyPolygon poly, int loop)
		{
			int loopStart = poly.GetLoopStart(loop);
			poly.GetVertex(loopStart, out MyPolygon.Vertex v);
			int result = loopStart;
			Vector3 coord = v.Coord;
			loopStart = v.Prev;
			poly.GetVertex(loopStart, out MyPolygon.Vertex v2);
			while (v2.Coord.Y > coord.Y || (v2.Coord.Y == coord.Y && v2.Coord.X > coord.X))
			{
				result = loopStart;
				coord = v2.Coord;
				loopStart = v2.Prev;
				poly.GetVertex(loopStart, out v2);
			}
			loopStart = v.Next;
			poly.GetVertex(loopStart, out v2);
			while (v2.Coord.Y > coord.Y || (v2.Coord.Y == coord.Y && v2.Coord.X > coord.X))
			{
				result = loopStart;
				coord = v2.Coord;
				loopStart = v2.Next;
				poly.GetVertex(loopStart, out v2);
			}
			return result;
		}

		private MyPolygon UnprojectResult()
		{
			MyPolygon myPolygon = new MyPolygon(new Plane(Vector3.Forward, 0f));
			MyPolygon myPolygon2 = new MyPolygon(m_projectionPlane);
			foreach (PartialPolygon result in m_results)
			{
				result.Postprocess();
				if (result.GetLoop().Count != 0)
				{
					myPolygon.AddLoop(result.GetLoop());
				}
			}
			ProjectPoly(myPolygon, myPolygon2, ref m_invProjectionTransform);
			return myPolygon2;
		}

		public void DebugDraw(MatrixD drawMatrix)
		{
			drawMatrix = drawMatrix * m_invProjectionTransform * Matrix.CreateTranslation(m_invProjectionTransform.Left * 12f);
			DebugDrawBoundList(drawMatrix, m_polyA, m_boundsA);
			DebugDrawBoundList(drawMatrix, m_polyB, m_boundsB);
		}

		private static MatrixD DebugDrawBoundList(MatrixD drawMatrix, MyPolygon drawPoly, List<BoundPair> boundList)
		{
			foreach (BoundPair bound in boundList)
			{
				Vector3 v = default(Vector3);
				int num = bound.Left;
				drawPoly.GetVertex(num, out MyPolygon.Vertex v2);
				int prev = v2.Prev;
				MyPolygon.Vertex v3;
				while (num != bound.Minimum)
				{
					drawPoly.GetVertex(prev, out v3);
					Vector3 v4 = Vector3D.Transform(v2.Coord, drawMatrix);
					v = Vector3D.Transform(v3.Coord, drawMatrix);
					MyRenderProxy.DebugDrawLine3D(v4, v, Color.Red, Color.Red, depthRead: false);
					num = prev;
					v2 = v3;
					prev = v2.Prev;
				}
				MatrixD matrix = drawMatrix;
				matrix.Translation = v;
				MyRenderProxy.DebugDrawAxis(matrix, 0.25f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(v, 0.03f, Color.Yellow, 1f, depthRead: false);
				num = bound.Minimum;
				drawPoly.GetVertex(num, out v2);
				prev = v2.Prev;
				while (num != bound.Right)
				{
					drawPoly.GetVertex(prev, out v3);
					Vector3 v5 = Vector3D.Transform(v2.Coord, drawMatrix);
					v = Vector3D.Transform(v3.Coord, drawMatrix);
					MyRenderProxy.DebugDrawLine3D(v5, v, Color.Green, Color.Green, depthRead: false);
					num = prev;
					v2 = v3;
					prev = v2.Prev;
				}
				if (bound.RightIsPrecededByHorizontal)
				{
					MyRenderProxy.DebugDrawSphere(v, 0.03f, Color.Red, 1f, depthRead: false);
				}
			}
			return drawMatrix;
		}
	}
}
