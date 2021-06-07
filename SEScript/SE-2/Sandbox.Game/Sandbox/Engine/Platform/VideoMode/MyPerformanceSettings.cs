using VRageRender;

namespace Sandbox.Engine.Platform.VideoMode
{
	public struct MyPerformanceSettings
	{
		public MyRenderSettings1 RenderSettings;

		public bool EnableDamageEffects;

		public override bool Equals(object other)
		{
			MyPerformanceSettings other2 = (MyPerformanceSettings)other;
			return Equals(ref other2);
		}

		private bool Equals(ref MyPerformanceSettings other)
		{
			if (EnableDamageEffects == other.EnableDamageEffects)
			{
				return RenderSettings.Equals(ref other.RenderSettings);
			}
			return false;
		}
	}
}
