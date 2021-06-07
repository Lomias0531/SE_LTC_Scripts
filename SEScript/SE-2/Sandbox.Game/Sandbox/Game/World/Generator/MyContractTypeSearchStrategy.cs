using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyContractTypeSearchStrategy : MyContractTypeBaseStrategy
	{
		private static readonly float GRAVITY_SQUARED_EPSILON = 0.0001f;

		public MyContractTypeSearchStrategy(MySessionComponentEconomyDefinition economyDefinition)
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
			int num = 20;
			int num2 = 50;
			MyContractFind myContractFind = new MyContractFind();
			MyContractTypeFindDefinition myContractTypeFindDefinition = myContractFind.GetDefinition() as MyContractTypeFindDefinition;
			if (myContractTypeFindDefinition == null)
			{
				return MyContractCreationResults.Error;
			}
			MyStation myStation = (factions.TryGetFactionById(factionId) as MyFaction)?.GetStationById(stationId);
			if (myStation == null)
			{
				return MyContractCreationResults.Error;
			}
			Vector3D vector3D = Vector3D.Zero;
			Vector3D vector3D2 = Vector3D.Zero;
			double num3 = 0.0;
			double num4 = 0.0;
			double num5 = 0.0;
			if (myStation.Type == MyStationTypeEnum.Outpost)
			{
				BoundingSphereD boundingSphereD = new BoundingSphereD(myStation.Position, myContractTypeFindDefinition.MaxGridDistance);
				bool flag = false;
				int num6 = 0;
				do
				{
					Vector3D? vector3D3 = boundingSphereD.RandomToUniformPointInSphereWithInnerCutout(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), myContractTypeFindDefinition.MinGridDistance);
					if (vector3D3.HasValue)
					{
						MyPlanet closestPlanet = MyPlanets.Static.GetClosestPlanet(vector3D3.Value);
						if (closestPlanet != null)
						{
							float scaleFactor = 10f;
							Vector3D closestSurfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(vector3D3.Value);
							vector3D = closestSurfacePointGlobal + Vector3.Normalize(closestSurfacePointGlobal - closestPlanet.PositionComp.GetPosition()) * scaleFactor;
							Vector3D? vector3D4 = new BoundingSphereD(vector3D, myContractTypeFindDefinition.MaxGpsOffset).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
							if (!vector3D4.HasValue)
							{
								return MyContractCreationResults.Fail_Common;
							}
							vector3D2 = closestPlanet.GetClosestSurfacePointGlobal(vector3D4.Value);
							flag = true;
							break;
						}
					}
					num6++;
				}
				while (num6 <= num);
				if (!flag)
				{
					return MyContractCreationResults.Fail_Common;
				}
			}
			else
			{
				BoundingSphereD boundingSphereD2 = new BoundingSphereD(myStation.Position, myContractTypeFindDefinition.MaxGridDistance);
				bool flag2 = false;
				int num7 = 0;
				do
				{
					Vector3D? vector3D5 = boundingSphereD2.RandomToUniformPointInSphereWithInnerCutout(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), myContractTypeFindDefinition.MinGridDistance);
					if (!vector3D5.HasValue)
					{
						continue;
					}
					List<MyObjectSeed> list = new List<MyObjectSeed>();
					MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(vector3D5.Value, num2), list);
					if (list.Count <= 0)
					{
						if (MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D5.Value).LengthSquared() <= GRAVITY_SQUARED_EPSILON)
						{
							flag2 = true;
							vector3D = vector3D5.Value;
							break;
						}
						num7++;
					}
				}
				while (num7 <= num);
				if (!flag2)
				{
					return MyContractCreationResults.Fail_Common;
				}
				Vector3D? vector3D6 = new BoundingSphereD(vector3D, myContractTypeFindDefinition.MaxGpsOffset).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble());
				if (!vector3D6.HasValue)
				{
					return MyContractCreationResults.Fail_Common;
				}
				vector3D2 = vector3D6.Value;
			}
			num5 = myContractTypeFindDefinition.MaxGpsOffset;
			num3 = (vector3D - myStation.Position).Length();
			num4 = (vector3D2 - myStation.Position).Length();
			MyObjectBuilder_ContractFind myObjectBuilder_ContractFind = new MyObjectBuilder_ContractFind();
			myObjectBuilder_ContractFind.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractFind.IsPlayerMade = false;
			myObjectBuilder_ContractFind.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractFind.RewardMoney = GetMoneyRewardForSearchContract(myContractTypeFindDefinition.MinimumMoney, num3, myContractTypeFindDefinition.MinGridDistance);
			if (myStation.IsDeepSpaceStation)
			{
				myObjectBuilder_ContractFind.RewardMoney = (long)((float)myObjectBuilder_ContractFind.RewardMoney * m_economyDefinition.DeepSpaceStationContractBonus);
			}
			myObjectBuilder_ContractFind.RewardReputation = GetReputationRewardForSearchContract(myContractTypeFindDefinition.MinimumReputation, num3, num5);
			myObjectBuilder_ContractFind.StartingDeposit = (long)(MyRandom.Instance.NextDouble() * (double)(myContractTypeFindDefinition.MaxStartingDeposit - myContractTypeFindDefinition.MinStartingDeposit));
			myObjectBuilder_ContractFind.FailReputationPrice = myContractTypeFindDefinition.FailReputationPrice;
			myObjectBuilder_ContractFind.StartFaction = factionId;
			myObjectBuilder_ContractFind.StartStation = stationId;
			myObjectBuilder_ContractFind.StartBlock = 0L;
			myObjectBuilder_ContractFind.GridPosition = vector3D;
			myObjectBuilder_ContractFind.GpsPosition = vector3D2;
			myObjectBuilder_ContractFind.GpsDistance = num4;
			myObjectBuilder_ContractFind.MaxGpsOffset = (float)myContractTypeFindDefinition.MaxGpsOffset;
			myObjectBuilder_ContractFind.TriggerRadius = myContractTypeFindDefinition.TriggerRadius;
			myObjectBuilder_ContractFind.GridId = 0L;
			myObjectBuilder_ContractFind.KeepGridAtTheEnd = false;
			myObjectBuilder_ContractFind.Creation = now.Ticks;
			myObjectBuilder_ContractFind.RemainingTimeInS = GetDurationForSearchContract(myContractTypeFindDefinition, num3, num5).Seconds;
			myObjectBuilder_ContractFind.TicksToDiscard = MyContractTypeBaseStrategy.TICKS_TO_LIVE;
			myContractFind.Init(myObjectBuilder_ContractFind);
			contract = myContractFind;
			return MyContractCreationResults.Success;
		}

		private long GetMoneyRewardForSearchContract(long baseRew, double distance, double minDistance)
		{
			return (long)((double)baseRew + 1000.0 * Math.Pow(distance / minDistance, 2.2));
		}

		private int GetReputationRewardForSearchContract(int baseRew, double distance, double searchAreaRadius)
		{
			return baseRew;
		}

		private MyTimeSpan GetDurationForSearchContract(MyContractTypeFindDefinition def, double distanceInM, double searchAreaRadius, bool cutOutJumps = true)
		{
			double num = searchAreaRadius / 1000.0;
			return MyTimeSpan.FromSeconds(def.DurationMultiplier * (def.Duration_BaseTime + (distanceInM * def.Duration_TimePerMeter + 3.1415929794311523 * num * num * num * def.Duration_TimePerCubicKm)));
		}
	}
}
