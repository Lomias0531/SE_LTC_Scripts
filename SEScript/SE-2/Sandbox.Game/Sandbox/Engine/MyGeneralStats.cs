using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Threading;
using VRage;
using VRage.Game.Entity;
using VRage.Library.Memory;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Replication;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine
{
	public class MyGeneralStats
	{
		private struct MemoryLogger : MyMemoryTracker.ILogger
		{
			private string m_currentSystem;

			public void BeginSystem(string systemName)
			{
				if (m_currentSystem != null)
				{
					MyStatsGraph.Begin(m_currentSystem, int.MaxValue, string.Empty, 335, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				}
				m_currentSystem = systemName;
			}

			public void EndSystem(long systemBytes, int totalAllocations)
			{
				float customValue = totalAllocations;
				string customValueFormat = (totalAllocations > 0) ? "Allocs: {0}" : string.Empty;
				if (m_currentSystem != null)
				{
					MyStatsGraph.CustomTime(m_currentSystem, (float)systemBytes / 1024f / 1024f, "{0} MB", customValue, customValueFormat, "EndSystem", 348, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
					m_currentSystem = null;
				}
				else
				{
					MyStatsGraph.End((float)systemBytes / 1024f / 1024f, customValue, customValueFormat, "{0} MB", null, string.Empty, 353, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				}
			}
		}

		private MyTimeSpan m_lastTime;

		private static int AVERAGE_WINDOW_SIZE;

		private static int SERVER_AVERAGE_WINDOW_SIZE;

		private readonly MyMovingAverage m_received = new MyMovingAverage(AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_sent = new MyMovingAverage(AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_timeIntervals = new MyMovingAverage(AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_serverReceived = new MyMovingAverage(SERVER_AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_serverSent = new MyMovingAverage(SERVER_AVERAGE_WINDOW_SIZE);

		private readonly MyMovingAverage m_serverTimeIntervals = new MyMovingAverage(SERVER_AVERAGE_WINDOW_SIZE);

		public MyTimeSpan LogInterval = MyTimeSpan.FromSeconds(60.0);

		private bool m_first = true;

		private MyTimeSpan m_lastLogTime;

		private MyTimeSpan m_firstLogTime;

		private int m_gridsCount;

		private int[] m_lastGcCount = new int[GC.MaxGeneration + 1];

		private int[] m_collectionsThisFrame = new int[GC.MaxGeneration + 1];

		public static MyGeneralStats Static
		{
			get;
			private set;
		}

		public float Received
		{
			get;
			private set;
		}

		public float Sent
		{
			get;
			private set;
		}

		public float ReceivedPerSecond
		{
			get;
			private set;
		}

		public float SentPerSecond
		{
			get;
			private set;
		}

		public float PeakReceivedPerSecond
		{
			get;
			private set;
		}

		public float PeakSentPerSecond
		{
			get;
			private set;
		}

		public long OverallReceived
		{
			get;
			private set;
		}

		public long OverallSent
		{
			get;
			private set;
		}

		public float ServerReceivedPerSecond
		{
			get;
			private set;
		}

		public float ServerSentPerSecond
		{
			get;
			private set;
		}

		public float ServerGCMemory
		{
			get;
			private set;
		}

		public float ServerProcessMemory
		{
			get;
			private set;
		}

		public int GridsCount => m_gridsCount;

		public long Ping
		{
			get;
			set;
		}

		public bool LowNetworkQuality
		{
			get;
			private set;
		}

		static MyGeneralStats()
		{
			AVERAGE_WINDOW_SIZE = 60;
			SERVER_AVERAGE_WINDOW_SIZE = 6;
			Static = new MyGeneralStats();
		}

		public void Update()
		{
			MyNetworkReader.GetAndClearStats(out int received, out int tamperred);
			int andClearStats = MyNetworkWriter.GetAndClearStats();
			OverallReceived += received;
			OverallSent += andClearStats;
			MyTimeSpan simulationTime = MySandboxGame.Static.SimulationTime;
			float value = (float)(simulationTime - m_lastTime).Seconds;
			m_lastTime = simulationTime;
			m_received.Enqueue(received);
			m_sent.Enqueue(andClearStats);
			m_timeIntervals.Enqueue(value);
			Received = m_received.Avg;
			Sent = m_sent.Avg;
			ReceivedPerSecond = (float)(m_received.Sum / m_timeIntervals.Sum);
			SentPerSecond = (float)(m_sent.Sum / m_timeIntervals.Sum);
			if (ReceivedPerSecond > PeakReceivedPerSecond)
			{
				PeakReceivedPerSecond = ReceivedPerSecond;
			}
			if (SentPerSecond > PeakSentPerSecond)
			{
				PeakSentPerSecond = SentPerSecond;
			}
			float gCMemory = MyVRage.Platform.GCMemory;
			float num = (float)MyVRage.Platform.ProcessPrivateMemory / 1024f / 1024f;
			for (int i = 0; i < GC.MaxGeneration; i++)
			{
				int num2 = GC.CollectionCount(i);
				m_collectionsThisFrame[i] = num2 - m_lastGcCount[i];
				m_lastGcCount[i] = num2;
			}
			if (Sync.MultiplayerActive && Sync.IsServer)
			{
				MyMultiplayer.Static.ReplicationLayer.UpdateStatisticsData(andClearStats, received, tamperred, gCMemory, num);
			}
			if (MySession.Static != null && simulationTime > m_lastLogTime + LogInterval)
			{
				m_lastLogTime = simulationTime;
				if (m_first)
				{
					m_firstLogTime = simulationTime;
					m_first = false;
				}
				MyLog.Default.WriteLine("STATISTICS LEGEND,time,ReceivedPerSecond,SentPerSecond,PeakReceivedPerSecond,PeakSentPerSecond,OverallReceived,OverallSent,CPULoadSmooth,ThreadLoadSmooth,GetOnlinePlayerCount,Ping,GCMemory,ProcessMemory,PCUBuilt,PCU,GridsCount,RenderCPULoadSmooth,RenderGPULoadSmooth,HardwareCPULoad,HardwareAvailableMemory,FrameTime");
				float cPUCounter = MyVRage.Platform.CPUCounter;
				float rAMCounter = MyVRage.Platform.RAMCounter;
				MyLog.Default.WriteLine($"STATISTICS,{(simulationTime - m_firstLogTime).Seconds},{ReceivedPerSecond / 1024f / 1024f},{SentPerSecond / 1024f / 1024f},{PeakReceivedPerSecond / 1024f / 1024f},{PeakSentPerSecond / 1024f / 1024f},{(float)OverallReceived / 1024f / 1024f},{(float)OverallSent / 1024f / 1024f},{MySandboxGame.Static.CPULoadSmooth},{MySandboxGame.Static.ThreadLoadSmooth},{Sync.Players.GetOnlinePlayerCount()},{Ping},{gCMemory},{num},{MySession.Static.GlobalBlockLimits.PCUBuilt},{MySession.Static.GlobalBlockLimits.PCU},{GridsCount},{MyRenderProxy.CPULoadSmooth},{MyRenderProxy.GPULoadSmooth},{cPUCounter},{rAMCounter},{MyFpsManager.FrameTimeAvg}");
			}
			if (Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			MyPacketStatistics myPacketStatistics = default(MyPacketStatistics);
			if (Sync.IsServer)
			{
				ServerReceivedPerSecond = ReceivedPerSecond;
				ServerSentPerSecond = SentPerSecond;
			}
			else if (Sync.MultiplayerActive)
			{
				myPacketStatistics = MyMultiplayer.Static.ReplicationLayer.ClearServerStatistics();
				if (myPacketStatistics.TimeInterval > 0f)
				{
					m_serverReceived.Enqueue(myPacketStatistics.IncomingData);
					m_serverSent.Enqueue(myPacketStatistics.OutgoingData);
					m_serverTimeIntervals.Enqueue(myPacketStatistics.TimeInterval);
					ServerReceivedPerSecond = (float)(m_serverReceived.Sum / m_serverTimeIntervals.Sum);
					ServerSentPerSecond = (float)(m_serverSent.Sum / m_serverTimeIntervals.Sum);
					ServerGCMemory = myPacketStatistics.GCMemory;
					ServerProcessMemory = myPacketStatistics.ProcessMemory;
				}
			}
			MyStatsGraph.Begin("Client Traffic Avg", int.MaxValue, "Update", 171, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Outgoing avg", SentPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 172, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Incoming avg", ReceivedPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 173, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End((SentPerSecond + ReceivedPerSecond) / 1024f, 0f, "", "{0} kB/s", null, "Update", 174, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Traffic Avg", int.MaxValue, "Update", 175, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Outgoing avg", ServerSentPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 176, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Incoming avg", ServerReceivedPerSecond / 1024f, "{0} kB/s", 0f, "", "Update", 177, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End((ServerSentPerSecond + ServerReceivedPerSecond) / 1024f, 0f, "", "{0} kB/s", null, "Update", 178, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Client Perf Avg", int.MaxValue, "Update", 180, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", MySandboxGame.Static.CPULoadSmooth, "{0}%", 0f, "", "Update", 181, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", MySandboxGame.Static.ThreadLoadSmooth, "{0}%", 0f, "", "Update", 182, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render CPU", MyRenderProxy.CPULoadSmooth, "{0}%", 0f, "", "Update", 183, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render GPU", MyRenderProxy.GPULoadSmooth, "{0}%", 0f, "", "Update", 184, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render Frame", MyFpsManager.FrameTimeAvg, "{0}ms", 0f, "", "Update", 185, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(MySandboxGame.Static.CPULoadSmooth, 0f, null, "{0}%", null, "Update", 186, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Perf Avg", int.MaxValue, "Update", 187, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoadSmooth, "{0}%", 0f, "", "Update", 188, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoadSmooth, "{0}%", 0f, "", "Update", 189, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(Sync.ServerCPULoadSmooth, 0f, null, "{0}%", null, "Update", 190, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			if (MySession.Static != null)
			{
				MyStatsGraph.Begin("World", int.MaxValue, "Update", 195, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("PCUBuilt", MySession.Static.GlobalBlockLimits.PCUBuilt, "{0}", 0f, "", "Update", 196, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("PCU", MySession.Static.GlobalBlockLimits.PCU, "{0}", 0f, "", "Update", 197, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("PiratePCUBuilt", MySession.Static.PirateBlockLimits.PCUBuilt, "{0}", 0f, "", "Update", 198, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("PiratePCU", MySession.Static.PirateBlockLimits.PCU, "{0}", 0f, "", "Update", 199, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("GridsCount", GridsCount, "{0}", 0f, "", "Update", 200, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.End(null, 0f, "", "{0} B", null, "Update", 201, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			}
			MyStatsGraph.Begin("Memory", int.MaxValue, "Update", 204, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Overview", int.MaxValue, "Update", 208, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Collections", int.MaxValue, "Update", 211, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			for (int j = 0; j < m_collectionsThisFrame.Length; j++)
			{
				MyStatsGraph.CustomTime("Gen" + j, m_collectionsThisFrame[j], "{0}", 0f, "", "Update", 214, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			}
			MyStatsGraph.End(null, 0f, "", "{0} B", null, "Update", 216, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Client GC", gCMemory, "{0} MB", 0f, "", "Update", 218, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Client Process", num, "{0} MB", 0f, "", "Update", 219, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Server GC", ServerGCMemory, "{0} MB", 0f, "", "Update", 220, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Server Process", ServerProcessMemory, "{0} MB", 0f, "", "Update", 221, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(null, 0f, "", "{0} MB", null, "Update", 225, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MemoryLogger logger = default(MemoryLogger);
			Singleton<MyMemoryTracker>.Instance.LogMemoryStats(ref logger);
			MyStatsGraph.End(null, 0f, "", "{0} B", null, "Update", 231, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			if (Sync.MultiplayerActive)
			{
				MyPacketStatistics myPacketStatistics2 = MyMultiplayer.Static.ReplicationLayer.ClearClientStatistics();
				int num3 = myPacketStatistics2.Drops + myPacketStatistics2.OutOfOrder + myPacketStatistics2.Duplicates + myPacketStatistics.PendingPackets + myPacketStatistics.Drops + myPacketStatistics.OutOfOrder + myPacketStatistics.Duplicates;
				MyStatsGraph.Begin("Packet errors", int.MaxValue, "Update", 238, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client Drops", myPacketStatistics2.Drops, "{0}", 0f, "", "Update", 239, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client OutOfOrder", myPacketStatistics2.OutOfOrder, "{0}", 0f, "", "Update", 240, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client Duplicates", myPacketStatistics2.Duplicates, "{0}", 0f, "", "Update", 241, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Client Tamperred", myPacketStatistics2.Tamperred, "{0}", 0f, "", "Update", 242, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Pending Packets", (int)myPacketStatistics.PendingPackets, "{0}", 0f, "", "Update", 243, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Drops", myPacketStatistics.Drops, "{0}", 0f, "", "Update", 244, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server OutOfOrder", myPacketStatistics.OutOfOrder, "{0}", 0f, "", "Update", 245, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Duplicates", myPacketStatistics.Duplicates, "{0}", 0f, "", "Update", 246, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Server Tamperred", myPacketStatistics2.Tamperred, "{0}", 0f, "", "Update", 247, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.End(num3, 0f, null, "{0}", null, "Update", 248, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				LowNetworkQuality = (num3 > 5);
			}
			else
			{
				LowNetworkQuality = false;
			}
			if (MySession.Static != null)
			{
				MyStatsGraph.Begin("Physics", int.MaxValue, "Update", 258, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("Clusters", MyPhysics.Clusters.GetClusters().Count, "{0}", 0f, "", "Update", 260, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("VoxelBodies", MyVoxelPhysicsBody.ActiveVoxelPhysicsBodies, "{0}", 0f, "", "Update", 261, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.CustomTime("LargeVoxelBodies", MyVoxelPhysicsBody.ActiveVoxelPhysicsBodiesWithExtendedCache, "{0}", 0f, "", "Update", 262, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
				MyStatsGraph.End(0f, 0f, null, "{0}", null, "Update", 264, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			}
			MyStatsGraph.ProfileAdvanced(begin: true);
			MyStatsGraph.Begin("Traffic", int.MaxValue, "Update", 269, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Outgoing", Sent / 1024f, "{0} kB", 0f, "", "Update", 270, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Incoming", Received / 1024f, "{0} kB", 0f, "", "Update", 271, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End((SentPerSecond + ReceivedPerSecond) / 1024f, 0f, "", "{0} kB", null, "Update", 272, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Perf Avg", int.MaxValue, "Update", 274, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoadSmooth, "{0}%", 0f, "", "Update", 275, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoadSmooth, "{0}%", 0f, "", "Update", 276, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(0f, 0f, null, "{0}", null, "Update", 277, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Client Performance", int.MaxValue, "Update", 279, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", MySandboxGame.Static.CPULoad, "{0}%", 0f, "", "Update", 280, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", MySandboxGame.Static.ThreadLoad, "{0}%", 0f, "", "Update", 281, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render CPU", MyRenderProxy.CPULoad, "{0}%", 0f, "", "Update", 282, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Render GPU", MyRenderProxy.GPULoad, "{0}%", 0f, "", "Update", 283, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(0f, 0f, null, "{0}", null, "Update", 284, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.Begin("Server Performance", int.MaxValue, "Update", 286, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Main CPU", Sync.ServerCPULoad, "{0}%", 0f, "", "Update", 287, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.CustomTime("Threads", Sync.ServerThreadLoad, "{0}%", 0f, "", "Update", 288, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.End(0f, 0f, null, "{0}", null, "Update", 289, "E:\\Repo1\\Sources\\Sandbox.Game\\Engine\\MyGeneralStats.cs");
			MyStatsGraph.ProfileAdvanced(begin: false);
		}

		public void LoadData()
		{
			m_gridsCount = 0;
			MyEntities.OnEntityCreate += OnEntityCreate;
			MyEntities.OnEntityDelete += OnEntityDelete;
		}

		private void OnEntityCreate(MyEntity entity)
		{
			if (entity is MyCubeGrid)
			{
				Interlocked.Increment(ref m_gridsCount);
			}
		}

		private void OnEntityDelete(MyEntity entity)
		{
			if (entity is MyCubeGrid)
			{
				Interlocked.Decrement(ref m_gridsCount);
			}
		}

		public static void Clear()
		{
			MyNetworkWriter.GetAndClearStats();
			MyNetworkReader.GetAndClearStats(out int _, out int _);
		}

		public static void ToggleProfiler()
		{
			MyRenderProfiler.EnableAutoscale(MyStatsGraph.PROFILER_NAME);
			MyRenderProfiler.ToggleProfiler(MyStatsGraph.PROFILER_NAME);
		}
	}
}
