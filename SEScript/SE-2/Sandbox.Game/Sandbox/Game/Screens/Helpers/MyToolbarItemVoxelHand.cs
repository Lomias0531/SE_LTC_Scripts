using Sandbox.Definitions;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemVoxelHand))]
	public class MyToolbarItemVoxelHand : MyToolbarItemDefinition
	{
		public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
		{
			base.Init(objBuilder);
			base.WantsToBeSelected = false;
			base.ActivateOnClick = false;
			return true;
		}

		public override bool Activate()
		{
			if (Definition == null)
			{
				return false;
			}
			if (!MySessionComponentVoxelHand.Static.TrySetBrush(Definition.Id.SubtypeName))
			{
				return false;
			}
			bool flag = MySession.Static.CreativeMode || MySession.Static.IsUserAdmin(Sync.MyId);
			if (flag)
			{
				MySession.Static.GameFocusManager.Clear();
			}
			MySessionComponentVoxelHand.Static.Enabled = flag;
			if (MySessionComponentVoxelHand.Static.Enabled)
			{
				MySessionComponentVoxelHand.Static.CurrentDefinition = (Definition as MyVoxelHandDefinition);
				MySession.Static.ControlledEntity?.SwitchToWeapon(null);
				return true;
			}
			return false;
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
			if (MySessionComponentVoxelHand.Static == null)
			{
				return ChangeInfo.None;
			}
			MyVoxelHandDefinition myVoxelHandDefinition = MySessionComponentVoxelHand.Static.Enabled ? MySessionComponentVoxelHand.Static.CurrentDefinition : null;
			base.WantsToBeSelected = (MySessionComponentVoxelHand.Static.Enabled && myVoxelHandDefinition != null && myVoxelHandDefinition.Id.SubtypeId == (Definition as MyVoxelHandDefinition).Id.SubtypeId);
			return ChangeInfo.None;
		}
	}
}
