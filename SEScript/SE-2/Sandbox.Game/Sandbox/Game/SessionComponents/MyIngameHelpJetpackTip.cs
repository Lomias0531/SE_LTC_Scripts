using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_JetpackTip", 50)]
	internal class MyIngameHelpJetpackTip : MyIngameHelpObjective
	{
		public MyIngameHelpJetpackTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Jetpack_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Jetpack2"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_JetpackTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_JetpackTip_Detail2,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.DAMPING)
					}
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
