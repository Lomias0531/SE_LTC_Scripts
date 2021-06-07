using ParallelTasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Voxels;
using VRage.Generics;
using VRage.Library.Threading;
using VRage.Profiler;
using VRage.Voxels.DualContouring;

namespace VRage.Voxels
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MyPrecalcComponent : MySessionComponentBase
	{
		private class MyPrecalcJobComparer : IComparer<MyPrecalcJob>
		{
			public int Compare(MyPrecalcJob x, MyPrecalcJob y)
			{
				return ((int)y.Priority).CompareTo(x.Priority);
			}
		}

		private class Work : IPrioritizedWork, IWork
		{
			private readonly List<MyPrecalcJob> m_finishedList = new List<MyPrecalcJob>();

			private readonly Stopwatch m_timer = new Stopwatch();

			public long MaxPrecalcTime;

			public readonly Action CompletionCallback;

			public MyPrecalcComponent Parent;

			public WorkPriority Priority
			{
				get;
				set;
			}

			WorkOptions IWork.Options => Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Precalc, "Precalc");

			public bool ShouldRequeue
			{
				get
				{
					if (Parent.Loaded && Parent.m_workQueue.Count > 0)
					{
						Interlocked.Increment(ref Parent.m_activeWorkers);
						return true;
					}
					return false;
				}
			}

			public Work()
			{
				CompletionCallback = OnComplete;
			}

			private void OnComplete()
			{
				Parent.m_workPool.Deallocate(this);
				Parent = null;
			}

			void IWork.DoWork(WorkData workData)
			{
				m_timer.Start();
				try
				{
					MyPrecalcJob job;
					while (Parent.Loaded && Parent.TryDequeue(out job))
					{
						if (job.IsCanceled && job.OnCompleteDelegate != null)
						{
							m_finishedList.Add(job);
						}
						else
						{
							job.DoWorkInternal();
							if (job.OnCompleteDelegate != null)
							{
								m_finishedList.Add(job);
							}
							if (m_timer.ElapsedTicks >= MaxPrecalcTime)
							{
								break;
							}
						}
					}
					Parent.m_finishedJobs.AddRange(m_finishedList);
					m_finishedList.Clear();
				}
				finally
				{
					Interlocked.Decrement(ref Parent.m_activeWorkers);
				}
				m_timer.Reset();
			}
		}

		private static bool MULTITHREADED = true;

		private static Type m_isoMesherType = typeof(MyDualContouringMesher);

		/// <summary>
		/// Maximum calculation time in milliseconds
		/// </summary>
		public static long MaxPrecalcTime = 20L;

		/// <summary>
		/// Enable debug draw.
		/// </summary>
		public static bool DebugDrawSorted = false;

		private static MyPrecalcComponent m_instance;

		private static SpinLockRef m_queueLock = new SpinLockRef();

		[ThreadStatic]
		private static IMyIsoMesher m_isoMesher;

		public static int UpdateThreadManagedId;

		private static readonly MyPrecalcJobComparer m_comparer = new MyPrecalcJobComparer();

		private readonly MyConcurrentQueue<MyPrecalcJob> m_workQueue = new MyConcurrentQueue<MyPrecalcJob>();

		private readonly MyConcurrentList<MyPrecalcJob> m_finishedJobs = new MyConcurrentList<MyPrecalcJob>();

		private MyDynamicObjectPool<Work> m_workPool;

		private volatile int m_activeWorkers;

		/// <summary>
		/// The IsoMesher type used by precalc jobs.
		/// </summary>
		public static Type IsoMesherType
		{
			get
			{
				return m_isoMesherType;
			}
			set
			{
				if (typeof(IMyIsoMesher).IsAssignableFrom(m_isoMesherType))
				{
					m_isoMesherType = value;
				}
			}
		}

		public static IMyIsoMesher IsoMesher => m_isoMesher ?? (m_isoMesher = (IMyIsoMesher)Activator.CreateInstance(IsoMesherType));

		public static int InvalidatedRangeInflate => IsoMesher.InvalidatedRangeInflate;

		public MyPrecalcComponent()
		{
			base.UpdateOnPause = true;
		}

		public static bool EnqueueBack(MyPrecalcJob job)
		{
			using (m_queueLock.Acquire())
			{
				if (m_instance != null)
				{
					m_instance.Enqueue(job);
					return true;
				}
				return false;
			}
		}

		[Conditional("DEBUG")]
		public static void AssertUpdateThread()
		{
		}

		public override void LoadData()
		{
			base.LoadData();
			m_instance = this;
			if (!MULTITHREADED)
			{
				MaxPrecalcTime = 6L;
			}
			m_workPool = new MyDynamicObjectPool<Work>(Parallel.Scheduler.ThreadCount);
		}

		protected override void UnloadData()
		{
			using (m_queueLock.Acquire())
			{
				base.UnloadData();
				m_instance = null;
			}
			while (m_activeWorkers > 0)
			{
				Thread.Yield();
			}
			foreach (MyPrecalcJob item in m_finishedJobs.Concat(m_workQueue))
			{
				item.Cancel();
				if (item.OnCompleteDelegate != null)
				{
					item.OnCompleteDelegate();
				}
			}
			m_finishedJobs.Clear();
			m_workQueue.Clear();
		}

		private void Enqueue(MyPrecalcJob job)
		{
			job.Started = false;
			m_workQueue.Enqueue(job);
		}

		private bool TryDequeue(out MyPrecalcJob job)
		{
			return m_workQueue.TryDequeue(out job);
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			UpdateQueue();
		}

		public override bool UpdatedBeforeInit()
		{
			return true;
		}

		public void UpdateQueue()
		{
			bool flag = false;
			MyPrecalcJob instance = null;
			while (m_workQueue.TryPeek(out instance))
			{
				if (instance.IsCanceled)
				{
					bool flag2 = false;
					if (m_workQueue.TryDequeue(out MyPrecalcJob instance2))
					{
						flag2 = (instance == instance2);
					}
					if (flag2)
					{
						if (instance.OnCompleteDelegate != null)
						{
							m_finishedJobs.Add(instance);
						}
						continue;
					}
					flag = false;
					if (instance2 != null)
					{
						m_workQueue.Enqueue(instance2);
					}
					continue;
				}
				flag = true;
				break;
			}
			if (flag)
			{
				while (m_workPool.Count > 0)
				{
					Work work = m_workPool.Allocate();
					work.Parent = this;
					work.Priority = WorkPriority.Low;
					work.MaxPrecalcTime = MaxPrecalcTime * 10000;
					Interlocked.Increment(ref m_activeWorkers);
					if (MULTITHREADED)
					{
						Parallel.Start(work, work.CompletionCallback);
						continue;
					}
					((IWork)work).DoWork((WorkData)null);
					work.CompletionCallback();
				}
			}
			while (m_finishedJobs.TryDequeueBack(out instance))
			{
				instance.OnCompleteDelegate();
			}
		}
	}
}
