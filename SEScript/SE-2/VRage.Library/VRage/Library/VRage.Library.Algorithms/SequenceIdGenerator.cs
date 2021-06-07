using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Threading;

namespace VRage.Library.Algorithms
{
	/// <summary>
	/// Generates IDs sequentially and reuses old IDs which are returned to pool by calling Return method.
	/// Protection count and time can be set to protect returned IDs. 
	/// Protection is useful especially in multiplayer where clients can still have objects with those IDs.
	/// </summary>
	public class SequenceIdGenerator
	{
		private struct Item
		{
			public uint Id;

			public uint Time;

			public Item(uint id, uint time)
			{
				Id = id;
				Time = time;
			}
		}

		/// <summary>
		/// Max used id, zero is reserved and never used.
		/// </summary>
		private uint m_maxId;

		private Queue<Item> m_reuseQueue;

		/// <summary>
		/// Minimal number of items in reuse queue until first item can be taken.
		/// </summary>
		private int m_protecionCount;

		/// <summary>
		/// Minimal time if item spent in reuse queue until it can be returned.
		/// Units are arbitrary
		/// </summary>
		private uint m_reuseProtectionTime;

		/// <summary>
		/// Function which returns current time, units are arbitrary and same as reuse protection time.
		/// </summary>
		private Func<uint> m_timeFunc;

		private SpinLockRef m_lock = new SpinLockRef();

		public int WaitingInQueue => m_reuseQueue.Count;

		/// <summary>
		/// Number of reserved ids, zero is also reserved, but not counted.
		/// </summary>
		public uint ReservedCount
		{
			get;
			private set;
		}

		public SequenceIdGenerator(int reuseProtectionCount = 2048, uint reuseProtectionTime = 60u, Func<uint> timeFunc = null)
		{
			m_reuseQueue = new Queue<Item>(reuseProtectionCount);
			m_protecionCount = Math.Max(0, reuseProtectionCount);
			m_reuseProtectionTime = reuseProtectionTime;
			m_timeFunc = timeFunc;
		}

		/// <summary>
		/// Creates new sequence id generator with stopwatch to measure protection time.
		/// </summary>
		/// <param name="reuseProtectionTime">Time to protect returned IDs.</param>
		/// <param name="reuseProtectionCount">Minimum number of IDs in protection queue, before first ID will be reused.</param>
		public static SequenceIdGenerator CreateWithStopwatch(TimeSpan reuseProtectionTime, int reuseProtectionCount = 2048)
		{
			Stopwatch sw = Stopwatch.StartNew();
			if (reuseProtectionTime.TotalSeconds > 5.0)
			{
				return new SequenceIdGenerator(reuseProtectionCount, (uint)reuseProtectionTime.TotalSeconds, () => (uint)sw.Elapsed.TotalSeconds);
			}
			if (reuseProtectionTime.TotalMilliseconds > 500.0)
			{
				return new SequenceIdGenerator(reuseProtectionCount, (uint)(reuseProtectionTime.TotalSeconds * 10.0), () => (uint)(sw.Elapsed.TotalSeconds * 10.0));
			}
			if (reuseProtectionTime.TotalMilliseconds > 50.0)
			{
				return new SequenceIdGenerator(reuseProtectionCount, (uint)(reuseProtectionTime.TotalSeconds * 100.0), () => (uint)(sw.Elapsed.TotalSeconds * 100.0));
			}
			return new SequenceIdGenerator(reuseProtectionCount, (uint)reuseProtectionTime.TotalMilliseconds, () => (uint)sw.Elapsed.TotalMilliseconds);
		}

		/// <summary>
		/// Reserves first several IDs, so it's never returned by generator.
		/// Zero is never returned, when reservedIdCount is 2, IDs 1 and 2 won't be ever returned.
		/// </summary>
		/// <param name="reservedIdCount">Number of reserved IDs which will be never returned by generator.</param>
		public void Reserve(uint reservedIdCount)
		{
			if (m_maxId != 0)
			{
				throw new InvalidOperationException("Reserve can be called only once and before any IDs are generated.");
			}
			m_maxId = reservedIdCount;
			ReservedCount = reservedIdCount;
		}

		private bool CheckFirstItemTime()
		{
			if (m_timeFunc == null)
			{
				return true;
			}
			uint num = m_timeFunc();
			uint time = m_reuseQueue.Peek().Time;
			if (num < time)
			{
				int count = m_reuseQueue.Count;
				for (int i = 0; i < count; i++)
				{
					Item item = m_reuseQueue.Dequeue();
					item.Time = num;
					m_reuseQueue.Enqueue(item);
				}
				return false;
			}
			return (ulong)((long)time + (long)m_reuseProtectionTime) < (ulong)num;
		}

		public uint NextId()
		{
			using (m_lock.Acquire())
			{
				if (m_reuseQueue.Count > m_protecionCount && CheckFirstItemTime())
				{
					return m_reuseQueue.Dequeue().Id;
				}
				return ++m_maxId;
			}
		}

		public void Return(uint id)
		{
			using (m_lock.Acquire())
			{
				m_reuseQueue.Enqueue(new Item(id, m_timeFunc()));
			}
		}
	}
}
