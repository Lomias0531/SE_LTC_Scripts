using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDebugDrawLine3D : MyDebugRenderMessage
	{
		public Vector3D PointFrom;

		public Vector3D PointTo;

		public Color ColorFrom;

		public Color ColorTo;

		public bool DepthRead;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DebugDrawLine3D;
	}
}
