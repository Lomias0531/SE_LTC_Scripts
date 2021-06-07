using System.Globalization;
using System.Threading;
using VRage.Library;

namespace ParallelTasks
{
	internal class Worker
	{
		private Thread thread;

		private Deque<Task> tasks;

		private WorkStealingScheduler scheduler;

		private static Hashtable<Thread, Worker> workers = new Hashtable<Thread, Worker>(MyEnvironment.ProcessorCount);

		public AutoResetEvent Gate
		{
			get;
			private set;
		}

		public ManualResetEvent HasNoWork
		{
			get;
			private set;
		}

		public static Worker CurrentWorker
		{
			get
			{
				Thread currentThread = Thread.CurrentThread;
				if (workers.TryGet(currentThread, out Worker data))
				{
					return data;
				}
				return null;
			}
		}

		public Worker(WorkStealingScheduler scheduler, int index, ThreadPriority priority)
		{
			thread = new Thread(Work);
			thread.Name = "Parallel " + index;
			thread.IsBackground = true;
			thread.Priority = priority;
			thread.CurrentCulture = CultureInfo.InvariantCulture;
			thread.CurrentUICulture = CultureInfo.InvariantCulture;
			tasks = new Deque<Task>();
			this.scheduler = scheduler;
			Gate = new AutoResetEvent(initialState: false);
			HasNoWork = new ManualResetEvent(initialState: false);
			workers.Add(thread, this);
		}

		public void Start()
		{
			thread.Start();
		}

		public void AddWork(Task task)
		{
			tasks.LocalPush(task);
		}

		private void Work()
		{
			while (true)
			{
				FindWork(out Task task);
				task.DoWork();
			}
		}

		private void FindWork(out Task task)
		{
			bool flag = false;
			task = default(Task);
			while (!tasks.LocalPop(ref task) && !scheduler.TryGetTask(out task))
			{
				for (int i = 0; i < scheduler.Workers.Count; i++)
				{
					Worker worker = scheduler.Workers[i];
					if (worker != this && worker.tasks.TrySteal(ref task))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					HasNoWork.Set();
					Gate.WaitOne();
					HasNoWork.Reset();
				}
				if (flag)
				{
					break;
				}
			}
		}
	}
}
