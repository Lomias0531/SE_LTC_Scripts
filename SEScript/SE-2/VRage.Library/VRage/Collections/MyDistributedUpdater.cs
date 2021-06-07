using System;
using System.Collections.Generic;

namespace VRage.Collections
{
	/// <summary>
	/// Class distributing updates on large amount of objects in configurable intervals. 
	/// </summary>
	public class MyDistributedUpdater<V, T> where V : IReadOnlyList<T>, new()
	{
		private V m_list = new V();

		private int m_updateInterval;

		private int m_updateIndex;

		public int UpdateInterval
		{
			set
			{
				m_updateInterval = value;
			}
		}

		public V List => m_list;

		public MyDistributedUpdater(int updateInterval)
		{
			m_updateInterval = updateInterval;
		}

		public void Iterate(Action<T> p)
		{
			for (int i = m_updateIndex; i < m_list.Count; i += m_updateInterval)
			{
				p(m_list[i]);
			}
		}

		public void Update()
		{
			m_updateIndex++;
			m_updateIndex %= m_updateInterval;
		}
	}
}
