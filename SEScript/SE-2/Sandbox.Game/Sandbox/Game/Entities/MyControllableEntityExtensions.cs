using Sandbox.Game.Weapons;
using VRage.Game.Components;
using VRageMath;

namespace Sandbox.Game.Entities
{
	internal static class MyControllableEntityExtensions
	{
		public static void SwitchControl(this IMyControllableEntity entity, IMyControllableEntity newControlledEntity)
		{
			if (entity.ControllerInfo.Controller != null)
			{
				entity.ControllerInfo.Controller.TakeControl(newControlledEntity);
			}
		}

		public static MyPhysicsComponentBase Physics(this IMyControllableEntity entity)
		{
			if (entity.Entity == null)
			{
				return null;
			}
			if (entity.Entity.Physics == null)
			{
				MyCockpit myCockpit = entity.Entity as MyCockpit;
				if (myCockpit != null && myCockpit.CubeGrid != null && myCockpit.CubeGrid.Physics != null)
				{
					return myCockpit.CubeGrid.Physics;
				}
				return null;
			}
			return entity.Entity.Physics;
		}

		public static void GetLinearVelocity(this IMyControllableEntity controlledEntity, ref Vector3 velocityVector, bool useRemoteControlVelocity = true)
		{
			if (controlledEntity.Entity.Physics == null)
			{
				MyCockpit myCockpit = controlledEntity as MyCockpit;
				if (myCockpit != null)
				{
					velocityVector = ((myCockpit.CubeGrid.Physics != null) ? myCockpit.CubeGrid.Physics.LinearVelocity : Vector3.Zero);
					return;
				}
				MyRemoteControl myRemoteControl = controlledEntity as MyRemoteControl;
				if (myRemoteControl != null && useRemoteControlVelocity)
				{
					velocityVector = ((myRemoteControl.CubeGrid.Physics != null) ? myRemoteControl.CubeGrid.Physics.LinearVelocity : Vector3.Zero);
					return;
				}
				MyLargeTurretBase myLargeTurretBase = controlledEntity as MyLargeTurretBase;
				if (myLargeTurretBase != null)
				{
					velocityVector = ((myLargeTurretBase.CubeGrid.Physics != null) ? myLargeTurretBase.CubeGrid.Physics.LinearVelocity : Vector3.Zero);
				}
			}
			else
			{
				velocityVector = ((controlledEntity.Entity.Physics != null) ? controlledEntity.Entity.Physics.LinearVelocity : Vector3.Zero);
			}
		}
	}
}
