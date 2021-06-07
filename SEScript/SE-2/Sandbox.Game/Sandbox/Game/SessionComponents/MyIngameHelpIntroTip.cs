using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_IntroTip", 10)]
	internal class MyIngameHelpIntroTip : MyIngameHelpObjective
	{
		public MyIngameHelpIntroTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Intro_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_IntroTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_IntroTip_Detail2,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.CHAT_SCREEN)
					}
				}
			};
			FollowingId = "IngameHelp_IntroTip2";
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 6f;
		}
	}
}
