using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Cube Grid")]
	internal class MyGuiScreenDebugCubeGrid : MyGuiScreenDebugBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugCubeGrid";
		}

		public MyGuiScreenDebugCubeGrid()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Cube Grid", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddLabel("Cube Grid", Color.White.ToVector4(), 1f);
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Grid names", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_NAMES));
			AddCheckBox("Grid origins", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_ORIGINS));
			AddCheckBox("Grid control", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_CONTROL));
			AddCheckBox("Grid AABBs", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_AABB));
			AddCheckBox("Removed cube coordinates", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_REMOVE_CUBE_COORDS));
			AddCheckBox("Grid counter", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_COUNTER));
			AddCheckBox("Grid terminal systems", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_TERMINAL_SYSTEMS));
			AddCheckBox("Grid dirty blocks", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_DIRTY_BLOCKS));
			AddCheckBox("Grid groups - physical", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_PHYSICAL));
			AddCheckBox("Grid groups - physical dynamic", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_DYNAMIC_PHYSICAL_GROUPS));
			AddCheckBox("Grid groups - logical", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_LOGICAL));
			AddCheckBox("CubeBlock AABBs", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CUBE_BLOCK_AABBS));
			AddCheckBox("Grid statistics", () => MyDebugDrawSettings.DEBUG_DRAW_GRID_STATISTICS, delegate(bool x)
			{
				MyDebugDrawSettings.DEBUG_DRAW_GRID_STATISTICS = x;
			});
			m_currentPosition.Y += 0.02f;
			AddLabel("Cube Grid Systems", Color.White.ToVector4(), 1f);
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Conveyor line IDs", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_IDS));
			AddCheckBox("Conveyor line capsules", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_CAPSULES));
			AddCheckBox("Connectors and merge blocks", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONNECTORS_AND_MERGE_BLOCKS));
			AddCheckBox("Terminal block names", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_BLOCK_NAMES));
			AddCheckBox("Radio broadcasters", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_RADIO_BROADCASTERS));
			AddCheckBox("Neutral ships", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_NEUTRAL_SHIPS));
			AddCheckBox("Resource Distribution", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS));
			AddCheckBox("Cockpit", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_COCKPIT));
			AddCheckBox("Conveyors", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS));
			AddCheckBox("Thruster damage", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_THRUSTER_DAMAGE));
			AddCheckBox("Block groups", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS));
			AddCheckBox("Rotors", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ROTORS));
			AddCheckBox("Gyros", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GYROS));
			AddCheckBox("Local coordinate system", null, MemberHelper.GetMember(() => MyFakes.ENABLE_DEBUG_DRAW_COORD_SYS));
		}
	}
}
