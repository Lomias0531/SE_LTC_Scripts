using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_EnergyTip", 140)]
	internal class MyIngameHelpEnergyTip : MyIngameHelpObjective
	{
		public MyIngameHelpEnergyTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Energy_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Energy"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_EnergyTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_EnergyTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
