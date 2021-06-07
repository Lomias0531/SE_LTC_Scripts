using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Rifle", 200)]
	internal class MyIngameHelpRifle : MyIngameHelpObjective
	{
		public MyIngameHelpRifle()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Rifle_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(PlayerHasRifle));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Rifle_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Rifle_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}

		private bool PlayerHasRifle()
		{
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null)
			{
				return myCharacter.EquippedTool is MyAutomaticRifleGun;
			}
			return false;
		}
	}
}
