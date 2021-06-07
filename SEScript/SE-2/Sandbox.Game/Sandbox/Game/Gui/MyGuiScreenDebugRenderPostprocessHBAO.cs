using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Postprocess HBAO")]
	internal class MyGuiScreenDebugRenderPostprocessHBAO : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugRenderPostprocessHBAO()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Postprocess HBAO", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			AddCheckBox("Use HBAO", MySector.HBAOSettings.Enabled, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.Enabled = state.IsChecked;
			});
			AddCheckBox("Show only HBAO", MyRenderProxy.Settings.DisplayAO, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayAO = x.IsChecked;
			});
			AddCheckBox("Show Normals", MyRenderProxy.Settings.DisplayNormals, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayNormals = x.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddSlider("Radius", MySector.HBAOSettings.Radius, 0f, 5f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.Radius = state.Value;
			});
			AddSlider("Bias", MySector.HBAOSettings.Bias, 0f, 0.5f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.Bias = state.Value;
			});
			AddSlider("SmallScaleAO", MySector.HBAOSettings.SmallScaleAO, 0f, 4f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.SmallScaleAO = state.Value;
			});
			AddSlider("LargeScaleAO", MySector.HBAOSettings.LargeScaleAO, 0f, 4f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.LargeScaleAO = state.Value;
			});
			AddSlider("PowerExponent", MySector.HBAOSettings.PowerExponent, 1f, 8f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.PowerExponent = state.Value;
			});
			AddCheckBox("Use Normals", MySector.HBAOSettings.UseGBufferNormals, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.UseGBufferNormals = state.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddCheckBox("ForegroundAOEnable", MySector.HBAOSettings.ForegroundAOEnable, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.ForegroundAOEnable = state.IsChecked;
			});
			AddSlider("ForegroundViewDepth", MySector.HBAOSettings.ForegroundViewDepth, 0f, 1000f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.ForegroundViewDepth = state.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddCheckBox("BackgroundAOEnable", MySector.HBAOSettings.BackgroundAOEnable, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.BackgroundAOEnable = state.IsChecked;
			});
			AddCheckBox("AdaptToFOV", MySector.HBAOSettings.AdaptToFOV, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.AdaptToFOV = state.IsChecked;
			});
			AddSlider("BackgroundViewDepth", MySector.HBAOSettings.BackgroundViewDepth, 0f, 1000f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.BackgroundViewDepth = state.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddCheckBox("DepthClampToEdge", MySector.HBAOSettings.DepthClampToEdge, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.DepthClampToEdge = state.IsChecked;
			});
			m_currentPosition.Y += 0.01f;
			AddCheckBox("DepthThresholdEnable", MySector.HBAOSettings.DepthThresholdEnable, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.DepthThresholdEnable = state.IsChecked;
			});
			AddSlider("DepthThreshold", MySector.HBAOSettings.DepthThreshold, 0f, 1000f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.DepthThreshold = state.Value;
			});
			AddSlider("DepthThresholdSharpness", MySector.HBAOSettings.DepthThresholdSharpness, 0f, 500f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.DepthThresholdSharpness = state.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Use blur", MySector.HBAOSettings.BlurEnable, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.BlurEnable = state.IsChecked;
			});
			AddCheckBox("Radius 4", MySector.HBAOSettings.BlurRadius4, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.BlurRadius4 = state.IsChecked;
			});
			AddSlider("Sharpness", MySector.HBAOSettings.BlurSharpness, 0f, 100f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.BlurSharpness = state.Value;
			});
			m_currentPosition.Y += 0.01f;
			AddCheckBox("Blur Sharpness Function", MySector.HBAOSettings.BlurSharpnessFunctionEnable, delegate(MyGuiControlCheckbox state)
			{
				MySector.HBAOSettings.BlurSharpnessFunctionEnable = state.IsChecked;
			});
			AddSlider("ForegroundScale", MySector.HBAOSettings.BlurSharpnessFunctionForegroundScale, 0f, 100f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.BlurSharpnessFunctionForegroundScale = state.Value;
			});
			AddSlider("ForegroundViewDepth", MySector.HBAOSettings.BlurSharpnessFunctionForegroundViewDepth, 0f, 1f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.BlurSharpnessFunctionForegroundViewDepth = state.Value;
			});
			AddSlider("BackgroundViewDepth", MySector.HBAOSettings.BlurSharpnessFunctionBackgroundViewDepth, 0f, 1f, delegate(MyGuiControlSlider state)
			{
				MySector.HBAOSettings.BlurSharpnessFunctionBackgroundViewDepth = state.Value;
			});
			m_currentPosition.Y += 0.01f;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderPostprocessHBAO";
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
		}
	}
}
