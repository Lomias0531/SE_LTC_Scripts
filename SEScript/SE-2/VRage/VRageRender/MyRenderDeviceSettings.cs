using System;
using VRage;

namespace VRageRender
{
	public struct MyRenderDeviceSettings : IEquatable<MyRenderDeviceSettings>
	{
		public int AdapterOrdinal;

		public int NewAdapterOrdinal;

		public bool DisableWindowedModeForOldDriver;

		public MyWindowModeEnum WindowMode;

		public int BackBufferWidth;

		public int BackBufferHeight;

		public int RefreshRate;

		public int VSync;

		public bool DebugDrawOnly;

		public bool UseStereoRendering;

		public bool SettingsMandatory;

		public bool InitParallel;

		public MyRenderDeviceSettings(int adapter, MyWindowModeEnum windowMode, int width, int height, int refreshRate, int vsync, bool useStereoRendering, bool settingsMandatory, bool initParallel = true)
		{
			AdapterOrdinal = adapter;
			NewAdapterOrdinal = adapter;
			DisableWindowedModeForOldDriver = false;
			WindowMode = windowMode;
			BackBufferWidth = width;
			BackBufferHeight = height;
			RefreshRate = refreshRate;
			VSync = vsync;
			UseStereoRendering = useStereoRendering;
			SettingsMandatory = settingsMandatory;
			InitParallel = initParallel;
			DebugDrawOnly = false;
		}

		bool IEquatable<MyRenderDeviceSettings>.Equals(MyRenderDeviceSettings other)
		{
			return Equals(ref other);
		}

		public bool Equals(ref MyRenderDeviceSettings other)
		{
			if (AdapterOrdinal == other.AdapterOrdinal && WindowMode == other.WindowMode && BackBufferWidth == other.BackBufferWidth && BackBufferHeight == other.BackBufferHeight && RefreshRate == other.RefreshRate && VSync == other.VSync && UseStereoRendering == other.UseStereoRendering)
			{
				return SettingsMandatory == other.SettingsMandatory;
			}
			return false;
		}

		public override string ToString()
		{
			string text = "MyRenderDeviceSettings: {\n";
			text = text + "AdapterOrdinal: " + AdapterOrdinal + "\n";
			text = text + "NewAdapterOrdinal: " + NewAdapterOrdinal + "\n";
			text = string.Concat(text, "WindowMode: ", WindowMode, "\n");
			text = text + "BackBufferWidth: " + BackBufferWidth + "\n";
			text = text + "BackBufferHeight: " + BackBufferHeight + "\n";
			text = text + "RefreshRate: " + RefreshRate + "\n";
			text = text + "VSync: " + VSync + "\n";
			text = text + "DebugDrawOnly: " + DebugDrawOnly + "\n";
			text = text + "UseStereoRendering: " + UseStereoRendering + "\n";
			text = text + "SettingsMandatory: " + SettingsMandatory + "\n";
			return text + "}";
		}
	}
}
