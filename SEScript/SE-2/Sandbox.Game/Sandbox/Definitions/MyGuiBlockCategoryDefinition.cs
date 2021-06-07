using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GuiBlockCategoryDefinition), null)]
	public class MyGuiBlockCategoryDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyGuiBlockCategoryDefinition_003C_003EActor : IActivator, IActivator<MyGuiBlockCategoryDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGuiBlockCategoryDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGuiBlockCategoryDefinition CreateInstance()
			{
				return new MyGuiBlockCategoryDefinition();
			}

			MyGuiBlockCategoryDefinition IActivator<MyGuiBlockCategoryDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Name;

		public HashSet<string> ItemIds;

		public bool IsShipCategory;

		public bool IsBlockCategory = true;

		public bool SearchBlocks = true;

		public bool ShowAnimations;

		public bool ShowInCreative = true;

		public bool IsAnimationCategory;

		public bool IsToolCategory;

		public int ValidItems;

		public new bool Public = true;

		public bool StrictSearch;

		protected override void Init(MyObjectBuilder_DefinitionBase ob)
		{
			base.Init(ob);
			MyObjectBuilder_GuiBlockCategoryDefinition myObjectBuilder_GuiBlockCategoryDefinition = (MyObjectBuilder_GuiBlockCategoryDefinition)ob;
			Name = myObjectBuilder_GuiBlockCategoryDefinition.Name;
			ItemIds = new HashSet<string>(myObjectBuilder_GuiBlockCategoryDefinition.ItemIds.ToList());
			IsBlockCategory = myObjectBuilder_GuiBlockCategoryDefinition.IsBlockCategory;
			IsShipCategory = myObjectBuilder_GuiBlockCategoryDefinition.IsShipCategory;
			SearchBlocks = myObjectBuilder_GuiBlockCategoryDefinition.SearchBlocks;
			ShowAnimations = myObjectBuilder_GuiBlockCategoryDefinition.ShowAnimations;
			ShowInCreative = myObjectBuilder_GuiBlockCategoryDefinition.ShowInCreative;
			Public = myObjectBuilder_GuiBlockCategoryDefinition.Public;
			IsAnimationCategory = myObjectBuilder_GuiBlockCategoryDefinition.IsAnimationCategory;
			IsToolCategory = myObjectBuilder_GuiBlockCategoryDefinition.IsToolCategory;
			StrictSearch = myObjectBuilder_GuiBlockCategoryDefinition.StrictSearch;
		}

		public bool HasItem(string itemId)
		{
			foreach (string itemId2 in ItemIds)
			{
				if (itemId.EndsWith(itemId2))
				{
					return true;
				}
			}
			return false;
		}
	}
}
