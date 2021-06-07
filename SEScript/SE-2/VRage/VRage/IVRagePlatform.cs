using System;
using System.Collections.Generic;
using VRage.Audio;
using VRage.Http;
using VRage.Input;
using VRage.Serialization;
using VRageMath;
using VRageRender;

namespace VRage
{
	public interface IVRagePlatform
	{
		IVRageHttp Http
		{
			get;
		}

		float CPUCounter
		{
			get;
		}

		float RAMCounter
		{
			get;
		}

		float GCMemory
		{
			get;
		}

		long RemainingAvailableMemory
		{
			get;
		}

		long ProcessPrivateMemory
		{
			get;
		}

		bool IsScriptCompilationSupported
		{
			get;
		}

		string Clipboard
		{
			get;
			set;
		}

		bool IsAllocationReady
		{
			get;
		}

		bool IsSingleInstance
		{
			get;
		}

		IAnsel Ansel
		{
			get;
		}

		IAfterMath AfterMath
		{
			get;
		}

		IVRageInput Input
		{
			get;
		}

		IVRageInput2 Input2
		{
			get;
		}

		IVRageWindow Window
		{
			get;
		}

		bool SessionReady
		{
			get;
			set;
		}

		IMyAnalytics Analytics
		{
			get;
		}

		bool IsRenderOutputDebugSupported
		{
			get;
		}

		IMyAudio Audio
		{
			get;
		}

		bool IsRemoteDebuggingSupported
		{
			get;
		}

		uint[] DeveloperKeys
		{
			get;
		}

		IMyImeProcessor ImeProcessor
		{
			get;
		}

		IMyCrashReporting CrashReporting
		{
			get;
		}

		event Action<IntPtr> OnSystemProtocolActivated;

		string GetOsName();

		string GetInfoCPU(out uint frequency);

		ulong GetTotalPhysicalMemory();

		void LogEnvironmentInformation();

		List<string> GetProcessesLockingFile(string path);

		void ResetColdStartRegister();

		ulong GetThreadAllocationStamp();

		ulong GetGlobalAllocationsStamp();

		IVideoPlayer CreateVideoPlayer();

		MessageBoxResult MessageBox(string text, string caption, MessageBoxOptions options);

		void ShowSplashScreen(string image, Vector2 scale);

		void HideSplashScreen();

		void CreateWindow(string gameName, string gameIcon, Type imeCandidateType);

		void CreateToolWindow(IntPtr windowHandle);

		void CreateInput2();

		IntPtr FindWindowInParent(string parent, string child);

		void PostMessage(IntPtr handle, uint wm, IntPtr wParam, IntPtr lParam);

		void InitAnalytics(string projectId, string version);

		void Init();

		void Done();

		void CreateRenderDevice(ref MyRenderDeviceSettings? settings, out object deviceInstance, out object swapChain);

		void DisposeRenderDevice();

		MyAdapterInfo[] GetRenderAdapterList();

		void ApplyRenderSettings(MyRenderDeviceSettings? settings);

		object CreateRenderAnnotation(object deviceContext);

		string GetAppDataPath();

		/// <summary>
		/// Get the ProtoBuf type model for this platform.
		/// </summary>
		/// <returns>A type model compatible with this platform.</returns>
		IProtoTypeModel GetTypeModel();

		void WriteLineToConsole(string msg);

		void LogToExternalDebugger(string message);

		bool OpenUrl(string url);

		void Update();
	}
}
