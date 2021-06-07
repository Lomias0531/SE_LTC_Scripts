namespace VRageRender.Messages
{
	public class MyRenderMessageCreatedDeviceSettings : MyRenderMessageBase
	{
		public MyRenderDeviceSettings Settings;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreatedDeviceSettings;
	}
}
