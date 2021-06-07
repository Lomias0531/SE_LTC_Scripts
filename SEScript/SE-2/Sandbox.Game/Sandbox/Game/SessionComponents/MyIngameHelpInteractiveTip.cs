using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_InteractiveTip", 23)]
	internal class MyIngameHelpInteractiveTip : MyIngameHelpObjective
	{
		public MyIngameHelpInteractiveTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Interactive_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Interactive"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_InteractiveTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_InteractiveTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
