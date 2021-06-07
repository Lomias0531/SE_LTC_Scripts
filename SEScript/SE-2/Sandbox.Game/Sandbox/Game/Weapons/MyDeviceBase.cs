using Sandbox.Definitions;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	public abstract class MyDeviceBase
	{
		public uint? InventoryItemId
		{
			get;
			set;
		}

		public static string GetGunNotificationName(MyDefinitionId gunId)
		{
			return MyDefinitionManager.Static.GetDefinition(gunId).DisplayNameText;
		}

		public void Init(MyObjectBuilder_DeviceBase objectBuilder)
		{
			InventoryItemId = objectBuilder.InventoryItemId;
		}

		public abstract Vector3D GetMuzzleLocalPosition();

		public abstract Vector3D GetMuzzleWorldPosition();

		public abstract bool CanSwitchAmmoMagazine();

		public abstract bool SwitchToNextAmmoMagazine();

		public abstract bool SwitchAmmoMagazineToNextAvailable();
	}
}
