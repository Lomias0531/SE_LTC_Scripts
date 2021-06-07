using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Engine.Utils
{
	public static class MySpaceBindingCreator
	{
		private enum RollMode
		{
			LS,
			Shoulders
		}

		private class JoystickBindingEvaluator : ITextEvaluator
		{
			public static void ParseToken(ref string token, out MyStringId controlContext, out MyStringId control)
			{
				int num = token.IndexOf(':');
				if (num >= 0)
				{
					controlContext = MyStringId.GetOrCompute(token.Substring(0, num));
					token = token.Substring(num + 1);
					control = MyStringId.GetOrCompute(token);
					return;
				}
				control = MyStringId.GetOrCompute(token);
				IMyControllableEntity myControllableEntity = MySession.Static?.ControlledEntity;
				if (myControllableEntity == null)
				{
					controlContext = CX_BASE;
					return;
				}
				MyStringId auxiliaryContext = myControllableEntity.AuxiliaryContext;
				if (MyControllerHelper.IsDefined(auxiliaryContext, control))
				{
					controlContext = auxiliaryContext;
				}
				else
				{
					controlContext = myControllableEntity.ControlContext;
				}
			}

			public string TokenEvaluate(string token, string context)
			{
				ParseToken(ref token, out MyStringId controlContext, out MyStringId control);
				return MyControllerHelper.GetCodeForControl(controlContext, control);
			}
		}

		private class SpaceBindingEvaluator : ITextEvaluator
		{
			public string TokenEvaluate(string token, string context)
			{
				if (MyInput.Static.IsJoystickLastUsed)
				{
					return JoystickEvaluator.TokenEvaluate(token, context);
				}
				ITextEvaluator textEvaluator;
				if ((textEvaluator = (MyInput.Static as ITextEvaluator)) != null)
				{
					JoystickBindingEvaluator.ParseToken(ref token, out MyStringId _, out MyStringId _);
					return "[" + textEvaluator.TokenEvaluate(token, context) + "]";
				}
				return token;
			}
		}

		public static readonly MyStringId CX_BASE = MyControllerHelper.CX_BASE;

		public static readonly MyStringId CX_GUI = MyControllerHelper.CX_GUI;

		public static readonly MyStringId CX_CHARACTER = MyControllerHelper.CX_CHARACTER;

		public static readonly MyStringId CX_JETPACK = MyStringId.GetOrCompute("JETPACK");

		public static readonly MyStringId CX_SPACESHIP = MyStringId.GetOrCompute("SPACESHIP");

		public static readonly MyStringId AX_BASE = MyStringId.GetOrCompute("ABASE");

		public static readonly MyStringId AX_TOOLS = MyStringId.GetOrCompute("TOOLS");

		public static readonly MyStringId AX_BUILD = MyStringId.GetOrCompute("BUILD");

		public static readonly MyStringId AX_SYMMETRY = MyStringId.GetOrCompute("AX_SYMMETRY");

		public static readonly MyStringId AX_VOXEL = MyStringId.GetOrCompute("VOXEL");

		public static readonly MyStringId AX_ACTIONS = MyStringId.GetOrCompute("ACTIONS");

		public static readonly MyStringId AX_COLOR_PICKER = MyStringId.GetOrCompute("COLOR_PICKER");

		public static readonly MyStringId AX_CLIPBOARD = MyStringId.GetOrCompute("CLIPBOARD");

		public static readonly ITextEvaluator BindingEvaluator = new SpaceBindingEvaluator();

		public static readonly ITextEvaluator JoystickEvaluator = new JoystickBindingEvaluator();

		public static void CreateBinding()
		{
			CreateForBase();
			CreateForGUI();
			CreateForCharacter();
			CreateForJetpack();
			CreateForSpaceship();
			CreateForAuxiliaryBase();
			CreateForTools();
			CreateForBuildMode();
			CreateForSymmetry();
			CreateForVoxelHands();
			CreateForClipboard();
			CreateForActions();
			CreateForColorPicker();
			MyTexts.RegisterEvaluator("CONTROL", BindingEvaluator);
			ITextEvaluator eval;
			if ((eval = (MyInput.Static as ITextEvaluator)) != null)
			{
				MyTexts.RegisterEvaluator("GAME_CONTROL", eval);
			}
			MyTexts.RegisterEvaluator("GAMEPAD", MyControllerHelper.ButtonTextEvaluator);
			MyTexts.RegisterEvaluator("GAMEPAD_CONTROL", JoystickEvaluator);
		}

		private static void CreateForBase()
		{
			MyJoystickButtonsEnum control = MyJoystickButtonsEnum.J01;
			MyControllerHelper.AddContext(CX_BASE);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.FAKE_MODIFIER_LB, MyJoystickButtonsEnum.J05);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.FAKE_MODIFIER_RB, MyJoystickButtonsEnum.J06);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.INVENTORY, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsGUI.MAIN_MENU, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J08, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.FORWARD, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Yneg, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.BACKWARD, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Ypos, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.STRAFE_LEFT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xneg, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.STRAFE_RIGHT, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Xpos, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.ROTATION_LEFT, MyJoystickAxesEnum.RotationXneg, () => !MyControllerHelper.IsControl(CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED));
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.ROTATION_RIGHT, MyJoystickAxesEnum.RotationXpos, () => !MyControllerHelper.IsControl(CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED));
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.ROTATION_UP, MyJoystickAxesEnum.RotationYneg, () => !MyControllerHelper.IsControl(CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED));
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.ROTATION_DOWN, MyJoystickAxesEnum.RotationYpos, () => !MyControllerHelper.IsControl(CX_BASE, MyControlsSpace.ROLL, MyControlStateType.PRESSED));
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.LOOKAROUND, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, pressed: true);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.ROLL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.CAMERA_ZOOM_IN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Yneg, pressed: true);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.CAMERA_ZOOM_OUT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Ypos, pressed: true);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.FAKE_LS, '\ue009'.ToString());
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.FAKE_RS, '\ue00a'.ToString());
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.CROUCH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.USE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, control, pressed: false);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.DAMPING, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.HEADLIGHTS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.CAMERA_MODE, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.SYSTEM_RADIAL_MENU, MyJoystickButtonsEnum.J10);
			MyControllerHelper.AddControl(CX_BASE, MyControlsSpace.CUTSCENE_SKIPPER, MyJoystickButtonsEnum.J07);
		}

		private static void CreateForGUI()
		{
			MyControllerHelper.AddContext(CX_GUI, CX_BASE);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACCEPT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.CANCEL, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACTION1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACTION2, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, pressed: false);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACCEPT_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: true);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.CANCEL_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, pressed: true);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACTION1_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, pressed: true);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.ACTION2_MOD1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04, pressed: true);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_UP, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_DOWN, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_LEFT, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.MOVE_RIGHT, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SWITCH_GUI_LEFT, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SWITCH_GUI_RIGHT, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SHIFT_LEFT, MyJoystickButtonsEnum.J05);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SHIFT_RIGHT, MyJoystickButtonsEnum.J06);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.PAGE_UP, MyJoystickAxesEnum.RotationYneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.PAGE_DOWN, MyJoystickAxesEnum.RotationYpos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.PAGE_LEFT, MyJoystickAxesEnum.RotationXneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.PAGE_RIGHT, MyJoystickAxesEnum.RotationXpos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SCROLL_UP, MyJoystickAxesEnum.Yneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SCROLL_DOWN, MyJoystickAxesEnum.Ypos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SCROLL_LEFT, MyJoystickAxesEnum.Xneg);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.SCROLL_RIGHT, MyJoystickAxesEnum.Xpos);
			MyControllerHelper.AddControl(CX_GUI, MyControlsGUI.FAKE_RS, '\ue00a'.ToString());
		}

		private static void CreateForCharacter()
		{
			MyControllerHelper.AddContext(CX_CHARACTER, CX_BASE);
			CreateCommonForCharacterAndJetpack(CX_CHARACTER);
		}

		private static void CreateCommonForJetpackAndShip(MyStringId context)
		{
			if (0 == 0)
			{
				MyControllerHelper.AddControl(context, MyControlsSpace.ROLL_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXneg);
				MyControllerHelper.AddControl(context, MyControlsSpace.ROLL_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.RotationXpos);
			}
			else
			{
				MyControllerHelper.AddControl(context, MyControlsSpace.ROLL_LEFT, MyJoystickButtonsEnum.J05);
				MyControllerHelper.AddControl(context, MyControlsSpace.ROLL_RIGHT, MyJoystickButtonsEnum.J06);
			}
		}

		private static void CreateCommonForCharacterAndJetpack(MyStringId context)
		{
			MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, pressed: false);
			MyControllerHelper.AddControl(context, MyControlsSpace.THRUSTS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, pressed: false);
			MyControllerHelper.AddControl(context, MyControlsSpace.HELMET, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(context, MyControlsSpace.LANDING_GEAR, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(context, MyControlsSpace.CONSUME_HEALTH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, pressed: true);
			MyControllerHelper.AddControl(context, MyControlsSpace.CONSUME_ENERGY, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04, pressed: true);
			MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER_DEPOSIT_ORE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, pressed: true);
			MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER_ADD_COMPONNETS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07);
			MyControllerHelper.AddControl(context, MyControlsSpace.BUILD_PLANNER_WITHDRAW_COMPONENTS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J07);
			MyControllerHelper.AddControl(context, MyControlsSpace.COLOR_TOOL, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(context, MyControlsSpace.TERMINAL, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07, pressed: true);
		}

		private static void CreateForJetpack()
		{
			MyControllerHelper.AddContext(CX_JETPACK, CX_BASE);
			CreateCommonForCharacterAndJetpack(CX_JETPACK);
			CreateCommonForJetpackAndShip(CX_JETPACK);
		}

		private static void CreateForSpaceship()
		{
			MyControllerHelper.AddContext(CX_SPACESHIP, CX_BASE);
			CreateCommonForJetpackAndShip(CX_SPACESHIP);
			MyControllerHelper.AddControl(CX_SPACESHIP, MyControlsSpace.LANDING_GEAR, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, pressed: false);
			MyControllerHelper.AddControl(CX_SPACESHIP, MyControlsSpace.TOGGLE_REACTORS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(CX_SPACESHIP, MyControlsSpace.WHEEL_JUMP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04);
			MyControllerHelper.AddControl(CX_SPACESHIP, MyControlsSpace.TERMINAL, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J07, pressed: true);
			MyControllerHelper.AddControl(CX_SPACESHIP, MyControlsSpace.FAKE_LS, "\ue005+\ue006+\ue009");
			MyControllerHelper.AddControl(CX_SPACESHIP, MyControlsSpace.FAKE_RS, "\ue005+\ue006+\ue00a");
		}

		private static void CreateForAuxiliaryBase()
		{
			MyControllerHelper.AddContext(AX_BASE);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.ADMIN_MENU, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J04);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.BLUEPRINTS_MENU, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, () => MySession.Static.CreativeMode);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.PROGRESSION_MENU, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J02, () => MySession.Static.SurvivalMode);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.BROADCASTING, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, () => MySession.Static.SurvivalMode);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.SYMMETRY_SWITCH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J03, () => MySession.Static.CreativeMode);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SWITCHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SWITCHER_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J03, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SWITCHER_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SELECT_1, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SELECT_2, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SELECT_3, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.EMOTE_SELECT_4, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: true);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.VOXEL_SELECT_SPHERE, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, () => MySession.Static.CreativeMode);
			MyControllerHelper.AddControl(AX_BASE, MyControlsSpace.TOGGLE_SIGNALS, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J01, () => MySession.Static.SurvivalMode);
		}

		private static void CreateForTools()
		{
			MyControllerHelper.AddContext(AX_TOOLS, AX_BASE);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOL_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOL_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOL_UP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOL_DOWN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J09);
			MyControllerHelper.AddControl(AX_TOOLS, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
		}

		private static void CreateForBuildMode()
		{
			MyControllerHelper.AddContext(AX_BUILD, AX_BASE);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zneg, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.FREE_ROTATION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.ROTATE_AXIS_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.ROTATE_AXIS_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.NEXT_BLOCK_STAGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.USE_SYMMETRY, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.CUBE_DEFAULT_MOUNTPOINT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.MOVE_FURTHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.MOVE_CLOSER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_CANCEL, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_REMOVE, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SYMMETRY_SETUP_ADD, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SYMMETRY_SWITCH_ALTERNATIVE, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J09);
			MyControllerHelper.AddControl(AX_BUILD, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
		}

		private static void CreateForSymmetry()
		{
			MyControllerHelper.AddContext(AX_SYMMETRY, AX_BASE);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.FREE_ROTATION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.CUBE_BUILDER_CUBESIZE_MODE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J04);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.NEXT_BLOCK_STAGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J09);
			MyControllerHelper.AddControl(AX_SYMMETRY, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
		}

		private static void CreateForVoxelHands()
		{
			MyControllerHelper.AddContext(AX_VOXEL, AX_BASE);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zpos, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_REVERT, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_PAINT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_SCALE_DOWN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_SCALE_UP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_MATERIAL_SELECT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_HAND_SETTINGS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.ROTATE_AXIS_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_FURTHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.VOXEL_CLOSER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J09);
			MyControllerHelper.AddControl(AX_VOXEL, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
		}

		private static void CreateForClipboard()
		{
			MyControllerHelper.AddContext(AX_CLIPBOARD, AX_BASE);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.FREE_ROTATION, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.COPY_PASTE_ACTION, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zneg, pressed: false);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.COPY_PASTE_CANCEL, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.ROTATE_AXIS_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.ROTATE_AXIS_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.CHANGE_ROTATION_AXIS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.SWITCH_BUILDING_MODE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.MOVE_FURTHER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.MOVE_CLOSER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J09);
			MyControllerHelper.AddControl(AX_CLIPBOARD, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
		}

		private static void CreateForColorPicker()
		{
			MyControllerHelper.AddContext(AX_COLOR_PICKER, AX_BASE);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.CYCLE_SKIN_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.CYCLE_COLOR_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.SATURATION_DECREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.VALUE_INCREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.VALUE_DECREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.SATURATION_INCREASE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.COPY_COLOR, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zpos, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.RECOLOR, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg, pressed: false);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.MEDIUM_COLOR_BRUSH, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.LARGE_COLOR_BRUSH, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J05, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.RECOLOR_WHOLE_GRID, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg, pressed: true);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.COLOR_PICKER, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.JDUp, pressed: true);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.TOOLBAR_RADIAL_MENU, MyJoystickButtonsEnum.J09);
			MyControllerHelper.AddControl(AX_COLOR_PICKER, MyControlsSpace.SLOT0, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
		}

		private static void CreateForActions()
		{
			MyControllerHelper.AddContext(AX_ACTIONS, AX_BASE);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.CUBE_COLOR_CHANGE, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.PRIMARY_TOOL_ACTION, MyJoystickAxesEnum.Zneg);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.SECONDARY_TOOL_ACTION, MyJoystickAxesEnum.Zpos);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.ACTION_UP, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDUp, pressed: false);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.ACTION_DOWN, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDDown, pressed: false);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.ACTION_LEFT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDLeft, pressed: false);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.ACTION_RIGHT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.JDRight, pressed: false);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.TOOLBAR_PREVIOUS, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J01);
			MyControllerHelper.AddControl(AX_ACTIONS, MyControlsSpace.TOOLBAR_NEXT, MyJoystickButtonsEnum.J05, MyJoystickButtonsEnum.J06, MyJoystickButtonsEnum.J02);
		}
	}
}
