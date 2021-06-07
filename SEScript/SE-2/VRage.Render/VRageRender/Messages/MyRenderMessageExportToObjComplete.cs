namespace VRageRender.Messages
{
	public class MyRenderMessageExportToObjComplete : MyRenderMessageBase
	{
		public bool Success;

		public string Filename;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.ExportToObjComplete;
	}
}
