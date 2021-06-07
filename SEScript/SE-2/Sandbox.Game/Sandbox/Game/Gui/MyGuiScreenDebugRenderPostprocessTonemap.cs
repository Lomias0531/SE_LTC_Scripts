using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Postprocess Tonemap")]
	internal class MyGuiScreenDebugRenderPostprocessTonemap : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugRenderPostprocessTonemap()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Postprocess Tonemap", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddLabel("Tonemapping", Color.Yellow.ToVector4(), 1.2f);
			AddCheckBox("Enable", () => MyPostprocessSettingsWrapper.Settings.EnableTonemapping, delegate(bool b)
			{
				MyPostprocessSettingsWrapper.Settings.EnableTonemapping = b;
			});
			AddCheckBox("Display HDR Test", MyRenderProxy.Settings.DisplayHDRTest, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayHDRTest = x.IsChecked;
			});
			AddSlider("Constant Luminance", 0.0001f, 2f, () => MyPostprocessSettingsWrapper.Settings.Data.ConstantLuminance, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.ConstantLuminance = f;
			});
			AddSlider("Exposure", -5f, 5f, () => MyPostprocessSettingsWrapper.Settings.Data.LuminanceExposure, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.LuminanceExposure = f;
			});
			AddSlider("White Point", MyPostprocessSettingsWrapper.Settings.Data.WhitePoint, 0f, 15f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.WhitePoint = x.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Grain Size", MyPostprocessSettingsWrapper.Settings.Data.GrainSize, 0f, 5f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.GrainSize = (int)x.Value;
			});
			AddSlider("Grain Amount", MyPostprocessSettingsWrapper.Settings.Data.GrainAmount, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.GrainAmount = x.Value;
			});
			AddSlider("Grain Strength", MyPostprocessSettingsWrapper.Settings.Data.GrainStrength, 0f, 0.5f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.GrainStrength = x.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Chromatic Factor", MyPostprocessSettingsWrapper.Settings.Data.ChromaticFactor, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.ChromaticFactor = x.Value;
			});
			AddSlider("Chromatic Iterations", MyPostprocessSettingsWrapper.Settings.ChromaticIterations, 1f, 15f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.ChromaticIterations = (int)x.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Vignette Start", MyPostprocessSettingsWrapper.Settings.Data.VignetteStart, 0f, 10f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.VignetteStart = x.Value;
			});
			AddSlider("Vignette Length", MyPostprocessSettingsWrapper.Settings.Data.VignetteLength, 0f, 10f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.VignetteLength = x.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Saturation", 0f, 5f, () => MyPostprocessSettingsWrapper.Settings.Data.Saturation, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.Saturation = f;
			});
			AddSlider("Brightness", 0f, 5f, () => MyPostprocessSettingsWrapper.Settings.Data.Brightness, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.Brightness = f;
			});
			AddSlider("Brightness Factor R", 0f, 1f, () => MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorR, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorR = f;
			});
			AddSlider("Brightness Factor G", 0f, 1f, () => MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorG, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorG = f;
			});
			AddSlider("Brightness Factor B", 0f, 1f, () => MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorB, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BrightnessFactorB = f;
			});
			AddSlider("Contrast", 0f, 2f, () => MyPostprocessSettingsWrapper.Settings.Data.Contrast, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.Contrast = f;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Vibrance", -1f, 1f, () => MyPostprocessSettingsWrapper.Settings.Data.Vibrance, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.Vibrance = f;
			});
			m_currentPosition.Y += 0.01f;
			AddLabel("Sepia", Color.Yellow.ToVector4(), 1.2f);
			AddColor("Light Color", MyPostprocessSettingsWrapper.Settings.Data.LightColor, delegate(MyGuiControlColor v)
			{
				MyPostprocessSettingsWrapper.Settings.Data.LightColor = v.Color;
			});
			AddColor("Dark Color", MyPostprocessSettingsWrapper.Settings.Data.DarkColor, delegate(MyGuiControlColor v)
			{
				MyPostprocessSettingsWrapper.Settings.Data.DarkColor = v.Color;
			});
			AddSlider("Sepia Strength", 0f, 1f, () => MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength = f;
			});
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderPostprocessTonemap";
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}
	}
}
