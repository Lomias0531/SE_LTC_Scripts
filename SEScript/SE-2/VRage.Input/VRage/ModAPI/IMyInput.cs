using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace VRage.ModAPI
{
	public interface IMyInput
	{
		string JoystickInstanceName
		{
			get;
		}

		ListReader<char> TextInput
		{
			get;
		}

		bool JoystickAsMouse
		{
			get;
		}

		bool IsJoystickLastUsed
		{
			get;
		}

		event Action<bool> JoystickConnected;

		List<string> EnumerateJoystickNames();

		bool IsAnyKeyPress();

		bool IsAnyMousePressed();

		bool IsAnyNewMousePressed();

		bool IsAnyShiftKeyPressed();

		bool IsAnyAltKeyPressed();

		bool IsAnyCtrlKeyPressed();

		void GetPressedKeys(List<MyKeys> keys);

		bool IsKeyPress(MyKeys key);

		bool WasKeyPress(MyKeys key);

		bool IsNewKeyPressed(MyKeys key);

		bool IsNewKeyReleased(MyKeys key);

		bool IsMousePressed(MyMouseButtonsEnum button);

		bool IsMouseReleased(MyMouseButtonsEnum button);

		bool IsNewMousePressed(MyMouseButtonsEnum button);

		bool IsNewLeftMousePressed();

		bool IsNewLeftMouseReleased();

		bool IsLeftMousePressed();

		bool IsLeftMouseReleased();

		bool IsRightMousePressed();

		bool IsNewRightMousePressed();

		bool IsNewRightMouseReleased();

		bool WasRightMousePressed();

		bool WasRightMouseReleased();

		bool IsMiddleMousePressed();

		bool IsNewMiddleMousePressed();

		bool IsNewMiddleMouseReleased();

		bool WasMiddleMousePressed();

		bool WasMiddleMouseReleased();

		bool IsXButton1MousePressed();

		bool IsNewXButton1MousePressed();

		bool IsNewXButton1MouseReleased();

		bool WasXButton1MousePressed();

		bool WasXButton1MouseReleased();

		bool IsXButton2MousePressed();

		bool IsNewXButton2MousePressed();

		bool IsNewXButton2MouseReleased();

		bool WasXButton2MousePressed();

		bool WasXButton2MouseReleased();

		bool IsJoystickButtonPressed(MyJoystickButtonsEnum button);

		bool IsJoystickButtonNewPressed(MyJoystickButtonsEnum button);

		bool IsNewJoystickButtonReleased(MyJoystickButtonsEnum button);

		float GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis);

		bool IsJoystickAxisPressed(MyJoystickAxesEnum axis);

		bool IsJoystickAxisNewPressed(MyJoystickAxesEnum axis);

		bool IsNewJoystickAxisReleased(MyJoystickAxesEnum axis);

		bool IsAnyMouseOrJoystickPressed();

		bool IsAnyNewMouseOrJoystickPressed();

		bool IsNewPrimaryButtonPressed();

		bool IsNewSecondaryButtonPressed();

		bool IsNewPrimaryButtonReleased();

		bool IsNewSecondaryButtonReleased();

		bool IsPrimaryButtonReleased();

		bool IsSecondaryButtonReleased();

		bool IsPrimaryButtonPressed();

		bool IsSecondaryButtonPressed();

		bool IsNewButtonPressed(MySharedButtonsEnum button);

		bool IsButtonPressed(MySharedButtonsEnum button);

		bool IsNewButtonReleased(MySharedButtonsEnum button);

		bool IsButtonReleased(MySharedButtonsEnum button);

		int MouseScrollWheelValue();

		int PreviousMouseScrollWheelValue();

		int DeltaMouseScrollWheelValue();

		int GetMouseXForGamePlay();

		int GetMouseYForGamePlay();

		int GetMouseX();

		int GetMouseY();

		bool GetMouseXInversion();

		bool GetMouseYInversion();

		float GetMouseSensitivity();

		Vector2 GetMousePosition();

		Vector2 GetMouseAreaSize();

		bool IsNewGameControlPressed(MyStringId controlEnum);

		bool IsGameControlPressed(MyStringId controlEnum);

		bool IsNewGameControlReleased(MyStringId controlEnum);

		float GetGameControlAnalogState(MyStringId controlEnum);

		bool IsGameControlReleased(MyStringId controlEnum);

		bool IsKeyValid(MyKeys key);

		bool IsKeyDigit(MyKeys key);

		bool IsMouseButtonValid(MyMouseButtonsEnum button);

		bool IsJoystickButtonValid(MyJoystickButtonsEnum button);

		bool IsJoystickAxisValid(MyJoystickAxesEnum axis);

		bool IsJoystickConnected();

		IMyControl GetControl(MyKeys key);

		IMyControl GetControl(MyMouseButtonsEnum button);

		void GetListOfPressedKeys(List<MyKeys> keys);

		void GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result);

		IMyControl GetGameControl(MyStringId controlEnum);

		string GetKeyName(MyKeys key);

		string GetName(MyMouseButtonsEnum mouseButton);

		string GetName(MyJoystickButtonsEnum joystickButton);

		string GetName(MyJoystickAxesEnum joystickAxis);

		string GetUnassignedName();
	}
}
