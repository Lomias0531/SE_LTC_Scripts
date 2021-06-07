using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public abstract class MyAdditionalModelGeneratorBase : IMyBlockAdditionalModelGenerator
	{
		protected class MyGridInfo
		{
			public MyCubeGrid Grid;

			public MatrixI Transform;
		}

		protected struct MyGeneratedBlockLocation
		{
			public MySlimBlock RefBlock;

			public MyCubeBlockDefinition BlockDefinition;

			public Vector3I Position;

			public MyBlockOrientation Orientation;

			public ushort? BlockIdInCompound;

			public MyGridInfo GridInfo;

			public MyStringId GeneratedBlockType;

			public MyGeneratedBlockLocation(MySlimBlock refBlock, MyCubeBlockDefinition blockDefinition, Vector3I position, MyBlockOrientation orientation, ushort? blockIdInCompound = null, MyGridInfo gridInfo = null)
			{
				RefBlock = refBlock;
				BlockDefinition = blockDefinition;
				Position = position;
				Orientation = orientation;
				BlockIdInCompound = blockIdInCompound;
				GridInfo = gridInfo;
				GeneratedBlockType = MyStringId.NullOrEmpty;
			}

			public MyGeneratedBlockLocation(MySlimBlock refBlock, MyCubeBlockDefinition blockDefinition, Vector3I position, Vector3I forward, Vector3I up, ushort? blockIdInCompound = null, MyGridInfo gridInfo = null)
			{
				RefBlock = refBlock;
				BlockDefinition = blockDefinition;
				Position = position;
				Orientation = new MyBlockOrientation(Base6Directions.GetDirection(ref forward), Base6Directions.GetDirection(ref up));
				BlockIdInCompound = blockIdInCompound;
				GridInfo = gridInfo;
				GeneratedBlockType = MyStringId.NullOrEmpty;
			}

			public static bool IsSameGeneratedBlockLocation(MyGeneratedBlockLocation blockLocAdded, MyGeneratedBlockLocation blockLocRemoved)
			{
				if (blockLocAdded.BlockDefinition == blockLocRemoved.BlockDefinition && blockLocAdded.Position == blockLocRemoved.Position)
				{
					return blockLocAdded.Orientation == blockLocRemoved.Orientation;
				}
				return false;
			}

			public static bool IsSameGeneratedBlockLocation(MyGeneratedBlockLocation blockLocAdded, MyGeneratedBlockLocation blockLocRemoved, MyStringId generatedBlockType)
			{
				if (blockLocAdded.BlockDefinition.GeneratedBlockType == generatedBlockType && blockLocAdded.Position == blockLocRemoved.Position)
				{
					return blockLocAdded.Orientation == blockLocRemoved.Orientation;
				}
				return false;
			}

			public override bool Equals(object ob)
			{
				if (ob is MyGeneratedBlockLocation)
				{
					MyGeneratedBlockLocation blockLocRemoved = (MyGeneratedBlockLocation)ob;
					return IsSameGeneratedBlockLocation(this, blockLocRemoved);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return BlockDefinition.Id.GetHashCode() + 17 * Position.GetHashCode() + 137 * Orientation.GetHashCode();
			}
		}

		protected static Vector3I[] Forwards = new Vector3I[4]
		{
			Vector3I.Forward,
			Vector3I.Right,
			Vector3I.Backward,
			Vector3I.Left
		};

		protected static Vector3I[] Rights = new Vector3I[4]
		{
			Vector3I.Right,
			Vector3I.Backward,
			Vector3I.Left,
			Vector3I.Forward
		};

		protected static readonly MyStringId BUILD_TYPE_WALL = MyStringId.GetOrCompute("wall");

		private static readonly List<Tuple<MyCubeGrid.MyBlockLocation, MySlimBlock>> m_tmpLocationsAndRefBlocks = new List<Tuple<MyCubeGrid.MyBlockLocation, MySlimBlock>>();

		private static readonly List<Vector3I> m_tmpLocations = new List<Vector3I>();

		private static readonly List<Tuple<Vector3I, ushort>> m_tmpLocationsAndIds = new List<Tuple<Vector3I, ushort>>();

		protected static readonly List<Vector3I> m_tmpPositionsAdd = new List<Vector3I>(32);

		protected static readonly List<Vector3I> m_tmpPositionsRemove = new List<Vector3I>(32);

		protected static readonly List<MyGeneratedBlockLocation> m_tmpLocationsRemove = new List<MyGeneratedBlockLocation>(32);

		protected MyCubeGrid m_grid;

		private bool m_enabled;

		private readonly HashSet<MyGeneratedBlockLocation> m_addLocations = new HashSet<MyGeneratedBlockLocation>();

		private readonly HashSet<MyGeneratedBlockLocation> m_removeLocations = new HashSet<MyGeneratedBlockLocation>();

		private readonly List<MyGeneratedBlockLocation> m_removeLocationsForGridSplits = new List<MyGeneratedBlockLocation>();

		private readonly HashSet<MyGridInfo> m_splitGridInfos = new HashSet<MyGridInfo>();

		public virtual bool Initialize(MyCubeGrid grid, MyCubeSize gridSizeEnum)
		{
			m_grid = grid;
			m_enabled = true;
			if (IsValid(gridSizeEnum))
			{
				m_grid.OnBlockAdded += Grid_OnBlockAdded;
				m_grid.OnBlockRemoved += Grid_OnBlockRemoved;
				m_grid.OnGridSplit += Grid_OnGridSplit;
				return true;
			}
			return false;
		}

		public virtual void Close()
		{
			m_grid.OnBlockAdded -= Grid_OnBlockAdded;
			m_grid.OnBlockRemoved -= Grid_OnBlockRemoved;
			m_grid.OnGridSplit -= Grid_OnGridSplit;
		}

		protected abstract bool IsValid(MyCubeSize gridSizeEnum);

		public virtual void EnableGenerator(bool enable)
		{
			m_enabled = enable;
		}

		public virtual void UpdateAfterGridSpawn(MySlimBlock block)
		{
			Grid_OnBlockAdded(block);
		}

		public virtual void BlockAddedToMergedGrid(MySlimBlock block)
		{
			Grid_OnBlockAdded(block);
		}

		public virtual void GenerateBlocks(MySlimBlock generatingBlock)
		{
			Grid_OnBlockAdded(generatingBlock);
		}

		public virtual void UpdateBeforeSimulation()
		{
			UpdateInternal();
		}

		public virtual void UpdateAfterSimulation()
		{
			UpdateInternal();
		}

		private void UpdateInternal()
		{
			if (m_addLocations.Count > 0)
			{
				m_addLocations.RemoveWhere((MyGeneratedBlockLocation loc) => loc.RefBlock != null && loc.RefBlock.CubeGrid != m_grid);
				m_addLocations.RemoveWhere((MyGeneratedBlockLocation loc) => !m_grid.CanAddCube(loc.Position, loc.Orientation, loc.BlockDefinition, ignoreSame: true));
				m_addLocations.RemoveWhere(delegate(MyGeneratedBlockLocation loc)
				{
					MyGeneratedBlockLocation? myGeneratedBlockLocation = null;
					foreach (MyGeneratedBlockLocation removeLocation in m_removeLocations)
					{
						if (MyGeneratedBlockLocation.IsSameGeneratedBlockLocation(loc, removeLocation))
						{
							myGeneratedBlockLocation = removeLocation;
							break;
						}
					}
					if (myGeneratedBlockLocation.HasValue)
					{
						m_removeLocations.Remove(myGeneratedBlockLocation.Value);
						return true;
					}
					return false;
				});
			}
			if (m_removeLocations.Count > 0)
			{
				RemoveBlocks();
			}
			if (m_addLocations.Count > 0)
			{
				AddBlocks();
			}
			m_addLocations.Clear();
			m_removeLocations.Clear();
			m_removeLocationsForGridSplits.Clear();
			m_splitGridInfos.Clear();
		}

		public abstract MySlimBlock GetGeneratingBlock(MySlimBlock generatedBlock);

		public abstract void OnAddedCube(MySlimBlock cube);

		public abstract void OnRemovedCube(MySlimBlock cube);

		protected bool CubeExistsOnPositions(List<Vector3I> positions)
		{
			foreach (Vector3I position in positions)
			{
				if (CubeExistsOnPosition(position))
				{
					return true;
				}
			}
			return false;
		}

		protected bool CubeExistsOnPosition(Vector3I pos)
		{
			MySlimBlock cubeBlock = m_grid.GetCubeBlock(pos);
			if (cubeBlock != null)
			{
				if (cubeBlock.FatBlock is MyCompoundCubeBlock)
				{
					foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
					{
						if (!block.BlockDefinition.IsGeneratedBlock)
						{
							return true;
						}
					}
				}
				else if (!cubeBlock.BlockDefinition.IsGeneratedBlock)
				{
					return true;
				}
			}
			return false;
		}

		protected bool CanPlaceBlock(Vector3I position, MyCubeBlockDefinition definition, Vector3I forward, Vector3I up)
		{
			MyBlockOrientation orientation = new MyBlockOrientation(Base6Directions.GetDirection(forward), Base6Directions.GetDirection(up));
			return m_grid.CanPlaceBlock(position, position, orientation, definition, 0uL);
		}

		protected static bool IsSameMaterial(MySlimBlock block1, MySlimBlock block2)
		{
			return block1.BlockDefinition.BuildMaterial == block2.BlockDefinition.BuildMaterial;
		}

		protected bool CanGenerateFromBlock(MySlimBlock cube)
		{
			if (cube == null)
			{
				return false;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = cube.FatBlock as MyCompoundCubeBlock;
			if (!m_enabled || !cube.CubeGrid.InScene || cube.BlockDefinition.IsGeneratedBlock || (myCompoundCubeBlock != null && myCompoundCubeBlock.GetBlocksCount() == 0) || (myCompoundCubeBlock == null && MySession.Static.SurvivalMode && cube.ComponentStack.BuildRatio < cube.BlockDefinition.BuildProgressToPlaceGeneratedBlocks) || (MyFakes.ENABLE_FRACTURE_COMPONENT && cube.FatBlock != null && cube.FatBlock.Components.Has<MyFractureComponentBase>()) || cube.FatBlock is MyFracturedBlock)
			{
				return false;
			}
			return true;
		}

		private void Grid_OnBlockAdded(MySlimBlock cube)
		{
			if (CanGenerateFromBlock(cube))
			{
				if (cube.FatBlock is MyCompoundCubeBlock)
				{
					foreach (MySlimBlock block in (cube.FatBlock as MyCompoundCubeBlock).GetBlocks())
					{
						if (CanGenerateFromBlock(block))
						{
							OnAddedCube(block);
						}
					}
				}
				else
				{
					OnAddedCube(cube);
				}
			}
		}

		private void Grid_OnBlockRemoved(MySlimBlock cube)
		{
			if (m_enabled && cube.CubeGrid.InScene && !cube.BlockDefinition.IsGeneratedBlock && (!(cube.FatBlock is MyCompoundCubeBlock) || ((MyCompoundCubeBlock)cube.FatBlock).GetBlocksCount() != 0))
			{
				OnRemovedCube(cube);
			}
		}

		private void Grid_OnGridSplit(MyCubeGrid originalGrid, MyCubeGrid newGrid)
		{
			ProcessChangedGrid(newGrid);
		}

		private void ProcessChangedGrid(MyCubeGrid newGrid)
		{
			Vector3I position = Vector3I.Round((m_grid.PositionComp.GetPosition() - newGrid.PositionComp.GetPosition()) / m_grid.GridSize);
			Vector3 vec = Vector3D.TransformNormal(m_grid.WorldMatrix.Forward, newGrid.PositionComp.WorldMatrixNormalizedInv);
			Vector3 vec2 = Vector3D.TransformNormal(m_grid.WorldMatrix.Up, newGrid.PositionComp.WorldMatrixNormalizedInv);
			Base6Directions.Direction closestDirection = Base6Directions.GetClosestDirection(vec);
			Base6Directions.Direction direction = Base6Directions.GetClosestDirection(vec2);
			if (direction == closestDirection)
			{
				direction = Base6Directions.GetPerpendicular(closestDirection);
			}
			MatrixI transform = new MatrixI(ref position, closestDirection, direction);
			MyGridInfo myGridInfo = new MyGridInfo();
			myGridInfo.Grid = newGrid;
			myGridInfo.Transform = transform;
			m_splitGridInfos.Add(myGridInfo);
			if (m_removeLocationsForGridSplits.Count > 0)
			{
				new List<int>();
				for (int i = 0; i < m_removeLocationsForGridSplits.Count; i++)
				{
					MyGeneratedBlockLocation location = m_removeLocationsForGridSplits[i];
					RemoveBlock(location, myGridInfo, location.GeneratedBlockType);
				}
			}
			List<MySlimBlock> newGridBlocks = new List<MySlimBlock>();
			m_addLocations.RemoveWhere(delegate(MyGeneratedBlockLocation loc)
			{
				if (loc.RefBlock != null && loc.RefBlock.CubeGrid == newGrid)
				{
					newGridBlocks.Add(loc.RefBlock);
					return true;
				}
				return false;
			});
			foreach (MySlimBlock newGridBlock in newGridBlocks)
			{
				newGridBlock.CubeGrid.AdditionalModelGenerators.ForEach(delegate(IMyBlockAdditionalModelGenerator g)
				{
					g.UpdateAfterGridSpawn(newGridBlock);
				});
			}
		}

		protected void AddGeneratedBlock(MySlimBlock refBlock, MyCubeBlockDefinition generatedBlockDefinition, Vector3I position, Vector3I forward, Vector3I up)
		{
			MyBlockOrientation orientation = new MyBlockOrientation(Base6Directions.GetDirection(ref forward), Base6Directions.GetDirection(ref up));
			if (generatedBlockDefinition.Size == Vector3I.One)
			{
				m_addLocations.Add(new MyGeneratedBlockLocation(refBlock, generatedBlockDefinition, position, orientation));
			}
		}

		protected void RemoveGeneratedBlock(MyStringId generatedBlockType, List<MyGeneratedBlockLocation> locations)
		{
			if (locations != null && locations.Count != 0)
			{
				foreach (MyGeneratedBlockLocation location in locations)
				{
					RemoveBlock(location, null, generatedBlockType);
					MyGeneratedBlockLocation item = location;
					item.GeneratedBlockType = generatedBlockType;
					m_removeLocationsForGridSplits.Add(item);
					if (m_addLocations.Count > 0)
					{
						m_addLocations.RemoveWhere((MyGeneratedBlockLocation loc) => MyGeneratedBlockLocation.IsSameGeneratedBlockLocation(loc, location, generatedBlockType));
					}
				}
			}
		}

		private bool RemoveBlock(MyGeneratedBlockLocation location, MyGridInfo gridInfo, MyStringId generatedBlockType)
		{
			MySlimBlock mySlimBlock = null;
			if (gridInfo != null)
			{
				Vector3I pos = Vector3I.Transform(location.Position, gridInfo.Transform);
				mySlimBlock = gridInfo.Grid.GetCubeBlock(pos);
			}
			else
			{
				mySlimBlock = m_grid.GetCubeBlock(location.Position);
			}
			if (mySlimBlock != null)
			{
				if (mySlimBlock.FatBlock is MyCompoundCubeBlock)
				{
					MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
					ListReader<MySlimBlock> blocks = myCompoundCubeBlock.GetBlocks();
					for (int i = 0; i < blocks.Count; i++)
					{
						MySlimBlock mySlimBlock2 = blocks[i];
						if (mySlimBlock2.BlockDefinition.IsGeneratedBlock && mySlimBlock2.BlockDefinition.GeneratedBlockType == generatedBlockType && mySlimBlock2.Orientation == location.Orientation)
						{
							ushort? blockId = myCompoundCubeBlock.GetBlockId(mySlimBlock2);
							m_removeLocations.Add(new MyGeneratedBlockLocation(null, mySlimBlock2.BlockDefinition, mySlimBlock2.Position, mySlimBlock2.Orientation, blockId, gridInfo));
							return true;
						}
					}
				}
				else if (mySlimBlock.BlockDefinition.IsGeneratedBlock && mySlimBlock.BlockDefinition.GeneratedBlockType == generatedBlockType && mySlimBlock.Orientation == location.Orientation)
				{
					m_removeLocations.Add(new MyGeneratedBlockLocation(null, mySlimBlock.BlockDefinition, mySlimBlock.Position, mySlimBlock.Orientation, null, gridInfo));
					return true;
				}
			}
			return false;
		}

		private void AddBlocks()
		{
			foreach (MyGeneratedBlockLocation addLocation in m_addLocations)
			{
				MyBlockOrientation orientation = addLocation.Orientation;
				orientation.GetQuaternion(out Quaternion result);
				MyCubeGrid.MyBlockLocation item = new MyCubeGrid.MyBlockLocation(addLocation.BlockDefinition.Id, addLocation.Position, addLocation.Position, addLocation.Position, result, MyEntityIdentifier.AllocateId(), MySession.Static.LocalPlayerId);
				m_tmpLocationsAndRefBlocks.Add(new Tuple<MyCubeGrid.MyBlockLocation, MySlimBlock>(item, addLocation.RefBlock));
			}
			foreach (Tuple<MyCubeGrid.MyBlockLocation, MySlimBlock> tmpLocationsAndRefBlock in m_tmpLocationsAndRefBlocks)
			{
				MySlimBlock mySlimBlock = m_grid.BuildGeneratedBlock(tmpLocationsAndRefBlock.Item1, Vector3I.Zero, MyStringHash.NullOrEmpty);
				if (mySlimBlock != null)
				{
					MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
					if (myCompoundCubeBlock != null)
					{
						foreach (MySlimBlock block in myCompoundCubeBlock.GetBlocks())
						{
							tmpLocationsAndRefBlock.Item1.Orientation.GetQuaternion(out Quaternion result2);
							MyBlockOrientation orientation2 = new MyBlockOrientation(ref result2);
							if (block.Orientation == orientation2 && block.BlockDefinition.Id == tmpLocationsAndRefBlock.Item1.BlockDefinition)
							{
								mySlimBlock = block;
								break;
							}
						}
					}
					MySlimBlock item2 = tmpLocationsAndRefBlock.Item2;
					if (mySlimBlock != null && mySlimBlock.BlockDefinition.IsGeneratedBlock && item2 != null)
					{
						mySlimBlock.SetGeneratedBlockIntegrity(item2);
					}
				}
			}
			m_tmpLocationsAndRefBlocks.Clear();
		}

		private void RemoveBlocks(bool removeLocalBlocks = true)
		{
			if (removeLocalBlocks)
			{
				foreach (MyGeneratedBlockLocation removeLocation in m_removeLocations)
				{
					if (removeLocation.GridInfo == null)
					{
						if (!removeLocation.BlockIdInCompound.HasValue)
						{
							m_tmpLocations.Add(removeLocation.Position);
						}
						else
						{
							m_tmpLocationsAndIds.Add(new Tuple<Vector3I, ushort>(removeLocation.Position, removeLocation.BlockIdInCompound.Value));
						}
					}
				}
				if (m_tmpLocations.Count > 0)
				{
					m_grid.RazeGeneratedBlocks(m_tmpLocations);
				}
				if (m_tmpLocationsAndIds.Count > 0)
				{
					m_grid.RazeGeneratedBlocksInCompoundBlock(m_tmpLocationsAndIds);
				}
			}
			foreach (MyGridInfo splitGridInfo in m_splitGridInfos)
			{
				m_tmpLocations.Clear();
				m_tmpLocationsAndIds.Clear();
				foreach (MyGeneratedBlockLocation removeLocation2 in m_removeLocations)
				{
					if (removeLocation2.GridInfo == splitGridInfo)
					{
						if (!removeLocation2.BlockIdInCompound.HasValue)
						{
							m_tmpLocations.Add(removeLocation2.Position);
						}
						else
						{
							m_tmpLocationsAndIds.Add(new Tuple<Vector3I, ushort>(removeLocation2.Position, removeLocation2.BlockIdInCompound.Value));
						}
					}
				}
				if (m_tmpLocations.Count > 0)
				{
					splitGridInfo.Grid.RazeGeneratedBlocks(m_tmpLocations);
				}
				if (m_tmpLocationsAndIds.Count > 0)
				{
					splitGridInfo.Grid.RazeGeneratedBlocksInCompoundBlock(m_tmpLocationsAndIds);
				}
			}
			m_tmpLocations.Clear();
			m_tmpLocationsAndIds.Clear();
		}

		protected bool GeneratedBlockExists(Vector3I pos, MyBlockOrientation orientation, MyCubeBlockDefinition definition)
		{
			MySlimBlock cubeBlock = m_grid.GetCubeBlock(pos);
			if (cubeBlock == null)
			{
				return false;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = cubeBlock.FatBlock as MyCompoundCubeBlock;
			if (MyFakes.ENABLE_COMPOUND_BLOCKS && myCompoundCubeBlock != null)
			{
				foreach (MySlimBlock block in myCompoundCubeBlock.GetBlocks())
				{
					if (block.BlockDefinition.Id.SubtypeId == definition.Id.SubtypeId && block.Orientation == orientation)
					{
						return true;
					}
				}
				return false;
			}
			if (cubeBlock.BlockDefinition.Id.SubtypeId == definition.Id.SubtypeId)
			{
				return cubeBlock.Orientation == orientation;
			}
			return false;
		}
	}
}
