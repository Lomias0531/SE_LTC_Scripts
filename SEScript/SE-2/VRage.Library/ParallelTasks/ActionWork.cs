using System;

namespace ParallelTasks
{
	public class ActionWork : AbstractWork
	{
		private class ParallelTasks_ActionWork_003C_003EActor
		{
		}

		public readonly Action<WorkData> _Action;

		public ActionWork(Action<WorkData> action)
			: this(action, Parallel.DefaultOptions)
		{
		}

		public ActionWork(Action<WorkData> action, WorkOptions options)
		{
			_Action = action;
			Options = options;
		}

		public override void DoWork(WorkData workData = null)
		{
			_Action(workData);
		}
	}
}
