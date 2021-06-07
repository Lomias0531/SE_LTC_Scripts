using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.World;
using System;
using VRage.Network;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	[PreloadRequired]
	public static class MySyncScenario
	{
		protected sealed class OnAskInfo_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnAskInfo();
			}
		}

		protected sealed class OnAnswerInfo_003C_003ESystem_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, bool, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool isRunning, in bool canJoin, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnAnswerInfo(isRunning, canJoin);
			}
		}

		protected sealed class OnSetTimeoutClient_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int index, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSetTimeoutClient(index);
			}
		}

		protected sealed class OnSetJoinRunningClient_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool canJoin, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSetJoinRunningClient(canJoin);
			}
		}

		protected sealed class OnPrepareScenarioFromLobby_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long PrepStartTime, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPrepareScenarioFromLobby(PrepStartTime);
			}
		}

		protected sealed class OnPlayerReadyToStartScenario_003C_003ESystem_UInt64 : ICallSite<IMyEventOwner, ulong, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong playerSteamId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPlayerReadyToStartScenario(playerSteamId);
			}
		}

		protected sealed class OnStartScenario_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gameStartTime, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnStartScenario(gameStartTime);
			}
		}

		protected sealed class OnSetTimeoutBroadcast_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int index, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSetTimeoutBroadcast(index);
			}
		}

		protected sealed class OnSetJoinRunningBroadcast_003C_003ESystem_Boolean : ICallSite<IMyEventOwner, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool canJoin, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnSetJoinRunningBroadcast(canJoin);
			}
		}

		internal static event Action<bool, bool> InfoAnswer;

		internal static event Action<long> PrepareScenario;

		internal static event Action<ulong> PlayerReadyToStartScenario;

		internal static event Action ClientWorldLoaded;

		internal static event Action<long> StartScenario;

		internal static event Action<int> TimeoutReceived;

		internal static event Action<bool> CanJoinRunningReceived;

		internal static void AskInfo()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnAskInfo);
		}

		[Event(null, 35)]
		[Reliable]
		[Server]
		private static void OnAskInfo()
		{
			EndpointId targetEndpoint = (!MyEventContext.Current.IsLocallyInvoked) ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
			bool flag = MyMultiplayer.Static.ScenarioStartTime > DateTime.MinValue;
			bool arg = !flag || MySession.Static.Settings.CanJoinRunning;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnAnswerInfo, flag, arg, targetEndpoint);
			int selectedIndex = MyGuiScreenScenarioMpBase.Static.TimeoutCombo.GetSelectedIndex();
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnSetTimeoutClient, selectedIndex, targetEndpoint);
			bool canJoinRunning = MySession.Static.Settings.CanJoinRunning;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnSetJoinRunningClient, canJoinRunning, targetEndpoint);
		}

		[Event(null, 55)]
		[Reliable]
		[Client]
		private static void OnAnswerInfo(bool isRunning, bool canJoin)
		{
			if (MySyncScenario.InfoAnswer != null)
			{
				MySyncScenario.InfoAnswer(isRunning, canJoin);
			}
		}

		[Event(null, 62)]
		[Reliable]
		[Client]
		private static void OnSetTimeoutClient(int index)
		{
			OnSetTimeout(index);
		}

		[Event(null, 68)]
		[Reliable]
		[Client]
		private static void OnSetJoinRunningClient(bool canJoin)
		{
			OnSetJoinRunning(canJoin);
		}

		internal static void PrepareScenarioFromLobby(long preparationStartTime)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPrepareScenarioFromLobby, preparationStartTime);
			}
		}

		[Event(null, 87)]
		[Reliable]
		[Broadcast]
		public static void OnPrepareScenarioFromLobby(long PrepStartTime)
		{
			if (MySyncScenario.PrepareScenario != null)
			{
				MySyncScenario.PrepareScenario(PrepStartTime);
			}
			MySessionLoader.ScenarioWorldLoaded += MyGuiScreenLoadSandbox_ScenarioWorldLoaded;
		}

		private static void MyGuiScreenLoadSandbox_ScenarioWorldLoaded()
		{
			MySessionLoader.ScenarioWorldLoaded -= MyGuiScreenLoadSandbox_ScenarioWorldLoaded;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPlayerReadyToStartScenario, Sync.MyId);
			if (MySyncScenario.ClientWorldLoaded != null)
			{
				MySyncScenario.ClientWorldLoaded();
			}
		}

		[Event(null, 111)]
		[Reliable]
		[Server]
		private static void OnPlayerReadyToStartScenario(ulong playerSteamId)
		{
			if (MySyncScenario.PlayerReadyToStartScenario != null)
			{
				MySyncScenario.PlayerReadyToStartScenario(playerSteamId);
			}
		}

		internal static void StartScenarioRequest(ulong playerSteamId, long gameStartTime)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnStartScenario, gameStartTime, new EndpointId(playerSteamId));
		}

		[Event(null, 139)]
		[Reliable]
		[Client]
		private static void OnStartScenario(long gameStartTime)
		{
			if (MySyncScenario.StartScenario != null)
			{
				MySyncScenario.StartScenario(gameStartTime);
			}
		}

		public static void SetTimeout(int index)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnSetTimeoutBroadcast, index);
		}

		[Event(null, 152)]
		[Reliable]
		[Broadcast]
		private static void OnSetTimeoutBroadcast(int index)
		{
			OnSetTimeout(index);
		}

		private static void OnSetTimeout(int index)
		{
			if (MySyncScenario.TimeoutReceived != null)
			{
				MySyncScenario.TimeoutReceived(index);
			}
		}

		public static void SetJoinRunning(bool canJoin)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnSetJoinRunningBroadcast, canJoin);
		}

		[Event(null, 170)]
		[Reliable]
		[Broadcast]
		private static void OnSetJoinRunningBroadcast(bool canJoin)
		{
			OnSetJoinRunning(canJoin);
		}

		private static void OnSetJoinRunning(bool canJoin)
		{
			if (MySyncScenario.CanJoinRunningReceived != null)
			{
				MySyncScenario.CanJoinRunningReceived(canJoin);
			}
		}
	}
}
