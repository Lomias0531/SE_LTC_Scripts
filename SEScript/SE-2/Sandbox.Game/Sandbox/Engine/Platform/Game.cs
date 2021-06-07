using ParallelTasks;
using Sandbox.Engine.Utils;
using Sandbox.Game.Debugging;
using Sandbox.Game.World;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Engine.Platform
{
	public abstract class Game
	{
		public static bool IsDedicated;

		public static bool IsPirated;

		public static bool IgnoreLastSession;

		public static IPEndPoint ConnectToServer;

		public static bool EnableSimSpeedLocking;

		[Obsolete("Remove asap, it is here only because of main menu music..")]
		protected readonly MyGameTimer m_gameTimer = new MyGameTimer();

		private MyTimeSpan m_drawTime;

		private MyTimeSpan m_totalTime;

		private ulong m_updateCounter;

		private MyTimeSpan m_simulationTimeWithSpeed;

		public const double TARGET_MS_PER_FRAME = 16.666666666666668;

		private const int NUM_FRAMES_FOR_DROP = 5;

		private const float NUM_MS_TO_INCREASE = 2000f;

		private const float PEAK_TRESHOLD_RATIO = 0.4f;

		private const float RATIO_TO_INCREASE_INSTANTLY = 0.25f;

		private float m_currentFrameIncreaseTime;

		private long m_currentMin;

		private long m_targetTicks;

		private MyQueue<long> m_lastFrameTiming = new MyQueue<long>(5);

		private bool isFirstUpdateDone;

		private bool isMouseVisible;

		public long FrameTimeTicks;

		private static long m_lastFrameTime;

		private static float m_targetMs;

		private readonly FixedLoop m_renderLoop = new FixedLoop(Stats.Generic, "WaitForUpdate");

		public MyTimeSpan DrawTime => m_drawTime;

		public MyTimeSpan TotalTime => m_totalTime;

		public ulong SimulationFrameCounter => m_updateCounter;

		public MyTimeSpan SimulationTime => MyTimeSpan.FromMilliseconds((double)m_updateCounter * 16.666666666666668);

		public MyTimeSpan SimulationTimeWithSpeed => m_simulationTimeWithSpeed;

		public Thread UpdateThread
		{
			get;
			protected set;
		}

		public Thread DrawThread
		{
			get;
			protected set;
		}

		public float CPULoad
		{
			get;
			private set;
		}

		public float CPULoadSmooth
		{
			get;
			private set;
		}

		public float CPUTimeSmooth
		{
			get;
			private set;
		}

		public float ThreadLoad
		{
			get;
			private set;
		}

		public float ThreadLoadSmooth
		{
			get;
			private set;
		}

		public float ThreadTimeSmooth
		{
			get;
			private set;
		}

		public static float SimulationRatio => 16.666666f / m_targetMs;

		public bool IsActive
		{
			get;
			private set;
		}

		public bool IsRunning
		{
			get;
			private set;
		}

		public bool IsFirstUpdateDone => isFirstUpdateDone;

		public bool EnableMaxSpeed
		{
			get
			{
				return m_renderLoop.EnableMaxSpeed;
			}
			set
			{
				m_renderLoop.EnableMaxSpeed = value;
			}
		}

		public bool Exiting => m_renderLoop.IsDone;

		public event Action OnGameExit;

		public Game()
		{
			IsActive = true;
			CPULoadSmooth = 1f;
		}

		public void SetNextFrameDelayDelta(float delta)
		{
			m_renderLoop.SetNextFrameDelayDelta(delta);
		}

		public void Exit()
		{
			this.OnGameExit?.Invoke();
			m_renderLoop.IsDone = true;
		}

		protected void RunLoop()
		{
			try
			{
				m_targetTicks = m_renderLoop.TickPerFrame;
				MyLog.Default.WriteLine("Timer Frequency: " + MyGameTimer.Frequency);
				MyLog.Default.WriteLine("Ticks per frame: " + m_renderLoop.TickPerFrame);
				m_renderLoop.Run(RunSingleFrame);
			}
			catch (SEHException ex)
			{
				MyLog.Default.WriteLine("SEHException caught. Error code: " + ex.ErrorCode);
				throw ex;
			}
		}

		public void RunSingleFrame()
		{
			_ = IsFirstUpdateDone;
			long elapsedTicks = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks;
			UpdateInternal();
			FrameTimeTicks = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks - elapsedTicks;
			float num = (float)new MyTimeSpan(FrameTimeTicks).Seconds;
			CPULoad = num / 0.0166666675f * 100f;
			CPULoadSmooth = MathHelper.Smooth(CPULoad, CPULoadSmooth);
			CPUTimeSmooth = MathHelper.Smooth(num * 1000f, CPUTimeSmooth);
			float num2 = (float)new MyTimeSpan(Parallel.Scheduler.ReadAndClearExecutionTime()).Seconds;
			ThreadLoad = num2 / 0.0166666675f * 100f;
			ThreadLoadSmooth = MathHelper.Smooth(ThreadLoad, ThreadLoadSmooth);
			ThreadTimeSmooth = MathHelper.Smooth(num2 * 1000f, ThreadTimeSmooth);
			if (MyFakes.PRECISE_SIM_SPEED)
			{
				long ticks = Math.Min(Math.Max(m_renderLoop.TickPerFrame, UpdateCurrentFrame()), 10 * m_renderLoop.TickPerFrame);
				m_targetMs = (float)Math.Max(16.666666666666668, Sandbox.Game.Debugging.MyPerformanceCounter.TicksToMs(ticks));
			}
			if (EnableSimSpeedLocking && MyFakes.ENABLE_SIMSPEED_LOCKING)
			{
				Lock(elapsedTicks);
			}
		}

		private void Lock(long beforeUpdate)
		{
			long num = Math.Min(Math.Max(m_renderLoop.TickPerFrame, UpdateCurrentFrame()), 10 * m_renderLoop.TickPerFrame);
			m_currentMin = Math.Max(num, m_currentMin);
			m_currentFrameIncreaseTime += m_targetMs;
			if (num > m_targetTicks)
			{
				m_targetTicks = num;
				m_currentFrameIncreaseTime = 0f;
				m_currentMin = 0L;
				m_targetMs = (float)Sandbox.Game.Debugging.MyPerformanceCounter.TicksToMs(m_targetTicks);
			}
			else
			{
				bool flag = (float)(m_targetTicks - m_currentMin) > 0.25f * (float)m_renderLoop.TickPerFrame;
				if (m_currentFrameIncreaseTime > 2000f || flag)
				{
					m_targetTicks = m_currentMin;
					m_currentFrameIncreaseTime = 0f;
					m_currentMin = 0L;
					m_targetMs = (float)Sandbox.Game.Debugging.MyPerformanceCounter.TicksToMs(m_targetTicks);
				}
			}
			long num2 = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks - beforeUpdate;
			int num3 = (int)(MyTimeSpan.FromTicks(m_targetTicks - num2).Milliseconds - 0.1);
			if (num3 > 0 && !EnableMaxSpeed)
			{
				Thread.CurrentThread.Join(num3);
			}
			num2 = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks - beforeUpdate;
			while (m_targetTicks > num2)
			{
				num2 = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks - beforeUpdate;
			}
		}

		private long UpdateCurrentFrame()
		{
			if (m_lastFrameTiming.Count > 5)
			{
				m_lastFrameTiming.Dequeue();
			}
			m_lastFrameTiming.Enqueue(FrameTimeTicks);
			long num = long.MaxValue;
			long num2 = 0L;
			double num3 = 0.0;
			for (int i = 0; i < m_lastFrameTiming.Count; i++)
			{
				num = Math.Min(num, m_lastFrameTiming[i]);
				num2 = Math.Max(num2, m_lastFrameTiming[i]);
				num3 += (double)m_lastFrameTiming[i];
			}
			num3 /= (double)m_lastFrameTiming.Count;
			double num4 = (float)(num2 - num) * 0.4f;
			long num5 = 0L;
			for (int j = 0; j < m_lastFrameTiming.Count; j++)
			{
				if (Math.Abs((double)m_lastFrameTiming[j] - num3) < num4)
				{
					num5 = Math.Max(num2, m_lastFrameTiming[j]);
				}
			}
			if (num5 == 0L)
			{
				return (long)num3;
			}
			return num5;
		}

		protected abstract void PrepareForDraw();

		protected abstract void AfterDraw();

		protected abstract void LoadData_UpdateThread();

		protected abstract void UnloadData_UpdateThread();

		private void UpdateInternal()
		{
			MySimpleProfiler.BeginBlock("UpdateFrame", MySimpleProfiler.ProfilingBlockType.INTERNAL);
			using (Stats.Generic.Measure("BeforeUpdate"))
			{
				MyRenderProxy.BeforeUpdate();
			}
			m_totalTime = m_gameTimer.Elapsed;
			m_updateCounter++;
			if (MySession.Static != null)
			{
				m_simulationTimeWithSpeed += MyTimeSpan.FromMilliseconds(16.666666666666668 * (double)MyFakes.SIMULATION_SPEED);
			}
			_ = MyCompilationSymbols.EnableNetworkPacketTracking;
			Update();
			if (!IsDedicated)
			{
				PrepareForDraw();
			}
			using (Stats.Generic.Measure("AfterUpdate"))
			{
				AfterDraw();
			}
			MySimpleProfiler.End("UpdateFrame");
			MySimpleProfiler.Commit();
		}

		protected virtual void Update()
		{
			isFirstUpdateDone = true;
		}
	}
}
