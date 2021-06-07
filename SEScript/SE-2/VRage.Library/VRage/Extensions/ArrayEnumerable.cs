using System.Collections;
using System.Collections.Generic;

namespace VRage.Extensions
{
	public struct ArrayEnumerable<T, TEnumerator> : IEnumerable<T>, IEnumerable where TEnumerator : struct, IEnumerator<T>
	{
		private TEnumerator m_enumerator;

		public ArrayEnumerable(TEnumerator enumerator)
		{
			m_enumerator = enumerator;
		}

		public TEnumerator GetEnumerator()
		{
			return m_enumerator;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
