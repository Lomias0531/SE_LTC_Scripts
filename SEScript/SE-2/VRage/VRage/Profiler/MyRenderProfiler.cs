using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage.FileSystem;
using VRage.Library.Utils;
using VRageMath;

namespace VRage.Profiler
{
	/// <summary>
	/// Provides profiling capability
	/// </summary>
	/// <remarks>
	/// Non-locking way of render profiler is used. Each thread has it's own profiler is ThreadStatic variable.
	/// Data for each profiling block are of two kinds: Immediate (current frame being profiled) and History (previous finished frames)
	/// Start/End locking is not necessary, because Start/Stop uses only immediate data and nothing else uses it at the moment.
	/// Commit is only other place which uses Immediate data, but it must be called from same thead, no racing condition.
	/// Draw and Commit both uses History data, and both can be called from different thread, so there's lock.
	/// This way everything runs with no waiting, unless Draw obtains lock in which case Commit wait for Draw to finish (Start/End is still exact).
	///
	/// For threads which does not call commit (background workers, parallel tasks), mechanism which calls commit automatically after each top level End should be added.
	/// This way each task will be one "frame" on display
	/// </remarks>
	public abstract class MyRenderProfiler
	{
		public struct FrameInfo
		{
			public long Time;

			public long FrameNumber;
		}

		private struct MyStats
		{
			public float Min;

			public float Max;

			public int MinCount;

			public int MaxCount;

			public bool Any;
		}

		private static readonly object m_pauseLock;

		/// <summary>
		/// Sorting order will sort the listed elements in the profiler by the specified ProfilerSortingOrder
		/// </summary>
		protected static RenderProfilerSortingOrder m_sortingOrder;

		protected static ProfilerGraphContent m_graphContent;

		protected static BlockRender m_blockRender;

		protected static SnapshotType m_dataType;

		public const string PERFORMANCE_PROFILING_SYMBOL = "__RANDOM_UNDEFINED_PROFILING_SYMBOL__";

		private static bool m_profilerProcessingEnabled;

		public static bool ShallowProfileOnly;

		public static bool AverageTimes;

		protected static readonly MyDrawArea MemoryGraphScale;

		private static readonly MyDrawArea m_milisecondsGraphScale;

		private static readonly MyDrawArea m_allocationsGraphScale;

		private static readonly Color[] m_colors;

		protected readonly StringBuilder Text = new StringBuilder(100);

		protected const bool ALLOCATION_PROFILING = false;

		protected static readonly MyProfilerBlock FpsBlock;

		protected static float m_fpsPctg;

		private static int m_pauseCount;

		public static bool Paused;

		public static Action GetProfilerFromServer;

		public static Action<int> SaveProfilerToFile;

		public static Action<int, bool> LoadProfilerFromFile;

		[ThreadStatic]
		private static MyProfiler m_threadProfiler;

		private static MyProfiler m_gpuProfiler;

		public static List<MyProfiler> ThreadProfilers;

		private static readonly List<MyProfiler> m_onlineThreadProfilers;

		protected static MyProfiler m_selectedProfiler;

		protected static bool m_enabled;

		protected static int m_selectedFrame;

		private static int m_levelLimit;

		protected static bool m_useCustomFrame;

		protected static int m_frameLocalArea;

		private int m_currentDumpNumber;

		protected static long m_targetTaskRenderTime;

		protected static long m_taskRenderDispersion;

		public static ConcurrentQueue<FrameInfo> FrameTimestamps;

		private static readonly ConcurrentQueue<FrameInfo> m_onlineFrameTimestamps;

		private static MyTimeSpan m_nextAutoScale;

		private static readonly MyTimeSpan AUTO_SCALE_UPDATE;

		protected static bool ProfilerProcessingEnabled => m_profilerProcessingEnabled;

		/// <summary>
		/// Returns true when profiler is visible.
		/// </summary>
		public static bool ProfilerVisible => m_enabled;

		private static MyProfiler GpuProfiler
		{
			get
			{
				MyProfiler gpuProfiler = m_gpuProfiler;
				if (gpuProfiler == null)
				{
					gpuProfiler = (m_gpuProfiler = CreateProfiler("GPU"));
					gpuProfiler.ViewPriority = 30;
					lock (ThreadProfilers)
					{
						SortProfilersLocked();
						return gpuProfiler;
					}
				}
				return gpuProfiler;
			}
		}

		public static MyProfiler ThreadProfiler => m_threadProfiler ?? (m_threadProfiler = CreateProfiler(null));

		public static MyProfiler SelectedProfiler
		{
			get
			{
				return m_selectedProfiler;
			}
			set
			{
				m_selectedProfiler = value;
			}
		}

		protected static Color IndexToColor(int index)
		{
			return m_colors[index % m_colors.Length];
		}

		static MyRenderProfiler()
		{
			m_pauseLock = new object();
			m_sortingOrder = RenderProfilerSortingOrder.MillisecondsLastFrame;
			m_graphContent = ProfilerGraphContent.Elapsed;
			m_blockRender = BlockRender.Name;
			m_dataType = SnapshotType.Online;
			m_profilerProcessingEnabled = false;
			ShallowProfileOnly = true;
			AverageTimes = false;
			MemoryGraphScale = new MyDrawArea(0.49f, 0f, 0.745f, 0.6f, 0.001f);
			m_milisecondsGraphScale = new MyDrawArea(0.49f, 0f, 0.745f, 0.9f, 25f);
			m_allocationsGraphScale = new MyDrawArea(0.49f, 0f, 0.745f, 0.9f, 25000f);
			m_colors = new Color[19]
			{
				new Color(0, 192, 192),
				Color.Orange,
				Color.BlueViolet * 1.5f,
				Color.BurlyWood,
				Color.Chartreuse,
				Color.CornflowerBlue,
				Color.Cyan,
				Color.ForestGreen,
				Color.Fuchsia,
				Color.Gold,
				Color.GreenYellow,
				Color.LightBlue,
				Color.LightGreen,
				Color.LimeGreen,
				Color.Magenta,
				Color.MintCream,
				Color.Orchid,
				Color.PeachPuff,
				Color.Purple
			};
			ThreadProfilers = new List<MyProfiler>(16);
			m_onlineThreadProfilers = ThreadProfilers;
			m_frameLocalArea = MyProfiler.MAX_FRAMES;
			m_taskRenderDispersion = MyTimeSpan.FromMilliseconds(30.0).Ticks;
			FrameTimestamps = new ConcurrentQueue<FrameInfo>();
			m_onlineFrameTimestamps = FrameTimestamps;
			AUTO_SCALE_UPDATE = MyTimeSpan.FromSeconds(1.0);
			m_levelLimit = 0;
			FpsBlock = MyProfiler.CreateExternalBlock("FPS", -2);
		}

		/// <summary>
		/// Creates new profiler which can be used to profile anything (e.g. network stats).
		/// </summary>
		public static MyProfiler CreateProfiler(string name, string axisName = null, bool allocationProfiling = false)
		{
			lock (ThreadProfilers)
			{
				MyProfiler myProfiler = new MyProfiler(allocationProfiling, name, axisName ?? "[ms]", ShallowProfileOnly);
				ThreadProfilers.Add(myProfiler);
				SortProfilersLocked();
				myProfiler.SetNewLevelLimit(m_profilerProcessingEnabled ? m_levelLimit : 0);
				myProfiler.Paused = Paused;
				if (m_selectedProfiler == null)
				{
					m_selectedProfiler = myProfiler;
				}
				return myProfiler;
			}
		}

		protected static List<MyProfilerBlock> GetSortedChildren(int frameToSortBy)
		{
			List<MyProfilerBlock> list = new List<MyProfilerBlock>(m_selectedProfiler.SelectedRootChildren);
			switch (m_sortingOrder)
			{
			case RenderProfilerSortingOrder.Id:
				list.Sort((MyProfilerBlock a, MyProfilerBlock b) => a.Id.CompareTo(b.Id));
				break;
			case RenderProfilerSortingOrder.MillisecondsLastFrame:
				list.Sort(delegate(MyProfilerBlock a, MyProfilerBlock b)
				{
					int num = b.RawMilliseconds[frameToSortBy].CompareTo(a.RawMilliseconds[frameToSortBy]);
					return (num != 0) ? num : a.Id.CompareTo(b.Id);
				});
				break;
			case RenderProfilerSortingOrder.MillisecondsAverage:
				list.Sort(delegate(MyProfilerBlock a, MyProfilerBlock b)
				{
					int num2 = b.AverageMilliseconds.CompareTo(a.AverageMilliseconds);
					return (num2 != 0) ? num2 : a.Id.CompareTo(b.Id);
				});
				break;
			case RenderProfilerSortingOrder.AllocatedLastFrame:
				list.Sort(delegate(MyProfilerBlock a, MyProfilerBlock b)
				{
					int num3 = b.RawAllocations[frameToSortBy].CompareTo(a.RawAllocations[frameToSortBy]);
					return (num3 != 0) ? num3 : a.Id.CompareTo(b.Id);
				});
				break;
			}
			return list;
		}

		private static MyProfilerBlock FindBlockByIndex(int index)
		{
			List<MyProfilerBlock> sortedChildren = GetSortedChildren(m_selectedFrame);
			if (index >= 0 && index < sortedChildren.Count)
			{
				return sortedChildren[index];
			}
			if (index == -1 && m_selectedProfiler.SelectedRoot != null)
			{
				return m_selectedProfiler.SelectedRoot.Parent;
			}
			return null;
		}

		protected static bool IsValidIndex(int frameIndex, int lastValidFrame)
		{
			return ((frameIndex > lastValidFrame) ? frameIndex : (frameIndex + MyProfiler.MAX_FRAMES)) > lastValidFrame + MyProfiler.UPDATE_WINDOW;
		}

		public static void HandleInput(RenderProfilerCommand command, int index, string value)
		{
			switch (command)
			{
			case RenderProfilerCommand.Enable:
				if (!m_enabled)
				{
					m_enabled = true;
					m_profilerProcessingEnabled = true;
					SetLevel();
					SelectThead(value);
				}
				break;
			case RenderProfilerCommand.ToggleEnabled:
				if (m_enabled)
				{
					m_enabled = false;
					m_useCustomFrame = false;
				}
				else
				{
					m_enabled = true;
					m_profilerProcessingEnabled = true;
					SelectThead(value);
				}
				break;
			case RenderProfilerCommand.JumpToRoot:
				m_selectedProfiler.SelectedRoot = null;
				break;
			case RenderProfilerCommand.JumpToLevel:
				if (index == 0 && !m_enabled)
				{
					m_enabled = true;
					m_profilerProcessingEnabled = true;
				}
				else
				{
					m_selectedProfiler.SelectedRoot = FindBlockByIndex(index - 1);
					m_nextAutoScale = MyTimeSpan.Zero;
				}
				break;
			case RenderProfilerCommand.Pause:
				SwitchPause();
				break;
			case RenderProfilerCommand.NextThread:
				if (m_graphContent == ProfilerGraphContent.Tasks)
				{
					long num = (long)((double)m_taskRenderDispersion / 1.1);
					if (num > 10)
					{
						m_taskRenderDispersion = num;
					}
				}
				else
				{
					lock (ThreadProfilers)
					{
						int index3 = (ThreadProfilers.IndexOf(m_selectedProfiler) + 1) % ThreadProfilers.Count;
						m_selectedProfiler = ThreadProfilers[index3];
						m_nextAutoScale = MyTimeSpan.Zero;
					}
				}
				break;
			case RenderProfilerCommand.PreviousThread:
				if (m_graphContent == ProfilerGraphContent.Tasks)
				{
					m_taskRenderDispersion = (long)((double)m_taskRenderDispersion * 1.1);
				}
				else
				{
					lock (ThreadProfilers)
					{
						int index2 = (ThreadProfilers.IndexOf(m_selectedProfiler) - 1 + ThreadProfilers.Count) % ThreadProfilers.Count;
						m_selectedProfiler = ThreadProfilers[index2];
						m_nextAutoScale = MyTimeSpan.Zero;
					}
				}
				break;
			case RenderProfilerCommand.Reset:
				lock (ThreadProfilers)
				{
					foreach (MyProfiler threadProfiler in ThreadProfilers)
					{
						threadProfiler.Reset();
					}
					FrameTimestamps = new ConcurrentQueue<FrameInfo>();
					m_selectedFrame = 0;
				}
				break;
			case RenderProfilerCommand.NextFrame:
				NextFrame(index);
				break;
			case RenderProfilerCommand.PreviousFrame:
				PreviousFrame(index);
				break;
			case RenderProfilerCommand.DisableFrameSelection:
				m_useCustomFrame = false;
				break;
			case RenderProfilerCommand.IncreaseLevel:
				m_levelLimit++;
				SetLevel();
				break;
			case RenderProfilerCommand.DecreaseLevel:
				m_levelLimit--;
				if (m_levelLimit < -1)
				{
					m_levelLimit = -1;
				}
				SetLevel();
				break;
			case RenderProfilerCommand.CopyPathToClipboard:
			{
				StringBuilder stringBuilder = new StringBuilder(200);
				for (MyProfilerBlock myProfilerBlock = m_selectedProfiler.SelectedRoot; myProfilerBlock != null; myProfilerBlock = myProfilerBlock.Parent)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Insert(0, " > ");
					}
					stringBuilder.Insert(0, myProfilerBlock.Name);
				}
				if (stringBuilder.Length > 0)
				{
					MyVRage.Platform.Clipboard = stringBuilder.ToString();
				}
				break;
			}
			case RenderProfilerCommand.TryGoToPathInClipboard:
			{
				string fullPath = string.Empty;
				Thread thread = new Thread((ThreadStart)delegate
				{
					try
					{
						fullPath = MyVRage.Platform.Clipboard;
					}
					catch
					{
					}
				});
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
				if (string.IsNullOrEmpty(fullPath))
				{
					break;
				}
				string[] array = fullPath.Split(new string[1]
				{
					" > "
				}, StringSplitOptions.None);
				MyProfilerBlock myProfilerBlock3 = null;
				List<MyProfilerBlock> list = m_selectedProfiler.RootBlocks;
				foreach (string b in array)
				{
					MyProfilerBlock myProfilerBlock4 = myProfilerBlock3;
					for (int j = 0; j < list.Count; j++)
					{
						MyProfilerBlock myProfilerBlock5 = list[j];
						if (myProfilerBlock5.Name == b)
						{
							myProfilerBlock3 = myProfilerBlock5;
							list = myProfilerBlock3.Children;
							break;
						}
					}
					if (myProfilerBlock4 == myProfilerBlock3)
					{
						break;
					}
				}
				if (myProfilerBlock3 != null)
				{
					m_selectedProfiler.SelectedRoot = myProfilerBlock3;
				}
				break;
			}
			case RenderProfilerCommand.SetLevel:
				m_levelLimit = index;
				if (m_levelLimit < -1)
				{
					m_levelLimit = -1;
				}
				SetLevel();
				break;
			case RenderProfilerCommand.DecreaseLocalArea:
				m_frameLocalArea = Math.Max(2, m_frameLocalArea / 2);
				break;
			case RenderProfilerCommand.IncreaseLocalArea:
				m_frameLocalArea = Math.Min(MyProfiler.MAX_FRAMES, m_frameLocalArea * 2);
				break;
			case RenderProfilerCommand.IncreaseRange:
				m_selectedProfiler.AutoScale = false;
				GetCurrentGraphScale().IncreaseYRange();
				break;
			case RenderProfilerCommand.DecreaseRange:
				m_selectedProfiler.AutoScale = false;
				GetCurrentGraphScale().DecreaseYRange();
				break;
			case RenderProfilerCommand.EnableAutoScale:
				m_selectedProfiler.AutoScale = true;
				break;
			case RenderProfilerCommand.ToggleAutoScale:
				m_selectedProfiler.AutoScale = !m_selectedProfiler.AutoScale;
				m_nextAutoScale = MyTimeSpan.Zero;
				break;
			case RenderProfilerCommand.ChangeSortingOrder:
				m_sortingOrder++;
				if (m_sortingOrder >= RenderProfilerSortingOrder.NumSortingTypes)
				{
					m_sortingOrder = RenderProfilerSortingOrder.Id;
				}
				break;
			case RenderProfilerCommand.GetFomServer:
				if (m_enabled && GetProfilerFromServer != null)
				{
					Pause();
					GetProfilerFromServer();
				}
				break;
			case RenderProfilerCommand.GetFromClient:
				RestoreOnlineSnapshot();
				break;
			case RenderProfilerCommand.SaveToFile:
				SaveProfilerToFile(index);
				break;
			case RenderProfilerCommand.LoadFromFile:
				Pause();
				LoadProfilerFromFile(index, arg2: false);
				break;
			case RenderProfilerCommand.SubtractFromFile:
				Pause();
				LoadProfilerFromFile(index, arg2: true);
				break;
			case RenderProfilerCommand.SwapBlockOptimized:
			{
				if (index == 0)
				{
					foreach (MyProfilerBlock selectedRootChild in m_selectedProfiler.SelectedRootChildren)
					{
						selectedRootChild.IsOptimized = !selectedRootChild.IsOptimized;
					}
					break;
				}
				MyProfilerBlock myProfilerBlock2 = FindBlockByIndex(index - 1);
				if (myProfilerBlock2 != null)
				{
					myProfilerBlock2.IsOptimized = !myProfilerBlock2.IsOptimized;
				}
				break;
			}
			case RenderProfilerCommand.ResetAllOptimizations:
				foreach (MyProfilerBlock rootBlock in m_selectedProfiler.RootBlocks)
				{
					ResetOptimizationsRecursive(rootBlock);
				}
				break;
			case RenderProfilerCommand.ToggleOptimizationsEnabled:
				m_selectedProfiler.EnableOptimizations = !m_selectedProfiler.EnableOptimizations;
				break;
			case RenderProfilerCommand.SwitchGraphContent:
				m_graphContent++;
				if (m_graphContent >= ProfilerGraphContent.ProfilerGraphContentMax)
				{
					m_graphContent = ProfilerGraphContent.Elapsed;
				}
				switch (m_graphContent)
				{
				case ProfilerGraphContent.Allocations:
					m_sortingOrder = RenderProfilerSortingOrder.AllocatedLastFrame;
					break;
				case ProfilerGraphContent.Elapsed:
					m_sortingOrder = RenderProfilerSortingOrder.MillisecondsLastFrame;
					break;
				case ProfilerGraphContent.Tasks:
					if (!FrameTimestamps.IsEmpty)
					{
						if (m_selectedProfiler == null)
						{
							m_targetTaskRenderTime = FrameTimestamps.Last().Time - m_taskRenderDispersion;
						}
						else
						{
							m_targetTaskRenderTime = m_selectedProfiler.CommitTimes[m_selectedFrame];
						}
					}
					break;
				}
				m_nextAutoScale = MyTimeSpan.Zero;
				break;
			case RenderProfilerCommand.SwitchShallowProfile:
				ShallowProfileOnly = !ShallowProfileOnly;
				lock (ThreadProfilers)
				{
					foreach (MyProfiler threadProfiler2 in ThreadProfilers)
					{
						threadProfiler2.SetShallowProfile(ShallowProfileOnly);
					}
				}
				break;
			case RenderProfilerCommand.SwitchAverageTimes:
				AverageTimes = !AverageTimes;
				lock (ThreadProfilers)
				{
					foreach (MyProfiler threadProfiler3 in ThreadProfilers)
					{
						threadProfiler3.AverageTimes = AverageTimes;
					}
				}
				break;
			case RenderProfilerCommand.SwitchBlockRender:
				m_blockRender++;
				if (m_blockRender == BlockRender.BlockRenderMax)
				{
					m_blockRender = BlockRender.Name;
				}
				break;
			}
		}

		private static void SelectThead(string threadName)
		{
			if (threadName != null)
			{
				lock (ThreadProfilers)
				{
					foreach (MyProfiler threadProfiler in ThreadProfilers)
					{
						if (threadProfiler.DisplayName == threadName)
						{
							m_selectedProfiler = threadProfiler;
							m_graphContent = ProfilerGraphContent.Elapsed;
							m_nextAutoScale = MyTimeSpan.Zero;
						}
					}
				}
			}
		}

		private static void Pause()
		{
			lock (m_pauseLock)
			{
				m_pauseCount = 1;
				ApplyPause(paused: true);
			}
		}

		private static void SwitchPause()
		{
			lock (m_pauseLock)
			{
				m_pauseCount = (Paused ? 1 : 0);
				ApplyPause(!Paused);
			}
		}

		public static void AddPause(bool pause)
		{
			lock (m_pauseLock)
			{
				m_pauseCount += (pause ? 1 : (-1));
				ApplyPause(m_pauseCount > 0);
			}
		}

		private static void ApplyPause(bool paused)
		{
			if (!paused && m_dataType != 0)
			{
				RestoreOnlineSnapshot();
			}
			if (paused && m_graphContent == ProfilerGraphContent.Tasks)
			{
				m_targetTaskRenderTime = Stopwatch.GetTimestamp() - m_taskRenderDispersion;
			}
			Thread.MemoryBarrier();
			Paused = paused;
			foreach (MyProfiler threadProfiler in ThreadProfilers)
			{
				threadProfiler.Paused = paused;
			}
			GpuProfiler.AutoCommit = !paused;
		}

		private static void ResetOptimizationsRecursive(MyProfilerBlock block)
		{
			foreach (MyProfilerBlock child in block.Children)
			{
				ResetOptimizationsRecursive(child);
			}
			block.IsOptimized = false;
		}

		private static void SetLevel()
		{
			lock (ThreadProfilers)
			{
				foreach (MyProfiler threadProfiler in ThreadProfilers)
				{
					threadProfiler.SetNewLevelLimit(m_profilerProcessingEnabled ? m_levelLimit : 0);
				}
			}
		}

		private static void PreviousFrame(int step)
		{
			if (m_graphContent == ProfilerGraphContent.Tasks)
			{
				m_targetTaskRenderTime -= (long)((float)(m_taskRenderDispersion * step) * 0.05f);
				return;
			}
			m_useCustomFrame = true;
			m_selectedFrame -= step;
			while (m_selectedFrame < 0)
			{
				m_selectedFrame += MyProfiler.MAX_FRAMES - 1;
			}
		}

		private static void NextFrame(int step)
		{
			if (m_graphContent == ProfilerGraphContent.Tasks)
			{
				m_targetTaskRenderTime += (long)((float)(m_taskRenderDispersion * step) * 0.05f);
				return;
			}
			m_useCustomFrame = true;
			m_selectedFrame += step;
			while (m_selectedFrame >= MyProfiler.MAX_FRAMES)
			{
				m_selectedFrame -= MyProfiler.MAX_FRAMES;
			}
		}

		private static void FindMax(MyProfilerBlock.DataReader data, int start, int end, ref float max, ref int maxIndex)
		{
			for (int i = start; i <= end; i++)
			{
				float num = data[i];
				if (num > max)
				{
					max = num;
					maxIndex = i;
				}
			}
		}

		private static void FindMax(MyProfilerBlock.DataReader data, int lower, int upper, int lastValidFrame, ref float max, ref int maxIndex)
		{
			int num = (lastValidFrame + 1 + MyProfiler.UPDATE_WINDOW) % MyProfiler.MAX_FRAMES;
			if (lastValidFrame > num)
			{
				FindMax(data, Math.Max(lower, num), Math.Min(lastValidFrame, upper), ref max, ref maxIndex);
				return;
			}
			FindMax(data, lower, Math.Min(lastValidFrame, upper), ref max, ref maxIndex);
			FindMax(data, Math.Max(lower, num), upper, ref max, ref maxIndex);
		}

		protected static float FindMaxWrap(MyProfilerBlock.DataReader data, int lower, int upper, int lastValidFrame, out int maxIndex)
		{
			lower = (lower + MyProfiler.MAX_FRAMES) % MyProfiler.MAX_FRAMES;
			upper = (upper + MyProfiler.MAX_FRAMES) % MyProfiler.MAX_FRAMES;
			float max = 0f;
			maxIndex = -1;
			if (upper > lower)
			{
				FindMax(data, lower, upper, lastValidFrame, ref max, ref maxIndex);
			}
			else
			{
				FindMax(data, 0, upper, lastValidFrame, ref max, ref maxIndex);
				FindMax(data, lower, MyProfiler.MAX_FRAMES - 1, lastValidFrame, ref max, ref maxIndex);
			}
			return max;
		}

		public static bool GetAutocommit()
		{
			return ThreadProfiler.AutoCommit;
		}

		public static void SetAutocommit(bool val)
		{
			ThreadProfiler.AutoCommit = val;
		}

		public static void Commit([CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			long timestamp = Stopwatch.GetTimestamp();
			MyProfiler threadProfiler = ThreadProfiler;
			if (!Paused)
			{
				threadProfiler.CommitFrame();
				m_useCustomFrame = true;
			}
			else
			{
				threadProfiler.ClearFrame();
			}
			MyTimeSpan.FromTicks(Stopwatch.GetTimestamp() - timestamp);
		}

		public void Draw([CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			if (m_enabled)
			{
				long timestamp = Stopwatch.GetTimestamp();
				MyProfiler selectedProfiler = m_selectedProfiler;
				if (selectedProfiler != null)
				{
					int lastValidFrame;
					using (selectedProfiler.LockHistory(out lastValidFrame))
					{
						int frameToDraw = m_useCustomFrame ? m_selectedFrame : lastValidFrame;
						Draw(selectedProfiler, lastValidFrame, frameToDraw);
					}
					MyTimeSpan.FromTicks(Stopwatch.GetTimestamp() - timestamp);
				}
			}
		}

		protected abstract void Draw(MyProfiler drawProfiler, int lastFrameIndex, int frameToDraw);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void StartProfilingBlock(string blockName = null, bool isDeepTreeRoot = false, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			ThreadProfiler.StartBlock(blockName, member, line, file, int.MaxValue, isDeepTreeRoot);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void EndProfilingBlock(float customValue = 0f, MyTimeSpan? customTime = null, string timeFormat = null, string valueFormat = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			ThreadProfiler.EndBlock(member, line, file, customTime, customValue, timeFormat, valueFormat);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void GPU_StartProfilingBlock(string blockName = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			GpuProfiler.StartBlock(blockName, member, line, file);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void GPU_EndProfilingBlock(float customValue = 0f, MyTimeSpan? customTime = null, string timeFormat = null, string valueFormat = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			GpuProfiler.EndBlock(member, line, file, customTime, customValue, timeFormat, valueFormat);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void StartNextBlock(string name, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "", float previousBlockCustomValue = 0f)
		{
		}

		public static void InitMemoryHack(string name)
		{
			ThreadProfiler.InitMemoryHack(name);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void ProfileCustomValue(string name, float value, MyTimeSpan? customTime = null, string timeFormat = null, string valueFormat = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
			_ = m_levelLimit;
			_ = -1;
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnTaskStarted(MyProfiler.TaskType taskType, string debugName, long scheduledTimestamp)
		{
			ThreadProfiler.OnTaskStarted(taskType, debugName, scheduledTimestamp);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnTaskFinished(MyProfiler.TaskType? taskType, float customValue)
		{
			ThreadProfiler.OnTaskFinished(taskType, customValue);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void CommitTask(MyProfiler.TaskInfo task)
		{
			ThreadProfiler.CommitTask(task);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void InitThraedInfo(int viewPriority)
		{
			ThreadProfiler.ViewPriority = viewPriority;
			lock (ThreadProfilers)
			{
				SortProfilersLocked();
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnBeginSimulationFrame(long frameNumber)
		{
			if (!Paused)
			{
				long timestamp = Stopwatch.GetTimestamp();
				FrameTimestamps.Enqueue(new FrameInfo
				{
					Time = timestamp,
					FrameNumber = frameNumber
				});
				if (FrameTimestamps.Count > MyProfiler.MAX_FRAMES)
				{
					FrameTimestamps.TryDequeue(out FrameInfo _);
				}
				FrameTimestamps.TryPeek(out FrameInfo result2);
				MyProfiler.LastFrameTime = timestamp;
				MyProfiler.LastInterestingFrameTime = result2.Time;
			}
		}

		internal static void DestroyThread()
		{
			lock (ThreadProfilers)
			{
				ThreadProfilers.Remove(m_threadProfiler);
				if (m_selectedProfiler == m_threadProfiler)
				{
					m_selectedProfiler = ((ThreadProfilers.Count > 0) ? ThreadProfilers[0] : null);
				}
				m_threadProfiler = null;
			}
		}

		public static void SetLevel(int index)
		{
			m_levelLimit = index;
			if (m_levelLimit < -1)
			{
				m_levelLimit = -1;
			}
			SetLevel();
		}

		public void Dump()
		{
			try
			{
				string text = null;
				while (m_currentDumpNumber < 100)
				{
					text = MyFileSystem.UserDataPath + $"\\dump{m_currentDumpNumber}.xml";
					if (!MyFileSystem.FileExists(text))
					{
						break;
					}
					m_currentDumpNumber++;
				}
				if (text != null)
				{
					Stream stream = MyFileSystem.OpenWrite(text);
					if (stream != null)
					{
						StreamWriter streamWriter = new StreamWriter(stream);
						StringBuilder value = ThreadProfiler.Dump();
						streamWriter.Write(value);
						streamWriter.Close();
						stream.Close();
					}
				}
			}
			catch
			{
			}
		}

		protected static MyDrawArea GetCurrentGraphScale()
		{
			switch (m_graphContent)
			{
			case ProfilerGraphContent.Elapsed:
			case ProfilerGraphContent.Tasks:
				return m_milisecondsGraphScale;
			case ProfilerGraphContent.Allocations:
				return m_allocationsGraphScale;
			default:
				throw new Exception("Unhandled enum value" + m_graphContent);
			}
		}

		protected static MyProfilerBlock.DataReader GetGraphData(MyProfilerBlock block)
		{
			bool enableOptimizations = m_selectedProfiler.EnableOptimizations;
			switch (m_graphContent)
			{
			case ProfilerGraphContent.Elapsed:
				return block.GetMillisecondsReader(enableOptimizations);
			case ProfilerGraphContent.Allocations:
				return block.GetAllocationsReader(enableOptimizations);
			default:
				throw new Exception("Unhandled enum value" + m_graphContent);
			}
		}

		protected static int GetWindowEnd(int lastFrameIndex)
		{
			return (lastFrameIndex + 1 + MyProfiler.UPDATE_WINDOW) % MyProfiler.MAX_FRAMES;
		}

		private static void UpdateStatsSeparated(ref MyStats stats, MyProfilerBlock.DataReader data, int lastFrameIndex, int windowEnd)
		{
			if (lastFrameIndex > windowEnd)
			{
				UpdateStats(ref stats, data, windowEnd, lastFrameIndex);
				return;
			}
			UpdateStats(ref stats, data, 0, lastFrameIndex);
			UpdateStats(ref stats, data, windowEnd, MyProfiler.MAX_FRAMES - 1);
		}

		private static void UpdateStats(ref MyStats stats, MyProfilerBlock.DataReader data, int start, int end)
		{
			for (int i = start; i <= end; i++)
			{
				float num = data[i];
				if (!(num > 0.01f))
				{
					continue;
				}
				if (num > stats.Min)
				{
					stats.MinCount++;
					if (num > stats.Max)
					{
						stats.MaxCount++;
					}
				}
				stats.Any = true;
			}
		}

		protected static void UpdateAutoScale(int lastFrameIndex)
		{
			MyTimeSpan a = new MyTimeSpan(Stopwatch.GetTimestamp());
			if (!m_selectedProfiler.AutoScale || !(a > m_nextAutoScale))
			{
				return;
			}
			MyDrawArea currentGraphScale = GetCurrentGraphScale();
			MyStats myStats = default(MyStats);
			myStats.Min = currentGraphScale.GetYRange(currentGraphScale.Index - 1);
			myStats.Max = currentGraphScale.YRange;
			MyStats stats = myStats;
			int windowEnd = GetWindowEnd(lastFrameIndex);
			List<MyProfilerBlock> selectedRootChildren = m_selectedProfiler.SelectedRootChildren;
			if (m_selectedProfiler.SelectedRoot != null && (!m_selectedProfiler.IgnoreRoot || selectedRootChildren.Count == 0))
			{
				MyProfilerBlock.DataReader graphData = GetGraphData(m_selectedProfiler.SelectedRoot);
				UpdateStatsSeparated(ref stats, graphData, lastFrameIndex, windowEnd);
			}
			foreach (MyProfilerBlock item in selectedRootChildren)
			{
				MyProfilerBlock.DataReader graphData2 = GetGraphData(item);
				UpdateStatsSeparated(ref stats, graphData2, lastFrameIndex, windowEnd);
			}
			if (stats.MaxCount > 0)
			{
				if (stats.MaxCount > 10)
				{
					currentGraphScale.IncreaseYRange();
					UpdateAutoScale(lastFrameIndex);
				}
			}
			else if (stats.MinCount < 10 && stats.Any)
			{
				currentGraphScale.DecreaseYRange();
				UpdateAutoScale(lastFrameIndex);
			}
			m_nextAutoScale = a + AUTO_SCALE_UPDATE;
		}

		public static void PushOnlineSnapshot(SnapshotType type, List<MyProfiler> threadProfilers, ConcurrentQueue<FrameInfo> frameTimestamps)
		{
			m_dataType = type;
			if (!FrameTimestamps.IsEmpty)
			{
				MyProfiler.LastFrameTime = FrameTimestamps.Last().Time;
				MyProfiler.LastInterestingFrameTime = FrameTimestamps.First().Time;
				m_targetTaskRenderTime = MyProfiler.LastFrameTime - m_taskRenderDispersion;
			}
			Volatile.Write(ref ThreadProfilers, threadProfilers);
			FrameTimestamps = frameTimestamps;
		}

		public static void SubtractOnlineSnapshot(SnapshotType type, List<MyProfiler> threadProfilers, ConcurrentQueue<FrameInfo> frameTimestamps)
		{
			m_dataType = type;
			if (!FrameTimestamps.IsEmpty)
			{
				MyProfiler.LastFrameTime = FrameTimestamps.Last().Time;
				MyProfiler.LastInterestingFrameTime = FrameTimestamps.First().Time;
				m_targetTaskRenderTime = MyProfiler.LastFrameTime - m_taskRenderDispersion;
			}
			foreach (MyProfiler threadProfiler in threadProfilers)
			{
				if (!string.IsNullOrEmpty(threadProfiler.DisplayName))
				{
					foreach (MyProfiler threadProfiler2 in ThreadProfilers)
					{
						if (threadProfiler2.DisplayName == threadProfiler.DisplayName)
						{
							threadProfiler.SubtractFrom(threadProfiler2);
						}
					}
				}
			}
			Volatile.Write(ref ThreadProfilers, threadProfilers);
			FrameTimestamps = frameTimestamps;
		}

		private static void RestoreOnlineSnapshot()
		{
			m_dataType = SnapshotType.Online;
			ThreadProfilers = m_onlineThreadProfilers;
			FrameTimestamps = m_onlineFrameTimestamps;
			lock (ThreadProfilers)
			{
				SelectedProfiler = ThreadProfilers[0];
				long time = FrameTimestamps.LastOrDefault().Time;
				if (time > 0)
				{
					MyProfiler.LastFrameTime = time;
					MyProfiler.LastInterestingFrameTime = time;
				}
			}
		}

		private static void SortProfilersLocked()
		{
			ThreadProfilers.SortNoAlloc((MyProfiler x, MyProfiler y) => x.ViewPriority.CompareTo(y.ViewPriority));
		}
	}
}
