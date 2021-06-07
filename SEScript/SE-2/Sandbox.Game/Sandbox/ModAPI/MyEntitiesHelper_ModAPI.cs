using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Scripting;
using VRage.Utils;
using VRageMath;

namespace Sandbox.ModAPI
{
	public class MyEntitiesHelper_ModAPI : IMyEntities
	{
		private List<MyEntity> m_entityList = new List<MyEntity>();

		event Action<IMyEntity> IMyEntities.OnEntityRemove
		{
			add
			{
				MyEntities.OnEntityRemove += GetDelegate(value);
			}
			remove
			{
				MyEntities.OnEntityRemove -= GetDelegate(value);
			}
		}

		event Action<IMyEntity> IMyEntities.OnEntityAdd
		{
			add
			{
				MyEntities.OnEntityAdd += GetDelegate(value);
			}
			remove
			{
				MyEntities.OnEntityAdd -= GetDelegate(value);
			}
		}

		event Action IMyEntities.OnCloseAll
		{
			add
			{
				MyEntities.OnCloseAll += value;
			}
			remove
			{
				MyEntities.OnCloseAll -= value;
			}
		}

		event Action<IMyEntity, string, string> IMyEntities.OnEntityNameSet
		{
			add
			{
				MyEntities.OnEntityNameSet += GetDelegate(value);
			}
			remove
			{
				MyEntities.OnEntityNameSet -= GetDelegate(value);
			}
		}

		void IMyEntities.GetEntities(HashSet<IMyEntity> entities, Func<IMyEntity, bool> collect)
		{
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				if (collect == null || collect(entity))
				{
					entities.Add(entity);
				}
			}
		}

		bool IMyEntities.TryGetEntityById(long id, out IMyEntity entity)
		{
			MyEntity entity2;
			bool result = MyEntities.TryGetEntityById(id, out entity2);
			entity = entity2;
			return result;
		}

		bool IMyEntities.TryGetEntityById(long? id, out IMyEntity entity)
		{
			entity = null;
			bool result = false;
			if (id.HasValue)
			{
				result = MyEntities.TryGetEntityById(id.Value, out MyEntity entity2);
				entity = entity2;
			}
			return result;
		}

		bool IMyEntities.TryGetEntityByName(string name, out IMyEntity entity)
		{
			MyEntity entity2;
			bool result = MyEntities.TryGetEntityByName(name, out entity2);
			entity = entity2;
			return result;
		}

		bool IMyEntities.EntityExists(string name)
		{
			return MyEntities.EntityExists(name);
		}

		void IMyEntities.AddEntity(IMyEntity entity, bool insertIntoScene)
		{
			if (entity is MyEntity)
			{
				MyEntities.Add(entity as MyEntity, insertIntoScene);
			}
		}

		IMyEntity IMyEntities.CreateFromObjectBuilder(MyObjectBuilder_EntityBase objectBuilder)
		{
			return MyEntities.CreateFromObjectBuilder(objectBuilder, fadeIn: false);
		}

		IMyEntity IMyEntities.CreateFromObjectBuilderAndAdd(MyObjectBuilder_EntityBase objectBuilder)
		{
			return MyEntities.CreateFromObjectBuilderAndAdd(objectBuilder, fadeIn: false);
		}

		void IMyEntities.RemoveEntity(IMyEntity entity)
		{
			MyEntities.Remove(entity as MyEntity);
		}

		private Action<MyEntity> GetDelegate(Action<IMyEntity> value)
		{
			return (Action<MyEntity>)Delegate.CreateDelegate(typeof(Action<MyEntity>), value.Target, value.Method);
		}

		private Action<MyEntity, string, string> GetDelegate(Action<IMyEntity, string, string> value)
		{
			return (Action<MyEntity, string, string>)Delegate.CreateDelegate(typeof(Action<MyEntity, string, string>), value.Target, value.Method);
		}

		bool IMyEntities.IsSpherePenetrating(ref BoundingSphereD bs)
		{
			return MyEntities.IsSpherePenetrating(ref bs);
		}

		Vector3D? IMyEntities.FindFreePlace(Vector3D basePos, float radius, int maxTestCount, int testsPerDistance, float stepSize)
		{
			return MyEntities.FindFreePlace(basePos, radius, maxTestCount, testsPerDistance, stepSize);
		}

		[Obsolete]
		void IMyEntities.GetInflatedPlayerBoundingBox(ref BoundingBox playerBox, float inflation)
		{
			BoundingBoxD playerBox2 = BoundingBoxD.CreateInvalid();
			MyEntities.GetInflatedPlayerBoundingBox(ref playerBox2, inflation);
			playerBox = (BoundingBox)playerBox2;
		}

		void IMyEntities.GetInflatedPlayerBoundingBox(ref BoundingBoxD playerBox, float inflation)
		{
			MyEntities.GetInflatedPlayerBoundingBox(ref playerBox, inflation);
		}

		[Obsolete]
		bool IMyEntities.IsInsideVoxel(Vector3 pos, Vector3 hintPosition, out Vector3 lastOutsidePos)
		{
			Vector3D lastOutsidePos2;
			bool result = MyEntities.IsInsideVoxel(pos, hintPosition, out lastOutsidePos2);
			lastOutsidePos = lastOutsidePos2;
			return result;
		}

		bool IMyEntities.IsInsideVoxel(Vector3D pos, Vector3D hintPosition, out Vector3D lastOutsidePos)
		{
			return MyEntities.IsInsideVoxel(pos, hintPosition, out lastOutsidePos);
		}

		bool IMyEntities.IsWorldLimited()
		{
			return MyEntities.IsWorldLimited();
		}

		float IMyEntities.WorldHalfExtent()
		{
			return MyEntities.WorldHalfExtent();
		}

		float IMyEntities.WorldSafeHalfExtent()
		{
			return MyEntities.WorldSafeHalfExtent();
		}

		bool IMyEntities.IsInsideWorld(Vector3D pos)
		{
			return MyEntities.IsInsideWorld(pos);
		}

		bool IMyEntities.IsRaycastBlocked(Vector3D pos, Vector3D target)
		{
			return MyEntities.IsRaycastBlocked(pos, target);
		}

		List<IMyEntity> IMyEntities.GetEntitiesInAABB(ref BoundingBoxD boundingBox)
		{
			List<MyEntity> entitiesInAABB = MyEntities.GetEntitiesInAABB(ref boundingBox);
			List<IMyEntity> list = new List<IMyEntity>(entitiesInAABB.Count);
			foreach (MyEntity item in entitiesInAABB)
			{
				list.Add(item);
			}
			entitiesInAABB.Clear();
			return list;
		}

		List<IMyEntity> IMyEntities.GetEntitiesInSphere(ref BoundingSphereD boundingSphere)
		{
			List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
			List<IMyEntity> list = new List<IMyEntity>(entitiesInSphere.Count);
			foreach (MyEntity item in entitiesInSphere)
			{
				list.Add(item);
			}
			entitiesInSphere.Clear();
			return list;
		}

		List<IMyEntity> IMyEntities.GetTopMostEntitiesInSphere(ref BoundingSphereD boundingSphere)
		{
			List<MyEntity> topMostEntitiesInSphere = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere);
			List<IMyEntity> list = new List<IMyEntity>(topMostEntitiesInSphere.Count);
			foreach (MyEntity item in topMostEntitiesInSphere)
			{
				list.Add(item);
			}
			topMostEntitiesInSphere.Clear();
			return list;
		}

		List<IMyEntity> IMyEntities.GetElementsInBox(ref BoundingBoxD boundingBox)
		{
			m_entityList.Clear();
			MyEntities.GetElementsInBox(ref boundingBox, m_entityList);
			List<IMyEntity> list = new List<IMyEntity>(m_entityList.Count);
			foreach (MyEntity entity in m_entityList)
			{
				list.Add(entity);
			}
			return list;
		}

		List<IMyEntity> IMyEntities.GetTopMostEntitiesInBox(ref BoundingBoxD boundingBox)
		{
			m_entityList.Clear();
			MyEntities.GetTopMostEntitiesInBox(ref boundingBox, m_entityList);
			List<IMyEntity> list = new List<IMyEntity>(m_entityList.Count);
			foreach (MyEntity entity in m_entityList)
			{
				list.Add(entity);
			}
			return list;
		}

		IMyEntity IMyEntities.CreateFromObjectBuilderParallel(MyObjectBuilder_EntityBase objectBuilder, bool addToScene, Action<IMyEntity> completionCallback)
		{
			return MyEntities.CreateFromObjectBuilderParallel(objectBuilder, addToScene, completionCallback);
		}

		void IMyEntities.SetEntityName(IMyEntity entity, bool possibleRename)
		{
			if (entity is MyEntity)
			{
				MyEntities.SetEntityName(entity as MyEntity, possibleRename);
			}
		}

		bool IMyEntities.IsNameExists(IMyEntity entity, string name)
		{
			if (entity is MyEntity)
			{
				return MyEntities.IsNameExists(entity as MyEntity, name);
			}
			return false;
		}

		void IMyEntities.RemoveFromClosedEntities(IMyEntity entity)
		{
			if (entity is MyEntity)
			{
				MyEntities.RemoveFromClosedEntities(entity as MyEntity);
			}
		}

		void IMyEntities.RemoveName(IMyEntity entity)
		{
			if (!string.IsNullOrEmpty(entity.Name))
			{
				MyEntities.m_entityNameDictionary.Remove(entity.Name);
			}
		}

		bool IMyEntities.Exist(IMyEntity entity)
		{
			if (entity is MyEntity)
			{
				return MyEntities.Exist(entity as MyEntity);
			}
			return false;
		}

		void IMyEntities.MarkForClose(IMyEntity entity)
		{
			if (entity is MyEntity)
			{
				MyEntities.Close(entity as MyEntity);
			}
		}

		void IMyEntities.RegisterForUpdate(IMyEntity entity)
		{
			MyEntity e = entity as MyEntity;
			if (e != null)
			{
				if (Thread.CurrentThread == MyUtils.MainThread)
				{
					MyEntities.RegisterForUpdate(e);
					return;
				}
				MyModWatchdog.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_EngineParallelAccess);
				MySandboxGame.Static.Invoke(delegate
				{
					MyEntities.RegisterForUpdate(e);
				}, "RegisterForUpdate");
			}
		}

		void IMyEntities.RegisterForDraw(IMyEntity entity)
		{
			MyEntity myEntity = entity as MyEntity;
			if (myEntity != null)
			{
				MyEntities.RegisterForDraw(myEntity);
			}
		}

		void IMyEntities.UnregisterForUpdate(IMyEntity entity, bool immediate)
		{
			MyEntity e = entity as MyEntity;
			if (e != null)
			{
				if (Thread.CurrentThread == MyUtils.MainThread)
				{
					MyEntities.UnregisterForUpdate(e, immediate);
					return;
				}
				MyModWatchdog.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_EngineParallelAccess);
				MySandboxGame.Static.Invoke(delegate
				{
					MyEntities.UnregisterForUpdate(e, immediate);
				}, "UnregisterForUpdate");
			}
		}

		void IMyEntities.UnregisterForDraw(IMyEntity entity)
		{
			MyEntity myEntity = entity as MyEntity;
			if (myEntity != null)
			{
				MyEntities.UnregisterForDraw(myEntity);
			}
		}

		IMyEntity IMyEntities.GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			return MyEntities.GetIntersectionWithSphere(ref sphere);
		}

		IMyEntity IMyEntities.GetIntersectionWithSphere(ref BoundingSphereD sphere, IMyEntity ignoreEntity0, IMyEntity ignoreEntity1)
		{
			return MyEntities.GetIntersectionWithSphere(ref sphere, ignoreEntity0 as MyEntity, ignoreEntity1 as MyEntity);
		}

		List<IMyEntity> IMyEntities.GetIntersectionWithSphere(ref BoundingSphereD sphere, IMyEntity ignoreEntity0, IMyEntity ignoreEntity1, bool ignoreVoxelMaps, bool volumetricTest)
		{
			m_entityList.Clear();
			MyEntities.GetIntersectionWithSphere(ref sphere, ignoreEntity0 as MyEntity, ignoreEntity1 as MyEntity, ignoreVoxelMaps, volumetricTest, ref m_entityList);
			List<IMyEntity> list = new List<IMyEntity>(m_entityList.Count);
			foreach (MyEntity entity in m_entityList)
			{
				list.Add(entity);
			}
			return list;
		}

		IMyEntity IMyEntities.GetIntersectionWithSphere(ref BoundingSphereD sphere, IMyEntity ignoreEntity0, IMyEntity ignoreEntity1, bool ignoreVoxelMaps, bool volumetricTest, bool excludeEntitiesWithDisabledPhysics, bool ignoreFloatingObjects, bool ignoreHandWeapons)
		{
			return MyEntities.GetIntersectionWithSphere(ref sphere, ignoreEntity0 as MyEntity, ignoreEntity1 as MyEntity, ignoreVoxelMaps, volumetricTest, excludeEntitiesWithDisabledPhysics, ignoreFloatingObjects, ignoreHandWeapons);
		}

		IMyEntity IMyEntities.GetEntityById(long entityId)
		{
			if (!MyEntities.EntityExists(entityId))
			{
				return null;
			}
			return MyEntities.GetEntityById(entityId);
		}

		IMyEntity IMyEntities.GetEntityById(long? entityId)
		{
			if (!entityId.HasValue)
			{
				return null;
			}
			return MyEntities.GetEntityById(entityId.Value);
		}

		bool IMyEntities.EntityExists(long entityId)
		{
			return MyEntities.EntityExists(entityId);
		}

		bool IMyEntities.EntityExists(long? entityId)
		{
			if (entityId.HasValue)
			{
				return MyEntities.EntityExists(entityId.Value);
			}
			return false;
		}

		IMyEntity IMyEntities.GetEntityByName(string name)
		{
			return MyEntities.GetEntityByName(name);
		}

		void IMyEntities.SetTypeHidden(Type type, bool hidden)
		{
			MyEntities.SetTypeHidden(type, hidden);
		}

		bool IMyEntities.IsTypeHidden(Type type)
		{
			return MyEntities.IsTypeHidden(type);
		}

		bool IMyEntities.IsVisible(IMyEntity entity)
		{
			return ((IMyEntities)this).IsTypeHidden(entity.GetType());
		}

		void IMyEntities.UnhideAllTypes()
		{
			MyEntities.UnhideAllTypes();
		}

		void IMyEntities.RemapObjectBuilderCollection(IEnumerable<MyObjectBuilder_EntityBase> objectBuilders)
		{
			MyEntities.RemapObjectBuilderCollection(objectBuilders);
		}

		void IMyEntities.RemapObjectBuilder(MyObjectBuilder_EntityBase objectBuilder)
		{
			MyEntities.RemapObjectBuilder(objectBuilder);
		}

		IMyEntity IMyEntities.CreateFromObjectBuilderNoinit(MyObjectBuilder_EntityBase objectBuilder)
		{
			return MyEntities.CreateFromObjectBuilderNoinit(objectBuilder);
		}

		void IMyEntities.EnableEntityBoundingBoxDraw(IMyEntity entity, bool enable, Vector4? color, float lineWidth, Vector3? inflateAmount)
		{
			if (entity is MyEntity)
			{
				if (Thread.CurrentThread == MyUtils.MainThread)
				{
					MyEntities.EnableEntityBoundingBoxDraw(entity as MyEntity, enable, color, lineWidth, inflateAmount);
					return;
				}
				MyModWatchdog.ReportIncorrectBehaviour(MyCommonTexts.ModRuleViolation_EngineParallelAccess);
				MySandboxGame.Static.Invoke(delegate
				{
					MyEntities.EnableEntityBoundingBoxDraw(entity as MyEntity, enable, color, lineWidth, inflateAmount);
				}, "EnableEntityBoundingBoxDraw");
			}
		}

		IMyEntity IMyEntities.GetEntity(Func<IMyEntity, bool> match)
		{
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				if (match(entity))
				{
					return entity;
				}
			}
			return null;
		}
	}
}
