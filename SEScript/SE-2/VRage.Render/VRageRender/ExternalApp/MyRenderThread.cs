using System;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Library.Utils;
using VRage.Stats;
using VRage.Utils;
using VRageRender.Utils;

namespace VRageRender.ExternalApp
{
	public class MyRenderThread
	{
		private class StartParams
		{
			public InitHandler InitHandler;

			public MyRenderDeviceSettings? SettingsToTry;

			public MyRenderQualityEnum RenderQuality;
		}

		private readonly MyGameTimer m_timer;

		private readonly WaitForTargetFrameRate m_waiter;

		private MyTimeSpan m_messageProcessingStart;

		private MyTimeSpan m_frameStart;

		private int m_stopped;

		private IVRageWindow m_renderWindow;

		private MyRenderQualityEnum m_currentQuality;

		private MyRenderDeviceSettings m_settings;

		private MyRenderDeviceSettings? m_newSettings;

		private int m_newQuality = -1;

		private readonly MyConcurrentQueue<Action> m_invokeQueue = new MyConcurrentQueue<Action>(16);

		public readonly Thread SystemThread;

		private readonly bool m_separateThread;

		private readonly MyConcurrentQueue<EventWaitHandle> m_debugWaitForPresentHandles = new MyConcurrentQueue<EventWaitHandle>(16);

		private int m_debugWaitForPresentHandleCount;

		private MyAdapterInfo[] m_adapterList;

		private MyTimeSpan m_waitStart;

		private MyTimeSpan m_drawStart;

		public IVRageWindow RenderWindow => m_renderWindow;

		public int CurrentAdapter => m_settings.AdapterOrdinal;

		public MyRenderDeviceSettings CurrentSettings => m_settings;

		public ManualResetEvent RenderUpdateSyncEvent
		{
			get;
			set;
		}

		public event Action BeforeDraw;

		public event SizeChangedHandler SizeChanged;

		private MyRenderThread(MyGameTimer timer, bool separateThread, float maxFrameRate)
		{
			m_timer = timer;
			m_waiter = new WaitForTargetFrameRate(timer, maxFrameRate);
			m_separateThread = separateThread;
			if (separateThread)
			{
				SystemThread = new Thread(RenderThreadStart);
				SystemThread.IsBackground = true;
				SystemThread.Name = "Render thread";
				SystemThread.CurrentCulture = CultureInfo.InvariantCulture;
				SystemThread.CurrentUICulture = CultureInfo.InvariantCulture;
			}
			else
			{
				SystemThread = Thread.CurrentThread;
			}
		}

		public static MyRenderThread Start(MyGameTimer timer, InitHandler initHandler, MyRenderDeviceSettings? settingsToTry, MyRenderQualityEnum renderQuality, float maxFrameRate)
		{
			MyRenderThread myRenderThread = new MyRenderThread(timer, separateThread: true, maxFrameRate);
			myRenderThread.SystemThread.Start(new StartParams
			{
				InitHandler = initHandler,
				SettingsToTry = settingsToTry,
				RenderQuality = renderQuality
			});
			return myRenderThread;
		}

		public static MyRenderThread StartSync(MyGameTimer timer, IVRageWindow renderWindow, MyRenderDeviceSettings? settingsToTry, MyRenderQualityEnum renderQuality, float maxFrameRate)
		{
			MyRenderThread myRenderThread = new MyRenderThread(timer, separateThread: false, maxFrameRate)
			{
				m_renderWindow = renderWindow
			};
			myRenderThread.m_settings = MyRenderProxy.CreateDevice(myRenderThread, settingsToTry, out myRenderThread.m_adapterList);
			MyRenderProxy.SendCreatedDeviceSettings(myRenderThread.m_settings);
			myRenderThread.m_currentQuality = renderQuality;
			myRenderThread.UpdateSize();
			return myRenderThread;
		}

		public void TickSync()
		{
			if (MyRenderProxy.EnableAppEventsCall)
			{
				m_renderWindow.DoEvents();
			}
			RenderCallback(async: false);
		}

		public void Invoke(Action action)
		{
			m_invokeQueue.Enqueue(action);
		}

		public void SwitchSettings(MyRenderDeviceSettings settings)
		{
			m_newSettings = settings;
		}

		public void SwitchQuality(MyRenderQualityEnum quality)
		{
			m_newQuality = (int)quality;
		}

		/// <summary>
		/// Signals the thread to exit and waits until it does so
		/// </summary>
		public void Exit()
		{
			if (Interlocked.Exchange(ref m_stopped, 1) == 1)
			{
				return;
			}
			if (SystemThread != null)
			{
				try
				{
					m_renderWindow.Exit();
				}
				catch
				{
				}
				if (Thread.CurrentThread != SystemThread)
				{
					SystemThread.Join();
				}
			}
			else
			{
				UnloadContent();
				MyRenderProxy.DisposeDevice();
			}
		}

		private void RenderThreadStart(object param)
		{
			StartParams startParams = (StartParams)param;
			m_renderWindow = startParams.InitHandler();
			m_settings = MyRenderProxy.CreateDevice(this, startParams.SettingsToTry, out m_adapterList);
			if (m_settings.AdapterOrdinal == -1)
			{
				return;
			}
			MyRenderProxy.SendCreatedDeviceSettings(m_settings);
			m_currentQuality = startParams.RenderQuality;
			UpdateSize();
			if (MyRenderProxy.Settings.EnableAnsel)
			{
				MyVRage.Platform.Ansel.Init(MyRenderProxy.Settings.EnableAnselWithSprites);
			}
			while (m_renderWindow.UpdateRenderThread())
			{
				if (RenderUpdateSyncEvent != null)
				{
					RenderUpdateSyncEvent.WaitOne();
				}
				RenderCallback(async: true);
			}
			MyRenderProxy.AfterUpdate(null);
			MyRenderProxy.BeforeUpdate();
			MyRenderProxy.ProcessMessages();
			UnloadContent();
			MyRenderProxy.DisposeDevice();
		}

		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		private void RenderCallback(bool async)
		{
			try
			{
				RenderFrame(async);
			}
			catch (Exception ex)
			{
				MyMiniDump.CollectExceptionDump(ex);
				string text = $"Exception in render!\n\nAftermath: {MyRenderProxy.GetLastExecutedAnnotation()}\nException: {ex}\nStatistics: {MyRenderProxy.GetStatistics()}";
				MyVRage.Platform.LogToExternalDebugger(text);
				MyLog.Default.WriteLine(text);
				MyLog.Default.Flush();
				string text2 = "Graphics device driver has crashed.\n\nYour card is probably overheating or driver is malfunctioning. Please, update your graphics drivers and remove any overclocking";
				MyMessageBox.Show("Game crashed", text2);
				throw;
			}
		}

		private void RenderFrame(bool async)
		{
			if (SystemThread != null)
			{
				ThreadPriority threadPriority = MyRenderProxy.Settings.RenderThreadHighPriority ? ThreadPriority.AboveNormal : ThreadPriority.Normal;
				if (SystemThread.Priority != threadPriority)
				{
					SystemThread.Priority = threadPriority;
				}
			}
			if (MyVRage.Platform.Ansel.IsCaptureRunning)
			{
				MyRenderProxy.Ansel_DrawScene();
				MyRenderProxy.Present();
				return;
			}
			if (m_messageProcessingStart != MyTimeSpan.Zero)
			{
				_ = m_timer.Elapsed - m_messageProcessingStart;
				m_waiter.Wait();
			}
			MySimpleProfiler.BeginBlock("RenderFrame", MySimpleProfiler.ProfilingBlockType.RENDER);
			m_drawStart = m_timer.Elapsed;
			MyTimeSpan cpuWait = m_drawStart - m_waitStart;
			m_frameStart = m_timer.Elapsed;
			switch (MyRenderProxy.FrameProcessStatus)
			{
			}
			Action instance;
			while (m_invokeQueue.TryDequeue(out instance))
			{
				instance();
			}
			ApplySettingsChanges();
			MyRenderStats.Generic.WriteFormat("Available GPU memory: {0} MB", (float)MyRenderProxy.GetAvailableTextureMemory() / 1024f / 1024f, MyStatTypeEnum.CurrentValue, 300, 2);
			MyRenderProxy.BeforeRender(m_frameStart);
			if (this.BeforeDraw != null)
			{
				this.BeforeDraw();
			}
			if (!m_renderWindow.DrawEnabled)
			{
				MyRenderProxy.ProcessMessages();
			}
			else
			{
				Draw();
			}
			MyRenderProxy.AfterRender();
			_ = m_separateThread;
			m_waitStart = m_timer.Elapsed;
			MyTimeSpan cpuDraw = m_waitStart - m_drawStart;
			MySimpleProfiler.End("RenderFrame");
			if (m_renderWindow.DrawEnabled)
			{
				DoBeforePresent();
				try
				{
					MyRenderProxy.Present();
				}
				catch (MyDeviceErrorException ex)
				{
					MyRenderProxy.Error(ex.Message, 0, shouldTerminate: true);
					Exit();
				}
				DoAfterPresent();
			}
			MyRenderProxy.SetTimings(cpuDraw, cpuWait);
			m_messageProcessingStart = m_timer.Elapsed;
			if (MyRenderProxy.Settings.ForceSlowCPU)
			{
				Thread.Sleep(200);
			}
		}

		private void DoBeforePresent()
		{
			m_debugWaitForPresentHandleCount = m_debugWaitForPresentHandles.Count;
		}

		private void DoAfterPresent()
		{
			for (int i = 0; i < m_debugWaitForPresentHandleCount; i++)
			{
				if (m_debugWaitForPresentHandles.TryDequeue(out EventWaitHandle instance))
				{
					instance?.Set();
				}
			}
			m_debugWaitForPresentHandleCount = 0;
		}

		public void DebugAddWaitingForPresent(EventWaitHandle handle)
		{
			m_debugWaitForPresentHandles.Enqueue(handle);
		}

		private void ApplySettingsChanges()
		{
			int num = Interlocked.Exchange(ref m_newQuality, -1);
			if (num != -1)
			{
				m_currentQuality = (MyRenderQualityEnum)num;
			}
			if (m_newSettings.HasValue && MyRenderProxy.SettingsChanged(m_newSettings.Value))
			{
				m_settings = m_newSettings.Value;
				m_newSettings = null;
				UnloadContent();
				MyRenderProxy.ApplySettings(m_settings);
				UpdateSize();
			}
		}

		public void UpdateSize()
		{
			this.SizeChanged?.Invoke(MyRenderProxy.BackBufferResolution.X, MyRenderProxy.BackBufferResolution.Y, MyRenderProxy.MainViewport);
		}

		private void UnloadContent()
		{
			MyRenderProxy.UnloadContent();
		}

		private void Draw()
		{
			MyRenderProxy.Draw();
			MyRenderProxy.GetRenderProfiler().Draw("Draw", 462, "E:\\Repo1\\Sources\\VRage.Render\\ExternalApp\\MyRenderThread.cs");
		}
	}
}
