using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Utils;

namespace VRageRender
{
	public static class MyPerformanceCounter
	{
		public struct Timer
		{
			private static Stopwatch m_timer;

			public static readonly Timer Empty;

			public long StartTime;

			public long Runtime;

			public float RuntimeMs => (float)((double)Runtime / (double)Stopwatch.Frequency * 1000.0);

			private bool IsRunning => StartTime != long.MaxValue;

			static Timer()
			{
				Empty = new Timer
				{
					Runtime = 0L,
					StartTime = long.MaxValue
				};
				m_timer = new Stopwatch();
				m_timer.Start();
			}

			public void Start()
			{
				StartTime = m_timer.ElapsedTicks;
			}

			public void Stop()
			{
				Runtime += m_timer.ElapsedTicks - StartTime;
				StartTime = long.MaxValue;
			}
		}

		public class MyPerCameraDraw
		{
			public readonly Dictionary<string, Timer> CustomTimers = new Dictionary<string, Timer>(5);

			public readonly Dictionary<string, float> CustomCounters = new Dictionary<string, float>(5);

			private long m_gcMemory;

			private readonly List<string> m_tmpKeys = new List<string>();

			public long GcMemory
			{
				get
				{
					return Interlocked.Read(ref m_gcMemory);
				}
				set
				{
					Interlocked.Exchange(ref m_gcMemory, value);
				}
			}

			public float this[string name]
			{
				get
				{
					if (!CustomCounters.TryGetValue(name, out float value))
					{
						return 0f;
					}
					return value;
				}
				set
				{
					CustomCounters[name] = value;
				}
			}

			public List<string> SortedCounterKeys
			{
				get
				{
					m_tmpKeys.Clear();
					return m_tmpKeys;
				}
			}

			public MyPerCameraDraw()
			{
				MyUtils.GetMaxValueFromEnum<MyLodTypeEnum>();
			}

			public void SetCounter(string name, float count)
			{
				CustomCounters[name] = count;
			}

			public void StartTimer(string name)
			{
				CustomTimers.TryGetValue(name, out Timer value);
				value.Start();
				CustomTimers[name] = value;
			}

			public void StopTimer(string name)
			{
				if (CustomTimers.TryGetValue(name, out Timer value))
				{
					value.Stop();
					CustomTimers[name] = value;
				}
			}

			public void Reset()
			{
				ClearCustomCounters();
				GcMemory = GC.GetTotalMemory(forceFullCollection: false);
			}

			public void ClearCustomCounters()
			{
				m_tmpKeys.Clear();
			}
		}

		public class MyPerAppLifetime
		{
			public int Textures2DCount;

			public int Textures2DSizeInPixels;

			public double Textures2DSizeInMb;

			public int NonMipMappedTexturesCount;

			public int NonDxtCompressedTexturesCount;

			public int DxtCompressedTexturesCount;

			public int TextureCubesCount;

			public int TextureCubesSizeInPixels;

			public double TextureCubesSizeInMb;

			public int ModelsCount;

			public int MyModelsCount;

			public int MyModelsMeshesCount;

			public int MyModelsVertexesCount;

			public int MyModelsTrianglesCount;

			public int ModelVertexBuffersSize;

			public int ModelIndexBuffersSize;

			public int VoxelVertexBuffersSize;

			public int VoxelIndexBuffersSize;

			public int MyModelsFilesSize;

			public List<string> LoadedTextureFiles = new List<string>();

			public List<string> LoadedModelFiles = new List<string>();
		}

		public const int NoSplit = 4;

		private static MyPerCameraDraw PerCameraDraw0 = new MyPerCameraDraw();

		private static MyPerCameraDraw PerCameraDraw1 = new MyPerCameraDraw();

		public static MyPerCameraDraw PerCameraDrawRead = PerCameraDraw0;

		public static MyPerCameraDraw PerCameraDrawWrite = PerCameraDraw0;

		public static MyPerAppLifetime PerAppLifetime = new MyPerAppLifetime();

		public static bool LogFiles = false;

		public static void Restart(string name)
		{
			PerCameraDrawRead.CustomTimers.Remove(name);
			PerCameraDrawWrite.CustomTimers.Remove(name);
			PerCameraDrawRead.StartTimer(name);
			PerCameraDrawWrite.StartTimer(name);
		}

		public static void Stop(string name)
		{
			PerCameraDrawRead.StopTimer(name);
			PerCameraDrawWrite.StopTimer(name);
		}

		internal static void SwitchCounters()
		{
			if (PerCameraDrawRead == PerCameraDraw0)
			{
				PerCameraDrawRead = PerCameraDraw1;
				PerCameraDrawWrite = PerCameraDraw0;
			}
			else
			{
				PerCameraDrawRead = PerCameraDraw0;
				PerCameraDrawWrite = PerCameraDraw1;
			}
		}
	}
}
