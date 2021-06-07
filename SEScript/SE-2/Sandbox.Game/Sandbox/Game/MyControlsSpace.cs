using VRage.Utils;

namespace Sandbox.Game
{
	public static class MyControlsSpace
	{
		public static readonly MyStringId FORWARD = MyStringId.GetOrCompute("FORWARD");

		public static readonly MyStringId BACKWARD = MyStringId.GetOrCompute("BACKWARD");

		public static readonly MyStringId STRAFE_LEFT = MyStringId.GetOrCompute("STRAFE_LEFT");

		public static readonly MyStringId STRAFE_RIGHT = MyStringId.GetOrCompute("STRAFE_RIGHT");

		public static readonly MyStringId ROLL_LEFT = MyStringId.GetOrCompute("ROLL_LEFT");

		public static readonly MyStringId ROLL_RIGHT = MyStringId.GetOrCompute("ROLL_RIGHT");

		public static readonly MyStringId SPRINT = MyStringId.GetOrCompute("SPRINT");

		public static readonly MyStringId PRIMARY_TOOL_ACTION = MyStringId.GetOrCompute("PRIMARY_TOOL_ACTION");

		public static readonly MyStringId SECONDARY_TOOL_ACTION = MyStringId.GetOrCompute("SECONDARY_TOOL_ACTION");

		public static readonly MyStringId JUMP = MyStringId.GetOrCompute("JUMP");

		public static readonly MyStringId CROUCH = MyStringId.GetOrCompute("CROUCH");

		public static readonly MyStringId SWITCH_WALK = MyStringId.GetOrCompute("SWITCH_WALK");

		public static readonly MyStringId USE = MyStringId.GetOrCompute("USE");

		public static readonly MyStringId PICK_UP = MyStringId.GetOrCompute("PICK_UP");

		public static readonly MyStringId TERMINAL = MyStringId.GetOrCompute("TERMINAL");

		public static readonly MyStringId HELP_SCREEN = MyStringId.GetOrCompute("HELP_SCREEN");

		public static readonly MyStringId CONTROL_MENU = MyStringId.GetOrCompute("CONTROL_MENU");

		public static readonly MyStringId FACTIONS_MENU = MyStringId.GetOrCompute("FACTIONS_MENU");

		public static readonly MyStringId SYSTEM_RADIAL_MENU = MyStringId.GetOrCompute("SYSTEM_RADIAL_MENU");

		public static readonly MyStringId TOOLBAR_RADIAL_MENU = MyStringId.GetOrCompute("TOOLBAR_RADIAL_MENU");

		public static readonly MyStringId CAMERA_ZOOM_IN = MyStringId.GetOrCompute("CAMERA_ZOOM_IN");

		public static readonly MyStringId CAMERA_ZOOM_OUT = MyStringId.GetOrCompute("CAMERA_ZOOM_OUT");

		public static readonly MyStringId ACTIVE_CONTRACT_SCREEN = MyStringId.GetOrCompute("ACTIVE_CONTRACT_SCREEN");

		public static readonly MyStringId ROTATION_LEFT = MyStringId.GetOrCompute("ROTATION_LEFT");

		public static readonly MyStringId ROTATION_RIGHT = MyStringId.GetOrCompute("ROTATION_RIGHT");

		public static readonly MyStringId ROTATION_UP = MyStringId.GetOrCompute("ROTATION_UP");

		public static readonly MyStringId ROTATION_DOWN = MyStringId.GetOrCompute("ROTATION_DOWN");

		public static readonly MyStringId HEADLIGHTS = MyStringId.GetOrCompute("HEADLIGHTS");

		public static readonly MyStringId SCREENSHOT = MyStringId.GetOrCompute("SCREENSHOT");

		public static readonly MyStringId LOOKAROUND = MyStringId.GetOrCompute("LOOKAROUND");

		public static readonly MyStringId ROLL = MyStringId.GetOrCompute("ROLL");

		public static readonly MyStringId TOGGLE_SIGNALS = MyStringId.GetOrCompute("TOGGLE_SIGNALS");

		public static readonly MyStringId SWITCH_LEFT = MyStringId.GetOrCompute("SWITCH_LEFT");

		public static readonly MyStringId SWITCH_RIGHT = MyStringId.GetOrCompute("SWITCH_RIGHT");

		public static readonly MyStringId CUBE_COLOR_CHANGE = MyStringId.GetOrCompute("CUBE_COLOR_CHANGE");

		public static readonly MyStringId TOGGLE_REACTORS = MyStringId.GetOrCompute("TOGGLE_REACTORS");

		public static readonly MyStringId WHEEL_JUMP = MyStringId.GetOrCompute("THRUSTS");

		public static readonly MyStringId BUILD_PLANNER = MyStringId.GetOrCompute("BUILD_PLANNER");

		public static readonly MyStringId CONSUME_HEALTH = MyStringId.GetOrCompute("CONSUME_HEALTH");

		public static readonly MyStringId CONSUME_ENERGY = MyStringId.GetOrCompute("CONSUME_ENERGY");

		public static readonly MyStringId BUILD_PLANNER_DEPOSIT_ORE = MyStringId.GetOrCompute("BUILD_PLANNER_DEPOSIT_ORE");

		public static readonly MyStringId BUILD_PLANNER_ADD_COMPONNETS = MyStringId.GetOrCompute("BUILD_PLANNER_ADD_COMPONNETS");

		public static readonly MyStringId BUILD_PLANNER_WITHDRAW_COMPONENTS = MyStringId.GetOrCompute("BUILD_PLANNER_WITHDRAW_COMPONENTS");

		public static readonly MyStringId COLOR_TOOL = MyStringId.GetOrCompute("COLOR_TOOL");

		public static readonly MyStringId BUILD_SCREEN = MyStringId.GetOrCompute("BUILD_SCREEN");

		public static readonly MyStringId CUBE_ROTATE_VERTICAL_POSITIVE = MyStringId.GetOrCompute("CUBE_ROTATE_VERTICAL_POSITIVE");

		public static readonly MyStringId CUBE_ROTATE_VERTICAL_NEGATIVE = MyStringId.GetOrCompute("CUBE_ROTATE_VERTICAL_NEGATIVE");

		public static readonly MyStringId CUBE_ROTATE_HORISONTAL_POSITIVE = MyStringId.GetOrCompute("CUBE_ROTATE_HORISONTAL_POSITIVE");

		public static readonly MyStringId CUBE_ROTATE_HORISONTAL_NEGATIVE = MyStringId.GetOrCompute("CUBE_ROTATE_HORISONTAL_NEGATIVE");

		public static readonly MyStringId CUBE_ROTATE_ROLL_POSITIVE = MyStringId.GetOrCompute("CUBE_ROTATE_ROLL_POSITIVE");

		public static readonly MyStringId CUBE_ROTATE_ROLL_NEGATIVE = MyStringId.GetOrCompute("CUBE_ROTATE_ROLL_NEGATIVE");

		public static readonly MyStringId SYMMETRY_SWITCH = MyStringId.GetOrCompute("SYMMETRY_SWITCH");

		public static readonly MyStringId SYMMETRY_SWITCH_ALTERNATIVE = MyStringId.GetOrCompute("SYMMETRY_SWITCH_ALTERNATIVE");

		public static readonly MyStringId SYMMETRY_SETUP_CANCEL = MyStringId.GetOrCompute("SYMMETRY_SETUP_CANCEL");

		public static readonly MyStringId SYMMETRY_SETUP_ADD = MyStringId.GetOrCompute("SYMMETRY_SETUP_ADD");

		public static readonly MyStringId SYMMETRY_SETUP_REMOVE = MyStringId.GetOrCompute("SYMMETRY_SETUP_REMOVE");

		public static readonly MyStringId USE_SYMMETRY = MyStringId.GetOrCompute("USE_SYMMETRY");

		public static readonly MyStringId SWITCH_COMPOUND = MyStringId.GetOrCompute("SWITCH_COMPOUND");

		public static readonly MyStringId SWITCH_BUILDING_MODE = MyStringId.GetOrCompute("SWITCH_BUILDING_MODE");

		public static readonly MyStringId VOXEL_HAND_SETTINGS = MyStringId.GetOrCompute("VOXEL_HAND_SETTINGS");

		public static readonly MyStringId MISSION_SETTINGS = MyStringId.GetOrCompute("MISSION_SETTINGS");

		public static readonly MyStringId CUBE_BUILDER_CUBESIZE_MODE = MyStringId.GetOrCompute("CUBE_BUILDER_CUBESIZE_MODE");

		public static readonly MyStringId CUBE_DEFAULT_MOUNTPOINT = MyStringId.GetOrCompute("CUBE_DEFAULT_MOUNTPOINT");

		public static readonly MyStringId SYMMETRY_SWITCH_MODE = MyStringId.GetOrCompute("SYMMETRY_SWITCH_MODE");

		public static readonly MyStringId SLOT1 = MyStringId.GetOrCompute("SLOT1");

		public static readonly MyStringId SLOT2 = MyStringId.GetOrCompute("SLOT2");

		public static readonly MyStringId SLOT3 = MyStringId.GetOrCompute("SLOT3");

		public static readonly MyStringId SLOT4 = MyStringId.GetOrCompute("SLOT4");

		public static readonly MyStringId SLOT5 = MyStringId.GetOrCompute("SLOT5");

		public static readonly MyStringId SLOT6 = MyStringId.GetOrCompute("SLOT6");

		public static readonly MyStringId SLOT7 = MyStringId.GetOrCompute("SLOT7");

		public static readonly MyStringId SLOT8 = MyStringId.GetOrCompute("SLOT8");

		public static readonly MyStringId SLOT9 = MyStringId.GetOrCompute("SLOT9");

		public static readonly MyStringId SLOT0 = MyStringId.GetOrCompute("SLOT0");

		public static readonly MyStringId TOOLBAR_UP = MyStringId.GetOrCompute("TOOLBAR_UP");

		public static readonly MyStringId TOOLBAR_DOWN = MyStringId.GetOrCompute("TOOLBAR_DOWN");

		public static readonly MyStringId TOOLBAR_NEXT_ITEM = MyStringId.GetOrCompute("TOOLBAR_NEXT_ITEM");

		public static readonly MyStringId TOOLBAR_PREV_ITEM = MyStringId.GetOrCompute("TOOLBAR_PREV_ITEM");

		public static readonly MyStringId TOGGLE_HUD = MyStringId.GetOrCompute("TOGGLE_HUD");

		public static readonly MyStringId DAMPING = MyStringId.GetOrCompute("DAMPING");

		public static readonly MyStringId THRUSTS = MyStringId.GetOrCompute("THRUSTS");

		public static readonly MyStringId CAMERA_MODE = MyStringId.GetOrCompute("CAMERA_MODE");

		public static readonly MyStringId BROADCASTING = MyStringId.GetOrCompute("BROADCASTING");

		public static readonly MyStringId HELMET = MyStringId.GetOrCompute("HELMET");

		public static readonly MyStringId CHAT_SCREEN = MyStringId.GetOrCompute("CHAT_SCREEN");

		public static readonly MyStringId CONSOLE = MyStringId.GetOrCompute("CONSOLE");

		public static readonly MyStringId SUICIDE = MyStringId.GetOrCompute("SUICIDE");

		public static readonly MyStringId LANDING_GEAR = MyStringId.GetOrCompute("LANDING_GEAR");

		public static readonly MyStringId INVENTORY = MyStringId.GetOrCompute("INVENTORY");

		public static readonly MyStringId PAUSE_GAME = MyStringId.GetOrCompute("PAUSE_GAME");

		public static readonly MyStringId SPECTATOR_NONE = MyStringId.GetOrCompute("SPECTATOR_NONE");

		public static readonly MyStringId SPECTATOR_DELTA = MyStringId.GetOrCompute("SPECTATOR_DELTA");

		public static readonly MyStringId SPECTATOR_FREE = MyStringId.GetOrCompute("SPECTATOR_FREE");

		public static readonly MyStringId SPECTATOR_STATIC = MyStringId.GetOrCompute("SPECTATOR_STATIC");

		public static readonly MyStringId FREE_ROTATION = MyStringId.GetOrCompute("FREE_ROTATION");

		public static readonly MyStringId VOICE_CHAT = MyStringId.GetOrCompute("VOICE_CHAT");

		public static readonly MyStringId BUILD_MODE = MyStringId.GetOrCompute("BUILD_MODE");

		public static readonly MyStringId NEXT_BLOCK_STAGE = MyStringId.GetOrCompute("NEXT_BLOCK_STAGE");

		public static readonly MyStringId PREV_BLOCK_STAGE = MyStringId.GetOrCompute("PREV_BLOCK_STAGE");

		public static readonly MyStringId MOVE_CLOSER = MyStringId.GetOrCompute("MOVE_CLOSER");

		public static readonly MyStringId MOVE_FURTHER = MyStringId.GetOrCompute("MOVE_FURTHER");

		public static readonly MyStringId COPY_PASTE_ACTION = MyStringId.GetOrCompute("COPY_PASTE_ACTION");

		public static readonly MyStringId COPY_PASTE_CANCEL = MyStringId.GetOrCompute("COPY_PASTE_CANCEL");

		public static readonly MyStringId CHANGE_ROTATION_AXIS = MyStringId.GetOrCompute("CHANGE_ROTATION_AXIS");

		public static readonly MyStringId ROTATE_AXIS_LEFT = MyStringId.GetOrCompute("ROTATE_AXIS_LEFT");

		public static readonly MyStringId ROTATE_AXIS_RIGHT = MyStringId.GetOrCompute("ROTATE_AXIS_RIGHT");

		public static readonly MyStringId ADMIN_MENU = MyStringId.GetOrCompute("ADMIN_MENU");

		public static readonly MyStringId BLUEPRINTS_MENU = MyStringId.GetOrCompute("BLUEPRINTS_MENU");

		public static readonly MyStringId PROGRESSION_MENU = MyStringId.GetOrCompute("PROGRESSION_MENU");

		public static readonly MyStringId VOXEL_PAINT = MyStringId.GetOrCompute("VOXEL_PAINT");

		public static readonly MyStringId VOXEL_REVERT = MyStringId.GetOrCompute("VOXEL_REVERT");

		public static readonly MyStringId VOXEL_FURTHER = MyStringId.GetOrCompute("VOXEL_FURTHER");

		public static readonly MyStringId VOXEL_CLOSER = MyStringId.GetOrCompute("VOXEL_CLOSER");

		public static readonly MyStringId VOXEL_SELECT = MyStringId.GetOrCompute("VOXEL_SELECT");

		public static readonly MyStringId VOXEL_MATERIAL_SELECT = MyStringId.GetOrCompute("VOXEL_MATERIAL_SELECT");

		public static readonly MyStringId VOXEL_SCALE_UP = MyStringId.GetOrCompute("VOXEL_SCALE_UP");

		public static readonly MyStringId VOXEL_SCALE_DOWN = MyStringId.GetOrCompute("VOXEL_SCALE_DOWN");

		public static readonly MyStringId VOXEL_SELECT_SPHERE = MyStringId.GetOrCompute("VOXEL_SELECT_SPHERE");

		public static readonly MyStringId CUTSCENE_SKIPPER = MyStringId.GetOrCompute("CUTSCENE_SKIPPER");

		public static readonly MyStringId TOOL_UP = MyStringId.GetOrCompute("TOOL_UP");

		public static readonly MyStringId TOOL_DOWN = MyStringId.GetOrCompute("TOOL_DOWN");

		public static readonly MyStringId TOOL_LEFT = MyStringId.GetOrCompute("TOOL_LEFT");

		public static readonly MyStringId TOOL_RIGHT = MyStringId.GetOrCompute("TOOL_RIGHT");

		public static readonly MyStringId ACTION_UP = MyStringId.GetOrCompute("ACTION_UP");

		public static readonly MyStringId ACTION_DOWN = MyStringId.GetOrCompute("ACTION_DOWN");

		public static readonly MyStringId ACTION_LEFT = MyStringId.GetOrCompute("ACTION_LEFT");

		public static readonly MyStringId ACTION_RIGHT = MyStringId.GetOrCompute("ACTION_RIGHT");

		public static readonly MyStringId EMOTE_SWITCHER = MyStringId.GetOrCompute("EMOTE_SWITCHER");

		public static readonly MyStringId EMOTE_SWITCHER_LEFT = MyStringId.GetOrCompute("EMOTE_SWITCHER_LEFT");

		public static readonly MyStringId EMOTE_SWITCHER_RIGHT = MyStringId.GetOrCompute("EMOTE_SWITCHER_RIGHT");

		public static readonly MyStringId EMOTE_SELECT_1 = MyStringId.GetOrCompute("EMOTE_SELECT_1");

		public static readonly MyStringId EMOTE_SELECT_2 = MyStringId.GetOrCompute("EMOTE_SELECT_2");

		public static readonly MyStringId EMOTE_SELECT_3 = MyStringId.GetOrCompute("EMOTE_SELECT_3");

		public static readonly MyStringId EMOTE_SELECT_4 = MyStringId.GetOrCompute("EMOTE_SELECT_4");

		public static readonly MyStringId TOOLBAR_PREVIOUS = MyStringId.GetOrCompute("TOOLBAR_PREVIOUS");

		public static readonly MyStringId TOOLBAR_NEXT = MyStringId.GetOrCompute("TOOLBAR_NEXT");

		public static readonly MyStringId CYCLE_COLOR_LEFT = MyStringId.GetOrCompute("CYCLE_COLOR_LEFT");

		public static readonly MyStringId CYCLE_COLOR_RIGHT = MyStringId.GetOrCompute("CYCLE_COLOR_RIGHT");

		public static readonly MyStringId CYCLE_SKIN_LEFT = MyStringId.GetOrCompute("CYCLE_SKIN_LEFT");

		public static readonly MyStringId CYCLE_SKIN_RIGHT = MyStringId.GetOrCompute("CYCLE_SKIN_RIGHT");

		public static readonly MyStringId SATURATION_DECREASE = MyStringId.GetOrCompute("SATURATION_DECREASE");

		public static readonly MyStringId SATURATION_INCREASE = MyStringId.GetOrCompute("SATURATION_INCREASE");

		public static readonly MyStringId VALUE_DECREASE = MyStringId.GetOrCompute("VALUE_DECREASE");

		public static readonly MyStringId VALUE_INCREASE = MyStringId.GetOrCompute("VALUE_INCREASE");

		public static readonly MyStringId COPY_COLOR = MyStringId.GetOrCompute("COPY_COLOR");

		public static readonly MyStringId RECOLOR = MyStringId.GetOrCompute("RECOLOR");

		public static readonly MyStringId MEDIUM_COLOR_BRUSH = MyStringId.GetOrCompute("MEDIUM_COLOR_BRUSH");

		public static readonly MyStringId LARGE_COLOR_BRUSH = MyStringId.GetOrCompute("LARGE_COLOR_BRUSH");

		public static readonly MyStringId RECOLOR_WHOLE_GRID = MyStringId.GetOrCompute("RECOLOR_WHOLE_GRID");

		public static readonly MyStringId COLOR_PICKER = MyStringId.GetOrCompute("COLOR_PICKER");

		public static readonly MyStringId FAKE_MODIFIER_LB = MyStringId.GetOrCompute("FAKE_MODIFIER_LB");

		public static readonly MyStringId FAKE_MODIFIER_RB = MyStringId.GetOrCompute("FAKE_MODIFIER_RB");

		public static readonly MyStringId FAKE_LS = MyStringId.GetOrCompute("FAKE_LS");

		public static readonly MyStringId FAKE_RS = MyStringId.GetOrCompute("FAKE_RS");

		public static readonly MyStringId FAKE_DPAD = MyStringId.GetOrCompute("FAKE_DPAD");
	}
}
