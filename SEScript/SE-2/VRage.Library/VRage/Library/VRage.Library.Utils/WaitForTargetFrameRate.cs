using System;
using System.Threading;

namespace VRage.Library.Utils
{
	public class WaitForTargetFrameRate
	{
		private long m_targetTicks;

		public bool EnableMaxSpeed;

		private const bool EnableUpdateWait = true;

		private readonly MyGameTimer m_timer;

		private readonly float m_targetFrequency;

		private float m_delta;

		public long TickPerFrame
		{
			get
			{
				int num = (int)Math.Round((float)MyGameTimer.Frequency / m_targetFrequency);
				return num;
			}
		}

		public WaitForTargetFrameRate(MyGameTimer timer, float targetFrequency = 60f)
		{
			m_timer = timer;
			m_targetFrequency = targetFrequency;
		}

		public void SetNextFrameDelayDelta(float delta)
		{
			m_delta = delta;
		}

		public void Wait()
		{
			m_timer.AddElapsed(MyTimeSpan.FromMilliseconds(0f - m_delta));
			long elapsedTicks = m_timer.ElapsedTicks;
			m_targetTicks += TickPerFrame;
			if (elapsedTicks > m_targetTicks + TickPerFrame * 5 || EnableMaxSpeed)
			{
				m_targetTicks = elapsedTicks;
			}
			else
			{
				int num = (int)(MyTimeSpan.FromTicks(m_targetTicks - elapsedTicks).Milliseconds - 0.1);
				if (num > 0)
				{
					Thread.CurrentThread.Join(num);
				}
				if (m_targetTicks < m_timer.ElapsedTicks + TickPerFrame + TickPerFrame / 4)
				{
					while (m_timer.ElapsedTicks < m_targetTicks)
					{
					}
				}
				else
				{
					m_targetTicks = m_timer.ElapsedTicks;
				}
			}
			m_delta = 0f;
		}
	}
}
