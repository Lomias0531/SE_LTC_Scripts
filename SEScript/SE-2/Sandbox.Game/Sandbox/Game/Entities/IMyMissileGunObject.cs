using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Weapons;

namespace Sandbox.Game.Entities
{
	public interface IMyMissileGunObject : IMyGunObject<MyGunBase>
	{
		void MissileShootEffect();

		void ShootMissile(MyObjectBuilder_Missile builder);

		void RemoveMissile(long entityId);
	}
}
