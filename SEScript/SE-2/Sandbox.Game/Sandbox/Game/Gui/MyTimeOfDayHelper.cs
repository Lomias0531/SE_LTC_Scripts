using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using VRage.Network;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	internal static class MyTimeOfDayHelper
	{
		protected sealed class UpdateTimeOfDayServer_003C_003ESystem_Single : ICallSite<IMyEventOwner, float, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in float time, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateTimeOfDayServer(time);
			}
		}

		protected sealed class UpdateTimeOfDayClient_003C_003ESystem_Int64_0023System_Single : ICallSite<IMyEventOwner, long, float, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long ticks, in float time, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateTimeOfDayClient(ticks, time);
			}
		}

		private static float timeOfDay;

		private static TimeSpan? OriginalTime;

		internal static float TimeOfDay => timeOfDay;

		internal static void UpdateTimeOfDay(float time)
		{
			timeOfDay = time;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateTimeOfDayServer, time);
		}

		public static void Reset()
		{
			timeOfDay = 0f;
			OriginalTime = null;
		}

		[Event(null, 30)]
		[Reliable]
		[Server]
		private static void UpdateTimeOfDayServer(float time)
		{
			if (MySession.Static == null)
			{
				return;
			}
			if (!MySession.Static.IsUserAdmin(MyEventContext.Current.IsLocallyInvoked ? Sync.MyId : MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			if (!OriginalTime.HasValue)
			{
				OriginalTime = MySession.Static.ElapsedGameTime;
			}
			MySession.Static.ElapsedGameTime = OriginalTime.Value.Add(new TimeSpan(0, 0, (int)(60f * time)));
			timeOfDay = time;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateTimeOfDayClient, MySession.Static.ElapsedGameTime.Ticks, time);
		}

		[Event(null, 53)]
		[Reliable]
		[Broadcast]
		private static void UpdateTimeOfDayClient(long ticks, float time)
		{
			timeOfDay = time;
			MySession.Static.ElapsedGameTime = new TimeSpan(ticks);
		}
	}
}
