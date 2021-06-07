using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_BuildingTip", 60)]
	internal class MyIngameHelpBuildingTip : MyIngameHelpObjective
	{
		private bool m_blockSelected;

		private bool m_gPressed;

		private bool m_toolbarDrop;

		public MyIngameHelpBuildingTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Building_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Building"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_BuildingTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_BuildingTip_Detail2,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.SECONDARY_TOOL_ACTION)
					}
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
