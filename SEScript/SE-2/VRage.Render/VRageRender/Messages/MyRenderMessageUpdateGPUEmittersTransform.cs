using System.Collections.Generic;

namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateGPUEmittersTransform : MyRenderMessageBase
	{
		public List<MyGPUEmitterTransformUpdate> Emitters = new List<MyGPUEmitterTransformUpdate>();

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateGPUEmittersTransform;

		public override void Close()
		{
			base.Close();
			Emitters.Clear();
		}
	}
}
