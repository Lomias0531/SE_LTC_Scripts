using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Generics
{
	/// This class provides similar functionality to MyDynamicObjectsPool with the addition of caching facilities.
	///
	/// The cache is intended to be used for objects that once allocated either perform expensive computations
	/// or allocate a lot of memory *and* that may be needed again after disposed in the same state.
	public class MyCachingDynamicObjectsPool<ObjectKey, ObjectType> where ObjectType : IDisposable, new()
	{
		private static readonly int DEFAULT_POOL_SIZE = 64;

		private static readonly int DEFAULT_CACHE_SIZE = 8;

		private static readonly int DEFAULT_POOL_GROWTH = 1;

		private int m_cacheSize;

		private int m_poolGrowth;

		private Dictionary<ObjectKey, ObjectType> m_cache;

		private MyQueue<ObjectKey> m_entryAge;

		private Stack<ObjectType> m_objectPool;

		public MyCachingDynamicObjectsPool()
			: this(DEFAULT_POOL_SIZE, DEFAULT_CACHE_SIZE, DEFAULT_POOL_GROWTH)
		{
		}

		public MyCachingDynamicObjectsPool(int poolSize)
			: this(poolSize, DEFAULT_CACHE_SIZE, DEFAULT_POOL_GROWTH)
		{
		}

		public MyCachingDynamicObjectsPool(int poolSize, int cacheSize)
			: this(poolSize, cacheSize, DEFAULT_POOL_GROWTH)
		{
		}

		public MyCachingDynamicObjectsPool(int poolSize, int cacheSize, int poolGrowth)
		{
			m_cacheSize = cacheSize;
			m_poolGrowth = poolGrowth;
			m_cache = new Dictionary<ObjectKey, ObjectType>(m_cacheSize);
			m_objectPool = new Stack<ObjectType>(poolSize);
			m_entryAge = new MyQueue<ObjectKey>(m_cacheSize);
			Restock(poolSize);
		}

		public ObjectType Allocate()
		{
			if (m_objectPool.Count > 0)
			{
				return m_objectPool.Pop();
			}
			if (m_entryAge.Count > 0)
			{
				ObjectKey key = m_entryAge.Dequeue();
				ObjectType result = m_cache[key];
				m_cache.Remove(key);
				result.Dispose();
				return result;
			}
			Restock(m_poolGrowth);
			return m_objectPool.Pop();
		}

		/// Deallocate object without key.
		///
		/// Object is disposed be callee.
		public void Deallocate(ObjectType obj)
		{
			obj.Dispose();
			m_objectPool.Push(obj);
		}

		/// Deallocate object with key.
		///
		/// Object is cached and disposed if necessary.
		public void Deallocate(ObjectKey key, ObjectType obj)
		{
			if (m_entryAge.Count == m_cacheSize)
			{
				ObjectKey key2 = m_entryAge.Dequeue();
				ObjectType obj2 = m_cache[key2];
				m_cache.Remove(key2);
				Deallocate(obj2);
			}
			m_entryAge.Enqueue(key);
			m_cache.Add(key, obj);
		}

		/// Allocate an object that may be cached.
		///
		/// Returns true if the object was found in the cache and false otherwise.
		public bool TryAllocateCached(ObjectKey key, out ObjectType obj)
		{
			if (!m_cache.TryGetValue(key, out obj))
			{
				obj = Allocate();
				return false;
			}
			m_entryAge.Remove(key);
			obj = m_cache[key];
			m_cache.Remove(key);
			return true;
		}

		private void Restock(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				m_objectPool.Push(new ObjectType());
			}
		}
	}
}
