using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Extensions
{
	public struct ArrayOfTypeEnumerator<T, TInner, TOfType> : IEnumerator<TOfType>, IEnumerator, IDisposable where TInner : struct, IEnumerator<T> where TOfType : T
	{
		private TInner m_inner;

		public TOfType Current => (TOfType)(object)m_inner.Current;

		object IEnumerator.Current => m_inner.Current;

		public ArrayOfTypeEnumerator(TInner enumerator)
		{
			m_inner = enumerator;
		}

		/// <summary>
		/// So we can put this into foreach
		/// </summary>
		public ArrayOfTypeEnumerator<T, TInner, TOfType> GetEnumerator()
		{
			return this;
		}

		public void Dispose()
		{
			m_inner.Dispose();
		}

		public bool MoveNext()
		{
			while (m_inner.MoveNext())
			{
				if (m_inner.Current is TOfType)
				{
					return true;
				}
			}
			return false;
		}

		public void Reset()
		{
			m_inner.Reset();
		}
	}
}
