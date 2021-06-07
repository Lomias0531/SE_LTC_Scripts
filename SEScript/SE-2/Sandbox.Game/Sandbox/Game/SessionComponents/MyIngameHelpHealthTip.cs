using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_HealthTip", 120)]
	internal class MyIngameHelpHealthTip : MyIngameHelpObjective
	{
		public MyIngameHelpHealthTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Health_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Health"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HealthTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HealthTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
