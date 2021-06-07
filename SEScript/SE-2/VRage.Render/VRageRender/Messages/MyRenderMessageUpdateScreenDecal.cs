using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateScreenDecal : MyRenderMessageBase
	{
		public List<MyDecalPositionUpdate> Decals = new List<MyDecalPositionUpdate>();

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeEvery;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateScreenDecal;

		public override void Init()
		{
			Decals.Clear();
		}
	}
}
