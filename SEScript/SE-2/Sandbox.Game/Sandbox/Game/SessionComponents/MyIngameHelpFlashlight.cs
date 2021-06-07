using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Flashlight", 584)]
	internal class MyIngameHelpFlashlight : MyIngameHelpObjective
	{
		private bool m_flashlightChanged;

		private bool m_originalFlashlight;

		public MyIngameHelpFlashlight()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Flashlight_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Flashlight_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Flashlight_Detail2,
					FinishCondition = SwitchedLights
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			DelayToAppear = (float)TimeSpan.FromMinutes(4.0).TotalSeconds;
			FollowingId = "IngameHelp_FlashlightTip";
		}

		public override void OnActivated()
		{
			base.OnActivated();
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				m_originalFlashlight = controlledEntity.EnabledLights;
			}
		}

		private bool SwitchedLights()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				m_flashlightChanged = (m_originalFlashlight != controlledEntity.EnabledLights);
			}
			return m_flashlightChanged;
		}
	}
}
