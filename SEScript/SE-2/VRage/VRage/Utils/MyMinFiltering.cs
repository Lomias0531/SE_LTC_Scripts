using System.Collections.Generic;

namespace VRage.Utils
{
	/// <summary>
	/// Nonlinear minimum filtering.
	/// </summary>
	public class MyMinFiltering
	{
		private readonly List<double> m_samples;

		private readonly int m_sampleMaxCount;

		private int m_sampleCursor;

		private double? m_cachedFilteredValue;

		/// <summary>
		///
		/// </summary>
		/// <param name="sampleCount">Number of samples used in this mean filter.</param>
		public MyMinFiltering(int sampleCount)
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
			if (m_samples.Count == 0)
			{
				return 0.0;
			}
			double num = double.MaxValue;
			foreach (double sample in m_samples)
			{
				num = ((sample < num) ? sample : num);
			}
			return num;
		}

		public void Clear()
		{
			m_samples.Clear();
			m_cachedFilteredValue = null;
		}

		public int GetCurrentSampleCount()
		{
			return m_samples.Count;
		}

		public int GetMaxSampleCount()
		{
			return m_sampleMaxCount;
		}
	}
}
