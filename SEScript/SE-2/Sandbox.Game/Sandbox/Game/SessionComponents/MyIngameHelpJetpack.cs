using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Jetpack", 40)]
	internal class MyIngameHelpJetpack : MyIngameHelpObjective
	{
		private bool m_jetpackEnabled;

		private bool m_downPressed;

		private bool m_upPressed;

		private bool m_forwardPressed;

		private bool m_backPressed;

		private bool m_leftPressed;

		private bool m_rightPressed;

		private bool m_rollLeftPressed;

		private bool m_rollRightPressed;

		public MyIngameHelpJetpack()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Jetpack_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = JetpackInWorldSettings;
			Details = new MyIngameHelpDetail[5]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail2,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.THRUSTS)
					},
					FinishCondition = JetpackCondition
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail3,
					Args = new object[2]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.JUMP),
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.CROUCH)
					},
					FinishCondition = FlyCondition
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail4,
					Args = new object[4]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.FORWARD),
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.BACKWARD),
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.STRAFE_LEFT),
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.STRAFE_RIGHT)
					},
					FinishCondition = WSADCondition
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Jetpack_Detail5,
					Args = new object[2]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.ROLL_LEFT),
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.ROLL_RIGHT)
					},
					FinishCondition = RollCondition
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_Jetpack2";
		}

		private bool JetpackInWorldSettings()
		{
			if (MySession.Static != null && MySession.Static.Settings.EnableJetpack && MySession.Static.LocalCharacter != null)
			{
				return !MySession.Static.LocalCharacter.IsSitting;
			}
			return false;
		}

		private bool JetpackCondition()
		{
			if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.EnabledThrusts)
			{
				m_jetpackEnabled = true;
			}
			return m_jetpackEnabled;
		}

		private bool FlyCondition()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity == null || !controlledEntity.EnabledThrusts)
			{
				return false;
			}
			m_downPressed |= (controlledEntity.LastMotionIndicator.Y < 0f);
			m_upPressed |= (controlledEntity.LastMotionIndicator.Y > 0f);
			if (m_downPressed)
			{
				return m_upPressed;
			}
			return false;
		}

		private bool WSADCondition()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity == null || !controlledEntity.EnabledThrusts)
			{
				return false;
			}
			m_forwardPressed |= (controlledEntity.LastMotionIndicator.Z > 0f);
			m_backPressed |= (controlledEntity.LastMotionIndicator.Z < 0f);
			m_leftPressed |= (controlledEntity.LastMotionIndicator.X < 0f);
			m_rightPressed |= (controlledEntity.LastMotionIndicator.X > 0f);
			if (m_forwardPressed && m_backPressed && m_leftPressed)
			{
				return m_rightPressed;
			}
			return false;
		}

		private bool RollCondition()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity == null || !controlledEntity.EnabledThrusts)
			{
				return false;
			}
			m_rollLeftPressed |= (controlledEntity.LastRotationIndicator.Z < 0f);
			m_rollRightPressed |= (controlledEntity.LastRotationIndicator.Z > 0f);
			if (m_rollLeftPressed)
			{
				return m_rollRightPressed;
			}
			return false;
		}
	}
}
