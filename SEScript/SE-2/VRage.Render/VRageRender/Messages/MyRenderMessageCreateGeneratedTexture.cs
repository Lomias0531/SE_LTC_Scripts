namespace VRageRender.Messages
{
	public class MyRenderMessageCreateGeneratedTexture : MyRenderMessageBase
	{
		public string TextureName;

		public int Width;

		public int Height;

		public MyGeneratedTextureType Type;

		public byte[] Data;

		public bool GenerateMipMaps;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.CreateGeneratedTexture;
	}
}
