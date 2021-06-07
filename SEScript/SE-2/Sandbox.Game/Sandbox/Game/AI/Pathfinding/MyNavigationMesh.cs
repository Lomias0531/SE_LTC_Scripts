using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Algorithms;
using VRage.Generics;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.AI.Pathfinding
{
	public abstract class MyNavigationMesh : MyPathFindingSystem<MyNavigationPrimitive>, IMyNavigationGroup
	{
		private class Funnel
		{
			private enum PointTestResult
			{
				LEFT,
				INSIDE,
				RIGHT
			}

			private Vector3 m_end;

			private int m_endIndex;

			private MyPath<MyNavigationPrimitive> m_input;

			private List<Vector4D> m_output;

			private Vector3 m_apex;

			private Vector3 m_apexNormal;

			private Vector3 m_leftPoint;

			private Vector3 m_rightPoint;

			private int m_leftIndex;

			private int m_rightIndex;

			private Vector3 m_leftPlaneNormal;

			private Vector3 m_rightPlaneNormal;

			private float m_leftD;

			private float m_rightD;

			private bool m_funnelConstructed;

			private bool m_segmentDangerous;

			private static float SAFE_DISTANCE = 0.7f;

			private static float SAFE_DISTANCE_SQ = SAFE_DISTANCE * SAFE_DISTANCE;

			private static float SAFE_DISTANCE2_SQ = (SAFE_DISTANCE + SAFE_DISTANCE) * (SAFE_DISTANCE + SAFE_DISTANCE);

			public void Calculate(MyPath<MyNavigationPrimitive> inputPath, List<Vector4D> refinedPath, ref Vector3 start, ref Vector3 end, int startIndex, int endIndex)
			{
				m_debugFunnel.Clear();
				m_debugPointsLeft.Clear();
				m_debugPointsRight.Clear();
				m_end = end;
				m_endIndex = endIndex;
				m_input = inputPath;
				m_output = refinedPath;
				m_apex = start;
				m_funnelConstructed = false;
				m_segmentDangerous = false;
				int num = startIndex;
				while (num < endIndex)
				{
					num = AddTriangle(num);
					if (num == endIndex)
					{
						PointTestResult pointTestResult = TestPoint(end);
						switch (pointTestResult)
						{
						case PointTestResult.LEFT:
							m_apex = m_leftPoint;
							m_funnelConstructed = false;
							ConstructFunnel(m_leftIndex);
							num = m_leftIndex + 1;
							break;
						case PointTestResult.RIGHT:
							m_apex = m_rightPoint;
							m_funnelConstructed = false;
							ConstructFunnel(m_rightIndex);
							num = m_rightIndex + 1;
							break;
						}
						if (pointTestResult == PointTestResult.INSIDE || num == endIndex)
						{
							AddPoint(ProjectEndOnTriangle(num));
						}
					}
				}
				if (startIndex == endIndex)
				{
					AddPoint(ProjectEndOnTriangle(num));
				}
				m_input = null;
				m_output = null;
			}

			private void AddPoint(Vector3D point)
			{
				float num = m_segmentDangerous ? 0.5f : 2f;
				m_output.Add(new Vector4D(point, num));
				int num2 = m_output.Count - 1;
				if (num2 >= 0)
				{
					Vector4D value = m_output[num2];
					if (value.W > (double)num)
					{
						value.W = num;
						m_output[num2] = value;
					}
				}
				m_segmentDangerous = false;
			}

			private Vector3 ProjectEndOnTriangle(int i)
			{
				return (m_input[i].Vertex as MyNavigationTriangle).ProjectLocalPoint(m_end);
			}

			private int AddTriangle(int index)
			{
				if (!m_funnelConstructed)
				{
					ConstructFunnel(index);
				}
				else
				{
					MyPath<MyNavigationPrimitive>.PathNode pathNode = m_input[index];
					MyNavigationTriangle myNavigationTriangle = pathNode.Vertex as MyNavigationTriangle;
					myNavigationTriangle.GetNavigationEdge(pathNode.nextVertex);
					GetEdgeVerticesSafe(myNavigationTriangle, pathNode.nextVertex, out Vector3 left, out Vector3 right);
					PointTestResult pointTestResult = TestPoint(left);
					PointTestResult pointTestResult2 = TestPoint(right);
					if (pointTestResult == PointTestResult.INSIDE)
					{
						NarrowFunnel(left, index, left: true);
					}
					if (pointTestResult2 == PointTestResult.INSIDE)
					{
						NarrowFunnel(right, index, left: false);
					}
					if (pointTestResult == PointTestResult.RIGHT)
					{
						m_apex = m_rightPoint;
						m_funnelConstructed = false;
						ConstructFunnel(m_rightIndex + 1);
						return m_rightIndex + 1;
					}
					if (pointTestResult2 == PointTestResult.LEFT)
					{
						m_apex = m_leftPoint;
						m_funnelConstructed = false;
						ConstructFunnel(m_leftIndex + 1);
						return m_leftIndex + 1;
					}
					if (pointTestResult == PointTestResult.INSIDE || pointTestResult2 == PointTestResult.INSIDE)
					{
						m_debugFunnel.Add(new FunnelState
						{
							Apex = m_apex,
							Left = m_leftPoint,
							Right = m_rightPoint
						});
					}
				}
				return index + 1;
			}

			private void GetEdgeVerticesSafe(MyNavigationTriangle triangle, int edgeIndex, out Vector3 left, out Vector3 right)
			{
				triangle.GetEdgeVertices(edgeIndex, out left, out right);
				float num = (left - right).LengthSquared();
				bool flag = triangle.IsEdgeVertexDangerous(edgeIndex, predVertex: true);
				bool flag2 = triangle.IsEdgeVertexDangerous(edgeIndex, predVertex: false);
				m_segmentDangerous |= (flag || flag2);
				if (flag)
				{
					if (flag2)
					{
						if (SAFE_DISTANCE2_SQ > num)
						{
							left = (left + right) * 0.5f;
							right = left;
						}
						else
						{
							float num2 = SAFE_DISTANCE / (float)Math.Sqrt(num);
							Vector3 vector = right * num2 + left * (1f - num2);
							right = left * num2 + right * (1f - num2);
							left = vector;
						}
					}
					else if (SAFE_DISTANCE_SQ > num)
					{
						left = right;
					}
					else
					{
						float num3 = SAFE_DISTANCE / (float)Math.Sqrt(num);
						left = right * num3 + left * (1f - num3);
					}
				}
				else if (flag2)
				{
					if (SAFE_DISTANCE_SQ > num)
					{
						right = left;
					}
					else
					{
						float num4 = SAFE_DISTANCE / (float)Math.Sqrt(num);
						right = left * num4 + right * (1f - num4);
					}
				}
				m_debugPointsLeft.Add(left);
				m_debugPointsRight.Add(right);
			}

			private void NarrowFunnel(Vector3 point, int index, bool left)
			{
				if (left)
				{
					m_leftPoint = point;
					m_leftIndex = index;
					RecalculateLeftPlane();
				}
				else
				{
					m_rightPoint = point;
					m_rightIndex = index;
					RecalculateRightPlane();
				}
			}

			private void ConstructFunnel(int index)
			{
				if (index >= m_endIndex)
				{
					AddPoint(m_apex);
					return;
				}
				MyPath<MyNavigationPrimitive>.PathNode pathNode = m_input[index];
				MyNavigationTriangle myNavigationTriangle = pathNode.Vertex as MyNavigationTriangle;
				myNavigationTriangle.GetNavigationEdge(pathNode.nextVertex);
				GetEdgeVerticesSafe(myNavigationTriangle, pathNode.nextVertex, out m_leftPoint, out m_rightPoint);
				if (Vector3.IsZero(m_leftPoint - m_apex))
				{
					m_apex = myNavigationTriangle.Center;
					return;
				}
				if (Vector3.IsZero(m_rightPoint - m_apex))
				{
					m_apex = myNavigationTriangle.Center;
					return;
				}
				m_apexNormal = myNavigationTriangle.Normal;
				float num = m_leftPoint.Dot(m_apexNormal);
				m_apex -= m_apexNormal * (m_apex.Dot(m_apexNormal) - num);
				m_leftIndex = (m_rightIndex = index);
				RecalculateLeftPlane();
				RecalculateRightPlane();
				m_funnelConstructed = true;
				AddPoint(m_apex);
				m_debugFunnel.Add(new FunnelState
				{
					Apex = m_apex,
					Left = m_leftPoint,
					Right = m_rightPoint
				});
			}

			private PointTestResult TestPoint(Vector3 point)
			{
				if (point.Dot(m_leftPlaneNormal) < 0f - m_leftD)
				{
					return PointTestResult.LEFT;
				}
				if (point.Dot(m_rightPlaneNormal) < 0f - m_rightD)
				{
					return PointTestResult.RIGHT;
				}
				return PointTestResult.INSIDE;
			}

			private void RecalculateLeftPlane()
			{
				Vector3 vector = m_leftPoint - m_apex;
				vector.Normalize();
				m_leftPlaneNormal = Vector3.Cross(vector, m_apexNormal);
				m_leftPlaneNormal.Normalize();
				m_leftD = 0f - m_leftPoint.Dot(m_leftPlaneNormal);
			}

			private void RecalculateRightPlane()
			{
				Vector3 vector = m_rightPoint - m_apex;
				vector.Normalize();
				m_rightPlaneNormal = Vector3.Cross(m_apexNormal, vector);
				m_rightPlaneNormal.Normalize();
				m_rightD = 0f - m_rightPoint.Dot(m_rightPlaneNormal);
			}
		}

		public struct FunnelState
		{
			public Vector3 Apex;

			public Vector3 Left;

			public Vector3 Right;
		}

		private MyDynamicObjectPool<MyNavigationTriangle> m_triPool;

		private MyWingedEdgeMesh m_mesh;

		private MyNavgroupLinks m_externalLinks;

		private Vector3 m_vertex;

		private Vector3 m_left;

		private Vector3 m_right;

		private Vector3 m_normal;

		private List<Vector3> m_vertexList = new List<Vector3>();

		private static List<Vector3> m_debugPointsLeft = new List<Vector3>();

		private static List<Vector3> m_debugPointsRight = new List<Vector3>();

		private static List<Vector3> m_path = new List<Vector3>();

		private static List<Vector3> m_path2;

		private static List<FunnelState> m_debugFunnel = new List<FunnelState>();

		public static int m_debugFunnelIdx = 0;

		public MyWingedEdgeMesh Mesh => m_mesh;

		public abstract MyHighLevelGroup HighLevelGroup
		{
			get;
		}

		public MyNavigationMesh(MyNavgroupLinks externalLinks, int trianglePrealloc = 16, Func<long> timestampFunction = null)
			: base(128, timestampFunction)
		{
			m_triPool = new MyDynamicObjectPool<MyNavigationTriangle>(trianglePrealloc);
			m_mesh = new MyWingedEdgeMesh();
			m_externalLinks = externalLinks;
		}

		protected MyNavigationTriangle AddTriangle(ref Vector3 A, ref Vector3 B, ref Vector3 C, ref int edgeAB, ref int edgeBC, ref int edgeCA)
		{
			MyNavigationTriangle myNavigationTriangle = m_triPool.Allocate();
			int num = 0;
			num += ((edgeAB == -1) ? 1 : 0);
			num += ((edgeBC == -1) ? 1 : 0);
			num += ((edgeCA == -1) ? 1 : 0);
			int num2 = -1;
			switch (num)
			{
			case 3:
				num2 = m_mesh.MakeNewTriangle(myNavigationTriangle, ref A, ref B, ref C, out edgeAB, out edgeBC, out edgeCA);
				break;
			case 2:
				num2 = ((edgeAB == -1) ? ((edgeBC == -1) ? m_mesh.ExtrudeTriangleFromEdge(ref B, edgeCA, myNavigationTriangle, out edgeAB, out edgeBC) : m_mesh.ExtrudeTriangleFromEdge(ref A, edgeBC, myNavigationTriangle, out edgeCA, out edgeAB)) : m_mesh.ExtrudeTriangleFromEdge(ref C, edgeAB, myNavigationTriangle, out edgeBC, out edgeCA));
				break;
			case 1:
				num2 = ((edgeAB != -1) ? ((edgeBC != -1) ? GetTriangleOneNewEdge(ref edgeCA, ref edgeAB, ref edgeBC, myNavigationTriangle) : GetTriangleOneNewEdge(ref edgeBC, ref edgeCA, ref edgeAB, myNavigationTriangle)) : GetTriangleOneNewEdge(ref edgeAB, ref edgeBC, ref edgeCA, myNavigationTriangle));
				break;
			default:
			{
				MyWingedEdgeMesh.Edge other = m_mesh.GetEdge(edgeAB);
				MyWingedEdgeMesh.Edge other2 = m_mesh.GetEdge(edgeBC);
				MyWingedEdgeMesh.Edge other3 = m_mesh.GetEdge(edgeCA);
				int num3 = other3.TryGetSharedVertex(ref other);
				int num4 = other.TryGetSharedVertex(ref other2);
				int num5 = other2.TryGetSharedVertex(ref other3);
				int num6 = 0;
				num6 += ((num3 != -1) ? 1 : 0);
				num6 += ((num4 != -1) ? 1 : 0);
				switch (num6 + ((num5 != -1) ? 1 : 0))
				{
				case 3:
					num2 = m_mesh.MakeFace(myNavigationTriangle, edgeAB);
					break;
				case 2:
					num2 = ((num3 != -1) ? ((num4 != -1) ? GetTriangleTwoSharedVertices(edgeCA, edgeAB, ref edgeBC, num3, num4, myNavigationTriangle) : GetTriangleTwoSharedVertices(edgeBC, edgeCA, ref edgeAB, num5, num3, myNavigationTriangle)) : GetTriangleTwoSharedVertices(edgeAB, edgeBC, ref edgeCA, num4, num5, myNavigationTriangle));
					break;
				case 1:
					num2 = ((num3 == -1) ? ((num4 == -1) ? GetTriangleOneSharedVertex(edgeBC, edgeCA, ref edgeAB, num5, myNavigationTriangle) : GetTriangleOneSharedVertex(edgeAB, edgeBC, ref edgeCA, num4, myNavigationTriangle)) : GetTriangleOneSharedVertex(edgeCA, edgeAB, ref edgeBC, num3, myNavigationTriangle));
					break;
				default:
				{
					num2 = m_mesh.ExtrudeTriangleFromEdge(ref C, edgeAB, myNavigationTriangle, out int newEdgeS, out int newEdgeP);
					m_mesh.MergeEdges(newEdgeP, edgeCA);
					m_mesh.MergeEdges(newEdgeS, edgeBC);
					break;
				}
				}
				break;
			}
			}
			myNavigationTriangle.Init(this, num2);
			return myNavigationTriangle;
		}

		protected void RemoveTriangle(MyNavigationTriangle tri)
		{
			m_mesh.RemoveFace(tri.Index);
			m_triPool.Deallocate(tri);
		}

		private int GetTriangleOneNewEdge(ref int newEdge, ref int succ, ref int pred, MyNavigationTriangle newTri)
		{
			MyWingedEdgeMesh.Edge edge = m_mesh.GetEdge(pred);
			MyWingedEdgeMesh.Edge other = m_mesh.GetEdge(succ);
			int num = edge.TryGetSharedVertex(ref other);
			if (num == -1)
			{
				int edge2 = succ;
				Vector3 newVertex = m_mesh.GetVertexPosition(other.GetFacePredVertex(-1));
				int result = m_mesh.ExtrudeTriangleFromEdge(ref newVertex, pred, newTri, out newEdge, out succ);
				m_mesh.MergeEdges(edge2, succ);
				return result;
			}
			int vert = edge.OtherVertex(num);
			int vert2 = other.OtherVertex(num);
			return m_mesh.MakeEdgeFace(vert, vert2, pred, succ, newTri, out newEdge);
		}

		private int GetTriangleOneSharedVertex(int edgeCA, int edgeAB, ref int edgeBC, int sharedA, MyNavigationTriangle newTri)
		{
			int vert = m_mesh.GetEdge(edgeAB).OtherVertex(sharedA);
			int vert2 = m_mesh.GetEdge(edgeCA).OtherVertex(sharedA);
			int edge = edgeBC;
			int result = m_mesh.MakeEdgeFace(vert, vert2, edgeAB, edgeCA, newTri, out edgeBC);
			m_mesh.MergeEdges(edge, edgeBC);
			return result;
		}

		private int GetTriangleTwoSharedVertices(int edgeAB, int edgeBC, ref int edgeCA, int sharedB, int sharedC, MyNavigationTriangle newTri)
		{
			int vert = m_mesh.GetEdge(edgeAB).OtherVertex(sharedB);
			int leftEdge = edgeCA;
			int result = m_mesh.MakeEdgeFace(sharedC, vert, edgeBC, edgeAB, newTri, out edgeCA);
			m_mesh.MergeAngle(leftEdge, edgeCA, sharedC);
			return result;
		}

		public MyNavigationTriangle GetTriangle(int index)
		{
			return m_mesh.GetFace(index).GetUserData<MyNavigationTriangle>();
		}

		protected MyNavigationTriangle GetEdgeTriangle(int edgeIndex)
		{
			MyWingedEdgeMesh.Edge edge = m_mesh.GetEdge(edgeIndex);
			if (edge.LeftFace == -1)
			{
				return GetTriangle(edge.RightFace);
			}
			return GetTriangle(edge.LeftFace);
		}

		protected List<Vector4D> FindRefinedPath(MyNavigationTriangle start, MyNavigationTriangle end, ref Vector3 startPoint, ref Vector3 endPoint)
		{
			MyPath<MyNavigationPrimitive> myPath = FindPath(start, end);
			if (myPath == null)
			{
				return null;
			}
			List<Vector4D> list = new List<Vector4D>();
			list.Add(new Vector4D(startPoint, 1.0));
			new Funnel().Calculate(myPath, list, ref startPoint, ref endPoint, 0, myPath.Count - 1);
			m_path.Clear();
			foreach (Vector4D item in list)
			{
				m_path.Add(new Vector3D(item));
			}
			return list;
		}

		public void RefinePath(MyPath<MyNavigationPrimitive> path, List<Vector4D> output, ref Vector3 startPoint, ref Vector3 endPoint, int begin, int end)
		{
			new Funnel().Calculate(path, output, ref startPoint, ref endPoint, begin, end);
		}

		public abstract Vector3 GlobalToLocal(Vector3D globalPos);

		public abstract Vector3D LocalToGlobal(Vector3 localPos);

		public abstract MyHighLevelPrimitive GetHighLevelPrimitive(MyNavigationPrimitive myNavigationTriangle);

		public abstract IMyHighLevelComponent GetComponent(MyHighLevelPrimitive highLevelPrimitive);

		public abstract MyNavigationPrimitive FindClosestPrimitive(Vector3D point, bool highLevel, ref double closestDistanceSq);

		public void ErasePools()
		{
			m_triPool = null;
		}

		[Conditional("DEBUG")]
		public virtual void DebugDraw(ref Matrix drawMatrix)
		{
			if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				return;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES != 0)
			{
				m_mesh.DebugDraw(ref drawMatrix, MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES);
				m_mesh.CustomDebugDrawFaces(ref drawMatrix, MyDebugDrawSettings.DEBUG_DRAW_NAVMESHES, (object obj) => (obj as MyNavigationTriangle).Index.ToString());
			}
			if (MyFakes.DEBUG_DRAW_FUNNEL)
			{
				MyRenderProxy.DebugDrawSphere(Vector3.Transform(m_vertex, drawMatrix), 0.05f, Color.Yellow.ToVector3(), 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(Vector3.Transform(m_vertex + m_normal, drawMatrix), 0.05f, Color.Orange.ToVector3(), 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(Vector3.Transform(m_left, drawMatrix), 0.05f, Color.Red.ToVector3(), 1f, depthRead: false);
				MyRenderProxy.DebugDrawSphere(Vector3.Transform(m_right, drawMatrix), 0.05f, Color.Green.ToVector3(), 1f, depthRead: false);
				foreach (Vector3 item in m_debugPointsLeft)
				{
					MyRenderProxy.DebugDrawSphere(Vector3.Transform(item, drawMatrix), 0.03f, Color.Red.ToVector3(), 1f, depthRead: false);
				}
				foreach (Vector3 item2 in m_debugPointsRight)
				{
					MyRenderProxy.DebugDrawSphere(Vector3.Transform(item2, drawMatrix), 0.04f, Color.Green.ToVector3(), 1f, depthRead: false);
				}
				Vector3? vector = null;
				if (m_path != null)
				{
					foreach (Vector3 item3 in m_path)
					{
						Vector3 vector2 = Vector3.Transform(item3, drawMatrix);
						MyRenderProxy.DebugDrawSphere(vector2 + Vector3.Up * 0.2f, 0.02f, Color.Orange.ToVector3(), 1f, depthRead: false);
						if (vector.HasValue)
						{
							MyRenderProxy.DebugDrawLine3D(vector.Value + Vector3.Up * 0.2f, vector2 + Vector3.Up * 0.2f, Color.Orange, Color.Orange, depthRead: true);
						}
						vector = vector2;
					}
				}
				vector = null;
				if (m_path2 != null)
				{
					foreach (Vector3 item4 in m_path2)
					{
						Vector3 vector3 = Vector3.Transform(item4, drawMatrix);
						if (vector.HasValue)
						{
							MyRenderProxy.DebugDrawLine3D(vector.Value + Vector3.Up * 0.1f, vector3 + Vector3.Up * 0.1f, Color.Violet, Color.Violet, depthRead: true);
						}
						vector = vector3;
					}
				}
				if (m_debugFunnel.Count > 0)
				{
					FunnelState funnelState = m_debugFunnel[m_debugFunnelIdx % m_debugFunnel.Count];
					Vector3 vector4 = Vector3.Transform(funnelState.Apex, drawMatrix);
					Vector3 value = Vector3.Transform(funnelState.Left, drawMatrix);
					Vector3 value2 = Vector3.Transform(funnelState.Right, drawMatrix);
					value = vector4 + (value - vector4) * 10f;
					value2 = vector4 + (value2 - vector4) * 10f;
					Color cyan = Color.Cyan;
					MyRenderProxy.DebugDrawLine3D(vector4 + Vector3.Up * 0.1f, value + Vector3.Up * 0.1f, cyan, cyan, depthRead: true);
					MyRenderProxy.DebugDrawLine3D(vector4 + Vector3.Up * 0.1f, value2 + Vector3.Up * 0.1f, cyan, cyan, depthRead: true);
				}
			}
		}

		public void RemoveFace(int index)
		{
			m_mesh.RemoveFace(index);
		}

		public virtual MatrixD GetWorldMatrix()
		{
			return MatrixD.Identity;
		}

		[Conditional("DEBUG")]
		public void CheckMeshConsistency()
		{
		}

		public int ApproximateMemoryFootprint()
		{
			return m_mesh.ApproximateMemoryFootprint() + m_triPool.Count * (Environment.Is64BitProcess ? 88 : 56);
		}

		public int GetExternalNeighborCount(MyNavigationPrimitive primitive)
		{
			if (m_externalLinks != null)
			{
				return m_externalLinks.GetLinkCount(primitive);
			}
			return 0;
		}

		public MyNavigationPrimitive GetExternalNeighbor(MyNavigationPrimitive primitive, int index)
		{
			if (m_externalLinks != null)
			{
				return m_externalLinks.GetLinkedNeighbor(primitive, index);
			}
			return null;
		}

		public IMyPathEdge<MyNavigationPrimitive> GetExternalEdge(MyNavigationPrimitive primitive, int index)
		{
			if (m_externalLinks != null)
			{
				return m_externalLinks.GetEdge(primitive, index);
			}
			return null;
		}
	}
}
