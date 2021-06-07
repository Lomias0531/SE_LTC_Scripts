using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library
{
	/// <summary>
	/// An implementation of a min-Priority Queue using a heap.  Has O(1) .Contains()!
	/// See https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp/wiki/Getting-Started for more information
	/// </summary>
	/// <typeparam name="T">The values in the queue.  Must extend the FastPriorityQueueNode class</typeparam>
	///
	/// <inheritdoc />
	public sealed class FastPriorityQueue<T> : IEnumerable where T : FastPriorityQueue<T>.Node
	{
		public class Node
		{
			/// <summary>
			/// The Priority to insert this node at.  Must be set BEFORE adding a node to the queue (ideally just once, in the node's constructor).
			/// Should not be manually edited once the node has been enqueued - use queue.UpdatePriority() instead
			/// </summary>
			internal long Priority;

			/// <summary>
			/// Represents the current position in the queue
			/// </summary>
			internal int QueueIndex;
		}

		private int m_numNodes;

		private T[] m_nodes;

		/// <summary>
		/// Returns the number of nodes in the queue.
		/// O(1)
		/// </summary>
		public int Count => m_numNodes;

		/// <summary>
		/// Returns the maximum number of items that can be enqueued at once in this queue.  Once you hit this number (ie. once Count == MaxSize),
		/// attempting to enqueue another item will cause undefined behavior.  O(1)
		/// </summary>
		public int MaxSize => m_nodes.Length - 1;

		/// <summary>
		/// Returns the head of the queue, without removing it (use Dequeue() for that).
		/// If the queue is empty, behavior is undefined.
		/// O(1)
		/// </summary>
		public T First => m_nodes[1];

		/// <summary>
		/// Instantiate a new Priority Queue
		/// </summary>
		/// <param name="maxNodes">The max nodes ever allowed to be enqueued (going over this will cause undefined behavior)</param>
		public FastPriorityQueue(int maxNodes)
		{
			m_numNodes = 0;
			m_nodes = new T[maxNodes + 1];
		}

		/// <summary>
		/// Removes every node from the queue.
		/// O(n) (So, don't do this often!)
		/// </summary>
		public void Clear()
		{
			Array.Clear(m_nodes, 1, m_numNodes);
			m_numNodes = 0;
		}

		/// <summary>
		/// Returns (in O(1)!) whether the given node is in the queue.  O(1)
		/// </summary>
		public bool Contains(T node)
		{
			return m_nodes[node.QueueIndex] == node;
		}

		/// <summary>
		/// Enqueue a node to the priority queue.  Lower values are placed in front. Ties are broken arbitrarily.
		/// If the queue is full, the result is undefined.
		/// If the node is already enqueued, the result is undefined.
		/// O(log n)
		/// </summary>
		public void Enqueue(T node, long priority)
		{
			if (m_numNodes >= m_nodes.Length - 1)
			{
				Resize((m_numNodes > 0) ? (m_numNodes * 2) : 16);
			}
			node.Priority = priority;
			m_numNodes++;
			m_nodes[m_numNodes] = node;
			node.QueueIndex = m_numNodes;
			CascadeUp(node);
		}

		private void CascadeUp(T node)
		{
			if (node.QueueIndex <= 1)
			{
				return;
			}
			int num = node.QueueIndex >> 1;
			T val = m_nodes[num];
			if (HasHigherOrEqualPriority(val, node))
			{
				return;
			}
			m_nodes[node.QueueIndex] = val;
			val.QueueIndex = node.QueueIndex;
			node.QueueIndex = num;
			while (num > 1)
			{
				num >>= 1;
				T val2 = m_nodes[num];
				if (HasHigherOrEqualPriority(val2, node))
				{
					break;
				}
				m_nodes[node.QueueIndex] = val2;
				val2.QueueIndex = node.QueueIndex;
				node.QueueIndex = num;
			}
			m_nodes[node.QueueIndex] = node;
		}

		private void CascadeDown(T node)
		{
			int queueIndex = node.QueueIndex;
			int num = 2 * queueIndex;
			if (num > m_numNodes)
			{
				return;
			}
			int num2 = num + 1;
			T val = m_nodes[num];
			if (val.Priority < node.Priority)
			{
				if (num2 > m_numNodes)
				{
					node.QueueIndex = num;
					val.QueueIndex = queueIndex;
					m_nodes[queueIndex] = val;
					m_nodes[num] = node;
					return;
				}
				T val2 = m_nodes[num2];
				if (val.Priority < val2.Priority)
				{
					val.QueueIndex = queueIndex;
					m_nodes[queueIndex] = val;
					queueIndex = num;
				}
				else
				{
					val2.QueueIndex = queueIndex;
					m_nodes[queueIndex] = val2;
					queueIndex = num2;
				}
			}
			else
			{
				if (num2 > m_numNodes)
				{
					return;
				}
				T val3 = m_nodes[num2];
				if (val3.Priority >= node.Priority)
				{
					return;
				}
				val3.QueueIndex = queueIndex;
				m_nodes[queueIndex] = val3;
				queueIndex = num2;
			}
			while (true)
			{
				num = 2 * queueIndex;
				if (num > m_numNodes)
				{
					node.QueueIndex = queueIndex;
					m_nodes[queueIndex] = node;
					return;
				}
				num2 = num + 1;
				val = m_nodes[num];
				if (val.Priority < node.Priority)
				{
					if (num2 > m_numNodes)
					{
						node.QueueIndex = num;
						val.QueueIndex = queueIndex;
						m_nodes[queueIndex] = val;
						m_nodes[num] = node;
						return;
					}
					T val4 = m_nodes[num2];
					if (val.Priority < val4.Priority)
					{
						val.QueueIndex = queueIndex;
						m_nodes[queueIndex] = val;
						queueIndex = num;
					}
					else
					{
						val4.QueueIndex = queueIndex;
						m_nodes[queueIndex] = val4;
						queueIndex = num2;
					}
				}
				else
				{
					if (num2 > m_numNodes)
					{
						node.QueueIndex = queueIndex;
						m_nodes[queueIndex] = node;
						return;
					}
					T val5 = m_nodes[num2];
					if (val5.Priority >= node.Priority)
					{
						break;
					}
					val5.QueueIndex = queueIndex;
					m_nodes[queueIndex] = val5;
					queueIndex = num2;
				}
			}
			node.QueueIndex = queueIndex;
			m_nodes[queueIndex] = node;
		}

		/// <summary>
		/// Returns true if 'higher' has higher priority than 'lower', false otherwise.
		/// Note that calling HasHigherOrEqualPriority(node, node) (ie. both arguments the same node) will return true
		/// </summary>
		private bool HasHigherOrEqualPriority(T higher, T lower)
		{
			return higher.Priority <= lower.Priority;
		}

		/// <summary>
		/// Removes the head of the queue and returns it.
		/// If queue is empty, result is undefined
		/// O(log n)
		/// </summary>
		public T Dequeue()
		{
			T result = m_nodes[1];
			if (m_numNodes == 1)
			{
				m_nodes[1] = null;
				m_numNodes = 0;
				return result;
			}
			T val = m_nodes[m_numNodes];
			m_nodes[1] = val;
			val.QueueIndex = 1;
			m_nodes[m_numNodes] = null;
			m_numNodes--;
			CascadeDown(val);
			return result;
		}

		/// <summary>
		/// Resize the queue so it can accept more nodes.  All currently enqueued nodes are remain.
		/// Attempting to decrease the queue size to a size too small to hold the existing nodes results in undefined behavior
		/// O(n)
		/// </summary>
		public void Resize(int maxNodes)
		{
			T[] array = new T[maxNodes + 1];
			int num = Math.Min(maxNodes, m_numNodes);
			Array.Copy(m_nodes, array, num + 1);
			m_nodes = array;
		}

		/// <summary>
		/// This method must be called on a node every time its priority changes while it is in the queue.  
		/// <b>Forgetting to call this method will result in a corrupted queue!</b>
		/// Calling this method on a node not in the queue results in undefined behavior
		/// O(log n)
		/// </summary>
		public void UpdatePriority(T node, long priority)
		{
			node.Priority = priority;
			OnNodeUpdated(node);
		}

		private void OnNodeUpdated(T node)
		{
			int num = node.QueueIndex >> 1;
			if (num > 0 && node.Priority < m_nodes[num].Priority)
			{
				CascadeUp(node);
			}
			else
			{
				CascadeDown(node);
			}
		}

		/// <summary>
		/// Removes a node from the queue.  The node does not need to be the head of the queue.  
		/// If the node is not in the queue, the result is undefined.  If unsure, check Contains() first
		/// O(log n)
		/// </summary>
		public void Remove(T node)
		{
			if (node.QueueIndex == m_numNodes)
			{
				m_nodes[m_numNodes] = null;
				m_numNodes--;
				return;
			}
			T val = m_nodes[m_numNodes];
			m_nodes[node.QueueIndex] = val;
			val.QueueIndex = node.QueueIndex;
			m_nodes[m_numNodes] = null;
			m_numNodes--;
			OnNodeUpdated(val);
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 1; i <= m_numNodes; i++)
			{
				yield return m_nodes[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// <b>Should not be called in production code.</b>
		/// Checks to make sure the queue is still in a valid state.  Used for testing/debugging the queue.
		/// </summary>
		public bool IsValidQueue()
		{
			for (int i = 1; i < m_nodes.Length; i++)
			{
				if (m_nodes[i] != null)
				{
					int num = 2 * i;
					if (num < m_nodes.Length && m_nodes[num] != null && m_nodes[num].Priority < m_nodes[i].Priority)
					{
						return false;
					}
					int num2 = num + 1;
					if (num2 < m_nodes.Length && m_nodes[num2] != null && m_nodes[num2].Priority < m_nodes[i].Priority)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
