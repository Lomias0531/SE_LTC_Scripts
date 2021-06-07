using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Collections
{
	public class MyConcurrentList<T> : IMyQueue<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		private readonly List<T> m_list;

		private readonly FastResourceLock m_lock = new FastResourceLock();

		/// <summary>
		/// Debug only!! Thread unsafe
		/// </summary>
		public ListReader<T> ListUnsafe => new ListReader<T>(m_list);

		/// <summary>
		/// Manage lock yourself when accesing the list!
		/// </summary>
		public List<T> List => m_list;

		public int Count
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_list.Count;
				}
			}
		}

		public bool Empty
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_list.Count == 0;
				}
			}
		}

		public T this[int index]
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_list[index];
				}
			}
			set
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					m_list[index] = value;
				}
			}
		}

		public bool IsReadOnly => false;

		public MyConcurrentList(int reserve)
		{
			m_list = new List<T>(reserve);
		}

		public MyConcurrentList()
		{
			m_list = new List<T>();
		}

		/// <summary>
		/// Does NOT call sort
		/// </summary>
		/// <param name="value"></param>
		public void Add(T value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_list.Add(value);
			}
		}

		/// <summary>
		/// Add a range of items to this list.
		/// </summary>
		/// <param name="value"></param>
		public void AddRange(IEnumerable<T> value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_list.AddRange(value);
			}
		}

		public void Sort(IComparer<T> comparer)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_list.Sort(comparer);
			}
		}

		public bool TryDequeueFront(out T value)
		{
			value = default(T);
			using (m_lock.AcquireExclusiveUsing())
			{
				if (m_list.Count == 0)
				{
					return false;
				}
				value = m_list[0];
				m_list.RemoveAt(0);
			}
			return true;
		}

		public bool TryDequeueBack(out T value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				if (m_list.Count == 0)
				{
					value = default(T);
					return false;
				}
				int index = m_list.Count - 1;
				value = m_list[index];
				m_list.RemoveAt(index);
			}
			return true;
		}

		public T Pop()
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				T result = m_list[m_list.Count - 1];
				m_list.RemoveAt(m_list.Count - 1);
				return result;
			}
		}

		public int IndexOf(T item)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_list.IndexOf(item);
			}
		}

		public void Insert(int index, T item)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_list.Insert(index, item);
			}
		}

		public void RemoveAt(int index)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_list.RemoveAt(index);
			}
		}

		public void Clear()
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_list.Clear();
			}
		}

		public bool Contains(T item)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_list.Contains(item);
			}
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			using (m_lock.AcquireSharedUsing())
			{
				m_list.CopyTo(array, arrayIndex);
			}
		}

		public bool Remove(T item)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				return m_list.Remove(item);
			}
		}

		public ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, T, List<T>.Enumerator> GetEnumerator()
		{
			FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
			return ConcurrentEnumerator.Create<FastResourceLockExtensions.MySharedLock, T, List<T>.Enumerator>(@lock, m_list.GetEnumerator());
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void RemoveAll(Predicate<T> callback)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				int num = 0;
				while (num < Count)
				{
					if (callback(m_list[num]))
					{
						m_list.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
			}
		}
	}
}
