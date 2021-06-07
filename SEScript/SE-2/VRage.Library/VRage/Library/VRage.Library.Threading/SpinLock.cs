using System;
using System.Threading;

namespace VRage.Library.Threading
{
	/// <summary>
	/// A struct which implements a spin lock.
	/// </summary>
	public struct SpinLock
	{
		private Thread owner;

		private int recursion;

		/// <summary>
		/// Enters the lock. The calling thread will spin wait until it gains ownership of the lock.
		/// </summary>
		public void Enter()
		{
			Thread currentThread = Thread.CurrentThread;
			if (owner == currentThread)
			{
				Interlocked.Increment(ref recursion);
				return;
			}
			while (Interlocked.CompareExchange(ref owner, currentThread, null) != null)
			{
			}
			Interlocked.Increment(ref recursion);
		}

		/// <summary>
		/// Tries to enter the lock.
		/// </summary>
		/// <returns><c>true</c> if the lock was successfully taken; else <c>false</c>.</returns>
		public bool TryEnter()
		{
			Thread currentThread = Thread.CurrentThread;
			if (owner == currentThread)
			{
				Interlocked.Increment(ref recursion);
				return true;
			}
			bool flag = Interlocked.CompareExchange(ref owner, currentThread, null) == null;
			if (flag)
			{
				Interlocked.Increment(ref recursion);
			}
			return flag;
		}

		/// <summary>
		/// Exits the lock. This allows other threads to take ownership of the lock.
		/// </summary>
		public void Exit()
		{
			Thread currentThread = Thread.CurrentThread;
			if (currentThread == owner)
			{
				Interlocked.Decrement(ref recursion);
				if (recursion == 0)
				{
					owner = null;
				}
				return;
			}
			throw new InvalidOperationException("Exit cannot be called by a thread which does not currently own the lock.");
		}
	}
}
