using VRage.Profiler;

namespace VRageRender.Messages
{
	public class MyRenderMessageRenderProfiler : MyRenderMessageBase
	{
		public RenderProfilerCommand Command;

		public int Index;

		public string Value;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.RenderProfiler;
	}
}
