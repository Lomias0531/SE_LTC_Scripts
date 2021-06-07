using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Network;

namespace ParallelTasks
{
	internal class ForEachLoopWork<T> : AbstractWork, IPrioritizedWork, IWork
	{
		private class ParallelTasks_ForEachLoopWork_00601_003C_003EActor : IActivator, IActivator<ForEachLoopWork<T>>
		{
			private sealed override object CreateInstance()
			{
				return new ForEachLoopWork<T>();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ForEachLoopWork<T> CreateInstance()
			{
				return new ForEachLoopWork<T>();
			}

			ForEachLoopWork<T> IActivator<ForEachLoopWork<T>>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static MyConcurrentPool<ForEachLoopWork<T>> pool = new MyConcurrentPool<ForEachLoopWork<T>>(10);

		private bool done;

		private object syncLock;

		private Action<T> action;

		private IEnumerator<T> enumerator;

		public WorkPriority Priority
		{
			get;
			private set;
		}

		public ForEachLoopWork()
		{
			syncLock = new object();
		}

		public void Prepare(Action<T> action, IEnumerator<T> enumerator, WorkPriority priority)
		{
			done = false;
			this.action = action;
			Priority = priority;
			this.enumerator = enumerator;
		}

		public override void DoWork(WorkData workData = null)
		{
			T obj = default(T);
			while (!done)
			{
				lock (syncLock)
				{
					if (done)
					{
						return;
					}
					done = !enumerator.MoveNext();
					if (done)
					{
						return;
					}
					obj = enumerator.Current;
				}
				action(obj);
			}
		}

		protected override void FillDebugInfo(ref WorkOptions info)
		{
			FillDebugInfo(ref info, action.Method.Name);
		}

		public static ForEachLoopWork<T> Get()
		{
			return pool.Get();
		}

		public void Return()
		{
			enumerator = null;
			action = null;
			pool.Return(this);
		}
	}
}
