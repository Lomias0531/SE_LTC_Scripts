namespace VRageRender.Messages
{
	public class MyRenderMessageResetRandomness : MyRenderMessageBase
	{
		public int? Seed;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.ResetRandomness;
	}
}
