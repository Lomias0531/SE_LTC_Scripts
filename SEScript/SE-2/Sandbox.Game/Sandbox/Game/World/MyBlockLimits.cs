using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Game.World
{
	[StaticEventOwner]
	public class MyBlockLimits
	{
		[Serializable]
		public class MyGridLimitData
		{
			protected class Sandbox_Game_World_MyBlockLimits_003C_003EMyGridLimitData_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyGridLimitData, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyGridLimitData owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyGridLimitData owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003EMyGridLimitData_003C_003EGridName_003C_003EAccessor : IMemberAccessor<MyGridLimitData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyGridLimitData owner, in string value)
				{
					owner.GridName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyGridLimitData owner, out string value)
				{
					value = owner.GridName;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003EMyGridLimitData_003C_003EBlocksBuilt_003C_003EAccessor : IMemberAccessor<MyGridLimitData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyGridLimitData owner, in int value)
				{
					owner.BlocksBuilt = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyGridLimitData owner, out int value)
				{
					value = owner.BlocksBuilt;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003EMyGridLimitData_003C_003EPCUBuilt_003C_003EAccessor : IMemberAccessor<MyGridLimitData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyGridLimitData owner, in int value)
				{
					owner.PCUBuilt = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyGridLimitData owner, out int value)
				{
					value = owner.PCUBuilt;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003EMyGridLimitData_003C_003EDirty_003C_003EAccessor : IMemberAccessor<MyGridLimitData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyGridLimitData owner, in int value)
				{
					owner.Dirty = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyGridLimitData owner, out int value)
				{
					value = owner.Dirty;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003EMyGridLimitData_003C_003ENameDirty_003C_003EAccessor : IMemberAccessor<MyGridLimitData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyGridLimitData owner, in int value)
				{
					owner.NameDirty = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyGridLimitData owner, out int value)
				{
					value = owner.NameDirty;
				}
			}

			public long EntityId;

			[NoSerialize]
			public string GridName;

			public int BlocksBuilt;

			public int PCUBuilt;

			[NoSerialize]
			public int Dirty;

			[NoSerialize]
			public int NameDirty;
		}

		[Serializable]
		public class MyTypeLimitData
		{
			protected class Sandbox_Game_World_MyBlockLimits_003C_003EMyTypeLimitData_003C_003EBlockPairName_003C_003EAccessor : IMemberAccessor<MyTypeLimitData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyTypeLimitData owner, in string value)
				{
					owner.BlockPairName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyTypeLimitData owner, out string value)
				{
					value = owner.BlockPairName;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003EMyTypeLimitData_003C_003EBlocksBuilt_003C_003EAccessor : IMemberAccessor<MyTypeLimitData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyTypeLimitData owner, in int value)
				{
					owner.BlocksBuilt = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyTypeLimitData owner, out int value)
				{
					value = owner.BlocksBuilt;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003EMyTypeLimitData_003C_003EDirty_003C_003EAccessor : IMemberAccessor<MyTypeLimitData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyTypeLimitData owner, in int value)
				{
					owner.Dirty = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyTypeLimitData owner, out int value)
				{
					value = owner.Dirty;
				}
			}

			public string BlockPairName;

			public int BlocksBuilt;

			[NoSerialize]
			public int Dirty;
		}

		[Serializable]
		private struct TransferMessageData
		{
			protected class Sandbox_Game_World_MyBlockLimits_003C_003ETransferMessageData_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<TransferMessageData, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TransferMessageData owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TransferMessageData owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003ETransferMessageData_003C_003EGridName_003C_003EAccessor : IMemberAccessor<TransferMessageData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TransferMessageData owner, in string value)
				{
					owner.GridName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TransferMessageData owner, out string value)
				{
					value = owner.GridName;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003ETransferMessageData_003C_003EBlocksBuilt_003C_003EAccessor : IMemberAccessor<TransferMessageData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TransferMessageData owner, in int value)
				{
					owner.BlocksBuilt = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TransferMessageData owner, out int value)
				{
					value = owner.BlocksBuilt;
				}
			}

			protected class Sandbox_Game_World_MyBlockLimits_003C_003ETransferMessageData_003C_003EPCUBuilt_003C_003EAccessor : IMemberAccessor<TransferMessageData, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref TransferMessageData owner, in int value)
				{
					owner.PCUBuilt = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref TransferMessageData owner, out int value)
				{
					value = owner.PCUBuilt;
				}
			}

			public long EntityId;

			public string GridName;

			public int BlocksBuilt;

			public int PCUBuilt;
		}

		protected sealed class SendTransferRequestMessage_003C_003ESandbox_Game_World_MyBlockLimits_003C_003EMyGridLimitData_0023System_Int64_0023System_Int64_0023System_UInt64 : ICallSite<IMyEventOwner, MyGridLimitData, long, long, ulong, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyGridLimitData gridData, in long oldOwner, in long newOwner, in ulong newOwnerSteamId, in DBNull arg5, in DBNull arg6)
			{
				SendTransferRequestMessage(gridData, oldOwner, newOwner, newOwnerSteamId);
			}
		}

		protected sealed class ReceiveTransferRequestMessage_003C_003ESandbox_Game_World_MyBlockLimits_003C_003ETransferMessageData_0023System_Int64_0023System_Int64_0023System_Int32_0023System_Int32 : ICallSite<IMyEventOwner, TransferMessageData, long, long, int, int, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in TransferMessageData gridData, in long oldOwner, in long newOwner, in int blocksCount, in int pcu, in DBNull arg6)
			{
				ReceiveTransferRequestMessage(gridData, oldOwner, newOwner, blocksCount, pcu);
			}
		}

		protected sealed class ReceiveTransferNotPossibleMessage_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long identityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ReceiveTransferNotPossibleMessage(identityId);
			}
		}

		protected sealed class TransferBlocksBuiltByID_003C_003ESystem_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridEntityId, in long oldOwner, in long newOwner, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TransferBlocksBuiltByID(gridEntityId, oldOwner, newOwner);
			}
		}

		protected sealed class TransferBlocksBuiltByIDClient_003C_003ESystem_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridEntityId, in long oldOwner, in long newOwner, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				TransferBlocksBuiltByIDClient(gridEntityId, oldOwner, newOwner);
			}
		}

		protected sealed class RemoveBlocksBuiltByID_003C_003ESystem_Int64_0023System_Int64 : ICallSite<IMyEventOwner, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridEntityId, in long identityID, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveBlocksBuiltByID(gridEntityId, identityID);
			}
		}

		protected sealed class SetGridNameFromServer_003C_003ESystem_Int64_0023System_String : ICallSite<IMyEventOwner, long, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long gridEntityId, in string newName, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetGridNameFromServer(gridEntityId, newName);
			}
		}

		public static readonly MyBlockLimits Empty = new MyBlockLimits(0, 0);

		private int m_blocksBuilt;

		private int m_PCUBuilt;

		private int m_PCU;

		private int m_PCUDirty;

		private int m_transferedDelta;

		public int BlockLimitModifier
		{
			get;
			set;
		}

		public ConcurrentDictionary<string, MyTypeLimitData> BlockTypeBuilt
		{
			get;
			private set;
		}

		public ConcurrentDictionary<long, MyGridLimitData> BlocksBuiltByGrid
		{
			get;
			private set;
		}

		public ConcurrentDictionary<long, MyGridLimitData> GridsRemoved
		{
			get;
			private set;
		}

		public int BlocksBuilt => m_blocksBuilt;

		public int PCU => m_PCU;

		public int PCUBuilt => m_PCUBuilt;

		public int TransferedDelta => m_transferedDelta;

		public int MaxBlocks => MySession.Static.MaxBlocksPerPlayer + BlockLimitModifier;

		public bool HasRemainingPCU => m_PCU > 0;

		public event Action BlockLimitsChanged;

		public static int GetInitialPCU(long identityId = -1L)
		{
			switch (MySession.Static.BlockLimitsEnabled)
			{
			case MyBlockLimitsEnabledEnum.NONE:
				return int.MaxValue;
			case MyBlockLimitsEnabledEnum.PER_PLAYER:
				return MySession.Static.TotalPCU / MySession.Static.MaxPlayers;
			case MyBlockLimitsEnabledEnum.PER_FACTION:
				if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && identityId != -1 && MySession.Static.Factions.GetPlayerFaction(identityId) == null)
				{
					return 0;
				}
				if (MySession.Static.MaxFactionsCount == 0)
				{
					return MySession.Static.TotalPCU;
				}
				return MySession.Static.TotalPCU / MySession.Static.MaxFactionsCount;
			default:
				return MySession.Static.TotalPCU;
			}
		}

		public static int GetMaxPCU(MyIdentity identity)
		{
			return GetInitialPCU(identity.IdentityId) + identity.BlockLimits.m_transferedDelta;
		}

		public MyBlockLimits(int initialPCU, int blockLimitModifier, int transferedDelta = 0)
		{
			BlockLimitModifier = blockLimitModifier;
			BlockTypeBuilt = new ConcurrentDictionary<string, MyTypeLimitData>();
			foreach (string key in MySession.Static.BlockTypeLimits.Keys)
			{
				BlockTypeBuilt.TryAdd(key, new MyTypeLimitData
				{
					BlockPairName = key
				});
			}
			BlocksBuiltByGrid = new ConcurrentDictionary<long, MyGridLimitData>();
			GridsRemoved = new ConcurrentDictionary<long, MyGridLimitData>();
			m_transferedDelta = transferedDelta;
			m_PCU = initialPCU + transferedDelta;
		}

		public static bool IsFactionChangePossible(long playerId, long newFaction)
		{
			if (MySession.Static.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION)
			{
				return true;
			}
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(playerId);
			if (myIdentity != null)
			{
				HashSet<MySlimBlock> blocksBuiltByPlayer = myIdentity.BlockLimits.GetBlocksBuiltByPlayer(playerId);
				int count = blocksBuiltByPlayer.Count;
				int num = blocksBuiltByPlayer.Sum((MySlimBlock x) => x.BlockDefinition.PCU);
				MyFaction myFaction = MySession.Static.Factions.TryGetFactionById(newFaction) as MyFaction;
				if (myFaction == null)
				{
					return false;
				}
				MyBlockLimits blockLimits = myFaction.BlockLimits;
				if (num > blockLimits.PCU && MySession.Static.Settings.TotalPCU > 0)
				{
					return false;
				}
				if (count > blockLimits.MaxBlocks && blockLimits.MaxBlocks > 0)
				{
					return false;
				}
				foreach (KeyValuePair<string, short> blockTypeLimit in MySession.Static.BlockTypeLimits)
				{
					if (myIdentity.BlockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, out MyTypeLimitData value) && blockLimits.BlockTypeBuilt.TryGetValue(blockTypeLimit.Key, out MyTypeLimitData value2) && value.BlocksBuilt + value2.BlocksBuilt > blockTypeLimit.Value)
					{
						return false;
					}
				}
			}
			return true;
		}

		public static void TransferBlockLimits(long playerId, MyBlockLimits oldLimits, MyBlockLimits newLimits)
		{
			foreach (KeyValuePair<long, MyGridLimitData> item in oldLimits.BlocksBuiltByGrid)
			{
				MyCubeGrid entity = null;
				if (MyEntities.TryGetEntityById(item.Key, out entity))
				{
					entity.TransferBlockLimitsBuiltByID(playerId, oldLimits, newLimits);
				}
			}
		}

		public static void TransferBlockLimits(long oldOwner, long newOwner)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(oldOwner);
			if (myIdentity != null)
			{
				using (IEnumerator<KeyValuePair<long, MyGridLimitData>> enumerator = myIdentity.BlockLimits.BlocksBuiltByGrid.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MyMultiplayer.RaiseStaticEvent<long, long, long>(arg2: enumerator.Current.Key, action: (IMyEventOwner x) => TransferBlocksBuiltByID, arg3: oldOwner, arg4: newOwner, targetEndpoint: new EndpointId(Sync.MyId));
					}
				}
			}
		}

		public static bool IsTransferBlocksBuiltByIDPossible(long gridEntityId, long oldOwner, long newOwner, out int blocksCount, out int pcu)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(oldOwner);
			MyIdentity myIdentity2 = MySession.Static.Players.TryGetIdentity(newOwner);
			blocksCount = 0;
			pcu = 0;
			if (myIdentity == null || myIdentity2 == null)
			{
				return false;
			}
			if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE)
			{
				return true;
			}
			if (!myIdentity.BlockLimits.BlocksBuiltByGrid.TryGetValue(gridEntityId, out MyGridLimitData _))
			{
				return false;
			}
			MyCubeGrid gridFromId = GetGridFromId(gridEntityId);
			if (gridFromId == null)
			{
				return false;
			}
			HashSet<MySlimBlock> hashSet = gridFromId.FindBlocksBuiltByID(oldOwner);
			blocksCount = hashSet.Count;
			pcu = hashSet.Sum((MySlimBlock x) => x.BlockDefinition.PCU);
			if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.GLOBALLY)
			{
				return true;
			}
			if (myIdentity2.BlockLimits.MaxBlocks > 0 && blocksCount + myIdentity2.BlockLimits.BlocksBuilt > myIdentity2.BlockLimits.MaxBlocks)
			{
				return false;
			}
			if (pcu > myIdentity2.BlockLimits.PCU)
			{
				return false;
			}
			Dictionary<string, short> dictionary = new Dictionary<string, short>();
			foreach (MySlimBlock item in gridFromId.FindBlocksBuiltByID(oldOwner))
			{
				if (MySession.Static.BlockTypeLimits.ContainsKey(item.BlockDefinition.BlockPairName))
				{
					if (dictionary.ContainsKey(item.BlockDefinition.BlockPairName))
					{
						dictionary[item.BlockDefinition.BlockPairName]++;
					}
					else
					{
						dictionary[item.BlockDefinition.BlockPairName] = 1;
					}
				}
			}
			foreach (KeyValuePair<string, MyTypeLimitData> item2 in myIdentity2.BlockLimits.BlockTypeBuilt)
			{
				if (dictionary.ContainsKey(item2.Key))
				{
					dictionary[item2.Key] += (short)item2.Value.BlocksBuilt;
				}
				else
				{
					dictionary[item2.Key] = (short)item2.Value.BlocksBuilt;
				}
			}
			foreach (KeyValuePair<string, short> item3 in dictionary)
			{
				if (item3.Value > MySession.Static.BlockTypeLimits[item3.Key])
				{
					return false;
				}
			}
			return true;
		}

		private static MyCubeGrid GetGridFromId(long gridEntityId)
		{
			MyEntity entityById = MyEntities.GetEntityById(gridEntityId);
			if (entityById == null)
			{
				return null;
			}
			MyCubeGrid myCubeGrid = entityById as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return null;
			}
			return myCubeGrid;
		}

		[Event(null, 328)]
		[Reliable]
		[Server]
		public static void SendTransferRequestMessage(MyGridLimitData gridData, long oldOwner, long newOwner, ulong newOwnerSteamId)
		{
			if (!IsTransferBlocksBuiltByIDPossible(gridData.EntityId, oldOwner, newOwner, out int blocksCount, out int pcu))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ReceiveTransferNotPossibleMessage, newOwner, MyEventContext.Current.Sender);
				return;
			}
			MyGridLimitData myGridLimitData = MySession.Static.Players.TryGetIdentity(oldOwner).BlockLimits.BlocksBuiltByGrid[gridData.EntityId];
			TransferMessageData transferMessageData;
			if (MyEventContext.Current.IsLocallyInvoked)
			{
				transferMessageData = default(TransferMessageData);
				transferMessageData.EntityId = myGridLimitData.EntityId;
				transferMessageData.GridName = myGridLimitData.GridName;
				transferMessageData.BlocksBuilt = myGridLimitData.BlocksBuilt;
				transferMessageData.PCUBuilt = myGridLimitData.PCUBuilt;
				ReceiveTransferRequestMessage(transferMessageData, oldOwner, newOwner, blocksCount, pcu);
			}
			else
			{
				Func<IMyEventOwner, Action<TransferMessageData, long, long, int, int>> action = (IMyEventOwner x) => ReceiveTransferRequestMessage;
				transferMessageData = new TransferMessageData
				{
					EntityId = myGridLimitData.EntityId,
					GridName = myGridLimitData.GridName,
					BlocksBuilt = myGridLimitData.BlocksBuilt,
					PCUBuilt = myGridLimitData.PCUBuilt
				};
				MyMultiplayer.RaiseStaticEvent(action, transferMessageData, oldOwner, newOwner, blocksCount, pcu, new EndpointId(newOwnerSteamId));
			}
		}

		[Event(null, 374)]
		[Reliable]
		[Client]
		private static void ReceiveTransferRequestMessage(TransferMessageData gridData, long oldOwner, long newOwner, int blocksCount, int pcu)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(oldOwner);
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextConfirmAcceptTransferGrid), myIdentity.DisplayName, blocksCount.ToString(), pcu, gridData.GridName), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TransferBlocksBuiltByID, gridData.EntityId, oldOwner, newOwner);
				}
			}, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		[Event(null, 394)]
		[Reliable]
		[Client]
		private static void ReceiveTransferNotPossibleMessage(long identityId)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(identityId);
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextNotEnoughFreeBlocksForTransfer, myIdentity.DisplayName), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		[Event(null, 407)]
		[Reliable]
		[Server]
		private static void TransferBlocksBuiltByID(long gridEntityId, long oldOwner, long newOwner)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && MySession.Static.Players.TryGetSteamId(newOwner) != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyCubeGrid gridFromId = GetGridFromId(gridEntityId);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(newOwner);
			if (gridFromId != null && myIdentity != null)
			{
				gridFromId.TransferBlocksBuiltByID(oldOwner, newOwner);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => TransferBlocksBuiltByIDClient, gridFromId.EntityId, oldOwner, newOwner);
				ulong num = MySession.Static.Players.TryGetSteamId(myIdentity.IdentityId);
				if (num != 0L)
				{
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetGridNameFromServer, gridFromId.EntityId, gridFromId.DisplayName, new EndpointId(num));
				}
			}
		}

		[Event(null, 436)]
		[Reliable]
		[BroadcastExcept]
		public static void TransferBlocksBuiltByIDClient(long gridEntityId, long oldOwner, long newOwner)
		{
			MyEntities.TryGetEntityById(gridEntityId, out MyEntity entity);
			(entity as MyCubeGrid)?.TransferBlocksBuiltByIDClient(oldOwner, newOwner);
		}

		[Event(null, 450)]
		[Reliable]
		[Server]
		public static void RemoveBlocksBuiltByID(long gridEntityId, long identityID)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && MySession.Static.Players.TryGetSteamId(identityID) != MyEventContext.Current.Sender.Value)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				GetGridFromId(gridEntityId)?.RemoveBlocksBuiltByID(identityID);
			}
		}

		[Event(null, 468)]
		[Reliable]
		[Client]
		public static void SetGridNameFromServer(long gridEntityId, string newName)
		{
			MyIdentity identity = MySession.Static.LocalHumanPlayer.Identity;
			if (identity.BlockLimits.BlocksBuiltByGrid.TryGetValue(gridEntityId, out MyGridLimitData value))
			{
				value.GridName = newName;
			}
			identity.BlockLimits.CallLimitsChanged();
		}

		private void OnGridNameChangedServer(MyCubeGrid grid)
		{
			if (BlocksBuiltByGrid.TryGetValue(grid.EntityId, out MyGridLimitData value))
			{
				value.GridName = grid.DisplayName;
				value.NameDirty = 1;
			}
		}

		public void IncreaseBlocksBuilt(string type, int pcu, MyCubeGrid grid, bool modifyBlockCount = true)
		{
			if (Empty == this || !Sync.IsServer)
			{
				return;
			}
			if (modifyBlockCount)
			{
				Interlocked.Increment(ref m_blocksBuilt);
			}
			if (grid != null)
			{
				Interlocked.Add(ref m_PCUBuilt, pcu);
				Interlocked.Add(ref m_PCU, -pcu);
			}
			if (modifyBlockCount && type != null && BlockTypeBuilt.TryGetValue(type, out MyTypeLimitData value))
			{
				Interlocked.Increment(ref value.BlocksBuilt);
				value.Dirty = 1;
			}
			if (grid == null)
			{
				return;
			}
			long entityId = grid.EntityId;
			bool flag = false;
			do
			{
				if (BlocksBuiltByGrid.TryGetValue(entityId, out MyGridLimitData value2))
				{
					if (modifyBlockCount)
					{
						Interlocked.Increment(ref value2.BlocksBuilt);
					}
					Interlocked.Add(ref value2.PCUBuilt, pcu);
					value2.Dirty = 1;
				}
				else if (BlocksBuiltByGrid.TryAdd(entityId, new MyGridLimitData
				{
					EntityId = grid.EntityId,
					BlocksBuilt = 1,
					PCUBuilt = pcu,
					GridName = (grid.DisplayName ?? "Unknown grid"),
					Dirty = 1
				}))
				{
					grid.OnNameChanged += OnGridNameChangedServer;
				}
				else
				{
					flag = true;
				}
			}
			while (flag);
			GridsRemoved.Remove(entityId);
		}

		public void DecreaseBlocksBuilt(string type, int pcu, MyCubeGrid grid, bool modifyBlockCount = true)
		{
			if (Empty == this || !Sync.IsServer)
			{
				return;
			}
			if (modifyBlockCount)
			{
				Interlocked.Decrement(ref m_blocksBuilt);
				_ = 0;
			}
			if (grid != null)
			{
				Interlocked.Add(ref m_PCUBuilt, -pcu);
				_ = 0;
				Interlocked.Add(ref m_PCU, pcu);
			}
			if (type != null && modifyBlockCount && BlockTypeBuilt.TryGetValue(type, out MyTypeLimitData value))
			{
				Interlocked.Decrement(ref value.BlocksBuilt);
				value.Dirty = 1;
			}
			if (grid == null)
			{
				return;
			}
			long entityId = grid.EntityId;
			if (BlocksBuiltByGrid.TryGetValue(entityId, out MyGridLimitData value2))
			{
				if (modifyBlockCount)
				{
					Interlocked.Decrement(ref value2.BlocksBuilt);
				}
				Interlocked.Add(ref value2.PCUBuilt, -pcu);
				value2.Dirty = 1;
				if (value2.BlocksBuilt == 0)
				{
					BlocksBuiltByGrid.Remove(entityId);
					GridsRemoved.TryAdd(entityId, value2);
					grid.OnNameChanged -= OnGridNameChangedServer;
				}
			}
		}

		internal void AddPCU(int pcuToAdd)
		{
			if (Sync.IsServer)
			{
				Interlocked.Add(ref m_PCU, pcuToAdd);
				Interlocked.Add(ref m_transferedDelta, pcuToAdd);
				m_PCUDirty = 1;
			}
		}

		internal bool CompareExchangePCUDirty()
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			return Interlocked.CompareExchange(ref m_PCUDirty, 0, 1) > 0;
		}

		public void SetAllDirty()
		{
			foreach (MyTypeLimitData value in BlockTypeBuilt.Values)
			{
				value.Dirty = 1;
			}
			foreach (MyGridLimitData value2 in BlocksBuiltByGrid.Values)
			{
				value2.Dirty = 1;
				value2.NameDirty = 1;
			}
		}

		public void SetTypeLimitsFromServer(MyTypeLimitData newLimit)
		{
			if (!BlockTypeBuilt.ContainsKey(newLimit.BlockPairName))
			{
				BlockTypeBuilt[newLimit.BlockPairName] = new MyTypeLimitData();
			}
			BlockTypeBuilt[newLimit.BlockPairName].BlocksBuilt = newLimit.BlocksBuilt;
			CallLimitsChanged();
		}

		public void SetGridLimitsFromServer(MyGridLimitData newLimit, int pcu, int pcuBuilt, int blocksBuilt, int transferedDelta)
		{
			Interlocked.Exchange(ref m_PCU, pcu);
			Interlocked.Exchange(ref m_PCUBuilt, pcuBuilt);
			Interlocked.Exchange(ref m_blocksBuilt, blocksBuilt);
			Interlocked.Exchange(ref m_transferedDelta, transferedDelta);
			if (newLimit.BlocksBuilt == 0)
			{
				BlocksBuiltByGrid.TryRemove(newLimit.EntityId, out MyGridLimitData _);
			}
			else if (!BlocksBuiltByGrid.TryAdd(newLimit.EntityId, newLimit))
			{
				MyGridLimitData value = BlocksBuiltByGrid[newLimit.EntityId];
				value.BlocksBuilt = newLimit.BlocksBuilt;
				value.PCUBuilt = newLimit.PCUBuilt;
			}
			CallLimitsChanged();
		}

		public void SetPCUFromServer(int pcu, int transferedDelta)
		{
			Interlocked.Exchange(ref m_PCU, pcu);
			Interlocked.Exchange(ref m_transferedDelta, transferedDelta);
		}

		public void CallLimitsChanged()
		{
			if (this.BlockLimitsChanged != null)
			{
				this.BlockLimitsChanged();
			}
		}

		private HashSet<MySlimBlock> GetBlocksBuiltByPlayer(long playerId)
		{
			HashSet<MySlimBlock> hashSet = new HashSet<MySlimBlock>();
			foreach (KeyValuePair<long, MyGridLimitData> item in BlocksBuiltByGrid)
			{
				if (MyEntities.TryGetEntityById(item.Key, out MyCubeGrid entity))
				{
					entity.FindBlocksBuiltByID(playerId, hashSet);
				}
			}
			return hashSet;
		}
	}
}
