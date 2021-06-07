using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_BuildingTip2", 60)]
	internal class MyIngameHelpBuildingTip2 : MyIngameHelpObjective
	{
		private bool m_blockSelected;

		private bool m_gPressed;

		private bool m_toolbarDrop;

		public MyIngameHelpBuildingTip2()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Building_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Building3"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_BuildingTip2_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_BuildingTip2_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
