namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateLodImmediately : MyRenderMessageBase
	{
		public uint Id;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateLodImmediately;
	}
}
