using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemWeapon))]
	public class MyToolbarItemWeapon : MyToolbarItemDefinition
	{
		protected int m_lastAmmoCount = -1;

		protected bool m_needsWeaponSwitching = true;

		protected string m_lastTextValue = string.Empty;

		public int AmmoCount => m_lastAmmoCount;

		public override bool Init(MyObjectBuilder_ToolbarItem data)
		{
			bool result = base.Init(data);
			base.ActivateOnClick = false;
			return result;
		}

		public override bool Equals(object obj)
		{
			bool flag = base.Equals(obj);
			if (flag && !(obj is MyToolbarItemWeapon))
			{
				flag = false;
			}
			return flag;
		}

		public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
		{
			return (MyObjectBuilder_ToolbarItemWeapon)base.GetObjectBuilder();
		}

		public override bool Activate()
		{
			if (Definition == null)
			{
				return false;
			}
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				if (m_needsWeaponSwitching)
				{
					controlledEntity.SwitchToWeapon(this);
					base.WantsToBeActivated = true;
				}
				else
				{
					controlledEntity.SwitchAmmoMagazine();
				}
			}
			return true;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			return true;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			bool flag = false;
			bool flag2 = false;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			bool flag3 = localCharacter != null && (localCharacter.FindWeaponItemByDefinition(Definition.Id).HasValue || !localCharacter.WeaponTakesBuilderFromInventory(Definition.Id));
			ChangeInfo changeInfo = ChangeInfo.None;
			if (flag3)
			{
				IMyGunObject<MyDeviceBase> currentWeapon = localCharacter.CurrentWeapon;
				if (currentWeapon != null)
				{
					flag = (MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id == Definition.Id);
				}
				if (localCharacter.LeftHandItem != null)
				{
					flag |= (Definition == localCharacter.LeftHandItem.PhysicalItemDefinition);
				}
				if (flag && currentWeapon != null)
				{
					MyWeaponItemDefinition myWeaponItemDefinition = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId) as MyWeaponItemDefinition;
					if (myWeaponItemDefinition != null && myWeaponItemDefinition.ShowAmmoCount)
					{
						int ammunitionAmount = localCharacter.CurrentWeapon.GetAmmunitionAmount();
						if (m_lastAmmoCount != ammunitionAmount)
						{
							m_lastAmmoCount = ammunitionAmount;
							base.IconText.Clear().AppendInt32(ammunitionAmount);
							changeInfo |= ChangeInfo.IconText;
						}
					}
				}
			}
			MyShipController myShipController = MySession.Static.ControlledEntity as MyShipController;
			if (myShipController != null && myShipController.GridSelectionSystem.WeaponSystem != null)
			{
				flag2 = myShipController.GridSelectionSystem.WeaponSystem.HasGunsOfId(Definition.Id);
				if (flag2 && myShipController.GridSelectionSystem.WeaponSystem.GetGun(Definition.Id).GunBase is MyGunBase)
				{
					int num = 0;
					foreach (IMyGunObject<MyDeviceBase> item in myShipController.GridSelectionSystem.WeaponSystem.GetGunsById(Definition.Id))
					{
						num += item.GetAmmunitionAmount();
					}
					if (num != m_lastAmmoCount)
					{
						m_lastAmmoCount = num;
						base.IconText.Clear().AppendInt32(num);
						changeInfo |= ChangeInfo.IconText;
					}
				}
				flag = (myShipController.GridSelectionSystem.GetGunId() == Definition.Id);
			}
			changeInfo |= SetEnabled(flag3 || flag2);
			base.WantsToBeSelected = flag;
			m_needsWeaponSwitching = !flag;
			if (m_lastTextValue != base.IconText.ToString())
			{
				changeInfo |= ChangeInfo.IconText;
			}
			m_lastTextValue = base.IconText.ToString();
			return changeInfo;
		}
	}
}
