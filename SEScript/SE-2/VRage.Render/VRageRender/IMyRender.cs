using VRage;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	public interface IMyRender
	{
		/// <summary>
		/// Must be possible to query during startup before render thread and window is created.
		/// </summary>
		bool IsSupported
		{
			get;
		}

		string RootDirectory
		{
			get;
			set;
		}

		string RootDirectoryEffects
		{
			get;
			set;
		}

		string RootDirectoryDebug
		{
			get;
			set;
		}

		MySharedData SharedData
		{
			get;
		}

		MyTimeSpan CurrentDrawTime
		{
			get;
			set;
		}

		MyLog Log
		{
			get;
		}

		FrameProcessStatusEnum FrameProcessStatus
		{
			get;
		}

		MyViewport MainViewport
		{
			get;
		}

		Vector2I BackBufferResolution
		{
			get;
		}

		MyMessageQueue OutputQueue
		{
			get;
		}

		uint GlobalMessageCounter
		{
			get;
			set;
		}

		MyRenderDeviceSettings CreateDevice(MyRenderDeviceSettings? settingsToTry, out MyAdapterInfo[] adaptersList);

		void DisposeDevice();

		long GetAvailableTextureMemory();

		bool SettingsChanged(MyRenderDeviceSettings settings);

		void ApplySettings(MyRenderDeviceSettings settings);

		void Present();

		void EnqueueMessage(MyRenderMessageBase message, bool limitMaxQueueSize);

		void EnqueueOutputMessage(MyRenderMessageBase message);

		MyRenderProfiler GetRenderProfiler();

		void Draw(bool draw = true);

		void Ansel_DrawScene();

		bool IsVideoValid(uint id);

		VideoState GetVideoState(uint id);

		void GenerateShaderCache(bool clean, bool fastBuild, OnShaderCacheProgressDelegate onShaderCacheProgress);

		string GetLastExecutedAnnotation();

		string GetStatistics();

		void SetTimings(MyTimeSpan cpuDraw, MyTimeSpan cpuWait);

		Vector2 GetTextureSize(string name);
	}
}
