using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenWardrobe : MyGuiScreenBase
	{
		public struct MyCameraControllerSettings
		{
			public double Distance;

			public MyCameraControllerEnum Controller;
		}

		private const string m_hueScaleTexture = "Textures\\GUI\\HueScale.png";

		private MyGuiControlCombobox m_modelPicker;

		private MyGuiControlSlider m_sliderHue;

		private MyGuiControlSlider m_sliderSaturation;

		private MyGuiControlSlider m_sliderValue;

		private MyGuiControlLabel m_labelHue;

		private MyGuiControlLabel m_labelSaturation;

		private MyGuiControlLabel m_labelValue;

		private string m_selectedModel;

		private Vector3 m_selectedHSV;

		private MyCharacter m_user;

		private Dictionary<string, int> m_displayModels;

		private Dictionary<int, string> m_models;

		private string m_storedModel;

		private Vector3 m_storedHSV;

		private MyCameraControllerSettings m_storedCamera;

		private bool m_colorOrModelChanged;

		public static event MyWardrobeChangeDelegate LookChanged;

		public MyGuiScreenWardrobe(MyCharacter user, HashSet<string> customCharacterNames = null)
			: base(size: new Vector2(0.31f, 0.55f), position: MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP), backgroundColor: MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false, backgroundTexture: MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture)
		{
			base.EnabledBackgroundFade = false;
			m_user = user;
			m_storedModel = m_user.ModelName;
			m_storedHSV = m_user.ColorMask;
			m_selectedModel = GetDisplayName(m_user.ModelName);
			m_selectedHSV = m_storedHSV;
			m_displayModels = new Dictionary<string, int>();
			m_models = new Dictionary<int, string>();
			int value = 0;
			if (customCharacterNames == null)
			{
				foreach (MyCharacterDefinition character in MyDefinitionManager.Static.Characters)
				{
					if ((!MySession.Static.SurvivalMode || character.UsableByPlayer) && character.Public)
					{
						string displayName = GetDisplayName(character.Name);
						m_displayModels[displayName] = value;
						m_models[value++] = character.Name;
					}
				}
			}
			else
			{
				DictionaryValuesReader<string, MyCharacterDefinition> characters = MyDefinitionManager.Static.Characters;
				foreach (string customCharacterName in customCharacterNames)
				{
					if (characters.TryGetValue(customCharacterName, out MyCharacterDefinition result) && (!MySession.Static.SurvivalMode || result.UsableByPlayer) && result.Public)
					{
						string displayName2 = GetDisplayName(result.Name);
						m_displayModels[displayName2] = value;
						m_models[value++] = result.Name;
					}
				}
			}
			RecreateControls(constructor: true);
			m_sliderHue.Value = m_selectedHSV.X * 360f;
			m_sliderSaturation.Value = MathHelper.Clamp(m_selectedHSV.Y + MyColorPickerConstants.SATURATION_DELTA, 0f, 1f);
			m_sliderValue.Value = MathHelper.Clamp(m_selectedHSV.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0f, 1f);
			MyGuiControlSlider sliderHue = m_sliderHue;
			sliderHue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderHue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderSaturation = m_sliderSaturation;
			sliderSaturation.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderSaturation.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderValue = m_sliderValue;
			sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			ChangeCamera();
			UpdateLabels();
		}

		private string GetDisplayName(string name)
		{
			return MyTexts.GetString(name);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenWardrobe";
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.USE))
			{
				ChangeCameraBack();
				CloseScreen();
			}
			base.HandleInput(receivedFocusInThisUpdate);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			MyGuiControlLabel myGuiControlLabel = AddCaption(MyCommonTexts.PlayerCharacterModel);
			Vector2 itemSize = MyGuiControlListbox.GetVisualStyle(MyGuiControlListboxStyleEnum.Default).ItemSize;
			float num = -0.19f;
			m_modelPicker = new MyGuiControlCombobox(new Vector2(0f, num));
			foreach (KeyValuePair<string, int> displayModel in m_displayModels)
			{
				m_modelPicker.AddItem(displayModel.Value, new StringBuilder(displayModel.Key));
			}
			if (m_displayModels.ContainsKey(m_selectedModel))
			{
				m_modelPicker.SelectItemByKey(m_displayModels[m_selectedModel]);
			}
			else if (m_displayModels.Count > 0)
			{
				m_modelPicker.SelectItemByKey(m_displayModels.First().Value);
			}
			m_modelPicker.ItemSelected += OnItemSelected;
			num += 0.045f;
			Vector2 vector = itemSize + myGuiControlLabel.Size;
			m_position.X -= vector.X / 2.5f;
			m_position.Y += vector.Y * 3.6f;
			Controls.Add(new MyGuiControlLabel(new Vector2(0f, num), null, MyTexts.GetString(MyCommonTexts.PlayerCharacterColor), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			num += 0.04f;
			Controls.Add(new MyGuiControlLabel(new Vector2(-0.135f, num), null, MyTexts.GetString(MyCommonTexts.ScreenWardrobeOld_Hue)));
			m_labelHue = new MyGuiControlLabel(new Vector2(0.09f, num), null, string.Empty);
			num += 0.035f;
			m_sliderHue = new MyGuiControlSlider(new Vector2(-0.135f, num), 0f, 360f, 0.3f, null, null, null, 0, 0.8f, 0.0416666679f, "White", null, MyGuiControlSliderStyleEnum.Hue, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, intValue: true);
			num += 0.045f;
			Controls.Add(new MyGuiControlLabel(new Vector2(-0.135f, num), null, MyTexts.GetString(MyCommonTexts.ScreenWardrobeOld_Saturation)));
			m_labelSaturation = new MyGuiControlLabel(new Vector2(0.09f, num), null, string.Empty);
			num += 0.035f;
			m_sliderSaturation = new MyGuiControlSlider(new Vector2(-0.135f, num), 0f, 1f, 0.3f, 0f, null, null, 1, 0.8f, 0.0416666679f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			num += 0.045f;
			Controls.Add(new MyGuiControlLabel(new Vector2(-0.135f, num), null, MyTexts.GetString(MyCommonTexts.ScreenWardrobeOld_Value)));
			m_labelValue = new MyGuiControlLabel(new Vector2(0.09f, num), null, string.Empty);
			num += 0.035f;
			m_sliderValue = new MyGuiControlSlider(new Vector2(-0.135f, num), 0f, 1f, 0.3f, 0f, null, null, 1, 0.8f, 0.0416666679f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			num += 0.045f;
			Controls.Add(myGuiControlLabel);
			Controls.Add(m_modelPicker);
			Controls.Add(m_labelHue);
			Controls.Add(m_labelSaturation);
			Controls.Add(m_labelValue);
			Controls.Add(m_sliderHue);
			Controls.Add(m_sliderSaturation);
			Controls.Add(m_sliderValue);
			Controls.Add(new MyGuiControlButton(new Vector2(0f, 0.16f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenWardrobeOld_Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkClick));
			Controls.Add(new MyGuiControlButton(new Vector2(0f, 0.22f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenWardrobeOld_Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelClick));
			m_colorOrModelChanged = false;
		}

		protected override void Canceling()
		{
			MyGuiControlSlider sliderHue = m_sliderHue;
			sliderHue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderHue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderSaturation = m_sliderSaturation;
			sliderSaturation.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderSaturation.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderValue = m_sliderValue;
			sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			ChangeCharacter(m_storedModel, m_storedHSV);
			ChangeCameraBack();
			base.Canceling();
		}

		protected override void OnClosed()
		{
			MyGuiControlSlider sliderHue = m_sliderHue;
			sliderHue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderHue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderSaturation = m_sliderSaturation;
			sliderSaturation.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderSaturation.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderValue = m_sliderValue;
			sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
			base.OnClosed();
		}

		private void OnOkClick(MyGuiControlButton sender)
		{
			if (m_colorOrModelChanged && MyGuiScreenWardrobe.LookChanged != null)
			{
				MyGuiScreenWardrobe.LookChanged(m_storedModel, m_storedHSV, m_user.ModelName, m_user.ColorMask);
			}
			if (m_user.Definition.UsableByPlayer)
			{
				MyLocalCache.SaveInventoryConfig(MySession.Static.LocalCharacter);
			}
			ChangeCameraBack();
			CloseScreenNow();
		}

		private void OnCancelClick(MyGuiControlButton sender)
		{
			ChangeCharacter(m_storedModel, m_storedHSV);
			ChangeCameraBack();
			CloseScreenNow();
		}

		private void OnItemSelected()
		{
			m_selectedModel = m_models[(int)m_modelPicker.GetSelectedKey()];
			ChangeCharacter(m_selectedModel, m_selectedHSV);
		}

		private void OnValueChange(MyGuiControlSlider sender)
		{
			UpdateLabels();
			m_selectedHSV.X = m_sliderHue.Value / 360f;
			m_selectedHSV.Y = m_sliderSaturation.Value - MyColorPickerConstants.SATURATION_DELTA;
			m_selectedHSV.Z = m_sliderValue.Value - MyColorPickerConstants.VALUE_DELTA + MyColorPickerConstants.VALUE_COLORIZE_DELTA;
			m_selectedModel = m_models[(int)m_modelPicker.GetSelectedKey()];
			ChangeCharacter(m_selectedModel, m_selectedHSV);
		}

		private void UpdateLabels()
		{
			m_labelHue.Text = m_sliderHue.Value + "°";
			m_labelSaturation.Text = m_sliderSaturation.Value.ToString("P1");
			m_labelValue.Text = m_sliderValue.Value.ToString("P1");
		}

		private void ChangeCamera()
		{
			if (MySession.Static.Settings.Enable3rdPersonView)
			{
				m_storedCamera.Controller = MySession.Static.GetCameraControllerEnum();
				m_storedCamera.Distance = MySession.Static.GetCameraTargetDistance();
				MySession.Static.SetCameraController(MyCameraControllerEnum.ThirdPersonSpectator);
				MySession.Static.SetCameraTargetDistance(2.0);
			}
		}

		private void ChangeCameraBack()
		{
			if (MySession.Static.Settings.Enable3rdPersonView)
			{
				MySession.Static.SetCameraController(m_storedCamera.Controller, m_user);
				MySession.Static.SetCameraTargetDistance(m_storedCamera.Distance);
			}
		}

		private void ChangeCharacter(string model, Vector3 colorMaskHSV)
		{
			m_colorOrModelChanged = true;
			m_user.ChangeModelAndColor(model, colorMaskHSV, resetToDefault: false, MySession.Static.LocalPlayerId);
		}
	}
}
