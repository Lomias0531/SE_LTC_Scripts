using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_WelderTip", 190)]
	internal class MyIngameHelpWelderTip : MyIngameHelpObjective
	{
		public MyIngameHelpWelderTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Welder_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Welder"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WelderTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WelderTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
