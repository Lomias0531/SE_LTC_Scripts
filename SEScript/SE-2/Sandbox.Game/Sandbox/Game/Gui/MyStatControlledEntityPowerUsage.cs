using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityPowerUsage : MyStatBase
	{
		private float m_maxValue;

		public override float MaxValue => m_maxValue;

		public MyStatControlledEntityPowerUsage()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_power_usage");
		}

		public override void Update()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				MyResourceDistributorComponent myResourceDistributorComponent = null;
				MyCockpit myCockpit = controlledEntity.Entity as MyCockpit;
				if (myCockpit != null)
				{
					myResourceDistributorComponent = myCockpit.CubeGrid.GridSystems.ResourceDistributor;
				}
				else
				{
					MyRemoteControl myRemoteControl = controlledEntity as MyRemoteControl;
					if (myRemoteControl != null)
					{
						myResourceDistributorComponent = myRemoteControl.CubeGrid.GridSystems.ResourceDistributor;
					}
					else
					{
						MyLargeTurretBase myLargeTurretBase = controlledEntity as MyLargeTurretBase;
						if (myLargeTurretBase != null)
						{
							myResourceDistributorComponent = myLargeTurretBase.CubeGrid.GridSystems.ResourceDistributor;
						}
					}
				}
				if (myResourceDistributorComponent != null)
				{
					m_maxValue = myResourceDistributorComponent.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId);
					base.CurrentValue = MyMath.Clamp(myResourceDistributorComponent.TotalRequiredInputByType(MyResourceDistributorComponent.ElectricityId), 0f, m_maxValue);
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

		public override string ToString()
		{
			float num = (m_maxValue > 0f) ? (base.CurrentValue / m_maxValue) : 0f;
			return $"{num * 100f:0}";
		}
	}
}
