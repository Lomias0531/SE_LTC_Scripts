using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyUsableItemHelper
	{
		public static void ItemClicked(MyPhysicalInventoryItem item, MyInventory inventory, MyCharacter character, MySharedButtonsEnum button)
		{
			if (item.Content is MyObjectBuilder_ConsumableItem && button == MySharedButtonsEnum.Secondary)
			{
				MyFixedPoint amount = item.Amount;
				if (amount > 0)
				{
					amount = MyFixedPoint.Min(amount, 1);
					if (character != null && character.StatComp != null && amount > 0)
					{
						inventory.ConsumeItem(item.Content.GetId(), amount, character.EntityId);
					}
				}
			}
			MyObjectBuilder_Datapad myObjectBuilder_Datapad = item.Content as MyObjectBuilder_Datapad;
			if (myObjectBuilder_Datapad != null && button == MySharedButtonsEnum.Secondary)
			{
				MyScreenManager.AddScreen(new MyGuiDatapadEditScreen(myObjectBuilder_Datapad, item, inventory, character));
			}
		}
	}
}
