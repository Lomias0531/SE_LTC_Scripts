using ParallelTasks;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;

namespace VRage.Profiler
{
	/// <summary>
	/// Helper class, "shortcuts" to profiler
	/// </summary>
	public static class ProfilerShort
	{
		public const string PerformanceProfilingSymbol = "__RANDOM_UNDEFINED_PROFILING_SYMBOL__";

		private static MyRenderProfiler m_profiler;

		public static MyRenderProfiler Profiler
		{
			get
			{
				return m_profiler;
			}
			private set
			{
				m_profiler = value;
			}
		}

		public static bool Autocommit
		{
			get
			{
				return MyRenderProfiler.GetAutocommit();
			}
			set
			{
				MyRenderProfiler.SetAutocommit(value);
			}
		}

		public static void SetProfiler(MyRenderProfiler profiler)
		{
			Profiler = profiler;
			DelegateExtensions.SetupProfiler(delegate
			{
			}, delegate
			{
			});
			WorkItem.SetupProfiler(delegate
			{
			}, delegate
			{
			}, delegate
			{
			}, delegate
			{
			}, delegate
			{
			});
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void Begin(string blockName = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void BeginDeepTree(string blockName = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void BeginNextBlock(string blockName = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "", float previousBlockCustomValue = 0f)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void End(float customValue = 0f, MyTimeSpan? customTime = null, string timeFormat = null, string valueFormat = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void CustomValue(string name, float value, MyTimeSpan? customTime, string timeFormat = null, string valueFormat = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void End(float customValue, float customTimeMs, string timeFormat = null, string valueFormat = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void CustomValue(string name, float value, float customTimeMs, string timeFormat = null, string valueFormat = null, [CallerMemberName] string member = "", [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnTaskStarted(MyProfiler.TaskType taskType, string debugName, long scheduledTimestamp = -1L)
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnTaskFinished(MyProfiler.TaskType? taskType = null, float customValue = 0f)
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void CommitTask(MyProfiler.TaskInfo task)
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void OnBeginSimulationFrame(long frameNumber)
		{
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void InitThread(int viewPriority)
		{
		}

		public static void Commit()
		{
			if (Profiler != null)
			{
				MyRenderProfiler.Commit("Commit", 123, "E:\\Repo1\\Sources\\VRage\\Profiler\\ProfilerShort.cs");
			}
		}

		public static void DestroyThread()
		{
			if (Profiler != null)
			{
				MyRenderProfiler.DestroyThread();
			}
		}
	}
}
