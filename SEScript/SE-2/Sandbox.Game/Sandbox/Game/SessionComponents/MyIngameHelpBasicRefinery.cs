using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_BasicRefinery", 550)]
	internal class MyIngameHelpBasicRefinery : MyIngameHelpObjective
	{
		private bool m_refineryAdded;

		public MyIngameHelpBasicRefinery()
		{
			TitleEnum = MySpaceTexts.IngameHelp_BasicRefinery_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Camera"
			};
			Details = new MyIngameHelpDetail[1]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_BasicRefinery_Detail1
				}
			};
			RequiredCondition = BlockAddedCondition;
			DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			if (MyCubeBuilder.Static != null)
			{
				MyCubeBuilder.Static.OnBlockAdded += Static_OnBlockAdded;
			}
		}

		private void Static_OnBlockAdded(MyCubeBlockDefinition definition)
		{
			m_refineryAdded = (definition.Id.SubtypeName == "Blast Furnace" || definition.Id.SubtypeName == "BasicAssembler");
		}

		private bool BlockAddedCondition()
		{
			return m_refineryAdded;
		}
	}
}
