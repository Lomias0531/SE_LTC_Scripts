using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_OwnershipTip", 90)]
	internal class MyIngameHelpOwnershipTip : MyIngameHelpObjective
	{
		public MyIngameHelpOwnershipTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Ownership_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Ownership"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_OwnershipTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_OwnershipTip_Detail2,
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
