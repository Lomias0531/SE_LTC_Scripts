using Sandbox.AppCode;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game;
using System;
using System.Threading;
using VRage;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox
{
	public class MySandboxExternal : MySandboxGame
	{
		public readonly IExternalApp ExternalApp;

		private MyRenderDeviceSettings m_currentSettings;

		public MySandboxExternal(IExternalApp externalApp, string[] commandlineArgs, IntPtr windowHandle)
			: base(commandlineArgs, windowHandle)
		{
			ExternalApp = externalApp;
		}

		public override void SwitchSettings(MyRenderDeviceSettings settings)
		{
			m_currentSettings = settings;
			m_currentSettings.WindowMode = MyWindowModeEnum.Window;
			base.SwitchSettings(m_currentSettings);
		}

		protected override void StartRenderComponent(MyRenderDeviceSettings? settings, IntPtr windowHandle)
		{
			base.DrawThread = Thread.CurrentThread;
			MyVRage.Platform.CreateToolWindow(windowHandle);
			IVRageWindow window = MyVRage.Platform.Window;
			MySandboxGame.m_windowCreatedEvent.Set();
			MyVRage.Platform.Window.OnExit += MySandboxGame.ExitThreadSafe;
			if (!settings.HasValue)
			{
				settings = new MyRenderDeviceSettings(0, MyWindowModeEnum.Window, window.ClientSize.X, window.ClientSize.Y, 0, 0, useStereoRendering: false, settingsMandatory: false);
			}
			GameRenderComponent.StartSync(m_gameTimer, window, settings, MyRenderQualityEnum.NORMAL, MyPerGameSettings.MaxFrameRate);
			GameRenderComponent.RenderThread.SizeChanged += base.RenderThread_SizeChanged;
			GameRenderComponent.RenderThread.BeforeDraw += base.RenderThread_BeforeDraw;
			RenderThread_SizeChanged(viewport: new MyViewport(0f, 0f, window.ClientSize.X, window.ClientSize.Y), width: window.ClientSize.X, height: window.ClientSize.Y);
		}

		protected override void Update()
		{
			base.Update();
			ExternalApp.Update();
		}

		protected override void CheckGraphicsCard(MyRenderMessageVideoAdaptersResponse msgVideoAdapters)
		{
			base.CheckGraphicsCard(msgVideoAdapters);
			MyPerformanceSettings myPerformanceSettings = default(MyPerformanceSettings);
			myPerformanceSettings.RenderSettings = new MyRenderSettings1
			{
				AnisotropicFiltering = MyTextureAnisoFiltering.NONE,
				AntialiasingMode = MyAntialiasingMode.FXAA,
				ShadowQuality = MyShadowsQuality.MEDIUM,
				AmbientOcclusionEnabled = true,
				TextureQuality = MyTextureQuality.MEDIUM,
				ModelQuality = MyRenderQualityEnum.NORMAL,
				VoxelQuality = MyRenderQualityEnum.NORMAL,
				GrassDrawDistance = 160f,
				GrassDensityFactor = 1f,
				HqDepth = true,
				VoxelShaderQuality = MyRenderQualityEnum.NORMAL,
				AlphaMaskedShaderQuality = MyRenderQualityEnum.NORMAL,
				AtmosphereShaderQuality = MyRenderQualityEnum.NORMAL,
				DistanceFade = 100f
			};
			myPerformanceSettings.EnableDamageEffects = true;
			MyPerformanceSettings defaults = myPerformanceSettings;
			MyVideoSettingsManager.UpdateRenderSettingsFromConfig(ref defaults);
		}

		protected override void AfterDraw()
		{
			base.AfterDraw();
			if (GameRenderComponent.RenderThread != null)
			{
				Vector2I clientSize = MyVRage.Platform.Window.ClientSize;
				if ((m_currentSettings.BackBufferWidth != clientSize.X || m_currentSettings.BackBufferHeight != clientSize.Y) && clientSize.X > 0 && clientSize.Y > 0)
				{
					MyRenderDeviceSettings settings = default(MyRenderDeviceSettings);
					settings.AdapterOrdinal = m_currentSettings.AdapterOrdinal;
					settings.RefreshRate = m_currentSettings.RefreshRate;
					settings.VSync = m_currentSettings.VSync;
					settings.WindowMode = m_currentSettings.WindowMode;
					settings.BackBufferWidth = clientSize.X;
					settings.BackBufferHeight = clientSize.Y;
					SwitchSettings(settings);
				}
				GameRenderComponent.RenderThread.TickSync();
			}
		}
	}
}
