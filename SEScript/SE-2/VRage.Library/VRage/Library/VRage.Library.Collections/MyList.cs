using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Network;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Implementation of a list with additional utility methods and access to the internal array.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	[DebuggerDisplay("Count = {Count}")]
	public class MyList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
	{
		internal sealed class FunctorComparer<TItem> : IComparer<TItem>
		{
			private readonly Comparison<TItem> m_comparison;

			public FunctorComparer(Comparison<TItem> comparison)
			{
				m_comparison = comparison;
			}

			public int Compare(TItem x, TItem y)
			{
				return m_comparison(x, y);
			}
		}

		[Serializable]
		internal class SynchronizedList : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
		{
			protected class VRage_Library_Collections_MyList_00601_003C_003ESynchronizedList_003C_003E_list_003C_003EAccessor : IMemberAccessor<SynchronizedList, List<T>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SynchronizedList owner, in List<T> value)
				{
					owner._list = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SynchronizedList owner, out List<T> value)
				{
					value = owner._list;
				}
			}

			protected class VRage_Library_Collections_MyList_00601_003C_003ESynchronizedList_003C_003E_root_003C_003EAccessor : IMemberAccessor<SynchronizedList, object>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SynchronizedList owner, in object value)
				{
					((MyList<>.SynchronizedList)(object)owner)._root = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SynchronizedList owner, out object value)
				{
					value = ((MyList<>.SynchronizedList)(object)owner)._root;
				}
			}

			private List<T> _list;

			private object _root;

			public int Count
			{
				get
				{
					lock (_root)
					{
						return _list.Count;
					}
				}
			}

			public bool IsReadOnly => ((ICollection<T>)_list).IsReadOnly;

			public T this[int index]
			{
				get
				{
					lock (_root)
					{
						return _list[index];
					}
				}
				set
				{
					lock (_root)
					{
						_list[index] = value;
					}
				}
			}

			internal SynchronizedList(List<T> list)
			{
				_list = list;
				_root = ((ICollection)list).SyncRoot;
			}

			public void Add(T item)
			{
				lock (_root)
				{
					_list.Add(item);
				}
			}

			public void Clear()
			{
				lock (_root)
				{
					_list.Clear();
				}
			}

			public bool Contains(T item)
			{
				lock (_root)
				{
					return _list.Contains(item);
				}
			}

			public void CopyTo(T[] array, int arrayIndex)
			{
				lock (_root)
				{
					_list.CopyTo(array, arrayIndex);
				}
			}

			public bool Remove(T item)
			{
				lock (_root)
				{
					return _list.Remove(item);
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				lock (_root)
				{
					return _list.GetEnumerator();
				}
			}

			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				lock (_root)
				{
					return ((IEnumerable<T>)_list).GetEnumerator();
				}
			}

			public int IndexOf(T item)
			{
				lock (_root)
				{
					return _list.IndexOf(item);
				}
			}

			public void Insert(int index, T item)
			{
				lock (_root)
				{
					_list.Insert(index, item);
				}
			}

			public void RemoveAt(int index)
			{
				lock (_root)
				{
					_list.RemoveAt(index);
				}
			}
		}

		[Serializable]
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			protected class VRage_Library_Collections_MyList_00601_003C_003EEnumerator_003C_003Elist_003C_003EAccessor : IMemberAccessor<Enumerator, MyList<T>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Enumerator owner, in MyList<T> value)
				{
					owner.list = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Enumerator owner, out MyList<T> value)
				{
					value = owner.list;
				}
			}

			protected class VRage_Library_Collections_MyList_00601_003C_003EEnumerator_003C_003Eindex_003C_003EAccessor : IMemberAccessor<Enumerator, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Set(ref Enumerator owner, in int value)
				{
					((MyList<>.Enumerator*)(&owner))->index = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Get(ref Enumerator owner, out int value)
				{
					value = ((MyList<>.Enumerator*)(&owner))->index;
				}
			}

			protected class VRage_Library_Collections_MyList_00601_003C_003EEnumerator_003C_003Eversion_003C_003EAccessor : IMemberAccessor<Enumerator, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Set(ref Enumerator owner, in int value)
				{
					((MyList<>.Enumerator*)(&owner))->version = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Get(ref Enumerator owner, out int value)
				{
					value = ((MyList<>.Enumerator*)(&owner))->version;
				}
			}

			protected class VRage_Library_Collections_MyList_00601_003C_003EEnumerator_003C_003Ecurrent_003C_003EAccessor : IMemberAccessor<Enumerator, T>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Enumerator owner, in T value)
				{
					owner.current = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Enumerator owner, out T value)
				{
					value = owner.current;
				}
			}

			private MyList<T> list;

			private int index;

			private int version;

			private T current;

			public T Current => current;

			object IEnumerator.Current
			{
				get
				{
					if (index == 0 || index == list.m_size + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return Current;
				}
			}

			internal Enumerator(MyList<T> list)
			{
				this.list = list;
				index = 0;
				version = list.m_version;
				current = default(T);
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				MyList<T> myList = list;
				if (version == myList.m_version && (uint)index < (uint)myList.m_size)
				{
					current = myList.m_items[index];
					index++;
					return true;
				}
				return MoveNextRare();
			}

			private bool MoveNextRare()
			{
				if (version != list.m_version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				index = list.m_size + 1;
				current = default(T);
				return false;
			}

			void IEnumerator.Reset()
			{
				if (version != list.m_version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				index = 0;
				current = default(T);
			}
		}

		protected class VRage_Library_Collections_MyList_00601_003C_003Em_items_003C_003EAccessor : IMemberAccessor<MyList<T>, T[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyList<T> owner, in T[] value)
			{
				owner.m_items = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyList<T> owner, out T[] value)
			{
				value = owner.m_items;
			}
		}

		protected class VRage_Library_Collections_MyList_00601_003C_003Em_size_003C_003EAccessor : IMemberAccessor<MyList<T>, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyList<T> owner, in int value)
			{
				((MyList<>)(object)owner).m_size = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyList<T> owner, out int value)
			{
				value = ((MyList<>)(object)owner).m_size;
			}
		}

		protected class VRage_Library_Collections_MyList_00601_003C_003Em_version_003C_003EAccessor : IMemberAccessor<MyList<T>, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyList<T> owner, in int value)
			{
				((MyList<>)(object)owner).m_version = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyList<T> owner, out int value)
			{
				value = ((MyList<>)(object)owner).m_version;
			}
		}

		protected class VRage_Library_Collections_MyList_00601_003C_003Em_syncRoot_003C_003EAccessor : IMemberAccessor<MyList<T>, object>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyList<T> owner, in object value)
			{
				((MyList<>)(object)owner).m_syncRoot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyList<T> owner, out object value)
			{
				value = ((MyList<>)(object)owner).m_syncRoot;
			}
		}

		protected class VRage_Library_Collections_MyList_00601_003C_003ECapacity_003C_003EAccessor : IMemberAccessor<MyList<T>, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyList<T> owner, in int value)
			{
				((MyList<>)(object)owner).Capacity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyList<T> owner, out int value)
			{
				value = ((MyList<>)(object)owner).Capacity;
			}
		}

		private const int DefaultCapacity = 4;

		private const int MaxLength = int.MaxValue;

		private T[] m_items;

		private int m_size;

		private int m_version;

		[NonSerialized]
		private object m_syncRoot;

		private static readonly T[] EmptyArray = new T[0];

		public int Capacity
		{
			get
			{
				return m_items.Length;
			}
			set
			{
				if (value < m_size)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
				}
				if (value == m_items.Length)
				{
					return;
				}
				if (value > 0)
				{
					T[] array = new T[value];
					if (m_size > 0)
					{
						Array.Copy(m_items, 0, array, 0, m_size);
					}
					m_items = array;
				}
				else
				{
					m_items = EmptyArray;
				}
			}
		}

		public int Count => m_size;

		bool IList.IsFixedSize => false;

		bool ICollection<T>.IsReadOnly => false;

		bool IList.IsReadOnly => false;

		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot
		{
			get
			{
				if (m_syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref m_syncRoot, new object(), (object)null);
				}
				return m_syncRoot;
			}
		}

		public T this[int index]
		{
			get
			{
				if ((uint)index >= (uint)m_size)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				return m_items[index];
			}
			set
			{
				if ((uint)index >= (uint)m_size)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				m_items[index] = value;
				m_version++;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);
				try
				{
					this[index] = (T)value;
				}
				catch (InvalidCastException)
				{
					ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
				}
			}
		}

		/// <summary>
		/// Add the specified range of items from <paramref name="list" /> into this collection at the specified position.
		/// </summary>
		/// <param name="list">The list to copy the elements from.</param>
		/// <param name="sourceIndex">The source </param>
		/// <param name="count">The number of elements to take from the list.</param>
		public void InsertFrom(IList<T> list, int sourceIndex, int destIndex, int count)
		{
			if (list.Count < sourceIndex + count)
			{
				throw new ArgumentOutOfRangeException("", "Some element indices are out of the bounds of the source array.");
			}
			if (destIndex < 0 || sourceIndex > m_size)
			{
				throw new ArgumentOutOfRangeException("destIndex", "The destination index is out of the bounds of this list.");
			}
			EnsureCapacity(m_size + count);
			MoveForward(destIndex, count);
			T[] sourceArray;
			if ((sourceArray = (list as T[])) != null)
			{
				Array.Copy(sourceArray, sourceIndex, m_items, destIndex, count);
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					m_items[destIndex + i] = list[sourceIndex + i];
				}
			}
			m_version++;
			m_size += count;
		}

		public void RemoveAtFast(int index)
		{
			if (index < 0 || index >= m_size)
			{
				throw new ArgumentOutOfRangeException("index", "Index cannot be negative or greater than the size of the list.");
			}
			m_items[index] = m_items[m_size - 1];
			m_items[m_size - 1] = default(T);
			m_size--;
			m_version++;
		}

		/// <summary>Quickly removes all the elements that match the conditions defined by the specified predicate.</summary>
		/// <remarks>This version of the method does not preserve the order of the list, but is faster to execute.</remarks>
		/// <param name="match">The <see cref="T:System.Predicate`1"></see> delegate that defines the conditions of the elements to remove.</param>
		/// <returns>The number of elements removed from the <see cref="T:System.Collections.Generic.List`1"></see> .</returns>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="match">match</paramref> is null.</exception>
		public int RemoveAllFast(Predicate<T> match)
		{
			int num = 0;
			for (int num2 = m_size - 1; num2 >= 0; num2--)
			{
				if (match(m_items[num2]))
				{
					num++;
					RemoveAtFast(num2);
				}
			}
			return num;
		}

		/// <summary>
		/// Quickly clear this list.
		/// </summary>
		/// <remarks>This method will only reset the internal size counter, individual items will not be modified and as a result object references may be kep for longer than expected.</remarks>
		public void ClearFast()
		{
			m_size = 0;
			m_version++;
		}

		/// <summary>
		/// Clear the entire allocated length of the internal array.
		/// </summary>
		/// <remarks><para>This method can be used after <see cref="M:VRage.Library.Collections.MyList`1.ClearFast" /> to ensure no object references are kept by free slots in the internal array.</para>
		/// <para>It is important to realize that calling both <see cref="M:VRage.Library.Collections.MyList`1.ClearFast" /> and <see cref="M:VRage.Library.Collections.MyList`1.ClearForced" /> is sequence is potentially slower than regular <see cref="M:VRage.Library.Collections.MyList`1.ClearFast" />.
		/// This method is instead intended to be used only occasionally or at the end of the life cycle of lists that act like buffers and are fast-cleared frequently.</para></remarks>
		public void ClearForced()
		{
			if (m_items != null)
			{
				for (int i = 0; i < m_items.Length; i++)
				{
					m_items[i] = default(T);
				}
			}
			m_size = 0;
			m_version++;
		}

		/// <summary>
		/// This method should be called after modifying the internal array of this list.
		/// </summary>
		/// <remarks>Some methods such as <see cref="M:VRage.Library.Collections.MyList`1.SetSize(System.Int32)" /> can be called instead to achieve the same result.</remarks>
		/// <seealso cref="M:VRage.Library.Collections.MyList`1.GetInternalArray" />
		public void Touch()
		{
			m_version++;
		}

		/// <summary>
		/// Modify the internal size of the list.
		/// </summary>
		/// <remarks>This method can be used for fast truncation or after the list contents are filled in directly.</remarks>
		/// <param name="size"></param>
		/// <seealso cref="M:VRage.Library.Collections.MyList`1.GetInternalArray" />
		public void SetSize(int size)
		{
			if (size < 0 || size > Capacity)
			{
				throw new ArgumentOutOfRangeException("size", "Size must not be smaller than zero or larger than the current capacity.");
			}
			m_size = size;
			m_version++;
		}

		/// <summary>
		/// Move array elements forward.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		private void MoveForward(int index, int count)
		{
			EnsureCapacity(m_size + count);
			Array.Copy(m_items, index, m_items, index + count, m_size - index);
		}

		/// <summary>
		/// Move array elements backward.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		private void MoveBackward(int index, int count)
		{
			EnsureCapacity(m_size + count);
			Array.Copy(m_items, index + count, m_items, index, m_size - index);
		}

		/// <summary>
		/// Provide access to the array used internally to store the list items.
		/// </summary>
		/// <remarks>If elements of the array are modified the user should call <see cref="M:VRage.Library.Collections.MyList`1.Touch" /> to inform of that change.</remarks>
		/// <returns></returns>
		public T[] GetInternalArray()
		{
			return m_items;
		}

		public MyList()
		{
			m_items = EmptyArray;
		}

		public MyList(int capacity)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (capacity == 0)
			{
				m_items = EmptyArray;
			}
			else
			{
				m_items = new T[capacity];
			}
		}

		public MyList(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				if (count == 0)
				{
					m_items = EmptyArray;
					return;
				}
				m_items = new T[count];
				collection2.CopyTo(m_items, 0);
				m_size = count;
			}
			else
			{
				m_size = 0;
				m_items = EmptyArray;
				foreach (T item in collection)
				{
					Add(item);
				}
			}
		}

		private static bool IsCompatibleObject(object value)
		{
			if (!(value is T))
			{
				if (value == null)
				{
					return default(T) == null;
				}
				return false;
			}
			return true;
		}

		public void Add(T item)
		{
			if (m_size == m_items.Length)
			{
				EnsureCapacity(m_size + 1);
			}
			m_items[m_size++] = item;
			m_version++;
		}

		int IList.Add(object item)
		{
			ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);
			try
			{
				Add((T)item);
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(item, typeof(T));
			}
			return Count - 1;
		}

		public void AddRange(IEnumerable<T> collection)
		{
			InsertRange(m_size, collection);
		}

		public ReadOnlyCollection<T> AsReadOnly()
		{
			return new ReadOnlyCollection<T>(this);
		}

		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (m_size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			return Array.BinarySearch(m_items, index, count, item, comparer);
		}

		public int BinarySearch(T item)
		{
			return BinarySearch(0, Count, item, null);
		}

		public int BinarySearch(T item, IComparer<T> comparer)
		{
			return BinarySearch(0, Count, item, comparer);
		}

		public void Clear()
		{
			if (m_size > 0)
			{
				Array.Clear(m_items, 0, m_size);
				m_size = 0;
			}
			m_version++;
		}

		public bool Contains(T item)
		{
			if (item == null)
			{
				for (int i = 0; i < m_size; i++)
				{
					if (m_items[i] == null)
					{
						return true;
					}
				}
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int j = 0; j < m_size; j++)
			{
				if (@default.Equals(m_items[j], item))
				{
					return true;
				}
			}
			return false;
		}

		bool IList.Contains(object item)
		{
			if (IsCompatibleObject(item))
			{
				return Contains((T)item);
			}
			return false;
		}

		public MyList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			if (converter == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.converter);
			}
			MyList<TOutput> myList = new MyList<TOutput>(m_size);
			for (int i = 0; i < m_size; i++)
			{
				myList.m_items[i] = converter(m_items[i]);
			}
			myList.m_size = m_size;
			return myList;
		}

		public void CopyTo(T[] array)
		{
			CopyTo(array, 0);
		}

		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if (array != null && array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			try
			{
				Array.Copy(m_items, 0, array, arrayIndex, m_size);
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			if (m_size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Copy(m_items, index, array, arrayIndex, count);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(m_items, 0, array, arrayIndex, m_size);
		}

		public void EnsureCapacity(int min)
		{
			if (m_items.Length < min)
			{
				int num = (m_items.Length == 0) ? 4 : (m_items.Length * 2);
				if ((uint)num > 2147483647u)
				{
					num = int.MaxValue;
				}
				if (num < min)
				{
					num = min;
				}
				Capacity = num;
			}
		}

		public bool Exists(Predicate<T> match)
		{
			return FindIndex(match) != -1;
		}

		public T Find(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < m_size; i++)
			{
				if (match(m_items[i]))
				{
					return m_items[i];
				}
			}
			return default(T);
		}

		public List<T> FindAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			List<T> list = new List<T>();
			for (int i = 0; i < m_size; i++)
			{
				if (match(m_items[i]))
				{
					list.Add(m_items[i]);
				}
			}
			return list;
		}

		public int FindIndex(Predicate<T> match)
		{
			return FindIndex(0, m_size, match);
		}

		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return FindIndex(startIndex, m_size - startIndex, match);
		}

		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			if ((uint)startIndex > (uint)m_size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (count < 0 || startIndex > m_size - count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
			}
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(m_items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public T FindLast(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int num = m_size - 1; num >= 0; num--)
			{
				if (match(m_items[num]))
				{
					return m_items[num];
				}
			}
			return default(T);
		}

		public int FindLastIndex(Predicate<T> match)
		{
			return FindLastIndex(m_size - 1, m_size, match);
		}

		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return FindLastIndex(startIndex, startIndex + 1, match);
		}

		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			if (m_size == 0)
			{
				if (startIndex != -1)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
				}
			}
			else if ((uint)startIndex >= (uint)m_size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (count < 0 || startIndex - count + 1 < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
			}
			int num = startIndex - count;
			for (int num2 = startIndex; num2 > num; num2--)
			{
				if (match(m_items[num2]))
				{
					return num2;
				}
			}
			return -1;
		}

		public void ForEach(Action<T> action)
		{
			if (action == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			int version = m_version;
			for (int i = 0; i < m_size; i++)
			{
				if (version != m_version)
				{
					break;
				}
				action(m_items[i]);
			}
			if (version != m_version)
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
			}
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <internalonly />
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		public MyList<T> GetRange(int index, int count)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (m_size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			MyList<T> myList = new MyList<T>(count);
			Array.Copy(m_items, index, myList.m_items, 0, count);
			myList.m_size = count;
			return myList;
		}

		public int IndexOf(T item)
		{
			return Array.IndexOf(m_items, item, 0, m_size);
		}

		int IList.IndexOf(object item)
		{
			if (IsCompatibleObject(item))
			{
				return IndexOf((T)item);
			}
			return -1;
		}

		public int IndexOf(T item, int index)
		{
			if (index > m_size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			return Array.IndexOf(m_items, item, index, m_size - index);
		}

		public int IndexOf(T item, int index, int count)
		{
			if (index > m_size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (count < 0 || index > m_size - count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
			}
			return Array.IndexOf(m_items, item, index, count);
		}

		public void Insert(int index, T item)
		{
			if ((uint)index > (uint)m_size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
			}
			if (m_size == m_items.Length)
			{
				EnsureCapacity(m_size + 1);
			}
			if (index < m_size)
			{
				Array.Copy(m_items, index, m_items, index + 1, m_size - index);
			}
			m_items[index] = item;
			m_size++;
			m_version++;
		}

		void IList.Insert(int index, object item)
		{
			ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);
			try
			{
				Insert(index, (T)item);
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(item, typeof(T));
			}
		}

		public void InsertRange(int index, IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			if ((uint)index > (uint)m_size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				if (count > 0)
				{
					EnsureCapacity(m_size + count);
					if (index < m_size)
					{
						Array.Copy(m_items, index, m_items, index + count, m_size - index);
					}
					if (this == collection2)
					{
						Array.Copy(m_items, 0, m_items, index, index);
						Array.Copy(m_items, index + count, m_items, index * 2, m_size - index);
					}
					else
					{
						collection2.CopyTo(m_items, index);
					}
					m_size += count;
				}
			}
			else
			{
				using (IEnumerator<T> enumerator = collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Insert(index++, enumerator.Current);
					}
				}
			}
			m_version++;
		}

		public int LastIndexOf(T item)
		{
			if (m_size == 0)
			{
				return -1;
			}
			return LastIndexOf(item, m_size - 1, m_size);
		}

		public int LastIndexOf(T item, int index)
		{
			if (index >= m_size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			return LastIndexOf(item, index, index + 1);
		}

		public int LastIndexOf(T item, int index, int count)
		{
			if (Count != 0 && index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (Count != 0 && count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (m_size == 0)
			{
				return -1;
			}
			if (index >= m_size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
			}
			if (count > index + 1)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
			}
			return Array.LastIndexOf(m_items, item, index, count);
		}

		public bool Remove(T item)
		{
			int num = IndexOf(item);
			if (num >= 0)
			{
				RemoveAt(num);
				return true;
			}
			return false;
		}

		void IList.Remove(object item)
		{
			if (IsCompatibleObject(item))
			{
				Remove((T)item);
			}
		}

		public int RemoveAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			int i;
			for (i = 0; i < m_size && !match(m_items[i]); i++)
			{
			}
			if (i >= m_size)
			{
				return 0;
			}
			int j = i + 1;
			while (j < m_size)
			{
				for (; j < m_size && match(m_items[j]); j++)
				{
				}
				if (j < m_size)
				{
					m_items[i++] = m_items[j++];
				}
			}
			Array.Clear(m_items, i, m_size - i);
			int result = m_size - i;
			m_size = i;
			m_version++;
			return result;
		}

		public void RemoveAt(int index)
		{
			if ((uint)index >= (uint)m_size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			m_size--;
			if (index < m_size)
			{
				Array.Copy(m_items, index + 1, m_items, index, m_size - index);
			}
			m_items[m_size] = default(T);
			m_version++;
		}

		public void RemoveRange(int index, int count)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (m_size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			if (count > 0)
			{
				int size = m_size;
				m_size -= count;
				if (index < m_size)
				{
					Array.Copy(m_items, index + count, m_items, index, m_size - index);
				}
				Array.Clear(m_items, m_size, count);
				m_version++;
			}
		}

		public void Reverse()
		{
			Reverse(0, Count);
		}

		public void Reverse(int index, int count)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (m_size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Reverse((Array)m_items, index, count);
			m_version++;
		}

		public void Sort()
		{
			Sort(0, Count, null);
		}

		public void Sort(IComparer<T> comparer)
		{
			Sort(0, Count, comparer);
		}

		public void Sort(int index, int count, IComparer<T> comparer)
		{
			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (m_size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Sort(m_items, index, count, comparer);
			m_version++;
		}

		public void Sort(Comparison<T> comparison)
		{
			if (comparison == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			if (m_size > 0)
			{
				IComparer<T> comparer = new FunctorComparer<T>(comparison);
				Array.Sort(m_items, 0, m_size, comparer);
			}
		}

		public T[] ToArray()
		{
			T[] array = new T[m_size];
			Array.Copy(m_items, 0, array, 0, m_size);
			return array;
		}

		public void TrimExcess()
		{
			int num = (int)((double)m_items.Length * 0.9);
			if (m_size < num)
			{
				Capacity = m_size;
			}
		}

		public bool TrueForAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < m_size; i++)
			{
				if (!match(m_items[i]))
				{
					return false;
				}
			}
			return true;
		}

		internal static IList<T> Synchronized(List<T> list)
		{
			return new SynchronizedList(list);
		}
	}
}
