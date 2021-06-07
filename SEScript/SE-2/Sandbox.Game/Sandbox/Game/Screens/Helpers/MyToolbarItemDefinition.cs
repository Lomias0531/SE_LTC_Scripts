using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
	public abstract class MyToolbarItemDefinition : MyToolbarItem
	{
		public MyDefinitionBase Definition;

		public MyToolbarItemDefinition()
		{
			SetEnabled(newEnabled: true);
			base.WantsToBeActivated = true;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			MyToolbarItemDefinition myToolbarItemDefinition = obj as MyToolbarItemDefinition;
			if (myToolbarItemDefinition != null && Definition != null)
			{
				return Definition.Id.Equals(myToolbarItemDefinition.Definition.Id);
			}
			return false;
		}

		public sealed override int GetHashCode()
		{
			return Definition.Id.GetHashCode();
		}

		public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
		{
			if (Definition == null)
			{
				return null;
			}
			MyObjectBuilder_ToolbarItemDefinition obj = (MyObjectBuilder_ToolbarItemDefinition)MyToolbarItemFactory.CreateObjectBuilder(this);
			obj.DefinitionId = Definition.Id;
			return obj;
		}

		public override bool Init(MyObjectBuilder_ToolbarItem data)
		{
			if (MyDefinitionManager.Static.TryGetDefinition(((MyObjectBuilder_ToolbarItemDefinition)data).DefinitionId, out Definition))
			{
				if (!Definition.Public && !MyFakes.ENABLE_NON_PUBLIC_BLOCKS)
				{
					return false;
				}
				SetDisplayName(Definition.DisplayNameText);
				SetIcons(Definition.Icons);
				return true;
			}
			return false;
		}
	}
}
