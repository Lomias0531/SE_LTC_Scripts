using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDrawSpriteExt : MySpriteDrawRenderMessage
	{
		public string Texture;

		public Color Color;

		public Rectangle? SourceRectangle;

		public RectangleF DestinationRectangle;

		public bool WaitTillLoaded;

		public string MaskTexture;

		public Vector2 RightVector;

		public Vector2 Origin;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DrawSpriteExt;
	}
}
