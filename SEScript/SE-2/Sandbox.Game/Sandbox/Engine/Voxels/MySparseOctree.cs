using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
	internal class MySparseOctree : IDisposable
	{
		private struct StackData<T> where T : struct, IEnumerator<byte>
		{
			public T Data;

			public MyCellCoord Cell;

			public MyOctreeNode DefaultNode;
		}

		private readonly NativeDictionary<uint, MyOctreeNode> m_nodes = new NativeDictionary<uint, MyOctreeNode>();

		private MyOctreeNode.FilterFunction m_nodeFilter;

		private int m_treeHeight;

		private int m_treeWidth;

		private byte m_defaultContent;

		public int TreeWidth => m_treeWidth;

		public unsafe bool IsAllSame
		{
			get
			{
				MyOctreeNode myOctreeNode = m_nodes[ComputeRootKey()];
				if (!myOctreeNode.HasChildren)
				{
					return MyOctreeNode.AllDataSame(myOctreeNode.Data);
				}
				return false;
			}
		}

		public int SerializedSize => 5 + m_nodes.Count * 13;

		public unsafe MySparseOctree(int height, MyOctreeNode.FilterFunction nodeFilter, byte defaultContent = 0)
		{
			m_treeHeight = height;
			m_treeWidth = 1 << height;
			m_defaultContent = defaultContent;
			m_nodeFilter = nodeFilter;
		}

		public void Build<TDataEnum>(TDataEnum data) where TDataEnum : struct, IEnumerator<byte>
		{
			m_nodes.Clear();
			StackData<TDataEnum> stack = default(StackData<TDataEnum>);
			stack.Data = data;
			stack.Cell = new MyCellCoord(m_treeHeight - 1, ref Vector3I.Zero);
			stack.DefaultNode = new MyOctreeNode(m_defaultContent);
			BuildNode(ref stack, out MyOctreeNode builtNode);
			m_nodes[ComputeRootKey()] = builtNode;
		}

		private unsafe void BuildNode<TDataEnum>(ref StackData<TDataEnum> stack, out MyOctreeNode builtNode) where TDataEnum : struct, IEnumerator<byte>
		{
			MyOctreeNode defaultNode = stack.DefaultNode;
			if (stack.Cell.Lod == 0)
			{
				for (int i = 0; i < 8; i++)
				{
					stack.Data.MoveNext();
					defaultNode.Data[i] = stack.Data.Current;
				}
			}
			else
			{
				stack.Cell.Lod--;
				Vector3I coordInLod = stack.Cell.CoordInLod;
				Vector3I a = coordInLod << 1;
				for (int j = 0; j < 8; j++)
				{
					ComputeChildCoord(j, out Vector3I relativeCoord);
					stack.Cell.CoordInLod = a + relativeCoord;
					BuildNode(ref stack, out MyOctreeNode builtNode2);
					if (!builtNode2.HasChildren && MyOctreeNode.AllDataSame(builtNode2.Data))
					{
						defaultNode.SetChild(j, childPresent: false);
						defaultNode.Data[j] = *builtNode2.Data;
					}
					else
					{
						defaultNode.SetChild(j, childPresent: true);
						defaultNode.Data[j] = m_nodeFilter(builtNode2.Data, stack.Cell.Lod);
						m_nodes.Add(stack.Cell.PackId32(), builtNode2);
					}
				}
				stack.Cell.Lod++;
				stack.Cell.CoordInLod = coordInLod;
			}
			builtNode = defaultNode;
		}

		public unsafe void Build(byte singleValue)
		{
			m_nodes.Clear();
			MyOctreeNode value = default(MyOctreeNode);
			value.ChildMask = 0;
			for (int i = 0; i < 8; i++)
			{
				value.Data[i] = singleValue;
			}
			m_nodes[ComputeRootKey()] = value;
		}

		internal unsafe byte GetFilteredValue()
		{
			MyOctreeNode myOctreeNode = m_nodes[ComputeRootKey()];
			return m_nodeFilter(myOctreeNode.Data, m_treeHeight);
		}

		internal void ReadRange(MyStorageData target, MyStorageDataTypeEnum type, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod)
		{
			MyStorageReadOperator target2 = new MyStorageReadOperator(target);
			ReadRange(ref target2, type, ref writeOffset, lodIndex, ref minInLod, ref maxInLod);
		}

		internal unsafe void ReadRange<TOperator>(ref TOperator target, MyStorageDataTypeEnum type, ref Vector3I writeOffset, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod) where TOperator : struct, IVoxelOperator
		{
			try
			{
				int num = 0;
				int num2 = EstimateStackSize(m_treeHeight);
				MyCellCoord* ptr = stackalloc MyCellCoord[num2];
				MyCellCoord myCellCoord = new MyCellCoord(m_treeHeight - 1, ref Vector3I.Zero);
				ptr[num++] = myCellCoord;
				while (num > 0)
				{
					myCellCoord = ptr[--num];
					MyOctreeNode myOctreeNode = m_nodes[myCellCoord.PackId32()];
					int num3 = myCellCoord.Lod - lodIndex;
					Vector3I min = minInLod >> num3;
					Vector3I max = maxInLod >> num3;
					Vector3I vector3I = myCellCoord.CoordInLod << 1;
					min -= vector3I;
					max -= vector3I;
					for (int i = 0; i < 8; i++)
					{
						ComputeChildCoord(i, out Vector3I relativeCoord);
						if (relativeCoord.IsInsideInclusiveEnd(ref min, ref max))
						{
							if (lodIndex < myCellCoord.Lod && myOctreeNode.HasChild(i))
							{
								ptr[num++] = new MyCellCoord(myCellCoord.Lod - 1, vector3I + relativeCoord);
							}
							else
							{
								byte inOutContent = myOctreeNode.Data[i];
								Vector3I value = vector3I + relativeCoord;
								if (num3 == 0)
								{
									Vector3I position = writeOffset + value - minInLod;
									target.Op(ref position, type, ref inOutContent);
								}
								else
								{
									value <<= num3;
									Vector3I value2 = value + (1 << num3) - 1;
									Vector3I.Max(ref value, ref minInLod, out value);
									Vector3I.Min(ref value2, ref maxInLod, out value2);
									for (int j = value.Z; j <= value2.Z; j++)
									{
										for (int k = value.Y; k <= value2.Y; k++)
										{
											for (int l = value.X; l <= value2.X; l++)
											{
												Vector3I position2 = writeOffset;
												position2.X += l - minInLod.X;
												position2.Y += k - minInLod.Y;
												position2.Z += j - minInLod.Z;
												target.Op(ref position2, type, ref inOutContent);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			finally
			{
			}
		}

		internal void ExecuteOperation<TOperator>(ref TOperator source, MyStorageDataTypeEnum type, ref Vector3I readOffset, ref Vector3I min, ref Vector3I max) where TOperator : struct, IVoxelOperator
		{
			if (source.Flags == VoxelOperatorFlags.Read)
			{
				ReadRange(ref source, type, ref readOffset, 0, ref min, ref max);
			}
			else
			{
				WriteRange(new MyCellCoord(m_treeHeight - 1, Vector3I.Zero), m_defaultContent, ref source, type, ref readOffset, ref min, ref max);
			}
		}

		private unsafe void WriteRange<TOperator>(MyCellCoord cell, byte defaultData, ref TOperator source, MyStorageDataTypeEnum type, ref Vector3I readOffset, ref Vector3I min, ref Vector3I max) where TOperator : struct, IVoxelOperator
		{
			uint key = cell.PackId32();
			if (!m_nodes.TryGetValue(key, out MyOctreeNode value))
			{
				for (int i = 0; i < 8; i++)
				{
					value.Data[i] = defaultData;
				}
			}
			if (cell.Lod == 0)
			{
				Vector3I a = cell.CoordInLod << 1;
				for (int j = 0; j < 8; j++)
				{
					ComputeChildCoord(j, out Vector3I relativeCoord);
					Vector3I position = a + relativeCoord;
					if (position.IsInsideInclusiveEnd(ref min, ref max))
					{
						position -= min;
						position += readOffset;
						source.Op(ref position, type, ref value.Data[j]);
					}
				}
				m_nodes[key] = value;
				return;
			}
			Vector3I vector3I = cell.CoordInLod << 1;
			Vector3I min2 = (min >> cell.Lod) - vector3I;
			Vector3I max2 = (max >> cell.Lod) - vector3I;
			for (int k = 0; k < 8; k++)
			{
				ComputeChildCoord(k, out Vector3I relativeCoord2);
				if (relativeCoord2.IsInsideInclusiveEnd(ref min2, ref max2))
				{
					MyCellCoord cell2 = new MyCellCoord(cell.Lod - 1, vector3I + relativeCoord2);
					WriteRange(cell2, value.Data[k], ref source, type, ref readOffset, ref min, ref max);
					uint key2 = cell2.PackId32();
					MyOctreeNode myOctreeNode = m_nodes[key2];
					if (!myOctreeNode.HasChildren && MyOctreeNode.AllDataSame(myOctreeNode.Data))
					{
						value.SetChild(k, childPresent: false);
						value.Data[k] = *myOctreeNode.Data;
						m_nodes.Remove(key2);
					}
					else
					{
						value.SetChild(k, childPresent: true);
						value.Data[k] = m_nodeFilter(myOctreeNode.Data, cell.Lod);
					}
				}
			}
			m_nodes[key] = value;
		}

		[Conditional("DEBUG")]
		private void CheckData<T>(T data) where T : struct, IEnumerator<byte>
		{
		}

		[Conditional("DEBUG")]
		private void CheckData<T>(ref T data, MyCellCoord cell) where T : struct, IEnumerator<byte>
		{
			uint key = cell.PackId32();
			MyOctreeNode myOctreeNode = m_nodes[key];
			for (int i = 0; i < 8; i++)
			{
				if (myOctreeNode.HasChild(i))
				{
					ComputeChildCoord(i, out Vector3I _);
					continue;
				}
				int num = 1 << 3 * cell.Lod;
				for (int j = 0; j < num; j++)
				{
				}
			}
		}

		private uint ComputeRootKey()
		{
			return new MyCellCoord(m_treeHeight - 1, ref Vector3I.Zero).PackId32();
		}

		private void ComputeChildCoord(int childIdx, out Vector3I relativeCoord)
		{
			relativeCoord.X = (childIdx & 1);
			relativeCoord.Y = ((childIdx >> 1) & 1);
			relativeCoord.Z = ((childIdx >> 2) & 1);
		}

		internal void DebugDraw(IMyDebugDrawBatchAabb batch, Vector3 worldPos, MyVoxelDebugDrawMode mode)
		{
			switch (mode)
			{
			case MyVoxelDebugDrawMode.Content_MicroNodes:
				foreach (KeyValuePair<uint, MyOctreeNode> node in m_nodes)
				{
					MyCellCoord myCellCoord2 = default(MyCellCoord);
					myCellCoord2.SetUnpack(node.Key);
					MyOctreeNode value3 = node.Value;
					BoundingBoxD aabb2 = default(BoundingBoxD);
					for (int j = 0; j < 8; j++)
					{
						if (!value3.HasChild(j) || myCellCoord2.Lod == 0)
						{
							ComputeChildCoord(j, out Vector3I relativeCoord2);
							Vector3I a2 = (myCellCoord2.CoordInLod << myCellCoord2.Lod + 1) + (relativeCoord2 << myCellCoord2.Lod);
							aabb2.Min = worldPos + a2 * 1f;
							aabb2.Max = aabb2.Min + 1f * (float)(1 << myCellCoord2.Lod);
							if (value3.GetData(j) != 0)
							{
								batch.Add(ref aabb2);
							}
						}
					}
				}
				break;
			case MyVoxelDebugDrawMode.Content_MicroNodesScaled:
				foreach (KeyValuePair<uint, MyOctreeNode> node2 in m_nodes)
				{
					MyCellCoord myCellCoord = default(MyCellCoord);
					myCellCoord.SetUnpack(node2.Key);
					MyOctreeNode value = node2.Value;
					BoundingBoxD aabb = default(BoundingBoxD);
					for (int i = 0; i < 8; i++)
					{
						if (!value.HasChild(i))
						{
							ComputeChildCoord(i, out Vector3I relativeCoord);
							float num = (float)(int)value.GetData(i) / 255f;
							if (num != 0f)
							{
								num = (float)Math.Pow((double)num * 1.0, 0.3333);
								Vector3I a = (myCellCoord.CoordInLod << myCellCoord.Lod + 1) + (relativeCoord << myCellCoord.Lod);
								float num2 = 1f * (float)(1 << myCellCoord.Lod);
								Vector3 value2 = worldPos + a * 1f + 0.5f * num2;
								aabb.Min = value2 - 0.5f * num * num2;
								aabb.Max = value2 + 0.5f * num * num2;
								batch.Add(ref aabb);
							}
						}
					}
				}
				break;
			}
		}

		internal unsafe void WriteTo(Stream stream)
		{
			stream.WriteNoAlloc(m_treeHeight);
			stream.WriteNoAlloc(m_defaultContent);
			foreach (KeyValuePair<uint, MyOctreeNode> node in m_nodes)
			{
				stream.WriteNoAlloc(node.Key);
				MyOctreeNode value = node.Value;
				stream.WriteNoAlloc(value.ChildMask);
				stream.WriteNoAlloc(value.Data, 0, 8);
			}
		}

		internal unsafe void ReadFrom(MyOctreeStorage.ChunkHeader header, Stream stream)
		{
			m_treeHeight = stream.ReadInt32();
			m_treeWidth = 1 << m_treeHeight;
			m_defaultContent = stream.ReadByteNoAlloc();
			header.Size -= 5;
			int num = header.Size / 13;
			m_nodes.Clear();
			MyOctreeNode value = default(MyOctreeNode);
			for (int i = 0; i < num; i++)
			{
				uint key = stream.ReadUInt32();
				value.ChildMask = stream.ReadByteNoAlloc();
				stream.ReadNoAlloc(value.Data, 0, 8);
				m_nodes.Add(key, value);
			}
		}

		internal static int EstimateStackSize(int treeHeight)
		{
			return (treeHeight - 1) * 7 + 8;
		}

		public void ReplaceValues(Dictionary<byte, byte> oldToNewValueMap)
		{
			ReplaceValues(m_nodes, oldToNewValueMap);
		}

		public unsafe static void ReplaceValues<TKey>(IDictionary<TKey, MyOctreeNode> nodeCollection, Dictionary<byte, byte> oldToNewValueMap) where TKey : unmanaged
		{
			KeyValuePair<TKey, MyOctreeNode>[] array = nodeCollection.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<TKey, MyOctreeNode> keyValuePair = array[i];
				MyOctreeNode value = keyValuePair.Value;
				for (int j = 0; j < 8; j++)
				{
					if (oldToNewValueMap.TryGetValue(value.Data[j], out byte value2))
					{
						value.Data[j] = value2;
					}
				}
				nodeCollection[keyValuePair.Key] = value;
			}
		}

		internal unsafe ContainmentType Intersect(ref BoundingBoxI box, int lod, bool exhaustiveContainmentCheck = true)
		{
			int num = 0;
			int num2 = EstimateStackSize(m_treeHeight);
			MyCellCoord* ptr = stackalloc MyCellCoord[num2];
			MyCellCoord myCellCoord = new MyCellCoord(m_treeHeight - 1, ref Vector3I.Zero);
			ptr[num++] = myCellCoord;
			Vector3I value = box.Min;
			Vector3I value2 = box.Max;
			ContainmentType result = ContainmentType.Disjoint;
			BoundingBoxI box2 = default(BoundingBoxI);
			while (num > 0)
			{
				myCellCoord = ptr[--num];
				MyOctreeNode myOctreeNode = m_nodes[myCellCoord.PackId32()];
				int lod2 = myCellCoord.Lod;
				Vector3I min = value >> lod2;
				Vector3I max = value2 >> lod2;
				Vector3I vector3I = myCellCoord.CoordInLod << 1;
				min -= vector3I;
				max -= vector3I;
				for (int i = 0; i < 8; i++)
				{
					ComputeChildCoord(i, out Vector3I relativeCoord);
					if (!relativeCoord.IsInsideInclusiveEnd(ref min, ref max))
					{
						continue;
					}
					if (myCellCoord.Lod > 0 && myOctreeNode.HasChild(i))
					{
						ptr[num++] = new MyCellCoord(myCellCoord.Lod - 1, vector3I + relativeCoord);
						continue;
					}
					byte b = myOctreeNode.Data[i];
					if (lod2 == 0)
					{
						if (b != 0)
						{
							return ContainmentType.Intersects;
						}
						continue;
					}
					box2.Min = vector3I + relativeCoord;
					box2.Min <<= lod2;
					box2.Max = box2.Min + (1 << lod2) - 1;
					Vector3I.Max(ref box2.Min, ref value, out box2.Min);
					Vector3I.Min(ref box2.Max, ref value2, out box2.Max);
					box2.Intersects(ref box2, out bool result2);
					if (result2)
					{
						return ContainmentType.Intersects;
					}
				}
			}
			return result;
		}

		internal bool Intersect(ref LineD line, out double startOffset, out double endOffset)
		{
			startOffset = 0.0;
			endOffset = 1.0;
			return true;
		}

		public void Dispose()
		{
			m_nodes.Dispose();
		}
	}
}
