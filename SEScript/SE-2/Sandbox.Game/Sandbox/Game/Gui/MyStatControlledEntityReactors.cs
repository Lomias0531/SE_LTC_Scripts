using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntityReactors : MyStatBase
	{
		public MyStatControlledEntityReactors()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_reactors");
		}

		public override void Update()
		{
			IMyControllableEntity myControllableEntity = MySession.Static.ControlledEntity;
			if (myControllableEntity != null)
			{
				if (myControllableEntity is MyLargeTurretBase)
				{
					myControllableEntity = (myControllableEntity as MyLargeTurretBase).PreviousControlledEntity;
				}
				base.CurrentValue = (myControllableEntity.EnabledReactors ? 1f : 0f);
			}
		}
	}
}
