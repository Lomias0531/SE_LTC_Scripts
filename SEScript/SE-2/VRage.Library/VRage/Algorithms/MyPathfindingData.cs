using System.Collections.Generic;
using System.Threading;
using VRage.Collections;

namespace VRage.Algorithms
{
	public class MyPathfindingData : HeapItem<float>
	{
		private object m_lockObject = new object();

		private Dictionary<Thread, long> threadedTimestamp = new Dictionary<Thread, long>();

		internal MyPathfindingData Predecessor;

		internal float PathLength;

		public object Parent
		{
			get;
			private set;
		}

		internal long Timestamp
		{
			get
			{
				long value = 0L;
				lock (m_lockObject)
				{
					if (!threadedTimestamp.TryGetValue(Thread.CurrentThread, out value))
					{
						return 0L;
					}
					return value;
				}
			}
			set
			{
				lock (m_lockObject)
				{
					threadedTimestamp[Thread.CurrentThread] = value;
				}
			}
		}

		public MyPathfindingData(object parent)
		{
			Parent = parent;
		}

		public long GetTimestamp()
		{
			return Timestamp;
		}
	}
}
