using Sandbox.Game.Debugging;
using Sandbox.Game.World;
using System;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public static class MyFpsManager
	{
		private static long m_lastTime = 0L;

		private static uint m_fpsCounter = 0u;

		private static uint m_sessionTotalFrames = 0u;

		private static uint m_maxSessionFPS = 0u;

		private static uint m_minSessionFPS = 2147483647u;

		private static uint m_lastFpsDrawn = 0u;

		private static long m_lastFrameTime = 0L;

		private static long m_lastFrameMin = long.MaxValue;

		private static long m_lastFrameMax = long.MinValue;

		private static byte m_firstFrames = 0;

		private static readonly MyMovingAverage m_frameTimeAvg = new MyMovingAverage(60);

		public static float FrameTime
		{
			get;
			private set;
		}

		public static float FrameTimeAvg => m_frameTimeAvg.Avg;

		public static float FrameTimeMin
		{
			get;
			private set;
		}

		public static float FrameTimeMax
		{
			get;
			private set;
		}

		public static int GetFps()
		{
			return (int)m_lastFpsDrawn;
		}

		public static int GetSessionTotalFrames()
		{
			return (int)m_sessionTotalFrames;
		}

		public static int GetMaxSessionFPS()
		{
			return (int)m_maxSessionFPS;
		}

		public static int GetMinSessionFPS()
		{
			return (int)m_minSessionFPS;
		}

		public static void Update()
		{
			m_fpsCounter++;
			m_sessionTotalFrames++;
			if (MySession.Static == null)
			{
				m_sessionTotalFrames = 0u;
				m_maxSessionFPS = 0u;
				m_minSessionFPS = 2147483647u;
			}
			long num = MyPerformanceCounter.ElapsedTicks - m_lastFrameTime;
			FrameTime = (float)MyPerformanceCounter.TicksToMs(num);
			m_lastFrameTime = MyPerformanceCounter.ElapsedTicks;
			m_frameTimeAvg.Enqueue(FrameTime);
			if (num > m_lastFrameMax)
			{
				m_lastFrameMax = num;
			}
			if (num < m_lastFrameMin)
			{
				m_lastFrameMin = num;
			}
			if ((float)MyPerformanceCounter.TicksToMs(MyPerformanceCounter.ElapsedTicks - m_lastTime) >= 1000f)
			{
				FrameTimeMin = (float)MyPerformanceCounter.TicksToMs(m_lastFrameMin);
				FrameTimeMax = (float)MyPerformanceCounter.TicksToMs(m_lastFrameMax);
				m_lastFrameMin = long.MaxValue;
				m_lastFrameMax = long.MinValue;
				if (MySession.Static != null && m_firstFrames > 20)
				{
					m_minSessionFPS = Math.Min(m_minSessionFPS, m_fpsCounter);
					m_maxSessionFPS = Math.Max(m_maxSessionFPS, m_fpsCounter);
				}
				if (m_firstFrames <= 20)
				{
					m_firstFrames++;
				}
				m_lastTime = MyPerformanceCounter.ElapsedTicks;
				m_lastFpsDrawn = m_fpsCounter;
				m_fpsCounter = 0u;
			}
		}

		public static void Reset()
		{
			m_maxSessionFPS = 0u;
			m_minSessionFPS = 2147483647u;
			m_fpsCounter = 0u;
			m_sessionTotalFrames = 0u;
			m_lastTime = MyPerformanceCounter.ElapsedTicks;
			m_firstFrames = 0;
		}

		public static void PrepareMinMax()
		{
			if (m_firstFrames <= 20)
			{
				m_minSessionFPS = m_lastFpsDrawn;
				m_maxSessionFPS = m_lastFpsDrawn;
			}
		}
	}
}
