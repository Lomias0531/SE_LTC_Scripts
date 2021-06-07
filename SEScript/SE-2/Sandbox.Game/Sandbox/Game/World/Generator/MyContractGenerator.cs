using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRage.ObjectBuilder;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyContractGenerator
	{
		private MySessionComponentEconomyDefinition m_economyDefinition;

		public MyContractGenerator(MySessionComponentEconomyDefinition economyDefinition)
		{
			m_economyDefinition = economyDefinition;
		}

		public MyContractCreationResults CreateCustomHaulingContract(out MyContract contract, MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long targetBlockId, MyTimeSpan now)
		{
			contract = null;
			MyContractDeliver myContractDeliver = new MyContractDeliver();
			MyContractBlock myContractBlock = MyEntities.GetEntityById(targetBlockId) as MyContractBlock;
			if (myContractBlock == null)
			{
				return MyContractCreationResults.Fail_BlockNotFound;
			}
			if (myContractBlock.OwnerId != startBlock.OwnerId)
			{
				return MyContractCreationResults.Fail_NotAnOwnerOfBlock;
			}
			if (startBlock == null || myContractBlock == null)
			{
				return MyContractCreationResults.Error;
			}
			double deliverDistance = (startBlock.PositionComp.GetPosition() - myContractBlock.PositionComp.GetPosition()).Length();
			MyObjectBuilder_ContractDeliver myObjectBuilder_ContractDeliver = new MyObjectBuilder_ContractDeliver();
			myObjectBuilder_ContractDeliver.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractDeliver.IsPlayerMade = true;
			myObjectBuilder_ContractDeliver.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractDeliver.RewardMoney = rewardMoney;
			myObjectBuilder_ContractDeliver.RewardReputation = 0;
			myObjectBuilder_ContractDeliver.StartingDeposit = startingDeposit;
			myObjectBuilder_ContractDeliver.FailReputationPrice = 0;
			myObjectBuilder_ContractDeliver.StartFaction = 0L;
			myObjectBuilder_ContractDeliver.StartStation = 0L;
			myObjectBuilder_ContractDeliver.StartBlock = startBlock.EntityId;
			myObjectBuilder_ContractDeliver.Creation = now.Ticks;
			if (durationInMin > 0)
			{
				myObjectBuilder_ContractDeliver.RemainingTimeInS = MyTimeSpan.FromMinutes(durationInMin).Seconds;
			}
			else
			{
				myObjectBuilder_ContractDeliver.RemainingTimeInS = null;
			}
			myObjectBuilder_ContractDeliver.TicksToDiscard = null;
			myObjectBuilder_ContractDeliver.DeliverDistance = deliverDistance;
			int num = 0;
			MyObjectBuilder_ContractConditionDeliverPackage contractCondition = new MyObjectBuilder_ContractConditionDeliverPackage
			{
				Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION),
				ContractId = myObjectBuilder_ContractDeliver.Id,
				FactionEndId = 0L,
				StationEndId = 0L,
				BlockEndId = myContractBlock.EntityId,
				SubId = num,
				IsFinished = false
			};
			num++;
			myObjectBuilder_ContractDeliver.ContractCondition = contractCondition;
			myContractDeliver.Init(myObjectBuilder_ContractDeliver);
			contract = myContractDeliver;
			return MyContractCreationResults.Success;
		}

		public MyContractCreationResults CreateCustomAcquisitionContract(out MyContract contract, MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long targetBlockId, MyDefinitionId itemType, int itemAmount, MyTimeSpan now)
		{
			contract = null;
			MyContractObtainAndDeliver myContractObtainAndDeliver = new MyContractObtainAndDeliver();
			long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			if (MyDefinitionManager.Static.GetPhysicalItemDefinition(itemType) == null)
			{
				return MyContractCreationResults.Error;
			}
			MyContractBlock myContractBlock = MyEntities.GetEntityById(targetBlockId) as MyContractBlock;
			if (myContractBlock == null)
			{
				return MyContractCreationResults.Fail_BlockNotFound;
			}
			if (myContractBlock.OwnerId != startBlock.OwnerId)
			{
				return MyContractCreationResults.Fail_NotAnOwnerOfBlock;
			}
			int num2 = 0;
			MyObjectBuilder_ContractConditionDeliverItems contractCondition = new MyObjectBuilder_ContractConditionDeliverItems
			{
				Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION),
				ContractId = num,
				FactionEndId = 0L,
				StationEndId = 0L,
				BlockEndId = myContractBlock.EntityId,
				SubId = num2,
				IsFinished = false,
				TransferItems = true,
				ItemType = itemType,
				ItemAmount = itemAmount
			};
			num2++;
			MyObjectBuilder_ContractObtainAndDeliver myObjectBuilder_ContractObtainAndDeliver = new MyObjectBuilder_ContractObtainAndDeliver();
			myObjectBuilder_ContractObtainAndDeliver.Id = num;
			myObjectBuilder_ContractObtainAndDeliver.IsPlayerMade = true;
			myObjectBuilder_ContractObtainAndDeliver.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractObtainAndDeliver.RewardMoney = rewardMoney;
			myObjectBuilder_ContractObtainAndDeliver.RewardReputation = 0;
			myObjectBuilder_ContractObtainAndDeliver.StartingDeposit = startingDeposit;
			myObjectBuilder_ContractObtainAndDeliver.FailReputationPrice = 0;
			myObjectBuilder_ContractObtainAndDeliver.StartFaction = 0L;
			myObjectBuilder_ContractObtainAndDeliver.StartStation = 0L;
			myObjectBuilder_ContractObtainAndDeliver.StartBlock = startBlock.EntityId;
			myObjectBuilder_ContractObtainAndDeliver.Creation = now.Ticks;
			if (durationInMin > 0)
			{
				myObjectBuilder_ContractObtainAndDeliver.RemainingTimeInS = MyTimeSpan.FromMinutes(durationInMin).Seconds;
			}
			else
			{
				myObjectBuilder_ContractObtainAndDeliver.RemainingTimeInS = null;
			}
			myObjectBuilder_ContractObtainAndDeliver.TicksToDiscard = null;
			myObjectBuilder_ContractObtainAndDeliver.ContractCondition = contractCondition;
			myContractObtainAndDeliver.Init(myObjectBuilder_ContractObtainAndDeliver);
			contract = myContractObtainAndDeliver;
			return MyContractCreationResults.Success;
		}

		public MyContractCreationResults CreateCustomEscortContract(out MyContract contract, MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, Vector3D start, Vector3D end, long escortOwner, MyTimeSpan now)
		{
			_ = MySession.Static.Factions;
			contract = null;
			MyContractEscort myContractEscort = new MyContractEscort();
			MyContractTypeEscortDefinition myContractTypeEscortDefinition = myContractEscort.GetDefinition() as MyContractTypeEscortDefinition;
			if (myContractTypeEscortDefinition == null)
			{
				return MyContractCreationResults.Error;
			}
			double pathLength = (start - end).Length();
			long num = 0L;
			num = GetPirateFactionId();
			if (num == 0L)
			{
				return MyContractCreationResults.Error;
			}
			MyObjectBuilder_ContractEscort myObjectBuilder_ContractEscort = new MyObjectBuilder_ContractEscort();
			myObjectBuilder_ContractEscort.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractEscort.IsPlayerMade = true;
			myObjectBuilder_ContractEscort.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractEscort.RewardMoney = rewardMoney;
			myObjectBuilder_ContractEscort.RewardReputation = 0;
			myObjectBuilder_ContractEscort.StartingDeposit = startingDeposit;
			myObjectBuilder_ContractEscort.FailReputationPrice = 0;
			myObjectBuilder_ContractEscort.StartFaction = 0L;
			myObjectBuilder_ContractEscort.StartStation = 0L;
			myObjectBuilder_ContractEscort.StartBlock = startBlock.EntityId;
			myObjectBuilder_ContractEscort.GridId = 0L;
			myObjectBuilder_ContractEscort.StartPosition = start;
			myObjectBuilder_ContractEscort.EndPosition = end;
			myObjectBuilder_ContractEscort.PathLength = pathLength;
			myObjectBuilder_ContractEscort.RewardRadius = myContractTypeEscortDefinition.RewardRadius;
			myObjectBuilder_ContractEscort.TriggerEntityId = 0L;
			myObjectBuilder_ContractEscort.TriggerRadius = myContractTypeEscortDefinition.TriggerRadius;
			myObjectBuilder_ContractEscort.DroneFirstDelay = MyTimeSpan.FromSeconds((num == 0L) ? int.MaxValue : myContractTypeEscortDefinition.DroneFirstDelayInS).Ticks;
			myObjectBuilder_ContractEscort.DroneAttackPeriod = MyTimeSpan.FromSeconds((num == 0L) ? int.MaxValue : myContractTypeEscortDefinition.DroneAttackPeriodInS).Ticks;
			myObjectBuilder_ContractEscort.DronesPerWave = ((num != 0L) ? myContractTypeEscortDefinition.DronesPerWave : 0);
			myObjectBuilder_ContractEscort.InitialDelay = MyTimeSpan.FromSeconds(myContractTypeEscortDefinition.InitialDelayInS).Ticks;
			myObjectBuilder_ContractEscort.WaveFactionId = num;
			myObjectBuilder_ContractEscort.EscortShipOwner = escortOwner;
			myObjectBuilder_ContractEscort.Creation = now.Ticks;
			if (durationInMin > 0)
			{
				myObjectBuilder_ContractEscort.RemainingTimeInS = MyTimeSpan.FromMinutes(durationInMin).Seconds;
			}
			else
			{
				myObjectBuilder_ContractEscort.RemainingTimeInS = null;
			}
			myObjectBuilder_ContractEscort.TicksToDiscard = null;
			myContractEscort.Init(myObjectBuilder_ContractEscort);
			contract = myContractEscort;
			return MyContractCreationResults.Success;
		}

		public MyContractCreationResults CreateCustomSearchContract(out MyContract contract, MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long targetGridId, double searchRadius, MyTimeSpan now)
		{
			contract = null;
			MyContractFind myContractFind = new MyContractFind();
			MyContractTypeFindDefinition myContractTypeFindDefinition = myContractFind.GetDefinition() as MyContractTypeFindDefinition;
			if (myContractTypeFindDefinition == null)
			{
				return MyContractCreationResults.Error;
			}
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(targetGridId) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return MyContractCreationResults.Fail_GridNotFound;
			}
			if (!myCubeGrid.BigOwners.Contains(startBlock.OwnerId))
			{
				return MyContractCreationResults.Fail_NotAnOwnerOfGrid;
			}
			Vector3D vector3D = new BoundingSphereD(myCubeGrid.PositionComp.GetPosition(), searchRadius).RandomToUniformPointInSphere(MyRandom.Instance.NextFloat(), MyRandom.Instance.NextFloat(), MyRandom.Instance.NextFloat());
			double gpsDistance = (vector3D - startBlock.PositionComp.GetPosition()).Length();
			MyObjectBuilder_ContractFind myObjectBuilder_ContractFind = new MyObjectBuilder_ContractFind();
			myObjectBuilder_ContractFind.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractFind.IsPlayerMade = true;
			myObjectBuilder_ContractFind.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractFind.RewardMoney = rewardMoney;
			myObjectBuilder_ContractFind.RewardReputation = 0;
			myObjectBuilder_ContractFind.StartingDeposit = startingDeposit;
			myObjectBuilder_ContractFind.FailReputationPrice = 0;
			myObjectBuilder_ContractFind.StartFaction = 0L;
			myObjectBuilder_ContractFind.StartStation = 0L;
			myObjectBuilder_ContractFind.StartBlock = startBlock.EntityId;
			myObjectBuilder_ContractFind.GridPosition = startBlock.PositionComp.GetPosition();
			myObjectBuilder_ContractFind.GpsPosition = vector3D;
			myObjectBuilder_ContractFind.GpsDistance = gpsDistance;
			myObjectBuilder_ContractFind.MaxGpsOffset = (float)searchRadius;
			myObjectBuilder_ContractFind.TriggerRadius = myContractTypeFindDefinition.TriggerRadius;
			myObjectBuilder_ContractFind.GridId = myCubeGrid.EntityId;
			myObjectBuilder_ContractFind.KeepGridAtTheEnd = true;
			myObjectBuilder_ContractFind.Creation = now.Ticks;
			if (durationInMin > 0)
			{
				myObjectBuilder_ContractFind.RemainingTimeInS = MyTimeSpan.FromMinutes(durationInMin).Seconds;
			}
			else
			{
				myObjectBuilder_ContractFind.RemainingTimeInS = null;
			}
			myObjectBuilder_ContractFind.TicksToDiscard = null;
			myContractFind.Init(myObjectBuilder_ContractFind);
			contract = myContractFind;
			return MyContractCreationResults.Success;
		}

		public MyContractCreationResults CreateCustomBountyContract(out MyContract contract, MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long targetIdentityId, MyTimeSpan now)
		{
			contract = null;
			MyContractHunt myContractHunt = new MyContractHunt();
			MyContractTypeHuntDefinition myContractTypeHuntDefinition = myContractHunt.GetDefinition() as MyContractTypeHuntDefinition;
			if (myContractTypeHuntDefinition == null)
			{
				return MyContractCreationResults.Error;
			}
			MyObjectBuilder_ContractHunt myObjectBuilder_ContractHunt = new MyObjectBuilder_ContractHunt();
			myObjectBuilder_ContractHunt.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractHunt.IsPlayerMade = true;
			myObjectBuilder_ContractHunt.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractHunt.RewardMoney = rewardMoney;
			myObjectBuilder_ContractHunt.RewardReputation = 0;
			myObjectBuilder_ContractHunt.StartingDeposit = startingDeposit;
			myObjectBuilder_ContractHunt.FailReputationPrice = 0;
			myObjectBuilder_ContractHunt.StartFaction = 0L;
			myObjectBuilder_ContractHunt.StartStation = 0L;
			myObjectBuilder_ContractHunt.StartBlock = startBlock.EntityId;
			myObjectBuilder_ContractHunt.Target = targetIdentityId;
			myObjectBuilder_ContractHunt.RemarkPeriod = MyTimeSpan.FromSeconds(myContractTypeHuntDefinition.RemarkPeriodInS).Ticks;
			myObjectBuilder_ContractHunt.RemarkVariance = myContractTypeHuntDefinition.RemarkVariance;
			myObjectBuilder_ContractHunt.KillRange = myContractTypeHuntDefinition.KillRange;
			myObjectBuilder_ContractHunt.KillRangeMultiplier = myContractTypeHuntDefinition.KillRangeMultiplier;
			myObjectBuilder_ContractHunt.ReputationLossForTarget = myContractTypeHuntDefinition.ReputationLossForTarget;
			myObjectBuilder_ContractHunt.RewardRadius = myContractTypeHuntDefinition.RewardRadius;
			myObjectBuilder_ContractHunt.Creation = now.Ticks;
			myObjectBuilder_ContractHunt.RemainingTimeInS = MyTimeSpan.FromMinutes(durationInMin).Seconds;
			myObjectBuilder_ContractHunt.TicksToDiscard = null;
			myContractHunt.Init(myObjectBuilder_ContractHunt);
			contract = myContractHunt;
			return MyContractCreationResults.Success;
		}

		public MyContractCreationResults CreateCustomRepairContract(out MyContract contract, MyContractBlock startBlock, int rewardMoney, int startingDeposit, int durationInMin, long gridId, MyTimeSpan now)
		{
			_ = MySession.Static.Factions;
			contract = null;
			MyContractRepair myContractRepair = new MyContractRepair();
			if (!(myContractRepair.GetDefinition() is MyContractTypeRepairDefinition))
			{
				return MyContractCreationResults.Error;
			}
			MyObjectBuilder_ContractRepair myObjectBuilder_ContractRepair = new MyObjectBuilder_ContractRepair();
			myObjectBuilder_ContractRepair.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractRepair.IsPlayerMade = true;
			myObjectBuilder_ContractRepair.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractRepair.RewardMoney = rewardMoney;
			myObjectBuilder_ContractRepair.RewardReputation = 0;
			myObjectBuilder_ContractRepair.StartingDeposit = startingDeposit;
			myObjectBuilder_ContractRepair.FailReputationPrice = 0;
			myObjectBuilder_ContractRepair.StartFaction = 0L;
			myObjectBuilder_ContractRepair.StartStation = 0L;
			myObjectBuilder_ContractRepair.StartBlock = startBlock.EntityId;
			myObjectBuilder_ContractRepair.GridPosition = Vector3D.Zero;
			myObjectBuilder_ContractRepair.GridId = gridId;
			myObjectBuilder_ContractRepair.PrefabName = string.Empty;
			myObjectBuilder_ContractRepair.BlocksToRepair = new MySerializableList<Vector3I>();
			myObjectBuilder_ContractRepair.KeepGridAtTheEnd = true;
			myObjectBuilder_ContractRepair.UnrepairedBlockCount = 0;
			myObjectBuilder_ContractRepair.Creation = now.Ticks;
			if (durationInMin > 0)
			{
				myObjectBuilder_ContractRepair.RemainingTimeInS = MyTimeSpan.FromMinutes(durationInMin).Seconds;
			}
			else
			{
				myObjectBuilder_ContractRepair.RemainingTimeInS = null;
			}
			myObjectBuilder_ContractRepair.TicksToDiscard = null;
			myContractRepair.Init(myObjectBuilder_ContractRepair);
			contract = myContractRepair;
			return MyContractCreationResults.Success;
		}

		public MyContractCreationResults CreateCustomCustomContract(out MyContract contract, MyDefinitionId definitionId, string name, string description, MyContractBlock startBlock, int rewardMoney, int startingDeposit, int reputationReward, int failReputationPrice, int durationInMin, MyTimeSpan now, MyContractBlock endBlock = null)
		{
			contract = null;
			MyContractCustom myContractCustom = new MyContractCustom();
			MyObjectBuilder_ContractCustom myObjectBuilder_ContractCustom = new MyObjectBuilder_ContractCustom();
			myObjectBuilder_ContractCustom.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractCustom.IsPlayerMade = true;
			myObjectBuilder_ContractCustom.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractCustom.RewardMoney = rewardMoney;
			myObjectBuilder_ContractCustom.RewardReputation = reputationReward;
			myObjectBuilder_ContractCustom.StartingDeposit = startingDeposit;
			myObjectBuilder_ContractCustom.FailReputationPrice = failReputationPrice;
			myObjectBuilder_ContractCustom.DefinitionId = definitionId;
			myObjectBuilder_ContractCustom.ContractName = name;
			myObjectBuilder_ContractCustom.ContractDescription = description;
			myObjectBuilder_ContractCustom.StartFaction = 0L;
			myObjectBuilder_ContractCustom.StartStation = 0L;
			myObjectBuilder_ContractCustom.StartBlock = startBlock.EntityId;
			myObjectBuilder_ContractCustom.Creation = now.Ticks;
			if (durationInMin > 0)
			{
				myObjectBuilder_ContractCustom.RemainingTimeInS = MyTimeSpan.FromMinutes(durationInMin).Seconds;
			}
			else
			{
				myObjectBuilder_ContractCustom.RemainingTimeInS = null;
			}
			myObjectBuilder_ContractCustom.TicksToDiscard = null;
			if (endBlock != null)
			{
				int num = 0;
				MyObjectBuilder_ContractConditionCustom contractCondition = new MyObjectBuilder_ContractConditionCustom
				{
					Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT_CONDITION),
					ContractId = myObjectBuilder_ContractCustom.Id,
					FactionEndId = 0L,
					StationEndId = 0L,
					BlockEndId = endBlock.EntityId,
					SubId = num,
					IsFinished = false
				};
				num++;
				myObjectBuilder_ContractCustom.ContractCondition = contractCondition;
			}
			myContractCustom.Init(myObjectBuilder_ContractCustom);
			contract = myContractCustom;
			return MyContractCreationResults.Success;
		}

		public long GetRandomPlayer()
		{
			ICollection<MyPlayer.PlayerId> allPlayers = MySession.Static.Players.GetAllPlayers();
			if (allPlayers.Count <= 0)
			{
				return 0L;
			}
			int num = MyRandom.Instance.Next(0, allPlayers.Count);
			int num2 = 0;
			foreach (MyPlayer.PlayerId item in allPlayers)
			{
				if (num2 == num)
				{
					return MySession.Static.Players.TryGetIdentityId(item.SteamId);
				}
				num2++;
			}
			return 0L;
		}

		public long GetPirateFactionId()
		{
			MyFactionDefinition myFactionDefinition = MyDefinitionManager.Static.GetDefinition(m_economyDefinition.PirateId) as MyFactionDefinition;
			if (myFactionDefinition == null)
			{
				return 0L;
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
			return myFaction?.FactionId ?? 0;
		}
	}
}
