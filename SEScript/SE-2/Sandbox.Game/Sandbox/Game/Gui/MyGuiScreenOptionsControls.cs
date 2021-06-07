using Sandbox.Engine.Utils;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyGuiScreenOptionsControls : MyGuiScreenBase
	{
		private class ControlButtonData
		{
			public readonly MyControl Control;

			public readonly MyGuiInputDeviceEnum Device;

			public ControlButtonData(MyControl control, MyGuiInputDeviceEnum device)
			{
				Control = control;
				Device = device;
			}
		}

		private class MyGuiControlAssignKeyMessageBox : MyGuiScreenMessageBox
		{
			private MyControl m_controlBeingSet;

			private MyGuiInputDeviceEnum m_deviceType;

			private List<MyKeys> m_newPressedKeys = new List<MyKeys>();

			private List<MyMouseButtonsEnum> m_newPressedMouseButtons = new List<MyMouseButtonsEnum>();

			private List<MyJoystickButtonsEnum> m_newPressedJoystickButtons = new List<MyJoystickButtonsEnum>();

			private List<MyJoystickAxesEnum> m_newPressedJoystickAxes = new List<MyJoystickAxesEnum>();

			private List<MyKeys> m_oldPressedKeys = new List<MyKeys>();

			private List<MyMouseButtonsEnum> m_oldPressedMouseButtons = new List<MyMouseButtonsEnum>();

			private List<MyJoystickButtonsEnum> m_oldPressedJoystickButtons = new List<MyJoystickButtonsEnum>();

			private List<MyJoystickAxesEnum> m_oldPressedJoystickAxes = new List<MyJoystickAxesEnum>();

			public MyGuiControlAssignKeyMessageBox(MyGuiInputDeviceEnum deviceType, MyControl controlBeingSet, MyStringId messageText)
				: base(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.NONE, MyTexts.Get(messageText), MyTexts.Get(MyCommonTexts.SelectControl), default(MyStringId), default(MyStringId), default(MyStringId), default(MyStringId), null, 0, ResultEnum.YES, canHideOthers: true, null)
			{
				base.DrawMouseCursor = false;
				m_isTopMostScreen = false;
				m_controlBeingSet = controlBeingSet;
				m_deviceType = deviceType;
				MyInput.Static.GetListOfPressedKeys(m_oldPressedKeys);
				MyInput.Static.GetListOfPressedMouseButtons(m_oldPressedMouseButtons);
				m_closeOnEsc = false;
				base.CanBeHidden = true;
			}

			public override void HandleInput(bool receivedFocusInThisUpdate)
			{
				base.HandleInput(receivedFocusInThisUpdate);
				if (MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
				{
					Canceling();
				}
				if (base.State != MyGuiScreenState.CLOSING && base.State != MyGuiScreenState.HIDING)
				{
					switch (m_deviceType)
					{
					case (MyGuiInputDeviceEnum)3:
					case (MyGuiInputDeviceEnum)4:
						break;
					case MyGuiInputDeviceEnum.Keyboard:
					case MyGuiInputDeviceEnum.KeyboardSecond:
						HandleKey();
						break;
					case MyGuiInputDeviceEnum.Mouse:
						HandleMouseButton();
						break;
					}
				}
			}

			private void HandleKey()
			{
				ReadPressedKeys();
				foreach (MyKeys key in m_newPressedKeys)
				{
					if (!m_oldPressedKeys.Contains(key))
					{
						if (!MyInput.Static.IsKeyValid(key))
						{
							ShowControlIsNotValidMessageBox();
						}
						else
						{
							MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
							MyControl ctrl = MyInput.Static.GetControl(key);
							if (ctrl != null)
							{
								if (ctrl.Equals(m_controlBeingSet))
								{
									OverwriteAssignment(ctrl, key);
									CloseScreen();
								}
								else
								{
									StringBuilder output = null;
									MyControl.AppendName(ref output, key);
									ShowControlIsAlreadyAssigned(ctrl, output, delegate
									{
										AnywayAssignment(ctrl, key);
									});
								}
							}
							else
							{
								m_controlBeingSet.SetControl(m_deviceType, key);
								CloseScreen();
							}
						}
						break;
					}
				}
				m_oldPressedKeys.Clear();
				MyUtils.Swap(ref m_oldPressedKeys, ref m_newPressedKeys);
			}

			private void HandleMouseButton()
			{
				MyInput.Static.GetListOfPressedMouseButtons(m_newPressedMouseButtons);
				foreach (MyMouseButtonsEnum button in m_newPressedMouseButtons)
				{
					if (!m_oldPressedMouseButtons.Contains(button))
					{
						MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
						if (!MyInput.Static.IsMouseButtonValid(button))
						{
							ShowControlIsNotValidMessageBox();
						}
						else
						{
							MyControl ctrl = MyInput.Static.GetControl(button);
							if (ctrl != null)
							{
								if (ctrl.Equals(m_controlBeingSet))
								{
									OverwriteAssignment(ctrl, button);
									CloseScreen();
								}
								else
								{
									StringBuilder output = null;
									MyControl.AppendName(ref output, button);
									ShowControlIsAlreadyAssigned(ctrl, output, delegate
									{
										AnywayAssignment(ctrl, button);
									});
								}
							}
							else
							{
								m_controlBeingSet.SetControl(button);
								CloseScreen();
							}
						}
						break;
					}
				}
				m_oldPressedMouseButtons.Clear();
				MyUtils.Swap(ref m_oldPressedMouseButtons, ref m_newPressedMouseButtons);
			}

			private void ReadPressedKeys()
			{
				MyInput.Static.GetListOfPressedKeys(m_newPressedKeys);
				m_newPressedKeys.Remove(MyKeys.Control);
				m_newPressedKeys.Remove(MyKeys.Shift);
				m_newPressedKeys.Remove(MyKeys.Alt);
				if (m_newPressedKeys.Contains(MyKeys.LeftControl) && m_newPressedKeys.Contains(MyKeys.RightAlt))
				{
					m_newPressedKeys.Remove(MyKeys.LeftControl);
				}
			}

			private void ShowControlIsNotValidMessageBox()
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.ControlIsNotValid), MyTexts.Get(MyCommonTexts.CanNotAssignControl)));
			}

			private void ShowControlIsAlreadyAssigned(MyControl controlAlreadySet, StringBuilder controlButtonName, Action overwriteAssignmentCallback)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox = MakeControlIsAlreadyAssignedDialog(controlAlreadySet, controlButtonName);
				myGuiScreenMessageBox.ResultCallback = delegate(ResultEnum r)
				{
					if (r == ResultEnum.YES)
					{
						overwriteAssignmentCallback();
						CloseScreen();
					}
					else
					{
						MyInput.Static.GetListOfPressedKeys(m_oldPressedKeys);
						MyInput.Static.GetListOfPressedMouseButtons(m_oldPressedMouseButtons);
					}
				};
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
			}

			private MyGuiScreenMessageBox MakeControlIsAlreadyAssignedDialog(MyControl controlAlreadySet, StringBuilder controlButtonName)
			{
				return MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.ControlAlreadyAssigned), controlButtonName, MyTexts.Get(controlAlreadySet.GetControlName()))), MyTexts.Get(MyCommonTexts.CanNotAssignControl));
			}

			private void OverwriteAssignment(MyControl controlAlreadySet, MyKeys key)
			{
				if (controlAlreadySet.GetKeyboardControl() == key)
				{
					controlAlreadySet.SetControl(MyGuiInputDeviceEnum.Keyboard, MyKeys.None);
				}
				else
				{
					controlAlreadySet.SetControl(MyGuiInputDeviceEnum.KeyboardSecond, MyKeys.None);
				}
				m_controlBeingSet.SetControl(m_deviceType, key);
			}

			private void AnywayAssignment(MyControl controlAlreadySet, MyKeys key)
			{
				m_controlBeingSet.SetControl(m_deviceType, key);
			}

			private void OverwriteAssignment(MyControl controlAlreadySet, MyMouseButtonsEnum button)
			{
				controlAlreadySet.SetControl(MyMouseButtonsEnum.None);
				m_controlBeingSet.SetControl(button);
			}

			private void AnywayAssignment(MyControl controlAlreadySet, MyMouseButtonsEnum button)
			{
				m_controlBeingSet.SetControl(button);
			}

			public override bool CloseScreen()
			{
				base.DrawMouseCursor = true;
				return base.CloseScreen();
			}
		}

		private MyGuiControlTypeEnum m_currentControlType;

		private MyGuiControlCombobox m_controlTypeList;

		private Dictionary<MyGuiControlTypeEnum, List<MyGuiControlBase>> m_allControls = new Dictionary<MyGuiControlTypeEnum, List<MyGuiControlBase>>();

		private List<MyGuiControlButton> m_key1Buttons;

		private List<MyGuiControlButton> m_key2Buttons;

		private List<MyGuiControlButton> m_mouseButtons;

		private List<MyGuiControlButton> m_joystickButtons;

		private List<MyGuiControlButton> m_joystickAxes;

		private MyGuiControlCheckbox m_invertMouseXCheckbox;

		private MyGuiControlCheckbox m_invertMouseYCheckbox;

		private MyGuiControlSlider m_mouseSensitivitySlider;

		private MyGuiControlSlider m_joystickSensitivitySlider;

		private MyGuiControlSlider m_joystickDeadzoneSlider;

		private MyGuiControlSlider m_joystickExponentSlider;

		private MyGuiControlCombobox m_joystickCombobox;

		private Vector2 m_controlsOriginLeft;

		private Vector2 m_controlsOriginRight;

		private MyGuiControlElementGroup m_elementGroup;

		public MyGuiScreenOptionsControls()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(183f / 280f, 0.9465649f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_elementGroup = new MyGuiControlElementGroup();
			m_elementGroup.HighlightChanged += m_elementGroup_HighlightChanged;
			AddCaption(MyCommonTexts.ScreenCaptionControls, null, new Vector2(0f, 0.003f));
			MyInput.Static.TakeSnapshot();
			_ = m_size.Value * new Vector2(0f, -0.5f);
			_ = m_size.Value * new Vector2(0f, 0.5f);
			_ = m_size.Value * -0.5f;
			m_controlsOriginLeft = (m_size.Value / 2f - new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE) * new Vector2(-1f, -1f) + new Vector2(0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y);
			m_controlsOriginRight = (m_size.Value / 2f - new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE) * new Vector2(1f, -1f) + new Vector2(0f, MyGuiConstants.SCREEN_CAPTION_DELTA_Y);
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList control = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, m_size.Value.Y / 2f - 0.144f), m_size.Value.X * 0.83f);
			Controls.Add(control);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.83f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.83f);
			Controls.Add(myGuiControlSeparatorList2);
			Vector2 value = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			Vector2 value2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			float num = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			float num2 = 25f;
			float sCREEN_CAPTION_DELTA_Y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y;
			float num3 = 0.0015f;
			new Vector2(0f, 0.045f);
			_ = (m_size.Value / 2f - value) * new Vector2(-1f, -1f) + new Vector2(0f, sCREEN_CAPTION_DELTA_Y);
			Vector2 vector = (m_size.Value / 2f - value) * new Vector2(1f, -1f) + new Vector2(0f, sCREEN_CAPTION_DELTA_Y);
			Vector2 value3 = (m_size.Value / 2f - value2) * new Vector2(0f, 1f);
			new Vector2(vector.X - (num + num3), vector.Y);
			_ = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlButton myGuiControlButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnOkClick);
			myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
			myGuiControlButton.Position = value3 + new Vector2(0f - num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			myGuiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
			MyGuiControlButton myGuiControlButton2 = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnCancelClick);
			myGuiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			myGuiControlButton2.Position = value3 + new Vector2(num2, 0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			myGuiControlButton2.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
			MyGuiControlButton myGuiControlButton3 = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.ComboBoxButton, MyGuiConstants.MESSAGE_BOX_BUTTON_SIZE_SMALL, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Revert), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_Defaults), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_ACTIVE, onButtonClick: OnResetDefaultsClick);
			myGuiControlButton3.Position = new Vector2(0f, 0f) - new Vector2(0f - m_size.Value.X * 0.832f / 2f + myGuiControlButton3.Size.X / 2f, m_size.Value.Y / 2f - 0.113f);
			myGuiControlButton3.TextScale = 0.7f;
			Controls.Add(myGuiControlButton);
			m_elementGroup.Add(myGuiControlButton);
			Controls.Add(myGuiControlButton2);
			m_elementGroup.Add(myGuiControlButton2);
			Controls.Add(myGuiControlButton3);
			m_elementGroup.Add(myGuiControlButton3);
			m_currentControlType = MyGuiControlTypeEnum.General;
			m_controlTypeList = new MyGuiControlCombobox(new Vector2(0f - myGuiControlButton3.Size.X / 2f - 0.009f, 0f) - new Vector2(0f, m_size.Value.Y / 2f - 0.11f));
			m_controlTypeList.Size = new Vector2(m_size.Value.X * 0.595f, 1f);
			m_controlTypeList.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_Category));
			m_controlTypeList.AddItem(0L, MyCommonTexts.ControlTypeGeneral);
			m_controlTypeList.AddItem(1L, MyCommonTexts.ControlTypeNavigation);
			m_controlTypeList.AddItem(5L, MyCommonTexts.ControlTypeSystems1);
			m_controlTypeList.AddItem(6L, MyCommonTexts.ControlTypeSystems2);
			m_controlTypeList.AddItem(7L, MyCommonTexts.ControlTypeSystems3);
			m_controlTypeList.AddItem(3L, MyCommonTexts.ControlTypeToolsOrWeapons);
			m_controlTypeList.AddItem(8L, MyCommonTexts.ControlTypeView);
			m_controlTypeList.SelectItemByKey((int)m_currentControlType);
			Controls.Add(m_controlTypeList);
			AddControls();
			ActivateControls(m_currentControlType);
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

		private void AddControls()
		{
			m_key1Buttons = new List<MyGuiControlButton>();
			m_key2Buttons = new List<MyGuiControlButton>();
			m_mouseButtons = new List<MyGuiControlButton>();
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				m_joystickButtons = new List<MyGuiControlButton>();
				m_joystickAxes = new List<MyGuiControlButton>();
			}
			AddControlsByType(MyGuiControlTypeEnum.General);
			AddControlsByType(MyGuiControlTypeEnum.Navigation);
			AddControlsByType(MyGuiControlTypeEnum.Systems1);
			AddControlsByType(MyGuiControlTypeEnum.Systems2);
			AddControlsByType(MyGuiControlTypeEnum.Systems3);
			AddControlsByType(MyGuiControlTypeEnum.ToolsOrWeapons);
			AddControlsByType(MyGuiControlTypeEnum.Spectator);
			foreach (KeyValuePair<MyGuiControlTypeEnum, List<MyGuiControlBase>> allControl in m_allControls)
			{
				foreach (MyGuiControlBase item in allControl.Value)
				{
					Controls.Add(item);
				}
				DeactivateControls(allControl.Key);
			}
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				RefreshJoystickControlEnabling();
			}
		}

		private MyGuiControlLabel MakeLabel(float deltaMultip, MyStringId textEnum)
		{
			return new MyGuiControlLabel(m_controlsOriginLeft + deltaMultip * MyGuiConstants.CONTROLS_DELTA, null, MyTexts.GetString(textEnum));
		}

		private MyGuiControlLabel MakeLabel(MyStringId textEnum, Vector2 position)
		{
			return new MyGuiControlLabel(position, null, MyTexts.GetString(textEnum), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
		}

		private MyGuiControlButton MakeControlButton(MyControl control, Vector2 position, MyGuiInputDeviceEnum device)
		{
			StringBuilder output = null;
			control.AppendBoundButtonNames(ref output, device);
			MyControl.AppendUnknownTextIfNeeded(ref output, MyTexts.GetString(MyCommonTexts.UnknownControl_None));
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.ControlSetting, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, output, 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnControlClick, GuiSounds.MouseClick, 1f, null, activateOnMouseRelease: false, OnSecondaryControlClick)
			{
				UserData = new ControlButtonData(control, device)
			};
		}

		private void AddControlsByType(MyGuiControlTypeEnum type)
		{
			if (type == MyGuiControlTypeEnum.General)
			{
				AddGeneralControls();
				return;
			}
			MyGuiControlButton.StyleDefinition visualStyle = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.ControlSetting);
			Vector2 controlsOriginRight = m_controlsOriginRight;
			controlsOriginRight.X -= 0.02f;
			controlsOriginRight.Y -= 0.01f;
			m_allControls[type] = new List<MyGuiControlBase>();
			float num = 2f;
			float num2 = 0.85f;
			DictionaryValuesReader<MyStringId, MyControl> gameControlsList = MyInput.Static.GetGameControlsList();
			MyGuiControlLabel myGuiControlLabel = MakeLabel(MyCommonTexts.ScreenOptionsControls_Keyboard1, Vector2.Zero);
			MyGuiControlLabel myGuiControlLabel2 = MakeLabel(MyCommonTexts.ScreenOptionsControls_Keyboard2, Vector2.Zero);
			MyGuiControlLabel myGuiControlLabel3 = MakeLabel(MyCommonTexts.ScreenOptionsControls_Mouse, Vector2.Zero);
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				MakeLabel(MyCommonTexts.ScreenOptionsControls_Gamepad, Vector2.Zero);
			}
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				MakeLabel(MyCommonTexts.ScreenOptionsControls_AnalogAxes, Vector2.Zero);
			}
			float num3 = 1.1f * Math.Max(Math.Max(myGuiControlLabel.Size.X, myGuiControlLabel2.Size.X), Math.Max(myGuiControlLabel3.Size.X, visualStyle.SizeOverride.Value.X));
			Vector2 position = (num - 1f) * MyGuiConstants.CONTROLS_DELTA + controlsOriginRight;
			position.X += num3 * 0.5f - 0.265f;
			position.Y -= 0.015f;
			myGuiControlLabel.Position = position;
			position.X += num3;
			myGuiControlLabel2.Position = position;
			position.X += num3;
			myGuiControlLabel3.Position = position;
			m_allControls[type].Add(myGuiControlLabel);
			m_allControls[type].Add(myGuiControlLabel2);
			m_allControls[type].Add(myGuiControlLabel3);
			_ = MyFakes.ENABLE_JOYSTICK_SETTINGS;
			foreach (MyControl item in gameControlsList)
			{
				if (item.GetControlTypeEnum() == type)
				{
					m_allControls[type].Add(new MyGuiControlLabel(m_controlsOriginLeft + num * MyGuiConstants.CONTROLS_DELTA - new Vector2(0f, 0.03f), null, MyTexts.GetString(item.GetControlName())));
					position = controlsOriginRight + num * MyGuiConstants.CONTROLS_DELTA - new Vector2(0.265f, 0.015f);
					position.X += num3 * 0.5f;
					MyGuiControlButton myGuiControlButton = MakeControlButton(item, position, MyGuiInputDeviceEnum.Keyboard);
					myGuiControlButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ClickToEdit));
					m_allControls[type].Add(myGuiControlButton);
					m_key1Buttons.Add(myGuiControlButton);
					position.X += num3;
					MyGuiControlButton myGuiControlButton2 = MakeControlButton(item, position, MyGuiInputDeviceEnum.KeyboardSecond);
					myGuiControlButton2.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ClickToEdit));
					m_allControls[type].Add(myGuiControlButton2);
					m_key2Buttons.Add(myGuiControlButton2);
					position.X += num3;
					MyGuiControlButton myGuiControlButton3 = MakeControlButton(item, position, MyGuiInputDeviceEnum.Mouse);
					myGuiControlButton3.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_ClickToEdit));
					m_allControls[type].Add(myGuiControlButton3);
					m_mouseButtons.Add(myGuiControlButton3);
					position.X += num3;
					_ = MyFakes.ENABLE_JOYSTICK_SETTINGS;
					num += num2;
				}
			}
		}

		private void AddGeneralControls()
		{
			m_controlsOriginRight.Y -= 0.025f;
			m_controlsOriginLeft.Y -= 0.025f;
			m_allControls[MyGuiControlTypeEnum.General] = new List<MyGuiControlBase>();
			m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(1.7f, MyCommonTexts.MouseSensitivity));
			m_mouseSensitivitySlider = new MyGuiControlSlider(m_controlsOriginRight + 1.7f * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f), 0.1f, 3f, 0.29f, MyInput.Static.GetMouseSensitivity(), null, null, 1, 0.8f, 0f, "White", MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_MouseSensitivity), MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_mouseSensitivitySlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
			m_mouseSensitivitySlider.Value = MyInput.Static.GetMouseSensitivity();
			m_allControls[MyGuiControlTypeEnum.General].Add(m_mouseSensitivitySlider);
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(2.63f, MyCommonTexts.JoystickSensitivity));
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(3.56f, MyCommonTexts.JoystickExponent));
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(4.49f, MyCommonTexts.JoystickDeadzone));
				m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(5.72f, MyCommonTexts.Joystick));
				m_joystickSensitivitySlider = new MyGuiControlSlider(m_controlsOriginRight + 2.63f * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f), 0.1f, 6f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickSensitivity), defaultValue: MyInput.Static.GetJoystickSensitivity());
				m_joystickSensitivitySlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
				m_joystickSensitivitySlider.Value = MyInput.Static.GetJoystickSensitivity();
				m_allControls[MyGuiControlTypeEnum.General].Add(m_joystickSensitivitySlider);
				m_joystickExponentSlider = new MyGuiControlSlider(m_controlsOriginRight + 3.56f * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f), 1f, 8f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickGradualPrecision), defaultValue: MyInput.Static.GetJoystickExponent());
				m_joystickExponentSlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
				m_joystickExponentSlider.Value = MyInput.Static.GetJoystickExponent();
				m_allControls[MyGuiControlTypeEnum.General].Add(m_joystickExponentSlider);
				m_joystickDeadzoneSlider = new MyGuiControlSlider(m_controlsOriginRight + 4.49f * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f), 0f, 0.5f, 0.29f, toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickDeadzoneWidth), defaultValue: MyInput.Static.GetJoystickDeadzone());
				m_joystickDeadzoneSlider.Size = new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
				m_joystickDeadzoneSlider.Value = MyInput.Static.GetJoystickDeadzone();
				m_allControls[MyGuiControlTypeEnum.General].Add(m_joystickDeadzoneSlider);
				m_joystickCombobox = new MyGuiControlCombobox(m_controlsOriginRight + 5.72f * MyGuiConstants.CONTROLS_DELTA - new Vector2(455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X / 2f, 0f));
				m_joystickCombobox.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_JoystickOrGamepad));
				m_joystickCombobox.ItemSelected += OnSelectJoystick;
				AddJoysticksToComboBox();
				m_joystickCombobox.Size = new Vector2(452f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f);
				m_joystickCombobox.Enabled = (!MyFakes.ENFORCE_CONTROLLER || !MyInput.Static.IsJoystickConnected());
				m_allControls[MyGuiControlTypeEnum.General].Add(m_joystickCombobox);
			}
			m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(6.85f, MyCommonTexts.InvertMouseX));
			m_invertMouseXCheckbox = new MyGuiControlCheckbox(m_controlsOriginRight + 6.85f * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f), null, isChecked: MyInput.Static.GetMouseXInversion(), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertMouseX), visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_allControls[MyGuiControlTypeEnum.General].Add(m_invertMouseXCheckbox);
			m_allControls[MyGuiControlTypeEnum.General].Add(MakeLabel(7.7f, MyCommonTexts.InvertMouseY));
			m_invertMouseYCheckbox = new MyGuiControlCheckbox(m_controlsOriginRight + 7.7f * MyGuiConstants.CONTROLS_DELTA - new Vector2(456.5f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, 0f), null, isChecked: MyInput.Static.GetMouseYInversion(), toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsControls_InvertMouseY), visualStyle: MyGuiControlCheckboxStyleEnum.Default, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			m_allControls[MyGuiControlTypeEnum.General].Add(m_invertMouseYCheckbox);
			m_controlsOriginRight.Y += 0.025f;
			m_controlsOriginLeft.Y += 0.025f;
		}

		private void DeactivateControls(MyGuiControlTypeEnum type)
		{
			foreach (MyGuiControlBase item in m_allControls[type])
			{
				item.Visible = false;
			}
		}

		private void ActivateControls(MyGuiControlTypeEnum type)
		{
			foreach (MyGuiControlBase item in m_allControls[type])
			{
				item.Visible = true;
			}
		}

		private void AddJoysticksToComboBox()
		{
			int num = 0;
			bool flag = false;
			m_joystickCombobox.AddItem(num++, MyTexts.Get(MyCommonTexts.Disabled));
			foreach (string item in MyInput.Static.EnumerateJoystickNames())
			{
				m_joystickCombobox.AddItem(num, new StringBuilder(item));
				if (MyInput.Static.JoystickInstanceName == item)
				{
					flag = true;
					m_joystickCombobox.SelectItemByIndex(num);
				}
				num++;
			}
			if (!flag)
			{
				m_joystickCombobox.SelectItemByIndex(0);
			}
		}

		private void OnSelectJoystick()
		{
			MyInput.Static.JoystickInstanceName = ((m_joystickCombobox.GetSelectedIndex() == 0) ? null : m_joystickCombobox.GetSelectedValue().ToString());
			RefreshJoystickControlEnabling();
		}

		private void RefreshJoystickControlEnabling()
		{
			bool enabled = m_joystickCombobox.GetSelectedIndex() != 0;
			foreach (MyGuiControlButton joystickButton in m_joystickButtons)
			{
				joystickButton.Enabled = enabled;
			}
			foreach (MyGuiControlButton joystickAxis in m_joystickAxes)
			{
				joystickAxis.Enabled = enabled;
			}
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenOptionsControls";
		}

		public override bool Update(bool hasFocus)
		{
			if (m_controlTypeList.GetSelectedKey() != (int)m_currentControlType)
			{
				DeactivateControls(m_currentControlType);
				m_currentControlType = (MyGuiControlTypeEnum)m_controlTypeList.GetSelectedKey();
				ActivateControls(m_currentControlType);
			}
			if (!base.Update(hasFocus))
			{
				return false;
			}
			return true;
		}

		private void OnControlClick(MyGuiControlButton button)
		{
			ControlButtonData controlButtonData = (ControlButtonData)button.UserData;
			MyStringId messageText = MyCommonTexts.AssignControlKeyboard;
			if (controlButtonData.Device == MyGuiInputDeviceEnum.Mouse)
			{
				messageText = MyCommonTexts.AssignControlMouse;
			}
			MyGuiControlAssignKeyMessageBox myGuiControlAssignKeyMessageBox = new MyGuiControlAssignKeyMessageBox(controlButtonData.Device, controlButtonData.Control, messageText);
			myGuiControlAssignKeyMessageBox.Closed += delegate
			{
				RefreshButtonTexts();
			};
			MyGuiSandbox.AddScreen(myGuiControlAssignKeyMessageBox);
		}

		private void OnSecondaryControlClick(MyGuiControlButton button)
		{
			ControlButtonData data = (ControlButtonData)button.UserData;
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextRemoveControlBinding), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum result)
			{
				if (result == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					if (data.Device == MyGuiInputDeviceEnum.Mouse)
					{
						data.Control.SetControl(MyMouseButtonsEnum.None);
					}
					else if (data.Device == MyGuiInputDeviceEnum.Keyboard || data.Device == MyGuiInputDeviceEnum.KeyboardSecond)
					{
						data.Control.SetControl(data.Device, MyKeys.None);
					}
					RefreshButtonTexts();
				}
			}));
		}

		private void OnResetDefaultsClick(MyGuiControlButton sender)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionResetControlsToDefault), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextResetControlsToDefault), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum res)
			{
				if (res == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					MyInput.Static.RevertToDefaultControls();
					DeactivateControls(m_currentControlType);
					AddControls();
					ActivateControls(m_currentControlType);
				}
			}));
		}

		protected override void Canceling()
		{
			MyInput.Static.RevertChanges();
			base.Canceling();
		}

		private void OnCancelClick(MyGuiControlButton sender)
		{
			MyInput.Static.RevertChanges();
			CloseScreen();
		}

		private void OnOkClick(MyGuiControlButton sender)
		{
			CloseScreenAndSave();
		}

		private void CloseScreenAndSave()
		{
			MyInput.Static.SetMouseXInversion(m_invertMouseXCheckbox.IsChecked);
			MyInput.Static.SetMouseYInversion(m_invertMouseYCheckbox.IsChecked);
			MyInput.Static.SetMouseSensitivity(m_mouseSensitivitySlider.Value);
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				MyInput.Static.JoystickInstanceName = ((m_joystickCombobox.GetSelectedIndex() == 0) ? null : m_joystickCombobox.GetSelectedValue().ToString());
				MyInput.Static.SetJoystickSensitivity(m_joystickSensitivitySlider.Value);
				MyInput.Static.SetJoystickExponent(m_joystickExponentSlider.Value);
				MyInput.Static.SetJoystickDeadzone(m_joystickDeadzoneSlider.Value);
				MyInput.Static.UpdateJoystickChanged();
			}
			MyInput.Static.SaveControls(MySandboxGame.Config.ControlsGeneral, MySandboxGame.Config.ControlsButtons);
			MySandboxGame.Config.Save();
			MyScreenManager.RecreateControls();
			CloseScreen();
		}

		private void RefreshButtonTexts()
		{
			RefreshButtonTexts(m_key1Buttons);
			RefreshButtonTexts(m_key2Buttons);
			RefreshButtonTexts(m_mouseButtons);
			if (MyFakes.ENABLE_JOYSTICK_SETTINGS)
			{
				RefreshButtonTexts(m_joystickButtons);
				RefreshButtonTexts(m_joystickAxes);
			}
		}

		private void RefreshButtonTexts(List<MyGuiControlButton> buttons)
		{
			StringBuilder output = null;
			foreach (MyGuiControlButton button in buttons)
			{
				ControlButtonData controlButtonData = (ControlButtonData)button.UserData;
				controlButtonData.Control.AppendBoundButtonNames(ref output, controlButtonData.Device);
				MyControl.AppendUnknownTextIfNeeded(ref output, MyTexts.GetString(MyCommonTexts.UnknownControl_None));
				button.Text = output.ToString();
				output.Clear();
			}
		}
	}
}
