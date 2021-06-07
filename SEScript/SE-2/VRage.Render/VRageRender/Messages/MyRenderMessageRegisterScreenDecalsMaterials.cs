using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageRegisterScreenDecalsMaterials : MyRenderMessageBase
	{
		public Dictionary<string, List<MyDecalMaterialDesc>> MaterialDescriptions;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.RegisterDecalsMaterials;
	}
}
