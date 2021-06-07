using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Postprocess Eye Adaptation")]
	internal class MyGuiScreenDebugRenderPostprocessEyeAdaptation : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugRenderPostprocessEyeAdaptation()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Postprocess Eye Adaptation", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddLabel("Eye adaptation", Color.Yellow.ToVector4(), 1.2f);
			AddCheckBox("Enable", () => MyPostprocessSettingsWrapper.Settings.EnableEyeAdaptation, delegate(bool b)
			{
				MyPostprocessSettingsWrapper.Settings.EnableEyeAdaptation = b;
			});
			AddSlider("Tau", 0f, 10f, () => MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationTau, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationTau = f;
			});
			AddCheckBox("Display Histogram", MyRenderProxy.Settings.DisplayHistogram, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayHistogram = x.IsChecked;
			});
			AddCheckBox("Display HDR intensity", MyRenderProxy.Settings.DisplayHdrIntensity, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayHdrIntensity = x.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Histogram Log Min", MyPostprocessSettingsWrapper.Settings.HistogramLogMin, -8f, 8f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.HistogramLogMin = x.Value;
			});
			AddSlider("Histogram Log Max", MyPostprocessSettingsWrapper.Settings.HistogramLogMax, -8f, 8f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.HistogramLogMax = x.Value;
			});
			AddSlider("Histogram Filter Min", MyPostprocessSettingsWrapper.Settings.HistogramFilterMin, 0f, 100f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.HistogramFilterMin = x.Value;
			});
			AddSlider("Histogram Filter Max", MyPostprocessSettingsWrapper.Settings.HistogramFilterMax, 0f, 100f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.HistogramFilterMax = x.Value;
			});
			AddSlider("Min Eye Adaptation Log Brightness", MyPostprocessSettingsWrapper.Settings.MinEyeAdaptationLogBrightness, -8f, 8f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.MinEyeAdaptationLogBrightness = x.Value;
			});
			AddSlider("Max Eye Adaptation Log Brightness", MyPostprocessSettingsWrapper.Settings.MaxEyeAdaptationLogBrightness, -8f, 8f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.MaxEyeAdaptationLogBrightness = x.Value;
			});
			AddSlider("Adaptation Speed Up", MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationSpeedUp, 0f, 4f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationSpeedUp = x.Value;
			});
			AddSlider("Adaptation Speed Down", MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationSpeedDown, 0f, 4f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.EyeAdaptationSpeedDown = x.Value;
			});
			AddCheckBox("Prioritize Screen Center", MyPostprocessSettingsWrapper.Settings.EyeAdaptationPrioritizeScreenCenter, delegate(MyGuiControlCheckbox x)
			{
				MyPostprocessSettingsWrapper.Settings.EyeAdaptationPrioritizeScreenCenter = x.IsChecked;
			});
			AddSlider("Histogram Luminance Threshold", MyPostprocessSettingsWrapper.Settings.HistogramLuminanceThreshold, 0f, 0.5f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.HistogramLuminanceThreshold = x.Value;
			});
			AddSlider("Histogram Skybox Factor", MyPostprocessSettingsWrapper.Settings.HistogramSkyboxFactor, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.HistogramSkyboxFactor = x.Value;
			});
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderPostprocessEyeAdaptation";
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}
	}
}
