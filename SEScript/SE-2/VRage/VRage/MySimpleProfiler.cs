using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage.FileSystem;
using VRage.Library.Utils;
using VRage.Utils;

namespace VRage
{
	/// <summary>
	/// A simple performance profiler intended to show players information about which area of the game is slowing it down
	/// </summary>
	public class MySimpleProfiler
	{
		public enum ProfilingBlockType : byte
		{
			GPU,
			MOD,
			BLOCK,
			RENDER,
			OTHER,
			INTERNAL,
			INTERNALGPU
		}

		public class MySimpleProfilingBlock
		{
			public const int MEASURE_AVG_OVER_FRAMES = 60;

			public readonly string Name;

			public readonly MyStringId Description;

			public readonly MyStringId DisplayStringId;

			public readonly ProfilingBlockType Type;

			public int ThresholdFrame = 100000;

			public int ThresholdAverage = 10000;

			private int m_tickTime;

			private float m_avgTickTime;

			public string DisplayName
			{
				get
				{
					if (!(DisplayStringId == MyStringId.NullOrEmpty))
					{
						return MyTexts.GetString(DisplayStringId);
					}
					return Name;
				}
			}

			public MySimpleProfilingBlock(string key, ProfilingBlockType type)
			{
				Name = key;
				Type = type;
				if (type == ProfilingBlockType.MOD)
				{
					ThresholdFrame = 50000;
					ThresholdAverage = 10000;
					DisplayStringId = MyStringId.GetOrCompute(key);
					Description = MyStringId.TryGet("PerformanceWarningAreaModsDescription");
				}
				else
				{
					DisplayStringId = MyStringId.TryGet("PerformanceWarningArea" + key);
					Description = MyStringId.TryGet("PerformanceWarningArea" + key + "Description");
				}
				if (DisplayStringId == MyStringId.NullOrEmpty && type == ProfilingBlockType.GPU)
				{
					DisplayStringId = MyStringId.TryGet("PerformanceWarningAreaRenderGPU");
					Description = MyStringId.TryGet("PerformanceWarningAreaRenderGPUDescription");
				}
			}

			public void CommitTime(int microseconds)
			{
				if (Type != 0 && Type != ProfilingBlockType.INTERNALGPU && Type != ProfilingBlockType.RENDER)
				{
					Interlocked.Add(ref m_tickTime, microseconds);
				}
				else
				{
					CommitAvgTime(microseconds);
				}
			}

			public void CommitSimulationFrame(out int tickTime, out int avgTime)
			{
				if (Type != 0 && Type != ProfilingBlockType.RENDER)
				{
					tickTime = Interlocked.Exchange(ref m_tickTime, 0);
					CommitAvgTime(tickTime);
					avgTime = (int)m_avgTickTime;
				}
				else
				{
					tickTime = 0;
					avgTime = (int)Interlocked.CompareExchange(ref m_avgTickTime, 0f, 0f);
				}
			}

			private void CommitAvgTime(int tickTime)
			{
				float avgTickTime = m_avgTickTime;
				m_avgTickTime += ((float)tickTime - avgTickTime) / 60f;
			}

			public void MergeFrom(MySimpleProfilingBlock other)
			{
				CommitTime(other.m_tickTime);
			}
		}

		public class PerformanceWarning
		{
			public int Time;

			public MySimpleProfilingBlock Block;

			public PerformanceWarning(MySimpleProfilingBlock block)
			{
				Time = 0;
				Block = block;
			}
		}

		private struct TimeKeepingItem
		{
			public readonly string InvokingMember;

			public readonly MyTimeSpan Timespan;

			public readonly MySimpleProfilingBlock ProfilingBlock;

			public TimeKeepingItem(string invokingMember, MySimpleProfilingBlock profilingBlock, MyTimeSpan? timeSpan = null)
			{
				InvokingMember = invokingMember;
				ProfilingBlock = profilingBlock;
				Timespan = (timeSpan ?? new MyTimeSpan(Stopwatch.GetTimestamp()));
			}
		}

		private const int MAX_LEVELS = 100;

		private const int MAX_ITEMS_IN_SYNC_QUEUE = 20;

		/// <summary>
		/// Global keeper of all known profiling blocks.
		///
		/// !!! Do not try to add new blocks into existing instance. !!!
		/// Allocate new instead, copy all existing blocks, merge new blocks and swap references.
		///
		/// !!! Do not pool dictionaries as they may be still in use by other threads. !!!
		/// Allocations should happen only during firs few frames. It's trade-off for wait-free progress guarantee during runtime.
		/// </summary>
		private static volatile Dictionary<string, MySimpleProfilingBlock> m_profilingBlocks;

		/// <summary>
		/// When thread fails to find required block in global keeper, it will add it here and it will be added later.
		/// </summary>
		private static readonly ConcurrentQueue<MySimpleProfilingBlock> m_addUponSync;

		[ThreadStatic]
		private static Stack<TimeKeepingItem> m_timers;

		private static readonly Stack<string> m_gpuBlocks;

		private static bool m_performanceTestEnabled;

		private static readonly List<int> m_updateTimes;

		private static readonly List<int> m_renderTimes;

		private static readonly List<int> m_gpuTimes;

		private static readonly List<int> m_memoryAllocs;

		private static ulong m_lastAllocationStamp;

		private static int m_skipFrames;

		public static Dictionary<MySimpleProfilingBlock, PerformanceWarning> CurrentWarnings
		{
			get;
			private set;
		}

		private static bool SkipProfiling => m_skipFrames > 0;

		public static event Action<MySimpleProfilingBlock> ShowPerformanceWarning;

		static MySimpleProfiler()
		{
			m_profilingBlocks = new Dictionary<string, MySimpleProfilingBlock>();
			m_addUponSync = new ConcurrentQueue<MySimpleProfilingBlock>();
			m_gpuBlocks = new Stack<string>();
			m_updateTimes = new List<int>();
			m_renderTimes = new List<int>();
			m_gpuTimes = new List<int>();
			m_memoryAllocs = new List<int>();
			m_lastAllocationStamp = 0uL;
			CurrentWarnings = new Dictionary<MySimpleProfilingBlock, PerformanceWarning>();
			Reset(resetFrameCounter: true);
		}

		/// <summary>
		/// Preferred way of measurement.
		/// Makes sure every block is properly ended and prevents Begin/End mismatch errors
		/// </summary>
		public static void BeginBlock(string key, ProfilingBlockType type = ProfilingBlockType.OTHER)
		{
			Begin(key, type, "BeginBlock");
		}

		/// <summary>
		/// Begin new profiling block
		/// </summary>
		public static void Begin(string key, ProfilingBlockType type = ProfilingBlockType.OTHER, [CallerMemberName] string callingMember = null)
		{
			if (m_timers == null)
			{
				m_timers = new Stack<TimeKeepingItem>();
			}
			MySimpleProfilingBlock orMakeBlock = GetOrMakeBlock(key, type);
			m_timers.Push(new TimeKeepingItem(callingMember, orMakeBlock));
		}

		/// <summary>
		/// End profiling block
		/// </summary>
		public static void End([CallerMemberName] string callingMember = "")
		{
			EndNoMemberPairingCheck();
		}

		public static void EndNoMemberPairingCheck()
		{
			if (m_timers.Count != 0)
			{
				double microseconds = new MyTimeSpan(Stopwatch.GetTimestamp()).Microseconds;
				TimeKeepingItem timeKeepingItem = m_timers.Pop();
				timeKeepingItem.ProfilingBlock.CommitTime((int)(microseconds - timeKeepingItem.Timespan.Microseconds));
			}
		}

		public static void EndMemberPairingCheck([CallerMemberName] string callingMember = "")
		{
			if (m_timers.Count != 0)
			{
				double microseconds = new MyTimeSpan(Stopwatch.GetTimestamp()).Microseconds;
				TimeKeepingItem timeKeepingItem = m_timers.Pop();
				if (callingMember != timeKeepingItem.InvokingMember)
				{
					EndMemberPairingCheck(callingMember);
				}
				timeKeepingItem.ProfilingBlock.CommitTime((int)(microseconds - timeKeepingItem.Timespan.Microseconds));
			}
		}

		/// <summary>
		/// Set which GPU profiling block is going to receive timing next
		/// </summary>
		public static void BeginGPUBlock(string key)
		{
			m_gpuBlocks.Push(key);
			if (m_gpuBlocks.Count > 100)
			{
				m_gpuBlocks.Clear();
			}
		}

		/// <summary>
		/// Log timing of currently set GPU block
		/// </summary>
		public static void EndGPUBlock(MyTimeSpan time)
		{
			if (m_gpuBlocks.Count != 0)
			{
				string key = m_gpuBlocks.Pop();
				if (m_profilingBlocks.ContainsKey(key))
				{
					GetOrMakeBlock(key, ProfilingBlockType.GPU).CommitTime((int)time.Microseconds);
				}
			}
		}

		private static MySimpleProfilingBlock GetOrMakeBlock(string key, ProfilingBlockType type, bool forceAdd = false)
		{
			if (m_profilingBlocks.TryGetValue(key, out MySimpleProfilingBlock value))
			{
				return value;
			}
			MySimpleProfilingBlock mySimpleProfilingBlock = new MySimpleProfilingBlock(key, type);
			if (forceAdd || m_addUponSync.Count < 20)
			{
				m_addUponSync.Enqueue(mySimpleProfilingBlock);
			}
			return mySimpleProfilingBlock;
		}

		public static void Reset(bool resetFrameCounter = false)
		{
			CurrentWarnings.Clear();
			m_profilingBlocks = new Dictionary<string, MySimpleProfilingBlock>();
			int thresholdFrame = (int)MyTimeSpan.FromMilliseconds(100.0).Microseconds;
			int thresholdAverage = (int)MyTimeSpan.FromMilliseconds(40.0).Microseconds;
			SetBlockSettings("GPUFrame", thresholdFrame, thresholdAverage, ProfilingBlockType.GPU);
			SetBlockSettings("RenderFrame", thresholdFrame, thresholdAverage, ProfilingBlockType.RENDER);
			if (resetFrameCounter)
			{
				m_skipFrames = 10;
			}
		}

		/// <summary>
		/// Check performance and reset time
		/// </summary>
		public static void Commit()
		{
			Dictionary<string, MySimpleProfilingBlock> dictionary = m_profilingBlocks;
			bool flag = false;
			MySimpleProfilingBlock result;
			while (m_addUponSync.TryDequeue(out result))
			{
				if (!flag)
				{
					flag = true;
					dictionary = new Dictionary<string, MySimpleProfilingBlock>(dictionary);
				}
				string name = result.Name;
				if (dictionary.TryGetValue(name, out MySimpleProfilingBlock value))
				{
					value.MergeFrom(result);
				}
				else
				{
					dictionary.Add(name, result);
				}
			}
			if (flag)
			{
				m_profilingBlocks = dictionary;
			}
			foreach (MySimpleProfilingBlock value2 in dictionary.Values)
			{
				value2.CommitSimulationFrame(out int tickTime, out int avgTime);
				CheckPerformance(value2, tickTime, avgTime);
				if (m_performanceTestEnabled)
				{
					if (value2.Name == "UpdateFrame")
					{
						m_updateTimes.Add(tickTime);
					}
					else if (value2.Name == "RenderFrame")
					{
						m_renderTimes.Add(tickTime);
					}
					else if (value2.Name == "GPUFrame")
					{
						m_gpuTimes.Add(avgTime);
					}
				}
			}
			if (m_performanceTestEnabled)
			{
				ulong globalAllocationsStamp = MyVRage.Platform.GetGlobalAllocationsStamp();
				m_memoryAllocs.Add((int)(globalAllocationsStamp - m_lastAllocationStamp));
				m_lastAllocationStamp = globalAllocationsStamp;
			}
			foreach (KeyValuePair<MySimpleProfilingBlock, PerformanceWarning> currentWarning in CurrentWarnings)
			{
				currentWarning.Value.Time++;
			}
			if (m_skipFrames > 0)
			{
				m_skipFrames--;
				if (m_skipFrames == 0)
				{
					Reset();
				}
			}
		}

		/// <summary>
		/// Set special settings for a profiling block
		/// <param name="thresholdFrame">Microseconds!</param>
		/// /// <param name="thresholdAverage">Microseconds!</param>
		/// </summary>
		public static void SetBlockSettings(string key, int thresholdFrame = 100000, int thresholdAverage = 10000, ProfilingBlockType type = ProfilingBlockType.OTHER)
		{
			MySimpleProfilingBlock orMakeBlock = GetOrMakeBlock(key, type, forceAdd: true);
			orMakeBlock.ThresholdFrame = thresholdFrame;
			orMakeBlock.ThresholdAverage = thresholdAverage;
		}

		/// <summary>
		/// Checks performance of each profiling block and sends notifications if above threshold
		/// </summary>
		private static void CheckPerformance(MySimpleProfilingBlock block, int tickTime, int average)
		{
			if (block.Type != ProfilingBlockType.INTERNAL && block.Type != ProfilingBlockType.INTERNALGPU)
			{
				bool flag = false;
				if (block.ThresholdFrame > 0)
				{
					flag |= (tickTime > block.ThresholdFrame);
				}
				else if (block.ThresholdFrame < 0)
				{
					flag |= (tickTime < -block.ThresholdFrame);
				}
				if (block.ThresholdAverage > 0)
				{
					flag |= (average > block.ThresholdAverage);
				}
				else if (block.ThresholdAverage < 0)
				{
					flag |= (average < -block.ThresholdAverage);
				}
				if (flag && !SkipProfiling)
				{
					InvokePerformanceWarning(block);
				}
			}
		}

		/// <summary>
		/// Show performance warning received from server
		/// </summary>
		public static void ShowServerPerformanceWarning(string key, ProfilingBlockType type)
		{
			InvokePerformanceWarning(GetOrMakeBlock(key, type));
		}

		private static void AddWarningToCurrent(MySimpleProfilingBlock block)
		{
			if (CurrentWarnings.ContainsKey(block))
			{
				CurrentWarnings[block].Time = 0;
			}
			else
			{
				CurrentWarnings.Add(block, new PerformanceWarning(block));
			}
		}

		private static void InvokePerformanceWarning(MySimpleProfilingBlock block)
		{
			AddWarningToCurrent(block);
			MySimpleProfiler.ShowPerformanceWarning.InvokeIfNotNull(block);
		}

		public static void LogPerformanceTestResults()
		{
			if (m_performanceTestEnabled)
			{
				GC.Collect();
				long totalMemory = GC.GetTotalMemory(forceFullCollection: true);
				Stream stream = MyFileSystem.OpenWrite(Path.Combine(MyFileSystem.UserDataPath, "Performance_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".csv"));
				StreamWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false));
				streamWriter.WriteLine("Update, Render, GPU, Memory");
				for (int i = 0; i < m_updateTimes.Count && i < m_renderTimes.Count && i < m_gpuTimes.Count && i < m_memoryAllocs.Count; i++)
				{
					object[] arg = new object[4]
					{
						m_updateTimes[i],
						m_renderTimes[i],
						m_gpuTimes[i],
						m_memoryAllocs[i]
					};
					streamWriter.WriteLine("{0},{1},{2},{3}", arg);
				}
				streamWriter.WriteLine("Final memory: {0}", totalMemory);
				streamWriter.Close();
				stream.Close();
			}
		}

		public static void AttachTestingTool()
		{
			m_performanceTestEnabled = MyVRage.Platform.IsAllocationReady;
		}
	}
}
