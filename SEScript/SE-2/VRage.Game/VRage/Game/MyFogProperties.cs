using VRageMath;

namespace VRage.Game
{
	public struct MyFogProperties
	{
		private static class Defaults
		{
			public static readonly Vector3 FogColor = new Vector3(0f, 0f, 0f);

			public const float FogMultiplier = 0.13f;

			public const float FogDensity = 0.003f;
		}

		[StructDefault]
		public static readonly MyFogProperties Default;

		public float FogMultiplier;

		public float FogDensity;

		public Vector3 FogColor;

		static MyFogProperties()
		{
			Default = new MyFogProperties
			{
				FogMultiplier = 0.13f,
				FogDensity = 0.003f,
				FogColor = Defaults.FogColor
			};
		}
	}
}
