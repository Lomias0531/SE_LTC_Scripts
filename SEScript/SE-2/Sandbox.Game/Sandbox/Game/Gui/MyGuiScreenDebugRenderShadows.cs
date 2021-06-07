using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Render", "Shadows")]
	internal class MyGuiScreenDebugRenderShadows : MyGuiScreenDebugBase
	{
		private int m_selectedVolume;

		private MyGuiControlCheckbox m_checkboxHigherRange;

		private MyGuiControlSlider m_sliderFullCoveredDepth;

		private MyGuiControlSlider m_sliderExtCoveredDepth;

		private MyGuiControlSlider m_sliderShadowNormalOffset;

		public MyGuiScreenDebugRenderShadows()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			m_sliderDebugScale = 0.7f;
			AddCaption("Shadows", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f;
			AddLabel("Setup", Color.Yellow.ToVector4(), 1.2f);
			AddCheckBox("Enable Shadows", () => MyRenderProxy.Settings.EnableShadows, delegate(bool newValue)
			{
				MyRenderProxy.Settings.EnableShadows = newValue;
			});
			AddCheckBox("Enable Shadow Blur", () => MySector.ShadowSettings.Data.EnableShadowBlur, delegate(bool newValue)
			{
				MySector.ShadowSettings.Data.EnableShadowBlur = newValue;
			});
			AddCheckBox("Force per-frame updating", MySector.ShadowSettings.Data.UpdateCascadesEveryFrame, delegate(MyGuiControlCheckbox x)
			{
				MySector.ShadowSettings.Data.UpdateCascadesEveryFrame = x.IsChecked;
			});
			AddCheckBox("Shadow cascade usage based skip", MyRenderProxy.Settings.ShadowCascadeUsageBasedSkip, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.ShadowCascadeUsageBasedSkip = x.IsChecked;
			});
			AddCheckBox("Use Occlusion culling", !MyRenderProxy.Settings.DisableShadowCascadeOcclusionQueries, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisableShadowCascadeOcclusionQueries = !x.IsChecked;
			});
			AddSlider("Max base shadow cascade distance", MySector.ShadowSettings.Data.ShadowCascadeMaxDistance, 1f, 20000f, delegate(MyGuiControlSlider x)
			{
				MySector.ShadowSettings.Data.ShadowCascadeMaxDistance = x.Value;
			});
			AddSlider("Back offset", MySector.ShadowSettings.Data.ShadowCascadeZOffset, 1f, 50000f, delegate(MyGuiControlSlider x)
			{
				MySector.ShadowSettings.Data.ShadowCascadeZOffset = x.Value;
			});
			AddSlider("Spread factor", MySector.ShadowSettings.Data.ShadowCascadeSpreadFactor, 0f, 2f, delegate(MyGuiControlSlider x)
			{
				MySector.ShadowSettings.Data.ShadowCascadeSpreadFactor = x.Value;
			});
			AddSlider("LightDirectionChangeDelayMultiplier", MySector.ShadowSettings.Data.LightDirectionChangeDelayMultiplier, 0f, 180f, delegate(MyGuiControlSlider x)
			{
				MySector.ShadowSettings.Data.LightDirectionChangeDelayMultiplier = x.Value;
			});
			AddSlider("LightDirectionDifferenceThreshold", MySector.ShadowSettings.Data.LightDirectionDifferenceThreshold, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.ShadowSettings.Data.LightDirectionDifferenceThreshold = x.Value;
			});
			AddSlider("Small objects threshold (broken)", 0f, 0f, 1000f, OnChangeSmallObjectsThreshold);
			m_sliderShadowNormalOffset = AddSlider("Shadow normal offset", MySector.ShadowSettings.Cascades[m_selectedVolume].ShadowNormalOffset, 0f, 1f, delegate(MyGuiControlSlider x)
			{
				MySector.ShadowSettings.Cascades[m_selectedVolume].ShadowNormalOffset = x.Value;
			});
			MyGuiControlSlider myGuiControlSlider = AddSlider("ZBias", MySector.ShadowSettings.Data.ZBias, 0f, 0.02f, delegate(MyGuiControlSlider x)
			{
				MySector.ShadowSettings.Data.ZBias = x.Value;
			});
			myGuiControlSlider.LabelDecimalPlaces = 9;
			float zBias = MySector.ShadowSettings.Data.ZBias;
			myGuiControlSlider.Value = -1f;
			myGuiControlSlider.Value = zBias;
			AddLabel("Debug", Color.Yellow.ToVector4(), 1.2f);
			AddCheckBox("Show shadows", MyRenderProxy.Settings.DisplayShadowsWithDebug, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayShadowsWithDebug = x.IsChecked;
			});
			AddCheckBox("Show cascade splits", MyRenderProxy.Settings.DisplayShadowSplitsWithDebug, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayShadowSplitsWithDebug = x.IsChecked;
			});
			AddCheckBox("Show cascade splits for particles", MyRenderProxy.Settings.DisplayParticleShadowSplitsWithDebug, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayParticleShadowSplitsWithDebug = x.IsChecked;
			});
			AddCheckBox("Show cascade volumes", MyRenderProxy.Settings.DisplayShadowVolumes, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DisplayShadowVolumes = x.IsChecked;
			});
			AddCheckBox("Show cascade textures", MyRenderProxy.Settings.DrawCascadeShadowTextures, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawCascadeShadowTextures = x.IsChecked;
			});
			AddCheckBox("Show spot textures", MyRenderProxy.Settings.DrawSpotShadowTextures, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.DrawSpotShadowTextures = x.IsChecked;
			});
			AddSlider("Zoom to cascade texture", MyRenderProxy.Settings.ZoomCascadeTextureIndex, -1f, 8f, delegate(MyGuiControlSlider x)
			{
				MyRenderProxy.Settings.ZoomCascadeTextureIndex = (int)x.Value;
			});
			AddCheckBox("Freeze camera", MyRenderProxy.Settings.ShadowCameraFrozen, delegate(MyGuiControlCheckbox x)
			{
				MyRenderProxy.Settings.ShadowCameraFrozen = x.IsChecked;
			});
			for (int i = 0; i < MySector.ShadowSettings.Data.CascadesCount; i++)
			{
				int captureIndex = i;
				AddCheckBox("Freeze cascade " + i, MySector.ShadowSettings.ShadowCascadeFrozen[captureIndex], delegate(MyGuiControlCheckbox x)
				{
					bool[] shadowCascadeFrozen = MySector.ShadowSettings.ShadowCascadeFrozen;
					shadowCascadeFrozen[captureIndex] = x.IsChecked;
					MySector.ShadowSettings.ShadowCascadeFrozen = shadowCascadeFrozen;
				});
			}
		}

		private void OnChangeSmallObjectsThreshold(MyGuiControlSlider slider)
		{
			float value = slider.Value;
			for (int i = 0; i < MySector.ShadowSettings.Cascades.Length; i++)
			{
				MySector.ShadowSettings.Cascades[i].SkippingSmallObjectThreshold = value;
			}
		}

		private void SetSelectedVolume(float value)
		{
			int value2 = (int)Math.Floor(value);
			value2 = MathHelper.Clamp(value2, 0, MySector.ShadowSettings.Data.CascadesCount - 1);
			if (m_selectedVolume != value2)
			{
				m_selectedVolume = value2;
				MyShadowsSettings.Cascade cascade = MySector.ShadowSettings.Cascades[m_selectedVolume];
				m_checkboxHigherRange.IsChecked = true;
				m_sliderFullCoveredDepth.Value = cascade.FullCoverageDepth;
				m_sliderExtCoveredDepth.Value = cascade.ExtendedCoverageDepth;
				m_sliderShadowNormalOffset.Value = cascade.ShadowNormalOffset;
			}
		}

		private float GetSelectedVolume()
		{
			return m_selectedVolume;
		}

		protected override void ValueChanged(MyGuiControlBase sender)
		{
			MyRenderProxy.SetSettingsDirty();
			MyRenderProxy.UpdateShadowsSettings(MySector.ShadowSettings);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugRenderShadows";
		}
	}
}
