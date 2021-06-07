using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_FlashlightTip", 84)]
	internal class MyIngameHelpFlashlightTip : MyIngameHelpObjective
	{
		public MyIngameHelpFlashlightTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Flashlight_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Flashlight"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlashlightTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlashlightTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
