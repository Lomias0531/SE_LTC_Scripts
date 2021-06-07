namespace VRageRender.Messages
{
	public class MyRenderMessagePlayVideo : MyRenderMessageBase
	{
		public uint ID;

		public string VideoFile;

		public float Volume;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.PlayVideo;
	}
}
