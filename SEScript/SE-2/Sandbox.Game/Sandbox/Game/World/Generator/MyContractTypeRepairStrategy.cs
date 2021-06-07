using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
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
	public class MyContractTypeRepairStrategy : MyContractTypeBaseStrategy
	{
		private static readonly float GRAVITY_SQUARED_EPSILON = 0.0001f;

		public MyContractTypeRepairStrategy(MySessionComponentEconomyDefinition economyDefinition)
			: base(economyDefinition)
		{
		}

		public override bool CanBeGenerated(MyStation station, MyFaction faction)
		{
			if (station.Type == MyStationTypeEnum.Outpost)
			{
				return false;
			}
			return true;
		}

		public override MyContractCreationResults GenerateContract(out MyContract contract, long factionId, long stationId, MyMinimalPriceCalculator calculator, MyTimeSpan now)
		{
			MyFactionCollection factions = MySession.Static.Factions;
			contract = null;
			int num = 20;
			int num2 = 50;
			MyContractRepair myContractRepair = new MyContractRepair();
			MyContractTypeRepairDefinition myContractTypeRepairDefinition = myContractRepair.GetDefinition() as MyContractTypeRepairDefinition;
			if (myContractTypeRepairDefinition == null)
			{
				return MyContractCreationResults.Error;
			}
			MyStation myStation = (factions.TryGetFactionById(factionId) as MyFaction)?.GetStationById(stationId);
			if (myStation == null)
			{
				return MyContractCreationResults.Error;
			}
			if (myStation.Type == MyStationTypeEnum.Outpost)
			{
				return MyContractCreationResults.Fail_Impossible;
			}
			Vector3D vector3D = Vector3D.Zero;
			BoundingSphereD boundingSphereD = new BoundingSphereD(myStation.Position, myContractTypeRepairDefinition.MaxGridDistance);
			bool flag = false;
			int num3 = 0;
			do
			{
				Vector3D? vector3D2 = boundingSphereD.RandomToUniformPointInSphereWithInnerCutout(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), myContractTypeRepairDefinition.MinGridDistance);
				if (!vector3D2.HasValue)
				{
					continue;
				}
				List<MyObjectSeed> list = new List<MyObjectSeed>();
				MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(vector3D2.Value, num2), list);
				if (list.Count <= 0)
				{
					if (MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D2.Value).LengthSquared() <= GRAVITY_SQUARED_EPSILON)
					{
						flag = true;
						vector3D = vector3D2.Value;
						break;
					}
					num3++;
				}
			}
			while (num3 <= num);
			if (!flag)
			{
				return MyContractCreationResults.Fail_Common;
			}
			double gridDistance = (vector3D - myStation.Position).Length();
			string text = "";
			int gridPrice = 0;
			int num4 = 0;
			if (myContractTypeRepairDefinition.PrefabNames.Count > 0)
			{
				text = myContractTypeRepairDefinition.PrefabNames[MyRandom.Instance.Next(0, myContractTypeRepairDefinition.PrefabNames.Count)];
			}
			if (!GetRepairDataFromPrefab(text, calculator, out gridPrice))
			{
				calculator.CalculatePrefabInformation(new string[1]
				{
					text
				});
				if (!GetRepairDataFromPrefab(text, calculator, out gridPrice))
				{
					return MyContractCreationResults.Fail_Common;
				}
			}
			num4 = (int)((myContractTypeRepairDefinition.TimeToPriceDenominator != 0f) ? ((float)gridPrice / myContractTypeRepairDefinition.TimeToPriceDenominator) : 0f);
			float num5 = MyRandom.Instance.NextFloat(-1f, 1f) * myContractTypeRepairDefinition.PriceSpread;
			int num6 = (int)((1f + num5) * (float)gridPrice);
			MyObjectBuilder_ContractRepair myObjectBuilder_ContractRepair = new MyObjectBuilder_ContractRepair();
			myObjectBuilder_ContractRepair.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractRepair.IsPlayerMade = false;
			myObjectBuilder_ContractRepair.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractRepair.RewardMoney = GetMoneyRewardForRepairContract(myContractTypeRepairDefinition.MinimumMoney, gridDistance, num6, myContractTypeRepairDefinition.PriceToRewardCoeficient);
			if (myStation.IsDeepSpaceStation)
			{
				myObjectBuilder_ContractRepair.RewardMoney = (long)((float)myObjectBuilder_ContractRepair.RewardMoney * m_economyDefinition.DeepSpaceStationContractBonus);
			}
			myObjectBuilder_ContractRepair.RewardReputation = GetReputationRewardForRepairContract(myContractTypeRepairDefinition.MinimumReputation);
			myObjectBuilder_ContractRepair.StartingDeposit = (long)(MyRandom.Instance.NextDouble() * (double)(myContractTypeRepairDefinition.MaxStartingDeposit - myContractTypeRepairDefinition.MinStartingDeposit));
			myObjectBuilder_ContractRepair.FailReputationPrice = myContractTypeRepairDefinition.FailReputationPrice;
			myObjectBuilder_ContractRepair.StartFaction = factionId;
			myObjectBuilder_ContractRepair.StartStation = stationId;
			myObjectBuilder_ContractRepair.StartBlock = 0L;
			myObjectBuilder_ContractRepair.GridPosition = vector3D;
			myObjectBuilder_ContractRepair.GridId = 0L;
			myObjectBuilder_ContractRepair.PrefabName = text;
			myObjectBuilder_ContractRepair.BlocksToRepair = new MySerializableList<Vector3I>();
			myObjectBuilder_ContractRepair.KeepGridAtTheEnd = false;
			myObjectBuilder_ContractRepair.UnrepairedBlockCount = 0;
			myObjectBuilder_ContractRepair.Creation = now.Ticks;
			myObjectBuilder_ContractRepair.RemainingTimeInS = GetDurationForRepairContract(myContractTypeRepairDefinition, gridDistance, num4).Seconds;
			myObjectBuilder_ContractRepair.TicksToDiscard = MyContractTypeBaseStrategy.TICKS_TO_LIVE;
			myContractRepair.Init(myObjectBuilder_ContractRepair);
			contract = myContractRepair;
			return MyContractCreationResults.Success;
		}

		private bool GetRepairDataFromPrefab(string prefabName, MyMinimalPriceCalculator calculator, out int gridPrice)
		{
			gridPrice = 0;
			if (string.IsNullOrEmpty(prefabName))
			{
				return false;
			}
			if (!calculator.TryGetPrefabMinimalRepairPrice(prefabName, out gridPrice))
			{
				return false;
			}
			return true;
		}

		private long GetMoneyRewardForRepairContract(long baseRew, double gridDistance, long gridPrice, float gridPriceToRewardcoef)
		{
			return (long)((double)baseRew * Math.Pow(2.0, Math.Log10(gridDistance))) + (long)(gridPriceToRewardcoef * (float)gridPrice);
		}

		private int GetReputationRewardForRepairContract(int baseRew)
		{
			return baseRew;
		}

		private MyTimeSpan GetDurationForRepairContract(MyContractTypeRepairDefinition def, double gridDistance, int repairComponentTimeInS)
		{
			return MyTimeSpan.FromSeconds(def.DurationMultiplier * (def.Duration_BaseTime + gridDistance * def.Duration_TimePerMeter + (double)repairComponentTimeInS));
		}
	}
}
