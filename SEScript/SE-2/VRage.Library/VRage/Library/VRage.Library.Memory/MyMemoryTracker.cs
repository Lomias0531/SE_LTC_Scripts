using ParallelTasks;

namespace VRage.Library.Memory
{
	public class MyMemoryTracker : Singleton<MyMemoryTracker>
	{
		public interface ILogger
		{
			void BeginSystem(string systemName);

			void EndSystem(long systemBytes, int totalAllocations);
		}

		public const bool ENABLED = true;

		public MyMemorySystem ProcessMemorySystem
		{
			get;
		}

		public MyMemoryTracker()
		{
			ProcessMemorySystem = MyMemorySystem.CreateRootMemorySystem("Systems");
		}

		public void LogMemoryStats<TLogger>(ref TLogger logger) where TLogger : struct, ILogger
		{
			ProcessMemorySystem.LogMemoryStats(ref logger);
		}
	}
}
