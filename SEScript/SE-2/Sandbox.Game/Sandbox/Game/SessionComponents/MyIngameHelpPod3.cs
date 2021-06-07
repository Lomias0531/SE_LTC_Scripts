using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Pod3", 25)]
	internal class MyIngameHelpPod3 : MyIngameHelpObjective
	{
		public MyIngameHelpPod3()
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
					TextEnum = MySpaceTexts.IngameHelp_Pod3_Detail1
				}
			};
			DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
		}
	}
}
