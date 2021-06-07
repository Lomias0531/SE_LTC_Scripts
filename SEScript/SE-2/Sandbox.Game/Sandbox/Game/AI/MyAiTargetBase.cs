using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.AI
{
	public class MyAiTargetBase
	{
		private const int UNREACHABLE_ENTITY_TIMEOUT = 20000;

		private const int UNREACHABLE_BLOCK_TIMEOUT = 60000;

		private const int UNREACHABLE_CHARACTER_TIMEOUT = 20000;

		protected MyAiTargetEnum m_currentTarget;

		protected IMyEntityBot m_user;

		protected MyAgentBot m_bot;

		protected MyEntity m_targetEntity;

		protected Vector3I m_targetCube = Vector3I.Zero;

		protected Vector3D m_targetPosition = Vector3D.Zero;

		protected Vector3I m_targetInVoxelCoord = Vector3I.Zero;

		protected ushort? m_compoundId;

		protected int m_targetTreeId;

		protected Dictionary<MyEntity, int> m_unreachableEntities = new Dictionary<MyEntity, int>();

		protected Dictionary<Tuple<MyEntity, int>, int> m_unreachableTrees = new Dictionary<Tuple<MyEntity, int>, int>();

		protected static List<MyEntity> m_tmpEntities;

		protected static List<Tuple<MyEntity, int>> m_tmpTrees;

		public MyAiTargetEnum TargetType => m_currentTarget;

		public MyCubeGrid TargetGrid => m_targetEntity as MyCubeGrid;

		public MyEntity TargetEntity => m_targetEntity;

		public Vector3D TargetPosition
		{
			get
			{
				switch (m_currentTarget)
				{
				case MyAiTargetEnum.NO_TARGET:
					return Vector3D.Zero;
				case MyAiTargetEnum.POSITION:
					return m_targetPosition;
				case MyAiTargetEnum.VOXEL:
					return m_targetPosition;
				case MyAiTargetEnum.CUBE:
				case MyAiTargetEnum.COMPOUND_BLOCK:
				{
					MyCubeGrid myCubeGrid = m_targetEntity as MyCubeGrid;
					if (myCubeGrid == null)
					{
						return Vector3D.Zero;
					}
					MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(m_targetCube);
					if (cubeBlock == null)
					{
						return Vector3D.Zero;
					}
					return myCubeGrid.GridIntegerToWorld(cubeBlock.Position);
				}
				case MyAiTargetEnum.ENVIRONMENT_ITEM:
					return m_targetEntity.PositionComp.GetPosition();
				case MyAiTargetEnum.GRID:
				case MyAiTargetEnum.CHARACTER:
				case MyAiTargetEnum.ENTITY:
					return m_targetEntity.PositionComp.GetPosition();
				default:
					return Vector3D.Zero;
				}
			}
		}

		public Vector3D TargetCubeWorldPosition
		{
			get
			{
				MySlimBlock cubeBlock = GetCubeBlock();
				if (cubeBlock != null && cubeBlock.FatBlock != null)
				{
					return cubeBlock.FatBlock.PositionComp.WorldAABB.Center;
				}
				return TargetGrid.GridIntegerToWorld(m_targetCube);
			}
		}

		public bool HasGotoFailed
		{
			get;
			set;
		}

		public bool HasTarget()
		{
			return m_currentTarget != MyAiTargetEnum.NO_TARGET;
		}

		private void Clear()
		{
			m_currentTarget = MyAiTargetEnum.NO_TARGET;
			m_targetEntity = null;
			m_targetCube = Vector3I.Zero;
			m_targetPosition = Vector3D.Zero;
			m_targetInVoxelCoord = Vector3I.Zero;
			m_compoundId = null;
			m_targetTreeId = 0;
		}

		private void SetMTargetPosition(Vector3D pos)
		{
			m_targetPosition = pos;
		}

		public bool IsTargetGridOrBlock(MyAiTargetEnum type)
		{
			if (type != MyAiTargetEnum.CUBE)
			{
				return type == MyAiTargetEnum.GRID;
			}
			return true;
		}

		public virtual bool IsMemoryTargetValid(MyBBMemoryTarget targetMemory)
		{
			if (targetMemory == null)
			{
				return false;
			}
			switch (targetMemory.TargetType)
			{
			case MyAiTargetEnum.CHARACTER:
			{
				MyCharacter entity3 = null;
				if (MyEntities.TryGetEntityById(targetMemory.EntityId.Value, out entity3))
				{
					return IsEntityReachable(entity3);
				}
				return false;
			}
			case MyAiTargetEnum.CUBE:
			case MyAiTargetEnum.COMPOUND_BLOCK:
			{
				MyCubeGrid entity2 = null;
				if (MyEntities.TryGetEntityById(targetMemory.EntityId.Value, out entity2))
				{
					MySlimBlock cubeBlock = entity2.GetCubeBlock(targetMemory.BlockPosition);
					if (cubeBlock == null)
					{
						return false;
					}
					if (cubeBlock.FatBlock != null)
					{
						return IsEntityReachable(cubeBlock.FatBlock);
					}
					return IsEntityReachable(entity2);
				}
				return false;
			}
			case MyAiTargetEnum.ENVIRONMENT_ITEM:
			case MyAiTargetEnum.VOXEL:
				return true;
			case MyAiTargetEnum.GRID:
			case MyAiTargetEnum.ENTITY:
			{
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(targetMemory.EntityId.Value, out entity))
				{
					return IsEntityReachable(entity);
				}
				return false;
			}
			default:
				return false;
			}
		}

		public Vector3D? GetMemoryTargetPosition(MyBBMemoryTarget targetMemory)
		{
			if (targetMemory == null)
			{
				return null;
			}
			switch (targetMemory.TargetType)
			{
			case MyAiTargetEnum.GRID:
			case MyAiTargetEnum.CHARACTER:
			case MyAiTargetEnum.ENTITY:
			{
				MyCharacter entity2 = null;
				if (MyEntities.TryGetEntityById(targetMemory.EntityId.Value, out entity2))
				{
					return entity2.PositionComp.GetPosition();
				}
				return null;
			}
			case MyAiTargetEnum.CUBE:
			case MyAiTargetEnum.COMPOUND_BLOCK:
			{
				MyCubeGrid entity = null;
				if (MyEntities.TryGetEntityById(targetMemory.EntityId.Value, out entity) && entity.CubeExists(targetMemory.BlockPosition))
				{
					return entity.GridIntegerToWorld(targetMemory.BlockPosition);
				}
				return null;
			}
			case MyAiTargetEnum.POSITION:
			case MyAiTargetEnum.ENVIRONMENT_ITEM:
			case MyAiTargetEnum.VOXEL:
				return m_targetPosition;
			default:
				return null;
			}
		}

		public virtual bool IsTargetValid()
		{
			switch (m_currentTarget)
			{
			case MyAiTargetEnum.CHARACTER:
			{
				MyCharacter myCharacter = m_targetEntity as MyCharacter;
				if (myCharacter != null)
				{
					return IsEntityReachable(myCharacter);
				}
				return false;
			}
			case MyAiTargetEnum.CUBE:
			case MyAiTargetEnum.COMPOUND_BLOCK:
			{
				MyCubeGrid myCubeGrid = m_targetEntity as MyCubeGrid;
				if (myCubeGrid == null)
				{
					return false;
				}
				MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(m_targetCube);
				if (cubeBlock == null)
				{
					return false;
				}
				if (cubeBlock.FatBlock != null)
				{
					return IsEntityReachable(cubeBlock.FatBlock);
				}
				return IsEntityReachable(myCubeGrid);
			}
			case MyAiTargetEnum.ENVIRONMENT_ITEM:
			case MyAiTargetEnum.VOXEL:
				return true;
			case MyAiTargetEnum.GRID:
			case MyAiTargetEnum.ENTITY:
				return IsEntityReachable(m_targetEntity);
			default:
				return false;
			}
		}

		public MyAiTargetBase(IMyEntityBot bot)
		{
			m_user = bot;
			m_bot = (bot as MyAgentBot);
			m_currentTarget = MyAiTargetEnum.NO_TARGET;
			MyAiTargetManager.AddAiTarget(this);
		}

		public virtual void Init(MyObjectBuilder_AiTarget builder)
		{
			m_currentTarget = builder.CurrentTarget;
			m_targetEntity = null;
			if (builder.EntityId.HasValue)
			{
				if (!MyEntities.TryGetEntityById(builder.EntityId.Value, out m_targetEntity))
				{
					m_currentTarget = MyAiTargetEnum.NO_TARGET;
				}
			}
			else
			{
				m_currentTarget = MyAiTargetEnum.NO_TARGET;
			}
			m_targetCube = builder.TargetCube;
			SetMTargetPosition(builder.TargetPosition);
			m_compoundId = builder.CompoundId;
			if (builder.UnreachableEntities != null)
			{
				foreach (MyObjectBuilder_AiTarget.UnreachableEntitiesData unreachableEntity in builder.UnreachableEntities)
				{
					MyEntity entity = null;
					if (MyEntities.TryGetEntityById(unreachableEntity.UnreachableEntityId, out entity))
					{
						m_unreachableEntities.Add(entity, MySandboxGame.TotalGamePlayTimeInMilliseconds + unreachableEntity.Timeout);
					}
				}
			}
		}

		public virtual MyObjectBuilder_AiTarget GetObjectBuilder()
		{
			MyObjectBuilder_AiTarget myObjectBuilder_AiTarget = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_AiTarget>();
			myObjectBuilder_AiTarget.EntityId = ((m_targetEntity != null) ? new long?(m_targetEntity.EntityId) : null);
			myObjectBuilder_AiTarget.CurrentTarget = m_currentTarget;
			myObjectBuilder_AiTarget.TargetCube = m_targetCube;
			myObjectBuilder_AiTarget.TargetPosition = m_targetPosition;
			myObjectBuilder_AiTarget.CompoundId = m_compoundId;
			myObjectBuilder_AiTarget.UnreachableEntities = new List<MyObjectBuilder_AiTarget.UnreachableEntitiesData>();
			foreach (KeyValuePair<MyEntity, int> unreachableEntity in m_unreachableEntities)
			{
				MyObjectBuilder_AiTarget.UnreachableEntitiesData unreachableEntitiesData = new MyObjectBuilder_AiTarget.UnreachableEntitiesData();
				unreachableEntitiesData.UnreachableEntityId = unreachableEntity.Key.EntityId;
				unreachableEntitiesData.Timeout = unreachableEntity.Value - MySandboxGame.TotalGamePlayTimeInMilliseconds;
				myObjectBuilder_AiTarget.UnreachableEntities.Add(unreachableEntitiesData);
			}
			return myObjectBuilder_AiTarget;
		}

		public virtual void UnsetTarget()
		{
			switch (m_currentTarget)
			{
			case MyAiTargetEnum.NO_TARGET:
			case MyAiTargetEnum.GRID:
			case MyAiTargetEnum.CUBE:
			case MyAiTargetEnum.CHARACTER:
			case MyAiTargetEnum.ENTITY:
			case MyAiTargetEnum.ENVIRONMENT_ITEM:
			case MyAiTargetEnum.VOXEL:
				if (m_targetEntity != null)
				{
					UnsetTargetEntity();
				}
				break;
			}
			Clear();
		}

		public virtual void DebugDraw()
		{
		}

		public virtual void DrawLineToTarget(Vector3D from)
		{
		}

		public virtual void Cleanup()
		{
			MyAiTargetManager.RemoveAiTarget(this);
		}

		public virtual void Update()
		{
			using (MyUtils.ReuseCollection(ref m_tmpEntities))
			{
				foreach (KeyValuePair<MyEntity, int> unreachableEntity in m_unreachableEntities)
				{
					if (unreachableEntity.Value - MySandboxGame.TotalGamePlayTimeInMilliseconds < 0)
					{
						m_tmpEntities.Add(unreachableEntity.Key);
					}
				}
				foreach (MyEntity tmpEntity in m_tmpEntities)
				{
					RemoveUnreachableEntity(tmpEntity);
				}
			}
			using (MyUtils.ReuseCollection(ref m_tmpTrees))
			{
				foreach (KeyValuePair<Tuple<MyEntity, int>, int> unreachableTree in m_unreachableTrees)
				{
					if (unreachableTree.Value - MySandboxGame.TotalGamePlayTimeInMilliseconds < 0)
					{
						m_tmpTrees.Add(unreachableTree.Key);
					}
				}
				foreach (Tuple<MyEntity, int> tmpTree in m_tmpTrees)
				{
					RemoveUnreachableTree(tmpTree);
				}
			}
		}

		private void AddUnreachableEntity(MyEntity entity, int timeout)
		{
			m_unreachableEntities[entity] = MySandboxGame.TotalGamePlayTimeInMilliseconds + timeout;
			entity.OnClosing -= RemoveUnreachableEntity;
			entity.OnClosing += RemoveUnreachableEntity;
		}

		private void AddUnreachableTree(MyEntity entity, int treeId, int timeout)
		{
			m_unreachableTrees[new Tuple<MyEntity, int>(entity, treeId)] = MySandboxGame.TotalGamePlayTimeInMilliseconds + timeout;
			entity.OnClosing -= RemoveUnreachableTrees;
			entity.OnClosing += RemoveUnreachableTrees;
		}

		public bool IsEntityReachable(MyEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			bool flag = true;
			if (entity.Parent != null)
			{
				flag &= IsEntityReachable(entity.Parent);
			}
			if (flag)
			{
				return !m_unreachableEntities.ContainsKey(entity);
			}
			return false;
		}

		public bool IsTreeReachable(MyEntity entity, int treeId)
		{
			if (entity == null)
			{
				return false;
			}
			bool flag = true;
			if (entity.Parent != null)
			{
				flag &= IsEntityReachable(entity.Parent);
			}
			if (flag)
			{
				return !m_unreachableTrees.ContainsKey(new Tuple<MyEntity, int>(entity, treeId));
			}
			return false;
		}

		private void RemoveUnreachableEntity(MyEntity entity)
		{
			entity.OnClosing -= RemoveUnreachableEntity;
			m_unreachableEntities.Remove(entity);
		}

		private void RemoveUnreachableTree(Tuple<MyEntity, int> tree)
		{
			m_unreachableTrees.Remove(tree);
		}

		private void RemoveUnreachableTrees(MyEntity entity)
		{
			entity.OnClosing -= RemoveUnreachableTrees;
			using (MyUtils.ReuseCollection(ref m_tmpTrees))
			{
				foreach (Tuple<MyEntity, int> key in m_unreachableTrees.Keys)
				{
					if (key.Item1 == entity)
					{
						m_tmpTrees.Add(key);
					}
				}
				foreach (Tuple<MyEntity, int> tmpTree in m_tmpTrees)
				{
					RemoveUnreachableTree(tmpTree);
				}
			}
		}

		public bool PositionIsNearTarget(Vector3D position, float radius)
		{
			if (!HasTarget())
			{
				return false;
			}
			GetTargetPosition(position, out Vector3D targetPosition, out float radius2);
			return Vector3D.Distance(position, targetPosition) <= (double)(radius + radius2);
		}

		public void ClearUnreachableEntities()
		{
			m_unreachableEntities.Clear();
		}

		public void GotoTarget()
		{
			if (HasTarget())
			{
				if (m_currentTarget == MyAiTargetEnum.POSITION || m_currentTarget == MyAiTargetEnum.VOXEL)
				{
					m_bot.Navigation.Goto(m_targetPosition, 0f, m_targetEntity);
					return;
				}
				GetTargetPosition(m_bot.Navigation.PositionAndOrientation.Translation, out Vector3D targetPosition, out float radius);
				m_bot.Navigation.Goto(targetPosition, radius, m_targetEntity);
			}
		}

		public void GotoTargetNoPath(float radius, bool resetStuckDetection = true)
		{
			if (HasTarget())
			{
				if (m_currentTarget == MyAiTargetEnum.POSITION || m_currentTarget == MyAiTargetEnum.VOXEL)
				{
					m_bot.Navigation.GotoNoPath(m_targetPosition, radius);
					return;
				}
				GetTargetPosition(m_bot.Navigation.PositionAndOrientation.Translation, out Vector3D targetPosition, out float radius2);
				m_bot.Navigation.GotoNoPath(targetPosition, radius + radius2, null, resetStuckDetection);
			}
		}

		public void GetTargetPosition(Vector3D startingPosition, out Vector3D targetPosition, out float radius)
		{
			targetPosition = default(Vector3D);
			radius = 0f;
			if (!HasTarget())
			{
				return;
			}
			if (m_currentTarget == MyAiTargetEnum.POSITION)
			{
				targetPosition = m_targetPosition;
				return;
			}
			Vector3D vector3D = m_targetEntity.PositionComp.GetPosition();
			radius = 0.75f;
			if (m_currentTarget == MyAiTargetEnum.CUBE)
			{
				Vector3D localCubeProjectedPosition = GetLocalCubeProjectedPosition(ref startingPosition);
				radius = (float)localCubeProjectedPosition.Length() * 0.3f;
				vector3D = TargetCubeWorldPosition + localCubeProjectedPosition;
			}
			else if (m_currentTarget == MyAiTargetEnum.CHARACTER)
			{
				radius = 0.65f;
				vector3D = (m_targetEntity as MyCharacter).PositionComp.WorldVolume.Center;
			}
			else if (m_currentTarget == MyAiTargetEnum.ENVIRONMENT_ITEM)
			{
				vector3D = m_targetPosition;
				radius = 0.75f;
			}
			else if (m_currentTarget == MyAiTargetEnum.VOXEL)
			{
				vector3D = m_targetPosition;
			}
			else if (m_currentTarget == MyAiTargetEnum.ENTITY)
			{
				if (m_targetPosition != Vector3D.Zero && m_targetEntity is MyFracturedPiece)
				{
					vector3D = m_targetPosition;
				}
				radius = m_targetEntity.PositionComp.LocalAABB.HalfExtents.Length();
			}
			targetPosition = vector3D;
		}

		public Vector3D GetTargetPosition(Vector3D startingPosition)
		{
			GetTargetPosition(startingPosition, out Vector3D targetPosition, out float _);
			return targetPosition;
		}

		public void AimAtTarget()
		{
			if (HasTarget())
			{
				if (m_currentTarget == MyAiTargetEnum.POSITION || m_currentTarget == MyAiTargetEnum.VOXEL)
				{
					m_bot.Navigation.AimAt(null, m_targetPosition);
					return;
				}
				SetMTargetPosition(GetAimAtPosition(m_bot.Navigation.AimingPositionAndOrientation.Translation));
				m_bot.Navigation.AimAt(m_targetEntity, m_targetPosition);
			}
		}

		public void GotoFailed()
		{
			HasGotoFailed = true;
			if (m_currentTarget == MyAiTargetEnum.CHARACTER)
			{
				AddUnreachableEntity(m_targetEntity, 20000);
			}
			else if (m_currentTarget == MyAiTargetEnum.CUBE)
			{
				_ = m_targetEntity;
				MySlimBlock cubeBlock = GetCubeBlock();
				if (cubeBlock != null && cubeBlock.FatBlock != null)
				{
					AddUnreachableEntity(cubeBlock.FatBlock, 60000);
				}
			}
			else if (m_targetEntity != null && m_targetEntity is MyTrees)
			{
				AddUnreachableTree(m_targetEntity, m_targetTreeId, 20000);
			}
			else if (m_targetEntity != null && m_currentTarget != MyAiTargetEnum.VOXEL)
			{
				AddUnreachableEntity(m_targetEntity, 20000);
			}
			UnsetTarget();
		}

		public virtual bool SetTargetFromMemory(MyBBMemoryTarget memoryTarget)
		{
			if (memoryTarget.TargetType == MyAiTargetEnum.POSITION)
			{
				if (!memoryTarget.Position.HasValue)
				{
					return false;
				}
				SetTargetPosition(memoryTarget.Position.Value);
				return true;
			}
			if (memoryTarget.TargetType == MyAiTargetEnum.ENVIRONMENT_ITEM)
			{
				if (!memoryTarget.TreeId.HasValue)
				{
					return false;
				}
				MyEnvironmentItems.ItemInfo targetTree = default(MyEnvironmentItems.ItemInfo);
				targetTree.LocalId = memoryTarget.TreeId.Value;
				targetTree.Transform.Position = memoryTarget.Position.Value;
				SetTargetTree(ref targetTree, memoryTarget.EntityId.Value);
				return true;
			}
			if (memoryTarget.TargetType != 0)
			{
				if (!memoryTarget.EntityId.HasValue)
				{
					return false;
				}
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(memoryTarget.EntityId.Value, out entity))
				{
					if (memoryTarget.TargetType == MyAiTargetEnum.CUBE || memoryTarget.TargetType == MyAiTargetEnum.COMPOUND_BLOCK)
					{
						MySlimBlock mySlimBlock = (entity as MyCubeGrid).GetCubeBlock(memoryTarget.BlockPosition);
						if (mySlimBlock == null)
						{
							return false;
						}
						if (memoryTarget.TargetType == MyAiTargetEnum.COMPOUND_BLOCK)
						{
							MySlimBlock block = (mySlimBlock.FatBlock as MyCompoundCubeBlock).GetBlock(memoryTarget.CompoundId.Value);
							if (block == null)
							{
								return false;
							}
							mySlimBlock = block;
							m_compoundId = memoryTarget.CompoundId;
						}
						SetTargetBlock(mySlimBlock);
					}
					else if (memoryTarget.TargetType == MyAiTargetEnum.ENTITY)
					{
						if (memoryTarget.Position.HasValue && entity is MyFracturedPiece)
						{
							SetMTargetPosition(memoryTarget.Position.Value);
						}
						else
						{
							SetMTargetPosition(entity.PositionComp.GetPosition());
						}
						SetTargetEntity(entity);
						m_targetEntity = entity;
					}
					else if (memoryTarget.TargetType == MyAiTargetEnum.VOXEL)
					{
						MyVoxelMap myVoxelMap = entity as MyVoxelMap;
						if (!memoryTarget.Position.HasValue)
						{
							return false;
						}
						if (myVoxelMap == null)
						{
							return false;
						}
						SetTargetVoxel(memoryTarget.Position.Value, myVoxelMap);
						m_targetEntity = myVoxelMap;
					}
					else
					{
						SetTargetEntity(entity);
					}
					return true;
				}
				UnsetTarget();
				return false;
			}
			if (memoryTarget.TargetType == MyAiTargetEnum.NO_TARGET)
			{
				UnsetTarget();
				return true;
			}
			UnsetTarget();
			return false;
		}

		protected virtual void SetTargetEntity(MyEntity entity)
		{
			if (entity is MyCubeBlock)
			{
				SetTargetBlock((entity as MyCubeBlock).SlimBlock);
				return;
			}
			if (m_targetEntity != null)
			{
				UnsetTargetEntity();
			}
			m_targetEntity = entity;
			if (entity is MyCubeGrid)
			{
				(entity as MyCubeGrid).OnBlockRemoved += BlockRemoved;
				m_currentTarget = MyAiTargetEnum.GRID;
			}
			else if (entity is MyCharacter)
			{
				m_currentTarget = MyAiTargetEnum.CHARACTER;
			}
			else if (entity is MyVoxelBase)
			{
				m_currentTarget = MyAiTargetEnum.VOXEL;
			}
			else if (entity is MyEnvironmentItems)
			{
				m_currentTarget = MyAiTargetEnum.ENVIRONMENT_ITEM;
			}
			else if (entity != null)
			{
				m_currentTarget = MyAiTargetEnum.ENTITY;
			}
		}

		protected virtual void UnsetTargetEntity()
		{
			if (IsTargetGridOrBlock(m_currentTarget) && m_targetEntity is MyCubeGrid)
			{
				(m_targetEntity as MyCubeGrid).OnBlockRemoved -= BlockRemoved;
			}
			m_compoundId = null;
			m_targetEntity = null;
			m_currentTarget = MyAiTargetEnum.NO_TARGET;
		}

		private void BlockRemoved(MySlimBlock block)
		{
			_ = TargetGrid;
			if (GetCubeBlock() == null)
			{
				UnsetTargetEntity();
			}
		}

		public void SetTargetBlock(MySlimBlock slimBlock, ushort? compoundId = null)
		{
			if (m_targetEntity != slimBlock.CubeGrid)
			{
				SetTargetEntity(slimBlock.CubeGrid);
			}
			m_targetCube = slimBlock.Position;
			m_currentTarget = MyAiTargetEnum.CUBE;
		}

		public MySlimBlock GetTargetBlock()
		{
			if (m_currentTarget != MyAiTargetEnum.CUBE)
			{
				return null;
			}
			if (TargetGrid == null)
			{
				return null;
			}
			return GetCubeBlock();
		}

		public void SetTargetTree(ref MyEnvironmentItems.ItemInfo targetTree, long treesId)
		{
			if (MyEntities.TryGetEntityById(treesId, out MyEntity entity))
			{
				UnsetTarget();
				SetMTargetPosition(targetTree.Transform.Position);
				m_targetEntity = entity;
				m_targetTreeId = targetTree.LocalId;
				SetTargetEntity(entity);
			}
		}

		public void SetTargetPosition(Vector3D pos)
		{
			UnsetTarget();
			SetMTargetPosition(pos);
			m_currentTarget = MyAiTargetEnum.POSITION;
		}

		public void SetTargetVoxel(Vector3D pos, MyVoxelMap voxelMap)
		{
			UnsetTarget();
			MyVoxelCoordSystems.WorldPositionToVoxelCoord(voxelMap.PositionLeftBottomCorner, ref pos, out m_targetInVoxelCoord);
			SetMTargetPosition(pos);
			m_currentTarget = MyAiTargetEnum.VOXEL;
		}

		protected Vector3D GetLocalCubeProjectedPosition(ref Vector3D toProject)
		{
			GetCubeBlock();
			Vector3D value = Vector3D.Transform(toProject, TargetGrid.PositionComp.WorldMatrixNormalizedInv);
			value -= (m_targetCube + new Vector3(0.5f)) * TargetGrid.GridSize;
			float num = 1f;
			num = ((Math.Abs(value.Y) > Math.Abs(value.Z)) ? ((!(Math.Abs(value.Y) > Math.Abs(value.X))) ? (1f / (float)Math.Abs(value.X)) : (1f / (float)Math.Abs(value.Y))) : ((!(Math.Abs(value.Z) > Math.Abs(value.X))) ? (1f / (float)Math.Abs(value.X)) : (1f / (float)Math.Abs(value.Z))));
			value *= (double)num;
			return value * (TargetGrid.GridSize * 0.5f);
		}

		public Vector3D GetAimAtPosition(Vector3D startingPosition)
		{
			if (!HasTarget())
			{
				return Vector3D.Zero;
			}
			if (m_currentTarget == MyAiTargetEnum.POSITION)
			{
				return m_targetPosition;
			}
			if (m_currentTarget == MyAiTargetEnum.ENVIRONMENT_ITEM)
			{
				return m_targetPosition;
			}
			Vector3D result = m_targetEntity.PositionComp.GetPosition();
			if (m_currentTarget == MyAiTargetEnum.CUBE)
			{
				GetLocalCubeProjectedPosition(ref startingPosition);
				result = TargetCubeWorldPosition;
			}
			else if (m_currentTarget == MyAiTargetEnum.CHARACTER)
			{
				MyPositionComponentBase positionComp = (m_targetEntity as MyCharacter).PositionComp;
				result = Vector3D.Transform(positionComp.LocalVolume.Center, positionComp.WorldMatrix);
			}
			else if (m_currentTarget == MyAiTargetEnum.VOXEL)
			{
				result = m_targetPosition;
			}
			else if (m_currentTarget == MyAiTargetEnum.ENTITY && m_targetPosition != Vector3D.Zero && m_targetEntity is MyFracturedPiece)
			{
				result = m_targetPosition;
			}
			return result;
		}

		public virtual bool GetRandomDirectedPosition(Vector3D initPosition, Vector3D direction, out Vector3D outPosition)
		{
			outPosition = MySession.Static.LocalCharacter.PositionComp.GetPosition();
			return true;
		}

		protected MySlimBlock GetCubeBlock()
		{
			if (m_compoundId.HasValue)
			{
				MySlimBlock cubeBlock = TargetGrid.GetCubeBlock(m_targetCube);
				if (cubeBlock == null)
				{
					return null;
				}
				return (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlock(m_compoundId.Value);
			}
			if (TargetGrid == null)
			{
				return null;
			}
			return TargetGrid.GetCubeBlock(m_targetCube);
		}
	}
}
