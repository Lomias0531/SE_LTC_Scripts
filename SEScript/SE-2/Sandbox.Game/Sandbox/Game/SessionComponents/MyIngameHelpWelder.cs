using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Welder", 190)]
	internal class MyIngameHelpWelder : MyIngameHelpObjective
	{
		public MyIngameHelpWelder()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Welder_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(PlayerHasWelder));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Welder_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Welder_Detail2,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.PRIMARY_TOOL_ACTION)
					},
					FinishCondition = PlayerIsWelding
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_WelderTip";
		}

		private bool PlayerHasWelder()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				return myCharacter.EquippedTool is MyWelder;
			}
			return false;
		}

		private bool PlayerIsWelding()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				if (myCharacter.EquippedTool is MyWelder && !string.IsNullOrEmpty((myCharacter.EquippedTool as MyWelder).EffectId) && (myCharacter.EquippedTool as MyWelder).IsShooting)
				{
					return (myCharacter.EquippedTool as MyWelder).HasHitBlock;
				}
				return false;
			}
			return false;
		}
	}
}
