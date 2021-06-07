using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Debug draw settings 2")]
	internal class MyGuiScreenDebugDrawSettings2 : MyGuiScreenDebugBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugDrawSettings";
		}

		public MyGuiScreenDebugDrawSettings2()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Debug draw settings 2", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddCheckBox("Entity components", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ENTITY_COMPONENTS));
			AddCheckBox("Controlled entities", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONTROLLED_ENTITIES));
			AddCheckBox("Copy paste", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_COPY_PASTE));
			AddCheckBox("Voxel geometry cell", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_GEOMETRY_CELL));
			AddCheckBox("Voxel map AABB", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MAP_AABB));
			AddCheckBox("Respawn ship counters", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_RESPAWN_SHIP_COUNTERS));
			AddCheckBox("Explosion Havok raycasts", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_HAVOK_RAYCASTS));
			AddCheckBox("Explosion DDA raycasts", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_EXPLOSION_DDA_RAYCASTS));
			AddCheckBox("Physics clusters", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS));
			AddCheckBox("Environment items (trees, bushes, ...)", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ENVIRONMENT_ITEMS));
			AddCheckBox("Block groups - small to large", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_SMALL_TO_LARGE_BLOCK_GROUPS));
			AddCheckBox("Ropes", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ROPES));
			AddCheckBox("Oxygen", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_OXYGEN));
			AddCheckBox("Voxel physics prediction", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_PHYSICS_PREDICTION));
			AddCheckBox("Update trigger", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER));
		}
	}
}
