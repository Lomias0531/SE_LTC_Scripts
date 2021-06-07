namespace VRageRender.Messages
{
	public class MyRenderMessageUpdatePostprocessSettings : MyRenderMessageBase
	{
		public MyPostprocessSettings Settings;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdatePostprocessSettings;
	}
}
