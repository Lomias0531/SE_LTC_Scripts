using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateGPUEmitters : MyRenderMessageBase
	{
		public List<MyGPUEmitter> Emitters = new List<MyGPUEmitter>();

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateGPUEmitters;

		public override void Close()
		{
			base.Close();
			Emitters.Clear();
		}
	}
}
