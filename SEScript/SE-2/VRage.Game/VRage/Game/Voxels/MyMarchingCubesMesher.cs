using System;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace VRage.Game.Voxels
{
	public class MyMarchingCubesMesher : IMyIsoMesher
	{
		private class MyEdgeVertex
		{
			public ushort VertexIndex;

			public int CalcCounter;
		}

		private class MyEdge
		{
			public Vector3 Position;

			public Vector3 Normal;

			public float Ambient;

			public byte Material;
		}

		private class MyTemporaryVoxel
		{
			public int IdxInCache;

			public Vector3 Position;

			public Vector3 Normal;

			public float Ambient;

			public int Normal_CalcCounter;

			public int Ambient_CalcCounter;
		}

		private const int POLYCUBE_EDGES = 12;

		private readonly MyEdge[] m_edges = new MyEdge[12];

		private const int CELL_EDGES_SIZE = 90;

		private MyEdgeVertex[][][][] m_edgeVertex;

		private int m_edgeVertexCalcCounter;

		private readonly MyVoxelVertex[] m_resultVertices = new MyVoxelVertex[7680];

		private int m_resultVerticesCounter;

		private readonly MyVoxelTriangle[] m_resultTriangles = new MyVoxelTriangle[25600];

		private int m_resultTrianglesCounter;

		private Vector3I m_polygCubes;

		private Vector3I m_voxelStart;

		private const int COPY_TABLE_SIZE = 11;

		private int m_temporaryVoxelsCounter;

		private readonly MyTemporaryVoxel[] m_temporaryVoxels = new MyTemporaryVoxel[13310];

		private const int m_sX = 1;

		private const int m_sY = 11;

		private const int m_sZ = 121;

		private readonly MyStorageData m_cache = new MyStorageData();

		private Vector3I m_sizeMinusOne;

		private float m_voxelSizeInMeters;

		private Vector3 m_originPosition;

		public int AffectedRangeOffset => -1;

		public int AffectedRangeSizeChange => 3;

		public int InvalidatedRangeInflate => 2;

		public int VertexPositionRangeSizeChange => 0;

		public float VertexPositionOffsetChange => 0.5f;

		public MyMarchingCubesMesher()
		{
			for (int i = 0; i < m_edges.Length; i++)
			{
				m_edges[i] = new MyEdge();
			}
			for (int j = 0; j < m_temporaryVoxels.Length; j++)
			{
				m_temporaryVoxels[j] = new MyTemporaryVoxel();
			}
			m_edgeVertexCalcCounter = 0;
			m_edgeVertex = new MyEdgeVertex[90][][][];
			for (int k = 0; k < 90; k++)
			{
				m_edgeVertex[k] = new MyEdgeVertex[90][][];
				for (int l = 0; l < 90; l++)
				{
					m_edgeVertex[k][l] = new MyEdgeVertex[90][];
					for (int m = 0; m < 90; m++)
					{
						m_edgeVertex[k][l][m] = new MyEdgeVertex[3];
						for (int n = 0; n < 3; n++)
						{
							m_edgeVertex[k][l][m][n] = new MyEdgeVertex();
							m_edgeVertex[k][l][m][n].CalcCounter = 0;
						}
					}
				}
			}
		}

		private void CalcPolygCubeSize(int lodIdx, Vector3I storageSize)
		{
			Vector3I vector3I = storageSize;
			vector3I >>= lodIdx;
			m_polygCubes.X = ((m_voxelStart.X + 8 >= vector3I.X) ? 8 : 9);
			m_polygCubes.Y = ((m_voxelStart.Y + 8 >= vector3I.Y) ? 8 : 9);
			m_polygCubes.Z = ((m_voxelStart.Z + 8 >= vector3I.Z) ? 8 : 9);
		}

		private byte GetVoxelContent(int x, int y, int z)
		{
			return m_cache.Content(x, y, z);
		}

		private void GetVoxelNormal(MyTemporaryVoxel temporaryVoxel, ref Vector3I coord, ref Vector3I voxelCoord, MyTemporaryVoxel centerVoxel)
		{
			if (temporaryVoxel.Normal_CalcCounter != m_temporaryVoxelsCounter)
			{
				Vector3I value = coord - 1;
				Vector3I value2 = coord + 1;
				MyStorageData cache = m_cache;
				Vector3I value3 = cache.Size3D - 1;
				Vector3I.Max(ref value, ref Vector3I.Zero, out value);
				Vector3I.Min(ref value2, ref value3, out value2);
				Vector3 vec = new Vector3((float)(cache.Content(value.X, coord.Y, coord.Z) - cache.Content(value2.X, coord.Y, coord.Z)) / 255f, (float)(cache.Content(coord.X, value.Y, coord.Z) - cache.Content(coord.X, value2.Y, coord.Z)) / 255f, (float)(cache.Content(coord.X, coord.Y, value.Z) - cache.Content(coord.X, coord.Y, value2.Z)) / 255f);
				if (vec.LengthSquared() <= 1E-06f)
				{
					temporaryVoxel.Normal = centerVoxel.Normal;
				}
				else
				{
					MyUtils.Normalize(ref vec, out temporaryVoxel.Normal);
				}
				temporaryVoxel.Normal_CalcCounter = m_temporaryVoxelsCounter;
			}
		}

		private Vector3 ComputeVertexNormal(ref Vector3 position)
		{
			Vector3 vector = (position - m_originPosition) / m_voxelSizeInMeters + 1f;
			Vector3 value = default(Vector3);
			value.X = SampleContent(vector.X - 0.01f, vector.Y, vector.Z) - SampleContent(vector.X + 0.01f, vector.Y, vector.Z);
			value.Y = SampleContent(vector.X, vector.Y - 0.01f, vector.Z) - SampleContent(vector.X, vector.Y + 0.01f, vector.Z);
			value.Z = SampleContent(vector.X, vector.Y, vector.Z - 0.01f) - SampleContent(vector.X, vector.Y, vector.Z + 0.01f);
			Vector3.Normalize(ref value, out value);
			return value;
		}

		private float SampleContent(float x, float y, float z)
		{
			Vector3 value = new Vector3(x, y, z);
			Vector3I value2 = Vector3I.Floor(value);
			value -= (Vector3)value2;
			float num = (int)m_cache.Content(value2.X, value2.Y, value2.Z);
			float num2 = (int)m_cache.Content(value2.X + 1, value2.Y, value2.Z);
			float num3 = (int)m_cache.Content(value2.X, value2.Y + 1, value2.Z);
			float num4 = (int)m_cache.Content(value2.X + 1, value2.Y + 1, value2.Z);
			float num5 = (int)m_cache.Content(value2.X, value2.Y, value2.Z + 1);
			float num6 = (int)m_cache.Content(value2.X + 1, value2.Y, value2.Z + 1);
			float num7 = (int)m_cache.Content(value2.X, value2.Y + 1, value2.Z + 1);
			float num8 = (int)m_cache.Content(value2.X + 1, value2.Y + 1, value2.Z + 1);
			num += value.X * (num2 - num);
			num3 += value.X * (num4 - num3);
			num5 += value.X * (num6 - num5);
			num7 += value.X * (num8 - num7);
			num += value.Y * (num3 - num);
			num5 += value.Y * (num7 - num5);
			return num + value.Z * (num5 - num);
		}

		private void GetVoxelAmbient(MyTemporaryVoxel temporaryVoxel, ref Vector3I coord, ref Vector3I tempVoxelCoord)
		{
			if (temporaryVoxel.Ambient_CalcCounter == m_temporaryVoxelsCounter)
			{
				return;
			}
			MyStorageData cache = m_cache;
			float num = 0f;
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					for (int k = -1; k <= 1; k++)
					{
						Vector3I vector3I = new Vector3I(coord.X + i - 1, coord.Y + j - 1, coord.Z + k - 1);
						if (vector3I.X >= 0 && vector3I.X <= m_sizeMinusOne.X && vector3I.Y >= 0 && vector3I.Y <= m_sizeMinusOne.Y && vector3I.Z >= 0 && vector3I.Z <= m_sizeMinusOne.Z)
						{
							num += (float)(int)cache.Content(coord.X + i, coord.Y + j, coord.Z + k);
						}
					}
				}
			}
			num /= 6885f;
			num = 1f - num;
			num = (temporaryVoxel.Ambient = MathHelper.Clamp(num, 0.4f, 0.9f));
			temporaryVoxel.Ambient_CalcCounter = m_temporaryVoxelsCounter;
		}

		private void GetVertexInterpolation(MyStorageData cache, MyTemporaryVoxel inputVoxelA, MyTemporaryVoxel inputVoxelB, int edgeIndex)
		{
			MyEdge myEdge = m_edges[edgeIndex];
			byte b = cache.Content(inputVoxelA.IdxInCache);
			byte b2 = cache.Content(inputVoxelB.IdxInCache);
			byte b3 = cache.Material(inputVoxelA.IdxInCache);
			byte b4 = cache.Material(inputVoxelB.IdxInCache);
			if ((float)Math.Abs(127 - b) < 1E-05f)
			{
				myEdge.Position = inputVoxelA.Position;
				myEdge.Normal = inputVoxelA.Normal;
				myEdge.Material = b3;
				myEdge.Ambient = inputVoxelA.Ambient;
				return;
			}
			if ((float)Math.Abs(127 - b2) < 1E-05f)
			{
				myEdge.Position = inputVoxelB.Position;
				myEdge.Normal = inputVoxelB.Normal;
				myEdge.Material = b4;
				myEdge.Ambient = inputVoxelB.Ambient;
				return;
			}
			float num = (float)(127 - b) / (float)(b2 - b);
			myEdge.Position.X = inputVoxelA.Position.X + num * (inputVoxelB.Position.X - inputVoxelA.Position.X);
			myEdge.Position.Y = inputVoxelA.Position.Y + num * (inputVoxelB.Position.Y - inputVoxelA.Position.Y);
			myEdge.Position.Z = inputVoxelA.Position.Z + num * (inputVoxelB.Position.Z - inputVoxelA.Position.Z);
			myEdge.Normal.X = inputVoxelA.Normal.X + num * (inputVoxelB.Normal.X - inputVoxelA.Normal.X);
			myEdge.Normal.Y = inputVoxelA.Normal.Y + num * (inputVoxelB.Normal.Y - inputVoxelA.Normal.Y);
			myEdge.Normal.Z = inputVoxelA.Normal.Z + num * (inputVoxelB.Normal.Z - inputVoxelA.Normal.Z);
			if (MathHelper.IsZero(myEdge.Normal))
			{
				myEdge.Normal = inputVoxelA.Normal;
			}
			else
			{
				myEdge.Normal = MyUtils.Normalize(myEdge.Normal);
			}
			if (MathHelper.IsZero(myEdge.Normal))
			{
				myEdge.Normal = inputVoxelA.Normal;
			}
			float num2 = (float)(int)b2 / ((float)(int)b + (float)(int)b2);
			myEdge.Material = ((num2 <= 0.5f) ? b3 : b4);
			myEdge.Ambient = inputVoxelA.Ambient + num2 * (inputVoxelB.Ambient - inputVoxelA.Ambient);
		}

		public MyIsoMesh Precalc(IMyStorage storage, int lod, Vector3I voxelStart, Vector3I voxelEnd, MyStorageDataTypeFlags properties = MyStorageDataTypeFlags.ContentAndMaterial, MyVoxelRequestFlags flags = (MyVoxelRequestFlags)0)
		{
			m_resultVerticesCounter = 0;
			m_resultTrianglesCounter = 0;
			m_edgeVertexCalcCounter++;
			m_temporaryVoxelsCounter++;
			CalcPolygCubeSize(lod, storage.Size);
			m_voxelStart = voxelStart;
			_ = storage.Size;
			m_cache.Resize(voxelStart, voxelEnd);
			storage.ReadRange(m_cache, MyStorageDataTypeFlags.Content, lod, voxelStart, voxelEnd);
			if (!m_cache.ContainsIsoSurface())
			{
				return null;
			}
			storage.ReadRange(m_cache, MyStorageDataTypeFlags.Material, lod, voxelStart, voxelEnd);
			ComputeSizeAndOrigin(lod, storage.Size);
			Vector3I start = Vector3I.Zero;
			Vector3I end = voxelEnd - voxelStart - 3;
			Vector3I coord = start;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
			while (vector3I_RangeIterator.IsValid())
			{
				int num = 0;
				if (m_cache.Content(coord.X, coord.Y, coord.Z) < 127)
				{
					num |= 1;
				}
				if (m_cache.Content(coord.X + 1, coord.Y, coord.Z) < 127)
				{
					num |= 2;
				}
				if (m_cache.Content(coord.X + 1, coord.Y, coord.Z + 1) < 127)
				{
					num |= 4;
				}
				if (m_cache.Content(coord.X, coord.Y, coord.Z + 1) < 127)
				{
					num |= 8;
				}
				if (m_cache.Content(coord.X, coord.Y + 1, coord.Z) < 127)
				{
					num |= 0x10;
				}
				if (m_cache.Content(coord.X + 1, coord.Y + 1, coord.Z) < 127)
				{
					num |= 0x20;
				}
				if (m_cache.Content(coord.X + 1, coord.Y + 1, coord.Z + 1) < 127)
				{
					num |= 0x40;
				}
				if (m_cache.Content(coord.X, coord.Y + 1, coord.Z + 1) < 127)
				{
					num |= 0x80;
				}
				if (MyMarchingCubesConstants.EdgeTable[num] != 0)
				{
					Vector3I tempVoxelCoord = ComputeTemporaryVoxelData(m_cache, ref coord, num, lod);
					CreateTriangles(ref coord, num, ref tempVoxelCoord);
				}
				vector3I_RangeIterator.GetNext(out coord);
			}
			_ = m_cache.Size3D;
			Vector3I vector3I = voxelStart - AffectedRangeOffset;
			MyIsoMesh myIsoMesh = new MyIsoMesh();
			for (int i = 0; i < m_resultVerticesCounter; i++)
			{
				Vector3 position = (m_resultVertices[i].Position - (Vector3)storage.Size / 2f) / storage.Size;
				m_resultVertices[i].Position = position;
			}
			for (int j = 0; j < m_resultVerticesCounter; j++)
			{
				myIsoMesh.WriteVertex(ref m_resultVertices[j].Cell, ref m_resultVertices[j].Position, ref m_resultVertices[j].Normal, (byte)m_resultVertices[j].Material, 0u);
			}
			for (int k = 0; k < m_resultTrianglesCounter; k++)
			{
				myIsoMesh.WriteTriangle(m_resultTriangles[k].V0, m_resultTriangles[k].V1, m_resultTriangles[k].V2);
			}
			MyIsoMesh myIsoMesh2 = myIsoMesh;
			myIsoMesh2.PositionOffset = storage.Size / 2;
			myIsoMesh2.PositionScale = storage.Size;
			myIsoMesh2.CellStart = voxelStart;
			myIsoMesh2.CellEnd = voxelEnd;
			Vector3I[] internalArray = myIsoMesh2.Cells.GetInternalArray();
			for (int l = 0; l < myIsoMesh2.VerticesCount; l++)
			{
				internalArray[l] += vector3I;
			}
			return myIsoMesh;
		}

		private Vector3I ComputeTemporaryVoxelData(MyStorageData cache, ref Vector3I coord0, int cubeIndex, int lod)
		{
			int num = coord0.X + coord0.Y * 11 + coord0.Z * 121;
			MyTemporaryVoxel myTemporaryVoxel = m_temporaryVoxels[num];
			MyTemporaryVoxel myTemporaryVoxel2 = m_temporaryVoxels[num + 1];
			MyTemporaryVoxel myTemporaryVoxel3 = m_temporaryVoxels[num + 1 + 121];
			MyTemporaryVoxel myTemporaryVoxel4 = m_temporaryVoxels[num + 121];
			MyTemporaryVoxel myTemporaryVoxel5 = m_temporaryVoxels[num + 11];
			MyTemporaryVoxel myTemporaryVoxel6 = m_temporaryVoxels[num + 1 + 11];
			MyTemporaryVoxel myTemporaryVoxel7 = m_temporaryVoxels[num + 1 + 11 + 121];
			MyTemporaryVoxel myTemporaryVoxel8 = m_temporaryVoxels[num + 11 + 121];
			Vector3I coord = new Vector3I(coord0.X + 1, coord0.Y, coord0.Z);
			Vector3I coord2 = new Vector3I(coord0.X + 1, coord0.Y, coord0.Z + 1);
			Vector3I coord3 = new Vector3I(coord0.X, coord0.Y, coord0.Z + 1);
			Vector3I coord4 = new Vector3I(coord0.X, coord0.Y + 1, coord0.Z);
			Vector3I coord5 = new Vector3I(coord0.X + 1, coord0.Y + 1, coord0.Z);
			Vector3I coord6 = new Vector3I(coord0.X + 1, coord0.Y + 1, coord0.Z + 1);
			Vector3I coord7 = new Vector3I(coord0.X, coord0.Y + 1, coord0.Z + 1);
			Vector3I p = coord0;
			Vector3I p2 = coord;
			Vector3I p3 = coord2;
			Vector3I p4 = coord3;
			Vector3I p5 = coord4;
			Vector3I p6 = coord5;
			Vector3I p7 = coord6;
			Vector3I p8 = coord7;
			myTemporaryVoxel.IdxInCache = cache.ComputeLinear(ref p);
			myTemporaryVoxel2.IdxInCache = cache.ComputeLinear(ref p2);
			myTemporaryVoxel3.IdxInCache = cache.ComputeLinear(ref p3);
			myTemporaryVoxel4.IdxInCache = cache.ComputeLinear(ref p4);
			myTemporaryVoxel5.IdxInCache = cache.ComputeLinear(ref p5);
			myTemporaryVoxel6.IdxInCache = cache.ComputeLinear(ref p6);
			myTemporaryVoxel7.IdxInCache = cache.ComputeLinear(ref p7);
			myTemporaryVoxel8.IdxInCache = cache.ComputeLinear(ref p8);
			myTemporaryVoxel.Position.X = (float)(m_voxelStart.X + coord0.X) * m_voxelSizeInMeters;
			myTemporaryVoxel.Position.Y = (float)(m_voxelStart.Y + coord0.Y) * m_voxelSizeInMeters;
			myTemporaryVoxel.Position.Z = (float)(m_voxelStart.Z + coord0.Z) * m_voxelSizeInMeters;
			myTemporaryVoxel2.Position.X = myTemporaryVoxel.Position.X + m_voxelSizeInMeters;
			myTemporaryVoxel2.Position.Y = myTemporaryVoxel.Position.Y;
			myTemporaryVoxel2.Position.Z = myTemporaryVoxel.Position.Z;
			myTemporaryVoxel3.Position.X = myTemporaryVoxel.Position.X + m_voxelSizeInMeters;
			myTemporaryVoxel3.Position.Y = myTemporaryVoxel.Position.Y;
			myTemporaryVoxel3.Position.Z = myTemporaryVoxel.Position.Z + m_voxelSizeInMeters;
			myTemporaryVoxel4.Position.X = myTemporaryVoxel.Position.X;
			myTemporaryVoxel4.Position.Y = myTemporaryVoxel.Position.Y;
			myTemporaryVoxel4.Position.Z = myTemporaryVoxel.Position.Z + m_voxelSizeInMeters;
			myTemporaryVoxel5.Position.X = myTemporaryVoxel.Position.X;
			myTemporaryVoxel5.Position.Y = myTemporaryVoxel.Position.Y + m_voxelSizeInMeters;
			myTemporaryVoxel5.Position.Z = myTemporaryVoxel.Position.Z;
			myTemporaryVoxel6.Position.X = myTemporaryVoxel.Position.X + m_voxelSizeInMeters;
			myTemporaryVoxel6.Position.Y = myTemporaryVoxel.Position.Y + m_voxelSizeInMeters;
			myTemporaryVoxel6.Position.Z = myTemporaryVoxel.Position.Z;
			myTemporaryVoxel7.Position.X = myTemporaryVoxel.Position.X + m_voxelSizeInMeters;
			myTemporaryVoxel7.Position.Y = myTemporaryVoxel.Position.Y + m_voxelSizeInMeters;
			myTemporaryVoxel7.Position.Z = myTemporaryVoxel.Position.Z + m_voxelSizeInMeters;
			myTemporaryVoxel8.Position.X = myTemporaryVoxel.Position.X;
			myTemporaryVoxel8.Position.Y = myTemporaryVoxel.Position.Y + m_voxelSizeInMeters;
			myTemporaryVoxel8.Position.Z = myTemporaryVoxel.Position.Z + m_voxelSizeInMeters;
			GetVoxelNormal(myTemporaryVoxel, ref coord0, ref p, myTemporaryVoxel);
			GetVoxelNormal(myTemporaryVoxel2, ref coord, ref p2, myTemporaryVoxel);
			GetVoxelNormal(myTemporaryVoxel3, ref coord2, ref p3, myTemporaryVoxel);
			GetVoxelNormal(myTemporaryVoxel4, ref coord3, ref p4, myTemporaryVoxel);
			GetVoxelNormal(myTemporaryVoxel5, ref coord4, ref p5, myTemporaryVoxel);
			GetVoxelNormal(myTemporaryVoxel6, ref coord5, ref p6, myTemporaryVoxel);
			GetVoxelNormal(myTemporaryVoxel7, ref coord6, ref p7, myTemporaryVoxel);
			GetVoxelNormal(myTemporaryVoxel8, ref coord7, ref p8, myTemporaryVoxel);
			GetVoxelAmbient(myTemporaryVoxel, ref coord0, ref p);
			GetVoxelAmbient(myTemporaryVoxel2, ref coord, ref p2);
			GetVoxelAmbient(myTemporaryVoxel3, ref coord2, ref p3);
			GetVoxelAmbient(myTemporaryVoxel4, ref coord3, ref p4);
			GetVoxelAmbient(myTemporaryVoxel5, ref coord4, ref p5);
			GetVoxelAmbient(myTemporaryVoxel6, ref coord5, ref p6);
			GetVoxelAmbient(myTemporaryVoxel7, ref coord6, ref p7);
			GetVoxelAmbient(myTemporaryVoxel8, ref coord7, ref p8);
			int num2 = MyMarchingCubesConstants.EdgeTable[cubeIndex];
			if ((num2 & 1) == 1)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel, myTemporaryVoxel2, 0);
			}
			if ((num2 & 2) == 2)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel2, myTemporaryVoxel3, 1);
			}
			if ((num2 & 4) == 4)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel3, myTemporaryVoxel4, 2);
			}
			if ((num2 & 8) == 8)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel4, myTemporaryVoxel, 3);
			}
			if ((num2 & 0x10) == 16)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel5, myTemporaryVoxel6, 4);
			}
			if ((num2 & 0x20) == 32)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel6, myTemporaryVoxel7, 5);
			}
			if ((num2 & 0x40) == 64)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel7, myTemporaryVoxel8, 6);
			}
			if ((num2 & 0x80) == 128)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel8, myTemporaryVoxel5, 7);
			}
			if ((num2 & 0x100) == 256)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel, myTemporaryVoxel5, 8);
			}
			if ((num2 & 0x200) == 512)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel2, myTemporaryVoxel6, 9);
			}
			if ((num2 & 0x400) == 1024)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel3, myTemporaryVoxel7, 10);
			}
			if ((num2 & 0x800) == 2048)
			{
				GetVertexInterpolation(cache, myTemporaryVoxel4, myTemporaryVoxel8, 11);
			}
			return p;
		}

		private void ComputeSizeAndOrigin(int lodIdx, Vector3I storageSize)
		{
			m_voxelSizeInMeters = 1f * (float)(1 << lodIdx);
			m_sizeMinusOne = (storageSize >> lodIdx) - 1;
			m_originPosition = m_voxelStart * m_voxelSizeInMeters + 0.5f * m_voxelSizeInMeters;
		}

		private void CreateTriangles(ref Vector3I coord0, int cubeIndex, ref Vector3I tempVoxelCoord0)
		{
			MyVoxelVertex myVoxelVertex = default(MyVoxelVertex);
			Vector3I vector3I = new Vector3I(coord0.X, coord0.Y, coord0.Z);
			for (int i = 0; MyMarchingCubesConstants.TriangleTable[cubeIndex, i] != -1; i += 3)
			{
				int num = MyMarchingCubesConstants.TriangleTable[cubeIndex, i];
				int num2 = MyMarchingCubesConstants.TriangleTable[cubeIndex, i + 1];
				int num3 = MyMarchingCubesConstants.TriangleTable[cubeIndex, i + 2];
				MyEdge myEdge = m_edges[num];
				MyEdge myEdge2 = m_edges[num2];
				MyEdge myEdge3 = m_edges[num3];
				Vector4I vector4I = MyMarchingCubesConstants.EdgeConversion[num];
				Vector4I vector4I2 = MyMarchingCubesConstants.EdgeConversion[num2];
				Vector4I vector4I3 = MyMarchingCubesConstants.EdgeConversion[num3];
				MyEdgeVertex myEdgeVertex = m_edgeVertex[vector3I.X + vector4I.X][vector3I.Y + vector4I.Y][vector3I.Z + vector4I.Z][vector4I.W];
				MyEdgeVertex myEdgeVertex2 = m_edgeVertex[vector3I.X + vector4I2.X][vector3I.Y + vector4I2.Y][vector3I.Z + vector4I2.Z][vector4I2.W];
				MyEdgeVertex myEdgeVertex3 = m_edgeVertex[vector3I.X + vector4I3.X][vector3I.Y + vector4I3.Y][vector3I.Z + vector4I3.Z][vector4I3.W];
				MyVoxelVertex edge = default(MyVoxelVertex);
				edge.Position = myEdge.Position;
				MyVoxelVertex edge2 = default(MyVoxelVertex);
				edge2.Position = myEdge2.Position;
				MyVoxelVertex edge3 = default(MyVoxelVertex);
				edge3.Position = myEdge3.Position;
				if (!IsWrongTriangle(ref edge, ref edge2, ref edge3))
				{
					if (myEdgeVertex.CalcCounter != m_edgeVertexCalcCounter)
					{
						myEdgeVertex.CalcCounter = m_edgeVertexCalcCounter;
						myEdgeVertex.VertexIndex = (ushort)m_resultVerticesCounter;
						myVoxelVertex.Position = myEdge.Position;
						myVoxelVertex.Normal = myEdge.Normal;
						myVoxelVertex.Material = myEdge.Material;
						m_resultVertices[m_resultVerticesCounter] = myVoxelVertex;
						m_resultVerticesCounter++;
					}
					if (myEdgeVertex2.CalcCounter != m_edgeVertexCalcCounter)
					{
						myEdgeVertex2.CalcCounter = m_edgeVertexCalcCounter;
						myEdgeVertex2.VertexIndex = (ushort)m_resultVerticesCounter;
						myVoxelVertex.Position = myEdge2.Position;
						myVoxelVertex.Normal = myEdge2.Normal;
						myVoxelVertex.Material = myEdge2.Material;
						m_resultVertices[m_resultVerticesCounter] = myVoxelVertex;
						m_resultVerticesCounter++;
					}
					if (myEdgeVertex3.CalcCounter != m_edgeVertexCalcCounter)
					{
						myEdgeVertex3.CalcCounter = m_edgeVertexCalcCounter;
						myEdgeVertex3.VertexIndex = (ushort)m_resultVerticesCounter;
						myVoxelVertex.Position = myEdge3.Position;
						myVoxelVertex.Normal = myEdge3.Normal;
						myVoxelVertex.Material = myEdge3.Material;
						myVoxelVertex.Cell = coord0;
						m_resultVertices[m_resultVerticesCounter] = myVoxelVertex;
						m_resultVerticesCounter++;
					}
					m_resultTriangles[m_resultTrianglesCounter].V0 = myEdgeVertex.VertexIndex;
					m_resultTriangles[m_resultTrianglesCounter].V1 = myEdgeVertex2.VertexIndex;
					m_resultTriangles[m_resultTrianglesCounter].V2 = myEdgeVertex3.VertexIndex;
					m_resultTrianglesCounter++;
				}
			}
		}

		private bool IsWrongTriangle(ref MyVoxelVertex edge0, ref MyVoxelVertex edge1, ref MyVoxelVertex edge2)
		{
			return MyUtils.IsWrongTriangle(edge0.Position, edge1.Position, edge2.Position);
		}
	}
}
