using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_MagneticBootsTip", 160)]
	internal class MyIngameHelpMagneticBootsTip : MyIngameHelpObjective
	{
		public MyIngameHelpMagneticBootsTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_MagneticBoots_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_MagneticBoots"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_MagneticBootsTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_MagneticBootsTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
