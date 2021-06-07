using System.Collections.Generic;
using VRage.Game.Voxels;
using VRageMath;

namespace Sandbox.Game.Utils
{
	internal class MyOctree
	{
		private const int NODE_COUNT = 73;

		private byte[] m_childEmpty = new byte[9];

		private short[] m_firstTriangleIndex = new short[73];

		private byte[] m_triangleCount = new byte[73];

		private Vector3 m_bbMin;

		private Vector3 m_bbInvScale;

		private const float CHILD_SIZE = 0.65f;

		private static readonly BoundingBox[] box;

		public void Init(Vector3[] positions, int vertexCount, MyVoxelTriangle[] triangles, int triangleCount, out MyVoxelTriangle[] sortedTriangles)
		{
			for (int i = 0; i < 73; i++)
			{
				m_firstTriangleIndex[i] = 0;
				m_triangleCount[i] = 0;
			}
			for (int j = 0; j < 9; j++)
			{
				m_childEmpty[j] = 0;
			}
			BoundingBox boundingBox = BoundingBox.CreateInvalid();
			for (int k = 0; k < vertexCount; k++)
			{
				boundingBox.Include(ref positions[k]);
			}
			m_bbMin = boundingBox.Min;
			Vector3 vector = boundingBox.Max - boundingBox.Min;
			m_bbInvScale = Vector3.One;
			if (vector.X > 1f)
			{
				m_bbInvScale.X = 1f / vector.X;
			}
			if (vector.Y > 1f)
			{
				m_bbInvScale.Y = 1f / vector.Y;
			}
			if (vector.Z > 1f)
			{
				m_bbInvScale.Z = 1f / vector.Z;
			}
			for (int l = 0; l < triangleCount; l++)
			{
				MyVoxelTriangle myVoxelTriangle = triangles[l];
				BoundingBox triangleAabb = BoundingBox.CreateInvalid();
				triangleAabb.Include(ref positions[myVoxelTriangle.V0], ref positions[myVoxelTriangle.V1], ref positions[myVoxelTriangle.V2]);
				m_triangleCount[GetNode(ref triangleAabb)]++;
			}
			m_firstTriangleIndex[0] = m_triangleCount[0];
			for (int m = 1; m < 73; m++)
			{
				m_firstTriangleIndex[m] = (short)(m_firstTriangleIndex[m - 1] + m_triangleCount[m]);
			}
			MyVoxelTriangle[] array = new MyVoxelTriangle[triangleCount];
			for (int n = 0; n < triangleCount; n++)
			{
				MyVoxelTriangle myVoxelTriangle2 = triangles[n];
				BoundingBox triangleAabb2 = BoundingBox.CreateInvalid();
				triangleAabb2.Include(ref positions[myVoxelTriangle2.V0], ref positions[myVoxelTriangle2.V1], ref positions[myVoxelTriangle2.V2]);
				array[--m_firstTriangleIndex[GetNode(ref triangleAabb2)]] = myVoxelTriangle2;
			}
			sortedTriangles = array;
			for (int num = 72; num > 0; num--)
			{
				if (m_triangleCount[num] == 0 && (num > 8 || m_childEmpty[num] == byte.MaxValue))
				{
					m_childEmpty[num - 1 >> 3] |= (byte)(1 << ((num - 1) & 7));
				}
			}
		}

		public void BoxQuery(ref BoundingBox bbox, List<int> triangleIndices)
		{
			BoundingBox boundingBox = new BoundingBox((bbox.Min - m_bbMin) * m_bbInvScale, (bbox.Max - m_bbMin) * m_bbInvScale);
			box[0].Intersects(ref boundingBox, out bool result);
			if (!result)
			{
				return;
			}
			for (int i = 0; i < m_triangleCount[0]; i++)
			{
				triangleIndices.Add(m_firstTriangleIndex[0] + i);
			}
			int num = 1;
			int num2 = 1;
			while (num < 9)
			{
				if ((m_childEmpty[0] & num2) == 0)
				{
					box[num].Intersects(ref boundingBox, out result);
					if (result)
					{
						for (int j = 0; j < m_triangleCount[num]; j++)
						{
							triangleIndices.Add(m_firstTriangleIndex[num] + j);
						}
						int num3 = num * 8 + 1;
						int num4 = 1;
						while (num3 < num * 8 + 9)
						{
							if ((m_childEmpty[num] & num4) == 0)
							{
								box[num3].Intersects(ref boundingBox, out result);
								if (result)
								{
									for (int k = 0; k < m_triangleCount[num3]; k++)
									{
										triangleIndices.Add(m_firstTriangleIndex[num3] + k);
									}
								}
							}
							num3++;
							num4 <<= 1;
						}
					}
				}
				num++;
				num2 <<= 1;
			}
		}

		public void GetIntersectionWithLine(ref Ray ray, List<int> triangleIndices)
		{
			Ray ray2 = new Ray((ray.Position - m_bbMin) * m_bbInvScale, ray.Direction * m_bbInvScale);
			box[0].Intersects(ref ray2, out float? result);
			if (!result.HasValue)
			{
				return;
			}
			for (int i = 0; i < m_triangleCount[0]; i++)
			{
				triangleIndices.Add(m_firstTriangleIndex[0] + i);
			}
			int num = 1;
			int num2 = 1;
			while (num < 9)
			{
				if ((m_childEmpty[0] & num2) == 0)
				{
					box[num].Intersects(ref ray2, out result);
					if (result.HasValue)
					{
						for (int j = 0; j < m_triangleCount[num]; j++)
						{
							triangleIndices.Add(m_firstTriangleIndex[num] + j);
						}
						int num3 = num * 8 + 1;
						int num4 = 1;
						while (num3 < num * 8 + 9)
						{
							if ((m_childEmpty[num] & num4) == 0)
							{
								box[num3].Intersects(ref ray2, out result);
								if (result.HasValue)
								{
									for (int k = 0; k < m_triangleCount[num3]; k++)
									{
										triangleIndices.Add(m_firstTriangleIndex[num3] + k);
									}
								}
							}
							num3++;
							num4 <<= 1;
						}
					}
				}
				num++;
				num2 <<= 1;
			}
		}

		static MyOctree()
		{
			box = new BoundingBox[73];
			int num = 0;
			int num2 = 0;
			while (num2 < 1)
			{
				box[num].Min = Vector3.Zero;
				box[num].Max = Vector3.One;
				num2++;
				num++;
			}
			int num3 = 0;
			while (num3 < 8)
			{
				if ((num3 & 4) == 0)
				{
					box[num].Min.Z = 0f;
					box[num].Max.Z = 0.65f;
				}
				else
				{
					box[num].Min.Z = 0.350000024f;
					box[num].Max.Z = 1f;
				}
				if ((num3 & 2) == 0)
				{
					box[num].Min.Y = 0f;
					box[num].Max.Y = 0.65f;
				}
				else
				{
					box[num].Min.Y = 0.350000024f;
					box[num].Max.Y = 1f;
				}
				if ((num3 & 1) == 0)
				{
					box[num].Min.X = 0f;
					box[num].Max.X = 0.65f;
				}
				else
				{
					box[num].Min.X = 0.350000024f;
					box[num].Max.X = 1f;
				}
				num3++;
				num++;
			}
			int num4 = 0;
			while (num4 < 64)
			{
				if ((num4 & 0x20) == 0)
				{
					box[num].Min.Z = 0f;
					box[num].Max.Z = 0.65f;
				}
				else
				{
					box[num].Min.Z = 0.350000024f;
					box[num].Max.Z = 1f;
				}
				if ((num4 & 0x10) == 0)
				{
					box[num].Min.Y = 0f;
					box[num].Max.Y = 0.65f;
				}
				else
				{
					box[num].Min.Y = 0.350000024f;
					box[num].Max.Y = 1f;
				}
				if ((num4 & 8) == 0)
				{
					box[num].Min.X = 0f;
					box[num].Max.X = 0.65f;
				}
				else
				{
					box[num].Min.X = 0.350000024f;
					box[num].Max.X = 1f;
				}
				if ((num4 & 4) == 0)
				{
					box[num].Max.Z = box[num].Min.Z + (box[num].Max.Z - box[num].Min.Z) * 0.65f;
				}
				else
				{
					box[num].Min.Z = box[num].Min.Z + (box[num].Max.Z - box[num].Min.Z) * 0.350000024f;
				}
				if ((num4 & 2) == 0)
				{
					box[num].Max.Y = box[num].Min.Y + (box[num].Max.Y - box[num].Min.Y) * 0.65f;
				}
				else
				{
					box[num].Min.Y = box[num].Min.Y + (box[num].Max.Y - box[num].Min.Y) * 0.350000024f;
				}
				if ((num4 & 1) == 0)
				{
					box[num].Max.X = box[num].Min.X + (box[num].Max.X - box[num].Min.X) * 0.65f;
				}
				else
				{
					box[num].Min.X = box[num].Min.X + (box[num].Max.X - box[num].Min.X) * 0.350000024f;
				}
				num4++;
				num++;
			}
		}

		private int GetNode(ref BoundingBox triangleAabb)
		{
			BoundingBox boundingBox = new BoundingBox((triangleAabb.Min - m_bbMin) * m_bbInvScale, (triangleAabb.Max - m_bbMin) * m_bbInvScale);
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				int num2 = num * 8 + 1;
				if (boundingBox.Min.X > box[num2 + 1].Min.X)
				{
					num2++;
				}
				else if (boundingBox.Max.X >= box[num2].Max.X)
				{
					break;
				}
				if (boundingBox.Min.Y > box[num2 + 2].Min.Y)
				{
					num2 += 2;
				}
				else if (boundingBox.Max.Y >= box[num2].Max.Y)
				{
					break;
				}
				if (boundingBox.Min.Z > box[num2 + 4].Min.Z)
				{
					num2 += 4;
				}
				else if (boundingBox.Max.Z >= box[num2].Max.Z)
				{
					break;
				}
				num = num2;
			}
			return num;
		}
	}
}
