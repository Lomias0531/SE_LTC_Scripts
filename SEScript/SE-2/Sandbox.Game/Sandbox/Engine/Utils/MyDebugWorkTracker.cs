using System.Diagnostics;
using VRage.Collections;
using VRage.Library.Threading;

namespace Sandbox.Engine.Utils
{
	public class MyDebugWorkTracker<T> where T : new()
	{
		private SpinLockRef m_lock = new SpinLockRef();

		public readonly MyQueue<T> History;

		public T Current;

		private uint m_historyLength;

		public MyDebugWorkTracker(uint historyLength = 10u)
		{
			m_historyLength = historyLength;
			History = new MyQueue<T>((int)m_historyLength);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public void Wrap()
		{
			using (m_lock.Acquire())
			{
				if (History.Count >= m_historyLength)
				{
					History.Dequeue();
				}
				History.Enqueue(Current);
				Current = new T();
			}
		}
	}
}
