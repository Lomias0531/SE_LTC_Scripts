using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemPrefabThrower))]
	internal class MyToolbarItemPrefabThrower : MyToolbarItemDefinition
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
			MySessionComponentThrower.Static.Enabled = MyFakes.ENABLE_PREFAB_THROWER;
			MySessionComponentThrower.Static.CurrentDefinition = (MyPrefabThrowerDefinition)Definition;
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
			MyPrefabThrowerDefinition myPrefabThrowerDefinition = MySessionComponentThrower.Static.Enabled ? MySessionComponentThrower.Static.CurrentDefinition : null;
			base.WantsToBeSelected = (MySessionComponentThrower.Static.Enabled && myPrefabThrowerDefinition != null && myPrefabThrowerDefinition.Id.SubtypeId == (Definition as MyPrefabThrowerDefinition).Id.SubtypeId);
			return ChangeInfo.None;
		}
	}
}
