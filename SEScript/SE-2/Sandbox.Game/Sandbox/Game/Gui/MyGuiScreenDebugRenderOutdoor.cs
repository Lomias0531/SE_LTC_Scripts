using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Outdoor")]
	internal class MyGuiScreenDebugRenderOutdoor : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugRenderOutdoor()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Outdoor", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddCheckBox("Freeze terrain queries", MyRenderProxy.Settings.FreezeTerrainQueries, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.FreezeTerrainQueries = x.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Grass", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Grass maximum draw distance", MyRenderProxy.Settings.User.GrassDrawDistance, 0f, 1000f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.User.GrassDrawDistance = x.Value;
			});
			AddSlider("Scaling near distance", MyRenderProxy.Settings.GrassGeometryScalingNearDistance, 0f, 1000f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.GrassGeometryScalingNearDistance = x.Value;
			});
			AddSlider("Scaling far distance", MyRenderProxy.Settings.GrassGeometryScalingFarDistance, 0f, 1000f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.GrassGeometryScalingFarDistance = x.Value;
			});
			AddSlider("Scaling factor", MyRenderProxy.Settings.GrassGeometryDistanceScalingFactor, 0f, 10f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.GrassGeometryDistanceScalingFactor = x.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Wind", Color.Yellow.ToVector4(), 1.2f);
			AddSlider("Strength", MyRenderProxy.Settings.WindStrength, 0f, 10f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.WindStrength = x.Value;
			});
			AddSlider("Azimuth", MyRenderProxy.Settings.WindAzimuth, 0f, 360f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.WindAzimuth = x.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Lights", Color.Yellow.ToVector4(), 1.2f);
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderOutdoor";
		}
	}
}
