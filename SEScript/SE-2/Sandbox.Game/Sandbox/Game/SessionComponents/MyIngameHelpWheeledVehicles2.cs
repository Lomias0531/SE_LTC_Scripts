using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_WheeledVehicles2", 230)]
	internal class MyIngameHelpWheeledVehicles2 : MyIngameHelpObjective
	{
		private bool m_xPressed;

		public MyIngameHelpWheeledVehicles2()
		{
			TitleEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_WheeledVehicles"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(InsidePoweredWheeledGrid));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles2_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles2_Detail2,
					FinishCondition = XPressed
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_WheeledVehiclesTip";
		}

		private bool InsidePoweredWheeledGrid()
		{
			MyShipController controller;
			return TryGetPoweredWheeledGrid(out controller);
		}

		private bool TryGetPoweredWheeledGrid(out MyShipController controller)
		{
			MyCockpit myCockpit;
			if ((myCockpit = (MySession.Static.ControlledEntity as MyCockpit)) != null && myCockpit.CubeGrid.IsPowered && myCockpit.BlockDefinition.EnableShipControl && myCockpit.ControlWheels && myCockpit.CubeGrid.GridSystems.WheelSystem.WheelCount > 0)
			{
				controller = myCockpit;
				return true;
			}
			controller = null;
			return false;
		}

		private bool XPressed()
		{
			if (TryGetPoweredWheeledGrid(out MyShipController controller) && !m_xPressed && controller.CubeGrid.GridSystems.WheelSystem.LastJumpInput)
			{
				m_xPressed = true;
			}
			return m_xPressed;
		}
	}
}
