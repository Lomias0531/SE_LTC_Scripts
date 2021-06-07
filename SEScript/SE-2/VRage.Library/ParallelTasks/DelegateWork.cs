using System;
using VRage.Collections;
using VRage.Network;
using VRage.Profiler;

namespace ParallelTasks
{
	internal class DelegateWork : AbstractWork, IPrioritizedWork, IWork
	{
		private class ParallelTasks_DelegateWork_003C_003EActor : IActivator, IActivator<DelegateWork>
		{
			private sealed override object CreateInstance()
			{
				return new DelegateWork();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override DelegateWork CreateInstance()
			{
				return new DelegateWork();
			}

			DelegateWork IActivator<DelegateWork>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static MyConcurrentPool<DelegateWork> instances = new MyConcurrentPool<DelegateWork>(100, null, 100000);

		public Action Action
		{
			get;
			set;
		}

		public Action<WorkData> DataAction
		{
			get;
			set;
		}

		public WorkPriority Priority
		{
			get;
			set;
		} = WorkPriority.Normal;


		public override void DoWork(WorkData workData = null)
		{
			try
			{
				if (Action != null)
				{
					Action();
					Action = null;
				}
				if (DataAction != null)
				{
					DataAction(workData);
					DataAction = null;
				}
			}
			finally
			{
				instances.Return(this);
			}
		}

		internal static DelegateWork GetInstance()
		{
			return instances.Get();
		}

		protected override void FillDebugInfo(ref WorkOptions info)
		{
			if (info.DebugName == null)
			{
				if (Action != null)
				{
					info.DebugName = Action.Method.Name;
				}
				else if (DataAction != null)
				{
					info.DebugName = DataAction.Method.Name;
				}
				else
				{
					info.DebugName = string.Empty;
				}
			}
			if (info.TaskType == MyProfiler.TaskType.None)
			{
				info.TaskType = MyProfiler.TaskType.WorkItem;
			}
		}
	}
}
