using System.Diagnostics;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Profiler;

namespace ParallelTasks
{
	[GenerateActivator]
	public abstract class AbstractWork : IWork
	{
		private WorkOptions m_options;

		private string m_cachedDebugName;

		public virtual WorkOptions Options
		{
			get
			{
				return m_options;
			}
			set
			{
				m_options = value;
			}
		}

		public abstract void DoWork(WorkData workData = null);

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private void FillDebugInfoInternal(ref WorkOptions info)
		{
			FillDebugInfo(ref info);
		}

		protected virtual void FillDebugInfo(ref WorkOptions info)
		{
			if (m_cachedDebugName == null)
			{
				m_cachedDebugName = GetType().Name;
			}
			FillDebugInfo(ref info, m_cachedDebugName);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void FillDebugInfo(ref WorkOptions info, string debugName, MyProfiler.TaskType taskType = MyProfiler.TaskType.WorkItem)
		{
			if (info.DebugName == null)
			{
				info.DebugName = debugName;
			}
			if (info.TaskType == MyProfiler.TaskType.None)
			{
				info.TaskType = taskType;
			}
		}
	}
}
