using System;
using System.Collections.Generic;

namespace VRage.Generics
{
	/// <summary>
	/// Contains items of any type. Each item has weight (float value).
	/// Allows to get item based on weight.
	/// </summary>
	/// <typeparam name="T">The item type</typeparam>
	public class MyWeightDictionary<T>
	{
		private Dictionary<T, float> m_data;

		private float m_sum;

		public int Count => m_data.Count;

		/// <summary>
		/// Initializes a new instance of the MyWeightDictionary class.
		/// </summary>
		/// <param name="data">Dictionary with items and weights.</param>
		public MyWeightDictionary(Dictionary<T, float> data)
		{
			m_data = data;
			m_sum = 0f;
			foreach (KeyValuePair<T, float> datum in data)
			{
				m_sum += datum.Value;
			}
		}

		/// <summary>
		/// Gets sum of weights.
		/// </summary>
		/// <returns>The sum of all weights.</returns>
		public float GetSum()
		{
			return m_sum;
		}

		/// <summary>
		/// Gets item based on weight.
		/// </summary>
		/// <param name="weightNormalized">Weight, value from 0 to 1.</param>
		/// <returns>The item.</returns>
		public T GetItemByWeightNormalized(float weightNormalized)
		{
			return GetItemByWeight(weightNormalized * m_sum);
		}

		/// <summary>
		/// Gets item based on weight.
		/// </summary>
		/// <param name="weight">Weight, value from 0 to sum.</param>
		/// <returns></returns>
		public T GetItemByWeight(float weight)
		{
			float num = 0f;
			T result = default(T);
			foreach (KeyValuePair<T, float> datum in m_data)
			{
				result = datum.Key;
				num += datum.Value;
				if (num > weight)
				{
					return result;
				}
			}
			return result;
		}

		/// <summary>
		/// Gets random item based on weight.
		/// </summary>
		/// <returns>The item.</returns>
		public T GetRandomItem(Random rnd)
		{
			float weight = (float)rnd.NextDouble() * m_sum;
			return GetItemByWeight(weight);
		}
	}
}
