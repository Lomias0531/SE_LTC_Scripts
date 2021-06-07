using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Interfaces;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
	public interface IMyControllableEntity : VRage.Game.ModAPI.Interfaces.IMyControllableEntity
	{
		new MyControllerInfo ControllerInfo
		{
			get;
		}

		new MyEntity Entity
		{
			get;
		}

		float HeadLocalXAngle
		{
			get;
			set;
		}

		float HeadLocalYAngle
		{
			get;
			set;
		}

		bool EnabledBroadcasting
		{
			get;
		}

		MyToolbarType ToolbarType
		{
			get;
		}

		MyStringId ControlContext
		{
			get;
		}

		MyStringId AuxiliaryContext
		{
			get;
		}

		MyToolbar Toolbar
		{
			get;
		}

		MyEntity RelativeDampeningEntity
		{
			get;
			set;
		}

		void BeginShoot(MyShootActionEnum action);

		void EndShoot(MyShootActionEnum action);

		bool ShouldEndShootingOnPause(MyShootActionEnum action);

		void OnBeginShoot(MyShootActionEnum action);

		void OnEndShoot(MyShootActionEnum action);

		void UseFinished();

		void PickUpFinished();

		void Sprint(bool enabled);

		void SwitchToWeapon(MyDefinitionId weaponDefinition);

		void SwitchToWeapon(MyToolbarItemWeapon weapon);

		bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition);

		void SwitchAmmoMagazine();

		bool CanSwitchAmmoMagazine();

		void SwitchBroadcasting();

		MyEntityCameraSettings GetCameraEntitySettings();
	}
}
