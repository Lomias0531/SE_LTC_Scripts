using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessagePreloadTextures : MyRenderMessageBase
	{
		public List<string> Files;

		public TextureType TextureType
		{
			get;
			set;
		}

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.PreloadTextures;

		public override void Close()
		{
			base.Close();
			Files.Clear();
		}
	}
}
