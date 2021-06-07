using System.Collections.Generic;
using VRage.Library.Threading;

namespace VRage.Collections
{
	/// <summary>
	/// Basic copy-on-commit implementation, later it will be faster by using one queue with 2 tails
	/// </summary>
	public class MyCommitQueue<T>
	{
		private Queue<T> m_commited = new Queue<T>();

		private SpinLock m_commitLock;

		private List<T> m_dirty = new List<T>();

		private SpinLock m_dirtyLock;

		public int Count
		{
			get
			{
				m_commitLock.Enter();
				try
				{
					return m_commited.Count;
				}
				finally
				{
					m_commitLock.Exit();
				}
			}
		}

		public int UncommitedCount
		{
			get
			{
				m_dirtyLock.Enter();
				try
				{
					return m_dirty.Count;
				}
				finally
				{
					m_dirtyLock.Exit();
				}
			}
		}

		public void Enqueue(T obj)
		{
			m_dirtyLock.Enter();
			try
			{
				m_dirty.Add(obj);
			}
			finally
			{
				m_dirtyLock.Exit();
			}
		}

		public void Commit()
		{
			m_dirtyLock.Enter();
			try
			{
				m_commitLock.Enter();
				try
				{
					foreach (T item in m_dirty)
					{
						m_commited.Enqueue(item);
					}
				}
				finally
				{
					m_commitLock.Exit();
				}
				m_dirty.Clear();
			}
			finally
			{
				m_dirtyLock.Exit();
			}
		}

		public bool TryDequeue(out T obj)
		{
			m_commitLock.Enter();
			try
			{
				if (m_commited.Count > 0)
				{
					obj = m_commited.Dequeue();
					return true;
				}
			}
			finally
			{
				m_commitLock.Exit();
			}
			obj = default(T);
			return false;
		}
	}
}
