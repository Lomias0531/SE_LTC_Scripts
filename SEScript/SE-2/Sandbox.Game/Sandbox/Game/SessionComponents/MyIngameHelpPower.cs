using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Power", 100)]
	internal class MyIngameHelpPower : MyIngameHelpObjective
	{
		private bool m_powerEnabled;

		public MyIngameHelpPower()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Power_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(InsideUnpoweredGrid));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Power_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Power_Detail2,
					FinishCondition = PowerEnabled
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_PowerTip";
		}

		private bool InsideUnpoweredGrid()
		{
			MyCockpit myCockpit = MySession.Static.ControlledEntity as MyCockpit;
			if (myCockpit != null && !myCockpit.CubeGrid.IsPowered && myCockpit.BlockDefinition.EnableShipControl)
			{
				return myCockpit.ControlThrusters;
			}
			return false;
		}

		private bool PowerEnabled()
		{
			if (!m_powerEnabled)
			{
				m_powerEnabled = ((MySession.Static.ControlledEntity as MyCockpit)?.CubeGrid.IsPowered ?? false);
			}
			return m_powerEnabled;
		}
	}
}
