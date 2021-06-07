using System.Threading;

namespace VRage.Parallelization
{
	/// <summary>
	/// Allows to pause one thread at exact points
	/// </summary>
	public class MyPausableJob
	{
		private volatile bool m_pause;

		private AutoResetEvent m_pausedEvent = new AutoResetEvent(initialState: false);

		private AutoResetEvent m_resumedEvent = new AutoResetEvent(initialState: false);

		public bool IsPaused => m_pause;

		public void Pause()
		{
			m_pause = true;
			m_pausedEvent.WaitOne();
		}

		public void Resume()
		{
			m_pause = false;
			m_resumedEvent.Set();
		}

		public void AllowPauseHere()
		{
			if (m_pause)
			{
				m_pausedEvent.Set();
				m_resumedEvent.WaitOne();
			}
		}
	}
}
