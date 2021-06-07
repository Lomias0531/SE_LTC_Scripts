using System;
using VRageMath;

namespace VRage.Utils
{
	public class NormalAggregator
	{
		private Vector3[] m_data;

		private int m_currentDataIndex;

		private Vector3 m_cachedSum = Vector3.Zero;

		private Vector3 m_normalCache;

		private bool? m_isValidCache;

		public NormalAggregator(int averageWindowSize)
		{
			m_data = new Vector3[averageWindowSize];
		}

		public void PushNext(ref Vector3 normal)
		{
			m_isValidCache = null;
			m_cachedSum -= m_data[m_currentDataIndex];
			m_data[m_currentDataIndex] = normal;
			m_cachedSum += normal;
			m_currentDataIndex++;
			if (m_currentDataIndex >= m_data.Length)
			{
				m_currentDataIndex = 0;
			}
		}

		public bool GetAvgNormal(out Vector3 normal)
		{
			if (m_isValidCache.HasValue)
			{
				normal = m_normalCache;
				return m_isValidCache.Value;
			}
			m_isValidCache = GetAvgNormalInternal(out normal);
			m_normalCache = normal;
			return m_isValidCache.Value;
		}

		public void Clear()
		{
			m_isValidCache = false;
			m_cachedSum = Vector3.Zero;
			Array.Clear(m_data, 0, m_data.Length);
		}

		private bool GetAvgNormalInternal(out Vector3 normal)
		{
			float num = m_cachedSum.Length();
			if (MyUtils.IsZero(num))
			{
				normal = Vector3.Zero;
				return false;
			}
			normal = m_cachedSum / num;
			return true;
		}
	}
}
