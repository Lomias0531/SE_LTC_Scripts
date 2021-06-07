using System.Collections.Generic;
using VRage.Library.Threading;

namespace VRage.Collections
{
	public class MyUniqueList<T>
	{
		private List<T> m_list = new List<T>();

		private HashSet<T> m_hashSet = new HashSet<T>();

		private SpinLockRef m_lock = new SpinLockRef();

		/// <summary>
		/// O(1)
		/// </summary>
		public int Count => m_list.Count;

		/// <summary>
		/// O(1)
		/// </summary>
		public T this[int index] => m_list[index];

		public UniqueListReader<T> Items => new UniqueListReader<T>(this);

		public ListReader<T> ItemList => new ListReader<T>(m_list);

		/// <summary>
		/// O(1)
		/// </summary>
		public bool Add(T item)
		{
			using (m_lock.Acquire())
			{
				if (m_hashSet.Add(item))
				{
					m_list.Add(item);
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// O(n)
		/// </summary>
		public bool Insert(int index, T item)
		{
			using (m_lock.Acquire())
			{
				if (m_hashSet.Add(item))
				{
					m_list.Insert(index, item);
					return true;
				}
				m_list.Remove(item);
				m_list.Insert(index, item);
				return false;
			}
		}

		/// <summary>
		/// O(n)
		/// </summary>
		public bool Remove(T item)
		{
			using (m_lock.Acquire())
			{
				if (m_hashSet.Remove(item))
				{
					m_list.Remove(item);
					return true;
				}
				return false;
			}
		}

		public void Clear()
		{
			m_list.Clear();
			m_hashSet.Clear();
		}

		/// <summary>
		/// O(1)
		/// </summary>
		public bool Contains(T item)
		{
			return m_hashSet.Contains(item);
		}

		public List<T>.Enumerator GetEnumerator()
		{
			return m_list.GetEnumerator();
		}
	}
}
