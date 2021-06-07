using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage.Library;
using VRage.Utils;
using VRageMath;

namespace VRageRender.Utils
{
	public class MyWingedEdgeMesh
	{
		private struct EdgeTableEntry
		{
			public int Vertex1;

			public int Vertex2;

			public int LeftFace;

			public int RightFace;

			public int LeftPred;

			public int LeftSucc;

			public int RightPred;

			public int RightSucc;

			/// <summary>
			/// Only valid for empty (deallocated) table entries. In that case, it points to the next free table entry.
			/// If this is -1, this entry is the last free entry.
			/// </summary>
			public int NextFreeEntry
			{
				get
				{
					return Vertex1;
				}
				set
				{
					Vertex1 = value;
				}
			}

			public void Init()
			{
				Vertex1 = INVALID_INDEX;
				Vertex2 = INVALID_INDEX;
				LeftFace = INVALID_INDEX;
				RightFace = INVALID_INDEX;
				LeftPred = INVALID_INDEX;
				LeftSucc = INVALID_INDEX;
				RightPred = INVALID_INDEX;
				RightSucc = INVALID_INDEX;
			}

			public int OtherVertex(int vert)
			{
				if (vert != Vertex1)
				{
					return Vertex1;
				}
				return Vertex2;
			}

			public void GetFaceVertices(int face, out int predVertex, out int succVertex)
			{
				if (face == LeftFace)
				{
					predVertex = Vertex2;
					succVertex = Vertex1;
				}
				else
				{
					predVertex = Vertex1;
					succVertex = Vertex2;
				}
			}

			public int GetFaceSuccVertex(int face)
			{
				if (face == LeftFace)
				{
					return Vertex1;
				}
				return Vertex2;
			}

			public int GetFacePredVertex(int face)
			{
				if (face == LeftFace)
				{
					return Vertex2;
				}
				return Vertex1;
			}

			public void SetFaceSuccVertex(int face, int vertex)
			{
				if (face == LeftFace)
				{
					Vertex1 = vertex;
				}
				else
				{
					Vertex2 = vertex;
				}
			}

			public void SetFacePredVertex(int face, int vertex)
			{
				if (face == LeftFace)
				{
					Vertex2 = vertex;
				}
				else
				{
					Vertex1 = vertex;
				}
			}

			/// <summary>
			/// Returns -1 if there is no shared edge
			/// </summary>
			public int TryGetSharedVertex(ref EdgeTableEntry otherEdge)
			{
				if (Vertex1 == otherEdge.Vertex1)
				{
					return Vertex1;
				}
				if (Vertex1 == otherEdge.Vertex2)
				{
					return Vertex1;
				}
				if (Vertex2 == otherEdge.Vertex1)
				{
					return Vertex2;
				}
				if (Vertex2 == otherEdge.Vertex2)
				{
					return Vertex2;
				}
				return INVALID_INDEX;
			}

			public void ChangeVertex(int oldVertex, int newVertex)
			{
				if (oldVertex == Vertex1)
				{
					Vertex1 = newVertex;
				}
				else
				{
					Vertex2 = newVertex;
				}
			}

			public int OtherFace(int face)
			{
				if (face == LeftFace)
				{
					return RightFace;
				}
				return LeftFace;
			}

			/// <summary>
			/// Returns a face to the left when going towards the given vertex
			/// </summary>
			public int VertexLeftFace(int vertex)
			{
				if (vertex != Vertex1)
				{
					return LeftFace;
				}
				return RightFace;
			}

			/// <summary>
			/// Returns a face to the right when going towards the given vertex
			/// </summary>
			public int VertexRightFace(int vertex)
			{
				if (vertex != Vertex1)
				{
					return RightFace;
				}
				return LeftFace;
			}

			public void AddFace(int face)
			{
				if (LeftFace == INVALID_INDEX)
				{
					LeftFace = face;
				}
				else
				{
					RightFace = face;
				}
			}

			public void ChangeFace(int previousFace, int newFace)
			{
				if (previousFace == LeftFace)
				{
					LeftFace = newFace;
				}
				else
				{
					RightFace = newFace;
				}
			}

			/// <summary>
			/// Returns the successor of this edge in the given face
			/// </summary>
			public int FaceSucc(int faceIndex)
			{
				if (faceIndex == LeftFace)
				{
					return LeftSucc;
				}
				return RightSucc;
			}

			/// <summary>
			/// Sets the successor of this edge in the given face
			/// </summary>
			public void SetFaceSucc(int faceIndex, int newSucc)
			{
				if (faceIndex == LeftFace)
				{
					LeftSucc = newSucc;
				}
				else
				{
					RightSucc = newSucc;
				}
			}

			/// <summary>
			/// Returns the predecessor of this edge in the given face
			/// </summary>
			public int FacePred(int faceIndex)
			{
				if (faceIndex == LeftFace)
				{
					return LeftPred;
				}
				return RightPred;
			}

			/// <summary>
			/// Sets the predecessor of this edge in the given face
			/// </summary>
			public void SetFacePred(int faceIndex, int newPred)
			{
				if (faceIndex == LeftFace)
				{
					LeftPred = newPred;
				}
				else
				{
					RightPred = newPred;
				}
			}

			/// <summary>
			/// Gets the successor around the given vertex.
			/// </summary>
			public int VertexSucc(int vertexIndex)
			{
				if (vertexIndex != Vertex1)
				{
					return RightSucc;
				}
				return LeftSucc;
			}

			/// <summary>
			/// Sets the successor around the given vertex.
			/// </summary>
			/// <returns>The old successor value</returns>
			public int SetVertexSucc(int vertexIndex, int newSucc)
			{
				int iNVALID_INDEX = INVALID_INDEX;
				if (vertexIndex == Vertex1)
				{
					iNVALID_INDEX = LeftSucc;
					LeftSucc = newSucc;
				}
				else
				{
					iNVALID_INDEX = RightSucc;
					RightSucc = newSucc;
				}
				return iNVALID_INDEX;
			}

			/// <summary>
			/// Gets the predecessor around the given vertex
			/// </summary>
			public int VertexPred(int vertexIndex)
			{
				if (vertexIndex != Vertex1)
				{
					return LeftPred;
				}
				return RightPred;
			}

			/// <summary>
			/// Sets the predecessor around the given vertex.
			/// </summary>
			/// <returns>The old predecessor value</returns>
			public int SetVertexPred(int vertexIndex, int newPred)
			{
				int iNVALID_INDEX = INVALID_INDEX;
				if (vertexIndex == Vertex1)
				{
					iNVALID_INDEX = RightPred;
					RightPred = newPred;
				}
				else
				{
					iNVALID_INDEX = LeftPred;
					LeftPred = newPred;
				}
				return iNVALID_INDEX;
			}

			public override string ToString()
			{
				return "V: " + Vertex1 + ", " + Vertex2 + "; Left (Pred, Face, Succ): " + LeftPred + ", " + LeftFace + ", " + LeftSucc + "; Right (Pred, Face, Succ): " + RightPred + ", " + RightFace + ", " + RightSucc;
			}
		}

		private struct VertexTableEntry
		{
			public int IncidentEdge;

			public Vector3 VertexPosition;

			/// <summary>
			/// Only valid for empty (deallocated) table entries. In that case, it points to the next free table entry.
			/// If this is -1, this entry is the last free entry.
			/// </summary>
			public int NextFreeEntry
			{
				get
				{
					return IncidentEdge;
				}
				set
				{
					IncidentEdge = value;
				}
			}

			public override string ToString()
			{
				return VertexPosition.ToString() + " -> " + IncidentEdge;
			}
		}

		private struct FaceTableEntry
		{
			public int IncidentEdge;

			public object UserData;

			/// <summary>
			/// Only valid for empty (deallocated) table entries. In that case, it points to the next free table entry.
			/// If this is -1, this entry is the last free entry.
			/// </summary>
			public int NextFreeEntry
			{
				get
				{
					return IncidentEdge;
				}
				set
				{
					IncidentEdge = value;
				}
			}

			public override string ToString()
			{
				return "-> " + IncidentEdge;
			}
		}

		/// <summary>
		/// Note: This is invalid after the mesh changes!
		/// </summary>
		public struct Edge
		{
			private EdgeTableEntry m_entry;

			private int m_index;

			public int LeftFace => m_entry.LeftFace;

			public int RightFace => m_entry.RightFace;

			public int Vertex1 => m_entry.Vertex1;

			public int Vertex2 => m_entry.Vertex2;

			public int Index => m_index;

			public Edge(MyWingedEdgeMesh mesh, int index)
			{
				m_entry = mesh.GetEdgeEntry(index);
				m_index = index;
			}

			public int TryGetSharedVertex(ref Edge other)
			{
				return m_entry.TryGetSharedVertex(ref other.m_entry);
			}

			public int GetFacePredVertex(int face)
			{
				return m_entry.GetFacePredVertex(face);
			}

			public int GetFaceSuccVertex(int face)
			{
				return m_entry.GetFaceSuccVertex(face);
			}

			public int OtherVertex(int vertex)
			{
				return m_entry.OtherVertex(vertex);
			}

			public int OtherFace(int face)
			{
				return m_entry.OtherFace(face);
			}

			public int GetPreviousFaceEdge(int faceIndex)
			{
				return m_entry.FacePred(faceIndex);
			}

			public int GetNextFaceEdge(int faceIndex)
			{
				return m_entry.FaceSucc(faceIndex);
			}

			public int GetNextVertexEdge(int vertexIndex)
			{
				return m_entry.VertexPred(vertexIndex);
			}

			public int VertexLeftFace(int vertexIndex)
			{
				return m_entry.VertexLeftFace(vertexIndex);
			}

			public void ToRay(MyWingedEdgeMesh mesh, ref Ray output)
			{
				Vector3 vertexPosition = mesh.GetVertexPosition(Vertex1);
				Vector3 vertexPosition2 = mesh.GetVertexPosition(Vertex2);
				output.Position = vertexPosition;
				output.Direction = vertexPosition2 - vertexPosition;
			}
		}

		/// <summary>
		/// Note: This is invalid after the mesh changes!
		/// </summary>
		public struct Face
		{
			private int m_faceIndex;

			private MyWingedEdgeMesh m_mesh;

			public Face(MyWingedEdgeMesh mesh, int index)
			{
				m_mesh = mesh;
				m_faceIndex = index;
			}

			public FaceEdgeEnumerator GetEnumerator()
			{
				return new FaceEdgeEnumerator(m_mesh, m_faceIndex);
			}

			public FaceVertexEnumerator GetVertexEnumerator()
			{
				return new FaceVertexEnumerator(m_mesh, m_faceIndex);
			}

			public T GetUserData<T>() where T : class
			{
				return m_mesh.GetFaceEntry(m_faceIndex).UserData as T;
			}
		}

		/// <summary>
		/// Note: This is invalid after the mesh changes!
		/// </summary>
		public struct VertexEdgeEnumerator
		{
			private int m_vertexIndex;

			private int m_startingEdge;

			private int m_currentEdgeIndex;

			private MyWingedEdgeMesh m_mesh;

			private Edge m_currentEdge;

			public int CurrentIndex => m_currentEdgeIndex;

			public Edge Current => m_currentEdge;

			public VertexEdgeEnumerator(MyWingedEdgeMesh mesh, int index)
			{
				m_vertexIndex = index;
				VertexTableEntry vertexEntry = mesh.GetVertexEntry(m_vertexIndex);
				m_startingEdge = vertexEntry.IncidentEdge;
				m_mesh = mesh;
				m_currentEdgeIndex = INVALID_INDEX;
				m_currentEdge = default(Edge);
			}

			public bool MoveNext()
			{
				if (m_currentEdgeIndex == INVALID_INDEX)
				{
					m_currentEdgeIndex = m_startingEdge;
					m_currentEdge = m_mesh.GetEdge(m_startingEdge);
					return true;
				}
				int nextVertexEdge = m_currentEdge.GetNextVertexEdge(m_vertexIndex);
				if (nextVertexEdge == m_startingEdge)
				{
					return false;
				}
				m_currentEdgeIndex = nextVertexEdge;
				m_currentEdge = m_mesh.GetEdge(m_currentEdgeIndex);
				return true;
			}
		}

		/// <summary>
		/// Note: This is invalid after the mesh changes!
		/// </summary>
		public struct FaceEdgeEnumerator
		{
			private MyWingedEdgeMesh m_mesh;

			private int m_faceIndex;

			private int m_currentEdge;

			private int m_startingEdge;

			public int Current => m_currentEdge;

			public FaceEdgeEnumerator(MyWingedEdgeMesh mesh, int faceIndex)
			{
				m_mesh = mesh;
				m_faceIndex = faceIndex;
				m_currentEdge = INVALID_INDEX;
				m_startingEdge = m_mesh.GetFaceEntry(faceIndex).IncidentEdge;
			}

			public bool MoveNext()
			{
				if (m_currentEdge == INVALID_INDEX)
				{
					m_currentEdge = m_startingEdge;
					return true;
				}
				m_currentEdge = m_mesh.GetEdgeEntry(m_currentEdge).FaceSucc(m_faceIndex);
				return m_currentEdge != m_startingEdge;
			}
		}

		/// <summary>
		/// Note: This is invalid after the mesh changes!
		/// </summary>
		public struct FaceVertexEnumerator
		{
			private MyWingedEdgeMesh m_mesh;

			private int m_faceIndex;

			private int m_currentEdge;

			private int m_startingEdge;

			public Vector3 Current
			{
				get
				{
					EdgeTableEntry edgeEntry = m_mesh.GetEdgeEntry(m_currentEdge);
					if (m_faceIndex == edgeEntry.LeftFace)
					{
						return m_mesh.m_vertexTable[edgeEntry.Vertex2].VertexPosition;
					}
					return m_mesh.m_vertexTable[edgeEntry.Vertex1].VertexPosition;
				}
			}

			public int CurrentIndex
			{
				get
				{
					EdgeTableEntry edgeEntry = m_mesh.GetEdgeEntry(m_currentEdge);
					if (m_faceIndex == edgeEntry.LeftFace)
					{
						return edgeEntry.Vertex2;
					}
					return edgeEntry.Vertex1;
				}
			}

			public FaceVertexEnumerator(MyWingedEdgeMesh mesh, int faceIndex)
			{
				m_mesh = mesh;
				m_faceIndex = faceIndex;
				m_currentEdge = INVALID_INDEX;
				m_startingEdge = m_mesh.GetFaceEntry(faceIndex).IncidentEdge;
			}

			public bool MoveNext()
			{
				if (m_currentEdge == INVALID_INDEX)
				{
					m_currentEdge = m_startingEdge;
					return true;
				}
				m_currentEdge = m_mesh.GetEdgeEntry(m_currentEdge).FaceSucc(m_faceIndex);
				return m_currentEdge != m_startingEdge;
			}
		}

		/// <summary>
		/// Note: This is invalid after the mesh changes!
		/// </summary>
		public struct EdgeEnumerator
		{
			private int m_currentEdge;

			private HashSet<int> m_freeEdges;

			private MyWingedEdgeMesh m_mesh;

			public int CurrentIndex => m_currentEdge;

			public Edge Current => new Edge(m_mesh, m_currentEdge);

			public EdgeEnumerator(MyWingedEdgeMesh mesh, HashSet<int> preallocatedHelperHashSet = null)
			{
				m_currentEdge = -1;
				m_freeEdges = (preallocatedHelperHashSet ?? new HashSet<int>());
				m_mesh = mesh;
				m_freeEdges.Clear();
				for (int num = mesh.m_freeEdges; num != INVALID_INDEX; num = m_mesh.m_edgeTable[num].NextFreeEntry)
				{
					m_freeEdges.Add(num);
				}
			}

			public bool MoveNext()
			{
				int count = m_mesh.m_edgeTable.Count;
				do
				{
					m_currentEdge++;
				}
				while (m_freeEdges.Contains(m_currentEdge) && m_currentEdge < count);
				return m_currentEdge < count;
			}

			public void Dispose()
			{
				m_freeEdges.Clear();
				m_freeEdges = null;
			}
		}

		public static bool BASIC_CONSISTENCY_CHECKS = false;

		public static bool ADVANCED_CONSISTENCY_CHECKS = false;

		public static int INVALID_INDEX = -1;

		private static HashSet<int> m_tmpFreeEdges = new HashSet<int>();

		private static HashSet<int> m_tmpFreeFaces = new HashSet<int>();

		private static HashSet<int> m_tmpFreeVertices = new HashSet<int>();

		private static HashSet<int> m_tmpVisitedIndices = new HashSet<int>();

		private static HashSet<int> m_tmpDebugDrawFreeIndices = new HashSet<int>();

		private static List<int> m_tmpIndexList = new List<int>();

		private List<EdgeTableEntry> m_edgeTable;

		private List<VertexTableEntry> m_vertexTable;

		private List<FaceTableEntry> m_faceTable;

		private int m_freeEdges;

		private int m_freeVertices;

		private int m_freeFaces;

		private static HashSet<int> m_debugDrawEdges = null;

		public static void DebugDrawEdgesReset()
		{
			if (m_debugDrawEdges == null)
			{
				m_debugDrawEdges = new HashSet<int>();
			}
			else if (m_debugDrawEdges.Count == 0)
			{
				m_debugDrawEdges = null;
			}
			else
			{
				m_debugDrawEdges.Clear();
			}
		}

		public static void DebugDrawEdgesAdd(int edgeIndex)
		{
			if (m_debugDrawEdges != null)
			{
				m_debugDrawEdges.Add(edgeIndex);
			}
		}

		private EdgeTableEntry GetEdgeEntry(int index)
		{
			return m_edgeTable[index];
		}

		private void SetEdgeEntry(int index, ref EdgeTableEntry entry)
		{
			m_edgeTable[index] = entry;
		}

		private FaceTableEntry GetFaceEntry(int index)
		{
			return m_faceTable[index];
		}

		private void SetFaceEntry(int index, FaceTableEntry entry)
		{
			m_faceTable[index] = entry;
		}

		private VertexTableEntry GetVertexEntry(int index)
		{
			return m_vertexTable[index];
		}

		public MyWingedEdgeMesh()
		{
			m_edgeTable = new List<EdgeTableEntry>();
			m_vertexTable = new List<VertexTableEntry>();
			m_faceTable = new List<FaceTableEntry>();
			m_freeEdges = INVALID_INDEX;
			m_freeVertices = INVALID_INDEX;
			m_freeFaces = INVALID_INDEX;
		}

		/// <summary>
		/// For testing purposes only! The copy is only a shallow copy (i.e. userdata is not copied)
		/// </summary>
		public MyWingedEdgeMesh Copy()
		{
			return new MyWingedEdgeMesh
			{
				m_freeEdges = m_freeEdges,
				m_freeFaces = m_freeFaces,
				m_freeVertices = m_freeVertices,
				m_edgeTable = m_edgeTable.ToList(),
				m_vertexTable = m_vertexTable.ToList(),
				m_faceTable = m_faceTable.ToList()
			};
		}

		public void Transform(Matrix transformation)
		{
			m_tmpFreeVertices.Clear();
			for (int num = m_freeVertices; num != INVALID_INDEX; num = m_vertexTable[num].NextFreeEntry)
			{
				m_tmpFreeVertices.Add(num);
			}
			for (int num = 0; num < m_vertexTable.Count; num++)
			{
				if (!m_tmpFreeVertices.Contains(num))
				{
					VertexTableEntry value = m_vertexTable[num];
					Vector3.Transform(ref value.VertexPosition, ref transformation, out value.VertexPosition);
					m_vertexTable[num] = value;
				}
			}
		}

		public Edge GetEdge(int edgeIndex)
		{
			return new Edge(this, edgeIndex);
		}

		public EdgeEnumerator GetEdges(HashSet<int> preallocatedHelperHashset = null)
		{
			return new EdgeEnumerator(this, preallocatedHelperHashset);
		}

		public Face GetFace(int faceIndex)
		{
			return new Face(this, faceIndex);
		}

		public VertexEdgeEnumerator GetVertexEdges(int vertexIndex)
		{
			return new VertexEdgeEnumerator(this, vertexIndex);
		}

		public Vector3 GetVertexPosition(int vertexIndex)
		{
			return m_vertexTable[vertexIndex].VertexPosition;
		}

		/// <summary>
		/// Creates a new face by closing the gap between vertices vert1 and vert2 by a new edge
		/// </summary>
		/// <param name="vert1">Point that will be shared by the new edge and edge1</param>
		/// <param name="vert2">Point that will be shared by the new edge and edge2</param>
		/// <param name="edge1">Predecessor of the new edge</param>
		/// <param name="edge2">Successor of the new edge</param>
		/// <param name="faceUserData">User data for the newly created face</param>
		public int MakeEdgeFace(int vert1, int vert2, int edge1, int edge2, object faceUserData, out int newEdge)
		{
			newEdge = AllocateEdge();
			int num = AllocateAndInsertFace(faceUserData, newEdge);
			EdgeTableEntry entry = GetEdgeEntry(edge1);
			EdgeTableEntry entry2 = GetEdgeEntry(edge2);
			EdgeTableEntry entry3 = default(EdgeTableEntry);
			entry3.Init();
			entry3.Vertex1 = vert1;
			entry3.Vertex2 = vert2;
			entry3.RightFace = num;
			entry.AddFace(num);
			entry2.AddFace(num);
			int num2 = entry2.OtherVertex(vert2);
			int num3 = entry2.VertexSucc(num2);
			while (num3 != edge1)
			{
				EdgeTableEntry entry4 = GetEdgeEntry(num3);
				entry4.AddFace(num);
				SetEdgeEntry(num3, ref entry4);
				num2 = entry4.OtherVertex(num2);
				num3 = entry4.VertexSucc(num2);
			}
			entry3.SetVertexSucc(vert2, edge2);
			int num4 = entry2.SetVertexPred(vert2, newEdge);
			entry3.SetVertexPred(vert1, edge1);
			int num5 = entry.SetVertexSucc(vert1, newEdge);
			EdgeTableEntry entry5 = GetEdgeEntry(num5);
			EdgeTableEntry entry6 = default(EdgeTableEntry);
			if (num5 != num4)
			{
				entry6 = GetEdgeEntry(num4);
			}
			entry5.SetVertexPred(vert1, newEdge);
			entry3.SetVertexSucc(vert1, num5);
			if (num5 != num4)
			{
				entry6.SetVertexSucc(vert2, newEdge);
			}
			else
			{
				entry5.SetVertexSucc(vert2, newEdge);
			}
			entry3.SetVertexPred(vert2, num4);
			SetEdgeEntry(num5, ref entry5);
			if (num5 != num4)
			{
				SetEdgeEntry(num4, ref entry6);
			}
			SetEdgeEntry(newEdge, ref entry3);
			SetEdgeEntry(edge1, ref entry);
			SetEdgeEntry(edge2, ref entry2);
			return num;
		}

		/// <summary>
		/// Merges two edges together into one. These edges have to border on the edge of the mesh (i.e. face -1)
		/// Note that this also merges the corresponding vertices!
		/// </summary>
		/// <param name="edge1">The edge that will be merged</param>
		/// <param name="edge2">The edge that will be kept</param>
		public void MergeEdges(int edge1, int edge2)
		{
			EdgeTableEntry edgeEntry = GetEdgeEntry(edge1);
			EdgeTableEntry entry = GetEdgeEntry(edge2);
			edgeEntry.GetFaceVertices(INVALID_INDEX, out int predVertex, out int succVertex);
			entry.GetFaceVertices(INVALID_INDEX, out int predVertex2, out int succVertex2);
			int num = edgeEntry.OtherFace(INVALID_INDEX);
			int num2 = edgeEntry.FaceSucc(INVALID_INDEX);
			int num3 = edgeEntry.FacePred(INVALID_INDEX);
			int num4 = entry.FaceSucc(INVALID_INDEX);
			int num5 = entry.FacePred(INVALID_INDEX);
			int num6 = edgeEntry.FaceSucc(num);
			int num7 = edgeEntry.FacePred(num);
			EdgeTableEntry entry2 = GetEdgeEntry(num2);
			EdgeTableEntry entry3 = default(EdgeTableEntry);
			if (num2 != num3)
			{
				entry3 = GetEdgeEntry(num3);
			}
			EdgeTableEntry entry4 = GetEdgeEntry(num4);
			EdgeTableEntry entry5 = default(EdgeTableEntry);
			if (num4 != num5)
			{
				entry5 = GetEdgeEntry(num5);
			}
			entry2.SetFacePred(INVALID_INDEX, num5);
			entry4.SetFacePred(INVALID_INDEX, num3);
			if (num2 != num3)
			{
				entry3.SetFaceSucc(INVALID_INDEX, num4);
			}
			else
			{
				entry2.SetFaceSucc(INVALID_INDEX, num4);
			}
			if (num4 != num5)
			{
				entry5.SetFaceSucc(INVALID_INDEX, num2);
			}
			else
			{
				entry4.SetFaceSucc(INVALID_INDEX, num2);
			}
			entry.AddFace(num);
			entry.SetFacePred(num, num7);
			entry.SetFaceSucc(num, num6);
			entry2.SetFacePredVertex(INVALID_INDEX, predVertex2);
			if (num2 != num3)
			{
				entry3.SetFaceSuccVertex(INVALID_INDEX, succVertex2);
			}
			else
			{
				entry2.SetFaceSuccVertex(INVALID_INDEX, succVertex2);
			}
			if (num7 == num2)
			{
				entry2.SetFaceSucc(num, edge2);
			}
			else
			{
				EdgeTableEntry entry6 = GetEdgeEntry(num7);
				entry6.SetFaceSucc(num, edge2);
				entry6.SetFaceSuccVertex(num, predVertex2);
				SetEdgeEntry(num7, ref entry6);
				for (num7 = entry6.VertexPred(predVertex2); num7 != num2; num7 = entry6.VertexPred(predVertex2))
				{
					entry6 = GetEdgeEntry(num7);
					entry6.ChangeVertex(succVertex, predVertex2);
					SetEdgeEntry(num7, ref entry6);
				}
			}
			if (num6 == num3)
			{
				if (num2 != num3)
				{
					entry3.SetFacePred(num, edge2);
				}
				else
				{
					entry2.SetFacePred(num, edge2);
				}
			}
			else
			{
				EdgeTableEntry entry7 = GetEdgeEntry(num6);
				entry7.SetFacePred(num, edge2);
				entry7.SetFacePredVertex(num, succVertex2);
				SetEdgeEntry(num6, ref entry7);
				for (num6 = entry7.VertexSucc(succVertex2); num6 != num3; num6 = entry7.VertexSucc(succVertex2))
				{
					entry7 = GetEdgeEntry(num6);
					entry7.ChangeVertex(predVertex, succVertex2);
					SetEdgeEntry(num6, ref entry7);
				}
			}
			FaceTableEntry faceEntry = GetFaceEntry(num);
			faceEntry.IncidentEdge = edge2;
			SetFaceEntry(num, faceEntry);
			DeallocateVertex(predVertex);
			DeallocateVertex(succVertex);
			DeallocateEdge(edge1);
			SetEdgeEntry(edge2, ref entry);
			SetEdgeEntry(num2, ref entry2);
			if (num2 != num3)
			{
				SetEdgeEntry(num3, ref entry3);
			}
			SetEdgeEntry(num4, ref entry4);
			if (num4 != num5)
			{
				SetEdgeEntry(num5, ref entry5);
			}
		}

		/// <summary>
		/// Creates a new triangle by adding a vertex to an existing edge
		/// </summary>
		/// <param name="newVertex">Position of the new vertex</param>
		/// <param name="edge">The edge from which we want to extrude</param>
		/// <param name="faceUserData">User data that will be saved in the face</param>
		/// <param name="newEdgeS">Index of the new edge that follows edge "edge" in the new triangle.</param>
		/// <param name="newEdgeP">Index of the new edge that precedes edge "edge" in the new triangle.</param>
		/// <returns></returns>
		public int ExtrudeTriangleFromEdge(ref Vector3 newVertex, int edge, object faceUserData, out int newEdgeS, out int newEdgeP)
		{
			EdgeTableEntry entry = GetEdgeEntry(edge);
			newEdgeP = AllocateEdge();
			newEdgeS = AllocateEdge();
			int num = entry.FacePred(INVALID_INDEX);
			int num2 = entry.FaceSucc(INVALID_INDEX);
			entry.GetFaceVertices(INVALID_INDEX, out int predVertex, out int succVertex);
			EdgeTableEntry entry2 = GetEdgeEntry(num);
			EdgeTableEntry entry3 = GetEdgeEntry(num2);
			EdgeTableEntry entry4 = default(EdgeTableEntry);
			entry4.Init();
			EdgeTableEntry entry5 = default(EdgeTableEntry);
			entry5.Init();
			int num3 = AllocateAndInsertFace(faceUserData, newEdgeP);
			int vertex = AllocateAndInsertVertex(ref newVertex, newEdgeP);
			entry4.AddFace(num3);
			entry4.SetFacePredVertex(num3, vertex);
			entry4.SetFacePred(num3, newEdgeS);
			entry4.SetFaceSuccVertex(num3, predVertex);
			entry4.SetFaceSucc(num3, edge);
			entry4.SetFacePred(INVALID_INDEX, num);
			entry4.SetFaceSucc(INVALID_INDEX, newEdgeS);
			entry5.AddFace(num3);
			entry5.SetFacePredVertex(num3, succVertex);
			entry5.SetFacePred(num3, edge);
			entry5.SetFaceSuccVertex(num3, vertex);
			entry5.SetFaceSucc(num3, newEdgeP);
			entry5.SetFacePred(INVALID_INDEX, newEdgeP);
			entry5.SetFaceSucc(INVALID_INDEX, num2);
			entry.AddFace(num3);
			entry.SetFacePred(num3, newEdgeP);
			entry.SetFaceSucc(num3, newEdgeS);
			entry2.SetFaceSucc(INVALID_INDEX, newEdgeP);
			entry3.SetFacePred(INVALID_INDEX, newEdgeS);
			SetEdgeEntry(newEdgeP, ref entry4);
			SetEdgeEntry(newEdgeS, ref entry5);
			SetEdgeEntry(num, ref entry2);
			SetEdgeEntry(num2, ref entry3);
			SetEdgeEntry(edge, ref entry);
			return num3;
		}

		public void MergeAngle(int leftEdge, int rightEdge, int commonVert)
		{
			EdgeTableEntry edgeEntry = GetEdgeEntry(leftEdge);
			EdgeTableEntry entry = GetEdgeEntry(rightEdge);
			int num = edgeEntry.FaceSucc(INVALID_INDEX);
			int num2 = entry.FacePred(INVALID_INDEX);
			int num3 = edgeEntry.OtherVertex(commonVert);
			int num4 = entry.OtherVertex(commonVert);
			int num5 = edgeEntry.OtherFace(INVALID_INDEX);
			int num6 = edgeEntry.FaceSucc(num5);
			int num7 = edgeEntry.FacePred(num5);
			EdgeTableEntry entry2 = GetEdgeEntry(num6);
			EdgeTableEntry entry3 = GetEdgeEntry(num);
			EdgeTableEntry entry4 = GetEdgeEntry(num2);
			entry3.SetFacePredVertex(INVALID_INDEX, num4);
			entry3.SetFacePred(INVALID_INDEX, num2);
			entry4.SetFaceSucc(INVALID_INDEX, num);
			if (num7 == num)
			{
				entry3.SetFaceSucc(num5, rightEdge);
			}
			else
			{
				EdgeTableEntry entry5 = GetEdgeEntry(num7);
				entry5.SetFaceSucc(num5, rightEdge);
				entry5.ChangeVertex(num3, num4);
				SetEdgeEntry(num7, ref entry5);
				for (int num8 = entry5.VertexPred(num4); num8 != num; num8 = entry5.VertexPred(num4))
				{
					entry5 = GetEdgeEntry(num8);
					entry5.ChangeVertex(num3, num4);
					SetEdgeEntry(num8, ref entry5);
				}
			}
			entry.AddFace(num5);
			entry.SetFacePred(num5, num7);
			entry.SetFaceSucc(num5, num6);
			entry2.SetFacePred(num5, rightEdge);
			VertexTableEntry value = m_vertexTable[commonVert];
			value.IncidentEdge = rightEdge;
			m_vertexTable[commonVert] = value;
			FaceTableEntry faceEntry = GetFaceEntry(num5);
			faceEntry.IncidentEdge = rightEdge;
			SetFaceEntry(num5, faceEntry);
			DeallocateEdge(leftEdge);
			DeallocateVertex(num3);
			SetEdgeEntry(rightEdge, ref entry);
			SetEdgeEntry(num, ref entry3);
			SetEdgeEntry(num6, ref entry2);
			SetEdgeEntry(num2, ref entry4);
		}

		/// <summary>
		/// Makes a face by filling in the empty edge loop incident to incidentEdge
		/// </summary>
		/// <param name="userData"></param>
		/// <param name="incidentEdge"></param>
		/// <returns></returns>
		public int MakeFace(object userData, int incidentEdge)
		{
			int num = AllocateAndInsertFace(userData, incidentEdge);
			int num2 = incidentEdge;
			do
			{
				EdgeTableEntry entry = GetEdgeEntry(num2);
				entry.AddFace(num);
				SetEdgeEntry(num2, ref entry);
				num2 = entry.FaceSucc(num);
			}
			while (num2 != incidentEdge);
			return num;
		}

		public int MakeNewTriangle(object userData, ref Vector3 A, ref Vector3 B, ref Vector3 C, out int edgeAB, out int edgeBC, out int edgeCA)
		{
			edgeAB = AllocateEdge();
			edgeBC = AllocateEdge();
			edgeCA = AllocateEdge();
			int vertex = AllocateAndInsertVertex(ref A, edgeAB);
			int vertex2 = AllocateAndInsertVertex(ref B, edgeBC);
			int vertex3 = AllocateAndInsertVertex(ref C, edgeCA);
			int num = AllocateAndInsertFace(userData, edgeAB);
			EdgeTableEntry entry = default(EdgeTableEntry);
			entry.Init();
			EdgeTableEntry entry2 = default(EdgeTableEntry);
			entry2.Init();
			EdgeTableEntry entry3 = default(EdgeTableEntry);
			entry3.Init();
			entry.AddFace(num);
			entry2.AddFace(num);
			entry3.AddFace(num);
			entry.SetFaceSuccVertex(num, vertex2);
			entry2.SetFaceSuccVertex(num, vertex3);
			entry3.SetFaceSuccVertex(num, vertex);
			entry.SetFacePredVertex(num, vertex);
			entry2.SetFacePredVertex(num, vertex2);
			entry3.SetFacePredVertex(num, vertex3);
			entry.SetFaceSucc(num, edgeBC);
			entry2.SetFaceSucc(num, edgeCA);
			entry3.SetFaceSucc(num, edgeAB);
			entry.SetFacePred(num, edgeCA);
			entry2.SetFacePred(num, edgeAB);
			entry3.SetFacePred(num, edgeBC);
			entry.SetFaceSucc(INVALID_INDEX, edgeCA);
			entry2.SetFaceSucc(INVALID_INDEX, edgeAB);
			entry3.SetFaceSucc(INVALID_INDEX, edgeBC);
			entry.SetFacePred(INVALID_INDEX, edgeBC);
			entry2.SetFacePred(INVALID_INDEX, edgeCA);
			entry3.SetFacePred(INVALID_INDEX, edgeAB);
			SetEdgeEntry(edgeAB, ref entry);
			SetEdgeEntry(edgeBC, ref entry2);
			SetEdgeEntry(edgeCA, ref entry3);
			return num;
		}

		public int MakeNewPoly(object userData, List<Vector3> points, List<int> outEdges)
		{
			if (outEdges.Count != 0 || points.Count < 3)
			{
				return INVALID_INDEX;
			}
			m_tmpIndexList.Clear();
			int num = INVALID_INDEX;
			for (int i = 0; i < points.Count; i++)
			{
				Vector3 position = points[i];
				num = AllocateEdge();
				outEdges.Add(num);
				m_tmpIndexList.Add(AllocateAndInsertVertex(ref position, num));
			}
			int num2 = AllocateAndInsertFace(userData, num);
			int num3 = outEdges[points.Count - 1];
			num = outEdges[0];
			int vertex = m_tmpIndexList[0];
			for (int j = 0; j < points.Count; j++)
			{
				int num4;
				int num5;
				if (j != points.Count - 1)
				{
					num4 = outEdges[j + 1];
					num5 = m_tmpIndexList[j + 1];
				}
				else
				{
					num4 = outEdges[0];
					num5 = m_tmpIndexList[0];
				}
				EdgeTableEntry entry = default(EdgeTableEntry);
				entry.Init();
				entry.AddFace(num2);
				entry.SetFacePred(num2, num3);
				entry.SetFaceSucc(num2, num4);
				entry.SetFacePred(INVALID_INDEX, num4);
				entry.SetFaceSucc(INVALID_INDEX, num3);
				entry.SetFacePredVertex(num2, vertex);
				entry.SetFaceSuccVertex(num2, num5);
				SetEdgeEntry(num, ref entry);
				num3 = num;
				num = num4;
				vertex = num5;
			}
			return num2;
		}

		public void RemoveFace(int faceIndex)
		{
			int num = GetFaceEntry(faceIndex).IncidentEdge;
			int num2 = num;
			bool flag = false;
			EdgeTableEntry entry = GetEdgeEntry(num);
			EdgeTableEntry edgeEntry = GetEdgeEntry(num2);
			entry.GetFaceVertices(faceIndex, out int predVertex, out int succVertex);
			do
			{
				int num3 = entry.FaceSucc(faceIndex);
				predVertex = succVertex;
				EdgeTableEntry entry2 = (num3 == num2 && flag) ? edgeEntry : GetEdgeEntry(num3);
				succVertex = entry2.OtherVertex(succVertex);
				if (entry.VertexLeftFace(predVertex) == INVALID_INDEX)
				{
					if (num == num2)
					{
						flag = true;
					}
					DeallocateEdge(num);
					if (entry2.VertexLeftFace(succVertex) == INVALID_INDEX)
					{
						if (entry2.VertexSucc(predVertex) == num)
						{
							DeallocateVertex(predVertex);
						}
						else
						{
							int num4 = entry2.VertexSucc(predVertex);
							int num5 = entry.VertexPred(predVertex);
							EdgeTableEntry entry3 = GetEdgeEntry(num4);
							EdgeTableEntry entry4 = GetEdgeEntry(num5);
							entry3.SetVertexPred(predVertex, num5);
							entry4.SetVertexSucc(predVertex, num4);
							SetEdgeEntry(num5, ref entry4);
							SetEdgeEntry(num4, ref entry3);
							VertexTableEntry value = m_vertexTable[predVertex];
							value.IncidentEdge = num5;
							m_vertexTable[predVertex] = value;
						}
					}
					else
					{
						int num6 = entry.FacePred(INVALID_INDEX);
						EdgeTableEntry entry5 = GetEdgeEntry(num6);
						entry5.SetFaceSucc(INVALID_INDEX, num3);
						if (num3 != num2)
						{
							entry2.SetFacePred(faceIndex, num6);
						}
						else
						{
							entry2.SetFacePred(INVALID_INDEX, num6);
						}
						SetEdgeEntry(num6, ref entry5);
						SetEdgeEntry(num3, ref entry2);
						VertexTableEntry value2 = m_vertexTable[predVertex];
						value2.IncidentEdge = num3;
						m_vertexTable[predVertex] = value2;
					}
				}
				else if (entry2.VertexLeftFace(succVertex) == INVALID_INDEX)
				{
					int num7 = entry2.FaceSucc(INVALID_INDEX);
					EdgeTableEntry entry6 = GetEdgeEntry(num7);
					entry6.SetFacePred(INVALID_INDEX, num);
					entry.SetFaceSucc(faceIndex, num7);
					entry.ChangeFace(faceIndex, INVALID_INDEX);
					SetEdgeEntry(num7, ref entry6);
					SetEdgeEntry(num, ref entry);
					VertexTableEntry value3 = m_vertexTable[predVertex];
					value3.IncidentEdge = num;
					m_vertexTable[predVertex] = value3;
				}
				else
				{
					int num8 = entry2.VertexSucc(predVertex);
					while (num8 != num)
					{
						EdgeTableEntry entry7 = GetEdgeEntry(num8);
						if (entry7.VertexRightFace(predVertex) == INVALID_INDEX)
						{
							int num9 = entry7.VertexSucc(predVertex);
							VertexTableEntry value4 = m_vertexTable[predVertex];
							Vector3 position = value4.VertexPosition;
							value4.IncidentEdge = num3;
							m_vertexTable[predVertex] = value4;
							int num10 = AllocateAndInsertVertex(ref position, num9);
							EdgeTableEntry entry8 = GetEdgeEntry(num9);
							entry8.SetVertexPred(predVertex, num);
							entry.SetVertexSucc(predVertex, num9);
							entry.ChangeVertex(predVertex, num10);
							while (true)
							{
								entry8.ChangeVertex(predVertex, num10);
								SetEdgeEntry(num9, ref entry8);
								num9 = entry8.VertexSucc(num10);
								if (num9 == num)
								{
									break;
								}
								entry8 = GetEdgeEntry(num9);
							}
							entry7.SetVertexSucc(predVertex, num3);
							entry2.SetVertexPred(predVertex, num8);
							SetEdgeEntry(num8, ref entry7);
							SetEdgeEntry(num3, ref entry2);
							break;
						}
						num8 = entry7.VertexSucc(predVertex);
					}
					entry.ChangeFace(faceIndex, INVALID_INDEX);
					SetEdgeEntry(num, ref entry);
				}
				num = num3;
				entry = entry2;
			}
			while (num != num2);
			DeallocateFace(faceIndex);
		}

		public bool IntersectEdge(ref Edge edge, ref Plane p, out Vector3 intersection)
		{
			intersection = default(Vector3);
			Ray output = default(Ray);
			edge.ToRay(this, ref output);
			float? num = output.Intersects(p);
			if (!num.HasValue)
			{
				return false;
			}
			float value = num.Value;
			if (value < 0f || value > 1f)
			{
				return false;
			}
			intersection = output.Position + value * output.Direction;
			return true;
		}

		/// <summary>
		/// Sorts the list of free faces. This ensures that subsequent face allocations will return increasing sequence of face indices,
		/// unless interrupted by face deallocation. This can be useful in some algorithms that rely on ordering of the face indices.
		/// </summary>
		public void SortFreeFaces()
		{
			if (m_freeFaces != INVALID_INDEX)
			{
				m_tmpIndexList.Clear();
				for (int num = m_freeFaces; num != INVALID_INDEX; num = m_faceTable[num].NextFreeEntry)
				{
					m_tmpIndexList.Add(num);
				}
				m_tmpIndexList.Sort();
				m_freeFaces = m_tmpIndexList[0];
				for (int i = 0; i < m_tmpIndexList.Count - 1; i++)
				{
					FaceTableEntry value = m_faceTable[m_tmpIndexList[i]];
					value.NextFreeEntry = m_tmpIndexList[i + 1];
					m_faceTable[m_tmpIndexList[i]] = value;
				}
				FaceTableEntry value2 = m_faceTable[m_tmpIndexList[m_tmpIndexList.Count - 1]];
				value2.NextFreeEntry = INVALID_INDEX;
				m_faceTable[m_tmpIndexList[m_tmpIndexList.Count - 1]] = value2;
				m_tmpIndexList.Clear();
			}
		}

		private int AllocateAndInsertFace(object userData, int incidentEdge)
		{
			FaceTableEntry faceTableEntry = default(FaceTableEntry);
			faceTableEntry.IncidentEdge = incidentEdge;
			faceTableEntry.UserData = userData;
			FaceTableEntry faceTableEntry2 = faceTableEntry;
			if (m_freeFaces == INVALID_INDEX)
			{
				int count = m_faceTable.Count;
				m_faceTable.Add(faceTableEntry2);
				return count;
			}
			int freeFaces = m_freeFaces;
			faceTableEntry = m_faceTable[m_freeFaces];
			m_freeFaces = faceTableEntry.NextFreeEntry;
			m_faceTable[freeFaces] = faceTableEntry2;
			return freeFaces;
		}

		private int AllocateAndInsertVertex(ref Vector3 position, int incidentEdge)
		{
			VertexTableEntry vertexTableEntry = default(VertexTableEntry);
			vertexTableEntry.IncidentEdge = incidentEdge;
			vertexTableEntry.VertexPosition = position;
			VertexTableEntry vertexTableEntry2 = vertexTableEntry;
			if (m_freeVertices == INVALID_INDEX)
			{
				int count = m_vertexTable.Count;
				m_vertexTable.Add(vertexTableEntry2);
				return count;
			}
			int freeVertices = m_freeVertices;
			if (freeVertices < 0 || freeVertices >= m_vertexTable.Count)
			{
				m_freeVertices = -1;
				return AllocateAndInsertVertex(ref position, incidentEdge);
			}
			vertexTableEntry = m_vertexTable[freeVertices];
			m_freeVertices = vertexTableEntry.NextFreeEntry;
			m_vertexTable[freeVertices] = vertexTableEntry2;
			return freeVertices;
		}

		private int AllocateEdge()
		{
			EdgeTableEntry edgeTableEntry = default(EdgeTableEntry);
			edgeTableEntry.Init();
			if (m_freeEdges == INVALID_INDEX)
			{
				int count = m_edgeTable.Count;
				m_edgeTable.Add(edgeTableEntry);
				return count;
			}
			int freeEdges = m_freeEdges;
			m_freeEdges = m_edgeTable[m_freeEdges].NextFreeEntry;
			m_edgeTable[freeEdges] = edgeTableEntry;
			return freeEdges;
		}

		private void DeallocateFace(int faceIndex)
		{
			FaceTableEntry value = default(FaceTableEntry);
			value.NextFreeEntry = m_freeFaces;
			m_faceTable[faceIndex] = value;
			m_freeFaces = faceIndex;
		}

		private void DeallocateVertex(int vertexIndex)
		{
			VertexTableEntry value = default(VertexTableEntry);
			value.NextFreeEntry = m_freeVertices;
			m_vertexTable[vertexIndex] = value;
			m_freeVertices = vertexIndex;
		}

		private void DeallocateEdge(int edgeIndex)
		{
			EdgeTableEntry value = default(EdgeTableEntry);
			value.NextFreeEntry = m_freeEdges;
			m_edgeTable[edgeIndex] = value;
			m_freeEdges = edgeIndex;
		}

		public void DebugDraw(ref Matrix drawMatrix, MyWEMDebugDrawMode draw)
		{
			m_tmpDebugDrawFreeIndices.Clear();
			for (int num = m_freeEdges; num != INVALID_INDEX; num = m_edgeTable[num].NextFreeEntry)
			{
				m_tmpDebugDrawFreeIndices.Add(num);
			}
			for (int i = 0; i < m_edgeTable.Count; i++)
			{
				if (m_tmpDebugDrawFreeIndices.Contains(i) || (m_debugDrawEdges != null && !m_debugDrawEdges.Contains(i)))
				{
					continue;
				}
				EdgeTableEntry edgeEntry = GetEdgeEntry(i);
				Vector3 vector = Vector3.Transform(m_vertexTable[edgeEntry.Vertex1].VertexPosition, drawMatrix);
				Vector3 vector2 = Vector3.Transform(m_vertexTable[edgeEntry.Vertex2].VertexPosition, drawMatrix);
				Vector3 vector3 = (vector + vector2) * 0.5f;
				EdgeTableEntry edgeEntry2 = GetEdgeEntry(edgeEntry.LeftSucc);
				EdgeTableEntry edgeEntry3 = GetEdgeEntry(edgeEntry.RightPred);
				EdgeTableEntry edgeEntry4 = GetEdgeEntry(edgeEntry.LeftPred);
				EdgeTableEntry edgeEntry5 = GetEdgeEntry(edgeEntry.RightSucc);
				Vector3 vector4 = Vector3.Transform(m_vertexTable[edgeEntry2.OtherVertex(edgeEntry.Vertex1)].VertexPosition, drawMatrix);
				Vector3 vector5 = Vector3.Transform(m_vertexTable[edgeEntry3.OtherVertex(edgeEntry.Vertex1)].VertexPosition, drawMatrix);
				Vector3 vector6 = Vector3.Transform(m_vertexTable[edgeEntry4.OtherVertex(edgeEntry.Vertex2)].VertexPosition, drawMatrix);
				Vector3 vector7 = Vector3.Transform(m_vertexTable[edgeEntry5.OtherVertex(edgeEntry.Vertex2)].VertexPosition, drawMatrix);
				if ((draw & MyWEMDebugDrawMode.LINES) != 0 || (draw & MyWEMDebugDrawMode.EDGES) != 0)
				{
					bool flag = edgeEntry.LeftFace == INVALID_INDEX || edgeEntry.RightFace == INVALID_INDEX;
					Color colorFrom = ((draw & MyWEMDebugDrawMode.LINES) == 0) ? Color.Black : (flag ? Color.Red : Color.DarkSlateBlue);
					Color colorTo = ((draw & MyWEMDebugDrawMode.LINES) == 0) ? Color.White : (flag ? Color.Red : Color.DarkSlateBlue);
					MyRenderProxy.DebugDrawLine3D(vector, vector2, colorFrom, colorTo, (draw & MyWEMDebugDrawMode.LINES_DEPTH) != 0);
				}
				if ((draw & MyWEMDebugDrawMode.EDGES) != 0)
				{
					if (edgeEntry.RightFace == INVALID_INDEX)
					{
						Vector3 vector8 = vector2 - vector;
						vector8.Normalize();
						vector7 = vector6 - vector8 * Vector3.Dot(vector6 - vector2, vector8);
						vector7 = vector2 + (vector2 - vector7);
						vector5 = vector4 - vector8 * Vector3.Dot(vector4 - vector, vector8);
						vector5 = vector + (vector - vector5);
					}
					if (edgeEntry.LeftFace == INVALID_INDEX)
					{
						Vector3 vector9 = vector - vector2;
						vector9.Normalize();
						vector6 = vector7 - vector9 * Vector3.Dot(vector7 - vector2, vector9);
						vector6 = vector2 + (vector2 - vector6);
						vector4 = vector5 - vector9 * Vector3.Dot(vector5 - vector, vector9);
						vector4 = vector + (vector - vector4);
					}
					vector4 = (vector * 0.8f + vector4 * 0.2f) * 0.5f + vector3 * 0.5f;
					vector5 = (vector * 0.8f + vector5 * 0.2f) * 0.5f + vector3 * 0.5f;
					vector6 = (vector2 * 0.8f + vector6 * 0.2f) * 0.5f + vector3 * 0.5f;
					vector7 = (vector2 * 0.8f + vector7 * 0.2f) * 0.5f + vector3 * 0.5f;
					MyRenderProxy.DebugDrawLine3D(vector3, vector4, Color.Black, Color.Gray, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector3, vector5, Color.Black, Color.Gray, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector3, vector6, Color.Black, Color.Gray, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector3, vector7, Color.Black, Color.Gray, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector3, (vector7 + vector5) * 0.5f, Color.Black, Color.Gray, depthRead: false);
					MyRenderProxy.DebugDrawLine3D(vector3, (vector6 + vector4) * 0.5f, Color.Black, Color.Gray, depthRead: false);
					MyRenderProxy.DebugDrawText3D(vector3, i.ToString(), Color.Yellow, 0.5f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					MyRenderProxy.DebugDrawText3D(vector4, edgeEntry.LeftSucc.ToString(), Color.LightYellow, 0.4f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					MyRenderProxy.DebugDrawText3D(vector5, edgeEntry.RightPred.ToString(), Color.LightYellow, 0.4f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					MyRenderProxy.DebugDrawText3D(vector6, edgeEntry.LeftPred.ToString(), Color.LightYellow, 0.4f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					MyRenderProxy.DebugDrawText3D(vector7, edgeEntry.RightSucc.ToString(), Color.LightYellow, 0.4f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					if (edgeEntry.RightFace != INVALID_INDEX)
					{
						MyRenderProxy.DebugDrawText3D((vector7 + vector5) * 0.5f, edgeEntry.RightFace.ToString(), Color.LightBlue, 0.4f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					}
					else
					{
						MyRenderProxy.DebugDrawText3D((vector7 + vector5) * 0.5f, edgeEntry.RightFace.ToString(), Color.Red, 0.8f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					}
					if (edgeEntry.LeftFace != INVALID_INDEX)
					{
						MyRenderProxy.DebugDrawText3D((vector6 + vector4) * 0.5f, edgeEntry.LeftFace.ToString(), Color.LightBlue, 0.4f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					}
					else
					{
						MyRenderProxy.DebugDrawText3D((vector6 + vector4) * 0.5f, edgeEntry.LeftFace.ToString(), Color.Red, 0.8f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					}
					MyRenderProxy.DebugDrawText3D(vector * 0.05f + vector2 * 0.95f, edgeEntry.Vertex2.ToString(), Color.LightGreen, 0.4f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
					MyRenderProxy.DebugDrawText3D(vector * 0.95f + vector2 * 0.05f, edgeEntry.Vertex1.ToString(), Color.LightGreen, 0.4f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				}
			}
			if ((draw & MyWEMDebugDrawMode.VERTICES) != 0 || (draw & MyWEMDebugDrawMode.VERTICES_DETAILED) != 0)
			{
				m_tmpDebugDrawFreeIndices.Clear();
				for (int num = m_freeVertices; num != INVALID_INDEX; num = m_vertexTable[num].NextFreeEntry)
				{
					m_tmpDebugDrawFreeIndices.Add(num);
				}
				for (int j = 0; j < m_vertexTable.Count; j++)
				{
					if (!m_tmpDebugDrawFreeIndices.Contains(j))
					{
						Vector3 v = Vector3.Transform(m_vertexTable[j].VertexPosition, drawMatrix);
						if ((draw & MyWEMDebugDrawMode.VERTICES_DETAILED) != 0)
						{
							MyRenderProxy.DebugDrawText3D(v, m_vertexTable[j].ToString(), Color.LightGreen, 0.5f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
						}
						else
						{
							MyRenderProxy.DebugDrawText3D(v, j.ToString(), Color.LightGreen, 0.5f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
						}
					}
				}
			}
			m_tmpDebugDrawFreeIndices.Clear();
		}

		public void CustomDebugDrawFaces(ref Matrix drawMatrix, MyWEMDebugDrawMode draw, Func<object, string> drawFunction)
		{
			if ((draw & MyWEMDebugDrawMode.FACES) == 0)
			{
				return;
			}
			m_tmpDebugDrawFreeIndices.Clear();
			for (int num = m_freeFaces; num != INVALID_INDEX; num = m_faceTable[num].NextFreeEntry)
			{
				m_tmpDebugDrawFreeIndices.Add(num);
			}
			for (int i = 0; i < m_faceTable.Count; i++)
			{
				if (!m_tmpDebugDrawFreeIndices.Contains(i))
				{
					Vector3 zero = Vector3.Zero;
					int num2 = 0;
					Face face = GetFace(i);
					FaceVertexEnumerator vertexEnumerator = face.GetVertexEnumerator();
					while (vertexEnumerator.MoveNext())
					{
						zero += vertexEnumerator.Current;
						num2++;
					}
					zero /= (float)num2;
					zero = Vector3.Transform(zero, drawMatrix);
					MyRenderProxy.DebugDrawText3D(zero, drawFunction(face.GetUserData<object>()), Color.CadetBlue, 0.6f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
				}
			}
		}

		[Conditional("DEBUG")]
		private void CheckVertexLoopConsistency(int vertexIndex)
		{
		}

		/// <summary>
		/// Checks for loops in the meshe's tables' freed entries
		/// </summary>
		[Conditional("DEBUG")]
		private void CheckFreeEntryConsistency()
		{
			if (BASIC_CONSISTENCY_CHECKS)
			{
				m_tmpVisitedIndices.Clear();
				int num = m_freeVertices;
				while (num != INVALID_INDEX)
				{
					num = m_vertexTable[num].NextFreeEntry;
					m_tmpVisitedIndices.Add(num);
				}
				m_tmpVisitedIndices.Clear();
				num = m_freeEdges;
				while (num != INVALID_INDEX)
				{
					num = m_edgeTable[num].NextFreeEntry;
					m_tmpVisitedIndices.Add(num);
				}
				m_tmpVisitedIndices.Clear();
				num = m_freeFaces;
				while (num != INVALID_INDEX)
				{
					num = m_faceTable[num].NextFreeEntry;
					m_tmpVisitedIndices.Add(num);
				}
				m_tmpVisitedIndices.Clear();
			}
		}

		[Conditional("DEBUG")]
		private void CheckEdgeIndexValid(int index)
		{
			if (BASIC_CONSISTENCY_CHECKS)
			{
				for (int num = m_freeEdges; num != INVALID_INDEX; num = m_edgeTable[num].NextFreeEntry)
				{
				}
			}
		}

		[Conditional("DEBUG")]
		private void CheckFaceIndexValid(int index)
		{
			if (BASIC_CONSISTENCY_CHECKS)
			{
				for (int num = m_freeFaces; num != INVALID_INDEX; num = m_faceTable[num].NextFreeEntry)
				{
				}
			}
		}

		[Conditional("DEBUG")]
		private void CheckVertexIndexValid(int index)
		{
			if (BASIC_CONSISTENCY_CHECKS)
			{
				for (int num = m_freeVertices; num != INVALID_INDEX; num = m_vertexTable[num].NextFreeEntry)
				{
				}
			}
		}

		[Conditional("DEBUG")]
		public void CheckFaceIndexValidQuick(int index)
		{
			_ = BASIC_CONSISTENCY_CHECKS;
		}

		[Conditional("DEBUG")]
		public void CheckEdgeIndexValidQuick(int index)
		{
			_ = BASIC_CONSISTENCY_CHECKS;
		}

		[Conditional("DEBUG")]
		public void CheckVertexIndexValidQuick(int index)
		{
			_ = BASIC_CONSISTENCY_CHECKS;
		}

		[Conditional("DEBUG")]
		public void PrepareFreeEdgeHashset()
		{
			m_tmpFreeEdges.Clear();
			for (int num = m_freeEdges; num != INVALID_INDEX; num = m_edgeTable[num].NextFreeEntry)
			{
				m_tmpFreeEdges.Add(num);
			}
		}

		[Conditional("DEBUG")]
		public void PrepareFreeFaceHashset()
		{
			m_tmpFreeFaces.Clear();
			for (int num = m_freeFaces; num != INVALID_INDEX; num = m_faceTable[num].NextFreeEntry)
			{
				m_tmpFreeFaces.Add(num);
			}
		}

		[Conditional("DEBUG")]
		public void PrepareFreeVertexHashset()
		{
			m_tmpFreeVertices.Clear();
			for (int num = m_freeVertices; num != INVALID_INDEX; num = m_vertexTable[num].NextFreeEntry)
			{
				m_tmpFreeVertices.Add(num);
			}
		}

		[Conditional("DEBUG")]
		public void CheckMeshConsistency()
		{
			if (!ADVANCED_CONSISTENCY_CHECKS)
			{
				return;
			}
			for (int i = 0; i < m_edgeTable.Count; i++)
			{
				if (!m_tmpFreeEdges.Contains(i))
				{
					EdgeTableEntry edgeTableEntry = m_edgeTable[i];
					_ = edgeTableEntry.LeftFace;
					_ = INVALID_INDEX;
					_ = edgeTableEntry.RightFace;
					_ = INVALID_INDEX;
					_ = edgeTableEntry.LeftPred;
					_ = edgeTableEntry.LeftSucc;
					_ = edgeTableEntry.RightPred;
					_ = edgeTableEntry.RightSucc;
				}
			}
			for (int j = 0; j < m_vertexTable.Count; j++)
			{
				if (m_tmpFreeVertices.Contains(j))
				{
					continue;
				}
				VertexTableEntry vertexTableEntry = m_vertexTable[j];
				int num = 0;
				int incidentEdge = vertexTableEntry.IncidentEdge;
				EdgeTableEntry edgeTableEntry2 = m_edgeTable[incidentEdge];
				if (edgeTableEntry2.VertexLeftFace(j) == INVALID_INDEX)
				{
					num++;
				}
				for (int num2 = edgeTableEntry2.VertexSucc(j); num2 != incidentEdge; num2 = edgeTableEntry2.VertexSucc(j))
				{
					edgeTableEntry2 = m_edgeTable[num2];
					if (edgeTableEntry2.VertexLeftFace(j) == INVALID_INDEX)
					{
						num++;
					}
				}
			}
			for (int k = 0; k < m_faceTable.Count; k++)
			{
				if (!m_tmpFreeFaces.Contains(k))
				{
					_ = m_faceTable[k];
				}
			}
		}

		public int ApproximateMemoryFootprint()
		{
			int num = MyEnvironment.Is64BitProcess ? 32 : 20;
			int num2 = MyEnvironment.Is64BitProcess ? 88 : 56;
			int num3 = MyEnvironment.Is64BitProcess ? 52 : 32;
			int num4 = 16;
			int num5 = 32;
			int num6 = (MyEnvironment.Is64BitProcess ? 8 : 12) + num2;
			return num3 + 3 * num + m_edgeTable.Capacity * num5 + m_faceTable.Capacity * num6 + m_vertexTable.Capacity * num4;
		}
	}
}
