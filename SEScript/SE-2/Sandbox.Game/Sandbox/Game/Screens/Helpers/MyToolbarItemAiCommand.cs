using Sandbox.Definitions;
using Sandbox.Game.AI;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemAiCommand))]
	public class MyToolbarItemAiCommand : MyToolbarItemDefinition
	{
		public override bool Init(MyObjectBuilder_ToolbarItem data)
		{
			bool result = base.Init(data);
			base.ActivateOnClick = false;
			return result;
		}

		public override bool Activate()
		{
			if (Definition == null)
			{
				return false;
			}
			MyAIComponent.Static.CommandDefinition = (Definition as MyAiCommandDefinition);
			MySession.Static.ControlledEntity?.SwitchToWeapon(null);
			return true;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			if (type != 0)
			{
				return type == MyToolbarType.Spectator;
			}
			return true;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			MyAiCommandDefinition commandDefinition = MyAIComponent.Static.CommandDefinition;
			base.WantsToBeSelected = (commandDefinition != null && commandDefinition.Id.SubtypeId == (Definition as MyAiCommandDefinition).Id.SubtypeId);
			return ChangeInfo.None;
		}
	}
}
