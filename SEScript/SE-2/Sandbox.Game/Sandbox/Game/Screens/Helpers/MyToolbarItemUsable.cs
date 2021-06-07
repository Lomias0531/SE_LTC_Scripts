using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemUsable))]
	public class MyToolbarItemUsable : MyToolbarItemDefinition
	{
		private MyFixedPoint m_lastAmount = 0;

		public MyInventory Inventory => (MySession.Static.ControlledEntity as MyCharacter)?.GetInventory();

		public MyFixedPoint Amount => m_lastAmount;

		public override bool Activate()
		{
			MyFixedPoint a = (Inventory != null) ? Inventory.GetItemAmount(Definition.Id) : ((MyFixedPoint)0);
			if (a > 0)
			{
				MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
				a = MyFixedPoint.Min(a, 1);
				if (myCharacter != null && a > 0)
				{
					Inventory.ConsumeItem(Definition.Id, a, myCharacter.EntityId);
				}
			}
			return true;
		}

		public override bool Init(MyObjectBuilder_ToolbarItem data)
		{
			bool result = base.Init(data);
			base.ActivateOnClick = false;
			base.WantsToBeActivated = false;
			return result;
		}

		public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
		{
			if (Definition == null)
			{
				return null;
			}
			MyObjectBuilder_ToolbarItemUsable obj = (MyObjectBuilder_ToolbarItemUsable)MyToolbarItemFactory.CreateObjectBuilder(this);
			obj.DefinitionId = Definition.Id;
			return obj;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			return type == MyToolbarType.Character;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			ChangeInfo changeInfo = ChangeInfo.None;
			if (localCharacter != null)
			{
				MyFixedPoint myFixedPoint = localCharacter.GetInventory()?.GetItemAmount(Definition.Id) ?? ((MyFixedPoint)0);
				if (m_lastAmount != myFixedPoint)
				{
					m_lastAmount = myFixedPoint;
					changeInfo |= ChangeInfo.IconText;
				}
			}
			bool enabled = m_lastAmount > 0;
			return changeInfo | SetEnabled(enabled);
		}

		public override void FillGridItem(MyGuiGridItem gridItem)
		{
			if (m_lastAmount > 0)
			{
				gridItem.AddText($"{m_lastAmount}x", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
			}
			else
			{
				gridItem.ClearText(MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
			}
		}
	}
}
