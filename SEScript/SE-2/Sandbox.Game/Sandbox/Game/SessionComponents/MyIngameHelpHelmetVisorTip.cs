using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_HelmetVisorTip", 330)]
	internal class MyIngameHelpHelmetVisorTip : MyIngameHelpObjective
	{
		public MyIngameHelpHelmetVisorTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_HelmetVisor_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_HelmetVisor"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HelmetVisorTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HelmetVisorTip_Detail2,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.HELP_SCREEN)
					}
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
