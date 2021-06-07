using System.Collections.Generic;

namespace VRage
{
	public class FastNoArgsEvent
	{
		private FastResourceLock m_lock = new FastResourceLock();

		private List<MyNoArgsDelegate> m_delegates = new List<MyNoArgsDelegate>(2);

		private List<MyNoArgsDelegate> m_delegatesIterator = new List<MyNoArgsDelegate>(2);

		public event MyNoArgsDelegate Event
		{
			add
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					m_delegates.Add(value);
				}
			}
			remove
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					m_delegates.Remove(value);
				}
			}
		}

		public void Raise()
		{
			using (m_lock.AcquireSharedUsing())
			{
				m_delegatesIterator.Clear();
				foreach (MyNoArgsDelegate @delegate in m_delegates)
				{
					m_delegatesIterator.Add(@delegate);
				}
			}
			foreach (MyNoArgsDelegate item in m_delegatesIterator)
			{
				item();
			}
			m_delegatesIterator.Clear();
		}
	}
}
