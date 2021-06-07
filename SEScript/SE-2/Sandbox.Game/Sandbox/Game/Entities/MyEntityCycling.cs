using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public static class MyEntityCycling
	{
		public struct Metric
		{
			public static readonly Metric Min;

			public static readonly Metric Max;

			public float Value;

			public long EntityId;

			public Metric(float value, long entityId)
			{
				Value = value;
				EntityId = entityId;
			}

			public static bool operator >(Metric a, Metric b)
			{
				if (!(a.Value > b.Value))
				{
					if (a.Value == b.Value)
					{
						return a.EntityId > b.EntityId;
					}
					return false;
				}
				return true;
			}

			public static bool operator <(Metric a, Metric b)
			{
				return b > a;
			}

			public static bool operator >=(Metric a, Metric b)
			{
				if (!(a.Value > b.Value))
				{
					if (a.Value == b.Value)
					{
						return a.EntityId >= b.EntityId;
					}
					return false;
				}
				return true;
			}

			public static bool operator <=(Metric a, Metric b)
			{
				return b >= a;
			}

			public static bool operator ==(Metric a, Metric b)
			{
				if (a.Value == b.Value)
				{
					return a.EntityId == b.EntityId;
				}
				return false;
			}

			public static bool operator !=(Metric a, Metric b)
			{
				return !(a == b);
			}

			static Metric()
			{
				Metric metric = new Metric
				{
					Value = float.MinValue,
					EntityId = 0L
				};
				Min = metric;
				metric = new Metric
				{
					Value = float.MaxValue,
					EntityId = 0L
				};
				Max = metric;
			}
		}

		public static float GetMetric(MyEntityCyclingOrder order, MyEntity entity)
		{
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			MyPhysicsComponentBase physics = entity.Physics;
			switch (order)
			{
			case MyEntityCyclingOrder.Characters:
				return (entity is MyCharacter) ? 1 : 0;
			case MyEntityCyclingOrder.BiggestGrids:
				return myCubeGrid?.GetBlocks().Count ?? 0;
			case MyEntityCyclingOrder.Fastest:
				if (physics == null)
				{
					return 0f;
				}
				return (float)Math.Round(physics.LinearVelocity.Length(), 2);
			case MyEntityCyclingOrder.BiggestDistanceFromPlayers:
				if (!(entity is MyVoxelBase) && !(entity is MySafeZone))
				{
					return GetPlayerDistance(entity);
				}
				return 0f;
			case MyEntityCyclingOrder.MostActiveDrills:
				return GetActiveBlockCount<MyShipDrill>(myCubeGrid);
			case MyEntityCyclingOrder.MostActiveProductionBuildings:
				return GetActiveBlockCount<MyProductionBlock>(myCubeGrid);
			case MyEntityCyclingOrder.MostActiveReactors:
				return GetActiveBlockCount<MyReactor>(myCubeGrid);
			case MyEntityCyclingOrder.MostActiveSensors:
				return GetActiveBlockCount<MySensorBlock>(myCubeGrid);
			case MyEntityCyclingOrder.MostActiveThrusters:
				return GetActiveBlockCount<MyThrust>(myCubeGrid);
			case MyEntityCyclingOrder.MostWheels:
				return GetActiveBlockCount<MyMotorSuspension>(myCubeGrid, includePassive: true);
			case MyEntityCyclingOrder.FloatingObjects:
				return (entity is MyFloatingObject) ? 1 : 0;
			case MyEntityCyclingOrder.StaticObjects:
				return (entity.Physics != null && !entity.Physics.IsPhantom && entity.Physics.AngularVelocity.AbsMax() < 0.05f && entity.Physics.LinearVelocity.AbsMax() < 0.05f) ? 1 : 0;
			case MyEntityCyclingOrder.Planets:
				return (entity is MyPlanet) ? 1 : 0;
			case MyEntityCyclingOrder.OwnerLoginTime:
				return GetOwnerLoginTime(myCubeGrid);
			default:
				return 0f;
			}
		}

		private static float GetOwnerLoginTime(MyCubeGrid grid)
		{
			if (grid == null)
			{
				return 0f;
			}
			if (grid.BigOwners.Count == 0)
			{
				return 0f;
			}
			MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(grid.BigOwners[0]);
			if (myIdentity == null)
			{
				return 0f;
			}
			return (float)Math.Round((DateTime.Now - myIdentity.LastLoginTime).TotalDays, 2);
		}

		private static float GetActiveBlockCount<T>(MyCubeGrid grid, bool includePassive = false) where T : MyFunctionalBlock
		{
			if (grid == null)
			{
				return 0f;
			}
			int num = 0;
			foreach (MySlimBlock block in grid.GetBlocks())
			{
				T val = block.FatBlock as T;
				if (val != null && (includePassive || val.IsWorking))
				{
					num++;
				}
			}
			return num;
		}

		private static float GetPlayerDistance(MyEntity entity)
		{
			Vector3D translation = entity.WorldMatrix.Translation;
			float num = float.MaxValue;
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				IMyControllableEntity controlledEntity = onlinePlayer.Controller.ControlledEntity;
				if (controlledEntity != null)
				{
					float num2 = Vector3.DistanceSquared(controlledEntity.Entity.WorldMatrix.Translation, translation);
					if (num2 < num)
					{
						num = num2;
					}
				}
			}
			return (float)Math.Sqrt(num);
		}

		public static void FindNext(MyEntityCyclingOrder order, ref float metric, ref long entityId, bool findLarger, CyclingOptions options)
		{
			Metric metric2 = default(Metric);
			metric2.Value = metric;
			metric2.EntityId = entityId;
			Metric b = metric2;
			Metric metric3 = findLarger ? Metric.Max : Metric.Min;
			Metric metric4 = metric3;
			Metric metric5 = metric3;
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				if (options.Enabled)
				{
					MyCubeGrid myCubeGrid = entity as MyCubeGrid;
					if ((options.OnlyLargeGrids && (myCubeGrid == null || myCubeGrid.GridSizeEnum != 0)) || (options.OnlySmallGrids && (myCubeGrid == null || myCubeGrid.GridSizeEnum != MyCubeSize.Small)))
					{
						continue;
					}
				}
				Metric metric6 = new Metric(GetMetric(order, entity), entity.EntityId);
				if (metric6.Value != 0f)
				{
					if (findLarger)
					{
						if (metric6 > b && metric6 < metric4)
						{
							metric4 = metric6;
						}
						if (metric6 < metric5)
						{
							metric5 = metric6;
						}
					}
					else
					{
						if (metric6 < b && metric6 > metric4)
						{
							metric4 = metric6;
						}
						if (metric6 > metric5)
						{
							metric5 = metric6;
						}
					}
				}
			}
			if (metric4 == metric3)
			{
				metric4 = metric5;
			}
			metric = metric4.Value;
			entityId = metric4.EntityId;
		}
	}
}
