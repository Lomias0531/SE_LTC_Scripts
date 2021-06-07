using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using VRage.ObjectBuilders;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Battery", 650)]
	internal class MyIngameHelpBattery : MyIngameHelpObjective
	{
		private bool m_batteryAdded;

		public MyIngameHelpBattery()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Battery_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Camera"
			};
			Details = new MyIngameHelpDetail[1]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Battery_Detail1
				}
			};
			RequiredCondition = BlockAddedCondition;
			DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_Battery2";
			if (MyCubeBuilder.Static != null)
			{
				MyCubeBuilder.Static.OnBlockAdded += Static_OnBlockAdded;
			}
		}

		private void Static_OnBlockAdded(MyCubeBlockDefinition definition)
		{
			MyObjectBuilderType typeId = definition.Id.TypeId;
			m_batteryAdded = (typeId.ToString() == "MyObjectBuilder_BatteryBlock");
		}

		private bool BlockAddedCondition()
		{
			return m_batteryAdded;
		}
	}
}
