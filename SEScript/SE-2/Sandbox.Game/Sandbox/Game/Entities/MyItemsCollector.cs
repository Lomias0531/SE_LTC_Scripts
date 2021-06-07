using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public static class MyItemsCollector
	{
		public struct ItemInfo
		{
			public Vector3D Target;

			public long ItemsEntityId;

			public int ItemId;
		}

		public struct EntityInfo
		{
			public Vector3D Target;

			public long EntityId;
		}

		public struct ComponentInfo
		{
			public long EntityId;

			public Vector3I BlockPosition;

			public MyDefinitionId ComponentDefinitionId;

			public int ComponentCount;

			public bool IsBlock;
		}

		public struct CollectibleInfo
		{
			public long EntityId;

			public MyDefinitionId DefinitionId;

			public MyFixedPoint Amount;
		}

		private static List<MyFracturedPiece> m_tmpFracturePieceList = new List<MyFracturedPiece>();

		private static List<MyEnvironmentItems.ItemInfo> m_tmpEnvItemList = new List<MyEnvironmentItems.ItemInfo>();

		private static List<ItemInfo> m_tmpItemInfoList = new List<ItemInfo>();

		private static List<ComponentInfo> m_retvalBlockInfos = new List<ComponentInfo>();

		private static List<CollectibleInfo> m_retvalCollectibleInfos = new List<CollectibleInfo>();

		public static bool FindClosestTreeInRadius(Vector3D fromPosition, float radius, out ItemInfo result)
		{
			result = default(ItemInfo);
			BoundingSphereD boundingSphere = new BoundingSphereD(fromPosition, radius);
			List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
			double num = double.MaxValue;
			foreach (MyEntity item in entitiesInSphere)
			{
				MyTrees myTrees = item as MyTrees;
				if (myTrees != null)
				{
					myTrees.GetPhysicalItemsInRadius(fromPosition, radius, m_tmpEnvItemList);
					foreach (MyEnvironmentItems.ItemInfo tmpEnvItem in m_tmpEnvItemList)
					{
						double num2 = Vector3D.DistanceSquared(fromPosition, tmpEnvItem.Transform.Position);
						if (num2 < num)
						{
							result.ItemsEntityId = item.EntityId;
							result.ItemId = tmpEnvItem.LocalId;
							result.Target = tmpEnvItem.Transform.Position;
							num = num2;
						}
					}
				}
			}
			entitiesInSphere.Clear();
			return num != double.MaxValue;
		}

		public static bool FindFallingTreeInRadius(Vector3D position, float radius, out EntityInfo result)
		{
			result = default(EntityInfo);
			BoundingSphereD searchSphere = new BoundingSphereD(position, radius);
			m_tmpFracturePieceList.Clear();
			MyFracturedPiecesManager.Static.GetFracturesInSphere(ref searchSphere, ref m_tmpFracturePieceList);
			foreach (MyFracturedPiece tmpFracturePiece in m_tmpFracturePieceList)
			{
				if (tmpFracturePiece.Physics.RigidBody != null && tmpFracturePiece.Physics.RigidBody.IsActive && !Vector3.IsZero(tmpFracturePiece.Physics.AngularVelocity) && !Vector3.IsZero(tmpFracturePiece.Physics.LinearVelocity))
				{
					result.Target = Vector3D.Transform(tmpFracturePiece.Shape.CoM, tmpFracturePiece.PositionComp.WorldMatrix);
					result.EntityId = tmpFracturePiece.EntityId;
					m_tmpFracturePieceList.Clear();
					return true;
				}
			}
			m_tmpFracturePieceList.Clear();
			return false;
		}

		public static bool FindCollectableItemInRadius(Vector3D position, float radius, HashSet<MyDefinitionId> itemDefs, Vector3D initialPosition, float ignoreRadius, out ComponentInfo result)
		{
			BoundingSphereD boundingSphere = new BoundingSphereD(position, radius);
			List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
			result = default(ComponentInfo);
			double num = double.MaxValue;
			foreach (MyEntity item in entitiesInSphere)
			{
				if (item is MyCubeGrid)
				{
					MyCubeGrid myCubeGrid = item as MyCubeGrid;
					if (myCubeGrid.BlocksCount == 1)
					{
						MySlimBlock mySlimBlock = myCubeGrid.CubeBlocks.First();
						if (itemDefs.Contains(mySlimBlock.BlockDefinition.Id))
						{
							Vector3D value = myCubeGrid.GridIntegerToWorld(mySlimBlock.Position);
							if (Vector3D.DistanceSquared(value, initialPosition) <= (double)(ignoreRadius * ignoreRadius))
							{
								continue;
							}
							double num2 = Vector3D.DistanceSquared(value, position);
							if (num2 < num)
							{
								num = num2;
								result.EntityId = myCubeGrid.EntityId;
								result.BlockPosition = mySlimBlock.Position;
								result.ComponentDefinitionId = GetComponentId(mySlimBlock);
								result.IsBlock = true;
							}
						}
					}
				}
				if (item is MyFloatingObject)
				{
					MyFloatingObject myFloatingObject = item as MyFloatingObject;
					MyDefinitionId id = myFloatingObject.Item.Content.GetId();
					if (itemDefs.Contains(id))
					{
						double num3 = Vector3D.DistanceSquared(myFloatingObject.PositionComp.WorldMatrix.Translation, position);
						if (num3 < num)
						{
							num = num3;
							result.EntityId = myFloatingObject.EntityId;
							result.IsBlock = false;
						}
					}
				}
			}
			entitiesInSphere.Clear();
			return num != double.MaxValue;
		}

		public static List<ComponentInfo> FindComponentsInRadius(Vector3D fromPosition, double radius)
		{
			BoundingSphereD boundingSphere = new BoundingSphereD(fromPosition, radius);
			List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
			foreach (MyEntity item3 in entitiesInSphere)
			{
				if (item3 is MyFloatingObject)
				{
					MyFloatingObject myFloatingObject = item3 as MyFloatingObject;
					if (myFloatingObject.Item.Content is MyObjectBuilder_Component)
					{
						ComponentInfo item = default(ComponentInfo);
						item.EntityId = myFloatingObject.EntityId;
						item.BlockPosition = Vector3I.Zero;
						item.ComponentDefinitionId = myFloatingObject.Item.Content.GetObjectId();
						item.IsBlock = false;
						item.ComponentCount = (int)myFloatingObject.Item.Amount;
						m_retvalBlockInfos.Add(item);
					}
				}
				else
				{
					MyCubeBlock block = null;
					MyCubeGrid myCubeGrid = TryGetAsComponent(item3, out block);
					if (myCubeGrid != null)
					{
						ComponentInfo item2 = default(ComponentInfo);
						item2.IsBlock = true;
						item2.EntityId = myCubeGrid.EntityId;
						item2.BlockPosition = block.Position;
						item2.ComponentDefinitionId = GetComponentId(block.SlimBlock);
						if (block.BlockDefinition.Components != null)
						{
							item2.ComponentCount = block.BlockDefinition.Components[0].Count;
						}
						else
						{
							item2.ComponentCount = 0;
						}
						m_retvalBlockInfos.Add(item2);
					}
				}
			}
			entitiesInSphere.Clear();
			return m_retvalBlockInfos;
		}

		public static List<CollectibleInfo> FindCollectiblesInRadius(Vector3D fromPosition, double radius, bool doRaycast = false)
		{
			List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
			BoundingSphereD boundingSphere = new BoundingSphereD(fromPosition, radius);
			List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
			foreach (MyEntity item2 in entitiesInSphere)
			{
				bool flag = false;
				CollectibleInfo item = default(CollectibleInfo);
				MyCubeBlock block = null;
				MyCubeGrid myCubeGrid = TryGetAsComponent(item2, out block);
				if (myCubeGrid != null)
				{
					item.EntityId = myCubeGrid.EntityId;
					item.DefinitionId = GetComponentId(block.SlimBlock);
					if (block.BlockDefinition.Components != null)
					{
						item.Amount = block.BlockDefinition.Components[0].Count;
					}
					else
					{
						item.Amount = 0;
					}
					flag = true;
				}
				else if (item2 is MyFloatingObject)
				{
					MyFloatingObject myFloatingObject = item2 as MyFloatingObject;
					MyDefinitionId objectId = myFloatingObject.Item.Content.GetObjectId();
					if (MyDefinitionManager.Static.GetPhysicalItemDefinition(objectId).Public)
					{
						item.EntityId = myFloatingObject.EntityId;
						item.DefinitionId = objectId;
						item.Amount = myFloatingObject.Item.Amount;
						flag = true;
					}
				}
				if (flag)
				{
					bool flag2 = false;
					MyPhysics.CastRay(fromPosition, item2.WorldMatrix.Translation, list, 15);
					foreach (MyPhysics.HitInfo item3 in list)
					{
						IMyEntity hitEntity = item3.HkHitInfo.GetHitEntity();
						if (hitEntity != item2 && !(hitEntity is MyCharacter) && !(hitEntity is MyFracturedPiece) && !(hitEntity is MyFloatingObject))
						{
							MyCubeBlock block2 = null;
							if (TryGetAsComponent(hitEntity as MyEntity, out block2) == null)
							{
								flag2 = true;
								break;
							}
						}
					}
					if (!flag2)
					{
						m_retvalCollectibleInfos.Add(item);
					}
				}
			}
			entitiesInSphere.Clear();
			return m_retvalCollectibleInfos;
		}

		public static MyCubeGrid TryGetAsComponent(MyEntity entity, out MyCubeBlock block, bool blockManipulatedEntity = true, Vector3D? hitPosition = null)
		{
			block = null;
			if (entity.MarkedForClose)
			{
				return null;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return null;
			}
			if (myCubeGrid.GridSizeEnum != MyCubeSize.Small)
			{
				return null;
			}
			MyCubeGrid result = null;
			if (MyFakes.ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID && hitPosition.HasValue)
			{
				Vector3D value = Vector3D.Transform(hitPosition.Value, myCubeGrid.PositionComp.WorldMatrixNormalizedInv);
				myCubeGrid.FixTargetCube(out Vector3I cube, value / myCubeGrid.GridSize);
				MySlimBlock cubeBlock = myCubeGrid.GetCubeBlock(cube);
				if (cubeBlock != null && cubeBlock.IsFullIntegrity)
				{
					block = cubeBlock.FatBlock;
				}
			}
			else
			{
				if (myCubeGrid.CubeBlocks.Count != 1)
				{
					return null;
				}
				if (myCubeGrid.IsStatic)
				{
					return null;
				}
				if (!MyCubeGrid.IsGridInCompleteState(myCubeGrid))
				{
					return null;
				}
				if (MyCubeGridSmallToLargeConnection.Static.TestGridSmallToLargeConnection(myCubeGrid))
				{
					return null;
				}
				HashSet<MySlimBlock>.Enumerator enumerator = myCubeGrid.CubeBlocks.GetEnumerator();
				enumerator.MoveNext();
				block = enumerator.Current.FatBlock;
				enumerator.Dispose();
				result = myCubeGrid;
			}
			if (block == null)
			{
				return null;
			}
			if (!MyDefinitionManager.Static.IsComponentBlock(block.BlockDefinition.Id))
			{
				return null;
			}
			if (block.IsSubBlock)
			{
				return null;
			}
			DictionaryReader<string, MySlimBlock> subBlocks = block.GetSubBlocks();
			if (subBlocks.IsValid && subBlocks.Count > 0)
			{
				return null;
			}
			return result;
		}

		private static MyDefinitionId GetComponentId(MySlimBlock block)
		{
			MyCubeBlockDefinition.Component[] components = block.BlockDefinition.Components;
			if (components == null || components.Length == 0)
			{
				return default(MyDefinitionId);
			}
			return components[0].Definition.Id;
		}

		private static bool IsFracturedTreeStump(MyFracturedPiece fracture)
		{
			fracture.Shape.GetShape().GetLocalAABB(0f, out Vector4 min, out Vector4 max);
			if ((double)(max.Y - min.Y) < 3.5 * (double)(max.X - min.X))
			{
				return true;
			}
			return false;
		}

		private static bool FindClosestPointOnFracturedTree(Vector3D fromPositionFractureLocal, MyFracturedPiece fracture, out Vector3D closestPoint)
		{
			closestPoint = default(Vector3D);
			if (fracture == null)
			{
				return false;
			}
			fracture.Shape.GetShape().GetLocalAABB(0f, out Vector4 min, out Vector4 max);
			Vector3D min2 = new Vector3D(min);
			Vector3D max2 = new Vector3D(max);
			closestPoint = Vector3D.Clamp(fromPositionFractureLocal, min2, max2);
			closestPoint.X = (closestPoint.X + 2.0 * (max2.X + min2.X) / 2.0) / 3.0;
			closestPoint.Y = MathHelper.Clamp(closestPoint.Y + (double)(0.25f * (float)((closestPoint.Y - min2.Y < max2.Y - closestPoint.Y) ? 1 : (-1))), min2.Y, max2.Y);
			closestPoint.Z = (closestPoint.Z + 2.0 * (max2.Z + min2.Z) / 2.0) / 3.0;
			closestPoint = Vector3D.Transform(closestPoint, fracture.PositionComp.WorldMatrix);
			return true;
		}
	}
}
