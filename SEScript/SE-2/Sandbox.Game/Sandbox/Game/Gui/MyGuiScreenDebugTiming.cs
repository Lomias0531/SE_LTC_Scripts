using Havok;
using Sandbox.Engine;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Debugging;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Network;
using VRage.Stats;
using VRageMath;
using VRageRender;
using VRageRender.Utils;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	internal class MyGuiScreenDebugTiming : MyGuiScreenDebugBase
	{
		private long m_startTime;

		private long m_ticks;

		private int m_frameCounter;

		private double m_updateLag;

		private static int m_callChainDepth;

		private static int m_numInstructions;

		public MyGuiScreenDebugTiming()
			: base(new Vector2(0.5f, 0.5f), default(Vector2), null, isTopMostScreen: true)
		{
			m_isTopMostScreen = true;
			m_drawEvenWithoutFocus = true;
			base.CanHaveFocus = false;
			m_canShareInput = false;
		}

		public override void LoadData()
		{
			base.LoadData();
			MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.SimpleTimingStats;
		}

		public override void UnloadData()
		{
			base.UnloadData();
			MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.NoDraw;
		}

		public override string GetFriendlyName()
		{
			return "DebugTimingScreen";
		}

		public override bool Update(bool hasFocus)
		{
			m_ticks = Sandbox.Game.Debugging.MyPerformanceCounter.ElapsedTicks;
			m_frameCounter++;
			double num = Sandbox.Game.Debugging.MyPerformanceCounter.TicksToMs(m_ticks - m_startTime) / 1000.0;
			if (num > 1.0)
			{
				double num2 = num - (double)((float)m_frameCounter * 0.0166666675f);
				m_updateLag = num2 / num * 1000.0;
				m_startTime = m_ticks;
				m_frameCounter = 0;
			}
			Stats.Timing.Write(MyStatKeys.StatKeysEnum.Frame, (MySandboxGame.Static != null) ? MySandboxGame.Static.SimulationFrameCounter : 0, MyStatTypeEnum.CurrentValue, 0, 0);
			Stats.Timing.Write(MyStatKeys.StatKeysEnum.FPS, MyFpsManager.GetFps(), MyStatTypeEnum.CurrentValue, 0, 0);
			Stats.Timing.Increment(MyStatKeys.StatKeysEnum.UPS, 1000);
			Stats.Timing.Write(MyStatKeys.StatKeysEnum.SimSpeed, MyPhysics.SimulationRatio, MyStatTypeEnum.CurrentValue, 100, 2);
			Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.SimCpuLoad, (int)MySandboxGame.Static.CPULoadSmooth, MySandboxGame.Static.CPUTimeSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
			Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ThreadCpuLoad, (int)MySandboxGame.Static.ThreadLoadSmooth, MySandboxGame.Static.ThreadTimeSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
			Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.RenderCpuLoad, (int)MyRenderProxy.CPULoadSmooth, MyRenderProxy.CPUTimeSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
			Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.RenderGpuLoad, (int)MyRenderProxy.GPULoadSmooth, MyRenderProxy.GPUTimeSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
			if (Sync.Layer != null)
			{
				Stats.Timing.Write(MyStatKeys.StatKeysEnum.ServerSimSpeed, Sync.ServerSimulationRatio, MyStatTypeEnum.CurrentValue, 100, 2);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ServerSimCpuLoad, (int)Sync.ServerCPULoadSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ServerThreadCpuLoad, (int)Sync.ServerThreadLoadSmooth, MyStatTypeEnum.CurrentValue, 0, 0);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.Up, MyGeneralStats.Static.SentPerSecond / 1024f, MyStatTypeEnum.CurrentValue, 0, 0);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.Down, MyGeneralStats.Static.ReceivedPerSecond / 1024f, MyStatTypeEnum.CurrentValue, 0, 0);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ServerUp, MyGeneralStats.Static.ServerSentPerSecond / 1024f, MyStatTypeEnum.CurrentValue, 0, 0);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ServerDown, MyGeneralStats.Static.ServerReceivedPerSecond / 1024f, MyStatTypeEnum.CurrentValue, 0, 0);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.Roundtrip, MyGeneralStats.Static.Ping, MyStatTypeEnum.CurrentValue, 0, 0);
			}
			if (MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.ComplexTimingStats)
			{
				int cachedChuncks = 0;
				int pendingCachedChuncks = 0;
				if (MySession.Static != null)
				{
					MySession.Static.VoxelMaps.GetCacheStats(out cachedChuncks, out pendingCachedChuncks);
				}
				Stats.Timing.WriteFormat("Voxel cache size: {0} / {3}", cachedChuncks, pendingCachedChuncks, MyStatTypeEnum.CurrentValue, 0, 1);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.FrameTime, MyFpsManager.FrameTime, MyStatTypeEnum.CurrentValue, 0, 1);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.FrameAvgTime, MyFpsManager.FrameTimeAvg, MyStatTypeEnum.CurrentValue, 0, 1);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.FrameMinTime, MyFpsManager.FrameTimeMin, MyStatTypeEnum.CurrentValue, 0, 1);
				Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.FrameMaxTime, MyFpsManager.FrameTimeMax, MyStatTypeEnum.CurrentValue, 0, 1);
				Stats.Timing.Write(MyStatKeys.StatKeysEnum.UpdateLag, (float)m_updateLag, MyStatTypeEnum.CurrentValue, 0, 4);
				Stats.Timing.Write(MyStatKeys.StatKeysEnum.GcMemory, GC.GetTotalMemory(forceFullCollection: false), MyStatTypeEnum.CurrentValue, 0, 0);
				Stats.Timing.Write(MyStatKeys.StatKeysEnum.ActiveParticleEffs, MyParticlesManager.ParticleEffectsForUpdate.Count, MyStatTypeEnum.CurrentValue, 0, 0);
				if (MyPhysics.GetClusterList().HasValue)
				{
					double num3 = 0.0;
					double num4 = 0.0;
					double num5 = 0.0;
					long num6 = 0L;
					foreach (HkWorld item in MyPhysics.GetClusterList().Value)
					{
						num3 += 1.0;
						double totalMilliseconds = item.StepDuration.TotalMilliseconds;
						num4 += totalMilliseconds;
						if (totalMilliseconds > num5)
						{
							num5 = totalMilliseconds;
						}
						num6 += item.ActiveRigidBodies.Count;
					}
					Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.PhysWorldCount, (float)num3, MyStatTypeEnum.CurrentValue, 0, 0);
					Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.ActiveRigBodies, num6, MyStatTypeEnum.CurrentValue, 0, 1);
					Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.PhysStepTimeSum, (float)num4, MyStatTypeEnum.CurrentValue, 0, 1);
					Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.PhysStepTimeAvg, (float)(num4 / num3), MyStatTypeEnum.CurrentValue, 0, 1);
					Stats.Timing.WriteFormat(MyStatKeys.StatKeysEnum.PhysStepTimeMax, (float)num5, MyStatTypeEnum.CurrentValue, 0, 1);
				}
			}
			return base.Update(hasFocus);
		}
	}
}
