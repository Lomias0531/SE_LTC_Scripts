using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Inventory;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Character.Components
{
	public class MyInventorySpawnComponent : MyCharacterComponent
	{
		private class Sandbox_Game_Entities_Character_Components_MyInventorySpawnComponent_003C_003EActor : IActivator, IActivator<MyInventorySpawnComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyInventorySpawnComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyInventorySpawnComponent CreateInstance()
			{
				return new MyInventorySpawnComponent();
			}

			MyInventorySpawnComponent IActivator<MyInventorySpawnComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyInventory m_spawnInventory;

		private const string INVENTORY_USE_DUMMY_NAME = "inventory";

		public override string ComponentTypeDebugString => "Inventory Spawn Component";

		public override void OnCharacterDead()
		{
			if (!base.Character.IsDead || !base.Character.Definition.EnableSpawnInventoryAsContainer || !base.Character.Definition.InventorySpawnContainerId.HasValue)
			{
				return;
			}
			if (base.Character.Components.Has<MyInventoryBase>())
			{
				MyInventoryBase myInventoryBase = base.Character.Components.Get<MyInventoryBase>();
				if (myInventoryBase is MyInventoryAggregate)
				{
					MyInventoryAggregate aggregate = myInventoryBase as MyInventoryAggregate;
					List<MyComponentBase> list = new List<MyComponentBase>();
					aggregate.GetComponentsFlattened(list);
					foreach (MyComponentBase item in list)
					{
						MyInventory myInventory = item as MyInventory;
						if (myInventory != null && myInventory.GetItemsCount() > 0)
						{
							if (MyDefinitionManager.Static.TryGetContainerDefinition(base.Character.Definition.InventorySpawnContainerId.Value, out MyContainerDefinition _))
							{
								aggregate.RemoveComponent(myInventory);
								if (Sync.IsServer)
								{
									MyInventory myInventory2 = new MyInventory();
									myInventory2.Init(myInventory.GetObjectBuilder());
									SpawnInventoryContainer(base.Character.Definition.InventorySpawnContainerId.Value, myInventory2, spawnAboveCharacter: true, 0L);
								}
							}
						}
						else
						{
							aggregate.RemoveComponent(item);
						}
					}
				}
				else if (myInventoryBase is MyInventory && base.Character.Definition.SpawnInventoryOnBodyRemoval)
				{
					m_spawnInventory = (myInventoryBase as MyInventory);
					SpawnBackpack(base.Character);
				}
			}
			CloseComponent();
		}

		private void SpawnBackpack(MyEntity obj)
		{
			MyInventory myInventory = new MyInventory();
			myInventory.Init(m_spawnInventory.GetObjectBuilder());
			m_spawnInventory = myInventory;
			if (m_spawnInventory != null)
			{
				if (!MyComponentContainerExtension.TryGetContainerDefinition(base.Character.Definition.InventorySpawnContainerId.Value.TypeId, base.Character.Definition.InventorySpawnContainerId.Value.SubtypeId, out MyContainerDefinition definition))
				{
					MyDefinitionId myDefinitionId = new MyDefinitionId(typeof(MyObjectBuilder_InventoryBagEntity), base.Character.Definition.InventorySpawnContainerId.Value.SubtypeId);
					MyComponentContainerExtension.TryGetContainerDefinition(myDefinitionId.TypeId, myDefinitionId.SubtypeId, out definition);
				}
				if (definition != null && Sync.IsServer && !MyFakes.USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS)
				{
					long entityId = SpawnInventoryContainer(base.Character.Definition.InventorySpawnContainerId.Value, m_spawnInventory, spawnAboveCharacter: false, base.Character.DeadPlayerIdentityId);
					MyGps gps = new MyGps
					{
						ShowOnHud = true,
						Name = new StringBuilder().AppendStringBuilder(MyTexts.Get(MySpaceTexts.GPS_Body_Location_Name)).Append(" - ").AppendFormatedDateTime(DateTime.Now)
							.ToString(),
						DisplayName = MyTexts.GetString(MySpaceTexts.GPS_Body_Location_Name),
						DiscardAt = null,
						Coords = base.Character.PositionComp.GetPosition(),
						Description = "",
						AlwaysVisible = true,
						GPSColor = new Color(117, 201, 241),
						IsContainerGPS = true
					};
					MySession.Static.Gpss.SendAddGps(base.Character.DeadPlayerIdentityId, ref gps, entityId, playSoundOnCreation: false);
				}
			}
		}

		private long SpawnInventoryContainer(MyDefinitionId bagDefinition, MyInventory inventory, bool spawnAboveCharacter = true, long ownerIdentityId = 0L)
		{
			if (MySession.Static == null || !MySession.Static.Ready)
			{
				return 0L;
			}
			MyEntity character = base.Character;
			MatrixD worldMatrix = base.Character.WorldMatrix;
			if (spawnAboveCharacter)
			{
				worldMatrix.Translation += worldMatrix.Up + worldMatrix.Forward;
			}
			else
			{
				worldMatrix.Translation = base.Character.PositionComp.WorldAABB.Center + worldMatrix.Backward * 0.40000000596046448;
			}
			if (!MyComponentContainerExtension.TryGetContainerDefinition(bagDefinition.TypeId, bagDefinition.SubtypeId, out MyContainerDefinition definition))
			{
				return 0L;
			}
			MyEntity myEntity = MyEntities.CreateFromComponentContainerDefinitionAndAdd(definition.Id, fadeIn: false);
			if (myEntity == null)
			{
				return 0L;
			}
			MyInventoryBagEntity myInventoryBagEntity = myEntity as MyInventoryBagEntity;
			if (myInventoryBagEntity != null)
			{
				myInventoryBagEntity.OwnerIdentityId = ownerIdentityId;
			}
			myEntity.PositionComp.SetWorldMatrix(worldMatrix);
			myEntity.Physics.LinearVelocity = character.Physics.LinearVelocity;
			myEntity.Physics.AngularVelocity = character.Physics.AngularVelocity;
			myEntity.Render.EnableColorMaskHsv = true;
			myEntity.Render.ColorMaskHsv = base.Character.Render.ColorMaskHsv;
			inventory.RemoveEntityOnEmpty = true;
			myEntity.Components.Add((MyInventoryBase)inventory);
			return myEntity.EntityId;
		}

		public override void OnAddedToContainer()
		{
			base.OnAddedToContainer();
		}

		private void CloseComponent()
		{
		}

		public override bool IsSerialized()
		{
			return false;
		}
	}
}
