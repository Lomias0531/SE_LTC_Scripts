using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using System.Collections.Generic;
using System.Reflection;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Screens.Helpers
{
	public static class MyToolbarItemFactory
	{
		private static MyObjectFactory<MyToolbarItemDescriptor, MyToolbarItem> m_objectFactory;

		static MyToolbarItemFactory()
		{
			m_objectFactory = new MyObjectFactory<MyToolbarItemDescriptor, MyToolbarItem>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyToolbarItem)));
			m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
			m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
		}

		public static MyToolbarItem CreateToolbarItem(MyObjectBuilder_ToolbarItem data)
		{
			MyToolbarItem myToolbarItem = m_objectFactory.CreateInstance(data.TypeId);
			if (!myToolbarItem.Init(data))
			{
				return null;
			}
			return myToolbarItem;
		}

		public static MyObjectBuilder_ToolbarItem CreateObjectBuilder(MyToolbarItem item)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_ToolbarItem>(item);
		}

		public static MyToolbarItem CreateToolbarItemFromInventoryItem(IMyInventoryItem inventoryItem)
		{
			MyDefinitionId definitionId = inventoryItem.GetDefinitionId();
			if (MyDefinitionManager.Static.TryGetDefinition(definitionId, out MyDefinitionBase definition) && (definition is MyPhysicalItemDefinition || definition is MyCubeBlockDefinition))
			{
				MyObjectBuilder_ToolbarItem myObjectBuilder_ToolbarItem = ObjectBuilderFromDefinition(definition);
				if (myObjectBuilder_ToolbarItem != null && !(myObjectBuilder_ToolbarItem is MyObjectBuilder_ToolbarItemEmpty))
				{
					return CreateToolbarItem(myObjectBuilder_ToolbarItem);
				}
			}
			return null;
		}

		public static MyObjectBuilder_ToolbarItem ObjectBuilderFromDefinition(MyDefinitionBase defBase)
		{
			if (defBase is MyUsableItemDefinition)
			{
				MyObjectBuilder_ToolbarItemUsable myObjectBuilder_ToolbarItemUsable = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemUsable>();
				myObjectBuilder_ToolbarItemUsable.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemUsable;
			}
			if (defBase is MyPhysicalItemDefinition && defBase.Id.TypeId == typeof(MyObjectBuilder_PhysicalGunObject))
			{
				MyObjectBuilder_ToolbarItemWeapon myObjectBuilder_ToolbarItemWeapon = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemWeapon>();
				myObjectBuilder_ToolbarItemWeapon.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemWeapon;
			}
			if (defBase is MyCubeBlockDefinition)
			{
				MyObjectBuilder_ToolbarItemCubeBlock myObjectBuilder_ToolbarItemCubeBlock = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemCubeBlock>();
				myObjectBuilder_ToolbarItemCubeBlock.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemCubeBlock;
			}
			if (defBase is MyEmoteDefinition)
			{
				MyObjectBuilder_ToolbarItemEmote myObjectBuilder_ToolbarItemEmote = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemEmote>();
				myObjectBuilder_ToolbarItemEmote.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemEmote;
			}
			if (defBase is MyAnimationDefinition)
			{
				MyObjectBuilder_ToolbarItemAnimation myObjectBuilder_ToolbarItemAnimation = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemAnimation>();
				myObjectBuilder_ToolbarItemAnimation.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemAnimation;
			}
			if (defBase is MyVoxelHandDefinition)
			{
				MyObjectBuilder_ToolbarItemVoxelHand myObjectBuilder_ToolbarItemVoxelHand = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemVoxelHand>();
				myObjectBuilder_ToolbarItemVoxelHand.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemVoxelHand;
			}
			if (defBase is MyPrefabThrowerDefinition)
			{
				MyObjectBuilder_ToolbarItemPrefabThrower myObjectBuilder_ToolbarItemPrefabThrower = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemPrefabThrower>();
				myObjectBuilder_ToolbarItemPrefabThrower.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemPrefabThrower;
			}
			if (defBase is MyBotDefinition)
			{
				MyObjectBuilder_ToolbarItemBot myObjectBuilder_ToolbarItemBot = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemBot>();
				myObjectBuilder_ToolbarItemBot.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemBot;
			}
			if (defBase is MyAiCommandDefinition)
			{
				MyObjectBuilder_ToolbarItemAiCommand myObjectBuilder_ToolbarItemAiCommand = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemAiCommand>();
				myObjectBuilder_ToolbarItemAiCommand.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemAiCommand;
			}
			if (defBase is MyGridCreateToolDefinition)
			{
				MyObjectBuilder_ToolbarItemCreateGrid myObjectBuilder_ToolbarItemCreateGrid = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemCreateGrid>();
				myObjectBuilder_ToolbarItemCreateGrid.DefinitionId = defBase.Id;
				return myObjectBuilder_ToolbarItemCreateGrid;
			}
			return new MyObjectBuilder_ToolbarItemEmpty();
		}

		public static string[] GetIconForTerminalGroup(MyBlockGroup group)
		{
			string[] result = new string[1]
			{
				"Textures\\GUI\\Icons\\GroupIcon.dds"
			};
			bool flag = false;
			HashSet<MyTerminalBlock> blocks = group.Blocks;
			if (blocks == null || blocks.Count == 0)
			{
				return result;
			}
			MyDefinitionBase blockDefinition = blocks.FirstElement().BlockDefinition;
			foreach (MyTerminalBlock item in blocks)
			{
				if (!item.BlockDefinition.Equals(blockDefinition))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				result = blockDefinition.Icons;
			}
			return result;
		}

		public static MyObjectBuilder_ToolbarItemTerminalBlock TerminalBlockObjectBuilderFromBlock(MyTerminalBlock block)
		{
			MyObjectBuilder_ToolbarItemTerminalBlock myObjectBuilder_ToolbarItemTerminalBlock = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemTerminalBlock>();
			myObjectBuilder_ToolbarItemTerminalBlock.BlockEntityId = block.EntityId;
			myObjectBuilder_ToolbarItemTerminalBlock._Action = null;
			return myObjectBuilder_ToolbarItemTerminalBlock;
		}

		public static MyObjectBuilder_ToolbarItemTerminalGroup TerminalGroupObjectBuilderFromGroup(MyBlockGroup group)
		{
			MyObjectBuilder_ToolbarItemTerminalGroup myObjectBuilder_ToolbarItemTerminalGroup = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemTerminalGroup>();
			myObjectBuilder_ToolbarItemTerminalGroup.GroupName = group.Name.ToString();
			myObjectBuilder_ToolbarItemTerminalGroup._Action = null;
			return myObjectBuilder_ToolbarItemTerminalGroup;
		}

		public static MyObjectBuilder_ToolbarItemWeapon WeaponObjectBuilder()
		{
			return MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemWeapon>();
		}
	}
}
