using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Building2", 70)]
	internal class MyIngameHelpBuilding2 : MyIngameHelpObjective
	{
		private class MouseAndKeyboardVersion
		{
			private bool m_insertPressed;

			private bool m_deletePressed;

			private bool m_homePressed;

			private bool m_endPressed;

			private bool m_pageUpPressed;

			private bool m_pageDownPressed;

			public MouseAndKeyboardVersion(MyIngameHelpBuilding2 help)
			{
				help.Details = new MyIngameHelpDetail[3]
				{
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building2_Detail1
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building2_Detail2,
						FinishCondition = help.SizeSelectCondition
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building2_Detail3,
						FinishCondition = RotateCondition
					}
				};
			}

			private bool RotateCondition()
			{
				if (!MyCubeBuilder.Static.IsActivated || MyCubeBuilder.Static.ToolbarBlockDefinition == null || MyScreenManager.FocusedControl != null)
				{
					return false;
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE))
				{
					m_insertPressed = true;
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE))
				{
					m_deletePressed = true;
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE))
				{
					m_homePressed = true;
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE))
				{
					m_endPressed = true;
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE))
				{
					m_pageUpPressed = true;
				}
				if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE))
				{
					m_pageDownPressed = true;
				}
				if (m_insertPressed && m_deletePressed && m_homePressed && m_endPressed && m_pageUpPressed)
				{
					return m_pageDownPressed;
				}
				return false;
			}
		}

		private class GamepadVersion
		{
			private bool m_axisChanged;

			private bool m_rotatedLeftOnAxis;

			private bool m_rotatedRightOnAxis;

			public GamepadVersion(MyIngameHelpBuilding2 help)
			{
				help.Details = new MyIngameHelpDetail[4]
				{
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building2_Detail1
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building2_Detail2_Gamepad,
						FinishCondition = help.SizeSelectCondition
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building2_Detail3_Gamepad,
						FinishCondition = ChangeAxisCondition
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building2_Detail4_Gamepad,
						FinishCondition = RotateOnAxisCondition
					}
				};
			}

			private bool ChangeAxisCondition()
			{
				MyStringId context = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
				m_axisChanged |= MyControllerHelper.IsControl(context, MyControlsSpace.CHANGE_ROTATION_AXIS);
				return m_axisChanged;
			}

			private bool RotateOnAxisCondition()
			{
				MyStringId context = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
				m_rotatedLeftOnAxis |= MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_LEFT, MyControlStateType.PRESSED);
				m_rotatedRightOnAxis |= MyControllerHelper.IsControl(context, MyControlsSpace.ROTATE_AXIS_RIGHT, MyControlStateType.PRESSED);
				if (m_rotatedLeftOnAxis)
				{
					return m_rotatedRightOnAxis;
				}
				return false;
			}
		}

		private bool m_blockSizeChanged;

		public MyIngameHelpBuilding2()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Building_Title;
			RequiredIds = new string[1]
			{
				"IngameHelp_Building"
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			if (MyCubeBuilder.Static != null)
			{
				MyCubeBuilder.Static.OnBlockSizeChanged += Static_OnBlockSizeChanged;
			}
		}

		private void Static_OnBlockSizeChanged()
		{
			m_blockSizeChanged = true;
		}

		private bool SizeSelectCondition()
		{
			return m_blockSizeChanged;
		}

		public override void OnBeforeActivate()
		{
			base.OnBeforeActivate();
			if (MyInput.Static.IsJoystickLastUsed)
			{
				new GamepadVersion(this);
			}
			else
			{
				new MouseAndKeyboardVersion(this);
			}
		}
	}
}
