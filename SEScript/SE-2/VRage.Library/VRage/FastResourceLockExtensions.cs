using System;
using System.Diagnostics;
using System.Threading;

namespace VRage
{
	public static class FastResourceLockExtensions
	{
		public struct MySharedLock : IDisposable
		{
			private readonly FastResourceLock m_lockObject;

			[DebuggerStepThrough]
			public MySharedLock(FastResourceLock lockObject)
			{
				m_lockObject = lockObject;
				m_lockObject.AcquireShared();
			}

			public void Dispose()
			{
				if (m_lockObject != null)
				{
					m_lockObject.ReleaseShared();
				}
			}
		}

		public struct MyExclusiveLock : IDisposable
		{
			private readonly FastResourceLock m_lockObject;

			[DebuggerStepThrough]
			public MyExclusiveLock(FastResourceLock lockObject)
			{
				m_lockObject = lockObject;
				m_lockObject.AcquireExclusive();
			}

			public void Dispose()
			{
				m_lockObject.ReleaseExclusive();
			}
		}

		public struct MyOwnedExclusiveLock : IDisposable
		{
			private Ref<int> m_owner;

			private MyExclusiveLock m_core;

			public MyOwnedExclusiveLock(FastResourceLock lockObject, Ref<int> ownerField)
			{
				m_owner = ownerField;
				m_core = new MyExclusiveLock(lockObject);
				m_owner.Value = Thread.CurrentThread.ManagedThreadId;
			}

			public void Dispose()
			{
				if (m_owner != null)
				{
					m_owner.Value = -1;
					m_core.Dispose();
				}
			}
		}

		/// <summary>
		/// Call dispose or use using block to release lock
		/// </summary>
		[DebuggerStepThrough]
		public static MySharedLock AcquireSharedUsing(this FastResourceLock lockObject)
		{
			return new MySharedLock(lockObject);
		}

		/// <summary>
		/// Call dispose or use using block to release lock
		/// </summary>
		[DebuggerStepThrough]
		public static MyExclusiveLock AcquireExclusiveUsing(this FastResourceLock lockObject)
		{
			return new MyExclusiveLock(lockObject);
		}

		[DebuggerStepThrough]
		public static MyOwnedExclusiveLock AcquireExclusiveRecursiveUsing(this FastResourceLock lockObject, Ref<int> ownerField)
		{
			if (lockObject.IsOwnedByCurrentThread(ownerField))
			{
				return default(MyOwnedExclusiveLock);
			}
			return new MyOwnedExclusiveLock(lockObject, ownerField);
		}

		[DebuggerStepThrough]
		public static MySharedLock AcquireSharedRecursiveUsing(this FastResourceLock lockObject, Ref<int> ownerField)
		{
			if (lockObject.IsOwnedByCurrentThread(ownerField))
			{
				return default(MySharedLock);
			}
			return new MySharedLock(lockObject);
		}

		[DebuggerStepThrough]
		public static bool IsOwnedByCurrentThread(this FastResourceLock lockObject, Ref<int> ownerField)
		{
			if (lockObject.Owned)
			{
				return ownerField.Value == Thread.CurrentThread.ManagedThreadId;
			}
			return false;
		}
	}
}
