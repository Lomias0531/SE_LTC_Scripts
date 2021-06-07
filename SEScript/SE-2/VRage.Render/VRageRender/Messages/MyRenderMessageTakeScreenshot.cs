using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageTakeScreenshot : MyRenderMessageBase
	{
		public bool IgnoreSprites;

		public bool Debug;

		public Vector2 SizeMultiplier;

		public string PathToSave;

		public bool ShowNotification;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.TakeScreenshot;
	}
}
