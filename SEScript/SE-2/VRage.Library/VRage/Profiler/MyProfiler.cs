using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using VRage.Collections;
using VRage.Library.Utils;

namespace VRage.Profiler
{
	/// <summary>
	/// Part of MyRenderProfiler, this is per-thread profiler
	/// </summary>
	[DebuggerDisplay("{DisplayName} Blocks({m_profilingBlocks.Count}) Tasks({FinishedTasks.Count})")]
	public class MyProfiler
	{
		public struct HistoryLock : IDisposable
		{
			private readonly MyProfiler m_profiler;

			private FastResourceLock m_lock;

			public HistoryLock(MyProfiler profiler, FastResourceLock historyLock)
			{
				m_profiler = profiler;
				m_lock = historyLock;
				m_lock.AcquireExclusive();
				m_profiler.OnHistorySafe();
			}

			public void Dispose()
			{
				m_profiler.OnHistorySafe();
				m_lock.ReleaseExclusive();
				m_lock = null;
			}
		}

		public enum TaskType
		{
			None = 0,
			Wait = 1,
			SyncWait = 2,
			WorkItem = 3,
			Block = 4,
			Physics = 5,
			RenderCull = 6,
			Voxels = 7,
			Precalc = 8,
			Deformations = 9,
			PreparePass = 10,
			RenderPass = 11,
			ClipMap = 12,
			HK_Schedule = 101,
			HK_Execute = 102,
			HK_AwaitTasks = 103,
			HK_Finish = 104,
			HK_JOB_TYPE_DYNAMICS = 105,
			HK_JOB_TYPE_COLLIDE = 106,
			HK_JOB_TYPE_COLLISION_QUERY = 107,
			HK_JOB_TYPE_RAYCAST_QUERY = 108,
			HK_JOB_TYPE_DESTRUCTION = 109,
			HK_JOB_TYPE_CHARACTER_PROXY = 110,
			HK_JOB_TYPE_COLLIDE_STATIC_COMPOUND = 111,
			HK_JOB_TYPE_OTHER = 112
		}

		public struct TaskInfo
		{
			public long Started;

			public long Finished;

			public long Scheduled;

			public string Name;

			public TaskType TaskType;

			public float CustomValue;
		}

		public class MyProfilerObjectBuilderInfo
		{
			public Dictionary<MyProfilerBlockKey, MyProfilerBlock> ProfilingBlocks;

			public List<MyProfilerBlock> RootBlocks;

			public string CustomName;

			public string AxisName;

			public int[] TotalCalls;

			public long[] CommitTimes;

			public bool ShallowProfile;

			public List<TaskInfo> Tasks;
		}

		public static long LastInterestingFrameTime;

		public static long LastFrameTime;

		private static readonly bool m_enableAsserts = true;

		public static readonly int MAX_FRAMES = 1024;

		public static readonly int UPDATE_WINDOW = 16;

		private const int ROOT_ID = 0;

		private int m_nextId = 1;

		private Dictionary<MyProfilerBlockKey, MyProfilerBlock> m_profilingBlocks = new Dictionary<MyProfilerBlockKey, MyProfilerBlock>(8192, new MyProfilerBlockKeyComparer());

		private List<MyProfilerBlock> m_rootBlocks = new List<MyProfilerBlock>(32);

		private readonly Stack<MyProfilerBlock> m_currentProfilingStack = new Stack<MyProfilerBlock>(1024);

		private int m_levelLimit = -1;

		private int m_levelSkipCount;

		private volatile int m_newLevelLimit = -1;

		private int m_remainingWindow = UPDATE_WINDOW;

		private readonly FastResourceLock m_historyLock = new FastResourceLock();

		public readonly object TaskLock = new object();

		private string m_customName;

		private string m_axisName;

		private readonly Dictionary<MyProfilerBlockKey, MyProfilerBlock> m_blocksToAdd = new Dictionary<MyProfilerBlockKey, MyProfilerBlock>(8192, new MyProfilerBlockKeyComparer());

		private volatile int m_lastFrameIndex;

		public int[] TotalCalls = new int[MAX_FRAMES];

		public long[] CommitTimes = new long[MAX_FRAMES];

		/// <summary>
		/// Enable for background workers.
		/// It will automatically commit after top level profiling block is closed
		/// </summary>
		public bool AutoCommit = true;

		public bool AutoScale;

		public bool IgnoreRoot;

		public bool AverageTimes;

		private bool AssertCommitFromOwningThread = true;

		public int ViewPriority;

		public bool EnableOptimizations = true;

		private int m_shallowMarker;

		public bool ShallowProfileEnabled;

		public volatile bool PendingShallowProfileState;

		public readonly bool AllocationProfiling;

		private readonly Thread m_ownerThread;

		public bool Paused;

		private readonly Stack<TaskInfo> m_runningTasks = new Stack<TaskInfo>(5);

		private readonly List<TaskInfo> m_pendingTasks = new List<TaskInfo>();

		public MyQueue<TaskInfo> FinishedTasks = new MyQueue<TaskInfo>(100);

		public MyProfilerBlock SelectedRoot
		{
			get;
			set;
		}

		public List<MyProfilerBlock> SelectedRootChildren
		{
			get
			{
				if (SelectedRoot == null)
				{
					return m_rootBlocks;
				}
				return SelectedRoot.Children;
			}
		}

		public List<MyProfilerBlock> RootBlocks => m_rootBlocks;

		public string DisplayName => m_customName;

		public string AxisName => m_axisName;

		public int LevelLimit => m_levelLimit;

		public Thread OwnerThread => m_ownerThread;

		private int GetParentId()
		{
			if (m_currentProfilingStack.Count > 0)
			{
				return m_currentProfilingStack.Peek().Id;
			}
			return 0;
		}

		public MyProfiler(bool allocationProfiling, string name, string axisName, bool shallowProfile, int viewPriority = 1000)
		{
			m_ownerThread = Thread.CurrentThread;
			m_customName = (name ?? m_ownerThread.Name);
			m_axisName = axisName;
			AllocationProfiling = allocationProfiling;
			PendingShallowProfileState = (ShallowProfileEnabled = shallowProfile);
			m_lastFrameIndex = MAX_FRAMES - 1;
			ViewPriority = viewPriority;
		}

		/// <summary>
		/// End operation on history data
		/// </summary>
		private void OnHistorySafe()
		{
			Interlocked.Exchange(ref m_remainingWindow, UPDATE_WINDOW);
		}

		public static MyProfilerBlock CreateExternalBlock(string name, int blockId)
		{
			MyProfilerBlockKey key = new MyProfilerBlockKey(string.Empty, string.Empty, name, 0, 0);
			MyProfilerBlock myProfilerBlock = new MyProfilerBlock();
			myProfilerBlock.SetBlockData(ref key, blockId);
			return myProfilerBlock;
		}

		public void SetNewLevelLimit(int newLevelLimit)
		{
			m_newLevelLimit = newLevelLimit;
		}

		public HistoryLock LockHistory(out int lastValidFrame)
		{
			HistoryLock result = new HistoryLock(this, m_historyLock);
			lastValidFrame = m_lastFrameIndex;
			return result;
		}

		/// <summary>
		/// Adds current frame to history and clear it
		/// Returns number of calls this frame
		/// </summary>
		public void CommitFrame()
		{
			CommitInternal();
		}

		private void CommitInternal()
		{
			_ = AssertCommitFromOwningThread;
			m_shallowMarker = 0;
			m_currentProfilingStack.Clear();
			if (m_blocksToAdd.Count > 0)
			{
				using (m_historyLock.AcquireExclusiveUsing())
				{
					foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> item in m_blocksToAdd)
					{
						if (item.Value.Parent != null)
						{
							item.Value.Parent.Children.AddOrInsert(item.Value, item.Value.ForceOrder);
						}
						else
						{
							m_rootBlocks.AddOrInsert(item.Value, item.Value.ForceOrder);
						}
						m_profilingBlocks.Add(item.Key, item.Value);
					}
					m_blocksToAdd.Clear();
					Interlocked.Exchange(ref m_remainingWindow, UPDATE_WINDOW - 1);
				}
			}
			else if (m_historyLock.TryAcquireExclusive())
			{
				Interlocked.Exchange(ref m_remainingWindow, UPDATE_WINDOW - 1);
				m_historyLock.ReleaseExclusive();
			}
			else if (Interlocked.Decrement(ref m_remainingWindow) < 0)
			{
				using (m_historyLock.AcquireExclusiveUsing())
				{
					Interlocked.Exchange(ref m_remainingWindow, UPDATE_WINDOW - 1);
				}
			}
			int num = 0;
			m_levelLimit = m_newLevelLimit;
			int num2 = (m_lastFrameIndex + 1) % MAX_FRAMES;
			foreach (MyProfilerBlock value in m_profilingBlocks.Values)
			{
				num += value.NumCalls;
				value.NumCallsArray[num2] = value.NumCalls;
				value.CustomValues[num2] = value.CustomValue;
				value.RawAllocations[num2] = value.Allocated;
				value.AverageMilliseconds = 0.9f * value.AverageMilliseconds + 0.1f * (float)value.Elapsed.Milliseconds;
				value.RawMilliseconds[num2] = (AverageTimes ? value.AverageMilliseconds : ((float)value.Elapsed.Milliseconds));
				value.Clear();
			}
			bool flag = m_pendingTasks.Count > 0;
			if (flag)
			{
				m_pendingTasks.SortNoAlloc((TaskInfo x, TaskInfo y) => x.Started.CompareTo(y.Started));
			}
			lock (TaskLock)
			{
				if (flag)
				{
					foreach (TaskInfo pendingTask in m_pendingTasks)
					{
						if (FinishedTasks.Count >= 10000000)
						{
							FinishedTasks.Dequeue();
						}
						FinishedTasks.Enqueue(pendingTask);
					}
					m_pendingTasks.Clear();
				}
				long lastInterestingFrameTime = LastInterestingFrameTime;
				while (FinishedTasks.Count > 0 && FinishedTasks.Peek().Finished < lastInterestingFrameTime)
				{
					FinishedTasks.Dequeue();
				}
			}
			m_lastFrameIndex = num2;
			TotalCalls[num2] = num;
			CommitTimes[num2] = Stopwatch.GetTimestamp();
			ShallowProfileEnabled = PendingShallowProfileState;
		}

		/// <summary>
		/// Clears current frame.
		/// </summary>
		public void ClearFrame()
		{
			_ = AssertCommitFromOwningThread;
			m_currentProfilingStack.Clear();
			if (m_blocksToAdd.Count > 0)
			{
				m_blocksToAdd.Clear();
			}
			m_levelLimit = m_newLevelLimit;
			foreach (MyProfilerBlock value in m_profilingBlocks.Values)
			{
				value.Clear();
			}
			m_pendingTasks.Clear();
		}

		public void Reset()
		{
			using (new HistoryLock(this, m_historyLock))
			{
				foreach (MyProfilerBlock value in m_profilingBlocks.Values)
				{
					value.AverageMilliseconds = 0f;
					for (int i = 0; i < MAX_FRAMES; i++)
					{
						value.CustomValues[i] = 0f;
						value.NumCallsArray[i] = 0;
						value.RawAllocations[i] = 0f;
						value.RawMilliseconds[i] = 0f;
					}
				}
				m_lastFrameIndex = MAX_FRAMES - 1;
			}
			lock (TaskLock)
			{
				FinishedTasks.Clear();
			}
		}

		public void InitMemoryHack(string name)
		{
			StartBlock(name, "InitMemoryHack", 0, string.Empty);
			MyProfilerBlock myProfilerBlock = m_currentProfilingStack.Peek();
			EndBlock("InitMemoryHack", 0, string.Empty);
			GC.GetTotalMemory(forceFullCollection: true);
			if (AllocationProfiling)
			{
				long workingSet = Environment.WorkingSet;
			}
		}

		public void StartBlock(string name, string memberName, int line, string file, int forceOrder = int.MaxValue, bool isDeepTreeRoot = false)
		{
			if ((m_levelLimit != -1 && m_currentProfilingStack.Count >= m_levelLimit) || (m_shallowMarker > 0 && ShallowProfileEnabled))
			{
				m_levelSkipCount++;
				return;
			}
			if (isDeepTreeRoot)
			{
				m_shallowMarker++;
			}
			MyProfilerBlock value = null;
			MyProfilerBlockKey key = new MyProfilerBlockKey(file, memberName, name, line, GetParentId());
			if (!m_profilingBlocks.TryGetValue(key, out value) && !m_blocksToAdd.TryGetValue(key, out value))
			{
				value = new MyProfilerBlock();
				value.SetBlockData(ref key, m_nextId++, forceOrder, isDeepTreeRoot);
				if (m_currentProfilingStack.Count > 0)
				{
					value.Parent = m_currentProfilingStack.Peek();
				}
				m_blocksToAdd.Add(key, value);
			}
			value.Start(AllocationProfiling);
			m_currentProfilingStack.Push(value);
		}

		[Conditional("DEBUG")]
		private static void CheckEndBlock(MyProfilerBlock profilingBlock, string member, string file, int parentId)
		{
			if ((!m_enableAsserts || profilingBlock.Key.Member.Equals(member)) && profilingBlock.Key.ParentId == parentId && !(profilingBlock.Key.File != file))
			{
				return;
			}
			StackTrace stackTrace = new StackTrace(2, fNeedFileInfo: true);
			for (int i = 0; i < stackTrace.FrameCount; i++)
			{
				StackFrame frame = stackTrace.GetFrame(i);
				if (frame.GetFileName() == profilingBlock.Key.File && frame.GetMethod().Name == member)
				{
					break;
				}
			}
		}

		public void EndBlock(string member, int line, string file, MyTimeSpan? customTime = null, float customValue = 0f, string timeFormat = null, string valueFormat = null, string callFormat = null)
		{
			if (m_levelSkipCount > 0)
			{
				m_levelSkipCount--;
				return;
			}
			if (m_currentProfilingStack.Count > 0)
			{
				MyProfilerBlock myProfilerBlock = m_currentProfilingStack.Pop();
				myProfilerBlock.CustomValue = Math.Max(myProfilerBlock.CustomValue, customValue);
				myProfilerBlock.TimeFormat = timeFormat;
				myProfilerBlock.ValueFormat = valueFormat;
				myProfilerBlock.CallFormat = callFormat;
				myProfilerBlock.End(AllocationProfiling, customTime);
				if (myProfilerBlock.IsDeepTreeRoot)
				{
					m_shallowMarker--;
				}
			}
			CommitWorklogIfNeeded();
		}

		private void CommitWorklogIfNeeded()
		{
			if (AutoCommit && m_currentProfilingStack.Count <= 0)
			{
				if (Paused)
				{
					ClearFrame();
				}
				else
				{
					CommitInternal();
				}
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public void ProfileCustomValue(string name, string member, int line, string file, float value, MyTimeSpan? customTime, string timeFormat, string valueFormat, string callFormat = null)
		{
			StartBlock(name, member, line, file);
			EndBlock(member, line, file, customTime, value, timeFormat, valueFormat, callFormat);
		}

		public void OnTaskStarted(TaskType taskType, string name, long scheduledTimestamp)
		{
			long timestamp = Stopwatch.GetTimestamp();
			m_runningTasks.Push(new TaskInfo
			{
				Name = name,
				TaskType = taskType,
				Started = timestamp,
				Scheduled = scheduledTimestamp
			});
		}

		public void OnTaskFinished(TaskType? taskType, float customValue)
		{
			if (m_runningTasks.Count != 0)
			{
				TaskInfo task = m_runningTasks.Pop();
				task.Finished = Stopwatch.GetTimestamp();
				task.CustomValue = customValue;
				if (taskType.HasValue)
				{
					task.TaskType = taskType.Value;
				}
				CommitTask(task);
			}
		}

		public void CommitTask(TaskInfo task)
		{
			m_pendingTasks.Add(task);
			if (m_runningTasks.Count == 0)
			{
				CommitWorklogIfNeeded();
			}
		}

		public StringBuilder Dump()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MyProfilerBlock rootBlock in m_rootBlocks)
			{
				rootBlock.Dump(stringBuilder, m_lastFrameIndex);
			}
			return stringBuilder;
		}

		public MyProfilerObjectBuilderInfo GetObjectBuilderInfo()
		{
			MyProfilerObjectBuilderInfo myProfilerObjectBuilderInfo = new MyProfilerObjectBuilderInfo
			{
				ProfilingBlocks = m_profilingBlocks,
				RootBlocks = m_rootBlocks,
				CustomName = m_customName,
				AxisName = m_axisName,
				TotalCalls = TotalCalls,
				ShallowProfile = ShallowProfileEnabled,
				CommitTimes = CommitTimes
			};
			lock (TaskLock)
			{
				myProfilerObjectBuilderInfo.Tasks = new List<TaskInfo>(FinishedTasks.Count);
				myProfilerObjectBuilderInfo.Tasks.AddRange(FinishedTasks);
				return myProfilerObjectBuilderInfo;
			}
		}

		public void SetShallowProfile(bool shallowProfile)
		{
			PendingShallowProfileState = shallowProfile;
		}

		public void Init(MyProfilerObjectBuilderInfo data)
		{
			m_profilingBlocks = data.ProfilingBlocks;
			foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock in m_profilingBlocks)
			{
				if (profilingBlock.Value.Id >= m_nextId)
				{
					m_nextId = profilingBlock.Value.Id + 1;
				}
			}
			m_rootBlocks = data.RootBlocks;
			m_customName = data.CustomName;
			m_axisName = data.AxisName;
			TotalCalls = data.TotalCalls;
			CommitTimes = (data.CommitTimes ?? new long[MAX_FRAMES]);
			PendingShallowProfileState = (ShallowProfileEnabled = data.ShallowProfile);
			FinishedTasks = new MyQueue<TaskInfo>(data.Tasks);
		}

		public void SubtractFrom(MyProfiler otherProfiler)
		{
			Dictionary<MyProfilerBlock, MyProfilerBlock> dictionary = new Dictionary<MyProfilerBlock, MyProfilerBlock>();
			foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock in m_profilingBlocks)
			{
				bool flag = false;
				foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock2 in otherProfiler.m_profilingBlocks)
				{
					if (profilingBlock2.Key.IsSimilarLocation(profilingBlock.Key))
					{
						MyProfilerBlock myProfilerBlock = profilingBlock.Value;
						MyProfilerBlock myProfilerBlock2 = profilingBlock2.Value;
						while (myProfilerBlock.Parent != null && myProfilerBlock2.Parent != null && myProfilerBlock.Parent.Key.IsSimilarLocation(myProfilerBlock2.Parent.Key))
						{
							myProfilerBlock = myProfilerBlock.Parent;
							myProfilerBlock2 = myProfilerBlock2.Parent;
						}
						if (myProfilerBlock.Parent == null && myProfilerBlock2.Parent == null)
						{
							flag = true;
							profilingBlock.Value.SubtractFrom(profilingBlock2.Value);
							dictionary.Add(profilingBlock2.Value, profilingBlock.Value);
							break;
						}
					}
				}
				if (!flag)
				{
					profilingBlock.Value.Invert();
				}
			}
			Stack<MyProfilerBlock> stack = new Stack<MyProfilerBlock>();
			foreach (KeyValuePair<MyProfilerBlockKey, MyProfilerBlock> profilingBlock3 in otherProfiler.m_profilingBlocks)
			{
				if (!dictionary.ContainsKey(profilingBlock3.Value))
				{
					MyProfilerBlock myProfilerBlock3 = profilingBlock3.Value;
					stack.Push(myProfilerBlock3);
					while (myProfilerBlock3.Parent != null && !dictionary.ContainsKey(myProfilerBlock3.Parent))
					{
						myProfilerBlock3 = myProfilerBlock3.Parent;
						stack.Push(myProfilerBlock3);
					}
					MyProfilerBlock myProfilerBlock4 = (myProfilerBlock3.Parent != null) ? dictionary[myProfilerBlock3.Parent] : null;
					while (stack.Count > 0)
					{
						MyProfilerBlock myProfilerBlock5 = stack.Pop();
						MyProfilerBlock myProfilerBlock6 = myProfilerBlock5.Duplicate(m_nextId++, myProfilerBlock4);
						if (myProfilerBlock4 == null)
						{
							m_rootBlocks.Add(myProfilerBlock6);
						}
						m_profilingBlocks.Add(myProfilerBlock6.Key, myProfilerBlock6);
						myProfilerBlock4 = myProfilerBlock6;
						dictionary.Add(myProfilerBlock5, myProfilerBlock6);
					}
					stack.Clear();
				}
			}
			for (int i = 0; i < MAX_FRAMES; i++)
			{
				TotalCalls[i] = otherProfiler.TotalCalls[i] - TotalCalls[i];
			}
		}
	}
}
