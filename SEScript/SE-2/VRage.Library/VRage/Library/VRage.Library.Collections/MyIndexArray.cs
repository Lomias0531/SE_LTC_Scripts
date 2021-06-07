using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Automatically resizing array when accessing index.
	/// </summary>
	public class MyIndexArray<T>
	{
		private T[] m_internalArray;

		public float MinimumGrowFactor = 2f;

		public T[] InternalArray => m_internalArray;

		public int Length => m_internalArray.Length;

		public T this[int index]
		{
			get
			{
				if (index < m_internalArray.Length)
				{
					return m_internalArray[index];
				}
				return default(T);
			}
			set
			{
				int num = m_internalArray.Length;
				if (index >= num)
				{
					int newSize = Math.Max((int)Math.Ceiling(MinimumGrowFactor * (float)num), index + 1);
					Array.Resize(ref m_internalArray, newSize);
				}
				m_internalArray[index] = value;
			}
		}

		public MyIndexArray(int defaultCapacity = 0)
		{
			m_internalArray = ((defaultCapacity > 0) ? new T[defaultCapacity] : EmptyArray<T>.Value);
		}

		public void Clear()
		{
			Array.Clear(m_internalArray, 0, m_internalArray.Length);
		}

		public void ClearItem(int index)
		{
			m_internalArray[index] = default(T);
		}

		/// <summary>
		/// Trims end of array which contains default elements.
		/// </summary>
		public void TrimExcess(float minimumShrinkFactor = 0.5f, IEqualityComparer<T> comparer = null)
		{
			comparer = (comparer ?? EqualityComparer<T>.Default);
			int num = m_internalArray.Length - 1;
			while (num >= 0 && comparer.Equals(m_internalArray[num], default(T)))
			{
				num--;
			}
			int num2 = num + 1;
			if ((float)num2 <= (float)m_internalArray.Length * minimumShrinkFactor)
			{
				Array.Resize(ref m_internalArray, num2);
			}
		}
	}
}
