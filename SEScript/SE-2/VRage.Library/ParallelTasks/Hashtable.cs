using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Library.Threading;

namespace ParallelTasks
{
	public class Hashtable<TKey, TData> : IEnumerable<KeyValuePair<TKey, TData>>, IEnumerable
	{
		private static readonly EqualityComparer<TKey> KeyComparer = EqualityComparer<TKey>.Default;

		public volatile HashtableNode<TKey, TData>[] array;

		private SpinLock writeLock;

		private static readonly HashtableNode<TKey, TData> DeletedNode = new HashtableNode<TKey, TData>(default(TKey), default(TData), HashtableToken.Deleted);

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ParallelTasks.Hashtable`2" /> class.
		/// </summary>
		/// <param name="initialCapacity">The initial capacity of the table.</param>
		public Hashtable(int initialCapacity)
		{
			if (initialCapacity < 1)
			{
				throw new ArgumentOutOfRangeException("initialCapacity", "cannot be < 1");
			}
			array = new HashtableNode<TKey, TData>[initialCapacity];
			writeLock = default(SpinLock);
		}

		/// <summary>
		/// Adds an item to this hashtable.
		/// </summary>
		/// <param name="key">The key at which to add the item.</param>
		/// <param name="data">The data to add.</param>
		public void Add(TKey key, TData data)
		{
			try
			{
				writeLock.Enter();
				if (!Insert(array, key, data))
				{
					Resize();
					Insert(array, key, data);
				}
			}
			finally
			{
				writeLock.Exit();
			}
		}

		private void Resize()
		{
			HashtableNode<TKey, TData>[] table = new HashtableNode<TKey, TData>[array.Length * 2];
			for (int i = 0; i < array.Length; i++)
			{
				HashtableNode<TKey, TData> hashtableNode = array[i];
				if (hashtableNode.Token == HashtableToken.Used)
				{
					Insert(table, hashtableNode.Key, hashtableNode.Data);
				}
			}
			array = table;
		}

		private bool Insert(HashtableNode<TKey, TData>[] table, TKey key, TData data)
		{
			int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % table.Length;
			int num2 = num;
			bool result = false;
			do
			{
				HashtableNode<TKey, TData> hashtableNode = table[num2];
				if (hashtableNode.Token == HashtableToken.Empty || hashtableNode.Token == HashtableToken.Deleted || KeyComparer.Equals(key, hashtableNode.Key))
				{
					table[num2] = new HashtableNode<TKey, TData>
					{
						Key = key,
						Data = data,
						Token = HashtableToken.Used
					};
					result = true;
					break;
				}
				num2 = (num2 + 1) % table.Length;
			}
			while (num2 != num);
			return result;
		}

		/// <summary>
		/// Sets the value of the item at the specified key location.
		/// This is only guaranteed to work correctly if no other thread is modifying the same key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The new value.</param>
		public void UnsafeSet(TKey key, TData value)
		{
			bool flag = false;
			HashtableNode<TKey, TData>[] array;
			do
			{
				array = this.array;
				int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % array.Length;
				int num2 = num;
				do
				{
					HashtableNode<TKey, TData> hashtableNode = array[num2];
					if (KeyComparer.Equals(key, hashtableNode.Key))
					{
						array[num2] = new HashtableNode<TKey, TData>
						{
							Key = key,
							Data = value,
							Token = HashtableToken.Used
						};
						flag = true;
						break;
					}
					num2 = (num2 + 1) % array.Length;
				}
				while (num2 != num);
			}
			while (array != this.array);
			if (!flag)
			{
				Add(key, value);
			}
		}

		private bool Find(TKey key, out HashtableNode<TKey, TData> node)
		{
			node = default(HashtableNode<TKey, TData>);
			HashtableNode<TKey, TData>[] array = this.array;
			int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % array.Length;
			int num2 = num;
			do
			{
				HashtableNode<TKey, TData> hashtableNode = array[num2];
				if (hashtableNode.Token == HashtableToken.Empty)
				{
					return false;
				}
				if (hashtableNode.Token == HashtableToken.Deleted || !KeyComparer.Equals(key, hashtableNode.Key))
				{
					num2 = (num2 + 1) % array.Length;
					continue;
				}
				node = hashtableNode;
				return true;
			}
			while (num2 != num);
			return false;
		}

		/// <summary>
		/// Tries to get the data at the specified key location.
		/// </summary>
		/// <param name="key">The key to search for.</param>
		/// <param name="data">The data at the key location.</param>
		/// <returns><c>true</c> if the data was found; else <c>false</c>.</returns>
		public bool TryGet(TKey key, out TData data)
		{
			if (Find(key, out HashtableNode<TKey, TData> node))
			{
				data = node.Data;
				return true;
			}
			data = default(TData);
			return false;
		}

		/// <summary>
		/// Removes the data at the specified key location.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Remove(TKey key)
		{
			try
			{
				writeLock.Enter();
				HashtableNode<TKey, TData>[] array = this.array;
				int num = Math.Abs(GetHashCode_HashTable<TKey>.GetHashCode(key)) % array.Length;
				int num2 = num;
				do
				{
					HashtableNode<TKey, TData> hashtableNode = array[num2];
					if (hashtableNode.Token == HashtableToken.Empty)
					{
						break;
					}
					if (hashtableNode.Token == HashtableToken.Deleted || !KeyComparer.Equals(key, hashtableNode.Key))
					{
						num2 = (num2 + 1) % array.Length;
					}
					else
					{
						array[num2] = DeletedNode;
					}
				}
				while (num2 != num);
			}
			finally
			{
				writeLock.Exit();
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<TKey, TData>> GetEnumerator()
		{
			return new HashTableEnumerator<TKey, TData>(this);
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
