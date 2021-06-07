using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDebugDrawFrustrum : MyDebugRenderMessage
	{
		public BoundingFrustumD Frustum;

		public Color Color;

		public float Alpha;

		public bool DepthRead;

		public bool Smooth;

		public bool Cull;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DebugDrawFrustrum;
	}
}
