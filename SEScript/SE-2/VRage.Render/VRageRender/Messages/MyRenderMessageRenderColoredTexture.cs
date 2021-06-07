using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageRenderColoredTexture : MyRenderMessageBase
	{
		public List<renderColoredTextureProperties> texturesToRender;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.RenderColoredTexture;
	}
}
