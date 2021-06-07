using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Debug draw settings")]
	internal class MyGuiScreenDebugDrawSettings : MyGuiScreenDebugBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugDrawSettings";
		}

		public MyGuiScreenDebugDrawSettings()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Debug draw settings 1", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddCheckBox("Debug draw", null, MemberHelper.GetMember(() => MyDebugDrawSettings.ENABLE_DEBUG_DRAW));
			AddCheckBox("Entity IDs", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ENTITY_IDS));
			AddCheckBox("    Only root entities", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ENTITY_IDS_ONLY_ROOT));
			AddCheckBox("Model dummies", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES));
			AddCheckBox("Displaced bones", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES));
			AddCheckBox("Interpolation", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATION));
			AddCheckBox("Mount points", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS));
			AddCheckBox("GUI screen borders", null, MemberHelper.GetMember(() => MyFakes.DRAW_GUI_SCREEN_BORDERS));
			AddCheckBox("Draw physics", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS));
			AddCheckBox("Triangle physics", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_TRIANGLE_PHYSICS));
			AddCheckBox("Audio debug draw", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_AUDIO));
			AddCheckBox("Show invalid triangles", null, MemberHelper.GetMember(() => MyFakes.SHOW_INVALID_TRIANGLES));
			AddCheckBox("Show stockpile quantities", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_STOCKPILE_QUANTITIES));
			AddCheckBox("Show suit battery capacity", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_SUIT_BATTERY_CAPACITY));
			AddCheckBox("Show character bones", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_BONES));
			AddCheckBox("Character miscellaneous", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_MISC));
			AddCheckBox("Game prunning structure", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GAME_PRUNNING));
			AddCheckBox("Miscellaneous", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_MISCELLANEOUS));
			AddCheckBox("Events", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_EVENTS));
			AddCheckBox("Volumetric explosion coloring", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOLUMETRIC_EXPLOSION_COLORING));
		}
	}
}
