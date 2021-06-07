using System.Diagnostics;

namespace Sandbox.Game.Debugging
{
	internal static class MyPerformanceCounter
	{
		private struct Timer
		{
			public static readonly Timer Empty = new Timer
			{
				Runtime = 0L,
				StartTime = long.MaxValue
			};

			public long StartTime;

			public long Runtime;

			public bool IsRunning => StartTime != long.MaxValue;
		}

		private static Stopwatch m_timer;

		public static long ElapsedTicks => m_timer.ElapsedTicks;

		static MyPerformanceCounter()
		{
			m_timer = new Stopwatch();
			m_timer.Start();
		}

		public static double TicksToMs(long ticks)
		{
			return (double)ticks / (double)Stopwatch.Frequency * 1000.0;
		}
	}
}
