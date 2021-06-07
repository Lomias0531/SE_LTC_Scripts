using System.Collections.Generic;
using VRage.Extensions;
using VRage.Library.Collections;

namespace System
{
	public static class ArrayExtensions
	{
		public static bool IsValidIndex<T>(this T[] self, int index)
		{
			return (uint)index < (uint)self.Length;
		}

		public static bool IsNullOrEmpty<T>(this T[] self)
		{
			if (self != null)
			{
				return self.Length == 0;
			}
			return true;
		}

		public static bool TryGetValue<T>(this T[] self, int index, out T value)
		{
			if ((uint)index < (uint)self.Length)
			{
				value = self[index];
				return true;
			}
			value = default(T);
			return false;
		}

		public static T[] Without<T>(this T[] self, IList<int> indices)
		{
			return self.RemoveIndices(indices);
		}

		public static T[] RemoveIndices<T>(this T[] self, IList<int> indices)
		{
			if (indices.Count >= self.Length)
			{
				return new T[0];
			}
			if (indices.Count == 0)
			{
				return self;
			}
			T[] array = new T[self.Length - indices.Count];
			int i = 0;
			for (int j = 0; j < self.Length - indices.Count; j++)
			{
				for (; i < indices.Count && j == indices[i] - i; i++)
				{
				}
				array[j] = self[j + i];
			}
			return array;
		}

		public static T[] Without<T>(this T[] self, int position)
		{
			T[] array = new T[self.Length - 1];
			for (int i = position; i < array.Length; i++)
			{
				array[i] = self[i + 1];
			}
			return array;
		}

		/// <summary>
		/// Do a binary search in an array of interval limits, each member is the interval threshold.
		///
		/// The result is the index of the interval that contains the value searched for.
		///
		/// If the interval array is empty 0 is returned (as we assume we have only the (-∞,+∞) interval).
		/// </summary>
		/// <returns>[0, Length]</returns>
		public static int BinaryIntervalSearch<T>(this T[] self, T value) where T : IComparable<T>
		{
			if (self.Length == 0)
			{
				return 0;
			}
			if (self.Length == 1)
			{
				if (value.CompareTo(self[0]) < 0)
				{
					return 0;
				}
				return 1;
			}
			int num = 0;
			int num2 = self.Length;
			while (num2 - num > 1)
			{
				int num3 = (num + num2) / 2;
				if (value.CompareTo(self[num3]) >= 0)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			int result = num;
			if (value.CompareTo(self[num]) >= 0)
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
		/// The search is done using the specified search method. The method must return an integer
		/// representing whether the argument is greater (1), smaller (-1) or equal(0) to the search condition.
		/// </summary>
		/// <param name="self">The array to search into.</param>
		///
		/// <returns>[0, Length]</returns>
		public static int BinaryIntervalSearch<T>(this T[] self, Func<T, int> selector)
		{
			if (self.Length == 0)
			{
				return 0;
			}
			if (self.Length == 1)
			{
				if (selector(self[0]) < 0)
				{
					return 0;
				}
				return 1;
			}
			int num = 0;
			int num2 = self.Length;
			while (num2 - num > 1)
			{
				int num3 = (num + num2) / 2;
				if (selector(self[num3]) >= 0)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			int result = num;
			if (selector(self[num]) >= 0)
			{
				result = num2;
			}
			return result;
		}

		public static MyRangeIterator<T>.Enumerable Range<T>(this T[] array, int start, int end)
		{
			return MyRangeIterator<T>.ForRange(array, start, end);
		}

		/// <summary>
		/// OfType on array implemented without allocations
		/// </summary>
		public static ArrayOfTypeEnumerator<TBase, ArrayEnumerator<TBase>, T> OfTypeFast<TBase, T>(this TBase[] array) where T : TBase
		{
			return new ArrayOfTypeEnumerator<TBase, ArrayEnumerator<TBase>, T>(new ArrayEnumerator<TBase>(array));
		}

		public static T[] CreateSubarray<T>(this T[] inputArray, int theFirstElement, int elementsCount)
		{
			int num = theFirstElement + elementsCount;
			if (inputArray.Length < num)
			{
				throw new ArgumentOutOfRangeException("The requested interval for the subarray is out of the boundaries");
			}
			T[] array = new T[elementsCount];
			for (int i = 0; i < elementsCount; i++)
			{
				array[i] = inputArray[theFirstElement + i];
			}
			return array;
		}

		public unsafe static bool Compare(this byte[] a1, byte[] a2)
		{
			if (a1 == null || a2 == null || a1.Length != a2.Length)
			{
				return false;
			}
			fixed (byte* ptr = a1)
			{
				fixed (byte* ptr3 = a2)
				{
					byte* ptr2 = ptr;
					byte* ptr4 = ptr3;
					int num = a1.Length;
					int num2 = 0;
					while (num2 < num / 8)
					{
						if (*(long*)ptr2 != *(long*)ptr4)
						{
							return false;
						}
						num2++;
						ptr2 += 8;
						ptr4 += 8;
					}
					if ((num & 4) != 0)
					{
						if (*(int*)ptr2 != *(int*)ptr4)
						{
							return false;
						}
						ptr2 += 4;
						ptr4 += 4;
					}
					if ((num & 2) != 0)
					{
						if (*(short*)ptr2 != *(short*)ptr4)
						{
							return false;
						}
						ptr2 += 2;
						ptr4 += 2;
					}
					if ((num & 1) != 0 && *ptr2 != *ptr4)
					{
						return false;
					}
					return true;
				}
			}
		}

		public static void ForEach(this Array array, Action<Array, int[]> action)
		{
			if (array.LongLength != 0L)
			{
				ArrayTraverse arrayTraverse = new ArrayTraverse(array);
				do
				{
					action(array, arrayTraverse.Position);
				}
				while (arrayTraverse.Step());
			}
		}

		/// <summary>
		/// Allocates for struct elements!
		/// </summary>
		public static bool Contains<T>(this T[] array, T element)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < array.Length; i++)
			{
				if (@default.Equals(array[i], element))
				{
					return true;
				}
			}
			return false;
		}

		public static Span<T> Span<T>(this T[] array, int offset, int? count = null)
		{
			return new Span<T>(array, offset, count ?? (array.Length - offset));
		}

		public static void EnsureCapacity<T>(ref T[] array, int size)
		{
			if (array == null || array.Length < size)
			{
				Array.Resize(ref array, size);
			}
		}
	}
}
