using System;
using System.Collections.Generic;
using VRage.Library.Threading;
using VRageMath;

namespace VRage.Voxels.Mesh
{
	/// <summary>
	/// Describes an isomesh and all the data required to stitch it to other iso-meshes.
	/// </summary>
	public class MyIsoMeshStitch
	{
		public readonly MyIsoMesh Mesh;

		private Dictionary<Vector3I, ushort> m_edgeIndex;

		private SpinLock m_lock;

		private readonly MyStorageData[] m_signField = new MyStorageData[6];

		private byte[] m_signFieldCache;

		private int m_signFieldSize;

		private Vector3I m_forwardLimit;

		private readonly int m_originlaVxCnt;

		private readonly int m_originalTriangleCnt;

		/// <summary>
		/// Whether this mesh has additional vertices/triangles added for stitching.
		/// </summary>
		public bool IsStitched => m_originlaVxCnt != Mesh.VerticesCount;

		/// <summary>
		/// Create a new mesh stitch helper, meshes to be stitched need to record
		/// the sign field at the edges to properly combine them with other meshes.
		/// </summary>
		/// <param name="mesh"></param>
		/// <param name="meshContent"></param>
		public MyIsoMeshStitch(MyIsoMesh mesh, MyStorageData meshContent)
		{
			Mesh = mesh;
			SliceSignField(meshContent);
			m_originlaVxCnt = mesh.VerticesCount;
			m_originalTriangleCnt = mesh.TrianglesCount;
		}

		/// <summary>
		/// Try to retrieve the vertex at the given edge position.
		/// </summary>
		/// <param name="coord"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public bool TryGetVertex(Vector3I coord, out ushort index)
		{
			if (m_edgeIndex == null)
			{
				index = 0;
				return false;
			}
			return m_edgeIndex.TryGetValue(coord, out index);
		}

		public void IndexEdges()
		{
			lock (this)
			{
				if (m_edgeIndex == null)
				{
					m_edgeIndex = new Dictionary<Vector3I, ushort>(Vector3I.Comparer);
					for (int i = 0; i < Mesh.Cells.Count; i++)
					{
						Vector3I edge = Mesh.Cells[i];
						if (IsEdge(ref edge))
						{
							m_edgeIndex[edge] = (ushort)i;
						}
					}
				}
			}
		}

		internal void AddVertexToIndex(ushort vx)
		{
		}

		public bool IsEdge(ref Vector3I edge)
		{
			Vector3I vector3I = Mesh.CellEnd - Mesh.CellStart - 1;
			if (edge.X != 0 && edge.X != vector3I.X && edge.Y != 0 && edge.Y != vector3I.Y && edge.Z != 0)
			{
				return edge.Z == vector3I.Z;
			}
			return true;
		}

		internal ushort WriteVertex(ref Vector3I cell, ref Vector3 pos, ref Vector3 normal, byte material, uint colorShift)
		{
			m_lock.Enter();
			try
			{
				return (ushort)Mesh.WriteVertex(ref cell, ref pos, ref normal, material, colorShift);
			}
			finally
			{
				m_lock.Exit();
			}
		}

		internal void WriteTriangle(ushort v2, ushort v1, ushort v0)
		{
			m_lock.Enter();
			try
			{
				Mesh.WriteTriangle(v2, v1, v0);
			}
			finally
			{
				m_lock.Exit();
			}
		}

		private void SliceSignField(MyStorageData field)
		{
			Vector3I a = field.Size3D - 1;
			m_forwardLimit = a - 1;
			m_signFieldSize = a.X;
			int num = a.X - 2;
			int num2 = m_signFieldSize * m_signFieldSize * 3 + m_signFieldSize * num * 3 + num * num * 3;
			m_signFieldCache = new byte[num2];
			for (int i = 0; i < 6; i++)
			{
				m_signField[i] = new MyStorageData();
			}
			ExtractRange(GetBorderSignField(0, side: false), field, new Vector3I(0, 0, 0), new Vector3I(0, a.Y, a.Z));
			ExtractRange(GetBorderSignField(0, side: true), field, new Vector3I(a.X - 1, 0, 0), new Vector3I(a.X, a.Y, a.Z));
			ExtractRange(GetBorderSignField(1, side: false), field, new Vector3I(0, 0, 0), new Vector3I(a.X, 0, a.Z));
			ExtractRange(GetBorderSignField(1, side: true), field, new Vector3I(0, a.Y - 1, 0), new Vector3I(a.X, a.Y, a.Z));
			ExtractRange(GetBorderSignField(2, side: false), field, new Vector3I(0, 0, 0), new Vector3I(a.X, a.Y, 0));
			ExtractRange(GetBorderSignField(2, side: true), field, new Vector3I(0, 0, a.Z - 1), new Vector3I(a.X, a.Y, a.Z));
		}

		private void ExtractRange(MyStorageData data, MyStorageData field, Vector3I min, Vector3I max)
		{
			data.Resize(max - min + 1);
			data.CopyRange(field, min, max, Vector3I.Zero, MyStorageDataTypeEnum.Content);
			data.CopyRange(field, min, max, Vector3I.Zero, MyStorageDataTypeEnum.Material);
		}

		public MyStorageData GetBorderSignField(int axis, bool side)
		{
			return m_signField[(axis << 1) + (side ? 1 : 0)];
		}

		public void SampleEdge(Vector3I localPosition, out byte material, out byte content)
		{
			MyStorageData borderSignField;
			if (localPosition.X == 0)
			{
				borderSignField = GetBorderSignField(0, side: false);
			}
			else if (localPosition.X >= m_forwardLimit.X)
			{
				borderSignField = GetBorderSignField(0, side: true);
				localPosition.X -= m_forwardLimit.X;
			}
			else if (localPosition.Y == 0)
			{
				borderSignField = GetBorderSignField(1, side: false);
			}
			else if (localPosition.Y >= m_forwardLimit.Y)
			{
				borderSignField = GetBorderSignField(1, side: true);
				localPosition.Y -= m_forwardLimit.Y;
			}
			else if (localPosition.Z == 0)
			{
				borderSignField = GetBorderSignField(2, side: false);
			}
			else
			{
				if (localPosition.Z < m_forwardLimit.Z)
				{
					throw new InvalidOperationException();
				}
				borderSignField = GetBorderSignField(2, side: true);
				localPosition.Z -= m_forwardLimit.Z;
			}
			int linearIdx = borderSignField.ComputeLinear(ref localPosition);
			material = borderSignField.Material(linearIdx);
			content = borderSignField.Content(linearIdx);
		}

		/// <summary>
		/// Reset the mesh to it's original configuration.
		/// </summary>
		public void Reset()
		{
			for (int i = m_originlaVxCnt; i < Mesh.VerticesCount; i++)
			{
				m_edgeIndex.Remove(Mesh.Cells[i]);
			}
			Mesh.Resize(m_originlaVxCnt, m_originalTriangleCnt);
		}
	}
}
