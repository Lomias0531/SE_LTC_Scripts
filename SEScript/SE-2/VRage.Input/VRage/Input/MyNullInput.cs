using System;
using System.Collections.Generic;
using System.Text;
using VRage.Collections;
using VRage.Input.Keyboard;
using VRage.ModAPI;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Input
{
	public class MyNullInput : IMyInput, VRage.ModAPI.IMyInput
	{
		private MyControl m_nullControl = new MyControl(default(MyStringId), default(MyStringId), MyGuiControlTypeEnum.General, null, null);

		private List<char> m_listChars = new List<char>();

		private List<string> m_listStrings = new List<string>();

		string IMyInput.JoystickInstanceName
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		ListReader<char> IMyInput.TextInput => m_listChars;

		bool IMyInput.JoystickAsMouse
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		bool IMyInput.IsJoystickLastUsed => false;

		bool IMyInput.ENABLE_DEVELOPER_KEYS => false;

		public bool OverrideUpdate
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public MyMouseState ActualMouseState => default(MyMouseState);

		public MyJoystickState ActualJoystickState => default(MyJoystickState);

		string VRage.ModAPI.IMyInput.JoystickInstanceName => ((IMyInput)this).JoystickInstanceName;

		ListReader<char> VRage.ModAPI.IMyInput.TextInput => ((IMyInput)this).TextInput;

		bool VRage.ModAPI.IMyInput.JoystickAsMouse => ((IMyInput)this).JoystickAsMouse;

		bool VRage.ModAPI.IMyInput.IsJoystickLastUsed => ((IMyInput)this).IsJoystickLastUsed;

		event Action<bool> IMyInput.JoystickConnected
		{
			add
			{
			}
			remove
			{
			}
		}

		event Action<bool> VRage.ModAPI.IMyInput.JoystickConnected
		{
			add
			{
				((IMyInput)this).JoystickConnected += value;
			}
			remove
			{
				((IMyInput)this).JoystickConnected -= value;
			}
		}

		public void SearchForJoystick()
		{
		}

		void IMyInput.LoadData(SerializableDictionary<string, string> controlsGeneral, SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
		{
		}

		void IMyInput.LoadContent()
		{
		}

		void IMyInput.UnloadData()
		{
		}

		List<string> IMyInput.EnumerateJoystickNames()
		{
			return m_listStrings;
		}

		bool IMyInput.Update(bool gameFocused)
		{
			return false;
		}

		public void SetControlBlock(MyStringId controlEnum, bool block = false)
		{
		}

		public bool IsControlBlocked(MyStringId controlEnum)
		{
			return false;
		}

		public void ClearBlacklist()
		{
		}

		bool IMyInput.IsAnyKeyPress()
		{
			return false;
		}

		bool IMyInput.IsAnyMousePressed()
		{
			return false;
		}

		bool IMyInput.IsAnyNewMousePressed()
		{
			return false;
		}

		bool IMyInput.IsAnyShiftKeyPressed()
		{
			return false;
		}

		bool IMyInput.IsAnyAltKeyPressed()
		{
			return false;
		}

		bool IMyInput.IsAnyCtrlKeyPressed()
		{
			return false;
		}

		void IMyInput.GetPressedKeys(List<MyKeys> keys)
		{
		}

		bool IMyInput.IsKeyPress(MyKeys key)
		{
			return false;
		}

		bool IMyInput.WasKeyPress(MyKeys key)
		{
			return false;
		}

		bool IMyInput.IsNewKeyPressed(MyKeys key)
		{
			return false;
		}

		bool IMyInput.IsNewKeyReleased(MyKeys key)
		{
			return false;
		}

		bool IMyInput.IsMousePressed(MyMouseButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsMouseReleased(MyMouseButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsNewMousePressed(MyMouseButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsNewLeftMousePressed()
		{
			return false;
		}

		bool IMyInput.IsNewLeftMouseReleased()
		{
			return false;
		}

		bool IMyInput.IsLeftMousePressed()
		{
			return false;
		}

		bool IMyInput.IsLeftMouseReleased()
		{
			return false;
		}

		bool IMyInput.IsRightMousePressed()
		{
			return false;
		}

		bool IMyInput.IsNewRightMousePressed()
		{
			return false;
		}

		bool IMyInput.IsNewRightMouseReleased()
		{
			return false;
		}

		bool IMyInput.WasRightMousePressed()
		{
			return false;
		}

		bool IMyInput.WasRightMouseReleased()
		{
			return false;
		}

		bool IMyInput.IsMiddleMousePressed()
		{
			return false;
		}

		bool IMyInput.IsNewMiddleMousePressed()
		{
			return false;
		}

		bool IMyInput.IsNewMiddleMouseReleased()
		{
			return false;
		}

		bool IMyInput.WasMiddleMousePressed()
		{
			return false;
		}

		bool IMyInput.WasMiddleMouseReleased()
		{
			return false;
		}

		bool IMyInput.IsXButton1MousePressed()
		{
			return false;
		}

		bool IMyInput.IsNewXButton1MousePressed()
		{
			return false;
		}

		bool IMyInput.IsNewXButton1MouseReleased()
		{
			return false;
		}

		bool IMyInput.WasXButton1MousePressed()
		{
			return false;
		}

		bool IMyInput.WasXButton1MouseReleased()
		{
			return false;
		}

		bool IMyInput.IsXButton2MousePressed()
		{
			return false;
		}

		bool IMyInput.IsNewXButton2MousePressed()
		{
			return false;
		}

		bool IMyInput.IsNewXButton2MouseReleased()
		{
			return false;
		}

		bool IMyInput.WasXButton2MousePressed()
		{
			return false;
		}

		bool IMyInput.WasXButton2MouseReleased()
		{
			return false;
		}

		bool IMyInput.IsJoystickButtonPressed(MyJoystickButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsJoystickButtonNewPressed(MyJoystickButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsNewJoystickButtonReleased(MyJoystickButtonsEnum button)
		{
			return false;
		}

		float IMyInput.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis)
		{
			return 0f;
		}

		public Vector3 GetJoystickPositionForGameplay(RequestedJoystickAxis requestedAxis)
		{
			return Vector3.Zero;
		}

		public Vector3 GetJoystickRotationForGameplay(RequestedJoystickAxis requestedAxis)
		{
			return Vector3.Zero;
		}

		public Vector2 GetJoystickAxesStateForGameplay(MyJoystickAxesEnum axis1, MyJoystickAxesEnum axis2)
		{
			return Vector2.Zero;
		}

		bool IMyInput.IsJoystickAxisPressed(MyJoystickAxesEnum axis)
		{
			return false;
		}

		bool IMyInput.IsJoystickAxisNewPressed(MyJoystickAxesEnum axis)
		{
			return false;
		}

		bool IMyInput.IsNewJoystickAxisReleased(MyJoystickAxesEnum axis)
		{
			return false;
		}

		bool IMyInput.IsNewGameControlJoystickOnlyPressed(MyStringId controlId)
		{
			return false;
		}

		public void DeviceChangeCallback()
		{
		}

		public void NegateEscapePress()
		{
		}

		public string ReplaceControlsInText(string text)
		{
			throw new NotImplementedException();
		}

		float IMyInput.GetJoystickSensitivity()
		{
			return 0f;
		}

		void IMyInput.SetJoystickSensitivity(float newSensitivity)
		{
		}

		float IMyInput.GetJoystickExponent()
		{
			return 0f;
		}

		void IMyInput.SetJoystickExponent(float newExponent)
		{
		}

		float IMyInput.GetJoystickDeadzone()
		{
			return 0f;
		}

		void IMyInput.SetJoystickDeadzone(float newDeadzone)
		{
		}

		int IMyInput.MouseScrollWheelValue()
		{
			return 0;
		}

		int IMyInput.PreviousMouseScrollWheelValue()
		{
			return 0;
		}

		int IMyInput.DeltaMouseScrollWheelValue()
		{
			return 0;
		}

		int IMyInput.GetMouseXForGamePlay()
		{
			return 0;
		}

		int IMyInput.GetMouseYForGamePlay()
		{
			return 0;
		}

		float IMyInput.GetMouseXForGamePlayF()
		{
			return 0f;
		}

		float IMyInput.GetMouseYForGamePlayF()
		{
			return 0f;
		}

		int IMyInput.GetMouseX()
		{
			return 0;
		}

		int IMyInput.GetMouseY()
		{
			return 0;
		}

		bool IMyInput.GetMouseXInversion()
		{
			return false;
		}

		bool IMyInput.GetMouseYInversion()
		{
			return false;
		}

		void IMyInput.SetMouseXInversion(bool inverted)
		{
		}

		void IMyInput.SetMouseYInversion(bool inverted)
		{
		}

		float IMyInput.GetMouseSensitivity()
		{
			return 0f;
		}

		void IMyInput.SetMouseSensitivity(float sensitivity)
		{
		}

		Vector2 IMyInput.GetMousePosition()
		{
			return Vector2.Zero;
		}

		void IMyInput.SetMousePosition(int x, int y)
		{
		}

		bool IMyInput.IsGamepadKeyRightPressed()
		{
			return false;
		}

		bool IMyInput.IsGamepadKeyLeftPressed()
		{
			return false;
		}

		bool IMyInput.IsNewGamepadKeyDownPressed()
		{
			return false;
		}

		bool IMyInput.IsNewGamepadKeyUpPressed()
		{
			return false;
		}

		void IMyInput.GetActualJoystickState(StringBuilder text)
		{
		}

		bool IMyInput.IsAnyMouseOrJoystickPressed()
		{
			return false;
		}

		bool IMyInput.IsAnyNewMouseOrJoystickPressed()
		{
			return false;
		}

		bool IMyInput.IsNewPrimaryButtonPressed()
		{
			return false;
		}

		bool IMyInput.IsNewSecondaryButtonPressed()
		{
			return false;
		}

		bool IMyInput.IsNewPrimaryButtonReleased()
		{
			return false;
		}

		bool IMyInput.IsNewSecondaryButtonReleased()
		{
			return false;
		}

		bool IMyInput.IsPrimaryButtonReleased()
		{
			return false;
		}

		bool IMyInput.IsSecondaryButtonReleased()
		{
			return false;
		}

		bool IMyInput.IsPrimaryButtonPressed()
		{
			return false;
		}

		bool IMyInput.IsSecondaryButtonPressed()
		{
			return false;
		}

		bool IMyInput.IsNewButtonPressed(MySharedButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsButtonPressed(MySharedButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsNewButtonReleased(MySharedButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsButtonReleased(MySharedButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsNewGameControlPressed(MyStringId controlEnum)
		{
			return false;
		}

		bool IMyInput.IsGameControlPressed(MyStringId controlEnum)
		{
			return false;
		}

		bool IMyInput.IsNewGameControlReleased(MyStringId controlEnum)
		{
			return false;
		}

		float IMyInput.GetGameControlAnalogState(MyStringId controlEnum)
		{
			return 0f;
		}

		bool IMyInput.IsGameControlReleased(MyStringId controlEnum)
		{
			return false;
		}

		bool IMyInput.IsKeyValid(MyKeys key)
		{
			return false;
		}

		bool IMyInput.IsKeyDigit(MyKeys key)
		{
			return false;
		}

		bool IMyInput.IsMouseButtonValid(MyMouseButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsJoystickButtonValid(MyJoystickButtonsEnum button)
		{
			return false;
		}

		bool IMyInput.IsJoystickAxisValid(MyJoystickAxesEnum axis)
		{
			return false;
		}

		bool IMyInput.IsJoystickConnected()
		{
			return false;
		}

		MyControl IMyInput.GetControl(MyKeys key)
		{
			return null;
		}

		MyControl IMyInput.GetControl(MyMouseButtonsEnum button)
		{
			return null;
		}

		void IMyInput.GetListOfPressedKeys(List<MyKeys> keys)
		{
		}

		void IMyInput.GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result)
		{
		}

		DictionaryValuesReader<MyStringId, MyControl> IMyInput.GetGameControlsList()
		{
			return null;
		}

		void IMyInput.TakeSnapshot()
		{
		}

		void IMyInput.RevertChanges()
		{
		}

		MyControl IMyInput.GetGameControl(MyStringId controlEnum)
		{
			return m_nullControl;
		}

		void IMyInput.RevertToDefaultControls()
		{
		}

		void IMyInput.SaveControls(SerializableDictionary<string, string> controlsGeneral, SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
		{
		}

		Vector2 IMyInput.GetMouseAreaSize()
		{
			return Vector2.Zero;
		}

		string IMyInput.GetName(MyMouseButtonsEnum mouseButton)
		{
			return "";
		}

		string IMyInput.GetName(MyJoystickButtonsEnum joystickButton)
		{
			return "";
		}

		string IMyInput.GetName(MyJoystickAxesEnum joystickAxis)
		{
			return "";
		}

		string IMyInput.GetUnassignedName()
		{
			return "";
		}

		string IMyInput.GetKeyName(MyKeys key)
		{
			return "";
		}

		public void UpdateStates()
		{
		}

		public void UpdateStatesFromPlayback(MyKeyboardState currentKeyboard, MyKeyboardState previousKeyboard, MyMouseState currentMouse, MyMouseState previousMouse, MyJoystickState currentJoystick, MyJoystickState previousJoystick, int x, int y, List<char> keyboardSnapshotText)
		{
		}

		public void ClearStates()
		{
		}

		public void UpdateJoystickChanged()
		{
		}

		public bool IsMouseMoved()
		{
			return false;
		}

		List<string> VRage.ModAPI.IMyInput.EnumerateJoystickNames()
		{
			return ((IMyInput)this).EnumerateJoystickNames();
		}

		bool VRage.ModAPI.IMyInput.IsAnyKeyPress()
		{
			return ((IMyInput)this).IsAnyKeyPress();
		}

		bool VRage.ModAPI.IMyInput.IsAnyMousePressed()
		{
			return ((IMyInput)this).IsAnyMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyNewMousePressed()
		{
			return ((IMyInput)this).IsAnyNewMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyShiftKeyPressed()
		{
			return ((IMyInput)this).IsAnyShiftKeyPressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyAltKeyPressed()
		{
			return ((IMyInput)this).IsAnyAltKeyPressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyCtrlKeyPressed()
		{
			return ((IMyInput)this).IsAnyCtrlKeyPressed();
		}

		void VRage.ModAPI.IMyInput.GetPressedKeys(List<MyKeys> keys)
		{
			((IMyInput)this).GetPressedKeys(keys);
		}

		public void AddDefaultControl(MyStringId stringId, MyControl control)
		{
		}

		bool VRage.ModAPI.IMyInput.IsKeyPress(MyKeys key)
		{
			return ((IMyInput)this).IsKeyPress(key);
		}

		bool VRage.ModAPI.IMyInput.WasKeyPress(MyKeys key)
		{
			return ((IMyInput)this).WasKeyPress(key);
		}

		bool VRage.ModAPI.IMyInput.IsNewKeyPressed(MyKeys key)
		{
			return ((IMyInput)this).IsNewKeyPressed(key);
		}

		bool VRage.ModAPI.IMyInput.IsNewKeyReleased(MyKeys key)
		{
			return ((IMyInput)this).IsNewKeyReleased(key);
		}

		bool VRage.ModAPI.IMyInput.IsMousePressed(MyMouseButtonsEnum button)
		{
			return ((IMyInput)this).IsMousePressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsMouseReleased(MyMouseButtonsEnum button)
		{
			return ((IMyInput)this).IsMouseReleased(button);
		}

		bool VRage.ModAPI.IMyInput.IsNewMousePressed(MyMouseButtonsEnum button)
		{
			return ((IMyInput)this).IsNewMousePressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsNewLeftMousePressed()
		{
			return ((IMyInput)this).IsNewLeftMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewLeftMouseReleased()
		{
			return ((IMyInput)this).IsNewLeftMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsLeftMousePressed()
		{
			return ((IMyInput)this).IsLeftMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsLeftMouseReleased()
		{
			return ((IMyInput)this).IsLeftMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsRightMousePressed()
		{
			return ((IMyInput)this).IsRightMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewRightMousePressed()
		{
			return ((IMyInput)this).IsNewRightMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewRightMouseReleased()
		{
			return ((IMyInput)this).IsNewRightMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.WasRightMousePressed()
		{
			return ((IMyInput)this).WasRightMousePressed();
		}

		bool VRage.ModAPI.IMyInput.WasRightMouseReleased()
		{
			return ((IMyInput)this).WasRightMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsMiddleMousePressed()
		{
			return ((IMyInput)this).IsMiddleMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewMiddleMousePressed()
		{
			return ((IMyInput)this).IsNewMiddleMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewMiddleMouseReleased()
		{
			return ((IMyInput)this).IsNewMiddleMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.WasMiddleMousePressed()
		{
			return ((IMyInput)this).WasMiddleMousePressed();
		}

		bool VRage.ModAPI.IMyInput.WasMiddleMouseReleased()
		{
			return ((IMyInput)this).WasMiddleMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsXButton1MousePressed()
		{
			return ((IMyInput)this).IsXButton1MousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewXButton1MousePressed()
		{
			return ((IMyInput)this).IsNewXButton1MousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewXButton1MouseReleased()
		{
			return ((IMyInput)this).IsNewXButton1MouseReleased();
		}

		bool VRage.ModAPI.IMyInput.WasXButton1MousePressed()
		{
			return ((IMyInput)this).WasXButton1MousePressed();
		}

		bool VRage.ModAPI.IMyInput.WasXButton1MouseReleased()
		{
			return ((IMyInput)this).WasXButton1MouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsXButton2MousePressed()
		{
			return ((IMyInput)this).IsXButton2MousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewXButton2MousePressed()
		{
			return ((IMyInput)this).IsNewXButton2MousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewXButton2MouseReleased()
		{
			return ((IMyInput)this).IsNewXButton2MouseReleased();
		}

		bool VRage.ModAPI.IMyInput.WasXButton2MousePressed()
		{
			return ((IMyInput)this).WasXButton2MousePressed();
		}

		bool VRage.ModAPI.IMyInput.WasXButton2MouseReleased()
		{
			return ((IMyInput)this).WasXButton2MouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsJoystickButtonPressed(MyJoystickButtonsEnum button)
		{
			return ((IMyInput)this).IsJoystickButtonPressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickButtonNewPressed(MyJoystickButtonsEnum button)
		{
			return ((IMyInput)this).IsJoystickButtonNewPressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsNewJoystickButtonReleased(MyJoystickButtonsEnum button)
		{
			return ((IMyInput)this).IsNewJoystickButtonReleased(button);
		}

		float VRage.ModAPI.IMyInput.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis)
		{
			return ((IMyInput)this).GetJoystickAxisStateForGameplay(axis);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickAxisPressed(MyJoystickAxesEnum axis)
		{
			return ((IMyInput)this).IsJoystickAxisPressed(axis);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickAxisNewPressed(MyJoystickAxesEnum axis)
		{
			return ((IMyInput)this).IsJoystickAxisNewPressed(axis);
		}

		bool VRage.ModAPI.IMyInput.IsNewJoystickAxisReleased(MyJoystickAxesEnum axis)
		{
			return ((IMyInput)this).IsNewJoystickAxisReleased(axis);
		}

		bool VRage.ModAPI.IMyInput.IsAnyMouseOrJoystickPressed()
		{
			return ((IMyInput)this).IsAnyMouseOrJoystickPressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyNewMouseOrJoystickPressed()
		{
			return ((IMyInput)this).IsAnyNewMouseOrJoystickPressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewPrimaryButtonPressed()
		{
			return ((IMyInput)this).IsNewPrimaryButtonPressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewSecondaryButtonPressed()
		{
			return ((IMyInput)this).IsNewSecondaryButtonPressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewPrimaryButtonReleased()
		{
			return ((IMyInput)this).IsNewPrimaryButtonReleased();
		}

		bool VRage.ModAPI.IMyInput.IsNewSecondaryButtonReleased()
		{
			return ((IMyInput)this).IsNewSecondaryButtonReleased();
		}

		bool VRage.ModAPI.IMyInput.IsPrimaryButtonReleased()
		{
			return ((IMyInput)this).IsPrimaryButtonReleased();
		}

		bool VRage.ModAPI.IMyInput.IsSecondaryButtonReleased()
		{
			return ((IMyInput)this).IsSecondaryButtonReleased();
		}

		bool VRage.ModAPI.IMyInput.IsPrimaryButtonPressed()
		{
			return ((IMyInput)this).IsPrimaryButtonPressed();
		}

		bool VRage.ModAPI.IMyInput.IsSecondaryButtonPressed()
		{
			return ((IMyInput)this).IsSecondaryButtonPressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewButtonPressed(MySharedButtonsEnum button)
		{
			return ((IMyInput)this).IsNewButtonPressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsButtonPressed(MySharedButtonsEnum button)
		{
			return ((IMyInput)this).IsButtonPressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsNewButtonReleased(MySharedButtonsEnum button)
		{
			return ((IMyInput)this).IsNewButtonReleased(button);
		}

		bool VRage.ModAPI.IMyInput.IsButtonReleased(MySharedButtonsEnum button)
		{
			return ((IMyInput)this).IsButtonReleased(button);
		}

		int VRage.ModAPI.IMyInput.MouseScrollWheelValue()
		{
			return ((IMyInput)this).MouseScrollWheelValue();
		}

		int VRage.ModAPI.IMyInput.PreviousMouseScrollWheelValue()
		{
			return ((IMyInput)this).PreviousMouseScrollWheelValue();
		}

		int VRage.ModAPI.IMyInput.DeltaMouseScrollWheelValue()
		{
			return ((IMyInput)this).DeltaMouseScrollWheelValue();
		}

		int VRage.ModAPI.IMyInput.GetMouseXForGamePlay()
		{
			return ((IMyInput)this).GetMouseXForGamePlay();
		}

		int VRage.ModAPI.IMyInput.GetMouseYForGamePlay()
		{
			return ((IMyInput)this).GetMouseYForGamePlay();
		}

		int VRage.ModAPI.IMyInput.GetMouseX()
		{
			return ((IMyInput)this).GetMouseX();
		}

		int VRage.ModAPI.IMyInput.GetMouseY()
		{
			return ((IMyInput)this).GetMouseY();
		}

		bool VRage.ModAPI.IMyInput.GetMouseXInversion()
		{
			return ((IMyInput)this).GetMouseXInversion();
		}

		bool VRage.ModAPI.IMyInput.GetMouseYInversion()
		{
			return ((IMyInput)this).GetMouseYInversion();
		}

		float VRage.ModAPI.IMyInput.GetMouseSensitivity()
		{
			return ((IMyInput)this).GetMouseSensitivity();
		}

		Vector2 VRage.ModAPI.IMyInput.GetMousePosition()
		{
			return ((IMyInput)this).GetMousePosition();
		}

		Vector2 VRage.ModAPI.IMyInput.GetMouseAreaSize()
		{
			return ((IMyInput)this).GetMouseAreaSize();
		}

		bool VRage.ModAPI.IMyInput.IsNewGameControlPressed(MyStringId controlEnum)
		{
			return ((IMyInput)this).IsNewGameControlPressed(controlEnum);
		}

		bool VRage.ModAPI.IMyInput.IsGameControlPressed(MyStringId controlEnum)
		{
			return ((IMyInput)this).IsGameControlPressed(controlEnum);
		}

		bool VRage.ModAPI.IMyInput.IsNewGameControlReleased(MyStringId controlEnum)
		{
			return ((IMyInput)this).IsNewGameControlReleased(controlEnum);
		}

		float VRage.ModAPI.IMyInput.GetGameControlAnalogState(MyStringId controlEnum)
		{
			return ((IMyInput)this).GetGameControlAnalogState(controlEnum);
		}

		bool VRage.ModAPI.IMyInput.IsGameControlReleased(MyStringId controlEnum)
		{
			return ((IMyInput)this).IsGameControlReleased(controlEnum);
		}

		bool VRage.ModAPI.IMyInput.IsKeyValid(MyKeys key)
		{
			return ((IMyInput)this).IsKeyValid(key);
		}

		bool VRage.ModAPI.IMyInput.IsKeyDigit(MyKeys key)
		{
			return ((IMyInput)this).IsKeyDigit(key);
		}

		bool VRage.ModAPI.IMyInput.IsMouseButtonValid(MyMouseButtonsEnum button)
		{
			return ((IMyInput)this).IsMouseButtonValid(button);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickButtonValid(MyJoystickButtonsEnum button)
		{
			return ((IMyInput)this).IsJoystickButtonValid(button);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickAxisValid(MyJoystickAxesEnum axis)
		{
			return ((IMyInput)this).IsJoystickAxisValid(axis);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickConnected()
		{
			return ((IMyInput)this).IsJoystickConnected();
		}

		IMyControl VRage.ModAPI.IMyInput.GetControl(MyKeys key)
		{
			return ((IMyInput)this).GetControl(key);
		}

		IMyControl VRage.ModAPI.IMyInput.GetControl(MyMouseButtonsEnum button)
		{
			return ((IMyInput)this).GetControl(button);
		}

		void VRage.ModAPI.IMyInput.GetListOfPressedKeys(List<MyKeys> keys)
		{
			((IMyInput)this).GetListOfPressedKeys(keys);
		}

		void VRage.ModAPI.IMyInput.GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result)
		{
			((IMyInput)this).GetListOfPressedMouseButtons(result);
		}

		IMyControl VRage.ModAPI.IMyInput.GetGameControl(MyStringId controlEnum)
		{
			return ((IMyInput)this).GetGameControl(controlEnum);
		}

		string VRage.ModAPI.IMyInput.GetKeyName(MyKeys key)
		{
			return ((IMyInput)this).GetKeyName(key);
		}

		string VRage.ModAPI.IMyInput.GetName(MyMouseButtonsEnum mouseButton)
		{
			return ((IMyInput)this).GetName(mouseButton);
		}

		string VRage.ModAPI.IMyInput.GetName(MyJoystickButtonsEnum joystickButton)
		{
			return ((IMyInput)this).GetName(joystickButton);
		}

		string VRage.ModAPI.IMyInput.GetName(MyJoystickAxesEnum joystickAxis)
		{
			return ((IMyInput)this).GetName(joystickAxis);
		}

		string VRage.ModAPI.IMyInput.GetUnassignedName()
		{
			return ((IMyInput)this).GetUnassignedName();
		}

		public void EnableInput(bool enable)
		{
		}

		public bool IsEnabled()
		{
			return false;
		}
	}
}
