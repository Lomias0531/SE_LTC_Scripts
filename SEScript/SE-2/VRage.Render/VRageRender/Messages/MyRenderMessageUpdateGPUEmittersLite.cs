using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateGPUEmittersLite : MyRenderMessageBase
	{
		public List<MyGPUEmitterLite> Emitters = new List<MyGPUEmitterLite>();

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateGPUEmittersLite;

		public override void Close()
		{
			base.Close();
			Emitters.Clear();
		}
	}
}
