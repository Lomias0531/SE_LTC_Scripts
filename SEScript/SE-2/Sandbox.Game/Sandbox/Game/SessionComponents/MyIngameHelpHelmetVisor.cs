using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_HelmetVisor", 330)]
	internal class MyIngameHelpHelmetVisor : MyIngameHelpObjective
	{
		private bool m_damageFromLowOxygen;

		private bool m_helmetClosed;

		public MyIngameHelpHelmetVisor()
		{
			TitleEnum = MySpaceTexts.IngameHelp_HelmetVisor_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(DamageFromLowOxygen));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HelmetVisor_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HelmetVisor_Detail2,
					FinishCondition = HelmetClosed
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_HelmetVisorTip";
		}

		public override bool IsCritical()
		{
			return DamageFromLowOxygen();
		}

		private bool DamageFromLowOxygen()
		{
			if (MySession.Static != null && MySession.Static.LocalCharacter != null && MySession.Static.ControlledEntity == MySession.Static.LocalCharacter && MySession.Static.LocalCharacter.Breath != null)
			{
				if (MySession.Static.LocalCharacter.Breath.CurrentState != MyCharacterBreath.State.Choking)
				{
					return MySession.Static.LocalCharacter.Breath.CurrentState == MyCharacterBreath.State.NoBreath;
				}
				return true;
			}
			return false;
		}

		private bool HelmetClosed()
		{
			MySession @static = MySession.Static;
			if (@static != null && @static.LocalCharacter?.OxygenComponent.HelmetEnabled == true)
			{
				m_helmetClosed = true;
			}
			return m_helmetClosed;
		}
	}
}
