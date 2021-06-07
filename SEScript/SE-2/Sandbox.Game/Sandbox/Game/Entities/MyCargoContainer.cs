using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Network;

namespace Sandbox.Game.Entities
{
	[MyCubeBlockType(typeof(MyObjectBuilder_CargoContainer))]
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyCargoContainer),
		typeof(Sandbox.ModAPI.Ingame.IMyCargoContainer)
	})]
	public class MyCargoContainer : MyTerminalBlock, IMyConveyorEndpointBlock, Sandbox.ModAPI.IMyCargoContainer, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyCargoContainer, IMyInventoryOwner
	{
		private class Sandbox_Game_Entities_MyCargoContainer_003C_003EActor : IActivator, IActivator<MyCargoContainer>
		{
			private sealed override object CreateInstance()
			{
				return new MyCargoContainer();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCargoContainer CreateInstance()
			{
				return new MyCargoContainer();
			}

			MyCargoContainer IActivator<MyCargoContainer>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private MyCargoContainerDefinition m_cargoDefinition;

		private bool m_useConveyorSystem = true;

		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private string m_containerType;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public string ContainerType
		{
			get
			{
				return m_containerType;
			}
			set
			{
				m_containerType = value;
			}
		}

		private bool UseConveyorSystem
		{
			get
			{
				return m_useConveyorSystem;
			}
			set
			{
				m_useConveyorSystem = value;
			}
		}

		int IMyInventoryOwner.InventoryCount => base.InventoryCount;

		long IMyInventoryOwner.EntityId => base.EntityId;

		bool IMyInventoryOwner.HasInventory => base.HasInventory;

		bool IMyInventoryOwner.UseConveyorSystem
		{
			get
			{
				return UseConveyorSystem;
			}
			set
			{
				UseConveyorSystem = value;
			}
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			m_cargoDefinition = (MyCargoContainerDefinition)MyDefinitionManager.Static.GetCubeBlockDefinition(objectBuilder.GetId());
			MyObjectBuilder_CargoContainer myObjectBuilder_CargoContainer = (MyObjectBuilder_CargoContainer)objectBuilder;
			m_containerType = myObjectBuilder_CargoContainer.ContainerType;
			if (MyFakes.ENABLE_INVENTORY_FIX)
			{
				FixSingleInventory();
			}
			if (this.GetInventory() == null)
			{
				MyInventory component = new MyInventory(m_cargoDefinition.InventorySize.Volume, m_cargoDefinition.InventorySize, MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend);
				base.Components.Add((MyInventoryBase)component);
			}
			this.GetInventory().SetFlags(MyInventoryFlags.CanReceive | MyInventoryFlags.CanSend);
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
			AddDebugRenderComponent(new MyDebugRenderComponentDrawConveyorEndpoint(m_conveyorEndpoint));
			UpdateIsWorking();
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_CargoContainer myObjectBuilder_CargoContainer = (MyObjectBuilder_CargoContainer)base.GetObjectBuilderCubeBlock(copy);
			if (m_containerType != null)
			{
				myObjectBuilder_CargoContainer.ContainerType = m_containerType;
			}
			return myObjectBuilder_CargoContainer;
		}

		public void SpawnRandomCargo()
		{
			if (m_containerType != null)
			{
				MyContainerTypeDefinition containerTypeDefinition = MyDefinitionManager.Static.GetContainerTypeDefinition(m_containerType);
				if (containerTypeDefinition != null && containerTypeDefinition.Items.Length != 0)
				{
					this.GetInventory().GenerateContent(containerTypeDefinition);
				}
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if (base.Components.TryGet(out MyContainerDropComponent component))
			{
				component.UpdateSound();
			}
		}

		protected override void OnInventoryComponentAdded(MyInventoryBase inventory)
		{
			base.OnInventoryComponentAdded(inventory);
		}

		protected override void OnInventoryComponentRemoved(MyInventoryBase inventory)
		{
			base.OnInventoryComponentRemoved(inventory);
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return this.GetInventory(index);
		}

		public PullInformation GetPullInformation()
		{
			return null;
		}

		public PullInformation GetPushInformation()
		{
			return null;
		}

		public bool AllowSelfPulling()
		{
			return false;
		}
	}
}
