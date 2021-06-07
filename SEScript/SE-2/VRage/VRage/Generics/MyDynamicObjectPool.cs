using System;
using System.Collections.Generic;

namespace VRage.Generics
{
	/// <summary>
	/// Dynamic object pool. It allocates a new instance when necessary.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MyDynamicObjectPool<T> where T : class, new()
	{
		private readonly Stack<T> m_poolStack;

		public int Count => m_poolStack.Count;

		public MyDynamicObjectPool(int capacity)
		{
			m_poolStack = new Stack<T>(capacity);
			Preallocate(capacity);
		}

		private void Preallocate(int count)
		{
			for (int i = 0; i < count; i++)
			{
				T item = new T();
				m_poolStack.Push(item);
			}
		}

		public T Allocate()
		{
			if (m_poolStack.Count == 0)
			{
				Preallocate(1);
			}
			return m_poolStack.Pop();
		}

		public void Deallocate(T item)
		{
			m_poolStack.Push(item);
		}

		public void TrimToSize(int size)
		{
			while (m_poolStack.Count > size)
			{
				m_poolStack.Pop();
			}
			m_poolStack.TrimExcess();
		}

		/// <summary>
		/// Suppress finalization of all items buffered in the pool.
		///
		/// This should only be called if the elements of the pool have some form of leak detecting finalizer.
		/// </summary>
		public void SuppressFinalize()
		{
			foreach (T item in m_poolStack)
			{
				GC.SuppressFinalize(item);
			}
		}
	}
}
