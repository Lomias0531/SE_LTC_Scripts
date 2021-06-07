using Sandbox.Game.Entities;
using System;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Library;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Engine.Voxels.Planet
{
	public class MyHeightmapFace : IMyWrappedCubemapFace, IDisposable
	{
		public struct HeightmapNode
		{
			public static readonly int HEIGHTMAP_BRANCH_FACTOR = 4;

			public static readonly int HEIGHTMAP_LEAF_SIZE = 8;

			public float Min;

			public float Max;

			internal ContainmentType Intersect(ref BoundingBox query)
			{
				if (query.Min.Z > Max)
				{
					return ContainmentType.Disjoint;
				}
				if (query.Max.Z < Min)
				{
					return ContainmentType.Contains;
				}
				return ContainmentType.Intersects;
			}
		}

		public struct HeightmapLevel
		{
			public HeightmapNode[] Nodes;

			private uint m_res;

			public float Recip;

			public uint Res
			{
				get
				{
					return m_res;
				}
				set
				{
					m_res = value;
					Recip = 1f / (float)m_res;
				}
			}

			public ContainmentType Intersect(int x, int y, ref BoundingBox query)
			{
				return Nodes[y * Res + x].Intersect(ref query);
			}

			public bool IsCellContained(int x, int y, ref BoundingBox box)
			{
				Vector2 value = new Vector2(x, y) * Recip;
				Vector2 vector = value + Recip;
				if (box.Min.X <= value.X && box.Min.Y <= value.Y && box.Max.X >= vector.X && box.Max.Y >= vector.Y)
				{
					return true;
				}
				return false;
			}

			public bool IsCellNotContained(int x, int y, ref BoundingBox box)
			{
				Vector2 value = new Vector2(x, y) * Recip;
				Vector2 vector = value + Recip;
				if (box.Min.X > value.X || box.Min.Y > value.Y || box.Max.X < vector.X || box.Max.Y > vector.Y)
				{
					return true;
				}
				return false;
			}
		}

		private struct Box2I
		{
			public Vector2I Min;

			public Vector2I Max;

			public Box2I(ref BoundingBox bb, uint scale)
			{
				Min = new Vector2I((int)(bb.Min.X * (float)scale), (int)(bb.Min.Y * (float)scale));
				Max = new Vector2I((int)(bb.Max.X * (float)scale), (int)(bb.Max.Y * (float)scale));
			}

			public Box2I(Vector2I min, Vector2I max)
			{
				Min = min;
				Max = max;
			}

			public void Intersect(ref Box2I other)
			{
				Min.X = Math.Max(Min.X, other.Min.X);
				Min.Y = Math.Max(Min.Y, other.Min.Y);
				Max.X = Math.Min(Max.X, other.Max.X);
				Max.Y = Math.Min(Max.Y, other.Max.Y);
			}

			public override string ToString()
			{
				return $"[({Min}), ({Max})]";
			}
		}

		private struct SEntry
		{
			public Box2I Bounds;

			public Vector2I Next;

			public ContainmentType Result;

			public ContainmentType Intersection;

			public uint Level;

			public bool Continue;

			public SEntry(ref BoundingBox query, uint res, Vector2I cell, ContainmentType result, uint level)
			{
				Box2I bounds = new Box2I(ref query, res);
				cell *= HeightmapNode.HEIGHTMAP_BRANCH_FACTOR;
				Box2I other = new Box2I(cell, cell + HeightmapNode.HEIGHTMAP_BRANCH_FACTOR - 1);
				bounds.Intersect(ref other);
				Bounds = bounds;
				Next = bounds.Min;
				Level = level;
				Result = result;
				Intersection = result;
				Continue = false;
			}
		}

		private static NativeArrayAllocator BufferAllocator;

		public static readonly MyHeightmapFace Default;

		private int m_realResolution;

		private float m_pixelSizeFour;

		private Box2I m_bounds;

		private NativeArray m_dataBuffer;

		public unsafe ushort* Data;

		public HeightmapNode Root;

		public HeightmapLevel[] PruningTree;

		[ThreadStatic]
		private static SEntry[] m_queryStack;

		public bool IsPersistent
		{
			get;
			private set;
		}

		public int Resolution
		{
			get;
			set;
		}

		public int ResolutionMinusOne
		{
			get;
			set;
		}

		public int RowStride => m_realResolution;

		public unsafe ushort this[int x, int y]
		{
			get
			{
				if (x < 0)
				{
					x = 0;
				}
				else if (x >= Resolution)
				{
					x = Resolution - 1;
				}
				if (y < 0)
				{
					y = 0;
				}
				else if (y >= Resolution)
				{
					y = Resolution - 1;
				}
				return Data[(y + 1) * m_realResolution + (x + 1)];
			}
		}

		unsafe static MyHeightmapFace()
		{
			BufferAllocator = new NativeArrayAllocator(MyPlanet.MemoryTracker.RegisterSubsystem("HeightmapFaces"));
			Default = new MyHeightmapFace(HeightmapNode.HEIGHTMAP_LEAF_SIZE);
			Default.IsPersistent = true;
			Unsafe.InitBlockUnaligned(Default.Data, 0, (uint)Default.m_dataBuffer.Size);
			MyVRage.RegisterExitCallback(delegate
			{
				Default.Dispose();
			});
		}

		public unsafe MyHeightmapFace(int resolution)
		{
			m_realResolution = resolution + 2;
			Resolution = resolution;
			ResolutionMinusOne = Resolution - 1;
			m_dataBuffer = BufferAllocator.Allocate(m_realResolution * m_realResolution * 2);
			Data = (ushort*)(void*)m_dataBuffer.Ptr;
			m_pixelSizeFour = 4f / (float)Resolution;
			m_bounds = new Box2I(Vector2I.Zero, new Vector2I(Resolution - 1));
		}

		public unsafe void GetValue(int x, int y, out ushort value)
		{
			value = Data[(y + 1) * m_realResolution + (x + 1)];
		}

		public unsafe void Get4Row(int linearOfft, float* values, ushort* map)
		{
			*values = (float)(int)map[linearOfft] * 1.52590219E-05f;
			values[1] = (float)(int)map[linearOfft + 1] * 1.52590219E-05f;
			values[2] = (float)(int)map[linearOfft + 2] * 1.52590219E-05f;
			values[3] = (float)(int)map[linearOfft + 3] * 1.52590219E-05f;
		}

		public unsafe void Get4Row(int linearOfft, float* values)
		{
			*values = (float)(int)Data[linearOfft] * 1.52590219E-05f;
			values[1] = (float)(int)Data[linearOfft + 1] * 1.52590219E-05f;
			values[2] = (float)(int)Data[linearOfft + 2] * 1.52590219E-05f;
			values[3] = (float)(int)Data[linearOfft + 3] * 1.52590219E-05f;
		}

		public unsafe void GetHermiteSliceRow(int linearOfft, float* values)
		{
			*values = (float)(int)Data[linearOfft + 1] * 1.52590219E-05f;
			values[1] = (float)(Data[linearOfft + 2] - Data[linearOfft]) * 3.05180438E-05f;
			values[2] = (float)(int)Data[linearOfft + 2] * 1.52590219E-05f;
			values[3] = (float)(Data[linearOfft + 3] - Data[linearOfft + 1]) * 3.05180438E-05f;
		}

		public unsafe ushort GetValue(int x, int y)
		{
			if (x < 0)
			{
				x = 0;
			}
			else if (x >= Resolution)
			{
				x = Resolution - 1;
			}
			if (y < 0)
			{
				y = 0;
			}
			else if (y >= Resolution)
			{
				y = Resolution - 1;
			}
			return Data[(y + 1) * m_realResolution + (x + 1)];
		}

		public unsafe float GetValuef(int x, int y)
		{
			return (float)(int)Data[(y + 1) * m_realResolution + (x + 1)] * 1.52590219E-05f;
		}

		public unsafe void SetValue(int x, int y, ushort value)
		{
			Data[(y + 1) * m_realResolution + (x + 1)] = value;
		}

		public int GetRowStart(int y)
		{
			return (y + 1) * m_realResolution + 1;
		}

		public void CopyRange(Vector2I start, Vector2I end, MyHeightmapFace other, Vector2I oStart, Vector2I oEnd)
		{
			Vector2I step = MyCubemapHelpers.GetStep(ref start, ref end);
			Vector2I step2 = MyCubemapHelpers.GetStep(ref oStart, ref oEnd);
			ushort value;
			while (start != end)
			{
				other.GetValue(oStart.X, oStart.Y, out value);
				SetValue(start.X, start.Y, value);
				start += step;
				oStart += step2;
			}
			other.GetValue(oStart.X, oStart.Y, out value);
			SetValue(start.X, start.Y, value);
		}

		public void CopyRange(Vector2I start, Vector2I end, IMyWrappedCubemapFace other, Vector2I oStart, Vector2I oEnd)
		{
			CopyRange(start, end, other as MyHeightmapFace, oStart, oEnd);
		}

		public void FinishFace(string faceName)
		{
			int num = Resolution - 1;
			int value = GetValue(0, 0);
			value += GetValue(-1, 0);
			value += GetValue(0, -1);
			SetValue(-1, -1, (ushort)(value / 3));
			value = GetValue(num, 0);
			value += GetValue(Resolution, 0);
			value += GetValue(num, -1);
			SetValue(Resolution, -1, (ushort)(value / 3));
			value = GetValue(0, num);
			value += GetValue(-1, num);
			value += GetValue(0, Resolution);
			SetValue(-1, Resolution, (ushort)(value / 3));
			value = GetValue(num, num);
			value += GetValue(Resolution, num);
			value += GetValue(num, Resolution);
			SetValue(Resolution, Resolution, (ushort)(value / 3));
			CreatePruningTree(faceName);
		}

		public unsafe void CreatePruningTree(string mapName)
		{
			int num = 0;
			int resolution = Resolution;
			for (resolution /= HeightmapNode.HEIGHTMAP_LEAF_SIZE; resolution != 1; resolution /= HeightmapNode.HEIGHTMAP_BRANCH_FACTOR)
			{
				if (resolution % HeightmapNode.HEIGHTMAP_BRANCH_FACTOR != 0)
				{
					MyLog.Default.Error("Cannot build prunning tree for heightmap face {0}!", mapName);
					MyLog.Default.Error("Heightmap resolution must be divisible by {1}, and after that a power of {0}. Failing to achieve so will disable several important optimizations!!", HeightmapNode.HEIGHTMAP_BRANCH_FACTOR, HeightmapNode.HEIGHTMAP_LEAF_SIZE);
					return;
				}
				num++;
			}
			PruningTree = new HeightmapLevel[num];
			int num2 = GetRowStart(0);
			if (num == 0)
			{
				float num3 = float.PositiveInfinity;
				float num4 = float.NegativeInfinity;
				int num5 = num2;
				for (int i = 0; i < HeightmapNode.HEIGHTMAP_LEAF_SIZE; i++)
				{
					for (int j = 0; j < HeightmapNode.HEIGHTMAP_LEAF_SIZE; j++)
					{
						float num6 = (float)(int)Data[num5 + j] * 1.52590219E-05f;
						if (num3 > num6)
						{
							num3 = num6;
						}
						if (num4 < num6)
						{
							num4 = num6;
						}
					}
					num5 += m_realResolution;
				}
				Root.Max = num4;
				Root.Min = num3;
				return;
			}
			_ = HeightmapNode.HEIGHTMAP_BRANCH_FACTOR;
			resolution = Resolution / HeightmapNode.HEIGHTMAP_LEAF_SIZE;
			PruningTree[0].Nodes = new HeightmapNode[resolution * resolution];
			PruningTree[0].Res = (uint)resolution;
			int num7 = 0;
			HeightmapNode heightmapNode;
			for (int k = 0; k < resolution; k++)
			{
				int num8 = num2;
				for (int l = 0; l < resolution; l++)
				{
					float num9 = float.PositiveInfinity;
					float num10 = float.NegativeInfinity;
					int num11 = num8 - m_realResolution;
					for (int m = -1; m <= HeightmapNode.HEIGHTMAP_LEAF_SIZE; m++)
					{
						for (int n = -1; n <= HeightmapNode.HEIGHTMAP_LEAF_SIZE; n++)
						{
							float num12 = (float)(int)Data[num11 + n] * 1.52590219E-05f;
							if (num9 > num12)
							{
								num9 = num12;
							}
							if (num10 < num12)
							{
								num10 = num12;
							}
						}
						num11 += m_realResolution;
					}
					heightmapNode = (PruningTree[0].Nodes[num7] = new HeightmapNode
					{
						Max = num10,
						Min = num9
					});
					num7++;
					num8 += HeightmapNode.HEIGHTMAP_LEAF_SIZE;
				}
				num2 += HeightmapNode.HEIGHTMAP_LEAF_SIZE * m_realResolution;
			}
			int num13 = 0;
			for (int num14 = 1; num14 < num; num14++)
			{
				num2 = 0;
				int num15 = resolution / HeightmapNode.HEIGHTMAP_BRANCH_FACTOR;
				PruningTree[num14].Nodes = new HeightmapNode[num15 * num15];
				PruningTree[num14].Res = (uint)num15;
				num7 = 0;
				for (int num16 = 0; num16 < num15; num16++)
				{
					int num17 = num2;
					for (int num18 = 0; num18 < num15; num18++)
					{
						float num19 = float.PositiveInfinity;
						float num20 = float.NegativeInfinity;
						int num21 = num17;
						for (int num22 = 0; num22 < HeightmapNode.HEIGHTMAP_BRANCH_FACTOR; num22++)
						{
							for (int num23 = 0; num23 < HeightmapNode.HEIGHTMAP_BRANCH_FACTOR; num23++)
							{
								HeightmapNode heightmapNode2 = PruningTree[num13].Nodes[num21 + num23];
								if (num19 > heightmapNode2.Min)
								{
									num19 = heightmapNode2.Min;
								}
								if (num20 < heightmapNode2.Max)
								{
									num20 = heightmapNode2.Max;
								}
							}
							num21 += resolution;
						}
						heightmapNode = (PruningTree[num14].Nodes[num7] = new HeightmapNode
						{
							Max = num20,
							Min = num19
						});
						num7++;
						num17 += HeightmapNode.HEIGHTMAP_BRANCH_FACTOR;
					}
					num2 += HeightmapNode.HEIGHTMAP_BRANCH_FACTOR * resolution;
				}
				num13++;
				resolution = num15;
			}
			float num24 = float.PositiveInfinity;
			float num25 = float.NegativeInfinity;
			num2 = 0;
			for (int num26 = 0; num26 < HeightmapNode.HEIGHTMAP_BRANCH_FACTOR; num26++)
			{
				for (int num27 = 0; num27 < HeightmapNode.HEIGHTMAP_BRANCH_FACTOR; num27++)
				{
					HeightmapNode heightmapNode3 = PruningTree[num - 1].Nodes[num2++];
					if (num24 > heightmapNode3.Min)
					{
						num24 = heightmapNode3.Min;
					}
					if (num25 < heightmapNode3.Max)
					{
						num25 = heightmapNode3.Max;
					}
				}
			}
			Root.Max = num25;
			Root.Min = num24;
		}

		public ContainmentType QueryHeight(ref BoundingBox query)
		{
			if (PruningTree == null)
			{
				return ContainmentType.Intersects;
			}
			if (m_queryStack == null || m_queryStack.Length < PruningTree.Length)
			{
				m_queryStack = new SEntry[PruningTree.Length];
			}
			if (query.Min.Z > Root.Max)
			{
				return ContainmentType.Disjoint;
			}
			if (query.Max.Z < Root.Min)
			{
				return ContainmentType.Contains;
			}
			if (query.Max.X < 0f || query.Max.Y < 0f || query.Min.X > 1f || query.Min.Y > 1f)
			{
				return ContainmentType.Disjoint;
			}
			if (PruningTree.Length == 0)
			{
				return ContainmentType.Intersects;
			}
			if ((double)query.Max.X == 1.0)
			{
				query.Max.X = 1f;
			}
			if ((double)query.Max.Y == 1.0)
			{
				query.Max.Y = 1f;
			}
			ContainmentType containmentType = ContainmentType.Intersects;
			float num = Math.Max(query.Width, query.Height);
			if (num < m_pixelSizeFour)
			{
				Box2I box2I = new Box2I(ref query, (uint)Resolution);
				box2I.Min -= 1;
				box2I.Max += 1;
				box2I.Intersect(ref m_bounds);
				int num2 = (int)(query.Min.Z * 65535f);
				int num3 = (int)(query.Max.Z * 65535f);
				GetValue(box2I.Min.X, box2I.Min.Y, out ushort value);
				if (value > num3)
				{
					containmentType = ContainmentType.Contains;
				}
				else
				{
					if (value >= num2)
					{
						return ContainmentType.Intersects;
					}
					containmentType = ContainmentType.Disjoint;
				}
				int num4 = 65535;
				int num5 = 0;
				for (int i = box2I.Min.Y; i <= box2I.Max.Y; i++)
				{
					for (int j = box2I.Min.X; j <= box2I.Max.X; j++)
					{
						GetValue(j, i, out value);
						if (value > num5)
						{
							num5 = value;
						}
						if (value < num4)
						{
							num4 = value;
						}
					}
				}
				int num6 = num5 - num4;
				num6 += num6 >> 1;
				num5 += num6;
				num4 -= num6;
				if (num2 > num5)
				{
					return ContainmentType.Disjoint;
				}
				if (num3 < num4)
				{
					return ContainmentType.Contains;
				}
				return ContainmentType.Intersects;
			}
			uint num7 = (uint)MathHelper.Clamp(Math.Log(num * (float)(Resolution / HeightmapNode.HEIGHTMAP_LEAF_SIZE)) / Math.Log(HeightmapNode.HEIGHTMAP_BRANCH_FACTOR), 0.0, PruningTree.Length - 1);
			int num8 = 0;
			SEntry[] queryStack = m_queryStack;
			Box2I other = new Box2I(Vector2I.Zero, new Vector2I((int)(PruningTree[num7].Res - 1)));
			queryStack[0].Bounds = new Box2I(ref query, PruningTree[num7].Res);
			queryStack[0].Bounds.Intersect(ref other);
			queryStack[0].Next = queryStack[0].Bounds.Min;
			queryStack[0].Level = num7;
			queryStack[0].Result = containmentType;
			queryStack[0].Continue = false;
			while (num8 != -1)
			{
				SEntry sEntry = queryStack[num8];
				int num9 = sEntry.Next.Y;
				while (true)
				{
					int k;
					if (num9 <= sEntry.Bounds.Max.Y)
					{
						for (k = sEntry.Bounds.Min.X; k <= sEntry.Bounds.Max.X; k++)
						{
							if (!sEntry.Continue)
							{
								sEntry.Intersection = PruningTree[sEntry.Level].Intersect(k, num9, ref query);
								if (sEntry.Intersection == ContainmentType.Intersects && PruningTree[sEntry.Level].IsCellNotContained(k, num9, ref query) && sEntry.Level != 0)
								{
									goto IL_0409;
								}
							}
							else
							{
								sEntry.Continue = false;
								k = sEntry.Next.X;
							}
							switch (sEntry.Intersection)
							{
							case ContainmentType.Intersects:
								sEntry.Result = ContainmentType.Intersects;
								break;
							case ContainmentType.Disjoint:
								if (sEntry.Result == ContainmentType.Contains)
								{
									sEntry.Result = ContainmentType.Intersects;
									break;
								}
								sEntry.Result = ContainmentType.Disjoint;
								continue;
							case ContainmentType.Contains:
								if (sEntry.Result == ContainmentType.Disjoint)
								{
									sEntry.Result = ContainmentType.Intersects;
									break;
								}
								sEntry.Result = ContainmentType.Contains;
								continue;
							default:
								continue;
							}
							goto IL_0527;
						}
						num9++;
						continue;
					}
					goto IL_0527;
					IL_0527:
					containmentType = sEntry.Result;
					num8--;
					if (num8 >= 0)
					{
						queryStack[num8].Intersection = containmentType;
					}
					break;
					IL_0409:
					sEntry.Next = new Vector2I(k, num9);
					sEntry.Continue = true;
					queryStack[num8] = sEntry;
					num8++;
					queryStack[num8] = new SEntry(ref query, PruningTree[sEntry.Level - 1].Res, new Vector2I(k, num9), sEntry.Result, sEntry.Level - 1);
					break;
				}
			}
			return containmentType;
		}

		public void GetBounds(ref BoundingBox query)
		{
			float num = Math.Max(query.Width, query.Height);
			if (num < m_pixelSizeFour || PruningTree == null || PruningTree.Length == 0)
			{
				Box2I box2I = new Box2I(ref query, (uint)Resolution);
				box2I.Min -= 1;
				box2I.Max += 1;
				box2I.Intersect(ref m_bounds);
				GetValue(box2I.Min.X, box2I.Min.Y, out ushort value);
				int num2 = 65535;
				int num3 = 0;
				for (int i = box2I.Min.Y; i <= box2I.Max.Y; i++)
				{
					for (int j = box2I.Min.X; j <= box2I.Max.X; j++)
					{
						GetValue(j, i, out value);
						if (value > num3)
						{
							num3 = value;
						}
						if (value < num2)
						{
							num2 = value;
						}
					}
				}
				int num4 = num3 - num2;
				num4 = num4 * 2 / 3;
				num3 += num4;
				num2 -= num4;
				query.Min.Z = (float)num2 * 1.52590219E-05f;
				query.Max.Z = (float)num3 * 1.52590219E-05f;
				return;
			}
			double value2 = Math.Log((float)Resolution / (num * (float)HeightmapNode.HEIGHTMAP_LEAF_SIZE)) / Math.Log(HeightmapNode.HEIGHTMAP_BRANCH_FACTOR);
			uint num5 = (uint)(PruningTree.Length - 1 - (int)(uint)MathHelper.Clamp(value2, 0.0, PruningTree.Length - 1));
			Box2I other = new Box2I(Vector2I.Zero, new Vector2I((int)(PruningTree[num5].Res - 1)));
			Box2I box2I2 = new Box2I(ref query, PruningTree[num5].Res);
			box2I2.Intersect(ref other);
			query.Min.Z = float.PositiveInfinity;
			query.Max.Z = float.NegativeInfinity;
			int res = (int)PruningTree[num5].Res;
			for (int k = box2I2.Min.Y; k <= box2I2.Max.Y; k++)
			{
				for (int l = box2I2.Min.X; l <= box2I2.Max.X; l++)
				{
					HeightmapNode heightmapNode = PruningTree[num5].Nodes[k * res + l];
					if (query.Min.Z > heightmapNode.Min)
					{
						query.Min.Z = heightmapNode.Min;
					}
					if (query.Max.Z < heightmapNode.Max)
					{
						query.Max.Z = heightmapNode.Max;
					}
				}
			}
		}

		public bool QueryLine(ref Vector3 from, ref Vector3 to, out float startOffset, out float endOffset)
		{
			if (PruningTree == null)
			{
				startOffset = 0f;
				endOffset = 1f;
				return true;
			}
			Vector2 vector = new Vector2(from.X, from.Y);
			Vector2 vector2 = new Vector2(to.X, to.Y);
			vector *= (float)ResolutionMinusOne;
			vector2 *= (float)ResolutionMinusOne;
			int num = (int)Math.Ceiling((vector2 - vector).Length());
			Vector3 vector3 = new Vector3(vector - vector2, (to.Z - from.Z) * 65535f);
			float z = vector3.Z;
			float num2 = 1f / (float)num;
			vector3 *= num2;
			Vector3 vector4 = new Vector3(vector, from.Z * 65535f);
			float z2 = vector4.Z;
			for (int i = 0; i < num; i++)
			{
				int num3 = (int)Math.Round(vector4.X);
				int num4 = (int)Math.Round(vector4.Y);
				int num5 = (int)vector4.Z;
				int num6 = (int)(vector4.Z + vector3.Z + 0.5f);
				if (num5 > num6)
				{
					int num7 = num6;
					num6 = num5;
					num5 = num7;
				}
				int num8 = int.MaxValue;
				int num9 = int.MinValue;
				for (int j = -1; j < 2; j++)
				{
					for (int k = -1; k < 2; k++)
					{
						ushort value = GetValue(num3 + j, num4 + k);
						num8 = Math.Min(value, num8);
						num9 = Math.Max(value, num9);
					}
				}
				if (num6 >= num8 && num5 <= num9)
				{
					if (vector3.Z < 0f)
					{
						startOffset = Math.Max((0f - (z2 - (float)num9)) / z, 0f);
					}
					else
					{
						startOffset = Math.Max(((float)num8 - z2) / z, 0f);
					}
					endOffset = 1f;
					return startOffset < endOffset;
				}
				vector4 += vector3;
			}
			startOffset = 0f;
			endOffset = 1f;
			return false;
		}

		public unsafe void Dispose()
		{
			if (!IsPersistent)
			{
				if (m_dataBuffer != null)
				{
					BufferAllocator.Dispose(m_dataBuffer);
				}
				m_dataBuffer = null;
				Data = null;
			}
		}
	}
}
