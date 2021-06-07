using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_WheeledVehicles", 230)]
	internal class MyIngameHelpWheeledVehicles : MyIngameHelpObjective
	{
		private bool m_powerSwitched;

		private bool m_toggleLandingGear;

		private bool m_forwardPressed;

		private bool m_backPressed;

		private bool m_leftPressed;

		private bool m_rightPressed;

		private bool m_spacePressed;

		private bool m_originalHandbrake;

		public MyIngameHelpWheeledVehicles()
		{
			TitleEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(InsidePoweredWheeledGrid));
			Details = new MyIngameHelpDetail[5]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail2,
					FinishCondition = PowerSwitched
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail3,
					FinishCondition = ToggleLandingGear
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail4,
					FinishCondition = WSADCondition
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_WheeledVehicles_Detail5,
					FinishCondition = BrakeCondition
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
		}

		public override void OnActivated()
		{
			base.OnActivated();
			if (TryGetPoweredWheeledGrid(out MyShipController controller))
			{
				m_originalHandbrake = controller.CubeGrid.GridSystems.WheelSystem.HandBrake;
			}
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

		private bool PowerSwitched()
		{
			if (!m_powerSwitched)
			{
				MyCockpit myCockpit;
				m_powerSwitched = ((myCockpit = (MySession.Static.ControlledEntity as MyCockpit)) != null && !myCockpit.CubeGrid.IsPowered);
			}
			return m_powerSwitched;
		}

		private bool ToggleLandingGear()
		{
			if (TryGetPoweredWheeledGrid(out MyShipController controller))
			{
				m_toggleLandingGear |= (m_originalHandbrake != controller.CubeGrid.GridSystems.WheelSystem.HandBrake);
			}
			return m_toggleLandingGear;
		}

		private bool WSADCondition()
		{
			if (!TryGetPoweredWheeledGrid(out MyShipController controller))
			{
				return false;
			}
			m_forwardPressed |= (controller.LastMotionIndicator.Z > 0f);
			m_backPressed |= (controller.LastMotionIndicator.Z < 0f);
			m_leftPressed |= (controller.LastMotionIndicator.X < 0f);
			m_rightPressed |= (controller.LastMotionIndicator.X > 0f);
			if (m_forwardPressed && m_backPressed && m_leftPressed)
			{
				return m_rightPressed;
			}
			return false;
		}

		private bool BrakeCondition()
		{
			if (TryGetPoweredWheeledGrid(out MyShipController controller))
			{
				m_spacePressed |= controller.CubeGrid.GridSystems.WheelSystem.Brake;
			}
			return m_spacePressed;
		}
	}
}
