using Sandbox.Definitions;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World
{
	public class MyFaction : IMyFaction
	{
		private Dictionary<long, MyFactionMember> m_members;

		private Dictionary<long, MyFactionMember> m_joinRequests;

		private Dictionary<long, MyStation> m_stations;

		private MyBlockLimits m_blockLimits;

		public string Tag;

		public string Name;

		public string Description;

		public string PrivateInfo;

		public MyStringId? FactionIcon;

		public bool AutoAcceptMember;

		public bool AutoAcceptPeace;

		public bool AcceptHumans;

		public bool EnableFriendlyFire = true;

		public MyFactionTypes FactionType;

		public long FactionId
		{
			get;
			private set;
		}

		public long FounderId
		{
			get;
			private set;
		}

		public Vector3 CustomColor
		{
			get;
			set;
		}

		public Vector3 IconColor
		{
			get;
			set;
		}

		public DictionaryReader<long, MyFactionMember> Members => new DictionaryReader<long, MyFactionMember>(m_members);

		public DictionaryReader<long, MyFactionMember> JoinRequests => new DictionaryReader<long, MyFactionMember>(m_joinRequests);

		public DictionaryValuesReader<long, MyStation> Stations => new DictionaryValuesReader<long, MyStation>(m_stations);

		public bool IsAnyLeaderOnline
		{
			get
			{
				ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
				foreach (KeyValuePair<long, MyFactionMember> member in m_members)
				{
					if (member.Value.IsLeader && onlinePlayers.Any((MyPlayer x) => x.Identity.IdentityId == member.Value.PlayerId))
					{
						return true;
					}
				}
				return false;
			}
		}

		public MyBlockLimits BlockLimits => m_blockLimits;

		long IMyFaction.FactionId => FactionId;

		string IMyFaction.Tag => Tag;

		string IMyFaction.Name => Name;

		string IMyFaction.Description => Description;

		string IMyFaction.PrivateInfo => PrivateInfo;

		MyStringId? IMyFaction.FactionIcon => FactionIcon;

		bool IMyFaction.AutoAcceptMember => AutoAcceptMember;

		bool IMyFaction.AutoAcceptPeace => AutoAcceptPeace;

		bool IMyFaction.AcceptHumans => AcceptHumans;

		long IMyFaction.FounderId => FounderId;

		DictionaryReader<long, MyFactionMember> IMyFaction.Members => Members;

		DictionaryReader<long, MyFactionMember> IMyFaction.JoinRequests => JoinRequests;

		public MyFaction(long id, string tag, string name, string desc, string privateInfo, long creatorId, MyFactionTypes factionType = MyFactionTypes.None, Vector3? customColor = null, Vector3? factionIconColor = null, string factionIcon = "")
		{
			FactionId = id;
			Tag = tag;
			Name = name;
			Description = desc;
			PrivateInfo = privateInfo;
			FounderId = creatorId;
			FactionType = factionType;
			FactionIcon = MyStringId.GetOrCompute(factionIcon);
			CustomColor = (customColor.HasValue ? customColor.Value : MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero));
			IconColor = (factionIconColor.HasValue ? factionIconColor.Value : MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero));
			AutoAcceptMember = false;
			AutoAcceptPeace = false;
			AcceptHumans = true;
			m_members = new Dictionary<long, MyFactionMember>();
			m_joinRequests = new Dictionary<long, MyFactionMember>();
			m_stations = new Dictionary<long, MyStation>();
			m_blockLimits = new MyBlockLimits(MyBlockLimits.GetInitialPCU(-1L), 0);
			m_members.Add(creatorId, new MyFactionMember(creatorId, isLeader: true, isFounder: true));
		}

		public MyFaction(long id, long creatorId, string privateInfo, MyFactionDefinition factionDefinition, Vector3? customColor = null, Vector3? iconColor = null)
		{
			FactionId = id;
			Tag = factionDefinition.Tag;
			Name = factionDefinition.DisplayNameText;
			Description = factionDefinition.DescriptionText;
			PrivateInfo = privateInfo;
			FounderId = creatorId;
			FactionType = factionDefinition.Type;
			FactionIcon = factionDefinition.FactionIcon;
			CustomColor = (customColor.HasValue ? customColor.Value : MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero));
			IconColor = (iconColor.HasValue ? iconColor.Value : MyColorPickerConstants.HSVToHSVOffset(Vector3.Zero));
			m_blockLimits = new MyBlockLimits(MyBlockLimits.GetInitialPCU(-1L), 0);
			AutoAcceptMember = factionDefinition.AutoAcceptMember;
			AutoAcceptPeace = false;
			AcceptHumans = factionDefinition.AcceptHumans;
			EnableFriendlyFire = factionDefinition.EnableFriendlyFire;
			m_members = new Dictionary<long, MyFactionMember>();
			m_joinRequests = new Dictionary<long, MyFactionMember>();
			m_stations = new Dictionary<long, MyStation>();
			m_members.Add(creatorId, new MyFactionMember(creatorId, isLeader: true, isFounder: true));
		}

		public MyFaction(MyObjectBuilder_Faction obj)
		{
			FactionId = obj.FactionId;
			Tag = obj.Tag;
			Name = obj.Name;
			Description = obj.Description;
			PrivateInfo = obj.PrivateInfo;
			AutoAcceptMember = obj.AutoAcceptMember;
			AutoAcceptPeace = obj.AutoAcceptPeace;
			EnableFriendlyFire = obj.EnableFriendlyFire;
			AcceptHumans = obj.AcceptHumans;
			FactionType = obj.FactionType;
			m_blockLimits = new MyBlockLimits(MyBlockLimits.GetInitialPCU(-1L), 0, obj.TransferedPCUDelta);
			m_members = new Dictionary<long, MyFactionMember>(obj.Members.Count);
			foreach (MyObjectBuilder_FactionMember member in obj.Members)
			{
				m_members.Add(member.PlayerId, member);
				if (member.IsFounder)
				{
					FounderId = member.PlayerId;
				}
			}
			if (obj.JoinRequests != null)
			{
				m_joinRequests = new Dictionary<long, MyFactionMember>(obj.JoinRequests.Count);
				foreach (MyObjectBuilder_FactionMember joinRequest in obj.JoinRequests)
				{
					m_joinRequests.Add(joinRequest.PlayerId, joinRequest);
				}
			}
			else
			{
				m_joinRequests = new Dictionary<long, MyFactionMember>();
			}
			MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.TryGetFactionDefinition(Tag);
			if (myFactionDefinition != null)
			{
				AutoAcceptMember = myFactionDefinition.AutoAcceptMember;
				AcceptHumans = myFactionDefinition.AcceptHumans;
				EnableFriendlyFire = myFactionDefinition.EnableFriendlyFire;
				Name = myFactionDefinition.DisplayNameText;
				Description = myFactionDefinition.DescriptionText;
			}
			m_stations = new Dictionary<long, MyStation>();
			foreach (MyObjectBuilder_Station station in obj.Stations)
			{
				MyStation myStation = new MyStation(station);
				m_stations.Add(myStation.Id, myStation);
			}
			CustomColor = obj.CustomColor;
			IconColor = obj.IconColor;
			if (obj.FactionIcon != null)
			{
				FactionIcon = MyStringId.GetOrCompute(obj.FactionIcon);
			}
			else if (myFactionDefinition == null)
			{
				FactionIcon = MyStringId.GetOrCompute("Textures\\FactionLogo\\Empty.dds");
			}
			CheckAndFixFactionRanks();
		}

		public bool IsFounder(long playerId)
		{
			if (m_members.TryGetValue(playerId, out MyFactionMember value))
			{
				return value.IsFounder;
			}
			return false;
		}

		public bool IsLeader(long playerId)
		{
			if (m_members.TryGetValue(playerId, out MyFactionMember value))
			{
				return value.IsLeader;
			}
			return false;
		}

		public bool IsMember(long playerId)
		{
			MyFactionMember value;
			return m_members.TryGetValue(playerId, out value);
		}

		public bool IsNeutral(long playerId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId);
			if (myFaction != null)
			{
				return MySession.Static.Factions.GetRelationBetweenFactions(myFaction.FactionId, FactionId).Item1 == MyRelationsBetweenFactions.Neutral;
			}
			return MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerId, FactionId).Item1 == MyRelationsBetweenFactions.Neutral;
		}

		public bool IsEnemy(long playerId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId);
			if (myFaction != null)
			{
				return MySession.Static.Factions.GetRelationBetweenFactions(myFaction.FactionId, FactionId).Item1 == MyRelationsBetweenFactions.Enemies;
			}
			return MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerId, FactionId).Item1 == MyRelationsBetweenFactions.Enemies;
		}

		public bool IsFriendly(long playerId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(playerId);
			if (myFaction != null)
			{
				return MySession.Static.Factions.GetRelationBetweenFactions(myFaction.FactionId, FactionId).Item1 == MyRelationsBetweenFactions.Friends;
			}
			return MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(playerId, FactionId).Item1 == MyRelationsBetweenFactions.Friends;
		}

		public bool IsEveryoneNpc()
		{
			foreach (KeyValuePair<long, MyFactionMember> member in m_members)
			{
				if (!Sync.Players.IdentityIsNpc(member.Key))
				{
					return false;
				}
			}
			return true;
		}

		public void AddJoinRequest(long playerId)
		{
			m_joinRequests[playerId] = new MyFactionMember(playerId, isLeader: false);
		}

		public void CancelJoinRequest(long playerId)
		{
			m_joinRequests.Remove(playerId);
		}

		public void AcceptJoin(long playerId, bool autoaccept = false)
		{
			MyFaction myFaction = null;
			myFaction = MySession.Static.Factions.GetPlayerFaction(playerId);
			if (myFaction != null)
			{
				myFaction.KickMember(playerId, raiseChanged: false);
				if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION)
				{
					MyBlockLimits.TransferBlockLimits(playerId, myFaction.BlockLimits, BlockLimits);
				}
			}
			MySession.Static.Factions.AddPlayerToFactionInternal(playerId, FactionId);
			if (m_joinRequests.ContainsKey(playerId))
			{
				m_members[playerId] = m_joinRequests[playerId];
				m_joinRequests.Remove(playerId);
			}
			else if (AutoAcceptMember || autoaccept)
			{
				m_members[playerId] = new MyFactionMember(playerId, isLeader: false);
			}
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(playerId);
			if (myIdentity != null)
			{
				myIdentity.BlockLimits.SetAllDirty();
				myIdentity.RaiseFactionChanged(myFaction, this);
			}
			MySession.Static.Factions.InvokePlayerJoined(this, playerId);
		}

		public void KickMember(long playerId, bool raiseChanged = true)
		{
			m_members.Remove(playerId);
			MySession.Static.Factions.KickPlayerFromFaction(playerId);
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(playerId);
			if (raiseChanged)
			{
				myIdentity?.RaiseFactionChanged(this, null);
			}
			CheckAndFixFactionRanks();
			MySession.Static.Factions.InvokePlayerLeft(this, playerId);
		}

		public void PromoteMember(long playerId)
		{
			if (m_members.TryGetValue(playerId, out MyFactionMember value))
			{
				value.IsLeader = true;
				m_members[playerId] = value;
			}
		}

		public void DemoteMember(long playerId)
		{
			if (m_members.TryGetValue(playerId, out MyFactionMember value))
			{
				value.IsLeader = false;
				m_members[playerId] = value;
			}
		}

		public void PromoteToFounder(long playerId)
		{
			if (m_members.TryGetValue(playerId, out MyFactionMember value))
			{
				value.IsLeader = true;
				value.IsFounder = true;
				m_members[playerId] = value;
				FounderId = playerId;
			}
		}

		public void CheckAndFixFactionRanks()
		{
			if (!HasFounder())
			{
				foreach (KeyValuePair<long, MyFactionMember> member in m_members)
				{
					if (member.Value.IsLeader)
					{
						PromoteToFounder(member.Key);
						return;
					}
				}
				if (m_members.Count > 0)
				{
					PromoteToFounder(m_members.Keys.FirstOrDefault());
				}
			}
		}

		private bool HasFounder()
		{
			if (m_members.TryGetValue(FounderId, out MyFactionMember value))
			{
				return value.IsFounder;
			}
			return false;
		}

		public MyObjectBuilder_Faction GetObjectBuilder()
		{
			MyObjectBuilder_Faction myObjectBuilder_Faction = new MyObjectBuilder_Faction();
			myObjectBuilder_Faction.FactionId = FactionId;
			myObjectBuilder_Faction.Tag = Tag;
			myObjectBuilder_Faction.Name = Name;
			myObjectBuilder_Faction.Description = Description;
			myObjectBuilder_Faction.PrivateInfo = PrivateInfo;
			myObjectBuilder_Faction.AutoAcceptMember = AutoAcceptMember;
			myObjectBuilder_Faction.AutoAcceptPeace = AutoAcceptPeace;
			myObjectBuilder_Faction.EnableFriendlyFire = EnableFriendlyFire;
			myObjectBuilder_Faction.AcceptHumans = AcceptHumans;
			myObjectBuilder_Faction.FactionType = FactionType;
			myObjectBuilder_Faction.Members = new List<MyObjectBuilder_FactionMember>(Members.Count);
			foreach (KeyValuePair<long, MyFactionMember> member in Members)
			{
				myObjectBuilder_Faction.Members.Add(member.Value);
			}
			myObjectBuilder_Faction.JoinRequests = new List<MyObjectBuilder_FactionMember>(JoinRequests.Count);
			foreach (KeyValuePair<long, MyFactionMember> joinRequest in JoinRequests)
			{
				myObjectBuilder_Faction.JoinRequests.Add(joinRequest.Value);
			}
			myObjectBuilder_Faction.Stations = new List<MyObjectBuilder_Station>(Stations.Count);
			foreach (MyStation station in Stations)
			{
				myObjectBuilder_Faction.Stations.Add(station.GetObjectBuilder());
			}
			myObjectBuilder_Faction.CustomColor = CustomColor;
			myObjectBuilder_Faction.IconColor = IconColor;
			myObjectBuilder_Faction.FactionIcon = (FactionIcon.HasValue ? FactionIcon.Value.String : null);
			myObjectBuilder_Faction.TransferedPCUDelta = m_blockLimits.TransferedDelta;
			return myObjectBuilder_Faction;
		}

		public void AddStation(MyStation station)
		{
			m_stations.Add(station.Id, station);
		}

		public MyStation GetStationById(long stationId)
		{
			m_stations.TryGetValue(stationId, out MyStation value);
			return value;
		}

		bool IMyFaction.IsFounder(long playerId)
		{
			return IsFounder(playerId);
		}

		bool IMyFaction.IsLeader(long playerId)
		{
			return IsLeader(playerId);
		}

		bool IMyFaction.IsMember(long playerId)
		{
			return IsMember(playerId);
		}

		bool IMyFaction.IsNeutral(long playerId)
		{
			return IsNeutral(playerId);
		}

		bool IMyFaction.IsEnemy(long playerId)
		{
			return IsEnemy(playerId);
		}

		bool IMyFaction.IsFriendly(long playerId)
		{
			return IsFriendly(playerId);
		}

		bool IMyFaction.TryGetBalanceInfo(out long balance)
		{
			balance = 0L;
			if (MyBankingSystem.Static != null && MyBankingSystem.Static.TryGetAccountInfo(FactionId, out MyAccountInfo account))
			{
				balance = account.Balance;
				return true;
			}
			return false;
		}

		string IMyFaction.GetBalanceShortString()
		{
			if (MyBankingSystem.Static == null)
			{
				return null;
			}
			return MyBankingSystem.Static.GetBalanceShortString(FactionId);
		}

		void IMyFaction.RequestChangeBalance(long amount)
		{
			if (MyBankingSystem.Static != null)
			{
				MyBankingSystem.RequestBalanceChange(FactionId, amount);
			}
		}
	}
}
