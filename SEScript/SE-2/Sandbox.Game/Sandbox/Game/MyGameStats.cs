using System;

namespace Sandbox.Game
{
	internal class MyGameStats
	{
		private DateTime m_lastStatMeasurePerSecond;

		private long m_previousUpdateCount;

		public static MyGameStats Static
		{
			get;
			private set;
		}

		public long UpdateCount
		{
			get;
			private set;
		}

		public long UpdatesPerSecond
		{
			get;
			private set;
		}

		static MyGameStats()
		{
			Static = new MyGameStats();
		}

		private MyGameStats()
		{
			m_previousUpdateCount = 0L;
			UpdateCount = 0L;
		}

		public void Update()
		{
			UpdateCount++;
			if ((DateTime.UtcNow - m_lastStatMeasurePerSecond).TotalSeconds >= 1.0)
			{
				UpdatesPerSecond = UpdateCount - m_previousUpdateCount;
				m_previousUpdateCount = UpdateCount;
				m_lastStatMeasurePerSecond = DateTime.UtcNow;
			}
		}
	}
}
