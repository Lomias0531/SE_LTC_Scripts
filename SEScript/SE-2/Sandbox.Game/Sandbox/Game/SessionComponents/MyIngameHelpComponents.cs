using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Components", 230)]
	internal class MyIngameHelpComponents : MyIngameHelpObjective
	{
		public MyIngameHelpComponents()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Components_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Ingots"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(ComponentsInInventory));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Components_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Components_Detail2,
					FinishCondition = BlockRepaired
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_ComponentsTip";
		}

		private bool ComponentsInInventory()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				foreach (MyPhysicalInventoryItem item in myCharacter.GetInventory().GetItems())
				{
					if (item.Content is MyObjectBuilder_Component)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool BlockRepaired()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				if (myCharacter.EquippedTool is MyWelder && !string.IsNullOrEmpty((myCharacter.EquippedTool as MyWelder).EffectId) && (myCharacter.EquippedTool as MyWelder).IsShooting && (myCharacter.EquippedTool as MyWelder).HasHitBlock)
				{
					return !(myCharacter.EquippedTool as MyWelder).GetTargetBlock().IsFullIntegrity;
				}
				return false;
			}
			return false;
		}
	}
}
