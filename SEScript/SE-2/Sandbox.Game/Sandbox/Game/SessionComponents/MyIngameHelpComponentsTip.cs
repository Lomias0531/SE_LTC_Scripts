using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_ComponentsTip", 230)]
	internal class MyIngameHelpComponentsTip : MyIngameHelpObjective
	{
		public MyIngameHelpComponentsTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Components_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Components"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_ComponentsTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_ComponentsTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
