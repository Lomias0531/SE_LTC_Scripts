using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_WheeledVehiclesTip", 230)]
	internal class MyIngameHelpWheeledVehiclesTip : MyIngameHelpObjective
	{
		public MyIngameHelpWheeledVehiclesTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_WheeledVehicles2"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WheeledVehiclesTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WheeledVehiclesTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
