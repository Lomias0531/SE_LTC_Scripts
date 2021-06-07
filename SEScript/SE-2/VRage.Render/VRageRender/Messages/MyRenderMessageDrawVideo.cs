using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDrawVideo : MyRenderMessageBase
	{
		public uint ID;

		public Rectangle Rectangle;

		public Color Color;

		public MyVideoRectangleFitMode FitMode;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.Draw;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DrawVideo;
	}
}
