using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_GrinderTip", 180)]
	internal class MyIngameHelpGrinderTip : MyIngameHelpObjective
	{
		public MyIngameHelpGrinderTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Grinder_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Grinder"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_GrinderTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_GrinderTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
