using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_HydrogenTip", 150)]
	internal class MyIngameHelpHydrogenTip : MyIngameHelpObjective
	{
		public MyIngameHelpHydrogenTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Hydrogen_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Hydrogen"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HydrogenTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_HydrogenTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
