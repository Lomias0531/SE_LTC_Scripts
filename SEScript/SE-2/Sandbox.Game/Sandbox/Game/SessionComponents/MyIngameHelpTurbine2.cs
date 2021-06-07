using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Turbine2", 520)]
	internal class MyIngameHelpTurbine2 : MyIngameHelpObjective
	{
		public MyIngameHelpTurbine2()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Turbine_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Turbine"
			};
			Details = new MyIngameHelpDetail[1]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Turbine2_Detail1
				}
			};
			DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
		}
	}
}
