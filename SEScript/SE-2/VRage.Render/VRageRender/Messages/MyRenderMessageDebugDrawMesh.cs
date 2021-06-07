using System.Collections.Generic;
using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDebugDrawMesh : MyRenderMessageBase
	{
		public uint ID;

		public MatrixD WorldMatrix;

		public bool DepthRead;

		public bool Shaded;

		public List<MyFormatPositionColor> Vertices;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DebugDrawMesh;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.DebugDraw;

		public override void Close()
		{
			base.Close();
			Vertices = null;
		}
	}
}
