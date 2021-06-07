using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Collections;
using VRage.Library.Collections;
using VRage.Library.Threading;

namespace VRage.Utils
{
	public class MyDebugHitCounter : IEnumerable<MyDebugHitCounter.Sample>, IEnumerable
	{
		public struct Sample
		{
			public uint Count;

			public uint Cycle;

			public float Value => (float)Count / (float)Cycle;

			public override string ToString()
			{
				return Value.ToString();
			}
		}

		public readonly MyQueue<Sample> History;

		private Sample current;

		private readonly uint m_sampleCycle;

		private readonly uint m_historyLength;

		private SpinLockRef m_lock = new SpinLockRef();

		public float CurrentHitRatio
		{
			get
			{
				using (m_lock.Acquire())
				{
					return current.Value;
				}
			}
		}

		public float LastCycleHitRatio
		{
			get
			{
				using (m_lock.Acquire())
				{
					if (History.Count > 1)
					{
						return History[1].Value;
					}
					return 0f;
				}
			}
		}

		public MyDebugHitCounter(uint cycleSize = 100000u)
		{
			m_sampleCycle = cycleSize;
			m_historyLength = 10u;
			History = new MyQueue<Sample>((int)m_historyLength);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public void Hit()
		{
			using (m_lock.Acquire())
			{
				current.Count++;
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public void Miss()
		{
			using (m_lock.Acquire())
			{
				current.Cycle++;
				if (current.Cycle == m_sampleCycle)
				{
					Cycle();
				}
			}
		}

		public void Cycle()
		{
			using (m_lock.Acquire())
			{
				if (History.Count >= m_historyLength)
				{
					History.Dequeue();
				}
				History.Enqueue(current);
				current = default(Sample);
			}
		}

		public float ValueAndCycle()
		{
			Cycle();
			return LastCycleHitRatio;
		}

		public void CycleWork()
		{
			if (current.Count != 0)
			{
				Cycle();
			}
		}

		private IEnumerator<Sample> GetEnumeratorInternal()
		{
			yield return current;
			foreach (Sample item in History)
			{
				yield return item;
			}
		}

		public ConcurrentEnumerator<SpinLockRef.Token, Sample, IEnumerator<Sample>> GetEnumerator()
		{
			return ConcurrentEnumerator.Create<SpinLockRef.Token, Sample, IEnumerator<Sample>>(m_lock.Acquire(), GetEnumeratorInternal());
		}

		IEnumerator<Sample> IEnumerable<Sample>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
