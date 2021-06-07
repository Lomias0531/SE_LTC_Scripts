using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.AI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilder;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Contracts
{
	[MyContractDescriptor(typeof(MyObjectBuilder_ContractEscort))]
	public class MyContractEscort : MyContract
	{
		public static readonly int START_PREFAB_TEST_RADIUS = 100;

		public static readonly int MAX_PREFAB_TEST_SPAWN = 20;

		public static readonly int MAX_PREFAB_TEST_PER_DIST = 10;

		public static readonly int DISPOSE_TIME_IN_S = 10;

		public static readonly float DRONE_SPEED_LIMIT = 30f;

		public static readonly float SPAWN_AHEAD_DISTANCE = 1000f;

		public static readonly float SIZE_OF_DRONES = 30f;

		public static readonly float SPAWN_RANDOMIZATION_RADIUS = 100f;

		public static readonly int DRONE_MAX_COUNT = 15;

		private bool m_isBeingDisposed;

		private float m_disposeTime;

		private MyTimeSpan? DisposeTime;

		private List<long> m_drones;

		public long GridId
		{
			get;
			private set;
		}

		public Vector3D StartPosition
		{
			get;
			private set;
		}

		public Vector3D EndPosition
		{
			get;
			private set;
		}

		public double PathLength
		{
			get;
			private set;
		}

		public double RewardRadius
		{
			get;
			private set;
		}

		public long TriggerEntityId
		{
			get;
			private set;
		}

		public double TriggerRadius
		{
			get;
			private set;
		}

		public MyTimeSpan DroneFirstDelay
		{
			get;
			private set;
		}

		public MyTimeSpan DroneAttackPeriod
		{
			get;
			private set;
		}

		public MyTimeSpan InnerTimer
		{
			get;
			private set;
		}

		public bool IsBehaviorAttached
		{
			get;
			private set;
		}

		public int DronesPerWave
		{
			get;
			private set;
		}

		public MyTimeSpan InitialDelay
		{
			get;
			private set;
		}

		public long WaveFactionId
		{
			get;
			private set;
		}

		public bool DestinationReached
		{
			get;
			private set;
		}

		public long EscortShipOwner
		{
			get;
			private set;
		}

		public override MyObjectBuilder_Contract GetObjectBuilder()
		{
			MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
			MyObjectBuilder_ContractEscort obj = objectBuilder as MyObjectBuilder_ContractEscort;
			obj.GridId = GridId;
			obj.StartPosition = StartPosition;
			obj.EndPosition = EndPosition;
			obj.PathLength = PathLength;
			obj.RewardRadius = RewardRadius;
			obj.TriggerEntityId = TriggerEntityId;
			obj.TriggerRadius = TriggerRadius;
			obj.DroneFirstDelay = DroneFirstDelay.Ticks;
			obj.DroneAttackPeriod = DroneAttackPeriod.Ticks;
			obj.InnerTimer = InnerTimer.Ticks;
			obj.InitialDelay = InitialDelay.Ticks;
			obj.DronesPerWave = DronesPerWave;
			obj.IsBehaviorAttached = IsBehaviorAttached;
			obj.WaveFactionId = WaveFactionId;
			obj.EscortShipOwner = EscortShipOwner;
			obj.Drones = new MySerializableList<long>(m_drones);
			obj.DestinationReached = DestinationReached;
			return objectBuilder;
		}

		public override void Init(MyObjectBuilder_Contract ob)
		{
			base.Init(ob);
			MyObjectBuilder_ContractEscort myObjectBuilder_ContractEscort = ob as MyObjectBuilder_ContractEscort;
			if (myObjectBuilder_ContractEscort != null)
			{
				GridId = myObjectBuilder_ContractEscort.GridId;
				StartPosition = myObjectBuilder_ContractEscort.StartPosition;
				EndPosition = myObjectBuilder_ContractEscort.EndPosition;
				PathLength = myObjectBuilder_ContractEscort.PathLength;
				RewardRadius = myObjectBuilder_ContractEscort.RewardRadius;
				TriggerEntityId = myObjectBuilder_ContractEscort.TriggerEntityId;
				TriggerRadius = myObjectBuilder_ContractEscort.TriggerRadius;
				DroneFirstDelay = new MyTimeSpan(myObjectBuilder_ContractEscort.DroneFirstDelay);
				DroneAttackPeriod = new MyTimeSpan(myObjectBuilder_ContractEscort.DroneAttackPeriod);
				InnerTimer = new MyTimeSpan(myObjectBuilder_ContractEscort.InnerTimer);
				InitialDelay = new MyTimeSpan(myObjectBuilder_ContractEscort.InitialDelay);
				DronesPerWave = myObjectBuilder_ContractEscort.DronesPerWave;
				IsBehaviorAttached = myObjectBuilder_ContractEscort.IsBehaviorAttached;
				WaveFactionId = myObjectBuilder_ContractEscort.WaveFactionId;
				EscortShipOwner = myObjectBuilder_ContractEscort.EscortShipOwner;
				m_drones = myObjectBuilder_ContractEscort.Drones;
				DestinationReached = myObjectBuilder_ContractEscort.DestinationReached;
				if (m_drones == null)
				{
					m_drones = new List<long>();
				}
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (base.State != MyContractStateEnum.Active)
			{
				return;
			}
			SubscribeToTrigger();
			SubscribePowerChange();
			if (!IsBehaviorAttached)
			{
				return;
			}
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyRemoteControl firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyRemoteControl>();
				if (firstBlockOfType != null)
				{
					firstBlockOfType.IsWorkingChanged += RemoteDestroyedCallback;
				}
			}
		}

		public override MyDefinitionId? GetDefinitionId()
		{
			return new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Escort");
		}

		protected override bool CanBeShared_Internal()
		{
			return base.State == MyContractStateEnum.Active;
		}

		protected override void Activate_Internal(MyTimeSpan timeOfActivation)
		{
			base.Activate_Internal(timeOfActivation);
			long num = MyEntityIdentifier.AllocateId();
			MyEntity myEntity = new MyEntity
			{
				WorldMatrix = MatrixD.Identity,
				EntityId = num,
				DisplayName = "EscortTriggerDummy_" + base.Id,
				Name = "EscortTriggerDummy_" + base.Id
			};
			myEntity.PositionComp.SetPosition(EndPosition);
			myEntity.Components.Remove<MyPhysicsComponentBase>();
			MyEntities.Add(myEntity);
			TriggerEntityId = num;
			AttachTrigger(myEntity);
			MyContractTypeEscortDefinition myContractTypeEscortDefinition = GetDefinition() as MyContractTypeEscortDefinition;
			if (myContractTypeEscortDefinition.PrefabsEscortShips.Count > 0)
			{
				SpawnPrefab(myContractTypeEscortDefinition.PrefabsEscortShips[MyRandom.Instance.Next(0, myContractTypeEscortDefinition.PrefabsEscortShips.Count)]);
			}
			else
			{
				SpawnPrefab("Eradicator_mk.1");
			}
			if (MyFakes.CONTRACT_ESCORT_DEBUGDRAW)
			{
				MyRenderProxy.DebugDrawSphere(StartPosition, 50f, Color.LimeGreen, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
				MyRenderProxy.DebugDrawSphere(EndPosition, 50f, Color.Red, 1f, depthRead: false, smooth: false, cull: false, persistent: true);
				MyRenderProxy.DebugDrawLine3D(StartPosition, EndPosition, Color.LimeGreen, Color.Red, depthRead: false, persistent: true);
			}
			IsBehaviorAttached = false;
			InnerTimer = timeOfActivation + InitialDelay;
		}

		protected void SpawnPrefab(string name)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(base.StartFaction);
			if (myFaction == null && EscortShipOwner == 0L)
			{
				MyLog.Default.Error("Contract - Escort: Starting faction is not in factions and EscortShipOwner is not specified!!!\n Cannot spawn prefab.");
				return;
			}
			Vector3 vector = Vector3.Normalize(Vector3.Normalize(EndPosition - StartPosition));
			Vector3 up = Vector3.CalculatePerpendicularVector(vector);
			Vector3D? vector3D = MyEntities.FindFreePlace(StartPosition, START_PREFAB_TEST_RADIUS, MAX_PREFAB_TEST_SPAWN, MAX_PREFAB_TEST_PER_DIST);
			if (!vector3D.HasValue)
			{
				vector3D = StartPosition;
			}
			MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties
			{
				Position = vector3D.Value,
				Forward = vector,
				Up = up,
				PrefabName = name,
				OwnerId = ((EscortShipOwner > 0) ? EscortShipOwner : myFaction.FounderId),
				Color = (myFaction?.CustomColor ?? Vector3.Zero),
				SpawningOptions = (SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix),
				UpdateSync = true
			};
			MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, delegate
			{
				if (spawnProperties.ResultList != null && spawnProperties.ResultList.Count != 0 && spawnProperties.ResultList.Count <= 1)
				{
					MyCubeGrid myCubeGrid = spawnProperties.ResultList[0];
					GridId = myCubeGrid.EntityId;
					MyCubeGridSystems gridSystems = myCubeGrid.GridSystems;
					gridSystems.GridPowerStateChanged = (Action<long, bool, string>)Delegate.Combine(gridSystems.GridPowerStateChanged, new Action<long, bool, string>(GridPowerStateChanged));
					MyRemoteControl firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyRemoteControl>();
					if (firstBlockOfType != null)
					{
						firstBlockOfType.IsWorkingChanged += RemoteDestroyedCallback;
					}
					MyGps gps = PrepareGPS();
					foreach (long owner in base.Owners)
					{
						MySession.Static.Gpss.SendAddGps(owner, ref gps, GridId);
					}
				}
			});
		}

		public string GetGpsName()
		{
			return MyTexts.GetString(MyCommonTexts.Contract_Escort_GpsName) + base.Id;
		}

		private MyGps PrepareGPS()
		{
			return new MyGps
			{
				DisplayName = MyTexts.GetString(MyCommonTexts.Contract_Escort_GpsName),
				Name = GetGpsName(),
				Description = MyTexts.GetString(MyCommonTexts.Contract_Escort_GpsDescription),
				Coords = StartPosition,
				ShowOnHud = true,
				DiscardAt = null,
				GPSColor = Color.DarkOrange,
				ContractId = base.Id
			};
		}

		protected override void Share_Internal(long identityId)
		{
			base.Share_Internal(identityId);
			MyEntities.GetEntityById(GridId);
			MyGps gps = PrepareGPS();
			MySession.Static.Gpss.SendAddGps(identityId, ref gps, GridId);
		}

		private string GetTriggerName()
		{
			return "Contract_Escort_Trig_" + base.Id;
		}

		protected void AttachTrigger(MyEntity entity)
		{
			MyAreaTriggerComponent myAreaTriggerComponent = new MyAreaTriggerComponent(GetTriggerName());
			myAreaTriggerComponent.Radius = TriggerRadius;
			myAreaTriggerComponent.Center = EndPosition;
			myAreaTriggerComponent.DefaultTranslation = EndPosition - entity.PositionComp.GetPosition();
			if (!entity.Components.Contains(typeof(MyTriggerAggregate)))
			{
				entity.Components.Add(typeof(MyTriggerAggregate), new MyTriggerAggregate());
			}
			entity.Components.Get<MyTriggerAggregate>().AddComponent(myAreaTriggerComponent);
			myAreaTriggerComponent.EntityEntered = (Action<long, string>)Delegate.Combine(myAreaTriggerComponent.EntityEntered, new Action<long, string>(EntityEnteredTrigger));
		}

		private void EntityEnteredTrigger(long entityId, string entityName)
		{
			if (entityId == GridId)
			{
				DestinationReached = true;
				Finish();
			}
		}

		private void SubscribeToTrigger()
		{
			if (GridId != 0L)
			{
				MyAreaTriggerComponent trigger = GetTrigger();
				trigger.EntityEntered = (Action<long, string>)Delegate.Combine(trigger.EntityEntered, new Action<long, string>(EntityEnteredTrigger));
			}
		}

		private void UnsubscribeFromTrigger()
		{
			if (GridId != 0L)
			{
				MyAreaTriggerComponent trigger = GetTrigger();
				if (trigger != null)
				{
					trigger.EntityEntered = (Action<long, string>)Delegate.Remove(trigger.EntityEntered, new Action<long, string>(EntityEnteredTrigger));
				}
			}
		}

		private MyAreaTriggerComponent GetTrigger()
		{
			if (TriggerEntityId == 0L)
			{
				return null;
			}
			MyEntity entityById = MyEntities.GetEntityById(TriggerEntityId);
			if (entityById == null)
			{
				return null;
			}
			if (!entityById.Components.Contains(typeof(MyTriggerAggregate)))
			{
				return null;
			}
			string triggerName = GetTriggerName();
			MyAggregateComponentList childList = entityById.Components.Get<MyTriggerAggregate>().ChildList;
			MyAreaTriggerComponent myAreaTriggerComponent = null;
			foreach (MyComponentBase item in childList.Reader)
			{
				myAreaTriggerComponent = (item as MyAreaTriggerComponent);
				if (myAreaTriggerComponent != null && myAreaTriggerComponent.Name == triggerName)
				{
					break;
				}
				myAreaTriggerComponent = null;
			}
			if (myAreaTriggerComponent == null)
			{
				return null;
			}
			return myAreaTriggerComponent;
		}

		protected override void FailFor_Internal(long player, bool abandon = false)
		{
			base.FailFor_Internal(player, abandon);
			RemoveGpsForPlayer(player);
		}

		protected override void FinishFor_Internal(long player, int rewardeeCount)
		{
			base.FinishFor_Internal(player, rewardeeCount);
			RemoveGpsForPlayer(player);
		}

		private void CloseDrones()
		{
			foreach (long drone in m_drones)
			{
				MyEntities.GetEntityById(drone)?.Close();
			}
		}

		protected override void CleanUp_Internal()
		{
			float disposeTime = 0f;
			UnsubscribePowerChange();
			UnsubscribeFromTrigger();
			foreach (long owner in base.Owners)
			{
				RemoveGpsForPlayer(owner);
			}
			MyEntity entityById = MyEntities.GetEntityById(GridId);
			if (entityById != null)
			{
				if (base.State == MyContractStateEnum.Finished)
				{
					CreateParticleEffectOnEntity("Warp", entityById.EntityId, offset: true);
					disposeTime = 10f;
				}
				else
				{
					CreateParticleEffectOnEntity("Explosion_Warhead_50", entityById.EntityId, offset: false);
					disposeTime = 2f;
				}
			}
			MyCubeGrid myCubeGrid = entityById as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyRemoteControl firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyRemoteControl>();
				if (firstBlockOfType != null)
				{
					firstBlockOfType.IsWorkingChanged -= RemoteDestroyedCallback;
				}
			}
			CloseDrones();
			MyEntities.GetEntityById(TriggerEntityId)?.Close();
			m_disposeTime = disposeTime;
			m_isBeingDisposed = true;
			base.State = MyContractStateEnum.ToBeDisposed;
		}

		private void RemoveGpsForPlayer(long identityId)
		{
			MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, base.Id);
			if (gpsByContractId != null)
			{
				MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
			}
		}

		private void UnsubscribePowerChange()
		{
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyCubeGridSystems gridSystems = myCubeGrid.GridSystems;
				gridSystems.GridPowerStateChanged = (Action<long, bool, string>)Delegate.Remove(gridSystems.GridPowerStateChanged, new Action<long, bool, string>(GridPowerStateChanged));
			}
		}

		private void SubscribePowerChange()
		{
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				MyCubeGridSystems gridSystems = myCubeGrid.GridSystems;
				gridSystems.GridPowerStateChanged = (Action<long, bool, string>)Delegate.Combine(gridSystems.GridPowerStateChanged, new Action<long, bool, string>(GridPowerStateChanged));
			}
		}

		private void GridPowerStateChanged(long entityId, bool isPowered, string entityName)
		{
			if (base.State == MyContractStateEnum.Active && !isPowered)
			{
				Fail();
			}
		}

		protected override bool CanPlayerReceiveReward(long identityId)
		{
			if (!MySession.Static.Players.TryGetPlayerId(identityId, out MyPlayer.PlayerId result))
			{
				return false;
			}
			MyPlayer playerById = MySession.Static.Players.GetPlayerById(result);
			if (playerById == null || playerById.Controller == null || playerById.Controller.ControlledEntity == null)
			{
				return false;
			}
			MyEntity myEntity = playerById.Controller.ControlledEntity as MyEntity;
			double num = RewardRadius + TriggerRadius + myEntity.PositionComp.WorldAABB.HalfExtents.Length();
			if ((myEntity.PositionComp.GetPosition() - EndPosition).LengthSquared() > num * num)
			{
				return false;
			}
			return true;
		}

		public override void Update(MyTimeSpan currentTime)
		{
			base.Update(currentTime);
			switch (base.State)
			{
			case MyContractStateEnum.Active:
				if (!(InnerTimer < currentTime))
				{
					break;
				}
				if (IsBehaviorAttached)
				{
					InnerTimer = currentTime + DroneAttackPeriod;
					SpawnDroneWave();
					break;
				}
				IsBehaviorAttached = AttachBehaviorToEscortShip();
				if (IsBehaviorAttached)
				{
					InnerTimer = currentTime + DroneFirstDelay;
				}
				break;
			case MyContractStateEnum.ToBeDisposed:
			{
				bool flag = false;
				if (m_isBeingDisposed)
				{
					if (!DisposeTime.HasValue)
					{
						DisposeTime = currentTime + MyTimeSpan.FromSeconds(m_disposeTime);
					}
					if (DisposeTime.Value <= currentTime)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					base.State = MyContractStateEnum.Disposed;
					MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
					if (myCubeGrid != null)
					{
						myCubeGrid.DismountAllCockpits();
						myCubeGrid.Close();
					}
				}
				break;
			}
			}
		}

		private bool AttachBehaviorToEscortShip()
		{
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
			if (myCubeGrid == null)
			{
				return false;
			}
			MyRemoteControl firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyRemoteControl>();
			if (firstBlockOfType == null)
			{
				return false;
			}
			firstBlockOfType.AddWaypoint(new MyWaypointInfo("endWaypoint", EndPosition));
			firstBlockOfType.SetCollisionAvoidance(enabled: true);
			firstBlockOfType.ChangeDirection(Base6Directions.Direction.Forward);
			firstBlockOfType.SetAutoPilotSpeedLimit(DRONE_SPEED_LIMIT);
			firstBlockOfType.SetWaypointThresholdDistance(1f);
			firstBlockOfType.ChangeFlightMode(FlightMode.OneWay);
			firstBlockOfType.SetWaitForFreeWay(waitForFreeWay: true);
			firstBlockOfType.SetAutoPilotEnabled(enabled: true);
			return true;
		}

		private void RemoteDestroyedCallback(MyCubeBlock obj)
		{
			if (base.State == MyContractStateEnum.Active && !obj.IsWorking)
			{
				Fail();
				obj.IsWorkingChanged -= RemoteDestroyedCallback;
			}
		}

		protected override bool NeedsUpdate_Internal()
		{
			return true;
		}

		private void SpawnDroneWave()
		{
			MyEntity entityById = MyEntities.GetEntityById(GridId);
			if (entityById == null)
			{
				return;
			}
			MyFaction myFaction = MySession.Static.Factions.TryGetFactionById(WaveFactionId) as MyFaction;
			if (myFaction == null)
			{
				return;
			}
			Vector3D position = entityById.PositionComp.GetPosition();
			Vector3D vector3D = Vector3D.Normalize(EndPosition - position);
			Vector3D center = position + vector3D * SPAWN_AHEAD_DISTANCE;
			MyContractTypeEscortDefinition myContractTypeEscortDefinition = GetDefinition() as MyContractTypeEscortDefinition;
			if (m_drones.Count >= DRONE_MAX_COUNT)
			{
				return;
			}
			for (int i = 0; i < DronesPerWave; i++)
			{
				string prefabName = "Barb_mk.1";
				if (myContractTypeEscortDefinition != null && myContractTypeEscortDefinition.PrefabsAttackDrones != null && myContractTypeEscortDefinition.PrefabsAttackDrones.Count > 0)
				{
					prefabName = myContractTypeEscortDefinition.PrefabsAttackDrones[MyRandom.Instance.Next(0, myContractTypeEscortDefinition.PrefabsAttackDrones.Count)];
				}
				string behaviorName = myContractTypeEscortDefinition.DroneBehaviours[MyRandom.Instance.Next(0, myContractTypeEscortDefinition.DroneBehaviours.Count)];
				Vector3D? vector3D2 = MyEntities.FindFreePlace(new BoundingSphereD(center, SPAWN_RANDOMIZATION_RADIUS).RandomToUniformPointInSphere(MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble(), MyRandom.Instance.NextDouble()), SIZE_OF_DRONES);
				if (vector3D2.HasValue)
				{
					MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties
					{
						Position = vector3D2.Value,
						Forward = -vector3D,
						Up = Vector3.CalculatePerpendicularVector(vector3D),
						PrefabName = prefabName,
						OwnerId = myFaction.FounderId,
						Color = myFaction.CustomColor,
						SpawningOptions = (SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix),
						UpdateSync = true
					};
					List<DroneTarget> targets = new List<DroneTarget>();
					targets.Add(new DroneTarget(entityById, 30));
					MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, delegate
					{
						if (spawnProperties.ResultList != null && spawnProperties.ResultList.Count != 0 && spawnProperties.ResultList.Count <= 1)
						{
							MyCubeGrid myCubeGrid = spawnProperties.ResultList[0];
							MyRemoteControl firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyRemoteControl>();
							if (firstBlockOfType != null)
							{
								MyDroneAI automaticBehaviour = new MyDroneAI(firstBlockOfType, behaviorName, activate: true, null, targets, 0, TargetPrioritization.PriorityRandom, 10000f, cycleWaypoints: false);
								firstBlockOfType.SetAutomaticBehaviour(automaticBehaviour);
								firstBlockOfType.SetAutoPilotEnabled(enabled: true);
								m_drones.Add(myCubeGrid.EntityId);
							}
						}
					});
				}
			}
		}

		public override bool CanBeFinished_Internal()
		{
			if (base.CanBeFinished_Internal())
			{
				return DestinationReached;
			}
			return false;
		}

		public override string ToDebugString()
		{
			return new StringBuilder(base.ToDebugString()).ToString();
		}
	}
}
