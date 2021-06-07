using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage;
using VRage.Library;

namespace ParallelTasks
{
	/// <summary>
	/// A "work stealing" work scheduler class.
	/// </summary>
	public class WorkStealingScheduler : IWorkScheduler
	{
		private Queue<Task> tasks;

		private FastResourceLock tasksLock;

		internal List<Worker> Workers
		{
			get;
			private set;
		}

		public int ThreadCount => Workers.Count;

		/// <summary>
		/// Creates a new instance of the <see cref="T:ParallelTasks.WorkStealingScheduler" /> class.
		/// </summary>
		public WorkStealingScheduler()
			: this(MyEnvironment.ProcessorCount, ThreadPriority.BelowNormal)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="T:ParallelTasks.WorkStealingScheduler" /> class.
		/// </summary>
		/// <param name="numThreads">The number of threads to create.</param>
		public WorkStealingScheduler(int numThreads, ThreadPriority priority)
		{
			tasks = new Queue<Task>();
			tasksLock = new FastResourceLock();
			Workers = new List<Worker>(numThreads);
			for (int i = 0; i < numThreads; i++)
			{
				Workers.Add(new Worker(this, i, priority));
			}
			for (int j = 0; j < numThreads; j++)
			{
				Workers[j].Start();
			}
		}

		internal bool TryGetTask(out Task task)
		{
			if (tasks.Count == 0)
			{
				task = default(Task);
				return false;
			}
			using (tasksLock.AcquireExclusiveUsing())
			{
				if (tasks.Count > 0)
				{
					task = tasks.Dequeue();
					return true;
				}
				task = default(Task);
				return false;
			}
		}

		/// <summary>
		/// Schedules a task for execution.
		/// </summary>
		/// <param name="task">The task to schedule.</param>
		public void Schedule(Task task)
		{
			if (task.Item.Work != null)
			{
				int maximumThreads = task.Item.Work.Options.MaximumThreads;
				Worker currentWorker = Worker.CurrentWorker;
				if (!task.Item.Work.Options.QueueFIFO && currentWorker != null)
				{
					currentWorker.AddWork(task);
				}
				else
				{
					using (tasksLock.AcquireExclusiveUsing())
					{
						tasks.Enqueue(task);
					}
				}
				for (int i = 0; i < Workers.Count; i++)
				{
					Workers[i].Gate.Set();
				}
			}
		}

		public bool WaitForTasksToFinish(TimeSpan waitTimeout)
		{
			ManualResetEvent[] array = Workers.Select((Worker s) => s.HasNoWork).ToArray();
			WaitHandle[] waitHandles = array;
			return Parallel.WaitForAll(waitHandles, waitTimeout);
		}

		public void ScheduleOnEachWorker(Action action)
		{
			foreach (Worker worker in Workers)
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
				Task task = workItem.PrepareStart(instance);
				worker.AddWork(task);
				worker.Gate.Set();
				task.Wait();
			}
		}

		public int ReadAndClearExecutionTime()
		{
			throw new NotImplementedException();
		}
	}
}
