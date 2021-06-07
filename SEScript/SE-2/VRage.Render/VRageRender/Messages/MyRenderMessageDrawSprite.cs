using VRageMath;

namespace VRageRender.Messages
{
	public class MyRenderMessageDrawSprite : MySpriteDrawRenderMessage
	{
		public string Texture;

		public Color Color;

		public Rectangle? SourceRectangle;

		public RectangleF DestinationRectangle;

		public bool WaitTillLoaded;

		public string MaskTexture;

		public float RotationSpeed;

		public float Rotation;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.DrawSprite;
	}
}
