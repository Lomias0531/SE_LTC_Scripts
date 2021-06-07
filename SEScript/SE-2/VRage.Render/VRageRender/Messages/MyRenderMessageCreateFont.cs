using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageCreateFont : MyRenderMessageBase
	{
		public int FontId;

		public string FontPath;

		public Color? ColorMask;

		public bool IsDebugFont;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateFont;
	}
}
