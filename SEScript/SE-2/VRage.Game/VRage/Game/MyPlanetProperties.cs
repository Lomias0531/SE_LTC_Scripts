namespace VRage.Game
{
	public struct MyPlanetProperties
	{
		private static class Defaults
		{
			public const float AtmosphereIntensityMultiplier = 35f;

			public const float AtmosphereIntensityAmbientMultiplier = 35f;

			public const float AtmosphereDesaturationFactorForward = 0.5f;

			public const float CloudsIntensityMultiplier = 40f;
		}

		[StructDefault]
		public static readonly MyPlanetProperties Default;

		public float AtmosphereIntensityMultiplier;

		public float AtmosphereIntensityAmbientMultiplier;

		public float AtmosphereDesaturationFactorForward;

		public float CloudsIntensityMultiplier;

		static MyPlanetProperties()
		{
			Default = new MyPlanetProperties
			{
				AtmosphereIntensityMultiplier = 35f,
				AtmosphereIntensityAmbientMultiplier = 35f,
				AtmosphereDesaturationFactorForward = 0.5f,
				CloudsIntensityMultiplier = 40f
			};
		}
	}
}
