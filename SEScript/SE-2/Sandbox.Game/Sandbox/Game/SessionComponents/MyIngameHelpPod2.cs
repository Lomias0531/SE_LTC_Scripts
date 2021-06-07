using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Pod2", 24)]
	internal class MyIngameHelpPod2 : MyIngameHelpObjective
	{
		public MyIngameHelpPod2()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Pod_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Pod1"
			};
			Details = new MyIngameHelpDetail[1]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Pod2_Detail1
				}
			};
			DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_Pod3";
		}
	}
}
