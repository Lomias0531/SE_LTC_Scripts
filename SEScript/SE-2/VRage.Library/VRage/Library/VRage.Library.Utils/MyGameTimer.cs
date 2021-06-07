using System;
using System.Diagnostics;

namespace VRage.Library.Utils
{
	/// <summary>
	/// Global thread-safe timer.
	/// Time for update and time for draw must be copied at the beginning of update and draw.
	/// </summary>
	public class MyGameTimer
	{
		private long m_startTicks;

		private long m_elapsedTicks;

		/// <summary>
		/// Number of ticks per seconds
		/// </summary>
		public static readonly long Frequency = Stopwatch.Frequency;

		/// <summary>
		/// This may not be accurate for large values - double accuracy
		/// </summary>
		public TimeSpan ElapsedTimeSpan => Elapsed.TimeSpan;

		public long ElapsedTicks => m_elapsedTicks + (Stopwatch.GetTimestamp() - m_startTicks);

		public MyTimeSpan Elapsed => new MyTimeSpan(ElapsedTicks);

		public void AddElapsed(MyTimeSpan timespan)
		{
			m_startTicks -= timespan.Ticks;
		}

		public MyGameTimer()
		{
			m_startTicks = Stopwatch.GetTimestamp();
			m_elapsedTicks = 0L;
		}
	}
}
