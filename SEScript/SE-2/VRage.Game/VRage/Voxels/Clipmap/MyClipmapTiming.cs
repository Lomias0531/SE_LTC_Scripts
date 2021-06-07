using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace VRage.Voxels.Clipmap
{
	public class MyClipmapTiming
	{
		[ThreadStatic]
		private static Stopwatch m_threadStopwatch;

		private static Dictionary<Thread, Stopwatch> m_stopwatches = new Dictionary<Thread, Stopwatch>();

		private static TimeSpan m_total;

		private static Stopwatch Stopwatch
		{
			get
			{
				if (m_threadStopwatch == null)
				{
					lock (m_stopwatches)
					{
						m_threadStopwatch = new Stopwatch();
						m_stopwatches[Thread.CurrentThread] = m_threadStopwatch;
					}
				}
				return m_threadStopwatch;
			}
		}

		/// <summary>
		/// Total time spent calculating ticks.
		/// </summary>
		public static TimeSpan Total => m_total;

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void StartTiming()
		{
			Stopwatch.Start();
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void StopTiming()
		{
			Stopwatch.Stop();
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void Reset()
		{
			lock (m_stopwatches)
			{
				foreach (Stopwatch value in m_stopwatches.Values)
				{
					value.Reset();
				}
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private static void ReadTotal()
		{
			lock (m_stopwatches)
			{
				long num = 0L;
				foreach (Stopwatch value in m_stopwatches.Values)
				{
					num += value.ElapsedTicks;
				}
				m_total = new TimeSpan(num);
			}
		}
	}
}
