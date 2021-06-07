using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Groups;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyCubeGridSmallToLargeConnection : MySessionComponentBase
	{
		private struct MySlimBlockPair : IEquatable<MySlimBlockPair>
		{
			public MySlimBlock Parent;

			public MySlimBlock Child;

			public override int GetHashCode()
			{
				return Parent.GetHashCode() ^ Child.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (!(obj is MySlimBlockPair))
				{
					return false;
				}
				MySlimBlockPair mySlimBlockPair = (MySlimBlockPair)obj;
				if (Parent == mySlimBlockPair.Parent)
				{
					return Child == mySlimBlockPair.Child;
				}
				return false;
			}

			public bool Equals(MySlimBlockPair other)
			{
				if (Parent == other.Parent)
				{
					return Child == other.Child;
				}
				return false;
			}
		}

		private static readonly HashSet<MyCubeBlock> m_tmpBlocks = new HashSet<MyCubeBlock>();

		private static readonly HashSet<MySlimBlock> m_tmpSlimBlocks = new HashSet<MySlimBlock>();

		private static readonly HashSet<MySlimBlock> m_tmpSlimBlocks2 = new HashSet<MySlimBlock>();

		private static readonly List<MySlimBlock> m_tmpSlimBlocksList = new List<MySlimBlock>();

		private static readonly HashSet<MyCubeGrid> m_tmpGrids = new HashSet<MyCubeGrid>();

		private static readonly List<MyCubeGrid> m_tmpGridList = new List<MyCubeGrid>();

		private static bool m_smallToLargeCheckEnabled = true;

		private static readonly List<MySlimBlockPair> m_tmpBlockConnections = new List<MySlimBlockPair>();

		public static MyCubeGridSmallToLargeConnection Static;

		private readonly Dictionary<MyCubeGrid, HashSet<MySlimBlockPair>> m_mapLargeGridToConnectedBlocks = new Dictionary<MyCubeGrid, HashSet<MySlimBlockPair>>();

		private readonly Dictionary<MyCubeGrid, HashSet<MySlimBlockPair>> m_mapSmallGridToConnectedBlocks = new Dictionary<MyCubeGrid, HashSet<MySlimBlockPair>>();

		public override bool IsRequiredByGame
		{
			get
			{
				if (base.IsRequiredByGame)
				{
					return MyFakes.ENABLE_SMALL_BLOCK_TO_LARGE_STATIC_CONNECTIONS;
				}
				return false;
			}
		}

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		private void GetSurroundingBlocksFromStaticGrids(MySlimBlock block, MyCubeSize cubeSizeEnum, HashSet<MyCubeBlock> outBlocks)
		{
			outBlocks.Clear();
			BoundingBoxD boundingBoxD = new BoundingBoxD(block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f, block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f);
			if (block.FatBlock != null)
			{
				Vector3D center = boundingBoxD.Center;
				boundingBoxD = block.FatBlock.Model.BoundingBox;
				block.FatBlock.Orientation.GetMatrix(out Matrix result);
				boundingBoxD = boundingBoxD.TransformFast(result);
				boundingBoxD.Translate(center);
			}
			boundingBoxD = boundingBoxD.TransformFast(block.CubeGrid.WorldMatrix);
			boundingBoxD.Inflate(0.125);
			List<MyEntity> list = new List<MyEntity>();
			MyEntities.GetElementsInBox(ref boundingBoxD, list);
			for (int i = 0; i < list.Count; i++)
			{
				MyCubeBlock myCubeBlock = list[i] as MyCubeBlock;
				if (myCubeBlock != null && myCubeBlock.SlimBlock != block && myCubeBlock.CubeGrid.IsStatic && myCubeBlock.CubeGrid.EnableSmallToLargeConnections && myCubeBlock.CubeGrid.SmallToLargeConnectionsInitialized && myCubeBlock.CubeGrid != block.CubeGrid && myCubeBlock.CubeGrid.GridSizeEnum == cubeSizeEnum && !(myCubeBlock is MyFracturedBlock) && !myCubeBlock.Components.Has<MyFractureComponentBase>())
				{
					MyCompoundCubeBlock myCompoundCubeBlock = myCubeBlock as MyCompoundCubeBlock;
					if (myCompoundCubeBlock != null)
					{
						foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
						{
							if (block2 != block && !block2.FatBlock.Components.Has<MyFractureComponentBase>())
							{
								outBlocks.Add(block2.FatBlock);
							}
						}
					}
					else
					{
						outBlocks.Add(myCubeBlock);
					}
				}
			}
			list.Clear();
		}

		private void GetSurroundingBlocksFromStaticGrids(MySlimBlock block, MyCubeSize cubeSizeEnum, HashSet<MySlimBlock> outBlocks)
		{
			outBlocks.Clear();
			BoundingBoxD aabbForNeighbors = new BoundingBoxD(block.Min * block.CubeGrid.GridSize, block.Max * block.CubeGrid.GridSize);
			BoundingBoxD box = new BoundingBoxD(block.Min * block.CubeGrid.GridSize - block.CubeGrid.GridSize / 2f, block.Max * block.CubeGrid.GridSize + block.CubeGrid.GridSize / 2f);
			if (block.FatBlock != null)
			{
				Vector3D center = box.Center;
				box = block.FatBlock.Model.BoundingBox;
				block.FatBlock.Orientation.GetMatrix(out Matrix result);
				box = box.TransformFast(result);
				box.Translate(center);
			}
			box.Inflate(0.125);
			BoundingBoxD boundingBox = box.TransformFast(block.CubeGrid.WorldMatrix);
			List<MyEntity> list = new List<MyEntity>();
			MyEntities.GetElementsInBox(ref boundingBox, list);
			for (int i = 0; i < list.Count; i++)
			{
				MyCubeGrid myCubeGrid = list[i] as MyCubeGrid;
				if (myCubeGrid != null && myCubeGrid.IsStatic && myCubeGrid != block.CubeGrid && myCubeGrid.EnableSmallToLargeConnections && myCubeGrid.SmallToLargeConnectionsInitialized && myCubeGrid.GridSizeEnum == cubeSizeEnum)
				{
					m_tmpSlimBlocksList.Clear();
					myCubeGrid.GetBlocksIntersectingOBB(box, block.CubeGrid.WorldMatrix, m_tmpSlimBlocksList);
					CheckNeighborBlocks(block, aabbForNeighbors, myCubeGrid, m_tmpSlimBlocksList);
					foreach (MySlimBlock tmpSlimBlocks in m_tmpSlimBlocksList)
					{
						if (tmpSlimBlocks.FatBlock != null)
						{
							if (!(tmpSlimBlocks.FatBlock is MyFracturedBlock) && !tmpSlimBlocks.FatBlock.Components.Has<MyFractureComponentBase>())
							{
								if (tmpSlimBlocks.FatBlock is MyCompoundCubeBlock)
								{
									foreach (MySlimBlock block2 in (tmpSlimBlocks.FatBlock as MyCompoundCubeBlock).GetBlocks())
									{
										if (!block2.FatBlock.Components.Has<MyFractureComponentBase>())
										{
											outBlocks.Add(block2);
										}
									}
								}
								else
								{
									outBlocks.Add(tmpSlimBlocks);
								}
							}
						}
						else
						{
							outBlocks.Add(tmpSlimBlocks);
						}
					}
					m_tmpSlimBlocksList.Clear();
				}
			}
			list.Clear();
		}

		private static void CheckNeighborBlocks(MySlimBlock block, BoundingBoxD aabbForNeighbors, MyCubeGrid cubeGrid, List<MySlimBlock> blocks)
		{
			MatrixD m = block.CubeGrid.WorldMatrix * cubeGrid.PositionComp.WorldMatrixNormalizedInv;
			BoundingBoxD boundingBoxD = aabbForNeighbors.TransformFast(ref m);
			Vector3I value = Vector3I.Round(cubeGrid.GridSizeR * boundingBoxD.Min);
			Vector3I value2 = Vector3I.Round(cubeGrid.GridSizeR * boundingBoxD.Max);
			Vector3I start = Vector3I.Min(value, value2);
			Vector3I end = Vector3I.Max(value, value2);
			for (int num = blocks.Count - 1; num >= 0; num--)
			{
				MySlimBlock mySlimBlock = blocks[num];
				bool flag = false;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref mySlimBlock.Min, ref mySlimBlock.Max);
				Vector3I next = vector3I_RangeIterator.Current;
				while (vector3I_RangeIterator.IsValid())
				{
					Vector3I_RangeIterator vector3I_RangeIterator2 = new Vector3I_RangeIterator(ref start, ref end);
					Vector3I next2 = vector3I_RangeIterator2.Current;
					while (vector3I_RangeIterator2.IsValid())
					{
						Vector3I vector3I = Vector3I.Abs(next - next2);
						if (next2 == next || vector3I.X + vector3I.Y + vector3I.Z == 1)
						{
							flag = true;
							break;
						}
						vector3I_RangeIterator2.GetNext(out next2);
					}
					if (flag)
					{
						break;
					}
					vector3I_RangeIterator.GetNext(out next);
				}
				if (!flag)
				{
					blocks.RemoveAt(num);
				}
			}
		}

		private void ConnectSmallToLargeBlock(MySlimBlock smallBlock, MySlimBlock largeBlock)
		{
			if (GetCubeSize(smallBlock) != MyCubeSize.Small || GetCubeSize(largeBlock) != 0 || smallBlock.FatBlock is MyCompoundCubeBlock || largeBlock.FatBlock is MyCompoundCubeBlock)
			{
				return;
			}
			long linkId = ((long)largeBlock.UniqueId << 32) + smallBlock.UniqueId;
			if (!MyCubeGridGroups.Static.SmallToLargeBlockConnections.LinkExists(linkId, largeBlock))
			{
				MyCubeGridGroups.Static.SmallToLargeBlockConnections.CreateLink(linkId, largeBlock, smallBlock);
				MyCubeGridGroups.Static.Physical.CreateLink(linkId, largeBlock.CubeGrid, smallBlock.CubeGrid);
				MyCubeGridGroups.Static.Logical.CreateLink(linkId, largeBlock.CubeGrid, smallBlock.CubeGrid);
				MySlimBlockPair item = default(MySlimBlockPair);
				item.Parent = largeBlock;
				item.Child = smallBlock;
				if (!m_mapLargeGridToConnectedBlocks.TryGetValue(largeBlock.CubeGrid, out HashSet<MySlimBlockPair> value))
				{
					value = new HashSet<MySlimBlockPair>();
					m_mapLargeGridToConnectedBlocks.Add(largeBlock.CubeGrid, value);
					largeBlock.CubeGrid.OnClosing += CubeGrid_OnClosing;
				}
				value.Add(item);
				if (!m_mapSmallGridToConnectedBlocks.TryGetValue(smallBlock.CubeGrid, out HashSet<MySlimBlockPair> value2))
				{
					value2 = new HashSet<MySlimBlockPair>();
					m_mapSmallGridToConnectedBlocks.Add(smallBlock.CubeGrid, value2);
					smallBlock.CubeGrid.OnClosing += CubeGrid_OnClosing;
				}
				value2.Add(item);
			}
		}

		private void DisconnectSmallToLargeBlock(MySlimBlock smallBlock, MyCubeGrid smallGrid, MySlimBlock largeBlock, MyCubeGrid largeGrid)
		{
			if (GetCubeSize(smallBlock) != MyCubeSize.Small || GetCubeSize(largeBlock) != 0 || smallBlock.FatBlock is MyCompoundCubeBlock || largeBlock.FatBlock is MyCompoundCubeBlock)
			{
				return;
			}
			long linkId = ((long)largeBlock.UniqueId << 32) + smallBlock.UniqueId;
			MyCubeGridGroups.Static.SmallToLargeBlockConnections.BreakLink(linkId, largeBlock);
			MyCubeGridGroups.Static.Physical.BreakLink(linkId, largeGrid);
			MyCubeGridGroups.Static.Logical.BreakLink(linkId, largeGrid);
			MySlimBlockPair item = default(MySlimBlockPair);
			item.Parent = largeBlock;
			item.Child = smallBlock;
			if (m_mapLargeGridToConnectedBlocks.TryGetValue(largeGrid, out HashSet<MySlimBlockPair> value))
			{
				value.Remove(item);
				if (value.Count == 0)
				{
					m_mapLargeGridToConnectedBlocks.Remove(largeGrid);
					largeGrid.OnClosing -= CubeGrid_OnClosing;
				}
			}
			if (m_mapSmallGridToConnectedBlocks.TryGetValue(smallGrid, out HashSet<MySlimBlockPair> value2))
			{
				value2.Remove(item);
				if (value2.Count == 0)
				{
					m_mapSmallGridToConnectedBlocks.Remove(smallGrid);
					smallGrid.OnClosing -= CubeGrid_OnClosing;
				}
			}
		}

		private void DisconnectSmallToLargeBlock(MySlimBlock smallBlock, MySlimBlock largeBlock)
		{
			DisconnectSmallToLargeBlock(smallBlock, smallBlock.CubeGrid, largeBlock, largeBlock.CubeGrid);
		}

		internal bool AddGridSmallToLargeConnection(MyCubeGrid grid)
		{
			if (!grid.IsStatic)
			{
				return false;
			}
			if (!grid.EnableSmallToLargeConnections || !grid.SmallToLargeConnectionsInitialized)
			{
				return false;
			}
			bool flag = false;
			foreach (MySlimBlock block in grid.GetBlocks())
			{
				if (!(block.FatBlock is MyFracturedBlock))
				{
					bool flag2 = AddBlockSmallToLargeConnection(block);
					flag = (flag || flag2);
				}
			}
			return flag;
		}

		public bool AddBlockSmallToLargeConnection(MySlimBlock block)
		{
			if (!m_smallToLargeCheckEnabled)
			{
				return true;
			}
			if (!block.CubeGrid.IsStatic || !block.CubeGrid.EnableSmallToLargeConnections || !block.CubeGrid.SmallToLargeConnectionsInitialized || (block.FatBlock != null && block.FatBlock.Components.Has<MyFractureComponentBase>()))
			{
				return false;
			}
			bool flag = false;
			if (block.FatBlock is MyCompoundCubeBlock)
			{
				foreach (MySlimBlock block2 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
				{
					bool flag2 = AddBlockSmallToLargeConnection(block2);
					flag = (flag || flag2);
				}
				return flag;
			}
			MyCubeSize cubeSizeEnum = (GetCubeSize(block) == MyCubeSize.Large) ? MyCubeSize.Small : MyCubeSize.Large;
			GetSurroundingBlocksFromStaticGrids(block, cubeSizeEnum, m_tmpSlimBlocks2);
			if (m_tmpSlimBlocks2.Count == 0)
			{
				return false;
			}
			MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Small);
			block.GetWorldBoundingBox(out BoundingBoxD aabb);
			aabb.Inflate(0.05);
			if (GetCubeSize(block) == MyCubeSize.Large)
			{
				foreach (MySlimBlock item in m_tmpSlimBlocks2)
				{
					item.GetWorldBoundingBox(out BoundingBoxD aabb2);
					if (aabb2.Intersects(aabb) && SmallBlockConnectsToLarge(item, ref aabb2, block, ref aabb))
					{
						ConnectSmallToLargeBlock(item, block);
						flag = true;
					}
				}
				return flag;
			}
			foreach (MySlimBlock item2 in m_tmpSlimBlocks2)
			{
				item2.GetWorldBoundingBox(out BoundingBoxD aabb3);
				if (aabb3.Intersects(aabb) && SmallBlockConnectsToLarge(block, ref aabb, item2, ref aabb3))
				{
					ConnectSmallToLargeBlock(block, item2);
					flag = true;
				}
			}
			return flag;
		}

		internal void RemoveBlockSmallToLargeConnection(MySlimBlock block)
		{
			if (!m_smallToLargeCheckEnabled || !block.CubeGrid.IsStatic)
			{
				return;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
			if (myCompoundCubeBlock != null)
			{
				foreach (MySlimBlock block2 in myCompoundCubeBlock.GetBlocks())
				{
					RemoveBlockSmallToLargeConnection(block2);
				}
				return;
			}
			m_tmpGrids.Clear();
			if (GetCubeSize(block) == MyCubeSize.Large)
			{
				RemoveChangedLargeBlockConnectionToSmallBlocks(block, m_tmpGrids);
				if (Sync.IsServer)
				{
					foreach (MyCubeGrid tmpGrid in m_tmpGrids)
					{
						if (tmpGrid.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(tmpGrid))
						{
							tmpGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
						}
					}
				}
				m_tmpGrids.Clear();
				return;
			}
			MyGroups<MySlimBlock, MyBlockGroupData>.Group group = MyCubeGridGroups.Static.SmallToLargeBlockConnections.GetGroup(block);
			if (group == null)
			{
				if (Sync.IsServer && block.CubeGrid.GetBlocks().Count > 0 && block.CubeGrid.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(block.CubeGrid))
				{
					block.CubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
				}
				return;
			}
			m_tmpSlimBlocks.Clear();
			foreach (MyGroups<MySlimBlock, MyBlockGroupData>.Node node in group.Nodes)
			{
				foreach (MyGroups<MySlimBlock, MyBlockGroupData>.Node child in node.Children)
				{
					if (child.NodeData == block)
					{
						m_tmpSlimBlocks.Add(node.NodeData);
						break;
					}
				}
			}
			foreach (MySlimBlock tmpSlimBlock in m_tmpSlimBlocks)
			{
				DisconnectSmallToLargeBlock(block, tmpSlimBlock);
			}
			m_tmpSlimBlocks.Clear();
			if (Sync.IsServer && !m_mapSmallGridToConnectedBlocks.TryGetValue(block.CubeGrid, out HashSet<MySlimBlockPair> _) && block.CubeGrid.GetBlocks().Count > 0 && block.CubeGrid.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(block.CubeGrid))
			{
				block.CubeGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
			}
		}

		internal void ConvertToDynamic(MyCubeGrid grid)
		{
			if (grid.GridSizeEnum == MyCubeSize.Small)
			{
				RemoveSmallGridConnections(grid);
			}
			else
			{
				RemoveLargeGridConnections(grid);
			}
		}

		private void RemoveLargeGridConnections(MyCubeGrid grid)
		{
			m_tmpGrids.Clear();
			if (m_mapLargeGridToConnectedBlocks.TryGetValue(grid, out HashSet<MySlimBlockPair> value))
			{
				m_tmpBlockConnections.Clear();
				m_tmpBlockConnections.AddRange(value);
				foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
				{
					DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
					m_tmpGrids.Add(tmpBlockConnection.Child.CubeGrid);
				}
				m_tmpBlockConnections.Clear();
				if (Sync.IsServer)
				{
					m_tmpGridList.Clear();
					foreach (MyCubeGrid tmpGrid in m_tmpGrids)
					{
						if (m_mapSmallGridToConnectedBlocks.ContainsKey(tmpGrid))
						{
							m_tmpGridList.Add(tmpGrid);
						}
					}
					foreach (MyCubeGrid tmpGrid2 in m_tmpGridList)
					{
						m_tmpGrids.Remove(tmpGrid2);
					}
					m_tmpGridList.Clear();
					foreach (MyCubeGrid tmpGrid3 in m_tmpGrids)
					{
						if (tmpGrid3.IsStatic && tmpGrid3.TestDynamic == MyCubeGrid.MyTestDynamicReason.NoReason && !SmallGridIsStatic(tmpGrid3))
						{
							tmpGrid3.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
						}
					}
				}
				m_tmpGrids.Clear();
			}
		}

		private void RemoveSmallGridConnections(MyCubeGrid grid)
		{
			if (m_mapSmallGridToConnectedBlocks.TryGetValue(grid, out HashSet<MySlimBlockPair> value))
			{
				m_tmpBlockConnections.Clear();
				m_tmpBlockConnections.AddRange(value);
				foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
				{
					DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
				}
				m_tmpBlockConnections.Clear();
			}
		}

		public bool TestGridSmallToLargeConnection(MyCubeGrid smallGrid)
		{
			if (!smallGrid.IsStatic)
			{
				return false;
			}
			if (!Sync.IsServer)
			{
				return true;
			}
			if (m_mapSmallGridToConnectedBlocks.TryGetValue(smallGrid, out HashSet<MySlimBlockPair> value) && value.Count > 0)
			{
				return true;
			}
			return false;
		}

		private Vector3I GetSmallBlockAddDirection(ref BoundingBoxD smallBlockWorldAabb, ref BoundingBoxD smallBlockWorldAabbReduced, ref BoundingBoxD largeBlockWorldAabb)
		{
			if (smallBlockWorldAabbReduced.Min.X > largeBlockWorldAabb.Max.X && smallBlockWorldAabb.Min.X <= largeBlockWorldAabb.Max.X)
			{
				return Vector3I.UnitX;
			}
			if (smallBlockWorldAabbReduced.Max.X < largeBlockWorldAabb.Min.X && smallBlockWorldAabb.Max.X >= largeBlockWorldAabb.Min.X)
			{
				return -Vector3I.UnitX;
			}
			if (smallBlockWorldAabbReduced.Min.Y > largeBlockWorldAabb.Max.Y && smallBlockWorldAabb.Min.Y <= largeBlockWorldAabb.Max.Y)
			{
				return Vector3I.UnitY;
			}
			if (smallBlockWorldAabbReduced.Max.Y < largeBlockWorldAabb.Min.Y && smallBlockWorldAabb.Max.Y >= largeBlockWorldAabb.Min.Y)
			{
				return -Vector3I.UnitY;
			}
			if (smallBlockWorldAabbReduced.Min.Z > largeBlockWorldAabb.Max.Z && smallBlockWorldAabb.Min.Z <= largeBlockWorldAabb.Max.Z)
			{
				return Vector3I.UnitZ;
			}
			return -Vector3I.UnitZ;
		}

		private bool SmallBlockConnectsToLarge(MySlimBlock smallBlock, ref BoundingBoxD smallBlockWorldAabb, MySlimBlock largeBlock, ref BoundingBoxD largeBlockWorldAabb)
		{
			BoundingBoxD smallBlockWorldAabbReduced = smallBlockWorldAabb;
			smallBlockWorldAabbReduced.Inflate((0f - smallBlock.CubeGrid.GridSize) / 4f);
			if (!largeBlockWorldAabb.Intersects(smallBlockWorldAabbReduced))
			{
				Vector3I addNormal = GetSmallBlockAddDirection(ref smallBlockWorldAabb, ref smallBlockWorldAabbReduced, ref largeBlockWorldAabb);
				smallBlock.Orientation.GetQuaternion(out Quaternion result);
				result = Quaternion.CreateFromRotationMatrix(smallBlock.CubeGrid.WorldMatrix) * result;
				if (!MyCubeGrid.CheckConnectivitySmallBlockToLargeGrid(largeBlock.CubeGrid, smallBlock.BlockDefinition, ref result, ref addNormal))
				{
					return false;
				}
			}
			BoundingBoxD boundingBoxD = smallBlockWorldAabb;
			boundingBoxD.Inflate(2f * smallBlock.CubeGrid.GridSize / 3f);
			BoundingBoxD boundingBoxD2 = boundingBoxD.Intersect(largeBlockWorldAabb);
			Vector3D translation = boundingBoxD2.Center;
			HkShape shape = new HkBoxShape(boundingBoxD2.HalfExtents);
			largeBlock.Orientation.GetQuaternion(out Quaternion result2);
			result2 = Quaternion.CreateFromRotationMatrix(largeBlock.CubeGrid.WorldMatrix) * result2;
			largeBlock.ComputeWorldCenter(out Vector3D worldCenter);
			bool flag = false;
			try
			{
				if (largeBlock.FatBlock == null)
				{
					HkShape shape2 = new HkBoxShape(largeBlock.BlockDefinition.Size * largeBlock.CubeGrid.GridSize / 2f);
					flag = MyPhysics.IsPenetratingShapeShape(shape, ref translation, ref Quaternion.Identity, shape2, ref worldCenter, ref result2);
					shape2.RemoveReference();
					return flag;
				}
				MyModel model = largeBlock.FatBlock.Model;
				if (model != null && model.HavokCollisionShapes != null)
				{
					HkShape[] havokCollisionShapes = model.HavokCollisionShapes;
					int num = 0;
					while (true)
					{
						if (num >= havokCollisionShapes.Length)
						{
							return flag;
						}
						flag = MyPhysics.IsPenetratingShapeShape(shape, ref translation, ref Quaternion.Identity, havokCollisionShapes[num], ref worldCenter, ref result2);
						if (flag)
						{
							break;
						}
						num++;
					}
					return flag;
				}
				HkShape shape3 = new HkBoxShape(largeBlock.BlockDefinition.Size * largeBlock.CubeGrid.GridSize / 2f);
				flag = MyPhysics.IsPenetratingShapeShape(shape, ref translation, ref Quaternion.Identity, shape3, ref worldCenter, ref result2);
				shape3.RemoveReference();
				return flag;
			}
			finally
			{
				shape.RemoveReference();
			}
		}

		private void RemoveChangedLargeBlockConnectionToSmallBlocks(MySlimBlock block, HashSet<MyCubeGrid> outSmallGrids)
		{
			MyGroups<MySlimBlock, MyBlockGroupData>.Group group = MyCubeGridGroups.Static.SmallToLargeBlockConnections.GetGroup(block);
			if (group != null)
			{
				m_tmpSlimBlocks.Clear();
				foreach (MyGroups<MySlimBlock, MyBlockGroupData>.Node node in group.Nodes)
				{
					if (node.NodeData == block)
					{
						foreach (MyGroups<MySlimBlock, MyBlockGroupData>.Node child in node.Children)
						{
							m_tmpSlimBlocks.Add(child.NodeData);
						}
						break;
					}
				}
				foreach (MySlimBlock tmpSlimBlock in m_tmpSlimBlocks)
				{
					DisconnectSmallToLargeBlock(tmpSlimBlock, block);
					outSmallGrids.Add(tmpSlimBlock.CubeGrid);
				}
				m_tmpSlimBlocks.Clear();
				m_tmpGridList.Clear();
				foreach (MyCubeGrid outSmallGrid in outSmallGrids)
				{
					if (m_mapSmallGridToConnectedBlocks.TryGetValue(outSmallGrid, out HashSet<MySlimBlockPair> _))
					{
						m_tmpGridList.Add(outSmallGrid);
					}
				}
				foreach (MyCubeGrid tmpGrid in m_tmpGridList)
				{
					outSmallGrids.Remove(tmpGrid);
				}
				m_tmpGridList.Clear();
			}
		}

		private bool SmallGridIsStatic(MyCubeGrid smallGrid)
		{
			if (TestGridSmallToLargeConnection(smallGrid))
			{
				return true;
			}
			return false;
		}

		internal void BeforeGridSplit_SmallToLargeGridConnectivity(MyCubeGrid originalGrid)
		{
			m_smallToLargeCheckEnabled = false;
		}

		internal void AfterGridSplit_SmallToLargeGridConnectivity(MyCubeGrid originalGrid, List<MyCubeGrid> gridSplits)
		{
			m_smallToLargeCheckEnabled = true;
			if (originalGrid.GridSizeEnum == MyCubeSize.Small)
			{
				AfterGridSplit_Small(originalGrid, gridSplits);
			}
			else
			{
				AfterGridSplit_Large(originalGrid, gridSplits);
			}
		}

		private void AfterGridSplit_Small(MyCubeGrid originalGrid, List<MyCubeGrid> gridSplits)
		{
			if (!originalGrid.IsStatic)
			{
				return;
			}
			if (m_mapSmallGridToConnectedBlocks.TryGetValue(originalGrid, out HashSet<MySlimBlockPair> value))
			{
				m_tmpBlockConnections.Clear();
				foreach (MySlimBlockPair item in value)
				{
					if (item.Child.CubeGrid != originalGrid)
					{
						m_tmpBlockConnections.Add(item);
					}
				}
				foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
				{
					DisconnectSmallToLargeBlock(tmpBlockConnection.Child, originalGrid, tmpBlockConnection.Parent, tmpBlockConnection.Parent.CubeGrid);
					ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
				}
				m_tmpBlockConnections.Clear();
			}
			if (Sync.IsServer)
			{
				if (!m_mapSmallGridToConnectedBlocks.TryGetValue(originalGrid, out value) || value.Count == 0)
				{
					originalGrid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
				}
				foreach (MyCubeGrid gridSplit in gridSplits)
				{
					if (!m_mapSmallGridToConnectedBlocks.TryGetValue(gridSplit, out value) || value.Count == 0)
					{
						gridSplit.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
					}
				}
			}
		}

		private void AfterGridSplit_Large(MyCubeGrid originalGrid, List<MyCubeGrid> gridSplits)
		{
			if (originalGrid.IsStatic && m_mapLargeGridToConnectedBlocks.TryGetValue(originalGrid, out HashSet<MySlimBlockPair> value))
			{
				m_tmpBlockConnections.Clear();
				foreach (MySlimBlockPair item in value)
				{
					if (item.Parent.CubeGrid != originalGrid)
					{
						m_tmpBlockConnections.Add(item);
					}
				}
				foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
				{
					DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Child.CubeGrid, tmpBlockConnection.Parent, originalGrid);
					ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
				}
				m_tmpBlockConnections.Clear();
			}
		}

		internal void BeforeGridMerge_SmallToLargeGridConnectivity(MyCubeGrid originalGrid, MyCubeGrid mergedGrid)
		{
			m_tmpGrids.Clear();
			if (originalGrid.IsStatic && mergedGrid.IsStatic)
			{
				m_tmpGrids.Add(mergedGrid);
			}
			m_smallToLargeCheckEnabled = false;
		}

		internal void AfterGridMerge_SmallToLargeGridConnectivity(MyCubeGrid originalGrid)
		{
			m_smallToLargeCheckEnabled = true;
			if (m_tmpGrids.Count != 0 && originalGrid.IsStatic)
			{
				if (originalGrid.GridSizeEnum == MyCubeSize.Large)
				{
					foreach (MyCubeGrid tmpGrid in m_tmpGrids)
					{
						if (m_mapLargeGridToConnectedBlocks.TryGetValue(tmpGrid, out HashSet<MySlimBlockPair> value))
						{
							m_tmpBlockConnections.Clear();
							m_tmpBlockConnections.AddRange(value);
							foreach (MySlimBlockPair tmpBlockConnection in m_tmpBlockConnections)
							{
								DisconnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Child.CubeGrid, tmpBlockConnection.Parent, tmpGrid);
								ConnectSmallToLargeBlock(tmpBlockConnection.Child, tmpBlockConnection.Parent);
							}
						}
					}
				}
				else
				{
					foreach (MyCubeGrid tmpGrid2 in m_tmpGrids)
					{
						if (m_mapSmallGridToConnectedBlocks.TryGetValue(tmpGrid2, out HashSet<MySlimBlockPair> value2))
						{
							m_tmpBlockConnections.Clear();
							m_tmpBlockConnections.AddRange(value2);
							foreach (MySlimBlockPair tmpBlockConnection2 in m_tmpBlockConnections)
							{
								DisconnectSmallToLargeBlock(tmpBlockConnection2.Child, tmpGrid2, tmpBlockConnection2.Parent, tmpBlockConnection2.Parent.CubeGrid);
								ConnectSmallToLargeBlock(tmpBlockConnection2.Child, tmpBlockConnection2.Parent);
							}
						}
					}
				}
				m_tmpGrids.Clear();
				m_tmpBlockConnections.Clear();
			}
		}

		private void CubeGrid_OnClosing(MyEntity entity)
		{
			MyCubeGrid myCubeGrid = (MyCubeGrid)entity;
			if (myCubeGrid.GridSizeEnum == MyCubeSize.Small)
			{
				RemoveSmallGridConnections(myCubeGrid);
			}
			else
			{
				RemoveLargeGridConnections(myCubeGrid);
			}
		}

		private static MyCubeSize GetCubeSize(MySlimBlock block)
		{
			if (block.CubeGrid != null)
			{
				return block.CubeGrid.GridSizeEnum;
			}
			MyFracturedBlock myFracturedBlock = block.FatBlock as MyFracturedBlock;
			if (myFracturedBlock != null && myFracturedBlock.OriginalBlocks.Count > 0 && MyDefinitionManager.Static.TryGetCubeBlockDefinition(myFracturedBlock.OriginalBlocks[0], out MyCubeBlockDefinition blockDefinition))
			{
				return blockDefinition.CubeSize;
			}
			return block.BlockDefinition.CubeSize;
		}
	}
}
