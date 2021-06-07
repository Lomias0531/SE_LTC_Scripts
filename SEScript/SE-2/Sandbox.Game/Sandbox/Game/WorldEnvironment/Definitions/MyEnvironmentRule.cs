using VRageMath;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
	public class MyEnvironmentRule
	{
		public SerializableRange Height = new SerializableRange(0f, 1f);

		public SymmetricSerializableRange Latitude = new SymmetricSerializableRange(-90f, 90f);

		public SerializableRange Longitude = new SerializableRange(-180f, 180f);

		public SerializableRange Slope = new SerializableRange(0f, 90f);

		public void ConvertRanges()
		{
			Latitude.ConvertToSine();
			Longitude.ConvertToCosineLongitude();
			Slope.ConvertToCosine();
		}

		public bool Check(float height, float latitude, float longitude, float slope)
		{
			if (Height.ValueBetween(height) && Latitude.ValueBetween(latitude) && Longitude.ValueBetween(longitude))
			{
				return Slope.ValueBetween(slope);
			}
			return false;
		}
	}
}
