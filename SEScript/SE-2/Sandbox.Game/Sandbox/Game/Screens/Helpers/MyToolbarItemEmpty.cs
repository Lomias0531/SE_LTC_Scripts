using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemEmpty))]
	internal class MyToolbarItemEmpty : MyToolbarItem
	{
		public static MyToolbarItemEmpty Default = new MyToolbarItemEmpty();

		public MyToolbarItemEmpty()
		{
			SetEnabled(newEnabled: true);
			base.ActivateOnClick = false;
			base.WantsToBeSelected = true;
		}

		public override bool Activate()
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return -1;
		}

		public override bool Init(MyObjectBuilder_ToolbarItem data)
		{
			return true;
		}

		public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
		{
			return null;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			return true;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			return ChangeInfo.None;
		}
	}
}
