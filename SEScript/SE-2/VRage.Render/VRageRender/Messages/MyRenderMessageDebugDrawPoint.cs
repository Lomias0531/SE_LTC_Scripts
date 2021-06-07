using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDebugDrawPoint : MyDebugRenderMessage
	{
		public Vector3D Position;

		public Color Color;

		public bool DepthRead;

		public float? ClipDistance;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DebugDrawPoint;
	}
}
