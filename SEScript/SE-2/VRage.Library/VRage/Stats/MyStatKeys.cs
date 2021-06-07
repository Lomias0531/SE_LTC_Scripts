using System.Collections.Generic;

namespace VRage.Stats
{
	public class MyStatKeys
	{
		public enum StatKeysEnum
		{
			None,
			Frame,
			FPS,
			UPS,
			SimSpeed,
			SimCpuLoad,
			ThreadCpuLoad,
			RenderCpuLoad,
			RenderGpuLoad,
			ServerSimSpeed,
			ServerSimCpuLoad,
			ServerThreadCpuLoad,
			Up,
			Down,
			ServerUp,
			ServerDown,
			Roundtrip,
			FrameTime,
			FrameAvgTime,
			FrameMinTime,
			FrameMaxTime,
			UpdateLag,
			GcMemory,
			ProcessMemory,
			ActiveParticleEffs,
			PhysWorldCount,
			ActiveRigBodies,
			PhysStepTimeSum,
			PhysStepTimeAvg,
			PhysStepTimeMax
		}

		public struct MyNamePriorityPair
		{
			public string Name;

			public int Priority;
		}

		private static Dictionary<StatKeysEnum, MyNamePriorityPair> m_collection;

		static MyStatKeys()
		{
			Dictionary<StatKeysEnum, MyNamePriorityPair> dictionary = new Dictionary<StatKeysEnum, MyNamePriorityPair>();
			MyNamePriorityPair value = new MyNamePriorityPair
			{
				Name = "Frame",
				Priority = 1100
			};
			dictionary.Add(StatKeysEnum.Frame, value);
			value = new MyNamePriorityPair
			{
				Name = "FPS",
				Priority = 1000
			};
			dictionary.Add(StatKeysEnum.FPS, value);
			value = new MyNamePriorityPair
			{
				Name = "UPS",
				Priority = 900
			};
			dictionary.Add(StatKeysEnum.UPS, value);
			value = new MyNamePriorityPair
			{
				Name = "Simulation speed",
				Priority = 800
			};
			dictionary.Add(StatKeysEnum.SimSpeed, value);
			value = new MyNamePriorityPair
			{
				Name = "Simulation CPU Load: {0}% {3:0.00}ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.SimCpuLoad, value);
			value = new MyNamePriorityPair
			{
				Name = "Thread CPU Load: {0}% {3:0.00}ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.ThreadCpuLoad, value);
			value = new MyNamePriorityPair
			{
				Name = "Render CPU Load: {0}% {3:0.00}ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.RenderCpuLoad, value);
			value = new MyNamePriorityPair
			{
				Name = "Render GPU Load: {0}% {3:0.00}ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.RenderGpuLoad, value);
			value = new MyNamePriorityPair
			{
				Name = "Server simulation speed",
				Priority = 700
			};
			dictionary.Add(StatKeysEnum.ServerSimSpeed, value);
			value = new MyNamePriorityPair
			{
				Name = "Server simulation CPU Load: {0}%",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.ServerSimCpuLoad, value);
			value = new MyNamePriorityPair
			{
				Name = "Server thread CPU Load: {0}%",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.ServerThreadCpuLoad, value);
			value = new MyNamePriorityPair
			{
				Name = "Up: {0.##} kB/s",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.Up, value);
			value = new MyNamePriorityPair
			{
				Name = "Down: {0.##} kB/s",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.Down, value);
			value = new MyNamePriorityPair
			{
				Name = "Server Up: {0.##} kB/s",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.ServerUp, value);
			value = new MyNamePriorityPair
			{
				Name = "Server Down: {0.##} kB/s",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.ServerDown, value);
			value = new MyNamePriorityPair
			{
				Name = "Roundtrip: {0}ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.Roundtrip, value);
			value = new MyNamePriorityPair
			{
				Name = "Frame time: {0} ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.FrameTime, value);
			value = new MyNamePriorityPair
			{
				Name = "Frame avg time: {0} ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.FrameAvgTime, value);
			value = new MyNamePriorityPair
			{
				Name = "Frame min time: {0} ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.FrameMinTime, value);
			value = new MyNamePriorityPair
			{
				Name = "Frame max time: {0} ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.FrameMaxTime, value);
			value = new MyNamePriorityPair
			{
				Name = "Update lag (per s)",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.UpdateLag, value);
			value = new MyNamePriorityPair
			{
				Name = "GC Memory",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.GcMemory, value);
			value = new MyNamePriorityPair
			{
				Name = "Process memory",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.ProcessMemory, value);
			value = new MyNamePriorityPair
			{
				Name = "Active particle effects",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.ActiveParticleEffs, value);
			value = new MyNamePriorityPair
			{
				Name = "Physics worlds count: {0}",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.PhysWorldCount, value);
			value = new MyNamePriorityPair
			{
				Name = "Active rigid bodies: {0}",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.ActiveRigBodies, value);
			value = new MyNamePriorityPair
			{
				Name = "Physics step time (sum): {0} ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.PhysStepTimeSum, value);
			value = new MyNamePriorityPair
			{
				Name = "Physics step time (avg): {0} ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.PhysStepTimeAvg, value);
			value = new MyNamePriorityPair
			{
				Name = "Physics step time (max): {0} ms",
				Priority = 0
			};
			dictionary.Add(StatKeysEnum.PhysStepTimeMax, value);
			m_collection = dictionary;
		}

		public static string GetName(StatKeysEnum key)
		{
			if (!m_collection.TryGetValue(key, out MyNamePriorityPair value))
			{
				return string.Empty;
			}
			return value.Name;
		}

		public static int GetPriority(StatKeysEnum key)
		{
			if (!m_collection.TryGetValue(key, out MyNamePriorityPair value))
			{
				return 0;
			}
			return value.Priority;
		}

		public static void GetNameAndPriority(StatKeysEnum key, out string name, out int priority)
		{
			if (!m_collection.TryGetValue(key, out MyNamePriorityPair value))
			{
				name = string.Empty;
				priority = 0;
			}
			else
			{
				name = value.Name;
				priority = value.Priority;
			}
		}
	}
}
