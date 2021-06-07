using System;

namespace ParallelTasks
{
	public class FakeTaskScheduler : IWorkScheduler
	{
		public int ThreadCount => 1;

		public void Schedule(Task item)
		{
			item.DoWork();
		}

		public bool WaitForTasksToFinish(TimeSpan waitTimeout)
		{
			return true;
		}

		public void ScheduleOnEachWorker(Action action)
		{
			DelegateWork instance = DelegateWork.GetInstance();
			instance.Action = action;
			instance.Options = new WorkOptions
			{
				MaximumThreads = 1,
				QueueFIFO = false
			};
			WorkItem workItem = WorkItem.Get();
			workItem.CompletionCallbacks = null;
			workItem.Callback = null;
			workItem.WorkData = null;
			workItem.PrepareStart(instance).DoWork();
		}

		public int ReadAndClearExecutionTime()
		{
			return 0;
		}
	}
}
