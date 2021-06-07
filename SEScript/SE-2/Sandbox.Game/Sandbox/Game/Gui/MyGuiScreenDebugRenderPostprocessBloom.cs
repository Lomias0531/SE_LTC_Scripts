using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Postprocess Bloom")]
	internal class MyGuiScreenDebugRenderPostprocessBloom : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugRenderPostprocessBloom()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Postprocess Bloom", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddLabel("Bloom", Color.Yellow.ToVector4(), 1.2f);
			AddCheckBox("Enabled", MyPostprocessSettingsWrapper.Settings.BloomEnabled, delegate(MyGuiControlCheckbox x)
			{
				MyPostprocessSettingsWrapper.Settings.BloomEnabled = x.IsChecked;
			});
			AddCheckBox("Display filter", MyRenderProxy.Settings.DisplayBloomFilter, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayBloomFilter = x.IsChecked;
			});
			AddCheckBox("Display min", MyRenderProxy.Settings.DisplayBloomMin, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayBloomMin = x.IsChecked;
			});
			AddSlider("Exposure", 0f, 10f, () => MyPostprocessSettingsWrapper.Settings.Data.BloomExposure, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BloomExposure = f;
			});
			AddSlider("Luma threshold", 0f, 100f, () => MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BloomLumaThreshold = f;
			});
			AddSlider("Emissiveness", 0f, 400f, () => MyPostprocessSettingsWrapper.Settings.Data.BloomEmissiveness, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BloomEmissiveness = f;
			});
			AddSlider("Size", 0f, 10f, () => MyPostprocessSettingsWrapper.Settings.BloomSize, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.BloomSize = (int)f;
			});
			AddSlider("Depth slope", 0f, 5f, () => MyPostprocessSettingsWrapper.Settings.Data.BloomDepthSlope, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BloomDepthSlope = f;
			});
			AddSlider("Depth strength", 0f, 4f, () => MyPostprocessSettingsWrapper.Settings.Data.BloomDepthStrength, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BloomDepthStrength = f;
			});
			AddSlider("Dirt/Bloom Ratio", MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BloomDirtRatio = x.Value;
			});
			AddSlider("Magnitude", 0f, 0.1f, () => MyPostprocessSettingsWrapper.Settings.Data.BloomMult, delegate(float f)
			{
				MyPostprocessSettingsWrapper.Settings.Data.BloomMult = f;
			});
			AddCheckBox("High Quality Bloom", MyPostprocessSettingsWrapper.Settings.HighQualityBloom, delegate(MyGuiControlCheckbox x)
			{
				MyPostprocessSettingsWrapper.Settings.HighQualityBloom = x.IsChecked;
			});
			AddCheckBox("AntiFlicker Filter", MyPostprocessSettingsWrapper.Settings.BloomAntiFlickerFilter, delegate(MyGuiControlCheckbox x)
			{
				MyPostprocessSettingsWrapper.Settings.BloomAntiFlickerFilter = x.IsChecked;
			});
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderPostprocessBloom";
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
			MyRenderProxy.UpdateDebugOverrides();
		}
	}
}
