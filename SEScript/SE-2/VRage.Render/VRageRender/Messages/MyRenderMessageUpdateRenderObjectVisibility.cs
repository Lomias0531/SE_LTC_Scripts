namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateRenderObjectVisibility : MyRenderMessageBase
	{
		public uint ID;

		public bool Visible;

		public bool NearFlag;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateRenderObjectVisibility;
	}
}
