using Sandbox.Definitions;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Contracts
{
	[MyContractDescriptor(typeof(MyObjectBuilder_ContractFind))]
	public class MyContractFind : MyContract
	{
		public static readonly int DISPOSE_TIME_IN_S = 10;

		private bool m_isBeingDisposed;

		private float m_disposeTime;

		private MyTimeSpan? DisposeTime;

		public Vector3D GridPosition
		{
			get;
			private set;
		}

		public Vector3D GpsPosition
		{
			get;
			private set;
		}

		public long GridId
		{
			get;
			private set;
		}

		public double GpsDistance
		{
			get;
			private set;
		}

		public double TriggerRadius
		{
			get;
			private set;
		}

		public bool GridFound
		{
			get;
			private set;
		}

		public bool KeepGridAtTheEnd
		{
			get;
			private set;
		}

		public float MaxGpsOffset
		{
			get;
			private set;
		}

		public override MyObjectBuilder_Contract GetObjectBuilder()
		{
			MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
			MyObjectBuilder_ContractFind obj = objectBuilder as MyObjectBuilder_ContractFind;
			obj.GridPosition = GridPosition;
			obj.GpsPosition = GpsPosition;
			obj.GridId = GridId;
			obj.GpsDistance = GpsDistance;
			obj.MaxGpsOffset = MaxGpsOffset;
			obj.TriggerRadius = TriggerRadius;
			obj.GridFound = GridFound;
			obj.KeepGridAtTheEnd = KeepGridAtTheEnd;
			return objectBuilder;
		}

		public override void Init(MyObjectBuilder_Contract ob)
		{
			base.Init(ob);
			MyObjectBuilder_ContractFind myObjectBuilder_ContractFind = ob as MyObjectBuilder_ContractFind;
			if (myObjectBuilder_ContractFind != null)
			{
				GridPosition = myObjectBuilder_ContractFind.GridPosition;
				GpsPosition = myObjectBuilder_ContractFind.GpsPosition;
				GridId = myObjectBuilder_ContractFind.GridId;
				GpsDistance = myObjectBuilder_ContractFind.GpsDistance;
				MaxGpsOffset = myObjectBuilder_ContractFind.MaxGpsOffset;
				TriggerRadius = myObjectBuilder_ContractFind.TriggerRadius;
				GridFound = myObjectBuilder_ContractFind.GridFound;
				KeepGridAtTheEnd = myObjectBuilder_ContractFind.KeepGridAtTheEnd;
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (base.State == MyContractStateEnum.Active)
			{
				SubscribeToTrigger();
				SubscribePowerChange();
			}
		}

		private void SubscribeToTrigger()
		{
			if (GridId != 0L)
			{
				MyAreaTriggerComponent trigger = GetTrigger();
				if (trigger != null)
				{
					trigger.EntityEntered = (Action<long, string>)Delegate.Combine(trigger.EntityEntered, new Action<long, string>(EntityEnteredTrigger));
					return;
				}
				MyLog.Default.WriteToLogAndAssert($"CONTRACT SEARCH - Critical fail. Grid {GridId} is not present in world or is missing the Trigger");
				Fail();
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
			if (GridId == 0L)
			{
				return null;
			}
			MyEntity entityById = MyEntities.GetEntityById(GridId);
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

		protected override void Activate_Internal(MyTimeSpan timeOfActivation)
		{
			base.Activate_Internal(timeOfActivation);
			MyGps gps = new MyGps();
			gps.DisplayName = MyTexts.GetString(MyCommonTexts.Contract_Find_GpsName);
			gps.Name = MyTexts.GetString(MyCommonTexts.Contract_Find_GpsName);
			gps.Description = MyTexts.GetString(MyCommonTexts.Contract_Find_GpsDescription);
			gps.Coords = GpsPosition;
			gps.ShowOnHud = true;
			gps.DiscardAt = null;
			gps.ContractId = base.Id;
			gps.GPSColor = Color.DarkOrange;
			foreach (long owner in base.Owners)
			{
				MySession.Static.Gpss.SendAddGps(owner, ref gps, 0L);
			}
			MyContractTypeFindDefinition myContractTypeFindDefinition = GetDefinition() as MyContractTypeFindDefinition;
			if (GridId <= 0)
			{
				string name = "Container_MK-19";
				if (myContractTypeFindDefinition != null && myContractTypeFindDefinition.PrefabsSearchableGrids != null && myContractTypeFindDefinition.PrefabsSearchableGrids.Count > 0)
				{
					name = myContractTypeFindDefinition.PrefabsSearchableGrids[MyRandom.Instance.Next(0, myContractTypeFindDefinition.PrefabsSearchableGrids.Count)];
				}
				SpawnPrefab(name);
				return;
			}
			MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
			if (myCubeGrid != null)
			{
				AttachTrigger(myCubeGrid);
				if (myCubeGrid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) != MyResourceStateEnum.NoPower)
				{
					MyCubeGridSystems gridSystems = myCubeGrid.GridSystems;
					gridSystems.GridPowerStateChanged = (Action<long, bool, string>)Delegate.Combine(gridSystems.GridPowerStateChanged, new Action<long, bool, string>(GridPowerStateChanged));
				}
				else
				{
					Fail();
				}
			}
			else
			{
				Fail();
			}
		}

		protected void SpawnPrefab(string name)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(base.StartFaction);
			if (myFaction == null)
			{
				MyLog.Default.Error("Contract - Find: Starting faction is not in factions!!!\n Cannot spawn prefab.");
				return;
			}
			Vector3 vector = Vector3.Normalize(MyUtils.GetRandomVector3());
			Vector3 up = Vector3.CalculatePerpendicularVector(vector);
			MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties
			{
				Position = GridPosition,
				Forward = vector,
				Up = up,
				PrefabName = name,
				OwnerId = myFaction.FounderId,
				Color = myFaction.CustomColor,
				SpawningOptions = (SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix),
				UpdateSync = true
			};
			MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, delegate
			{
				if (spawnProperties.ResultList != null && spawnProperties.ResultList.Count != 0 && spawnProperties.ResultList.Count <= 1)
				{
					MyCubeGrid myCubeGrid = spawnProperties.ResultList[0];
					GridId = myCubeGrid.EntityId;
					AttachTrigger(myCubeGrid);
					MyCubeGridSystems gridSystems = myCubeGrid.GridSystems;
					gridSystems.GridPowerStateChanged = (Action<long, bool, string>)Delegate.Combine(gridSystems.GridPowerStateChanged, new Action<long, bool, string>(GridPowerStateChanged));
				}
			});
		}

		private string GetTriggerName()
		{
			return "Contract_Find_Trig_" + base.Id;
		}

		protected void AttachTrigger(MyCubeGrid grid)
		{
			MyAreaTriggerComponent myAreaTriggerComponent = new MyAreaTriggerComponent(GetTriggerName());
			myAreaTriggerComponent.Radius = TriggerRadius;
			myAreaTriggerComponent.Center = grid.PositionComp.GetPosition();
			myAreaTriggerComponent.DefaultTranslation = Vector3D.Zero;
			if (!grid.Components.Contains(typeof(MyTriggerAggregate)))
			{
				grid.Components.Add(typeof(MyTriggerAggregate), new MyTriggerAggregate());
			}
			grid.Components.Get<MyTriggerAggregate>().AddComponent(myAreaTriggerComponent);
			myAreaTriggerComponent.EntityEntered = (Action<long, string>)Delegate.Combine(myAreaTriggerComponent.EntityEntered, new Action<long, string>(EntityEnteredTrigger));
		}

		protected void DetachTrigger()
		{
			if (MySessionComponentTriggerSystem.Static == null)
			{
				return;
			}
			MyTriggerComponent foundTrigger;
			MyEntity triggersEntity = MySessionComponentTriggerSystem.Static.GetTriggersEntity(GetTriggerName(), out foundTrigger);
			if (triggersEntity != null && foundTrigger != null)
			{
				if (triggersEntity.Components.TryGet(out MyTriggerAggregate component))
				{
					component.RemoveComponent(foundTrigger);
				}
				else
				{
					triggersEntity.Components.Remove(typeof(MyAreaTriggerComponent), foundTrigger as MyAreaTriggerComponent);
				}
			}
		}

		private void GridPowerStateChanged(long entityId, bool isPowered, string entityName)
		{
			if (!isPowered)
			{
				Fail();
			}
		}

		public override bool CanBeFinished_Internal()
		{
			if (base.CanBeFinished_Internal())
			{
				return GridFound;
			}
			return false;
		}

		private void EntityEnteredTrigger(long entityId, string entityName)
		{
			MyEntity entityById = MyEntities.GetEntityById(entityId);
			if (entityById == null)
			{
				return;
			}
			long num = 0L;
			MyEntityController entityController = MySession.Static.Players.GetEntityController(entityById);
			if (entityController != null && entityController.Player != null && entityController.Player.Identity != null)
			{
				num = entityController.Player.Identity.IdentityId;
				bool flag = false;
				foreach (long owner in base.Owners)
				{
					if (owner == num)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					GridFound = true;
					Finish();
				}
			}
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

		protected override void CleanUp_Internal()
		{
			UnsubscribePowerChange();
			UnsubscribeFromTrigger();
			if (KeepGridAtTheEnd)
			{
				DetachTrigger();
			}
			float disposeTime = 0f;
			MyEntity entityById = MyEntities.GetEntityById(GridId);
			if (entityById != null)
			{
				if (base.State == MyContractStateEnum.Finished)
				{
					if (!KeepGridAtTheEnd)
					{
						string empty = string.Empty;
						bool offset = false;
						if ((double)MyGravityProviderSystem.CalculateNaturalGravityInPoint(entityById.PositionComp.GetPosition()).LengthSquared() > 0.001)
						{
							empty = "BlockDestroyed_Large";
							disposeTime = 1f;
						}
						else
						{
							empty = "Warp";
							disposeTime = 10f;
							offset = true;
						}
						CreateParticleEffectOnEntity(empty, entityById.EntityId, offset);
					}
				}
				else if (!KeepGridAtTheEnd)
				{
					CreateParticleEffectOnEntity("Explosion_Warhead_30", entityById.EntityId, offset: false);
					disposeTime = 2f;
				}
				else
				{
					CreateParticleEffectOnEntity("", entityById.EntityId, offset: false);
					disposeTime = 0f;
				}
			}
			m_isBeingDisposed = true;
			m_disposeTime = disposeTime;
			base.State = MyContractStateEnum.ToBeDisposed;
		}

		private void RemoveTriggerFromEntity()
		{
			throw new NotImplementedException();
		}

		public override void Update(MyTimeSpan currentTime)
		{
			base.Update(currentTime);
			MyContractStateEnum state = base.State;
			if (state != MyContractStateEnum.ToBeDisposed)
			{
				return;
			}
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
			if (!flag)
			{
				return;
			}
			base.State = MyContractStateEnum.Disposed;
			if (!KeepGridAtTheEnd)
			{
				MyCubeGrid myCubeGrid = MyEntities.GetEntityById(GridId) as MyCubeGrid;
				if (myCubeGrid != null)
				{
					myCubeGrid.DismountAllCockpits();
					myCubeGrid.Close();
				}
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

		public override MyDefinitionId? GetDefinitionId()
		{
			return new MyDefinitionId(typeof(MyObjectBuilder_ContractTypeDefinition), "Find");
		}

		private void RemoveGpsForPlayer(long identityId)
		{
			MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, base.Id);
			if (gpsByContractId != null)
			{
				MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
			}
		}

		public override string ToDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToDebugString());
			stringBuilder.AppendLine($"Station<->Gps distance: {GpsDistance}");
			return stringBuilder.ToString();
		}
	}
}
