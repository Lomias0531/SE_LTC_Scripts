using System;
using System.Diagnostics;
using VRageMath;

namespace VRageRender
{
	[DebuggerDisplay("DeviceName: '{Name}', Description: '{Description}'")]
	public struct MyAdapterInfo
	{
		public string Name;

		public string DeviceName;

		public MyDisplayMode[] SupportedDisplayModes;

		public string OutputName;

		public string Description;

		public int AdapterDeviceId;

		public int OutputId;

		public Rectangle DesktopBounds;

		public Vector2I DesktopResolution;

		public int MaxTextureSize;

		public bool Has512MBRam;

		public bool IsDx11Supported;

		public int Priority;

		public bool IsOutputAttached;

		public ulong VRAM;

		public ulong SVRAM;

		public bool MultithreadedRenderingSupported;

		public VendorIds VendorId;

		public int DeviceId;

		public string DriverVersion;

		public string DriverDate;

		public bool DriverUpdateNecessary;

		public string DriverUpdateLink;

		public bool IsNvidiaNotebookGpu;

		public bool AftermathSupported;

		public MyRenderQualityEnum Quality;

		public bool Mobile;

		public bool ParallelVertexBufferMapping;

		public void LogInfo(Action<string> lineWriter)
		{
			lineWriter("Adapter: " + Name);
			lineWriter("VendorId: " + VendorId);
			lineWriter("DeviceId: " + DeviceId);
			lineWriter("Description: " + Description);
		}

		public override string ToString()
		{
			return $"DeviceName: '{Name}', Description: '{Description}'";
		}

		public MyDisplayMode? GetDisplayMode(int width, int height, int refreshRate)
		{
			MyDisplayMode[] supportedDisplayModes = SupportedDisplayModes;
			for (int i = 0; i < supportedDisplayModes.Length; i++)
			{
				MyDisplayMode value = supportedDisplayModes[i];
				if (value.Width == width && value.Height == height && (refreshRate == 0 || Math.Abs(value.RefreshRateF - (float)refreshRate / 1000f) < 0.1f))
				{
					return value;
				}
			}
			return null;
		}
	}
}
