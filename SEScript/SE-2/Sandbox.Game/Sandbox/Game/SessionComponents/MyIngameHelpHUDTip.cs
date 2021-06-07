using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_HUDTip", 50)]
	internal class MyIngameHelpHUDTip : MyIngameHelpObjective
	{
		public MyIngameHelpHUDTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_HUD_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_HUD"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HUDTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HUDTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
