using System.Diagnostics;
using System.Runtime.CompilerServices;
using VRage.Collections;

namespace System.Collections.Generic
{
	public static class ListExtensions
	{
		private sealed class FunctorComparer<T> : IComparer<T>
		{
			private Comparison<T> m_comparison;

			[ThreadStatic]
			private static FunctorComparer<T> m_Instance;

			public int Compare(T x, T y)
			{
				return m_comparison(x, y);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static FunctorComparer<T> Get(Comparison<T> comparison)
			{
				FunctorComparer<T> functorComparer = m_Instance;
				if (functorComparer == null)
				{
					functorComparer = (m_Instance = new FunctorComparer<T>());
				}
				functorComparer.m_comparison = comparison;
				return functorComparer;
			}
		}

		public static ClearToken<T> GetClearToken<T>(this List<T> list)
		{
			ClearToken<T> result = default(ClearToken<T>);
			result.List = list;
			return result;
		}

		/// <summary>
		/// Remove element at index by replacing it with last element in list.
		/// Removing is very fast but it breaks order of items in list!
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="index">The index.</param>
		public static void RemoveAtFast<T>(this List<T> list, int index)
		{
			int index2 = list.Count - 1;
			list[index] = list[index2];
			list.RemoveAt(index2);
		}

		public static void RemoveAtFast<T>(this IList<T> list, int index)
		{
			int index2 = list.Count - 1;
			list[index] = list[index2];
			list.RemoveAt(index2);
		}

		[Obsolete("Due to changes required for XBOX this method is obsolete. Do not use it, as now it simply does list.ToArray")]
		public static T[] GetInternalArray<T>(this List<T> list)
		{
			return list.ToArray();
		}

		public static void AddOrInsert<T>(this List<T> list, T item, int index)
		{
			if (index < 0 || index > list.Count)
			{
				list.Add(item);
			}
			else
			{
				list.Insert(index, item);
			}
		}

		public static void AddHashsetCasting<T1, T2>(this List<T1> list, HashSet<T2> hashset)
		{
			foreach (T2 item in hashset)
			{
				list.Add((T1)(object)item);
			}
		}

		/// <summary>
		/// Moves item in the list from original index to target index, reordering elements as if Remove and Insert was called.
		/// However, only elements in the range between the two indices are affected.
		/// </summary>
		public static void Move<T>(this List<T> list, int originalIndex, int targetIndex)
		{
			int num = Math.Sign(targetIndex - originalIndex);
			if (num != 0)
			{
				T value = list[originalIndex];
				for (int i = originalIndex; i != targetIndex; i += num)
				{
					list[i] = list[i + num];
				}
				list[targetIndex] = value;
			}
		}

		public static bool IsValidIndex<T>(this List<T> list, int index)
		{
			if (0 <= index)
			{
				return index < list.Count;
			}
			return false;
		}

		/// Remove each element in indices from the list.
		///
		/// The list of indices must be sorted.
		public static void RemoveIndices<T>(this List<T> list, List<int> indices)
		{
			if (indices.Count == 0)
			{
				return;
			}
			int i = 0;
			for (int j = indices[i]; j < list.Count - indices.Count; j++)
			{
				for (; i < indices.Count && j == indices[i] - i; i++)
				{
				}
				list[j] = list[j + i];
			}
			list.RemoveRange(list.Count - indices.Count, indices.Count);
		}

		public static void Swap<T>(this List<T> list, int a, int b)
		{
			T value = list[a];
			list[a] = list[b];
			list[b] = value;
		}

		public static void AddList<T>(this List<T> list, List<T> itemsToAdd)
		{
			if (itemsToAdd.Count != 0)
			{
				if (list.Capacity < list.Count + itemsToAdd.Count)
				{
					list.Capacity = list.Count + itemsToAdd.Count;
				}
				for (int i = 0; i < itemsToAdd.Count; i++)
				{
					list.Add(itemsToAdd[i]);
				}
			}
		}

		public static void AddArray<T>(this List<T> list, T[] itemsToAdd)
		{
			list.AddArray(itemsToAdd, itemsToAdd.Length);
		}

		public static void AddArray<T>(this List<T> list, T[] itemsToAdd, int itemCount)
		{
			if (list.Capacity < list.Count + itemCount)
			{
				list.Capacity = list.Count + itemCount;
			}
			for (int i = 0; i < itemsToAdd.Length; i++)
			{
				list.Add(itemsToAdd[i]);
			}
		}

		/// Do a binary search in an array of interval limits, each member is the interval threshold.
		///
		/// The result is the index of the interval that contains the value searched for.
		///
		/// If the interval array is empty 0 is returned (as we assume we have only the (-∞,+∞) interval).
		///
		/// Return range: [0, Length]
		public static int BinaryIntervalSearch<T>(this IList<T> self, T value, IComparer<T> comparer = null)
		{
			if (self.Count == 0)
			{
				return 0;
			}
			if (comparer == null)
			{
				comparer = Comparer<T>.Default;
			}
			if (self.Count == 1)
			{
				if (comparer.Compare(value, self[0]) < 0)
				{
					return 0;
				}
				return 1;
			}
			int num = 0;
			int num2 = self.Count;
			while (num2 - num > 1)
			{
				int num3 = (num + num2) / 2;
				if (comparer.Compare(value, self[num3]) >= 0)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			int result = num;
			if (comparer.Compare(value, self[num]) >= 0)
			{
				result = num2;
			}
			return result;
		}

		/// <summary>
		/// Do a binary search in an array of interval limits, each member is the interval threshold.
		///
		/// The result is the index of the interval that contains the value searched for.
		///
		/// If the interval array is empty 0 is returned (as we assume we have only the (-∞,+∞) interval).
		///
		/// Return range: [0, Length]
		/// </summary>
		public static int BinaryIntervalSearch<T>(this IList<T> self, Func<T, bool> less)
		{
			if (less == null)
			{
				throw new ArgumentNullException("less");
			}
			if (self.Count == 0)
			{
				return 0;
			}
			if (self.Count == 1)
			{
				if (!less(self[0]))
				{
					return 0;
				}
				return 1;
			}
			int num = 0;
			int num2 = self.Count;
			while (num2 - num > 1)
			{
				int num3 = (num + num2) / 2;
				if (less(self[num3]))
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			int result = num;
			if (less(self[num]))
			{
				result = num2;
			}
			return result;
		}

		public static int BinaryIntervalSearch<T>(this IList<T> self, T value, Comparison<T> comparison)
		{
			if (comparison == null)
			{
				throw new ArgumentNullException("comparison");
			}
			if (self.Count == 0)
			{
				return 0;
			}
			return self.BinaryIntervalSearch(value, FunctorComparer<T>.Get(comparison));
		}

		public static void InsertInOrder<T>(this List<T> self, T value, IComparer<T> comparer)
		{
			int num = self.BinarySearch(value, comparer);
			if (num < 0)
			{
				num = ~num;
			}
			self.Insert(num, value);
		}

		public static void InsertInOrder<T>(this List<T> self, T value) where T : IComparable<T>
		{
			self.InsertInOrder(value, Comparer<T>.Default);
		}

		public static bool IsSorted<T>(this List<T> self, IComparer<T> comparer)
		{
			for (int i = 1; i < self.Count; i++)
			{
				if (comparer.Compare(self[i - 1], self[i]) > 0)
				{
					return false;
				}
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[DebuggerStepThrough]
		public static void AssertEmpty<T>(this List<T> list)
		{
			if (list.Count != 0)
			{
				list.Clear();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[DebuggerStepThrough]
		public static void EnsureCapacity<T>(this List<T> list, int capacity)
		{
			if (list.Capacity < capacity)
			{
				list.Capacity = capacity;
			}
		}

		public static TValue Pop<TValue>(this List<TValue> self)
		{
			TValue result = self[self.Count - 1];
			self.RemoveAt(self.Count - 1);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[DebuggerStepThrough]
		public static void SortNoAlloc<T>(this List<T> list, Comparison<T> comparator)
		{
			list.Sort(FunctorComparer<T>.Get(comparator));
		}

		public static T AtMod<T>(this List<T> list, int index)
		{
			return list[index % list.Count];
		}

		public static T AtMod<T>(this ListReader<T> list, int index)
		{
			return list[index % list.Count];
		}

		public static T MinBy<T>(this IEnumerable<T> source, Func<T, float> selector)
		{
			return source.MaxBy((T x) => 0f - selector(x));
		}

		public static T MaxBy<T>(this IEnumerable<T> source, Func<T, float> selector)
		{
			T val = default(T);
			using (IEnumerator<T> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw new Exception("No elements");
				}
				val = enumerator.Current;
				float num = selector(val);
				while (enumerator.MoveNext())
				{
					T current = enumerator.Current;
					float num2 = selector(current);
					if (num2 > num)
					{
						num = num2;
						val = current;
					}
				}
				return val;
			}
		}

		public static O[] ToArray<I, O>(this IList<I> collection, Func<I, O> selector)
		{
			int count = collection.Count;
			O[] array = new O[collection.Count];
			for (int i = 0; i < count; i++)
			{
				array[i] = selector(collection[i]);
			}
			return array;
		}
	}
}
