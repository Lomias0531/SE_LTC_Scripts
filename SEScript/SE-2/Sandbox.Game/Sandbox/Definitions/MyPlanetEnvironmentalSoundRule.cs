using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
	public struct MyPlanetEnvironmentalSoundRule
	{
		public SymmetricSerializableRange Latitude;

		public SerializableRange Height;

		public SerializableRange SunAngleFromZenith;

		public MyStringHash EnvironmentSound;

		public bool Check(float angleFromEquator, float height, float sunAngleFromZenith)
		{
			if (Latitude.ValueBetween(angleFromEquator) && Height.ValueBetween(height))
			{
				return SunAngleFromZenith.ValueBetween(sunAngleFromZenith);
			}
			return false;
		}
	}
}
