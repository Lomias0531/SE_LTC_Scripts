using System.Diagnostics;
using VRage.Utils;

namespace Sandbox.Game.AI.Pathfinding
{
	internal static class MyPathfindingStopwatch
	{
		private static Stopwatch s_stopWatch;

		private static Stopwatch s_gloabalStopwatch;

		private static MyLog s_log;

		private const int StopTimeMs = 10000;

		private static int s_levelOfStarting;

		static MyPathfindingStopwatch()
		{
			s_stopWatch = null;
			s_gloabalStopwatch = null;
			s_log = new MyLog();
			s_levelOfStarting = 0;
			s_stopWatch = new Stopwatch();
			s_gloabalStopwatch = new Stopwatch();
			s_log = new MyLog();
		}

		[Conditional("DEBUG")]
		public static void StartMeasuring()
		{
			s_stopWatch.Reset();
			s_gloabalStopwatch.Reset();
			s_gloabalStopwatch.Start();
		}

		[Conditional("DEBUG")]
		public static void CheckStopMeasuring()
		{
			if (s_gloabalStopwatch.IsRunning)
			{
				_ = s_gloabalStopwatch.ElapsedMilliseconds;
				_ = 10000;
			}
		}

		[Conditional("DEBUG")]
		public static void StopMeasuring()
		{
			s_gloabalStopwatch.Stop();
			string msg = $"pathfinding elapsed time: {s_stopWatch.ElapsedMilliseconds} ms / in {10000} ms";
			s_log.WriteLineAndConsole(msg);
		}

		[Conditional("DEBUG")]
		public static void Start()
		{
			if (!s_stopWatch.IsRunning)
			{
				s_stopWatch.Start();
				s_levelOfStarting = 1;
			}
			else
			{
				s_levelOfStarting++;
			}
		}

		[Conditional("DEBUG")]
		public static void Stop()
		{
			if (s_stopWatch.IsRunning)
			{
				s_levelOfStarting--;
				if (s_levelOfStarting == 0)
				{
					s_stopWatch.Stop();
				}
			}
		}

		[Conditional("DEBUG")]
		public static void Reset()
		{
			s_stopWatch.Reset();
		}
	}
}
