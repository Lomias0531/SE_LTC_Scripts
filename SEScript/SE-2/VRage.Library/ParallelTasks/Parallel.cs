using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VRage.Collections;
using VRage.Profiler;

namespace ParallelTasks
{
	/// <summary>
	/// A static class containing factory methods for creating tasks.
	/// </summary>
	public static class Parallel
	{
		public static readonly bool THROW_WORKER_EXCEPTIONS = false;

		public static readonly WorkOptions DefaultOptions = new WorkOptions
		{
			MaximumThreads = 1,
			TaskType = MyProfiler.TaskType.WorkItem
		};

		private static IWorkScheduler scheduler;

		private static Pool<List<Task>> taskPool = new Pool<List<Task>>();

		private static readonly Dictionary<Thread, ConcurrentCachingList<WorkItem>> Buffers = new Dictionary<Thread, ConcurrentCachingList<WorkItem>>(8);

		[ThreadStatic]
		private static ConcurrentCachingList<WorkItem> m_callbackBuffer;

		private static int[] _processorAffinity = new int[4]
		{
			3,
			4,
			5,
			1
		};

		public static ConcurrentCachingList<WorkItem> CallbackBuffer
		{
			get
			{
				if (m_callbackBuffer == null)
				{
					m_callbackBuffer = new ConcurrentCachingList<WorkItem>(16);
					lock (Buffers)
					{
						Buffers.Add(Thread.CurrentThread, m_callbackBuffer);
					}
				}
				return m_callbackBuffer;
			}
		}

		/// <summary>
		/// Gets or sets the processor affinity of the worker threads.
		/// </summary>
		/// <value>
		/// The processor affinity of the worker threads. The default value is <c>{ 3, 4, 5, 1 }</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// In the .NET Compact Framework for Xbox 360 the processor affinity determines the processors 
		/// on which a thread runs. 
		/// </para>
		/// <para>
		/// <strong>Note:</strong> The processor affinity is only relevant in the .NET Compact Framework 
		/// for Xbox 360. Setting the processor affinity has no effect in Windows!
		/// </para>
		/// <para>
		/// <strong>Important:</strong> The processor affinity needs to be set before any parallel tasks
		/// are created. Changing the processor affinity afterwards has no effect.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="value" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The specified array is empty or contains invalid values.
		/// </exception>
		public static int[] ProcessorAffinity
		{
			get
			{
				return _processorAffinity;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length < 1)
				{
					throw new ArgumentException("The Parallel.ProcessorAffinity must contain at least one value.", "value");
				}
				if (value.Any((int id) => id < 0))
				{
					throw new ArgumentException("The processor affinity must not be negative.", "value");
				}
				_processorAffinity = value;
			}
		}

		/// <summary>
		/// Gets or sets the work scheduler.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="value" /> is <see langword="null" />.
		/// </exception>
		public static IWorkScheduler Scheduler
		{
			get
			{
				if (scheduler == null)
				{
					IWorkScheduler value = new WorkStealingScheduler();
					Interlocked.CompareExchange(ref scheduler, value, null);
				}
				return scheduler;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				Interlocked.Exchange(ref scheduler, value);
			}
		}

		/// <summary>
		/// Executes all task callbacks for the thread calling this function.
		/// It is thread safe.
		/// </summary>
		public static void RunCallbacks()
		{
			CallbackBuffer.ApplyChanges();
			for (int i = 0; i < CallbackBuffer.Count; i++)
			{
				WorkItem workItem = CallbackBuffer[i];
				if (workItem != null)
				{
					if (workItem.Callback != null)
					{
						workItem.Callback();
						workItem.Callback = null;
					}
					if (workItem.DataCallback != null)
					{
						workItem.DataCallback(workItem.WorkData);
						workItem.DataCallback = null;
					}
					workItem.WorkData = null;
					workItem.Requeue();
				}
			}
			CallbackBuffer.ClearList();
		}

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking, work
		/// such as I/O.
		/// </summary>
		/// <param name="work">The work to execute.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		public static Task StartBackground(IWork work)
		{
			return StartBackground(work, null);
		}

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking, work
		/// such as I/O.
		/// </summary>
		/// <param name="work">The work to execute.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="work" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// Invalid number of maximum threads set in <see cref="P:ParallelTasks.IWork.Options" />.
		/// </exception>
		public static Task StartBackground(IWork work, Action completionCallback)
		{
			if (work == null)
			{
				throw new ArgumentNullException("work");
			}
			if (work.Options.MaximumThreads < 1)
			{
				throw new ArgumentException("work.Options.MaximumThreads cannot be less than one.");
			}
			WorkItem workItem = WorkItem.Get();
			if (completionCallback != null)
			{
				workItem.CompletionCallbacks = CallbackBuffer;
				workItem.Callback = completionCallback;
			}
			workItem.WorkData = null;
			Task task = workItem.PrepareStart(work);
			BackgroundWorker.StartWork(task);
			return task;
		}

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking, work
		/// such as I/O.
		/// </summary>
		/// <param name="action">The work to execute.</param>
		/// <returns>A task which represents one execution of the action.</returns>
		public static Task StartBackground(Action action)
		{
			return StartBackground(action, null);
		}

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking, work
		/// such as I/O.
		/// </summary>
		/// <param name="action">The work to execute.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the action.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="action" /> is <see langword="null" />.
		/// </exception>
		public static Task StartBackground(Action action, Action completionCallback)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			DelegateWork instance = DelegateWork.GetInstance();
			instance.Action = action;
			instance.Options = DefaultOptions;
			return StartBackground(instance, completionCallback);
		}

		/// <summary>
		/// Starts a task in a secondary worker thread. Intended for long running, blocking work such as I/O.
		/// </summary>
		/// <param name="action">The work to execute.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <param name="workData">Data to be passed along both the work and the completion callback.</param>
		/// <returns>A task which represents one execution of the action.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="action" /> is <see langword="null" />.
		/// </exception>
		public static Task StartBackground(Action<WorkData> action, Action<WorkData> completionCallback, WorkData workData)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			DelegateWork instance = DelegateWork.GetInstance();
			instance.DataAction = action;
			instance.Options = DefaultOptions;
			WorkItem workItem = WorkItem.Get();
			if (completionCallback != null)
			{
				workItem.CompletionCallbacks = CallbackBuffer;
				workItem.DataCallback = completionCallback;
			}
			workItem.WorkData = workData;
			Task task = workItem.PrepareStart(instance);
			BackgroundWorker.StartWork(task);
			return task;
		}

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="work">The work to execute in parallel.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		public static Task Start(IWork work)
		{
			return Start(work, null);
		}

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="work">The work to execute in parallel.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="work" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// Invalid number of maximum threads set in <see cref="P:ParallelTasks.IWork.Options" />.
		/// </exception>
		public static Task Start(IWork work, Action completionCallback)
		{
			if (work == null)
			{
				throw new ArgumentNullException("work");
			}
			if (work.Options.MaximumThreads < 1)
			{
				throw new ArgumentException("work.Options.MaximumThreads cannot be less than one.");
			}
			WorkItem workItem = WorkItem.Get();
			if (completionCallback != null)
			{
				workItem.CompletionCallbacks = CallbackBuffer;
				workItem.Callback = completionCallback;
			}
			workItem.WorkData = null;
			Task task = workItem.PrepareStart(work);
			Scheduler.Schedule(task);
			return task;
		}

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		public static Task Start(Action action, WorkPriority priority = WorkPriority.Normal)
		{
			return Start(action, null, priority);
		}

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		public static Task Start(Action action, Action completionCallback, WorkPriority priority = WorkPriority.Normal)
		{
			return Start(action, new WorkOptions
			{
				MaximumThreads = 1,
				QueueFIFO = false
			}, completionCallback, priority);
		}

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <param name="options">The work options to use with this action.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		public static Task Start(Action action, WorkOptions options, WorkPriority priority = WorkPriority.Normal)
		{
			return Start(action, options, null, priority);
		}

		/// <summary>
		/// Creates and starts a task to execute the given work.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <param name="options">The work options to use with this action.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A task which represents one execution of the work.</returns>
		public static Task Start(Action action, WorkOptions options, Action completionCallback, WorkPriority priority = WorkPriority.Normal)
		{
			if (options.MaximumThreads != 1)
			{
				throw new ArgumentOutOfRangeException("options", "options.MaximumThreads has to be 1 for delegate work");
			}
			DelegateWork instance = DelegateWork.GetInstance();
			instance.Action = action;
			instance.Options = options;
			instance.Priority = priority;
			return Start(instance, completionCallback);
		}

		/// <summary>
		/// Creates and schedules a task to execute the given work with the given work data.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <param name="workData">Data to be passed along both the work and the completion callback.</param>
		/// <returns>A task which represents one execution of the action.</returns>
		public static Task Start(Action<WorkData> action, Action<WorkData> completionCallback, WorkData workData, WorkPriority workPriority = WorkPriority.Normal)
		{
			WorkOptions workOptions = default(WorkOptions);
			workOptions.MaximumThreads = 1;
			workOptions.QueueFIFO = false;
			WorkOptions options = workOptions;
			DelegateWork instance = DelegateWork.GetInstance();
			instance.DataAction = action;
			instance.Options = options;
			instance.Priority = workPriority;
			WorkItem workItem = WorkItem.Get();
			if (completionCallback != null)
			{
				workItem.CompletionCallbacks = CallbackBuffer;
				workItem.DataCallback = completionCallback;
			}
			workItem.WorkData = workData;
			Task task = workItem.PrepareStart(instance);
			Scheduler.Schedule(task);
			return task;
		}

		/// <summary>
		/// Creates and schedules a task to execute on the given work-tracking thread.
		/// If the requested thread that does not execute completion callbacks the callback will never be called.
		/// </summary>
		/// <param name="action">The work to execute in parallel.</param>
		/// <param name="workData">Data to be passed along both the work and the completion callback.</param>
		/// <param name="thread">Thread to execute the callback on. If not provided this is the calling thread.</param>
		/// <returns>A task which represents one execution of the action.</returns>
		public static Task ScheduleForThread(Action<WorkData> action, WorkData workData, Thread thread = null)
		{
			if (thread == null)
			{
				thread = Thread.CurrentThread;
			}
			WorkOptions workOptions = default(WorkOptions);
			workOptions.MaximumThreads = 1;
			workOptions.QueueFIFO = false;
			WorkOptions options = workOptions;
			DelegateWork instance = DelegateWork.GetInstance();
			instance.Options = options;
			WorkItem workItem = WorkItem.Get();
			lock (Buffers)
			{
				workItem.CompletionCallbacks = Buffers[thread];
			}
			workItem.DataCallback = action;
			workItem.WorkData = workData;
			Task result = workItem.PrepareStart(instance);
			workItem.CompletionCallbacks.Add(workItem);
			return result;
		}

		/// <summary>
		/// Starts same task on each worker, each worker executes this task exactly once.
		/// Good for initialization and release of per-thread resources.
		/// THIS CANNOT BE RUN FROM ANY WORKER!
		/// </summary>
		public static void StartOnEachWorker(Action action)
		{
			Scheduler.ScheduleOnEachWorker(action);
		}

		/// <summary>
		/// Creates and starts a task which executes the given function and stores the result for later retrieval.
		/// </summary>
		/// <typeparam name="T">The type of result the function returns.</typeparam>
		/// <param name="function">The function to execute in parallel.</param>
		/// <returns>A future which represults one execution of the function.</returns>
		public static Future<T> Start<T>(Func<T> function)
		{
			return Start(function, null);
		}

		/// <summary>
		/// Creates and starts a task which executes the given function and stores the result for later retrieval.
		/// </summary>
		/// <typeparam name="T">The type of result the function returns.</typeparam>
		/// <param name="function">The function to execute in parallel.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A future which represults one execution of the function.</returns>
		public static Future<T> Start<T>(Func<T> function, Action completionCallback)
		{
			return Start(function, DefaultOptions, completionCallback);
		}

		/// <summary>
		/// Creates and starts a task which executes the given function and stores the result for later retrieval.
		/// </summary>
		/// <typeparam name="T">The type of result the function returns.</typeparam>
		/// <param name="function">The function to execute in parallel.</param>
		/// <param name="options">The work options to use with this action.</param>
		/// <returns>A future which represents one execution of the function.</returns>
		public static Future<T> Start<T>(Func<T> function, WorkOptions options)
		{
			return Start(function, options, null);
		}

		/// <summary>
		/// Creates and starts a task which executes the given function and stores the result for later retrieval.
		/// </summary>
		/// <typeparam name="T">The type of result the function returns.</typeparam>
		/// <param name="function">The function to execute in parallel.</param>
		/// <param name="options">The work options to use with this action.</param>
		/// <param name="completionCallback">A method which will be called in Parallel.RunCallbacks() once this task has completed.</param>
		/// <returns>A future which represents one execution of the function.</returns>
		public static Future<T> Start<T>(Func<T> function, WorkOptions options, Action completionCallback)
		{
			if (options.MaximumThreads < 1)
			{
				throw new ArgumentOutOfRangeException("options", "options.MaximumThreads cannot be less than 1.");
			}
			FutureWork<T> instance = FutureWork<T>.GetInstance();
			instance.Function = function;
			instance.Options = options;
			Task task = Start(instance, completionCallback);
			return new Future<T>(task, instance);
		}

		/// <summary>
		/// Executes the given work items potentially in parallel with each other.
		/// This method will block until all work is completed.
		/// </summary>
		/// <param name="a">Work to execute.</param>
		/// <param name="b">Work to execute.</param>
		public static void Do(IWork a, IWork b)
		{
			Task task = Start(b);
			a.DoWork();
			task.WaitOrExecute();
		}

		/// <summary>
		/// Executes the given work items potentially in parallel with each other.
		/// This method will block until all work is completed.
		/// </summary>
		/// <param name="work">The work to execute.</param>
		public static void Do(params IWork[] work)
		{
			List<Task> list = taskPool.Get(Thread.CurrentThread);
			for (int i = 0; i < work.Length; i++)
			{
				list.Add(Start(work[i]));
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].WaitOrExecute();
			}
			list.Clear();
			taskPool.Return(Thread.CurrentThread, list);
		}

		/// <summary>
		/// Executes the given work items potentially in parallel with each other.
		/// This method will block until all work is completed.
		/// </summary>
		/// <param name="action1">The work to execute.</param>
		/// <param name="action2">The work to execute.</param>
		public static void Do(Action action1, Action action2)
		{
			DelegateWork instance = DelegateWork.GetInstance();
			instance.Action = action2;
			instance.Options = DefaultOptions;
			Task task = Start(instance);
			action1();
			task.WaitOrExecute();
		}

		/// <summary>
		/// Executes the given work items potentially in parallel with each other.
		/// This method will block until all work is completed.
		/// </summary>
		/// <param name="actions">The work to execute.</param>
		public static void Do(params Action[] actions)
		{
			List<Task> list = taskPool.Get(Thread.CurrentThread);
			for (int i = 0; i < actions.Length; i++)
			{
				DelegateWork instance = DelegateWork.GetInstance();
				instance.Action = actions[i];
				instance.Options = DefaultOptions;
				list.Add(Start(instance));
			}
			for (int j = 0; j < actions.Length; j++)
			{
				list[j].WaitOrExecute();
			}
			list.Clear();
			taskPool.Return(Thread.CurrentThread, list);
		}

		/// <summary>
		/// Executes a for loop, where each iteration can potentially occur in parallel with others.
		/// </summary>
		/// <param name="startInclusive">The index (inclusive) at which to start iterating.</param>
		/// <param name="endExclusive">The index (exclusive) at which to end iterating.</param>
		/// <param name="body">The method to execute at each iteration. The current index is supplied as the parameter.</param>
		public static void For(int startInclusive, int endExclusive, Action<int> body, WorkPriority priority = WorkPriority.Normal, WorkOptions? options = null)
		{
			For(startInclusive, endExclusive, body, 1, priority, options);
		}

		/// <summary>
		/// Executes a for loop, where each iteration can potentially occur in parallel with others.
		/// </summary>
		/// <param name="startInclusive">The index (inclusive) at which to start iterating.</param>
		/// <param name="endExclusive">The index (exclusive) at which to end iterating.</param>
		/// <param name="body">The method to execute at each iteration. The current index is supplied as the parameter.</param>
		/// <param name="stride">The number of iterations that each processor takes at a time.</param>
		public static void For(int startInclusive, int endExclusive, Action<int> body, int stride, WorkPriority priority = WorkPriority.Normal, WorkOptions? options = null, bool blocking = false)
		{
			int num = endExclusive - startInclusive;
			int num2 = (num + (stride - 1)) / stride;
			if (num2 > 0)
			{
				if (num2 == 1)
				{
					body(startInclusive);
					return;
				}
				ForLoopWork forLoopWork = ForLoopWork.Get();
				forLoopWork.Prepare(body, startInclusive, endExclusive, stride, priority);
				WorkOptions options2 = options ?? DefaultOptions;
				options2.MaximumThreads = num2;
				forLoopWork.Options = options2;
				Start(forLoopWork).WaitOrExecute(blocking);
				forLoopWork.Return();
			}
		}

		/// <summary>
		/// Executes a foreach loop, where each iteration can potentially occur in parallel with others.
		/// </summary>
		/// <typeparam name="T">The type of item to iterate over.</typeparam>
		/// <param name="collection">The enumerable data source.</param>
		/// <param name="action">The method to execute at each iteration. The item to process is supplied as the parameter.</param>
		public static void ForEach<T>(IEnumerable<T> collection, Action<T> action, WorkPriority priority = WorkPriority.Normal, WorkOptions? options = null, bool blocking = false)
		{
			ForEachLoopWork<T> forEachLoopWork = ForEachLoopWork<T>.Get();
			forEachLoopWork.Prepare(action, collection.GetEnumerator(), priority);
			WorkOptions options2;
			if (options.HasValue)
			{
				options2 = options.Value;
			}
			else
			{
				options2 = DefaultOptions;
				options2.MaximumThreads = int.MaxValue;
			}
			forEachLoopWork.Options = options2;
			Start(forEachLoopWork).WaitOrExecute(blocking);
			forEachLoopWork.Return();
		}

		public static void Clean()
		{
			CallbackBuffer.ApplyChanges();
			CallbackBuffer.ClearList();
			taskPool.Clean();
			lock (Buffers)
			{
				foreach (ConcurrentCachingList<WorkItem> value in Buffers.Values)
				{
					value.ClearImmediate();
				}
				Buffers.Clear();
			}
			WorkItem.Clean();
		}

		/// <summary>
		/// Safe version of WaitHandle.WaitForMultiple, but create new MTA thread when called from STA thread
		/// </summary>
		/// <param name="waitHandles"></param>
		public static bool WaitForAll(WaitHandle[] waitHandles, TimeSpan timeout)
		{
			if (Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA)
			{
				return WaitHandle.WaitAll(waitHandles, timeout);
			}
			bool result = false;
			Thread thread = new Thread((ThreadStart)delegate
			{
				result = WaitHandle.WaitAll(waitHandles, timeout);
			});
			thread.SetApartmentState(ApartmentState.MTA);
			thread.Start();
			thread.Join();
			return result;
		}
	}
}
