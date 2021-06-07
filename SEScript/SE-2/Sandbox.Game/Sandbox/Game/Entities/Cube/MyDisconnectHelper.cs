using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyDisconnectHelper
	{
		[Serializable]
		public struct Group
		{
			protected class Sandbox_Game_Entities_Cube_MyDisconnectHelper_003C_003EGroup_003C_003EFirstBlockIndex_003C_003EAccessor : IMemberAccessor<Group, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Group owner, in int value)
				{
					owner.FirstBlockIndex = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Group owner, out int value)
				{
					value = owner.FirstBlockIndex;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MyDisconnectHelper_003C_003EGroup_003C_003EBlockCount_003C_003EAccessor : IMemberAccessor<Group, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Group owner, in int value)
				{
					owner.BlockCount = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Group owner, out int value)
				{
					value = owner.BlockCount;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MyDisconnectHelper_003C_003EGroup_003C_003EIsValid_003C_003EAccessor : IMemberAccessor<Group, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Group owner, in bool value)
				{
					owner.IsValid = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Group owner, out bool value)
				{
					value = owner.IsValid;
				}
			}

			protected class Sandbox_Game_Entities_Cube_MyDisconnectHelper_003C_003EGroup_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<Group, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Group owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Group owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			public int FirstBlockIndex;

			public int BlockCount;

			public bool IsValid;

			public long EntityId;
		}

		private HashSet<MySlimBlock> m_disconnectHelper = new HashSet<MySlimBlock>();

		private Queue<MySlimBlock> m_neighborSearchBaseStack = new Queue<MySlimBlock>();

		private List<MySlimBlock> m_sortedBlocks = new List<MySlimBlock>();

		private List<Group> m_groups = new List<Group>();

		private Group m_largestGroupWithPhysics;

		private List<MySlimBlock> m_tmpBlocks = new List<MySlimBlock>();

		public bool Disconnect(MyCubeGrid grid, MyCubeGrid.MyTestDisconnectsReason reason, MySlimBlock testBlock = null, bool testDisconnect = false)
		{
			m_largestGroupWithPhysics = default(Group);
			m_groups.Clear();
			m_sortedBlocks.Clear();
			m_disconnectHelper.Clear();
			foreach (MySlimBlock block in grid.GetBlocks())
			{
				if (block != testBlock)
				{
					m_disconnectHelper.Add(block);
				}
			}
			while (m_disconnectHelper.Count > 0)
			{
				Group group = default(Group);
				group.FirstBlockIndex = m_sortedBlocks.Count;
				AddNeighbours(m_disconnectHelper.FirstElement(), out group.IsValid, testBlock);
				group.BlockCount = m_sortedBlocks.Count - group.FirstBlockIndex;
				if (group.IsValid && group.BlockCount > m_largestGroupWithPhysics.BlockCount)
				{
					if (m_largestGroupWithPhysics.BlockCount > 0)
					{
						int num = 0;
						for (num = 0; num < m_groups.Count; num++)
						{
							if (m_groups[num].FirstBlockIndex > m_largestGroupWithPhysics.FirstBlockIndex)
							{
								m_groups.Insert(num, m_largestGroupWithPhysics);
								break;
							}
						}
						if (num == m_groups.Count)
						{
							m_groups.Add(m_largestGroupWithPhysics);
						}
					}
					m_largestGroupWithPhysics = group;
				}
				else
				{
					m_groups.Add(group);
				}
			}
			m_sortedBlocks.RemoveRange(m_largestGroupWithPhysics.FirstBlockIndex, m_largestGroupWithPhysics.BlockCount);
			for (int i = 0; i < m_groups.Count; i++)
			{
				Group value = m_groups[i];
				if (value.FirstBlockIndex > m_largestGroupWithPhysics.FirstBlockIndex)
				{
					value.FirstBlockIndex -= m_largestGroupWithPhysics.BlockCount;
					m_groups[i] = value;
				}
			}
			if (m_groups.Count > 0)
			{
				if (testDisconnect)
				{
					m_groups.Clear();
					m_sortedBlocks.Clear();
					m_disconnectHelper.Clear();
					return true;
				}
				MyCubeGrid.CreateSplits(grid, m_sortedBlocks, m_groups, reason);
			}
			else if (!MySession.Static.Settings.StationVoxelSupport && grid.IsStatic)
			{
				grid.TestDynamic = MyCubeGrid.MyTestDynamicReason.GridSplit;
			}
			m_groups.Clear();
			m_sortedBlocks.Clear();
			m_disconnectHelper.Clear();
			return false;
		}

		private void AddNeighbours(MySlimBlock firstBlock, out bool anyWithPhysics, MySlimBlock testBlock)
		{
			anyWithPhysics = false;
			if (m_disconnectHelper.Remove(firstBlock))
			{
				anyWithPhysics |= firstBlock.BlockDefinition.HasPhysics;
				m_sortedBlocks.Add(firstBlock);
				m_neighborSearchBaseStack.Enqueue(firstBlock);
			}
			while (m_neighborSearchBaseStack.Count > 0)
			{
				foreach (MySlimBlock neighbour in m_neighborSearchBaseStack.Dequeue().Neighbours)
				{
					if (neighbour != testBlock && m_disconnectHelper.Remove(neighbour))
					{
						anyWithPhysics |= neighbour.BlockDefinition.HasPhysics;
						m_sortedBlocks.Add(neighbour);
						m_neighborSearchBaseStack.Enqueue(neighbour);
					}
				}
			}
		}

		public static bool IsDestroyedInVoxels(MySlimBlock block)
		{
			if (block == null || block.CubeGrid.IsStatic)
			{
				return false;
			}
			MyCubeGrid cubeGrid = block.CubeGrid;
			Vector3D vector3D = Vector3D.Transform((block.Max + block.Min) * 0.5f * cubeGrid.GridSize, cubeGrid.WorldMatrix);
			Vector3D hintPosition = vector3D - cubeGrid.Physics.LinearVelocity * 1.5f;
			Vector3D lastOutsidePos;
			return MyEntities.IsInsideVoxel(vector3D, hintPosition, out lastOutsidePos);
		}

		public bool TryDisconnect(MySlimBlock testBlock)
		{
			return Disconnect(testBlock.CubeGrid, MyCubeGrid.MyTestDisconnectsReason.NoReason, testBlock, testDisconnect: true);
		}
	}
}
