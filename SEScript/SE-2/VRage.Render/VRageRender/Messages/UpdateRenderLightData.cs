using VRageMath;

namespace VRageRender.Messages
{
	public struct UpdateRenderLightData
	{
		public uint ID;

		public Vector3D Position;

		public bool CastShadows;

		public bool PointLightOn;

		public MyLightLayout PointLight;

		public float PointIntensity;

		public float PointOffset;

		public bool SpotLightOn;

		public MySpotLightLayout SpotLight;

		public float SpotIntensity;

		public string ReflectorTexture;

		public MyFlareDesc Glare;

		public bool PositionChanged;

		public bool AabbChanged;
	}
}
