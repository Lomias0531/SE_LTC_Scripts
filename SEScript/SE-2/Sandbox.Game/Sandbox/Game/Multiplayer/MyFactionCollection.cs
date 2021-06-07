using ProtoBuf;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions.Reputation;
using VRage.Game.Factions.Definitions;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Definitions.Reputation;
using VRage.Library.Utils;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
	[StaticEventOwner]
	public class MyFactionCollection : IEnumerable<KeyValuePair<long, MyFaction>>, IEnumerable, IMyFactionCollection
	{
		public enum MyFactionPeaceRequestState
		{
			None,
			Pending,
			Sent
		}

		public struct MyRelatablePair
		{
			public class ComparerType : IEqualityComparer<MyRelatablePair>
			{
				public bool Equals(MyRelatablePair x, MyRelatablePair y)
				{
					if (x.RelateeId1 != y.RelateeId1 || x.RelateeId2 != y.RelateeId2)
					{
						if (x.RelateeId1 == y.RelateeId2)
						{
							return x.RelateeId2 == y.RelateeId1;
						}
						return false;
					}
					return true;
				}

				public int GetHashCode(MyRelatablePair obj)
				{
					return obj.RelateeId1.GetHashCode() ^ obj.RelateeId2.GetHashCode();
				}
			}

			public long RelateeId1;

			public long RelateeId2;

			public static readonly ComparerType Comparer = new ComparerType();

			public MyRelatablePair(long id1, long id2)
			{
				RelateeId1 = id1;
				RelateeId2 = id2;
			}
		}

		[ProtoContract]
		public struct AddFactionMsg
		{
			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFounderId_003C_003EAccessor : IMemberAccessor<AddFactionMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in long value)
				{
					owner.FounderId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out long value)
				{
					value = owner.FounderId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionId_003C_003EAccessor : IMemberAccessor<AddFactionMsg, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in long value)
				{
					owner.FactionId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out long value)
				{
					value = owner.FactionId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionTag_003C_003EAccessor : IMemberAccessor<AddFactionMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in string value)
				{
					owner.FactionTag = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out string value)
				{
					value = owner.FactionTag;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionName_003C_003EAccessor : IMemberAccessor<AddFactionMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in string value)
				{
					owner.FactionName = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out string value)
				{
					value = owner.FactionName;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionDescription_003C_003EAccessor : IMemberAccessor<AddFactionMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in string value)
				{
					owner.FactionDescription = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out string value)
				{
					value = owner.FactionDescription;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionPrivateInfo_003C_003EAccessor : IMemberAccessor<AddFactionMsg, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in string value)
				{
					owner.FactionPrivateInfo = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out string value)
				{
					value = owner.FactionPrivateInfo;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003ECreateFromDefinition_003C_003EAccessor : IMemberAccessor<AddFactionMsg, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in bool value)
				{
					owner.CreateFromDefinition = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out bool value)
				{
					value = owner.CreateFromDefinition;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EType_003C_003EAccessor : IMemberAccessor<AddFactionMsg, MyFactionTypes>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in MyFactionTypes value)
				{
					owner.Type = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out MyFactionTypes value)
				{
					value = owner.Type;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionColor_003C_003EAccessor : IMemberAccessor<AddFactionMsg, SerializableVector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in SerializableVector3 value)
				{
					owner.FactionColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out SerializableVector3 value)
				{
					value = owner.FactionColor;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionIconColor_003C_003EAccessor : IMemberAccessor<AddFactionMsg, SerializableVector3>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in SerializableVector3 value)
				{
					owner.FactionIconColor = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out SerializableVector3 value)
				{
					value = owner.FactionIconColor;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionIconGroupId_003C_003EAccessor : IMemberAccessor<AddFactionMsg, SerializableDefinitionId?>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in SerializableDefinitionId? value)
				{
					owner.FactionIconGroupId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out SerializableDefinitionId? value)
				{
					value = owner.FactionIconGroupId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EFactionIconId_003C_003EAccessor : IMemberAccessor<AddFactionMsg, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref AddFactionMsg owner, in int value)
				{
					owner.FactionIconId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref AddFactionMsg owner, out int value)
				{
					value = owner.FactionIconId;
				}
			}

			private class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg_003C_003EActor : IActivator, IActivator<AddFactionMsg>
			{
				private sealed override object CreateInstance()
				{
					return default(AddFactionMsg);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override AddFactionMsg CreateInstance()
				{
					return (AddFactionMsg)(object)default(AddFactionMsg);
				}

				AddFactionMsg IActivator<AddFactionMsg>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public long FounderId;

			[ProtoMember(4)]
			public long FactionId;

			[ProtoMember(7)]
			public string FactionTag;

			[ProtoMember(10)]
			public string FactionName;

			[Serialize(MyObjectFlags.DefaultZero)]
			[ProtoMember(13)]
			public string FactionDescription;

			[ProtoMember(16)]
			public string FactionPrivateInfo;

			[ProtoMember(19)]
			public bool CreateFromDefinition;

			[ProtoMember(22)]
			public MyFactionTypes Type;

			[ProtoMember(25)]
			public SerializableVector3 FactionColor;

			[ProtoMember(26)]
			public SerializableVector3 FactionIconColor;

			[ProtoMember(28)]
			public SerializableDefinitionId? FactionIconGroupId;

			[ProtoMember(31)]
			public int FactionIconId;
		}

		[Serializable]
		public struct MyReputationChangeWrapper
		{
			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003C_003EFactionId_003C_003EAccessor : IMemberAccessor<MyReputationChangeWrapper, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationChangeWrapper owner, in long value)
				{
					owner.FactionId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationChangeWrapper owner, out long value)
				{
					value = owner.FactionId;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003C_003ERepTotal_003C_003EAccessor : IMemberAccessor<MyReputationChangeWrapper, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationChangeWrapper owner, in int value)
				{
					owner.RepTotal = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationChangeWrapper owner, out int value)
				{
					value = owner.RepTotal;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003C_003EChange_003C_003EAccessor : IMemberAccessor<MyReputationChangeWrapper, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationChangeWrapper owner, in int value)
				{
					owner.Change = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationChangeWrapper owner, out int value)
				{
					value = owner.Change;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003C_003EShowNotification_003C_003EAccessor : IMemberAccessor<MyReputationChangeWrapper, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationChangeWrapper owner, in bool value)
				{
					owner.ShowNotification = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationChangeWrapper owner, out bool value)
				{
					value = owner.ShowNotification;
				}
			}

			public long FactionId;

			public int RepTotal;

			public int Change;

			public bool ShowNotification;
		}

		[Serializable]
		public struct MyReputationModifiers
		{
			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationModifiers_003C_003EOwner_003C_003EAccessor : IMemberAccessor<MyReputationModifiers, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationModifiers owner, in float value)
				{
					owner.Owner = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationModifiers owner, out float value)
				{
					value = owner.Owner;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationModifiers_003C_003EFriend_003C_003EAccessor : IMemberAccessor<MyReputationModifiers, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationModifiers owner, in float value)
				{
					owner.Friend = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationModifiers owner, out float value)
				{
					value = owner.Friend;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationModifiers_003C_003ENeutral_003C_003EAccessor : IMemberAccessor<MyReputationModifiers, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationModifiers owner, in float value)
				{
					owner.Neutral = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationModifiers owner, out float value)
				{
					value = owner.Neutral;
				}
			}

			protected class Sandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationModifiers_003C_003EHostile_003C_003EAccessor : IMemberAccessor<MyReputationModifiers, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyReputationModifiers owner, in float value)
				{
					owner.Hostile = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyReputationModifiers owner, out float value)
				{
					value = owner.Hostile;
				}
			}

			public float Owner;

			public float Friend;

			public float Neutral;

			public float Hostile;
		}

		protected sealed class CreateFactionByDefinition_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string tag, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateFactionByDefinition(tag);
			}
		}

		protected sealed class UnlockAchievementForClient_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string achievement, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UnlockAchievementForClient(achievement);
			}
		}

		protected sealed class Invoke_AddRep_DEBUG_003C_003ESystem_Int64_0023System_Int64_0023System_Int32 : ICallSite<IMyEventOwner, long, long, int, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in long factionId, in int delta, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				Invoke_AddRep_DEBUG(playerId, factionId, delta);
			}
		}

		protected sealed class AddFactionPlayerReputationSuccess_003C_003ESystem_Int64_0023System_Collections_Generic_List_00601_003CSandbox_Game_Multiplayer_MyFactionCollection_003C_003EMyReputationChangeWrapper_003E : ICallSite<IMyEventOwner, long, List<MyReputationChangeWrapper>, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long playerId, in List<MyReputationChangeWrapper> changes, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddFactionPlayerReputationSuccess(playerId, changes);
			}
		}

		protected sealed class FactionStateChangeRequest_003C_003EVRage_Game_ModAPI_MyFactionStateChange_0023System_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, MyFactionStateChange, long, long, long, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyFactionStateChange action, in long fromFactionId, in long toFactionId, in long playerId, in DBNull arg5, in DBNull arg6)
			{
				FactionStateChangeRequest(action, fromFactionId, toFactionId, playerId);
			}
		}

		protected sealed class FactionStateChangeSuccess_003C_003EVRage_Game_ModAPI_MyFactionStateChange_0023System_Int64_0023System_Int64_0023System_Int64_0023System_Int64 : ICallSite<IMyEventOwner, MyFactionStateChange, long, long, long, long, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyFactionStateChange action, in long fromFactionId, in long toFactionId, in long playerId, in long senderId, in DBNull arg6)
			{
				FactionStateChangeSuccess(action, fromFactionId, toFactionId, playerId, senderId);
			}
		}

		protected sealed class ChangeAutoAcceptRequest_003C_003ESystem_Int64_0023System_Int64_0023System_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, long, long, bool, bool, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long factionId, in long playerId, in bool autoAcceptMember, in bool autoAcceptPeace, in DBNull arg5, in DBNull arg6)
			{
				ChangeAutoAcceptRequest(factionId, playerId, autoAcceptMember, autoAcceptPeace);
			}
		}

		protected sealed class ChangeAutoAcceptSuccess_003C_003ESystem_Int64_0023System_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, long, bool, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long factionId, in bool autoAcceptMember, in bool autoAcceptPeace, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ChangeAutoAcceptSuccess(factionId, autoAcceptMember, autoAcceptPeace);
			}
		}

		protected sealed class EditFactionRequest_003C_003ESandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg : ICallSite<IMyEventOwner, AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AddFactionMsg msgEdit, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EditFactionRequest(msgEdit);
			}
		}

		protected sealed class EditFactionSuccess_003C_003ESandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg : ICallSite<IMyEventOwner, AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AddFactionMsg msgEdit, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				EditFactionSuccess(msgEdit);
			}
		}

		protected sealed class CreateFactionRequest_003C_003ESandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg : ICallSite<IMyEventOwner, AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AddFactionMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateFactionRequest(msg);
			}
		}

		protected sealed class CreateFactionSuccess_003C_003ESandbox_Game_Multiplayer_MyFactionCollection_003C_003EAddFactionMsg : ICallSite<IMyEventOwner, AddFactionMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in AddFactionMsg msg, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				CreateFactionSuccess(msg);
			}
		}

		protected sealed class SetDefaultFactionStates_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long recivedFactionId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetDefaultFactionStates(recivedFactionId);
			}
		}

		protected sealed class AddDiscoveredFaction_Clients_003C_003ESystem_UInt64_0023System_Int32_0023System_Int64 : ICallSite<IMyEventOwner, ulong, int, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong playerId, in int serialId, in long factionId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				AddDiscoveredFaction_Clients(playerId, serialId, factionId);
			}
		}

		protected sealed class RemoveDiscoveredFaction_Clients_003C_003ESystem_UInt64_0023System_Int32_0023System_Int64 : ICallSite<IMyEventOwner, ulong, int, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong playerId, in int serialId, in long factionId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RemoveDiscoveredFaction_Clients(playerId, serialId, factionId);
			}
		}

		public const int MAX_CHARACTER_FACTION = 512;

		public const string DLC_ECONOMY_ICON_CATEGORY = "Other";

		public const int ACHIEVEMENT_FRIEND_OF_FACTION_COUNT = 3;

		public const string ACHIEVEMENT_KEY_FRIEND_OF_FACTION = "FriendOfFactions";

		private const string SPIDER_FACTION_TAG = "SPID";

		public Action<long, long> PlayerKilledByPlayer;

		public Action<long> PlayerKilledByUnknown;

		private Dictionary<long, MyFaction> m_factions = new Dictionary<long, MyFaction>();

		private Dictionary<string, MyFaction> m_factionsByTag = new Dictionary<string, MyFaction>();

		private Dictionary<long, HashSet<long>> m_factionRequests = new Dictionary<long, HashSet<long>>();

		private Dictionary<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> m_relationsBetweenFactions = new Dictionary<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>(MyRelatablePair.Comparer);

		private Dictionary<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> m_relationsBetweenPlayersAndFactions = new Dictionary<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>(MyRelatablePair.Comparer);

		private Dictionary<long, long> m_playerFaction = new Dictionary<long, long>();

		private Dictionary<MyPlayer.PlayerId, List<long>> m_playerToFactionsVis = new Dictionary<MyPlayer.PlayerId, List<long>>();

		private MyReputationSettingsDefinition m_reputationSettings;

		private Dictionary<long, Tuple<int, TimeSpan>> m_playerToReputationLimits = new Dictionary<long, Tuple<int, TimeSpan>>();

		private MyReputationNotification m_notificationRepInc;

		private MyReputationNotification m_notificationRepDec;

		public bool JoinableFactionsPresent
		{
			get
			{
				foreach (KeyValuePair<long, MyFaction> faction in m_factions)
				{
					if (faction.Value.AcceptHumans)
					{
						return true;
					}
				}
				return false;
			}
		}

		public MyFaction this[long factionId] => m_factions[factionId];

		public Dictionary<long, IMyFaction> Factions => ((IEnumerable<KeyValuePair<long, MyFaction>>)m_factions).ToDictionary((Func<KeyValuePair<long, MyFaction>, long>)((KeyValuePair<long, MyFaction> e) => e.Key), (Func<KeyValuePair<long, MyFaction>, IMyFaction>)((KeyValuePair<long, MyFaction> e) => e.Value));

		public event Action<MyFaction, MyPlayer.PlayerId> OnFactionDiscovered;

		public event Action<MyFaction, long> OnPlayerJoined;

		public event Action<MyFaction, long> OnPlayerLeft;

		public event Action<MyFactionStateChange, long, long, long, long> FactionStateChanged;

		public event Action<long, bool, bool> FactionAutoAcceptChanged;

		public event Action<long> FactionEdited;

		public event Action<long> FactionCreated;

		event Action<long, bool, bool> IMyFactionCollection.FactionAutoAcceptChanged
		{
			add
			{
				FactionAutoAcceptChanged += value;
			}
			remove
			{
				FactionAutoAcceptChanged -= value;
			}
		}

		event Action<long> IMyFactionCollection.FactionCreated
		{
			add
			{
				FactionCreated += value;
			}
			remove
			{
				FactionCreated -= value;
			}
		}

		event Action<long> IMyFactionCollection.FactionEdited
		{
			add
			{
				FactionEdited += value;
			}
			remove
			{
				FactionEdited -= value;
			}
		}

		event Action<MyFactionStateChange, long, long, long, long> IMyFactionCollection.FactionStateChanged
		{
			add
			{
				FactionStateChanged += value;
			}
			remove
			{
				FactionStateChanged -= value;
			}
		}

		public bool Contains(long factionId)
		{
			return m_factions.ContainsKey(factionId);
		}

		public bool FactionTagExists(string tag, IMyFaction doNotCheck = null)
		{
			return TryGetFactionByTag(tag, doNotCheck) != null;
		}

		public bool FactionNameExists(string name, IMyFaction doNotCheck = null)
		{
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				MyFaction value = faction.Value;
				if ((doNotCheck == null || doNotCheck.FactionId != value.FactionId) && string.Equals(name, value.Name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public IMyFaction TryGetFactionById(long factionId)
		{
			if (m_factions.TryGetValue(factionId, out MyFaction value))
			{
				return value;
			}
			return null;
		}

		public MyFaction TryGetOrCreateFactionByTag(string tag)
		{
			MyFaction myFaction = TryGetFactionByTag(tag);
			if (myFaction == null)
			{
				string tag2 = tag.ToUpperInvariant();
				if (MyDefinitionManager.Static.TryGetFactionDefinition(tag2) == null)
				{
					return null;
				}
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CreateFactionByDefinition, tag);
				myFaction = TryGetFactionByTag(tag);
			}
			return myFaction;
		}

		public bool IsNpcFaction(string tag)
		{
			string tag2 = tag.ToUpperInvariant();
			if (MyDefinitionManager.Static.TryGetFactionDefinition(tag2) != null)
			{
				return true;
			}
			MyFaction myFaction = TryGetFactionByTag(tag);
			if (myFaction != null)
			{
				MyFactionTypes factionType = myFaction.FactionType;
				if ((uint)(factionType - 2) <= 2u)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsNpcFaction(long factionId)
		{
			if (!m_factions.ContainsKey(factionId))
			{
				return false;
			}
			MyFaction myFaction = m_factions[factionId];
			if (myFaction != null)
			{
				MyFactionTypes factionType = myFaction.FactionType;
				if ((uint)(factionType - 2) <= 2u)
				{
					return true;
				}
			}
			return false;
		}

		internal bool IsDiscoveredByDefault(string tag)
		{
			string tag2 = tag.ToUpperInvariant();
			return MyDefinitionManager.Static.TryGetFactionDefinition(tag2)?.DiscoveredByDefault ?? false;
		}

		[Event(null, 299)]
		[Reliable]
		[Server]
		public static void CreateFactionByDefinition(string tag)
		{
			string text = tag.ToUpperInvariant();
			if (!MySession.Static.Factions.m_factionsByTag.ContainsKey(text))
			{
				MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(text);
				if (myFactionDefinition != null)
				{
					MyIdentity myIdentity = Sync.Players.CreateNewIdentity(myFactionDefinition.Founder);
					Sync.Players.MarkIdentityAsNPC(myIdentity.IdentityId);
					MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.FACTION);
					CreateFactionServer(myIdentity.IdentityId, text, myFactionDefinition.DisplayNameText, myFactionDefinition.DescriptionText, "", myFactionDefinition);
				}
			}
		}

		public void CreateDefaultFactions()
		{
			foreach (MyFactionDefinition defaultFaction in MyDefinitionManager.Static.GetDefaultFactions())
			{
				if (TryGetFactionByTag(defaultFaction.Tag) == null)
				{
					MyIdentity myIdentity = Sync.Players.CreateNewIdentity(defaultFaction.Founder);
					if (myIdentity != null)
					{
						Sync.Players.MarkIdentityAsNPC(myIdentity.IdentityId);
						long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.FACTION);
						if (!CreateFactionInternal(myIdentity.IdentityId, num, defaultFaction))
						{
							Sync.Players.RemoveIdentity(myIdentity.IdentityId);
						}
						MyBankingSystem.Static.CreateAccount(num, defaultFaction.StartingBalance);
					}
				}
			}
			MyPlayer.PlayerId? playerId = null;
			if (MySession.Static.LocalHumanPlayer != null)
			{
				playerId = MySession.Static.LocalHumanPlayer.Id;
			}
			CompatDefaultFactions(playerId);
		}

		public void CompatDefaultFactions(MyPlayer.PlayerId? playerId = null)
		{
			foreach (MyFactionDefinition item in MyDefinitionManager.Static.GetFactionsFromDefinition())
			{
				MyFaction myFaction = TryGetFactionByTag(item.Tag);
				if (myFaction != null)
				{
					if (playerId.HasValue && IsDiscoveredByDefault(item.Tag) && !IsFactionDiscovered(playerId.Value, myFaction.FactionId))
					{
						AddDiscoveredFaction(playerId.Value, myFaction.FactionId, triggerEvent: false);
					}
					if (!MyBankingSystem.Static.TryGetAccountInfo(myFaction.FactionId, out MyAccountInfo _))
					{
						MyBankingSystem.Static.CreateAccount(myFaction.FactionId, item.StartingBalance);
					}
					if (myFaction.FactionType != item.Type)
					{
						myFaction.FactionType = item.Type;
					}
					if (!myFaction.FactionIcon.HasValue && !string.IsNullOrEmpty(item.FactionIcon.String))
					{
						myFaction.FactionIcon = item.FactionIcon;
						myFaction.CustomColor = MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero);
					}
				}
			}
		}

		public MyFaction TryGetFactionByTag(string tag, IMyFaction doNotCheck = null)
		{
			string key = tag.ToUpperInvariant();
			MyFaction value = null;
			m_factionsByTag.TryGetValue(key, out value);
			if (value == null)
			{
				return null;
			}
			if (doNotCheck != null && value.FactionId == doNotCheck.FactionId)
			{
				return null;
			}
			return value;
		}

		private void UnregisterFactionTag(MyFaction faction)
		{
			if (faction != null)
			{
				m_factionsByTag.Remove(faction.Tag.ToUpperInvariant());
			}
		}

		private void RegisterFactionTag(MyFaction faction)
		{
			if (faction != null)
			{
				string key = faction.Tag.ToUpperInvariant();
				MyFaction value = null;
				m_factionsByTag.TryGetValue(key, out value);
				m_factionsByTag[key] = faction;
			}
		}

		public IMyFaction TryGetPlayerFaction(long playerId)
		{
			return GetPlayerFaction(playerId);
		}

		public MyFaction GetPlayerFaction(long playerId)
		{
			MyFaction value = null;
			if (m_playerFaction.TryGetValue(playerId, out long value2))
			{
				m_factions.TryGetValue(value2, out value);
			}
			return value;
		}

		public void AddPlayerToFaction(long playerId, long factionId)
		{
			if (m_factions.TryGetValue(factionId, out MyFaction value))
			{
				value.AcceptJoin(playerId, autoaccept: true);
			}
			else
			{
				AddPlayerToFactionInternal(playerId, factionId);
			}
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				faction.Value.CancelJoinRequest(playerId);
			}
		}

		public void AddNewNPCToFaction(long factionId)
		{
			string npcName = m_factions[factionId].Tag + " NPC" + MyRandom.Instance.Next(1000, 9999);
			AddNewNPCToFaction(factionId, npcName);
		}

		public void AddNewNPCToFaction(long factionId, string npcName)
		{
			MyIdentity myIdentity = Sync.Players.CreateNewIdentity(npcName, null, null, initialPlayer: false, addToNpcs: true);
			AddPlayerToFaction(myIdentity.IdentityId, factionId);
		}

		internal void AddPlayerToFactionInternal(long playerId, long factionId)
		{
			m_playerFaction[playerId] = factionId;
		}

		public void KickPlayerFromFaction(long playerId)
		{
			m_playerFaction.Remove(playerId);
		}

		public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenFactions(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2, MyPerGameSettings.DefaultFactionRelationshipAndReputation);
		}

		public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenFactions(long factionId1, long factionId2, Tuple<MyRelationsBetweenFactions, int> defaultState)
		{
			if (factionId1 == factionId2 && factionId1 != 0L)
			{
				return new Tuple<MyRelationsBetweenFactions, int>(MyRelationsBetweenFactions.Neutral, 0);
			}
			return m_relationsBetweenFactions.GetValueOrDefault(new MyRelatablePair(factionId1, factionId2), defaultState);
		}

		public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenPlayerAndFaction(long playerId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(playerId, factionId, MyPerGameSettings.DefaultFactionRelationshipAndReputation);
		}

		public Tuple<MyRelationsBetweenFactions, int> GetRelationBetweenPlayerAndFaction(long playerId, long factionId, Tuple<MyRelationsBetweenFactions, int> defaultState)
		{
			return m_relationsBetweenPlayersAndFactions.GetValueOrDefault(new MyRelatablePair(playerId, factionId), defaultState);
		}

		public bool AreFactionsEnemies(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item1 == MyRelationsBetweenFactions.Enemies;
		}

		public bool AreFactionsNeutrals(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item1 == MyRelationsBetweenFactions.Neutral;
		}

		public bool AreFactionsFriends(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item1 == MyRelationsBetweenFactions.Friends;
		}

		public bool IsFactionWithPlayerEnemy(long playerId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(playerId, factionId).Item1 == MyRelationsBetweenFactions.Enemies;
		}

		public bool IsFactionWithPlayerNeutral(long playerId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(playerId, factionId).Item1 == MyRelationsBetweenFactions.Neutral;
		}

		public bool IsFactionWithPlayerFriend(long playerId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(playerId, factionId).Item1 == MyRelationsBetweenFactions.Friends;
		}

		public MyFactionPeaceRequestState GetRequestState(long myFactionId, long foreignFactionId)
		{
			if (m_factionRequests.ContainsKey(myFactionId) && m_factionRequests[myFactionId].Contains(foreignFactionId))
			{
				return MyFactionPeaceRequestState.Sent;
			}
			if (m_factionRequests.ContainsKey(foreignFactionId) && m_factionRequests[foreignFactionId].Contains(myFactionId))
			{
				return MyFactionPeaceRequestState.Pending;
			}
			return MyFactionPeaceRequestState.None;
		}

		public bool IsPeaceRequestStateSent(long myFactionId, long foreignFactionId)
		{
			return GetRequestState(myFactionId, foreignFactionId) == MyFactionPeaceRequestState.Sent;
		}

		public bool IsPeaceRequestStatePending(long myFactionId, long foreignFactionId)
		{
			return GetRequestState(myFactionId, foreignFactionId) == MyFactionPeaceRequestState.Pending;
		}

		public static void RemoveFaction(long factionId)
		{
			SendFactionChange(MyFactionStateChange.RemoveFaction, factionId, factionId, 0L);
		}

		public static void SendPeaceRequest(long fromFactionId, long toFactionId)
		{
			SendFactionChange(MyFactionStateChange.SendPeaceRequest, fromFactionId, toFactionId, 0L);
		}

		public static void CancelPeaceRequest(long fromFactionId, long toFactionId)
		{
			SendFactionChange(MyFactionStateChange.CancelPeaceRequest, fromFactionId, toFactionId, 0L);
		}

		public static void AcceptPeace(long fromFactionId, long toFactionId)
		{
			SendFactionChange(MyFactionStateChange.AcceptPeace, fromFactionId, toFactionId, 0L);
		}

		public static void DeclareWar(long fromFactionId, long toFactionId)
		{
			SendFactionChange(MyFactionStateChange.DeclareWar, fromFactionId, toFactionId, 0L);
		}

		public static void SendJoinRequest(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberSendJoin, factionId, factionId, playerId);
		}

		public static void CancelJoinRequest(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberCancelJoin, factionId, factionId, playerId);
		}

		public static void AcceptJoin(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberAcceptJoin, factionId, factionId, playerId);
		}

		public static void KickMember(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberKick, factionId, factionId, playerId);
		}

		public static void PromoteMember(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberPromote, factionId, factionId, playerId);
		}

		public static void DemoteMember(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberDemote, factionId, factionId, playerId);
		}

		public static void MemberLeaves(long factionId, long playerId)
		{
			SendFactionChange(MyFactionStateChange.FactionMemberLeave, factionId, factionId, playerId);
		}

		private bool CheckFactionStateChange(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			if (!m_factions.ContainsKey(fromFactionId) || !m_factions.ContainsKey(toFactionId))
			{
				return false;
			}
			if (senderId != 0L && ((uint)action <= 7u || (uint)(action - 10) <= 3u) && !m_factions[fromFactionId].IsLeader(senderId) && !MySession.Static.IsUserAdmin(MySession.Static.Players.TryGetSteamId(senderId)))
			{
				return false;
			}
			HashSet<long> value;
			switch (action)
			{
			case MyFactionStateChange.RemoveFaction:
				return true;
			case MyFactionStateChange.SendPeaceRequest:
				if (!m_factionRequests.TryGetValue(fromFactionId, out value) || !value.Contains(toFactionId))
				{
					return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Enemies;
				}
				return false;
			case MyFactionStateChange.CancelPeaceRequest:
				if (m_factionRequests.TryGetValue(fromFactionId, out value) && value.Contains(toFactionId))
				{
					return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Enemies;
				}
				return false;
			case MyFactionStateChange.SendFriendRequest:
				if (!m_factionRequests.TryGetValue(fromFactionId, out value) || !value.Contains(toFactionId))
				{
					return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Neutral;
				}
				return false;
			case MyFactionStateChange.CancelFriendRequest:
				if (m_factionRequests.TryGetValue(fromFactionId, out value) && value.Contains(toFactionId))
				{
					return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Neutral;
				}
				return false;
			case MyFactionStateChange.AcceptPeace:
				return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 != MyRelationsBetweenFactions.Neutral;
			case MyFactionStateChange.DeclareWar:
				return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 != MyRelationsBetweenFactions.Enemies;
			case MyFactionStateChange.AcceptFriendRequest:
				return GetRelationBetweenFactions(fromFactionId, toFactionId).Item1 == MyRelationsBetweenFactions.Friends;
			case MyFactionStateChange.FactionMemberSendJoin:
				if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION)
				{
					return !m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
				}
				if (!m_factions[fromFactionId].IsMember(playerId))
				{
					return !m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
				}
				return false;
			case MyFactionStateChange.FactionMemberCancelJoin:
				if (!m_factions[fromFactionId].IsMember(playerId))
				{
					return m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
				}
				return false;
			case MyFactionStateChange.FactionMemberAcceptJoin:
				return m_factions[fromFactionId].JoinRequests.ContainsKey(playerId);
			case MyFactionStateChange.FactionMemberKick:
				return m_factions[fromFactionId].IsMember(playerId);
			case MyFactionStateChange.FactionMemberPromote:
				return m_factions[fromFactionId].IsMember(playerId);
			case MyFactionStateChange.FactionMemberDemote:
				return m_factions[fromFactionId].IsLeader(playerId);
			case MyFactionStateChange.FactionMemberLeave:
				return m_factions[fromFactionId].IsMember(playerId);
			default:
				return false;
			}
		}

		private void ApplyFactionStateChange(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			switch (action)
			{
			case MyFactionStateChange.RemoveFaction:
			{
				if (m_factions[fromFactionId].IsMember(MySession.Static.LocalPlayerId))
				{
					m_playerFaction.Remove(playerId);
				}
				foreach (KeyValuePair<long, MyFaction> faction in m_factions)
				{
					if (faction.Key != fromFactionId)
					{
						ClearRequest(fromFactionId, faction.Key);
						RemoveRelation(fromFactionId, faction.Key);
					}
				}
				MyFaction value2 = null;
				m_factions.TryGetValue(fromFactionId, out value2);
				UnregisterFactionTag(value2);
				m_factions.Remove(fromFactionId);
				break;
			}
			case MyFactionStateChange.SendPeaceRequest:
			case MyFactionStateChange.SendFriendRequest:
			{
				if (m_factionRequests.TryGetValue(fromFactionId, out HashSet<long> value))
				{
					value.Add(toFactionId);
					break;
				}
				value = new HashSet<long>();
				value.Add(toFactionId);
				m_factionRequests.Add(fromFactionId, value);
				break;
			}
			case MyFactionStateChange.CancelPeaceRequest:
			case MyFactionStateChange.CancelFriendRequest:
				ClearRequest(fromFactionId, toFactionId);
				break;
			case MyFactionStateChange.AcceptFriendRequest:
				ClearRequest(fromFactionId, toFactionId);
				ChangeFactionRelation(fromFactionId, toFactionId, MyRelationsBetweenFactions.Friends);
				break;
			case MyFactionStateChange.AcceptPeace:
				ClearRequest(fromFactionId, toFactionId);
				ChangeFactionRelation(fromFactionId, toFactionId, MyRelationsBetweenFactions.Neutral);
				break;
			case MyFactionStateChange.DeclareWar:
				ClearRequest(fromFactionId, toFactionId);
				ChangeFactionRelation(fromFactionId, toFactionId, MyRelationsBetweenFactions.Enemies);
				break;
			case MyFactionStateChange.FactionMemberSendJoin:
				m_factions[fromFactionId].AddJoinRequest(playerId);
				break;
			case MyFactionStateChange.FactionMemberCancelJoin:
				m_factions[fromFactionId].CancelJoinRequest(playerId);
				break;
			case MyFactionStateChange.FactionMemberAcceptJoin:
			{
				ulong steamId = MySession.Static.Players.TryGetSteamId(senderId);
				bool flag = MySession.Static.IsUserSpaceMaster(steamId) || m_factions[fromFactionId].Members.Count == 0;
				if (flag && m_factions[fromFactionId].IsEveryoneNpc())
				{
					m_factions[fromFactionId].AcceptJoin(playerId, flag);
					m_factions[fromFactionId].PromoteMember(playerId);
				}
				else
				{
					m_factions[fromFactionId].AcceptJoin(playerId, flag);
				}
				break;
			}
			case MyFactionStateChange.FactionMemberLeave:
				m_factions[fromFactionId].KickMember(playerId);
				break;
			case MyFactionStateChange.FactionMemberKick:
				if (Sync.IsServer && playerId != m_factions[fromFactionId].FounderId)
				{
					MyBlockLimits.TransferBlockLimits(playerId, m_factions[fromFactionId].FounderId);
				}
				m_factions[fromFactionId].KickMember(playerId);
				break;
			case MyFactionStateChange.FactionMemberPromote:
				m_factions[fromFactionId].PromoteMember(playerId);
				break;
			case MyFactionStateChange.FactionMemberDemote:
				m_factions[fromFactionId].DemoteMember(playerId);
				break;
			}
		}

		private void ClearRequest(long fromFactionId, long toFactionId)
		{
			if (m_factionRequests.ContainsKey(fromFactionId))
			{
				m_factionRequests[fromFactionId].Remove(toFactionId);
			}
			if (m_factionRequests.ContainsKey(toFactionId))
			{
				m_factionRequests[toFactionId].Remove(fromFactionId);
			}
		}

		private void ChangeFactionRelation(long fromFactionId, long toFactionId, MyRelationsBetweenFactions relation)
		{
			int num = TranslateRelationToReputation(relation);
			m_relationsBetweenFactions[new MyRelatablePair(fromFactionId, toFactionId)] = new Tuple<MyRelationsBetweenFactions, int>(relation, num);
			foreach (KeyValuePair<long, MyFactionMember> member in TryGetFactionById(fromFactionId).Members)
			{
				SetReputationBetweenPlayerAndFaction(member.Key, toFactionId, num);
			}
			foreach (KeyValuePair<long, MyFactionMember> member2 in TryGetFactionById(toFactionId).Members)
			{
				SetReputationBetweenPlayerAndFaction(member2.Key, fromFactionId, num);
			}
		}

		private void ChangeReputationBetweenFactions(long fromFactionId, long toFactionId, int reputation)
		{
			m_relationsBetweenFactions[new MyRelatablePair(fromFactionId, toFactionId)] = new Tuple<MyRelationsBetweenFactions, int>(TranslateReputationToRelation(reputation), reputation);
		}

		public void SetReputationBetweenFactions(long fromFactionId, long toFactionId, int reputation)
		{
			ChangeReputationBetweenFactions(fromFactionId, toFactionId, reputation);
		}

		public void SetReputationBetweenPlayerAndFaction(long identityId, long factionId, int reputation)
		{
			ChangeReputationWithPlayer(identityId, factionId, reputation);
		}

		public DictionaryReader<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> GetAllFactionRelations()
		{
			return new DictionaryReader<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>>(m_relationsBetweenFactions);
		}

		public int TranslateRelationToReputation(MyRelationsBetweenFactions relation)
		{
			return (MySession.Static?.GetComponent<MySessionComponentEconomy>())?.TranslateRelationToReputation(relation) ?? MyPerGameSettings.DefaultFactionReputation;
		}

		public MyRelationsBetweenFactions TranslateReputationToRelation(int reputation)
		{
			return (MySession.Static?.GetComponent<MySessionComponentEconomy>())?.TranslateReputationToRelationship(reputation) ?? MyPerGameSettings.DefaultFactionRelationship;
		}

		public int ClampReputation(int reputation)
		{
			return (MySession.Static?.GetComponent<MySessionComponentEconomy>())?.ClampReputation(reputation) ?? reputation;
		}

		private void ChangeRelationWithPlayer(long fromPlayerId, long toFactionId, MyRelationsBetweenFactions relation)
		{
			m_relationsBetweenPlayersAndFactions[new MyRelatablePair(fromPlayerId, toFactionId)] = new Tuple<MyRelationsBetweenFactions, int>(relation, TranslateRelationToReputation(relation));
		}

		private void ChangeReputationWithPlayer(long fromPlayerId, long toFactionId, int reputation)
		{
			MyRelatablePair key = new MyRelatablePair(fromPlayerId, toFactionId);
			Tuple<MyRelationsBetweenFactions, int> tuple = new Tuple<MyRelationsBetweenFactions, int>(TranslateReputationToRelation(reputation), reputation);
			if (m_relationsBetweenPlayersAndFactions.ContainsKey(key))
			{
				Tuple<MyRelationsBetweenFactions, int> tuple2 = m_relationsBetweenPlayersAndFactions[key];
				m_relationsBetweenPlayersAndFactions[key] = tuple;
				if (tuple2.Item1 != tuple.Item1)
				{
					PlayerReputationLevelChanged(fromPlayerId, toFactionId, tuple2.Item1, tuple.Item1);
				}
			}
			else
			{
				m_relationsBetweenPlayersAndFactions[key] = tuple;
			}
		}

		private void PlayerReputationLevelChanged(long fromPlayerId, long toFactionId, MyRelationsBetweenFactions oldRel, MyRelationsBetweenFactions newRel)
		{
			CheckPlayerReputationAchievements(fromPlayerId, toFactionId, oldRel, newRel);
		}

		private void CheckPlayerReputationAchievements(long fromPlayerId, long toFactionId, MyRelationsBetweenFactions oldRel, MyRelationsBetweenFactions newRel)
		{
			if (newRel != MyRelationsBetweenFactions.Friends || oldRel != 0)
			{
				return;
			}
			int num = 0;
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, MyFaction> current = enumerator.Current;
					MyRelatablePair key = new MyRelatablePair(fromPlayerId, current.Key);
					if (current.Value.FactionType != MyFactionTypes.PlayerMade && m_relationsBetweenPlayersAndFactions.ContainsKey(key) && m_relationsBetweenPlayersAndFactions[key].Item1 == MyRelationsBetweenFactions.Friends)
					{
						num++;
					}
				}
			}
			if (num == 3)
			{
				if (Sync.IsServer)
				{
					ulong value = MySession.Static.Players.TryGetSteamId(fromPlayerId);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UnlockAchievementForClient, "FriendOfFactions", new EndpointId(value));
				}
				else
				{
					UnlockAchievement_Internal("FriendOfFactions");
				}
			}
		}

		[Event(null, 926)]
		[Reliable]
		[Client]
		private static void UnlockAchievementForClient(string achievement)
		{
			UnlockAchievement_Internal(achievement);
		}

		private static void UnlockAchievement_Internal(string achievement)
		{
			MyGameService.GetAchievement(achievement, null, 0f).Unlock();
		}

		public bool HasRelationWithPlayer(long fromPlayerId, long toFactionId)
		{
			return m_relationsBetweenPlayersAndFactions.ContainsKey(new MyRelatablePair(fromPlayerId, toFactionId));
		}

		private void RemoveRelation(long fromFactionId, long toFactionId)
		{
			m_relationsBetweenFactions.Remove(new MyRelatablePair(fromFactionId, toFactionId));
		}

		public bool AddFactionPlayerReputation(long playerIdentityId, long factionId, int delta, bool propagate = true, bool adminChange = false)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			List<MyReputationChangeWrapper> list = GenerateChanges(playerIdentityId, factionId, delta, propagate, adminChange);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AddFactionPlayerReputationSuccess, playerIdentityId, list);
			AddFactionPlayerReputationSuccess(playerIdentityId, list);
			return true;
		}

		public void DamageFactionPlayerReputation(long playerIdentityId, long attackedIdentityId, MyReputationDamageType repDamageType)
		{
			if (!Sync.IsServer || attackedIdentityId == 0L)
			{
				return;
			}
			if (MySession.Static == null || MySession.Static.Factions == null)
			{
				MyLog.Default.Error("Session.Static or MySession.Static.Factions is null. Should not happen!");
				return;
			}
			long piratesId = MyPirateAntennas.GetPiratesId();
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(piratesId);
			MyFaction myFaction = TryGetPlayerFaction(attackedIdentityId) as MyFaction;
			if (myFaction == null && playerFaction != null)
			{
				int reputationDamageDelta = GetReputationDamageDelta(repDamageType, isPirates: true);
				AddFactionPlayerReputation(playerIdentityId, playerFaction.FactionId, reputationDamageDelta, propagate: false);
			}
			else if (myFaction != null && !myFaction.IsMember(playerIdentityId))
			{
				int reputationDamageDelta2 = GetReputationDamageDelta(repDamageType, playerFaction == myFaction);
				AddFactionPlayerReputation(playerIdentityId, myFaction.FactionId, -reputationDamageDelta2, propagate: false);
				if (playerFaction != null && myFaction != playerFaction)
				{
					AddFactionPlayerReputation(playerIdentityId, playerFaction.FactionId, reputationDamageDelta2, propagate: false);
				}
			}
		}

		private int GetReputationDamageDelta(MyReputationDamageType repDamageType, bool isPirates = false)
		{
			MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings myReputationDamageSettings = isPirates ? m_reputationSettings.PirateDamageSettings : m_reputationSettings.DamageSettings;
			int result = 0;
			switch (repDamageType)
			{
			case MyReputationDamageType.GrindingWelding:
				result = myReputationDamageSettings.GrindingWelding;
				break;
			case MyReputationDamageType.Damaging:
				result = myReputationDamageSettings.Damaging;
				break;
			case MyReputationDamageType.Stealing:
				result = myReputationDamageSettings.Stealing;
				break;
			case MyReputationDamageType.Killing:
				result = myReputationDamageSettings.Killing;
				break;
			default:
				MyLog.Default.Error("Reputation damage type not handled. Check and update.");
				break;
			}
			return result;
		}

		[Event(null, 1062)]
		[Reliable]
		[Server]
		public static void Invoke_AddRep_DEBUG(long playerId, long factionId, int delta)
		{
		}

		private List<MyReputationChangeWrapper> GenerateChanges(long playerId, long factionId, int delta, bool propagate, bool adminChange = false)
		{
			List<MyReputationChangeWrapper> list = new List<MyReputationChangeWrapper>();
			MyReputationModifiers reputationModifiers = GetReputationModifiers();
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			if (myFaction != null)
			{
				Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = GetRelationBetweenPlayerAndFaction(playerId, factionId);
				int num = ClampReputation(relationBetweenPlayerAndFaction.Item2 + (int)(reputationModifiers.Owner * (float)delta));
				int num2 = num - relationBetweenPlayerAndFaction.Item2;
				bool flag = !adminChange && CheckIfMaxPirateRep(playerId, myFaction, num2);
				MyReputationChangeWrapper item;
				if (!flag)
				{
					bool showNotification = !MyCampaignManager.Static.IsCampaignRunning;
					item = new MyReputationChangeWrapper
					{
						FactionId = factionId,
						RepTotal = num,
						Change = num2,
						ShowNotification = showNotification
					};
					list.Add(item);
				}
				if (!propagate || flag)
				{
					return list;
				}
				using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<long, MyFaction> current = enumerator.Current;
						if (current.Value.FactionId != factionId && current.Value.FactionType != 0 && current.Value.FactionType != MyFactionTypes.PlayerMade)
						{
							Tuple<MyRelationsBetweenFactions, int> relationBetweenFactions = GetRelationBetweenFactions(factionId, current.Value.FactionId);
							int num3 = 0;
							switch (relationBetweenFactions.Item1)
							{
							case MyRelationsBetweenFactions.Neutral:
								num3 = (int)(reputationModifiers.Neutral * (float)delta);
								break;
							case MyRelationsBetweenFactions.Enemies:
								num3 = (int)(reputationModifiers.Hostile * (float)delta);
								break;
							case MyRelationsBetweenFactions.Friends:
								num3 = (int)(reputationModifiers.Friend * (float)delta);
								break;
							default:
								continue;
							}
							relationBetweenPlayerAndFaction = GetRelationBetweenPlayerAndFaction(playerId, current.Value.FactionId);
							num = ClampReputation(relationBetweenPlayerAndFaction.Item2 + num3);
							num2 = num - relationBetweenPlayerAndFaction.Item2;
							bool flag2 = !adminChange && CheckIfMaxPirateRep(playerId, current.Value, num2);
							if (num2 != 0 && !flag2)
							{
								item = new MyReputationChangeWrapper
								{
									FactionId = current.Value.FactionId,
									RepTotal = num,
									Change = num2,
									ShowNotification = false
								};
								list.Add(item);
							}
						}
					}
					return list;
				}
			}
			return list;
		}

		private bool CheckIfMaxPirateRep(long playerId, IMyFaction faction, int clampedDelta)
		{
			long piratesId = MyPirateAntennas.GetPiratesId();
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(piratesId);
			if (clampedDelta > 0 && faction == playerFaction)
			{
				if (!m_playerToReputationLimits.TryGetValue(playerId, out Tuple<int, TimeSpan> value))
				{
					value = new Tuple<int, TimeSpan>(clampedDelta, MySession.Static.ElapsedGameTime + TimeSpan.FromMinutes(m_reputationSettings.ResetTimeMinForRepGain));
					m_playerToReputationLimits.Add(playerId, value);
					return false;
				}
				if (value.Item2 > MySession.Static.ElapsedGameTime)
				{
					if (value.Item1 < m_reputationSettings.MaxReputationGainInTime)
					{
						int item = Math.Min(value.Item1 + clampedDelta, m_reputationSettings.MaxReputationGainInTime);
						m_playerToReputationLimits[playerId] = new Tuple<int, TimeSpan>(item, value.Item2);
						return false;
					}
					return true;
				}
				int item2 = Math.Min(clampedDelta, m_reputationSettings.MaxReputationGainInTime);
				m_playerToReputationLimits[playerId] = new Tuple<int, TimeSpan>(item2, MySession.Static.ElapsedGameTime + TimeSpan.FromMinutes(m_reputationSettings.ResetTimeMinForRepGain));
				return false;
			}
			return false;
		}

		private MyReputationModifiers GetReputationModifiers(bool positive = true)
		{
			return MySession.Static.GetComponent<MySessionComponentEconomy>()?.GetReputationModifiers(positive) ?? default(MyReputationModifiers);
		}

		[Event(null, 1187)]
		[Reliable]
		[Broadcast]
		private static void AddFactionPlayerReputationSuccess(long playerId, List<MyReputationChangeWrapper> changes)
		{
			MyFactionCollection factions = MySession.Static.Factions;
			bool flag = !Sandbox.Engine.Platform.Game.IsDedicated && playerId == MySession.Static.LocalPlayerId;
			foreach (MyReputationChangeWrapper change in changes)
			{
				factions.ChangeReputationWithPlayer(playerId, change.FactionId, change.RepTotal);
				if (change.ShowNotification && flag)
				{
					MyFaction myFaction = MySession.Static.Factions.TryGetFactionById(change.FactionId) as MyFaction;
					MySession.Static.Factions.DisplayReputationChangeNotification(myFaction.Tag, change.Change);
				}
			}
		}

		private void DisplayReputationChangeNotification(string tag, int value)
		{
			if (value > 0)
			{
				m_notificationRepInc.UpdateReputationNotification(in tag, in value);
			}
			else if (value < 0)
			{
				m_notificationRepDec.UpdateReputationNotification(in tag, in value);
			}
		}

		private static void SendFactionChange(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => FactionStateChangeRequest, action, fromFactionId, toFactionId, playerId);
		}

		[Event(null, 1222)]
		[Reliable]
		[Server]
		private static void FactionStateChangeRequest(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(fromFactionId);
			IMyFaction myFaction2 = MySession.Static.Factions.TryGetFactionById(toFactionId);
			long num = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (myFaction == null || myFaction2 == null || !MySession.Static.Factions.CheckFactionStateChange(action, fromFactionId, toFactionId, playerId, num))
			{
				return;
			}
			if ((action == MyFactionStateChange.FactionMemberKick || action == MyFactionStateChange.FactionMemberLeave) && myFaction.Members.Count == 1 && MySession.Static.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION)
			{
				action = MyFactionStateChange.RemoveFaction;
			}
			else
			{
				switch (action)
				{
				case MyFactionStateChange.FactionMemberSendJoin:
				{
					ulong num2 = MySession.Static.Players.TryGetSteamId(playerId);
					bool flag = MySession.Static.IsUserSpaceMaster(num2);
					if (myFaction2.AutoAcceptMember || myFaction2.Members.Count == 0)
					{
						flag = true;
						if (!myFaction2.AcceptHumans && num2 != 0L && MySession.Static.Players.TryGetSerialId(playerId) == 0)
						{
							flag = false;
							action = MyFactionStateChange.FactionMemberCancelJoin;
						}
					}
					if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && !MyBlockLimits.IsFactionChangePossible(playerId, myFaction2.FactionId))
					{
						flag = false;
						action = MyFactionStateChange.FactionMemberNotPossibleJoin;
					}
					if (flag)
					{
						action = MyFactionStateChange.FactionMemberAcceptJoin;
					}
					break;
				}
				case MyFactionStateChange.FactionMemberAcceptJoin:
					if (!MyBlockLimits.IsFactionChangePossible(playerId, myFaction2.FactionId))
					{
						action = MyFactionStateChange.FactionMemberNotPossibleJoin;
					}
					break;
				case MyFactionStateChange.SendPeaceRequest:
					if (myFaction2.AutoAcceptPeace)
					{
						action = MyFactionStateChange.AcceptPeace;
						num = 0L;
					}
					break;
				}
			}
			if (action == MyFactionStateChange.RemoveFaction)
			{
				MyBankingSystem.Static.RemoveAccount(toFactionId);
			}
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => FactionStateChangeSuccess, action, fromFactionId, toFactionId, playerId, num);
			FactionStateChangeSuccess(action, fromFactionId, toFactionId, playerId, num);
		}

		[Event(null, 1298)]
		[Reliable]
		[Broadcast]
		private static void FactionStateChangeSuccess(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(fromFactionId);
			IMyFaction myFaction2 = MySession.Static.Factions.TryGetFactionById(toFactionId);
			if (myFaction != null && myFaction2 != null)
			{
				MySession.Static.Factions.ApplyFactionStateChange(action, fromFactionId, toFactionId, playerId, senderId);
				MySession.Static.Factions.FactionStateChanged?.Invoke(action, fromFactionId, toFactionId, playerId, senderId);
			}
		}

		internal List<MyObjectBuilder_Faction> SaveFactions()
		{
			List<MyObjectBuilder_Faction> list = new List<MyObjectBuilder_Faction>();
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				MyObjectBuilder_Faction objectBuilder = faction.Value.GetObjectBuilder();
				list.Add(objectBuilder);
			}
			return list;
		}

		internal void LoadFactions(List<MyObjectBuilder_Faction> factionBuilders, bool removeOldData = true)
		{
			if (removeOldData)
			{
				m_factions.Clear();
				m_factionRequests.Clear();
				m_relationsBetweenFactions.Clear();
				m_playerFaction.Clear();
				m_factionsByTag.Clear();
			}
			if (factionBuilders != null)
			{
				foreach (MyObjectBuilder_Faction factionBuilder in factionBuilders)
				{
					if (!m_factions.ContainsKey(factionBuilder.FactionId))
					{
						MyFaction myFaction = new MyFaction(factionBuilder);
						Add(myFaction);
						foreach (KeyValuePair<long, MyFactionMember> member in myFaction.Members)
						{
							AddPlayerToFaction(member.Value.PlayerId, myFaction.FactionId);
						}
					}
				}
			}
		}

		public void InvokePlayerJoined(MyFaction faction, long identityId)
		{
			this.OnPlayerJoined.InvokeIfNotNull(faction, identityId);
		}

		public void InvokePlayerLeft(MyFaction faction, long identityId)
		{
			this.OnPlayerLeft.InvokeIfNotNull(faction, identityId);
		}

		public MyStation GetStationByGridId(long gridEntityId)
		{
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (MyStation station in enumerator.Current.Value.Stations)
					{
						if (station.StationEntityId == gridEntityId)
						{
							return station;
						}
					}
				}
			}
			return null;
		}

		internal MyStation GetStationByStationId(long stationId)
		{
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MyStation stationById = enumerator.Current.Value.GetStationById(stationId);
					if (stationById != null)
					{
						return stationById;
					}
				}
			}
			return null;
		}

		public void ChangeAutoAccept(long factionId, long playerId, bool autoAcceptMember, bool autoAcceptPeace)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ChangeAutoAcceptRequest, factionId, playerId, autoAcceptMember, autoAcceptPeace);
		}

		[Event(null, 1420)]
		[Reliable]
		[Server]
		private static void ChangeAutoAcceptRequest(long factionId, long playerId, bool autoAcceptMember, bool autoAcceptPeace)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(factionId);
			ulong num = MySession.Static.Players.TryGetSteamId(playerId);
			if (myFaction != null && (myFaction.IsLeader(playerId) || (num != 0L && MySession.Static.IsUserAdmin(num))))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ChangeAutoAcceptSuccess, factionId, autoAcceptMember, autoAcceptPeace);
			}
			else if (!MyEventContext.Current.IsLocallyInvoked)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}

		[Event(null, 1436)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void ChangeAutoAcceptSuccess(long factionId, bool autoAcceptMember, bool autoAcceptPeace)
		{
			MySession.Static.Factions[factionId].AutoAcceptMember = autoAcceptMember;
			MySession.Static.Factions[factionId].AutoAcceptPeace = autoAcceptPeace;
			MySession.Static.Factions.FactionAutoAcceptChanged?.Invoke(factionId, autoAcceptMember, autoAcceptPeace);
		}

		public void EditFaction(long factionId, string tag, string name, string desc, string privateInfo, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0, Vector3 factionColor = default(Vector3), Vector3 factionIconColor = default(Vector3))
		{
			AddFactionMsg addFactionMsg = default(AddFactionMsg);
			addFactionMsg.FactionId = factionId;
			addFactionMsg.FactionTag = tag;
			addFactionMsg.FactionName = name;
			addFactionMsg.FactionDescription = (string.IsNullOrEmpty(desc) ? string.Empty : ((desc.Length > 512) ? desc.Substring(0, 512) : desc));
			addFactionMsg.FactionPrivateInfo = (string.IsNullOrEmpty(privateInfo) ? string.Empty : ((privateInfo.Length > 512) ? privateInfo.Substring(0, 512) : privateInfo));
			addFactionMsg.FactionColor = factionColor;
			addFactionMsg.FactionIconColor = factionIconColor;
			addFactionMsg.FactionIconGroupId = factionIconGroupId;
			addFactionMsg.FactionIconId = factionIconId;
			AddFactionMsg arg = addFactionMsg;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => EditFactionRequest, arg);
		}

		[Event(null, 1472)]
		[Reliable]
		[Server]
		private static void EditFactionRequest(AddFactionMsg msgEdit)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(msgEdit.FactionId);
			long playerId = MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value);
			if (myFaction != null && !MySession.Static.Factions.FactionTagExists(msgEdit.FactionTag, myFaction) && !MySession.Static.Factions.FactionNameExists(msgEdit.FactionName, myFaction) && (myFaction.IsLeader(playerId) || MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value)))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => EditFactionSuccess, msgEdit);
			}
			else if (!MyEventContext.Current.IsLocallyInvoked)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}

		[Event(null, 1488)]
		[Reliable]
		[ServerInvoked]
		[Broadcast]
		private static void EditFactionSuccess(AddFactionMsg msgEdit)
		{
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionById(msgEdit.FactionId) as MyFaction;
			if (myFaction != null)
			{
				MySession.Static.Factions.UnregisterFactionTag(myFaction);
				string str = string.Empty;
				if (msgEdit.FactionIconGroupId.HasValue)
				{
					str = GetFactionIcon(msgEdit.FactionIconGroupId.Value, msgEdit.FactionIconId);
				}
				myFaction.Tag = msgEdit.FactionTag;
				myFaction.Name = msgEdit.FactionName;
				myFaction.Description = msgEdit.FactionDescription;
				myFaction.PrivateInfo = msgEdit.FactionPrivateInfo;
				myFaction.FactionIcon = MyStringId.GetOrCompute(str);
				myFaction.CustomColor = msgEdit.FactionColor;
				myFaction.IconColor = msgEdit.FactionIconColor;
				MySession.Static.Factions.RegisterFactionTag(myFaction);
				MySession.Static.Factions.FactionEdited?.Invoke(msgEdit.FactionId);
			}
		}

		public void CreateFaction(long founderId, string tag, string name, string desc, string privateInfo, MyFactionTypes type, Vector3 factionColor = default(Vector3), Vector3 factionIconColor = default(Vector3), SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0)
		{
			SendCreateFaction(founderId, tag, name, desc, privateInfo, type, factionColor, factionIconColor, factionIconGroupId, factionIconId);
		}

		public void CreateNPCFaction(string tag, string name, string desc, string privateInfo)
		{
			string name2 = tag + " NPC" + MyRandom.Instance.Next(1000, 9999);
			MyIdentity myIdentity = Sync.Players.CreateNewIdentity(name2);
			Sync.Players.MarkIdentityAsNPC(myIdentity.IdentityId);
			SendCreateFaction(myIdentity.IdentityId, tag, name, desc, privateInfo, MyFactionTypes.None, default(Vector3), default(Vector3));
		}

		private void Add(MyFaction faction)
		{
			m_factions.Add(faction.FactionId, faction);
			RegisterFactionTag(faction);
		}

		private void SendCreateFaction(long founderId, string factionTag, string factionName, string factionDesc, string factionPrivate, MyFactionTypes type, Vector3 factionColor, Vector3 factionIconColor, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0)
		{
			AddFactionMsg arg = default(AddFactionMsg);
			arg.FounderId = founderId;
			arg.FactionTag = factionTag;
			arg.FactionName = factionName;
			arg.FactionDescription = factionDesc;
			arg.FactionPrivateInfo = factionPrivate;
			arg.Type = type;
			arg.FactionColor = factionColor;
			arg.FactionIconColor = factionIconColor;
			arg.FactionIconGroupId = factionIconGroupId;
			arg.FactionIconId = factionIconId;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => CreateFactionRequest, arg);
		}

		[Event(null, 1561)]
		[Reliable]
		[Server]
		private static void CreateFactionRequest(AddFactionMsg msg)
		{
			if (MySession.Static.MaxFactionsCount == 0 || (MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() < MySession.Static.MaxFactionsCount))
			{
				CreateFactionServer(msg.FounderId, msg.FactionTag, msg.FactionName, msg.FactionDescription, msg.FactionPrivateInfo, null, msg.Type, msg.FactionIconGroupId, msg.FactionIconId, msg.FactionColor, msg.FactionIconColor);
			}
			else if (!MyEventContext.Current.IsLocallyInvoked)
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
		}

		private static void CreateFactionServer(long founderId, string factionTag, string factionName, string description, string privateInfo, MyFactionDefinition factionDef = null, MyFactionTypes type = MyFactionTypes.None, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0, Vector3 factionColor = default(Vector3), Vector3 factionIconColor = default(Vector3))
		{
			if (!Sync.IsServer)
			{
				return;
			}
			long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.FACTION);
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(num);
			if (MySession.Static.Factions.TryGetPlayerFaction(founderId) == null && myFaction == null && !MySession.Static.Factions.FactionTagExists(factionTag) && !MySession.Static.Factions.FactionNameExists(factionName) && Sync.Players.HasIdentity(founderId))
			{
				bool flag = (factionDef != null) ? true : false;
				bool flag2 = false;
				if ((!flag) ? CreateFactionInternal(founderId, num, factionTag, factionName, description, privateInfo, type, factionColor, factionIconColor, factionIconGroupId, factionIconId) : CreateFactionInternal(founderId, num, factionDef))
				{
					MyBankingSystem.Static.CreateAccount(num, 0L);
					FactionCreationFinished(num, founderId, factionTag, factionName, description, privateInfo, flag, type, factionIconGroupId, factionIconId, factionColor, factionIconColor);
				}
			}
		}

		public static bool GetDefinitionIdsByIconName(string iconName, out SerializableDefinitionId? factionIconGroupId, out int factionIconId)
		{
			factionIconGroupId = null;
			factionIconId = 0;
			IEnumerable<MyFactionIconsDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MyFactionIconsDefinition>();
			if (allDefinitions == null)
			{
				return false;
			}
			int num = 0;
			foreach (MyFactionIconsDefinition item in allDefinitions)
			{
				num = 0;
				string[] icons = item.Icons;
				foreach (string b in icons)
				{
					if (string.Equals(iconName, b))
					{
						factionIconGroupId = item.Id;
						factionIconId = num;
						return true;
					}
					num++;
				}
			}
			return false;
		}

		public static bool CanPlayerUseFactionIcon(SerializableDefinitionId factionIconGroupId, int factionIconId, long identityId, out string icon)
		{
			ulong num = MySession.Static.Players.TryGetSteamId(identityId);
			if (num == 0L)
			{
				icon = string.Empty;
				return false;
			}
			return CanPlayerUseFactionIcon(factionIconGroupId, factionIconId, num, out icon);
		}

		public static bool CanPlayerUseFactionIcon(SerializableDefinitionId factionIconGroupId, int factionIconId, ulong steamId, out string icon)
		{
			MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(factionIconGroupId);
			if (definition != null && definition.Icons.Count() > factionIconId)
			{
				if (!(definition.Id.SubtypeId.String == "Other"))
				{
					icon = definition.Icons[factionIconId];
					return true;
				}
				if (MySession.Static.GetComponent<MySessionComponentDLC>().HasDLC(MyDLCs.MyDLC.EconomyExpansion.Name, steamId))
				{
					icon = definition.Icons[factionIconId];
					return true;
				}
			}
			icon = "";
			return false;
		}

		public static string GetFactionIcon(SerializableDefinitionId factionIconGroupId, int factionIconId)
		{
			if (factionIconGroupId.IsNull())
			{
				return string.Empty;
			}
			MyDefinitionBase definition = MyDefinitionManager.Static.GetDefinition(factionIconGroupId);
			if (definition != null && definition.Icons.Count() > factionIconId)
			{
				return definition.Icons[factionIconId];
			}
			return string.Empty;
		}

		public static void FactionCreationFinished(long factionId, long founderId, string factionTag, string factionName, string description, string privateInfo, bool createFromDef = false, MyFactionTypes type = MyFactionTypes.None, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0, Vector3 factionColor = default(Vector3), Vector3 factionIconColor = default(Vector3))
		{
			AddFactionMsg arg = default(AddFactionMsg);
			arg.FactionId = factionId;
			arg.FounderId = founderId;
			arg.FactionTag = factionTag;
			arg.FactionName = factionName;
			arg.FactionDescription = description;
			arg.FactionPrivateInfo = privateInfo;
			arg.CreateFromDefinition = createFromDef;
			arg.FactionColor = factionColor;
			arg.FactionIconColor = factionIconColor;
			arg.Type = type;
			arg.FactionIconGroupId = factionIconGroupId;
			arg.FactionIconId = factionIconId;
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => CreateFactionSuccess, arg);
			SetDefaultFactionStates(factionId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetDefaultFactionStates, factionId);
		}

		[Event(null, 1717)]
		[Reliable]
		[Broadcast]
		private static void CreateFactionSuccess(AddFactionMsg msg)
		{
			if (msg.CreateFromDefinition)
			{
				MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(msg.FactionTag);
				if (myFactionDefinition != null)
				{
					CreateFactionInternal(msg.FounderId, msg.FactionId, myFactionDefinition);
				}
			}
			else
			{
				CreateFactionInternal(msg.FounderId, msg.FactionId, msg.FactionTag, msg.FactionName, msg.FactionDescription, msg.FactionPrivateInfo, msg.Type, msg.FactionColor, msg.FactionIconColor, msg.FactionIconGroupId, msg.FactionIconId);
			}
		}

		public static bool CreateFactionInternal(long founderId, long factionId, MyFactionDefinition factionDef, Vector3? customColor = null, Vector3? iconColor = null)
		{
			if (MySession.Static.Factions.Contains(factionId))
			{
				return false;
			}
			if (MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() >= MySession.Static.MaxFactionsCount)
			{
				return false;
			}
			MyFaction faction = new MyFaction(factionId, founderId, "", factionDef, customColor, iconColor);
			MySession.Static.Factions.Add(faction);
			MySession.Static.Factions.AddPlayerToFaction(founderId, factionId);
			AfterFactionCreated(founderId, factionId);
			return true;
		}

		private static bool CreateFactionInternal(long founderId, long factionId, string factionTag, string factionName, string factionDescription, string factionPrivateInfo, MyFactionTypes type, Vector3 factionColor, Vector3? factionIconColor = null, SerializableDefinitionId? factionIconGroupId = null, int factionIconId = 0)
		{
			if (MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() >= MySession.Static.MaxFactionsCount)
			{
				return false;
			}
			string icon = string.Empty;
			if (factionIconGroupId.HasValue)
			{
				if (Sync.IsServer)
				{
					if (!CanPlayerUseFactionIcon(factionIconGroupId.Value, factionIconId, founderId, out icon))
					{
						return false;
					}
				}
				else
				{
					icon = GetFactionIcon(factionIconGroupId.Value, factionIconId);
				}
			}
			MySession.Static.Factions.AddPlayerToFaction(founderId, factionId);
			MySession.Static.Factions.Add(new MyFaction(factionId, factionTag, factionName, factionDescription, factionPrivateInfo, founderId, type, factionColor, factionIconColor, icon));
			AfterFactionCreated(founderId, factionId);
			return true;
		}

		private static MyFactionStateChange DetermineRequestFromRelation(MyRelationsBetweenFactions relation)
		{
			switch (relation)
			{
			case MyRelationsBetweenFactions.Enemies:
				return MyFactionStateChange.DeclareWar;
			case MyRelationsBetweenFactions.Friends:
				return MyFactionStateChange.SendFriendRequest;
			default:
				return MyFactionStateChange.SendPeaceRequest;
			}
		}

		private static void AfterFactionCreated(long founderId, long factionId)
		{
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				faction.Value.CancelJoinRequest(founderId);
			}
			MySession.Static.Factions.FactionCreated?.Invoke(factionId);
		}

		[Event(null, 1835)]
		[Reliable]
		[Broadcast]
		private static void SetDefaultFactionStates(long recivedFactionId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(recivedFactionId);
			MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(myFaction.Tag);
			MyFaction myFaction2 = myFaction as MyFaction;
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				MyFaction value = faction.Value;
				if (value.FactionId != recivedFactionId)
				{
					if (ShouldForceRelationToFactions(value, myFaction2))
					{
						MySession.Static.Factions.ForceRelationToFactions(value, myFaction2);
					}
					else if (myFactionDefinition != null)
					{
						SetDefaultFactionStateInternal(value.FactionId, myFaction, myFactionDefinition);
					}
					else
					{
						MyFactionDefinition myFactionDefinition2 = MyDefinitionManager.Static.TryGetFactionDefinition(value.Tag);
						if (myFactionDefinition2 != null)
						{
							SetDefaultFactionStateInternal(recivedFactionId, value, myFactionDefinition2);
						}
					}
				}
			}
			if (ShouldForceRelationToPlayers(myFaction2))
			{
				foreach (MyIdentity allIdentity in MySession.Static.Players.GetAllIdentities())
				{
					if (!MySession.Static.Players.IdentityIsNpc(allIdentity.IdentityId))
					{
						MySession.Static.Factions.ForceRelationToPlayers(myFaction2, allIdentity.IdentityId);
					}
				}
			}
		}

		public bool ForceRelationToPlayers(MyFaction faction, long playerId)
		{
			bool flag = false;
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				return false;
			}
			MyRelationsBetweenFactions relation;
			if ((MyDefinitionManager.Static.GetDefinition(component.EconomyDefinition.PirateId) as MyFactionDefinition).Tag == faction.Tag)
			{
				ChangeReputationWithPlayer(playerId, faction.FactionId, component.EconomyDefinition.ReputationHostileMin);
				relation = MyRelationsBetweenFactions.Enemies;
				flag = true;
			}
			else
			{
				switch (faction.FactionType)
				{
				case MyFactionTypes.PlayerMade:
					relation = MyRelationsBetweenFactions.Enemies;
					flag = true;
					break;
				case MyFactionTypes.Miner:
				case MyFactionTypes.Trader:
				case MyFactionTypes.Builder:
					if (component != null)
					{
						ChangeReputationWithPlayer(playerId, faction.FactionId, component.GetDefaultReputationPlayer());
						return true;
					}
					relation = MyRelationsBetweenFactions.Neutral;
					flag = true;
					break;
				default:
					relation = MyRelationsBetweenFactions.Enemies;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				ChangeRelationWithPlayer(playerId, faction.FactionId, relation);
				return true;
			}
			return false;
		}

		private static bool ShouldForceRelationToPlayers(MyFaction faction)
		{
			if (faction.FactionType != MyFactionTypes.Miner && faction.FactionType != MyFactionTypes.Trader)
			{
				return faction.FactionType == MyFactionTypes.Builder;
			}
			return true;
		}

		public bool ForceRelationsOnNewIdentity(long identityId)
		{
			if (MySession.Static.Players.IdentityIsNpc(identityId))
			{
				return false;
			}
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, MyFaction> current = enumerator.Current;
					if (!HasRelationWithPlayer(current.Value.FactionId, identityId) && ShouldForceRelationToPlayers(current.Value))
					{
						ForceRelationToPlayers(current.Value, identityId);
					}
				}
			}
			return true;
		}

		private bool ForceRelationToFactions(MyFaction faction1, MyFaction faction2)
		{
			bool num = faction1.FactionType == MyFactionTypes.PlayerMade;
			bool flag = faction2.FactionType == MyFactionTypes.PlayerMade;
			MyFaction myFaction;
			MyFaction myFaction2;
			if (num)
			{
				myFaction = faction1;
				myFaction2 = faction2;
			}
			else
			{
				if (!flag)
				{
					SetFactionStateInternal(faction1.FactionId, faction2, MyFactionStateChange.SendPeaceRequest);
					SetFactionStateInternal(faction2.FactionId, faction1, MyFactionStateChange.AcceptPeace);
					return true;
				}
				myFaction = faction2;
				myFaction2 = faction1;
			}
			GetRelationBetweenPlayerAndFaction(myFaction.FounderId, myFaction2.FactionId).Deconstruct(out MyRelationsBetweenFactions item, out int item2);
			_ = (float)item2;
			MyRelationsBetweenFactions myRelationsBetweenFactions = item;
			MyFactionStateChange request;
			MyFactionStateChange request2 = request = MyFactionStateChange.DeclareWar;
			bool flag2 = false;
			switch (myRelationsBetweenFactions)
			{
			case MyRelationsBetweenFactions.Neutral:
				request2 = MyFactionStateChange.SendPeaceRequest;
				request = MyFactionStateChange.AcceptPeace;
				flag2 = true;
				break;
			case MyRelationsBetweenFactions.Enemies:
				request2 = MyFactionStateChange.DeclareWar;
				request = MyFactionStateChange.DeclareWar;
				flag2 = true;
				break;
			case MyRelationsBetweenFactions.Friends:
				request2 = MyFactionStateChange.SendFriendRequest;
				request = MyFactionStateChange.AcceptFriendRequest;
				flag2 = true;
				break;
			}
			if (flag2)
			{
				SetFactionStateInternal(myFaction2.FactionId, myFaction, request2);
				SetFactionStateInternal(myFaction.FactionId, myFaction2, request);
				return true;
			}
			return false;
		}

		private static bool ShouldForceRelationToFactions(MyFaction faction, MyFaction fac)
		{
			bool num = faction.FactionType == MyFactionTypes.Miner || faction.FactionType == MyFactionTypes.Trader || faction.FactionType == MyFactionTypes.Builder;
			bool flag = fac.FactionType == MyFactionTypes.Miner || fac.FactionType == MyFactionTypes.Trader || fac.FactionType == MyFactionTypes.Builder;
			bool flag2 = faction.FactionType == MyFactionTypes.PlayerMade;
			bool flag3 = fac.FactionType == MyFactionTypes.PlayerMade;
			if (num != flag)
			{
				return flag2 != flag3;
			}
			return false;
		}

		private static void SetDefaultFactionStateInternal(long factionId, IMyFaction defaultFaction, MyFactionDefinition defaultFactionDef)
		{
			MyFactionStateChange myFactionStateChange = DetermineRequestFromRelation(defaultFactionDef.DefaultRelation);
			MySession.Static.Factions.ApplyFactionStateChange(myFactionStateChange, defaultFaction.FactionId, factionId, defaultFaction.FounderId, defaultFaction.FounderId);
			MySession.Static.Factions.FactionStateChanged?.Invoke(myFactionStateChange, defaultFaction.FactionId, factionId, defaultFaction.FounderId, defaultFaction.FounderId);
		}

		private void SetFactionStateInternal(long factionId, IMyFaction faction, MyFactionStateChange request)
		{
			ApplyFactionStateChange(request, faction.FactionId, factionId, faction.FounderId, faction.FounderId);
			MySession.Static.Factions.FactionStateChanged?.Invoke(request, faction.FactionId, factionId, faction.FounderId, faction.FounderId);
		}

		public int HumansCount()
		{
			return Factions.Where((KeyValuePair<long, IMyFaction> x) => x.Value.AcceptHumans).Count();
		}

		public bool IsFactionDiscovered(MyPlayer.PlayerId playerId, long factionId)
		{
			if (!m_playerToFactionsVis.TryGetValue(playerId, out List<long> value))
			{
				return false;
			}
			if (!value.Contains(factionId))
			{
				return false;
			}
			return true;
		}

		public void AddDiscoveredFaction(MyPlayer.PlayerId playerId, long factionId, bool triggerEvent = true)
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.Error("It is illegal to add discovered factions on clients.");
				return;
			}
			AddDiscoveredFaction_Internal(playerId, factionId, triggerEvent);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => AddDiscoveredFaction_Clients, playerId.SteamId, playerId.SerialId, factionId);
		}

		public List<MyFaction> GetNpcFactions()
		{
			List<MyFaction> list = new List<MyFaction>();
			foreach (MyFaction value in m_factions.Values)
			{
				if (IsNpcFaction(value.Tag) && !(value.Tag == "SPID"))
				{
					list.Add(value);
				}
			}
			return list;
		}

		[Event(null, 2128)]
		[Reliable]
		[Broadcast]
		private static void AddDiscoveredFaction_Clients(ulong playerId, int serialId, long factionId)
		{
			MyPlayer.PlayerId playerId2 = new MyPlayer.PlayerId(playerId, serialId);
			MySession.Static.Factions.AddDiscoveredFaction_Internal(playerId2, factionId);
		}

		private void AddDiscoveredFaction_Internal(MyPlayer.PlayerId playerId, long factionId, bool triggerEvent = true)
		{
			if (!m_playerToFactionsVis.TryGetValue(playerId, out List<long> value))
			{
				value = new List<long>();
				m_playerToFactionsVis.Add(playerId, value);
			}
			if (!value.Contains(factionId))
			{
				value.Add(factionId);
				IMyFaction myFaction = TryGetFactionById(factionId);
				MyFaction arg;
				if (triggerEvent && (arg = (myFaction as MyFaction)) != null)
				{
					this.OnFactionDiscovered?.Invoke(arg, playerId);
				}
			}
		}

		public void RemoveDiscoveredFaction(MyPlayer.PlayerId playerId, long factionId)
		{
			if (!Sync.IsServer)
			{
				MyLog.Default.Error("It is illegal to removed discovered factions on clients.");
				return;
			}
			RemoveDiscoveredFaction_Internal(playerId, factionId);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RemoveDiscoveredFaction_Clients, playerId.SteamId, playerId.SerialId, factionId);
		}

		[Event(null, 2173)]
		[Reliable]
		[Broadcast]
		private static void RemoveDiscoveredFaction_Clients(ulong playerId, int serialId, long factionId)
		{
			MyPlayer.PlayerId playerId2 = new MyPlayer.PlayerId(playerId, serialId);
			MySession.Static.Factions.RemoveDiscoveredFaction_Internal(playerId2, factionId);
		}

		private void RemoveDiscoveredFaction_Internal(MyPlayer.PlayerId playerId, long factionId)
		{
			if (m_playerToFactionsVis.TryGetValue(playerId, out List<long> value))
			{
				value.Remove(factionId);
				if (value.Count == 0)
				{
					m_playerToFactionsVis.Remove(playerId);
				}
			}
		}

		public MyObjectBuilder_FactionCollection GetObjectBuilder()
		{
			MyObjectBuilder_FactionCollection myObjectBuilder_FactionCollection = new MyObjectBuilder_FactionCollection();
			myObjectBuilder_FactionCollection.Factions = new List<MyObjectBuilder_Faction>(m_factions.Count);
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				myObjectBuilder_FactionCollection.Factions.Add(faction.Value.GetObjectBuilder());
			}
			myObjectBuilder_FactionCollection.Players = new SerializableDictionary<long, long>();
			foreach (KeyValuePair<long, long> item4 in m_playerFaction)
			{
				myObjectBuilder_FactionCollection.Players.Dictionary.Add(item4.Key, item4.Value);
			}
			myObjectBuilder_FactionCollection.Relations = new List<MyObjectBuilder_FactionRelation>(m_relationsBetweenFactions.Count);
			foreach (KeyValuePair<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> relationsBetweenFaction in m_relationsBetweenFactions)
			{
				MyObjectBuilder_FactionRelation item = default(MyObjectBuilder_FactionRelation);
				item.FactionId1 = relationsBetweenFaction.Key.RelateeId1;
				item.FactionId2 = relationsBetweenFaction.Key.RelateeId2;
				item.Relation = relationsBetweenFaction.Value.Item1;
				item.Reputation = relationsBetweenFaction.Value.Item2;
				myObjectBuilder_FactionCollection.Relations.Add(item);
			}
			myObjectBuilder_FactionCollection.Requests = new List<MyObjectBuilder_FactionRequests>();
			foreach (KeyValuePair<long, HashSet<long>> factionRequest in m_factionRequests)
			{
				List<long> list = new List<long>(factionRequest.Value.Count);
				foreach (long item5 in m_factionRequests[factionRequest.Key])
				{
					list.Add(item5);
				}
				myObjectBuilder_FactionCollection.Requests.Add(new MyObjectBuilder_FactionRequests
				{
					FactionId = factionRequest.Key,
					FactionRequests = list
				});
			}
			myObjectBuilder_FactionCollection.RelationsWithPlayers = new List<MyObjectBuilder_PlayerFactionRelation>();
			foreach (KeyValuePair<MyRelatablePair, Tuple<MyRelationsBetweenFactions, int>> relationsBetweenPlayersAndFaction in m_relationsBetweenPlayersAndFactions)
			{
				MyObjectBuilder_PlayerFactionRelation item2 = default(MyObjectBuilder_PlayerFactionRelation);
				item2.PlayerId = relationsBetweenPlayersAndFaction.Key.RelateeId1;
				item2.FactionId = relationsBetweenPlayersAndFaction.Key.RelateeId2;
				item2.Relation = relationsBetweenPlayersAndFaction.Value.Item1;
				item2.Reputation = relationsBetweenPlayersAndFaction.Value.Item2;
				myObjectBuilder_FactionCollection.RelationsWithPlayers.Add(item2);
			}
			myObjectBuilder_FactionCollection.PlayerToFactionsVis = new List<MyObjectBuilder_FactionsVisEntry>(m_playerToFactionsVis.Count);
			foreach (KeyValuePair<MyPlayer.PlayerId, List<long>> playerToFactionsVi in m_playerToFactionsVis)
			{
				MyObjectBuilder_FactionsVisEntry item3 = default(MyObjectBuilder_FactionsVisEntry);
				item3.PlayerId = playerToFactionsVi.Key.SteamId;
				item3.SerialId = playerToFactionsVi.Key.SerialId;
				item3.DiscoveredFactions = new List<long>(playerToFactionsVi.Value.Count);
				foreach (long item6 in playerToFactionsVi.Value)
				{
					item3.DiscoveredFactions.Add(item6);
				}
				myObjectBuilder_FactionCollection.PlayerToFactionsVis.Add(item3);
			}
			return myObjectBuilder_FactionCollection;
		}

		public void Init(MyObjectBuilder_FactionCollection builder)
		{
			foreach (MyObjectBuilder_Faction faction in builder.Factions)
			{
				if (!MyBankingSystem.Static.TryGetAccountInfo(faction.FactionId, out MyAccountInfo _))
				{
					MyBankingSystem.Static.CreateAccount(faction.FactionId, 0L);
				}
				MySession.Static.Factions.Add(new MyFaction(faction));
			}
			foreach (KeyValuePair<long, long> item3 in builder.Players.Dictionary)
			{
				m_playerFaction.Add(item3.Key, item3.Value);
			}
			MySessionComponentEconomy mySessionComponentEconomy = null;
			if (MySession.Static != null)
			{
				mySessionComponentEconomy = MySession.Static.GetComponent<MySessionComponentEconomy>();
			}
			MyRelationsBetweenFactions item;
			int item2;
			foreach (MyObjectBuilder_FactionRelation relation in builder.Relations)
			{
				MyRelationsBetweenFactions myRelationsBetweenFactions = relation.Relation;
				int num = relation.Reputation;
				if (mySessionComponentEconomy != null)
				{
					mySessionComponentEconomy.ValidateReputationConsistency(myRelationsBetweenFactions, num).Deconstruct(out item, out item2);
					myRelationsBetweenFactions = item;
					num = item2;
				}
				m_relationsBetweenFactions.Add(new MyRelatablePair(relation.FactionId1, relation.FactionId2), new Tuple<MyRelationsBetweenFactions, int>(myRelationsBetweenFactions, num));
			}
			foreach (MyObjectBuilder_PlayerFactionRelation relationsWithPlayer in builder.RelationsWithPlayers)
			{
				MyRelationsBetweenFactions myRelationsBetweenFactions = relationsWithPlayer.Relation;
				int num = relationsWithPlayer.Reputation;
				if (mySessionComponentEconomy != null)
				{
					mySessionComponentEconomy.ValidateReputationConsistency(myRelationsBetweenFactions, num).Deconstruct(out item, out item2);
					myRelationsBetweenFactions = item;
					num = item2;
				}
				m_relationsBetweenPlayersAndFactions.Add(new MyRelatablePair(relationsWithPlayer.PlayerId, relationsWithPlayer.FactionId), new Tuple<MyRelationsBetweenFactions, int>(myRelationsBetweenFactions, num));
			}
			foreach (MyObjectBuilder_FactionRequests request in builder.Requests)
			{
				HashSet<long> hashSet = new HashSet<long>();
				foreach (long factionRequest in request.FactionRequests)
				{
					hashSet.Add(factionRequest);
				}
				m_factionRequests.Add(request.FactionId, hashSet);
			}
			if (builder.PlayerToFactionsVis != null)
			{
				m_playerToFactionsVis.Clear();
				foreach (MyObjectBuilder_FactionsVisEntry playerToFactionsVi in builder.PlayerToFactionsVis)
				{
					List<long> list = new List<long>();
					foreach (long discoveredFaction in playerToFactionsVi.DiscoveredFactions)
					{
						list.Add(discoveredFaction);
					}
					m_playerToFactionsVis.Add(new MyPlayer.PlayerId(playerToFactionsVi.PlayerId, playerToFactionsVi.SerialId), list);
				}
			}
			m_reputationSettings = MyDefinitionManager.Static.GetDefinition<MyReputationSettingsDefinition>("DefaultReputationSettings");
			m_notificationRepInc = new MyReputationNotification(new MyHudNotification(MySpaceTexts.Economy_Notification_ReputationIncreased, 2500, "Green"));
			m_notificationRepDec = new MyReputationNotification(new MyHudNotification(MySpaceTexts.Economy_Notification_ReputationDecreased, 2500, "Red"));
			CompatDefaultFactions();
		}

		public IEnumerator<KeyValuePair<long, MyFaction>> GetEnumerator()
		{
			return m_factions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator<KeyValuePair<long, MyFaction>> IEnumerable<KeyValuePair<long, MyFaction>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool GetRandomFriendlyStation(long factionId, long stationId, out MyFaction friendlyFaction, out MyStation friendlyStation, bool includeSameFaction = false)
		{
			friendlyFaction = null;
			friendlyStation = null;
			List<MyFaction> list = new List<MyFaction>();
			using (IEnumerator<KeyValuePair<long, MyFaction>> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<long, MyFaction> current = enumerator.Current;
					if (current.Value.FactionType != 0 && current.Value.FactionType != MyFactionTypes.PlayerMade && ((includeSameFaction && factionId == current.Value.FactionId) || GetRelationBetweenFactions(factionId, current.Value.FactionId).Item1 == MyRelationsBetweenFactions.Friends))
					{
						list.Add(current.Value);
					}
				}
			}
			if (list.Count <= 0)
			{
				return false;
			}
			int num = 0;
			int num2 = 10;
			bool flag = false;
			MyStation myStation;
			MyFaction myFaction;
			do
			{
				num++;
				myFaction = null;
				myStation = null;
				flag = false;
				myFaction = list[MyRandom.Instance.Next(0, list.Count)];
				if (myFaction.Stations.Count > ((myFaction.FactionId == factionId) ? 1 : 0))
				{
					int num3 = MyRandom.Instance.Next(0, myFaction.Stations.Count);
					myStation = myFaction.Stations.ElementAt(num3);
					if (myStation.Id == stationId)
					{
						myStation = myFaction.Stations.ElementAt((num3 + 1) % myFaction.Stations.Count);
					}
					flag = true;
				}
			}
			while (!flag && num <= num2);
			if (!flag)
			{
				return false;
			}
			friendlyFaction = myFaction;
			friendlyStation = myStation;
			return true;
		}

		bool IMyFactionCollection.FactionTagExists(string tag, IMyFaction doNotCheck)
		{
			return FactionTagExists(tag, doNotCheck);
		}

		bool IMyFactionCollection.FactionNameExists(string name, IMyFaction doNotCheck)
		{
			return FactionNameExists(name, doNotCheck);
		}

		IMyFaction IMyFactionCollection.TryGetFactionById(long factionId)
		{
			return TryGetFactionById(factionId);
		}

		IMyFaction IMyFactionCollection.TryGetPlayerFaction(long playerId)
		{
			return TryGetPlayerFaction(playerId);
		}

		IMyFaction IMyFactionCollection.TryGetFactionByTag(string tag)
		{
			return TryGetFactionByTag(tag);
		}

		IMyFaction IMyFactionCollection.TryGetFactionByName(string name)
		{
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				MyFaction value = faction.Value;
				if (string.Equals(name, value.Name, StringComparison.OrdinalIgnoreCase))
				{
					return value;
				}
			}
			return null;
		}

		void IMyFactionCollection.AddPlayerToFaction(long playerId, long factionId)
		{
			AddPlayerToFaction(playerId, factionId);
		}

		void IMyFactionCollection.KickPlayerFromFaction(long playerId)
		{
			KickPlayerFromFaction(playerId);
		}

		MyRelationsBetweenFactions IMyFactionCollection.GetRelationBetweenFactions(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item1;
		}

		int IMyFactionCollection.GetReputationBetweenFactions(long factionId1, long factionId2)
		{
			return GetRelationBetweenFactions(factionId1, factionId2).Item2;
		}

		void IMyFactionCollection.SetReputation(long fromFactionId, long toFactionId, int reputation)
		{
			SetReputationBetweenFactions(fromFactionId, toFactionId, ClampReputation(reputation));
		}

		int IMyFactionCollection.GetReputationBetweenPlayerAndFaction(long identityId, long factionId)
		{
			return GetRelationBetweenPlayerAndFaction(identityId, factionId).Item2;
		}

		void IMyFactionCollection.SetReputationBetweenPlayerAndFaction(long identityId, long factionId, int reputation)
		{
			SetReputationBetweenPlayerAndFaction(identityId, factionId, ClampReputation(reputation));
		}

		bool IMyFactionCollection.AreFactionsEnemies(long factionId1, long factionId2)
		{
			return AreFactionsEnemies(factionId1, factionId2);
		}

		bool IMyFactionCollection.IsPeaceRequestStateSent(long myFactionId, long foreignFactionId)
		{
			return IsPeaceRequestStateSent(myFactionId, foreignFactionId);
		}

		bool IMyFactionCollection.IsPeaceRequestStatePending(long myFactionId, long foreignFactionId)
		{
			return IsPeaceRequestStatePending(myFactionId, foreignFactionId);
		}

		void IMyFactionCollection.RemoveFaction(long factionId)
		{
			RemoveFaction(factionId);
		}

		void IMyFactionCollection.SendPeaceRequest(long fromFactionId, long toFactionId)
		{
			SendPeaceRequest(fromFactionId, toFactionId);
		}

		void IMyFactionCollection.CancelPeaceRequest(long fromFactionId, long toFactionId)
		{
			CancelPeaceRequest(fromFactionId, toFactionId);
		}

		void IMyFactionCollection.AcceptPeace(long fromFactionId, long toFactionId)
		{
			AcceptPeace(fromFactionId, toFactionId);
		}

		void IMyFactionCollection.DeclareWar(long fromFactionId, long toFactionId)
		{
			DeclareWar(fromFactionId, toFactionId);
		}

		void IMyFactionCollection.SendJoinRequest(long factionId, long playerId)
		{
			SendJoinRequest(factionId, playerId);
		}

		void IMyFactionCollection.CancelJoinRequest(long factionId, long playerId)
		{
			CancelJoinRequest(factionId, playerId);
		}

		void IMyFactionCollection.AcceptJoin(long factionId, long playerId)
		{
			AcceptJoin(factionId, playerId);
		}

		void IMyFactionCollection.KickMember(long factionId, long playerId)
		{
			KickMember(factionId, playerId);
		}

		void IMyFactionCollection.PromoteMember(long factionId, long playerId)
		{
			PromoteMember(factionId, playerId);
		}

		void IMyFactionCollection.DemoteMember(long factionId, long playerId)
		{
			DemoteMember(factionId, playerId);
		}

		void IMyFactionCollection.MemberLeaves(long factionId, long playerId)
		{
			MemberLeaves(factionId, playerId);
		}

		void IMyFactionCollection.ChangeAutoAccept(long factionId, long playerId, bool autoAcceptMember, bool autoAcceptPeace)
		{
			ChangeAutoAccept(factionId, playerId, autoAcceptMember, autoAcceptPeace);
		}

		void IMyFactionCollection.EditFaction(long factionId, string tag, string name, string desc, string privateInfo)
		{
			EditFaction(factionId, tag, name, desc, privateInfo);
		}

		void IMyFactionCollection.CreateFaction(long founderId, string tag, string name, string desc, string privateInfo)
		{
			CreateFaction(founderId, tag, name, desc, privateInfo, MyFactionTypes.None);
		}

		void IMyFactionCollection.CreateFaction(long founderId, string tag, string name, string desc, string privateInfo, MyFactionTypes type)
		{
			CreateFaction(founderId, tag, name, desc, privateInfo, type);
		}

		void IMyFactionCollection.CreateNPCFaction(string tag, string name, string desc, string privateInfo)
		{
			CreateNPCFaction(tag, name, desc, privateInfo);
		}

		MyObjectBuilder_FactionCollection IMyFactionCollection.GetObjectBuilder()
		{
			return GetObjectBuilder();
		}
	}
}
