using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.AI
{
	public class MySpaceBotFactory : MyBotFactoryBase
	{
		public override int MaximumUncontrolledBotCount => 10;

		public override int MaximumBotPerPlayer => 32;

		public override bool CanCreateBotOfType(string behaviorType, bool load)
		{
			return true;
		}

		public override bool GetBotSpawnPosition(string behaviorType, out Vector3D spawnPosition)
		{
			if (behaviorType == "Spider")
			{
				MatrixD spawnPosition2;
				bool spiderSpawnPosition = GetSpiderSpawnPosition(out spawnPosition2, null);
				spawnPosition = spawnPosition2.Translation;
				return spiderSpawnPosition;
			}
			if (MySession.Static.LocalCharacter != null)
			{
				Vector3D center = MySession.Static.LocalCharacter.PositionComp.GetPosition();
				Vector3 v = MyGravityProviderSystem.CalculateNaturalGravityInPoint(center);
				v = ((!(v.LengthSquared() < 0.0001f)) ? ((Vector3)Vector3D.Normalize(v)) : Vector3.Up);
				Vector3D tangent = Vector3.CalculatePerpendicularVector(v);
				Vector3D bitangent = Vector3.Cross(tangent, v);
				spawnPosition = MyUtils.GetRandomDiscPosition(ref center, 5.0, ref tangent, ref bitangent);
				return true;
			}
			spawnPosition = Vector3D.Zero;
			return false;
		}

		public static bool GetSpiderSpawnPosition(out MatrixD spawnPosition, Vector3D? oldPosition)
		{
			spawnPosition = MatrixD.Identity;
			Vector3D? vector3D = null;
			MyPlanet myPlanet = null;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SerialId == 0 && onlinePlayer.Character != null)
				{
					vector3D = onlinePlayer.GetPosition();
					myPlanet = MyGamePruningStructure.GetClosestPlanet(vector3D.Value);
					MyPlanetAnimalSpawnInfo dayOrNightAnimalSpawnInfo = GetDayOrNightAnimalSpawnInfo(myPlanet, vector3D.Value);
					if (dayOrNightAnimalSpawnInfo == null || dayOrNightAnimalSpawnInfo.Animals == null || !dayOrNightAnimalSpawnInfo.Animals.Any((MyPlanetAnimal x) => x.AnimalType.Contains("Spider")))
					{
						vector3D = null;
						myPlanet = null;
					}
					else
					{
						if (!oldPosition.HasValue)
						{
							break;
						}
						MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(oldPosition.Value);
						if (myPlanet == closestPlanet)
						{
							break;
						}
						vector3D = null;
						myPlanet = null;
					}
				}
			}
			if (!vector3D.HasValue || myPlanet == null)
			{
				return false;
			}
			Vector3D vector3D2 = myPlanet.Components.Get<MyGravityProviderComponent>().GetWorldGravity(vector3D.Value);
			if (Vector3D.IsZero(vector3D2))
			{
				vector3D2 = Vector3D.Down;
			}
			else
			{
				vector3D2.Normalize();
			}
			vector3D2.CalculatePerpendicularVector(out Vector3D result);
			Vector3D bitangent = Vector3D.Cross(vector3D2, result);
			Vector3D center = vector3D.Value;
			center = MyUtils.GetRandomDiscPosition(ref center, 20.0, ref result, ref bitangent);
			center -= vector3D2 * 500.0;
			Vector3D closestSurfacePointGlobal = myPlanet.GetClosestSurfacePointGlobal(ref center);
			Vector3D vector3D3 = vector3D.Value - closestSurfacePointGlobal;
			if (!Vector3D.IsZero(vector3D3))
			{
				vector3D3.Normalize();
			}
			else
			{
				vector3D3 = Vector3D.CalculatePerpendicularVector(vector3D2);
			}
			spawnPosition = MatrixD.CreateWorld(closestSurfacePointGlobal, vector3D3, -vector3D2);
			return true;
		}

		public override bool GetBotGroupSpawnPositions(string behaviorType, int count, List<Vector3D> spawnPositions)
		{
			throw new NotImplementedException();
		}

		public static MyPlanetAnimalSpawnInfo GetDayOrNightAnimalSpawnInfo(MyPlanet planet, Vector3D position)
		{
			if (planet == null)
			{
				return null;
			}
			if (planet.Generator.NightAnimalSpawnInfo != null && planet.Generator.NightAnimalSpawnInfo.Animals != null && planet.Generator.NightAnimalSpawnInfo.Animals.Length != 0 && MySectorWeatherComponent.IsThereNight(planet, ref position))
			{
				return planet.Generator.NightAnimalSpawnInfo;
			}
			if (planet.Generator.AnimalSpawnInfo != null && planet.Generator.AnimalSpawnInfo.Animals != null && planet.Generator.AnimalSpawnInfo.Animals.Length != 0)
			{
				return planet.Generator.AnimalSpawnInfo;
			}
			return null;
		}
	}
}
