using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Generics
{
	public class MyConcurrentObjectsPool<T> where T : class, new()
	{
		private FastResourceLock m_lock = new FastResourceLock();

		private MyQueue<T> m_unused;

		private HashSet<T> m_active;

		private HashSet<T> m_marked;

		private int m_baseCapacity;

		public int ActiveCount
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_active.Count;
				}
			}
		}

		public int BaseCapacity
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					m_lock.AcquireShared();
					return m_baseCapacity;
				}
			}
		}

		public int Capacity
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					m_lock.AcquireShared();
					return m_unused.Count + m_active.Count;
				}
			}
		}

		public void ApplyActionOnAllActives(Action<T> action)
		{
			using (m_lock.AcquireSharedUsing())
			{
				foreach (T item in m_active)
				{
					action(item);
				}
			}
		}

		private MyConcurrentObjectsPool()
		{
		}

		public MyConcurrentObjectsPool(int baseCapacity)
		{
			m_baseCapacity = baseCapacity;
			m_unused = new MyQueue<T>(m_baseCapacity);
			m_active = new HashSet<T>();
			m_marked = new HashSet<T>();
			for (int i = 0; i < m_baseCapacity; i++)
			{
				m_unused.Enqueue(new T());
			}
		}

		/// <summary>
		/// Returns true when new item was allocated
		/// </summary>
		public bool AllocateOrCreate(out T item)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				bool flag = m_unused.Count == 0;
				if (flag)
				{
					item = new T();
				}
				else
				{
					item = m_unused.Dequeue();
				}
				m_active.Add(item);
				return flag;
			}
		}

		public T Allocate(bool nullAllowed = false)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				T val = null;
				if (m_unused.Count > 0)
				{
					val = m_unused.Dequeue();
					m_active.Add(val);
				}
				return val;
			}
		}

		public void Deallocate(T item)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_active.Remove(item);
				m_unused.Enqueue(item);
			}
		}

		public void MarkForDeallocate(T item)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_marked.Add(item);
			}
		}

		public void DeallocateAllMarked()
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				foreach (T item in m_marked)
				{
					m_active.Remove(item);
					m_unused.Enqueue(item);
				}
				m_marked.Clear();
			}
		}

		public void DeallocateAll()
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				foreach (T item in m_active)
				{
					m_unused.Enqueue(item);
				}
				m_active.Clear();
				m_marked.Clear();
			}
		}
	}
}
