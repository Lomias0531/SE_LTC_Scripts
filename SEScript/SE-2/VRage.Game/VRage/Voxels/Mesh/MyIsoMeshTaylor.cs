using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.Voxels.DualContouring;
using VRage.Voxels.Sewing;
using VRageMath;

namespace VRage.Voxels.Mesh
{
	/// <summary>
	/// Class responsible for stitching Iso-meshes together. This class will essentially contour the gap in between provided meshes.
	/// </summary>
	public class MyIsoMeshTaylor
	{
		private class VertexGenerator
		{
			private readonly MyIsoMeshTaylor m_taylor;

			private MyIsoMeshStitch m_target;

			private sbyte m_targetIndex;

			private Vector3I m_targetOffset;

			/// <summary>
			/// Maps buffer positions to generated vertices.
			/// </summary>
			private readonly Dictionary<Vector3I, ushort> m_createdVertices = new Dictionary<Vector3I, ushort>(Vector3I.Comparer);

			/// <summary>
			/// Maps generated vertices to their correspondants on the  neighbouring mesh if any.
			/// </summary>
			private readonly Dictionary<ushort, Vx> m_generatedPairs = new Dictionary<ushort, Vx>();

			/// <summary>
			/// Maps generated vertices to adjacent vertices.
			/// </summary>
			private readonly MyHashSetDictionary<ushort, ushort> m_adjacentVertices = new MyHashSetDictionary<ushort, ushort>();

			private Vector3I[] m_cornerPositions = new Vector3I[8]
			{
				new Vector3I(0, 0, 0),
				new Vector3I(1, 0, 0),
				new Vector3I(0, 1, 0),
				new Vector3I(1, 1, 0),
				new Vector3I(0, 0, 1),
				new Vector3I(1, 0, 1),
				new Vector3I(0, 1, 1),
				new Vector3I(1, 1, 1)
			};

			private HashSet<uint> m_queued = new HashSet<uint>();

			public VertexGenerator(MyIsoMeshTaylor taylor)
			{
				m_taylor = taylor;
			}

			public void Prepare(MyIsoMeshStitch target, sbyte targetIndex, Vector3I targetOffset)
			{
				m_target = target;
				m_targetIndex = targetIndex;
				m_targetOffset = targetOffset;
			}

			public void Clear()
			{
				m_createdVertices.Clear();
				m_adjacentVertices.Clear();
				m_generatedPairs.Clear();
				m_queued.Clear();
			}

			public void GenerateVertex(ref Vx vertex, Vector3I baseIndex, ref MyVoxelQuad quad, int index)
			{
				if (vertex.Valid)
				{
					return;
				}
				int coordinateIndex = m_taylor.m_coordinateIndex;
				Vector3I vector3I = InverseAxes[coordinateIndex];
				Vector3I vector3I2 = m_cornerPositions[quad[index]];
				vector3I2 = new Vector3I(vector3I2[vector3I.X], vector3I2[vector3I.Y], vector3I2[vector3I.Z]);
				baseIndex = new Vector3I(baseIndex[vector3I.X], baseIndex[vector3I.Y], baseIndex[vector3I.Z]);
				Vector3I vector3I3 = baseIndex + vector3I2;
				if (!m_createdVertices.TryGetValue(vector3I3, out ushort value))
				{
					int num = m_target.Mesh.Lod - m_taylor.Lod;
					Vector3I cell = vector3I3;
					if (num > 0)
					{
						cell >>= num;
					}
					cell += m_targetOffset;
					Vx vertex2 = FindGoodNeighbour(index, ref quad);
					if (vertex2.Valid)
					{
						m_taylor.TranslateVertex(ref vertex2);
						m_createdVertices[vector3I3] = vertex2.Index;
						vertex = vertex2;
						return;
					}
					Vector3 pos = cell + 0.5f;
					Vector3 normal = Vector3.Normalize(pos);
					if (!vector3I3.IsInsideInclusiveEnd(Vector3I.Zero, m_target.Mesh.Size - 2))
					{
						sbyte b = (sbyte)Vector3I.Dot(Vector3I.Sign(Vector3I.Max(vector3I3 - m_target.Mesh.Size + 2, Vector3I.Zero)), new Vector3I(1, 2, 4));
						_ = m_taylor.Meshes[b];
					}
					value = m_target.WriteVertex(ref cell, ref pos, ref normal, byte.MaxValue, 0u);
					m_createdVertices[vector3I3] = value;
				}
				vertex = new Vx(m_targetIndex, value);
			}

			/// <summary>
			/// Look for a close enough neighbour, we only use neighbours that are directly adjacent.
			/// </summary>
			/// <param name="index">Vertex index in the quad.</param>
			/// <param name="quad"></param>
			/// <returns></returns>
			private Vx FindGoodNeighbour(int index, ref MyVoxelQuad quad)
			{
				ushort num = quad[index];
				int num2 = num ^ 1;
				int num3 = num ^ 2;
				int num4 = num ^ 4;
				int num5 = CountTriangles(num2, index, ref quad);
				int num6 = CountTriangles(num3, index, ref quad);
				int num7 = CountTriangles(num4, index, ref quad);
				if (num5 == num6 && num6 == num7 && num5 == 0)
				{
					return Vx.Invalid;
				}
				int cornerIndex = (num5 > num6) ? ((num5 <= num7) ? num4 : num2) : ((num6 <= num7) ? num4 : num3);
				return GetBufferVertex(cornerIndex);
			}

			/// <summary>
			/// Count how many triangles are left if the provided index of the quad is replaced with the given ccorner.
			/// </summary>
			/// <param name="corner"></param>
			/// <param name="index"></param>
			/// <param name="quad"></param>
			private int CountTriangles(int corner, int index, ref MyVoxelQuad quad)
			{
				MyVoxelQuad myVoxelQuad = quad;
				myVoxelQuad[index] = (ushort)corner;
				if (!GetBufferVertex(corner).Valid)
				{
					return 0;
				}
				MyTuple<Vx, Vx, Vx, Vx> myTuple = new MyTuple<Vx, Vx, Vx, Vx>(GetBufferVertex(myVoxelQuad.V0), GetBufferVertex(myVoxelQuad.V1), GetBufferVertex(myVoxelQuad.V2), GetBufferVertex(myVoxelQuad.V3));
				if (myTuple.Item1 == myTuple.Item3)
				{
					return 0;
				}
				if (myTuple.Item2 == myTuple.Item4)
				{
					return 1;
				}
				if (myTuple.Item2 == myTuple.Item1 || myTuple.Item2 == myTuple.Item3)
				{
					return 1;
				}
				if (myTuple.Item4 == myTuple.Item1 || myTuple.Item4 == myTuple.Item3)
				{
					return 1;
				}
				return 2;
			}

			private Vx GetBufferVertex(int cornerIndex)
			{
				return m_taylor.m_buffer[m_taylor.m_cornerIndices[cornerIndex]];
			}

			public void RegisterConnections(ushort v0, ushort v1, ushort v2)
			{
				if (IsGenerated(v0))
				{
					m_adjacentVertices.Add(v0, v1, v2);
				}
				if (IsGenerated(v1))
				{
					m_adjacentVertices.Add(v1, v2, v0);
				}
				if (IsGenerated(v2))
				{
					m_adjacentVertices.Add(v2, v0, v1);
				}
			}

			public void FinalizeGeneratedVertices()
			{
				foreach (ushort value in m_createdVertices.Values)
				{
					m_target.AddVertexToIndex(value);
					if (IsGenerated(value))
					{
						FinalizeVertex(value);
					}
				}
				m_queued.Clear();
			}

			private void FinalizeVertex(ushort vx)
			{
				if (m_queued.Add(vx) && m_adjacentVertices.TryGet(vx, out HashSet<ushort> list))
				{
					PoolManager.Get(out List<Vector3> poolObject);
					PoolManager.Get(out Dictionary<byte, int> poolObject2);
					foreach (ushort item in list)
					{
						if (!IsGenerated(item))
						{
							poolObject.Add(m_target.Mesh.Positions[item]);
							poolObject2.TryGetValue(m_target.Mesh.Materials[item], out int value);
							poolObject2[m_target.Mesh.Materials[item]] = value + 1;
						}
					}
					if (FitPosition(poolObject, out Vector3 pos, out Vector3 normal))
					{
						m_target.Mesh.Positions[vx] = pos;
						m_target.Mesh.Normals[vx] = normal;
					}
					int num = 0;
					byte value2 = 0;
					foreach (KeyValuePair<byte, int> item2 in poolObject2)
					{
						if (item2.Value > num)
						{
							value2 = item2.Key;
							num = item2.Value;
						}
					}
					m_target.Mesh.Materials[vx] = value2;
					if (m_generatedPairs.TryGetValue(vx, out Vx value3))
					{
						MyIsoMesh mesh = m_taylor.Meshes[value3.Mesh].Mesh;
						pos = RemapVertex(m_target.Mesh, mesh, vx);
						normal = m_target.Mesh.Normals[vx];
						mesh.Positions[value3.Index] = pos;
						mesh.Normals[value3.Index] = normal;
						mesh.Materials[value3.Index] = value2;
					}
					PoolManager.Return(ref poolObject);
					PoolManager.Return(ref poolObject2);
				}
			}

			private bool FitPosition(List<Vector3> positions, out Vector3 pos, out Vector3 normal)
			{
				Vector3 zero = Vector3.Zero;
				if (positions.Count < 3)
				{
					pos = default(Vector3);
					normal = default(Vector3);
					return false;
				}
				foreach (Vector3 position in positions)
				{
					zero += position;
				}
				Vector3 vector = zero / positions.Count;
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				float num6 = 0f;
				foreach (Vector3 position2 in positions)
				{
					Vector3 vector2 = position2 - vector;
					num += vector2.X * vector2.X;
					num2 += vector2.X * vector2.Y;
					num3 += vector2.X * vector2.Z;
					num4 += vector2.Y * vector2.Y;
					num5 += vector2.Y * vector2.Z;
					num6 += vector2.Z * vector2.Z;
				}
				float num7 = num4 * num6 - num5 * num5;
				float num8 = num * num6 - num3 * num3;
				float num9 = num * num4 - num2 * num2;
				float num10 = num7;
				int num11 = 0;
				if (num8 > num10)
				{
					num11 = 1;
					num10 = num8;
				}
				if (num9 > num10)
				{
					num11 = 2;
					num10 = num9;
				}
				if (num10 < 0.0001f)
				{
					pos = default(Vector3);
					normal = default(Vector3);
					return false;
				}
				switch (num11)
				{
				default:
				{
					float y2 = (num3 * num5 - num2 * num6) / num7;
					float z2 = (num2 * num5 - num3 * num4) / num7;
					normal = new Vector3(1f, y2, z2);
					break;
				}
				case 1:
				{
					float x2 = (num5 * num3 - num2 * num6) / num8;
					float z = (num2 * num3 - num5 * num) / num8;
					normal = new Vector3(x2, 1f, z);
					break;
				}
				case 2:
				{
					float x = (num5 * num2 - num3 * num4) / num9;
					float y = (num3 * num2 - num5 * num) / num9;
					normal = new Vector3(x, y, 1f);
					break;
				}
				}
				normal.Normalize();
				pos = vector;
				return true;
			}

			public bool IsGenerated(ushort index)
			{
				return m_target.Mesh.Materials[index] == byte.MaxValue;
			}
		}

		private struct Vx
		{
			private sealed class MeshIndexEqualityComparer : IEqualityComparer<Vx>
			{
				public bool Equals(Vx x, Vx y)
				{
					if (x.Mesh == y.Mesh)
					{
						return x.Index == y.Index;
					}
					return false;
				}

				public int GetHashCode(Vx obj)
				{
					return (obj.Mesh.GetHashCode() * 397) ^ obj.Index.GetHashCode();
				}
			}

			/// <summary>
			/// Index of the mesh that contains this vertex, if the mesh or the vertex is not available this will be set to -1.
			/// </summary>
			public sbyte Mesh;

			/// <summary>
			/// Signed distance in the voxel field for this position.
			/// </summary>
			public bool OverIso;

			/// <summary>
			/// Index of the vertex in the mesh.
			/// </summary>
			public ushort Index;

			/// <summary>
			/// Description of an invalid vertex reference.
			/// </summary>
			public static Vx Invalid = new Vx(-1, 0);

			public static readonly IEqualityComparer<Vx> Comparer = new MeshIndexEqualityComparer();

			/// <summary>
			/// Wether this represents a valid index.
			/// </summary>
			public bool Valid => Mesh != -1;

			public Vx(sbyte mesh, int index)
			{
				Mesh = mesh;
				Index = (ushort)index;
				OverIso = false;
			}

			public static bool operator ==(Vx left, Vx right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(Vx left, Vx right)
			{
				return !(left == right);
			}

			public bool Equals(Vx other)
			{
				if (Mesh == other.Mesh)
				{
					return Index == other.Index;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (obj is Vx)
				{
					return Equals((Vx)obj);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (Mesh.GetHashCode() * 397) ^ Index.GetHashCode();
			}

			public override string ToString()
			{
				if (!Valid)
				{
					return "[Invalid]";
				}
				return $"[M{Mesh}: {Index}";
			}
		}

		[ThreadStatic]
		private static MyIsoMeshTaylor m_instance;

		[ThreadStatic]
		private static VrTailor m_nativeInstance;

		private static readonly Vector3I[] Axes = new Vector3I[3]
		{
			new Vector3I(0, 1, 2),
			new Vector3I(2, 0, 1),
			new Vector3I(1, 2, 0)
		};

		private static readonly Vector3I[] InverseAxes = new Vector3I[3]
		{
			new Vector3I(0, 1, 2),
			new Vector3I(1, 2, 0),
			new Vector3I(2, 0, 1)
		};

		/// <summary>
		/// Offsets to the next cube in the mesh grid for each axis.
		/// </summary>
		private static readonly int[] FaceOffsets = (int[])(object)new int[3]
		{
			1,
			2,
			4
		};

		private Vector3I m_startOffset;

		private VertexGenerator m_generator;

		/// <summary>
		/// Lod difference between the smallest lod (m_lod) and the mesh at m_meshes[0].
		/// </summary>
		private int m_minRelativeLod;

		private Dictionary<Vx, ushort> m_addedVertices = new Dictionary<Vx, ushort>(Vx.Comparer);

		private Vector3I m_min;

		private Vector3I m_max;

		private List<MyVoxelQuad> m_tmpQuads = new List<MyVoxelQuad>(3);

		private int[] m_cornerIndices = (int[])(object)new int[8];

		private ushort[] m_cornerOffsets = new ushort[8]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};

		private Vx[] m_buffer;

		private Vector3I m_maxes;

		private Vector3I m_bufferMin;

		private int m_coordinateIndex;

		/// <summary>
		/// Current thread's global taylor instance.
		/// </summary>
		public static MyIsoMeshTaylor Instance => m_instance ?? (m_instance = new MyIsoMeshTaylor());

		/// <summary>
		/// Current thread's global taylor instance.
		/// </summary>
		public static VrTailor NativeInstance => m_nativeInstance ?? (m_nativeInstance = new VrTailor());

		internal MyIsoMeshStitch[] Meshes
		{
			get;
			private set;
		}

		internal int Target
		{
			get;
			private set;
		}

		internal int Lod
		{
			get;
			private set;
		}

		public MyIsoMeshTaylor()
		{
			m_generator = new VertexGenerator(this);
		}

		public void Stitch(MyIsoMeshStitch[] meshes, int primary = 0, VrSewOperation operation = VrSewOperation.All, BoundingBoxI? range = null)
		{
			if (meshes == null)
			{
				throw new ArgumentNullException("meshes");
			}
			if (meshes.Length != 8)
			{
				throw new ArgumentException("Expecting exactly 8 neighboring mesh references.", "meshes");
			}
			if (meshes[primary] == null)
			{
				throw new ArgumentException("Primary mesh cannot be null");
			}
			if (!CheckVicinity(meshes))
			{
				throw new ArgumentException("The meshes to be stitched do not line up!", "meshes");
			}
			Lod = int.MaxValue;
			foreach (MyIsoMeshStitch myIsoMeshStitch in meshes)
			{
				if (myIsoMeshStitch != null)
				{
					myIsoMeshStitch.IndexEdges();
					Lod = Math.Min(myIsoMeshStitch.Mesh.Lod, Lod);
				}
			}
			m_minRelativeLod = meshes[0].Mesh.Lod - Lod;
			m_startOffset = meshes[0].Mesh.CellStart << m_minRelativeLod;
			Meshes = meshes;
			Target = primary;
			CalculateRange(ref range);
			Vector3I startOffset = m_startOffset;
			int num = Meshes[primary].Mesh.Lod - Lod;
			if (num > 0)
			{
				startOffset >>= num;
			}
			startOffset -= Meshes[primary].Mesh.CellStart;
			m_generator.Prepare(Meshes[primary], (sbyte)primary, startOffset);
			for (int j = 0; j < 3; j++)
			{
				if (operation.Contains((VrSewOperation)(1 << 3 - j)) && CollectFace(j))
				{
					GenerateQuads();
				}
			}
			for (int k = 0; k < 3; k++)
			{
				if (operation.Contains((VrSewOperation)(1 << k + 4)) && CollectEdge(k))
				{
					GenerateQuads();
				}
			}
			if (operation.Contains(VrSewOperation.XYZ) && CollectCorner())
			{
				GenerateQuads();
			}
			m_generator.FinalizeGeneratedVertices();
			m_generator.Clear();
			m_addedVertices.Clear();
		}

		private void CalculateRange(ref BoundingBoxI? range)
		{
			if (range.HasValue)
			{
				m_min = range.Value.Min;
				m_max = range.Value.Max;
				return;
			}
			int lod = Meshes[0].Mesh.Lod;
			if (lod <= Lod)
			{
				m_min = Vector3I.Zero;
				m_max = Meshes[0].Mesh.Size - 1;
				return;
			}
			BoundingBoxI boundingBoxI = BoundsInLod(0);
			Vector3I min = boundingBoxI.Min;
			if (Meshes[1] != null && Meshes[1].Mesh.Lod < lod)
			{
				BoundingBoxI boundingBoxI2 = BoundsInLod(1);
				boundingBoxI.Min.Y = Math.Max(boundingBoxI.Min.Y, boundingBoxI2.Min.Y);
				boundingBoxI.Min.Z = Math.Max(boundingBoxI.Min.Z, boundingBoxI2.Min.Z);
				boundingBoxI.Max.Y = Math.Min(boundingBoxI.Max.Y, boundingBoxI2.Max.Y);
				boundingBoxI.Max.Z = Math.Min(boundingBoxI.Max.Z, boundingBoxI2.Max.Z);
			}
			if (Meshes[2] != null && Meshes[2].Mesh.Lod < lod)
			{
				BoundingBoxI boundingBoxI3 = BoundsInLod(2);
				boundingBoxI.Min.X = Math.Max(boundingBoxI.Min.X, boundingBoxI3.Min.X);
				boundingBoxI.Min.Z = Math.Max(boundingBoxI.Min.Z, boundingBoxI3.Min.Z);
				boundingBoxI.Max.X = Math.Min(boundingBoxI.Max.X, boundingBoxI3.Max.X);
				boundingBoxI.Max.Z = Math.Min(boundingBoxI.Max.Z, boundingBoxI3.Max.Z);
			}
			if (Meshes[4] != null && Meshes[4].Mesh.Lod < lod)
			{
				BoundingBoxI boundingBoxI4 = BoundsInLod(4);
				boundingBoxI.Min.X = Math.Max(boundingBoxI.Min.X, boundingBoxI4.Min.X);
				boundingBoxI.Min.Y = Math.Max(boundingBoxI.Min.Y, boundingBoxI4.Min.Y);
				boundingBoxI.Max.X = Math.Min(boundingBoxI.Max.X, boundingBoxI4.Max.X);
				boundingBoxI.Max.Y = Math.Min(boundingBoxI.Max.Y, boundingBoxI4.Max.Y);
			}
			if (Meshes[3] != null && Meshes[3].Mesh.Lod < lod)
			{
				BoundingBoxI boundingBoxI5 = BoundsInLod(3);
				boundingBoxI.Max.X = Math.Min(boundingBoxI.Max.X, boundingBoxI5.Min.X);
				boundingBoxI.Max.Y = Math.Min(boundingBoxI.Max.Y, boundingBoxI5.Min.Y);
				boundingBoxI.Min.Z = Math.Max(boundingBoxI.Min.Z, boundingBoxI5.Min.Z);
				boundingBoxI.Max.Z = Math.Min(boundingBoxI.Max.Z, boundingBoxI5.Max.Z);
			}
			if (Meshes[5] != null && Meshes[5].Mesh.Lod < lod)
			{
				BoundingBoxI boundingBoxI6 = BoundsInLod(5);
				boundingBoxI.Max.X = Math.Min(boundingBoxI.Max.X, boundingBoxI6.Min.X);
				boundingBoxI.Max.Z = Math.Min(boundingBoxI.Max.Z, boundingBoxI6.Min.Z);
				boundingBoxI.Min.Y = Math.Max(boundingBoxI.Min.Y, boundingBoxI6.Min.Y);
				boundingBoxI.Max.Y = Math.Min(boundingBoxI.Max.Y, boundingBoxI6.Max.Y);
			}
			if (Meshes[6] != null && Meshes[6].Mesh.Lod < lod)
			{
				BoundingBoxI boundingBoxI7 = BoundsInLod(6);
				boundingBoxI.Max.Y = Math.Min(boundingBoxI.Max.Y, boundingBoxI7.Min.Y);
				boundingBoxI.Max.Z = Math.Min(boundingBoxI.Max.Z, boundingBoxI7.Min.Z);
				boundingBoxI.Min.X = Math.Max(boundingBoxI.Min.X, boundingBoxI7.Min.X);
				boundingBoxI.Max.X = Math.Min(boundingBoxI.Max.X, boundingBoxI7.Max.X);
			}
			if (Meshes[7] != null && Meshes[7].Mesh.Lod < lod)
			{
				BoundingBoxI boundingBoxI8 = BoundsInLod(7);
				boundingBoxI.Max.X = Math.Min(boundingBoxI.Max.X, boundingBoxI8.Min.X);
				boundingBoxI.Max.Y = Math.Min(boundingBoxI.Max.Y, boundingBoxI8.Min.Y);
				boundingBoxI.Max.Z = Math.Min(boundingBoxI.Max.Z, boundingBoxI8.Min.Z);
			}
			Vector3I size = boundingBoxI.Size;
			if (size.Size == 0)
			{
				Debugger.Break();
			}
			if (size.X != size.Y && size.Y != size.Z && size.X != size.Z)
			{
				Debugger.Break();
			}
			int shift = Meshes[0].Mesh.Lod - Lod;
			m_min = boundingBoxI.Min - min >> shift;
			m_max = boundingBoxI.Max - min >> shift;
		}

		private BoundingBoxI BoundsInLod(int index)
		{
			MyIsoMesh mesh = Meshes[index].Mesh;
			int shift = mesh.Lod - Lod;
			return new BoundingBoxI(mesh.CellStart << shift, mesh.CellEnd << shift);
		}

		public static bool CheckVicinity(MyIsoMeshStitch[] meshes)
		{
			int num = meshes.Where((MyIsoMeshStitch x) => x != null).Min((MyIsoMeshStitch x) => x.Mesh.Lod);
			Vector3I vector3I = meshes[0].Mesh.CellEnd << meshes[0].Mesh.Lod - num;
			for (int i = 1; i < 8; i++)
			{
				if (meshes[i] != null && meshes[i] != meshes[0])
				{
					Vector3I vector3I2 = meshes[i].Mesh.CellStart << meshes[i].Mesh.Lod - num;
					if (vector3I.X != vector3I2.X && vector3I.Y != vector3I2.Y && vector3I.Z != vector3I2.Z)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static bool CheckVicinity(VrSewGuide[] meshes)
		{
			int num = meshes.Where((VrSewGuide x) => x != null).Min((VrSewGuide x) => x.Lod);
			Vector3I vector3I = meshes[0].End << meshes[0].Lod - num;
			for (int i = 1; i < 8; i++)
			{
				if (meshes[i] != null && meshes[i] != meshes[0])
				{
					Vector3I vector3I2 = meshes[i].Start << meshes[i].Lod - num;
					if (vector3I.X != vector3I2.X && vector3I.Y != vector3I2.Y && vector3I.Z != vector3I2.Z)
					{
						return false;
					}
				}
			}
			return true;
		}

		private bool CollectFace(int coordIndex)
		{
			int x = Axes[coordIndex].X;
			int y = Axes[coordIndex].Y;
			int z = Axes[coordIndex].Z;
			sbyte b = (sbyte)FaceOffsets[z];
			MyIsoMeshStitch myIsoMeshStitch = Meshes[0];
			MyIsoMeshStitch myIsoMeshStitch2 = Meshes[b];
			if (myIsoMeshStitch == myIsoMeshStitch2)
			{
				return false;
			}
			Vector3I vector3I = (m_max << m_minRelativeLod) - 1;
			Vector3I vector3I2 = m_min << m_minRelativeLod;
			vector3I2 = new Vector3I(vector3I2[x], vector3I2[y], vector3I[z]);
			vector3I = new Vector3I(vector3I[x], vector3I[y], vector3I[z] + 1);
			ResizeBuffer(vector3I2, vector3I, coordIndex);
			Vector3I vector3I3 = default(Vector3I);
			vector3I3[z] = vector3I2.Z;
			int num = 0;
			vector3I3[y] = vector3I2.Y;
			while (vector3I3[y] <= vector3I.Y)
			{
				int num2 = 0;
				vector3I3[x] = vector3I2.X;
				while (vector3I3[x] <= vector3I.X)
				{
					int num3 = num2 + num;
					if (TryGetVertex(myIsoMeshStitch, vector3I3, out ushort index))
					{
						m_buffer[num3] = new Vx(0, index);
					}
					m_buffer[num3].OverIso = IsContentOverIso(myIsoMeshStitch, vector3I3);
					Vector3I vector3I4 = vector3I3;
					vector3I4[z]++;
					int num4 = num3 + m_maxes.Y;
					if (TryGetVertex(myIsoMeshStitch2, vector3I4, out index))
					{
						m_buffer[num4] = new Vx(b, index);
					}
					m_buffer[num4].OverIso = IsContentOverIso(myIsoMeshStitch2, vector3I4);
					vector3I3[x]++;
					num2++;
				}
				vector3I3[y]++;
				num += m_maxes.X;
			}
			return true;
		}

		private bool CollectEdge(int coordIndex)
		{
			int x = Axes[coordIndex].X;
			int y = Axes[coordIndex].Y;
			int z = Axes[coordIndex].Z;
			sbyte b = (sbyte)FaceOffsets[x];
			sbyte b2 = (sbyte)FaceOffsets[y];
			sbyte b3 = (sbyte)(b + b2);
			MyIsoMeshStitch myIsoMeshStitch = Meshes[0];
			MyIsoMeshStitch myIsoMeshStitch2 = Meshes[b];
			MyIsoMeshStitch myIsoMeshStitch3 = Meshes[b2];
			MyIsoMeshStitch myIsoMeshStitch4 = Meshes[b3];
			Vector3I vector3I = (m_max << m_minRelativeLod) - 1;
			Vector3I vector3I2 = m_min << m_minRelativeLod;
			if (myIsoMeshStitch == myIsoMeshStitch2 && myIsoMeshStitch2 == myIsoMeshStitch3 && myIsoMeshStitch3 == myIsoMeshStitch4)
			{
				return false;
			}
			ResizeBuffer(new Vector3I(vector3I2[z], vector3I[x], vector3I[y]), new Vector3I(vector3I[z], vector3I[x] + 1, vector3I[y] + 1), (coordIndex + 1) % 3);
			Vector3I vector3I3 = vector3I;
			vector3I3[z] = vector3I2[z];
			int num = 0;
			while (vector3I3[z] <= vector3I[z])
			{
				int num2 = num;
				int num3 = num + m_maxes.X;
				int num4 = num + m_maxes.Y;
				int num5 = num + m_maxes.X + m_maxes.Y;
				if (TryGetVertex(myIsoMeshStitch, vector3I3, out ushort index))
				{
					m_buffer[num2] = new Vx(0, index);
				}
				m_buffer[num2].OverIso = IsContentOverIso(myIsoMeshStitch, vector3I3);
				vector3I3[x]++;
				if (TryGetVertex(myIsoMeshStitch2, vector3I3, out index))
				{
					m_buffer[num3] = new Vx(b, index);
				}
				m_buffer[num3].OverIso = IsContentOverIso(myIsoMeshStitch2, vector3I3);
				vector3I3[y]++;
				if (TryGetVertex(myIsoMeshStitch4, vector3I3, out index))
				{
					m_buffer[num5] = new Vx(b3, index);
				}
				m_buffer[num5].OverIso = IsContentOverIso(myIsoMeshStitch4, vector3I3);
				vector3I3[x]--;
				if (TryGetVertex(myIsoMeshStitch3, vector3I3, out index))
				{
					m_buffer[num4] = new Vx(b2, index);
				}
				m_buffer[num4].OverIso = IsContentOverIso(myIsoMeshStitch3, vector3I3);
				vector3I3[y]--;
				vector3I3[z]++;
				num++;
			}
			return true;
		}

		private bool CollectCorner()
		{
			MyIsoMeshStitch mesh = Meshes[0];
			MyIsoMeshStitch mesh2 = Meshes[1];
			MyIsoMeshStitch mesh3 = Meshes[2];
			MyIsoMeshStitch mesh4 = Meshes[3];
			MyIsoMeshStitch mesh5 = Meshes[4];
			MyIsoMeshStitch mesh6 = Meshes[5];
			MyIsoMeshStitch mesh7 = Meshes[6];
			MyIsoMeshStitch mesh8 = Meshes[7];
			Vector3I vector3I = (m_max << m_minRelativeLod) - 1;
			ResizeBuffer(vector3I, vector3I + 1, 0);
			Vector3I vector3I2 = vector3I;
			if (TryGetVertex(mesh, vector3I2, out ushort index))
			{
				m_buffer[0] = new Vx(0, index);
			}
			m_buffer[0].OverIso = IsContentOverIso(mesh, vector3I2);
			vector3I2.X++;
			if (TryGetVertex(mesh2, vector3I2, out index))
			{
				m_buffer[1] = new Vx(1, index);
			}
			m_buffer[1].OverIso = IsContentOverIso(mesh2, vector3I2);
			vector3I2.Y++;
			if (TryGetVertex(mesh4, vector3I2, out index))
			{
				m_buffer[3] = new Vx(3, index);
			}
			m_buffer[3].OverIso = IsContentOverIso(mesh4, vector3I2);
			vector3I2.X--;
			if (TryGetVertex(mesh3, vector3I2, out index))
			{
				m_buffer[2] = new Vx(2, index);
			}
			m_buffer[2].OverIso = IsContentOverIso(mesh3, vector3I2);
			vector3I2.Z++;
			if (TryGetVertex(mesh7, vector3I2, out index))
			{
				m_buffer[6] = new Vx(6, index);
			}
			m_buffer[6].OverIso = IsContentOverIso(mesh7, vector3I2);
			vector3I2.X++;
			if (TryGetVertex(mesh8, vector3I2, out index))
			{
				m_buffer[7] = new Vx(7, index);
			}
			m_buffer[7].OverIso = IsContentOverIso(mesh8, vector3I2);
			vector3I2.Y--;
			if (TryGetVertex(mesh6, vector3I2, out index))
			{
				m_buffer[5] = new Vx(5, index);
			}
			m_buffer[5].OverIso = IsContentOverIso(mesh6, vector3I2);
			vector3I2.X--;
			if (TryGetVertex(mesh5, vector3I2, out index))
			{
				m_buffer[4] = new Vx(4, index);
			}
			m_buffer[4].OverIso = IsContentOverIso(mesh5, vector3I2);
			return true;
		}

		private bool TryGetVertex(MyIsoMeshStitch mesh, Vector3I coord, out ushort index)
		{
			if (mesh == null)
			{
				index = 0;
				return false;
			}
			coord += m_startOffset;
			int num = mesh.Mesh.Lod - Lod;
			if (num > 0)
			{
				coord >>= num;
			}
			coord -= mesh.Mesh.CellStart;
			return mesh.TryGetVertex(coord, out index);
		}

		private bool IsContentOverIso(MyIsoMeshStitch mesh, Vector3I pos)
		{
			if (mesh == null)
			{
				mesh = Meshes[0];
			}
			pos += m_startOffset;
			int num = mesh.Mesh.Lod - Lod;
			if (num > 0)
			{
				pos >>= num;
			}
			pos -= mesh.Mesh.CellStart;
			mesh.SampleEdge(pos, out byte _, out byte content);
			return content - 128 >= 0;
		}

		/// <summary>
		/// Generate all quads for the buffer, this is actually similar to how our dual contouring works.
		/// </summary>
		private void GenerateQuads()
		{
			Vector3I b = new Vector3I(1, m_maxes.X, m_maxes.Y);
			Vector3I vector3I = m_maxes - b;
			int[] cornerIndices = m_cornerIndices;
			List<MyVoxelQuad> tmpQuads = m_tmpQuads;
			for (int i = 0; i < vector3I.Z; i += b.Z)
			{
				int num = i + m_maxes.Y;
				for (int j = 0; j < vector3I.Y; j += m_maxes.X)
				{
					int num2 = j + m_maxes.X;
					byte b2 = 0;
					if (m_buffer[j + i].OverIso)
					{
						b2 = (byte)(b2 | 2);
					}
					if (m_buffer[num2 + i].OverIso)
					{
						b2 = (byte)(b2 | 8);
					}
					if (m_buffer[j + num].OverIso)
					{
						b2 = (byte)(b2 | 0x20);
					}
					if (m_buffer[num2 + num].OverIso)
					{
						b2 = (byte)(b2 | 0x80);
					}
					cornerIndices[1] = (ushort)(j + i);
					cornerIndices[3] = (ushort)(num2 + i);
					cornerIndices[5] = (ushort)(j + num);
					cornerIndices[7] = (ushort)(num2 + num);
					uint num3 = 0u;
					while (num3 < vector3I.X)
					{
						uint num4 = num3 + 1;
						b2 = (byte)(b2 >> 1);
						b2 = (byte)(b2 & 0x55);
						if (m_buffer[num4 + j + i].OverIso)
						{
							b2 = (byte)(b2 | 2);
						}
						if (m_buffer[num4 + num2 + i].OverIso)
						{
							b2 = (byte)(b2 | 8);
						}
						if (m_buffer[num4 + j + num].OverIso)
						{
							b2 = (byte)(b2 | 0x20);
						}
						if (m_buffer[num4 + num2 + num].OverIso)
						{
							b2 = (byte)(b2 | 0x80);
						}
						LeftShift(cornerIndices);
						cornerIndices[1] = (ushort)(num4 + j + i);
						cornerIndices[3] = (ushort)(num4 + num2 + i);
						cornerIndices[5] = (ushort)(num4 + j + num);
						cornerIndices[7] = (ushort)(num4 + num2 + num);
						if (MyDualContouringMesher.EdgeTable[b2] != 0)
						{
							MyDualContouringMesher.GenerateQuads(b2, m_cornerOffsets, tmpQuads);
							for (int k = 0; k < tmpQuads.Count; k++)
							{
								MyVoxelQuad quad = tmpQuads[k];
								Vx vertex = m_buffer[cornerIndices[quad.V0]];
								Vx vertex2 = m_buffer[cornerIndices[quad.V1]];
								Vx vertex3 = m_buffer[cornerIndices[quad.V2]];
								Vx vertex4 = m_buffer[cornerIndices[quad.V3]];
								bool flag = false;
								if (!vertex.Valid || !vertex2.Valid || !vertex3.Valid || !vertex4.Valid)
								{
									Vector3I baseIndex = m_bufferMin + new Vector3I(num3, j, i) / b;
									m_generator.GenerateVertex(ref vertex, baseIndex, ref quad, 0);
									m_generator.GenerateVertex(ref vertex2, baseIndex, ref quad, 1);
									m_generator.GenerateVertex(ref vertex3, baseIndex, ref quad, 2);
									m_generator.GenerateVertex(ref vertex4, baseIndex, ref quad, 3);
									flag = true;
								}
								if (vertex.Mesh != -1 && vertex2.Mesh != -1 && vertex3.Mesh != -1 && vertex4.Mesh != -1)
								{
									TranslateVertex(ref vertex);
									TranslateVertex(ref vertex2);
									TranslateVertex(ref vertex3);
									TranslateVertex(ref vertex4);
									if (vertex != vertex2 && vertex2 != vertex3 && vertex3 != vertex)
									{
										Meshes[Target].WriteTriangle(vertex2.Index, vertex3.Index, vertex.Index);
									}
									if (vertex != vertex4 && vertex4 != vertex3 && vertex3 != vertex)
									{
										Meshes[Target].WriteTriangle(vertex3.Index, vertex4.Index, vertex.Index);
									}
									if (flag)
									{
										m_generator.RegisterConnections(vertex.Index, vertex2.Index, vertex3.Index);
										m_generator.RegisterConnections(vertex4.Index, vertex3.Index, vertex.Index);
									}
								}
							}
							tmpQuads.Clear();
						}
						num3 = num4;
					}
				}
			}
		}

		private void LeftShift(int[] corners)
		{
			corners[0] = corners[1];
			corners[2] = corners[3];
			corners[4] = corners[5];
			corners[6] = corners[7];
		}

		/// <summary>
		/// Ensures the primary mesh conmtains the provided vertex and translates the reference to the matching vertex in that mesh.
		/// </summary>
		/// <param name="vertex"></param>
		private void TranslateVertex(ref Vx vertex)
		{
			if (vertex.Mesh != Target)
			{
				if (!m_addedVertices.TryGetValue(vertex, out ushort value))
				{
					MyIsoMesh mesh = Meshes[vertex.Mesh].Mesh;
					MyIsoMesh mesh2 = Meshes[Target].Mesh;
					ushort index = vertex.Index;
					Vector3I cell = new Vector3I(-1);
					Vector3 pos = RemapVertex(mesh, mesh2, index);
					Vector3 normal = mesh.Normals[index];
					uint colorShift = mesh.ColorShiftHSV[index];
					byte material = mesh.Materials[index];
					value = Meshes[Target].WriteVertex(ref cell, ref pos, ref normal, material, colorShift);
					m_addedVertices[vertex] = value;
				}
				vertex = new Vx((sbyte)Target, value);
			}
		}

		private static Vector3 RemapVertex(MyIsoMesh src, MyIsoMesh target, ushort index)
		{
			return (Vector3)(src.Positions[index] * src.PositionScale + src.PositionOffset - target.PositionOffset) / target.PositionScale;
		}

		private void ResizeBuffer(Vector3I min, Vector3I max, int coordinateIndex)
		{
			m_bufferMin = min;
			m_coordinateIndex = coordinateIndex;
			Vector3I vector3I = max - min + 1;
			m_maxes.X = vector3I.X;
			m_maxes.Y = vector3I.Y * m_maxes.X;
			m_maxes.Z = vector3I.Z * m_maxes.Y;
			if (m_buffer == null || m_maxes.Z > m_buffer.Length)
			{
				m_buffer = new Vx[m_maxes.Z];
			}
			ClearBuffer();
		}

		private void ClearBuffer(int start = 0)
		{
			for (int i = start; i < m_buffer.Length; i++)
			{
				m_buffer[i] = Vx.Invalid;
			}
		}
	}
}
