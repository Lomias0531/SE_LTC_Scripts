using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_InventoryTip", 110)]
	internal class MyIngameHelpInventoryTip : MyIngameHelpObjective
	{
		public MyIngameHelpInventoryTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Inventory_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Inventory"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_InventoryTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_InventoryTip_Detail2
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 4f;
		}
	}
}
