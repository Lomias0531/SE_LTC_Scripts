using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_StuckTip", 470)]
	internal class MyIngameHelpStuckTip : MyIngameHelpObjective
	{
		public MyIngameHelpStuckTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Stuck_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Stuck"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_StuckTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_StuckTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
