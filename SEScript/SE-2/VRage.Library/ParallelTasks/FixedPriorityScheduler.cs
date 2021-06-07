using System;
using System.Globalization;
using System.Threading;
using VRage.Collections;

namespace ParallelTasks
{
	public class FixedPriorityScheduler : IWorkScheduler
	{
		private class Worker
		{
			private readonly FixedPriorityScheduler m_scheduler;

			private readonly Thread m_thread;

			public readonly ManualResetEvent HasNoWork;

			public readonly AutoResetEvent Gate;

			public Worker(FixedPriorityScheduler scheduler, string name, ThreadPriority priority)
			{
				m_scheduler = scheduler;
				m_thread = new Thread(WorkerLoop);
				HasNoWork = new ManualResetEvent(initialState: false);
				Gate = new AutoResetEvent(initialState: false);
				m_thread.Name = name;
				m_thread.IsBackground = true;
				m_thread.Priority = priority;
				m_thread.CurrentCulture = CultureInfo.InvariantCulture;
				m_thread.CurrentUICulture = CultureInfo.InvariantCulture;
				m_thread.Start(null);
			}

			private void WorkerLoop(object o)
			{
				while (true)
				{
					if (m_scheduler.TryGetTask(out Task task))
					{
						task.DoWork();
						continue;
					}
					HasNoWork.Set();
					Gate.WaitOne();
					HasNoWork.Reset();
				}
			}
		}

		private readonly MyConcurrentQueue<Task>[] m_taskQueuesByPriority;

		private readonly Worker[] m_workers;

		private readonly ManualResetEvent[] m_hasNoWork;

		private long m_scheduledTaskCount;

		public int ThreadCount => m_workers.Length;

		public FixedPriorityScheduler(int threadCount, ThreadPriority priority)
		{
			m_taskQueuesByPriority = new MyConcurrentQueue<Task>[typeof(WorkPriority).GetEnumValues().Length];
			for (int i = 0; i < m_taskQueuesByPriority.Length; i++)
			{
				m_taskQueuesByPriority[i] = new MyConcurrentQueue<Task>();
			}
			m_hasNoWork = new ManualResetEvent[threadCount];
			m_workers = new Worker[threadCount];
			for (int j = 0; j < threadCount; j++)
			{
				m_workers[j] = new Worker(this, "Parallel " + j, priority);
				m_hasNoWork[j] = m_workers[j].HasNoWork;
			}
		}

		private bool TryGetTask(out Task task)
		{
			while (m_scheduledTaskCount > 0)
			{
				for (int i = 0; i < m_taskQueuesByPriority.Length; i++)
				{
					if (m_taskQueuesByPriority[i].TryDequeue(out task))
					{
						Interlocked.Decrement(ref m_scheduledTaskCount);
						return true;
					}
				}
			}
			task = default(Task);
			return false;
		}

		public void Schedule(Task task)
		{
			if (task.Item.Work != null)
			{
				WorkPriority workPriority = WorkPriority.Normal;
				IPrioritizedWork prioritizedWork = task.Item.Work as IPrioritizedWork;
				if (prioritizedWork != null)
				{
					workPriority = prioritizedWork.Priority;
				}
				m_taskQueuesByPriority[(int)workPriority].Enqueue(task);
				Interlocked.Increment(ref m_scheduledTaskCount);
				Worker[] workers = m_workers;
				foreach (Worker worker in workers)
				{
					worker.Gate.Set();
				}
			}
		}

		public bool WaitForTasksToFinish(TimeSpan waitTimeout)
		{
			WaitHandle[] hasNoWork = m_hasNoWork;
			return Parallel.WaitForAll(hasNoWork, waitTimeout);
		}

		public void ScheduleOnEachWorker(Action action)
		{
			Worker[] workers = m_workers;
			foreach (Worker worker in workers)
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
				Task instance2 = workItem.PrepareStart(instance);
				m_taskQueuesByPriority[0].Enqueue(instance2);
				Interlocked.Increment(ref m_scheduledTaskCount);
				worker.Gate.Set();
				instance2.Wait();
			}
		}

		public int ReadAndClearExecutionTime()
		{
			throw new NotImplementedException();
		}
	}
}
