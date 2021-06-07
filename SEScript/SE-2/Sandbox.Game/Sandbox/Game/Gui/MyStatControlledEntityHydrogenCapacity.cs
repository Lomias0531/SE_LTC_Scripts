using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityHydrogenCapacity : MyStatBase
	{
		private float m_maxValue;

		private MyCubeGrid m_lastConnected;

		private List<IMyGasTank> m_tankBlocks = new List<IMyGasTank>();

		public override float MaxValue => m_maxValue;

		public MyStatControlledEntityHydrogenCapacity()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_hydrogen_capacity");
		}

		public override void Update()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				MyResourceDistributorComponent myResourceDistributorComponent = null;
				MyCubeGrid myCubeGrid = null;
				MyCockpit myCockpit = controlledEntity.Entity as MyCockpit;
				if (myCockpit != null)
				{
					myCubeGrid = myCockpit.CubeGrid;
					myResourceDistributorComponent = myCockpit.CubeGrid.GridSystems.ResourceDistributor;
				}
				else
				{
					MyRemoteControl myRemoteControl = controlledEntity as MyRemoteControl;
					if (myRemoteControl != null)
					{
						myCubeGrid = myRemoteControl.CubeGrid;
						myResourceDistributorComponent = myRemoteControl.CubeGrid.GridSystems.ResourceDistributor;
					}
					else
					{
						MyLargeTurretBase myLargeTurretBase = controlledEntity as MyLargeTurretBase;
						if (myLargeTurretBase != null)
						{
							myCubeGrid = myLargeTurretBase.CubeGrid;
							myResourceDistributorComponent = myLargeTurretBase.CubeGrid.GridSystems.ResourceDistributor;
						}
					}
				}
				if (myCubeGrid != m_lastConnected)
				{
					if (m_lastConnected != null)
					{
						m_lastConnected.GridSystems.ConveyorSystem.BlockAdded -= ConveyorSystemOnBlockAdded;
						m_lastConnected.GridSystems.ConveyorSystem.BlockRemoved -= ConveyorSystemOnBlockRemoved;
					}
					m_lastConnected = myCubeGrid;
					m_tankBlocks.Clear();
					if (myCubeGrid != null)
					{
						myCubeGrid.GridSystems.ConveyorSystem.BlockAdded += ConveyorSystemOnBlockAdded;
						myCubeGrid.GridSystems.ConveyorSystem.BlockRemoved += ConveyorSystemOnBlockRemoved;
						Recalculate();
					}
				}
				if (myResourceDistributorComponent != null)
				{
					base.CurrentValue = 0f;
					foreach (IMyGasTank tankBlock in m_tankBlocks)
					{
						base.CurrentValue += (float)(tankBlock.FilledRatio * (double)tankBlock.GasCapacity);
					}
				}
				else
				{
					base.CurrentValue = 0f;
					m_maxValue = 0f;
				}
			}
			else
			{
				base.CurrentValue = 0f;
				m_maxValue = 0f;
			}
		}

		private void Recalculate()
		{
			m_maxValue = 0f;
			foreach (IMyConveyorEndpointBlock conveyorEndpointBlock in m_lastConnected.GridSystems.ConveyorSystem.ConveyorEndpointBlocks)
			{
				IMyGasTank myGasTank = conveyorEndpointBlock as IMyGasTank;
				if (myGasTank != null && myGasTank.IsResourceStorage(MyResourceDistributorComponent.HydrogenId))
				{
					m_maxValue += myGasTank.GasCapacity;
					m_tankBlocks.Add(myGasTank);
				}
			}
		}

		private void ConveyorSystemOnBlockRemoved(MyCubeBlock myCubeBlock)
		{
			IMyGasTank myGasTank = myCubeBlock as IMyGasTank;
			if (myGasTank != null && myGasTank.IsResourceStorage(MyResourceDistributorComponent.HydrogenId))
			{
				m_maxValue -= myGasTank.GasCapacity;
				m_tankBlocks.Remove(myGasTank);
			}
		}

		private void ConveyorSystemOnBlockAdded(MyCubeBlock myCubeBlock)
		{
			IMyGasTank myGasTank = myCubeBlock as IMyGasTank;
			if (myGasTank != null && myGasTank.IsResourceStorage(MyResourceDistributorComponent.HydrogenId))
			{
				m_maxValue += myGasTank.GasCapacity;
				m_tankBlocks.Add(myGasTank);
			}
		}

		public override string ToString()
		{
			float num = (m_maxValue > 0f) ? (base.CurrentValue / m_maxValue) : 0f;
			return $"{num * 100f:0}";
		}
	}
}
