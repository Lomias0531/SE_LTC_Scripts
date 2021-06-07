using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public static class MyGuiConstants
	{
		public static readonly Vector2 GUI_OPTIMAL_SIZE = new Vector2(1600f, 1200f);

		public const float DOUBLE_CLICK_DELAY = 500f;

		public const float CLICK_RELEASE_DELAY = 500f;

		public const float SCROLL_SPEED = 0.05f;

		public const float DEFAULT_TEXT_SCALE = 0.8f;

		public const float HUD_TEXT_SCALE = 0.8f;

		public const float HUD_LINE_SPACING = 0.025f;

		public static readonly Vector4 LABEL_TEXT_COLOR = Vector4.One;

		public static readonly Vector2 DEFAULT_LISTBOX_ITEM_SIZE = (new Vector2(648f, 390f) - new Vector2(228f, 348f)) / GUI_OPTIMAL_SIZE;

		public static Vector4 DISABLED_CONTROL_COLOR_MASK_MULTIPLIER = new Vector4(0.7f);

		public static Color THEMED_GUI_LINE_COLOR = new Color(77, 99, 113);

		public static Color THEMED_GUI_LINE_BORDER = new Color(0u);

		public static Color THEMED_GUI_BACKGROUND_COLOR = new Color(73, 86, 94);

		public static readonly Color GUI_NEWS_BACKGROUND_COLOR = new Color(35, 66, 85);

		public static readonly MyGuiSizedTexture TEXTURE_ICON_FAKE;

		public static readonly string TEXTURE_ICON_FILTER_URANIUM;

		public static readonly string TEXTURE_ICON_FILTER_ORE;

		public static readonly string TEXTURE_ICON_FILTER_INGOT;

		public static readonly string TEXTURE_ICON_FILTER_MISSILE;

		public static readonly string TEXTURE_ICON_FILTER_AMMO_25MM;

		public static readonly string TEXTURE_ICON_FILTER_AMMO_5_54MM;

		public static readonly string TEXTURE_ICON_FILTER_COMPONENT;

		public static readonly string TEXTURE_ICON_LARGE_BLOCK;

		public static readonly string TEXTURE_ICON_SMALL_BLOCK;

		public static readonly string TEXTURE_ICON_CLOSE;

		public static readonly string TEXTURE_TERMINAL_GROUP;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_DEFAULT_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_DEFAULT_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_RED_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_RED_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_SKINS_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_SKINS_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_CLOSE_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_CLOSE_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_CLOSE_BCG_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_CLOSE_BCG_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_INFO_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_INFO_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_KEEN_LOGO;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_STRIPE_LEFT_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_STRIPE_LEFT_NORMAL_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_WELCOMESCREEN_SIGNATURE;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_CHARACTER;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_CHARACTER_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_GRID;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_GRID_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_ALL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_ALL_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_ENERGY;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_ENERGY_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_STORAGE;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_STORAGE_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_SHIP;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_SHIP_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_SYSTEM;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_FILTER_SYSTEM_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_NULL;

		public static readonly MyGuiCompositeTexture TEXTURE_HIGHLIGHT_DARK;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_INCREASE;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_DECREASE;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_INCREASE_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_DECREASE_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_ARROW_LEFT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_ARROW_LEFT_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_ARROW_RIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_ARROW_RIGHT_HIGHLIGHT;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ARROW_SINGLE;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ARROW_DOUBLE;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_LIKE_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_LIKE_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_BUG_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_BUG_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_HELP_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_HELP_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_ENVELOPE_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_ENVELOPE_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_SQUARE_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_SQUARE_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_SWITCHONOFF_LEFT_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_SWITCHONOFF_LEFT_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_SWITCHONOFF_RIGHT_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_SWITCHONOFF_RIGHT_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_INVENTORY_TRASH_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_INVENTORY_TRASH_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_WITHDRAW_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_WITHDRAW_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_DEPOSITALL_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_DEPOSITALL_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_ADDTOPRODUCTION_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_ADDTOPRODUCTION_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_SELECTEDTOPRODUCTION_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_SELECTEDTOPRODUCTION_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_TEXTBOX;

		public static readonly MyGuiCompositeTexture TEXTURE_TEXTBOX_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLABLE_LIST_TOOLS_BLOCKS;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLABLE_LIST;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLABLE_LIST_BORDER;

		public static readonly MyGuiCompositeTexture TEXTURE_WBORDER_LIST;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLABLE_WBORDER_LIST;

		public static readonly MyGuiCompositeTexture TEXTURE_RECTANGLE_DARK;

		public static readonly MyGuiCompositeTexture TEXTURE_RECTANGLE_BUTTON_BORDER;

		public static readonly MyGuiCompositeTexture TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER;

		public static readonly MyGuiCompositeTexture TEXTURE_RECTANGLE_BUTTON;

		public static readonly MyGuiCompositeTexture TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED;

		public static readonly MyGuiCompositeTexture TEXTURE_RECTANGLE_DARK_BORDER;

		public static readonly MyGuiCompositeTexture TEXTURE_RECTANGLE_LOAD_BORDER;

		public static readonly MyGuiCompositeTexture TEXTURE_NEWS_BACKGROUND;

		public static readonly MyGuiCompositeTexture TEXTURE_NEWS_BACKGROUND_BlueLine;

		public static readonly MyGuiCompositeTexture TEXTURE_NEWS_PAGING_BACKGROUND;

		public static readonly MyGuiCompositeTexture TEXTURE_RECTANGLE_NEUTRAL;

		public static readonly MyGuiCompositeTexture TEXTURE_COMBOBOX_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_COMBOBOX_HIGHLIGHT;

		public static readonly MyGuiHighlightTexture TEXTURE_GRID_ITEM;

		public static readonly MyGuiHighlightTexture TEXTURE_GRID_ITEM_WHITE;

		public static readonly MyGuiHighlightTexture TEXTURE_GRID_ITEM_SMALL;

		public static readonly MyGuiHighlightTexture TEXTURE_GRID_ITEM_TINY;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ICON_LARGE_BLOCK;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ICON_SMALL_BLOCK;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ICON_TOOL;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ICON_COMPONENT;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ICON_DISASSEMBLY;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ICON_REPEAT;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ICON_REPEAT_INACTIVE;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ICON_SLAVE;

		public static readonly MyGuiHighlightTexture TEXTURE_BUTTON_ICON_SLAVE_INACTIVE;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_WHITE_FLAG;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_SENT_WHITE_FLAG;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_SENT_JOIN_REQUEST;

		public static readonly MyGuiPaddedTexture TEXTURE_MESSAGEBOX_BACKGROUND_ERROR;

		public static readonly MyGuiPaddedTexture TEXTURE_MESSAGEBOX_BACKGROUND_INFO;

		public static readonly MyGuiPaddedTexture TEXTURE_QUESTLOG_BACKGROUND_INFO;

		public static readonly MyGuiPaddedTexture TEXTURE_SCREEN_BACKGROUND;

		public static readonly MyGuiPaddedTexture TEXTURE_SCREEN_BACKGROUND_RED;

		public static readonly MyGuiPaddedTexture TEXTURE_SCREEN_TOOLS_BACKGROUND_BLOCKS;

		public static readonly MyGuiPaddedTexture TEXTURE_SCREEN_TOOLS_BACKGROUND_CONTROLS;

		public static readonly MyGuiPaddedTexture TEXTURE_SCREEN_TOOLS_BACKGROUND_WEAPONS;

		public static readonly MyGuiPaddedTexture TEXTURE_SCREEN_STATS_BACKGROUND;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_MODS_LOCAL;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_BLUEPRINTS_LOCAL;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_BLUEPRINTS_CLOUD;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_STAR;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_LOCK;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_EXPERIMENTAL;

		public static readonly MyGuiHighlightTexture TEXTURE_BLUEPRINTS_ARROW;

		public static readonly MyGuiHighlightTexture TEXTURE_ICON_MODS_WORKSHOP;

		public static readonly MyGuiCompositeTexture TEXTURE_CHECKBOX_DEFAULT_NORMAL_CHECKED;

		public static readonly MyGuiCompositeTexture TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED;

		public static readonly MyGuiCompositeTexture TEXTURE_CHECKBOX_DEFAULT_NORMAL_INDETERMINATE;

		public static readonly MyGuiCompositeTexture TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_CHECKED;

		public static readonly MyGuiCompositeTexture TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_UNCHECKED;

		public static readonly MyGuiCompositeTexture TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_INDETERMINATE;

		public static readonly MyGuiCompositeTexture TEXTURE_CHECKBOX_GREEN_CHECKED;

		public static readonly MyGuiCompositeTexture TEXTURE_CHECKBOX_BLANK;

		public static MyGuiHighlightTexture TEXTURE_SLIDER_THUMB_DEFAULT;

		public static MyGuiHighlightTexture TEXTURE_HUE_SLIDER_THUMB_DEFAULT;

		public static readonly MyGuiPaddedTexture TEXTURE_HUD_BG_MEDIUM_DEFAULT;

		public static readonly MyGuiPaddedTexture TEXTURE_HUD_BG_LARGE_DEFAULT;

		public static readonly MyGuiPaddedTexture TEXTURE_HUD_BG_MEDIUM_RED;

		public static readonly MyGuiPaddedTexture TEXTURE_HUD_BG_MEDIUM_RED2;

		public static readonly MyGuiPaddedTexture TEXTURE_HUD_BG_PERFORMANCE;

		public static readonly MyGuiPaddedTexture TEXTURE_VOICE_CHAT;

		public static readonly MyGuiPaddedTexture TEXTURE_DISCONNECTED_PLAYER;

		public static readonly MyGuiCompositeTexture TEXTURE_SLIDER_RAIL;

		public static readonly MyGuiCompositeTexture TEXTURE_SLIDER_RAIL_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_HUE_SLIDER_RAIL;

		public static readonly MyGuiCompositeTexture TEXTURE_HUE_SLIDER_RAIL_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLBAR_V_THUMB;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLBAR_V_THUMB_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLBAR_V_BACKGROUND;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLBAR_H_THUMB;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLBAR_H_THUMB_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_SCROLLBAR_H_BACKGROUND;

		public static readonly MyGuiCompositeTexture TEXTURE_TOOLBAR_TAB;

		public static readonly MyGuiCompositeTexture TEXTURE_TOOLBAR_TAB_HIGHLIGHT;

		public const string TEXTURE_BACKGROUND_FADE = "Textures\\Gui\\Screens\\screen_background_fade.dds";

		public const string BUTTON_LOCKED = "Textures\\GUI\\LockedButton.dds";

		public const string BLANK_TEXTURE = "Textures\\GUI\\Blank.dds";

		public static readonly MyGuiCompositeTexture TEXTURE_COMPOSITE_ROUND_ALL;

		public static readonly MyGuiCompositeTexture TEXTURE_COMPOSITE_ROUND_ALL_SMALL;

		public static readonly MyGuiCompositeTexture TEXTURE_COMPOSITE_ROUND_TOP;

		public static readonly MyGuiCompositeTexture TEXTURE_COMPOSITE_SLOPE_LEFTBOTTOM;

		public static readonly MyGuiCompositeTexture TEXTURE_COMPOSITE_SLOPE_LEFTBOTTOM_30;

		public static readonly MyGuiCompositeTexture TEXTURE_COMPOSITE_BLOCKINFO_PROGRESSBAR;

		public static MyGuiPaddedTexture TEXTURE_HUD_GRAVITY_GLOBE;

		public static MyGuiPaddedTexture TEXTURE_HUD_GRAVITY_LINE;

		public static MyGuiPaddedTexture TEXTURE_HUD_GRAVITY_HORIZON;

		public static readonly MyGuiCompositeTexture TEXTURE_GUI_BLANK;

		public static MyGuiPaddedTexture TEXTURE_HUD_STATS_BG;

		public static MyGuiPaddedTexture TEXTURE_HUD_STAT_EFFECT_ARROW_UP;

		public static MyGuiPaddedTexture TEXTURE_HUD_STAT_EFFECT_ARROW_DOWN;

		public static MyGuiPaddedTexture TEXTURE_HUD_STAT_BAR_BG;

		public static readonly MyGuiCompositeTexture TEXTURE_HUD_GRID_LARGE;

		public static readonly MyGuiCompositeTexture TEXTURE_HUD_GRID_LARGE_FIT;

		public static readonly MyGuiCompositeTexture TEXTURE_HUD_GRID_SMALL;

		public static readonly MyGuiCompositeTexture TEXTURE_HUD_GRID_SMALL_FIT;

		public const string CURSOR_ARROW = "Textures\\GUI\\MouseCursor.dds";

		public const string CURSOR_HAND = "Textures\\GUI\\MouseCursorHand.dds";

		public const string PROGRESS_BAR = "Textures\\GUI\\ProgressBar.dds";

		public const string LOADING_TEXTURE = "Textures\\GUI\\screens\\screen_loading_wheel.dds";

		public const string LOADING_TEXTURE_LOADING_SCREEN = "Textures\\GUI\\screens\\screen_loading_wheel_loading_screen.dds";

		public const float MOUSE_CURSOR_SPEED_MULTIPLIER = 1.3f;

		public const int VIDEO_OPTIONS_CONFIRMATION_TIMEOUT_IN_MILISECONDS = 60000;

		public static readonly Vector2 SHADOW_OFFSET;

		public static readonly Vector4 CONTROL_MOUSE_OVER_BACKGROUND_COLOR_MULTIPLIER;

		public static readonly Vector2 CONTROLS_DELTA;

		public static readonly Vector4 ROTATING_WHEEL_COLOR;

		public const float ROTATING_WHEEL_DEFAULT_SCALE = 0.36f;

		public static readonly int SHOW_CONTROL_TOOLTIP_DELAY;

		public static readonly float TOOLTIP_DISTANCE_FROM_BORDER;

		public static readonly Vector4 DEFAULT_CONTROL_BACKGROUND_COLOR;

		public static readonly Vector4 DEFAULT_CONTROL_NONACTIVE_COLOR;

		public static Color DISABLED_BUTTON_COLOR;

		public static Vector4 DISABLED_BUTTON_COLOR_VECTOR;

		public static Vector4 DISABLED_BUTTON_TEXT_COLOR;

		public static float LOCKBUTTON_SIZE_MODIFICATION;

		public const float APP_VERSION_TEXT_SCALE = 0.95f;

		public const float APP_VERSION_TEXT_SCALE_MainMenu = 0.6f;

		public const float APP_VERSION_TEXT_ALPHA_MainMenu = 0.6f;

		public static readonly Vector4 SCREEN_BACKGROUND_FADE_BLANK_DARK;

		public static readonly Vector4 SCREEN_BACKGROUND_FADE_BLANK_DARK_PROGRESS_SCREEN;

		public static readonly float SCREEN_CAPTION_DELTA_Y;

		public static readonly Vector4 SCREEN_BACKGROUND_COLOR;

		public const float REFERENCE_SCREEN_HEIGHT = 1080f;

		public const float SAFE_ASPECT_RATIO = 1.33333337f;

		public const float LOADING_PLEASE_WAIT_SCALE = 1.1f;

		public static readonly Vector2 LOADING_PLEASE_WAIT_POSITION;

		public static readonly Vector4 LOADING_PLEASE_WAIT_COLOR;

		public const int TEXTBOX_MOVEMENT_DELAY = 100;

		public const int TEXTBOX_CHANGE_DELAY = 500;

		public const int TEXTBOX_INITIAL_THROTTLE_DELAY = 500;

		public const int TEXTBOX_REPEAT_THROTTLE_DELAY = 50;

		public const string TEXTBOX_FALLBACK_CHARACTER = "#";

		public static readonly Vector2 TEXTBOX_TEXT_OFFSET;

		public static readonly Vector2 TEXTBOX_MEDIUM_SIZE;

		public static readonly Vector4 MOUSE_CURSOR_COLOR;

		public const float MOUSE_CURSOR_SCALE = 1f;

		public const float MOUSE_ROTATION_INDICATOR_MULTIPLIER = 0.075f;

		public const float ROTATION_INDICATOR_MULTIPLIER = 0.15f;

		public static readonly Vector4 BUTTON_BACKGROUND_COLOR;

		public static readonly Vector2 MENU_BUTTONS_POSITION_DELTA;

		public static readonly Vector4 BACK_BUTTON_BACKGROUND_COLOR;

		public static readonly Vector4 BACK_BUTTON_TEXT_COLOR;

		public static readonly Vector2 BACK_BUTTON_SIZE;

		public static readonly Vector2 OK_BUTTON_SIZE;

		public static readonly Vector2 GENERIC_BUTTON_SPACING;

		public const float MAIN_MENU_BUTTON_TEXT_SCALE = 0.8f;

		public static Vector4 TREEVIEW_SELECTED_ITEM_COLOR;

		public static Vector4 TREEVIEW_DISABLED_ITEM_COLOR;

		public static readonly Vector4 TREEVIEW_TEXT_COLOR;

		public static readonly Vector4 TREEVIEW_VERTICAL_LINE_COLOR;

		public static readonly Vector2 TREEVIEW_VSCROLLBAR_SIZE;

		public static readonly Vector2 TREEVIEW_HSCROLLBAR_SIZE;

		public static readonly Vector2 COMBOBOX_MEDIUM_SIZE;

		public static readonly Vector2 COMBOBOX_MEDIUM_ELEMENT_SIZE;

		public static readonly Vector2 COMBOBOX_VSCROLLBAR_SIZE;

		public static readonly Vector2 COMBOBOX_HSCROLLBAR_SIZE;

		public static readonly Vector4 LISTBOX_BACKGROUND_COLOR;

		public static readonly Vector2 LISTBOX_ICON_SIZE;

		public static readonly Vector2 LISTBOX_ICON_OFFSET;

		public static readonly float LISTBOX_WIDTH;

		public static readonly Vector2 DRAG_AND_DROP_TEXT_OFFSET;

		public static readonly Vector4 DRAG_AND_DROP_TEXT_COLOR;

		public static readonly Vector2 DRAG_AND_DROP_SMALL_SIZE;

		public static readonly Vector4 DRAG_AND_DROP_BACKGROUND_COLOR;

		public const float DRAG_AND_DROP_ICON_SIZE_X = 0.07395f;

		public const float DRAG_AND_DROP_ICON_SIZE_Y = 0.0986f;

		public static readonly float SLIDER_INSIDE_OFFSET_X;

		public static readonly int REPEAT_PRESS_DELAY;

		public static readonly Vector2 MESSAGE_BOX_BUTTON_SIZE_SMALL;

		public static Vector2 TOOL_TIP_RELATIVE_DEFAULT_POSITION;

		public const float TOOL_TIP_TEXT_SCALE = 0.7f;

		public const int TRANSITION_OPENING_TIME = 200;

		public const int TRANSITION_CLOSING_TIME = 200;

		public const float TRANSITION_ALPHA_MIN = 0f;

		public const float TRANSITION_ALPHA_MAX = 1f;

		public static readonly Vector2I LOADING_BACKGROUND_TEXTURE_REAL_SIZE;

		public const int LOADING_THREAD_DRAW_SLEEP_IN_MILISECONDS = 10;

		public const float COLORED_TEXT_DEFAULT_TEXT_SCALE = 0.75f;

		public static readonly Color COLORED_TEXT_DEFAULT_COLOR;

		public static readonly Color COLORED_TEXT_DEFAULT_HIGHLIGHT_COLOR;

		public static readonly Vector2 MULTILINE_LABEL_BORDER;

		public static readonly float DEBUG_LABEL_TEXT_SCALE;

		public static readonly float DEBUG_BUTTON_TEXT_SCALE;

		public static readonly float DEBUG_STATISTICS_TEXT_SCALE;

		public static readonly float DEBUG_STATISTICS_ROW_DISTANCE;

		public const float FONT_SCALE = 144f / 185f;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_SQUARE_SMALL_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_SQUARE_SMALL_NORMAL;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_SQUARE_48_HIGHLIGHT;

		public static readonly MyGuiCompositeTexture TEXTURE_BUTTON_SQUARE_48_NORMAL;

		public const string CB_FREE_MODE_ICON = "Textures\\GUI\\CubeBuilder\\FreeModIcon.png";

		public const string CB_LCS_GRID_ICON = "Textures\\GUI\\CubeBuilder\\OnGridIcon.png";

		public const string CB_LARGE_GRID_MODE = "Textures\\GUI\\CubeBuilder\\GridModeLargeHighl.png";

		public const string CB_SMALL_GRID_MODE = "Textures\\GUI\\CubeBuilder\\GridModeSmallHighl.png";

		public const string BS_ANTENNA_ON = "Textures\\GUI\\Icons\\BroadcastStatus\\AntennaOn.png";

		public const string BS_ANTENNA_OFF = "Textures\\GUI\\Icons\\BroadcastStatus\\AntennaOff.png";

		public const string BS_KEY_ON = "Textures\\GUI\\Icons\\BroadcastStatus\\KeyOn.png";

		public const string BS_KEY_OFF = "Textures\\GUI\\Icons\\BroadcastStatus\\KeyOff.png";

		public const string BS_REMOTE_ON = "Textures\\GUI\\Icons\\BroadcastStatus\\RemoteOn.png";

		public const string BS_REMOTE_OFF = "Textures\\GUI\\Icons\\BroadcastStatus\\RemoteOff.png";

		static MyGuiConstants()
		{
			MyGuiSizedTexture myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Icons\\Fake.dds",
				SizePx = new Vector2(81f, 81f)
			};
			TEXTURE_ICON_FAKE = myGuiSizedTexture;
			TEXTURE_ICON_FILTER_URANIUM = "Textures\\GUI\\Icons\\filter_uranium.dds";
			TEXTURE_ICON_FILTER_ORE = "Textures\\GUI\\Icons\\filter_ore.dds";
			TEXTURE_ICON_FILTER_INGOT = "Textures\\GUI\\Icons\\filter_ingot.dds";
			TEXTURE_ICON_FILTER_MISSILE = "Textures\\GUI\\Icons\\FilterMissile.dds";
			TEXTURE_ICON_FILTER_AMMO_25MM = "Textures\\GUI\\Icons\\FilterAmmo25mm.dds";
			TEXTURE_ICON_FILTER_AMMO_5_54MM = "Textures\\GUI\\Icons\\FilterAmmo5.54mm.dds";
			TEXTURE_ICON_FILTER_COMPONENT = "Textures\\GUI\\Icons\\FilterComponent.dds";
			TEXTURE_ICON_LARGE_BLOCK = "Textures\\GUI\\CubeBuilder\\GridModeLargeHighl.PNG";
			TEXTURE_ICON_SMALL_BLOCK = "Textures\\GUI\\CubeBuilder\\GridModeSmallHighl.PNG";
			TEXTURE_ICON_CLOSE = "Textures\\GUI\\Controls\\button_close_symbol.dds";
			TEXTURE_TERMINAL_GROUP = "Textures\\GUI\\Icons\\GroupIcon.dds";
			MyGuiCompositeTexture myGuiCompositeTexture = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(281f, 62f),
				Texture = "Textures\\GUI\\Controls\\button_default.dds"
			};
			myGuiCompositeTexture.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_DEFAULT_NORMAL = myGuiCompositeTexture;
			MyGuiCompositeTexture myGuiCompositeTexture2 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(281f, 62f),
				Texture = "Textures\\GUI\\Controls\\button_default_highlight.dds"
			};
			myGuiCompositeTexture2.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_DEFAULT_HIGHLIGHT = myGuiCompositeTexture2;
			MyGuiCompositeTexture myGuiCompositeTexture3 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(281f, 62f),
				Texture = "Textures\\GUI\\Controls\\button_red.dds"
			};
			myGuiCompositeTexture3.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_RED_NORMAL = myGuiCompositeTexture3;
			MyGuiCompositeTexture myGuiCompositeTexture4 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(281f, 62f),
				Texture = "Textures\\GUI\\Controls\\button_red_highlight.dds"
			};
			myGuiCompositeTexture4.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_RED_HIGHLIGHT = myGuiCompositeTexture4;
			MyGuiCompositeTexture myGuiCompositeTexture5 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(211f, 51f),
				Texture = "Textures\\GUI\\Controls\\button_skins_default.dds"
			};
			myGuiCompositeTexture5.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_SKINS_NORMAL = myGuiCompositeTexture5;
			MyGuiCompositeTexture myGuiCompositeTexture6 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(211f, 51f),
				Texture = "Textures\\GUI\\Controls\\button_skins_default_highlight.dds"
			};
			myGuiCompositeTexture6.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_SKINS_HIGHLIGHT = myGuiCompositeTexture6;
			MyGuiCompositeTexture myGuiCompositeTexture7 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(34f, 31f),
				Texture = "Textures\\GUI\\Controls\\button_close_symbol.dds"
			};
			myGuiCompositeTexture7.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_CLOSE_NORMAL = myGuiCompositeTexture7;
			MyGuiCompositeTexture myGuiCompositeTexture8 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(34f, 31f),
				Texture = "Textures\\GUI\\Controls\\button_close_symbol_highlight.dds"
			};
			myGuiCompositeTexture8.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_CLOSE_HIGHLIGHT = myGuiCompositeTexture8;
			MyGuiCompositeTexture myGuiCompositeTexture9 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(34f, 31f),
				Texture = "Textures\\GUI\\Controls\\button_close_symbol_bcg.dds"
			};
			myGuiCompositeTexture9.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_CLOSE_BCG_NORMAL = myGuiCompositeTexture9;
			MyGuiCompositeTexture myGuiCompositeTexture10 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(34f, 31f),
				Texture = "Textures\\GUI\\Controls\\button_close_symbol_bcg_highlight.dds"
			};
			myGuiCompositeTexture10.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_CLOSE_BCG_HIGHLIGHT = myGuiCompositeTexture10;
			MyGuiCompositeTexture myGuiCompositeTexture11 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(34f, 31f),
				Texture = "Textures\\GUI\\Controls\\button_info_symbol.dds"
			};
			myGuiCompositeTexture11.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_INFO_NORMAL = myGuiCompositeTexture11;
			MyGuiCompositeTexture myGuiCompositeTexture12 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(34f, 31f),
				Texture = "Textures\\GUI\\Controls\\button_info_symbol_highlight.dds"
			};
			myGuiCompositeTexture12.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_INFO_HIGHLIGHT = myGuiCompositeTexture12;
			MyGuiCompositeTexture myGuiCompositeTexture13 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(198f, 99f),
				Texture = "Textures\\Gui\\KeenLogo.dds"
			};
			myGuiCompositeTexture13.LeftTop = myGuiSizedTexture;
			TEXTURE_KEEN_LOGO = myGuiCompositeTexture13;
			MyGuiCompositeTexture myGuiCompositeTexture14 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(286f, 62f),
				Texture = "Textures\\GUI\\Controls\\button_stripe_left.dds"
			};
			myGuiCompositeTexture14.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_STRIPE_LEFT_NORMAL = myGuiCompositeTexture14;
			MyGuiCompositeTexture myGuiCompositeTexture15 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(286f, 62f),
				Texture = "Textures\\GUI\\Controls\\button_stripe_left_highlight.dds"
			};
			myGuiCompositeTexture15.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_STRIPE_LEFT_NORMAL_HIGHLIGHT = myGuiCompositeTexture15;
			MyGuiCompositeTexture myGuiCompositeTexture16 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(255f, 52f),
				Texture = "Textures\\Gui\\Signature.dds"
			};
			myGuiCompositeTexture16.LeftTop = myGuiSizedTexture;
			TEXTURE_WELCOMESCREEN_SIGNATURE = myGuiCompositeTexture16;
			MyGuiCompositeTexture myGuiCompositeTexture17 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_character.dds"
			};
			myGuiCompositeTexture17.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_CHARACTER = myGuiCompositeTexture17;
			MyGuiCompositeTexture myGuiCompositeTexture18 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_character_highlight.dds"
			};
			myGuiCompositeTexture18.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_CHARACTER_HIGHLIGHT = myGuiCompositeTexture18;
			MyGuiCompositeTexture myGuiCompositeTexture19 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_grid.dds"
			};
			myGuiCompositeTexture19.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_GRID = myGuiCompositeTexture19;
			MyGuiCompositeTexture myGuiCompositeTexture20 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_grid_highlight.dds"
			};
			myGuiCompositeTexture20.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_GRID_HIGHLIGHT = myGuiCompositeTexture20;
			MyGuiCompositeTexture myGuiCompositeTexture21 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_all.dds"
			};
			myGuiCompositeTexture21.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_ALL = myGuiCompositeTexture21;
			MyGuiCompositeTexture myGuiCompositeTexture22 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_all_highlight.dds"
			};
			myGuiCompositeTexture22.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_ALL_HIGHLIGHT = myGuiCompositeTexture22;
			MyGuiCompositeTexture myGuiCompositeTexture23 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_energy.dds"
			};
			myGuiCompositeTexture23.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_ENERGY = myGuiCompositeTexture23;
			MyGuiCompositeTexture myGuiCompositeTexture24 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_energy_highlight.dds"
			};
			myGuiCompositeTexture24.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_ENERGY_HIGHLIGHT = myGuiCompositeTexture24;
			MyGuiCompositeTexture myGuiCompositeTexture25 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_storage.dds"
			};
			myGuiCompositeTexture25.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_STORAGE = myGuiCompositeTexture25;
			MyGuiCompositeTexture myGuiCompositeTexture26 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_storage_highlight.dds"
			};
			myGuiCompositeTexture26.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_STORAGE_HIGHLIGHT = myGuiCompositeTexture26;
			MyGuiCompositeTexture myGuiCompositeTexture27 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_ship.png"
			};
			myGuiCompositeTexture27.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_SHIP = myGuiCompositeTexture27;
			MyGuiCompositeTexture myGuiCompositeTexture28 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_ship_highlight.png"
			};
			myGuiCompositeTexture28.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_SHIP_HIGHLIGHT = myGuiCompositeTexture28;
			MyGuiCompositeTexture myGuiCompositeTexture29 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_system.dds"
			};
			myGuiCompositeTexture29.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_SYSTEM = myGuiCompositeTexture29;
			MyGuiCompositeTexture myGuiCompositeTexture30 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(69f, 69f),
				Texture = "Textures\\GUI\\Controls\\button_filter_system_highlight.dds"
			};
			myGuiCompositeTexture30.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_FILTER_SYSTEM_HIGHLIGHT = myGuiCompositeTexture30;
			TEXTURE_NULL = new MyGuiCompositeTexture();
			MyGuiCompositeTexture myGuiCompositeTexture31 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = Vector2.Zero,
				Texture = "Textures\\GUI\\Controls\\item_highlight_dark.dds"
			};
			myGuiCompositeTexture31.Center = myGuiSizedTexture;
			TEXTURE_HIGHLIGHT_DARK = myGuiCompositeTexture31;
			MyGuiCompositeTexture myGuiCompositeTexture32 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(60f, 60f),
				Texture = "Textures\\GUI\\Controls\\button_increase.dds"
			};
			myGuiCompositeTexture32.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_INCREASE = myGuiCompositeTexture32;
			MyGuiCompositeTexture myGuiCompositeTexture33 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(60f, 60f),
				Texture = "Textures\\GUI\\Controls\\button_decrease.dds"
			};
			myGuiCompositeTexture33.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_DECREASE = myGuiCompositeTexture33;
			MyGuiCompositeTexture myGuiCompositeTexture34 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(60f, 60f),
				Texture = "Textures\\GUI\\Controls\\button_increase_highlight.dds"
			};
			myGuiCompositeTexture34.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_INCREASE_HIGHLIGHT = myGuiCompositeTexture34;
			MyGuiCompositeTexture myGuiCompositeTexture35 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(60f, 60f),
				Texture = "Textures\\GUI\\Controls\\button_decrease_highlight.dds"
			};
			myGuiCompositeTexture35.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_DECREASE_HIGHLIGHT = myGuiCompositeTexture35;
			MyGuiCompositeTexture myGuiCompositeTexture36 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(30f, 29f),
				Texture = "Textures\\GUI\\Controls\\button_arrow_left.dds"
			};
			myGuiCompositeTexture36.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_ARROW_LEFT = myGuiCompositeTexture36;
			MyGuiCompositeTexture myGuiCompositeTexture37 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(30f, 29f),
				Texture = "Textures\\GUI\\Controls\\button_arrow_left_highlight.dds"
			};
			myGuiCompositeTexture37.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_ARROW_LEFT_HIGHLIGHT = myGuiCompositeTexture37;
			MyGuiCompositeTexture myGuiCompositeTexture38 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(30f, 29f),
				Texture = "Textures\\GUI\\Controls\\button_arrow_right.dds"
			};
			myGuiCompositeTexture38.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_ARROW_RIGHT = myGuiCompositeTexture38;
			MyGuiCompositeTexture myGuiCompositeTexture39 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(30f, 29f),
				Texture = "Textures\\GUI\\Controls\\button_arrow_right_highlight.dds"
			};
			myGuiCompositeTexture39.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_ARROW_RIGHT_HIGHLIGHT = myGuiCompositeTexture39;
			TEXTURE_BUTTON_ARROW_SINGLE = new MyGuiHighlightTexture
			{
				SizePx = new Vector2(64f, 64f),
				Normal = "Textures\\GUI\\Icons\\buttons\\ArrowSingle.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\ArrowSingleHighlight.dds"
			};
			TEXTURE_BUTTON_ARROW_DOUBLE = new MyGuiHighlightTexture
			{
				SizePx = new Vector2(64f, 64f),
				Normal = "Textures\\GUI\\Icons\\buttons\\ArrowDouble.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\ArrowDoubleHighlight.dds"
			};
			MyGuiCompositeTexture myGuiCompositeTexture40 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 64f),
				Texture = "Textures\\GUI\\Icons\\Like.dds"
			};
			myGuiCompositeTexture40.Center = myGuiSizedTexture;
			TEXTURE_BUTTON_LIKE_NORMAL = myGuiCompositeTexture40;
			MyGuiCompositeTexture myGuiCompositeTexture41 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(128f, 128f),
				Texture = "Textures\\GUI\\Icons\\LikeHighlight.dds"
			};
			myGuiCompositeTexture41.Center = myGuiSizedTexture;
			TEXTURE_BUTTON_LIKE_HIGHLIGHT = myGuiCompositeTexture41;
			MyGuiCompositeTexture myGuiCompositeTexture42 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(16f, 64f),
				Texture = "Textures\\GUI\\Icons\\Bug.dds"
			};
			myGuiCompositeTexture42.Center = myGuiSizedTexture;
			TEXTURE_BUTTON_BUG_NORMAL = myGuiCompositeTexture42;
			MyGuiCompositeTexture myGuiCompositeTexture43 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(128f, 128f),
				Texture = "Textures\\GUI\\Icons\\BugHighlight.dds"
			};
			myGuiCompositeTexture43.Center = myGuiSizedTexture;
			TEXTURE_BUTTON_BUG_HIGHLIGHT = myGuiCompositeTexture43;
			MyGuiCompositeTexture myGuiCompositeTexture44 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 64f),
				Texture = "Textures\\GUI\\Icons\\Help.dds"
			};
			myGuiCompositeTexture44.Center = myGuiSizedTexture;
			TEXTURE_BUTTON_HELP_NORMAL = myGuiCompositeTexture44;
			MyGuiCompositeTexture myGuiCompositeTexture45 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 64f),
				Texture = "Textures\\GUI\\Icons\\HelpHighlight.dds"
			};
			myGuiCompositeTexture45.Center = myGuiSizedTexture;
			TEXTURE_BUTTON_HELP_HIGHLIGHT = myGuiCompositeTexture45;
			MyGuiCompositeTexture myGuiCompositeTexture46 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 64f),
				Texture = "Textures\\GUI\\Icons\\Envelope.dds"
			};
			myGuiCompositeTexture46.Center = myGuiSizedTexture;
			TEXTURE_BUTTON_ENVELOPE_NORMAL = myGuiCompositeTexture46;
			MyGuiCompositeTexture myGuiCompositeTexture47 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 64f),
				Texture = "Textures\\GUI\\Icons\\EnvelopeHighlight.dds"
			};
			myGuiCompositeTexture47.Center = myGuiSizedTexture;
			TEXTURE_BUTTON_ENVELOPE_HIGHLIGHT = myGuiCompositeTexture47;
			MyGuiCompositeTexture myGuiCompositeTexture48 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 64f),
				Texture = "Textures\\GUI\\Icons\\buttons\\SquareButtonHighlight.dds"
			};
			myGuiCompositeTexture48.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_SQUARE_HIGHLIGHT = myGuiCompositeTexture48;
			MyGuiCompositeTexture myGuiCompositeTexture49 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 64f),
				Texture = "Textures\\GUI\\Icons\\buttons\\SquareButton.dds"
			};
			myGuiCompositeTexture49.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_SQUARE_NORMAL = myGuiCompositeTexture49;
			MyGuiCompositeTexture myGuiCompositeTexture50 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(90f, 65f),
				Texture = "Textures\\GUI\\Controls\\switch_on_off_left_highlight.dds"
			};
			myGuiCompositeTexture50.LeftTop = myGuiSizedTexture;
			TEXTURE_SWITCHONOFF_LEFT_HIGHLIGHT = myGuiCompositeTexture50;
			MyGuiCompositeTexture myGuiCompositeTexture51 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(90f, 65f),
				Texture = "Textures\\GUI\\Controls\\switch_on_off_left.dds"
			};
			myGuiCompositeTexture51.LeftTop = myGuiSizedTexture;
			TEXTURE_SWITCHONOFF_LEFT_NORMAL = myGuiCompositeTexture51;
			MyGuiCompositeTexture myGuiCompositeTexture52 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(90f, 65f),
				Texture = "Textures\\GUI\\Controls\\switch_on_off_right_highlight.dds"
			};
			myGuiCompositeTexture52.LeftTop = myGuiSizedTexture;
			TEXTURE_SWITCHONOFF_RIGHT_HIGHLIGHT = myGuiCompositeTexture52;
			MyGuiCompositeTexture myGuiCompositeTexture53 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(90f, 65f),
				Texture = "Textures\\GUI\\Controls\\switch_on_off_right.dds"
			};
			myGuiCompositeTexture53.LeftTop = myGuiSizedTexture;
			TEXTURE_SWITCHONOFF_RIGHT_NORMAL = myGuiCompositeTexture53;
			MyGuiCompositeTexture myGuiCompositeTexture54 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\DropItem.DDS"
			};
			myGuiCompositeTexture54.LeftTop = myGuiSizedTexture;
			TEXTURE_INVENTORY_TRASH_NORMAL = myGuiCompositeTexture54;
			MyGuiCompositeTexture myGuiCompositeTexture55 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\DropItem_Highlight.DDS"
			};
			myGuiCompositeTexture55.LeftTop = myGuiSizedTexture;
			TEXTURE_INVENTORY_TRASH_HIGHLIGHT = myGuiCompositeTexture55;
			MyGuiCompositeTexture myGuiCompositeTexture56 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\WithdrawComponents.DDS"
			};
			myGuiCompositeTexture56.LeftTop = myGuiSizedTexture;
			TEXTURE_WITHDRAW_NORMAL = myGuiCompositeTexture56;
			MyGuiCompositeTexture myGuiCompositeTexture57 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\WithdrawComponents_Highlight.DDS"
			};
			myGuiCompositeTexture57.LeftTop = myGuiSizedTexture;
			TEXTURE_WITHDRAW_HIGHLIGHT = myGuiCompositeTexture57;
			MyGuiCompositeTexture myGuiCompositeTexture58 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\DepositAll.DDS"
			};
			myGuiCompositeTexture58.LeftTop = myGuiSizedTexture;
			TEXTURE_DEPOSITALL_NORMAL = myGuiCompositeTexture58;
			MyGuiCompositeTexture myGuiCompositeTexture59 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\DepositAll_Highlight.DDS"
			};
			myGuiCompositeTexture59.LeftTop = myGuiSizedTexture;
			TEXTURE_DEPOSITALL_HIGHLIGHT = myGuiCompositeTexture59;
			MyGuiCompositeTexture myGuiCompositeTexture60 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\AddComponents.DDS"
			};
			myGuiCompositeTexture60.LeftTop = myGuiSizedTexture;
			TEXTURE_ADDTOPRODUCTION_NORMAL = myGuiCompositeTexture60;
			MyGuiCompositeTexture myGuiCompositeTexture61 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\AddComponents_Highlight.DDS"
			};
			myGuiCompositeTexture61.LeftTop = myGuiSizedTexture;
			TEXTURE_ADDTOPRODUCTION_HIGHLIGHT = myGuiCompositeTexture61;
			MyGuiCompositeTexture myGuiCompositeTexture62 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\AddSelected.DDS"
			};
			myGuiCompositeTexture62.LeftTop = myGuiSizedTexture;
			TEXTURE_SELECTEDTOPRODUCTION_NORMAL = myGuiCompositeTexture62;
			MyGuiCompositeTexture myGuiCompositeTexture63 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(67f, 145f) * 0.75f,
				Texture = "Textures\\GUI\\Controls\\AddSelected_Highlight.DDS"
			};
			myGuiCompositeTexture63.LeftTop = myGuiSizedTexture;
			TEXTURE_SELECTEDTOPRODUCTION_HIGHLIGHT = myGuiCompositeTexture63;
			MyGuiCompositeTexture myGuiCompositeTexture64 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\textbox_left.dds",
				SizePx = new Vector2(8f, 48f)
			};
			myGuiCompositeTexture64.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\textbox_center.dds",
				SizePx = new Vector2(4f, 48f)
			};
			myGuiCompositeTexture64.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\textbox_right.dds",
				SizePx = new Vector2(8f, 48f)
			};
			myGuiCompositeTexture64.RightTop = myGuiSizedTexture;
			TEXTURE_TEXTBOX = myGuiCompositeTexture64;
			MyGuiCompositeTexture myGuiCompositeTexture65 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\textbox_left_highlight.dds",
				SizePx = new Vector2(8f, 48f)
			};
			myGuiCompositeTexture65.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\textbox_center_highlight.dds",
				SizePx = new Vector2(4f, 48f)
			};
			myGuiCompositeTexture65.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\textbox_right_highlight.dds",
				SizePx = new Vector2(8f, 48f)
			};
			myGuiCompositeTexture65.RightTop = myGuiSizedTexture;
			TEXTURE_TEXTBOX_HIGHLIGHT = myGuiCompositeTexture65;
			MyGuiCompositeTexture myGuiCompositeTexture66 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(512f, 1030f),
				Texture = "Textures\\GUI\\Screens\\TabGScreen.dds"
			};
			myGuiCompositeTexture66.Center = myGuiSizedTexture;
			TEXTURE_SCROLLABLE_LIST_TOOLS_BLOCKS = myGuiCompositeTexture66;
			MyGuiCompositeTexture myGuiCompositeTexture67 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_left_top.dds"
			};
			myGuiCompositeTexture67.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_left_center.dds"
			};
			myGuiCompositeTexture67.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_left_bottom.dds"
			};
			myGuiCompositeTexture67.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_center_top.dds"
			};
			myGuiCompositeTexture67.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_center.dds"
			};
			myGuiCompositeTexture67.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_center_bottom.dds"
			};
			myGuiCompositeTexture67.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_right_top.dds"
			};
			myGuiCompositeTexture67.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_right_center.dds"
			};
			myGuiCompositeTexture67.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_right_bottom.dds"
			};
			myGuiCompositeTexture67.RightBottom = myGuiSizedTexture;
			TEXTURE_SCROLLABLE_LIST = myGuiCompositeTexture67;
			MyGuiCompositeTexture myGuiCompositeTexture68 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_border_left_top.dds"
			};
			myGuiCompositeTexture68.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_border_left_center.dds"
			};
			myGuiCompositeTexture68.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_border_left_bottom.dds"
			};
			myGuiCompositeTexture68.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_border_center_top.dds"
			};
			myGuiCompositeTexture68.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_center.dds"
			};
			myGuiCompositeTexture68.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_border_center_bottom.dds"
			};
			myGuiCompositeTexture68.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_border_right_top.dds"
			};
			myGuiCompositeTexture68.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_border_right_center.dds"
			};
			myGuiCompositeTexture68.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_border_right_bottom.dds"
			};
			myGuiCompositeTexture68.RightBottom = myGuiSizedTexture;
			TEXTURE_SCROLLABLE_LIST_BORDER = myGuiCompositeTexture68;
			MyGuiCompositeTexture myGuiCompositeTexture69 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_left_top.dds"
			};
			myGuiCompositeTexture69.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_left_center.dds"
			};
			myGuiCompositeTexture69.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_left_bottom.dds"
			};
			myGuiCompositeTexture69.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_center_top.dds"
			};
			myGuiCompositeTexture69.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_center.dds"
			};
			myGuiCompositeTexture69.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_center_bottom.dds"
			};
			myGuiCompositeTexture69.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_right_top.dds"
			};
			myGuiCompositeTexture69.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\list_WBorder_right_center.png"
			};
			myGuiCompositeTexture69.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_right_bottom.dds"
			};
			myGuiCompositeTexture69.RightBottom = myGuiSizedTexture;
			TEXTURE_WBORDER_LIST = myGuiCompositeTexture69;
			MyGuiCompositeTexture myGuiCompositeTexture70 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_left_top.dds"
			};
			myGuiCompositeTexture70.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_left_center.dds"
			};
			myGuiCompositeTexture70.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_left_bottom.dds"
			};
			myGuiCompositeTexture70.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_center_top.dds"
			};
			myGuiCompositeTexture70.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_center.dds"
			};
			myGuiCompositeTexture70.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_center_bottom.dds"
			};
			myGuiCompositeTexture70.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_right_top.dds"
			};
			myGuiCompositeTexture70.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_right_center.dds"
			};
			myGuiCompositeTexture70.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(50f, 4f),
				Texture = "Textures\\GUI\\Controls\\scrollable_list_WBorder_right_bottom.dds"
			};
			myGuiCompositeTexture70.RightBottom = myGuiSizedTexture;
			TEXTURE_SCROLLABLE_WBORDER_LIST = myGuiCompositeTexture70;
			MyGuiCompositeTexture myGuiCompositeTexture71 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_left_top.dds"
			};
			myGuiCompositeTexture71.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_left_center.dds"
			};
			myGuiCompositeTexture71.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_left_bottom.dds"
			};
			myGuiCompositeTexture71.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_center_top.dds"
			};
			myGuiCompositeTexture71.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_center.dds"
			};
			myGuiCompositeTexture71.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_center_bottom.dds"
			};
			myGuiCompositeTexture71.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_right_top.dds"
			};
			myGuiCompositeTexture71.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_right_center.dds"
			};
			myGuiCompositeTexture71.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_right_bottom.dds"
			};
			myGuiCompositeTexture71.RightBottom = myGuiSizedTexture;
			TEXTURE_RECTANGLE_DARK = myGuiCompositeTexture71;
			MyGuiCompositeTexture myGuiCompositeTexture72 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_left_top.dds"
			};
			myGuiCompositeTexture72.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_left_center.dds"
			};
			myGuiCompositeTexture72.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_left_bottom.dds"
			};
			myGuiCompositeTexture72.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_center_top.dds"
			};
			myGuiCompositeTexture72.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_center.dds"
			};
			myGuiCompositeTexture72.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_center_bottom.dds"
			};
			myGuiCompositeTexture72.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_right_top.dds"
			};
			myGuiCompositeTexture72.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_right_center.dds"
			};
			myGuiCompositeTexture72.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_right_bottom.dds"
			};
			myGuiCompositeTexture72.RightBottom = myGuiSizedTexture;
			TEXTURE_RECTANGLE_BUTTON_BORDER = myGuiCompositeTexture72;
			MyGuiCompositeTexture myGuiCompositeTexture73 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_highlighted_left_top.dds"
			};
			myGuiCompositeTexture73.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_highlighted_left_center.dds"
			};
			myGuiCompositeTexture73.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_highlighted_left_bottom.dds"
			};
			myGuiCompositeTexture73.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_highlighted_center_top.dds"
			};
			myGuiCompositeTexture73.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_highlighted_center.dds"
			};
			myGuiCompositeTexture73.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_highlighted_center_bottom.dds"
			};
			myGuiCompositeTexture73.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_highlighted_right_top.dds"
			};
			myGuiCompositeTexture73.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_highlighted_right_center.dds"
			};
			myGuiCompositeTexture73.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_button_highlighted_right_bottom.dds"
			};
			myGuiCompositeTexture73.RightBottom = myGuiSizedTexture;
			TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED_BORDER = myGuiCompositeTexture73;
			TEXTURE_RECTANGLE_BUTTON = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\DarkBlueBackground.png");
			TEXTURE_RECTANGLE_BUTTON_HIGHLIGHTED = new MyGuiCompositeTexture("Textures\\GUI\\Controls\\DarkBlueBackgroundHighlighted.png");
			MyGuiCompositeTexture myGuiCompositeTexture74 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_border_left_top.dds"
			};
			myGuiCompositeTexture74.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_border_left_center.dds"
			};
			myGuiCompositeTexture74.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_border_left_bottom.dds"
			};
			myGuiCompositeTexture74.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_border_center_top.dds"
			};
			myGuiCompositeTexture74.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_center.dds"
			};
			myGuiCompositeTexture74.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_border_center_bottom.dds"
			};
			myGuiCompositeTexture74.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_border_right_top.dds"
			};
			myGuiCompositeTexture74.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_border_right_center.dds"
			};
			myGuiCompositeTexture74.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_dark_border_right_bottom.dds"
			};
			myGuiCompositeTexture74.RightBottom = myGuiSizedTexture;
			TEXTURE_RECTANGLE_DARK_BORDER = myGuiCompositeTexture74;
			MyGuiCompositeTexture myGuiCompositeTexture75 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_load_border_left_top.dds"
			};
			myGuiCompositeTexture75.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_load_border_left_center.dds"
			};
			myGuiCompositeTexture75.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_load_border_left_bottom.dds"
			};
			myGuiCompositeTexture75.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_load_border_center_top.dds"
			};
			myGuiCompositeTexture75.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_load_border_center.dds"
			};
			myGuiCompositeTexture75.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_load_border_center_bottom.dds"
			};
			myGuiCompositeTexture75.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_load_border_right_top.dds"
			};
			myGuiCompositeTexture75.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_load_border_right_center.dds"
			};
			myGuiCompositeTexture75.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_load_border_right_bottom.dds"
			};
			myGuiCompositeTexture75.RightBottom = myGuiSizedTexture;
			TEXTURE_RECTANGLE_LOAD_BORDER = myGuiCompositeTexture75;
			MyGuiCompositeTexture myGuiCompositeTexture76 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(16f, 16f),
				Texture = "Textures\\GUI\\Controls\\news_background_left_top.dds"
			};
			myGuiCompositeTexture76.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(16f, 24f),
				Texture = "Textures\\GUI\\Controls\\news_background_left_center.dds"
			};
			myGuiCompositeTexture76.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(16f, 16f),
				Texture = "Textures\\GUI\\Controls\\news_background_left_bottom.dds"
			};
			myGuiCompositeTexture76.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(15f, 16f),
				Texture = "Textures\\GUI\\Controls\\news_background_center_top.dds"
			};
			myGuiCompositeTexture76.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(15f, 24f),
				Texture = "Textures\\GUI\\Controls\\news_background_center.dds"
			};
			myGuiCompositeTexture76.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(15f, 16f),
				Texture = "Textures\\GUI\\Controls\\news_background_center_bottom.dds"
			};
			myGuiCompositeTexture76.CenterBottom = myGuiSizedTexture;
			TEXTURE_NEWS_BACKGROUND = myGuiCompositeTexture76;
			MyGuiCompositeTexture myGuiCompositeTexture77 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(7f, 14f),
				Texture = "Textures\\GUI\\Controls\\news_background_right_top.dds"
			};
			myGuiCompositeTexture77.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(7f, 24f),
				Texture = "Textures\\GUI\\Controls\\news_background_right_center.dds"
			};
			myGuiCompositeTexture77.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(7f, 16f),
				Texture = "Textures\\GUI\\Controls\\news_background_right_bottom.dds"
			};
			myGuiCompositeTexture77.RightBottom = myGuiSizedTexture;
			TEXTURE_NEWS_BACKGROUND_BlueLine = myGuiCompositeTexture77;
			MyGuiCompositeTexture myGuiCompositeTexture78 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(14f, 14f),
				Texture = "Textures\\GUI\\Controls\\news_background_left_top.dds"
			};
			myGuiCompositeTexture78.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(14f, 24f),
				Texture = "Textures\\GUI\\Controls\\news_background_left_center.dds"
			};
			myGuiCompositeTexture78.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(15f, 14f),
				Texture = "Textures\\GUI\\Controls\\news_background_center_top.dds"
			};
			myGuiCompositeTexture78.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(15f, 24f),
				Texture = "Textures\\GUI\\Controls\\news_background_center.dds"
			};
			myGuiCompositeTexture78.Center = myGuiSizedTexture;
			TEXTURE_NEWS_PAGING_BACKGROUND = myGuiCompositeTexture78;
			MyGuiCompositeTexture myGuiCompositeTexture79 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_neutral_left_top.dds"
			};
			myGuiCompositeTexture79.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_neutral_left_center.dds"
			};
			myGuiCompositeTexture79.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_neutral_left_bottom.dds"
			};
			myGuiCompositeTexture79.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_neutral_center_top.dds"
			};
			myGuiCompositeTexture79.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_neutral_center.dds"
			};
			myGuiCompositeTexture79.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_neutral_center_bottom.dds"
			};
			myGuiCompositeTexture79.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_neutral_right_top.dds"
			};
			myGuiCompositeTexture79.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_neutral_right_center.dds"
			};
			myGuiCompositeTexture79.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 4f),
				Texture = "Textures\\GUI\\Controls\\rectangle_neutral_right_bottom.dds"
			};
			myGuiCompositeTexture79.RightBottom = myGuiSizedTexture;
			TEXTURE_RECTANGLE_NEUTRAL = myGuiCompositeTexture79;
			MyGuiCompositeTexture myGuiCompositeTexture80 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(20f, 48f),
				Texture = "Textures\\GUI\\Controls\\combobox_default_left.dds"
			};
			myGuiCompositeTexture80.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 48f),
				Texture = "Textures\\GUI\\Controls\\combobox_default_center.dds"
			};
			myGuiCompositeTexture80.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(51f, 48f),
				Texture = "Textures\\GUI\\Controls\\combobox_default_right.dds"
			};
			myGuiCompositeTexture80.RightTop = myGuiSizedTexture;
			TEXTURE_COMBOBOX_NORMAL = myGuiCompositeTexture80;
			MyGuiCompositeTexture myGuiCompositeTexture81 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(20f, 48f),
				Texture = "Textures\\GUI\\Controls\\combobox_default_highlight_left.dds"
			};
			myGuiCompositeTexture81.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(4f, 48f),
				Texture = "Textures\\GUI\\Controls\\combobox_default_highlight_center.dds"
			};
			myGuiCompositeTexture81.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(51f, 48f),
				Texture = "Textures\\GUI\\Controls\\combobox_default_highlight_right.dds"
			};
			myGuiCompositeTexture81.RightTop = myGuiSizedTexture;
			TEXTURE_COMBOBOX_HIGHLIGHT = myGuiCompositeTexture81;
			TEXTURE_GRID_ITEM = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Controls\\grid_item.dds",
				Highlight = "Textures\\GUI\\Controls\\grid_item_highlight.dds",
				SizePx = new Vector2(82f, 82f)
			};
			TEXTURE_GRID_ITEM_WHITE = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Controls\\grid_item.dds",
				Highlight = "Textures\\GUI\\Controls\\grid_item_highlight_white.dds",
				SizePx = new Vector2(82f, 82f)
			};
			TEXTURE_GRID_ITEM_SMALL = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Controls\\grid_item.dds",
				Highlight = "Textures\\GUI\\Controls\\grid_item_highlight.dds",
				SizePx = new Vector2(78f, 78f)
			};
			TEXTURE_GRID_ITEM_TINY = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Controls\\grid_item.dds",
				Highlight = "Textures\\GUI\\Controls\\grid_item_highlight.dds",
				SizePx = new Vector2(50f, 50f)
			};
			TEXTURE_BUTTON_ICON_LARGE_BLOCK = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\large_block.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\large_block_highlight.dds",
				SizePx = new Vector2(41f, 41f)
			};
			TEXTURE_BUTTON_ICON_SMALL_BLOCK = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\small_block.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\small_block_highlight.dds",
				SizePx = new Vector2(43f, 43f)
			};
			TEXTURE_BUTTON_ICON_TOOL = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\tool.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\tool_highlight.dds",
				SizePx = new Vector2(41f, 41f)
			};
			TEXTURE_BUTTON_ICON_COMPONENT = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\component.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\component_highlight.dds",
				SizePx = new Vector2(37f, 45f)
			};
			TEXTURE_BUTTON_ICON_DISASSEMBLY = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\disassembly.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\disassembly_highlight.dds",
				SizePx = new Vector2(32f, 32f)
			};
			TEXTURE_BUTTON_ICON_REPEAT = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\repeat.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\repeat_highlight.dds",
				SizePx = new Vector2(54f, 34f)
			};
			TEXTURE_BUTTON_ICON_REPEAT_INACTIVE = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\repeat_Inactive.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\repeat_Inactive_Highlight.dds",
				SizePx = new Vector2(54f, 34f)
			};
			TEXTURE_BUTTON_ICON_SLAVE = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\coopmode.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\coopmode_highlight.dds",
				SizePx = new Vector2(54f, 34f)
			};
			TEXTURE_BUTTON_ICON_SLAVE_INACTIVE = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\coopmode_Inactive.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\coopmode_Inactive_HighLight.dds",
				SizePx = new Vector2(54f, 34f)
			};
			TEXTURE_ICON_WHITE_FLAG = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\WhiteFlag.dds",
				Highlight = "Textures\\GUI\\WhiteFlag.dds",
				SizePx = new Vector2(53f, 40f)
			};
			TEXTURE_ICON_SENT_WHITE_FLAG = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\RequestSent.dds",
				Highlight = "Textures\\GUI\\RequestSent.dds",
				SizePx = new Vector2(53f, 40f)
			};
			TEXTURE_ICON_SENT_JOIN_REQUEST = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\PlayerRequest.dds",
				Highlight = "Textures\\GUI\\PlayerRequest.dds",
				SizePx = new Vector2(53f, 40f)
			};
			MyGuiPaddedTexture myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\message_background_red.dds",
				SizePx = new Vector2(1343f, 321f),
				PaddingSizePx = new Vector2(20f, 25f)
			};
			TEXTURE_MESSAGEBOX_BACKGROUND_ERROR = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\message_background_blue.dds",
				SizePx = new Vector2(1343f, 321f),
				PaddingSizePx = new Vector2(20f, 25f)
			};
			TEXTURE_MESSAGEBOX_BACKGROUND_INFO = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\message_background_questlog_blue.dds",
				SizePx = new Vector2(1343f, 321f),
				PaddingSizePx = new Vector2(20f, 25f)
			};
			TEXTURE_QUESTLOG_BACKGROUND_INFO = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\screen_background.dds",
				SizePx = new Vector2(1024f, 1024f),
				PaddingSizePx = new Vector2(24f, 24f)
			};
			TEXTURE_SCREEN_BACKGROUND = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\screen_background_red.dds",
				SizePx = new Vector2(1024f, 1024f),
				PaddingSizePx = new Vector2(24f, 24f)
			};
			TEXTURE_SCREEN_BACKGROUND_RED = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\CenterGScreen.dds",
				SizePx = new Vector2(913f, 820f),
				PaddingSizePx = new Vector2(12f, 10f)
			};
			TEXTURE_SCREEN_TOOLS_BACKGROUND_BLOCKS = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\screen_tools_background_controls.dds",
				SizePx = new Vector2(397f, 529f),
				PaddingSizePx = new Vector2(24f, 24f)
			};
			TEXTURE_SCREEN_TOOLS_BACKGROUND_CONTROLS = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\screen_tools_background_weapons.dds",
				SizePx = new Vector2(868f, 110f),
				PaddingSizePx = new Vector2(12f, 9f)
			};
			TEXTURE_SCREEN_TOOLS_BACKGROUND_WEAPONS = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\screen_stats_background.dss",
				SizePx = new Vector2(256f, 128f),
				PaddingSizePx = new Vector2(12f, 12f)
			};
			TEXTURE_SCREEN_STATS_BACKGROUND = myGuiPaddedTexture;
			TEXTURE_ICON_MODS_LOCAL = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\ModFolderIcon.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\ModFolderIcon.dds",
				SizePx = new Vector2(53f, 40f)
			};
			TEXTURE_ICON_BLUEPRINTS_LOCAL = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\BluePrintFolderIcon.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\BluePrintFolderIcon.dds",
				SizePx = new Vector2(53f, 40f)
			};
			TEXTURE_ICON_BLUEPRINTS_CLOUD = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\BluePrintCloud.png",
				Highlight = "Textures\\GUI\\Icons\\buttons\\BluePrintCloud.png",
				SizePx = new Vector2(53f, 40f)
			};
			TEXTURE_ICON_STAR = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\star.png",
				Highlight = "Textures\\GUI\\Icons\\star.png",
				SizePx = new Vector2(24f, 24f)
			};
			TEXTURE_ICON_LOCK = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\Lock.png",
				Highlight = "Textures\\GUI\\Icons\\Lock.png",
				SizePx = new Vector2(24f, 24f)
			};
			TEXTURE_ICON_EXPERIMENTAL = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\warning.png",
				Highlight = "Textures\\GUI\\Icons\\warning.png",
				SizePx = new Vector2(24f, 24f)
			};
			TEXTURE_BLUEPRINTS_ARROW = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\ArrowFolderIcon.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\ArrowFolderIcon.dds",
				SizePx = new Vector2(53f, 40f)
			};
			TEXTURE_ICON_MODS_WORKSHOP = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Icons\\buttons\\ModSteamIcon.dds",
				Highlight = "Textures\\GUI\\Icons\\buttons\\ModSteamIcon.dds",
				SizePx = new Vector2(53f, 40f)
			};
			MyGuiCompositeTexture myGuiCompositeTexture82 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(57f, 54f),
				Texture = "Textures\\GUI\\Controls\\checkbox_checked.dds"
			};
			myGuiCompositeTexture82.LeftTop = myGuiSizedTexture;
			TEXTURE_CHECKBOX_DEFAULT_NORMAL_CHECKED = myGuiCompositeTexture82;
			MyGuiCompositeTexture myGuiCompositeTexture83 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(57f, 54f),
				Texture = "Textures\\GUI\\Controls\\checkbox_unchecked.dds"
			};
			myGuiCompositeTexture83.LeftTop = myGuiSizedTexture;
			TEXTURE_CHECKBOX_DEFAULT_NORMAL_UNCHECKED = myGuiCompositeTexture83;
			MyGuiCompositeTexture myGuiCompositeTexture84 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(57f, 54f),
				Texture = "Textures\\GUI\\Controls\\checkbox_indeterminate.dds"
			};
			myGuiCompositeTexture84.LeftTop = myGuiSizedTexture;
			TEXTURE_CHECKBOX_DEFAULT_NORMAL_INDETERMINATE = myGuiCompositeTexture84;
			MyGuiCompositeTexture myGuiCompositeTexture85 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(57f, 54f),
				Texture = "Textures\\GUI\\Controls\\checkbox_checked_highlight.dds"
			};
			myGuiCompositeTexture85.LeftTop = myGuiSizedTexture;
			TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_CHECKED = myGuiCompositeTexture85;
			MyGuiCompositeTexture myGuiCompositeTexture86 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(57f, 54f),
				Texture = "Textures\\GUI\\Controls\\checkbox_unchecked_highlight.dds"
			};
			myGuiCompositeTexture86.LeftTop = myGuiSizedTexture;
			TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_UNCHECKED = myGuiCompositeTexture86;
			MyGuiCompositeTexture myGuiCompositeTexture87 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(57f, 54f),
				Texture = "Textures\\GUI\\Controls\\checkbox_indeterminate_highlight.dds"
			};
			myGuiCompositeTexture87.LeftTop = myGuiSizedTexture;
			TEXTURE_CHECKBOX_DEFAULT_HIGHLIGHT_INDETERMINATE = myGuiCompositeTexture87;
			MyGuiCompositeTexture myGuiCompositeTexture88 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(40f, 38f),
				Texture = "Textures\\GUI\\Controls\\checkbox_green_checked.png"
			};
			myGuiCompositeTexture88.LeftTop = myGuiSizedTexture;
			TEXTURE_CHECKBOX_GREEN_CHECKED = myGuiCompositeTexture88;
			MyGuiCompositeTexture myGuiCompositeTexture89 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(0f, 0f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture89.LeftTop = myGuiSizedTexture;
			TEXTURE_CHECKBOX_BLANK = myGuiCompositeTexture89;
			TEXTURE_SLIDER_THUMB_DEFAULT = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Controls\\slider_thumb.dds",
				Highlight = "Textures\\GUI\\Controls\\slider_thumb_highlight.dds",
				SizePx = new Vector2(32f, 32f)
			};
			TEXTURE_HUE_SLIDER_THUMB_DEFAULT = new MyGuiHighlightTexture
			{
				Normal = "Textures\\GUI\\Controls\\hue_slider_thumb.dds",
				Highlight = "Textures\\GUI\\Controls\\hue_slider_thumb_highlight.dds",
				SizePx = new Vector2(32f, 32f)
			};
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\hud_bg_medium_default.dds",
				SizePx = new Vector2(213f, 183f),
				PaddingSizePx = new Vector2(9f, 15f)
			};
			TEXTURE_HUD_BG_MEDIUM_DEFAULT = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\hud_bg_large_default.dds",
				SizePx = new Vector2(300f, 366f),
				PaddingSizePx = new Vector2(9f, 15f)
			};
			TEXTURE_HUD_BG_LARGE_DEFAULT = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\hud_bg_medium_red.dds",
				SizePx = new Vector2(213f, 181f),
				PaddingSizePx = new Vector2(9f, 15f)
			};
			TEXTURE_HUD_BG_MEDIUM_RED = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\hud_bg_medium_red2.dds",
				SizePx = new Vector2(3f, 127f),
				PaddingSizePx = new Vector2(0f, 15f)
			};
			TEXTURE_HUD_BG_MEDIUM_RED2 = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\hud_bg_performance.dds",
				SizePx = new Vector2(441f, 124f),
				PaddingSizePx = new Vector2(0f, 0f)
			};
			TEXTURE_HUD_BG_PERFORMANCE = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Icons\\VoiceIcon.dds",
				SizePx = new Vector2(128f, 128f),
				PaddingSizePx = new Vector2(5f, 5f)
			};
			TEXTURE_VOICE_CHAT = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Icons\\DisconnectedPlayerIcon.png",
				SizePx = new Vector2(128f, 128f),
				PaddingSizePx = new Vector2(5f, 5f)
			};
			TEXTURE_DISCONNECTED_PLAYER = myGuiPaddedTexture;
			MyGuiCompositeTexture myGuiCompositeTexture90 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\slider_rail_left.dds",
				SizePx = new Vector2(23f, 55f)
			};
			myGuiCompositeTexture90.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\slider_rail_center.dds",
				SizePx = new Vector2(4f, 55f)
			};
			myGuiCompositeTexture90.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\slider_rail_right.dds",
				SizePx = new Vector2(23f, 55f)
			};
			myGuiCompositeTexture90.RightTop = myGuiSizedTexture;
			TEXTURE_SLIDER_RAIL = myGuiCompositeTexture90;
			MyGuiCompositeTexture myGuiCompositeTexture91 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\slider_rail_left_highlight.dds",
				SizePx = new Vector2(23f, 55f)
			};
			myGuiCompositeTexture91.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\slider_rail_center_highlight.dds",
				SizePx = new Vector2(4f, 55f)
			};
			myGuiCompositeTexture91.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\slider_rail_right_highlight.dds",
				SizePx = new Vector2(23f, 55f)
			};
			myGuiCompositeTexture91.RightTop = myGuiSizedTexture;
			TEXTURE_SLIDER_RAIL_HIGHLIGHT = myGuiCompositeTexture91;
			MyGuiCompositeTexture myGuiCompositeTexture92 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\hue_slider_rail_left.dds",
				SizePx = new Vector2(23f, 55f)
			};
			myGuiCompositeTexture92.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\hue_slider_rail_center.dds",
				SizePx = new Vector2(4f, 55f)
			};
			myGuiCompositeTexture92.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\hue_slider_rail_right.dds",
				SizePx = new Vector2(23f, 55f)
			};
			myGuiCompositeTexture92.RightTop = myGuiSizedTexture;
			TEXTURE_HUE_SLIDER_RAIL = myGuiCompositeTexture92;
			MyGuiCompositeTexture myGuiCompositeTexture93 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\hue_slider_rail_left_highlight.dds",
				SizePx = new Vector2(23f, 55f)
			};
			myGuiCompositeTexture93.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\hue_slider_rail_center_highlight.dds",
				SizePx = new Vector2(4f, 55f)
			};
			myGuiCompositeTexture93.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\hue_slider_rail_right_highlight.dds",
				SizePx = new Vector2(23f, 55f)
			};
			myGuiCompositeTexture93.RightTop = myGuiSizedTexture;
			TEXTURE_HUE_SLIDER_RAIL_HIGHLIGHT = myGuiCompositeTexture93;
			MyGuiCompositeTexture myGuiCompositeTexture94 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_v_thumb_top.dds",
				SizePx = new Vector2(46f, 46f)
			};
			myGuiCompositeTexture94.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_v_thumb_center.dds",
				SizePx = new Vector2(46f, 4f)
			};
			myGuiCompositeTexture94.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom.dds",
				SizePx = new Vector2(46f, 23f)
			};
			myGuiCompositeTexture94.LeftBottom = myGuiSizedTexture;
			TEXTURE_SCROLLBAR_V_THUMB = myGuiCompositeTexture94;
			MyGuiCompositeTexture myGuiCompositeTexture95 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_v_thumb_top_highlight.dds",
				SizePx = new Vector2(46f, 46f)
			};
			myGuiCompositeTexture95.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_v_thumb_center_highlight.dds",
				SizePx = new Vector2(46f, 4f)
			};
			myGuiCompositeTexture95.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_v_thumb_bottom_highlight.dds",
				SizePx = new Vector2(46f, 23f)
			};
			myGuiCompositeTexture95.LeftBottom = myGuiSizedTexture;
			TEXTURE_SCROLLBAR_V_THUMB_HIGHLIGHT = myGuiCompositeTexture95;
			TEXTURE_SCROLLBAR_V_BACKGROUND = new MyGuiCompositeTexture();
			MyGuiCompositeTexture myGuiCompositeTexture96 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_h_thumb_left.dds",
				SizePx = new Vector2(39f, 46f)
			};
			myGuiCompositeTexture96.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_h_thumb_center.dds",
				SizePx = new Vector2(4f, 46f)
			};
			myGuiCompositeTexture96.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_h_thumb_right.dds",
				SizePx = new Vector2(26f, 46f)
			};
			myGuiCompositeTexture96.RightTop = myGuiSizedTexture;
			TEXTURE_SCROLLBAR_H_THUMB = myGuiCompositeTexture96;
			MyGuiCompositeTexture myGuiCompositeTexture97 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_h_thumb_left_highlight.dds",
				SizePx = new Vector2(39f, 46f)
			};
			myGuiCompositeTexture97.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_h_thumb_center_highlight.dds",
				SizePx = new Vector2(4f, 46f)
			};
			myGuiCompositeTexture97.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\scrollbar_h_thumb_right_highlight.dds",
				SizePx = new Vector2(26f, 46f)
			};
			myGuiCompositeTexture97.RightTop = myGuiSizedTexture;
			TEXTURE_SCROLLBAR_H_THUMB_HIGHLIGHT = myGuiCompositeTexture97;
			TEXTURE_SCROLLBAR_H_BACKGROUND = new MyGuiCompositeTexture();
			MyGuiCompositeTexture myGuiCompositeTexture98 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\ToolBarTab.dds"
			};
			myGuiCompositeTexture98.Center = myGuiSizedTexture;
			TEXTURE_TOOLBAR_TAB = myGuiCompositeTexture98;
			MyGuiCompositeTexture myGuiCompositeTexture99 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Controls\\ToolBarTabHighlight.dds"
			};
			myGuiCompositeTexture99.Center = myGuiSizedTexture;
			TEXTURE_TOOLBAR_TAB_HIGHLIGHT = myGuiCompositeTexture99;
			MyGuiCompositeTexture myGuiCompositeTexture100 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Composite\\white_left_top.dds"
			};
			myGuiCompositeTexture100.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Composite\\white_right_top.dds"
			};
			myGuiCompositeTexture100.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture100.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Composite\\white_left_bottom.dds"
			};
			myGuiCompositeTexture100.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Composite\\white_right_bottom.dds"
			};
			myGuiCompositeTexture100.RightBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture100.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture100.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture100.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture100.RightCenter = myGuiSizedTexture;
			TEXTURE_COMPOSITE_ROUND_ALL = myGuiCompositeTexture100;
			MyGuiCompositeTexture myGuiCompositeTexture101 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(3f, 3f),
				Texture = "Textures\\GUI\\Composite\\white_left_top.dds"
			};
			myGuiCompositeTexture101.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(3f, 3f),
				Texture = "Textures\\GUI\\Composite\\white_right_top.dds"
			};
			myGuiCompositeTexture101.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(3f, 3f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture101.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(3f, 3f),
				Texture = "Textures\\GUI\\Composite\\white_left_bottom.dds"
			};
			myGuiCompositeTexture101.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(3f, 3f),
				Texture = "Textures\\GUI\\Composite\\white_right_bottom.dds"
			};
			myGuiCompositeTexture101.RightBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(3f, 3f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture101.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(3f, 3f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture101.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(3f, 3f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture101.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(3f, 3f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture101.RightCenter = myGuiSizedTexture;
			TEXTURE_COMPOSITE_ROUND_ALL_SMALL = myGuiCompositeTexture101;
			MyGuiCompositeTexture myGuiCompositeTexture102 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Composite\\white_left_top.dds"
			};
			myGuiCompositeTexture102.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Composite\\white_right_top.dds"
			};
			myGuiCompositeTexture102.RightTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture102.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture102.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture102.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(8f, 8f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture102.RightCenter = myGuiSizedTexture;
			TEXTURE_COMPOSITE_ROUND_TOP = myGuiCompositeTexture102;
			MyGuiCompositeTexture myGuiCompositeTexture103 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(48f, 48f),
				Texture = "Textures\\GUI\\Composite\\white_left_bottom_slope.dds"
			};
			myGuiCompositeTexture103.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(48f, 1f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture103.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = Vector2.One,
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture103.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(1f, 48f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture103.CenterBottom = myGuiSizedTexture;
			TEXTURE_COMPOSITE_SLOPE_LEFTBOTTOM = myGuiCompositeTexture103;
			MyGuiCompositeTexture myGuiCompositeTexture104 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(48f, 30f),
				Texture = "Textures\\GUI\\Composite\\white_left_bottom_slope_30.dds"
			};
			myGuiCompositeTexture104.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(48f, 1f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture104.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = Vector2.One,
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture104.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(1f, 30f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture104.CenterBottom = myGuiSizedTexture;
			TEXTURE_COMPOSITE_SLOPE_LEFTBOTTOM_30 = myGuiCompositeTexture104;
			MyGuiCompositeTexture myGuiCompositeTexture105 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 40f),
				Texture = "Textures\\GUI\\Composite\\white_left_bottom_blockinfo.dds"
			};
			myGuiCompositeTexture105.LeftBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 1f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture105.LeftCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(64f, 1f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture105.LeftTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(1f, 40f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture105.CenterBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = Vector2.One,
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture105.Center = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = Vector2.One,
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture105.CenterTop = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(1f, 40f),
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture105.RightBottom = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = Vector2.One,
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture105.RightCenter = myGuiSizedTexture;
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = Vector2.One,
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture105.RightTop = myGuiSizedTexture;
			TEXTURE_COMPOSITE_BLOCKINFO_PROGRESSBAR = myGuiCompositeTexture105;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\GravityHudGlobe.dds",
				SizePx = new Vector2(138f, 138f),
				PaddingSizePx = new Vector2(0f, 0f)
			};
			TEXTURE_HUD_GRAVITY_GLOBE = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\GravityHudLine.dds",
				SizePx = new Vector2(228f, 2f),
				PaddingSizePx = new Vector2(0f, 0f)
			};
			TEXTURE_HUD_GRAVITY_LINE = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\GravityHudHorizon.dds",
				SizePx = new Vector2(512f, 512f),
				PaddingSizePx = new Vector2(0f, 0f)
			};
			TEXTURE_HUD_GRAVITY_HORIZON = myGuiPaddedTexture;
			MyGuiCompositeTexture myGuiCompositeTexture106 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Blank.dds"
			};
			myGuiCompositeTexture106.Center = myGuiSizedTexture;
			TEXTURE_GUI_BLANK = myGuiCompositeTexture106;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\screen_stats_background.dds",
				SizePx = new Vector2(256f, 128f),
				PaddingSizePx = new Vector2(6f, 6f)
			};
			TEXTURE_HUD_STATS_BG = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Icons\\ArrowUpBrown.dds"
			};
			TEXTURE_HUD_STAT_EFFECT_ARROW_UP = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Icons\\ArrowDownRed.dds"
			};
			TEXTURE_HUD_STAT_EFFECT_ARROW_DOWN = myGuiPaddedTexture;
			myGuiPaddedTexture = new MyGuiPaddedTexture
			{
				Texture = "Textures\\GUI\\Screens\\screen_stats_bar_background.dds",
				SizePx = new Vector2(72f, 13f),
				PaddingSizePx = new Vector2(1f, 1f)
			};
			TEXTURE_HUD_STAT_BAR_BG = myGuiPaddedTexture;
			MyGuiCompositeTexture myGuiCompositeTexture107 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Icons\\HUD 2017\\GridSizeLarge.png"
			};
			myGuiCompositeTexture107.Center = myGuiSizedTexture;
			TEXTURE_HUD_GRID_LARGE = myGuiCompositeTexture107;
			MyGuiCompositeTexture myGuiCompositeTexture108 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Icons\\HUD 2017\\GridSizeLargeFit.png"
			};
			myGuiCompositeTexture108.Center = myGuiSizedTexture;
			TEXTURE_HUD_GRID_LARGE_FIT = myGuiCompositeTexture108;
			MyGuiCompositeTexture myGuiCompositeTexture109 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Icons\\HUD 2017\\GridSizeSmall.png"
			};
			myGuiCompositeTexture109.Center = myGuiSizedTexture;
			TEXTURE_HUD_GRID_SMALL = myGuiCompositeTexture109;
			MyGuiCompositeTexture myGuiCompositeTexture110 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				Texture = "Textures\\GUI\\Icons\\HUD 2017\\GridSizeSmallFit.png"
			};
			myGuiCompositeTexture110.Center = myGuiSizedTexture;
			TEXTURE_HUD_GRID_SMALL_FIT = myGuiCompositeTexture110;
			SHADOW_OFFSET = new Vector2(0f, 0f);
			CONTROL_MOUSE_OVER_BACKGROUND_COLOR_MULTIPLIER = new Vector4(1.2f, 1.2f, 1.2f, 1f);
			CONTROLS_DELTA = new Vector2(0f, 0.0525f);
			ROTATING_WHEEL_COLOR = Vector4.One;
			SHOW_CONTROL_TOOLTIP_DELAY = 20;
			TOOLTIP_DISTANCE_FROM_BORDER = 0.003f;
			DEFAULT_CONTROL_BACKGROUND_COLOR = new Vector4(1f, 1f, 1f, 1f);
			DEFAULT_CONTROL_NONACTIVE_COLOR = new Vector4(0.9f, 0.9f, 0.9f, 0.95f);
			DISABLED_BUTTON_COLOR = new Color(87, 127, 147, 210);
			DISABLED_BUTTON_COLOR_VECTOR = new Vector4(0.52f, 0.6f, 0.63f, 0.9f);
			DISABLED_BUTTON_TEXT_COLOR = new Vector4(0.4f, 0.47f, 0.5f, 0.8f);
			LOCKBUTTON_SIZE_MODIFICATION = 0.85f;
			SCREEN_BACKGROUND_FADE_BLANK_DARK = new Vector4(0.03f, 0.04f, 0.05f, 0.7f);
			SCREEN_BACKGROUND_FADE_BLANK_DARK_PROGRESS_SCREEN = new Vector4(0.03f, 0.04f, 0.05f, 0.4f);
			SCREEN_CAPTION_DELTA_Y = 0.05f;
			SCREEN_BACKGROUND_COLOR = Vector4.One;
			LOADING_PLEASE_WAIT_POSITION = new Vector2(0.5f, 0.95f);
			LOADING_PLEASE_WAIT_COLOR = Vector4.One;
			TEXTBOX_TEXT_OFFSET = new Vector2(0.0075f, 0.007f);
			TEXTBOX_MEDIUM_SIZE = new Vector2(0.2525f, 0.055f);
			MOUSE_CURSOR_COLOR = Vector4.One;
			BUTTON_BACKGROUND_COLOR = DEFAULT_CONTROL_BACKGROUND_COLOR;
			MENU_BUTTONS_POSITION_DELTA = new Vector2(0f, 0.06f);
			BACK_BUTTON_BACKGROUND_COLOR = BUTTON_BACKGROUND_COLOR;
			BACK_BUTTON_TEXT_COLOR = DEFAULT_CONTROL_NONACTIVE_COLOR;
			BACK_BUTTON_SIZE = new Vector2(0.1625f, 7f / 120f);
			OK_BUTTON_SIZE = new Vector2(0.177f, 0.0765f);
			GENERIC_BUTTON_SPACING = new Vector2(0.002f, 0.002f);
			TREEVIEW_SELECTED_ITEM_COLOR = new Vector4(0.03f, 0.02f, 0.03f, 0.4f);
			TREEVIEW_DISABLED_ITEM_COLOR = new Vector4(1f, 0.3f, 0.3f, 1f);
			TREEVIEW_TEXT_COLOR = DEFAULT_CONTROL_NONACTIVE_COLOR;
			TREEVIEW_VERTICAL_LINE_COLOR = new Vector4(158f / 255f, 208f / 255f, 1f, 1f);
			TREEVIEW_VSCROLLBAR_SIZE = new Vector2(60f, 636f) / 3088f;
			TREEVIEW_HSCROLLBAR_SIZE = new Vector2(477f, 80f) / 3088f;
			COMBOBOX_MEDIUM_SIZE = new Vector2(0.3f, 0.03f);
			COMBOBOX_MEDIUM_ELEMENT_SIZE = new Vector2(0.3f, 0.03f);
			COMBOBOX_VSCROLLBAR_SIZE = new Vector2(0.02f, 0.08059585f);
			COMBOBOX_HSCROLLBAR_SIZE = new Vector2(0.08059585f, 0.02f);
			LISTBOX_BACKGROUND_COLOR = DEFAULT_CONTROL_BACKGROUND_COLOR;
			LISTBOX_ICON_SIZE = new Vector2(0.0205f, 0.02733f);
			LISTBOX_ICON_OFFSET = LISTBOX_ICON_SIZE / 8f;
			LISTBOX_WIDTH = 0.198f;
			DRAG_AND_DROP_TEXT_OFFSET = new Vector2(0.01f, 0f);
			DRAG_AND_DROP_TEXT_COLOR = DEFAULT_CONTROL_NONACTIVE_COLOR;
			DRAG_AND_DROP_SMALL_SIZE = new Vector2(0.07395f, 0.0986f);
			DRAG_AND_DROP_BACKGROUND_COLOR = new Vector4(1f, 1f, 1f, 1f);
			SLIDER_INSIDE_OFFSET_X = 0.017f;
			REPEAT_PRESS_DELAY = 100;
			MESSAGE_BOX_BUTTON_SIZE_SMALL = new Vector2(19f / 160f, 13f / 240f);
			TOOL_TIP_RELATIVE_DEFAULT_POSITION = new Vector2(0.025f, 0.03f);
			LOADING_BACKGROUND_TEXTURE_REAL_SIZE = new Vector2I(1920, 1080);
			COLORED_TEXT_DEFAULT_COLOR = new Color(DEFAULT_CONTROL_NONACTIVE_COLOR);
			COLORED_TEXT_DEFAULT_HIGHLIGHT_COLOR = new Color(CONTROL_MOUSE_OVER_BACKGROUND_COLOR_MULTIPLIER * DEFAULT_CONTROL_NONACTIVE_COLOR);
			MULTILINE_LABEL_BORDER = new Vector2(0.01f, 0.005f);
			DEBUG_LABEL_TEXT_SCALE = 1f;
			DEBUG_BUTTON_TEXT_SCALE = 0.8f;
			DEBUG_STATISTICS_TEXT_SCALE = 0.75f;
			DEBUG_STATISTICS_ROW_DISTANCE = 0.02f;
			MyGuiCompositeTexture myGuiCompositeTexture111 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(45f, 45f),
				Texture = "Textures\\GUI\\Icons\\buttons\\SquareButtonHighlight.dds"
			};
			myGuiCompositeTexture111.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_SQUARE_SMALL_HIGHLIGHT = myGuiCompositeTexture111;
			MyGuiCompositeTexture myGuiCompositeTexture112 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(45f, 45f),
				Texture = "Textures\\GUI\\Icons\\buttons\\SquareButton.dds"
			};
			myGuiCompositeTexture112.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_SQUARE_SMALL_NORMAL = myGuiCompositeTexture112;
			MyGuiCompositeTexture myGuiCompositeTexture113 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(48f, 48f),
				Texture = "Textures\\GUI\\Icons\\buttons\\SquareButtonHighlight.dds"
			};
			myGuiCompositeTexture113.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_SQUARE_48_HIGHLIGHT = myGuiCompositeTexture113;
			MyGuiCompositeTexture myGuiCompositeTexture114 = new MyGuiCompositeTexture();
			myGuiSizedTexture = new MyGuiSizedTexture
			{
				SizePx = new Vector2(48f, 48f),
				Texture = "Textures\\GUI\\Icons\\buttons\\SquareButton.dds"
			};
			myGuiCompositeTexture114.LeftTop = myGuiSizedTexture;
			TEXTURE_BUTTON_SQUARE_48_NORMAL = myGuiCompositeTexture114;
		}
	}
}
