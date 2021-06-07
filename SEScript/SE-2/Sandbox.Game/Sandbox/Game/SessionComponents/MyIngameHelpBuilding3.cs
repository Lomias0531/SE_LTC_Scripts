using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Building3", 80)]
	internal class MyIngameHelpBuilding3 : MyIngameHelpObjective
	{
		private bool m_blockAdded;

		public MyIngameHelpBuilding3()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Building_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Building2"
			};
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Building3_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Building3_Detail2,
					FinishCondition = BlockAddedCondition
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_BuildingTip2";
		}

		public override void OnActivated()
		{
			base.OnActivated();
			MyCubeBuilder.Static.OnBlockAdded += Static_OnBlockAdded;
		}

		private void Static_OnBlockAdded(MyCubeBlockDefinition definition)
		{
			m_blockAdded = true;
		}

		private bool BlockAddedCondition()
		{
			return m_blockAdded;
		}
	}
}
