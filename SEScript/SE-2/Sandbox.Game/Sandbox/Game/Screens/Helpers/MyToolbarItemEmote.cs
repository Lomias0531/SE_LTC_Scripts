using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.GameServices;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemEmote))]
	internal class MyToolbarItemEmote : MyToolbarItemDefinition
	{
		public override bool Activate()
		{
			if (Definition == null || !MyGameService.HasInventoryItem(Definition.Id.SubtypeName))
			{
				return false;
			}
			bool flag = MySession.Static.ControlledEntity is MyCockpit;
			MyCharacter myCharacter = flag ? ((MyCockpit)MySession.Static.ControlledEntity).Pilot : MySession.Static.LocalCharacter;
			if (myCharacter != null)
			{
				MyAnimationDefinition animationForCharacter = ((MyEmoteDefinition)Definition).GetAnimationForCharacter(myCharacter.Definition.Id);
				if (animationForCharacter == null)
				{
					return false;
				}
				if (myCharacter.IsOnLadder)
				{
					return true;
				}
				if (flag && !animationForCharacter.AllowInCockpit)
				{
					return true;
				}
				if (myCharacter.UseNewAnimationSystem)
				{
					if (!MySession.Static.GetComponent<MySessionComponentDLC>().HasDefinitionDLC(Definition, MySession.Static.LocalHumanPlayer.Id.SteamId))
					{
						return false;
					}
					string subtypeName = animationForCharacter.Id.SubtypeName;
					myCharacter.TriggerCharacterAnimationEvent("emote", sync: true, animationForCharacter.InfluenceAreas);
					myCharacter.TriggerCharacterAnimationEvent(subtypeName, sync: true, animationForCharacter.InfluenceAreas);
				}
			}
			return true;
		}

		public override bool Init(MyObjectBuilder_ToolbarItem objBuilder)
		{
			base.Init(objBuilder);
			base.ActivateOnClick = true;
			base.WantsToBeActivated = true;
			if (Sync.IsDedicated)
			{
				return true;
			}
			MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(Definition.Id.SubtypeName);
			if (inventoryItemDefinition == null || inventoryItemDefinition.ItemSlot != MyGameInventoryItemSlot.Emote)
			{
				return false;
			}
			return MyGameService.HasInventoryItem(Definition.Id.SubtypeName);
		}

		public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
		{
			return base.GetObjectBuilder() as MyObjectBuilder_ToolbarItemEmote;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			if (type != 0 && type != MyToolbarType.Ship)
			{
				return type == MyToolbarType.Seat;
			}
			return true;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			ChangeInfo changeInfo = ChangeInfo.None;
			if (Sync.IsDedicated)
			{
				return changeInfo;
			}
			if (MySession.Static.LocalHumanPlayer == null)
			{
				if (base.Enabled)
				{
					changeInfo |= SetEnabled(newEnabled: false);
				}
				return changeInfo;
			}
			bool flag = MyGameService.HasInventoryItem(Definition.Id.SubtypeName);
			MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
			flag &= component.HasDefinitionDLC(Definition, MySession.Static.LocalHumanPlayer.Id.SteamId);
			if (base.Enabled != flag)
			{
				changeInfo |= SetEnabled(flag);
			}
			return changeInfo;
		}
	}
}
