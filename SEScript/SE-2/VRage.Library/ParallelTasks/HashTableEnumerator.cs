using System;
using System.Collections;
using System.Collections.Generic;

namespace ParallelTasks
{
	public class HashTableEnumerator<TKey, TData> : IEnumerator<KeyValuePair<TKey, TData>>, IEnumerator, IDisposable
	{
		private int currentIndex = -1;

		private Hashtable<TKey, TData> table;

		public KeyValuePair<TKey, TData> Current
		{
			get;
			private set;
		}

		object IEnumerator.Current => Current;

		public HashTableEnumerator(Hashtable<TKey, TData> table)
		{
			this.table = table;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			HashtableNode<TKey, TData> hashtableNode;
			do
			{
				currentIndex++;
				if (table.array.Length <= currentIndex)
				{
					return false;
				}
				hashtableNode = table.array[currentIndex];
			}
			while (hashtableNode.Token != HashtableToken.Used);
			Current = new KeyValuePair<TKey, TData>(hashtableNode.Key, hashtableNode.Data);
			return true;
		}

		public void Reset()
		{
			currentIndex = -1;
		}
	}
}
