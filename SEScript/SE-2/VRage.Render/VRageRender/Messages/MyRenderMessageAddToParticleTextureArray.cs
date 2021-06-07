using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageAddToParticleTextureArray : MyRenderMessageBase
	{
		public HashSet<string> Files;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.AddToParticleTextureArray;
	}
}
