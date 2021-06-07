using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyStatControlledEntitySpeed : MyStatBase
	{
		public override float MaxValue => MyGridPhysics.ShipMaxLinearVelocity() + 7f;

		public MyStatControlledEntitySpeed()
		{
			base.Id = MyStringHash.GetOrCompute("controlled_speed");
		}

		public override void Update()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			float currentValue = 0f;
			if (controlledEntity != null)
			{
				Vector3 velocityVector = Vector3.Zero;
				controlledEntity.GetLinearVelocity(ref velocityVector);
				currentValue = velocityVector.Length();
			}
			base.CurrentValue = currentValue;
		}
	}
}
