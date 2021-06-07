using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;

namespace VRage
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct MyEntityIdentifier
	{
		private class PerThreadData
		{
			public bool AllocationSuspended;

			public Dictionary<long, IMyEntity> EntityList;

			public PerThreadData(int defaultCapacity)
			{
				EntityList = new Dictionary<long, IMyEntity>(defaultCapacity);
			}
		}

		public enum ID_OBJECT_TYPE : byte
		{
			UNKNOWN,
			ENTITY,
			IDENTITY,
			FACTION,
			NPC,
			SPAWN_GROUP,
			ASTEROID,
			PLANET,
			VOXEL_PHYSICS,
			PLANET_ENVIRONMENT_SECTOR,
			PLANET_ENVIRONMENT_ITEM,
			PLANET_VOXEL_DETAIL,
			STATION,
			CONTRACT,
			CONTRACT_CONDITION,
			STORE_ITEM
		}

		public enum ID_ALLOCATION_METHOD : byte
		{
			RANDOM,
			SERIAL_START_WITH_1
		}

		private const int DEFAULT_DICTIONARY_SIZE = 32768;

		[ThreadStatic]
		private static bool m_inEntityCreationBlock;

		[ThreadStatic]
		private static PerThreadData m_perThreadData;

		private static PerThreadData m_mainData;

		private static bool m_singleThreadClearWarnings;

		private static bool m_isSwapPrepared;

		private static bool m_isSwapped;

		private static PerThreadData m_perThreadData_Swap;

		private static long[] m_lastGeneratedIds;

		public static bool InEntityCreationBlock
		{
			get
			{
				return m_inEntityCreationBlock;
			}
			set
			{
				if (value)
				{
					_ = MyUtils.MainThread;
					_ = Thread.CurrentThread;
				}
				m_inEntityCreationBlock = value;
			}
		}

		private static Dictionary<long, IMyEntity> EntityList => (m_perThreadData ?? m_mainData).EntityList;

		/// <summary>
		/// Freezes allocating entity ids.
		/// This is important, because during load, no entity cannot allocate new id, because it could allocate id which already has entity which will be loaded soon.
		/// </summary>
		public static bool AllocationSuspended
		{
			get
			{
				return (m_perThreadData ?? m_mainData).AllocationSuspended;
			}
			set
			{
				(m_perThreadData ?? m_mainData).AllocationSuspended = value;
			}
		}

		public static void SetSingleThreadClearWarnings(bool enable)
		{
			m_singleThreadClearWarnings = enable;
		}

		public static bool SwapPerThreadData()
		{
			PerThreadData perThreadData = m_perThreadData;
			m_perThreadData = m_perThreadData_Swap;
			m_perThreadData_Swap = perThreadData;
			m_isSwapped = !m_isSwapped;
			return m_isSwapped;
		}

		public static void PrepareSwapData()
		{
			m_isSwapPrepared = true;
			m_perThreadData_Swap = new PerThreadData(32768);
		}

		public static void ClearSwapDataAndRestore()
		{
			if (m_isSwapped)
			{
				m_perThreadData = m_perThreadData_Swap;
				m_perThreadData_Swap = null;
				m_isSwapped = false;
				m_isSwapPrepared = false;
			}
			else
			{
				m_perThreadData_Swap = null;
				m_isSwapPrepared = false;
			}
		}

		static MyEntityIdentifier()
		{
			m_singleThreadClearWarnings = false;
			m_isSwapPrepared = false;
			m_isSwapped = false;
			m_perThreadData_Swap = null;
			m_lastGeneratedIds = new long[(uint)(MyEnum<ID_OBJECT_TYPE>.Range.Max + 1)];
			m_mainData = new PerThreadData(32768);
			m_perThreadData = m_mainData;
		}

		public static void InitPerThreadStorage(int defaultCapacity)
		{
			m_perThreadData = new PerThreadData(defaultCapacity);
		}

		public static void LazyInitPerThreadStorage(int defaultCapacity)
		{
			if (m_perThreadData == null || m_perThreadData == m_mainData)
			{
				m_perThreadData = new PerThreadData(defaultCapacity);
			}
		}

		public static void DestroyPerThreadStorage()
		{
			m_perThreadData = null;
		}

		public static void GetPerThreadEntities(List<IMyEntity> result)
		{
			foreach (KeyValuePair<long, IMyEntity> entity in m_perThreadData.EntityList)
			{
				result.Add(entity.Value);
			}
		}

		public static void ClearPerThreadEntities()
		{
			if (m_singleThreadClearWarnings)
			{
				_ = m_isSwapped;
			}
			m_perThreadData.EntityList.Clear();
		}

		public static void Reset()
		{
			Array.Clear(m_lastGeneratedIds, 0, m_lastGeneratedIds.Length);
		}

		/// <summary>
		/// This method is used when loading existing entity IDs to track the last generated ID
		/// </summary>
		public static void MarkIdUsed(long id)
		{
			long idUniqueNumber = GetIdUniqueNumber(id);
			ID_OBJECT_TYPE idObjectType = GetIdObjectType(id);
			MyUtils.InterlockedMax(ref m_lastGeneratedIds[(uint)idObjectType], idUniqueNumber);
		}

		/// <summary>
		/// Registers entity with given ID. Do not call this directly, it is called automatically
		/// when EntityID is first time assigned.
		/// </summary>
		/// <param name="entity"></param>
		public static void AddEntityWithId(IMyEntity entity)
		{
			if (EntityList.ContainsKey(entity.EntityId))
			{
				throw new DuplicateIdException(entity, EntityList[entity.EntityId]);
			}
			EntityList.Add(entity.EntityId, entity);
		}

		/// <summary>
		/// Allocated new entity ID (won't add to list)
		/// Entity with this ID should be added immediatelly
		/// </summary>
		public static long AllocateId(ID_OBJECT_TYPE objectType = ID_OBJECT_TYPE.ENTITY, ID_ALLOCATION_METHOD generationMethod = ID_ALLOCATION_METHOD.RANDOM)
		{
			long uniqueNumber = (generationMethod != 0) ? Interlocked.Increment(ref m_lastGeneratedIds[(uint)objectType]) : (MyRandom.Instance.NextLong() & 0xFFFFFFFFFFFFFF);
			return ConstructId(objectType, uniqueNumber);
		}

		public static ID_OBJECT_TYPE GetIdObjectType(long id)
		{
			return (ID_OBJECT_TYPE)(id >> 56);
		}

		public static long GetIdUniqueNumber(long id)
		{
			return id & 0xFFFFFFFFFFFFFF;
		}

		/// Construct an ID using the hash from a string.
		public static long ConstructIdFromString(ID_OBJECT_TYPE type, string uniqueString)
		{
			long hashCode = uniqueString.GetHashCode64();
			hashCode = (hashCode >> 8) + hashCode + (hashCode << 13);
			return (hashCode & 0xFFFFFFFFFFFFFF) | (long)((ulong)type << 56);
		}

		public static long ConstructId(ID_OBJECT_TYPE type, long uniqueNumber)
		{
			return (uniqueNumber & 0xFFFFFFFFFFFFFF) | (long)((ulong)type << 56);
		}

		public static long FixObsoleteIdentityType(long id)
		{
			if (GetIdObjectType(id) == ID_OBJECT_TYPE.NPC || GetIdObjectType(id) == ID_OBJECT_TYPE.SPAWN_GROUP)
			{
				id = ConstructId(ID_OBJECT_TYPE.IDENTITY, GetIdUniqueNumber(id));
			}
			return id;
		}

		public static void RemoveEntity(long entityId)
		{
			EntityList.Remove(entityId);
		}

		public static IMyEntity GetEntityById(long entityId, bool allowClosed = false)
		{
			if (!EntityList.TryGetValue(entityId, out IMyEntity value) && m_perThreadData != null)
			{
				m_mainData.EntityList.TryGetValue(entityId, out value);
			}
			if (value != null && !allowClosed && value.GetTopMostParent().Closed)
			{
				return null;
			}
			return value;
		}

		public static bool TryGetEntity(long entityId, out IMyEntity entity, bool allowClosed = false)
		{
			bool flag = EntityList.TryGetValue(entityId, out entity);
			if (!flag && m_perThreadData != null)
			{
				flag = m_mainData.EntityList.TryGetValue(entityId, out entity);
			}
			if (entity != null && !allowClosed && entity.GetTopMostParent().Closed)
			{
				entity = null;
				flag = false;
			}
			return flag;
		}

		public static bool TryGetEntity<T>(long entityId, out T entity, bool allowClosed = false) where T : class, IMyEntity
		{
			IMyEntity entity2;
			bool num = TryGetEntity(entityId, out entity2, allowClosed);
			entity = (entity2 as T);
			if (num)
			{
				return entity != null;
			}
			return false;
		}

		public static bool ExistsById(long entityId)
		{
			if (!EntityList.ContainsKey(entityId))
			{
				if (m_perThreadData != null)
				{
					return m_mainData.EntityList.ContainsKey(entityId);
				}
				return false;
			}
			return true;
		}

		/// <summary>
		/// Changes ID by which an entity is registered. Do not call this directly, it is called automatically when
		/// EntityID changes.
		/// </summary>
		/// <param name="entity">Entity whose ID has changed.</param>
		/// <param name="oldId">Old ID of the entity.</param>
		/// <param name="newId">New ID of the entity.</param>
		public static void SwapRegisteredEntityId(IMyEntity entity, long oldId, long newId)
		{
			if (EntityList.TryGetValue(oldId, out IMyEntity _))
			{
				EntityList.Remove(oldId);
			}
			EntityList[newId] = entity;
		}

		public static void Clear()
		{
			EntityList.Clear();
		}
	}
}
