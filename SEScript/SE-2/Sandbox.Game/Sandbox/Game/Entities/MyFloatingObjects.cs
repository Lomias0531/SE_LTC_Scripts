using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Input;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 800)]
	[StaticEventOwner]
	public class MyFloatingObjects : MySessionComponentBase
	{
		private class MyFloatingObjectComparer : IEqualityComparer<MyFloatingObject>
		{
			public bool Equals(MyFloatingObject x, MyFloatingObject y)
			{
				return x.EntityId == y.EntityId;
			}

			public int GetHashCode(MyFloatingObject obj)
			{
				return (int)obj.EntityId;
			}
		}

		private class MyFloatingObjectTimestampComparer : IComparer<MyFloatingObject>
		{
			public int Compare(MyFloatingObject x, MyFloatingObject y)
			{
				if (x.CreationTime != y.CreationTime)
				{
					return y.CreationTime.CompareTo(x.CreationTime);
				}
				return y.EntityId.CompareTo(x.EntityId);
			}
		}

		private class MyFloatingObjectsSynchronizationComparer : IComparer<MyFloatingObject>
		{
			public int Compare(MyFloatingObject x, MyFloatingObject y)
			{
				return x.ClosestDistanceToAnyPlayerSquared.CompareTo(y.ClosestDistanceToAnyPlayerSquared);
			}
		}

		private struct StabilityInfo
		{
			public MyPositionAndOrientation PositionAndOr;

			public StabilityInfo(MyPositionAndOrientation posAndOr)
			{
				PositionAndOr = posAndOr;
			}
		}

		protected sealed class RequestSpawnCreative_Implementation_003C_003EVRage_Game_MyObjectBuilder_FloatingObject : ICallSite<IMyEventOwner, MyObjectBuilder_FloatingObject, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_FloatingObject obj, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestSpawnCreative_Implementation(obj);
			}
		}

		private static MyFloatingObjectComparer m_entityComparer = new MyFloatingObjectComparer();

		private static MyFloatingObjectTimestampComparer m_comparer = new MyFloatingObjectTimestampComparer();

		private static SortedSet<MyFloatingObject> m_floatingOres = new SortedSet<MyFloatingObject>(m_comparer);

		private static SortedSet<MyFloatingObject> m_floatingItems = new SortedSet<MyFloatingObject>(m_comparer);

		private static List<MyVoxelBase> m_tmpResultList = new List<MyVoxelBase>();

		private static List<MyFloatingObject> m_synchronizedFloatingObjects = new List<MyFloatingObject>();

		private static List<MyFloatingObject> m_floatingObjectsToSyncCreate = new List<MyFloatingObject>();

		private static MyFloatingObjectsSynchronizationComparer m_synchronizationComparer = new MyFloatingObjectsSynchronizationComparer();

		private static List<MyFloatingObject> m_highPriority = new List<MyFloatingObject>();

		private static List<MyFloatingObject> m_normalPriority = new List<MyFloatingObject>();

		private static List<MyFloatingObject> m_lowPriority = new List<MyFloatingObject>();

		private static int m_updateCounter = 0;

		private static bool m_needReupdateNewObjects = false;

		private static int m_checkObjectInsideVoxel = 0;

		private static List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>> m_itemsToSpawnNextUpdate = new List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>>();

		private static readonly MyConcurrentPool<List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>>> m_itemsToSpawnPool = new MyConcurrentPool<List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>>>();

		public override Type[] Dependencies => new Type[1]
		{
			typeof(MyDebris)
		};

		public static int FloatingOreCount => m_floatingOres.Count;

		public static int FloatingItemCount => m_floatingItems.Count;

		public override void LoadData()
		{
			base.LoadData();
			foreach (MyPhysicalItemDefinition physicalItemDefinition in MyDefinitionManager.Static.GetPhysicalItemDefinitions())
			{
				if (physicalItemDefinition.HasModelVariants)
				{
					if (physicalItemDefinition.Models != null)
					{
						string[] models = physicalItemDefinition.Models;
						for (int i = 0; i < models.Length; i++)
						{
							MyModels.GetModelOnlyData(models[i]);
						}
					}
				}
				else if (!string.IsNullOrEmpty(physicalItemDefinition.Model))
				{
					MyModels.GetModelOnlyData(physicalItemDefinition.Model);
				}
			}
			MyVoxelMaterialDefinition voxelMaterialDefinition = MyDefinitionManager.Static.GetVoxelMaterialDefinition(0);
			Spawn(new MyPhysicalInventoryItem(1, MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Ore>(voxelMaterialDefinition.MinedOre)), default(BoundingSphereD), null, voxelMaterialDefinition, delegate(MyEntity item)
			{
				item.Close();
			});
		}

		public override void UpdateAfterSimulation()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			CheckObjectInVoxel();
			m_updateCounter++;
			if (m_updateCounter > 100)
			{
				ReduceFloatingObjects();
			}
			if (m_itemsToSpawnNextUpdate.Count > 0)
			{
				List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>> tmp = m_itemsToSpawnNextUpdate;
				m_itemsToSpawnNextUpdate = m_itemsToSpawnPool.Get();
				if (MySandboxGame.Config.SyncRendering)
				{
					MyEntityIdentifier.PrepareSwapData();
					MyEntityIdentifier.SwapPerThreadData();
				}
				Parallel.Start(delegate
				{
					SpawnInventoryItems(tmp);
					tmp.Clear();
					m_itemsToSpawnPool.Return(tmp);
				});
				if (MySandboxGame.Config.SyncRendering)
				{
					MyEntityIdentifier.ClearSwapDataAndRestore();
				}
			}
			base.UpdateAfterSimulation();
			if (m_updateCounter > 100)
			{
				OptimizeFloatingObjects();
			}
			else
			{
				if (m_needReupdateNewObjects)
				{
					OptimizeCloseDistances();
				}
				OptimizeQualityType();
			}
			if (MyInput.Static.ENABLE_DEVELOPER_KEYS)
			{
				UpdateObjectCounters();
			}
			if (m_updateCounter > 100)
			{
				m_updateCounter = 0;
			}
		}

		private void UpdateObjectCounters()
		{
			MyPerformanceCounter.PerCameraDrawRead.CustomCounters["Floating Ores"] = FloatingOreCount;
			MyPerformanceCounter.PerCameraDrawRead.CustomCounters["Floating Items"] = FloatingItemCount;
			MyPerformanceCounter.PerCameraDrawWrite.CustomCounters["Floating Ores"] = FloatingOreCount;
			MyPerformanceCounter.PerCameraDrawWrite.CustomCounters["Floating Items"] = FloatingItemCount;
		}

		private void OptimizeFloatingObjects()
		{
			ReduceFloatingObjects();
			OptimizeCloseDistances();
			OptimizeQualityType();
		}

		private void OptimizeCloseDistances()
		{
			UpdateClosestDistancesToPlayers();
			m_synchronizedFloatingObjects.Sort(m_synchronizationComparer);
			m_highPriority.Clear();
			m_normalPriority.Clear();
			m_lowPriority.Clear();
			m_needReupdateNewObjects = false;
			float num = 16f;
			float num2 = 256f;
			int num3 = 32;
			float num4 = 4096f;
			int num5 = 128;
			float num6 = 0.00250000018f;
			for (int i = 0; i < m_synchronizedFloatingObjects.Count; i++)
			{
				MyFloatingObject myFloatingObject = m_synchronizedFloatingObjects[i];
				m_needReupdateNewObjects |= (myFloatingObject.ClosestDistanceToAnyPlayerSquared == -1f || (myFloatingObject.ClosestDistanceToAnyPlayerSquared < num && myFloatingObject.SyncWaitCounter > 5));
				float num7 = myFloatingObject.Physics.LinearVelocity.LengthSquared();
				float num8 = myFloatingObject.Physics.AngularVelocity.LengthSquared();
				if (myFloatingObject.ClosestDistanceToAnyPlayerSquared == -1f || num7 > num6 || num8 > num6)
				{
					if (myFloatingObject.ClosestDistanceToAnyPlayerSquared < num2 && i < num3)
					{
						m_highPriority.Add(myFloatingObject);
					}
					else if (myFloatingObject.ClosestDistanceToAnyPlayerSquared < num4 && i < num5)
					{
						m_normalPriority.Add(myFloatingObject);
					}
					else
					{
						m_lowPriority.Add(myFloatingObject);
					}
				}
			}
		}

		private void CheckObjectInVoxel()
		{
			if (m_checkObjectInsideVoxel >= m_synchronizedFloatingObjects.Count)
			{
				m_checkObjectInsideVoxel = 0;
			}
			if (m_synchronizedFloatingObjects.Count > 0)
			{
				MyFloatingObject myFloatingObject = m_synchronizedFloatingObjects[m_checkObjectInsideVoxel];
				BoundingBoxD aabb = myFloatingObject.PositionComp.LocalAABB;
				MatrixD aabbWorldTransform = myFloatingObject.PositionComp.WorldMatrix;
				BoundingBoxD box = myFloatingObject.PositionComp.WorldAABB;
				using (m_tmpResultList.GetClearToken())
				{
					MyGamePruningStructure.GetAllVoxelMapsInBox(ref box, m_tmpResultList);
					foreach (MyVoxelBase tmpResult in m_tmpResultList)
					{
						if (tmpResult != null && !tmpResult.MarkedForClose && !(tmpResult is MyVoxelPhysics))
						{
							if (tmpResult.AreAllAabbCornersInside(ref aabbWorldTransform, aabb))
							{
								myFloatingObject.NumberOfFramesInsideVoxel++;
								if (myFloatingObject.NumberOfFramesInsideVoxel > 5 && Sync.IsServer)
								{
									RemoveFloatingObject(myFloatingObject);
								}
							}
							else
							{
								myFloatingObject.NumberOfFramesInsideVoxel = 0;
							}
						}
					}
				}
			}
			m_checkObjectInsideVoxel++;
		}

		private void SpawnInventoryItems(List<Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D>> itemsList)
		{
			for (int i = 0; i < itemsList.Count; i++)
			{
				Tuple<MyPhysicalInventoryItem, BoundingBoxD, Vector3D> item = itemsList[i];
				item.Item1.Spawn(item.Item1.Amount, item.Item2, null, delegate(MyEntity entity)
				{
					entity.Physics.LinearVelocity = item.Item3;
					entity.Physics.ApplyImpulse(MyUtils.GetRandomVector3Normalized() * entity.Physics.Mass / 5f, entity.PositionComp.GetPosition());
				});
			}
			itemsList.Clear();
		}

		public static void Spawn(MyPhysicalInventoryItem item, Vector3D position, Vector3D forward, Vector3D up, MyPhysicsComponentBase motionInheritedFrom = null, Action<MyEntity> completionCallback = null)
		{
			if (!MyEntities.IsInsideWorld(position))
			{
				return;
			}
			Vector3D forward2 = forward;
			Vector3D up2 = up;
			Vector3D vector3D = Vector3D.Cross(up, forward);
			MyPhysicalItemDefinition definition = null;
			if (MyDefinitionManager.Static.TryGetDefinition(item.Content.GetObjectId(), out definition))
			{
				if (definition.RotateOnSpawnX)
				{
					forward2 = up;
					up2 = -forward;
				}
				if (definition.RotateOnSpawnY)
				{
					forward2 = vector3D;
				}
				if (definition.RotateOnSpawnZ)
				{
					up2 = -vector3D;
				}
			}
			Spawn(item, MatrixD.CreateWorld(position, forward2, up2), motionInheritedFrom, completionCallback);
		}

		public static void Spawn(MyPhysicalInventoryItem item, MatrixD worldMatrix, MyPhysicsComponentBase motionInheritedFrom, Action<MyEntity> completionCallback)
		{
			if (MyEntities.IsInsideWorld(worldMatrix.Translation))
			{
				MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = PrepareBuilder(ref item);
				myObjectBuilder_FloatingObject.PositionAndOrientation = new MyPositionAndOrientation(worldMatrix);
				MyEntities.CreateFromObjectBuilderParallel(myObjectBuilder_FloatingObject, addToScene: true, delegate(MyEntity entity)
				{
					if (entity != null && entity.Physics != null)
					{
						entity.Physics.ForceActivate();
						ApplyPhysics(entity, motionInheritedFrom);
						if (MyVisualScriptLogicProvider.ItemSpawned != null)
						{
							MyVisualScriptLogicProvider.ItemSpawned(item.Content.TypeId.ToString(), item.Content.SubtypeName, entity.EntityId, item.Amount.ToIntSafe(), worldMatrix.Translation);
						}
						if (completionCallback != null)
						{
							completionCallback(entity);
						}
					}
				});
			}
		}

		internal static MyEntity Spawn(MyPhysicalInventoryItem item, BoundingBoxD box, MyPhysicsComponentBase motionInheritedFrom = null)
		{
			MyEntity myEntity = MyEntities.CreateFromObjectBuilder(PrepareBuilder(ref item), fadeIn: false);
			if (myEntity != null)
			{
				float radius = myEntity.PositionComp.LocalVolume.Radius;
				Vector3D v = box.Size / 2.0 - new Vector3(radius);
				v = Vector3.Max(v, Vector3.Zero);
				box = new BoundingBoxD(box.Center - v, box.Center + v);
				Vector3D randomPosition = MyUtils.GetRandomPosition(ref box);
				AddToPos(myEntity, randomPosition, motionInheritedFrom);
				myEntity.Physics.ForceActivate();
				if (MyVisualScriptLogicProvider.ItemSpawned != null)
				{
					MyVisualScriptLogicProvider.ItemSpawned(item.Content.TypeId.ToString(), item.Content.SubtypeName, myEntity.EntityId, item.Amount.ToIntSafe(), randomPosition);
				}
			}
			return myEntity;
		}

		public static void Spawn(MyPhysicalInventoryItem item, BoundingSphereD sphere, MyPhysicsComponentBase motionInheritedFrom, MyVoxelMaterialDefinition voxelMaterial, Action<MyEntity> OnDone)
		{
			MyEntities.CreateFromObjectBuilderParallel(PrepareBuilder(ref item), addToScene: false, delegate(MyEntity entity)
			{
				if (voxelMaterial.DamagedMaterial != MyStringHash.NullOrEmpty)
				{
					voxelMaterial = MyDefinitionManager.Static.GetVoxelMaterialDefinition(voxelMaterial.DamagedMaterial.ToString());
				}
				((MyFloatingObject)entity).VoxelMaterial = voxelMaterial;
				float radius = entity.PositionComp.LocalVolume.Radius;
				double val = sphere.Radius - (double)radius;
				val = Math.Max(val, 0.0);
				sphere = new BoundingSphereD(sphere.Center, val);
				Vector3D randomBorderPosition = MyUtils.GetRandomBorderPosition(ref sphere);
				AddToPos(entity, randomBorderPosition, motionInheritedFrom);
				if (MyVisualScriptLogicProvider.ItemSpawned != null)
				{
					MyVisualScriptLogicProvider.ItemSpawned(item.Content.TypeId.ToString(), item.Content.SubtypeName, entity.EntityId, item.Amount.ToIntSafe(), randomBorderPosition);
				}
				OnDone(entity);
			});
		}

		public static void Spawn(MyPhysicalItemDefinition itemDefinition, Vector3D translation, Vector3D forward, Vector3D up, int amount = 1, float scale = 1f)
		{
			MyObjectBuilder_PhysicalObject content = MyObjectBuilderSerializer.CreateNewObject(itemDefinition.Id.TypeId, itemDefinition.Id.SubtypeName) as MyObjectBuilder_PhysicalObject;
			Spawn(new MyPhysicalInventoryItem(amount, content, scale), translation, forward, up);
		}

		public static void EnqueueInventoryItemSpawn(MyPhysicalInventoryItem inventoryItem, BoundingBoxD boundingBox, Vector3D inheritedVelocity)
		{
			m_itemsToSpawnNextUpdate.Add(Tuple.Create(inventoryItem, boundingBox, inheritedVelocity));
		}

		private static MyObjectBuilder_FloatingObject PrepareBuilder(ref MyPhysicalInventoryItem item)
		{
			MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_FloatingObject>();
			myObjectBuilder_FloatingObject.Item = item.GetObjectBuilder();
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(item.Content);
			myObjectBuilder_FloatingObject.ModelVariant = (physicalItemDefinition.HasModelVariants ? MyUtils.GetRandomInt(physicalItemDefinition.Models.Length) : 0);
			myObjectBuilder_FloatingObject.PersistentFlags |= (MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene);
			return myObjectBuilder_FloatingObject;
		}

		private static void AddToPos(MyEntity thrownEntity, Vector3D pos, MyPhysicsComponentBase motionInheritedFrom)
		{
			Vector3 randomVector3Normalized = MyUtils.GetRandomVector3Normalized();
			Vector3 randomVector3Normalized2 = MyUtils.GetRandomVector3Normalized();
			while (randomVector3Normalized == randomVector3Normalized2)
			{
				randomVector3Normalized2 = MyUtils.GetRandomVector3Normalized();
			}
			randomVector3Normalized2 = Vector3.Cross(Vector3.Cross(randomVector3Normalized, randomVector3Normalized2), randomVector3Normalized);
			thrownEntity.WorldMatrix = MatrixD.CreateWorld(pos, randomVector3Normalized, randomVector3Normalized2);
			MyEntities.Add(thrownEntity);
			ApplyPhysics(thrownEntity, motionInheritedFrom);
		}

		private static void ApplyPhysics(MyEntity thrownEntity, MyPhysicsComponentBase motionInheritedFrom)
		{
			if (thrownEntity.Physics != null && motionInheritedFrom != null)
			{
				thrownEntity.Physics.LinearVelocity = motionInheritedFrom.LinearVelocity;
				thrownEntity.Physics.AngularVelocity = motionInheritedFrom.AngularVelocity;
			}
		}

		private void OptimizeQualityType()
		{
			for (int i = 0; i < m_synchronizedFloatingObjects.Count; i++)
			{
				m_synchronizedFloatingObjects[i].Physics.ChangeQualityType(HkCollidableQualityType.Critical);
			}
		}

		internal static void RegisterFloatingObject(MyFloatingObject obj)
		{
			if (!obj.WasRemovedFromWorld)
			{
				obj.CreationTime = Stopwatch.GetTimestamp();
				if (obj.VoxelMaterial != null)
				{
					m_floatingOres.Add(obj);
				}
				else
				{
					m_floatingItems.Add(obj);
				}
				if (Sync.IsServer)
				{
					AddToSynchronization(obj);
				}
			}
		}

		internal static void UnregisterFloatingObject(MyFloatingObject obj)
		{
			if (obj.VoxelMaterial != null)
			{
				m_floatingOres.Remove(obj);
			}
			else
			{
				m_floatingItems.Remove(obj);
			}
			if (Sync.IsServer)
			{
				RemoveFromSynchronization(obj);
			}
			obj.WasRemovedFromWorld = true;
		}

		public static void AddFloatingObjectAmount(MyFloatingObject obj, MyFixedPoint amount)
		{
			MyPhysicalInventoryItem item = obj.Item;
			item.Amount += amount;
			obj.Item = item;
			obj.Amount.Value = item.Amount;
			obj.UpdateInternalState();
		}

		public static void RemoveFloatingObject(MyFloatingObject obj, bool sync)
		{
			if (sync)
			{
				if (Sync.IsServer)
				{
					RemoveFloatingObject(obj);
				}
				else
				{
					obj.SendCloseRequest();
				}
			}
			else
			{
				RemoveFloatingObject(obj);
			}
		}

		public static void RemoveFloatingObject(MyFloatingObject obj)
		{
			RemoveFloatingObject(obj, MyFixedPoint.MaxValue);
		}

		internal static void RemoveFloatingObject(MyFloatingObject obj, MyFixedPoint amount)
		{
			if (!(amount <= 0))
			{
				if (amount < obj.Item.Amount)
				{
					obj.Amount.Value -= amount;
					obj.RefreshDisplayName();
				}
				else
				{
					obj.Render.FadeOut = false;
					obj.Close();
					obj.WasRemovedFromWorld = true;
				}
			}
		}

		public static void ReduceFloatingObjects()
		{
			int num = m_floatingOres.Count + m_floatingItems.Count;
			int num2 = Math.Max(MySession.Static.MaxFloatingObjects / 5, 4);
			while (num > MySession.Static.MaxFloatingObjects)
			{
				SortedSet<MyFloatingObject> sortedSet = (m_floatingOres.Count <= num2 && m_floatingItems.Count != 0) ? m_floatingItems : m_floatingOres;
				if (sortedSet.Count > 0)
				{
					MyFloatingObject myFloatingObject = sortedSet.Last();
					sortedSet.Remove(myFloatingObject);
					if (Sync.IsServer)
					{
						RemoveFloatingObject(myFloatingObject);
					}
				}
				num--;
			}
		}

		private static void AddToSynchronization(MyFloatingObject floatingObject)
		{
			m_floatingObjectsToSyncCreate.Add(floatingObject);
			m_synchronizedFloatingObjects.Add(floatingObject);
			floatingObject.OnClose += floatingObject_OnClose;
			m_needReupdateNewObjects = true;
		}

		private static void floatingObject_OnClose(MyEntity obj)
		{
		}

		private static void RemoveFromSynchronization(MyFloatingObject floatingObject)
		{
			floatingObject.OnClose -= floatingObject_OnClose;
			m_synchronizedFloatingObjects.Remove(floatingObject);
			m_floatingObjectsToSyncCreate.Remove(floatingObject);
			m_highPriority.Remove(floatingObject);
			m_normalPriority.Remove(floatingObject);
			m_lowPriority.Remove(floatingObject);
		}

		private void UpdateClosestDistancesToPlayers()
		{
			foreach (MyFloatingObject synchronizedFloatingObject in m_synchronizedFloatingObjects)
			{
				if (synchronizedFloatingObject.ClosestDistanceToAnyPlayerSquared != -1f)
				{
					synchronizedFloatingObject.ClosestDistanceToAnyPlayerSquared = float.MaxValue;
					foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
					{
						if (onlinePlayer.Identity.Character != null)
						{
							float num = (float)Vector3D.DistanceSquared(synchronizedFloatingObject.PositionComp.GetPosition(), onlinePlayer.Identity.Character.PositionComp.GetPosition());
							if (num < synchronizedFloatingObject.ClosestDistanceToAnyPlayerSquared)
							{
								synchronizedFloatingObject.ClosestDistanceToAnyPlayerSquared = num;
							}
						}
					}
				}
			}
		}

		public static MyObjectBuilder_FloatingObject ChangeObjectBuilder(MyComponentDefinition componentDef, MyObjectBuilder_EntityBase entityOb)
		{
			MyObjectBuilder_PhysicalObject content = MyObjectBuilderSerializer.CreateNewObject(componentDef.Id.TypeId, componentDef.Id.SubtypeName) as MyObjectBuilder_PhysicalObject;
			Vector3 up = entityOb.PositionAndOrientation.Value.Up;
			Vector3 forward = entityOb.PositionAndOrientation.Value.Forward;
			Vector3D position = entityOb.PositionAndOrientation.Value.Position;
			MyPhysicalInventoryItem item = new MyPhysicalInventoryItem(1, content);
			MyObjectBuilder_FloatingObject myObjectBuilder_FloatingObject = PrepareBuilder(ref item);
			myObjectBuilder_FloatingObject.PositionAndOrientation = new MyPositionAndOrientation(position, forward, up);
			myObjectBuilder_FloatingObject.EntityId = entityOb.EntityId;
			return myObjectBuilder_FloatingObject;
		}

		public static void RequestSpawnCreative(MyObjectBuilder_FloatingObject obj)
		{
			if (MySession.Static.HasCreativeRights || MySession.Static.CreativeMode)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestSpawnCreative_Implementation, obj);
			}
		}

		[Event(null, 757)]
		[Reliable]
		[Server]
		private static void RequestSpawnCreative_Implementation(MyObjectBuilder_FloatingObject obj)
		{
			if (MySession.Static.CreativeMode || MyEventContext.Current.IsLocallyInvoked || MySession.Static.HasPlayerCreativeRights(MyEventContext.Current.Sender.Value))
			{
				MyEntities.CreateFromObjectBuilderAndAdd(obj, fadeIn: false);
			}
			else
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}
	}
}
