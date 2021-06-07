using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_HumanoidBotDefinition), null)]
	public class MyHumanoidBotDefinition : MyAgentDefinition
	{
		private class Sandbox_Definitions_MyHumanoidBotDefinition_003C_003EActor : IActivator, IActivator<MyHumanoidBotDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyHumanoidBotDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyHumanoidBotDefinition CreateInstance()
			{
				return new MyHumanoidBotDefinition();
			}

			MyHumanoidBotDefinition IActivator<MyHumanoidBotDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId StartingWeaponDefinitionId;

		public List<MyDefinitionId> InventoryItems;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_HumanoidBotDefinition myObjectBuilder_HumanoidBotDefinition = builder as MyObjectBuilder_HumanoidBotDefinition;
			if (myObjectBuilder_HumanoidBotDefinition.StartingItem != null && !string.IsNullOrWhiteSpace(myObjectBuilder_HumanoidBotDefinition.StartingItem.Subtype))
			{
				StartingWeaponDefinitionId = new MyDefinitionId(myObjectBuilder_HumanoidBotDefinition.StartingItem.Type, myObjectBuilder_HumanoidBotDefinition.StartingItem.Subtype);
			}
			InventoryItems = new List<MyDefinitionId>();
			if (myObjectBuilder_HumanoidBotDefinition.InventoryItems != null)
			{
				MyObjectBuilder_HumanoidBotDefinition.Item[] inventoryItems = myObjectBuilder_HumanoidBotDefinition.InventoryItems;
				foreach (MyObjectBuilder_HumanoidBotDefinition.Item item in inventoryItems)
				{
					InventoryItems.Add(new MyDefinitionId(item.Type, item.Subtype));
				}
			}
		}

		public override void AddItems(MyCharacter character)
		{
			base.AddItems(character);
			MyObjectBuilder_PhysicalGunObject objectBuilder = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(StartingWeaponDefinitionId.SubtypeName);
			if (character.WeaponTakesBuilderFromInventory(StartingWeaponDefinitionId))
			{
				character.GetInventory().AddItems(1, objectBuilder);
			}
			foreach (MyDefinitionId inventoryItem in InventoryItems)
			{
				objectBuilder = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(inventoryItem.SubtypeName);
				character.GetInventory().AddItems(1, objectBuilder);
			}
			character.SwitchToWeapon(StartingWeaponDefinitionId);
			MyDefinitionId defId = new MyDefinitionId(typeof(MyObjectBuilder_WeaponDefinition), StartingWeaponDefinitionId.SubtypeName);
			if (MyDefinitionManager.Static.TryGetWeaponDefinition(defId, out MyWeaponDefinition definition) && definition.HasAmmoMagazines())
			{
				MyObjectBuilder_AmmoMagazine objectBuilder2 = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_AmmoMagazine>(definition.AmmoMagazinesId[0].SubtypeName);
				character.GetInventory().AddItems(3, objectBuilder2);
			}
		}
	}
}
