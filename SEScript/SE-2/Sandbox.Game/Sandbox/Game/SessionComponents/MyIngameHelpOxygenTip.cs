using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_OxygenTip", 130)]
	internal class MyIngameHelpOxygenTip : MyIngameHelpObjective
	{
		public MyIngameHelpOxygenTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Oxygen_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Oxygen"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_OxygenTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_OxygenTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
