using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_IntroTip2", 10)]
	internal class MyIngameHelpIntroTip2 : MyIngameHelpObjective
	{
		public MyIngameHelpIntroTip2()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Intro_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_IntroTip"
			};
			Details = new MyIngameHelpDetail[1]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_IntroTip2_Detail1
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
