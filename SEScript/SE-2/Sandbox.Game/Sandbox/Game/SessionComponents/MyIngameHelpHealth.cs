using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Health", 120)]
	internal class MyIngameHelpHealth : MyIngameHelpObjective
	{
		public MyIngameHelpHealth()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Health_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(LowHealth));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Health_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Health_Detail2,
					FinishCondition = HealthReplenished
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_HealthTip";
		}

		private bool LowHealth()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null && myCharacter.StatComp != null)
			{
				if (myCharacter.StatComp.HealthRatio > 0f)
				{
					return myCharacter.StatComp.HealthRatio < 0.9f;
				}
				return false;
			}
			return false;
		}

		private bool HealthReplenished()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null && myCharacter.StatComp != null)
			{
				return myCharacter.StatComp.HealthRatio > 0.99f;
			}
			return false;
		}
	}
}
