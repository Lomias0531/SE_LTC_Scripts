using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRage.Collections;

namespace VRage.Generics
{
	public class MyRuntimeObjectsPool<TPool> where TPool : class
	{
		private readonly Queue<TPool> m_unused;

		private readonly Func<TPool> m_constructor;

		private readonly HashSet<TPool> m_active;

		private readonly HashSet<TPool> m_marked;

		private readonly int m_baseCapacity;

		public QueueReader<TPool> Unused => new QueueReader<TPool>(m_unused);

		public HashSetReader<TPool> Active => new HashSetReader<TPool>(m_active);

		public int ActiveCount => m_active.Count;

		public int BaseCapacity => m_baseCapacity;

		public int Capacity => m_unused.Count + m_active.Count;

		public MyRuntimeObjectsPool(int baseCapacity, Type type)
			: this(baseCapacity, ExpressionExtension.CreateActivator<TPool>(type))
		{
		}

		public MyRuntimeObjectsPool(int baseCapacity, Func<TPool> constructor)
		{
			m_constructor = constructor;
			m_baseCapacity = baseCapacity;
			m_unused = new Queue<TPool>(m_baseCapacity);
			m_active = new HashSet<TPool>();
			m_marked = new HashSet<TPool>();
			for (int i = 0; i < m_baseCapacity; i++)
			{
				m_unused.Enqueue(m_constructor());
			}
		}

		/// <summary>
		/// Returns true when new item was allocated
		/// </summary>
		public bool AllocateOrCreate(out TPool item)
		{
			bool num = m_unused.Count == 0;
			if (num)
			{
				item = m_constructor();
			}
			else
			{
				item = m_unused.Dequeue();
			}
			m_active.Add(item);
			return num;
		}

		public TPool Allocate(bool nullAllowed = false)
		{
			TPool val = null;
			if (m_unused.Count > 0)
			{
				val = m_unused.Dequeue();
				m_active.Add(val);
			}
			return val;
		}

		public void Deallocate(TPool item)
		{
			m_active.Remove(item);
			m_unused.Enqueue(item);
		}

		public void MarkForDeallocate(TPool item)
		{
			m_marked.Add(item);
		}

		public void MarkAllActiveForDeallocate()
		{
			m_marked.UnionWith(m_active);
		}

		public void DeallocateAllMarked()
		{
			foreach (TPool item in m_marked)
			{
				Deallocate(item);
			}
			m_marked.Clear();
		}

		public void DeallocateAll()
		{
			foreach (TPool item in m_active)
			{
				m_unused.Enqueue(item);
			}
			m_active.Clear();
			m_marked.Clear();
		}

		public void TrimToBaseCapacity()
		{
			while (Capacity > BaseCapacity && m_unused.Count > 0)
			{
				m_unused.Dequeue();
			}
			m_unused.TrimExcess();
			m_active.TrimExcess();
			m_marked.TrimExcess();
		}
	}
}
