using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_PowerTip", 100)]
	internal class MyIngameHelpPowerTip : MyIngameHelpObjective
	{
		public MyIngameHelpPowerTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Power_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Power"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_PowerTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_PowerTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
