using VRage.Collections;

namespace VRage.Library.Utils
{
	public class MyInterpolationQueue<T>
	{
		private struct Item
		{
			public T Userdata;

			public MyTimeSpan Timestamp;

			public Item(T userdata, MyTimeSpan timespan)
			{
				Userdata = userdata;
				Timestamp = timespan;
			}
		}

		private MyQueue<Item> m_queue;

		private InterpolationHandler<T> m_interpolator;

		private MyTimeSpan m_lastTimeStamp = MyTimeSpan.Zero;

		public MyTimeSpan LastSample => m_lastTimeStamp;

		public int Count => m_queue.Count;

		public MyInterpolationQueue(int defaultCapacity, InterpolationHandler<T> interpolator)
		{
			m_queue = new MyQueue<Item>(defaultCapacity);
			m_interpolator = interpolator;
		}

		/// <summary>
		/// Discards old samples, keeps at least 2 samples to be able to interpolate or extrapolate.
		/// </summary>
		public void DiscardOld(MyTimeSpan currentTimestamp)
		{
			int num = -1;
			for (int i = 0; i < m_queue.Count && m_queue[i].Timestamp < currentTimestamp; i++)
			{
				num++;
			}
			for (int j = 0; j < num; j++)
			{
				if (m_queue.Count <= 2)
				{
					break;
				}
				m_queue.Dequeue();
			}
		}

		public void Clear()
		{
			m_queue.Clear();
			m_lastTimeStamp = MyTimeSpan.Zero;
		}

		/// <summary>
		/// Adds sample with timestamp, it must be larger than last timestamp!
		/// </summary>
		public void AddSample(ref T item, MyTimeSpan sampleTimestamp)
		{
			if (!(sampleTimestamp < m_lastTimeStamp))
			{
				if (sampleTimestamp == m_lastTimeStamp && m_queue.Count > 0)
				{
					m_queue[m_queue.Count - 1] = new Item(item, sampleTimestamp);
					return;
				}
				m_queue.Enqueue(new Item(item, sampleTimestamp));
				m_lastTimeStamp = sampleTimestamp;
			}
		}

		/// <summary>
		/// Discards old frame (keeps one older) and interpolates between two samples using interpolator.
		/// Returns interpolator
		/// There must be at least one sample!
		/// </summary>
		public float Interpolate(MyTimeSpan currentTimestamp, out T result)
		{
			DiscardOld(currentTimestamp);
			if (m_queue.Count > 1)
			{
				Item item = m_queue[0];
				Item item2 = m_queue[1];
				float num = (float)((currentTimestamp - item.Timestamp).Seconds / (item2.Timestamp - item.Timestamp).Seconds);
				m_interpolator(item.Userdata, item2.Userdata, num, out result);
				return num;
			}
			result = m_queue[0].Userdata;
			return 0f;
		}
	}
}
