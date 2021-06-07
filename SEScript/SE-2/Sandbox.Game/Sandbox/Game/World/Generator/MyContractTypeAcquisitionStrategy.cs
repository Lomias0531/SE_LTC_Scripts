using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Multiplayer;
using System;
using VRage;
using VRage.Collections;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World.Generator
{
	public class MyContractTypeAcquisitionStrategy : MyContractTypeBaseStrategy
	{
		public MyContractTypeAcquisitionStrategy(MySessionComponentEconomyDefinition economyDefinition)
			: base(economyDefinition)
		{
		}

		public override bool CanBeGenerated(MyStation station, MyFaction faction)
		{
			return true;
		}

		public override MyContractCreationResults GenerateContract(out MyContract contract, long factionId, long stationId, MyMinimalPriceCalculator calculator, MyTimeSpan now)
		{
			MyFactionCollection factions = MySession.Static.Factions;
			contract = null;
			MyContractObtainAndDeliver myContractObtainAndDeliver = new MyContractObtainAndDeliver();
			MyContractTypeObtainAndDeliverDefinition myContractTypeObtainAndDeliverDefinition = myContractObtainAndDeliver.GetDefinition() as MyContractTypeObtainAndDeliverDefinition;
			if (myContractTypeObtainAndDeliverDefinition == null)
			{
				return MyContractCreationResults.Error;
			}
			MyStation myStation = (factions.TryGetFactionById(factionId) as MyFaction)?.GetStationById(stationId);
			if (myStation == null)
			{
				return MyContractCreationResults.Error;
			}
			long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			long num2 = 0L;
			if (!GenerateObtainAndDeliverItem(factionId, stationId, calculator, out MyPhysicalItemDefinition itemType, out int itemAmount, out float itemVolume, out long itemPrice))
			{
				return MyContractCreationResults.Error;
			}
			int num3 = 0;
			MyObjectBuilder_ContractConditionDeliverItems contractCondition = new MyObjectBuilder_ContractConditionDeliverItems
			{
				Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION),
				ContractId = num,
				FactionEndId = factionId,
				StationEndId = stationId,
				BlockEndId = 0L,
				SubId = num3,
				IsFinished = false,
				TransferItems = false,
				ItemType = itemType.Id,
				ItemAmount = itemAmount,
				ItemVolume = itemVolume
			};
			num3++;
			num2 += itemPrice;
			MyObjectBuilder_ContractObtainAndDeliver myObjectBuilder_ContractObtainAndDeliver = new MyObjectBuilder_ContractObtainAndDeliver();
			myObjectBuilder_ContractObtainAndDeliver.Id = num;
			myObjectBuilder_ContractObtainAndDeliver.IsPlayerMade = false;
			myObjectBuilder_ContractObtainAndDeliver.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractObtainAndDeliver.RewardMoney = GetMoneyRewardForAcquisitionContract(myContractTypeObtainAndDeliverDefinition.MinimumMoney, itemAmount) + num2;
			if (myStation.IsDeepSpaceStation)
			{
				myObjectBuilder_ContractObtainAndDeliver.RewardMoney = (long)((float)myObjectBuilder_ContractObtainAndDeliver.RewardMoney * m_economyDefinition.DeepSpaceStationContractBonus);
			}
			myObjectBuilder_ContractObtainAndDeliver.RewardReputation = GetReputationRewardForAcquisitionContract(myContractTypeObtainAndDeliverDefinition.MinimumReputation);
			myObjectBuilder_ContractObtainAndDeliver.StartingDeposit = (long)(MyRandom.Instance.NextDouble() * (double)(myContractTypeObtainAndDeliverDefinition.MaxStartingDeposit - myContractTypeObtainAndDeliverDefinition.MinStartingDeposit));
			myObjectBuilder_ContractObtainAndDeliver.FailReputationPrice = myContractTypeObtainAndDeliverDefinition.FailReputationPrice;
			myObjectBuilder_ContractObtainAndDeliver.StartFaction = factionId;
			myObjectBuilder_ContractObtainAndDeliver.StartStation = stationId;
			myObjectBuilder_ContractObtainAndDeliver.StartBlock = 0L;
			myObjectBuilder_ContractObtainAndDeliver.Creation = now.Ticks;
			myObjectBuilder_ContractObtainAndDeliver.RemainingTimeInS = null;
			myObjectBuilder_ContractObtainAndDeliver.TicksToDiscard = MyContractTypeBaseStrategy.TICKS_TO_LIVE;
			myObjectBuilder_ContractObtainAndDeliver.ContractCondition = contractCondition;
			myContractObtainAndDeliver.Init(myObjectBuilder_ContractObtainAndDeliver);
			contract = myContractObtainAndDeliver;
			return MyContractCreationResults.Success;
		}

		private bool GenerateObtainAndDeliverItem(long factionId, long stationId, MyMinimalPriceCalculator calculator, out MyPhysicalItemDefinition itemType, out int itemAmount, out float itemVolume, out long itemPrice)
		{
			long num = 25L;
			float baseCostProductionSpeedMultiplier = 1f;
			MyContractTypeObtainAndDeliverDefinition myContractTypeObtainAndDeliverDefinition = new MyContractObtainAndDeliver().GetDefinition() as MyContractTypeObtainAndDeliverDefinition;
			if (myContractTypeObtainAndDeliverDefinition != null)
			{
				int num2 = 0;
				do
				{
					int num3 = MyRandom.Instance.Next(0, myContractTypeObtainAndDeliverDefinition.AvailableItems.Count);
					MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(myContractTypeObtainAndDeliverDefinition.AvailableItems[(num3 + num2) % myContractTypeObtainAndDeliverDefinition.AvailableItems.Count]);
					if (physicalItemDefinition != null)
					{
						itemType = physicalItemDefinition;
						itemAmount = MyRandom.Instance.Next(physicalItemDefinition.MinimumAcquisitionAmount, physicalItemDefinition.MaximumAcquisitionAmount + 1);
						itemVolume = physicalItemDefinition.Volume * (float)itemAmount;
						int minimalPrice = 0;
						if (calculator.TryGetItemMinimalPrice(itemType.Id, out minimalPrice))
						{
							itemPrice = minimalPrice * itemAmount;
						}
						else
						{
							calculator.CalculateMinimalPrices(new SerializableDefinitionId[1]
							{
								itemType.Id
							}, baseCostProductionSpeedMultiplier);
							if (calculator.TryGetItemMinimalPrice(itemType.Id, out minimalPrice))
							{
								itemPrice = minimalPrice * itemAmount;
							}
							else
							{
								itemPrice = num * itemAmount;
							}
						}
						return true;
					}
					num2++;
				}
				while (num2 < myContractTypeObtainAndDeliverDefinition.AvailableItems.Count);
			}
			ListReader<MyPhysicalItemDefinition> physicalItemDefinitions = MyDefinitionManager.Static.GetPhysicalItemDefinitions();
			if (physicalItemDefinitions.Count <= 0)
			{
				itemType = null;
				itemAmount = 0;
				itemVolume = 0f;
				itemPrice = 0L;
				return false;
			}
			itemType = physicalItemDefinitions[MyRandom.Instance.Next(0, physicalItemDefinitions.Count)];
			itemAmount = 100;
			itemVolume = (float)itemAmount * itemType.Volume;
			itemPrice = 500L;
			return true;
		}

		private long GetMoneyRewardForAcquisitionContract(long baseRew, int amount)
		{
			return (long)((double)baseRew * Math.Pow(2.0, Math.Log10(amount)));
		}

		private int GetReputationRewardForAcquisitionContract(int baseRew)
		{
			return baseRew;
		}
	}
}
