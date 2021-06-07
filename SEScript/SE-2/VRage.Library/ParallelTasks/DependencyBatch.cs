using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Profiler;

namespace ParallelTasks
{
	public class DependencyBatch : AbstractWork, IPrioritizedWork, IWork, IDisposable
	{
		private static class JobState
		{
			public const int DependencyPending = 2147483643;

			public const int Scheduled = 2147483644;

			public const int Running = 2147483645;

			public const int Finished = 2147483646;
		}

		private static class ControlThreadState
		{
			public const int Waiting = 0;

			public const int Scheduled = 1;

			public const int Running = 2;

			public const int Exit = 3;
		}

		public struct StartToken : IDisposable
		{
			private readonly int m_jobId;

			private readonly DependencyBatch m_batch;

			private int m_writeOffset;

			public StartToken(int jobId, DependencyBatch batch)
			{
				m_jobId = jobId;
				m_batch = batch;
				m_writeOffset = batch.m_dependencyStarts[jobId];
			}

			/// <summary>
			/// Marks task to be executed only after `this` task is done
			/// </summary>
			public void Starts(int jobId)
			{
				int[] array = m_batch.m_dependencies;
				int num = m_writeOffset++;
				if (array.Length <= num)
				{
					Array.Resize(ref array, array.Length * 2);
					m_batch.m_dependencies = array;
				}
				array[num] = jobId;
			}

			public void Dispose()
			{
				m_batch.m_dependencyStarts[m_jobId + 1] = m_writeOffset;
			}
		}

		private class ParallelTasks_DependencyBatch_003C_003EActor
		{
		}

		public static Action<Exception> ErrorReportingFunction;

		private int m_maxThreads;

		private int m_jobCount;

		private int m_completedJobs;

		private Action[] m_jobs;

		private int[] m_jobStates;

		private int[] m_dependencies;

		private int[] m_dependencyStarts;

		private List<Exception> m_exceptionBuffer;

		private readonly AutoResetEvent m_completionAwaiter;

		private int m_controlThreadState = 3;

		private const int WORKERS_MASK = 65535;

		private const int JOBS_MASK = -65536;

		private const int JOBS_SHIFT = 16;

		private long m_scheduledJobsAndWorkers;

		private int m_allCompletedCachedIndex;

		public WorkPriority Priority
		{
			get;
			private set;
		}

		WorkOptions IWork.Options
		{
			get
			{
				WorkOptions options = base.Options;
				options.MaximumThreads = 1;
				return options;
			}
		}

		public int MaxThreads => m_maxThreads;

		public sealed override WorkOptions Options
		{
			get
			{
				return base.Options;
			}
			set
			{
				base.Options = value;
				m_maxThreads = Math.Min(value.MaximumThreads, Parallel.Scheduler.ThreadCount + 1);
			}
		}

		/// <summary>
		/// <para>
		/// Adds new job for execution.
		/// </para>
		///
		/// <para>
		/// !!! All jobs need to be scheduled before the batch is started !!!
		/// </para>
		///
		/// <para>
		/// Note: In case there is multiple dependency-free jobs ready to be executed scheduler prefers jobs in order in which they were added to batch.
		///       Use that to you advantage when optimizing for cache coherency for example.
		/// </para>
		/// </summary>
		/// <returns>Job id. See <see cref="M:ParallelTasks.DependencyBatch.Job(System.Int32)" />.<see cref="M:ParallelTasks.DependencyBatch.StartToken.Starts(System.Int32)" /></returns>
		public int Add(Action job)
		{
			int num = m_jobCount++;
			EnsureCapacity(m_jobCount);
			m_jobs[num] = job;
			return num;
		}

		/// <summary>
		/// Entry point of dependency fluent API.
		/// !!! Jobs needs to be queried in ascending order !!!
		/// </summary>
		/// <returns>Dependency fluent API</returns>
		public StartToken Job(int jobId)
		{
			if (m_dependencyStarts[jobId] == -1)
			{
				m_dependencyStarts[jobId] = GetLastInitializedStart(jobId - 1);
			}
			return new StartToken(jobId, this);
		}

		public void Preallocate(int size)
		{
			if (m_jobs == null || m_jobs.Length < size)
			{
				AllocateInternal(size);
			}
			Clear(m_jobs.Length);
		}

		private void AllocateInternal(int size)
		{
			m_jobs = new Action[size];
			m_jobStates = new int[size];
			m_dependencyStarts = new int[size + 1];
			if (m_dependencies == null || m_dependencies.Length < size)
			{
				m_dependencies = new int[size];
			}
		}

		public void Execute()
		{
			m_completedJobs = 0;
			if (m_jobCount == 0)
			{
				return;
			}
			int num = m_jobCount;
			int num2 = GetLastInitializedStart(m_jobCount) - 1;
			for (int i = 0; i <= num2; i++)
			{
				int num3 = m_dependencies[i];
				int num4 = m_jobStates[num3];
				if (num4 == 2147483644)
				{
					num--;
				}
				m_jobStates[num3] = num4 - 1;
			}
			m_controlThreadState = 0;
			RegisterJobsForConsumption(num);
			int num5 = Interlocked.CompareExchange(ref m_controlThreadState, 2, 1);
			m_completionAwaiter.Reset();
			if (num5 != 3)
			{
				do
				{
					WorkerLoop();
				}
				while (!MainThreadAwaiter());
			}
			try
			{
				if (m_exceptionBuffer != null && m_exceptionBuffer.Count > 0)
				{
					throw new TaskException(m_exceptionBuffer.ToArray());
				}
			}
			finally
			{
				Clear(m_jobCount);
			}
		}

		private bool MainThreadAwaiter()
		{
			int num = Interlocked.CompareExchange(ref m_controlThreadState, 0, 2);
			if (num == 3)
			{
				return true;
			}
			m_completionAwaiter.WaitOne();
			num = Interlocked.CompareExchange(ref m_controlThreadState, 2, 1);
			if (num == 3)
			{
				return true;
			}
			return false;
		}

		private void ReleaseMainThread()
		{
			int num = 2;
			bool flag;
			bool flag2;
			while (true)
			{
				flag = false;
				flag2 = false;
				switch (num)
				{
				default:
					return;
				case 3:
					return;
				case 0:
					flag = true;
					break;
				case 1:
					flag2 = true;
					break;
				case 2:
					break;
				}
				int num2 = Interlocked.CompareExchange(ref m_controlThreadState, 3, num);
				if (num2 == num)
				{
					break;
				}
				num = num2;
			}
			if (flag2)
			{
				bool flag3 = TryAcquireJob();
			}
			if (flag)
			{
				m_completionAwaiter.Set();
			}
		}

		private bool TryWakingUpMainThread()
		{
			if (Interlocked.CompareExchange(ref m_controlThreadState, 1, 0) == 0)
			{
				m_completionAwaiter.Set();
				return true;
			}
			return false;
		}

		private void WorkerLoop()
		{
			if (!TryAcquireJob())
			{
				return;
			}
			do
			{
				IL_0009:
				Exception e = null;
				int num = ExecuteSingleJob(ref e);
				if (e != null)
				{
					if (m_exceptionBuffer == null)
					{
						List<Exception> value = new List<Exception>();
						Interlocked.CompareExchange(ref m_exceptionBuffer, value, null);
					}
					lock (m_exceptionBuffer)
					{
						m_exceptionBuffer.Add(e);
					}
				}
				if (num > 0)
				{
					if (num > 1)
					{
						RegisterJobsForConsumption(num - 1);
						goto IL_0009;
					}
					int num2 = 65536;
					Interlocked.Add(ref m_scheduledJobsAndWorkers, num2);
					if (TryWakingUpMainThread())
					{
						break;
					}
				}
			}
			while (TryAcquireJob());
		}

		private int ExecuteSingleJob(ref Exception e)
		{
			int num = -1;
			while (true)
			{
				bool flag = false;
				int i;
				for (i = m_allCompletedCachedIndex; i < m_jobCount && m_jobStates[i] >= 2147483645; i++)
				{
					flag = true;
				}
				if (flag)
				{
					int num2 = i - 1;
					int allCompletedCachedIndex = m_allCompletedCachedIndex;
					if (allCompletedCachedIndex < num2)
					{
						Interlocked.CompareExchange(ref m_allCompletedCachedIndex, num2, allCompletedCachedIndex);
					}
				}
				while (true)
				{
					if (i < m_jobCount)
					{
						if (m_jobStates[i] != 2147483644)
						{
							goto IL_0095;
						}
						int num3 = Interlocked.CompareExchange(ref m_jobStates[i], 2147483645, 2147483644);
						if (num3 != 2147483644)
						{
							goto IL_0095;
						}
						num = i;
					}
					else
					{
						if (Volatile.Read(ref m_completedJobs) < m_jobCount)
						{
							break;
						}
						if (num == -1)
						{
							return 0;
						}
					}
					try
					{
						m_jobs[num]();
					}
					catch (Exception ex)
					{
						ErrorReportingFunction?.Invoke(ex);
						e = ex;
					}
					Volatile.Write(ref m_jobStates[num], 2147483646);
					int num4 = m_dependencyStarts[num];
					int num5 = m_dependencyStarts[num + 1];
					int num6;
					if (num4 == -1 || num5 == -1)
					{
						num6 = 0;
					}
					else
					{
						num6 = num5 - num4;
						for (int j = num4; j < num5; j++)
						{
							int num7 = Interlocked.Increment(ref m_jobStates[m_dependencies[j]]);
							if (num7 != 2147483644)
							{
								num6--;
							}
						}
					}
					int num8 = Interlocked.Increment(ref m_completedJobs);
					if (num8 == m_jobCount)
					{
						ReleaseMainThread();
					}
					return num6;
					IL_0095:
					i++;
				}
			}
		}

		private void RegisterJobsForConsumption(int count)
		{
			long num = m_scheduledJobsAndWorkers;
			int num5;
			while (true)
			{
				int num2 = (int)((num & -65536) >> 16);
				long num3 = num & 0xFFFF;
				int num4 = Math.Min(m_maxThreads, (int)num3 + count);
				num5 = num4 - (int)num3;
				long num6 = num3 + num5;
				long num7 = (long)(num2 + count) << 16;
				long num8 = Interlocked.CompareExchange(ref m_scheduledJobsAndWorkers, num7 | num6, num);
				if (num8 == num)
				{
					break;
				}
				num = num8;
			}
			if (num5 <= 0)
			{
				return;
			}
			if (TryWakingUpMainThread())
			{
				num5--;
			}
			if (num5 > 0)
			{
				for (int i = 0; i < num5; i++)
				{
					Parallel.Start(this);
				}
			}
		}

		private bool TryAcquireJob()
		{
			long num = m_scheduledJobsAndWorkers;
			bool flag;
			while (true)
			{
				long num2 = num & 0xFFFF;
				long num3 = num & -65536;
				flag = (num3 != 0);
				if (flag)
				{
					num3 >>= 16;
					num3--;
					num3 <<= 16;
				}
				else
				{
					num2--;
				}
				long num4 = Interlocked.CompareExchange(ref m_scheduledJobsAndWorkers, num3 | num2, num);
				if (num4 == num)
				{
					break;
				}
				num = num4;
			}
			return flag;
		}

		public void Clear(int length)
		{
			m_jobCount = 0;
			m_completedJobs = 0;
			m_dependencyStarts[0] = 0;
			m_allCompletedCachedIndex = 0;
			if (m_exceptionBuffer != null)
			{
				m_exceptionBuffer.Clear();
			}
			for (int num = length - 1; num >= 0; num--)
			{
				m_jobs[num] = null;
				m_dependencyStarts[num + 1] = -1;
				m_jobStates[num] = 2147483644;
			}
		}

		private int GetLastInitializedStart(int maxIndex)
		{
			int num;
			while (true)
			{
				num = m_dependencyStarts[maxIndex];
				if (num != -1)
				{
					break;
				}
				maxIndex--;
			}
			return num;
		}

		public DependencyBatch(WorkPriority priority = WorkPriority.Normal)
		{
			Priority = priority;
			m_completionAwaiter = new AutoResetEvent(initialState: false);
			WorkOptions defaultOptions = Parallel.DefaultOptions;
			defaultOptions.MaximumThreads = int.MaxValue;
			Options = defaultOptions.WithDebugInfo(MyProfiler.TaskType.Wait, "Batch");
		}

		public override void DoWork(WorkData workData = null)
		{
			WorkerLoop();
		}

		private void EnsureCapacity(int size)
		{
			int num = (m_jobs != null) ? m_jobs.Length : 0;
			if (num < size)
			{
				Action[] jobs = m_jobs;
				int[] jobStates = m_jobStates;
				int[] dependencies = m_dependencies;
				int[] dependencyStarts = m_dependencyStarts;
				AllocateInternal((m_jobs == null) ? 50 : (num * 2));
				if (jobs != null)
				{
					Array.Copy(jobs, m_jobs, num);
					Array.Copy(jobStates, m_jobStates, num);
					Array.Copy(dependencyStarts, m_dependencyStarts, num + 1);
				}
				for (int i = num; i < m_jobStates.Length; i++)
				{
					m_dependencyStarts[i + 1] = -1;
					m_jobStates[i] = 2147483644;
				}
				if (m_dependencies != dependencies && dependencies != null)
				{
					Array.Copy(dependencies, m_dependencies, dependencies.Length);
				}
			}
		}

		[Conditional("DEBUG")]
		private void AssertDependencyOrder(int currentJobId)
		{
			for (int i = currentJobId + 1; i < m_dependencyStarts.Length; i++)
			{
			}
		}

		[Conditional("DEBUG")]
		private void AssertExecutionConsistency()
		{
			int num = m_dependencyStarts[0];
			for (int i = 1; i <= m_jobCount; i++)
			{
				int num2 = m_dependencyStarts[i];
				if (num2 != -1)
				{
					num = num2;
				}
			}
			if (num > 0)
			{
				for (int j = 0; j < num; j++)
				{
				}
			}
		}

		public void Dispose()
		{
			m_completionAwaiter.Dispose();
		}
	}
}
