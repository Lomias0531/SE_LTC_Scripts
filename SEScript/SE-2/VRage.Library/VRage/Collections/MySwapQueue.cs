using System;
using System.Threading;

namespace VRage.Collections
{
	public class MySwapQueue
	{
		public static MySwapQueue<T> Create<T>() where T : class, new()
		{
			return new MySwapQueue<T>(new T(), new T(), new T());
		}
	}
	/// <summary>
	/// Holds three objects in safe manner, use when Reader requires only last valid data.
	/// One object is used for reading, one for writing and third is used as buffer, so reader/writer don't have to wait on the other.
	/// </summary>
	public class MySwapQueue<T> where T : class
	{
		private T m_read;

		private T m_write;

		private T m_waitingData;

		private T m_unusedData;

		public T Read => m_read;

		public T Write => m_write;

		public MySwapQueue(Func<T> factoryMethod)
			: this(factoryMethod(), factoryMethod(), factoryMethod())
		{
		}

		public MySwapQueue(T first, T second, T third)
		{
			m_read = first;
			m_write = second;
			m_unusedData = third;
			m_waitingData = null;
		}

		/// <summary>
		/// Updates data for reading if there's something new
		/// Returns true when Read was updated, returns false when Read was not changed
		/// </summary>
		public bool RefreshRead()
		{
			if (Interlocked.CompareExchange(ref m_unusedData, m_read, null) == null)
			{
				m_read = Interlocked.Exchange(ref m_waitingData, null);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Commits Write and replaces write with new object ready for new writing
		/// </summary>
		public void CommitWrite()
		{
			m_write = Interlocked.Exchange(ref m_waitingData, m_write);
			if (m_write == null)
			{
				m_write = Interlocked.Exchange(ref m_unusedData, null);
			}
		}
	}
}
