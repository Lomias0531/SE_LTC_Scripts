using Havok;
using Sandbox.Engine;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	[MyDebugScreen("VRage", "System")]
	internal class MyGuiScreenDebugSystem : MyGuiScreenDebugBase
	{
		protected sealed class HavokMemoryStatsRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				HavokMemoryStatsRequest();
			}
		}

		protected sealed class HavokMemoryStatsReply_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string stats, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				HavokMemoryStatsReply(stats);
			}
		}

		private MyGuiControlMultilineText m_havokStatsMultiline;

		private static StringBuilder m_buffer = new StringBuilder();

		private static string m_statsFromServer = string.Empty;

		public MyGuiScreenDebugSystem()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("System debug", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddLabel("System", Color.Yellow.ToVector4(), 1.2f);
			AddCheckBox("Simulate slow update", null, MemberHelper.GetMember(() => MyFakes.SIMULATE_SLOW_UPDATE));
			AddButton(new StringBuilder("Force GC"), OnClick_ForceGC);
			AddCheckBox("Pause physics", null, MemberHelper.GetMember(() => MyFakes.PAUSE_PHYSICS));
			AddButton(new StringBuilder("Step physics"), delegate
			{
				MyFakes.STEP_PHYSICS = true;
			});
			AddSlider("Simulation speed", 0.001f, 3f, null, MemberHelper.GetMember(() => MyFakes.SIMULATION_SPEED));
			AddSlider("Statistics Logging Frequency [s]", (float)MyGeneralStats.Static.LogInterval.Seconds, 0f, 120f, delegate(MyGuiControlSlider slider)
			{
				MyGeneralStats.Static.LogInterval = MyTimeSpan.FromSeconds(slider.Value);
			});
			if (MySession.Static != null && MySession.Static.Settings != null)
			{
				AddCheckBox("Enable save", MySession.Static.Settings, MemberHelper.GetMember(() => MySession.Static.Settings.EnableSaving));
			}
			AddCheckBox("Optimize grid update", null, MemberHelper.GetMember(() => MyFakes.OPTIMIZE_GRID_UPDATES));
			AddButton(new StringBuilder("Clear achievements and stats"), delegate
			{
				MyGameService.ResetAllStats(achievementsToo: true);
				MyGameService.StoreStats();
			});
			m_currentPosition.Y += 0.01f;
			m_havokStatsMultiline = AddMultilineText(null, null, 0.8f);
		}

		public override bool Draw()
		{
			m_havokStatsMultiline.Clear();
			m_havokStatsMultiline.AppendText(GetHavokMemoryStats());
			return base.Draw();
		}

		private static string GetHavokMemoryStats()
		{
			if (Sync.IsServer || MySession.Static == null)
			{
				m_buffer.Append("Out of mem: ").Append(HkBaseSystem.IsOutOfMemory).AppendLine();
				HkBaseSystem.GetMemoryStatistics(m_buffer);
				string result = m_buffer.ToString();
				m_buffer.Clear();
				return result;
			}
			if (MySession.Static.GameplayFrameCounter % 100 == 0)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => HavokMemoryStatsRequest);
			}
			return m_statsFromServer;
		}

		[Event(null, 109)]
		[Reliable]
		[Server]
		private static void HavokMemoryStatsRequest()
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => HavokMemoryStatsReply, GetHavokMemoryStats(), MyEventContext.Current.Sender);
			}
		}

		[Event(null, 121)]
		[Reliable]
		[Client]
		private static void HavokMemoryStatsReply(string stats)
		{
			m_statsFromServer = stats;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugSystem";
		}

		private void OnClick_ForceGC(MyGuiControlButton button)
		{
			GC.Collect();
		}
	}
}
