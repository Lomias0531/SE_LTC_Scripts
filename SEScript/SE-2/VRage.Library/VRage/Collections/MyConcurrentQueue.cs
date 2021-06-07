using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Library.Threading;

namespace VRage.Collections
{
	/// <summary>
	/// Simple thread-safe queue.
	/// Uses spin-lock
	/// </summary>
	public class MyConcurrentQueue<T> : IEnumerable<T>, IEnumerable
	{
		private MyQueue<T> m_queue;

		private SpinLockRef m_lock = new SpinLockRef();

		public int Count
		{
			get
			{
				using (m_lock.Acquire())
				{
					return m_queue.Count;
				}
			}
		}

		public MyConcurrentQueue(int capacity)
		{
			m_queue = new MyQueue<T>(capacity);
		}

		public MyConcurrentQueue()
		{
			m_queue = new MyQueue<T>(8);
		}

		public void Clear()
		{
			using (m_lock.Acquire())
			{
				m_queue.Clear();
			}
		}

		public void Remove(T instance)
		{
			using (m_lock.Acquire())
			{
				m_queue.Remove(instance);
			}
		}

		public void Enqueue(T instance)
		{
			using (m_lock.Acquire())
			{
				m_queue.Enqueue(instance);
			}
		}

		public T Dequeue()
		{
			using (m_lock.Acquire())
			{
				return m_queue.Dequeue();
			}
		}

		public bool TryDequeue(out T instance)
		{
			using (m_lock.Acquire())
			{
				if (m_queue.Count > 0)
				{
					instance = m_queue.Dequeue();
					return true;
				}
			}
			instance = default(T);
			return false;
		}

		public bool TryPeek(out T instance)
		{
			using (m_lock.Acquire())
			{
				if (m_queue.Count > 0)
				{
					instance = m_queue.Peek();
					return true;
				}
			}
			instance = default(T);
			return false;
		}

		public ConcurrentEnumerator<SpinLockRef.Token, T, MyQueue<T>.Enumerator> GetEnumerator()
		{
			SpinLockRef.Token @lock = m_lock.Acquire();
			return ConcurrentEnumerator.Create<SpinLockRef.Token, T, MyQueue<T>.Enumerator>(@lock, m_queue.GetEnumerator());
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
