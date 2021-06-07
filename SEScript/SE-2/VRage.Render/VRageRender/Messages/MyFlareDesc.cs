using VRageMath;
using VRageRender.Lights;

namespace VRageRender.Messages
{
	public struct MyFlareDesc
	{
		public bool Enabled;

		public MyGlareTypeEnum Type;

		public float MaxDistance;

		public float QuerySize;

		public float QueryShift;

		public float QueryFreqMinMs;

		public float QueryFreqRndMs;

		public float Intensity;

		public Vector2 SizeMultiplier;

		public MySubGlare[] Glares;
	}
}
