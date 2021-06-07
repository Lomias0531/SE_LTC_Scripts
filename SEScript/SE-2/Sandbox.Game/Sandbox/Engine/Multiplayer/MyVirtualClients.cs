using Sandbox.Game;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
	[StaticEventOwner]
	internal class MyVirtualClients
	{
		protected sealed class OnVirtualClientAdded_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int index, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnVirtualClientAdded(index);
			}
		}

		private readonly List<MyVirtualClient> m_clients = new List<MyVirtualClient>();

		public void Init()
		{
			Sync.Players.NewPlayerRequestSucceeded += OnNewPlayerSuccess;
		}

		public void Add(int idx)
		{
			int num = m_clients.Count + 1;
			MyPlayer.PlayerId playerId = Sync.Players.FindFreePlayerId(Sync.MyId);
			MyPlayer.PlayerId playerId2 = new MyPlayer.PlayerId(playerId.SteamId, playerId.SerialId + idx);
			MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(Sync.MyId, 0));
			Sync.Players.RequestNewPlayer(playerId2.SerialId, "Virtual " + playerById.DisplayName + " #" + (num + idx), null, realPlayer: true, initialPlayer: false);
		}

		private void OnNewPlayerSuccess(MyPlayer.PlayerId playerId)
		{
			if (playerId.SteamId == Sync.MyId && playerId.SerialId != 0 && Sync.Players.GetPlayerById(playerId).IsRealPlayer)
			{
				MyPlayerCollection.RespawnRequest(joinGame: true, newIdentity: true, 0L, string.Empty, playerId.SerialId, null, Color.Red);
				int num = m_clients.Count + 1;
				MyVirtualClient item = new MyVirtualClient(new Endpoint(Sync.MyId, (byte)num), CreateClientState(), playerId);
				m_clients.Add(item);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => OnVirtualClientAdded, num);
			}
		}

		[Event(null, 46)]
		[Reliable]
		[Server]
		private static void OnVirtualClientAdded(int index)
		{
			EndpointId id = MyEventContext.Current.IsLocallyInvoked ? new EndpointId(Sync.MyId) : MyEventContext.Current.Sender;
			MyReplicationServer obj = MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
			MyClientState clientState = CreateClientState();
			Endpoint endpoint = new Endpoint(id, (byte)index);
			obj.AddClient(endpoint, clientState);
			ClientReadyDataMsg msg = new ClientReadyDataMsg
			{
				UsePlayoutDelayBufferForCharacter = true,
				UsePlayoutDelayBufferForJetpack = true,
				UsePlayoutDelayBufferForGrids = true
			};
			obj.OnClientReady(endpoint, ref msg);
		}

		private static MyClientState CreateClientState()
		{
			return Activator.CreateInstance(MyPerGameSettings.ClientStateType) as MyClientState;
		}

		public void Tick()
		{
			foreach (MyVirtualClient client in m_clients)
			{
				client.Tick();
			}
		}

		public bool Any()
		{
			return m_clients.Count > 0;
		}

		public MyPlayer GetNextControlledPlayer(MyPlayer controllingPlayer)
		{
			if (!Any())
			{
				return null;
			}
			for (int i = 0; i < m_clients.Count; i++)
			{
				MyVirtualClient myVirtualClient = m_clients[i];
				if (Sync.Players.GetPlayerById(myVirtualClient.PlayerId) == controllingPlayer)
				{
					if (i == m_clients.Count - 1)
					{
						return null;
					}
					return Sync.Players.GetPlayerById(m_clients[i + 1].PlayerId);
				}
			}
			return Sync.Players.GetPlayerById(m_clients[0].PlayerId);
		}
	}
}
