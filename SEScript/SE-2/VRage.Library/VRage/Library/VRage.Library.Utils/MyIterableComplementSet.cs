using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Library.Utils
{
	/// Over a given set of elements W this class maintains two subsets A and B such that
	/// A ⋂ B = ∅, A ⋃ B = W, with constant time operations for moving elements from one set to the other.
	///
	/// Both subsets are individually iterable, as well as the whole W.
	///
	/// The order of elements in either set is never preserved.
	///
	/// When using this class with value types beware that they will be duplicated internally.
	/// Prefer to use class types or some form of lightweight reference with this.
	public class MyIterableComplementSet<T> : IEnumerable<T>, IEnumerable
	{
		private Dictionary<T, int> m_index = new Dictionary<T, int>();

		private List<T> m_data = new List<T>();

		private int m_split;

		public void Add(T item)
		{
			m_index.Add(item, m_data.Count);
			m_data.Add(item);
		}

		public void AddToComplement(T item)
		{
			m_index.Add(item, m_data.Count);
			m_data.Add(item);
			MoveToComplement(item);
		}

		public void Remove(T item)
		{
			int num = m_index[item];
			if (m_split > num)
			{
				m_split--;
				T val = m_data[m_split];
				m_index[val] = num;
				m_data[num] = val;
				num = m_split;
			}
			int index = m_data.Count - 1;
			m_data[num] = m_data[index];
			m_index[m_data[index]] = num;
			m_index.Remove(item);
			m_data.RemoveAt(index);
		}

		public void MoveToComplement(T item)
		{
			T val = m_data[m_split];
			int num = m_index[item];
			m_data[m_split] = item;
			m_index[item] = m_split;
			m_data[num] = val;
			m_index[val] = num;
			m_split++;
		}

		public bool Contains(T item)
		{
			return m_index.ContainsKey(item);
		}

		public bool IsInComplement(T item)
		{
			return m_index[item] < m_split;
		}

		public void MoveToSet(T item)
		{
			m_split--;
			T val = m_data[m_split];
			int num = m_index[item];
			m_data[m_split] = item;
			m_index[item] = m_split;
			m_data[num] = val;
			m_index[val] = num;
		}

		public void ClearSet()
		{
			for (int i = m_split; i < m_data.Count; i++)
			{
				m_index.Remove(m_data[i]);
			}
			m_data.RemoveRange(m_split, m_data.Count - m_split);
		}

		public void ClearComplement()
		{
			for (int i = m_split; i < m_data.Count; i++)
			{
				m_index.Remove(m_data[i]);
			}
			m_data.RemoveRange(m_split, m_data.Count - m_split);
		}

		public void AllToComplement()
		{
			m_split = m_data.Count;
		}

		public void AllToSet()
		{
			m_split = 0;
		}

		public IEnumerable<T> Set()
		{
			return MyRangeIterator<T>.ForRange(m_data, m_split, m_data.Count);
		}

		public IEnumerable<T> Complement()
		{
			return MyRangeIterator<T>.ForRange(m_data, 0, m_split);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return m_data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Clear()
		{
			m_split = 0;
			m_index.Clear();
			m_data.Clear();
		}
	}
}
