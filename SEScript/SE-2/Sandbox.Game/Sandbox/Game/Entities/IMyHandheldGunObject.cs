using Sandbox.Definitions;
using Sandbox.Game.Weapons;
using VRage.Game;

namespace Sandbox.Game.Entities
{
	public interface IMyHandheldGunObject<out T> : IMyGunObject<T> where T : MyDeviceBase
	{
		MyObjectBuilder_PhysicalGunObject PhysicalObject
		{
			get;
		}

		MyPhysicalItemDefinition PhysicalItemDefinition
		{
			get;
		}

		bool ForceAnimationInsteadOfIK
		{
			get;
		}

		bool IsBlocking
		{
			get;
		}

		int CurrentAmmunition
		{
			get;
			set;
		}

		int CurrentMagazineAmmunition
		{
			get;
			set;
		}

		long OwnerId
		{
			get;
		}

		long OwnerIdentityId
		{
			get;
		}

		bool CanDoubleClickToStick(MyShootActionEnum action);

		bool ShouldEndShootOnPause(MyShootActionEnum action);

		void DoubleClicked(MyShootActionEnum action);
	}
}
