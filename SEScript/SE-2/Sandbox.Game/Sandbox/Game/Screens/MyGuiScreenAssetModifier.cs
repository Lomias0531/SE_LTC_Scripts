using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenAssetModifier : MyGuiScreenBase
	{
		private struct MyCameraControllerSettings
		{
			public MatrixD ViewMatrix;

			public double Distance;

			public MyCameraControllerEnum Controller;
		}

		private MyGuiControlCombobox m_modelPicker;

		private MyGuiControlSlider m_sliderHue;

		private MyGuiControlSlider m_sliderSaturation;

		private MyGuiControlSlider m_sliderValue;

		private MyGuiControlLabel m_labelHue;

		private MyGuiControlLabel m_labelSaturation;

		private MyGuiControlLabel m_labelValue;

		private Dictionary<string, MyTextureChange> m_selectedModifier;

		private Vector3 m_selectedHSV;

		private readonly MyCharacter m_user;

		private readonly List<KeyValuePair<MyStringHash, Dictionary<string, MyTextureChange>>> m_modifiers;

		private readonly Vector3 m_storedHSV;

		private MyCameraControllerSettings m_storedCamera;

		private bool m_colorOrModelChanged;

		public static event MyAssetChangeDelegate LookChanged;

		public MyGuiScreenAssetModifier(MyCharacter user)
			: base(size: new Vector2(0.31f, 0.66f), position: MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP), backgroundColor: MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false, backgroundTexture: MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture)
		{
			base.EnabledBackgroundFade = false;
			m_user = user;
			m_storedHSV = m_user.ColorMask;
			m_selectedModifier = null;
			m_selectedHSV = m_storedHSV;
			m_modifiers = new List<KeyValuePair<MyStringHash, Dictionary<string, MyTextureChange>>>();
			int num = 0;
			MyDefinitionManager.Static.GetAssetModifierDefinitionsForRender();
			foreach (MyGameInventoryItem inventoryItem in MyGameService.InventoryItems)
			{
				MyDefinitionManager.MyAssetModifiers assetModifierDefinitionForRender = MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(inventoryItem.ItemDefinition.AssetModifierId);
				m_modifiers.Add(new KeyValuePair<MyStringHash, Dictionary<string, MyTextureChange>>(MyStringHash.GetOrCompute(inventoryItem.ItemDefinition.AssetModifierId), assetModifierDefinitionForRender.SkinTextureChanges));
				num++;
			}
			m_modifiers.Sort((KeyValuePair<MyStringHash, Dictionary<string, MyTextureChange>> a, KeyValuePair<MyStringHash, Dictionary<string, MyTextureChange>> b) => string.CompareOrdinal(a.Key.ToString(), b.Key.ToString()));
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

		public override string GetFriendlyName()
		{
			return "MyGuiScreenAssetModifier";
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
			int num2 = 0;
			foreach (KeyValuePair<MyStringHash, Dictionary<string, MyTextureChange>> modifier in m_modifiers)
			{
				MyGameInventoryItemDefinition inventoryItemDefinition = MyGameService.GetInventoryItemDefinition(modifier.Key.ToString());
				m_modelPicker.AddItem(num2, new StringBuilder((modifier.Key != MyStringHash.NullOrEmpty) ? inventoryItemDefinition.Name : "<null>"));
				num2++;
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
			Controls.Add(new MyGuiControlButton(new Vector2(0f, 0.16f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MySpaceTexts.ToolbarAction_Reset), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnResetClick));
			Controls.Add(new MyGuiControlButton(new Vector2(0f, 0.24f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenWardrobeOld_Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkClick));
			Controls.Add(new MyGuiControlButton(new Vector2(0f, 0.3f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.ScreenWardrobeOld_Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelClick));
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
			ChangeCharacter(null, m_storedHSV);
			ChangeCameraBack();
			base.Canceling();
		}

		protected override void OnClosed()
		{
			if (m_modifiers != null)
			{
				m_modifiers.Clear();
			}
			MyGuiControlSlider sliderHue = m_sliderHue;
			sliderHue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderHue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderSaturation = m_sliderSaturation;
			sliderSaturation.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderSaturation.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiControlSlider sliderValue = m_sliderValue;
			sliderValue.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Remove(sliderValue.ValueChanged, new Action<MyGuiControlSlider>(OnValueChange));
			MyGuiScreenGamePlay.ActiveGameplayScreen = null;
			base.OnClosed();
		}

		private void OnResetClick(MyGuiControlButton sender)
		{
			ChangeCharacter(null, m_storedHSV);
		}

		private void OnOkClick(MyGuiControlButton sender)
		{
			if (m_colorOrModelChanged && MyGuiScreenAssetModifier.LookChanged != null)
			{
				MyGuiScreenAssetModifier.LookChanged(m_user.ModelName, m_storedHSV, m_user.ModelName, m_user.ColorMask);
			}
			ChangeCameraBack();
			CloseScreenNow();
		}

		private void OnCancelClick(MyGuiControlButton sender)
		{
			ChangeCharacter(null, m_storedHSV);
			ChangeCameraBack();
			CloseScreenNow();
		}

		private void OnItemSelected()
		{
			m_selectedModifier = m_modifiers[(int)m_modelPicker.GetSelectedKey()].Value;
			ChangeCharacter(m_selectedModifier, m_selectedHSV);
		}

		private void OnValueChange(MyGuiControlSlider sender)
		{
			UpdateLabels();
			m_selectedHSV.X = m_sliderHue.Value / 360f;
			m_selectedHSV.Y = m_sliderSaturation.Value - MyColorPickerConstants.SATURATION_DELTA;
			m_selectedHSV.Z = m_sliderValue.Value - MyColorPickerConstants.VALUE_DELTA + MyColorPickerConstants.VALUE_COLORIZE_DELTA;
			ChangeCharacter(m_selectedModifier, m_selectedHSV);
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
				m_storedCamera.ViewMatrix = MySpectatorCameraController.Static.GetViewMatrix();
				Vector3D vector3D = m_user.WorldMatrix.Translation + m_user.WorldMatrix.Up + m_user.WorldMatrix.Forward * 2.0;
				MatrixD viewMatrix = MatrixD.CreateWorld(vector3D, m_user.WorldMatrix.Backward, m_user.WorldMatrix.Up);
				MySpectatorCameraController.Static.SetViewMatrix(viewMatrix);
				MySession.Static.SetCameraTargetDistance(2.0);
				MySession.Static.SetCameraController(MyCameraControllerEnum.Spectator, null, vector3D);
				MySpectator.Static.SetTarget(m_user.WorldMatrix.Translation + m_user.WorldMatrix.Up, m_user.WorldMatrix.Up);
			}
		}

		private void ChangeCameraBack()
		{
			if (MySession.Static.Settings.Enable3rdPersonView)
			{
				MySession.Static.SetCameraController(m_storedCamera.Controller, m_user);
				MySession.Static.SetCameraTargetDistance(m_storedCamera.Distance);
				MySpectatorCameraController.Static.SetViewMatrix(m_storedCamera.ViewMatrix);
			}
		}

		private void ChangeCharacter(Dictionary<string, MyTextureChange> modifier, Vector3 colorMaskHSV)
		{
			if (modifier == null || m_user == null)
			{
				ResetCharacter();
				m_colorOrModelChanged = true;
				m_user.ChangeModelAndColor(m_user.ModelName, colorMaskHSV, resetToDefault: false, 0L);
				return;
			}
			if (m_user.Render != null && m_user.Render.RenderObjectIDs[0] != uint.MaxValue)
			{
				MyRenderProxy.ChangeMaterialTexture(m_user.Render.RenderObjectIDs[0], modifier);
			}
			m_colorOrModelChanged = true;
			m_user.ChangeModelAndColor(m_user.ModelName, colorMaskHSV, resetToDefault: false, 0L);
		}

		private void ResetCharacter()
		{
			if (m_user != null)
			{
				MyAssetModifierComponent.SetDefaultTextures(m_user, "Astronaut_head");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "Head");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "Spacesuit_hood");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "LeftGlove");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "RightGlove");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "Boots");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "Arms");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "RightArm");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "Gear");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "Cloth");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "Emissive");
				MyAssetModifierComponent.SetDefaultTextures(m_user, "Backpack");
				MyAssetModifierComponent.ResetRifle(m_user);
				MyAssetModifierComponent.ResetWelder(m_user);
				MyAssetModifierComponent.ResetGrinder(m_user);
				MyAssetModifierComponent.ResetDrill(m_user);
			}
		}
	}
}
