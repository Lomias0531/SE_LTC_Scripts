using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VRage.Collections
{
	public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>
	{
		/// <summary>
		/// Enumerator which uses index access.
		/// Index access on Collection is O(1) operation
		/// </summary>
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private ObservableCollection<T> m_collection;

			private int m_index;

			public T Current => m_collection[m_index];

			object IEnumerator.Current => Current;

			public Enumerator(ObservableCollection<T> collection)
			{
				m_index = -1;
				m_collection = collection;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				m_index++;
				return m_index < m_collection.Count;
			}

			public void Reset()
			{
				m_index = -1;
			}
		}

		public bool FireEvents = true;

		/// <summary>
		/// Clears the items.
		/// </summary>
		protected override void ClearItems()
		{
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this);
			if (FireEvents)
			{
				OnCollectionChanged(e);
			}
			base.ClearItems();
		}

		/// <summary>
		/// Gets allocation free enumerator (returns struct)
		/// </summary>
		public new Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		public int FindIndex(Predicate<T> match)
		{
			int result = -1;
			for (int i = 0; i < base.Items.Count; i++)
			{
				if (match(base.Items[i]))
				{
					result = i;
					break;
				}
			}
			return result;
		}
	}
}
