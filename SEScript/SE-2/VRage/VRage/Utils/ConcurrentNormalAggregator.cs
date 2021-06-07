using System.Collections.Concurrent;
using System.Threading;
using VRageMath;

namespace VRage.Utils
{
	public class ConcurrentNormalAggregator
	{
		private int m_averageWindowSize;

		private NormalAggregator m_normalAggregator;

		private int m_newNormalsCount;

		private FastResourceLock m_lock = new FastResourceLock();

		private ConcurrentQueue<Vector3> m_newNormals = new ConcurrentQueue<Vector3>();

		public ConcurrentNormalAggregator(int averageWindowSize)
		{
			m_averageWindowSize = averageWindowSize;
			m_normalAggregator = new NormalAggregator(averageWindowSize);
		}

		/// <summary>
		/// Might be safely called from multiple threads at the same time.
		/// </summary>
		public void PushNext(ref Vector3 normal)
		{
			m_newNormals.Enqueue(normal);
			if (Interlocked.Increment(ref m_newNormalsCount) > m_averageWindowSize)
			{
				Interlocked.Decrement(ref m_newNormalsCount);
				using (m_lock.AcquireSharedUsing())
				{
					m_newNormals.TryDequeue(out Vector3 _);
				}
			}
		}

		/// <summary>
		/// Consumption is allowed only from one thread at the time!
		/// It's not safe to call <see cref="M:VRage.Utils.ConcurrentNormalAggregator.Clear" /> method in parallel!
		/// </summary>
		public bool GetAvgNormal(out Vector3 normal)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				while (m_newNormals.TryDequeue(out normal))
				{
					Interlocked.Decrement(ref m_newNormalsCount);
					m_normalAggregator.PushNext(ref normal);
				}
			}
			return m_normalAggregator.GetAvgNormal(out normal);
		}

		public bool GetAvgNormalCached(out Vector3 normal)
		{
			return m_normalAggregator.GetAvgNormal(out normal);
		}

		/// <summary>
		/// Allowed to be called only from one thread at the time!
		/// It's not safe to call <see cref="M:VRage.Utils.ConcurrentNormalAggregator.GetAvgNormal(VRageMath.Vector3@)" /> method in parallel!
		/// </summary>
		public void Clear()
		{
			using (m_lock.AcquireSharedUsing())
			{
				m_normalAggregator.Clear();
			}
		}
	}
}
