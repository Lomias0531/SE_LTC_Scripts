using VRageMath;

namespace Sandbox.Game.World
{
	internal class MyGodRaysProperties
	{
		public bool Enabled;

		public float Density = 0.34f;

		public float Weight = 1.27f;

		public float Decay = 0.97f;

		public float Exposition = 0.077f;

		public MyGodRaysProperties InterpolateWith(MyGodRaysProperties otherProperties, float interpolator)
		{
			return new MyGodRaysProperties
			{
				Density = MathHelper.Lerp(Density, otherProperties.Density, interpolator),
				Weight = MathHelper.Lerp(Weight, otherProperties.Weight, interpolator),
				Decay = MathHelper.Lerp(Decay, otherProperties.Decay, interpolator),
				Exposition = MathHelper.Lerp(Exposition, otherProperties.Exposition, interpolator),
				Enabled = (MathHelper.Lerp(Enabled ? 1 : 0, otherProperties.Enabled ? 1 : 0, interpolator) > 0.5f)
			};
		}
	}
}
