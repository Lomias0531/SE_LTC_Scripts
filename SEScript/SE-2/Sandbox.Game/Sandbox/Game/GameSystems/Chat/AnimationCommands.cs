using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using System;
using System.Collections.Generic;
using VRage.Game.Definitions.Animation;
using VRage.GameServices;

namespace Sandbox.Game.GameSystems.Chat
{
	public static class AnimationCommands
	{
		internal static void LoadAnimations(MyChatCommandSystem system)
		{
			system.OnUnhandledCommand += GetAnimationCommands;
		}

		internal static void GetAnimationCommands(string command, string body, List<IMyChatCommand> executableCommands)
		{
			foreach (MyAnimationDefinition def2 in MyDefinitionManager.Static.GetAnimationDefinitions())
			{
				if (!string.IsNullOrWhiteSpace(def2.ChatCommand) && def2.ChatCommand.Equals(command, StringComparison.InvariantCultureIgnoreCase))
				{
					MyChatCommand item = new MyChatCommand(def2.ChatCommand, def2.ChatCommandName, def2.ChatCommandDescription, delegate
					{
						MyAnimationActivator.Activate(def2);
					});
					executableCommands.Add(item);
					return;
				}
			}
			if (!Sync.IsDedicated)
			{
				foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
				{
					if (inventoryItem != null && inventoryItem.ItemDefinition != null && inventoryItem.ItemDefinition.ItemSlot == MyGameInventoryItemSlot.Emote)
					{
						MyEmoteDefinition def = MyDefinitionManager.Static.GetDefinition<MyEmoteDefinition>(inventoryItem.ItemDefinition.AssetModifierId);
						if (def != null && !string.IsNullOrWhiteSpace(def.ChatCommand) && def.ChatCommand.Equals(command, StringComparison.InvariantCultureIgnoreCase))
						{
							MyChatCommand item2 = new MyChatCommand(def.ChatCommand, def.ChatCommandName, def.ChatCommandDescription, delegate
							{
								MyAnimationActivator.Activate(def);
							});
							executableCommands.Add(item2);
							break;
						}
					}
				}
			}
		}
	}
}
