using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessagePreloadModels : MyRenderMessageBase
	{
		public List<string> Models;

		public bool ForInstancedComponent;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.PreloadModels;

		public override void Close()
		{
			base.Close();
			Models.Clear();
		}
	}
}
