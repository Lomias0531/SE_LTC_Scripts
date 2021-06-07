using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageSpriteScissorPush : MySpriteDrawRenderMessage
	{
		public Rectangle ScreenRectangle;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.SpriteScissorPush;
	}
}
