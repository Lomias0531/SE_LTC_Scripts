using System;

namespace ParallelTasks
{
	/// <summary>
	/// An interface defining a task scheduler.
	/// </summary>
	public interface IWorkScheduler
	{
		/// <summary>
		/// Gets a number of threads.
		/// This number must be correct, because it's used to run per-thread initialization task on all threads (by using barrier)
		/// </summary>
		int ThreadCount
		{
			get;
		}

		/// <summary>
		/// Schedules a task for execution.
		/// </summary>
		/// <param name="item">The task to schedule.</param>
		void Schedule(Task item);

		/// <summary>
		/// Wait for all tasks to finish the work.
		/// </summary>
		bool WaitForTasksToFinish(TimeSpan waitTimeout);

		void ScheduleOnEachWorker(Action action);

		/// <summary>
		/// Returns number of ticks the parallel threads spent working on scheduled tasks since last request or since the threadpool was created if no query was executed yet.
		/// Result includes time spent working on tasks that are not finished fully (e.g. running) at the moment of request.
		/// Does not represent actual time spent on CPU, only time spent by "not waiting for a task"!
		/// </summary>
		int ReadAndClearExecutionTime();
	}
}
