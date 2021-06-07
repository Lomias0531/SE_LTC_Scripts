using System.Collections.Generic;

namespace VRage.Utils
{
	/// <summary>
	/// Mean (average) filtering.
	/// </summary>
	public class MyAverageFiltering
	{
		private readonly List<double> m_samples;

		private readonly int m_sampleMaxCount;

		private int m_sampleCursor;

		private double? m_cachedFilteredValue;

		/// <summary>
		///
		/// </summary>
		/// <param name="sampleCount">Number of samples used in this mean filter.</param>
		public MyAverageFiltering(int sampleCount)
		{
			m_sampleMaxCount = sampleCount;
			m_samples = new List<double>(sampleCount);
			m_cachedFilteredValue = null;
		}

		/// <summary>
		/// Add raw value to be filtered.
		/// </summary>
		public void Add(double value)
		{
			m_cachedFilteredValue = null;
			if (m_samples.Count < m_sampleMaxCount)
			{
				m_samples.Add(value);
				return;
			}
			m_samples[m_sampleCursor++] = value;
			if (m_sampleCursor >= m_sampleMaxCount)
			{
				m_sampleCursor = 0;
			}
		}

		/// <summary>
		/// Get filtered value.
		/// </summary>
		public double Get()
		{
			if (m_cachedFilteredValue.HasValue)
			{
				return m_cachedFilteredValue.Value;
			}
			double num = 0.0;
			foreach (double sample in m_samples)
			{
				num += sample;
			}
			if (m_samples.Count > 0)
			{
				double num2 = num / (double)m_samples.Count;
				m_cachedFilteredValue = num2;
				return num2;
			}
			return 0.0;
		}

		public void Clear()
		{
			m_samples.Clear();
			m_cachedFilteredValue = null;
		}
	}
}
