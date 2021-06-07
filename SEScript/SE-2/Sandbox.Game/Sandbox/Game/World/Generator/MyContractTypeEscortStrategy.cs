using Sandbox.Definitions;
using Sandbox.Game.Contracts;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Library.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyContractTypeEscortStrategy : MyContractTypeBaseStrategy
	{
		private static readonly int TOO_MANY_TRIES = 10;

		private static readonly float GRAVITY_SQUARED_EPSILON = 0.0001f;

		private static readonly float MAX_ESCORT_SHIP_RADIUS = 75f;

		private static readonly int ESCORT_GRID_START_OFFSET = 550;

		public MyContractTypeEscortStrategy(MySessionComponentEconomyDefinition economyDefinition)
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
			MyContractEscort myContractEscort = new MyContractEscort();
			MyContractTypeEscortDefinition myContractTypeEscortDefinition = myContractEscort.GetDefinition() as MyContractTypeEscortDefinition;
			if (myContractTypeEscortDefinition == null)
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
			List<MyPlanet> list = new List<MyPlanet>();
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyPlanet myPlanet = entity as MyPlanet;
				if (myPlanet != null)
				{
					list.Add(myPlanet);
				}
			}
			BoundingSphereD boundingSphereD = new BoundingSphereD(myStation.Position, myContractTypeEscortDefinition.TravelDistanceMax);
			Vector3D vector3D = Vector3D.Zero;
			Vector3D? vector3D2 = null;
			bool flag = false;
			int num = -1;
			do
			{
				num++;
				vector3D2 = boundingSphereD.RandomToUniformPointInSphereWithInnerCutout(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), myContractTypeEscortDefinition.TravelDistanceMin);
				if (!vector3D2.HasValue)
				{
					continue;
				}
				vector3D = myStation.Position + Vector3.Normalize(vector3D2.Value - myStation.Position) * ESCORT_GRID_START_OFFSET;
				if (MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D2.Value).LengthSquared() > GRAVITY_SQUARED_EPSILON || MyGravityProviderSystem.CalculateNaturalGravityInPoint(vector3D).LengthSquared() > GRAVITY_SQUARED_EPSILON)
				{
					continue;
				}
				List<MyObjectSeed> list2 = new List<MyObjectSeed>();
				MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(vector3D2.Value, MAX_ESCORT_SHIP_RADIUS), list2);
				if (list2.Count > 0)
				{
					continue;
				}
				list2.Clear();
				MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(vector3D, MAX_ESCORT_SHIP_RADIUS), list2);
				if (list2.Count <= 0)
				{
					bool flag2 = true;
					foreach (MyPlanet item in list)
					{
						MySphericalNaturalGravityComponent mySphericalNaturalGravityComponent = item.Components.Get<MyGravityProviderComponent>() as MySphericalNaturalGravityComponent;
						if (mySphericalNaturalGravityComponent != null)
						{
							Vector3D vector3D3 = item.PositionComp.GetPosition() - vector3D;
							Vector3D vector3D4 = Vector3D.Normalize(vector3D2.Value - vector3D);
							if ((vector3D3 - vector3D4 * Vector3D.Dot(vector3D4, vector3D3)).LengthSquared() <= (double)mySphericalNaturalGravityComponent.GravityLimitSq)
							{
								flag2 = false;
								break;
							}
						}
					}
					if (flag2)
					{
						flag = true;
						break;
					}
				}
			}
			while (num < TOO_MANY_TRIES);
			if (!flag)
			{
				return MyContractCreationResults.Fail_Common;
			}
			double num2 = (vector3D - vector3D2.Value).Length();
			long num3 = 0L;
			IMyFaction myFaction = factions.TryGetFactionById(myStation.FactionId);
			if (myFaction != null)
			{
				num3 = myFaction.FounderId;
			}
			long num4 = 0L;
			num4 = GetPirateFactionId();
			if (num4 == 0L)
			{
				return MyContractCreationResults.Error;
			}
			if (myFaction.FactionId == num4 || MySession.Static.Factions.GetRelationBetweenFactions(num3, num4).Item1 != MyRelationsBetweenFactions.Enemies)
			{
				MyFaction myFaction2 = FindRandomEnemyFaction(myFaction.FactionId);
				if (myFaction2 != null)
				{
					num4 = myFaction2.FactionId;
				}
			}
			MyObjectBuilder_ContractEscort myObjectBuilder_ContractEscort = new MyObjectBuilder_ContractEscort();
			myObjectBuilder_ContractEscort.Id = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.CONTRACT);
			myObjectBuilder_ContractEscort.IsPlayerMade = false;
			myObjectBuilder_ContractEscort.State = MyContractStateEnum.Inactive;
			myObjectBuilder_ContractEscort.RewardMoney = GetMoneyReward_Escort(myContractTypeEscortDefinition.MinimumMoney, num2);
			if (myStation.IsDeepSpaceStation)
			{
				myObjectBuilder_ContractEscort.RewardMoney = (long)((float)myObjectBuilder_ContractEscort.RewardMoney * m_economyDefinition.DeepSpaceStationContractBonus);
			}
			myObjectBuilder_ContractEscort.RewardReputation = GetReputationReward_Escort(myContractTypeEscortDefinition.MinimumReputation, num2);
			myObjectBuilder_ContractEscort.StartingDeposit = (long)(MyRandom.Instance.NextDouble() * (double)(myContractTypeEscortDefinition.MaxStartingDeposit - myContractTypeEscortDefinition.MinStartingDeposit));
			myObjectBuilder_ContractEscort.FailReputationPrice = myContractTypeEscortDefinition.FailReputationPrice;
			myObjectBuilder_ContractEscort.StartFaction = factionId;
			myObjectBuilder_ContractEscort.StartStation = stationId;
			myObjectBuilder_ContractEscort.StartBlock = 0L;
			myObjectBuilder_ContractEscort.GridId = 0L;
			myObjectBuilder_ContractEscort.StartPosition = vector3D;
			myObjectBuilder_ContractEscort.EndPosition = vector3D2.Value;
			myObjectBuilder_ContractEscort.PathLength = num2;
			myObjectBuilder_ContractEscort.RewardRadius = myContractTypeEscortDefinition.RewardRadius;
			myObjectBuilder_ContractEscort.TriggerEntityId = 0L;
			myObjectBuilder_ContractEscort.TriggerRadius = myContractTypeEscortDefinition.TriggerRadius;
			myObjectBuilder_ContractEscort.DroneFirstDelay = MyTimeSpan.FromSeconds((num4 == 0L) ? int.MaxValue : myContractTypeEscortDefinition.DroneFirstDelayInS).Ticks;
			myObjectBuilder_ContractEscort.DroneAttackPeriod = MyTimeSpan.FromSeconds((num4 == 0L) ? int.MaxValue : myContractTypeEscortDefinition.DroneAttackPeriodInS).Ticks;
			myObjectBuilder_ContractEscort.DronesPerWave = ((num4 != 0L) ? myContractTypeEscortDefinition.DronesPerWave : 0);
			myObjectBuilder_ContractEscort.InitialDelay = MyTimeSpan.FromSeconds(myContractTypeEscortDefinition.DroneFirstDelayInS).Ticks;
			myObjectBuilder_ContractEscort.WaveFactionId = num4;
			myObjectBuilder_ContractEscort.EscortShipOwner = num3;
			myObjectBuilder_ContractEscort.Creation = now.Ticks;
			myObjectBuilder_ContractEscort.RemainingTimeInS = GetDurationForEscortContract(myContractTypeEscortDefinition, num2, MyContractEscort.DRONE_SPEED_LIMIT).Seconds;
			myObjectBuilder_ContractEscort.TicksToDiscard = MyContractTypeBaseStrategy.TICKS_TO_LIVE;
			myContractEscort.Init(myObjectBuilder_ContractEscort);
			contract = myContractEscort;
			return MyContractCreationResults.Success;
		}

		private MyTimeSpan GetDurationForEscortContract(MyContractTypeEscortDefinition def, double distanceInM, float maxSpeed)
		{
			return MyTimeSpan.FromSeconds(def.DurationMultiplier * (double)(def.Duration_BaseTime + (float)(long)((double)def.Duration_FlightTimeMultiplier * distanceInM / (double)maxSpeed)));
		}

		private long GetMoneyReward_Escort(long baseRew, double distance)
		{
			return (long)((double)baseRew * Math.Pow(3.0, Math.Log10(distance)));
		}

		private int GetReputationReward_Escort(int baseRew, double distance)
		{
			return baseRew;
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

		private MyFaction FindRandomEnemyFaction(long factionId)
		{
			MyFactionCollection factions = MySession.Static.Factions;
			List<MyFaction> list = new List<MyFaction>();
			foreach (KeyValuePair<long, MyFaction> item in factions)
			{
				if (item.Value.FactionType != 0 && item.Value.FactionType != MyFactionTypes.PlayerMade && factions.GetRelationBetweenFactions(factionId, item.Value.FactionId).Item1 == MyRelationsBetweenFactions.Enemies)
				{
					list.Add(item.Value);
				}
			}
			if (list.Count <= 0)
			{
				return null;
			}
			return list[MyRandom.Instance.Next(0, list.Count)];
		}
	}
}
