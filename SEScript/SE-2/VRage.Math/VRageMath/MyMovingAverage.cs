using System.Collections.Generic;

namespace VRageMath
{
	public class MyMovingAverage
	{
		private readonly Queue<float> m_queue = new Queue<float>();

		private readonly int m_windowSize;

		private int m_enqueueCounter;

		private readonly int m_enqueueCountToReset;

		public float Avg
		{
			get
			{
				if (m_queue.Count <= 0)
				{
					return 0f;
				}
				return (float)Sum / (float)m_queue.Count;
			}
		}

		public double Sum
		{
			get;
			private set;
		}

		public MyMovingAverage(int windowSize, int enqueueCountToReset = 1000)
		{
			m_windowSize = windowSize;
			m_enqueueCountToReset = enqueueCountToReset;
		}

		private void UpdateSum()
		{
			Sum = 0.0;
			foreach (float item in m_queue)
			{
				Sum += item;
			}
		}

		public void Enqueue(float value)
		{
			m_queue.Enqueue(value);
			m_enqueueCounter++;
			if (m_enqueueCounter > m_enqueueCountToReset)
			{
				m_enqueueCounter = 0;
				UpdateSum();
			}
			else
			{
				Sum += value;
			}
			while (m_queue.Count > m_windowSize)
			{
				float num = m_queue.Dequeue();
				Sum -= num;
			}
		}

		public void Reset()
		{
			Sum = 0.0;
			m_queue.Clear();
		}
	}
}
