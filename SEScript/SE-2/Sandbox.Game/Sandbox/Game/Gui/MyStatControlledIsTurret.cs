using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledIsTurret : MyStatBase
	{
		public MyStatControlledIsTurret()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_is_turret");
		}

		public override void Update()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity == null)
			{
				base.CurrentValue = 0f;
			}
			else
			{
				base.CurrentValue = ((controlledEntity is IMyUserControllableGun) ? 1 : 0);
			}
		}
	}
}
