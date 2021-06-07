using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRage.Collections;
using VRage.Library.Threading;

namespace VRage.Generics
{
	public class MyObjectsPool<T> where T : class, new()
	{
		private MyConcurrentQueue<T> m_unused;

		private HashSet<T> m_active;

		private HashSet<T> m_marked;

		private SpinLockRef m_activeLock = new SpinLockRef();

		private Func<T> m_activator;

		private int m_baseCapacity;

		public SpinLockRef ActiveLock => m_activeLock;

		public HashSetReader<T> ActiveWithoutLock => new HashSetReader<T>(m_active);

		public HashSetReader<T> Active
		{
			get
			{
				using (m_activeLock.Acquire())
				{
					return new HashSetReader<T>(m_active);
				}
			}
		}

		public int ActiveCount
		{
			get
			{
				using (m_activeLock.Acquire())
				{
					return m_active.Count;
				}
			}
		}

		public int BaseCapacity => m_baseCapacity;

		public int Capacity
		{
			get
			{
				using (m_activeLock.Acquire())
				{
					return m_unused.Count + m_active.Count;
				}
			}
		}

		public MyObjectsPool(int baseCapacity, Func<T> activator = null)
		{
			m_activator = (activator ?? ExpressionExtension.CreateActivator<T>());
			m_baseCapacity = baseCapacity;
			m_unused = new MyConcurrentQueue<T>(m_baseCapacity);
			m_active = new HashSet<T>();
			m_marked = new HashSet<T>();
			for (int i = 0; i < m_baseCapacity; i++)
			{
				m_unused.Enqueue(m_activator());
			}
		}

		/// <summary>
		/// Returns true when new item was allocated
		/// </summary>
		public bool AllocateOrCreate(out T item)
		{
			bool flag = false;
			using (m_activeLock.Acquire())
			{
				flag = (m_unused.Count == 0);
				if (flag)
				{
					item = m_activator();
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
			T result = null;
			using (m_activeLock.Acquire())
			{
				if (m_unused.Count <= 0)
				{
					return result;
				}
				result = m_unused.Dequeue();
				m_active.Add(result);
				return result;
			}
		}

		public void Deallocate(T item)
		{
			using (m_activeLock.Acquire())
			{
				m_active.Remove(item);
				m_unused.Enqueue(item);
			}
		}

		public void MarkForDeallocate(T item)
		{
			using (m_activeLock.Acquire())
			{
				m_marked.Add(item);
			}
		}

		public void MarkAllActiveForDeallocate()
		{
			using (m_activeLock.Acquire())
			{
				m_marked.UnionWith(m_active);
			}
		}

		public void DeallocateAllMarked()
		{
			using (m_activeLock.Acquire())
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
			using (m_activeLock.Acquire())
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
