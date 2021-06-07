namespace VRage.Utils
{
	/// <summary>
	/// Item that is accessible only for defined amount of time ticks.
	/// </summary>
	/// <typeparam name="T">item type</typeparam>
	public struct MyTickTimedItem<T>
	{
		private T m_storage;

		private int m_ticksLeft;

		/// <summary>
		/// Get the stored item.
		/// </summary>
		/// <returns>storage on success, default value of T on failure.</returns>
		public T Get()
		{
			if (m_ticksLeft > 0)
			{
				m_ticksLeft--;
				return m_storage;
			}
			return default(T);
		}

		/// <summary>
		/// Get the stored item.
		/// </summary>
		/// <param name="outStoredItem">item stored internally</param>
		/// <returns>true on success, false on timeout</returns>
		public bool TryGet(out T outStoredItem)
		{
			if (m_ticksLeft > 0)
			{
				m_ticksLeft--;
				outStoredItem = m_storage;
				return true;
			}
			outStoredItem = default(T);
			return false;
		}

		/// <summary>
		/// Set the stored value.
		/// </summary>
		/// <param name="itemTickTimeout">Number of time ticks for which the item is accessible.</param>
		/// <param name="item">Item to be stored.</param>
		public void Set(int itemTickTimeout, T item)
		{
			m_storage = item;
			m_ticksLeft = itemTickTimeout;
		}
	}
}
