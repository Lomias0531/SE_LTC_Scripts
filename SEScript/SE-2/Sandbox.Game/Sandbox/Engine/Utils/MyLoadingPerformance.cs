using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sandbox.Engine.Utils
{
	internal class MyLoadingPerformance
	{
		private static MyLoadingPerformance m_instance;

		private Dictionary<uint, Tuple<int, string>> m_voxelCounts = new Dictionary<uint, Tuple<int, string>>();

		private TimeSpan m_loadingTime;

		private Stopwatch m_stopwatch;

		public static MyLoadingPerformance Instance => m_instance ?? (m_instance = new MyLoadingPerformance());

		public string LoadingName
		{
			get;
			set;
		}

		public bool IsTiming
		{
			get;
			private set;
		}

		private void Reset()
		{
			LoadingName = null;
			m_loadingTime = TimeSpan.Zero;
			m_voxelCounts.Clear();
		}

		public void StartTiming()
		{
			if (!IsTiming)
			{
				Reset();
				IsTiming = true;
				m_stopwatch = Stopwatch.StartNew();
			}
		}

		public void AddVoxelHandCount(int count, uint entityID, string name)
		{
			if (IsTiming && !m_voxelCounts.ContainsKey(entityID))
			{
				m_voxelCounts.Add(entityID, new Tuple<int, string>(count, name));
			}
		}

		public void FinishTiming()
		{
			m_stopwatch.Stop();
			IsTiming = false;
			m_loadingTime = m_stopwatch.Elapsed;
			WriteToLog();
		}

		public void WriteToLog()
		{
			MySandboxGame.Log.WriteLine("LOADING REPORT FOR: " + LoadingName);
			MySandboxGame.Log.IncreaseIndent();
			MySandboxGame.Log.WriteLine("Loading time: " + m_loadingTime);
			MySandboxGame.Log.IncreaseIndent();
			foreach (KeyValuePair<uint, Tuple<int, string>> voxelCount in m_voxelCounts)
			{
				if (voxelCount.Value.Item1 > 0)
				{
					MySandboxGame.Log.WriteLine("Asteroid: " + voxelCount.Key + " voxel hands: " + voxelCount.Value.Item1 + ". Voxel File: " + voxelCount.Value.Item2);
				}
			}
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.DecreaseIndent();
			MySandboxGame.Log.WriteLine("END OF LOADING REPORT");
		}
	}
}
