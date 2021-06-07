using Havok;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage.Game;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Movement", 30)]
	internal class MyIngameHelpMovement : MyIngameHelpObjective
	{
		private bool m_crouched;

		private bool m_jumped;

		private bool m_forwardPressed;

		private bool m_backPressed;

		private bool m_leftPressed;

		private bool m_rightPressed;

		private bool m_running;

		public MyIngameHelpMovement()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Movement_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Intro"
			};
			RequiredCondition = StandingCondition;
			Details = new MyIngameHelpDetail[5]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Movement_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Movement_Detail2,
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
					TextEnum = MySpaceTexts.IngameHelp_Movement_Detail3,
					Args = new object[2]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.SPRINT),
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.FORWARD)
					},
					FinishCondition = SprintCondition
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Movement_Detail4,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.JUMP)
					},
					FinishCondition = JumpCondition
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Movement_Detail5,
					Args = new object[1]
					{
						MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.CROUCH)
					},
					FinishCondition = CrouchCondition
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
		}

		private bool StandingCondition()
		{
			MyCharacter myCharacter;
			if ((myCharacter = (MySession.Static.ControlledEntity as MyCharacter)) != null)
			{
				return myCharacter.CharacterGroundState == HkCharacterStateType.HK_CHARACTER_ON_GROUND;
			}
			return false;
		}

		private bool WSADCondition()
		{
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity == null)
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

		private bool JumpCondition()
		{
			MyCharacter myCharacter;
			if ((myCharacter = (MySession.Static.ControlledEntity as MyCharacter)) != null && myCharacter.CurrentMovementState == MyCharacterMovementEnum.Jump)
			{
				m_jumped = true;
			}
			return m_jumped;
		}

		private bool SprintCondition()
		{
			MyCharacter myCharacter;
			if ((myCharacter = (MySession.Static.ControlledEntity as MyCharacter)) != null && myCharacter.CharacterGroundState == HkCharacterStateType.HK_CHARACTER_ON_GROUND && myCharacter.IsSprinting)
			{
				m_running = true;
			}
			return m_running;
		}

		private bool CrouchCondition()
		{
			MyCharacter myCharacter;
			if ((myCharacter = (MySession.Static.ControlledEntity as MyCharacter)) != null && myCharacter.CharacterGroundState == HkCharacterStateType.HK_CHARACTER_ON_GROUND && myCharacter.IsCrouching)
			{
				m_crouched = true;
			}
			return m_crouched;
		}
	}
}
