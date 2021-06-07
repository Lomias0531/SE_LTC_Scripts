using System;
using System.Collections.Generic;

namespace VRage.Library.Collections
{
	public class CacheList<T> : List<T>, IDisposable
	{
		public CacheList<T> Empty => this;

		public CacheList()
		{
		}

		public CacheList(int capacity)
			: base(capacity)
		{
		}

		void IDisposable.Dispose()
		{
			Clear();
		}
	}
}
