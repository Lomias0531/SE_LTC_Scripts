using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Hydrogen", 150)]
	internal class MyIngameHelpHydrogen : MyIngameHelpObjective
	{
		public MyIngameHelpHydrogen()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Hydrogen_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(LowHydrogen));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Hydrogen_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Hydrogen_Detail2,
					FinishCondition = HydrogenReplenished
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_HydrogenTip";
		}

		private bool LowHydrogen()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null && myCharacter.OxygenComponent != null)
			{
				if (myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) > 0f)
				{
					return myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) < 0.5f;
				}
				return false;
			}
			return false;
		}

		private bool HydrogenReplenished()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null && myCharacter.OxygenComponent != null)
			{
				return myCharacter.OxygenComponent.GetGasFillLevel(MyCharacterOxygenComponent.HydrogenId) > 0.99f;
			}
			return false;
		}
	}
}
