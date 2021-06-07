using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Multiplayer;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.World.Generator
{
	public class MyContractTypeHaulingStrategy : MyContractTypeBaseStrategy
	{
		private readonly double JUMP_DRIVE_DISTANCE = 2000000.0;

		private readonly float AMOUNT_URANIUM_TO_RECHARGE = 3.75f;

		private MyDefinitionId m_uranium = new MyDefinitionId(typeof(MyObjectBuilder_Ingot), "Uranium");

		public MyContractTypeHaulingStrategy(MySessionComponentEconomyDefinition economyDefinition)
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
			MyContractDeliver myContractDeliver = new MyContractDeliver();
			MyContractTypeDeliverDefinition myContractTypeDeliverDefinition = myContractDeliver.GetDefinition() as MyContractTypeDeliverDefinition;
			if (myContractTypeDeliverDefinition == null)
			{
				return MyContractCreationResults.Error;
			}
			MyStation myStation = (factions.TryGetFactionById(factionId) as MyFaction)?.GetStationById(stationId);
			if (myStation == null)
			{
				return MyContractCreationResults.Error;
			}
			if (!factions.GetRandomFriendlyStation(factionId, stationId, out MyFaction friendlyFaction, out MyStation friendlyStation, includeSameFaction: true))
			{
				return MyContractCreationResults.Fail_Impossible;
			}
			double num = (friendlyStation.Position - myStation.Position).Length();
			MyObjectBuilder_ContractDeliver myObjectBuilder_ContractDeliver = new MyObjectBuilder_ContractDeliver();
			myObjectBuilder_ContractDeliver.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractDeliver.IsPlayerMade = false;
			myObjectBuilder_ContractDeliver.State = MyContractStateEnum.Inactive;
			int minimalPrice = 0;
			if (!calculator.TryGetItemMinimalPrice(m_uranium, out minimalPrice))
			{
				calculator.CalculateMinimalPrices(new SerializableDefinitionId[1]
				{
					m_uranium
				});
				calculator.TryGetItemMinimalPrice(m_uranium, out minimalPrice);
			}
			myObjectBuilder_ContractDeliver.RewardMoney = GetMoneyRewardForHaulingContract(myContractTypeDeliverDefinition.MinimumMoney, num, minimalPrice);
			if (myStation.IsDeepSpaceStation)
			{
				myObjectBuilder_ContractDeliver.RewardMoney = (long)((float)myObjectBuilder_ContractDeliver.RewardMoney * m_economyDefinition.DeepSpaceStationContractBonus);
			}
			myObjectBuilder_ContractDeliver.RewardReputation = GetReputationRewardForHaulingContract(myContractTypeDeliverDefinition.MinimumReputation, num);
			myObjectBuilder_ContractDeliver.StartingDeposit = (long)(MyRandom.Instance.NextDouble() * (double)(myContractTypeDeliverDefinition.MaxStartingDeposit - myContractTypeDeliverDefinition.MinStartingDeposit));
			myObjectBuilder_ContractDeliver.FailReputationPrice = myContractTypeDeliverDefinition.FailReputationPrice;
			myObjectBuilder_ContractDeliver.StartFaction = factionId;
			myObjectBuilder_ContractDeliver.StartStation = stationId;
			myObjectBuilder_ContractDeliver.StartBlock = 0L;
			myObjectBuilder_ContractDeliver.Creation = now.Ticks;
			myObjectBuilder_ContractDeliver.RemainingTimeInS = GetDurationForHaulingContract(myContractTypeDeliverDefinition, num).Seconds;
			myObjectBuilder_ContractDeliver.DeliverDistance = num;
			myObjectBuilder_ContractDeliver.TicksToDiscard = MyContractTypeBaseStrategy.TICKS_TO_LIVE;
			int num2 = 0;
			MyObjectBuilder_ContractConditionDeliverPackage contractCondition = new MyObjectBuilder_ContractConditionDeliverPackage
			{
				Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION),
				ContractId = myObjectBuilder_ContractDeliver.Id,
				FactionEndId = friendlyFaction.FactionId,
				StationEndId = friendlyStation.Id,
				BlockEndId = 0L,
				SubId = num2,
				IsFinished = false
			};
			num2++;
			myObjectBuilder_ContractDeliver.ContractCondition = contractCondition;
			myContractDeliver.Init(myObjectBuilder_ContractDeliver);
			contract = myContractDeliver;
			return MyContractCreationResults.Success;
		}

		private long GetMoneyRewardForHaulingContract(long baseRew, double distance, int uraniumPrice)
		{
			double num = distance / JUMP_DRIVE_DISTANCE;
			double num2 = num * (double)((float)uraniumPrice * AMOUNT_URANIUM_TO_RECHARGE);
			return (long)((double)baseRew + (double)baseRew * num + num2);
		}

		private int GetReputationRewardForHaulingContract(int baseRew, double distance)
		{
			return baseRew;
		}

		private MyTimeSpan GetDurationForHaulingContract(MyContractTypeDeliverDefinition def, double distanceInM, bool cutOutJumps = true)
		{
			double num = 0.0;
			if (cutOutJumps)
			{
				int num2 = (int)(distanceInM / JUMP_DRIVE_DISTANCE);
				_ = JUMP_DRIVE_DISTANCE;
				num = def.DurationMultiplier * (def.Duration_BaseTime + (double)(long)(def.Duration_TimePerJumpDist * (double)(num2 + 1)));
			}
			else
			{
				num = def.DurationMultiplier * (def.Duration_BaseTime + (double)(long)(def.Duration_TimePerMeter * distanceInM));
			}
			return MyTimeSpan.FromSeconds(num);
		}
	}
}
