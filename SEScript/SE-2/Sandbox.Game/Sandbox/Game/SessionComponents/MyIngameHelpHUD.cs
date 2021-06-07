using Sandbox.Game.Gui;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_HUD", 50)]
	internal class MyIngameHelpHUD : MyIngameHelpObjective
	{
		private bool m_hudStateChanged;

		private bool m_signalsChangedPressed;

		private int m_initialHudState;

		private MyHudMarkerRender.SignalMode m_initialSignalMode;

		public MyIngameHelpHUD()
		{
			TitleEnum = MySpaceTexts.IngameHelp_HUD_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			Details = new MyIngameHelpDetail[3]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HUD_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HUD_Detail2,
					FinishCondition = TabCondition
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HUD_Detail3,
					FinishCondition = SignalCondition
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_HUDTip";
			DelayToAppear = (float)TimeSpan.FromMinutes(3.0).TotalSeconds;
		}

		public override void OnActivated()
		{
			base.OnActivated();
			m_initialHudState = MyHud.HudState;
			m_initialSignalMode = MyHudMarkerRender.SignalDisplayMode;
		}

		private bool TabCondition()
		{
			m_hudStateChanged |= (MyHud.HudState != m_initialHudState);
			return m_hudStateChanged;
		}

		private bool SignalCondition()
		{
			m_signalsChangedPressed |= (MyHudMarkerRender.SignalDisplayMode != m_initialSignalMode);
			return m_signalsChangedPressed;
		}
	}
}
