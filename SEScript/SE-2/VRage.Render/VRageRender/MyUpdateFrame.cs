using System.Collections.Generic;
using VRage.Library.Threading;
using VRage.Library.Utils;
using VRage.Network;
using VRageRender.Messages;

namespace VRageRender
{
	/// <summary>
	/// Contains data produced by update frame, sent to render in thread-safe manner
	/// </summary>
	[GenerateActivator]
	public class MyUpdateFrame
	{
		public bool Processed;

		public MyTimeSpan UpdateTimestamp;

		public readonly List<MyRenderMessageBase> RenderInput = new List<MyRenderMessageBase>(2048);

		private readonly SpinLockRef m_lock = new SpinLockRef();

		public void Enqueue(MyRenderMessageBase message)
		{
			using (m_lock.Acquire())
			{
				RenderInput.Add(message);
			}
		}
	}
}
