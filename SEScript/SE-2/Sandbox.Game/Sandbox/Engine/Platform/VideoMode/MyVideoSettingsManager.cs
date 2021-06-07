using Sandbox.AppCode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.EntityComponents.Renders;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Engine.Platform.VideoMode
{
	public static class MyVideoSettingsManager
	{
		public enum ChangeResult
		{
			Success,
			NothingChanged,
			Failed
		}

		private static Dictionary<int, MyAspectRatio> m_recommendedAspectRatio;

		private static MyAdapterInfo[] m_adapters;

		private static readonly MyAspectRatio[] m_aspectRatios;

		private static MyRenderDeviceSettings m_currentDeviceSettings;

		private static bool m_currentDeviceIsTripleHead;

		private static MyGraphicsSettings m_currentGraphicsSettings;

		public static readonly MyDisplayMode[] DebugDisplayModes;

		public static MyAdapterInfo[] Adapters => m_adapters;

		public static MyRenderDeviceSettings CurrentDeviceSettings => m_currentDeviceSettings;

		public static MyGraphicsSettings CurrentGraphicsSettings => m_currentGraphicsSettings;

		public static MyStringId RunningGraphicsRenderer
		{
			get;
			private set;
		}

		public static bool GpuUnderMinimum
		{
			get;
			private set;
		}

		static MyVideoSettingsManager()
		{
			m_aspectRatios = new MyAspectRatio[MyUtils.GetMaxValueFromEnum<MyAspectRatioEnum>() + 1];
			Action<bool, MyAspectRatioEnum, float, string, bool> obj = delegate(bool isTripleHead, MyAspectRatioEnum aspectRatioEnum, float aspectRatioNumber, string textShort, bool isSupported)
			{
				m_aspectRatios[(int)aspectRatioEnum] = new MyAspectRatio(isTripleHead, aspectRatioEnum, aspectRatioNumber, textShort, isSupported);
			};
			obj(arg1: false, MyAspectRatioEnum.Normal_4_3, 1.33333337f, "4:3", arg5: true);
			obj(arg1: false, MyAspectRatioEnum.Normal_16_9, 1.77777779f, "16:9", arg5: true);
			obj(arg1: false, MyAspectRatioEnum.Normal_16_10, 1.6f, "16:10", arg5: true);
			obj(arg1: false, MyAspectRatioEnum.Dual_4_3, 2.66666675f, "Dual 4:3", arg5: true);
			obj(arg1: false, MyAspectRatioEnum.Dual_16_9, 3.55555558f, "Dual 16:9", arg5: true);
			obj(arg1: false, MyAspectRatioEnum.Dual_16_10, 3.2f, "Dual 16:10", arg5: true);
			obj(arg1: true, MyAspectRatioEnum.Triple_4_3, 4f, "Triple 4:3", arg5: true);
			obj(arg1: true, MyAspectRatioEnum.Triple_16_9, 5.33333349f, "Triple 16:9", arg5: true);
			obj(arg1: true, MyAspectRatioEnum.Triple_16_10, 4.8f, "Triple 16:10", arg5: true);
			obj(arg1: false, MyAspectRatioEnum.Unsupported_5_4, 1.25f, "5:4", arg5: false);
			DebugDisplayModes = new MyDisplayMode[0];
		}

		public static void UpdateRenderSettingsFromConfig(ref MyPerformanceSettings defaults, bool force = false)
		{
			Apply(GetGraphicsSettingsFromConfig(ref defaults, force));
		}

		public static MyGraphicsSettings GetGraphicsSettingsFromConfig(ref MyPerformanceSettings defaults, bool force)
		{
			MyGraphicsSettings currentGraphicsSettings = CurrentGraphicsSettings;
			MyConfig config = MySandboxGame.Config;
			currentGraphicsSettings.PerformanceSettings = defaults;
			currentGraphicsSettings.GraphicsRenderer = config.GraphicsRenderer;
			currentGraphicsSettings.FieldOfView = config.FieldOfView;
			currentGraphicsSettings.PostProcessingEnabled = config.PostProcessingEnabled;
			currentGraphicsSettings.FlaresIntensity = config.FlaresIntensity;
			if (!config.EnableDamageEffects.HasValue)
			{
				config.EnableDamageEffects = defaults.EnableDamageEffects;
			}
			currentGraphicsSettings.PerformanceSettings.EnableDamageEffects = config.EnableDamageEffects.Value;
			if (force)
			{
				currentGraphicsSettings.PerformanceSettings.RenderSettings = defaults.RenderSettings;
			}
			else
			{
				currentGraphicsSettings.PerformanceSettings.RenderSettings.DistanceFade = (config.VegetationDrawDistance ?? defaults.RenderSettings.DistanceFade);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.GrassDensityFactor = (config.GrassDensityFactor ?? defaults.RenderSettings.GrassDensityFactor);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.GrassDrawDistance = (config.GrassDrawDistance ?? defaults.RenderSettings.GrassDrawDistance);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.AntialiasingMode = (config.AntialiasingMode ?? defaults.RenderSettings.AntialiasingMode);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.ShadowQuality = (config.ShadowQuality ?? defaults.RenderSettings.ShadowQuality);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.AmbientOcclusionEnabled = (config.AmbientOcclusionEnabled ?? defaults.RenderSettings.AmbientOcclusionEnabled);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.TextureQuality = (config.TextureQuality ?? defaults.RenderSettings.TextureQuality);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.AnisotropicFiltering = (config.AnisotropicFiltering ?? defaults.RenderSettings.AnisotropicFiltering);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.ModelQuality = (config.ModelQuality ?? defaults.RenderSettings.ModelQuality);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality = (config.VoxelQuality ?? defaults.RenderSettings.VoxelQuality);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.HqDepth = true;
				currentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelShaderQuality = (config.ShaderQuality ?? defaults.RenderSettings.VoxelShaderQuality);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.AlphaMaskedShaderQuality = (config.ShaderQuality ?? defaults.RenderSettings.AlphaMaskedShaderQuality);
				currentGraphicsSettings.PerformanceSettings.RenderSettings.AtmosphereShaderQuality = (config.ShaderQuality ?? defaults.RenderSettings.AtmosphereShaderQuality);
			}
			return currentGraphicsSettings;
		}

		public static MyRenderDeviceSettings? Initialize()
		{
			MyRenderProxy.RequestVideoAdapters();
			MyConfig config = MySandboxGame.Config;
			RunningGraphicsRenderer = config.GraphicsRenderer;
			int? screenWidth = config.ScreenWidth;
			int? screenHeight = config.ScreenHeight;
			int? num = config.VideoAdapter;
			if (num.HasValue && screenWidth.HasValue && screenHeight.HasValue)
			{
				MyRenderDeviceSettings myRenderDeviceSettings = default(MyRenderDeviceSettings);
				myRenderDeviceSettings.AdapterOrdinal = num.Value;
				myRenderDeviceSettings.NewAdapterOrdinal = num.Value;
				myRenderDeviceSettings.BackBufferHeight = screenHeight.Value;
				myRenderDeviceSettings.BackBufferWidth = screenWidth.Value;
				myRenderDeviceSettings.RefreshRate = config.RefreshRate;
				myRenderDeviceSettings.VSync = config.VerticalSync;
				myRenderDeviceSettings.WindowMode = config.WindowMode;
				MyRenderDeviceSettings value = myRenderDeviceSettings;
				if (MyPerGameSettings.DefaultRenderDeviceSettings.HasValue)
				{
					value.UseStereoRendering = MyPerGameSettings.DefaultRenderDeviceSettings.Value.UseStereoRendering;
					value.SettingsMandatory = MyPerGameSettings.DefaultRenderDeviceSettings.Value.SettingsMandatory;
				}
				return value;
			}
			return MyPerGameSettings.DefaultRenderDeviceSettings;
		}

		public static ChangeResult Apply(MyRenderDeviceSettings settings)
		{
			MySandboxGame.Log.WriteLine("MyVideoModeManager.Apply(MyRenderDeviceSettings)");
			using (MySandboxGame.Log.IndentUsing())
			{
				MySandboxGame.Log.WriteLine("VideoAdapter: " + settings.AdapterOrdinal);
				MySandboxGame.Log.WriteLine("Width: " + settings.BackBufferWidth);
				MySandboxGame.Log.WriteLine("Height: " + settings.BackBufferHeight);
				MySandboxGame.Log.WriteLine("RefreshRate: " + settings.RefreshRate);
				MySandboxGame.Log.WriteLine("WindowMode: " + ((settings.WindowMode == MyWindowModeEnum.Fullscreen) ? "Fullscreen" : ((settings.WindowMode == MyWindowModeEnum.Window) ? "Window" : "Fullscreen window")));
				MySandboxGame.Log.WriteLine("VerticalSync: " + settings.VSync);
				if (settings.Equals(ref m_currentDeviceSettings) && settings.NewAdapterOrdinal == settings.AdapterOrdinal)
				{
					return ChangeResult.NothingChanged;
				}
				if (!IsSupportedDisplayMode(settings.AdapterOrdinal, settings.BackBufferWidth, settings.BackBufferHeight, settings.WindowMode))
				{
					return ChangeResult.Failed;
				}
				m_currentDeviceSettings = settings;
				m_currentDeviceSettings.VSync = settings.VSync;
				MySandboxGame.Static.SwitchSettings(m_currentDeviceSettings);
				float aspectRatio = (float)m_currentDeviceSettings.BackBufferWidth / (float)m_currentDeviceSettings.BackBufferHeight;
				m_currentDeviceIsTripleHead = GetAspectRatio(GetClosestAspectRatio(aspectRatio)).IsTripleHead;
				GetFovBounds(aspectRatio, out float minRadians, out float maxRadians);
				SetFov(MathHelper.Clamp(m_currentGraphicsSettings.FieldOfView, minRadians, maxRadians));
				SetPostProcessingEnabled(m_currentGraphicsSettings.PostProcessingEnabled);
			}
			return ChangeResult.Success;
		}

		private static void SetEnableDamageEffects(bool enableDamageEffects)
		{
			m_currentGraphicsSettings.PerformanceSettings.EnableDamageEffects = enableDamageEffects;
			MySandboxGame.Static.EnableDamageEffects = enableDamageEffects;
		}

		private static void SetHardwareCursor(bool useHardwareCursor)
		{
			MySandboxGame.Static.SetMouseVisible(IsHardwareCursorUsed());
			MyGuiSandbox.SetMouseCursorVisibility(IsHardwareCursorUsed(), changePosition: false);
		}

		public static ChangeResult Apply(MyGraphicsSettings settings)
		{
			MySandboxGame.Log.WriteLine("MyVideoModeManager.Apply(MyGraphicsSettings1)");
			using (MySandboxGame.Log.IndentUsing())
			{
				MySandboxGame.Log.WriteLine("Flares Intensity: " + settings.FlaresIntensity);
				MySandboxGame.Log.WriteLine("Field of view: " + settings.FieldOfView);
				MySandboxGame.Log.WriteLine("PostProcessingEnabled: " + settings.PostProcessingEnabled);
				MySandboxGame.Log.WriteLine("Render.GrassDensityFactor: " + settings.PerformanceSettings.RenderSettings.GrassDensityFactor);
				MySandboxGame.Log.WriteLine("Render.GrassDrawDistance: " + settings.PerformanceSettings.RenderSettings.GrassDrawDistance);
				MySandboxGame.Log.WriteLine("Render.DistanceFade: " + settings.PerformanceSettings.RenderSettings.DistanceFade);
				MySandboxGame.Log.WriteLine("Render.AntialiasingMode: " + settings.PerformanceSettings.RenderSettings.AntialiasingMode);
				MySandboxGame.Log.WriteLine("Render.ShadowQuality: " + settings.PerformanceSettings.RenderSettings.ShadowQuality);
				MySandboxGame.Log.WriteLine("Render.AmbientOcclusionEnabled: " + settings.PerformanceSettings.RenderSettings.AmbientOcclusionEnabled);
				MySandboxGame.Log.WriteLine("Render.TextureQuality: " + settings.PerformanceSettings.RenderSettings.TextureQuality);
				MySandboxGame.Log.WriteLine("Render.AnisotropicFiltering: " + settings.PerformanceSettings.RenderSettings.AnisotropicFiltering);
				MySandboxGame.Log.WriteLine("Render.VoxelShaderQuality: " + settings.PerformanceSettings.RenderSettings.VoxelShaderQuality);
				MySandboxGame.Log.WriteLine("Render.AlphaMaskedShaderQuality: " + settings.PerformanceSettings.RenderSettings.AlphaMaskedShaderQuality);
				MySandboxGame.Log.WriteLine("Render.AtmosphereShaderQuality: " + settings.PerformanceSettings.RenderSettings.AtmosphereShaderQuality);
				if (m_currentGraphicsSettings.Equals(ref settings))
				{
					return ChangeResult.NothingChanged;
				}
				SetEnableDamageEffects(settings.PerformanceSettings.EnableDamageEffects);
				SetFov(settings.FieldOfView);
				SetPostProcessingEnabled(settings.PostProcessingEnabled);
				if (MyRenderProxy.Settings.FlaresIntensity != settings.FlaresIntensity)
				{
					MyRenderProxy.Settings.FlaresIntensity = settings.FlaresIntensity;
					MyRenderProxy.SetSettingsDirty();
				}
				if (!m_currentGraphicsSettings.PerformanceSettings.RenderSettings.Equals(ref settings.PerformanceSettings.RenderSettings))
				{
					MyRenderProxy.SwitchRenderSettings(settings.PerformanceSettings.RenderSettings);
				}
				if (m_currentGraphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality != settings.PerformanceSettings.RenderSettings.VoxelQuality)
				{
					MyRenderComponentVoxelMap.SetLodQuality(settings.PerformanceSettings.RenderSettings.VoxelQuality);
				}
				m_currentGraphicsSettings = settings;
				MySector.Lodding.SelectQuality(settings.PerformanceSettings.RenderSettings.ModelQuality);
			}
			return ChangeResult.Success;
		}

		private static void SetFov(float fov)
		{
			if (m_currentGraphicsSettings.FieldOfView == fov)
			{
				return;
			}
			m_currentGraphicsSettings.FieldOfView = fov;
			if (MySector.MainCamera != null)
			{
				MySector.MainCamera.FieldOfView = fov;
				if (MySector.MainCamera.Zoom != null)
				{
					MySector.MainCamera.Zoom.Update(0.0166666675f);
				}
			}
		}

		private static void SetPostProcessingEnabled(bool enable)
		{
			if (m_currentGraphicsSettings.PostProcessingEnabled != enable)
			{
				m_currentGraphicsSettings.PostProcessingEnabled = enable;
			}
		}

		public static ChangeResult ApplyVideoSettings(MyRenderDeviceSettings deviceSettings, MyGraphicsSettings graphicsSettings)
		{
			ChangeResult changeResult = Apply(deviceSettings);
			if (changeResult == ChangeResult.Failed)
			{
				return changeResult;
			}
			ChangeResult result = Apply(graphicsSettings);
			if (changeResult != 0)
			{
				return result;
			}
			return changeResult;
		}

		private static bool IsSupportedDisplayMode(int videoAdapter, int width, int height, MyWindowModeEnum windowMode)
		{
			bool result = false;
			if (windowMode == MyWindowModeEnum.Fullscreen)
			{
				MyDisplayMode[] supportedDisplayModes = m_adapters[videoAdapter].SupportedDisplayModes;
				for (int i = 0; i < supportedDisplayModes.Length; i++)
				{
					MyDisplayMode myDisplayMode = supportedDisplayModes[i];
					if (myDisplayMode.Width == width && myDisplayMode.Height == height)
					{
						result = true;
					}
				}
			}
			else
			{
				result = true;
			}
			int maxTextureSize = m_adapters[videoAdapter].MaxTextureSize;
			if (width > maxTextureSize || height > maxTextureSize)
			{
				MySandboxGame.Log.WriteLine($"VideoMode {width}x{height} requires texture size which is not supported by this HW (this HW supports max {maxTextureSize})");
				result = false;
			}
			return result;
		}

		public static void LogApplicationInformation()
		{
			MySandboxGame.Log.WriteLine("MyVideoModeManager.LogApplicationInformation - START");
			MySandboxGame.Log.IncreaseIndent();
			try
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				MySandboxGame.Log.WriteLine("Assembly.GetName: " + executingAssembly.GetName().ToString());
				MySandboxGame.Log.WriteLine("Assembly.FullName: " + executingAssembly.FullName);
				MySandboxGame.Log.WriteLine("Assembly.Location: " + executingAssembly.Location);
				MySandboxGame.Log.WriteLine("Assembly.ImageRuntimeVersion: " + executingAssembly.ImageRuntimeVersion);
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine("Error occured during enumerating application information. Application will still continue. Detail description: " + ex.ToString());
			}
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("MyVideoModeManager.LogApplicationInformation - END");
		}

		public static bool IsTripleHead()
		{
			return m_currentDeviceIsTripleHead;
		}

		public static bool IsTripleHead(Vector2I screenSize)
		{
			return GetAspectRatio(GetClosestAspectRatio((float)screenSize.X / (float)screenSize.Y)).IsTripleHead;
		}

		public static bool IsHardwareCursorUsed()
		{
			if (MyExternalAppBase.Static != null)
			{
				return false;
			}
			OperatingSystem oSVersion = Environment.OSVersion;
			if (oSVersion.Platform == PlatformID.Win32NT && oSVersion.Version.Major == 6 && oSVersion.Version.Minor == 0)
			{
				return false;
			}
			if (oSVersion.Platform == PlatformID.Win32NT && oSVersion.Version.Major == 5 && oSVersion.Version.Minor == 1)
			{
				return false;
			}
			return true;
		}

		public static bool IsCurrentAdapterNvidia()
		{
			if (m_adapters.Length > m_currentDeviceSettings.AdapterOrdinal && m_currentDeviceSettings.AdapterOrdinal >= 0)
			{
				return m_adapters[m_currentDeviceSettings.AdapterOrdinal].VendorId == VendorIds.Nvidia;
			}
			return false;
		}

		public static MyAspectRatio GetAspectRatio(MyAspectRatioEnum aspectRatioEnum)
		{
			return m_aspectRatios[(int)aspectRatioEnum];
		}

		public static MyAspectRatio GetRecommendedAspectRatio(int adapterIndex)
		{
			return m_recommendedAspectRatio[adapterIndex];
		}

		public static MyAspectRatioEnum GetClosestAspectRatio(float aspectRatio)
		{
			MyAspectRatioEnum result = MyAspectRatioEnum.Normal_4_3;
			float num = float.MaxValue;
			for (int i = 0; i < m_aspectRatios.Length; i++)
			{
				float num2 = Math.Abs(aspectRatio - m_aspectRatios[i].AspectRatioNumber);
				if (num2 < num)
				{
					num = num2;
					result = m_aspectRatios[i].AspectRatioEnum;
				}
			}
			return result;
		}

		public static void GetFovBounds(out float minRadians, out float maxRadians)
		{
			GetFovBounds((float)m_currentDeviceSettings.BackBufferWidth / (float)m_currentDeviceSettings.BackBufferHeight, out minRadians, out maxRadians);
		}

		public static void GetFovBounds(float aspectRatio, out float minRadians, out float maxRadians)
		{
			minRadians = MyConstants.FIELD_OF_VIEW_CONFIG_MIN;
			if ((double)aspectRatio >= 4.0)
			{
				maxRadians = MyConstants.FIELD_OF_VIEW_CONFIG_MAX_TRIPLE_HEAD;
			}
			else if ((double)aspectRatio >= 2.6666666666666665)
			{
				maxRadians = MyConstants.FIELD_OF_VIEW_CONFIG_MAX_DUAL_HEAD;
			}
			else
			{
				maxRadians = MyConstants.FIELD_OF_VIEW_CONFIG_MAX;
			}
		}

		internal static void OnVideoAdaptersResponse(MyRenderMessageVideoAdaptersResponse message)
		{
			MyRenderProxy.Log.WriteLine("MyVideoSettingsManager.OnVideoAdaptersResponse");
			using (MyRenderProxy.Log.IndentUsing())
			{
				m_adapters = message.Adapters;
				int num = -1;
				MyAdapterInfo myAdapterInfo = default(MyAdapterInfo);
				myAdapterInfo.Priority = 1000;
				try
				{
					num = MySandboxGame.Static.GameRenderComponent.RenderThread.CurrentAdapter;
					myAdapterInfo = m_adapters[num];
					GpuUnderMinimum = !myAdapterInfo.Has512MBRam;
				}
				catch
				{
				}
				m_recommendedAspectRatio = new Dictionary<int, MyAspectRatio>();
				if (m_adapters.Length == 0)
				{
					MyRenderProxy.Log.WriteLine("ERROR: Adapters count is 0!");
				}
				for (int i = 0; i < m_adapters.Length; i++)
				{
					MyAdapterInfo myAdapterInfo2 = m_adapters[i];
					MyRenderProxy.Log.WriteLine($"Adapter {myAdapterInfo2}");
					using (MyRenderProxy.Log.IndentUsing())
					{
						float aspectRatio = (float)myAdapterInfo2.DesktopBounds.Width / (float)myAdapterInfo2.DesktopBounds.Height;
						m_recommendedAspectRatio.Add(i, GetAspectRatio(GetClosestAspectRatio(aspectRatio)));
						if (myAdapterInfo2.SupportedDisplayModes.Length == 0)
						{
							MyRenderProxy.Log.WriteLine($"WARNING: Adapter {i} count of supported display modes is 0!");
						}
						int maxTextureSize = myAdapterInfo2.MaxTextureSize;
						MyDisplayMode[] supportedDisplayModes = myAdapterInfo2.SupportedDisplayModes;
						for (int j = 0; j < supportedDisplayModes.Length; j++)
						{
							MyDisplayMode myDisplayMode = supportedDisplayModes[j];
							MyRenderProxy.Log.WriteLine(myDisplayMode.ToString());
							if (myDisplayMode.Width > maxTextureSize || myDisplayMode.Height > maxTextureSize)
							{
								MyRenderProxy.Log.WriteLine($"WARNING: Display mode {myDisplayMode} requires texture size which is not supported by this HW (this HW supports max {maxTextureSize})");
							}
						}
					}
					MySandboxGame.ShowIsBetterGCAvailableNotification |= (num != i && myAdapterInfo.Priority < myAdapterInfo2.Priority);
				}
			}
		}

		internal static void OnCreatedDeviceSettings(MyRenderMessageCreatedDeviceSettings message)
		{
			m_currentDeviceSettings = message.Settings;
			m_currentDeviceSettings.NewAdapterOrdinal = m_currentDeviceSettings.AdapterOrdinal;
			m_currentDeviceIsTripleHead = GetAspectRatio(GetClosestAspectRatio((float)m_currentDeviceSettings.BackBufferWidth / (float)m_currentDeviceSettings.BackBufferHeight)).IsTripleHead;
		}

		public static void WriteCurrentSettingsToConfig()
		{
			MyConfig config = MySandboxGame.Config;
			config.VideoAdapter = m_currentDeviceSettings.NewAdapterOrdinal;
			config.ScreenWidth = m_currentDeviceSettings.BackBufferWidth;
			config.ScreenHeight = m_currentDeviceSettings.BackBufferHeight;
			config.RefreshRate = m_currentDeviceSettings.RefreshRate;
			config.WindowMode = m_currentDeviceSettings.WindowMode;
			config.VerticalSync = m_currentDeviceSettings.VSync;
			config.FieldOfView = m_currentGraphicsSettings.FieldOfView;
			config.PostProcessingEnabled = m_currentGraphicsSettings.PostProcessingEnabled;
			config.FlaresIntensity = m_currentGraphicsSettings.FlaresIntensity;
			config.GraphicsRenderer = m_currentGraphicsSettings.GraphicsRenderer;
			config.EnableDamageEffects = m_currentGraphicsSettings.PerformanceSettings.EnableDamageEffects;
			MyRenderSettings1 renderSettings = m_currentGraphicsSettings.PerformanceSettings.RenderSettings;
			config.VegetationDrawDistance = renderSettings.DistanceFade;
			config.GrassDensityFactor = renderSettings.GrassDensityFactor;
			config.GrassDrawDistance = renderSettings.GrassDrawDistance;
			config.AntialiasingMode = renderSettings.AntialiasingMode;
			config.ShadowQuality = renderSettings.ShadowQuality;
			config.AmbientOcclusionEnabled = renderSettings.AmbientOcclusionEnabled;
			config.TextureQuality = renderSettings.TextureQuality;
			config.AnisotropicFiltering = renderSettings.AnisotropicFiltering;
			config.ModelQuality = renderSettings.ModelQuality;
			config.VoxelQuality = renderSettings.VoxelQuality;
			config.ShaderQuality = renderSettings.VoxelShaderQuality;
			config.LowMemSwitchToLow = MyConfig.LowMemSwitch.ARMED;
			MySandboxGame.Static.UpdateMouseCapture();
		}

		public static void SaveCurrentSettings()
		{
			WriteCurrentSettingsToConfig();
			MySandboxGame.Config.Save();
		}
	}
}
