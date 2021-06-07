using System;

namespace VRageRender.Messages
{
	public class MyRenderMessageMainThreadCallback : MyRenderMessageBase
	{
		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.MainThreadCallback;

		public Action Callback
		{
			get;
			set;
		}
	}
}
