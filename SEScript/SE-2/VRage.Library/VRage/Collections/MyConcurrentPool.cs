using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VRage.Collections
{
	/// <summary>
	/// Simple thread-safe pool.
	/// Can store external objects by calling return.
	/// Creates new instances when empty.
	/// </summary>
	public class MyConcurrentPool<T> : IConcurrentPool where T : new()
	{
		private readonly int m_expectedAllocations;

		private readonly Action<T> m_clear;

		private readonly Stack<T> m_instances;

		private readonly Func<T> m_activator;

		private readonly Action<T> m_deactivator;

		public int Allocated
		{
			get;
			set;
		}

		public int Count
		{
			get
			{
				lock (m_instances)
				{
					return m_instances.Count;
				}
			}
		}

		public MyConcurrentPool(int defaultCapacity = 0, Action<T> clear = null, int expectedAllocations = 10000, Func<T> activator = null, Action<T> deactivator = null)
		{
			m_clear = clear;
			m_expectedAllocations = expectedAllocations;
			m_instances = new Stack<T>(defaultCapacity);
			m_activator = (activator ?? ExpressionExtension.CreateActivator<T>());
			m_deactivator = deactivator;
			if (defaultCapacity > 0)
			{
				Allocated = defaultCapacity;
				for (int i = 0; i < defaultCapacity; i++)
				{
					m_instances.Push(m_activator());
				}
			}
		}

		public T Get()
		{
			lock (m_instances)
			{
				if (m_instances.Count > 0)
				{
					return m_instances.Pop();
				}
			}
			return m_activator();
		}

		public void Return(T instance)
		{
			m_clear?.Invoke(instance);
			lock (m_instances)
			{
				m_instances.Push(instance);
			}
		}

		public void Clean()
		{
			lock (m_instances)
			{
				if (m_deactivator != null)
				{
					while (m_instances.Count > 0)
					{
						m_deactivator(m_instances.Pop());
					}
				}
				else
				{
					m_instances.Clear();
				}
			}
		}

		void IConcurrentPool.Return(object obj)
		{
			Return((T)obj);
		}

		object IConcurrentPool.Get()
		{
			return Get();
		}
	}
}
