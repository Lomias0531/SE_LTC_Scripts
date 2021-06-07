using System.Collections.Generic;

namespace VRage.Collections
{
	public class MyConcurrentCollectionPool<TCollection, TItem> : IConcurrentPool where TCollection : ICollection<TItem>, new()
	{
		private readonly Stack<TCollection> m_instances;

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

		public MyConcurrentCollectionPool(int defaultCapacity = 0)
		{
			m_instances = new Stack<TCollection>(defaultCapacity);
			if (defaultCapacity > 0)
			{
				for (int i = 0; i < defaultCapacity; i++)
				{
					m_instances.Push(new TCollection());
				}
			}
		}

		public TCollection Get()
		{
			lock (m_instances)
			{
				if (m_instances.Count > 0)
				{
					return m_instances.Pop();
				}
			}
			return new TCollection();
		}

		public void Return(TCollection instance)
		{
			instance.Clear();
			lock (m_instances)
			{
				m_instances.Push(instance);
			}
		}

		void IConcurrentPool.Return(object obj)
		{
			Return((TCollection)obj);
		}

		object IConcurrentPool.Get()
		{
			return Get();
		}
	}
}
