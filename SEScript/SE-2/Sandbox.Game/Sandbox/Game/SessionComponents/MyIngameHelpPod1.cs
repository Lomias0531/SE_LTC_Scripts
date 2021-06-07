using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Pod1", 23)]
	internal class MyIngameHelpPod1 : MyIngameHelpObjective
	{
		public static bool StartingInPod;

		public MyIngameHelpPod1()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Pod_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Camera"
			};
			Details = new MyIngameHelpDetail[1]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Pod1_Detail1
				}
			};
			DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			RequiredCondition = PlayerInPod;
			FollowingId = "IngameHelp_Pod2";
		}

		private bool PlayerInPod()
		{
			return StartingInPod;
		}
	}
}
