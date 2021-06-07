using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageChangeMaterialTexture : MyRenderMessageBase
	{
		public uint RenderObjectID;

		public Dictionary<string, MyTextureChange> Changes;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.ChangeMaterialTexture;

		public override void Close()
		{
			Changes = null;
		}
	}
}
