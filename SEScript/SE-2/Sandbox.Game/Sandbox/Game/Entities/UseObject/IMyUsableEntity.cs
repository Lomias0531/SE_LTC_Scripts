using VRage.Game.Entity.UseObject;

namespace Sandbox.Game.Entities.UseObject
{
	public interface IMyUsableEntity
	{
		UseActionResult CanUse(UseActionEnum actionEnum, IMyControllableEntity user);

		void RemoveUsers(bool local);
	}
}
