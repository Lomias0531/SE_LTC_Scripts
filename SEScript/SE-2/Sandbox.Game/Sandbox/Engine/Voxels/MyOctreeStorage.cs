using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Utils;
using VRage.Game.Voxels;
using VRage.ModAPI;
using VRage.Plugins;
using VRage.Voxels;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
	public class MyOctreeStorage : MyStorageBase, IMyCompositeShape
	{
		public enum ChunkTypeEnum : ushort
		{
			StorageMetaData = 1,
			MaterialIndexTable = 2,
			MacroContentNodes = 3,
			MacroMaterialNodes = 4,
			ContentLeafProvider = 5,
			ContentLeafOctree = 6,
			MaterialLeafProvider = 7,
			MaterialLeafOctree = 8,
			DataProvider = 9,
			EndOfFile = ushort.MaxValue
		}

		public struct ChunkHeader
		{
			public ChunkTypeEnum ChunkType;

			public int Version;

			public int Size;

			public void WriteTo(Stream stream)
			{
				stream.Write7BitEncodedInt((int)ChunkType);
				stream.Write7BitEncodedInt(Version);
				stream.Write7BitEncodedInt(Size);
			}

			public void ReadFrom(Stream stream)
			{
				ChunkType = (ChunkTypeEnum)stream.Read7BitEncodedInt();
				Version = stream.Read7BitEncodedInt();
				Size = stream.Read7BitEncodedInt();
			}
		}

		internal struct WriteRangeOps<TOperator> : ITraverseOps where TOperator : struct, IVoxelOperator
		{
			public TOperator Source;

			public ChildType Init<TThis>(ref TraverseArgs<TThis> args, ref MyCellCoord coord, byte defaultData, out MyOctreeNode node) where TThis : struct, ITraverseOps
			{
				args.Storage.AccessRange(MyAccessType.Write, args.DataType, ref coord);
				MyCellCoord myCellCoord = coord;
				myCellCoord.Lod -= 4;
				ulong key = myCellCoord.PackId64();
				if (args.Leaves.TryGetValue(key, out IMyOctreeLeafNode value) && args.Storage.DataProvider != null)
				{
					value.Dispose();
					args.Leaves.Remove(key);
					Vector3I a = myCellCoord.CoordInLod << 1;
					MyCellCoord myCellCoord2 = default(MyCellCoord);
					myCellCoord2.Lod = myCellCoord.Lod - 1;
					MyCellCoord myCellCoord3 = myCellCoord2;
					node = default(MyOctreeNode);
					for (int i = 0; i < 8; i++)
					{
						ComputeChildCoord(i, out Vector3I relativeCoord);
						myCellCoord3.CoordInLod = a + relativeCoord;
						MyCellCoord cell = myCellCoord3;
						cell.Lod += 4;
						IMyOctreeLeafNode myOctreeLeafNode = new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref cell);
						args.Leaves.Add(myCellCoord3.PackId64(), myOctreeLeafNode);
						node.SetData(i, myOctreeLeafNode.GetFilteredValue());
					}
					node.SetChildren();
				}
				else
				{
					myCellCoord.Lod--;
					ulong key2 = myCellCoord.PackId64();
					if (!args.Nodes.TryGetValue(key2, out node))
					{
						node = default(MyOctreeNode);
						node.SetAllData(defaultData);
					}
				}
				return ChildType.Node;
			}

			public ChildType LeafOp<TThis>(ref TraverseArgs<TThis> args, ref MyCellCoord coord, byte defaultData, ref MyOctreeNode node) where TThis : struct, ITraverseOps
			{
				int num = 0;
				MyCellCoord myCellCoord = default(MyCellCoord);
				Vector3I a = coord.CoordInLod << 1;
				Vector3I min = args.Min >> 4;
				Vector3I max = args.Max >> 4;
				for (int i = 0; i < 8; i++)
				{
					ComputeChildCoord(i, out Vector3I relativeCoord);
					myCellCoord.CoordInLod = a + relativeCoord;
					ulong key = myCellCoord.PackId64();
					if (args.Leaves.TryGetValue(key, out IMyOctreeLeafNode value) && value.ReadOnly)
					{
						num++;
					}
					if (!myCellCoord.CoordInLod.IsInsideInclusiveEnd(ref min, ref max))
					{
						continue;
					}
					Vector3I value2 = myCellCoord.CoordInLod << 4;
					Vector3I value3 = value2 + 16 - 1;
					Vector3I.Max(ref value2, ref args.Min, out value2);
					Vector3I.Min(ref value3, ref args.Max, out value3);
					Vector3I readOffset = value2 - args.Min;
					Vector3I start = value2 - (myCellCoord.CoordInLod << 4);
					Vector3I end = value3 - (myCellCoord.CoordInLod << 4);
					byte data = node.GetData(i);
					if (value == null)
					{
						MyMicroOctreeLeaf myMicroOctreeLeaf = new MyMicroOctreeLeaf(args.DataType, 4, myCellCoord.CoordInLod << myCellCoord.Lod + 4);
						myMicroOctreeLeaf.BuildFrom(data);
						value = myMicroOctreeLeaf;
					}
					if (value.ReadOnly)
					{
						num--;
						MyStorageData tempStorage = TempStorage;
						Vector3I maxInLod = new Vector3I(15);
						tempStorage.Resize(Vector3I.Zero, maxInLod);
						tempStorage.Clear(args.DataType, defaultData);
						if (start != Vector3I.Zero || end != maxInLod || Source.Flags != VoxelOperatorFlags.WriteAll)
						{
							MyVoxelRequestFlags flags = MyVoxelRequestFlags.EmptyData;
							if (args.DataType == MyStorageDataTypeEnum.Content)
							{
								value.ReadRange(tempStorage, args.DataType.ToFlags(), ref Vector3I.Zero, 0, ref Vector3I.Zero, ref maxInLod, ref flags);
							}
							else
							{
								tempStorage.Clear(MyStorageDataTypeEnum.Content, 0);
								if (args.ContentLeaves.TryGetValue(key, out IMyOctreeLeafNode value4))
								{
									value4.ReadRange(tempStorage, MyStorageDataTypeFlags.Content, ref Vector3I.Zero, 0, ref Vector3I.Zero, ref maxInLod, ref flags);
								}
								else
								{
									Vector3I lodVoxelRangeMin = myCellCoord.CoordInLod << 4;
									Vector3I lodVoxelRangeMax = lodVoxelRangeMin + 16 - 1;
									args.Storage.ReadRangeInternal(tempStorage, ref Vector3I.Zero, MyStorageDataTypeFlags.Content, 0, ref lodVoxelRangeMin, ref lodVoxelRangeMax, ref flags);
								}
								flags = MyVoxelRequestFlags.ConsiderContent;
								value.ReadRange(tempStorage, args.DataType.ToFlags(), ref Vector3I.Zero, 0, ref Vector3I.Zero, ref maxInLod, ref flags);
							}
						}
						byte[] array = tempStorage[args.DataType];
						Vector3I p = start;
						Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
						while (vector3I_RangeIterator.IsValid())
						{
							Vector3I position = readOffset + (p - start);
							int num2 = tempStorage.ComputeLinear(ref p);
							Source.Op(ref position, args.DataType, ref array[num2]);
							vector3I_RangeIterator.GetNext(out p);
						}
						MyMicroOctreeLeaf myMicroOctreeLeaf2 = new MyMicroOctreeLeaf(args.DataType, 4, myCellCoord.CoordInLod << myCellCoord.Lod + 4);
						myMicroOctreeLeaf2.BuildFrom(tempStorage);
						value = myMicroOctreeLeaf2;
					}
					else
					{
						value.ExecuteOperation(ref Source, ref readOffset, ref start, ref end);
					}
					byte uniformValue;
					bool num3 = value.TryGetUniformValue(out uniformValue);
					node.SetData(i, value.GetFilteredValue());
					if (!num3)
					{
						args.Leaves[key] = value;
						node.SetChild(i, childPresent: true);
					}
					else
					{
						value?.Dispose();
						args.Leaves.Remove(key);
						node.SetChild(i, childPresent: false);
					}
				}
				if (num == 8)
				{
					return ChildType.NodeWithLeafReadonly;
				}
				return ChildType.Node;
			}
		}

		private struct DeleteRangeOps : ITraverseOps
		{
			public MyOctreeStorage Storage;

			public ChildType Init<TThis>(ref TraverseArgs<TThis> args, ref MyCellCoord coord, byte defaultData, out MyOctreeNode node) where TThis : struct, ITraverseOps
			{
				node = default(MyOctreeNode);
				MyCellCoord myCellCoord = coord;
				myCellCoord.Lod -= 4;
				ulong key = myCellCoord.PackId64();
				if (args.Leaves.TryGetValue(key, out IMyOctreeLeafNode value) && myCellCoord.Lod > 0)
				{
					if (!value.ReadOnly)
					{
						return ChildType.Leaf;
					}
					return ChildType.LeafReadonly;
				}
				myCellCoord.Lod--;
				ulong key2 = myCellCoord.PackId64();
				if (!args.Nodes.TryGetValue(key2, out node))
				{
					return ChildType.NodeMissing;
				}
				return ChildType.Node;
			}

			public ChildType LeafOp<TThis>(ref TraverseArgs<TThis> args, ref MyCellCoord coord, byte defaultData, ref MyOctreeNode node) where TThis : struct, ITraverseOps
			{
				MyCellCoord myCellCoord = default(MyCellCoord);
				Vector3I a = coord.CoordInLod << 1;
				Vector3I inclusiveMin = args.Min >> 4;
				Vector3I exclusiveMax = args.Max >> 4;
				int num = 0;
				for (int i = 0; i < 8; i++)
				{
					ComputeChildCoord(i, out Vector3I relativeCoord);
					myCellCoord.CoordInLod = a + relativeCoord;
					ulong key = myCellCoord.PackId64();
					if (!myCellCoord.CoordInLod.IsInside(ref inclusiveMin, ref exclusiveMax))
					{
						continue;
					}
					if (node.HasChild(i))
					{
						if (!args.Leaves.TryGetValue(key, out IMyOctreeLeafNode value))
						{
							num++;
							continue;
						}
						if (value.ReadOnly)
						{
							num++;
							continue;
						}
						value.Dispose();
					}
					MyCellCoord cell = myCellCoord;
					cell.Lod += 4;
					IMyOctreeLeafNode myOctreeLeafNode = new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref cell);
					args.Leaves[key] = myOctreeLeafNode;
					node.SetData(i, myOctreeLeafNode.GetFilteredValue());
					node.SetChild(i, childPresent: true);
					num++;
				}
				if (num == 8)
				{
					return ChildType.NodeWithLeafReadonly;
				}
				return ChildType.Node;
			}
		}

		private struct SweepRangeOps : ITraverseOps
		{
			public MyOctreeStorage Storage;

			public ChildType Init<TThis>(ref TraverseArgs<TThis> args, ref MyCellCoord coord, byte defaultData, out MyOctreeNode node) where TThis : struct, ITraverseOps
			{
				node = default(MyOctreeNode);
				MyCellCoord myCellCoord = coord;
				myCellCoord.Lod -= 4;
				ulong key = myCellCoord.PackId64();
				if (args.Leaves.TryGetValue(key, out IMyOctreeLeafNode _) && args.Storage.DataProvider != null)
				{
					return ChildType.LeafReadonly;
				}
				myCellCoord.Lod--;
				ulong key2 = myCellCoord.PackId64();
				if (!args.Nodes.TryGetValue(key2, out node))
				{
					MyCellCoord cell = myCellCoord;
					cell.Lod++;
					MyProviderLeaf myProviderLeaf = new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref cell);
					if (myProviderLeaf.TryGetUniformValue(out byte uniformValue) && defaultData == uniformValue)
					{
						args.Leaves[key] = myProviderLeaf;
						return ChildType.LeafReadonly;
					}
					return ChildType.NodeMissing;
				}
				return ChildType.Node;
			}

			public ChildType LeafOp<TThis>(ref TraverseArgs<TThis> args, ref MyCellCoord coord, byte defaultData, ref MyOctreeNode node) where TThis : struct, ITraverseOps
			{
				MyCellCoord myCellCoord = default(MyCellCoord);
				Vector3I a = coord.CoordInLod << 1;
				int num = 0;
				for (int i = 0; i < 8; i++)
				{
					ComputeChildCoord(i, out Vector3I relativeCoord);
					myCellCoord.CoordInLod = a + relativeCoord;
					ulong key = myCellCoord.PackId64();
					IMyOctreeLeafNode value;
					if (!node.HasChild(i))
					{
						MyCellCoord cell = myCellCoord;
						cell.Lod += 4;
						MyProviderLeaf myProviderLeaf = new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref cell);
						if (myProviderLeaf.TryGetUniformValue(out byte uniformValue) && node.GetData(i) == uniformValue)
						{
							args.Leaves[key] = myProviderLeaf;
							num++;
						}
					}
					else if (args.Leaves.TryGetValue(key, out value) && value.ReadOnly)
					{
						num++;
					}
				}
				if (num == 8)
				{
					return ChildType.NodeWithLeafReadonly;
				}
				return ChildType.Node;
			}
		}

		internal enum ChildType
		{
			NodeMissing,
			NodeEmpty,
			Node,
			LeafReadonly,
			NodeWithLeafReadonly,
			Leaf
		}

		internal interface ITraverseOps
		{
			ChildType Init<THis>(ref TraverseArgs<THis> args, ref MyCellCoord coord, byte defaultData, out MyOctreeNode node) where THis : struct, ITraverseOps;

			ChildType LeafOp<TThis>(ref TraverseArgs<TThis> args, ref MyCellCoord coord, byte defaultData, ref MyOctreeNode node) where TThis : struct, ITraverseOps;
		}

		internal struct TraverseArgs<TOperator> where TOperator : struct, ITraverseOps
		{
			public TOperator Operator;

			public MyOctreeStorage Storage;

			public Dictionary<ulong, MyOctreeNode> Nodes;

			public Dictionary<ulong, IMyOctreeLeafNode> Leaves;

			public Dictionary<ulong, IMyOctreeLeafNode> ContentLeaves;

			public MyOctreeNode.FilterFunction DataFilter;

			public MyStorageDataTypeEnum DataType;

			public Vector3I Min;

			public Vector3I Max;
		}

		private const int CURRENT_FILE_VERSION = 1;

		public const int LeafLodCount = 4;

		public const int LeafSizeInVoxels = 16;

		private readonly Dictionary<byte, byte> m_oldToNewIndexMap = new Dictionary<byte, byte>();

		[ThreadStatic]
		private static MyStorageData m_temporaryCache;

		private int m_treeHeight;

		private readonly Dictionary<ulong, MyOctreeNode> m_contentNodes = new Dictionary<ulong, MyOctreeNode>();

		private readonly Dictionary<ulong, IMyOctreeLeafNode> m_contentLeaves = new Dictionary<ulong, IMyOctreeLeafNode>();

		private readonly Dictionary<ulong, MyOctreeNode> m_materialNodes = new Dictionary<ulong, MyOctreeNode>();

		private readonly Dictionary<ulong, IMyOctreeLeafNode> m_materialLeaves = new Dictionary<ulong, IMyOctreeLeafNode>();

		private IMyStorageDataProvider m_dataProvider;

		[ThreadStatic]
		private static MyStorageData m_storageCached;

		private static readonly Dictionary<int, MyStorageDataProviderAttribute> m_attributesById;

		private static readonly Dictionary<Type, MyStorageDataProviderAttribute> m_attributesByType;

		private const int VERSION_OCTREE_NODES_32BIT_KEY = 1;

		private const int CURRENT_VERSION_OCTREE_NODES = 2;

		private const int VERSION_OCTREE_LEAVES_32BIT_KEY = 2;

		private const int CURRENT_VERSION_OCTREE_LEAVES = 3;

		private static MyStorageData TempStorage => m_temporaryCache ?? (m_temporaryCache = new MyStorageData());

		public override IMyStorageDataProvider DataProvider
		{
			get
			{
				return m_dataProvider;
			}
			set
			{
				m_dataProvider = value;
				foreach (IMyOctreeLeafNode value2 in m_contentLeaves.Values)
				{
					value2.OnDataProviderChanged(value);
				}
				foreach (IMyOctreeLeafNode value3 in m_materialLeaves.Values)
				{
					value3.OnDataProviderChanged(value);
				}
				OnRangeChanged(Vector3I.Zero, base.Size - 1, MyStorageDataTypeFlags.ContentAndMaterial);
			}
		}

		public MyOctreeStorage()
		{
		}

		public MyOctreeStorage(IMyStorageDataProvider dataProvider, Vector3I size)
		{
			int v = MathHelper.Max(size.X, size.Y, size.Z);
			base.Size = new Vector3I(MathHelper.GetNearestBiggerPowerOfTwo(v));
			m_dataProvider = dataProvider;
			InitTreeHeight();
			ResetInternal(MyStorageDataTypeFlags.ContentAndMaterial);
			base.Geometry.Init(this);
		}

		private void InitTreeHeight()
		{
			Vector3I vector3I = base.Size >> 4;
			m_treeHeight = -1;
			Vector3I a = vector3I;
			while (a != Vector3I.Zero)
			{
				a >>= 1;
				m_treeHeight++;
			}
			if (m_treeHeight < 0)
			{
				m_treeHeight = 1;
			}
		}

		protected override void ResetInternal(MyStorageDataTypeFlags dataToReset)
		{
			AccessReset();
			bool flag = (dataToReset & MyStorageDataTypeFlags.Content) != 0;
			bool flag2 = (dataToReset & MyStorageDataTypeFlags.Material) != 0;
			if (flag)
			{
				m_contentLeaves.Clear();
				m_contentNodes.Clear();
			}
			if (flag2)
			{
				m_materialLeaves.Clear();
				m_materialNodes.Clear();
			}
			if (m_dataProvider != null)
			{
				MyCellCoord cell = new MyCellCoord(m_treeHeight, ref Vector3I.Zero);
				ulong key = cell.PackId64();
				cell.Lod += 4;
				_ = base.Size - 1;
				if (flag)
				{
					m_contentLeaves.Add(key, new MyProviderLeaf(m_dataProvider, MyStorageDataTypeEnum.Content, ref cell));
				}
				if (flag2)
				{
					m_materialLeaves.Add(key, new MyProviderLeaf(m_dataProvider, MyStorageDataTypeEnum.Material, ref cell));
				}
				return;
			}
			MyCellCoord coord = new MyCellCoord(m_treeHeight - 1, ref Vector3I.Zero);
			ulong key2 = coord.PackId64();
			coord.Lod += 5;
			if (flag)
			{
				m_contentNodes.Add(key2, default(MyOctreeNode));
				AccessRange(MyAccessType.Write, MyStorageDataTypeEnum.Content, ref coord);
			}
			if (flag2)
			{
				m_materialNodes.Add(key2, new MyOctreeNode(byte.MaxValue));
				AccessRange(MyAccessType.Write, MyStorageDataTypeEnum.Material, ref coord);
			}
		}

		protected override void OverwriteAllMaterialsInternal(MyVoxelMaterialDefinition material)
		{
		}

		protected override void LoadInternal(int fileVersion, Stream stream, ref bool isOldFormat)
		{
			if (fileVersion == 2)
			{
				ReadStorageAccess(stream);
			}
			ChunkHeader header = default(ChunkHeader);
			Dictionary<byte, MyVoxelMaterialDefinition> dictionary = null;
			HashSet<ulong> hashSet = new HashSet<ulong>();
			HashSet<ulong> hashSet2 = new HashSet<ulong>();
			while (header.ChunkType != ChunkTypeEnum.EndOfFile)
			{
				header.ReadFrom(stream);
				ulong key;
				switch (header.ChunkType)
				{
				case ChunkTypeEnum.StorageMetaData:
					ReadStorageMetaData(stream, header, ref isOldFormat);
					break;
				case ChunkTypeEnum.MaterialIndexTable:
					dictionary = ReadMaterialTable(stream, header, ref isOldFormat);
					break;
				case ChunkTypeEnum.MacroContentNodes:
					ReadOctreeNodes(stream, header, ref isOldFormat, m_contentNodes, delegate(MyCellCoord coord)
					{
						LoadAccess(stream, coord);
					});
					break;
				case ChunkTypeEnum.MacroMaterialNodes:
					ReadOctreeNodes(stream, header, ref isOldFormat, m_materialNodes, null);
					break;
				case ChunkTypeEnum.ContentLeafProvider:
					ReadProviderLeaf(stream, header, ref isOldFormat, hashSet2);
					break;
				case ChunkTypeEnum.ContentLeafOctree:
				{
					ReadOctreeLeaf(stream, header, ref isOldFormat, MyStorageDataTypeEnum.Content, out key, out MyMicroOctreeLeaf contentLeaf2);
					m_contentLeaves.Add(key, contentLeaf2);
					break;
				}
				case ChunkTypeEnum.MaterialLeafProvider:
					ReadProviderLeaf(stream, header, ref isOldFormat, hashSet);
					break;
				case ChunkTypeEnum.MaterialLeafOctree:
				{
					ReadOctreeLeaf(stream, header, ref isOldFormat, MyStorageDataTypeEnum.Material, out key, out MyMicroOctreeLeaf contentLeaf);
					m_materialLeaves.Add(key, contentLeaf);
					break;
				}
				case ChunkTypeEnum.DataProvider:
					ReadDataProvider(stream, header, ref isOldFormat, out m_dataProvider);
					break;
				default:
					throw new InvalidBranchException();
				case ChunkTypeEnum.EndOfFile:
					break;
				}
			}
			MyCellCoord cell = default(MyCellCoord);
			foreach (ulong item in hashSet2)
			{
				cell.SetUnpack(item);
				cell.Lod += 4;
				m_contentLeaves.Add(item, new MyProviderLeaf(m_dataProvider, MyStorageDataTypeEnum.Content, ref cell));
			}
			foreach (ulong item2 in hashSet)
			{
				cell.SetUnpack(item2);
				cell.Lod += 4;
				m_materialLeaves.Add(item2, new MyProviderLeaf(m_dataProvider, MyStorageDataTypeEnum.Material, ref cell));
			}
			bool flag = false;
			foreach (KeyValuePair<byte, MyVoxelMaterialDefinition> item3 in dictionary)
			{
				if (item3.Key != item3.Value.Index)
				{
					flag = true;
				}
				m_oldToNewIndexMap.Add(item3.Key, item3.Value.Index);
			}
			if (flag)
			{
				if (m_dataProvider != null)
				{
					m_dataProvider.ReindexMaterials(m_oldToNewIndexMap);
				}
				foreach (KeyValuePair<ulong, IMyOctreeLeafNode> materialLeaf in m_materialLeaves)
				{
					materialLeaf.Value.ReplaceValues(m_oldToNewIndexMap);
				}
				MySparseOctree.ReplaceValues(m_materialNodes, m_oldToNewIndexMap);
			}
			m_oldToNewIndexMap.Clear();
		}

		protected override void SaveInternal(Stream stream)
		{
			WriteStorageAccess(stream);
			WriteStorageMetaData(stream);
			WriteMaterialTable(stream);
			WriteDataProvider(stream, m_dataProvider);
			WriteOctreeNodes(stream, ChunkTypeEnum.MacroContentNodes, m_contentNodes, base.SaveAccess);
			WriteOctreeNodes(stream, ChunkTypeEnum.MacroMaterialNodes, m_materialNodes, null);
			WriteOctreeLeaves(stream, m_contentLeaves);
			WriteOctreeLeaves(stream, m_materialLeaves);
			ChunkHeader chunkHeader = default(ChunkHeader);
			chunkHeader.ChunkType = ChunkTypeEnum.EndOfFile;
			chunkHeader.WriteTo(stream);
		}

		public override VRage.Game.Voxels.IMyStorage Copy()
		{
			Save(out byte[] outCompressedData);
			MyStorageBase myStorageBase = MyStorageBase.Load(outCompressedData);
			myStorageBase.Shared = false;
			return myStorageBase;
		}

		private static void ConsiderContent(MyStorageData storage)
		{
			byte[] array = storage[MyStorageDataTypeEnum.Content];
			byte[] array2 = storage[MyStorageDataTypeEnum.Material];
			for (int i = 0; i < storage.SizeLinear; i++)
			{
				if (array[i] == 0)
				{
					array2[i] = byte.MaxValue;
				}
			}
		}

		protected override void ReadRangeInternal(MyStorageData target, ref Vector3I targetWriteOffset, MyStorageDataTypeFlags dataToRead, int lodIndex, ref Vector3I lodVoxelCoordStart, ref Vector3I lodVoxelCoordEnd, ref MyVoxelRequestFlags flags)
		{
			bool flag = lodIndex <= m_treeHeight + 4;
			MyVoxelRequestFlags flags2 = (MyVoxelRequestFlags)0;
			MyVoxelRequestFlags flags3 = (MyVoxelRequestFlags)0;
			if ((dataToRead & MyStorageDataTypeFlags.Content) != 0 && flag)
			{
				flags2 = flags;
				ReadRange(target, ref targetWriteOffset, MyStorageDataTypeFlags.Content, m_treeHeight, m_contentNodes, m_contentLeaves, lodIndex, ref lodVoxelCoordStart, ref lodVoxelCoordEnd, ref flags2);
			}
			if (dataToRead.Requests(MyStorageDataTypeEnum.Material) && !flags2.HasFlags(MyVoxelRequestFlags.EmptyData) && flag)
			{
				flags3 = flags;
				ReadRange(target, ref targetWriteOffset, dataToRead.Without(MyStorageDataTypeEnum.Content), m_treeHeight, m_materialNodes, m_materialLeaves, lodIndex, ref lodVoxelCoordStart, ref lodVoxelCoordEnd, ref flags3);
				if ((dataToRead & MyStorageDataTypeFlags.Content) != 0)
				{
					flags3 &= ~MyVoxelRequestFlags.EmptyData;
				}
				if (flags.HasFlags(MyVoxelRequestFlags.ConsiderContent))
				{
					ConsiderContent(target);
				}
			}
			flags = (flags2 | flags3);
		}

		protected unsafe override bool IsModifiedInternal(ref BoundingBoxI range)
		{
			int num = 0;
			int num2 = MySparseOctree.EstimateStackSize(m_treeHeight);
			MyCellCoord* ptr = stackalloc MyCellCoord[num2];
			MyCellCoord myCellCoord = new MyCellCoord(m_treeHeight + 4, ref Vector3I.Zero);
			ptr[num++] = myCellCoord;
			MyCellCoord myCellCoord2 = default(MyCellCoord);
			BoundingBoxI boundingBoxI = range;
			while (num > 0)
			{
				myCellCoord = ptr[--num];
				myCellCoord2.Lod = Math.Max(myCellCoord.Lod - 4, 0);
				myCellCoord2.CoordInLod = myCellCoord.CoordInLod;
				if (m_contentLeaves.TryGetValue(myCellCoord2.PackId64(), out IMyOctreeLeafNode value))
				{
					if (!(value is MyProviderLeaf))
					{
						return true;
					}
					continue;
				}
				myCellCoord2.Lod--;
				int shift = myCellCoord.Lod - 1;
				MyOctreeNode myOctreeNode = m_contentNodes[myCellCoord2.PackId64()];
				Vector3I min = boundingBoxI.Min >> shift;
				Vector3I max = boundingBoxI.Max >> shift;
				Vector3I vector3I = myCellCoord.CoordInLod << 1;
				min -= vector3I;
				max -= vector3I;
				for (int i = 0; i < 8; i++)
				{
					ComputeChildCoord(i, out Vector3I relativeCoord);
					if (relativeCoord.IsInsideInclusiveEnd(ref min, ref max))
					{
						if (myCellCoord.Lod == 0)
						{
							return true;
						}
						if (myOctreeNode.HasChild(i))
						{
							ptr[num++] = new MyCellCoord(myCellCoord.Lod - 1, vector3I + relativeCoord);
						}
					}
				}
			}
			return false;
		}

		public override void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode)
		{
			base.DebugDraw(ref worldMatrix, mode);
			Color cornflowerBlue = Color.CornflowerBlue;
			cornflowerBlue.A = 25;
			DebugDraw(ref worldMatrix, mode, cornflowerBlue);
		}

		public void DebugDraw(ref MatrixD worldMatrix, MyVoxelDebugDrawMode mode, Color color)
		{
			switch (mode)
			{
			case MyVoxelDebugDrawMode.FullCells:
				break;
			case MyVoxelDebugDrawMode.Content_MicroNodes:
			case MyVoxelDebugDrawMode.Content_MicroNodesScaled:
				DrawSparseOctrees(ref worldMatrix, color, mode, m_contentLeaves);
				break;
			case MyVoxelDebugDrawMode.Content_MacroNodes:
				DrawNodes(ref worldMatrix, color, m_contentNodes);
				break;
			case MyVoxelDebugDrawMode.Content_MacroLeaves:
				DrawLeaves(ref worldMatrix, color, m_contentLeaves);
				break;
			case MyVoxelDebugDrawMode.Content_MacroScaled:
				DrawScaledNodes(ref worldMatrix, color, m_contentNodes);
				break;
			case MyVoxelDebugDrawMode.Materials_MacroNodes:
				DrawNodes(ref worldMatrix, color, m_materialNodes);
				break;
			case MyVoxelDebugDrawMode.Materials_MacroLeaves:
				DrawLeaves(ref worldMatrix, color, m_materialLeaves);
				break;
			case MyVoxelDebugDrawMode.Content_DataProvider:
				if (m_dataProvider != null)
				{
					m_dataProvider.DebugDraw(ref worldMatrix);
				}
				break;
			}
		}

		public void Voxelize(MyStorageDataTypeFlags data)
		{
			MyVoxelRequestFlags requestFlags = MyVoxelRequestFlags.EmptyData;
			MyStorageData myStorageData = new MyStorageData();
			myStorageData.Resize(new Vector3I(16));
			Vector3I a = base.Size / 16;
			Vector3I next = Vector3I.Zero;
			Vector3I end = a - 1;
			Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref Vector3I.Zero, ref end);
			while (vector3I_RangeIterator.IsValid())
			{
				Vector3I lodVoxelRangeMin = next * 16;
				Vector3I lodVoxelRangeMax = lodVoxelRangeMin + 15;
				ReadRangeInternal(myStorageData, ref Vector3I.Zero, data, 0, ref lodVoxelRangeMin, ref lodVoxelRangeMax, ref requestFlags);
				MyStorageDataWriteOperator source = new MyStorageDataWriteOperator(myStorageData);
				WriteRangeInternal(ref source, data, ref lodVoxelRangeMin, ref lodVoxelRangeMax);
				vector3I_RangeIterator.GetNext(out next);
			}
			OnRangeChanged(Vector3I.Zero, base.Size - 1, data);
		}

		private unsafe void ReadRange(MyStorageData target, ref Vector3I targetWriteOffset, MyStorageDataTypeFlags types, int treeHeight, Dictionary<ulong, MyOctreeNode> nodes, Dictionary<ulong, IMyOctreeLeafNode> leaves, int lodIndex, ref Vector3I minInLod, ref Vector3I maxInLod, ref MyVoxelRequestFlags flags)
		{
			int num = 0;
			int num2 = MySparseOctree.EstimateStackSize(treeHeight);
			MyCellCoord* ptr = stackalloc MyCellCoord[num2];
			MyCellCoord myCellCoord = new MyCellCoord(treeHeight + 4, ref Vector3I.Zero);
			ptr[num++] = myCellCoord;
			MyCellCoord myCellCoord2 = default(MyCellCoord);
			MyStorageDataTypeEnum type = (!types.Requests(MyStorageDataTypeEnum.Content)) ? MyStorageDataTypeEnum.Material : MyStorageDataTypeEnum.Content;
			Vector3I v = target.Step;
			byte b = (byte)((!types.Requests(MyStorageDataTypeEnum.Content)) ? byte.MaxValue : 0);
			byte b2 = byte.MaxValue;
			flags |= MyVoxelRequestFlags.FullContent;
			FillOutOfBounds(target, type, ref targetWriteOffset, lodIndex, minInLod, maxInLod);
			while (num > 0)
			{
				myCellCoord = ptr[--num];
				myCellCoord2.Lod = Math.Max(myCellCoord.Lod - 4, 0);
				myCellCoord2.CoordInLod = myCellCoord.CoordInLod;
				int num3;
				if (leaves.TryGetValue(myCellCoord2.PackId64(), out IMyOctreeLeafNode value))
				{
					num3 = myCellCoord.Lod - lodIndex;
					Vector3I min = minInLod >> num3;
					Vector3I max = maxInLod >> num3;
					if (myCellCoord.CoordInLod.IsInsideInclusiveEnd(ref min, ref max))
					{
						Vector3I vector3I = myCellCoord.CoordInLod << num3;
						Vector3I value2 = vector3I - minInLod;
						Vector3I.Max(ref value2, ref Vector3I.Zero, out value2);
						value2 += targetWriteOffset;
						Vector3I max2 = new Vector3I((1 << num3) - 1);
						Vector3I minInLod2 = minInLod - vector3I;
						Vector3I maxInLod2 = maxInLod - vector3I;
						if (!minInLod2.IsInsideInclusiveEnd(Vector3I.Zero, max2) || !maxInLod2.IsInsideInclusiveEnd(Vector3I.Zero, max2))
						{
							minInLod2 = Vector3I.Clamp(minInLod - vector3I, Vector3I.Zero, max2);
							maxInLod2 = Vector3I.Clamp(maxInLod - vector3I, Vector3I.Zero, max2);
						}
						MyVoxelRequestFlags flags2 = flags;
						value.ReadRange(target, types, ref value2, lodIndex, ref minInLod2, ref maxInLod2, ref flags2);
						flags = ((flags & (flags2 & (MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.FullContent))) | (flags & ~(MyVoxelRequestFlags.EmptyData | MyVoxelRequestFlags.FullContent)));
					}
					continue;
				}
				myCellCoord2.Lod--;
				num3 = myCellCoord.Lod - 1 - lodIndex;
				if (!nodes.TryGetValue(myCellCoord2.PackId64(), out MyOctreeNode value3))
				{
					continue;
				}
				Vector3I min2 = minInLod >> num3;
				Vector3I max3 = maxInLod >> num3;
				Vector3I vector3I2 = myCellCoord.CoordInLod << 1;
				min2 -= vector3I2;
				max3 -= vector3I2;
				for (int i = 0; i < 8; i++)
				{
					ComputeChildCoord(i, out Vector3I relativeCoord);
					if (!relativeCoord.IsInsideInclusiveEnd(ref min2, ref max3))
					{
						continue;
					}
					if (lodIndex < myCellCoord.Lod && value3.HasChild(i))
					{
						ptr[num++] = new MyCellCoord(myCellCoord.Lod - 1, vector3I2 + relativeCoord);
						continue;
					}
					flags &= MyVoxelRequestFlags.RequestFlags;
					Vector3I value4 = vector3I2 + relativeCoord;
					value4 <<= num3;
					Vector3I value5 = value4 - minInLod;
					Vector3I.Max(ref value5, ref Vector3I.Zero, out value5);
					value5 += targetWriteOffset;
					byte data = value3.GetData(i);
					if (data != b)
					{
						flags &= ~MyVoxelRequestFlags.EmptyData;
					}
					if (data != b2)
					{
						flags &= ~MyVoxelRequestFlags.FullContent;
					}
					if (num3 == 0)
					{
						int num4 = value5.Dot(ref v);
						target[type][num4] = data;
						continue;
					}
					Vector3I value6 = value4 + ((1 << num3) - 1);
					Vector3I.Max(ref value4, ref minInLod, out value4);
					Vector3I.Min(ref value6, ref maxInLod, out value6);
					for (int j = value4.Z; j <= value6.Z; j++)
					{
						for (int k = value4.Y; k <= value6.Y; k++)
						{
							for (int l = value4.X; l <= value6.X; l++)
							{
								Vector3I vector3I3 = value5;
								vector3I3.X += l - value4.X;
								vector3I3.Y += k - value4.Y;
								vector3I3.Z += j - value4.Z;
								int num5 = vector3I3.Dot(ref v);
								target[type][num5] = data;
							}
						}
					}
				}
			}
		}

		private void FillOutOfBounds(MyStorageData target, MyStorageDataTypeEnum type, ref Vector3I woffset, int lodIndex, Vector3I minInLod, Vector3I maxInLod)
		{
			byte content = MyVoxelDataConstants.DefaultValue(type);
			Vector3I max = new Vector3I((1 << m_treeHeight + 4 - lodIndex) - 1);
			Vector3I vector3I = woffset - minInLod;
			BoundingBoxI boundingBoxI = new BoundingBoxI(minInLod, maxInLod);
			BoundingBoxI box = new BoundingBoxI(Vector3I.Zero, max);
			if (!boundingBoxI.Intersects(ref box))
			{
				target.BlockFill(type, vector3I + minInLod, vector3I + maxInLod, content);
				return;
			}
			if (minInLod.X < 0)
			{
				Vector3I a = minInLod;
				Vector3I a2 = maxInLod;
				a2.X = -1;
				minInLod.X = 0;
				target.BlockFill(type, a + vector3I, a2 + vector3I, content);
			}
			if (maxInLod.X > max.X)
			{
				Vector3I a3 = minInLod;
				Vector3I a4 = maxInLod;
				a3.X = max.X + 1;
				minInLod.X = max.X;
				target.BlockFill(type, a3 + vector3I, a4 + vector3I, content);
			}
			if (minInLod.Y < 0)
			{
				Vector3I a5 = minInLod;
				Vector3I a6 = maxInLod;
				a6.Y = -1;
				minInLod.Y = 0;
				target.BlockFill(type, a5 + vector3I, a6 + vector3I, content);
			}
			if (maxInLod.Y > max.Y)
			{
				Vector3I a7 = minInLod;
				Vector3I a8 = maxInLod;
				a7.Y = max.Y + 1;
				minInLod.Y = max.Y;
				target.BlockFill(type, a7 + vector3I, a8 + vector3I, content);
			}
			if (minInLod.Y < 0)
			{
				Vector3I a9 = minInLod;
				Vector3I a10 = maxInLod;
				a10.Y = -1;
				minInLod.Y = 0;
				target.BlockFill(type, a9 + vector3I, a10 + vector3I, content);
			}
			if (maxInLod.Y > max.Y)
			{
				Vector3I a11 = minInLod;
				Vector3I a12 = maxInLod;
				a11.Y = max.Y + 1;
				minInLod.Y = max.Y;
				target.BlockFill(type, a11 + vector3I, a12 + vector3I, content);
			}
		}

		protected override void WriteRangeInternal<TOperator>(ref TOperator source, MyStorageDataTypeFlags dataToWrite, ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax)
		{
			if ((dataToWrite & MyStorageDataTypeFlags.Content) != 0)
			{
				TraverseArgs<WriteRangeOps<TOperator>> args = ContentArgs<WriteRangeOps<TOperator>>(ref voxelRangeMin, ref voxelRangeMax);
				args.Operator.Source = source;
				Traverse(ref args, 0, m_treeHeight + 4, Vector3I.Zero);
				source = args.Operator.Source;
			}
			if ((dataToWrite & MyStorageDataTypeFlags.Material) != 0)
			{
				TraverseArgs<WriteRangeOps<TOperator>> args2 = MaterialArgs<WriteRangeOps<TOperator>>(ref voxelRangeMin, ref voxelRangeMax);
				args2.Operator.Source = source;
				Traverse(ref args2, 0, m_treeHeight + 4, Vector3I.Zero);
				source = args2.Operator.Source;
			}
		}

		protected override void DeleteRangeInternal(MyStorageDataTypeFlags dataToDelete, ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax)
		{
			if ((dataToDelete & MyStorageDataTypeFlags.Content) != 0)
			{
				TraverseArgs<DeleteRangeOps> args = ContentArgs<DeleteRangeOps>(ref voxelRangeMin, ref voxelRangeMax);
				Traverse(ref args, 0, m_treeHeight + 4, Vector3I.Zero);
			}
			if ((dataToDelete & MyStorageDataTypeFlags.Material) != 0)
			{
				TraverseArgs<DeleteRangeOps> args2 = MaterialArgs<DeleteRangeOps>(ref voxelRangeMin, ref voxelRangeMax);
				Traverse(ref args2, 0, m_treeHeight + 4, Vector3I.Zero);
			}
		}

		protected override void SweepInternal(MyStorageDataTypeFlags dataToSweep)
		{
			SweepRangeOps sweepRangeOps = default(SweepRangeOps);
			sweepRangeOps.Storage = this;
			if ((dataToSweep & MyStorageDataTypeFlags.Content) != 0)
			{
				Vector3I voxelRangeMax = base.Size;
				TraverseArgs<SweepRangeOps> args = ContentArgs<SweepRangeOps>(ref Vector3I.Zero, ref voxelRangeMax);
				Traverse(ref args, 0, m_treeHeight + 4, Vector3I.Zero);
			}
			if ((dataToSweep & MyStorageDataTypeFlags.Material) != 0)
			{
				Vector3I voxelRangeMax2 = base.Size;
				TraverseArgs<SweepRangeOps> args2 = MaterialArgs<SweepRangeOps>(ref Vector3I.Zero, ref voxelRangeMax2);
				Traverse(ref args2, 0, m_treeHeight + 4, Vector3I.Zero);
			}
		}

		private unsafe TraverseArgs<TOperator> ContentArgs<TOperator>(ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax) where TOperator : struct, ITraverseOps
		{
			TraverseArgs<TOperator> result = default(TraverseArgs<TOperator>);
			result.Storage = this;
			result.Operator = default(TOperator);
			result.DataFilter = MyOctreeNode.ContentFilter;
			result.DataType = MyStorageDataTypeEnum.Content;
			result.Leaves = m_contentLeaves;
			result.Nodes = m_contentNodes;
			result.Min = voxelRangeMin;
			result.Max = voxelRangeMax;
			return result;
		}

		private unsafe TraverseArgs<TOperator> MaterialArgs<TOperator>(ref Vector3I voxelRangeMin, ref Vector3I voxelRangeMax) where TOperator : struct, ITraverseOps
		{
			TraverseArgs<TOperator> result = default(TraverseArgs<TOperator>);
			result.Storage = this;
			result.Operator = default(TOperator);
			result.DataFilter = MyOctreeNode.MaterialFilter;
			result.DataType = MyStorageDataTypeEnum.Material;
			result.Leaves = m_materialLeaves;
			result.ContentLeaves = m_contentLeaves;
			result.Nodes = m_materialNodes;
			result.Min = voxelRangeMin;
			result.Max = voxelRangeMax;
			return result;
		}

		private unsafe static ChildType Traverse<TOps>(ref TraverseArgs<TOps> args, byte defaultData, int lodIdx, Vector3I lodCoord) where TOps : struct, ITraverseOps
		{
			MyCellCoord coord = new MyCellCoord(lodIdx, ref lodCoord);
			MyOctreeNode node;
			ChildType childType = args.Operator.Init(ref args, ref coord, defaultData, out node);
			if (childType != ChildType.Node)
			{
				return childType;
			}
			int num = 0;
			MyCellCoord myCellCoord;
			if (lodIdx <= 5)
			{
				childType = args.Operator.LeafOp(ref args, ref coord, defaultData, ref node);
			}
			else
			{
				myCellCoord = default(MyCellCoord);
				myCellCoord.Lod = lodIdx - 2 - 4;
				MyCellCoord myCellCoord2 = myCellCoord;
				Vector3I vector3I = lodCoord << 1;
				Vector3I min = (args.Min >> lodIdx - 1) - vector3I;
				Vector3I max = (args.Max >> lodIdx - 1) - vector3I;
				for (int i = 0; i < 8; i++)
				{
					ComputeChildCoord(i, out Vector3I relativeCoord);
					myCellCoord2.CoordInLod = vector3I + relativeCoord;
					if (!relativeCoord.IsInsideInclusiveEnd(ref min, ref max))
					{
						MyCellCoord myCellCoord3 = myCellCoord2;
						myCellCoord3.Lod = lodIdx - 1 - 4;
						ulong key = myCellCoord3.PackId64();
						if (args.Leaves.TryGetValue(key, out IMyOctreeLeafNode value) && value.ReadOnly)
						{
							num++;
						}
						continue;
					}
					switch (Traverse(ref args, node.GetData(i), lodIdx - 1, myCellCoord2.CoordInLod))
					{
					case ChildType.NodeMissing:
						num++;
						break;
					case ChildType.NodeEmpty:
					{
						ulong key5 = myCellCoord2.PackId64();
						MyOctreeNode myOctreeNode = args.Nodes[key5];
						node.SetChild(i, childPresent: false);
						byte data = myOctreeNode.GetData(0);
						node.SetData(i, data);
						args.Nodes.Remove(key5);
						break;
					}
					case ChildType.Node:
					{
						ulong key6 = myCellCoord2.PackId64();
						MyOctreeNode myOctreeNode2 = args.Nodes[key6];
						node.SetChild(i, childPresent: true);
						byte data2 = myOctreeNode2.ComputeFilteredValue(args.DataFilter, myCellCoord2.Lod);
						node.SetData(i, data2);
						break;
					}
					case ChildType.LeafReadonly:
						num++;
						break;
					case ChildType.NodeWithLeafReadonly:
					{
						num++;
						ulong key2 = myCellCoord2.PackId64();
						args.Nodes.Remove(key2);
						int num2 = lodIdx - 2 - 4;
						Vector3I a = myCellCoord2.CoordInLod << 1;
						for (int j = 0; j < 8; j++)
						{
							ComputeChildCoord(j, out Vector3I relativeCoord2);
							ulong key3 = new MyCellCoord(num2, a + relativeCoord2).PackId64();
							if (args.Leaves.TryGetValue(key3, out IMyOctreeLeafNode value2))
							{
								value2.Dispose();
								args.Leaves.Remove(key3);
							}
						}
						num2++;
						MyCellCoord myCellCoord4 = new MyCellCoord(num2, ref myCellCoord2.CoordInLod);
						ulong key4 = myCellCoord4.PackId64();
						MyCellCoord cell = myCellCoord4;
						cell.Lod += 4;
						IMyOctreeLeafNode myOctreeLeafNode = new MyProviderLeaf(args.Storage.DataProvider, args.DataType, ref cell);
						args.Leaves[key4] = myOctreeLeafNode;
						node.SetData(i, myOctreeLeafNode.GetFilteredValue());
						args.Storage.AccessRange(MyAccessType.Delete, args.DataType, ref cell);
						break;
					}
					}
				}
				if (num == 8)
				{
					childType = ChildType.NodeWithLeafReadonly;
				}
			}
			Dictionary<ulong, MyOctreeNode> nodes = args.Nodes;
			myCellCoord = new MyCellCoord(lodIdx - 1 - 4, ref lodCoord);
			nodes[myCellCoord.PackId64()] = node;
			if (!node.HasChildren && node.AllDataSame())
			{
				childType = ChildType.NodeEmpty;
			}
			return childType;
		}

		private static void DrawNodes(ref MatrixD worldMatrix, Color color, Dictionary<ulong, MyOctreeNode> octree)
		{
			using (IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, color))
			{
				MyCellCoord myCellCoord = default(MyCellCoord);
				foreach (KeyValuePair<ulong, MyOctreeNode> item in octree)
				{
					myCellCoord.SetUnpack(item.Key);
					myCellCoord.Lod += 4;
					MyOctreeNode value = item.Value;
					for (int i = 0; i < 8; i++)
					{
						if (!value.HasChild(i))
						{
							ComputeChildCoord(i, out Vector3I relativeCoord);
							Vector3I a = (myCellCoord.CoordInLod << myCellCoord.Lod + 1) + (relativeCoord << myCellCoord.Lod);
							float num = 1f * (float)(1 << myCellCoord.Lod);
							Vector3 value2 = a * 1f + 0.5f * num;
							BoundingBoxD aabb = new BoundingBoxD(value2 - 0.5f * num, value2 + 0.5f * num);
							myDebugDrawBatchAabb.Add(ref aabb);
						}
					}
				}
			}
		}

		private static void DrawLeaves(ref MatrixD worldMatrix, Color color, Dictionary<ulong, IMyOctreeLeafNode> octree)
		{
			using (IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, color))
			{
				MyCellCoord myCellCoord = default(MyCellCoord);
				foreach (KeyValuePair<ulong, IMyOctreeLeafNode> item in octree)
				{
					myCellCoord.SetUnpack(item.Key);
					myCellCoord.Lod += 4;
					_ = item.Value;
					Vector3I a = myCellCoord.CoordInLod << myCellCoord.Lod;
					BoundingBoxD aabb = new BoundingBoxD(a * 1f, (a + (1 << myCellCoord.Lod)) * 1f);
					myDebugDrawBatchAabb.Add(ref aabb);
				}
			}
		}

		private static void DrawScaledNodes(ref MatrixD worldMatrix, Color color, Dictionary<ulong, MyOctreeNode> octree)
		{
			using (IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, color))
			{
				MyCellCoord myCellCoord = default(MyCellCoord);
				foreach (KeyValuePair<ulong, MyOctreeNode> item in octree)
				{
					myCellCoord.SetUnpack(item.Key);
					myCellCoord.Lod += 4;
					MyOctreeNode value = item.Value;
					for (int i = 0; i < 8; i++)
					{
						if (!value.HasChild(i) || myCellCoord.Lod == 4)
						{
							ComputeChildCoord(i, out Vector3I relativeCoord);
							float num = (float)(int)value.GetData(i) / 255f;
							if (num != 0f)
							{
								Vector3I a = (myCellCoord.CoordInLod << myCellCoord.Lod + 1) + (relativeCoord << myCellCoord.Lod);
								float num2 = 1f * (float)(1 << myCellCoord.Lod);
								Vector3 value2 = a * 1f + 0.5f * num2;
								num = (float)Math.Pow((double)num * 1.0, 0.3333);
								num2 *= num;
								BoundingBoxD aabb = new BoundingBoxD(value2 - 0.5f * num2, value2 + 0.5f * num2);
								myDebugDrawBatchAabb.Add(ref aabb);
							}
						}
					}
				}
			}
		}

		private static void DrawSparseOctrees(ref MatrixD worldMatrix, Color color, MyVoxelDebugDrawMode mode, Dictionary<ulong, IMyOctreeLeafNode> octree)
		{
			MyCamera mainCamera = MySector.MainCamera;
			if (mainCamera != null)
			{
				MatrixD worldMatrix2 = mainCamera.WorldMatrix;
				Vector3 vector = Vector3D.Transform(worldMatrix2.Translation + worldMatrix2.Forward * 10.0, MatrixD.Invert(worldMatrix));
				using (IMyDebugDrawBatchAabb batch = MyRenderProxy.DebugDrawBatchAABB(worldMatrix, color))
				{
					MyCellCoord myCellCoord = default(MyCellCoord);
					foreach (KeyValuePair<ulong, IMyOctreeLeafNode> item in octree)
					{
						MyMicroOctreeLeaf myMicroOctreeLeaf = item.Value as MyMicroOctreeLeaf;
						if (myMicroOctreeLeaf != null)
						{
							myCellCoord.SetUnpack(item.Key);
							Vector3 min = (myCellCoord.CoordInLod << 4) * 1f;
							Vector3 max = min + 16f;
							if (vector.IsInsideInclusive(ref min, ref max))
							{
								myMicroOctreeLeaf.DebugDraw(batch, min, mode);
							}
						}
					}
				}
			}
		}

		private static void ComputeChildCoord(int childIdx, out Vector3I relativeCoord)
		{
			relativeCoord.X = (childIdx & 1);
			relativeCoord.Y = ((childIdx >> 1) & 1);
			relativeCoord.Z = ((childIdx >> 2) & 1);
		}

		public override ContainmentType Intersect(ref BoundingBox box, bool lazy)
		{
			using (StorageLock.AcquireSharedUsing())
			{
				if (base.Closed)
				{
					return ContainmentType.Disjoint;
				}
				BoundingBoxI box2 = new BoundingBoxI(new Vector3I(box.Min), new Vector3I(Vector3.Ceiling(box.Max)));
				return IntersectInternal(ref box2, 0, !lazy);
			}
		}

		protected unsafe override ContainmentType IntersectInternal(ref BoundingBoxI box, int lod, bool exhaustiveContainmentCheck)
		{
			int num = 0;
			int num2 = MySparseOctree.EstimateStackSize(m_treeHeight);
			MyCellCoord* ptr = stackalloc MyCellCoord[num2];
			MyCellCoord myCellCoord = new MyCellCoord(m_treeHeight + 4, ref Vector3I.Zero);
			ptr[num++] = myCellCoord;
			MyCellCoord myCellCoord2 = default(MyCellCoord);
			BoundingBoxI box2 = box;
			ContainmentType containmentType = (ContainmentType)(-1);
			while (num > 0)
			{
				myCellCoord = ptr[--num];
				myCellCoord2.Lod = Math.Max(myCellCoord.Lod - 4, 0);
				myCellCoord2.CoordInLod = myCellCoord.CoordInLod;
				int lod2;
				if (m_contentLeaves.TryGetValue(myCellCoord2.PackId64(), out IMyOctreeLeafNode value))
				{
					lod2 = myCellCoord.Lod;
					Vector3I min = box2.Min >> lod2;
					Vector3I max = box2.Max >> lod2;
					if (myCellCoord.CoordInLod.IsInsideInclusiveEnd(ref min, ref max))
					{
						Vector3I vector3I = myCellCoord2.CoordInLod << lod2;
						BoundingBoxI box3 = new BoundingBoxI(vector3I, vector3I + (1 << lod2));
						box3.IntersectWith(ref box2);
						ContainmentType containmentType2 = value.Intersect(ref box3, lod);
						if (containmentType == (ContainmentType)(-1))
						{
							containmentType = containmentType2;
						}
						if (containmentType2 == ContainmentType.Intersects)
						{
							return ContainmentType.Intersects;
						}
						if (containmentType2 != containmentType || (containmentType == ContainmentType.Contains && exhaustiveContainmentCheck))
						{
							return ContainmentType.Intersects;
						}
					}
					continue;
				}
				myCellCoord2.Lod--;
				lod2 = myCellCoord.Lod - 1;
				if (!m_contentNodes.TryGetValue(myCellCoord2.PackId64(), out MyOctreeNode value2))
				{
					continue;
				}
				Vector3I min2 = box2.Min >> lod2;
				Vector3I max2 = box2.Max >> lod2;
				Vector3I vector3I2 = myCellCoord.CoordInLod << 1;
				min2 -= vector3I2;
				max2 -= vector3I2;
				for (int i = 0; i < 8; i++)
				{
					ComputeChildCoord(i, out Vector3I relativeCoord);
					if (relativeCoord.IsInsideInclusiveEnd(ref min2, ref max2))
					{
						if (myCellCoord.Lod == 0)
						{
							return ContainmentType.Intersects;
						}
						if (value2.HasChild(i))
						{
							ptr[num++] = new MyCellCoord(myCellCoord.Lod - 1, vector3I2 + relativeCoord);
						}
						else if (value2.GetData(i) != 0 && containmentType != ContainmentType.Contains)
						{
							containmentType = ContainmentType.Intersects;
						}
					}
				}
			}
			if (containmentType == (ContainmentType)(-1))
			{
				containmentType = ContainmentType.Disjoint;
			}
			return containmentType;
		}

		public unsafe override bool IntersectInternal(ref LineD line)
		{
			int num = MySparseOctree.EstimateStackSize(m_treeHeight);
			MyCellCoord* ptr = stackalloc MyCellCoord[num];
			BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
			LineD lineD = default(LineD);
			lineD.From = line.To;
			lineD.To = line.From;
			MyCellCoord myCellCoord = new MyCellCoord(m_treeHeight + 4, ref Vector3I.Zero);
			int num2 = 0;
			ptr[num2++] = myCellCoord;
			MyCellCoord myCellCoord2 = default(MyCellCoord);
			BoundingBoxI boundingBoxI = (BoundingBoxI)line.GetBoundingBox();
			BoundingBoxD box = default(BoundingBoxD);
			while (num2 > 0)
			{
				myCellCoord = ptr[--num2];
				myCellCoord2.Lod = Math.Max(myCellCoord.Lod - 4, 0);
				myCellCoord2.CoordInLod = myCellCoord.CoordInLod;
				int lod;
				if (m_contentLeaves.TryGetValue(myCellCoord2.PackId64(), out IMyOctreeLeafNode value))
				{
					lod = myCellCoord.Lod;
					Vector3I min = boundingBoxI.Min >> lod;
					Vector3I max = boundingBoxI.Max >> lod;
					if (!myCellCoord.CoordInLod.IsInsideInclusiveEnd(ref min, ref max))
					{
						continue;
					}
					Vector3I value2 = min << myCellCoord.Lod;
					Vector3I value3 = max << myCellCoord.Lod;
					new BoundingBoxD(value2, value3).Intersect(ref line, out LineD intersectedLine);
					if (value.Intersect(ref intersectedLine, out double _, out double _))
					{
						if (boundingBoxD.Contains(intersectedLine.From) == ContainmentType.Disjoint)
						{
							lineD.From = intersectedLine.From;
							boundingBoxD.Include(intersectedLine.From);
						}
						if (boundingBoxD.Contains(intersectedLine.To) == ContainmentType.Disjoint)
						{
							lineD.To = intersectedLine.To;
							boundingBoxD.Include(intersectedLine.To);
						}
					}
					continue;
				}
				myCellCoord2.Lod--;
				lod = myCellCoord.Lod - 1;
				if (!m_contentNodes.TryGetValue(myCellCoord2.PackId64(), out MyOctreeNode value4))
				{
					continue;
				}
				Vector3I min2 = boundingBoxI.Min >> lod;
				Vector3I max2 = boundingBoxI.Max >> lod;
				Vector3I vector3I = myCellCoord.CoordInLod << 1;
				min2 -= vector3I;
				max2 -= vector3I;
				for (int i = 0; i < 8; i++)
				{
					ComputeChildCoord(i, out Vector3I relativeCoord);
					if (!relativeCoord.IsInsideInclusiveEnd(ref min2, ref max2))
					{
						continue;
					}
					box.Min = relativeCoord + myCellCoord2.CoordInLod << lod;
					box.Max = box.Min + (1 << myCellCoord.Lod);
					if (boundingBoxD.Contains(box) != ContainmentType.Contains)
					{
						if (value4.HasChild(i))
						{
							ptr[num2++] = new MyCellCoord(myCellCoord.Lod - 1, vector3I + relativeCoord);
						}
						else
						{
							value4.GetData(i);
						}
					}
				}
			}
			if (!boundingBoxD.Valid)
			{
				return false;
			}
			line.To = lineD.To;
			line.From = lineD.From;
			line.Length = (lineD.To - lineD.From).Length();
			return true;
		}

		public ContainmentType Contains(ref BoundingBox queryBox, ref BoundingSphere querySphere, int lodVoxelSize)
		{
			return Intersect(ref queryBox, lazy: false);
		}

		public float SignedDistance(ref Vector3 localPos, int lodVoxelSize)
		{
			MyStorageData myStorageData = m_storageCached;
			m_storageCached = null;
			if (myStorageData == null)
			{
				myStorageData = new MyStorageData(MyStorageDataTypeFlags.Content);
				myStorageData.Resize(Vector3I.One);
			}
			Vector3I vector3I = new Vector3I(localPos);
			ReadRange(myStorageData, MyStorageDataTypeFlags.Content, 0, vector3I, vector3I);
			byte num = myStorageData.Get(MyStorageDataTypeEnum.Content, 0);
			m_storageCached = myStorageData;
			return ((float)(int)num / 255f - 0.5f) * -2f;
		}

		public void ComputeContent(MyStorageData storage, int lodIndex, Vector3I lodVoxelRangeMin, Vector3I lodVoxelRangeMax, int lodVoxelSize)
		{
			ReadRange(storage, MyStorageDataTypeFlags.Content, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax);
		}

		public void DebugDraw(ref MatrixD worldMatrix, Color color)
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_COMPOSITION_CONTENT)
			{
				color.A = 10;
				DebugDraw(ref worldMatrix, MyVoxelDebugDrawMode.Content_MacroLeaves, color);
			}
		}

		void IMyCompositeShape.Close()
		{
			if (!base.Shared)
			{
				Close();
			}
		}

		public override void CloseInternal()
		{
			base.CloseInternal();
			foreach (IMyOctreeLeafNode value in m_contentLeaves.Values)
			{
				value.Dispose();
			}
			foreach (IMyOctreeLeafNode value2 in m_materialLeaves.Values)
			{
				value2.Dispose();
			}
		}

		static MyOctreeStorage()
		{
			m_attributesById = new Dictionary<int, MyStorageDataProviderAttribute>();
			m_attributesByType = new Dictionary<Type, MyStorageDataProviderAttribute>();
			RegisterTypes(Assembly.GetExecutingAssembly());
			RegisterTypes(MyPlugins.GameAssembly);
			RegisterTypes(MyPlugins.SandboxAssembly);
			RegisterTypes(MyPlugins.UserAssemblies);
		}

		public static void RegisterTypes(Assembly[] assemblies)
		{
			if (assemblies != null)
			{
				for (int i = 0; i < assemblies.Length; i++)
				{
					RegisterTypes(assemblies[i]);
				}
			}
		}

		private static void RegisterTypes(Assembly assembly)
		{
			if (assembly == null)
			{
				return;
			}
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(MyStorageDataProviderAttribute), inherit: false);
				if (customAttributes != null && customAttributes.Length != 0)
				{
					MyStorageDataProviderAttribute myStorageDataProviderAttribute = (MyStorageDataProviderAttribute)customAttributes[0];
					myStorageDataProviderAttribute.ProviderType = type;
					m_attributesById.Add(myStorageDataProviderAttribute.ProviderTypeId, myStorageDataProviderAttribute);
					m_attributesByType.Add(myStorageDataProviderAttribute.ProviderType, myStorageDataProviderAttribute);
				}
			}
		}

		private void WriteStorageMetaData(Stream stream)
		{
			ChunkHeader chunkHeader = default(ChunkHeader);
			chunkHeader.ChunkType = ChunkTypeEnum.StorageMetaData;
			chunkHeader.Version = 1;
			chunkHeader.Size = 17;
			chunkHeader.WriteTo(stream);
			stream.WriteNoAlloc(4);
			stream.WriteNoAlloc(base.Size.X);
			stream.WriteNoAlloc(base.Size.Y);
			stream.WriteNoAlloc(base.Size.Z);
			stream.WriteNoAlloc((byte)0);
		}

		private void ReadStorageMetaData(Stream stream, ChunkHeader header, ref bool isOldFormat)
		{
			stream.ReadInt32();
			Vector3I size = default(Vector3I);
			size.X = stream.ReadInt32();
			size.Y = stream.ReadInt32();
			size.Z = stream.ReadInt32();
			base.Size = size;
			stream.ReadByteNoAlloc();
			InitTreeHeight();
			AccessReset();
		}

		private unsafe static void WriteOctreeNodes(Stream stream, ChunkTypeEnum type, Dictionary<ulong, MyOctreeNode> nodes, Action<Stream, MyCellCoord> saveAccess)
		{
			ChunkHeader chunkHeader = default(ChunkHeader);
			chunkHeader.ChunkType = type;
			chunkHeader.Version = 2;
			chunkHeader.Size = nodes.Count * 17;
			chunkHeader.WriteTo(stream);
			foreach (KeyValuePair<ulong, MyOctreeNode> node in nodes)
			{
				stream.WriteNoAlloc(node.Key);
				MyOctreeNode value = node.Value;
				stream.WriteNoAlloc(value.ChildMask);
				stream.WriteNoAlloc(value.Data, 0, 8);
				if (saveAccess != null)
				{
					MyCellCoord arg = new MyCellCoord(MyCellCoord.UnpackLod(node.Key), MyCellCoord.UnpackCoord(node.Key));
					arg.Lod += 5;
					saveAccess(stream, arg);
				}
			}
		}

		private unsafe static void ReadOctreeNodes(Stream stream, ChunkHeader header, ref bool isOldFormat, Dictionary<ulong, MyOctreeNode> contentNodes, Action<MyCellCoord> loadAccess)
		{
			switch (header.Version)
			{
			case 1:
			{
				int num3 = header.Size / 13;
				MyCellCoord myCellCoord = default(MyCellCoord);
				MyOctreeNode value2 = default(MyOctreeNode);
				for (int j = 0; j < num3; j++)
				{
					myCellCoord.SetUnpack(stream.ReadUInt32());
					value2.ChildMask = stream.ReadByteNoAlloc();
					stream.ReadNoAlloc(value2.Data, 0, 8);
					ulong num4 = myCellCoord.PackId64();
					contentNodes.Add(num4, value2);
					if (loadAccess != null)
					{
						MyCellCoord obj2 = new MyCellCoord(MyCellCoord.UnpackLod(num4), MyCellCoord.UnpackCoord(num4));
						obj2.Lod += 5;
						loadAccess(obj2);
					}
				}
				isOldFormat = true;
				break;
			}
			case 2:
			{
				int num = header.Size / 17;
				MyOctreeNode value = default(MyOctreeNode);
				for (int i = 0; i < num; i++)
				{
					ulong num2 = stream.ReadUInt64();
					value.ChildMask = stream.ReadByteNoAlloc();
					stream.ReadNoAlloc(value.Data, 0, 8);
					contentNodes.Add(num2, value);
					if (loadAccess != null)
					{
						MyCellCoord obj = new MyCellCoord(MyCellCoord.UnpackLod(num2), MyCellCoord.UnpackCoord(num2));
						obj.Lod += 5;
						loadAccess(obj);
					}
				}
				break;
			}
			default:
				throw new InvalidBranchException();
			}
		}

		private static void WriteMaterialTable(Stream stream)
		{
			DictionaryValuesReader<string, MyVoxelMaterialDefinition> voxelMaterialDefinitions = MyDefinitionManager.Static.GetVoxelMaterialDefinitions();
			MemoryStream memoryStream;
			using (memoryStream = new MemoryStream(1024))
			{
				memoryStream.WriteNoAlloc(voxelMaterialDefinitions.Count);
				foreach (MyVoxelMaterialDefinition item in voxelMaterialDefinitions)
				{
					memoryStream.Write7BitEncodedInt(item.Index);
					memoryStream.WriteNoAlloc(item.Id.SubtypeName);
				}
			}
			byte[] array = memoryStream.ToArray();
			ChunkHeader chunkHeader = default(ChunkHeader);
			chunkHeader.ChunkType = ChunkTypeEnum.MaterialIndexTable;
			chunkHeader.Version = 1;
			chunkHeader.Size = array.Length;
			chunkHeader.WriteTo(stream);
			stream.Write(array, 0, array.Length);
		}

		private static Dictionary<byte, MyVoxelMaterialDefinition> ReadMaterialTable(Stream stream, ChunkHeader header, ref bool isOldFormat)
		{
			int num = stream.ReadInt32();
			Dictionary<byte, MyVoxelMaterialDefinition> dictionary = new Dictionary<byte, MyVoxelMaterialDefinition>(num);
			for (int i = 0; i < num; i++)
			{
				int num2 = stream.Read7BitEncodedInt();
				string name = stream.ReadString();
				MyVoxelMaterialDefinition myVoxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(name);
				if (myVoxelMaterialDefinition == null)
				{
					myVoxelMaterialDefinition = new MyVoxelMaterialDefinition();
				}
				dictionary.Add((byte)num2, myVoxelMaterialDefinition);
			}
			return dictionary;
		}

		private static void WriteOctreeLeaves<TLeaf>(Stream stream, Dictionary<ulong, TLeaf> leaves) where TLeaf : IMyOctreeLeafNode
		{
			foreach (KeyValuePair<ulong, TLeaf> leaf in leaves)
			{
				ChunkHeader chunkHeader = default(ChunkHeader);
				chunkHeader.ChunkType = leaf.Value.SerializedChunkType;
				chunkHeader.Size = leaf.Value.SerializedChunkSize + 8;
				chunkHeader.Version = 3;
				ChunkHeader chunkHeader2 = chunkHeader;
				chunkHeader2.WriteTo(stream);
				stream.WriteNoAlloc(leaf.Key);
				switch (chunkHeader2.ChunkType)
				{
				case ChunkTypeEnum.ContentLeafOctree:
					(leaf.Value as MyMicroOctreeLeaf).WriteTo(stream);
					break;
				case ChunkTypeEnum.MaterialLeafOctree:
					(leaf.Value as MyMicroOctreeLeaf).WriteTo(stream);
					break;
				default:
					throw new InvalidBranchException();
				case ChunkTypeEnum.ContentLeafProvider:
				case ChunkTypeEnum.MaterialLeafProvider:
					break;
				}
			}
		}

		private void ReadOctreeLeaf(Stream stream, ChunkHeader header, ref bool isOldFormat, MyStorageDataTypeEnum dataType, out ulong key, out MyMicroOctreeLeaf contentLeaf)
		{
			MyCellCoord myCellCoord = default(MyCellCoord);
			if (header.Version <= 2)
			{
				uint unpack = stream.ReadUInt32();
				myCellCoord.SetUnpack(unpack);
				key = myCellCoord.PackId64();
				header.Size -= 4;
				isOldFormat = true;
			}
			else
			{
				key = stream.ReadUInt64();
				myCellCoord.SetUnpack(key);
				header.Size -= 8;
			}
			contentLeaf = new MyMicroOctreeLeaf(dataType, 4, myCellCoord.CoordInLod << myCellCoord.Lod + 4);
			contentLeaf.ReadFrom(header, stream);
		}

		private void ReadProviderLeaf(Stream stream, ChunkHeader header, ref bool isOldFormat, HashSet<ulong> outKeySet)
		{
			ulong item;
			if (header.Version <= 2)
			{
				uint unpack = stream.ReadUInt32();
				MyCellCoord myCellCoord = default(MyCellCoord);
				myCellCoord.SetUnpack(unpack);
				item = myCellCoord.PackId64();
				header.Size -= 4;
				isOldFormat = true;
			}
			else
			{
				item = stream.ReadUInt64();
				header.Size -= 8;
			}
			outKeySet.Add(item);
			stream.SkipBytes(header.Size);
		}

		private static void WriteDataProvider(Stream stream, IMyStorageDataProvider provider)
		{
			if (provider != null)
			{
				ChunkHeader chunkHeader = default(ChunkHeader);
				chunkHeader.ChunkType = ChunkTypeEnum.DataProvider;
				chunkHeader.Version = 2;
				chunkHeader.Size = provider.SerializedSize + 4;
				ChunkHeader chunkHeader2 = chunkHeader;
				chunkHeader2.WriteTo(stream);
				stream.WriteNoAlloc(m_attributesByType[provider.GetType()].ProviderTypeId);
				provider.WriteTo(stream);
			}
		}

		private static void ReadDataProvider(Stream stream, ChunkHeader header, ref bool isOldFormat, out IMyStorageDataProvider provider)
		{
			switch (header.Version)
			{
			case 2:
			{
				int key = stream.ReadInt32();
				provider = (IMyStorageDataProvider)Activator.CreateInstance(m_attributesById[key].ProviderType);
				header.Size -= 4;
				provider.ReadFrom(header.Version, stream, header.Size, ref isOldFormat);
				break;
			}
			case 1:
				provider = (IMyStorageDataProvider)Activator.CreateInstance(m_attributesById[1].ProviderType);
				provider.ReadFrom(header.Version, stream, header.Size, ref isOldFormat);
				isOldFormat = true;
				break;
			default:
				throw new InvalidBranchException();
			}
		}
	}
}
