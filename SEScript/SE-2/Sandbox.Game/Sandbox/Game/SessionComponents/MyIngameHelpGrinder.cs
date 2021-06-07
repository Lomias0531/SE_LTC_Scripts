using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Grinder", 180)]
	internal class MyIngameHelpGrinder : MyIngameHelpObjective
	{
		public MyIngameHelpGrinder()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Grinder_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(PlayerHasGrinder));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Grinder_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Grinder_Detail2,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.PRIMARY_TOOL_ACTION)
					},
					FinishCondition = PlayerIsGrinding
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_GrinderTip";
		}

		private bool PlayerHasGrinder()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				return myCharacter.EquippedTool is MyAngleGrinder;
			}
			return false;
		}

		private bool PlayerIsGrinding()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				if (myCharacter.EquippedTool is MyAngleGrinder)
				{
					return !string.IsNullOrEmpty((myCharacter.EquippedTool as MyAngleGrinder).EffectId);
				}
				return false;
			}
			return false;
		}
	}
}
