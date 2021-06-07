using System;
using System.Threading;
using VRage.Network;

namespace ParallelTasks
{
	internal class FutureWork<T> : AbstractWork
	{
		private class ParallelTasks_FutureWork_00601_003C_003EActor : IActivator, IActivator<FutureWork<T>>
		{
			private sealed override object CreateInstance()
			{
				return new FutureWork<T>();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override FutureWork<T> CreateInstance()
			{
				return new FutureWork<T>();
			}

			FutureWork<T> IActivator<FutureWork<T>>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public int ID
		{
			get;
			private set;
		}

		public Func<T> Function
		{
			get;
			set;
		}

		public T Result
		{
			get;
			set;
		}

		public override void DoWork(WorkData workData = null)
		{
			Result = Function();
		}

		public static FutureWork<T> GetInstance()
		{
			return Singleton<Pool<FutureWork<T>>>.Instance.Get(Thread.CurrentThread);
		}

		public void ReturnToPool()
		{
			if (ID < int.MaxValue)
			{
				ID++;
				Function = null;
				Result = default(T);
				Singleton<Pool<FutureWork<T>>>.Instance.Return(Thread.CurrentThread, this);
			}
		}
	}
}
