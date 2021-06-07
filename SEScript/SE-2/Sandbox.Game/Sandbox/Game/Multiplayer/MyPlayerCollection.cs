using ProtoBuf;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	public class MyPlayerCollection : MyIdentity.Friend, IMyPlayerCollection
	{
		[Serializable]
		public struct RespawnMsg
		{
			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003ERespawnMsg_003C_003EJoinGame_003C_003EAccessor : IMemberAccessor<RespawnMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnMsg owner, in bool value)
				{
					owner.JoinGame = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnMsg owner, out bool value)
				{
					value = owner.JoinGame;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003ERespawnMsg_003C_003ENewIdentity_003C_003EAccessor : IMemberAccessor<RespawnMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnMsg owner, in bool value)
				{
					owner.NewIdentity = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnMsg owner, out bool value)
				{
					value = owner.NewIdentity;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003ERespawnMsg_003C_003ERespawnEntityId_003C_003EAccessor : IMemberAccessor<RespawnMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnMsg owner, in long value)
				{
					owner.RespawnEntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnMsg owner, out long value)
				{
					value = owner.RespawnEntityId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003ERespawnMsg_003C_003ERespawnShipId_003C_003EAccessor : IMemberAccessor<RespawnMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnMsg owner, in string value)
				{
					owner.RespawnShipId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnMsg owner, out string value)
				{
					value = owner.RespawnShipId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003ERespawnMsg_003C_003EPlayerSerialId_003C_003EAccessor : IMemberAccessor<RespawnMsg, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnMsg owner, in int value)
				{
					owner.PlayerSerialId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnMsg owner, out int value)
				{
					value = owner.PlayerSerialId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003ERespawnMsg_003C_003EModelName_003C_003EAccessor : IMemberAccessor<RespawnMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnMsg owner, in string value)
				{
					owner.ModelName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnMsg owner, out string value)
				{
					value = owner.ModelName;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003ERespawnMsg_003C_003EColor_003C_003EAccessor : IMemberAccessor<RespawnMsg, Color>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref RespawnMsg owner, in Color value)
				{
					owner.Color = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref RespawnMsg owner, out Color value)
				{
					value = owner.Color;
				}
			}

			public bool JoinGame;

			public bool NewIdentity;

			public long RespawnEntityId;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string RespawnShipId;

			public int PlayerSerialId;

			[Serialize(MyObjectFlags.DefaultZero)]
			public string ModelName;

			public Color Color;
		}

		[ProtoContract]
		public struct AllPlayerData
		{
			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003EAllPlayerData_003C_003ESteamId_003C_003EAccessor : IMemberAccessor<AllPlayerData, ulong>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AllPlayerData owner, in ulong value)
				{
					owner.SteamId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AllPlayerData owner, out ulong value)
				{
					value = owner.SteamId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003EAllPlayerData_003C_003ESerialId_003C_003EAccessor : IMemberAccessor<AllPlayerData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AllPlayerData owner, in int value)
				{
					owner.SerialId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AllPlayerData owner, out int value)
				{
					value = owner.SerialId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003EAllPlayerData_003C_003EPlayer_003C_003EAccessor : IMemberAccessor<AllPlayerData, MyObjectBuilder_Player>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AllPlayerData owner, in MyObjectBuilder_Player value)
				{
					owner.Player = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AllPlayerData owner, out MyObjectBuilder_Player value)
				{
					value = owner.Player;
				}
			}

			private class Sandbox_Game_Multiplayer_MyPlayerCollection_003C_003EAllPlayerData_003C_003EActor : IActivator, IActivator<AllPlayerData>
			{
				private sealed override object CreateInstance()
				{
					return default(AllPlayerData);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override AllPlayerData CreateInstance()
				{
					return (AllPlayerData)(object)default(AllPlayerData);
				}

				AllPlayerData IActivator<AllPlayerData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public ulong SteamId;

			[ProtoMember(4)]
			public int SerialId;

			[ProtoMember(7)]
			public MyObjectBuilder_Player Player;
		}

		public delegate void RespawnRequestedDelegate(ref RespawnMsg respawnMsg, MyNetworkClient client);

		protected sealed class OnControlChangedSuccess_003C_003ESystem_UInt64_0023System_Int32_0023System_Int64_0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, long, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in long entityId, in bool justUpdateClientCache, in DBNull arg5, in DBNull arg6)
			{
				OnControlChangedSuccess(clientSteamId, playerSerialId, entityId, justUpdateClientCache);
			}
		}

		protected sealed class OnControlReleased_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnControlReleased(entityId);
			}
		}

		protected sealed class OnIdentityCreated_003C_003ESystem_Boolean_0023System_Int64_0023System_String : ICallSite<IMyEventOwner, bool, long, string, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool isNpc, in long identityId, in string displayName, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnIdentityCreated(isNpc, identityId, displayName);
			}
		}

		protected sealed class OnIdentityRemovedRequest_003C_003ESystem_Int64_0023System_UInt64_0023System_Int32 : ICallSite<IMyEventOwner, long, ulong, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in ulong steamId, in int serialId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnIdentityRemovedRequest(identityId, steamId, serialId);
			}
		}

		protected sealed class OnIdentityRemovedSuccess_003C_003ESystem_Int64_0023System_UInt64_0023System_Int32 : ICallSite<IMyEventOwner, long, ulong, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in ulong steamId, in int serialId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnIdentityRemovedSuccess(identityId, steamId, serialId);
			}
		}

		protected sealed class OnPlayerIdentityChanged_003C_003ESystem_UInt64_0023System_Int32_0023System_Int64 : ICallSite<IMyEventOwner, ulong, int, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in long identityId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPlayerIdentityChanged(clientSteamId, playerSerialId, identityId);
			}
		}

		protected sealed class OnRespawnRequestFailure_003C_003ESystem_Int32 : ICallSite<IMyEventOwner, int, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int playerSerialId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRespawnRequestFailure(playerSerialId);
			}
		}

		protected sealed class OnSetPlayerDeadRequest_003C_003ESystem_UInt64_0023System_Int32_0023System_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, bool, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in bool isDead, in bool resetIdentity, in DBNull arg5, in DBNull arg6)
			{
				OnSetPlayerDeadRequest(clientSteamId, playerSerialId, isDead, resetIdentity);
			}
		}

		protected sealed class OnSetPlayerDeadSuccess_003C_003ESystem_UInt64_0023System_Int32_0023System_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, bool, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in bool isDead, in bool resetIdentity, in DBNull arg5, in DBNull arg6)
			{
				OnSetPlayerDeadSuccess(clientSteamId, playerSerialId, isDead, resetIdentity);
			}
		}

		protected sealed class OnNewPlayerRequest_003C_003ESystem_UInt64_0023System_Int32_0023System_String_0023System_String_0023System_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, string, string, bool, bool>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in string displayName, in string characterModel, in bool realPlayer, in bool initialPlayer)
			{
				OnNewPlayerRequest(clientSteamId, playerSerialId, displayName, characterModel, realPlayer, initialPlayer);
			}
		}

		protected sealed class OnNewPlayerSuccess_003C_003ESystem_UInt64_0023System_Int32 : ICallSite<IMyEventOwner, ulong, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnNewPlayerSuccess(clientSteamId, playerSerialId);
			}
		}

		protected sealed class OnNewPlayerFailure_003C_003ESystem_UInt64_0023System_Int32 : ICallSite<IMyEventOwner, ulong, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnNewPlayerFailure(clientSteamId, playerSerialId);
			}
		}

		protected sealed class OnPlayerCreated_003C_003ESystem_UInt64_0023System_Int32_0023System_Int64_0023System_String_0023System_Collections_Generic_List_00601_003CVRageMath_Vector3_003E_0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, long, string, List<Vector3>, bool>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in long identityId, in string displayName, in List<Vector3> buildColors, in bool realPlayer)
			{
				OnPlayerCreated(clientSteamId, playerSerialId, identityId, displayName, buildColors, realPlayer);
			}
		}

		protected sealed class OnPlayerRemoveRequest_003C_003ESystem_UInt64_0023System_Int32_0023System_Boolean : ICallSite<IMyEventOwner, ulong, int, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in bool removeCharacter, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPlayerRemoveRequest(clientSteamId, playerSerialId, removeCharacter);
			}
		}

		protected sealed class OnPlayerRemoved_003C_003ESystem_UInt64_0023System_Int32 : ICallSite<IMyEventOwner, ulong, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong clientSteamId, in int playerSerialId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPlayerRemoved(clientSteamId, playerSerialId);
			}
		}

		protected sealed class OnPlayerColorChangedRequest_003C_003ESystem_Int32_0023System_Int32_0023VRageMath_Vector3 : ICallSite<IMyEventOwner, int, int, Vector3, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int serialId, in int colorIndex, in Vector3 newColor, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPlayerColorChangedRequest(serialId, colorIndex, newColor);
			}
		}

		protected sealed class OnPlayerColorsChangedRequest_003C_003ESystem_Int32_0023System_Collections_Generic_List_00601_003CVRageMath_Vector3_003E : ICallSite<IMyEventOwner, int, List<Vector3>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int serialId, in List<Vector3> newColors, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnPlayerColorsChangedRequest(serialId, newColors);
			}
		}

		protected sealed class OnNpcIdentityRequest_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnNpcIdentityRequest();
			}
		}

		protected sealed class OnNpcIdentitySuccess_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identidyId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnNpcIdentitySuccess(identidyId);
			}
		}

		protected sealed class OnIdentityFirstSpawn_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identidyId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnIdentityFirstSpawn(identidyId);
			}
		}

		protected sealed class SetIdentityBlockTypesBuilt_003C_003ESandbox_Game_World_MyBlockLimits_003C_003EMyTypeLimitData : ICallSite<IMyEventOwner, MyBlockLimits.MyTypeLimitData, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyBlockLimits.MyTypeLimitData limits, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetIdentityBlockTypesBuilt(limits);
			}
		}

		protected sealed class SetIdentityGridBlocksBuilt_003C_003ESandbox_Game_World_MyBlockLimits_003C_003EMyGridLimitData_0023System_Int32_0023System_Int32_0023System_Int32_0023System_Int32 : ICallSite<IMyEventOwner, MyBlockLimits.MyGridLimitData, int, int, int, int, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyBlockLimits.MyGridLimitData limits, in int pcu, in int pcuBuilt, in int blocksBuilt, in int transferedDelta, in DBNull arg6)
			{
				SetIdentityGridBlocksBuilt(limits, pcu, pcuBuilt, blocksBuilt, transferedDelta);
			}
		}

		protected sealed class SetPCU_Client_003C_003ESystem_Int32_0023System_Int32 : ICallSite<IMyEventOwner, int, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in int pcu, in int transferedDelta, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetPCU_Client(pcu, transferedDelta);
			}
		}

		protected sealed class SetDampeningEntity_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long controlledEntityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetDampeningEntity(controlledEntityId);
			}
		}

		protected sealed class ClearDampeningEntity_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long controlledEntityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ClearDampeningEntity(controlledEntityId);
			}
		}

		protected sealed class SetDampeningEntityClient_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long controlledEntityId, in long dampeningEntityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetDampeningEntityClient(controlledEntityId, dampeningEntityId);
			}
		}

		protected sealed class OnRespawnRequest_003C_003ESandbox_Game_Multiplayer_MyPlayerCollection_003C_003ERespawnMsg : ICallSite<IMyEventOwner, RespawnMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in RespawnMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnRespawnRequest(msg);
			}
		}

		private readonly ConcurrentDictionary<MyPlayer.PlayerId, MyPlayer> m_players = new ConcurrentDictionary<MyPlayer.PlayerId, MyPlayer>(MyPlayer.PlayerId.Comparer);

		private List<MyPlayer> m_tmpRemovedPlayers = new List<MyPlayer>();

		private CachingDictionary<long, MyPlayer.PlayerId> m_controlledEntities = new CachingDictionary<long, MyPlayer.PlayerId>();

		private Dictionary<long, MyPlayer.PlayerId> m_previousControlledEntities = new Dictionary<long, MyPlayer.PlayerId>();

		private ConcurrentDictionary<long, MyIdentity> m_allIdentities = new ConcurrentDictionary<long, MyIdentity>();

		private readonly ConcurrentDictionary<MyPlayer.PlayerId, long> m_playerIdentityIds = new ConcurrentDictionary<MyPlayer.PlayerId, long>(MyPlayer.PlayerId.Comparer);

		private readonly Dictionary<long, MyPlayer.PlayerId> m_identityPlayerIds = new Dictionary<long, MyPlayer.PlayerId>();

		private HashSet<long> m_npcIdentities = new HashSet<long>();

		private List<EndpointId> m_tmpPlayersLinkedToBlockLimit = new List<EndpointId>();

		private Action<MyEntity> m_entityClosingHandler;

		private static Dictionary<long, MyPlayer.PlayerId> m_controlledEntitiesClientCache;

		public MyRespawnComponentBase RespawnComponent
		{
			get;
			set;
		}

		public DictionaryReader<long, MyPlayer.PlayerId> ControlledEntities => m_controlledEntities.Reader;

		long IMyPlayerCollection.Count => m_players.Count;

		public static event Action<ulong> OnRespawnRequestFailureEvent;

		public event Action<MyPlayer.PlayerId> NewPlayerRequestSucceeded;

		public event Action<int> NewPlayerRequestFailed;

		public event Action<int> LocalPlayerRemoved;

		public event Action<int> LocalPlayerLoaded;

		public event Action<string, Color> LocalRespawnRequested;

		public event Action<MyPlayer.PlayerId> PlayerRemoved;

		public event PlayerRequestDelegate PlayerRequesting;

		public event Action<bool, MyPlayer.PlayerId> PlayersChanged;

		public event Action<long> PlayerCharacterDied;

		public event Action IdentitiesChanged;

		public MyPlayerCollection()
		{
			m_entityClosingHandler = EntityClosing;
			m_controlledEntitiesClientCache = ((!Sync.IsServer) ? new Dictionary<long, MyPlayer.PlayerId>() : null);
		}

		public void LoadIdentities(MyObjectBuilder_Checkpoint checkpoint, MyPlayer.PlayerId? savingPlayerId = null)
		{
			if (checkpoint.NonPlayerIdentities != null)
			{
				LoadNpcIdentities(checkpoint.NonPlayerIdentities);
			}
			if (checkpoint.AllPlayers.Count != 0)
			{
				LoadIdentitiesObsolete(checkpoint.AllPlayers, savingPlayerId);
			}
			else
			{
				LoadIdentities(checkpoint.Identities);
			}
		}

		private void LoadNpcIdentities(List<long> list)
		{
			foreach (long item in list)
			{
				MarkIdentityAsNPC(item);
			}
		}

		public List<MyObjectBuilder_Identity> SaveIdentities()
		{
			List<MyObjectBuilder_Identity> list = new List<MyObjectBuilder_Identity>();
			foreach (KeyValuePair<long, MyIdentity> allIdentity in m_allIdentities)
			{
				if (MySession.Static != null && MySession.Static.Players.TryGetPlayerId(allIdentity.Key, out MyPlayer.PlayerId result) && MySession.Static.Players.GetPlayerById(result) != null)
				{
					allIdentity.Value.LastLogoutTime = DateTime.Now;
				}
				list.Add(allIdentity.Value.GetObjectBuilder());
			}
			return list;
		}

		public List<long> SaveNpcIdentities()
		{
			List<long> list = new List<long>();
			foreach (long npcIdentity in m_npcIdentities)
			{
				list.Add(npcIdentity);
			}
			return list;
		}

		public void LoadControlledEntities(SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> controlledEntities, long controlledObject, MyPlayer.PlayerId? savingPlayerId = null)
		{
			if (controlledEntities != null)
			{
				foreach (KeyValuePair<long, MyObjectBuilder_Checkpoint.PlayerId> item in controlledEntities.Dictionary)
				{
					MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(item.Value.ClientId, item.Value.SerialId);
					if (savingPlayerId.HasValue && playerId.SteamId == savingPlayerId.Value.SteamId)
					{
						playerId = new MyPlayer.PlayerId(Sync.MyId, playerId.SerialId);
					}
					MyPlayer playerById = Sync.Players.GetPlayerById(playerId);
					if (!Sync.IsServer)
					{
						m_controlledEntitiesClientCache[item.Key] = playerId;
					}
					if (playerById != null)
					{
						TryTakeControl(playerById, item.Key);
					}
				}
			}
		}

		private void TryTakeControl(MyPlayer player, long controlledEntityId)
		{
			MyEntities.TryGetEntityById(controlledEntityId, out MyEntity entity);
			if (entity != null)
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated && entity is Sandbox.Game.Entities.IMyControllableEntity)
				{
					player.Controller.TakeControl(entity as Sandbox.Game.Entities.IMyControllableEntity);
					MyCharacter myCharacter = entity as MyCharacter;
					if (myCharacter == null)
					{
						if (entity is MyShipController)
						{
							myCharacter = (entity as MyShipController).Pilot;
						}
						else if (entity is MyLargeTurretBase)
						{
							myCharacter = (entity as MyLargeTurretBase).Pilot;
						}
					}
					if (myCharacter != null)
					{
						player.Identity.ChangeCharacter(myCharacter);
						myCharacter.SetPlayer(player, update: false);
					}
				}
				else
				{
					m_controlledEntities.Add(controlledEntityId, player.Id, immediate: true);
					if (Sync.IsServer)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnControlChangedSuccess, player.Id.SteamId, player.Id.SerialId, controlledEntityId, arg5: true);
					}
				}
				if (player.CachedControllerId != null)
				{
					player.CachedControllerId.Remove(controlledEntityId);
					if (player.CachedControllerId.Count == 0)
					{
						player.CachedControllerId = null;
					}
				}
			}
			else
			{
				if (player.CachedControllerId == null)
				{
					player.CachedControllerId = new List<long>();
				}
				player.CachedControllerId.Add(controlledEntityId);
			}
		}

		public SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> SerializeControlledEntities()
		{
			SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId> serializableDictionary = new SerializableDictionary<long, MyObjectBuilder_Checkpoint.PlayerId>();
			foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity in m_controlledEntities)
			{
				MyObjectBuilder_Checkpoint.PlayerId value = default(MyObjectBuilder_Checkpoint.PlayerId);
				value.ClientId = controlledEntity.Value.SteamId;
				value.SerialId = controlledEntity.Value.SerialId;
				serializableDictionary.Dictionary.Add(controlledEntity.Key, value);
			}
			return serializableDictionary;
		}

		private void ChangeDisplayNameOfPlayerAndIdentity(MyObjectBuilder_Player playerOb, string name)
		{
			playerOb.DisplayName = MyGameService.UserName;
			TryGetIdentity(playerOb.IdentityId)?.SetDisplayName(MyGameService.UserName);
		}

		public void LoadPlayers(List<AllPlayerData> allPlayersData)
		{
			if (allPlayersData != null)
			{
				foreach (AllPlayerData allPlayersDatum in allPlayersData)
				{
					MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(allPlayersDatum.SteamId, allPlayersDatum.SerialId);
					LoadPlayerInternal(ref playerId, allPlayersDatum.Player);
				}
			}
		}

		public void LoadConnectedPlayers(MyObjectBuilder_Checkpoint checkpoint, MyPlayer.PlayerId? savingPlayerId = null)
		{
			if (checkpoint.AllPlayers != null && checkpoint.AllPlayers.Count != 0)
			{
				foreach (MyObjectBuilder_Checkpoint.PlayerItem allPlayer in checkpoint.AllPlayers)
				{
					long playerId = allPlayer.PlayerId;
					MyObjectBuilder_Player playerOb = new MyObjectBuilder_Player
					{
						Connected = true,
						DisplayName = allPlayer.Name,
						IdentityId = playerId
					};
					MyPlayer.PlayerId playerId2 = new MyPlayer.PlayerId(allPlayer.SteamId, 0);
					if (savingPlayerId.HasValue)
					{
						MyPlayer.PlayerId value = playerId2;
						MyPlayer.PlayerId? b = savingPlayerId;
						if (value == b)
						{
							playerId2 = new MyPlayer.PlayerId(Sync.MyId);
							ChangeDisplayNameOfPlayerAndIdentity(playerOb, MyGameService.UserName);
						}
					}
					LoadPlayerInternal(ref playerId2, playerOb, obsolete: true);
				}
			}
			else if (checkpoint.ConnectedPlayers != null && checkpoint.ConnectedPlayers.Dictionary.Count != 0)
			{
				foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> item in checkpoint.ConnectedPlayers.Dictionary)
				{
					MyPlayer.PlayerId playerId3 = new MyPlayer.PlayerId(item.Key.ClientId, item.Key.SerialId);
					if (savingPlayerId.HasValue)
					{
						MyPlayer.PlayerId value = playerId3;
						MyPlayer.PlayerId? b = savingPlayerId;
						if (value == b)
						{
							playerId3 = new MyPlayer.PlayerId(Sync.MyId);
							ChangeDisplayNameOfPlayerAndIdentity(item.Value, MyGameService.UserName);
						}
					}
					item.Value.Connected = true;
					LoadPlayerInternal(ref playerId3, item.Value);
				}
				foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, long> item2 in checkpoint.DisconnectedPlayers.Dictionary)
				{
					MyPlayer.PlayerId playerId4 = new MyPlayer.PlayerId(item2.Key.ClientId, item2.Key.SerialId);
					MyObjectBuilder_Player playerOb2 = new MyObjectBuilder_Player
					{
						Connected = false,
						IdentityId = item2.Value,
						DisplayName = null
					};
					if (savingPlayerId.HasValue)
					{
						MyPlayer.PlayerId value = playerId4;
						MyPlayer.PlayerId? b = savingPlayerId;
						if (value == b)
						{
							playerId4 = new MyPlayer.PlayerId(Sync.MyId);
							ChangeDisplayNameOfPlayerAndIdentity(playerOb2, MyGameService.UserName);
						}
					}
					LoadPlayerInternal(ref playerId4, playerOb2);
				}
			}
			else if (checkpoint.AllPlayersData != null)
			{
				foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> item3 in checkpoint.AllPlayersData.Dictionary)
				{
					MyPlayer.PlayerId playerId5 = new MyPlayer.PlayerId(item3.Key.ClientId, item3.Key.SerialId);
					if (savingPlayerId.HasValue && playerId5.SteamId == savingPlayerId.Value.SteamId)
					{
						playerId5 = new MyPlayer.PlayerId(Sync.MyId, playerId5.SerialId);
						if (playerId5.SerialId == 0)
						{
							ChangeDisplayNameOfPlayerAndIdentity(item3.Value, MyGameService.UserName);
						}
					}
					LoadPlayerInternal(ref playerId5, item3.Value);
					MyPlayer value2 = null;
					if (m_players.TryGetValue(playerId5, out value2))
					{
						List<Vector3> value3 = null;
						if (checkpoint.AllPlayersColors != null && checkpoint.AllPlayersColors.Dictionary.TryGetValue(item3.Key, out value3))
						{
							value2.SetBuildColorSlots(value3);
						}
						else if (checkpoint.CharacterToolbar != null && checkpoint.CharacterToolbar.ColorMaskHSVList != null && checkpoint.CharacterToolbar.ColorMaskHSVList.Count > 0)
						{
							value2.SetBuildColorSlots(checkpoint.CharacterToolbar.ColorMaskHSVList);
						}
					}
				}
			}
			if (MyCubeBuilder.AllPlayersColors != null && checkpoint.AllPlayersColors != null)
			{
				foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, List<Vector3>> item4 in checkpoint.AllPlayersColors.Dictionary)
				{
					MyPlayer.PlayerId key = new MyPlayer.PlayerId(item4.Key.ClientId, item4.Key.SerialId);
					if (!MyCubeBuilder.AllPlayersColors.ContainsKey(key))
					{
						MyCubeBuilder.AllPlayersColors.Add(key, item4.Value);
					}
				}
			}
		}

		public void LoadDisconnectedPlayers(Dictionary<MyObjectBuilder_Checkpoint.PlayerId, long> dictionary)
		{
			foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, long> item in dictionary)
			{
				MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(item.Key.ClientId, item.Key.SerialId);
				m_playerIdentityIds.TryAdd(playerId, item.Value);
				m_identityPlayerIds.Add(item.Value, playerId);
			}
		}

		public void SavePlayers(MyObjectBuilder_Checkpoint checkpoint)
		{
			checkpoint.ConnectedPlayers = new SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player>();
			checkpoint.DisconnectedPlayers = new SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, long>();
			checkpoint.AllPlayersData = new SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player>();
			checkpoint.AllPlayersColors = new SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, List<Vector3>>();
			MyObjectBuilder_Checkpoint.PlayerId playerId;
			foreach (MyPlayer value in m_players.Values)
			{
				playerId = default(MyObjectBuilder_Checkpoint.PlayerId);
				playerId.ClientId = value.Id.SteamId;
				playerId.SerialId = value.Id.SerialId;
				MyObjectBuilder_Checkpoint.PlayerId key = playerId;
				MyObjectBuilder_Player objectBuilder = value.GetObjectBuilder();
				checkpoint.AllPlayersData.Dictionary.Add(key, objectBuilder);
			}
			foreach (KeyValuePair<MyPlayer.PlayerId, long> playerIdentityId in m_playerIdentityIds)
			{
				if (!m_players.ContainsKey(playerIdentityId.Key))
				{
					playerId = default(MyObjectBuilder_Checkpoint.PlayerId);
					playerId.ClientId = playerIdentityId.Key.SteamId;
					playerId.SerialId = playerIdentityId.Key.SerialId;
					MyObjectBuilder_Checkpoint.PlayerId key2 = playerId;
					MyIdentity myIdentity = TryGetIdentity(playerIdentityId.Value);
					MyObjectBuilder_Player myObjectBuilder_Player = new MyObjectBuilder_Player
					{
						DisplayName = myIdentity?.DisplayName,
						IdentityId = playerIdentityId.Value,
						Connected = false
					};
					if (MyCubeBuilder.AllPlayersColors != null)
					{
						MyCubeBuilder.AllPlayersColors.TryGetValue(playerIdentityId.Key, out myObjectBuilder_Player.BuildColorSlots);
					}
					checkpoint.AllPlayersData.Dictionary.Add(key2, myObjectBuilder_Player);
				}
			}
			if (MyCubeBuilder.AllPlayersColors != null)
			{
				foreach (KeyValuePair<MyPlayer.PlayerId, List<Vector3>> allPlayersColor in MyCubeBuilder.AllPlayersColors)
				{
					if (!m_players.ContainsKey(allPlayersColor.Key) && !m_playerIdentityIds.ContainsKey(allPlayersColor.Key))
					{
						playerId = default(MyObjectBuilder_Checkpoint.PlayerId);
						playerId.ClientId = allPlayersColor.Key.SteamId;
						playerId.SerialId = allPlayersColor.Key.SerialId;
						MyObjectBuilder_Checkpoint.PlayerId key3 = playerId;
						checkpoint.AllPlayersColors.Dictionary.Add(key3, allPlayersColor.Value);
					}
				}
			}
		}

		public List<AllPlayerData> SavePlayers()
		{
			List<AllPlayerData> list = new List<AllPlayerData>();
			foreach (MyPlayer value in m_players.Values)
			{
				AllPlayerData allPlayerData = default(AllPlayerData);
				allPlayerData.SteamId = value.Id.SteamId;
				allPlayerData.SerialId = value.Id.SerialId;
				allPlayerData.Player = value.GetObjectBuilder();
				AllPlayerData item = allPlayerData;
				list.Add(item);
			}
			return list;
		}

		private void RemovePlayerFromDictionary(MyPlayer.PlayerId playerId)
		{
			if (m_players.ContainsKey(playerId))
			{
				if (Sync.IsServer && MyVisualScriptLogicProvider.PlayerDisconnected != null)
				{
					MyVisualScriptLogicProvider.PlayerDisconnected(m_players[playerId].Identity.IdentityId);
				}
				m_players[playerId].Identity.LastLogoutTime = DateTime.Now;
			}
			m_players.Remove(playerId);
			OnPlayersChanged(added: false, playerId);
		}

		private void AddPlayer(MyPlayer.PlayerId playerId, MyPlayer newPlayer)
		{
			if (Sync.IsServer && MyVisualScriptLogicProvider.PlayerConnected != null)
			{
				MyVisualScriptLogicProvider.PlayerConnected(newPlayer.Identity.IdentityId);
			}
			newPlayer.Identity.LastLoginTime = DateTime.Now;
			newPlayer.Identity.BlockLimits.SetAllDirty();
			m_players.TryAdd(playerId, newPlayer);
			OnPlayersChanged(added: true, playerId);
		}

		public void RegisterEvents()
		{
			MyClientCollection clients = Sync.Clients;
			clients.ClientRemoved = (Action<ulong>)Delegate.Combine(clients.ClientRemoved, new Action<ulong>(Multiplayer_ClientRemoved));
		}

		public void UnregisterEvents()
		{
			if (Sync.Clients != null)
			{
				MyClientCollection clients = Sync.Clients;
				clients.ClientRemoved = (Action<ulong>)Delegate.Remove(clients.ClientRemoved, new Action<ulong>(Multiplayer_ClientRemoved));
			}
		}

		private void OnPlayersChanged(bool added, MyPlayer.PlayerId playerId)
		{
			this.PlayersChanged?.Invoke(added, playerId);
		}

		public void ClearPlayers()
		{
			m_players.Clear();
			m_controlledEntities.Clear();
			m_playerIdentityIds.Clear();
			m_identityPlayerIds.Clear();
		}

		[Event(null, 572)]
		[Reliable]
		[Broadcast]
		private static void OnControlChangedSuccess(ulong clientSteamId, int playerSerialId, long entityId, bool justUpdateClientCache)
		{
			MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
			MyEntity entity = null;
			if (m_controlledEntitiesClientCache != null)
			{
				if (playerId.IsValid)
				{
					m_controlledEntitiesClientCache[entityId] = playerId;
				}
				else if (m_controlledEntitiesClientCache.ContainsKey(entityId))
				{
					m_controlledEntitiesClientCache.Remove(entityId);
				}
			}
			if (!justUpdateClientCache && MyEntities.TryGetEntityById(entityId, out entity))
			{
				Sync.Players.SetControlledEntityInternal(playerId, entity);
			}
		}

		public static void UpdateControl(MyEntity entity)
		{
			if (m_controlledEntitiesClientCache != null && m_controlledEntitiesClientCache.TryGetValue(entity.EntityId, out MyPlayer.PlayerId value) && (!Sync.Players.m_controlledEntities.TryGetValue(entity.EntityId, out MyPlayer.PlayerId value2) || !(value2 == value)))
			{
				Sync.Players.SetControlledEntityInternal(value, entity);
			}
		}

		[Event(null, 621)]
		[Reliable]
		[Server]
		[Broadcast]
		private static void OnControlReleased(long entityId)
		{
			if (!Sync.IsServer && m_controlledEntitiesClientCache != null && m_controlledEntitiesClientCache.ContainsKey(entityId))
			{
				m_controlledEntitiesClientCache.Remove(entityId);
			}
			if (!MyEventContext.Current.IsLocallyInvoked)
			{
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(entityId, out entity))
				{
					Sync.Players.RemoveControlledEntityInternal(entity);
				}
			}
		}

		[Event(null, 639)]
		[Reliable]
		[Broadcast]
		private static void OnIdentityCreated(bool isNpc, long identityId, string displayName)
		{
			if (isNpc)
			{
				Sync.Players.CreateNewNpcIdentity(displayName, identityId);
			}
			else
			{
				Sync.Players.CreateNewIdentity(displayName, identityId, null, null);
			}
		}

		[Event(null, 652)]
		[Reliable]
		[Server]
		private static void OnIdentityRemovedRequest(long identityId, ulong steamId, int serialId)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && steamId != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (Sync.Players.RemoveIdentityInternal(identityId, new MyPlayer.PlayerId(steamId, serialId)))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnIdentityRemovedSuccess, identityId, steamId, serialId);
			}
		}

		[Event(null, 669)]
		[Reliable]
		[Broadcast]
		private static void OnIdentityRemovedSuccess(long identityId, ulong steamId, int serialId)
		{
			Sync.Players.RemoveIdentityInternal(identityId, new MyPlayer.PlayerId(steamId, serialId));
		}

		[Event(null, 675)]
		[Reliable]
		[Broadcast]
		private static void OnPlayerIdentityChanged(ulong clientSteamId, int playerSerialId, long identityId)
		{
			MyPlayer.PlayerId id = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
			MyPlayer playerById = Sync.Players.GetPlayerById(id);
			if (playerById != null)
			{
				MyIdentity value = null;
				Sync.Players.m_allIdentities.TryGetValue(identityId, out value);
				if (value != null)
				{
					playerById.Identity = value;
				}
			}
		}

		[Event(null, 692)]
		[Reliable]
		[Client]
		private static void OnRespawnRequestFailure(int playerSerialId)
		{
			if (playerSerialId == 0)
			{
				MyPlayerCollection.OnRespawnRequestFailureEvent.InvokeIfNotNull(Sync.MyId);
				RequestLocalRespawn();
			}
		}

		[Event(null, 702)]
		[Reliable]
		[Server]
		private static void OnSetPlayerDeadRequest(ulong clientSteamId, int playerSerialId, bool isDead, bool resetIdentity)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && clientSteamId != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (Sync.Players.SetPlayerDeadInternal(clientSteamId, playerSerialId, isDead, resetIdentity))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnSetPlayerDeadSuccess, clientSteamId, playerSerialId, isDead, resetIdentity);
			}
		}

		[Event(null, 719)]
		[Reliable]
		[Broadcast]
		private static void OnSetPlayerDeadSuccess(ulong clientSteamId, int playerSerialId, bool isDead, bool resetIdentity)
		{
			Sync.Players.SetPlayerDeadInternal(clientSteamId, playerSerialId, isDead, resetIdentity);
		}

		[Event(null, 725)]
		[Reliable]
		[Server]
		private static void OnNewPlayerRequest(ulong clientSteamId, int playerSerialId, string displayName, [Serialize(MyObjectFlags.DefaultZero)] string characterModel, bool realPlayer = false, bool initialPlayer = false)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && clientSteamId != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
			if (Sync.Players.m_players.ContainsKey(playerId))
			{
				return;
			}
			if (Sync.Players.PlayerRequesting != null)
			{
				PlayerRequestArgs playerRequestArgs = new PlayerRequestArgs(playerId);
				Sync.Players.PlayerRequesting(playerRequestArgs);
				if (playerRequestArgs.Cancel)
				{
					if (MyEventContext.Current.IsLocallyInvoked)
					{
						OnNewPlayerFailure(clientSteamId, playerSerialId);
					}
					else
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnNewPlayerFailure, clientSteamId, playerSerialId, MyEventContext.Current.Sender);
					}
					return;
				}
			}
			MyIdentity myIdentity = Sync.Players.TryGetPlayerIdentity(playerId);
			bool flag = myIdentity == null;
			if (flag)
			{
				myIdentity = Sync.Players.RespawnComponent.CreateNewIdentity(displayName, playerId, characterModel, initialPlayer);
			}
			Sync.Players.CreateNewPlayer(myIdentity, playerId, myIdentity.DisplayName, realPlayer, initialPlayer, flag);
			MySession.Static.Factions.CompatDefaultFactions(playerId);
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				OnNewPlayerSuccess(clientSteamId, playerSerialId);
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnNewPlayerSuccess, clientSteamId, playerSerialId, MyEventContext.Current.Sender);
			}
			if (!MyBankingSystem.Static.TryGetAccountInfo(myIdentity.IdentityId, out MyAccountInfo _))
			{
				MyBankingSystem.Static.CreateAccount(myIdentity.IdentityId);
			}
		}

		[Event(null, 773)]
		[Reliable]
		[Client]
		private static void OnNewPlayerSuccess(ulong clientSteamId, int playerSerialId)
		{
			MyPlayer.PlayerId b = new MyPlayer.PlayerId(Sync.MyId, 0);
			MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
			if (playerId == b && (!MySession.Static.IsScenario || MySession.Static.OnlineMode == MyOnlineModeEnum.OFFLINE))
			{
				RequestLocalRespawn();
			}
			Sync.Players.NewPlayerRequestSucceeded?.Invoke(playerId);
		}

		[Event(null, 789)]
		[Reliable]
		[Client]
		private static void OnNewPlayerFailure(ulong clientSteamId, int playerSerialId)
		{
			if (clientSteamId == Sync.MyId)
			{
				MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
				if (Sync.Players.NewPlayerRequestFailed != null)
				{
					Sync.Players.NewPlayerRequestFailed(playerId.SerialId);
				}
			}
		}

		public void OnInitialPlayerCreated(ulong clientSteamId, int playerSerialId, long identityId, string displayName, List<Vector3> buildColors, bool realPlayer, bool newIdentity)
		{
			if (newIdentity)
			{
				OnIdentityCreated(isNpc: false, identityId, displayName);
			}
			OnPlayerCreated(clientSteamId, playerSerialId, identityId, displayName, buildColors, realPlayer);
			if (clientSteamId == Sync.MyId)
			{
				MyMultiplayer.Static.StartProcessingClientMessages();
			}
		}

		[Event(null, 817)]
		[Reliable]
		[Broadcast]
		private static void OnPlayerCreated(ulong clientSteamId, int playerSerialId, long identityId, string displayName, [Serialize(MyObjectFlags.DefaultZero)] List<Vector3> buildColors, bool realPlayer)
		{
			if (Sync.Players.TryGetIdentity(identityId) != null)
			{
				MyNetworkClient client = null;
				Sync.Clients.TryGetClient(clientSteamId, out client);
				if (client != null)
				{
					MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
					MyObjectBuilder_Player playerBuilder = new MyObjectBuilder_Player
					{
						DisplayName = displayName,
						IdentityId = identityId,
						BuildColorSlots = buildColors,
						ForceRealPlayer = realPlayer
					};
					Sync.Players.CreateNewPlayerInternal(client, playerId, playerBuilder);
				}
			}
		}

		[Event(null, 842)]
		[Reliable]
		[Server]
		private static void OnPlayerRemoveRequest(ulong clientSteamId, int playerSerialId, bool removeCharacter)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && clientSteamId != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyPlayer playerById = Sync.Players.GetPlayerById(new MyPlayer.PlayerId(clientSteamId, playerSerialId));
			if (playerById != null)
			{
				Sync.Players.RemovePlayer(playerById, removeCharacter);
			}
		}

		[Event(null, 862)]
		[Reliable]
		[Broadcast]
		private static void OnPlayerRemoved(ulong clientSteamId, int playerSerialId)
		{
			MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(clientSteamId, playerSerialId);
			if (clientSteamId == Sync.MyId)
			{
				Sync.Players.RaiseLocalPlayerRemoved(playerSerialId);
			}
			Sync.Players.RemovePlayerFromDictionary(playerId);
		}

		[Event(null, 874)]
		[Reliable]
		[Server]
		private static void OnPlayerColorChangedRequest(int serialId, int colorIndex, Vector3 newColor)
		{
			ulong steamId = (!MyEventContext.Current.IsLocallyInvoked) ? MyEventContext.Current.Sender.Value : Sync.MyId;
			MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(steamId, serialId);
			MyPlayer playerById = Sync.Players.GetPlayerById(playerId);
			if (playerById == null)
			{
				if (MyCubeBuilder.AllPlayersColors.TryGetValue(playerId, out List<Vector3> value))
				{
					value[colorIndex] = newColor;
				}
			}
			else
			{
				playerById.SelectedBuildColorSlot = colorIndex;
				playerById.ChangeOrSwitchToColor(newColor);
			}
		}

		[Event(null, 899)]
		[Reliable]
		[Server]
		private static void OnPlayerColorsChangedRequest(int serialId, [Serialize(MyObjectFlags.DefaultZero)] List<Vector3> newColors)
		{
			ulong steamId = (!MyEventContext.Current.IsLocallyInvoked) ? MyEventContext.Current.Sender.Value : Sync.MyId;
			MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(steamId, serialId);
			MyPlayer playerById = Sync.Players.GetPlayerById(playerId);
			if (playerById == null)
			{
				if (MyCubeBuilder.AllPlayersColors.TryGetValue(playerId, out List<Vector3> value))
				{
					value.Clear();
					foreach (Vector3 newColor in newColors)
					{
						value.Add(newColor);
					}
				}
			}
			else
			{
				playerById.SetBuildColorSlots(newColors);
			}
		}

		[Event(null, 928)]
		[Reliable]
		[Server]
		private static void OnNpcIdentityRequest()
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			string name = "NPC " + MyRandom.Instance.Next(1000, 9999);
			MyIdentity myIdentity = Sync.Players.CreateNewNpcIdentity(name, 0L);
			if (myIdentity != null)
			{
				long identityId = myIdentity.IdentityId;
				if (MyEventContext.Current.IsLocallyInvoked)
				{
					OnNpcIdentitySuccess(identityId);
				}
				else
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnNpcIdentitySuccess, identityId, MyEventContext.Current.Sender);
				}
			}
		}

		[Event(null, 951)]
		[Reliable]
		[Client]
		private static void OnNpcIdentitySuccess(long identidyId)
		{
			MyIdentity myIdentity = Sync.Players.TryGetIdentity(identidyId);
			if (myIdentity != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionInfo), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.NPCIdentityAdded), myIdentity.DisplayName)));
			}
		}

		[Event(null, 964)]
		[Reliable]
		[Broadcast]
		private static void OnIdentityFirstSpawn(long identidyId)
		{
			Sync.Players.TryGetIdentity(identidyId)?.PerformFirstSpawn();
		}

		[Event(null, 974)]
		[Reliable]
		[Client]
		private static void SetIdentityBlockTypesBuilt(MyBlockLimits.MyTypeLimitData limits)
		{
			if (MySession.Static == null || MySession.Static.LocalHumanPlayer == null)
			{
				MyLog.Default.Error("Missing session or local player in SetIdentityBlockTypesBuilt");
				return;
			}
			MyIdentity identity = MySession.Static.LocalHumanPlayer.Identity;
			if (identity != null)
			{
				if (identity.BlockLimits == null)
				{
					MyLog.Default.Error("BlockLimits is null in SetIdentityBlockTypesBuilt");
				}
				else if (MyEventContext.Current.IsLocallyInvoked)
				{
					identity.BlockLimits.CallLimitsChanged();
				}
				else
				{
					identity.BlockLimits.SetTypeLimitsFromServer(limits);
				}
			}
		}

		[Event(null, 1001)]
		[Reliable]
		[Client]
		private static void SetIdentityGridBlocksBuilt(MyBlockLimits.MyGridLimitData limits, int pcu, int pcuBuilt, int blocksBuilt, int transferedDelta)
		{
			MyIdentity identity = MySession.Static.LocalHumanPlayer.Identity;
			if (identity != null)
			{
				if (MyEventContext.Current.IsLocallyInvoked)
				{
					identity.BlockLimits.CallLimitsChanged();
				}
				else
				{
					identity.BlockLimits.SetGridLimitsFromServer(limits, pcu, pcuBuilt, blocksBuilt, transferedDelta);
				}
			}
		}

		[Event(null, 1016)]
		[Reliable]
		[Client]
		private static void SetPCU_Client(int pcu, int transferedDelta)
		{
			MyIdentity identity = MySession.Static.LocalHumanPlayer.Identity;
			if (identity != null)
			{
				if (MyEventContext.Current.IsLocallyInvoked)
				{
					identity.BlockLimits.CallLimitsChanged();
				}
				else
				{
					identity.BlockLimits.SetPCUFromServer(pcu, transferedDelta);
				}
			}
		}

		public void RequestPlayerColorChanged(int playerSerialId, int colorIndex, Vector3 newColor)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPlayerColorChangedRequest, playerSerialId, colorIndex, newColor);
		}

		public void RequestPlayerColorsChanged(int playerSerialId, List<Vector3> newColors)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPlayerColorsChangedRequest, playerSerialId, newColors);
		}

		public void RequestNewPlayer(int serialNumber, string playerName, string characterModel, bool realPlayer, bool initialPlayer)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnNewPlayerRequest, Sync.MyId, serialNumber, playerName, characterModel, realPlayer, initialPlayer);
		}

		public void RequestNewNpcIdentity()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnNpcIdentityRequest);
		}

		public MyPlayer CreateNewPlayer(MyIdentity identity, MyNetworkClient steamClient, string playerName, bool realPlayer)
		{
			MyPlayer.PlayerId playerId = FindFreePlayerId(steamClient.SteamUserId);
			MyObjectBuilder_Player playerBuilder = new MyObjectBuilder_Player
			{
				DisplayName = playerName,
				IdentityId = identity.IdentityId
			};
			return CreateNewPlayerInternal(steamClient, playerId, playerBuilder);
		}

		public MyPlayer CreateNewPlayer(MyIdentity identity, MyPlayer.PlayerId id, string playerName, bool realPlayer, bool initialPlayer, bool newIdentity)
		{
			Sync.Clients.TryGetClient(id.SteamId, out MyNetworkClient client);
			if (client == null)
			{
				return null;
			}
			MyObjectBuilder_Player playerBuilder = new MyObjectBuilder_Player
			{
				DisplayName = playerName,
				IdentityId = identity.IdentityId,
				ForceRealPlayer = realPlayer
			};
			MyPlayer myPlayer = CreateNewPlayerInternal(client, id, playerBuilder);
			if (myPlayer != null)
			{
				List<Vector3> list = null;
				if (!MyPlayer.IsColorsSetToDefaults(myPlayer.BuildColorSlots))
				{
					list = myPlayer.BuildColorSlots;
				}
				if (!initialPlayer || MyMultiplayer.Static == null)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPlayerCreated, id.SteamId, id.SerialId, identity.IdentityId, playerName, list, realPlayer);
				}
				else
				{
					PlayerDataMsg playerDataMsg = default(PlayerDataMsg);
					playerDataMsg.ClientSteamId = id.SteamId;
					playerDataMsg.PlayerSerialId = id.SerialId;
					playerDataMsg.IdentityId = identity.IdentityId;
					playerDataMsg.DisplayName = playerName;
					playerDataMsg.BuildColors = list;
					playerDataMsg.RealPlayer = realPlayer;
					playerDataMsg.NewIdentity = newIdentity;
					PlayerDataMsg msg = playerDataMsg;
					MyMultiplayer.GetReplicationServer().SendPlayerData(ref msg);
				}
			}
			return myPlayer;
		}

		public MyPlayer InitNewPlayer(MyPlayer.PlayerId id, MyObjectBuilder_Player playerOb)
		{
			Sync.Clients.TryGetClient(id.SteamId, out MyNetworkClient client);
			if (client == null)
			{
				return null;
			}
			return CreateNewPlayerInternal(client, id, playerOb);
		}

		public void RemovePlayer(MyPlayer player, bool removeCharacter = true)
		{
			if (Sync.IsServer)
			{
				if (removeCharacter && player.Character != null && !(player.Character.Parent is MyCryoChamber))
				{
					player.Character.Close();
				}
				KillPlayer(player);
				if (player.IsLocalPlayer)
				{
					RaiseLocalPlayerRemoved(player.Id.SerialId);
				}
				if (this.PlayerRemoved != null)
				{
					this.PlayerRemoved(player.Id);
				}
				RespawnComponent.AfterRemovePlayer(player);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPlayerRemoved, player.Id.SteamId, player.Id.SerialId);
				RemovePlayerFromDictionary(player.Id);
			}
			else if (!player.IsRemotePlayer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPlayerRemoveRequest, player.Id.SteamId, player.Id.SerialId, removeCharacter);
			}
		}

		public MyPlayer GetPlayerById(MyPlayer.PlayerId id)
		{
			MyPlayer value = null;
			m_players.TryGetValue(id, out value);
			return value;
		}

		public MyPlayer GetPlayerByName(string name)
		{
			foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in m_players)
			{
				if (player.Value.DisplayName.Equals(name))
				{
					return player.Value;
				}
			}
			return null;
		}

		public bool TryGetPlayerById(MyPlayer.PlayerId id, out MyPlayer player)
		{
			return m_players.TryGetValue(id, out player);
		}

		public bool TrySetControlledEntity(MyPlayer.PlayerId id, MyEntity entity)
		{
			MyPlayer controllingPlayer = GetControllingPlayer(entity);
			if (controllingPlayer == null)
			{
				SetControlledEntity(id, entity);
				return true;
			}
			return controllingPlayer.Id == id;
		}

		public void SetControlledEntity(ulong steamUserId, MyEntity entity)
		{
			MyPlayer.PlayerId id = new MyPlayer.PlayerId(steamUserId);
			SetControlledEntity(id, entity);
		}

		public void SetControlledEntityLocally(MyPlayer.PlayerId id, MyEntity entity)
		{
			SetControlledEntityInternal(id, entity, sync: false);
		}

		public void SetControlledEntity(MyPlayer.PlayerId id, MyEntity entity)
		{
			if (Sync.IsServer)
			{
				SetControlledEntityInternal(id, entity);
			}
		}

		public List<MyPlayer> GetPlayersStartingNameWith(string prefix)
		{
			List<MyPlayer> list = new List<MyPlayer>();
			foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in m_players)
			{
				string displayName = player.Value.DisplayName;
				if (prefix.Length == 0 || (displayName.Length >= prefix.Length && prefix.Equals(displayName.Substring(0, prefix.Length))))
				{
					list.Add(player.Value);
				}
			}
			return list;
		}

		public void RemoveControlledEntity(MyEntity entity)
		{
			RemoveControlledEntityProxy(entity, immediateOnServer: true);
		}

		private void RemoveControlledEntityProxy(MyEntity entity, bool immediateOnServer)
		{
			if (Sync.IsServer)
			{
				RemoveControlledEntityInternal(entity, immediateOnServer);
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnControlReleased, entity.EntityId);
		}

		public void SetPlayerToCockpit(MyPlayer player, MyEntity controlledEntity)
		{
			Sync.Players.SetControlledEntityInternal(player.Id, controlledEntity);
			if (player == MySession.Static.LocalHumanPlayer && MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity && !MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.LocalCharacter);
			}
		}

		public void SetPlayerCharacter(MyPlayer player, MyCharacter newCharacter, MyEntity spawnedBy)
		{
			newCharacter.SetPlayer(player);
			if (MyVisualScriptLogicProvider.PlayerSpawned != null && !newCharacter.IsBot)
			{
				MyVisualScriptLogicProvider.PlayerSpawned(newCharacter.ControllerInfo.Controller.Player.Identity.IdentityId);
			}
			if (spawnedBy != null)
			{
				long entityId = spawnedBy.EntityId;
				Vector3 arg = (newCharacter.WorldMatrix * MatrixD.Invert(spawnedBy.WorldMatrix)).Translation;
				MyMultiplayer.RaiseEvent(newCharacter, (MyCharacter x) => x.SpawnCharacterRelative, entityId, arg);
			}
		}

		public MyPlayer GetControllingPlayer(MyEntity entity)
		{
			if (m_controlledEntities.TryGetValue(entity.EntityId, out MyPlayer.PlayerId value) && m_players.TryGetValue(value, out MyPlayer value2))
			{
				return value2;
			}
			return null;
		}

		public MyPlayer GetPreviousControllingPlayer(MyEntity entity)
		{
			if (m_previousControlledEntities.TryGetValue(entity.EntityId, out MyPlayer.PlayerId value) && m_players.TryGetValue(value, out MyPlayer value2))
			{
				return value2;
			}
			return null;
		}

		public MyEntityController GetEntityController(MyEntity entity)
		{
			return GetControllingPlayer(entity)?.Controller;
		}

		public ICollection<MyPlayer> GetOnlinePlayers()
		{
			return m_players.Values;
		}

		public int GetOnlinePlayerCount()
		{
			return m_players.Values.Count;
		}

		public bool IsPlayerOnline(ref MyPlayer.PlayerId playerId)
		{
			if (m_players.ContainsKey(playerId))
			{
				return true;
			}
			return false;
		}

		public bool IsPlayerOnline(long identityId)
		{
			if (!MySession.Static.Players.TryGetPlayerId(identityId, out MyPlayer.PlayerId result))
			{
				return false;
			}
			if (!MySession.Static.Players.IsPlayerOnline(ref result))
			{
				return false;
			}
			return true;
		}

		public ICollection<MyIdentity> GetAllIdentities()
		{
			return m_allIdentities.Values;
		}

		public IOrderedEnumerable<KeyValuePair<long, MyIdentity>> GetAllIdentitiesOrderByName()
		{
			return m_allIdentities.OrderBy((KeyValuePair<long, MyIdentity> pair) => pair.Value.DisplayName);
		}

		public HashSet<long> GetNPCIdentities()
		{
			return m_npcIdentities;
		}

		public ICollection<MyPlayer.PlayerId> GetAllPlayers()
		{
			return m_playerIdentityIds.Keys;
		}

		public void UpdatePlayerControllers(long controllerId)
		{
			foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in m_players)
			{
				if (player.Value.CachedControllerId != null && player.Value.CachedControllerId.Contains(controllerId))
				{
					TryTakeControl(player.Value, controllerId);
				}
			}
		}

		public void SendDirtyBlockLimits()
		{
			switch (MySession.Static.BlockLimitsEnabled)
			{
			case MyBlockLimitsEnabledEnum.GLOBALLY:
				foreach (MyPlayer onlinePlayer in GetOnlinePlayers())
				{
					if (onlinePlayer.Identity != null && onlinePlayer.IsRealPlayer)
					{
						m_tmpPlayersLinkedToBlockLimit.Add(new EndpointId(onlinePlayer.Id.SteamId));
					}
				}
				SendDirtyBlockLimit(MySession.Static.GlobalBlockLimits, m_tmpPlayersLinkedToBlockLimit);
				m_tmpPlayersLinkedToBlockLimit.Clear();
				break;
			case MyBlockLimitsEnabledEnum.PER_FACTION:
				foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
				{
					foreach (MyPlayer onlinePlayer2 in GetOnlinePlayers())
					{
						if (faction.Value.IsMember(onlinePlayer2.Identity.IdentityId))
						{
							m_tmpPlayersLinkedToBlockLimit.Add(new EndpointId(onlinePlayer2.Id.SteamId));
						}
					}
					if (m_tmpPlayersLinkedToBlockLimit.Count > 0)
					{
						SendDirtyBlockLimit(faction.Value.BlockLimits, m_tmpPlayersLinkedToBlockLimit);
					}
					m_tmpPlayersLinkedToBlockLimit.Clear();
				}
				foreach (MyPlayer onlinePlayer3 in GetOnlinePlayers())
				{
					if (MySession.Static.Factions.GetPlayerFaction(onlinePlayer3.Identity.IdentityId) == null)
					{
						m_tmpPlayersLinkedToBlockLimit.Add(new EndpointId(onlinePlayer3.Id.SteamId));
						SendDirtyBlockLimit(onlinePlayer3.Identity.BlockLimits, m_tmpPlayersLinkedToBlockLimit);
					}
				}
				break;
			case MyBlockLimitsEnabledEnum.PER_PLAYER:
				foreach (MyPlayer onlinePlayer4 in GetOnlinePlayers())
				{
					if (onlinePlayer4.Identity != null && onlinePlayer4.IsRealPlayer)
					{
						m_tmpPlayersLinkedToBlockLimit.Add(new EndpointId(onlinePlayer4.Id.SteamId));
						SendDirtyBlockLimit(onlinePlayer4.Identity.BlockLimits, m_tmpPlayersLinkedToBlockLimit);
						m_tmpPlayersLinkedToBlockLimit.Clear();
					}
				}
				break;
			}
		}

		public void SendDirtyBlockLimit(MyBlockLimits blockLimit, List<EndpointId> playersToSendTo)
		{
			foreach (MyBlockLimits.MyTypeLimitData value2 in blockLimit.BlockTypeBuilt.Values)
			{
				if (Interlocked.CompareExchange(ref value2.Dirty, 0, 1) > 0)
				{
					foreach (EndpointId item in playersToSendTo)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetIdentityBlockTypesBuilt, value2, item);
					}
				}
			}
			foreach (MyBlockLimits.MyGridLimitData value3 in blockLimit.BlocksBuiltByGrid.Values)
			{
				if (Interlocked.CompareExchange(ref value3.Dirty, 0, 1) > 0)
				{
					foreach (EndpointId item2 in playersToSendTo)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetIdentityGridBlocksBuilt, value3, blockLimit.PCU, blockLimit.PCUBuilt, blockLimit.BlocksBuilt, blockLimit.TransferedDelta, item2);
					}
				}
				if (Interlocked.CompareExchange(ref value3.NameDirty, 0, 1) > 0)
				{
					foreach (EndpointId item3 in playersToSendTo)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MyBlockLimits.SetGridNameFromServer, value3.EntityId, value3.GridName, item3);
					}
				}
			}
			if (blockLimit.CompareExchangePCUDirty())
			{
				foreach (EndpointId item4 in playersToSendTo)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetPCU_Client, blockLimit.PCU, blockLimit.TransferedDelta, item4);
				}
			}
			while (true)
			{
				long num = blockLimit.GridsRemoved.Keys.ElementAtOrDefault(0);
				if (num != 0L)
				{
					if (blockLimit.GridsRemoved.TryRemove(num, out MyBlockLimits.MyGridLimitData value))
					{
						foreach (EndpointId item5 in playersToSendTo)
						{
							MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetIdentityGridBlocksBuilt, value, blockLimit.PCU, blockLimit.PCUBuilt, blockLimit.BlocksBuilt, blockLimit.TransferedDelta, item5);
						}
					}
					continue;
				}
				break;
			}
		}

		public void TryExtendControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity, MyEntity entityGettingControl)
		{
			MyEntityController controller = baseEntity.ControllerInfo.Controller;
			if (controller != null)
			{
				TrySetControlledEntity(controller.Player.Id, entityGettingControl);
			}
		}

		public void ExtendControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity, MyEntity entityGettingControl)
		{
			MyEntityController controller = baseEntity.ControllerInfo.Controller;
			if (controller != null)
			{
				TrySetControlledEntity(controller.Player.Id, entityGettingControl);
			}
			else
			{
				_ = (baseEntity is MyRemoteControl);
			}
		}

		public bool TryReduceControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity, MyEntity entityWhichLoosesControl)
		{
			MyEntityController controller = baseEntity.ControllerInfo.Controller;
			if (controller != null && m_controlledEntities.TryGetValue(entityWhichLoosesControl.EntityId, out MyPlayer.PlayerId value) && controller.Player.Id == value)
			{
				RemoveControlledEntity(entityWhichLoosesControl);
				return true;
			}
			return false;
		}

		public void ReduceControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity, MyEntity entityWhichLoosesControl)
		{
			if (!TryReduceControl(baseEntity, entityWhichLoosesControl))
			{
				_ = (baseEntity is MyRemoteControl);
			}
		}

		public void ReduceAllControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity)
		{
			if (m_controlledEntities.TryGetValue(baseEntity.Entity.EntityId, out MyPlayer.PlayerId value))
			{
				foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity in m_controlledEntities)
				{
					if (!(controlledEntity.Value != value) && controlledEntity.Key != baseEntity.Entity.EntityId)
					{
						MyEntity entity = null;
						MyEntities.TryGetEntityById(controlledEntity.Key, out entity, allowClosed: true);
						if (entity != null)
						{
							RemoveControlledEntityProxy(entity, immediateOnServer: false);
						}
					}
				}
				m_controlledEntities.ApplyRemovals();
			}
		}

		public bool HasExtendedControl(Sandbox.Game.Entities.IMyControllableEntity baseEntity, MyEntity secondEntity)
		{
			return baseEntity.ControllerInfo.Controller == GetEntityController(secondEntity);
		}

		[Event(null, 1558)]
		[Reliable]
		[Server]
		public static void SetDampeningEntity(long controlledEntityId)
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = MyEntities.GetEntityByIdOrDefault(controlledEntityId) as Sandbox.Game.Entities.IMyControllableEntity;
			if (myControllableEntity != null && myControllableEntity.Entity != null)
			{
				MatrixD headMatrix = myControllableEntity.GetHeadMatrix(includeY: true);
				Vector3D forward = headMatrix.Forward;
				Vector3D translation = headMatrix.Translation;
				Vector3D to = translation + forward * 1000.0;
				LineD line = new LineD(translation, to);
				MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, (MyEntity)myControllableEntity, myControllableEntity.Entity.GetTopMostParent(), ignoreChildren: false, ignoreFloatingObjects: false);
				if (intersectionWithLine.HasValue && intersectionWithLine.Value.Entity != null)
				{
					IMyEntity topMostParent = intersectionWithLine.Value.Entity.GetTopMostParent();
					myControllableEntity.RelativeDampeningEntity = (MyEntity)topMostParent;
				}
				else
				{
					myControllableEntity.RelativeDampeningEntity = null;
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetDampeningEntityClient, myControllableEntity.Entity.EntityId, (myControllableEntity.RelativeDampeningEntity != null) ? myControllableEntity.RelativeDampeningEntity.EntityId : 0);
			}
		}

		[Event(null, 1594)]
		[Reliable]
		[Server]
		public static void ClearDampeningEntity(long controlledEntityId)
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = MyEntities.GetEntityByIdOrDefault(controlledEntityId) as Sandbox.Game.Entities.IMyControllableEntity;
			if (myControllableEntity != null)
			{
				myControllableEntity.RelativeDampeningEntity = null;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SetDampeningEntityClient, myControllableEntity.Entity.EntityId, 0L);
			}
		}

		[Event(null, 1607)]
		[Reliable]
		[BroadcastExcept]
		public static void SetDampeningEntityClient(long controlledEntityId, long dampeningEntityId)
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = MyEntities.GetEntityByIdOrDefault(controlledEntityId) as Sandbox.Game.Entities.IMyControllableEntity;
			if (myControllableEntity != null)
			{
				MyEntity entityByIdOrDefault = MyEntities.GetEntityByIdOrDefault(dampeningEntityId);
				if (entityByIdOrDefault != null)
				{
					myControllableEntity.RelativeDampeningEntity = entityByIdOrDefault;
				}
				else
				{
					myControllableEntity.RelativeDampeningEntity = null;
				}
			}
		}

		public MyIdentity CreateNewNpcIdentity(string name, long identityId = 0L)
		{
			MyIdentity myIdentity = (identityId != 0L) ? base.CreateNewIdentity(name, identityId, null, null) : base.CreateNewIdentity(name);
			if (Sync.IsServer && !MyBankingSystem.Static.TryGetAccountInfo(myIdentity.IdentityId, out MyAccountInfo _))
			{
				MyBankingSystem.Static.CreateAccount(myIdentity.IdentityId);
			}
			AfterCreateIdentity(myIdentity, addToNpcs: true);
			return myIdentity;
		}

		public MyIdentity CreateNewIdentity(string name, string model = null, Vector3? colorMask = null, bool initialPlayer = false, bool addToNpcs = false)
		{
			MyIdentity myIdentity = base.CreateNewIdentity(name, model);
			bool sendToClients = !initialPlayer;
			AfterCreateIdentity(myIdentity, addToNpcs, sendToClients);
			return myIdentity;
		}

		public override MyIdentity CreateNewIdentity(string name, long identityId, string model, Vector3? colorMask)
		{
			bool flag = false;
			MyEntityIdentifier.ID_OBJECT_TYPE idObjectType = MyEntityIdentifier.GetIdObjectType(identityId);
			if (idObjectType == MyEntityIdentifier.ID_OBJECT_TYPE.NPC || idObjectType == MyEntityIdentifier.ID_OBJECT_TYPE.SPAWN_GROUP)
			{
				flag = ((byte)((flag ? 1 : 0) | 1) != 0);
			}
			MyIdentity myIdentity = base.CreateNewIdentity(name, identityId, model, colorMask);
			AfterCreateIdentity(myIdentity, flag);
			return myIdentity;
		}

		public override MyIdentity CreateNewIdentity(MyObjectBuilder_Identity objectBuilder)
		{
			bool addToNpcs = false;
			MyEntityIdentifier.ID_OBJECT_TYPE idObjectType = MyEntityIdentifier.GetIdObjectType(objectBuilder.IdentityId);
			if (idObjectType == MyEntityIdentifier.ID_OBJECT_TYPE.NPC || idObjectType == MyEntityIdentifier.ID_OBJECT_TYPE.SPAWN_GROUP)
			{
				addToNpcs = true;
			}
			MyIdentity myIdentity = base.CreateNewIdentity(objectBuilder);
			AfterCreateIdentity(myIdentity, addToNpcs);
			return myIdentity;
		}

		public void RemoveIdentity(long identityId, MyPlayer.PlayerId playerId = default(MyPlayer.PlayerId))
		{
			if (Sync.IsServer)
			{
				if (RemoveIdentityInternal(identityId, playerId))
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnIdentityRemovedSuccess, identityId, playerId.SteamId, playerId.SerialId);
				}
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnIdentityRemovedRequest, identityId, playerId.SteamId, playerId.SerialId);
			}
		}

		public bool HasIdentity(long identityId)
		{
			return m_allIdentities.ContainsKey(identityId);
		}

		public MyIdentity TryGetIdentity(long identityId)
		{
			m_allIdentities.TryGetValue(identityId, out MyIdentity value);
			return value;
		}

		public bool TryGetPlayerId(long identityId, out MyPlayer.PlayerId result)
		{
			return m_identityPlayerIds.TryGetValue(identityId, out result);
		}

		public MyIdentity TryGetPlayerIdentity(MyPlayer.PlayerId playerId)
		{
			MyIdentity result = null;
			long num = TryGetIdentityId(playerId.SteamId, playerId.SerialId);
			if (num != 0L)
			{
				result = TryGetIdentity(num);
			}
			return result;
		}

		public long TryGetIdentityId(ulong steamId, int serialId = 0)
		{
			long value = 0L;
			MyPlayer.PlayerId key = new MyPlayer.PlayerId(steamId, serialId);
			m_playerIdentityIds.TryGetValue(key, out value);
			return value;
		}

		public ulong TryGetSteamId(long identityId)
		{
			if (!TryGetPlayerId(identityId, out MyPlayer.PlayerId result))
			{
				return 0uL;
			}
			return result.SteamId;
		}

		public int TryGetSerialId(long identityId)
		{
			if (!TryGetPlayerId(identityId, out MyPlayer.PlayerId result))
			{
				return 0;
			}
			return result.SerialId;
		}

		public string TryGetIdentityNameFromSteamId(ulong steamId)
		{
			MyIdentity myIdentity = TryGetIdentity(TryGetIdentityId(steamId));
			if (myIdentity != null)
			{
				return myIdentity.DisplayName;
			}
			return string.Empty;
		}

		public void MarkIdentityAsNPC(long identityId)
		{
			m_npcIdentities.Add(identityId);
		}

		public void UnmarkIdentityAsNPC(long identityId)
		{
			m_npcIdentities.Remove(identityId);
		}

		public bool IdentityIsNpc(long identityId)
		{
			return m_npcIdentities.Contains(identityId);
		}

		public void LoadIdentities(List<MyObjectBuilder_Identity> list)
		{
			if (list != null)
			{
				foreach (MyObjectBuilder_Identity item in list)
				{
					MyIdentity myIdentity = CreateNewIdentity(item);
					if (Sync.IsServer && !MyBankingSystem.Static.TryGetAccountInfo(myIdentity.IdentityId, out MyAccountInfo _))
					{
						MyBankingSystem.Static.CreateAccount(myIdentity.IdentityId);
					}
				}
			}
		}

		public void ClearIdentities()
		{
			m_allIdentities.Clear();
			m_npcIdentities.Clear();
		}

		public void SetRespawnComponent(MyRespawnComponentBase respawnComponent)
		{
			RespawnComponent = respawnComponent;
		}

		public static void RequestLocalRespawn()
		{
			MySandboxGame.Log.WriteLine("RequestRespawn");
			if (!Sandbox.Engine.Platform.Game.IsDedicated && Sync.Players != null)
			{
				string model = null;
				Color color = Color.Red;
				MyLocalCache.GetCharacterInfoFromInventoryConfig(ref model, ref color);
				Sync.Players.LocalRespawnRequested.InvokeIfNotNull(model, color);
				if (MyMultiplayer.Static != null)
				{
					MyMultiplayer.Static.InvokeLocalRespawnRequested();
				}
			}
		}

		public static void RespawnRequest(bool joinGame, bool newIdentity, long respawnEntityId, string shipPrefabId, int playerSerialId, string modelName, Color color)
		{
			RespawnMsg respawnMsg = default(RespawnMsg);
			respawnMsg.JoinGame = joinGame;
			respawnMsg.RespawnEntityId = respawnEntityId;
			respawnMsg.NewIdentity = newIdentity;
			respawnMsg.RespawnShipId = shipPrefabId;
			respawnMsg.PlayerSerialId = playerSerialId;
			respawnMsg.ModelName = modelName;
			respawnMsg.Color = color;
			RespawnMsg arg = respawnMsg;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRespawnRequest, arg);
		}

		public void KillPlayer(MyPlayer player)
		{
			SetPlayerDead(player, deadState: true, MySession.Static.Settings.PermanentDeath.Value);
		}

		public void RevivePlayer(MyPlayer player)
		{
			SetPlayerDead(player, deadState: false, resetIdentity: false);
		}

		private void SetPlayerDead(MyPlayer player, bool deadState, bool resetIdentity)
		{
			if (Sync.IsServer)
			{
				if (SetPlayerDeadInternal(player.Id.SteamId, player.Id.SerialId, deadState, resetIdentity))
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnSetPlayerDeadSuccess, player.Id.SteamId, player.Id.SerialId, deadState, resetIdentity);
				}
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnSetPlayerDeadRequest, player.Id.SteamId, player.Id.SerialId, deadState, resetIdentity);
			}
		}

		private bool SetPlayerDeadInternal(ulong playerSteamId, int playerSerialId, bool deadState, bool resetIdentity)
		{
			MyPlayer.PlayerId id = new MyPlayer.PlayerId(playerSteamId, playerSerialId);
			MyPlayer playerById = Sync.Players.GetPlayerById(id);
			if (playerById == null)
			{
				return false;
			}
			if (playerById.Identity == null)
			{
				return false;
			}
			playerById.Identity.SetDead(resetIdentity);
			if (deadState)
			{
				playerById.Controller.TakeControl(null);
				foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity in m_controlledEntities)
				{
					if (controlledEntity.Value == playerById.Id)
					{
						MyEntity entity = null;
						MyEntities.TryGetEntityById(controlledEntity.Key, out entity);
						if (entity != null)
						{
							RemoveControlledEntityInternal(entity, immediate: false);
						}
					}
				}
				m_controlledEntities.ApplyRemovals();
				if (Sync.Clients.LocalClient != null && playerById == Sync.Clients.LocalClient.FirstPlayer)
				{
					RequestLocalRespawn();
				}
			}
			return true;
		}

		[Event(null, 1909)]
		[Reliable]
		[Server]
		private static void OnRespawnRequest(RespawnMsg msg)
		{
			OnRespawnRequest(msg.JoinGame, msg.NewIdentity, msg.RespawnEntityId, msg.RespawnShipId, null, null, realPlayer: true, msg.PlayerSerialId, msg.ModelName, msg.Color);
		}

		public static void OnRespawnRequest(bool joinGame, bool newIdentity, long respawnEntityId, string respawnShipId, Vector3D? spawnPosition, SerializableDefinitionId? botDefinitionId, bool realPlayer, int playerSerialId, string modelName, Color color)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			EndpointId targetEndpoint = (!MyEventContext.Current.IsLocallyInvoked) ? MyEventContext.Current.Sender : new EndpointId(Sync.MyId);
			if (Sync.Players.RespawnComponent == null)
			{
				return;
			}
			MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(targetEndpoint.Value, playerSerialId);
			if (Sync.Players.RespawnComponent.HandleRespawnRequest(joinGame, newIdentity, respawnEntityId, respawnShipId, playerId, spawnPosition, botDefinitionId, realPlayer, modelName, color))
			{
				MyIdentity myIdentity = Sync.Players.TryGetPlayerIdentity(playerId);
				if (myIdentity != null)
				{
					if (!myIdentity.FirstSpawnDone)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnIdentityFirstSpawn, myIdentity.IdentityId);
						myIdentity.PerformFirstSpawn();
					}
					myIdentity.LogRespawnTime();
				}
				MyPlayer playerById = Sync.Players.GetPlayerById(playerId);
				if (playerById != null && playerById.Controller != null)
				{
					MyEntity myEntity = playerById.Controller.ControlledEntity as MyEntity;
					if (myEntity != null)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnControlChangedSuccess, playerId.SteamId, playerId.SerialId, myEntity.EntityId, arg5: true);
					}
				}
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnRespawnRequestFailure, playerSerialId, targetEndpoint);
			}
		}

		private MyPlayer CreateNewPlayerInternal(MyNetworkClient steamClient, MyPlayer.PlayerId playerId, MyObjectBuilder_Player playerBuilder)
		{
			if (!m_playerIdentityIds.ContainsKey(playerId))
			{
				m_playerIdentityIds.TryAdd(playerId, playerBuilder.IdentityId);
				if (!m_identityPlayerIds.ContainsKey(playerBuilder.IdentityId))
				{
					m_identityPlayerIds.Add(playerBuilder.IdentityId, playerId);
				}
			}
			MyPlayer myPlayer = GetPlayerById(playerId);
			if (myPlayer == null)
			{
				myPlayer = new MyPlayer(steamClient, playerId);
				myPlayer.Init(playerBuilder);
				myPlayer.IdentityChanged += player_IdentityChanged;
				myPlayer.Controller.ControlledEntityChanged += controller_ControlledEntityChanged;
				AddPlayer(playerId, myPlayer);
				if (MyFakes.ENABLE_MISSION_TRIGGERS && MySessionComponentMissionTriggers.Static != null)
				{
					MySessionComponentMissionTriggers.Static.TryCreateFromDefault(playerId);
				}
			}
			return myPlayer;
		}

		public static void ChangePlayerCharacter(MyPlayer player, MyCharacter characterEntity, MyEntity entity)
		{
			if (player == null)
			{
				MySandboxGame.Log.WriteLine("Player not found");
				return;
			}
			if (player.Identity == null)
			{
				MySandboxGame.Log.WriteLine("Player identity was null");
			}
			player.Identity.ChangeCharacter(characterEntity);
			if (player.Controller == null || player.Controller.ControlledEntity == null)
			{
				Sync.Players.SetControlledEntityInternal(player.Id, entity, sync: false);
			}
			if (player == MySession.Static.LocalHumanPlayer)
			{
				MyShipController myShipController = MySession.Static.ControlledEntity as MyShipController;
				if (myShipController == null || myShipController.Pilot != characterEntity)
				{
					MySession.Static.SetCameraController(MySession.Static.LocalCharacter.IsInFirstPersonView ? MyCameraControllerEnum.Entity : MyCameraControllerEnum.ThirdPersonSpectator, MySession.Static.LocalCharacter);
				}
			}
		}

		private void SetControlledEntityInternal(MyPlayer.PlayerId id, MyEntity entity, bool sync = true)
		{
			if (!Sync.IsServer && (!m_controlledEntitiesClientCache.TryGetValue(entity.EntityId, out MyPlayer.PlayerId value) || !(value == id)))
			{
				return;
			}
			MyPlayer playerById = Sync.Players.GetPlayerById(id);
			RemoveControlledEntityInternal(entity);
			entity.OnClosing += m_entityClosingHandler;
			if (playerById != null && playerById.Controller != null && entity is Sandbox.Game.Entities.IMyControllableEntity)
			{
				if (entity is MyCharacter && playerById.Identity != null && entity != playerById.Identity.Character)
				{
					playerById.Identity.ChangeCharacter(entity as MyCharacter);
				}
				playerById.Controller.TakeControl((Sandbox.Game.Entities.IMyControllableEntity)entity);
			}
			else if (playerById != null)
			{
				m_controlledEntities.Add(entity.EntityId, playerById.Id, immediate: true);
			}
			if (Sync.IsServer && sync && playerById != null)
			{
				ulong steamId = playerById.Id.SteamId;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnControlChangedSuccess, steamId, playerById.Id.SerialId, entity.EntityId, arg5: false);
			}
			if (MySession.Static.LocalHumanPlayer != null && id == MySession.Static.LocalHumanPlayer.Id)
			{
				IMyCameraController myCameraController = entity as IMyCameraController;
				if (myCameraController != null)
				{
					MySession.Static.SetCameraController(myCameraController.IsInFirstPersonView ? MyCameraControllerEnum.Entity : MyCameraControllerEnum.ThirdPersonSpectator, entity);
				}
			}
		}

		private void controller_ControlledEntityChanged(Sandbox.Game.Entities.IMyControllableEntity oldEntity, Sandbox.Game.Entities.IMyControllableEntity newEntity)
		{
			MyEntityController myEntityController = (newEntity == null) ? oldEntity.ControllerInfo.Controller : newEntity.ControllerInfo.Controller;
			MyEntity myEntity = oldEntity as MyEntity;
			if (myEntity != null)
			{
				if (m_controlledEntities.TryGetValue(myEntity.EntityId, out MyPlayer.PlayerId value))
				{
					m_previousControlledEntities[myEntity.EntityId] = value;
				}
				m_controlledEntities.Remove(myEntity.EntityId, immediate: true);
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnControlChangedSuccess, 0uL, 0, myEntity.EntityId, arg5: true);
				}
			}
			MyEntity myEntity2 = newEntity as MyEntity;
			if (myEntity2 != null && myEntityController != null)
			{
				m_controlledEntities.Add(myEntity2.EntityId, myEntityController.Player.Id, immediate: true);
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnControlChangedSuccess, myEntityController.Player.Id.SteamId, myEntityController.Player.Id.SerialId, myEntity2.EntityId, arg5: true);
				}
			}
		}

		private void RemoveControlledEntityInternal(MyEntity entity, bool immediate = true)
		{
			entity.OnClosing -= m_entityClosingHandler;
			if (m_controlledEntities.TryGetValue(entity.EntityId, out MyPlayer.PlayerId value))
			{
				m_previousControlledEntities[entity.EntityId] = value;
			}
			m_controlledEntities.Remove(entity.EntityId, immediate);
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnControlChangedSuccess, 0uL, 0, entity.EntityId, arg5: true);
			}
		}

		private void EntityClosing(MyEntity entity)
		{
			entity.OnClosing -= m_entityClosingHandler;
			if (!(entity is Sandbox.Game.Entities.IMyControllableEntity))
			{
				m_controlledEntities.Remove(entity.EntityId, immediate: true);
				m_previousControlledEntities.Remove(entity.EntityId);
				if (Sync.IsServer)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnControlReleased, entity.EntityId);
				}
			}
		}

		private void Multiplayer_ClientRemoved(ulong steamId)
		{
			if (Sync.IsServer)
			{
				m_tmpRemovedPlayers.Clear();
				foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in m_players)
				{
					if (player.Key.SteamId == steamId)
					{
						m_tmpRemovedPlayers.Add(player.Value);
					}
				}
				foreach (MyPlayer tmpRemovedPlayer in m_tmpRemovedPlayers)
				{
					RemovePlayer(tmpRemovedPlayer, removeCharacter: false);
				}
				m_tmpRemovedPlayers.Clear();
			}
		}

		private void RaiseLocalPlayerRemoved(int serialId)
		{
			this.LocalPlayerRemoved?.Invoke(serialId);
		}

		private bool RemoveIdentityInternal(long identityId, MyPlayer.PlayerId playerId)
		{
			if (playerId.IsValid && m_players.ContainsKey(playerId))
			{
				return false;
			}
			if (m_allIdentities.TryGetValue(identityId, out MyIdentity value))
			{
				value.ChangeCharacter(null);
				value.CharacterChanged -= Identity_CharacterChanged;
			}
			m_allIdentities.Remove(identityId);
			m_npcIdentities.Remove(identityId);
			if (playerId.IsValid)
			{
				if (m_playerIdentityIds.TryGetValue(playerId, out long value2))
				{
					m_identityPlayerIds.Remove(value2);
				}
				m_playerIdentityIds.Remove(playerId);
			}
			this.IdentitiesChanged?.Invoke();
			return true;
		}

		private void LoadIdentitiesObsolete(List<MyObjectBuilder_Checkpoint.PlayerItem> playersFromSession, MyPlayer.PlayerId? savingPlayerId = null)
		{
			foreach (MyObjectBuilder_Checkpoint.PlayerItem item in playersFromSession)
			{
				MyIdentity myIdentity = CreateNewIdentity(item.Name, item.PlayerId, item.Model, null);
				MyPlayer.PlayerId playerId = new MyPlayer.PlayerId(item.SteamId);
				if (savingPlayerId.HasValue && playerId == savingPlayerId.Value)
				{
					playerId = new MyPlayer.PlayerId(Sync.MyId);
				}
				if (!item.IsDead && !m_playerIdentityIds.ContainsKey(playerId))
				{
					m_playerIdentityIds.TryAdd(playerId, myIdentity.IdentityId);
					m_identityPlayerIds.Add(myIdentity.IdentityId, playerId);
					myIdentity.SetDead(dead: false);
				}
			}
		}

		private void AfterCreateIdentity(MyIdentity identity, bool addToNpcs = false, bool sendToClients = true)
		{
			if (addToNpcs)
			{
				MarkIdentityAsNPC(identity.IdentityId);
			}
			if (!m_allIdentities.ContainsKey(identity.IdentityId))
			{
				m_allIdentities.TryAdd(identity.IdentityId, identity);
				identity.CharacterChanged += Identity_CharacterChanged;
				if (identity.Character != null)
				{
					identity.Character.CharacterDied += Character_CharacterDied;
				}
			}
			if (Sync.IsServer && Sync.MyId != 0 && sendToClients)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnIdentityCreated, addToNpcs, identity.IdentityId, identity.DisplayName);
			}
			this.IdentitiesChanged?.Invoke();
			MySession.Static.Factions.ForceRelationsOnNewIdentity(identity.IdentityId);
		}

		private void Character_CharacterDied(MyCharacter diedCharacter)
		{
			if (this.PlayerCharacterDied != null && diedCharacter != null && diedCharacter.ControllerInfo.ControllingIdentityId != 0L)
			{
				this.PlayerCharacterDied(diedCharacter.ControllerInfo.ControllingIdentityId);
			}
		}

		private void Identity_CharacterChanged(MyCharacter oldCharacter, MyCharacter newCharacter)
		{
			if (oldCharacter != null)
			{
				oldCharacter.CharacterDied -= Character_CharacterDied;
			}
			if (newCharacter != null)
			{
				newCharacter.CharacterDied += Character_CharacterDied;
			}
		}

		private void LoadPlayerInternal(ref MyPlayer.PlayerId playerId, MyObjectBuilder_Player playerOb, bool obsolete = false)
		{
			MyIdentity myIdentity = TryGetIdentity(playerOb.IdentityId);
			if (myIdentity == null || (obsolete && myIdentity.IsDead))
			{
				return;
			}
			if (Sync.IsServer && Sync.MyId != playerId.SteamId)
			{
				playerOb.Connected = Sync.Clients.HasClient(playerId.SteamId);
			}
			if (!playerOb.Connected)
			{
				if (!m_playerIdentityIds.ContainsKey(playerId))
				{
					m_playerIdentityIds.TryAdd(playerId, playerOb.IdentityId);
					m_identityPlayerIds.Add(playerOb.IdentityId, playerId);
				}
				myIdentity.SetDead(dead: true);
			}
			else if (InitNewPlayer(playerId, playerOb).IsLocalPlayer)
			{
				Sync.Players.LocalPlayerLoaded?.Invoke(playerId.SerialId);
			}
		}

		public MyPlayer.PlayerId FindFreePlayerId(ulong steamId)
		{
			MyPlayer.PlayerId playerId;
			for (playerId = new MyPlayer.PlayerId(steamId); m_playerIdentityIds.ContainsKey(playerId); ++playerId)
			{
			}
			return playerId;
		}

		private long FindLocalIdentityId(MyObjectBuilder_Checkpoint checkpoint)
		{
			long num = 0L;
			num = TryGetIdentityId(Sync.MyId);
			if (num != 0L)
			{
				return num;
			}
			if (checkpoint.Players != null && checkpoint.Players.Dictionary.ContainsKey(Sync.MyId))
			{
				num = ((checkpoint.Players[Sync.MyId].PlayerId != 0L) ? checkpoint.Players[Sync.MyId].PlayerId : num);
			}
			if (checkpoint.AllPlayers != null)
			{
				foreach (MyObjectBuilder_Checkpoint.PlayerItem allPlayer in checkpoint.AllPlayers)
				{
					if (allPlayer.SteamId == Sync.MyId && !allPlayer.IsDead)
					{
						return allPlayer.PlayerId;
					}
					if (allPlayer.SteamId == Sync.MyId && num == allPlayer.PlayerId && allPlayer.IsDead)
					{
						return 0L;
					}
				}
				return num;
			}
			return num;
		}

		private void player_IdentityChanged(MyPlayer player, MyIdentity identity)
		{
			long key = m_playerIdentityIds[player.Id];
			m_identityPlayerIds.Remove(key);
			m_playerIdentityIds[player.Id] = identity.IdentityId;
			m_identityPlayerIds[identity.IdentityId] = player.Id;
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnPlayerIdentityChanged, player.Id.SteamId, player.Id.SerialId, identity.IdentityId);
			}
		}

		private string GetPlayerCharacter(MyPlayer player)
		{
			if (player.Identity.Character != null)
			{
				return player.Identity.Character.Entity.ToString();
			}
			return "<empty>";
		}

		private string GetControlledEntity(MyPlayer player)
		{
			if (player.Controller.ControlledEntity != null)
			{
				return player.Controller.ControlledEntity.Entity.ToString();
			}
			return "<empty>";
		}

		[Conditional("DEBUG")]
		public void WriteDebugInfo()
		{
			StackFrame frame = new StackTrace().GetFrame(1);
			foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in m_players)
			{
				bool isLocalPlayer = player.Value.IsLocalPlayer;
				string.Concat(frame.GetMethod().Name + (isLocalPlayer ? "; Control: [L] " : "; Control: "), player.Value.Id.ToString());
				(from s in m_controlledEntities
					where s.Value == player.Value.Id
					select s.Key.ToString("X")).ToArray();
			}
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				_ = entity;
			}
		}

		[Conditional("DEBUG")]
		public void DebugDraw()
		{
			int num = 0;
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "Steam clients:", Color.GreenYellow, 0.5f);
			foreach (MyNetworkClient client in Sync.Clients.GetClients())
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "  SteamId: " + client.SteamUserId + ", Name: " + client.DisplayName, Color.LightYellow, 0.5f);
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "Online players:", Color.GreenYellow, 0.5f);
			foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in m_players)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "  PlayerId: " + player.Key.ToString() + ", Name: " + player.Value.DisplayName, Color.LightYellow, 0.5f);
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "Player identities:", Color.GreenYellow, 0.5f);
			foreach (KeyValuePair<MyPlayer.PlayerId, long> playerIdentityId in m_playerIdentityIds)
			{
				m_players.TryGetValue(playerIdentityId.Key, out MyPlayer value);
				string text = (value == null) ? "N.A." : value.DisplayName;
				m_allIdentities.TryGetValue(playerIdentityId.Value, out MyIdentity value2);
				Color color = (value2 == null || value2.IsDead) ? Color.Salmon : Color.LightYellow;
				string text2 = (value2 == null) ? "N.A." : value2.DisplayName;
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "  PlayerId: " + playerIdentityId.Key.ToString() + ", Name: " + text + "; IdentityId: " + playerIdentityId.Value + ", Name: " + text2, color, 0.5f);
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "All identities:", Color.GreenYellow, 0.5f);
			foreach (KeyValuePair<long, MyIdentity> allIdentity in m_allIdentities)
			{
				bool isDead = allIdentity.Value.IsDead;
				Color color2 = isDead ? Color.Salmon : Color.LightYellow;
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "  IdentityId: " + allIdentity.Key + ", Name: " + allIdentity.Value.DisplayName + ", State: " + (isDead ? "DEAD" : "ALIVE"), color2, 0.5f);
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "Control:", Color.GreenYellow, 0.5f);
			foreach (KeyValuePair<long, MyPlayer.PlayerId> controlledEntity in m_controlledEntities)
			{
				MyEntities.TryGetEntityById(controlledEntity.Key, out MyEntity entity);
				Color color3 = (entity == null) ? Color.Salmon : Color.LightYellow;
				string text3 = (entity == null) ? "Unknown entity" : entity.ToString();
				string text4 = (entity == null) ? "N.A." : entity.EntityId.ToString();
				m_players.TryGetValue(controlledEntity.Value, out MyPlayer value3);
				string text5 = (value3 == null) ? "N.A." : value3.DisplayName;
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "  " + text3 + " controlled by " + text5 + " (entityId = " + text4 + ", playerId = " + controlledEntity.Value.ToString() + ")", color3, 0.5f);
			}
			if (MySession.Static.ControlledEntity != null)
			{
				MyShipController myShipController = MySession.Static.ControlledEntity as MyShipController;
				if (myShipController != null)
				{
					(myShipController.Parent as MyCubeGrid)?.GridSystems.ControlSystem.DebugDraw((float)(++num) * 13f);
				}
			}
		}

		void IMyPlayerCollection.GetPlayers(List<IMyPlayer> players, Func<IMyPlayer, bool> collect)
		{
			foreach (KeyValuePair<MyPlayer.PlayerId, MyPlayer> player in m_players)
			{
				if (collect == null || collect(player.Value))
				{
					players.Add(player.Value);
				}
			}
		}

		void IMyPlayerCollection.ExtendControl(VRage.Game.ModAPI.Interfaces.IMyControllableEntity entityWithControl, IMyEntity entityGettingControl)
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = entityWithControl as Sandbox.Game.Entities.IMyControllableEntity;
			MyEntity myEntity = entityGettingControl as MyEntity;
			if (myControllableEntity != null && myEntity != null)
			{
				ExtendControl(myControllableEntity, myEntity);
			}
		}

		bool IMyPlayerCollection.HasExtendedControl(VRage.Game.ModAPI.Interfaces.IMyControllableEntity firstEntity, IMyEntity secondEntity)
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = firstEntity as Sandbox.Game.Entities.IMyControllableEntity;
			MyEntity myEntity = secondEntity as MyEntity;
			if (myControllableEntity != null && myEntity != null)
			{
				return HasExtendedControl(myControllableEntity, myEntity);
			}
			return false;
		}

		void IMyPlayerCollection.ReduceControl(VRage.Game.ModAPI.Interfaces.IMyControllableEntity entityWhichKeepsControl, IMyEntity entityWhichLoosesControl)
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = entityWhichKeepsControl as Sandbox.Game.Entities.IMyControllableEntity;
			MyEntity myEntity = entityWhichLoosesControl as MyEntity;
			if (myControllableEntity != null && myEntity != null)
			{
				ReduceControl(myControllableEntity, myEntity);
			}
		}

		void IMyPlayerCollection.RemoveControlledEntity(IMyEntity entity)
		{
			MyEntity myEntity = entity as MyEntity;
			if (myEntity != null)
			{
				RemoveControlledEntity(myEntity);
			}
		}

		void IMyPlayerCollection.SetControlledEntity(ulong steamUserId, IMyEntity entity)
		{
			MyEntity myEntity = entity as MyEntity;
			if (myEntity != null)
			{
				SetControlledEntity(steamUserId, myEntity);
			}
		}

		void IMyPlayerCollection.TryExtendControl(VRage.Game.ModAPI.Interfaces.IMyControllableEntity entityWithControl, IMyEntity entityGettingControl)
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = entityWithControl as Sandbox.Game.Entities.IMyControllableEntity;
			MyEntity myEntity = entityGettingControl as MyEntity;
			if (myControllableEntity != null && myEntity != null)
			{
				TryExtendControl(myControllableEntity, myEntity);
			}
		}

		bool IMyPlayerCollection.TryReduceControl(VRage.Game.ModAPI.Interfaces.IMyControllableEntity entityWhichKeepsControl, IMyEntity entityWhichLoosesControl)
		{
			Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = entityWhichKeepsControl as Sandbox.Game.Entities.IMyControllableEntity;
			MyEntity myEntity = entityWhichLoosesControl as MyEntity;
			if (myControllableEntity != null && myEntity != null)
			{
				return TryReduceControl(myControllableEntity, myEntity);
			}
			return false;
		}

		IMyPlayer IMyPlayerCollection.GetPlayerControllingEntity(IMyEntity entity)
		{
			MyEntity myEntity = entity as MyEntity;
			if (myEntity != null)
			{
				MyEntityController entityController = GetEntityController(myEntity);
				if (entityController != null)
				{
					return entityController.Player;
				}
			}
			return null;
		}

		void IMyPlayerCollection.GetAllIdentites(List<IMyIdentity> identities, Func<IMyIdentity, bool> collect)
		{
			foreach (KeyValuePair<long, MyIdentity> allIdentity in m_allIdentities)
			{
				if (collect == null || collect(allIdentity.Value))
				{
					identities.Add(allIdentity.Value);
				}
			}
		}

		long IMyPlayerCollection.TryGetIdentityId(ulong steamId)
		{
			return TryGetIdentityId(steamId);
		}

		ulong IMyPlayerCollection.TryGetSteamId(long identityId)
		{
			return TryGetSteamId(identityId);
		}

		void IMyPlayerCollection.RequestChangeBalance(long identityId, long amount)
		{
			if (MyBankingSystem.Static != null)
			{
				MyBankingSystem.RequestBalanceChange(identityId, amount);
			}
		}
	}
}
