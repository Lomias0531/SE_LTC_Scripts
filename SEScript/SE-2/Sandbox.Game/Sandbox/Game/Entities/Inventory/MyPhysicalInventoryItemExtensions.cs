using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Inventory
{
	public static class MyPhysicalInventoryItemExtensions
	{
		private const float ITEM_SPAWN_RADIUS = 1f;

		private static List<HkBodyCollision> m_tmpCollisions = new List<HkBodyCollision>();

		public static void Spawn(this MyPhysicalInventoryItem thisItem, MyFixedPoint amount, BoundingBoxD box, MyEntity owner = null, Action<MyEntity> completionCallback = null)
		{
			if (!(amount < 0))
			{
				MatrixD identity = MatrixD.Identity;
				identity.Translation = box.Center;
				thisItem.Spawn(amount, identity, owner, delegate(MyEntity entity)
				{
					InitSpawned(entity, box, completionCallback);
				});
			}
		}

		private static void InitSpawned(MyEntity entity, BoundingBoxD box, Action<MyEntity> completionCallback)
		{
			if (entity != null)
			{
				float radius = entity.PositionComp.LocalVolume.Radius;
				Vector3D v = box.Size / 2.0 - new Vector3(radius);
				v = Vector3.Max(v, Vector3.Zero);
				box = new BoundingBoxD(box.Center - v, box.Center + v);
				Vector3D randomPosition = MyUtils.GetRandomPosition(ref box);
				Vector3 randomVector3Normalized = MyUtils.GetRandomVector3Normalized();
				Vector3 randomVector3Normalized2 = MyUtils.GetRandomVector3Normalized();
				while (randomVector3Normalized == randomVector3Normalized2)
				{
					randomVector3Normalized2 = MyUtils.GetRandomVector3Normalized();
				}
				randomVector3Normalized2 = Vector3.Cross(Vector3.Cross(randomVector3Normalized, randomVector3Normalized2), randomVector3Normalized);
				entity.WorldMatrix = MatrixD.CreateWorld(randomPosition, randomVector3Normalized, randomVector3Normalized2);
				completionCallback?.Invoke(entity);
			}
		}

		public static void Spawn(this MyPhysicalInventoryItem thisItem, MyFixedPoint amount, MatrixD worldMatrix, MyEntity owner, Action<MyEntity> completionCallback)
		{
			if (amount < 0 || thisItem.Content == null)
			{
				return;
			}
			if (thisItem.Content is MyObjectBuilder_BlockItem)
			{
				if (!typeof(MyObjectBuilder_CubeBlock).IsAssignableFrom(thisItem.Content.GetObjectId().TypeId))
				{
					return;
				}
				MyObjectBuilder_BlockItem myObjectBuilder_BlockItem = thisItem.Content as MyObjectBuilder_BlockItem;
				MyDefinitionManager.Static.TryGetCubeBlockDefinition(myObjectBuilder_BlockItem.BlockDefId, out MyCubeBlockDefinition blockDefinition);
				if (blockDefinition == null)
				{
					return;
				}
				MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid = MyObjectBuilderSerializer.CreateNewObject(typeof(MyObjectBuilder_CubeGrid)) as MyObjectBuilder_CubeGrid;
				myObjectBuilder_CubeGrid.GridSizeEnum = blockDefinition.CubeSize;
				myObjectBuilder_CubeGrid.IsStatic = false;
				myObjectBuilder_CubeGrid.PersistentFlags |= (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
				myObjectBuilder_CubeGrid.PositionAndOrientation = new MyPositionAndOrientation(worldMatrix);
				MyObjectBuilder_CubeBlock myObjectBuilder_CubeBlock = MyObjectBuilderSerializer.CreateNewObject(myObjectBuilder_BlockItem.BlockDefId) as MyObjectBuilder_CubeBlock;
				if (myObjectBuilder_CubeBlock != null)
				{
					myObjectBuilder_CubeBlock.Min = blockDefinition.Size / 2 - blockDefinition.Size + Vector3I.One;
					myObjectBuilder_CubeGrid.CubeBlocks.Add(myObjectBuilder_CubeBlock);
					for (int i = 0; i < amount; i++)
					{
						myObjectBuilder_CubeGrid.EntityId = MyEntityIdentifier.AllocateId();
						myObjectBuilder_CubeBlock.EntityId = MyEntityIdentifier.AllocateId();
						MyEntities.CreateFromObjectBuilderParallel(myObjectBuilder_CubeGrid, addToScene: true, completionCallback);
					}
				}
			}
			else
			{
				MyPhysicalItemDefinition definition = null;
				if (MyDefinitionManager.Static.TryGetPhysicalItemDefinition(thisItem.Content.GetObjectId(), out definition))
				{
					MyFloatingObjects.Spawn(new MyPhysicalInventoryItem(amount, thisItem.Content), worldMatrix, owner?.Physics, completionCallback);
				}
			}
		}

		public static MyDefinitionBase GetItemDefinition(this MyPhysicalInventoryItem thisItem)
		{
			if (thisItem.Content == null)
			{
				return null;
			}
			MyDefinitionBase myDefinitionBase = null;
			if (thisItem.Content is MyObjectBuilder_BlockItem)
			{
				SerializableDefinitionId blockDefId = (thisItem.Content as MyObjectBuilder_BlockItem).BlockDefId;
				MyCubeBlockDefinition blockDefinition = null;
				if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(blockDefId, out blockDefinition))
				{
					myDefinitionBase = blockDefinition;
				}
			}
			else
			{
				myDefinitionBase = MyDefinitionManager.Static.TryGetComponentBlockDefinition(thisItem.Content.GetId());
			}
			if (myDefinitionBase == null && MyDefinitionManager.Static.TryGetPhysicalItemDefinition(thisItem.Content.GetId(), out MyPhysicalItemDefinition definition))
			{
				myDefinitionBase = definition;
			}
			return myDefinitionBase;
		}

		private static bool GetNonPenetratingTransformPosition(ref BoundingBox box, ref MatrixD transform)
		{
			Quaternion rotation = Quaternion.CreateFromRotationMatrix(transform);
			Vector3 halfExtents = box.HalfExtents;
			try
			{
				for (int i = 0; i < 11; i++)
				{
					float num = 0.3f * (float)i;
					Vector3D translation = transform.Translation + Vector3D.UnitY * num;
					m_tmpCollisions.Clear();
					MyPhysics.GetPenetrationsBox(ref halfExtents, ref translation, ref rotation, m_tmpCollisions, 15);
					if (m_tmpCollisions.Count == 0)
					{
						transform.Translation = translation;
						return true;
					}
				}
				return false;
			}
			finally
			{
				m_tmpCollisions.Clear();
			}
		}

		private static void AddItemToLootBag(MyEntity itemOwner, MyPhysicalInventoryItem item, ref MyEntity lootBagEntity)
		{
			MyLootBagDefinition lootBagDefinition = MyDefinitionManager.Static.GetLootBagDefinition();
			if (lootBagDefinition == null)
			{
				return;
			}
			MyDefinitionBase itemDefinition = item.GetItemDefinition();
			if (itemDefinition == null)
			{
				return;
			}
			if (lootBagEntity == null && lootBagDefinition.SearchRadius > 0f)
			{
				Vector3D position = itemOwner.PositionComp.GetPosition();
				BoundingSphereD boundingSphere = new BoundingSphereD(position, lootBagDefinition.SearchRadius);
				List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
				double num = double.MaxValue;
				foreach (MyEntity item2 in entitiesInSphere)
				{
					if (!item2.MarkedForClose && item2.GetType() == typeof(MyEntity) && item2.DefinitionId.HasValue && item2.DefinitionId.Value == lootBagDefinition.ContainerDefinition)
					{
						double num2 = (item2.PositionComp.GetPosition() - position).LengthSquared();
						if (num2 < num)
						{
							lootBagEntity = item2;
							num = num2;
						}
					}
				}
				entitiesInSphere.Clear();
			}
			if (lootBagEntity == null || (lootBagEntity.Components.Has<MyInventoryBase>() && !(lootBagEntity.Components.Get<MyInventoryBase>() as MyInventory).CanItemsBeAdded(item.Amount, itemDefinition.Id)))
			{
				lootBagEntity = null;
				if (MyComponentContainerExtension.TryGetContainerDefinition(lootBagDefinition.ContainerDefinition.TypeId, lootBagDefinition.ContainerDefinition.SubtypeId, out MyContainerDefinition definition))
				{
					lootBagEntity = SpawnBagAround(itemOwner, definition);
				}
			}
			if (lootBagEntity == null)
			{
				return;
			}
			MyInventory myInventory = lootBagEntity.Components.Get<MyInventoryBase>() as MyInventory;
			if (myInventory != null)
			{
				if (itemDefinition is MyCubeBlockDefinition)
				{
					myInventory.AddBlocks(itemDefinition as MyCubeBlockDefinition, item.Amount);
				}
				else
				{
					myInventory.AddItems(item.Amount, item.Content);
				}
			}
		}

		private static MyEntity SpawnBagAround(MyEntity itemOwner, MyContainerDefinition bagDefinition, int sideCheckCount = 3, int frontCheckCount = 2, int upCheckCount = 5, float stepSize = 1f)
		{
			Vector3D? vector3D = null;
			MyModel myModel = null;
			foreach (MyContainerDefinition.DefaultComponent defaultComponent in bagDefinition.DefaultComponents)
			{
				if (typeof(MyObjectBuilder_ModelComponent).IsAssignableFrom(defaultComponent.BuilderType))
				{
					MyComponentDefinitionBase componentDefinition = null;
					MyStringHash subtypeName = bagDefinition.Id.SubtypeId;
					if (defaultComponent.SubtypeId.HasValue)
					{
						subtypeName = defaultComponent.SubtypeId.Value;
					}
					if (MyComponentContainerExtension.TryGetComponentDefinition(defaultComponent.BuilderType, subtypeName, out componentDefinition))
					{
						MyModelComponentDefinition myModelComponentDefinition = componentDefinition as MyModelComponentDefinition;
						if (myModelComponentDefinition != null)
						{
							myModel = MyModels.GetModelOnlyData(myModelComponentDefinition.Model);
						}
					}
					break;
				}
			}
			if (myModel == null)
			{
				return null;
			}
			float num = myModel.BoundingBox.HalfExtents.Max();
			HkShape shape = new HkSphereShape(num);
			try
			{
				Vector3D translation = itemOwner.PositionComp.WorldMatrix.Translation;
				float num2 = num * stepSize;
				Vector3 vector = -MyGravityProviderSystem.CalculateNaturalGravityInPoint(itemOwner.PositionComp.WorldMatrix.Translation);
				if (vector == Vector3.Zero)
				{
					vector = Vector3.Up;
				}
				else
				{
					vector.Normalize();
				}
				vector.CalculatePerpendicularVector(out Vector3 result);
				Vector3 vector2 = Vector3.Cross(result, vector);
				vector2.Normalize();
				Quaternion rotation = Quaternion.Identity;
				Vector3[] array = new Vector3[4]
				{
					result,
					vector2,
					-result,
					-vector2
				};
				Vector3[] array2 = new Vector3[4]
				{
					vector2,
					-result,
					-vector2,
					result
				};
				for (int i = 0; i < array.Length; i++)
				{
					if (vector3D.HasValue)
					{
						break;
					}
					Vector3 value = array[i];
					Vector3 value2 = array2[i];
					for (int j = 0; j < frontCheckCount; j++)
					{
						if (vector3D.HasValue)
						{
							break;
						}
						Vector3D value3 = translation + 0.25f * value + num * value + (float)j * num2 * value - 0.5f * (float)(sideCheckCount - 1) * num2 * value2;
						for (int k = 0; k < sideCheckCount; k++)
						{
							if (vector3D.HasValue)
							{
								break;
							}
							for (int l = 0; l < upCheckCount; l++)
							{
								if (vector3D.HasValue)
								{
									break;
								}
								Vector3D position = value3 + (float)k * num2 * value2 + (float)l * num2 * vector;
								if (MyEntities.IsInsideWorld(position) && !MyEntities.IsShapePenetrating(shape, ref position, ref rotation))
								{
									BoundingSphereD sphere = new BoundingSphereD(position, num);
									if (MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere) == null)
									{
										vector3D = position;
										break;
									}
								}
							}
						}
					}
				}
				if (!vector3D.HasValue)
				{
					MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(itemOwner.PositionComp.LocalAABB, itemOwner.PositionComp.WorldMatrix);
					Vector3D[] array3 = new Vector3D[8];
					myOrientedBoundingBoxD.GetCorners(array3, 0);
					float num3 = float.MinValue;
					Vector3D[] array4 = array3;
					for (int m = 0; m < array4.Length; m++)
					{
						float val = Vector3.Dot(array4[m] - myOrientedBoundingBoxD.Center, vector);
						num3 = Math.Max(num3, val);
					}
					vector3D = itemOwner.PositionComp.WorldMatrix.Translation;
					if (num3 > 0f)
					{
						vector3D = myOrientedBoundingBoxD.Center + num3 * vector;
					}
				}
			}
			finally
			{
				shape.RemoveReference();
			}
			MatrixD worldMatrix = itemOwner.PositionComp.WorldMatrix;
			worldMatrix.Translation = vector3D.Value;
			MyEntity myEntity = MyEntities.CreateFromComponentContainerDefinitionAndAdd(bagDefinition.Id, fadeIn: false);
			if (myEntity == null)
			{
				return null;
			}
			myEntity.PositionComp.SetWorldMatrix(worldMatrix);
			myEntity.Physics.LinearVelocity = Vector3.Zero;
			myEntity.Physics.AngularVelocity = Vector3.Zero;
			return myEntity;
		}

		public static MyInventoryItem? MakeAPIItem(this MyPhysicalInventoryItem? item)
		{
			if (!item.HasValue)
			{
				return null;
			}
			return item.Value.MakeAPIItem();
		}

		public static MyInventoryItem MakeAPIItem(this MyPhysicalInventoryItem item)
		{
			return new MyInventoryItem(item.Content.GetObjectId(), item.ItemId, item.Amount);
		}
	}
}
