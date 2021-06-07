using ParallelTasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Library.Memory;
using VRage.Network;

namespace VRage.Library.Collections
{
	[DebuggerDisplay("Count = {Count}")]
	public class NativeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IDisposable where TKey : unmanaged where TValue : unmanaged
	{
		private struct Entry
		{
			public int hashCode;

			public int next;

			public TKey key;

			public TValue value;
		}

		private struct UnmanagedMemory<T> : IDisposable
		{
			public readonly int Length;

			public NativeArray Array
			{
				get;
				private set;
			}

			public bool IsNull => Array == null;

			public unsafe ref T this[int i] => ref Unsafe.Add(ref Unsafe.AsRef<T>(Array.Ptr.ToPointer()), i);

			private UnmanagedMemory(NativeArray array, int length)
			{
				Array = array;
				Length = length;
			}

			public static UnmanagedMemory<T> Create(int size)
			{
				NativeArray array = NativeDictionary<TKey, TValue>.m_nativeArrayAllocator.Allocate(size * Unsafe.SizeOf<T>());
				return new UnmanagedMemory<T>(array, size);
			}

			public void Dispose()
			{
				NativeDictionary<TKey, TValue>.m_nativeArrayAllocator.Dispose(Array);
				Array = null;
			}

			public void Clear(int count)
			{
				for (int i = 0; i < count; i++)
				{
					this[i] = default(T);
				}
			}

			public void Clear()
			{
				Clear(Length);
			}

			public Span<T> GetSpan()
			{
				return Array.AsSpan<T>(Length);
			}
		}

		[Serializable]
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable, IDictionaryEnumerator
		{
			protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EEnumerator_003C_003Edictionary_003C_003EAccessor : IMemberAccessor<Enumerator, NativeDictionary<TKey, TValue>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Enumerator owner, in NativeDictionary<TKey, TValue> value)
				{
					owner.dictionary = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Enumerator owner, out NativeDictionary<TKey, TValue> value)
				{
					value = owner.dictionary;
				}
			}

			protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EEnumerator_003C_003Eversion_003C_003EAccessor : IMemberAccessor<Enumerator, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Set(ref Enumerator owner, in int value)
				{
					((NativeDictionary<, >.Enumerator*)(&owner))->version = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Get(ref Enumerator owner, out int value)
				{
					value = ((NativeDictionary<, >.Enumerator*)(&owner))->version;
				}
			}

			protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EEnumerator_003C_003Eindex_003C_003EAccessor : IMemberAccessor<Enumerator, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Set(ref Enumerator owner, in int value)
				{
					((NativeDictionary<, >.Enumerator*)(&owner))->index = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Get(ref Enumerator owner, out int value)
				{
					value = ((NativeDictionary<, >.Enumerator*)(&owner))->index;
				}
			}

			protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EEnumerator_003C_003Ecurrent_003C_003EAccessor : IMemberAccessor<Enumerator, KeyValuePair<TKey, TValue>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Enumerator owner, in KeyValuePair<TKey, TValue> value)
				{
					owner.current = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Enumerator owner, out KeyValuePair<TKey, TValue> value)
				{
					value = owner.current;
				}
			}

			protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EEnumerator_003C_003EgetEnumeratorRetType_003C_003EAccessor : IMemberAccessor<Enumerator, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Set(ref Enumerator owner, in int value)
				{
					((NativeDictionary<, >.Enumerator*)(&owner))->getEnumeratorRetType = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public unsafe sealed override void Get(ref Enumerator owner, out int value)
				{
					value = ((NativeDictionary<, >.Enumerator*)(&owner))->getEnumeratorRetType;
				}
			}

			private NativeDictionary<TKey, TValue> dictionary;

			private int version;

			private int index;

			private KeyValuePair<TKey, TValue> current;

			private int getEnumeratorRetType;

			internal const int DictEntry = 1;

			internal const int KeyValuePair = 2;

			public KeyValuePair<TKey, TValue> Current => current;

			object IEnumerator.Current
			{
				get
				{
					if (index == 0 || index == dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					if (getEnumeratorRetType == 1)
					{
						return new DictionaryEntry(current.Key, current.Value);
					}
					return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
				}
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (index == 0 || index == dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return new DictionaryEntry(current.Key, current.Value);
				}
			}

			object IDictionaryEnumerator.Key
			{
				get
				{
					if (index == 0 || index == dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return current.Key;
				}
			}

			object IDictionaryEnumerator.Value
			{
				get
				{
					if (index == 0 || index == dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return current.Value;
				}
			}

			internal Enumerator(NativeDictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
			{
				this.dictionary = dictionary;
				version = dictionary.version;
				index = 0;
				this.getEnumeratorRetType = getEnumeratorRetType;
				current = default(KeyValuePair<TKey, TValue>);
			}

			public bool MoveNext()
			{
				if (version != dictionary.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				while ((uint)index < (uint)dictionary.count)
				{
					if (dictionary.entries[index].hashCode >= 0)
					{
						current = new KeyValuePair<TKey, TValue>(dictionary.entries[index].key, dictionary.entries[index].value);
						index++;
						return true;
					}
					index++;
				}
				index = dictionary.count + 1;
				current = default(KeyValuePair<TKey, TValue>);
				return false;
			}

			public void Dispose()
			{
			}

			void IEnumerator.Reset()
			{
				if (version != dictionary.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				index = 0;
				current = default(KeyValuePair<TKey, TValue>);
			}
		}

		[Serializable]
		[DebuggerDisplay("Count = {Count}")]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, IEnumerable, ICollection, IReadOnlyCollection<TKey>
		{
			[Serializable]
			public struct Enumerator : IEnumerator<TKey>, IEnumerator, IDisposable
			{
				protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EKeyCollection_003C_003EEnumerator_003C_003Edictionary_003C_003EAccessor : IMemberAccessor<Enumerator, NativeDictionary<TKey, TValue>>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref Enumerator owner, in NativeDictionary<TKey, TValue> value)
					{
						owner.dictionary = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref Enumerator owner, out NativeDictionary<TKey, TValue> value)
					{
						value = owner.dictionary;
					}
				}

				protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EKeyCollection_003C_003EEnumerator_003C_003Eindex_003C_003EAccessor : IMemberAccessor<Enumerator, int>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public unsafe sealed override void Set(ref Enumerator owner, in int value)
					{
						((NativeDictionary<, >.KeyCollection.Enumerator*)(&owner))->index = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public unsafe sealed override void Get(ref Enumerator owner, out int value)
					{
						value = ((NativeDictionary<, >.KeyCollection.Enumerator*)(&owner))->index;
					}
				}

				protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EKeyCollection_003C_003EEnumerator_003C_003Eversion_003C_003EAccessor : IMemberAccessor<Enumerator, int>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public unsafe sealed override void Set(ref Enumerator owner, in int value)
					{
						((NativeDictionary<, >.KeyCollection.Enumerator*)(&owner))->version = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public unsafe sealed override void Get(ref Enumerator owner, out int value)
					{
						value = ((NativeDictionary<, >.KeyCollection.Enumerator*)(&owner))->version;
					}
				}

				protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EKeyCollection_003C_003EEnumerator_003C_003EcurrentKey_003C_003EAccessor : IMemberAccessor<Enumerator, TKey>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref Enumerator owner, in TKey value)
					{
						owner.currentKey = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref Enumerator owner, out TKey value)
					{
						value = owner.currentKey;
					}
				}

				private NativeDictionary<TKey, TValue> dictionary;

				private int index;

				private int version;

				private TKey currentKey;

				public TKey Current => currentKey;

				object IEnumerator.Current
				{
					get
					{
						if (index == 0 || index == dictionary.count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return currentKey;
					}
				}

				internal Enumerator(NativeDictionary<TKey, TValue> dictionary)
				{
					this.dictionary = dictionary;
					version = dictionary.version;
					index = 0;
					currentKey = default(TKey);
				}

				public void Dispose()
				{
				}

				public bool MoveNext()
				{
					if (version != dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					while ((uint)index < (uint)dictionary.count)
					{
						if (dictionary.entries[index].hashCode >= 0)
						{
							currentKey = dictionary.entries[index].key;
							index++;
							return true;
						}
						index++;
					}
					index = dictionary.count + 1;
					currentKey = default(TKey);
					return false;
				}

				void IEnumerator.Reset()
				{
					if (version != dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					index = 0;
					currentKey = default(TKey);
				}
			}

			protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EKeyCollection_003C_003Edictionary_003C_003EAccessor : IMemberAccessor<KeyCollection, NativeDictionary<TKey, TValue>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref KeyCollection owner, in NativeDictionary<TKey, TValue> value)
				{
					owner.dictionary = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref KeyCollection owner, out NativeDictionary<TKey, TValue> value)
				{
					value = owner.dictionary;
				}
			}

			private NativeDictionary<TKey, TValue> dictionary;

			public int Count => dictionary.Count;

			bool ICollection<TKey>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)dictionary).SyncRoot;

			public KeyCollection(NativeDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(dictionary);
			}

			public void CopyTo(TKey[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = dictionary.count;
				UnmanagedMemory<Entry> entries = dictionary.entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].key;
					}
				}
			}

			void ICollection<TKey>.Add(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			void ICollection<TKey>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			bool ICollection<TKey>.Contains(TKey item)
			{
				return dictionary.ContainsKey(item);
			}

			bool ICollection<TKey>.Remove(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
				return false;
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return new Enumerator(dictionary);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(dictionary);
			}

			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (array.Rank != 1)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				if (array.GetLowerBound(0) != 0)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TKey[] array2 = array as TKey[];
				if (array2 != null)
				{
					CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if (array3 == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				int count = dictionary.count;
				UnmanagedMemory<Entry> entries = dictionary.entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].key;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}
		}

		[Serializable]
		[DebuggerDisplay("Count = {Count}")]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, IEnumerable, ICollection, IReadOnlyCollection<TValue>
		{
			[Serializable]
			public struct Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
			{
				protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EValueCollection_003C_003EEnumerator_003C_003Edictionary_003C_003EAccessor : IMemberAccessor<Enumerator, NativeDictionary<TKey, TValue>>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref Enumerator owner, in NativeDictionary<TKey, TValue> value)
					{
						owner.dictionary = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref Enumerator owner, out NativeDictionary<TKey, TValue> value)
					{
						value = owner.dictionary;
					}
				}

				protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EValueCollection_003C_003EEnumerator_003C_003Eindex_003C_003EAccessor : IMemberAccessor<Enumerator, int>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public unsafe sealed override void Set(ref Enumerator owner, in int value)
					{
						((NativeDictionary<, >.ValueCollection.Enumerator*)(&owner))->index = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public unsafe sealed override void Get(ref Enumerator owner, out int value)
					{
						value = ((NativeDictionary<, >.ValueCollection.Enumerator*)(&owner))->index;
					}
				}

				protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EValueCollection_003C_003EEnumerator_003C_003Eversion_003C_003EAccessor : IMemberAccessor<Enumerator, int>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public unsafe sealed override void Set(ref Enumerator owner, in int value)
					{
						((NativeDictionary<, >.ValueCollection.Enumerator*)(&owner))->version = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public unsafe sealed override void Get(ref Enumerator owner, out int value)
					{
						value = ((NativeDictionary<, >.ValueCollection.Enumerator*)(&owner))->version;
					}
				}

				protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EValueCollection_003C_003EEnumerator_003C_003EcurrentValue_003C_003EAccessor : IMemberAccessor<Enumerator, TValue>
				{
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Set(ref Enumerator owner, in TValue value)
					{
						owner.currentValue = value;
					}

					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public sealed override void Get(ref Enumerator owner, out TValue value)
					{
						value = owner.currentValue;
					}
				}

				private NativeDictionary<TKey, TValue> dictionary;

				private int index;

				private int version;

				private TValue currentValue;

				public TValue Current => currentValue;

				object IEnumerator.Current
				{
					get
					{
						if (index == 0 || index == dictionary.count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return currentValue;
					}
				}

				internal Enumerator(NativeDictionary<TKey, TValue> dictionary)
				{
					this.dictionary = dictionary;
					version = dictionary.version;
					index = 0;
					currentValue = default(TValue);
				}

				public void Dispose()
				{
				}

				public bool MoveNext()
				{
					if (version != dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					while ((uint)index < (uint)dictionary.count)
					{
						if (dictionary.entries[index].hashCode >= 0)
						{
							currentValue = dictionary.entries[index].value;
							index++;
							return true;
						}
						index++;
					}
					index = dictionary.count + 1;
					currentValue = default(TValue);
					return false;
				}

				void IEnumerator.Reset()
				{
					if (version != dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					index = 0;
					currentValue = default(TValue);
				}
			}

			protected class VRage_Library_Collections_NativeDictionary_00602_003C_003EValueCollection_003C_003Edictionary_003C_003EAccessor : IMemberAccessor<ValueCollection, NativeDictionary<TKey, TValue>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ValueCollection owner, in NativeDictionary<TKey, TValue> value)
				{
					owner.dictionary = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ValueCollection owner, out NativeDictionary<TKey, TValue> value)
				{
					value = owner.dictionary;
				}
			}

			private NativeDictionary<TKey, TValue> dictionary;

			public int Count => dictionary.Count;

			bool ICollection<TValue>.IsReadOnly => true;

			bool ICollection.IsSynchronized => false;

			object ICollection.SyncRoot => ((ICollection)dictionary).SyncRoot;

			public ValueCollection(NativeDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			public Enumerator GetEnumerator()
			{
				return new Enumerator(dictionary);
			}

			public void CopyTo(TValue[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = dictionary.count;
				UnmanagedMemory<Entry> entries = dictionary.entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].value;
					}
				}
			}

			void ICollection<TValue>.Add(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			bool ICollection<TValue>.Remove(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
				return false;
			}

			void ICollection<TValue>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			bool ICollection<TValue>.Contains(TValue item)
			{
				return dictionary.ContainsValue(item);
			}

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return new Enumerator(dictionary);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Enumerator(dictionary);
			}

			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (array.Rank != 1)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				if (array.GetLowerBound(0) != 0)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TValue[] array2 = array as TValue[];
				if (array2 != null)
				{
					CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if (array3 == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				int count = dictionary.count;
				UnmanagedMemory<Entry> entries = dictionary.entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].value;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}
		}

		private static readonly NativeArrayAllocator m_nativeArrayAllocator = new NativeArrayAllocator(Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("NativeDictionaries"));

		private UnmanagedMemory<int> buckets;

		private UnmanagedMemory<Entry> entries;

		private int count;

		private int version;

		private int freeList;

		private int freeCount;

		private IEqualityComparer<TKey> comparer;

		private KeyCollection keys;

		private ValueCollection values;

		private object _syncRoot;

		private const string VersionName = "Version";

		private const string HashSizeName = "HashSize";

		private const string KeyValuePairsName = "KeyValuePairs";

		private const string ComparerName = "Comparer";

		public IEqualityComparer<TKey> Comparer => comparer;

		public int Count => count - freeCount;

		public KeyCollection Keys
		{
			get
			{
				if (keys == null)
				{
					keys = new KeyCollection(this);
				}
				return keys;
			}
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				if (keys == null)
				{
					keys = new KeyCollection(this);
				}
				return keys;
			}
		}

		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			get
			{
				if (keys == null)
				{
					keys = new KeyCollection(this);
				}
				return keys;
			}
		}

		public ValueCollection Values
		{
			get
			{
				if (values == null)
				{
					values = new ValueCollection(this);
				}
				return values;
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				if (values == null)
				{
					values = new ValueCollection(this);
				}
				return values;
			}
		}

		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			get
			{
				if (values == null)
				{
					values = new ValueCollection(this);
				}
				return values;
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				int num = FindEntry(key);
				if (num >= 0)
				{
					return entries[num].value;
				}
				ThrowHelper.ThrowKeyNotFoundException();
				return default(TValue);
			}
			set
			{
				Insert(key, value, add: false);
			}
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref _syncRoot, new object(), (object)null);
				}
				return _syncRoot;
			}
		}

		bool IDictionary.IsFixedSize => false;

		bool IDictionary.IsReadOnly => false;

		ICollection IDictionary.Keys => Keys;

		ICollection IDictionary.Values => Values;

		object IDictionary.this[object key]
		{
			get
			{
				if (IsCompatibleKey(key))
				{
					int num = FindEntry((TKey)key);
					if (num >= 0)
					{
						return entries[num].value;
					}
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
				}
				ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
				try
				{
					TKey key2 = (TKey)key;
					try
					{
						this[key2] = (TValue)value;
					}
					catch (InvalidCastException)
					{
						ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
					}
				}
				catch (InvalidCastException)
				{
					ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
				}
			}
		}

		public NativeDictionary()
			: this(0, (IEqualityComparer<TKey>)null)
		{
		}

		public NativeDictionary(int capacity)
			: this(capacity, (IEqualityComparer<TKey>)null)
		{
		}

		public NativeDictionary(IEqualityComparer<TKey> comparer)
			: this(0, comparer)
		{
		}

		public NativeDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
			}
			if (capacity > 0)
			{
				Initialize(capacity);
			}
			this.comparer = (comparer ?? EqualityComparer<TKey>.Default);
		}

		public NativeDictionary(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, (IEqualityComparer<TKey>)null)
		{
		}

		public NativeDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: this(dictionary?.Count ?? 0, comparer)
		{
			if (dictionary == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
			}
			foreach (KeyValuePair<TKey, TValue> item in dictionary)
			{
				Add(item.Key, item.Value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			Insert(key, value, add: true);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			Add(keyValuePair.Key, keyValuePair.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = FindEntry(keyValuePair.Key);
			if (num >= 0 && EqualityComparer<TValue>.Default.Equals(entries[num].value, keyValuePair.Value))
			{
				return true;
			}
			return false;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = FindEntry(keyValuePair.Key);
			if (num >= 0 && EqualityComparer<TValue>.Default.Equals(entries[num].value, keyValuePair.Value))
			{
				Remove(keyValuePair.Key);
				return true;
			}
			return false;
		}

		public void Clear()
		{
			if (count > 0)
			{
				for (int i = 0; i < buckets.Length; i++)
				{
					buckets[i] = -1;
				}
				entries.Clear(count);
				freeList = -1;
				count = 0;
				freeCount = 0;
				version++;
			}
		}

		public bool ContainsKey(TKey key)
		{
			return FindEntry(key) >= 0;
		}

		public bool ContainsValue(TValue value)
		{
			EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
			for (int i = 0; i < count; i++)
			{
				if (entries[i].hashCode >= 0 && @default.Equals(entries[i].value, value))
				{
					return true;
				}
			}
			return false;
		}

		private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (index < 0 || index > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			int num = count;
			UnmanagedMemory<Entry> unmanagedMemory = entries;
			for (int i = 0; i < num; i++)
			{
				if (unmanagedMemory[i].hashCode >= 0)
				{
					array[index++] = new KeyValuePair<TKey, TValue>(unmanagedMemory[i].key, unmanagedMemory[i].value);
				}
			}
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		private int FindEntry(TKey key)
		{
			if (!buckets.IsNull)
			{
				int num = comparer.GetHashCode(key) & int.MaxValue;
				for (int num2 = buckets[num % buckets.Length]; num2 >= 0; num2 = entries[num2].next)
				{
					if (entries[num2].hashCode == num && comparer.Equals(entries[num2].key, key))
					{
						return num2;
					}
				}
			}
			return -1;
		}

		private void Initialize(int capacity)
		{
			int prime = HashHelpers.GetPrime(capacity);
			buckets = UnmanagedMemory<int>.Create(prime * 4);
			for (int i = 0; i < buckets.Length; i++)
			{
				buckets[i] = -1;
			}
			entries = UnmanagedMemory<Entry>.Create(prime);
			entries.Clear();
			freeList = -1;
		}

		private void Insert(TKey key, TValue value, bool add)
		{
			if (buckets.IsNull)
			{
				Initialize(0);
			}
			int num = comparer.GetHashCode(key) & int.MaxValue;
			int i = num % buckets.Length;
			for (int num2 = buckets[i]; num2 >= 0; num2 = entries[num2].next)
			{
				if (entries[num2].hashCode == num && comparer.Equals(entries[num2].key, key))
				{
					if (add)
					{
						ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
					}
					entries[num2].value = value;
					version++;
					return;
				}
			}
			int num3;
			if (freeCount > 0)
			{
				num3 = freeList;
				freeList = entries[num3].next;
				freeCount--;
			}
			else
			{
				if (count == entries.Length)
				{
					Resize();
					i = num % buckets.Length;
				}
				num3 = count;
				count++;
			}
			entries[num3].hashCode = num;
			entries[num3].next = buckets[i];
			entries[num3].key = key;
			entries[num3].value = value;
			buckets[i] = num3;
			version++;
		}

		private void Resize()
		{
			Resize(HashHelpers.ExpandPrime(count), forceNewHashCodes: false);
		}

		private void Resize(int newSize, bool forceNewHashCodes)
		{
			UnmanagedMemory<int> unmanagedMemory = UnmanagedMemory<int>.Create(newSize);
			UnmanagedMemory<Entry> unmanagedMemory2 = UnmanagedMemory<Entry>.Create(newSize);
			Span<int> span = unmanagedMemory.Array.AsSpan<int>(newSize);
			for (int i = 0; i < span.Length; i++)
			{
				span[i] = -1;
			}
			Span<Entry> destination = unmanagedMemory2.Array.AsSpan<Entry>(newSize);
			destination.Clear();
			Span<Entry> span2 = entries.GetSpan();
			span2 = span2.Slice(0, count);
			span2.CopyTo(destination);
			if (forceNewHashCodes)
			{
				for (int j = 0; j < count; j++)
				{
					if (destination[j].hashCode != -1)
					{
						destination[j].hashCode = (comparer.GetHashCode(destination[j].key) & int.MaxValue);
					}
				}
			}
			for (int k = 0; k < count; k++)
			{
				if (destination[k].hashCode >= 0)
				{
					int index = destination[k].hashCode % newSize;
					destination[k].next = span[index];
					span[index] = k;
				}
			}
			buckets.Dispose();
			buckets = unmanagedMemory;
			entries.Dispose();
			entries = unmanagedMemory2;
		}

		public bool Remove(TKey key)
		{
			if (!buckets.IsNull)
			{
				int num = comparer.GetHashCode(key) & int.MaxValue;
				int i = num % buckets.Length;
				int num2 = -1;
				for (int num3 = buckets[i]; num3 >= 0; num3 = entries[num3].next)
				{
					if (entries[num3].hashCode == num && comparer.Equals(entries[num3].key, key))
					{
						if (num2 < 0)
						{
							buckets[i] = entries[num3].next;
						}
						else
						{
							entries[num2].next = entries[num3].next;
						}
						entries[num3].hashCode = -1;
						entries[num3].next = freeList;
						entries[num3].key = default(TKey);
						entries[num3].value = default(TValue);
						freeList = num3;
						freeCount++;
						version++;
						return true;
					}
					num2 = num3;
				}
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			int num = FindEntry(key);
			if (num >= 0)
			{
				value = entries[num].value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		internal TValue GetValueOrDefault(TKey key)
		{
			int num = FindEntry(key);
			if (num >= 0)
			{
				return entries[num].value;
			}
			return default(TValue);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			CopyTo(array, index);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if (array.GetLowerBound(0) != 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
			}
			if (index < 0 || index > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				CopyTo(array2, index);
			}
			else if (array is DictionaryEntry[])
			{
				DictionaryEntry[] array3 = array as DictionaryEntry[];
				UnmanagedMemory<Entry> unmanagedMemory = entries;
				for (int i = 0; i < count; i++)
				{
					if (unmanagedMemory[i].hashCode >= 0)
					{
						array3[index++] = new DictionaryEntry(unmanagedMemory[i].key, unmanagedMemory[i].value);
					}
				}
			}
			else
			{
				object[] array4 = array as object[];
				if (array4 == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				try
				{
					int num = count;
					UnmanagedMemory<Entry> unmanagedMemory2 = entries;
					for (int j = 0; j < num; j++)
					{
						if (unmanagedMemory2[j].hashCode >= 0)
						{
							array4[index++] = new KeyValuePair<TKey, TValue>(unmanagedMemory2[j].key, unmanagedMemory2[j].value);
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this, 2);
		}

		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			return key is TKey;
		}

		void IDictionary.Add(object key, object value)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			ThrowHelper.IfNullAndNullsAreIllegalThenThrow<TValue>(value, ExceptionArgument.value);
			try
			{
				TKey key2 = (TKey)key;
				try
				{
					Add(key2, (TValue)value);
				}
				catch (InvalidCastException)
				{
					ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
				}
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
			}
		}

		bool IDictionary.Contains(object key)
		{
			if (IsCompatibleKey(key))
			{
				return ContainsKey((TKey)key);
			}
			return false;
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Enumerator(this, 1);
		}

		void IDictionary.Remove(object key)
		{
			if (IsCompatibleKey(key))
			{
				Remove((TKey)key);
			}
		}

		public void Dispose()
		{
			Clear();
			buckets.Dispose();
			buckets = default(UnmanagedMemory<int>);
			entries.Dispose();
			entries = default(UnmanagedMemory<Entry>);
		}
	}
}
