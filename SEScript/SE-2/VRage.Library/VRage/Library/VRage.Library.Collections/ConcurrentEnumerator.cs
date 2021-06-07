using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	public static class ConcurrentEnumerator
	{
		public static ConcurrentEnumerator<TLock, TItem, TEnumerator> Create<TLock, TItem, TEnumerator>(TLock @lock, TEnumerator enumerator) where TLock : struct, IDisposable where TEnumerator : IEnumerator<TItem>
		{
			return new ConcurrentEnumerator<TLock, TItem, TEnumerator>(@lock, enumerator);
		}
	}
	public struct ConcurrentEnumerator<TLock, TItem, TEnumerator> : IEnumerator<TItem>, IEnumerator, IDisposable where TLock : struct, IDisposable where TEnumerator : IEnumerator<TItem>
	{
		private TEnumerator m_enumerator;

		private TLock m_lock;

		public TItem Current => m_enumerator.Current;

		object IEnumerator.Current => Current;

		public ConcurrentEnumerator(TLock @lock, TEnumerator enumerator)
		{
			m_enumerator = enumerator;
			m_lock = @lock;
		}

		public void Dispose()
		{
			m_enumerator.Dispose();
			m_lock.Dispose();
		}

		public bool MoveNext()
		{
			return m_enumerator.MoveNext();
		}

		public void Reset()
		{
			m_enumerator.Reset();
		}
	}
}
