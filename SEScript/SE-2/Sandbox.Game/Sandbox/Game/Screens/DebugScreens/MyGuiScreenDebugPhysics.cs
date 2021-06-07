using Havok;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using System;
using VRage;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Physics")]
	public class MyGuiScreenDebugPhysics : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugPhysics()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugPhysics";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 0.5f);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCaption("Physics", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddCaption("Debug Draw");
			AddCheckBox("Shapes", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES));
			AddCheckBox("Inertia tensors", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_INERTIA_TENSORS));
			AddCheckBox("Clusters", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS));
			AddCheckBox("Forces", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_FORCES));
			AddCheckBox("Friction", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_FRICTION));
			AddCheckBox("Constraints", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_CONSTRAINTS));
			AddCheckBox("Simulation islands", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SIMULATION_ISLANDS));
			AddCheckBox("Motion types", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_MOTION_TYPES));
			AddCheckBox("Velocities", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VELOCITIES));
			AddCheckBox("Velocities interpolated", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATED_VELOCITIES));
			AddCheckBox("TOI optimized grids", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_TOI_OPTIMIZED_GRIDS));
			AddSubcaption("Hk scheduling");
			AddCheckBox("Havok multithreading", null, MemberHelper.GetMember(() => MyFakes.ENABLE_HAVOK_MULTITHREADING));
			AddCheckBox("Parallel scheduling", null, MemberHelper.GetMember(() => MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING));
			AddButton("Set on server", delegate
			{
				MyPhysics.CommitSchedulingSettingToServer();
			});
			AddButton("Record VDB", delegate
			{
				MyPhysics.SyncVDBCamera = true;
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner _) => MyPhysics.ControlVDBRecording, DateTime.Now.ToString() + ".hkm");
			});
			AddButton("Stop VDB recording", delegate
			{
				MyMultiplayer.RaiseStaticEvent<string>((IMyEventOwner _) => MyPhysics.ControlVDBRecording, null);
			});
			AddSubcaption("Physics options");
			AddCheckBox("Enable Welding", null, MemberHelper.GetMember(() => MyFakes.WELD_LANDING_GEARS));
			AddCheckBox("Weld pistons", null, MemberHelper.GetMember(() => MyFakes.WELD_PISTONS));
			AddCheckBox("Wheel softness", null, MemberHelper.GetMember(() => MyFakes.WHEEL_SOFTNESS)).SetToolTip("Needs to be true at world load.");
			AddCheckBox("Suspension power ratio", null, MemberHelper.GetMember(() => MyFakes.SUSPENSION_POWER_RATIO));
			AddCheckBox("Two step simulations", null, MemberHelper.GetMember(() => MyFakes.TWO_STEP_SIMULATIONS));
			AddButton("Start VDB", delegate
			{
				HkVDB.Start();
			});
			AddButton("Force cluster reorder", delegate
			{
				MyFakes.FORCE_CLUSTER_REORDER = true;
			});
		}
	}
}
