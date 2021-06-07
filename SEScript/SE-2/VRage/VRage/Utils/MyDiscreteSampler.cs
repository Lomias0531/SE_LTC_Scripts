using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRage.Library.Utils;

namespace VRage.Utils
{
	/// <summary>
	/// A templated class for sampling from a set of objects with given probabilities. Uses MyDiscreteSampler.
	/// </summary>
	public class MyDiscreteSampler<T> : IEnumerable<T>, IEnumerable
	{
		private T[] m_values;

		private MyDiscreteSampler m_sampler;

		public bool Initialized => m_sampler.Initialized;

		public int Count => m_values.Length;

		public MyDiscreteSampler(T[] values, IEnumerable<float> densities)
		{
			m_values = new T[values.Length];
			Array.Copy(values, m_values, values.Length);
			m_sampler = new MyDiscreteSampler();
			m_sampler.Prepare(densities);
		}

		public MyDiscreteSampler(List<T> values, IEnumerable<float> densities)
		{
			m_values = new T[values.Count];
			for (int i = 0; i < values.Count; i++)
			{
				m_values[i] = values[i];
			}
			m_sampler = new MyDiscreteSampler();
			m_sampler.Prepare(densities);
		}

		public MyDiscreteSampler(IEnumerable<T> values, IEnumerable<float> densities)
		{
			int num = values.Count();
			m_values = new T[num];
			int num2 = 0;
			foreach (T value in values)
			{
				m_values[num2] = value;
				num2++;
			}
			m_sampler = new MyDiscreteSampler();
			m_sampler.Prepare(densities);
		}

		public MyDiscreteSampler(Dictionary<T, float> densities)
			: this((IEnumerable<T>)densities.Keys, (IEnumerable<float>)densities.Values)
		{
		}

		public T Sample(MyRandom rng)
		{
			return m_values[m_sampler.Sample(rng)];
		}

		public T Sample(float sample)
		{
			return m_values[m_sampler.Sample(sample)];
		}

		public T Sample()
		{
			return m_values[m_sampler.Sample()];
		}

		public IEnumerator<T> GetEnumerator()
		{
			return m_values.AsEnumerable().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	/// <summary>
	/// Provides a simple and efficient way of sampling a discrete probability distribution as described in http://www.jstatsoft.org/v11/i03/paper
	/// Instances can be reused by calling the Prepare method every time you want to change the distribution.
	/// Sampling a value is O(1), while the storage requirements are O(N), where N is number of possible values
	/// </summary>
	public class MyDiscreteSampler
	{
		public struct SamplingBin
		{
			public float Split;

			public int BinIndex;

			public int Donator;

			public override string ToString()
			{
				return "[" + BinIndex + "] <- (" + Donator + ") : " + Split;
			}
		}

		private class BinComparer : IComparer<SamplingBin>
		{
			public static BinComparer Static = new BinComparer();

			public int Compare(SamplingBin x, SamplingBin y)
			{
				float num = x.Split - y.Split;
				if (num < 0f)
				{
					return -1;
				}
				if (num > 0f)
				{
					return 1;
				}
				return 0;
			}
		}

		private SamplingBin[] m_bins;

		private int m_binCount;

		private bool m_initialized;

		public bool Initialized => m_initialized;

		public MyDiscreteSampler()
		{
			m_binCount = 0;
			m_bins = null;
			m_initialized = false;
		}

		public MyDiscreteSampler(int prealloc)
			: this()
		{
			m_bins = new SamplingBin[prealloc];
		}

		/// The list supplied to the method does not have to add up to 1.0f, that's why it's called "densities" instead of "probabilities"
		public void Prepare(IEnumerable<float> densities)
		{
			float num = 0f;
			int num2 = 0;
			foreach (float density in densities)
			{
				num += density;
				num2++;
			}
			if (num2 != 0)
			{
				float normalizationFactor = (float)num2 / num;
				AllocateBins(num2);
				InitializeBins(densities, normalizationFactor);
				ProcessDonators();
				m_initialized = true;
			}
		}

		private void InitializeBins(IEnumerable<float> densities, float normalizationFactor)
		{
			int num = 0;
			foreach (float density in densities)
			{
				m_bins[num].BinIndex = num;
				m_bins[num].Split = density * normalizationFactor;
				m_bins[num].Donator = 0;
				num++;
			}
			Array.Sort(m_bins, 0, m_binCount, BinComparer.Static);
		}

		private void AllocateBins(int numDensities)
		{
			if (m_bins == null || m_binCount < numDensities)
			{
				m_bins = new SamplingBin[numDensities];
			}
			m_binCount = numDensities;
		}

		private void ProcessDonators()
		{
			int num = 0;
			int num2 = 1;
			int num3 = m_binCount - 1;
			while (num2 <= num3)
			{
				m_bins[num].Donator = m_bins[num3].BinIndex;
				m_bins[num3].Split -= 1f - m_bins[num].Split;
				if (m_bins[num3].Split < 1f)
				{
					num = num3;
					num3--;
				}
				else
				{
					num = num2;
					num2++;
				}
			}
		}

		public int Sample(MyRandom rng)
		{
			int num = rng.Next(m_binCount);
			SamplingBin samplingBin = m_bins[num];
			if (rng.NextFloat() <= samplingBin.Split)
			{
				return samplingBin.BinIndex;
			}
			return samplingBin.Donator;
		}

		/// Beware that Cestmir thinks this can be less precise if you have a billiard numbers.
		///
		/// He is probably right. So only use this version if you don't care.
		public int Sample(float rate)
		{
			float num = (float)m_binCount * rate;
			int num2 = (int)num;
			if (num2 == m_binCount)
			{
				num2--;
			}
			SamplingBin samplingBin = m_bins[num2];
			if (num - (float)num2 < samplingBin.Split)
			{
				return samplingBin.BinIndex;
			}
			return samplingBin.Donator;
		}

		public int Sample()
		{
			int randomInt = MyUtils.GetRandomInt(m_binCount);
			SamplingBin samplingBin = m_bins[randomInt];
			if (MyUtils.GetRandomFloat() <= samplingBin.Split)
			{
				return samplingBin.BinIndex;
			}
			return samplingBin.Donator;
		}

		/// <summary>
		/// Get a copy of the internal bins.
		/// </summary>
		/// <returns></returns>
		public SamplingBin[] ReadBins()
		{
			SamplingBin[] array = new SamplingBin[m_binCount];
			Array.Copy(m_bins, array, array.Length);
			return array;
		}
	}
}
