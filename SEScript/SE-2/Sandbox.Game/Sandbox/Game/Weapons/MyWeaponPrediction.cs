using Sandbox.Definitions;
using System;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	public static class MyWeaponPrediction
	{
		public static bool GetPredictedTargetPosition(MyGunBase gun, MyEntity shooter, MyEntity target, out Vector3 predictedPosition, out float timeToHit, float shootDelay = 0f)
		{
			if (target == null || target.PositionComp == null || shooter == null || shooter.PositionComp == null)
			{
				predictedPosition = Vector3.Zero;
				timeToHit = 0f;
				return false;
			}
			Vector3 value = target.PositionComp.WorldAABB.Center;
			Vector3 value2 = gun.GetMuzzleWorldPosition();
			Vector3 vector = value - value2;
			Vector3 value3 = Vector3.Zero;
			if (target.Physics != null)
			{
				value3 = target.Physics.LinearVelocity;
			}
			Vector3 value4 = Vector3.Zero;
			if (shooter.Physics != null)
			{
				value4 = shooter.Physics.LinearVelocity;
			}
			Vector3 vector2 = value3 - value4;
			float projectileSpeed = GetProjectileSpeed(gun);
			float num = vector2.LengthSquared() - projectileSpeed * projectileSpeed;
			float num2 = 2f * Vector3.Dot(vector2, vector);
			float num3 = vector.LengthSquared();
			float num4 = (0f - num2) / (2f * num);
			float num5 = (float)Math.Sqrt(num2 * num2 - 4f * num * num3) / (2f * num);
			float num6 = num4 - num5;
			float num7 = num4 + num5;
			float num8 = (!(num6 > num7) || !(num7 > 0f)) ? num6 : num7;
			num8 += shootDelay;
			predictedPosition = value + vector2 * num8;
			timeToHit = (predictedPosition - value2).Length() / projectileSpeed;
			return true;
		}

		public static float GetProjectileSpeed(MyGunBase gun)
		{
			if (gun == null)
			{
				return 0f;
			}
			float result = 0f;
			if (gun.CurrentAmmoMagazineDefinition != null)
			{
				result = MyDefinitionManager.Static.GetAmmoDefinition(gun.CurrentAmmoMagazineDefinition.AmmoDefinitionId).DesiredSpeed;
			}
			return result;
		}
	}
}
