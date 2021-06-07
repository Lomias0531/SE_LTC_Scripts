using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Collections;
using VRage.Network;
using VRage.Profiler;

namespace ParallelTasks
{
	[GenerateActivator]
	public class WorkItem
	{
		private class ParallelTasks_WorkItem_003C_003EActor : IActivator, IActivator<WorkItem>
		{
			private sealed override object CreateInstance()
			{
				return new WorkItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override WorkItem CreateInstance()
			{
				return new WorkItem();
			}

			WorkItem IActivator<WorkItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static Action<Exception> ErrorReportingFunction;

		private IWork m_work;

		private int m_executing;

		private long m_scheduledTimestamp;

		private volatile int m_runCount;

		private List<Exception> m_exceptionBuffer;

		private object m_executionLock = new object();

		private readonly ManualResetEvent m_resetEvent;

		private Exception[] m_exceptions;

		private static readonly MyConcurrentPool<WorkItem> m_idleWorkItems = new MyConcurrentPool<WorkItem>(1000, null, 100000);

		private static readonly ConcurrentDictionary<Thread, Stack<Task>> m_runningTasks = new ConcurrentDictionary<Thread, Stack<Task>>(Environment.ProcessorCount, Environment.ProcessorCount);

		public const string PerformanceProfilingSymbol = "__RANDOM_UNDEFINED_PROFILING_SYMBOL__";

		private static Action<MyProfiler.TaskType, string, long> m_onTaskStartedDelegate = delegate
		{
		};

		private static Action m_onTaskFinishedDelegate = delegate
		{
		};

		private static Action<string> m_onProfilerBeginDelegate = delegate
		{
		};

		private static Action<float> m_onProfilerEndDelegate = delegate
		{
		};

		private static Action<int> m_initThread = delegate
		{
		};

		public IWork Work => m_work;

		public WorkData WorkData
		{
			get;
			set;
		}

		public Action Callback
		{
			get;
			set;
		}

		public Action<WorkData> DataCallback
		{
			get;
			set;
		}

		public ConcurrentCachingList<WorkItem> CompletionCallbacks
		{
			get;
			set;
		}

		public int RunCount => m_runCount;

		public static Stack<Task> ThisThreadTasks
		{
			get
			{
				Thread currentThread = Thread.CurrentThread;
				if (!m_runningTasks.TryGetValue(currentThread, out Stack<Task> value))
				{
					value = new Stack<Task>(5);
					m_runningTasks.TryAdd(currentThread, value);
				}
				return value;
			}
		}

		public static Task? CurrentTask
		{
			get
			{
				Stack<Task> thisThreadTasks = ThisThreadTasks;
				if (thisThreadTasks.Count == 0)
				{
					return null;
				}
				return thisThreadTasks.Peek();
			}
		}

		public WorkItem()
		{
			m_resetEvent = new ManualResetEvent(initialState: true);
		}

		public Task PrepareStart(IWork work, Thread thread = null)
		{
			if (m_exceptions != null)
			{
				m_exceptions = null;
			}
			m_work = work;
			m_resetEvent.Reset();
			return new Task(this);
		}

		public bool DoWork(int expectedID)
		{
			lock (m_executionLock)
			{
				if (expectedID < m_runCount)
				{
					return true;
				}
				if (m_work == null)
				{
					return false;
				}
				if (m_executing == m_work.Options.MaximumThreads)
				{
					return false;
				}
				m_executing++;
			}
			Stack<Task> thisThreadTasks = ThisThreadTasks;
			thisThreadTasks.Push(new Task(this));
			try
			{
				m_work.DoWork(WorkData);
			}
			catch (Exception ex)
			{
				if (Parallel.THROW_WORKER_EXCEPTIONS)
				{
					ErrorReportingFunction(ex);
					throw;
				}
				if (m_exceptionBuffer == null)
				{
					List<Exception> value = new List<Exception>();
					Interlocked.CompareExchange(ref m_exceptionBuffer, value, null);
				}
				lock (m_exceptionBuffer)
				{
					m_exceptionBuffer.Add(ex);
				}
			}
			thisThreadTasks.Pop();
			lock (m_executionLock)
			{
				m_executing--;
				if (m_executing == 0)
				{
					if (m_exceptionBuffer != null)
					{
						m_exceptions = m_exceptionBuffer.ToArray();
						m_exceptionBuffer = null;
					}
					m_runCount++;
					m_resetEvent.Set();
					if (Callback == null && DataCallback == null)
					{
						Requeue();
					}
					else
					{
						CompletionCallbacks.Add(this);
					}
					return true;
				}
				return false;
			}
		}

		public void Requeue()
		{
			if (m_runCount < int.MaxValue && m_exceptions == null)
			{
				m_work = null;
				m_idleWorkItems.Return(this);
			}
		}

		public Exception[] GetExceptions(int runId)
		{
			lock (m_executionLock)
			{
				return GetExceptionsInternal(runId);
			}
		}

		public void WaitOrExecute(int id, bool blocking = false)
		{
			WaitOrExecuteInternal(id, blocking);
			ThrowExceptionsInternal(id);
		}

		public void Execute(int id)
		{
			if (m_runCount == id && DoWork(id))
			{
				ThrowExceptionsInternal(id);
			}
		}

		private void WaitOrExecuteInternal(int id, bool blocking = false)
		{
			if (m_runCount == id && !DoWork(id))
			{
				Wait(id, blocking);
			}
		}

		private void ThrowExceptionsInternal(int runId)
		{
			Exception[] exceptionsInternal = GetExceptionsInternal(runId);
			if (exceptionsInternal != null)
			{
				throw new TaskException(exceptionsInternal);
			}
		}

		public Exception[] GetExceptionsInternal(int runId)
		{
			int runCount = m_runCount;
			if (m_exceptions != null && runCount == runId + 1)
			{
				return m_exceptions;
			}
			return null;
		}

		public static WorkItem Get()
		{
			return m_idleWorkItems.Get();
		}

		public static void Clean()
		{
			m_idleWorkItems.Clean();
		}

		public void Wait(int id, bool blocking)
		{
			if (m_runCount == id)
			{
				try
				{
					if (blocking)
					{
						while (m_runCount == id)
						{
						}
					}
					else
					{
						SpinWait spinWait = default(SpinWait);
						while (m_runCount == id)
						{
							if (spinWait.Count > 1000)
							{
								m_resetEvent.WaitOne();
							}
							else
							{
								spinWait.SpinOnce();
							}
						}
					}
				}
				finally
				{
				}
			}
		}

		public static void SetupProfiler(Action<MyProfiler.TaskType, string, long> onTaskStarted, Action onTaskFinished, Action<string> begin, Action<float> end, Action<int> initThread)
		{
			m_onTaskStartedDelegate = onTaskStarted;
			m_onTaskFinishedDelegate = onTaskFinished;
			m_onProfilerBeginDelegate = begin;
			m_onProfilerEndDelegate = end;
			m_initThread = initThread;
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private static void OnTaskScheduled(WorkItem task)
		{
			task.m_scheduledTimestamp = Stopwatch.GetTimestamp();
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnTaskStarted(WorkItem task)
		{
			WorkOptions options = task.Work.Options;
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnTaskFinished(WorkItem task)
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnTaskStarted(MyProfiler.TaskType taskType, string debugName, long scheduledTimestamp = -1L)
		{
			m_onTaskStartedDelegate(taskType, debugName, scheduledTimestamp);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnTaskFinished()
		{
			m_onTaskFinishedDelegate();
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void ProfilerBegin(string symbol)
		{
			m_onProfilerBeginDelegate(symbol);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void ProfilerEnd(float customValue = 0f)
		{
			m_onProfilerEndDelegate(customValue);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void InitThread(int priority)
		{
			m_initThread(priority);
		}
	}
}
