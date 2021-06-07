using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_FlyingAShip", 170)]
	internal class MyIngameHelpFlyingAShip : MyIngameHelpObjective
	{
		private bool m_cPressed;

		private bool m_spacePressed;

		private bool m_qPressed;

		private bool m_ePressed;

		private bool m_powerSwitched;

		public MyIngameHelpFlyingAShip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_FlyingAShip_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(InsidePoweredGrid));
			Details = new MyIngameHelpDetail[4]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShip_Detail2,
					FinishCondition = PowerSwitched
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShip_Detail3,
					FinishCondition = FlyCondition
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShip_Detail4,
					FinishCondition = RollCondition
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_FlyingAShipTip";
		}

		private bool TryGetPoweredGrid(out MyShipController controller)
		{
			MyCockpit myCockpit;
			if ((myCockpit = (MySession.Static.ControlledEntity as MyCockpit)) != null && myCockpit.CubeGrid.IsPowered && myCockpit.BlockDefinition.EnableShipControl && myCockpit.ControlThrusters && myCockpit.EntityThrustComponent != null && myCockpit.EntityThrustComponent.ThrustCount > 0)
			{
				controller = myCockpit;
				return true;
			}
			controller = null;
			return false;
		}

		private bool InsidePoweredGrid()
		{
			MyShipController controller;
			return TryGetPoweredGrid(out controller);
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

		private bool FlyCondition()
		{
			if (TryGetPoweredGrid(out MyShipController controller))
			{
				m_cPressed |= (controller.LastMotionIndicator.Y < 0f);
				m_spacePressed |= (controller.LastMotionIndicator.Y > 0f);
			}
			if (m_cPressed)
			{
				return m_spacePressed;
			}
			return false;
		}

		private bool RollCondition()
		{
			if (TryGetPoweredGrid(out MyShipController controller))
			{
				m_qPressed |= (controller.LastRotationIndicator.Z < 0f);
				m_ePressed |= (controller.LastRotationIndicator.Z > 0f);
			}
			if (m_qPressed)
			{
				return m_ePressed;
			}
			return false;
		}
	}
}
