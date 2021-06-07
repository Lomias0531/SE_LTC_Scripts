using System;

namespace VRage.Library.Threading
{
	public class SpinLockRef
	{
		public struct Token : IDisposable
		{
			private SpinLockRef m_lock;

			public Token(SpinLockRef spin)
			{
				m_lock = spin;
				m_lock.Enter();
			}

			public void Dispose()
			{
				m_lock.Exit();
			}
		}

		private SpinLock m_spinLock;

		public Token Acquire()
		{
			return new Token(this);
		}

		public void Enter()
		{
			m_spinLock.Enter();
		}

		public void Exit()
		{
			m_spinLock.Exit();
		}
	}
}
