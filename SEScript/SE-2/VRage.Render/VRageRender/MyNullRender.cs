using VRage;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;
using VRageRender.Profiler;

namespace VRageRender
{
	public class MyNullRender : IMyRender
	{
		private MyMessageQueue m_outputQueue = new MyMessageQueue();

		private MyNullRenderProfiler m_profiler = new MyNullRenderProfiler();

		public string RootDirectory
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public string RootDirectoryEffects
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public string RootDirectoryDebug
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public MyLog Log => null;

		public FrameProcessStatusEnum FrameProcessStatus
		{
			get;
			private set;
		}

		public MySharedData SharedData => null;

		public MyTimeSpan CurrentDrawTime
		{
			get;
			set;
		}

		public MyViewport MainViewport
		{
			get;
			private set;
		}

		public Vector2I BackBufferResolution
		{
			get;
			private set;
		}

		public MyMessageQueue OutputQueue => m_outputQueue;

		public uint GlobalMessageCounter
		{
			get;
			set;
		}

		bool IMyRender.IsSupported => true;

		public MyNullRender()
		{
			MyRenderProfiler.SetAutocommit(val: false);
		}

		public void EnqueueMessage(MyRenderMessageBase message, bool limitMaxQueueSize)
		{
			message.Dispose();
		}

		public void ProcessMessages()
		{
		}

		public void EnqueueOutputMessage(MyRenderMessageBase message)
		{
		}

		public void ResetEnvironmentProbes()
		{
		}

		public MyRenderProfiler GetRenderProfiler()
		{
			return m_profiler;
		}

		public void Draw(bool draw = true)
		{
		}

		public void Ansel_DrawScene()
		{
		}

		public bool IsVideoValid(uint id)
		{
			return false;
		}

		public VideoState GetVideoState(uint id)
		{
			return VideoState.Stopped;
		}

		public double GetVideoPosition(uint id)
		{
			return 0.0;
		}

		public Vector2 GetVideoSize(uint id)
		{
			return Vector2.Zero;
		}

		public MyRenderDeviceSettings CreateDevice(MyRenderDeviceSettings? settingsToTry, out MyAdapterInfo[] adaptersList)
		{
			adaptersList = null;
			return default(MyRenderDeviceSettings);
		}

		public void DisposeDevice()
		{
		}

		public long GetAvailableTextureMemory()
		{
			return 0L;
		}

		public MyRenderDeviceCooperativeLevel TestDeviceCooperativeLevel()
		{
			return MyRenderDeviceCooperativeLevel.Ok;
		}

		public bool SettingsChanged(MyRenderDeviceSettings settings)
		{
			return true;
		}

		public void ApplySettings(MyRenderDeviceSettings settings)
		{
		}

		public void Present()
		{
		}

		public void GenerateShaderCache(bool clean, bool fastBuild, OnShaderCacheProgressDelegate onShaderCacheProgress)
		{
		}

		public string GetLastExecutedAnnotation()
		{
			return null;
		}

		public string GetStatistics()
		{
			return null;
		}

		public void SetTimings(MyTimeSpan cpuDraw, MyTimeSpan cpuWait)
		{
		}

		public Vector2 GetTextureSize(string name)
		{
			return Vector2.Zero;
		}
	}
}
