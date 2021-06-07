using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public class MySinglyLinkedList<V> : IList<V>, ICollection<V>, IEnumerable<V>, IEnumerable
	{
		internal class Node
		{
			public Node Next;

			public V Data;

			public Node(Node next, V data)
			{
				Next = next;
				Data = data;
			}
		}

		public struct Enumerator : IEnumerator<V>, IEnumerator, IDisposable
		{
			internal Node m_previousNode;

			internal Node m_currentNode;

			internal MySinglyLinkedList<V> m_list;

			public V Current => m_currentNode.Data;

			public bool HasCurrent => m_currentNode != null;

			object IEnumerator.Current => m_currentNode.Data;

			public Enumerator(MySinglyLinkedList<V> parentList)
			{
				m_list = parentList;
				m_currentNode = null;
				m_previousNode = null;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				if (m_currentNode == null)
				{
					if (m_previousNode != null)
					{
						return false;
					}
					m_currentNode = m_list.m_rootNode;
					m_previousNode = null;
				}
				else
				{
					m_previousNode = m_currentNode;
					m_currentNode = m_currentNode.Next;
				}
				return m_currentNode != null;
			}

			public V RemoveCurrent()
			{
				if (m_currentNode == null)
				{
					throw new InvalidOperationException();
				}
				if (m_previousNode == null)
				{
					m_currentNode = m_currentNode.Next;
					return m_list.PopFirst();
				}
				m_previousNode.Next = m_currentNode.Next;
				if (m_list.m_lastNode == m_currentNode)
				{
					m_list.m_lastNode = m_previousNode;
				}
				Node currentNode = m_currentNode;
				m_currentNode = m_currentNode.Next;
				m_list.m_count--;
				return currentNode.Data;
			}

			public void InsertBeforeCurrent(V toInsert)
			{
				Node node = new Node(m_currentNode, toInsert);
				if (m_currentNode == null)
				{
					if (m_previousNode == null)
					{
						if (m_list.m_count != 0)
						{
							throw new InvalidOperationException("Inserting into a MySinglyLinkedList using an uninitialized enumerator!");
						}
						m_list.m_rootNode = node;
						m_list.m_lastNode = node;
					}
					else
					{
						m_previousNode.Next = node;
						m_list.m_lastNode = node;
					}
				}
				else if (m_previousNode == null)
				{
					m_list.m_rootNode = node;
				}
				else
				{
					m_previousNode.Next = node;
				}
				m_previousNode = node;
				m_list.m_count++;
			}

			public void Reset()
			{
				m_currentNode = null;
				m_previousNode = null;
			}
		}

		private Node m_rootNode;

		private Node m_lastNode;

		private int m_count;

		public V this[int index]
		{
			get
			{
				if (index < 0 || index >= m_count)
				{
					throw new IndexOutOfRangeException();
				}
				Enumerator enumerator = GetEnumerator();
				for (int i = -1; i < index; i++)
				{
					enumerator.MoveNext();
				}
				return enumerator.Current;
			}
			set
			{
				if (index < 0 || index >= m_count)
				{
					throw new IndexOutOfRangeException();
				}
				Enumerator enumerator = GetEnumerator();
				for (int i = -1; i < index; i++)
				{
					enumerator.MoveNext();
				}
				enumerator.m_currentNode.Data = value;
			}
		}

		public int Count => m_count;

		public bool IsReadOnly
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public MySinglyLinkedList()
		{
			m_rootNode = null;
			m_lastNode = null;
			m_count = 0;
		}

		public int IndexOf(V item)
		{
			int num = 0;
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Equals(item))
					{
						return num;
					}
					num++;
				}
			}
			return -1;
		}

		public void Insert(int index, V item)
		{
			if (index < 0 || index > m_count)
			{
				throw new IndexOutOfRangeException();
			}
			if (index == 0)
			{
				Prepend(item);
				return;
			}
			if (index == m_count)
			{
				Add(item);
				return;
			}
			Enumerator enumerator = GetEnumerator();
			for (int i = 0; i < index; i++)
			{
				enumerator.MoveNext();
			}
			Node next = new Node(enumerator.m_currentNode.Next, item);
			enumerator.m_currentNode.Next = next;
			m_count++;
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= m_count)
			{
				throw new IndexOutOfRangeException();
			}
			if (index == 0)
			{
				m_rootNode = m_rootNode.Next;
				m_count--;
				if (m_count == 0)
				{
					m_lastNode = null;
				}
				return;
			}
			Enumerator enumerator = GetEnumerator();
			for (int i = 0; i < index; i++)
			{
				enumerator.MoveNext();
			}
			enumerator.m_currentNode.Next = enumerator.m_currentNode.Next.Next;
			m_count--;
			if (m_count == index)
			{
				m_lastNode = enumerator.m_currentNode;
			}
		}

		/// <summary>
		/// Splits the list into two.
		/// This list's end will be the node pointed by newLastPosition and the newly created list will begin with the next node.
		/// </summary>
		/// <param name="newLastPosition">Enumerator that points to the new last position in the list.</param>
		/// <param name="newCount">New number of elements in this list. If set to -1, it is calculated automatically,
		/// but that would make the split an O(N) operation. Beware: If you set this parameter, be sure to always set the
		/// correct number, otherwise, you'd cause both lists (this one and the returned one) to return a wrong number of
		/// elements in the future.</param>
		/// <returns>The newly created list</returns>
		public MySinglyLinkedList<V> Split(Enumerator newLastPosition, int newCount = -1)
		{
			if (newCount == -1)
			{
				newCount = 1;
				for (Node node = m_rootNode; node != newLastPosition.m_currentNode; node = node.Next)
				{
					newCount++;
				}
			}
			MySinglyLinkedList<V> mySinglyLinkedList = new MySinglyLinkedList<V>();
			mySinglyLinkedList.m_rootNode = newLastPosition.m_currentNode.Next;
			mySinglyLinkedList.m_lastNode = ((mySinglyLinkedList.m_rootNode == null) ? null : m_lastNode);
			mySinglyLinkedList.m_count = m_count - newCount;
			m_lastNode = newLastPosition.m_currentNode;
			m_lastNode.Next = null;
			m_count = newCount;
			return mySinglyLinkedList;
		}

		public void Add(V item)
		{
			if (m_lastNode == null)
			{
				Prepend(item);
				return;
			}
			m_lastNode.Next = new Node(null, item);
			m_count++;
			m_lastNode = m_lastNode.Next;
		}

		public void Append(V item)
		{
			Add(item);
		}

		public void Prepend(V item)
		{
			m_rootNode = new Node(m_rootNode, item);
			m_count++;
			if (m_count == 1)
			{
				m_lastNode = m_rootNode;
			}
		}

		public void Merge(MySinglyLinkedList<V> otherList)
		{
			if (m_lastNode == null)
			{
				m_rootNode = otherList.m_rootNode;
				m_lastNode = otherList.m_lastNode;
			}
			else if (otherList.m_lastNode != null)
			{
				m_lastNode.Next = otherList.m_rootNode;
				m_lastNode = otherList.m_lastNode;
			}
			m_count += otherList.m_count;
			otherList.m_count = 0;
			otherList.m_lastNode = null;
			otherList.m_rootNode = null;
		}

		public V PopFirst()
		{
			if (m_count == 0)
			{
				throw new InvalidOperationException();
			}
			Node rootNode = m_rootNode;
			if (rootNode == m_lastNode)
			{
				m_lastNode = null;
			}
			m_rootNode = rootNode.Next;
			m_count--;
			return rootNode.Data;
		}

		public V First()
		{
			if (m_count == 0)
			{
				throw new InvalidOperationException();
			}
			return m_rootNode.Data;
		}

		public V Last()
		{
			if (m_count == 0)
			{
				throw new InvalidOperationException();
			}
			return m_lastNode.Data;
		}

		public void Clear()
		{
			m_rootNode = null;
			m_lastNode = null;
			m_count = 0;
		}

		public bool Contains(V item)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Equals(item))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void CopyTo(V[] array, int arrayIndex)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					V val = array[arrayIndex] = enumerator.Current;
					arrayIndex++;
				}
			}
		}

		public void Reverse()
		{
			if (m_count > 1)
			{
				Node next = null;
				Node node = m_rootNode;
				while (node != m_lastNode)
				{
					Node next2 = node.Next;
					node.Next = next;
					next = node;
					node = next2;
				}
				Node rootNode = m_rootNode;
				m_rootNode = m_lastNode;
				m_lastNode = rootNode;
			}
		}

		public bool VerifyConsistency()
		{
			bool flag = true;
			if (m_lastNode == null)
			{
				flag = (flag && m_rootNode == null && m_count == 0);
			}
			if (m_rootNode == null)
			{
				flag = (flag && m_lastNode == null && m_count == 0);
			}
			if (m_rootNode == m_lastNode)
			{
				flag = (flag && (m_rootNode == null || m_count == 1));
			}
			int num = 0;
			Node node = m_rootNode;
			while (node != null)
			{
				node = node.Next;
				num++;
				flag = (flag && num <= m_count);
			}
			return flag && num == m_count;
		}

		public bool Remove(V item)
		{
			Node node = m_rootNode;
			if (node == null)
			{
				return false;
			}
			if (m_rootNode.Data.Equals(item))
			{
				m_rootNode = m_rootNode.Next;
				m_count--;
				if (m_count == 0)
				{
					m_lastNode = null;
				}
				return true;
			}
			for (Node next = node.Next; next != null; next = next.Next)
			{
				if (next.Data.Equals(item))
				{
					node.Next = next.Next;
					m_count--;
					if (next == m_lastNode)
					{
						m_lastNode = node;
					}
					return true;
				}
				node = next;
			}
			return false;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<V> IEnumerable<V>.GetEnumerator()
		{
			return new Enumerator(this);
		}
	}
}
