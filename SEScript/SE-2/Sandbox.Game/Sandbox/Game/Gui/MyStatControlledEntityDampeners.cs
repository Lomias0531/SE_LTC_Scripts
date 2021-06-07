using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityDampeners : MyStatBase
	{
		public MyStatControlledEntityDampeners()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_dampeners");
		}

		public override void Update()
		{
			IMyControllableEntity myControllableEntity = MySession.Static.ControlledEntity;
			if (myControllableEntity == null)
			{
				return;
			}
			if (myControllableEntity is MyLargeTurretBase)
			{
				myControllableEntity = (myControllableEntity as MyLargeTurretBase).PreviousControlledEntity;
			}
			if (myControllableEntity.EnabledDamping)
			{
				if (myControllableEntity.RelativeDampeningEntity == null)
				{
					base.CurrentValue = 1f;
				}
				else
				{
					base.CurrentValue = 0.5f;
				}
			}
			else
			{
				base.CurrentValue = 0f;
			}
		}
	}
}
