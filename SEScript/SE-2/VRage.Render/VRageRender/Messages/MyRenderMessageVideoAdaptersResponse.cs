namespace VRageRender.Messages
{
	public class MyRenderMessageVideoAdaptersResponse : MyRenderMessageBase
	{
		public MyAdapterInfo[] Adapters;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.VideoAdaptersResponse;
	}
}
