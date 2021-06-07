using Sandbox;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenOptionsGraphics : MyGuiScreenBase
	{
		private enum PresetEnum
		{
			Low,
			Medium,
			High,
			Custom
		}

		private static readonly MyPerformanceSettings[] m_presets;

		private bool m_writingSettings;

		private MyGuiControlCombobox m_comboAntialiasing;

		private MyGuiControlCombobox m_comboShadowMapResolution;

		private MyGuiControlCheckbox m_checkboxAmbientOcclusionHBAO;

		private MyGuiControlCheckbox m_checkboxPostProcessing;

		private MyGuiControlCombobox m_comboTextureQuality;

		private MyGuiControlCombobox m_comboShaderQuality;

		private MyGuiControlCombobox m_comboAnisotropicFiltering;

		private MyGuiControlCombobox m_comboGraphicsPresets;

		private MyGuiControlCombobox m_comboModelQuality;

		private MyGuiControlCombobox m_comboVoxelQuality;

		private MyGuiControlSliderBase m_vegetationViewDistance;

		private MyGuiControlSlider m_grassDensitySlider;

		private MyGuiControlSliderBase m_grassDrawDistanceSlider;

		private MyGuiControlSlider m_sliderFov;

		private MyGuiControlSlider m_sliderFlares;

		private MyGuiControlCheckbox m_checkboxEnableDamageEffects;

		private MyGraphicsSettings m_settingsOld;

		private MyGraphicsSettings m_settingsNew;

		private MyGuiControlElementGroup m_elementGroup;

		public MyGuiScreenOptionsGraphics()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.9379771f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			if (!constructor)
			{
				return;
			}
			base.RecreateControls(constructor);
			m_elementGroup = new MyGuiControlElementGroup();
			m_elementGroup.HighlightChanged += m_elementGroup_HighlightChanged;
			AddCaption(MyTexts.GetString(MyCommonTexts.ScreenCaptionGraphicsOptions), null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			MyGuiDrawAlignEnum originAlign2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
			Vector2 value = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 value2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			float num = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			float num2 = 25f;
			float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
			float num3 = 0.0015f;
			Vector2 value3 = new Vector2(0f, 0.008f);
			Vector2 value4 = new Vector2(0f, 0.045f);
			float num4 = 0f;
			Vector2 value5 = new Vector2(0.05f, 0f);
			Vector2 value6 = (m_size.Value / 2f - value) * new Vector2(-1f, -1f) + new Vector2(0f, y);
			Vector2 value7 = (m_size.Value / 2f - value) * new Vector2(1f, -1f) + new Vector2(0f, y);
			Vector2 value8 = (m_size.Value / 2f - value2) * new Vector2(0f, 1f);
			Vector2 value9 = new Vector2(value7.X - (num + num3), value7.Y);
			Vector2 value10 = value6 + new Vector2(0.255f, 0f);
			Vector2 value11 = value9 + new Vector2(0.26f, 0f);
			num4 -= 0.045f;
			MyGuiControlLabel control = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_comboGraphicsPresets = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_QualityPreset))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2
			};
			m_comboGraphicsPresets.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset_Low));
			m_comboGraphicsPresets.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset_Medium));
			m_comboGraphicsPresets.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset_High));
			m_comboGraphicsPresets.AddItem(3L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset_Custom));
			num4 += 1f;
			MyGuiControlLabel myGuiControlLabel = null;
			myGuiControlLabel = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ModelQuality))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_comboModelQuality = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_ModelQuality))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2
			};
			m_comboModelQuality.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Low));
			m_comboModelQuality.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Medium));
			m_comboModelQuality.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_High));
			m_comboModelQuality.AddItem(3L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Extreme) + " " + MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_PerformanceHeavy));
			num4 += 1f;
			MyGuiControlLabel control2 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.ScreenGraphicsOptions_ShaderQuality))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_comboShaderQuality = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_ShaderQuality))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2
			};
			m_comboShaderQuality.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Low));
			m_comboShaderQuality.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Medium));
			m_comboShaderQuality.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_High));
			num4 += 1f;
			MyGuiControlLabel control3 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.ScreenGraphicsOptions_VoxelQuality))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_comboVoxelQuality = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_VoxelQuality))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2
			};
			m_comboVoxelQuality.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Low));
			m_comboVoxelQuality.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Medium));
			m_comboVoxelQuality.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_High));
			m_comboVoxelQuality.AddItem(3L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Extreme) + " " + MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_PerformanceHeavy));
			num4 += 1f;
			MyGuiControlLabel control4 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_TextureQuality))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_comboTextureQuality = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_TextureQuality))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2
			};
			m_comboTextureQuality.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_TextureQuality_Low));
			m_comboTextureQuality.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_TextureQuality_Medium));
			m_comboTextureQuality.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_TextureQuality_High));
			num4 += 1f;
			MyGuiControlLabel control5 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.ScreenGraphicsOptions_ShadowMapResolution))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_comboShadowMapResolution = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_ShadowQuality))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2
			};
			m_comboShadowMapResolution.AddItem(3L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ShadowMapResolution_Disabled));
			m_comboShadowMapResolution.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ShadowMapResolution_Low));
			m_comboShadowMapResolution.AddItem(1L, MyTexts.GetString(MySpaceTexts.ScreenGraphicsOptions_ShadowMapResolution_Medium));
			m_comboShadowMapResolution.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ShadowMapResolution_High));
			m_comboShadowMapResolution.AddItem(4L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ShadowMapResolution_Extreme) + " " + MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_PerformanceHeavy));
			num4 += 1f;
			MyGuiControlLabel control6 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AntiAliasing))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_comboAntialiasing = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_Antialiasing))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2
			};
			m_comboAntialiasing.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AntiAliasing_None));
			m_comboAntialiasing.AddItem(1L, "FXAA");
			num4 += 1f;
			MyGuiControlLabel control7 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AnisotropicFiltering))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_comboAnisotropicFiltering = new MyGuiControlCombobox(null, null, null, null, 10, null, useScrollBarOffset: false, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_AnisotropicFiltering))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2
			};
			m_comboAnisotropicFiltering.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AnisotropicFiltering_Off));
			m_comboAnisotropicFiltering.AddItem(1L, "1x");
			m_comboAnisotropicFiltering.AddItem(2L, "4x");
			m_comboAnisotropicFiltering.AddItem(3L, "8x");
			m_comboAnisotropicFiltering.AddItem(4L, "16x");
			num4 += 1.05f;
			MyGuiControlLabel control8 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.FieldOfView))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			MyVideoSettingsManager.GetFovBounds(out float minRadians, out float maxRadians);
			if (!MySandboxGame.Config.ExperimentalMode)
			{
				maxRadians = Math.Min(maxRadians, MyConstants.FIELD_OF_VIEW_CONFIG_MAX_SAFE);
			}
			m_sliderFov = new MyGuiControlSlider(null, toolTip: MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsFieldOfView), minValue: MathHelper.ToDegrees(minRadians), maxValue: MathHelper.ToDegrees(maxRadians), width: 0.29f, labelText: new StringBuilder("{0}").ToString(), defaultValue: MathHelper.ToDegrees(MySandboxGame.Config.FieldOfView), color: null, labelDecimalPlaces: 1, labelScale: 0.8f, labelSpaceWidth: 0.07f, labelFont: "Blue", visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: false, showLabel: true)
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_FOV));
			stringBuilder.Append(" ");
			stringBuilder.AppendFormat(MyCommonTexts.DefaultFOV, MathHelper.ToDegrees(MyConstants.FIELD_OF_VIEW_CONFIG_DEFAULT));
			m_sliderFov.SetToolTip(stringBuilder.ToString());
			num4 += 1.1f;
			MyGuiControlLabel control9 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.FlaresIntensity))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_sliderFlares = new MyGuiControlSlider(null, 0.1f, 2f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipFlaresIntensity), labelText: new StringBuilder("{0}").ToString(), defaultValue: MySandboxGame.Config.FlaresIntensity, color: null, labelDecimalPlaces: 1, labelScale: 0.8f, labelSpaceWidth: 0.07f, labelFont: "Blue", visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: false, showLabel: true)
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			num4 += 1.1f;
			MyGuiControlLabel control10 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.WorldSettings_GrassDrawDistance))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_grassDrawDistanceSlider = new MyGuiControlSliderBase(null, 0.29f, new MyGuiSliderPropertiesExponential(50f, 5000f, 10f, integer: true), null, null, 0.8f, 0.07f, "Blue", MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_GrassDrawDistance))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			num4 += 1.1f;
			MyGuiControlLabel control11 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.WorldSettings_GrassDensity))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_grassDensitySlider = new MyGuiControlSlider(null, 0f, 10f, 0.29f, labelText: new StringBuilder("{0}").ToString(), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_GrassDensity), defaultValue: MySandboxGame.Config.GrassDensityFactor, color: null, labelDecimalPlaces: 1, labelScale: 0.8f, labelSpaceWidth: 0.07f, labelFont: "Blue", visualStyle: MyGuiControlSliderStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, intValue: false, showLabel: true)
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			m_grassDensitySlider.SetBounds(0f, 10f);
			num4 += 1.1f;
			MyGuiControlLabel control12 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.WorldSettings_VegetationDistance))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_vegetationViewDistance = new MyGuiControlSliderBase(null, 0.29f, new MyGuiSliderPropertiesExponential(500f, 10000f, 10f, integer: true), null, null, 0.8f, 0.07f, "Blue", MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_TreeDrawDistance))
			{
				Position = value7 + num4 * value4,
				OriginAlign = originAlign2,
				Size = new Vector2(num, 0f)
			};
			num4 += 1.1f;
			MyGuiControlLabel control13 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AmbientOcclusion))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_checkboxAmbientOcclusionHBAO = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_AmbientOcclusion))
			{
				Position = value10 + num4 * value4,
				OriginAlign = originAlign
			};
			MyGuiControlLabel control14 = new MyGuiControlLabel(null, null, MyTexts.GetString(MySpaceTexts.EnableDamageEffects))
			{
				Position = value9 + value5 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_checkboxEnableDamageEffects = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipVideoOptionsEnableDamageEffects))
			{
				Position = value11 + num4 * value4,
				OriginAlign = originAlign
			};
			num4 += 1f;
			MyGuiControlLabel control15 = new MyGuiControlLabel(null, null, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_PostProcessing))
			{
				Position = value6 + num4 * value4 + value3,
				OriginAlign = originAlign
			};
			m_checkboxPostProcessing = new MyGuiControlCheckbox(null, null, MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_PostProcessing))
			{
				Position = value10 + num4 * value4,
				OriginAlign = originAlign,
				IsChecked = MySandboxGame.Config.PostProcessingEnabled
			};
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkClick);
			myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
			MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelClick);
			myGuiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			myGuiControlButton.Position = value8 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			myGuiControlButton2.Position = value8 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			myGuiControlButton2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			Controls.Add(control9);
			Controls.Add(m_sliderFlares);
			Controls.Add(control8);
			Controls.Add(m_sliderFov);
			if (MyVideoSettingsManager.RunningGraphicsRenderer == MySandboxGame.DirectX11RendererKey)
			{
				Controls.Add(control);
				Controls.Add(m_comboGraphicsPresets);
				Controls.Add(control6);
				Controls.Add(m_comboAntialiasing);
				Controls.Add(control5);
				Controls.Add(m_comboShadowMapResolution);
				Controls.Add(control4);
				Controls.Add(m_comboTextureQuality);
				Controls.Add(myGuiControlLabel);
				Controls.Add(m_comboModelQuality);
				Controls.Add(control2);
				Controls.Add(m_comboShaderQuality);
				Controls.Add(control3);
				Controls.Add(m_comboVoxelQuality);
				Controls.Add(control7);
				Controls.Add(m_comboAnisotropicFiltering);
				if (MyFakes.ENABLE_PLANETS || MyInput.Static.ENABLE_DEVELOPER_KEYS)
				{
					Controls.Add(control10);
					Controls.Add(m_grassDrawDistanceSlider);
					Controls.Add(control11);
					Controls.Add(m_grassDensitySlider);
					Controls.Add(control12);
					Controls.Add(m_vegetationViewDistance);
				}
				Controls.Add(control14);
				Controls.Add(m_checkboxEnableDamageEffects);
				Controls.Add(control13);
				Controls.Add(m_checkboxAmbientOcclusionHBAO);
				Controls.Add(control15);
				Controls.Add(m_checkboxPostProcessing);
			}
			Controls.Add(myGuiControlButton);
			m_elementGroup.Add(myGuiControlButton);
			Controls.Add(myGuiControlButton2);
			m_elementGroup.Add(myGuiControlButton2);
			m_settingsOld = MyVideoSettingsManager.CurrentGraphicsSettings;
			m_settingsNew = m_settingsOld;
			WriteSettingsToControls(m_settingsOld);
			ReadSettingsFromControls(ref m_settingsOld);
			ReadSettingsFromControls(ref m_settingsNew);
			MyGuiControlCombobox.ItemSelectedDelegate value12 = OnSettingsChanged;
			Action<MyGuiControlCheckbox> action = delegate
			{
				OnSettingsChanged();
			};
			m_comboGraphicsPresets.ItemSelected += OnPresetSelected;
			m_comboAnisotropicFiltering.ItemSelected += value12;
			m_comboAntialiasing.ItemSelected += value12;
			m_comboShadowMapResolution.ItemSelected += value12;
			MyGuiControlCheckbox checkboxAmbientOcclusionHBAO = m_checkboxAmbientOcclusionHBAO;
			checkboxAmbientOcclusionHBAO.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkboxAmbientOcclusionHBAO.IsCheckedChanged, action);
			m_comboVoxelQuality.ItemSelected += value12;
			m_comboModelQuality.ItemSelected += value12;
			m_comboTextureQuality.ItemSelected += value12;
			m_comboShaderQuality.ItemSelected += value12;
			m_sliderFlares.ValueChanged = delegate
			{
				OnSettingsChanged();
			};
			m_checkboxEnableDamageEffects.IsCheckedChanged = action;
			m_sliderFov.ValueChanged = delegate
			{
				OnSettingsChanged();
			};
			MyGuiControlCheckbox checkboxPostProcessing = m_checkboxPostProcessing;
			checkboxPostProcessing.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkboxPostProcessing.IsCheckedChanged, action);
			RefreshPresetCombo(m_settingsOld);
			base.FocusedControl = myGuiControlButton;
			base.CloseButtonEnabled = true;
		}

		private void m_elementGroup_HighlightChanged(MyGuiControlElementGroup obj)
		{
			foreach (MyGuiControlBase item in m_elementGroup)
			{
				if (item.HasFocus && obj.SelectedElement != item)
				{
					base.FocusedControl = obj.SelectedElement;
					break;
				}
			}
		}

		private void RefreshPresetCombo(MyGraphicsSettings settings)
		{
			int i;
			for (i = 0; i < m_presets.Length; i++)
			{
				MyPerformanceSettings myPerformanceSettings = m_presets[i];
				if (myPerformanceSettings.Equals(settings.PerformanceSettings))
				{
					break;
				}
			}
			if (i >= 3)
			{
				m_comboGraphicsPresets.SelectItemByKey(3L);
			}
			else
			{
				m_comboGraphicsPresets.SelectItemByKey(i, sendEvent: false);
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenOptionsVideo";
		}

		private void OnPresetSelected()
		{
			PresetEnum presetEnum = (PresetEnum)m_comboGraphicsPresets.GetSelectedKey();
			if (presetEnum != PresetEnum.Custom)
			{
				m_settingsNew.PerformanceSettings = m_presets[(int)presetEnum];
				WriteSettingsToControls(m_settingsNew);
			}
		}

		private void OnSettingsChanged()
		{
			m_comboGraphicsPresets.SelectItemByKey(3L);
			ReadSettingsFromControls(ref m_settingsNew);
			RefreshPresetCombo(m_settingsNew);
		}

		/// <returns>Bool indicating a game restart is required</returns>
		private bool ReadSettingsFromControls(ref MyGraphicsSettings graphicsSettings)
		{
			if (m_writingSettings)
			{
				return false;
			}
			MyGraphicsSettings myGraphicsSettings = default(MyGraphicsSettings);
			myGraphicsSettings.GraphicsRenderer = graphicsSettings.GraphicsRenderer;
			myGraphicsSettings.FieldOfView = MathHelper.ToRadians(m_sliderFov.Value);
			myGraphicsSettings.PostProcessingEnabled = m_checkboxPostProcessing.IsChecked;
			myGraphicsSettings.FlaresIntensity = m_sliderFlares.Value;
			myGraphicsSettings.PerformanceSettings = new MyPerformanceSettings
			{
				EnableDamageEffects = m_checkboxEnableDamageEffects.IsChecked,
				RenderSettings = 
				{
					AntialiasingMode = (MyAntialiasingMode)m_comboAntialiasing.GetSelectedKey(),
					AmbientOcclusionEnabled = m_checkboxAmbientOcclusionHBAO.IsChecked,
					ShadowQuality = (MyShadowsQuality)m_comboShadowMapResolution.GetSelectedKey(),
					TextureQuality = (MyTextureQuality)m_comboTextureQuality.GetSelectedKey(),
					AnisotropicFiltering = (MyTextureAnisoFiltering)m_comboAnisotropicFiltering.GetSelectedKey(),
					ModelQuality = (MyRenderQualityEnum)m_comboModelQuality.GetSelectedKey(),
					VoxelQuality = (MyRenderQualityEnum)m_comboVoxelQuality.GetSelectedKey(),
					GrassDrawDistance = m_grassDrawDistanceSlider.Value,
					GrassDensityFactor = m_grassDensitySlider.Value,
					VoxelShaderQuality = (MyRenderQualityEnum)m_comboShaderQuality.GetSelectedKey(),
					AlphaMaskedShaderQuality = (MyRenderQualityEnum)m_comboShaderQuality.GetSelectedKey(),
					AtmosphereShaderQuality = (MyRenderQualityEnum)m_comboShaderQuality.GetSelectedKey(),
					HqDepth = true,
					DistanceFade = m_vegetationViewDistance.Value
				}
			};
			MyGraphicsSettings myGraphicsSettings2 = myGraphicsSettings;
			bool result = myGraphicsSettings2.GraphicsRenderer != graphicsSettings.GraphicsRenderer;
			graphicsSettings = myGraphicsSettings2;
			return result;
		}

		private void WriteSettingsToControls(MyGraphicsSettings graphicsSettings)
		{
			m_writingSettings = true;
			m_sliderFlares.Value = graphicsSettings.FlaresIntensity;
			m_sliderFov.Value = MathHelper.ToDegrees(graphicsSettings.FieldOfView);
			m_checkboxPostProcessing.IsChecked = graphicsSettings.PostProcessingEnabled;
			m_comboModelQuality.SelectItemByKey((long)graphicsSettings.PerformanceSettings.RenderSettings.ModelQuality, sendEvent: false);
			m_comboVoxelQuality.SelectItemByKey((long)graphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality, sendEvent: false);
			m_grassDrawDistanceSlider.Value = graphicsSettings.PerformanceSettings.RenderSettings.GrassDrawDistance;
			m_grassDensitySlider.Value = graphicsSettings.PerformanceSettings.RenderSettings.GrassDensityFactor;
			m_vegetationViewDistance.Value = graphicsSettings.PerformanceSettings.RenderSettings.DistanceFade;
			m_checkboxEnableDamageEffects.IsChecked = graphicsSettings.PerformanceSettings.EnableDamageEffects;
			m_comboAntialiasing.SelectItemByKey((long)graphicsSettings.PerformanceSettings.RenderSettings.AntialiasingMode, sendEvent: false);
			m_checkboxAmbientOcclusionHBAO.IsChecked = graphicsSettings.PerformanceSettings.RenderSettings.AmbientOcclusionEnabled;
			m_comboShadowMapResolution.SelectItemByKey((long)graphicsSettings.PerformanceSettings.RenderSettings.ShadowQuality, sendEvent: false);
			m_comboTextureQuality.SelectItemByKey((long)graphicsSettings.PerformanceSettings.RenderSettings.TextureQuality, sendEvent: false);
			m_comboShaderQuality.SelectItemByKey((long)graphicsSettings.PerformanceSettings.RenderSettings.VoxelShaderQuality, sendEvent: false);
			m_comboAnisotropicFiltering.SelectItemByKey((long)graphicsSettings.PerformanceSettings.RenderSettings.AnisotropicFiltering, sendEvent: false);
			m_comboShaderQuality.SelectItemByKey((long)graphicsSettings.PerformanceSettings.RenderSettings.VoxelShaderQuality, sendEvent: false);
			m_writingSettings = false;
		}

		public void OnCancelClick(MyGuiControlButton sender)
		{
			MyVideoSettingsManager.Apply(m_settingsOld);
			MyVideoSettingsManager.SaveCurrentSettings();
			CloseScreen();
		}

		public void OnOkClick(MyGuiControlButton sender)
		{
			if (ReadSettingsFromControls(ref m_settingsNew))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.MessageBoxTextRestartNeededAfterRendererSwitch), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
			}
			MyVideoSettingsManager.Apply(m_settingsNew);
			MyVideoSettingsManager.SaveCurrentSettings();
			CloseScreen();
		}

		public static MyPerformanceSettings GetPreset(MyRenderQualityEnum adapterQuality)
		{
			return m_presets[(int)adapterQuality];
		}

		static MyGuiScreenOptionsGraphics()
		{
			MyPerformanceSettings[] array = new MyPerformanceSettings[5];
			MyPerformanceSettings myPerformanceSettings = default(MyPerformanceSettings);
			MyRenderSettings1 renderSettings = new MyRenderSettings1
			{
				AnisotropicFiltering = MyTextureAnisoFiltering.NONE,
				AntialiasingMode = MyAntialiasingMode.NONE,
				ShadowQuality = MyShadowsQuality.LOW,
				AmbientOcclusionEnabled = false,
				TextureQuality = MyTextureQuality.LOW,
				ModelQuality = MyRenderQualityEnum.LOW,
				VoxelQuality = MyRenderQualityEnum.LOW,
				GrassDrawDistance = 50f,
				GrassDensityFactor = 0f,
				HqDepth = true,
				VoxelShaderQuality = MyRenderQualityEnum.LOW,
				AlphaMaskedShaderQuality = MyRenderQualityEnum.LOW,
				AtmosphereShaderQuality = MyRenderQualityEnum.LOW,
				DistanceFade = 500f
			};
			myPerformanceSettings.RenderSettings = renderSettings;
			myPerformanceSettings.EnableDamageEffects = false;
			array[0] = myPerformanceSettings;
			myPerformanceSettings = default(MyPerformanceSettings);
			renderSettings = new MyRenderSettings1
			{
				AnisotropicFiltering = MyTextureAnisoFiltering.NONE,
				AntialiasingMode = MyAntialiasingMode.FXAA,
				ShadowQuality = MyShadowsQuality.MEDIUM,
				AmbientOcclusionEnabled = true,
				TextureQuality = MyTextureQuality.MEDIUM,
				ModelQuality = MyRenderQualityEnum.NORMAL,
				VoxelQuality = MyRenderQualityEnum.NORMAL,
				GrassDrawDistance = 160f,
				GrassDensityFactor = 1f,
				HqDepth = true,
				VoxelShaderQuality = MyRenderQualityEnum.NORMAL,
				AlphaMaskedShaderQuality = MyRenderQualityEnum.NORMAL,
				AtmosphereShaderQuality = MyRenderQualityEnum.NORMAL,
				DistanceFade = 1000f
			};
			myPerformanceSettings.RenderSettings = renderSettings;
			myPerformanceSettings.EnableDamageEffects = true;
			array[1] = myPerformanceSettings;
			myPerformanceSettings = default(MyPerformanceSettings);
			renderSettings = new MyRenderSettings1
			{
				AnisotropicFiltering = MyTextureAnisoFiltering.ANISO_16,
				AntialiasingMode = MyAntialiasingMode.FXAA,
				ShadowQuality = MyShadowsQuality.HIGH,
				AmbientOcclusionEnabled = true,
				TextureQuality = MyTextureQuality.HIGH,
				ModelQuality = MyRenderQualityEnum.HIGH,
				VoxelQuality = MyRenderQualityEnum.HIGH,
				GrassDrawDistance = 1000f,
				GrassDensityFactor = 3f,
				HqDepth = true,
				VoxelShaderQuality = MyRenderQualityEnum.HIGH,
				AlphaMaskedShaderQuality = MyRenderQualityEnum.HIGH,
				AtmosphereShaderQuality = MyRenderQualityEnum.HIGH,
				DistanceFade = 2000f
			};
			myPerformanceSettings.RenderSettings = renderSettings;
			myPerformanceSettings.EnableDamageEffects = true;
			array[2] = myPerformanceSettings;
			myPerformanceSettings = default(MyPerformanceSettings);
			renderSettings = new MyRenderSettings1
			{
				AnisotropicFiltering = MyTextureAnisoFiltering.ANISO_16,
				AntialiasingMode = MyAntialiasingMode.FXAA,
				ShadowQuality = MyShadowsQuality.EXTREME,
				AmbientOcclusionEnabled = true,
				TextureQuality = MyTextureQuality.HIGH,
				ModelQuality = MyRenderQualityEnum.EXTREME,
				VoxelQuality = MyRenderQualityEnum.EXTREME,
				GrassDrawDistance = 1000f,
				GrassDensityFactor = 3f,
				HqDepth = true,
				VoxelShaderQuality = MyRenderQualityEnum.EXTREME,
				AlphaMaskedShaderQuality = MyRenderQualityEnum.HIGH,
				AtmosphereShaderQuality = MyRenderQualityEnum.HIGH,
				DistanceFade = 2000f
			};
			myPerformanceSettings.RenderSettings = renderSettings;
			myPerformanceSettings.EnableDamageEffects = true;
			array[3] = myPerformanceSettings;
			myPerformanceSettings = default(MyPerformanceSettings);
			renderSettings = new MyRenderSettings1
			{
				AnisotropicFiltering = MyTextureAnisoFiltering.ANISO_4,
				AntialiasingMode = MyAntialiasingMode.FXAA,
				ShadowQuality = MyShadowsQuality.MEDIUM,
				AmbientOcclusionEnabled = true,
				TextureQuality = MyTextureQuality.MEDIUM,
				ModelQuality = MyRenderQualityEnum.NORMAL,
				VoxelQuality = MyRenderQualityEnum.NORMAL,
				GrassDrawDistance = 600f,
				GrassDensityFactor = 1f,
				HqDepth = true,
				VoxelShaderQuality = MyRenderQualityEnum.HIGH,
				AlphaMaskedShaderQuality = MyRenderQualityEnum.NORMAL,
				AtmosphereShaderQuality = MyRenderQualityEnum.HIGH,
				DistanceFade = 1500f
			};
			myPerformanceSettings.RenderSettings = renderSettings;
			myPerformanceSettings.EnableDamageEffects = true;
			array[4] = myPerformanceSettings;
			m_presets = array;
		}
	}
}
