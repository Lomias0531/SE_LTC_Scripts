using VRage;

namespace VRageRender.Messages
{
	/// This is kinda your universal array operator splice().
	///
	/// The only thing is we cannot move elements with this.
	public class MyRenderMessageUpdateRenderInstanceBufferRange : MyRenderMessageBase
	{
		public uint ID;

		public MyInstanceData[] InstanceData;

		public int StartOffset;

		public bool Trim;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateRenderInstanceBufferRange;
	}
}
