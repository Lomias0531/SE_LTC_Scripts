using System;
using System.Threading;
using VRage.Collections;
using VRage.Network;

namespace ParallelTasks
{
	internal class ForLoopWork : AbstractWork, IPrioritizedWork, IWork
	{
		private class ParallelTasks_ForLoopWork_003C_003EActor : IActivator, IActivator<ForLoopWork>
		{
			private sealed override object CreateInstance()
			{
				return new ForLoopWork();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ForLoopWork CreateInstance()
			{
				return new ForLoopWork();
			}

			ForLoopWork IActivator<ForLoopWork>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static MyConcurrentPool<ForLoopWork> pool = new MyConcurrentPool<ForLoopWork>(10);

		private int index;

		private int length;

		private int stride;

		private Action<int> action;

		public WorkPriority Priority
		{
			get;
			private set;
		}

		public void Prepare(Action<int> action, int startInclusive, int endExclusive, int stride, WorkPriority priority)
		{
			this.action = action;
			index = startInclusive;
			length = endExclusive;
			this.stride = stride;
			Priority = priority;
		}

		public override void DoWork(WorkData workData = null)
		{
			int num;
			while ((num = IncrementIndex()) < length)
			{
				int num2 = Math.Min(num + stride, length);
				for (int i = num; i < num2; i++)
				{
					action(i);
				}
			}
		}

		private int IncrementIndex()
		{
			return Interlocked.Add(ref index, stride) - stride;
		}

		protected override void FillDebugInfo(ref WorkOptions info)
		{
			FillDebugInfo(ref info, action.Method.Name);
		}

		public static ForLoopWork Get()
		{
			return pool.Get();
		}

		public void Return()
		{
			action = null;
			pool.Return(this);
		}
	}
}
