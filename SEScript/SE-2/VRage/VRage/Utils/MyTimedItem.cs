namespace VRage.Utils
{
	/// <summary>
	/// Item that is accessible only for defined time span.
	/// </summary>
	/// <typeparam name="T">item type</typeparam>
	public struct MyTimedItem<T>
	{
		private T m_storage;

		private int m_setTime;

		private int m_timeout;

		/// <summary>
		/// Get the stored item.
		/// </summary>
		/// <param name="currentTime">Pass current time.</param>
		/// <param name="autoRefreshTimeout">Should the storage timeout be refreshed?</param>
		/// <returns>storage on success, default value of T on failure.</returns>
		public T Get(int currentTime, bool autoRefreshTimeout)
		{
			if (currentTime < m_setTime + m_timeout)
			{
				if (autoRefreshTimeout)
				{
					m_setTime = currentTime + m_timeout;
				}
				return m_storage;
			}
			return default(T);
		}

		/// <summary>
		/// Get the stored item.
		/// </summary>
		/// <param name="currentTime">Pass current time.</param>
		/// <param name="autoRefreshTimeout">Should the storage timeout be refreshed?</param>
		/// <param name="outStoredItem">item stored internally</param>
		/// <returns>true on success, false on timeout</returns>
		public bool TryGet(int currentTime, bool autoRefreshTimeout, out T outStoredItem)
		{
			if (currentTime < m_setTime + m_timeout)
			{
				if (autoRefreshTimeout)
				{
					m_setTime = currentTime + m_timeout;
				}
				outStoredItem = m_storage;
				return true;
			}
			outStoredItem = default(T);
			return false;
		}

		/// <summary>
		/// Set the stored value.
		/// </summary>
		/// <param name="currentTime">Pass current time.</param>
		/// <param name="itemTimeout">Period of time for which the item is accessible.</param>
		/// <param name="item">Item to be stored.</param>
		public void Set(int currentTime, int itemTimeout, T item)
		{
			m_setTime = currentTime;
			m_timeout = itemTimeout;
			m_storage = item;
		}
	}
}
