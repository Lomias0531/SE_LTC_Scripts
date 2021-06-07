using VRageMath;

namespace VRageRender
{
	public struct MyEnvironmentData
	{
		public MyEnvironmentLightData EnvironmentLight;

		public string Skybox;

		public Quaternion SkyboxOrientation;

		public MyEnvironmentProbeData EnvironmentProbe;

		public int EnvMapResolution;

		public int EnvMapFilteredResolution;

		public MyTextureDebugMultipliers TextureMultipliers;
	}
}
