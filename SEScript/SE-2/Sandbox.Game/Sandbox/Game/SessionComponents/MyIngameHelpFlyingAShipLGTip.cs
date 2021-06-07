using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_FlyingAShipLGTip", 165)]
	internal class MyIngameHelpFlyingAShipLGTip : MyIngameHelpObjective
	{
		public MyIngameHelpFlyingAShipLGTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_FlyingAShip_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_FlyingAShipLG"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShipLGTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShipLGTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
