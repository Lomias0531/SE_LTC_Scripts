namespace VRageMath
{
	/// <summary>
	/// A class for simpler traversal of ranges of longeger vectors
	/// </summary>
	public struct Vector3L_RangeIterator
	{
		private Vector3L m_start;

		private Vector3L m_end;

		/// <summary>
		/// Do not modify, public only for optimization!
		/// </summary>
		public Vector3L Current;

		/// <summary>
		/// Note: both start and end are inclusive
		/// </summary>
		public Vector3L_RangeIterator(ref Vector3L start, ref Vector3L end)
		{
			m_start = start;
			m_end = end;
			Current = m_start;
		}

		public bool IsValid()
		{
			if (Current.X >= m_start.X && Current.Y >= m_start.Y && Current.Z >= m_start.Z && Current.X <= m_end.X && Current.Y <= m_end.Y)
			{
				return Current.Z <= m_end.Z;
			}
			return false;
		}

		public void GetNext(out Vector3L next)
		{
			MoveNext();
			next = Current;
		}

		public void MoveNext()
		{
			Current.X++;
			if (Current.X > m_end.X)
			{
				Current.X = m_start.X;
				Current.Y++;
				if (Current.Y > m_end.Y)
				{
					Current.Y = m_start.Y;
					Current.Z++;
				}
			}
		}
	}
}
