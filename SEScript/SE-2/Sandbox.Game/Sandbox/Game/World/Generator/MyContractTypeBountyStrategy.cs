using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;

namespace Sandbox.Game.World.Generator
{
	public class MyContractTypeBountyStrategy : MyContractTypeBaseStrategy
	{
		public MyContractTypeBountyStrategy(MySessionComponentEconomyDefinition economyDefinition)
			: base(economyDefinition)
		{
		}

		public override bool CanBeGenerated(MyStation station, MyFaction faction)
		{
			if (MySession.Static.Settings.EnableBountyContracts)
			{
				return IsAnyPiratePlayerAvailable();
			}
			return false;
		}

		public override MyContractCreationResults GenerateContract(out MyContract contract, long factionId, long stationId, MyMinimalPriceCalculator calculator, MyTimeSpan now)
		{
			MyFactionCollection factions = MySession.Static.Factions;
			contract = null;
			MyContractHunt myContractHunt = new MyContractHunt();
			MyContractTypeHuntDefinition myContractTypeHuntDefinition = myContractHunt.GetDefinition() as MyContractTypeHuntDefinition;
			if (myContractTypeHuntDefinition == null)
			{
				return MyContractCreationResults.Error;
			}
			MyStation myStation = (factions.TryGetFactionById(factionId) as MyFaction)?.GetStationById(stationId);
			if (myStation == null)
			{
				return MyContractCreationResults.Error;
			}
			long randomPiratePlayer_WeightedByRep = GetRandomPiratePlayer_WeightedByRep(factions);
			if (randomPiratePlayer_WeightedByRep == 0L)
			{
				return MyContractCreationResults.Fail_Impossible;
			}
			MyObjectBuilder_ContractHunt myObjectBuilder_ContractHunt = new MyObjectBuilder_ContractHunt();
			myObjectBuilder_ContractHunt.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractHunt.IsPlayerMade = false;
			myObjectBuilder_ContractHunt.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractHunt.RewardMoney = GetMoneyRewardForBountyContract(randomPiratePlayer_WeightedByRep, myContractTypeHuntDefinition.MinimumMoney, myContractTypeHuntDefinition.MoneyReputationCoeficient);
			if (myStation.IsDeepSpaceStation)
			{
				myObjectBuilder_ContractHunt.RewardMoney = (long)((float)myObjectBuilder_ContractHunt.RewardMoney * m_economyDefinition.DeepSpaceStationContractBonus);
			}
			myObjectBuilder_ContractHunt.RewardReputation = GetReputationRewardForBountyContract(myContractTypeHuntDefinition.MinimumReputation);
			myObjectBuilder_ContractHunt.StartingDeposit = (long)(MyRandom.Instance.NextDouble() * (double)(myContractTypeHuntDefinition.MaxStartingDeposit - myContractTypeHuntDefinition.MinStartingDeposit));
			myObjectBuilder_ContractHunt.FailReputationPrice = myContractTypeHuntDefinition.FailReputationPrice;
			myObjectBuilder_ContractHunt.StartFaction = factionId;
			myObjectBuilder_ContractHunt.StartStation = stationId;
			myObjectBuilder_ContractHunt.StartBlock = 0L;
			myObjectBuilder_ContractHunt.Target = randomPiratePlayer_WeightedByRep;
			myObjectBuilder_ContractHunt.RemarkPeriod = MyTimeSpan.FromSeconds(myContractTypeHuntDefinition.RemarkPeriodInS).Ticks;
			myObjectBuilder_ContractHunt.RemarkVariance = myContractTypeHuntDefinition.RemarkVariance;
			myObjectBuilder_ContractHunt.KillRange = myContractTypeHuntDefinition.KillRange;
			myObjectBuilder_ContractHunt.KillRangeMultiplier = myContractTypeHuntDefinition.KillRangeMultiplier;
			myObjectBuilder_ContractHunt.ReputationLossForTarget = myContractTypeHuntDefinition.ReputationLossForTarget;
			myObjectBuilder_ContractHunt.RewardRadius = myContractTypeHuntDefinition.RewardRadius;
			myObjectBuilder_ContractHunt.Creation = now.Ticks;
			myObjectBuilder_ContractHunt.RemainingTimeInS = GetDurationForBountyContract(myContractTypeHuntDefinition).Seconds;
			myObjectBuilder_ContractHunt.TicksToDiscard = MyContractTypeBaseStrategy.TICKS_TO_LIVE;
			myContractHunt.Init(myObjectBuilder_ContractHunt);
			contract = myContractHunt;
			return MyContractCreationResults.Success;
		}

		public long GetRandomPiratePlayer_WeightedByRep(MyFactionCollection factions)
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				return 0L;
			}
			MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.GetDefinition(m_economyDefinition.PirateId) as MyFactionDefinition;
			if (myFactionDefinition == null)
			{
				return 0L;
			}
			MyFaction myFaction = null;
			foreach (KeyValuePair<long, MyFaction> faction in factions)
			{
				if (faction.Value.Tag == myFactionDefinition.Tag)
				{
					myFaction = faction.Value;
					break;
				}
			}
			if (myFaction == null)
			{
				return 0L;
			}
			List<int> list = new List<int>();
			List<long> list2 = new List<long>();
			int num = 0;
			foreach (MyPlayer.PlayerId allPlayer in MySession.Static.Players.GetAllPlayers())
			{
				long num2 = MySession.Static.Players.TryGetIdentityId(allPlayer.SteamId);
				Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = factions.GetRelationBetweenPlayerAndFaction(num2, myFaction.FactionId);
				if (relationBetweenPlayerAndFaction.Item1 != MyRelationsBetweenFactions.Enemies && relationBetweenPlayerAndFaction.Item1 != 0)
				{
					int num3 = component.ConvertPirateReputationToChance(relationBetweenPlayerAndFaction.Item2);
					num += num3;
					list.Add(num3);
					list2.Add(num2);
				}
			}
			if (list2.Count <= 0)
			{
				return 0L;
			}
			int num4 = MyRandom.Instance.Next(0, num);
			for (int i = 0; i < list2.Count; i++)
			{
				if (num4 < list[i])
				{
					return list2[i];
				}
				num4 -= list[i];
			}
			return 0L;
		}

		private long GetMoneyRewardForBountyContract(long identityId, long baseRew, long reputationCoeficient)
		{
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				return baseRew;
			}
			MySessionComponentEconomyDefinition economyDefinition = component.EconomyDefinition;
			if (economyDefinition == null)
			{
				return baseRew;
			}
			MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.GetDefinition(economyDefinition.PirateId) as MyFactionDefinition;
			if (myFactionDefinition == null)
			{
				return baseRew;
			}
			MyFaction myFaction = null;
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				if (faction.Value.Tag == myFactionDefinition.Tag)
				{
					myFaction = faction.Value;
					break;
				}
			}
			if (myFaction == null)
			{
				return baseRew;
			}
			Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(identityId, myFaction.FactionId);
			if (relationBetweenPlayerAndFaction.Item1 == MyRelationsBetweenFactions.Enemies)
			{
				return baseRew;
			}
			int num = Math.Max(relationBetweenPlayerAndFaction.Item2 - component.GetFriendlyMin(), 0);
			return baseRew + num * num * reputationCoeficient;
		}

		private int GetReputationRewardForBountyContract(int baseRew)
		{
			return baseRew;
		}

		private MyTimeSpan GetDurationForBountyContract(MyContractTypeHuntDefinition def)
		{
			return MyTimeSpan.FromSeconds(def.DurationMultiplier * def.Duration_BaseTime);
		}

		public bool IsAnyPiratePlayerAvailable()
		{
			MyFactionCollection factions = MySession.Static.Factions;
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component == null)
			{
				return false;
			}
			MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.GetDefinition(m_economyDefinition.PirateId) as MyFactionDefinition;
			if (myFactionDefinition == null)
			{
				return false;
			}
			MyFaction myFaction = null;
			foreach (KeyValuePair<long, MyFaction> item in factions)
			{
				if (item.Value.Tag == myFactionDefinition.Tag)
				{
					myFaction = item.Value;
					break;
				}
			}
			if (myFaction == null)
			{
				return false;
			}
			List<int> list = new List<int>();
			List<long> list2 = new List<long>();
			int num = 0;
			foreach (MyPlayer.PlayerId allPlayer in MySession.Static.Players.GetAllPlayers())
			{
				long num2 = MySession.Static.Players.TryGetIdentityId(allPlayer.SteamId);
				Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = factions.GetRelationBetweenPlayerAndFaction(num2, myFaction.FactionId);
				if (relationBetweenPlayerAndFaction.Item1 != MyRelationsBetweenFactions.Enemies)
				{
					int num3 = component.ConvertPirateReputationToChance(relationBetweenPlayerAndFaction.Item2);
					num += num3;
					list.Add(num3);
					list2.Add(num2);
				}
			}
			return list2.Count > 0;
		}
	}
}
