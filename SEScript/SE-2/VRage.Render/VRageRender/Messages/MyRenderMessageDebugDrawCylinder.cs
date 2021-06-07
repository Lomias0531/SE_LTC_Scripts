using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDebugDrawCylinder : MyDebugRenderMessage
	{
		public MatrixD Matrix;

		public Color Color;

		public float Alpha;

		public bool DepthRead;

		public bool Smooth;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DebugDrawCylinder;
	}
}
