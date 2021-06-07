using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_MagneticBoots", 160)]
	internal class MyIngameHelpMagneticBoots : MyIngameHelpObjective
	{
		private Queue<float> m_averageGravity = new Queue<float>();

		public MyIngameHelpMagneticBoots()
		{
			TitleEnum = MySpaceTexts.IngameHelp_MagneticBoots_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Jetpack2"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(ZeroGravity));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_MagneticBoots_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_MagneticBoots_Detail2,
					FinishCondition = BootsLocked
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_MagneticBootsTip";
		}

		private bool ZeroGravity()
		{
			int num = 5;
			MyCharacter myCharacter = (MySession.Static != null) ? MySession.Static.LocalCharacter : null;
			if (myCharacter != null && myCharacter.CurrentMovementState == MyCharacterMovementEnum.Flying)
			{
				m_averageGravity.Enqueue(myCharacter.Gravity.LengthSquared());
				if (m_averageGravity.Count < num)
				{
					return false;
				}
				if (m_averageGravity.Count > num)
				{
					m_averageGravity.Dequeue();
				}
				return m_averageGravity.Average() < 0.001f;
			}
			return false;
		}

		private bool BootsLocked()
		{
			return ((MySession.Static != null) ? MySession.Static.LocalCharacter : null)?.IsMagneticBootsActive ?? false;
		}
	}
}
