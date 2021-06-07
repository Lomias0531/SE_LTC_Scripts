using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Energy", 140)]
	internal class MyIngameHelpEnergy : MyIngameHelpObjective
	{
		public MyIngameHelpEnergy()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Energy_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(LowEnergy));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Energy_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Energy_Detail2,
					FinishCondition = EnergyReplenished
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_EnergyTip";
		}

		private bool LowEnergy()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				if (myCharacter.SuitEnergyLevel > 0f)
				{
					return myCharacter.SuitEnergyLevel < 0.5f;
				}
				return false;
			}
			return false;
		}

		private bool EnergyReplenished()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				return myCharacter.SuitEnergyLevel > 0.99f;
			}
			return false;
		}
	}
}
