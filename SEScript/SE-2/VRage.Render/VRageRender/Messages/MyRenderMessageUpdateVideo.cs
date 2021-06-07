namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateVideo : MyRenderMessageBase
	{
		public uint ID;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.Draw;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateVideo;
	}
}
