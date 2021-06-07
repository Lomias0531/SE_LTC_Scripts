using Sandbox.Definitions.GUI;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Weapons;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.GUI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Debug draw settings 3")]
	internal class MyGuiScreenDebugDrawSettings3 : MyGuiScreenDebugBase
	{
		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugDrawSettings3";
		}

		public MyGuiScreenDebugDrawSettings3()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			m_scale = 0.7f;
			AddCaption("Debug draw settings 3", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddCheckBox("Debug decals", MyRenderProxy.Settings.DebugDrawDecals, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DebugDrawDecals = x.IsChecked;
			});
			AddCheckBox("Decals default material", null, MemberHelper.GetMember(() => MyFakes.ENABLE_USE_DEFAULT_DAMAGE_DECAL));
			AddButton(new StringBuilder("Clear decals"), ClearDecals);
			AddCheckBox("Debug Particles", () => MyDebugDrawSettings.DEBUG_DRAW_PARTICLES, delegate(bool x)
			{
				MyDebugDrawSettings.DEBUG_DRAW_PARTICLES = x;
			});
			AddCheckBox("Entity update statistics", () => MyDebugDrawSettings.DEBUG_DRAW_ENTITY_STATISTICS, delegate(bool x)
			{
				MyDebugDrawSettings.DEBUG_DRAW_ENTITY_STATISTICS = x;
			});
			AddCheckBox("3rd person camera", () => MyThirdPersonSpectator.Static != null && MyThirdPersonSpectator.Static.EnableDebugDraw, delegate(bool x)
			{
				if (MyThirdPersonSpectator.Static != null)
				{
					MyThirdPersonSpectator.Static.EnableDebugDraw = x;
				}
			});
			AddCheckBox("Inverse kinematics", () => MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS, delegate(bool x)
			{
				MyDebugDrawSettings.DEBUG_DRAW_INVERSE_KINEMATICS = x;
			});
			AddCheckBox("Character tools", () => MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS, delegate(bool x)
			{
				MyDebugDrawSettings.DEBUG_DRAW_CHARACTER_TOOLS = x;
			});
			AddCheckBox("Force tools 1st person", () => MyFakes.FORCE_CHARTOOLS_1ST_PERSON, delegate(bool x)
			{
				MyFakes.FORCE_CHARTOOLS_1ST_PERSON = x;
			});
			AddCheckBox("HUD", () => MyDebugDrawSettings.DEBUG_DRAW_HUD, delegate(bool x)
			{
				MyDebugDrawSettings.DEBUG_DRAW_HUD = x;
			});
			AddCheckBox("Server Messages (Performance Warnings)", () => MyDebugDrawSettings.DEBUG_DRAW_SERVER_WARNINGS, delegate(bool x)
			{
				MyDebugDrawSettings.DEBUG_DRAW_SERVER_WARNINGS = x;
			});
			AddCheckBox("Network Sync", () => MyDebugDrawSettings.DEBUG_DRAW_NETWORK_SYNC, delegate(bool x)
			{
				MyDebugDrawSettings.DEBUG_DRAW_NETWORK_SYNC = x;
			});
			AddCheckBox("Grid hierarchy", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_GRID_HIERARCHY));
			AddButton("Reload HUD", delegate
			{
				ReloadHud();
			});
			AddCheckBox("Turret Target Prediction", null, MemberHelper.GetMember(() => MyLargeTurretBase.DEBUG_DRAW_TARGET_PREDICTION));
			AddCheckBox("Projectile Trajectory", null, MemberHelper.GetMember(() => MyProjectile.DEBUG_DRAW_PROJECTILE_TRAJECTORY));
			AddCheckBox("Missile Trajectory", null, MemberHelper.GetMember(() => MyMissile.DEBUG_DRAW_MISSILE_TRAJECTORY));
			AddCheckBox("Show Joystick Controls", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_JOYSTICK_CONTROL_HINTS));
			AddCheckBox("Draw Gui Control Borders", null, MemberHelper.GetMember(() => MyGuiControlBase.DEBUG_CONTROL_BORDERS));
			AddCheckBox("Voxel - full cells", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_FULLCELLS));
			AddCheckBox("Voxel - content micro nodes", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES));
			AddCheckBox("Voxel - content micro nodes scaled", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MICRONODES_SCALED));
			AddCheckBox("Voxel - content macro nodes", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRONODES));
			AddCheckBox("Voxel - content macro leaves", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACROLEAVES));
			AddCheckBox("Voxel - content macro scaled", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTENT_MACRO_SCALED));
			AddCheckBox("Voxel - materials macro nodes", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACRONODES));
			AddCheckBox("Voxel - materials macro leaves", null, MemberHelper.GetMember(() => MyDebugDrawSettings.DEBUG_DRAW_VOXEL_MATERIALS_MACROLEAVES));
		}

		private static void ClearDecals(MyGuiControlButton button)
		{
			MyRenderProxy.ClearDecals();
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}

		private static bool ReloadHud()
		{
			MyHudDefinition hudDefinition = MyHud.HudDefinition;
			MyGuiTextureAtlasDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyGuiTextureAtlasDefinition>(MyStringHash.GetOrCompute("Base"));
			if (!MyObjectBuilderSerializer.DeserializeXML(hudDefinition.Context.CurrentFile, out MyObjectBuilder_Definitions objectBuilder))
			{
				MyAPIGateway.Utilities.ShowNotification("Failed to load Hud.sbc!", 3000, "Red");
				return false;
			}
			hudDefinition.Init(objectBuilder.Definitions[0], hudDefinition.Context);
			if (!MyObjectBuilderSerializer.DeserializeXML(definition.Context.CurrentFile, out objectBuilder))
			{
				MyAPIGateway.Utilities.ShowNotification("Failed to load GuiTextures.sbc!", 3000, "Red");
				return false;
			}
			definition.Init(objectBuilder.Definitions[0], definition.Context);
			MyGuiTextures.Static.Reload();
			MyScreenManager.CloseScreen(MyPerGameSettings.GUI.HUDScreen);
			MyScreenManager.AddScreen(Activator.CreateInstance(MyPerGameSettings.GUI.HUDScreen) as MyGuiScreenBase);
			return true;
		}
	}
}
