using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	public static class ConcurrentEnumerable
	{
		public static ConcurrentEnumerable<TLock, TItem, TEnumerable> Create<TLock, TItem, TEnumerable>(TLock @lock, IEnumerable<TItem> enumerator) where TLock : struct, IDisposable where TEnumerable : IEnumerable<TItem>
		{
			return new ConcurrentEnumerable<TLock, TItem, TEnumerable>(@lock, enumerator);
		}
	}
	public struct ConcurrentEnumerable<TLock, TItem, TEnumerable> : IEnumerable<TItem>, IEnumerable where TLock : struct, IDisposable where TEnumerable : IEnumerable<TItem>
	{
		private IEnumerable<TItem> m_enumerable;

		private TLock m_lock;

		public ConcurrentEnumerable(TLock lk, IEnumerable<TItem> enumerable)
		{
			m_enumerable = enumerable;
			m_lock = lk;
		}

		public ConcurrentEnumerator<TLock, TItem, IEnumerator<TItem>> GetEnumerator()
		{
			return ConcurrentEnumerator.Create<TLock, TItem, IEnumerator<TItem>>(m_lock, m_enumerable.GetEnumerator());
		}

		IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
