using System;
using System.Collections.Generic;

namespace VRage.Collections
{
	/// <summary>
	/// <para>A set of integer numbers optimized for sets with long consecutive runs. Each interval is stored as two values in m_list: the lower and the upper bound.</para>
	/// <para>For example, the set of numbers 2, 3, 4, 5, 7, 9, 10, 11, 12, 13 (or alternatively in the interval notation &lt;2, 5&gt; U &lt;7, 7&gt; U &lt;9, 13&gt;)
	/// is saved as a list { 2, 5, 7, 7, 9, 13 }</para>
	/// </summary>
	public class MyIntervalList
	{
		public struct Enumerator
		{
			private int m_interval;

			private int m_dist;

			private int m_lowerBound;

			private int m_upperBound;

			private MyIntervalList m_parent;

			public int Current => m_lowerBound + m_dist;

			public Enumerator(MyIntervalList parent)
			{
				m_interval = -1;
				m_dist = 0;
				m_lowerBound = 0;
				m_upperBound = 0;
				m_parent = parent;
			}

			public bool MoveNext()
			{
				if (m_interval == -1)
				{
					return MoveNextInterval();
				}
				if (m_lowerBound + m_dist >= m_upperBound)
				{
					return MoveNextInterval();
				}
				m_dist++;
				return true;
			}

			private bool MoveNextInterval()
			{
				m_interval++;
				if (m_interval >= m_parent.IntervalCount)
				{
					return false;
				}
				m_dist = 0;
				m_lowerBound = m_parent.m_list[m_interval * 2];
				m_upperBound = m_parent.m_list[m_interval * 2 + 1];
				return true;
			}
		}

		private List<int> m_list;

		private int m_count;

		public int Count => m_count;

		public int IntervalCount => m_list.Count / 2;

		public int this[int index]
		{
			get
			{
				if (index < 0 || index >= m_count)
				{
					throw new IndexOutOfRangeException("Index " + index + " is out of range in MyIntervalList. Valid indices are in range <0, Count)");
				}
				int num = index;
				for (int i = 0; i < m_list.Count; i += 2)
				{
					int num2 = m_list[i + 1] - m_list[i] + 1;
					if (num < num2)
					{
						return m_list[i] + num;
					}
					num -= num2;
				}
				return 0;
			}
		}

		public MyIntervalList()
		{
			m_list = new List<int>(8);
		}

		private MyIntervalList(int capacity)
		{
			m_list = new List<int>(capacity);
		}

		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < m_list.Count; i += 2)
			{
				if (i != 0)
				{
					text += "; ";
				}
				text = text + "<" + m_list[i] + "," + m_list[i + 1] + ">";
			}
			return text;
		}

		public int IndexOf(int value)
		{
			int num = 0;
			for (int i = 0; i < m_list.Count; i += 2)
			{
				if (value < m_list[i])
				{
					return -1;
				}
				if (value <= m_list[i + 1])
				{
					return num + value - m_list[i];
				}
				num += m_list[i + 1] - m_list[i] + 1;
			}
			return -1;
		}

		/// <summary>
		/// Add a value to the list
		/// </summary>
		public void Add(int value)
		{
			switch (value)
			{
			case int.MinValue:
				if (m_list.Count == 0)
				{
					InsertInterval(0, value, value);
				}
				else if (m_list[0] == -2147483647)
				{
					ExtendIntervalDown(0);
				}
				else if (m_list[0] != int.MinValue)
				{
					InsertInterval(0, value, value);
				}
				return;
			case int.MaxValue:
			{
				int num = m_list.Count - 2;
				if (num < 0)
				{
					InsertInterval(0, value, value);
				}
				else if (m_list[num + 1] == 2147483646)
				{
					ExtendIntervalUp(num);
				}
				else if (m_list[num + 1] != int.MaxValue)
				{
					InsertInterval(m_list.Count, value, value);
				}
				return;
			}
			}
			for (int i = 0; i < m_list.Count; i += 2)
			{
				if (value + 1 < m_list[i])
				{
					InsertInterval(i, value, value);
					return;
				}
				if (value - 1 <= m_list[i + 1])
				{
					if (value + 1 == m_list[i])
					{
						ExtendIntervalDown(i);
					}
					else if (value - 1 == m_list[i + 1])
					{
						ExtendIntervalUp(i);
					}
					return;
				}
			}
			InsertInterval(m_list.Count, value, value);
		}

		public void Clear()
		{
			m_list.Clear();
			m_count = 0;
		}

		public MyIntervalList GetCopy()
		{
			MyIntervalList myIntervalList = new MyIntervalList(m_list.Count);
			for (int i = 0; i < m_list.Count; i++)
			{
				myIntervalList.m_list.Add(m_list[i]);
			}
			myIntervalList.m_count = m_count;
			return myIntervalList;
		}

		public bool Contains(int value)
		{
			for (int i = 0; i < m_list.Count; i += 2)
			{
				if (value < m_list[i])
				{
					return false;
				}
				if (value <= m_list[i + 1])
				{
					return true;
				}
			}
			return false;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		private void InsertInterval(int listPosition, int min, int max)
		{
			if (listPosition == m_list.Count)
			{
				m_list.Add(min);
				m_list.Add(max);
				m_count += max - min + 1;
				return;
			}
			int num = m_list.Count - 2;
			m_list.Add(m_list[num]);
			m_list.Add(m_list[num + 1]);
			while (num > listPosition)
			{
				m_list[num] = m_list[num - 2];
				m_list[num + 1] = m_list[num - 1];
				num -= 2;
			}
			m_list[num] = min;
			m_list[num + 1] = max;
			m_count += max - min + 1;
		}

		private void ExtendIntervalDown(int i)
		{
			m_list[i]--;
			m_count++;
			if (i != 0)
			{
				TryMergeIntervals(i - 1, i);
			}
		}

		private void ExtendIntervalUp(int i)
		{
			m_list[i + 1]++;
			m_count++;
			if (i < m_list.Count - 2)
			{
				TryMergeIntervals(i + 1, i + 2);
			}
		}

		private void TryMergeIntervals(int i1, int i2)
		{
			if (m_list[i1] + 1 == m_list[i2])
			{
				for (int j = i1; j < m_list.Count - 2; j++)
				{
					m_list[j] = m_list[j + 2];
				}
				m_list.RemoveAt(m_list.Count - 1);
				m_list.RemoveAt(m_list.Count - 1);
			}
		}
	}
}
