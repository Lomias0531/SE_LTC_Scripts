using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;

namespace SpaceEngineers.Game.Weapons.Guns
{
	[MyCubeBlockType(typeof(MyObjectBuilder_ConveyorTurretBase))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase)
	})]
	public abstract class MyLargeConveyorTurretBase : MyLargeTurretBase, IMyConveyorEndpointBlock, SpaceEngineers.Game.ModAPI.IMyLargeConveyorTurretBase, Sandbox.ModAPI.IMyLargeTurretBase, Sandbox.ModAPI.IMyUserControllableGun, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase, IMyInventoryOwner
	{
		protected class m_useConveyorSystem_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType useConveyorSystem;
				ISyncType result = useConveyorSystem = new Sync<bool, SyncDirection.BothWays>(P_1, P_2);
				((MyLargeConveyorTurretBase)P_0).m_useConveyorSystem = (Sync<bool, SyncDirection.BothWays>)useConveyorSystem;
				return result;
			}
		}

		protected readonly Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;

		private MyMultilineConveyorEndpoint m_endpoint;

		public IMyConveyorEndpoint ConveyorEndpoint => m_endpoint;

		bool SpaceEngineers.Game.ModAPI.Ingame.IMyLargeConveyorTurretBase.UseConveyorSystem => m_useConveyorSystem;

		private bool UseConveyorSystem
		{
			get
			{
				return m_useConveyorSystem;
			}
			set
			{
				m_useConveyorSystem.Value = value;
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

		public MyLargeConveyorTurretBase()
		{
			CreateTerminalControls();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyLargeConveyorTurretBase>())
			{
				base.CreateTerminalControls();
				MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MyLargeConveyorTurretBase>());
				MyTerminalControlOnOffSwitch<MyLargeConveyorTurretBase> obj = new MyTerminalControlOnOffSwitch<MyLargeConveyorTurretBase>("UseConveyor", MySpaceTexts.Terminal_UseConveyorSystem)
				{
					Getter = ((MyLargeConveyorTurretBase x) => x.UseConveyorSystem),
					Setter = delegate(MyLargeConveyorTurretBase x, bool v)
					{
						x.UseConveyorSystem = v;
					}
				};
				obj.EnableToggleAction();
				MyTerminalControlFactory.AddControl(obj);
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.Init(objectBuilder, cubeGrid);
			m_useConveyorSystem.SetLocalValue(newValue: true);
			MyObjectBuilder_ConveyorTurretBase myObjectBuilder_ConveyorTurretBase = objectBuilder as MyObjectBuilder_ConveyorTurretBase;
			if (myObjectBuilder_ConveyorTurretBase != null)
			{
				m_useConveyorSystem.SetLocalValue(myObjectBuilder_ConveyorTurretBase.UseConveyorSystem);
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ConveyorTurretBase obj = (MyObjectBuilder_ConveyorTurretBase)base.GetObjectBuilderCubeBlock(copy);
			obj.UseConveyorSystem = m_useConveyorSystem;
			return obj;
		}

		public override void UpdateBeforeSimulation100()
		{
			base.UpdateBeforeSimulation100();
			if ((bool)m_useConveyorSystem && MySession.Static.SurvivalMode && Sync.IsServer && base.IsWorking && this.GetInventory().VolumeFillFactor < 0.5f)
			{
				base.CubeGrid.GridSystems.ConveyorSystem.PullItem(m_gunBase.CurrentAmmoMagazineId, 1, this, this.GetInventory(), remove: false, calcImmediately: false);
			}
		}

		public void InitializeConveyorEndpoint()
		{
			m_endpoint = new MyMultilineConveyorEndpoint(this);
		}

		VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(int index)
		{
			return MyEntityExtensions.GetInventory(this, index);
		}

		public PullInformation GetPullInformation()
		{
			PullInformation obj = new PullInformation
			{
				Inventory = this.GetInventory(),
				OwnerID = base.OwnerId
			};
			obj.Constraint = obj.Inventory.Constraint;
			return obj;
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
