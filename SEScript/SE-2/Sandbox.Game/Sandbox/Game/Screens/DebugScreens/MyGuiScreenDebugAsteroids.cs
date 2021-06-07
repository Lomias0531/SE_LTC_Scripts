using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Asteroids")]
	public class MyGuiScreenDebugAsteroids : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugAsteroids()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return GetType().Name;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCaption("Asteroids", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddLabel("Asteroid generator " + MySession.Static?.Settings.VoxelGeneratorVersion, Color.Yellow, 1f);
			AddCheckBox("Draw voxelmap AABB", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MAP_AABB));
			AddCheckBox("Draw asteroid composition", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_COMPOSITION));
			AddCheckBox("Draw asteroid seeds", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_SEEDS));
			AddCheckBox("Draw asteroid content", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_COMPOSITION_CONTENT));
			AddCheckBox("Draw asteroid ores", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_ASTEROID_ORES));
		}
	}
}
