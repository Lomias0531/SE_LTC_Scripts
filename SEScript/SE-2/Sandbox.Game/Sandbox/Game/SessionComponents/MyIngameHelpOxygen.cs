using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Oxygen", 130)]
	internal class MyIngameHelpOxygen : MyIngameHelpObjective
	{
		public MyIngameHelpOxygen()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Oxygen_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(LowOxygen));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Oxygen_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Oxygen_Detail2,
					FinishCondition = OxygenReplenished
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_OxygenTip";
		}

		private bool LowOxygen()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null && myCharacter.OxygenComponent != null)
			{
				if (myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) > 0f)
				{
					return myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) < 0.5f;
				}
				return false;
			}
			return false;
		}

		private bool OxygenReplenished()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null && myCharacter.OxygenComponent != null)
			{
				return myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.OxygenId) > 0.99f;
			}
			return false;
		}
	}
}
