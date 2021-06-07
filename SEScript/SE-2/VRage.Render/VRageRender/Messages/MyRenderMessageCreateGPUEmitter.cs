namespace VRageRender.Messages
{
	public class MyRenderMessageCreateGPUEmitter : MyRenderMessageBase
	{
		public uint ID;

		public string DebugName;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateGPUEmitter;
	}
}
