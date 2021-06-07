using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using VRage.ObjectBuilders;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Turbine", 500)]
	internal class MyIngameHelpTurbine : MyIngameHelpObjective
	{
		private bool m_turbineAdded;

		public MyIngameHelpTurbine()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Turbine_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Camera"
			};
			Details = new MyIngameHelpDetail[1]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Turbine_Detail1
				}
			};
			RequiredCondition = BlockAddedCondition;
			DelayToHide = 4f * MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			if (MyCubeBuilder.Static != null)
			{
				MyCubeBuilder.Static.OnBlockAdded += Static_OnBlockAdded;
			}
			FollowingId = "IngameHelp_Turbine2";
		}

		private void Static_OnBlockAdded(MyCubeBlockDefinition definition)
		{
			MyObjectBuilderType typeId = definition.Id.TypeId;
			int turbineAdded;
			if (!(typeId.ToString() == "MyObjectBuilder_WindTurbine"))
			{
				typeId = definition.Id.TypeId;
				turbineAdded = ((typeId.ToString() == "MyObjectBuilder_SolarPanel") ? 1 : 0);
			}
			else
			{
				turbineAdded = 1;
			}
			m_turbineAdded = ((byte)turbineAdded != 0);
		}

		private bool BlockAddedCondition()
		{
			return m_turbineAdded;
		}
	}
}
