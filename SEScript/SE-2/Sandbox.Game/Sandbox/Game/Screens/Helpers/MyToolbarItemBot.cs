using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemBot))]
	public class MyToolbarItemBot : MyToolbarItemDefinition
	{
		public override bool Init(MyObjectBuilder_ToolbarItem data)
		{
			base.Init(data);
			base.ActivateOnClick = false;
			return true;
		}

		public override bool Activate()
		{
			if (!MyFakes.ENABLE_BARBARIANS || !MyPerGameSettings.EnableAi)
			{
				return false;
			}
			if (Definition == null)
			{
				return false;
			}
			MyAIComponent.Static.BotToSpawn = (Definition as MyAgentDefinition);
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
			MyAgentDefinition botToSpawn = MyAIComponent.Static.BotToSpawn;
			base.WantsToBeSelected = (botToSpawn != null && botToSpawn.Id.SubtypeId == (Definition as MyAgentDefinition).Id.SubtypeId);
			return ChangeInfo.None;
		}
	}
}
