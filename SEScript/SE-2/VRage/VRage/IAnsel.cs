using System;

namespace VRage
{
	public interface IAnsel
	{
		bool IsSessionEnabled
		{
			get;
			set;
		}

		bool IsGamePausable
		{
			get;
			set;
		}

		bool IsCaptureRunning
		{
			get;
		}

		bool IsSessionRunning
		{
			get;
		}

		bool Is360Capturing
		{
			get;
		}

		bool IsMultiresCapturing
		{
			get;
		}

		bool IsOverlayEnabled
		{
			get;
		}

		bool IsInitializedSuccessfuly
		{
			get;
		}

		event Action<int> StartCaptureDelegate;

		event Action StopCaptureDelegate;

		event Action<bool, bool> WarningMessageDelegate;

		event Func<bool> IsSpectatorEnabledDelegate;

		int Init(bool settingsEnableAnselWithSprites);

		void SetCamera(ref MyCameraSetup cameraSetup);

		void GetCamera(out MyCameraSetup cameraSetup);

		void Enable();

		void StopSession();

		void MarkHdrBufferBind();

		void MarkHdrBufferFinished();
	}
}
