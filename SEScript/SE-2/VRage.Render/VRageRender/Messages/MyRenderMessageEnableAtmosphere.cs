namespace VRageRender.Messages
{
	public class MyRenderMessageEnableAtmosphere : MyRenderMessageBase
	{
		public bool Enabled;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.EnableAtmosphere;
	}
}
