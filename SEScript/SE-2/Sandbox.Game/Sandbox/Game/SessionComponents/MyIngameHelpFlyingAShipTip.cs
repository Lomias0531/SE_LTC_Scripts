using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_FlyingAShipTip", 170)]
	internal class MyIngameHelpFlyingAShipTip : MyIngameHelpObjective
	{
		public MyIngameHelpFlyingAShipTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_FlyingAShip_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_FlyingAShip"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShipTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShipTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
