using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Battery2", 670)]
	internal class MyIngameHelpBattery2 : MyIngameHelpObjective
	{
		public MyIngameHelpBattery2()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Battery_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Battery"
			};
			Details = new MyIngameHelpDetail[1]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Battery2_Detail1
				}
			};
			DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
		}
	}
}
