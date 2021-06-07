using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VRage.Collections
{
	/// <summary>
	/// Allows access to queue by index
	/// Otherwise implementation is similar to regular queue
	/// </summary>
	public class MyQueue<T> : IEnumerable<T>, IEnumerable
	{
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private readonly int m_version;

			private readonly MyQueue<T> m_queue;

			private int m_index;

			private bool m_first;

			public T Current => m_queue.m_array[m_index];

			object IEnumerator.Current => Current;

			public Enumerator(MyQueue<T> queue)
			{
				m_index = 0;
				m_first = true;
				m_queue = queue;
				m_version = m_queue.m_version;
				Reset();
			}

			public bool MoveNext()
			{
				if (m_version != m_queue.m_version)
				{
					throw new InvalidOperationException("Collection modified");
				}
				if (m_queue.Count == 0)
				{
					return false;
				}
				m_index++;
				if (m_index == m_queue.m_array.Length)
				{
					m_index = 0;
				}
				if (m_first)
				{
					m_first = false;
				}
				else if (m_index == m_queue.m_tail)
				{
					return false;
				}
				return true;
			}

			public void Reset()
			{
				m_first = true;
				m_index = m_queue.m_head - 1;
			}

			public void Dispose()
			{
			}
		}

		protected T[] m_array;

		protected int m_head;

		protected int m_tail;

		protected int m_size;

		private int m_version;

		public T Tail
		{
			get
			{
				if (m_size == 0)
				{
					throw new InvalidOperationException("Queue is empty.");
				}
				return m_array[m_tail];
			}
		}

		public T[] InternalArray
		{
			get
			{
				T[] array = new T[Count];
				for (int i = 0; i < Count; i++)
				{
					array[i] = this[i];
				}
				return array;
			}
		}

		public int Count => m_size;

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
				{
					throw new ArgumentException("Index must be larger or equal to 0 and smaller than Count");
				}
				return m_array[(m_head + index) % m_array.Length];
			}
			set
			{
				if (index < 0 || index >= Count)
				{
					throw new ArgumentException("Index must be larger or equal to 0 and smaller than Count");
				}
				m_array[(m_head + index) % m_array.Length] = value;
			}
		}

		public MyQueue()
			: this(0)
		{
		}

		public MyQueue(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentException("Capacity cannot be < 0", "capacity");
			}
			m_array = new T[capacity];
			m_head = 0;
			m_tail = 0;
			m_size = 0;
			m_version = 0;
		}

		public MyQueue(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentException("Collection cannot be empty", "collection");
			}
			m_size = 0;
			m_version = 0;
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				m_array = new T[collection2.Count];
			}
			else
			{
				m_array = new T[4];
			}
			foreach (T item in collection)
			{
				Enqueue(item);
			}
		}

		public void Clear()
		{
			if (m_head < m_tail)
			{
				Array.Clear(m_array, m_head, m_size);
			}
			else
			{
				Array.Clear(m_array, m_head, m_array.Length - m_head);
				Array.Clear(m_array, 0, m_tail);
			}
			m_head = 0;
			m_tail = 0;
			m_size = 0;
			m_version++;
		}

		public void Enqueue(T item)
		{
			if (m_size == m_array.Length)
			{
				int num = (int)((long)m_array.Length * 200L / 100);
				if (num < m_array.Length + 4)
				{
					num = m_array.Length + 4;
				}
				SetCapacity(num);
			}
			m_array[m_tail] = item;
			m_tail = (m_tail + 1) % m_array.Length;
			m_size++;
			m_version++;
		}

		public T Peek()
		{
			if (m_size == 0)
			{
				throw new InvalidOperationException("Queue is empty");
			}
			return m_array[m_head];
		}

		public T Last()
		{
			if (m_size == 0)
			{
				throw new InvalidOperationException("Queue is empty");
			}
			return m_array[(m_tail - 1 + m_array.Length) % m_array.Length];
		}

		public T Dequeue()
		{
			if (m_size == 0)
			{
				throw new InvalidOperationException("Queue is empty");
			}
			T result = m_array[m_head];
			m_array[m_head] = default(T);
			m_head = (m_head + 1) % m_array.Length;
			m_size--;
			m_version++;
			return result;
		}

		public bool TryDequeue(out T item)
		{
			if (m_size > 0)
			{
				item = Dequeue();
				return true;
			}
			item = default(T);
			return false;
		}

		public bool Contains(T item)
		{
			int num = m_head;
			int num2 = 0;
			while (num2 < m_size)
			{
				if (m_array[num % m_array.Length].Equals(item))
				{
					return true;
				}
				num2++;
				num++;
			}
			return false;
		}

		public bool Remove(T item)
		{
			int num = m_head;
			int num2 = 0;
			while (num2 < m_size && !m_array[num % m_array.Length].Equals(item))
			{
				num2++;
				num++;
			}
			if (num2 == m_size)
			{
				return false;
			}
			Remove(num);
			return true;
		}

		public bool RemoveWhere(Func<T, bool> predicate, out T item)
		{
			int num = m_head;
			int num2 = 0;
			while (num2 < m_size && !predicate(m_array[num % m_array.Length]))
			{
				num2++;
				num++;
			}
			if (num2 == m_size)
			{
				item = default(T);
				return false;
			}
			item = m_array[num];
			Remove(num);
			return true;
		}

		public void Remove(int idx)
		{
			if (idx >= m_size)
			{
				throw new InvalidOperationException("Index out of range " + idx + "/" + m_size);
			}
			if (idx == 0)
			{
				Dequeue();
				return;
			}
			int num = idx % m_array.Length;
			int num2 = (m_tail + m_array.Length - 1) % m_array.Length;
			while (num != num2)
			{
				int num3 = (num + 1) % m_array.Length;
				m_array[num] = m_array[num3];
				num = num3;
			}
			m_array[num2] = default(T);
			m_tail = num2;
			m_size--;
			m_version++;
		}

		protected void SetCapacity(int capacity)
		{
			T[] array = new T[capacity];
			if (m_size > 0)
			{
				if (m_head < m_tail)
				{
					Array.Copy(m_array, m_head, array, 0, m_size);
				}
				else
				{
					Array.Copy(m_array, m_head, array, 0, m_array.Length - m_head);
					Array.Copy(m_array, 0, array, m_array.Length - m_head, m_tail);
				}
			}
			m_array = array;
			m_head = 0;
			m_tail = ((m_size != capacity) ? m_size : 0);
		}

		public void TrimExcess()
		{
			if (m_size < (int)((double)m_array.Length * 0.9))
			{
				SetCapacity(m_size);
			}
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			if (Count > 0)
			{
				stringBuilder.Append(this[Count - 1]);
				for (int num = Count - 2; num >= 0; num--)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(this[num]);
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}
	}
}
