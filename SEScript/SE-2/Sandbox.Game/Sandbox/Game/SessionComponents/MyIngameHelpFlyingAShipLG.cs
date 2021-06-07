using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_FlyingAShipLG", 165)]
	internal class MyIngameHelpFlyingAShipLG : MyIngameHelpObjective
	{
		private bool m_toggleLandingGear;

		private bool m_initialLandingGear;

		public MyIngameHelpFlyingAShipLG()
		{
			TitleEnum = MySpaceTexts.IngameHelp_FlyingAShip_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = (Func<bool>)Delegate.Combine(RequiredCondition, new Func<bool>(InsidePoweredGridWithLG));
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShipLG_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_FlyingAShipLG_Detail2,
					FinishCondition = ToggleLandingGear
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_FlyingAShipLGTip";
		}

		public override void OnActivated()
		{
			base.OnActivated();
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				m_initialLandingGear = controlledEntity.EnabledLeadingGears;
			}
		}

		private bool InsidePoweredGridWithLG()
		{
			MyCockpit myCockpit = MySession.Static.ControlledEntity as MyCockpit;
			if (myCockpit != null && myCockpit.CubeGrid.IsPowered && myCockpit.BlockDefinition.EnableShipControl && myCockpit.ControlThrusters)
			{
				return myCockpit.CubeGrid.GridSystems.LandingSystem.TotalGearCount > 0;
			}
			return false;
		}

		private bool ToggleLandingGear()
		{
			if (InsidePoweredGridWithLG() && !m_toggleLandingGear)
			{
				m_toggleLandingGear = (m_initialLandingGear != MySession.Static.ControlledEntity.EnabledLeadingGears);
			}
			return m_toggleLandingGear;
		}
	}
}
