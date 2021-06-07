using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens
{
	[MyDebugScreen("VRage", "Deformation")]
	public class MyGuiScreenDebugDeformation : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugDeformation()
		{
			RecreateControls(constructor: true);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugDeformation";
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.BackgroundColor = new Vector4(1f, 1f, 1f, 0.5f);
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.13f);
			AddCaption("Deformation", Color.Yellow.ToVector4());
			AddShareFocusHint();
			AddSlider("Break Velocity", 0f, 100f, () => MyFakes.DEFORMATION_MINIMUM_VELOCITY, delegate(float v)
			{
				MyFakes.DEFORMATION_MINIMUM_VELOCITY = v;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Vertical Offset Ratio", 0f, 5f, () => MyFakes.DEFORMATION_OFFSET_RATIO, delegate(float v)
			{
				MyFakes.DEFORMATION_OFFSET_RATIO = v;
			});
			AddSlider("Vertical Offset Limit", 0f, 100f, () => MyFakes.DEFORMATION_OFFSET_MAX, delegate(float v)
			{
				MyFakes.DEFORMATION_OFFSET_MAX = v;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Velocity Relay", 0f, 1f, () => MyFakes.DEFORMATION_VELOCITY_RELAY, delegate(float v)
			{
				MyFakes.DEFORMATION_VELOCITY_RELAY = v;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Projectile Vertical Offset", 0f, 0.05f, () => MyFakes.DEFORMATION_PROJECTILE_OFFSET_RATIO, delegate(float v)
			{
				MyFakes.DEFORMATION_PROJECTILE_OFFSET_RATIO = v;
			});
			AddCaption("Simple controls (use on your own risk)");
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Voxel cutouts enabled", MyFakes.DEFORMATION_EXPLOSIONS, delegate(MyGuiControlCheckbox x)
			{
				MyFakes.DEFORMATION_EXPLOSIONS = x.IsChecked;
			});
			AddSlider("Voxel cutouts multiplier", 0f, 15f, () => MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER, delegate(float v)
			{
				MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER = v;
			});
			AddSlider("Voxel cutout max radius", 0f, 100f, () => MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS, delegate(float v)
			{
				MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS = v;
			});
			AddSlider("Grid damage multiplier", 0f, 10f, () => MyFakes.DEFORMATION_DAMAGE_MULTIPLIER, delegate(float v)
			{
				MyFakes.DEFORMATION_DAMAGE_MULTIPLIER = v;
			});
		}
	}
}
