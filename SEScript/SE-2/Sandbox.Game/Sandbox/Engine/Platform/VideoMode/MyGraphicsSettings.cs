using VRage.Utils;

namespace Sandbox.Engine.Platform.VideoMode
{
	public struct MyGraphicsSettings
	{
		public MyPerformanceSettings PerformanceSettings;

		public float FieldOfView;

		public bool PostProcessingEnabled;

		public MyStringId GraphicsRenderer;

		public float FlaresIntensity;

		public override bool Equals(object other)
		{
			MyGraphicsSettings other2 = (MyGraphicsSettings)other;
			return Equals(ref other2);
		}

		public bool Equals(ref MyGraphicsSettings other)
		{
			if (FieldOfView == other.FieldOfView && PostProcessingEnabled == other.PostProcessingEnabled && FlaresIntensity == other.FlaresIntensity && GraphicsRenderer == other.GraphicsRenderer)
			{
				return PerformanceSettings.Equals(other.PerformanceSettings);
			}
			return false;
		}
	}
}
