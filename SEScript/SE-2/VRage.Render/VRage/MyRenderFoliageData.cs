using VRageMath;

namespace VRage
{
	public struct MyRenderFoliageData
	{
		public struct FoliageEntry
		{
			/// <summary>
			/// Absolute size of the blade.
			/// </summary>
			public Vector2 Size;

			/// <summary>
			/// Random variation ratio.
			/// </summary>
			public float SizeVariation;

			/// <summary>
			/// Color metal texture for this entry.
			/// </summary>
			public string ColorAlphaTexture;

			/// <summary>
			/// Normal texture for this entry.
			/// </summary>
			public string NormalGlossTexture;

			/// <summary>
			/// Probability of this blade being displayed.
			/// </summary>
			public float Probability;
		}

		/// <summary>
		/// Density of the foliage in blades per sq metre.
		/// </summary>
		public float Density;

		/// <summary>
		/// Type of the foliage blade to generate.
		/// </summary>
		public MyFoliageType Type;

		/// <summary>
		/// The possible grass blades.
		/// </summary>
		public FoliageEntry[] Entries;
	}
}
