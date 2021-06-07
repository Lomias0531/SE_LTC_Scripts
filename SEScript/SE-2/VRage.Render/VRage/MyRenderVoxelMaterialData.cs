using VRageMath;

namespace VRage
{
	public struct MyRenderVoxelMaterialData
	{
		public struct TextureSet
		{
			public string ColorMetalXZnY;

			public string ColorMetalY;

			public string NormalGlossXZnY;

			public string NormalGlossY;

			public string ExtXZnY;

			public string ExtY;

			public void Check()
			{
				if (ColorMetalY == null)
				{
					ColorMetalY = ColorMetalXZnY;
				}
				if (NormalGlossY == null)
				{
					NormalGlossY = NormalGlossXZnY;
				}
				if (ExtY == null)
				{
					ExtY = ExtXZnY;
				}
			}
		}

		public struct TilingSetup
		{
			public Vector4 DistanceAndScale;

			public Vector3 DistanceAndScaleFar;

			public float TilingScale;

			public Vector3 DistanceAndScaleFar2;

			public float _padding1;

			public Vector3 DistanceAndScaleFar3;

			public float ExtensionDetailScale;

			public float InitialScale
			{
				get
				{
					return DistanceAndScale.X;
				}
				set
				{
					DistanceAndScale.X = value;
				}
			}

			public float InitialDistance
			{
				get
				{
					return DistanceAndScale.Y;
				}
				set
				{
					DistanceAndScale.Y = value;
				}
			}

			public float ScaleMultiplier
			{
				get
				{
					return DistanceAndScale.Z;
				}
				set
				{
					DistanceAndScale.Z = value;
				}
			}

			public float DistanceMultiplier
			{
				get
				{
					return DistanceAndScale.W;
				}
				set
				{
					DistanceAndScale.W = value;
				}
			}

			public float Far1Scale
			{
				get
				{
					return DistanceAndScaleFar.X;
				}
				set
				{
					DistanceAndScaleFar.X = value;
					DistanceAndScaleFar.Z = 1f;
				}
			}

			public float Far1Distance
			{
				get
				{
					return DistanceAndScaleFar.Y;
				}
				set
				{
					DistanceAndScaleFar.Y = value;
				}
			}

			public float Far2Scale
			{
				get
				{
					return DistanceAndScaleFar2.X;
				}
				set
				{
					DistanceAndScaleFar2.X = value;
					DistanceAndScaleFar2.Z = 2f;
				}
			}

			public float Far2Distance
			{
				get
				{
					return DistanceAndScaleFar2.Y;
				}
				set
				{
					DistanceAndScaleFar2.Y = value;
				}
			}

			public float Far3Scale
			{
				get
				{
					return DistanceAndScaleFar3.X;
				}
				set
				{
					DistanceAndScaleFar3.X = value;
					DistanceAndScaleFar3.Z = 3f;
				}
			}

			public float Far3Distance
			{
				get
				{
					return DistanceAndScaleFar3.Y;
				}
				set
				{
					DistanceAndScaleFar3.Y = value;
				}
			}
		}

		public byte Index;

		public TextureSet[] TextureSets;

		public Vector4 Far3Color;

		public TilingSetup StandardTilingSetup;

		public TilingSetup SimpleTilingSetup;

		public MyRenderFoliageData? Foliage;
	}
}
