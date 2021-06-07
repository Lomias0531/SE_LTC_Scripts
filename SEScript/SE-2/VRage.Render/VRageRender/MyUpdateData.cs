using System.Threading;
using VRage.Collections;
using VRage.Library.Threading;
using VRage.Library.Utils;

namespace VRageRender
{
	internal class MyUpdateData
	{
		private bool MY_FAKE__ENABLE_FRAMES_OVERFLOW_BARRIER = true;

		private const int OVERFLOW_THRESHOLD = 1000;

		private ManualResetEvent m_overflowGate = new ManualResetEvent(initialState: true);

		private MyConcurrentPool<MyUpdateFrame> m_frameDataPool;

		private MyConcurrentQueue<MyUpdateFrame> m_updateDataQueue;

		public MyUpdateFrame CurrentUpdateFrame
		{
			get;
			private set;
		}

		public MyUpdateData()
		{
			m_frameDataPool = new MyConcurrentPool<MyUpdateFrame>(5);
			m_updateDataQueue = new MyConcurrentQueue<MyUpdateFrame>(5);
			CurrentUpdateFrame = m_frameDataPool.Get();
		}

		/// <summary>
		/// Commits current frame as atomic operation and prepares new frame
		/// </summary>
		public void CommitUpdateFrame(SpinLockRef heldLock)
		{
			MyTimeSpan updateTimestamp = CurrentUpdateFrame.UpdateTimestamp;
			CurrentUpdateFrame.Processed = false;
			if (MY_FAKE__ENABLE_FRAMES_OVERFLOW_BARRIER && m_updateDataQueue.Count > 1000)
			{
				heldLock?.Exit();
				m_overflowGate.Reset();
				m_overflowGate.WaitOne();
				heldLock?.Enter();
			}
			m_updateDataQueue.Enqueue(CurrentUpdateFrame);
			CurrentUpdateFrame = m_frameDataPool.Get();
			CurrentUpdateFrame.UpdateTimestamp = updateTimestamp;
		}

		/// <summary>
		/// Gets next frame for rendering, can return null in case there's nothing for rendering (no update frame submitted).
		/// When isPreFrame is true, don't handle draw messages, just process update messages and call method again.
		/// Pre frame must release messages and must be returned.
		/// Final frame is kept unmodified in queue, in case of slower update, so we can interpolate and draw frame again.
		/// </summary>
		public MyUpdateFrame GetRenderFrame(out bool isPreFrame)
		{
			if (m_updateDataQueue.Count > 1)
			{
				isPreFrame = true;
				return m_updateDataQueue.Dequeue();
			}
			isPreFrame = false;
			m_overflowGate.Set();
			if (!m_updateDataQueue.TryPeek(out MyUpdateFrame instance))
			{
				return null;
			}
			return instance;
		}

		/// <summary>
		/// PreFrame must be empty in this place
		/// </summary>
		public void ReturnPreFrame(MyUpdateFrame frame)
		{
			m_frameDataPool.Return(frame);
		}
	}
}
