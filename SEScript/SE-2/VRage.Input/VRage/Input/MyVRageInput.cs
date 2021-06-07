using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VRage.Collections;
using VRage.Input.Joystick;
using VRage.Input.Keyboard;
using VRage.ModAPI;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Input
{
	public class MyVRageInput : VRage.ModAPI.IMyInput, IMyInput, ITextEvaluator
	{
		private bool m_overrideUpdate;

		private Vector2 m_absoluteMousePosition;

		private MyMouseState m_previousMouseState;

		private MyJoystickState m_previousJoystickState;

		private MyGuiLocalizedKeyboardState m_keyboardState;

		private MyMouseState m_actualMouseState;

		private MyMouseState m_actualMouseStateRaw;

		private MyJoystickState m_actualJoystickState;

		private bool m_mouseXIsInverted;

		private bool m_mouseYIsInverted;

		private float m_mouseSensitivity;

		private string m_joystickInstanceName;

		private float m_joystickSensitivity;

		private float m_joystickDeadzone;

		private float m_joystickExponent;

		private const bool IS_MOUSE_X_INVERTED_DEFAULT = false;

		private const bool IS_MOUSE_Y_INVERTED_DEFAULT = false;

		private const float MOUSE_SENSITIVITY_DEFAULT = 1.655f;

		private const string JOYSTICK_INSTANCE_NAME_DEFAULT = null;

		private const float JOYSTICK_SENSITIVITY_DEFAULT = 2f;

		private const float JOYSTICK_EXPONENT_DEFAULT = 2f;

		private const float JOYSTICK_DEADZONE_DEFAULT = 0.2f;

		private string m_joystickInstanceNameSnapshot;

		private bool m_enabled = true;

		private readonly MyKeyHasher m_hasher = new MyKeyHasher();

		private readonly Dictionary<MyStringId, MyControl> m_defaultGameControlsList;

		private readonly Dictionary<MyStringId, MyControl> m_gameControlsList;

		private readonly Dictionary<MyStringId, MyControl> m_gameControlsSnapshot;

		private readonly HashSet<MyStringId> m_gameControlsBlacklist = new HashSet<MyStringId>();

		private readonly List<MyKeys> m_validKeyboardKeys = new List<MyKeys>();

		private readonly List<MyJoystickButtonsEnum> m_validJoystickButtons = new List<MyJoystickButtonsEnum>();

		private readonly List<MyJoystickAxesEnum> m_validJoystickAxes = new List<MyJoystickAxesEnum>();

		private readonly List<MyMouseButtonsEnum> m_validMouseButtons = new List<MyMouseButtonsEnum>();

		private readonly List<MyKeys> m_digitKeys = new List<MyKeys>();

		private readonly IVRageInput m_platformInput;

		private readonly IMyControlNameLookup m_nameLookup;

		private List<char> m_currentTextInput = new List<char>();

		private bool m_gameWasFocused;

		private List<string> m_joysticks;

		private bool m_joystickConnected;

		private bool m_initializeJoystick;

		string VRage.ModAPI.IMyInput.JoystickInstanceName => JoystickInstanceName;

		ListReader<char> VRage.ModAPI.IMyInput.TextInput => TextInput;

		bool VRage.ModAPI.IMyInput.JoystickAsMouse => JoystickAsMouse;

		bool VRage.ModAPI.IMyInput.IsJoystickLastUsed => IsJoystickLastUsed;

		public string JoystickInstanceName
		{
			get
			{
				return m_joystickInstanceName;
			}
			set
			{
				if (m_joystickInstanceName != value)
				{
					m_joystickInstanceName = value;
					m_initializeJoystick = true;
				}
			}
		}

		private bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled != value)
				{
					ClearStates();
					m_enabled = value;
				}
			}
		}

		public bool OverrideUpdate
		{
			get
			{
				return m_overrideUpdate;
			}
			set
			{
				m_overrideUpdate = value;
			}
		}

		public MyMouseState ActualMouseState => m_actualMouseState;

		public MyJoystickState ActualJoystickState => m_actualJoystickState;

		public ListReader<char> TextInput => new ListReader<char>(m_currentTextInput);

		public bool JoystickAsMouse
		{
			get;
			set;
		}

		public bool IsJoystickLastUsed
		{
			get;
			set;
		}

		public bool ENABLE_DEVELOPER_KEYS
		{
			get;
			private set;
		}

		public bool Trichording
		{
			get;
			set;
		}

		event Action<bool> VRage.ModAPI.IMyInput.JoystickConnected
		{
			add
			{
				JoystickConnected += value;
			}
			remove
			{
				JoystickConnected -= value;
			}
		}

		public event Action<bool> JoystickConnected;

		List<string> VRage.ModAPI.IMyInput.EnumerateJoystickNames()
		{
			return EnumerateJoystickNames();
		}

		bool VRage.ModAPI.IMyInput.IsAnyKeyPress()
		{
			return IsAnyKeyPress();
		}

		bool VRage.ModAPI.IMyInput.IsAnyMousePressed()
		{
			return IsAnyMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyNewMousePressed()
		{
			return IsAnyNewMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyShiftKeyPressed()
		{
			return IsAnyShiftKeyPressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyAltKeyPressed()
		{
			return IsAnyAltKeyPressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyCtrlKeyPressed()
		{
			return IsAnyCtrlKeyPressed();
		}

		void VRage.ModAPI.IMyInput.GetPressedKeys(List<MyKeys> keys)
		{
			GetPressedKeys(keys);
		}

		bool VRage.ModAPI.IMyInput.IsKeyPress(MyKeys key)
		{
			return IsKeyPress(key);
		}

		bool VRage.ModAPI.IMyInput.WasKeyPress(MyKeys key)
		{
			return WasKeyPress(key);
		}

		bool VRage.ModAPI.IMyInput.IsNewKeyPressed(MyKeys key)
		{
			return IsNewKeyPressed(key);
		}

		bool VRage.ModAPI.IMyInput.IsNewKeyReleased(MyKeys key)
		{
			return IsNewKeyReleased(key);
		}

		bool VRage.ModAPI.IMyInput.IsMousePressed(MyMouseButtonsEnum button)
		{
			return IsMousePressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsMouseReleased(MyMouseButtonsEnum button)
		{
			return IsMouseReleased(button);
		}

		bool VRage.ModAPI.IMyInput.IsNewMousePressed(MyMouseButtonsEnum button)
		{
			return IsNewMousePressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsNewLeftMousePressed()
		{
			return IsNewLeftMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewLeftMouseReleased()
		{
			return IsNewLeftMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsLeftMousePressed()
		{
			return IsLeftMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsLeftMouseReleased()
		{
			return IsLeftMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsRightMousePressed()
		{
			return IsRightMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewRightMousePressed()
		{
			return IsNewRightMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewRightMouseReleased()
		{
			return IsNewRightMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.WasRightMousePressed()
		{
			return WasRightMousePressed();
		}

		bool VRage.ModAPI.IMyInput.WasRightMouseReleased()
		{
			return WasRightMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsMiddleMousePressed()
		{
			return IsMiddleMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewMiddleMousePressed()
		{
			return IsNewMiddleMousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewMiddleMouseReleased()
		{
			return IsNewMiddleMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.WasMiddleMousePressed()
		{
			return WasMiddleMousePressed();
		}

		bool VRage.ModAPI.IMyInput.WasMiddleMouseReleased()
		{
			return WasMiddleMouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsXButton1MousePressed()
		{
			return IsXButton1MousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewXButton1MousePressed()
		{
			return IsNewXButton1MousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewXButton1MouseReleased()
		{
			return IsNewXButton1MouseReleased();
		}

		bool VRage.ModAPI.IMyInput.WasXButton1MousePressed()
		{
			return WasXButton1MousePressed();
		}

		bool VRage.ModAPI.IMyInput.WasXButton1MouseReleased()
		{
			return WasXButton1MouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsXButton2MousePressed()
		{
			return IsXButton2MousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewXButton2MousePressed()
		{
			return IsNewXButton2MousePressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewXButton2MouseReleased()
		{
			return IsNewXButton2MouseReleased();
		}

		bool VRage.ModAPI.IMyInput.WasXButton2MousePressed()
		{
			return WasXButton2MousePressed();
		}

		bool VRage.ModAPI.IMyInput.WasXButton2MouseReleased()
		{
			return WasXButton2MouseReleased();
		}

		bool VRage.ModAPI.IMyInput.IsJoystickButtonPressed(MyJoystickButtonsEnum button)
		{
			return IsJoystickButtonPressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickButtonNewPressed(MyJoystickButtonsEnum button)
		{
			return IsJoystickButtonNewPressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsNewJoystickButtonReleased(MyJoystickButtonsEnum button)
		{
			return IsNewJoystickButtonReleased(button);
		}

		float VRage.ModAPI.IMyInput.GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis)
		{
			return GetJoystickAxisStateForGameplay(axis);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickAxisPressed(MyJoystickAxesEnum axis)
		{
			return IsJoystickAxisPressed(axis);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickAxisNewPressed(MyJoystickAxesEnum axis)
		{
			return IsJoystickAxisNewPressed(axis);
		}

		bool VRage.ModAPI.IMyInput.IsNewJoystickAxisReleased(MyJoystickAxesEnum axis)
		{
			return IsNewJoystickAxisReleased(axis);
		}

		bool VRage.ModAPI.IMyInput.IsAnyMouseOrJoystickPressed()
		{
			return IsAnyMouseOrJoystickPressed();
		}

		bool VRage.ModAPI.IMyInput.IsAnyNewMouseOrJoystickPressed()
		{
			return IsAnyNewMouseOrJoystickPressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewPrimaryButtonPressed()
		{
			return IsNewPrimaryButtonPressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewSecondaryButtonPressed()
		{
			return IsNewSecondaryButtonPressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewPrimaryButtonReleased()
		{
			return IsNewPrimaryButtonReleased();
		}

		bool VRage.ModAPI.IMyInput.IsNewSecondaryButtonReleased()
		{
			return IsNewSecondaryButtonReleased();
		}

		bool VRage.ModAPI.IMyInput.IsPrimaryButtonReleased()
		{
			return IsPrimaryButtonReleased();
		}

		bool VRage.ModAPI.IMyInput.IsSecondaryButtonReleased()
		{
			return IsSecondaryButtonReleased();
		}

		bool VRage.ModAPI.IMyInput.IsPrimaryButtonPressed()
		{
			return IsPrimaryButtonPressed();
		}

		bool VRage.ModAPI.IMyInput.IsSecondaryButtonPressed()
		{
			return IsSecondaryButtonPressed();
		}

		bool VRage.ModAPI.IMyInput.IsNewButtonPressed(MySharedButtonsEnum button)
		{
			return IsNewButtonPressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsButtonPressed(MySharedButtonsEnum button)
		{
			return IsButtonPressed(button);
		}

		bool VRage.ModAPI.IMyInput.IsNewButtonReleased(MySharedButtonsEnum button)
		{
			return IsNewButtonReleased(button);
		}

		bool VRage.ModAPI.IMyInput.IsButtonReleased(MySharedButtonsEnum button)
		{
			return IsButtonReleased(button);
		}

		int VRage.ModAPI.IMyInput.MouseScrollWheelValue()
		{
			return MouseScrollWheelValue();
		}

		int VRage.ModAPI.IMyInput.PreviousMouseScrollWheelValue()
		{
			return PreviousMouseScrollWheelValue();
		}

		int VRage.ModAPI.IMyInput.DeltaMouseScrollWheelValue()
		{
			return DeltaMouseScrollWheelValue();
		}

		int VRage.ModAPI.IMyInput.GetMouseXForGamePlay()
		{
			return GetMouseXForGamePlay();
		}

		int VRage.ModAPI.IMyInput.GetMouseYForGamePlay()
		{
			return GetMouseYForGamePlay();
		}

		int VRage.ModAPI.IMyInput.GetMouseX()
		{
			return GetMouseX();
		}

		int VRage.ModAPI.IMyInput.GetMouseY()
		{
			return GetMouseY();
		}

		bool VRage.ModAPI.IMyInput.GetMouseXInversion()
		{
			return GetMouseXInversion();
		}

		bool VRage.ModAPI.IMyInput.GetMouseYInversion()
		{
			return GetMouseYInversion();
		}

		float VRage.ModAPI.IMyInput.GetMouseSensitivity()
		{
			return GetMouseSensitivity();
		}

		Vector2 VRage.ModAPI.IMyInput.GetMousePosition()
		{
			return GetMousePosition();
		}

		Vector2 VRage.ModAPI.IMyInput.GetMouseAreaSize()
		{
			return GetMouseAreaSize();
		}

		bool VRage.ModAPI.IMyInput.IsNewGameControlPressed(MyStringId controlEnum)
		{
			return IsNewGameControlPressed(controlEnum);
		}

		bool VRage.ModAPI.IMyInput.IsGameControlPressed(MyStringId controlEnum)
		{
			return IsGameControlPressed(controlEnum);
		}

		bool VRage.ModAPI.IMyInput.IsNewGameControlReleased(MyStringId controlEnum)
		{
			return IsNewGameControlReleased(controlEnum);
		}

		float VRage.ModAPI.IMyInput.GetGameControlAnalogState(MyStringId controlEnum)
		{
			return GetGameControlAnalogState(controlEnum);
		}

		bool VRage.ModAPI.IMyInput.IsGameControlReleased(MyStringId controlEnum)
		{
			return IsGameControlReleased(controlEnum);
		}

		bool VRage.ModAPI.IMyInput.IsKeyValid(MyKeys key)
		{
			return IsKeyValid(key);
		}

		bool VRage.ModAPI.IMyInput.IsKeyDigit(MyKeys key)
		{
			return IsKeyDigit(key);
		}

		bool VRage.ModAPI.IMyInput.IsMouseButtonValid(MyMouseButtonsEnum button)
		{
			return IsMouseButtonValid(button);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickButtonValid(MyJoystickButtonsEnum button)
		{
			return IsJoystickButtonValid(button);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickAxisValid(MyJoystickAxesEnum axis)
		{
			return IsJoystickAxisValid(axis);
		}

		bool VRage.ModAPI.IMyInput.IsJoystickConnected()
		{
			return IsJoystickConnected();
		}

		IMyControl VRage.ModAPI.IMyInput.GetControl(MyKeys key)
		{
			return GetControl(key);
		}

		IMyControl VRage.ModAPI.IMyInput.GetControl(MyMouseButtonsEnum button)
		{
			return GetControl(button);
		}

		void VRage.ModAPI.IMyInput.GetListOfPressedKeys(List<MyKeys> keys)
		{
			GetListOfPressedKeys(keys);
		}

		void VRage.ModAPI.IMyInput.GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result)
		{
			GetListOfPressedMouseButtons(result);
		}

		IMyControl VRage.ModAPI.IMyInput.GetGameControl(MyStringId controlEnum)
		{
			return GetGameControl(controlEnum);
		}

		string VRage.ModAPI.IMyInput.GetKeyName(MyKeys key)
		{
			return GetKeyName(key);
		}

		string VRage.ModAPI.IMyInput.GetName(MyMouseButtonsEnum mouseButton)
		{
			return GetName(mouseButton);
		}

		string VRage.ModAPI.IMyInput.GetName(MyJoystickButtonsEnum joystickButton)
		{
			return GetName(joystickButton);
		}

		string VRage.ModAPI.IMyInput.GetName(MyJoystickAxesEnum joystickAxis)
		{
			return GetName(joystickAxis);
		}

		string VRage.ModAPI.IMyInput.GetUnassignedName()
		{
			return GetUnassignedName();
		}

		public bool IsEnabled()
		{
			return m_enabled;
		}

		public void EnableInput(bool enable)
		{
			Enabled = enable;
		}

		public MyVRageInput(IVRageInput textInputBuffer, IMyControlNameLookup nameLookup, Dictionary<MyStringId, MyControl> gameControls, bool enableDevKeys)
		{
			m_platformInput = textInputBuffer;
			m_nameLookup = nameLookup;
			m_defaultGameControlsList = gameControls;
			m_gameControlsList = new Dictionary<MyStringId, MyControl>(MyStringId.Comparer);
			m_gameControlsSnapshot = new Dictionary<MyStringId, MyControl>(MyStringId.Comparer);
			CloneControls(m_defaultGameControlsList, m_gameControlsList);
			ENABLE_DEVELOPER_KEYS = enableDevKeys;
			ResetJoystickState();
		}

		public void AddDefaultControl(MyStringId stringId, MyControl control)
		{
			m_gameControlsList[stringId] = control;
			m_defaultGameControlsList[stringId] = control;
		}

		public void SearchForJoystick()
		{
			m_initializeJoystick = true;
		}

		public void LoadData(SerializableDictionary<string, string> controlsGeneral, SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
		{
			m_mouseXIsInverted = false;
			m_mouseYIsInverted = false;
			m_mouseSensitivity = 1.655f;
			m_joystickInstanceName = null;
			m_joystickSensitivity = 2f;
			m_joystickDeadzone = 0.2f;
			m_joystickExponent = 2f;
			m_digitKeys.Add(MyKeys.D0);
			m_digitKeys.Add(MyKeys.D1);
			m_digitKeys.Add(MyKeys.D2);
			m_digitKeys.Add(MyKeys.D3);
			m_digitKeys.Add(MyKeys.D4);
			m_digitKeys.Add(MyKeys.D5);
			m_digitKeys.Add(MyKeys.D6);
			m_digitKeys.Add(MyKeys.D7);
			m_digitKeys.Add(MyKeys.D8);
			m_digitKeys.Add(MyKeys.D9);
			m_digitKeys.Add(MyKeys.NumPad0);
			m_digitKeys.Add(MyKeys.NumPad1);
			m_digitKeys.Add(MyKeys.NumPad2);
			m_digitKeys.Add(MyKeys.NumPad3);
			m_digitKeys.Add(MyKeys.NumPad4);
			m_digitKeys.Add(MyKeys.NumPad5);
			m_digitKeys.Add(MyKeys.NumPad6);
			m_digitKeys.Add(MyKeys.NumPad7);
			m_digitKeys.Add(MyKeys.NumPad8);
			m_digitKeys.Add(MyKeys.NumPad9);
			m_validKeyboardKeys.Add(MyKeys.A);
			m_validKeyboardKeys.Add(MyKeys.Add);
			m_validKeyboardKeys.Add(MyKeys.B);
			m_validKeyboardKeys.Add(MyKeys.Back);
			m_validKeyboardKeys.Add(MyKeys.C);
			m_validKeyboardKeys.Add(MyKeys.CapsLock);
			m_validKeyboardKeys.Add(MyKeys.D);
			m_validKeyboardKeys.Add(MyKeys.D0);
			m_validKeyboardKeys.Add(MyKeys.D1);
			m_validKeyboardKeys.Add(MyKeys.D2);
			m_validKeyboardKeys.Add(MyKeys.D3);
			m_validKeyboardKeys.Add(MyKeys.D4);
			m_validKeyboardKeys.Add(MyKeys.D5);
			m_validKeyboardKeys.Add(MyKeys.D6);
			m_validKeyboardKeys.Add(MyKeys.D7);
			m_validKeyboardKeys.Add(MyKeys.D8);
			m_validKeyboardKeys.Add(MyKeys.D9);
			m_validKeyboardKeys.Add(MyKeys.Decimal);
			m_validKeyboardKeys.Add(MyKeys.Delete);
			m_validKeyboardKeys.Add(MyKeys.Divide);
			m_validKeyboardKeys.Add(MyKeys.Down);
			m_validKeyboardKeys.Add(MyKeys.E);
			m_validKeyboardKeys.Add(MyKeys.End);
			m_validKeyboardKeys.Add(MyKeys.Enter);
			m_validKeyboardKeys.Add(MyKeys.F);
			m_validKeyboardKeys.Add(MyKeys.G);
			m_validKeyboardKeys.Add(MyKeys.H);
			m_validKeyboardKeys.Add(MyKeys.Home);
			m_validKeyboardKeys.Add(MyKeys.I);
			m_validKeyboardKeys.Add(MyKeys.Insert);
			m_validKeyboardKeys.Add(MyKeys.J);
			m_validKeyboardKeys.Add(MyKeys.K);
			m_validKeyboardKeys.Add(MyKeys.L);
			m_validKeyboardKeys.Add(MyKeys.Left);
			m_validKeyboardKeys.Add(MyKeys.LeftAlt);
			m_validKeyboardKeys.Add(MyKeys.LeftControl);
			m_validKeyboardKeys.Add(MyKeys.LeftShift);
			m_validKeyboardKeys.Add(MyKeys.M);
			m_validKeyboardKeys.Add(MyKeys.Multiply);
			m_validKeyboardKeys.Add(MyKeys.N);
			m_validKeyboardKeys.Add(MyKeys.None);
			m_validKeyboardKeys.Add(MyKeys.NumPad0);
			m_validKeyboardKeys.Add(MyKeys.NumPad1);
			m_validKeyboardKeys.Add(MyKeys.NumPad2);
			m_validKeyboardKeys.Add(MyKeys.NumPad3);
			m_validKeyboardKeys.Add(MyKeys.NumPad4);
			m_validKeyboardKeys.Add(MyKeys.NumPad5);
			m_validKeyboardKeys.Add(MyKeys.NumPad6);
			m_validKeyboardKeys.Add(MyKeys.NumPad7);
			m_validKeyboardKeys.Add(MyKeys.NumPad8);
			m_validKeyboardKeys.Add(MyKeys.NumPad9);
			m_validKeyboardKeys.Add(MyKeys.O);
			m_validKeyboardKeys.Add(MyKeys.OemCloseBrackets);
			m_validKeyboardKeys.Add(MyKeys.OemComma);
			m_validKeyboardKeys.Add(MyKeys.OemMinus);
			m_validKeyboardKeys.Add(MyKeys.OemOpenBrackets);
			m_validKeyboardKeys.Add(MyKeys.OemPeriod);
			m_validKeyboardKeys.Add(MyKeys.OemPipe);
			m_validKeyboardKeys.Add(MyKeys.OemPlus);
			m_validKeyboardKeys.Add(MyKeys.OemQuestion);
			m_validKeyboardKeys.Add(MyKeys.OemQuotes);
			m_validKeyboardKeys.Add(MyKeys.OemSemicolon);
			m_validKeyboardKeys.Add(MyKeys.OemTilde);
			m_validKeyboardKeys.Add(MyKeys.OemBackslash);
			m_validKeyboardKeys.Add(MyKeys.P);
			m_validKeyboardKeys.Add(MyKeys.PageDown);
			m_validKeyboardKeys.Add(MyKeys.PageUp);
			m_validKeyboardKeys.Add(MyKeys.Pause);
			m_validKeyboardKeys.Add(MyKeys.Q);
			m_validKeyboardKeys.Add(MyKeys.R);
			m_validKeyboardKeys.Add(MyKeys.Right);
			m_validKeyboardKeys.Add(MyKeys.RightAlt);
			m_validKeyboardKeys.Add(MyKeys.RightControl);
			m_validKeyboardKeys.Add(MyKeys.RightShift);
			m_validKeyboardKeys.Add(MyKeys.Shift);
			m_validKeyboardKeys.Add(MyKeys.Alt);
			m_validKeyboardKeys.Add(MyKeys.S);
			m_validKeyboardKeys.Add(MyKeys.Space);
			m_validKeyboardKeys.Add(MyKeys.Subtract);
			m_validKeyboardKeys.Add(MyKeys.T);
			m_validKeyboardKeys.Add(MyKeys.Tab);
			m_validKeyboardKeys.Add(MyKeys.U);
			m_validKeyboardKeys.Add(MyKeys.Up);
			m_validKeyboardKeys.Add(MyKeys.V);
			m_validKeyboardKeys.Add(MyKeys.W);
			m_validKeyboardKeys.Add(MyKeys.X);
			m_validKeyboardKeys.Add(MyKeys.Y);
			m_validKeyboardKeys.Add(MyKeys.Z);
			m_validKeyboardKeys.Add(MyKeys.F1);
			m_validKeyboardKeys.Add(MyKeys.F2);
			m_validKeyboardKeys.Add(MyKeys.F3);
			m_validKeyboardKeys.Add(MyKeys.F4);
			m_validKeyboardKeys.Add(MyKeys.F5);
			m_validKeyboardKeys.Add(MyKeys.F6);
			m_validKeyboardKeys.Add(MyKeys.F7);
			m_validKeyboardKeys.Add(MyKeys.F8);
			m_validKeyboardKeys.Add(MyKeys.F9);
			m_validKeyboardKeys.Add(MyKeys.F10);
			m_validKeyboardKeys.Add(MyKeys.F11);
			m_validKeyboardKeys.Add(MyKeys.F12);
			m_validMouseButtons.Add(MyMouseButtonsEnum.Left);
			m_validMouseButtons.Add(MyMouseButtonsEnum.Middle);
			m_validMouseButtons.Add(MyMouseButtonsEnum.Right);
			m_validMouseButtons.Add(MyMouseButtonsEnum.XButton1);
			m_validMouseButtons.Add(MyMouseButtonsEnum.XButton2);
			m_validMouseButtons.Add(MyMouseButtonsEnum.None);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J01);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J02);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J03);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J04);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J05);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J06);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J07);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J08);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J09);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J10);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J11);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J12);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J13);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J14);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J15);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.J16);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.JDLeft);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.JDRight);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.JDUp);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.JDDown);
			m_validJoystickButtons.Add(MyJoystickButtonsEnum.None);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Xpos);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Xneg);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Ypos);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Yneg);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Zpos);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Zneg);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationXpos);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationXneg);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationYpos);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationYneg);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationZpos);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.RotationZneg);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Slider1pos);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Slider1neg);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Slider2pos);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.Slider2neg);
			m_validJoystickAxes.Add(MyJoystickAxesEnum.None);
			CheckValidControls(m_defaultGameControlsList);
			LoadControls(controlsGeneral, controlsButtons);
			InitializeJoystickIfPossible();
			TakeSnapshot();
			ClearBlacklist();
		}

		public void LoadContent()
		{
			MyVRage.Platform.CreateInput2();
			if (ENABLE_DEVELOPER_KEYS)
			{
				MyLog.Default.WriteLine("DEVELOPER KEYS ENABLED");
			}
			m_keyboardState = new MyGuiLocalizedKeyboardState(MyVRage.Platform.Input2);
		}

		public void UnloadData()
		{
		}

		public void NegateEscapePress()
		{
			m_keyboardState.NegateEscapePress();
		}

		private void CheckValidControls(Dictionary<MyStringId, MyControl> controls)
		{
			foreach (MyControl value in controls.Values)
			{
				_ = value;
			}
		}

		public List<string> EnumerateJoystickNames()
		{
			return m_joysticks;
		}

		private void InitializeJoystickIfPossible()
		{
			m_joysticks = MyVRage.Platform.Input2.EnumerateJoystickNames();
			SetConnectedJoystick(MyVRage.Platform.Input2.InitializeJoystickIfPossible(m_joystickInstanceName));
		}

		public void UpdateJoystickChanged()
		{
			InitializeJoystickIfPossible();
		}

		public void DeviceChangeCallback()
		{
			if (!MyVRage.Platform.Input2.IsJoystickConnected() || m_initializeJoystick)
			{
				m_initializeJoystick = false;
				InitializeJoystickIfPossible();
			}
			else
			{
				m_joysticks = MyVRage.Platform.Input2.EnumerateJoystickNames();
			}
		}

		public void ClearStates()
		{
			m_keyboardState.ClearStates();
			m_previousMouseState = m_actualMouseState;
			m_actualMouseState = default(MyMouseState);
			m_actualMouseStateRaw = default(MyMouseState);
			m_absoluteMousePosition = -Vector2.One;
		}

		public void UpdateStatesFromPlayback(MyKeyboardState currentKeyboard, MyKeyboardState previousKeyboard, MyMouseState currentMouse, MyMouseState previousMouse, MyJoystickState currentJoystick, MyJoystickState previousJoystick, int x, int y, List<char> keyboardSnapshotText)
		{
			m_keyboardState.UpdateStatesFromSnapshot(currentKeyboard, previousKeyboard);
			m_previousMouseState = previousMouse;
			m_actualMouseState = currentMouse;
			m_actualJoystickState = currentJoystick;
			m_previousJoystickState = previousJoystick;
			m_absoluteMousePosition = new Vector2(x, y);
			if (m_gameWasFocused)
			{
				m_platformInput.MousePosition = m_absoluteMousePosition;
			}
			if (keyboardSnapshotText != null)
			{
				foreach (char item in keyboardSnapshotText)
				{
					m_platformInput.AddChar(item);
				}
			}
		}

		public void UpdateStates()
		{
			if (!m_enabled)
			{
				return;
			}
			m_keyboardState.UpdateStates();
			m_hasher.Keys.Clear();
			GetPressedKeys(m_hasher.Keys);
			uint[] developerKeys = MyVRage.Platform.DeveloperKeys;
			if (!ENABLE_DEVELOPER_KEYS && m_hasher.TestHash(developerKeys[0], developerKeys[1], developerKeys[2], developerKeys[3], "salt!@#"))
			{
				ENABLE_DEVELOPER_KEYS = true;
				MyLog.Default.WriteLine("DEVELOPER KEYS ENABLED");
			}
			m_previousMouseState = m_actualMouseState;
			MyVRage.Platform.Input2.GetMouseState(out m_actualMouseStateRaw);
			int scrollWheelValue = m_actualMouseState.ScrollWheelValue + m_actualMouseStateRaw.ScrollWheelValue;
			m_actualMouseState = m_actualMouseStateRaw;
			m_actualMouseState.ScrollWheelValue = scrollWheelValue;
			m_absoluteMousePosition = m_platformInput.MousePosition;
			if (m_initializeJoystick)
			{
				DeviceChangeCallback();
			}
			if (IsJoystickConnected())
			{
				try
				{
					m_previousJoystickState = m_actualJoystickState;
					MyVRage.Platform.Input2.GetJoystickState(ref m_actualJoystickState);
					if (JoystickAsMouse)
					{
						float joystickAxisStateForGameplay = GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Xpos);
						float num = 0f - GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Xneg);
						float joystickAxisStateForGameplay2 = GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Ypos);
						float num2 = 0f - GetJoystickAxisStateForGameplay(MyJoystickAxesEnum.Yneg);
						float num3 = (joystickAxisStateForGameplay + num) * 4f;
						float num4 = (joystickAxisStateForGameplay2 + num2) * 4f;
						if (num3 != 0f || num4 != 0f)
						{
							m_absoluteMousePosition.X += num3;
							m_absoluteMousePosition.Y += num4;
							m_platformInput.MousePosition = m_absoluteMousePosition;
						}
					}
				}
				catch
				{
					SetConnectedJoystick(null);
				}
				if (!MyVRage.Platform.Input2.IsJoystickConnected())
				{
					SetConnectedJoystick(null);
				}
			}
			else
			{
				ResetJoystickState();
			}
			if (IsJoystickLastUsed)
			{
				if (IsAnyMousePressed() || IsAnyKeyPress() || IsMouseMoved())
				{
					IsJoystickLastUsed = false;
				}
			}
			else if (IsAnyJoystickButtonPressed() || IsAnyJoystickAxisPressed())
			{
				IsJoystickLastUsed = true;
			}
		}

		private void ResetJoystickState()
		{
			MyJoystickState actualJoystickState = new MyJoystickState
			{
				X = 32768,
				Y = 32768,
				RotationX = 32768,
				RotationY = 32768
			};
			actualJoystickState = (m_previousJoystickState = (m_actualJoystickState = actualJoystickState));
		}

		public bool Update(bool gameFocused)
		{
			if (!m_gameWasFocused && gameFocused && !m_overrideUpdate)
			{
				UpdateStates();
			}
			m_gameWasFocused = gameFocused;
			if (!gameFocused && !m_overrideUpdate)
			{
				ClearStates();
				return false;
			}
			if (!m_overrideUpdate)
			{
				UpdateStates();
			}
			m_platformInput.GetBufferedTextInput(ref m_currentTextInput);
			return true;
		}

		public bool IsAnyKeyPress()
		{
			return m_keyboardState.IsAnyKeyPressed();
		}

		public bool IsAnyNewKeyPress()
		{
			if (m_keyboardState.IsAnyKeyPressed())
			{
				return !m_keyboardState.GetPreviousKeyboardState().IsAnyKeyPressed();
			}
			return false;
		}

		public bool IsAnyMousePressed()
		{
			if (!m_actualMouseState.LeftButton && !m_actualMouseState.MiddleButton && !m_actualMouseState.RightButton && !m_actualMouseState.XButton1)
			{
				return m_actualMouseState.XButton2;
			}
			return true;
		}

		public bool IsAnyNewMousePressed()
		{
			if (!IsNewLeftMousePressed() && !IsNewMiddleMousePressed() && !IsNewRightMousePressed() && !IsNewXButton1MousePressed())
			{
				return IsNewXButton2MousePressed();
			}
			return true;
		}

		private unsafe bool IsAnyJoystickButtonPressed()
		{
			if (m_joystickConnected)
			{
				if (IsGamepadKeyDownPressed() || IsGamepadKeyLeftPressed() || IsGamepadKeyRightPressed() || IsGamepadKeyUpPressed())
				{
					return true;
				}
				for (int i = 0; i < 16; i++)
				{
					if (m_actualJoystickState.Buttons[i] > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		private unsafe bool IsAnyNewJoystickButtonPressed()
		{
			if (m_joystickConnected)
			{
				for (int i = 0; i < 16; i++)
				{
					if (m_actualJoystickState.Buttons[i] > 0 && m_previousJoystickState.Buttons[i] == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsNewGameControlJoystickOnlyPressed(MyStringId controlId)
		{
			if (IsControlBlocked(controlId))
			{
				return false;
			}
			if (m_gameControlsList.TryGetValue(controlId, out MyControl value))
			{
				return value.IsNewJoystickPressed();
			}
			return false;
		}

		public string TokenEvaluate(string token, string context)
		{
			if (m_gameControlsList.TryGetValue(MyStringId.GetOrCompute(token), out MyControl value))
			{
				string controlButtonName = value.GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
				string controlButtonName2 = value.GetControlButtonName(MyGuiInputDeviceEnum.Mouse);
				if (!string.IsNullOrEmpty(controlButtonName))
				{
					if (!string.IsNullOrEmpty(controlButtonName2))
					{
						return controlButtonName + "'/'" + controlButtonName2;
					}
					return controlButtonName;
				}
				return controlButtonName2;
			}
			return "";
		}

		public static object GetHighlightedControl(MyStringId controlId)
		{
			string text = (MyInput.Static.GetGameControl(controlId) != null) ? MyInput.Static.GetGameControl(controlId).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) : null;
			string text2 = (MyInput.Static.GetGameControl(controlId) != null) ? MyInput.Static.GetGameControl(controlId).GetControlButtonName(MyGuiInputDeviceEnum.Mouse) : null;
			if (!string.IsNullOrEmpty(text))
			{
				if (!string.IsNullOrEmpty(text2))
				{
					return "[" + text + "'/'" + text2 + "]";
				}
				return "[" + text + "]";
			}
			return "[" + text2 + "]";
		}

		public bool IsGameControlJoystickOnlyPressed(MyStringId controlId)
		{
			if (IsControlBlocked(controlId))
			{
				return false;
			}
			if (m_gameControlsList.TryGetValue(controlId, out MyControl value))
			{
				return value.IsJoystickPressed();
			}
			return false;
		}

		public bool IsNewGameControlJoystickOnlyReleased(MyStringId controlId)
		{
			if (IsControlBlocked(controlId))
			{
				return false;
			}
			if (m_gameControlsList.TryGetValue(controlId, out MyControl value))
			{
				return value.IsNewJoystickReleased();
			}
			return false;
		}

		private bool IsAnyJoystickAxisPressed()
		{
			if (m_joystickConnected)
			{
				foreach (MyJoystickAxesEnum validJoystickAxis in m_validJoystickAxes)
				{
					if (validJoystickAxis != 0 && IsJoystickAxisPressed(validJoystickAxis))
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsAnyMouseOrJoystickPressed()
		{
			if (!IsAnyMousePressed())
			{
				return IsAnyJoystickButtonPressed();
			}
			return true;
		}

		public bool IsAnyNewMouseOrJoystickPressed()
		{
			if (!IsAnyNewMousePressed())
			{
				return IsAnyNewJoystickButtonPressed();
			}
			return true;
		}

		public bool IsNewPrimaryButtonPressed()
		{
			if (!IsNewLeftMousePressed())
			{
				return IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J01);
			}
			return true;
		}

		public bool IsNewSecondaryButtonPressed()
		{
			if (!IsNewRightMousePressed())
			{
				return IsJoystickButtonNewPressed(MyJoystickButtonsEnum.J02);
			}
			return true;
		}

		public bool IsNewPrimaryButtonReleased()
		{
			if (!IsNewLeftMouseReleased())
			{
				return IsNewJoystickButtonReleased(MyJoystickButtonsEnum.J01);
			}
			return true;
		}

		public bool IsNewSecondaryButtonReleased()
		{
			if (!IsNewRightMouseReleased())
			{
				return IsNewJoystickButtonReleased(MyJoystickButtonsEnum.J02);
			}
			return true;
		}

		public bool IsPrimaryButtonReleased()
		{
			if (!IsLeftMouseReleased())
			{
				return IsJoystickButtonReleased(MyJoystickButtonsEnum.J01);
			}
			return true;
		}

		public bool IsSecondaryButtonReleased()
		{
			if (!IsRightMouseReleased())
			{
				return IsJoystickButtonReleased(MyJoystickButtonsEnum.J02);
			}
			return true;
		}

		public bool IsPrimaryButtonPressed()
		{
			if (!IsLeftMousePressed())
			{
				return IsJoystickButtonPressed(MyJoystickButtonsEnum.J01);
			}
			return true;
		}

		public bool IsSecondaryButtonPressed()
		{
			if (!IsRightMousePressed())
			{
				return IsJoystickButtonPressed(MyJoystickButtonsEnum.J02);
			}
			return true;
		}

		public bool IsNewButtonPressed(MySharedButtonsEnum button)
		{
			switch (button)
			{
			case MySharedButtonsEnum.Primary:
				return IsNewPrimaryButtonPressed();
			case MySharedButtonsEnum.Secondary:
				return IsNewSecondaryButtonPressed();
			default:
				return false;
			}
		}

		public bool IsButtonPressed(MySharedButtonsEnum button)
		{
			switch (button)
			{
			case MySharedButtonsEnum.Primary:
				return IsPrimaryButtonPressed();
			case MySharedButtonsEnum.Secondary:
				return IsSecondaryButtonPressed();
			default:
				return false;
			}
		}

		public bool IsNewButtonReleased(MySharedButtonsEnum button)
		{
			switch (button)
			{
			case MySharedButtonsEnum.Primary:
				return IsNewPrimaryButtonReleased();
			case MySharedButtonsEnum.Secondary:
				return IsNewSecondaryButtonReleased();
			default:
				return false;
			}
		}

		public bool IsButtonReleased(MySharedButtonsEnum button)
		{
			switch (button)
			{
			case MySharedButtonsEnum.Primary:
				return IsPrimaryButtonReleased();
			case MySharedButtonsEnum.Secondary:
				return IsSecondaryButtonReleased();
			default:
				return false;
			}
		}

		public bool IsAnyWinKeyPressed()
		{
			if (!IsKeyPress(MyKeys.LeftWindows))
			{
				return IsKeyPress(MyKeys.RightWindows);
			}
			return true;
		}

		public bool IsAnyShiftKeyPressed()
		{
			if (!IsKeyPress(MyKeys.Shift) && !IsKeyPress(MyKeys.LeftShift))
			{
				return IsKeyPress(MyKeys.RightShift);
			}
			return true;
		}

		public bool IsAnyAltKeyPressed()
		{
			if (!IsKeyPress(MyKeys.Alt) && !IsKeyPress(MyKeys.LeftAlt))
			{
				return IsKeyPress(MyKeys.RightAlt);
			}
			return true;
		}

		public bool IsAnyCtrlKeyPressed()
		{
			if (!IsKeyPress(MyKeys.Control) && !IsKeyPress(MyKeys.LeftControl))
			{
				return IsKeyPress(MyKeys.RightControl);
			}
			return true;
		}

		public void GetPressedKeys(List<MyKeys> keys)
		{
			m_keyboardState.GetActualPressedKeys(keys);
		}

		public bool IsKeyPress(MyKeys key)
		{
			return m_keyboardState.IsKeyDown(key);
		}

		public bool WasKeyPress(MyKeys key)
		{
			return m_keyboardState.IsPreviousKeyDown(key);
		}

		public bool IsNewKeyPressed(MyKeys key)
		{
			if (m_keyboardState.IsKeyDown(key))
			{
				return m_keyboardState.IsPreviousKeyUp(key);
			}
			return false;
		}

		public bool IsNewKeyReleased(MyKeys key)
		{
			if (m_keyboardState.IsKeyUp(key))
			{
				return m_keyboardState.IsPreviousKeyDown(key);
			}
			return false;
		}

		public bool IsMousePressed(MyMouseButtonsEnum button)
		{
			switch (button)
			{
			case MyMouseButtonsEnum.Left:
				return IsLeftMousePressed();
			case MyMouseButtonsEnum.Middle:
				return IsMiddleMousePressed();
			case MyMouseButtonsEnum.Right:
				return IsRightMousePressed();
			case MyMouseButtonsEnum.XButton1:
				return IsXButton1MousePressed();
			case MyMouseButtonsEnum.XButton2:
				return IsXButton2MousePressed();
			default:
				return false;
			}
		}

		public bool IsMouseReleased(MyMouseButtonsEnum button)
		{
			switch (button)
			{
			case MyMouseButtonsEnum.Left:
				return IsLeftMouseReleased();
			case MyMouseButtonsEnum.Middle:
				return IsMiddleMouseReleased();
			case MyMouseButtonsEnum.Right:
				return IsRightMouseReleased();
			case MyMouseButtonsEnum.XButton1:
				return IsXButton1MouseReleased();
			case MyMouseButtonsEnum.XButton2:
				return IsXButton2MouseReleased();
			default:
				return false;
			}
		}

		public bool WasMousePressed(MyMouseButtonsEnum button)
		{
			switch (button)
			{
			case MyMouseButtonsEnum.Left:
				return WasLeftMousePressed();
			case MyMouseButtonsEnum.Middle:
				return WasMiddleMousePressed();
			case MyMouseButtonsEnum.Right:
				return WasRightMousePressed();
			case MyMouseButtonsEnum.XButton1:
				return WasXButton1MousePressed();
			case MyMouseButtonsEnum.XButton2:
				return WasXButton2MousePressed();
			default:
				return false;
			}
		}

		public bool WasMouseReleased(MyMouseButtonsEnum button)
		{
			switch (button)
			{
			case MyMouseButtonsEnum.Left:
				return WasLeftMouseReleased();
			case MyMouseButtonsEnum.Middle:
				return WasMiddleMouseReleased();
			case MyMouseButtonsEnum.Right:
				return WasRightMouseReleased();
			case MyMouseButtonsEnum.XButton1:
				return WasXButton1MouseReleased();
			case MyMouseButtonsEnum.XButton2:
				return WasXButton2MouseReleased();
			default:
				return false;
			}
		}

		public bool IsNewMousePressed(MyMouseButtonsEnum button)
		{
			switch (button)
			{
			case MyMouseButtonsEnum.Left:
				return IsNewLeftMousePressed();
			case MyMouseButtonsEnum.Middle:
				return IsNewMiddleMousePressed();
			case MyMouseButtonsEnum.Right:
				return IsNewRightMousePressed();
			case MyMouseButtonsEnum.XButton1:
				return IsNewXButton1MousePressed();
			case MyMouseButtonsEnum.XButton2:
				return IsNewXButton2MousePressed();
			default:
				return false;
			}
		}

		public bool IsNewMouseReleased(MyMouseButtonsEnum button)
		{
			switch (button)
			{
			case MyMouseButtonsEnum.Left:
				return IsNewLeftMouseReleased();
			case MyMouseButtonsEnum.Middle:
				return IsNewMiddleMouseReleased();
			case MyMouseButtonsEnum.Right:
				return IsNewRightMouseReleased();
			case MyMouseButtonsEnum.XButton1:
				return IsNewXButton1MouseReleased();
			case MyMouseButtonsEnum.XButton2:
				return IsNewXButton2MouseReleased();
			default:
				return false;
			}
		}

		public bool IsNewLeftMousePressed()
		{
			if (IsLeftMousePressed())
			{
				return WasLeftMouseReleased();
			}
			return false;
		}

		public bool IsNewLeftMouseReleased()
		{
			if (IsLeftMouseReleased())
			{
				return WasLeftMousePressed();
			}
			return false;
		}

		public bool IsLeftMousePressed()
		{
			return m_actualMouseState.LeftButton;
		}

		public bool IsLeftMouseReleased()
		{
			return !m_actualMouseState.LeftButton;
		}

		public bool WasLeftMouseReleased()
		{
			return !m_previousMouseState.LeftButton;
		}

		public bool WasLeftMousePressed()
		{
			return m_previousMouseState.LeftButton;
		}

		public bool IsRightMousePressed()
		{
			return m_actualMouseState.RightButton;
		}

		public bool IsRightMouseReleased()
		{
			return !m_actualMouseState.RightButton;
		}

		public bool IsNewRightMousePressed()
		{
			if (m_actualMouseState.RightButton)
			{
				return !m_previousMouseState.RightButton;
			}
			return false;
		}

		public bool IsNewRightMouseReleased()
		{
			if (!m_actualMouseState.RightButton)
			{
				return m_previousMouseState.RightButton;
			}
			return false;
		}

		public bool WasRightMousePressed()
		{
			return m_previousMouseState.RightButton;
		}

		public bool WasRightMouseReleased()
		{
			return !m_previousMouseState.RightButton;
		}

		public bool IsMiddleMousePressed()
		{
			return m_actualMouseState.MiddleButton;
		}

		public bool IsMiddleMouseReleased()
		{
			return !m_actualMouseState.MiddleButton;
		}

		public bool IsNewMiddleMousePressed()
		{
			if (m_actualMouseState.MiddleButton)
			{
				return !m_previousMouseState.MiddleButton;
			}
			return false;
		}

		public bool IsNewMiddleMouseReleased()
		{
			if (!m_actualMouseState.MiddleButton)
			{
				return m_previousMouseState.MiddleButton;
			}
			return false;
		}

		public bool WasMiddleMousePressed()
		{
			return m_previousMouseState.MiddleButton;
		}

		public bool WasMiddleMouseReleased()
		{
			return !m_previousMouseState.MiddleButton;
		}

		public bool IsXButton1MousePressed()
		{
			return m_actualMouseState.XButton1;
		}

		public bool IsXButton1MouseReleased()
		{
			return !m_actualMouseState.XButton1;
		}

		public bool IsNewXButton1MousePressed()
		{
			if (m_actualMouseState.XButton1)
			{
				return !m_previousMouseState.XButton1;
			}
			return false;
		}

		public bool IsNewXButton1MouseReleased()
		{
			if (!m_actualMouseState.XButton1)
			{
				return m_previousMouseState.XButton1;
			}
			return false;
		}

		public bool WasXButton1MousePressed()
		{
			return m_previousMouseState.XButton1;
		}

		public bool WasXButton1MouseReleased()
		{
			return !m_previousMouseState.XButton1;
		}

		public bool IsXButton2MousePressed()
		{
			return m_actualMouseState.XButton2;
		}

		public bool IsXButton2MouseReleased()
		{
			return !m_actualMouseState.XButton2;
		}

		public bool IsNewXButton2MousePressed()
		{
			if (m_actualMouseState.XButton2)
			{
				return !m_previousMouseState.XButton2;
			}
			return false;
		}

		public bool IsNewXButton2MouseReleased()
		{
			if (!m_actualMouseState.XButton2)
			{
				return m_previousMouseState.XButton2;
			}
			return false;
		}

		public bool WasXButton2MousePressed()
		{
			return m_previousMouseState.XButton2;
		}

		public bool WasXButton2MouseReleased()
		{
			return !m_previousMouseState.XButton2;
		}

		public unsafe bool IsJoystickButtonPressed(MyJoystickButtonsEnum button)
		{
			bool flag = false;
			if (m_joystickConnected)
			{
				switch (button)
				{
				case MyJoystickButtonsEnum.JDLeft:
					flag = IsGamepadKeyLeftPressed();
					break;
				case MyJoystickButtonsEnum.JDRight:
					flag = IsGamepadKeyRightPressed();
					break;
				case MyJoystickButtonsEnum.JDUp:
					flag = IsGamepadKeyUpPressed();
					break;
				case MyJoystickButtonsEnum.JDDown:
					flag = IsGamepadKeyDownPressed();
					break;
				default:
					flag = (m_actualJoystickState.Buttons[button - 5] > 0);
					break;
				case MyJoystickButtonsEnum.None:
					break;
				}
			}
			if (!flag && button == MyJoystickButtonsEnum.None)
			{
				return true;
			}
			return flag;
		}

		public bool IsJoystickButtonNewPressed(MyJoystickButtonsEnum button)
		{
			bool flag = false;
			if (m_joystickConnected)
			{
				switch (button)
				{
				case MyJoystickButtonsEnum.JDLeft:
					flag = IsNewGamepadKeyLeftPressed();
					break;
				case MyJoystickButtonsEnum.JDRight:
					flag = IsNewGamepadKeyRightPressed();
					break;
				case MyJoystickButtonsEnum.JDUp:
					flag = IsNewGamepadKeyUpPressed();
					break;
				case MyJoystickButtonsEnum.JDDown:
					flag = IsNewGamepadKeyDownPressed();
					break;
				default:
					flag = (m_actualJoystickState.IsPressed((int)(button - 5)) && !m_previousJoystickState.IsPressed((int)(button - 5)));
					break;
				case MyJoystickButtonsEnum.None:
					break;
				}
			}
			if (!flag && button == MyJoystickButtonsEnum.None)
			{
				return true;
			}
			return flag;
		}

		public bool IsNewJoystickButtonReleased(MyJoystickButtonsEnum button)
		{
			bool flag = false;
			if (m_joystickConnected)
			{
				switch (button)
				{
				case MyJoystickButtonsEnum.JDLeft:
					flag = IsNewGamepadKeyLeftReleased();
					break;
				case MyJoystickButtonsEnum.JDRight:
					flag = IsNewGamepadKeyRightReleased();
					break;
				case MyJoystickButtonsEnum.JDUp:
					flag = IsNewGamepadKeyUpReleased();
					break;
				case MyJoystickButtonsEnum.JDDown:
					flag = IsNewGamepadKeyDownReleased();
					break;
				default:
					flag = (m_actualJoystickState.IsReleased((int)(button - 5)) && m_previousJoystickState.IsPressed((int)(button - 5)));
					break;
				case MyJoystickButtonsEnum.None:
					break;
				}
			}
			if (!flag && button == MyJoystickButtonsEnum.None)
			{
				return true;
			}
			return flag;
		}

		public bool IsJoystickButtonReleased(MyJoystickButtonsEnum button)
		{
			bool flag = false;
			if (m_joystickConnected)
			{
				switch (button)
				{
				case MyJoystickButtonsEnum.JDLeft:
					flag = !IsGamepadKeyLeftPressed();
					break;
				case MyJoystickButtonsEnum.JDRight:
					flag = !IsGamepadKeyRightPressed();
					break;
				case MyJoystickButtonsEnum.JDUp:
					flag = !IsGamepadKeyUpPressed();
					break;
				case MyJoystickButtonsEnum.JDDown:
					flag = !IsGamepadKeyDownPressed();
					break;
				default:
					flag = m_actualJoystickState.IsReleased((int)(button - 5));
					break;
				case MyJoystickButtonsEnum.None:
					break;
				}
			}
			if (!flag && button == MyJoystickButtonsEnum.None)
			{
				return true;
			}
			return flag;
		}

		public unsafe bool WasJoystickButtonPressed(MyJoystickButtonsEnum button)
		{
			bool flag = false;
			if (m_joystickConnected)
			{
				switch (button)
				{
				case MyJoystickButtonsEnum.JDLeft:
					flag = WasGamepadKeyLeftPressed();
					break;
				case MyJoystickButtonsEnum.JDRight:
					flag = WasGamepadKeyRightPressed();
					break;
				case MyJoystickButtonsEnum.JDUp:
					flag = WasGamepadKeyUpPressed();
					break;
				case MyJoystickButtonsEnum.JDDown:
					flag = WasGamepadKeyDownPressed();
					break;
				default:
					flag = (m_previousJoystickState.Buttons[button - 5] > 0);
					break;
				case MyJoystickButtonsEnum.None:
					break;
				}
			}
			if (!flag && button == MyJoystickButtonsEnum.None)
			{
				return true;
			}
			return flag;
		}

		public bool WasJoystickButtonReleased(MyJoystickButtonsEnum button)
		{
			bool flag = false;
			if (m_joystickConnected)
			{
				switch (button)
				{
				case MyJoystickButtonsEnum.JDLeft:
					flag = !WasGamepadKeyLeftPressed();
					break;
				case MyJoystickButtonsEnum.JDRight:
					flag = !WasGamepadKeyRightPressed();
					break;
				case MyJoystickButtonsEnum.JDUp:
					flag = !WasGamepadKeyUpPressed();
					break;
				case MyJoystickButtonsEnum.JDDown:
					flag = !WasGamepadKeyDownPressed();
					break;
				default:
					flag = m_previousJoystickState.IsReleased((int)(button - 5));
					break;
				case MyJoystickButtonsEnum.None:
					break;
				}
			}
			if (!flag && button == MyJoystickButtonsEnum.None)
			{
				return true;
			}
			return flag;
		}

		public unsafe float GetJoystickAxisStateRaw(MyJoystickAxesEnum axis)
		{
			int num = 32768;
			if (m_joystickConnected && axis != 0 && IsJoystickAxisSupported(axis))
			{
				switch (axis)
				{
				case MyJoystickAxesEnum.RotationXpos:
				case MyJoystickAxesEnum.RotationXneg:
					num = m_actualJoystickState.RotationX;
					break;
				case MyJoystickAxesEnum.RotationYpos:
				case MyJoystickAxesEnum.RotationYneg:
					num = m_actualJoystickState.RotationY;
					break;
				case MyJoystickAxesEnum.RotationZpos:
				case MyJoystickAxesEnum.RotationZneg:
					num = m_actualJoystickState.RotationZ;
					break;
				case MyJoystickAxesEnum.Xpos:
				case MyJoystickAxesEnum.Xneg:
					num = m_actualJoystickState.X;
					break;
				case MyJoystickAxesEnum.Ypos:
				case MyJoystickAxesEnum.Yneg:
					num = m_actualJoystickState.Y;
					break;
				case MyJoystickAxesEnum.Zpos:
				case MyJoystickAxesEnum.Zneg:
					num = m_actualJoystickState.Z;
					break;
				case MyJoystickAxesEnum.Slider1pos:
				case MyJoystickAxesEnum.Slider1neg:
					num = *m_actualJoystickState.Sliders;
					break;
				case MyJoystickAxesEnum.Slider2pos:
				case MyJoystickAxesEnum.Slider2neg:
					num = m_actualJoystickState.Sliders[1];
					break;
				}
			}
			return num;
		}

		public unsafe float GetPreviousJoystickAxisStateRaw(MyJoystickAxesEnum axis)
		{
			int num = 32768;
			if (m_joystickConnected && axis != 0 && IsJoystickAxisSupported(axis))
			{
				switch (axis)
				{
				case MyJoystickAxesEnum.RotationXpos:
				case MyJoystickAxesEnum.RotationXneg:
					num = m_previousJoystickState.RotationX;
					break;
				case MyJoystickAxesEnum.RotationYpos:
				case MyJoystickAxesEnum.RotationYneg:
					num = m_previousJoystickState.RotationY;
					break;
				case MyJoystickAxesEnum.RotationZpos:
				case MyJoystickAxesEnum.RotationZneg:
					num = m_previousJoystickState.RotationZ;
					break;
				case MyJoystickAxesEnum.Xpos:
				case MyJoystickAxesEnum.Xneg:
					num = m_previousJoystickState.X;
					break;
				case MyJoystickAxesEnum.Ypos:
				case MyJoystickAxesEnum.Yneg:
					num = m_previousJoystickState.Y;
					break;
				case MyJoystickAxesEnum.Zpos:
				case MyJoystickAxesEnum.Zneg:
					num = m_previousJoystickState.Z;
					break;
				case MyJoystickAxesEnum.Slider1pos:
				case MyJoystickAxesEnum.Slider1neg:
					num = *m_previousJoystickState.Sliders;
					break;
				case MyJoystickAxesEnum.Slider2pos:
				case MyJoystickAxesEnum.Slider2neg:
					num = m_previousJoystickState.Sliders[1];
					break;
				}
			}
			return num;
		}

		public float GetJoystickX()
		{
			return GetJoystickAxisStateRaw(MyJoystickAxesEnum.Xpos);
		}

		public float GetJoystickY()
		{
			return GetJoystickAxisStateRaw(MyJoystickAxesEnum.Ypos);
		}

		public float GetJoystickAxisStateForGameplay(MyJoystickAxesEnum axis)
		{
			if (m_joystickConnected && IsJoystickAxisSupported(axis))
			{
				float num = (GetJoystickAxisStateRaw(axis) - 32767f) / 32767f;
				switch (axis)
				{
				case MyJoystickAxesEnum.Xneg:
				case MyJoystickAxesEnum.Yneg:
				case MyJoystickAxesEnum.Zneg:
				case MyJoystickAxesEnum.RotationXneg:
				case MyJoystickAxesEnum.RotationYneg:
				case MyJoystickAxesEnum.RotationZneg:
				case MyJoystickAxesEnum.Slider1neg:
				case MyJoystickAxesEnum.Slider2neg:
					if (num >= 0f)
					{
						return 0f;
					}
					break;
				case MyJoystickAxesEnum.Xpos:
				case MyJoystickAxesEnum.Ypos:
				case MyJoystickAxesEnum.Zpos:
				case MyJoystickAxesEnum.RotationXpos:
				case MyJoystickAxesEnum.RotationYpos:
				case MyJoystickAxesEnum.RotationZpos:
				case MyJoystickAxesEnum.Slider1pos:
				case MyJoystickAxesEnum.Slider2pos:
					if (num <= 0f)
					{
						return 0f;
					}
					break;
				}
				float num2 = Math.Abs(num);
				if (num2 > m_joystickDeadzone)
				{
					num2 = (num2 - m_joystickDeadzone) / (1f - m_joystickDeadzone);
					return m_joystickSensitivity * (float)Math.Pow(num2, m_joystickExponent);
				}
			}
			return 0f;
		}

		public float GetPreviousJoystickAxisStateForGameplay(MyJoystickAxesEnum axis)
		{
			if (m_joystickConnected && IsJoystickAxisSupported(axis))
			{
				float num = (GetPreviousJoystickAxisStateRaw(axis) - 32767f) / 32767f;
				switch (axis)
				{
				case MyJoystickAxesEnum.Xneg:
				case MyJoystickAxesEnum.Yneg:
				case MyJoystickAxesEnum.Zneg:
				case MyJoystickAxesEnum.RotationXneg:
				case MyJoystickAxesEnum.RotationYneg:
				case MyJoystickAxesEnum.RotationZneg:
				case MyJoystickAxesEnum.Slider1neg:
				case MyJoystickAxesEnum.Slider2neg:
					if (num >= 0f)
					{
						return 0f;
					}
					break;
				case MyJoystickAxesEnum.Xpos:
				case MyJoystickAxesEnum.Ypos:
				case MyJoystickAxesEnum.Zpos:
				case MyJoystickAxesEnum.RotationXpos:
				case MyJoystickAxesEnum.RotationYpos:
				case MyJoystickAxesEnum.RotationZpos:
				case MyJoystickAxesEnum.Slider1pos:
				case MyJoystickAxesEnum.Slider2pos:
					if (num <= 0f)
					{
						return 0f;
					}
					break;
				}
				float num2 = Math.Abs(num);
				if (num2 > m_joystickDeadzone)
				{
					num2 = (num2 - m_joystickDeadzone) / (1f - m_joystickDeadzone);
					return m_joystickSensitivity * (float)Math.Pow(num2, m_joystickExponent);
				}
			}
			return 0f;
		}

		public Vector3 GetJoystickPositionForGameplay(RequestedJoystickAxis requestedAxis)
		{
			Vector3 input = new Vector3(m_actualJoystickState.X, m_actualJoystickState.Y, m_actualJoystickState.Z) - 32767f;
			return FilterAndNormalizeJoystickInput(input, requestedAxis);
		}

		public Vector3 GetJoystickRotationForGameplay(RequestedJoystickAxis requestedAxis)
		{
			Vector3 input = new Vector3(m_actualJoystickState.RotationX - 32767, m_actualJoystickState.RotationY - 32767, m_actualJoystickState.RotationZ);
			return FilterAndNormalizeJoystickInput(input, requestedAxis);
		}

		private Vector3 FilterAndNormalizeJoystickInput(Vector3 input, RequestedJoystickAxis requestedAxis)
		{
			input /= 32767f;
			if ((requestedAxis & RequestedJoystickAxis.X) == 0)
			{
				input.X = 0f;
			}
			if ((requestedAxis & RequestedJoystickAxis.Y) == 0)
			{
				input.Y = 0f;
			}
			if ((requestedAxis & RequestedJoystickAxis.Z) == 0)
			{
				input.Z = 0f;
			}
			float num = input.Length();
			if (num <= m_joystickDeadzone)
			{
				return Vector3.Zero;
			}
			float num2 = num;
			num2 = (num2 - m_joystickDeadzone) / (1f - m_joystickDeadzone);
			num2 = (float)Math.Pow(num2, m_joystickExponent);
			return m_joystickSensitivity * num2 / num * input;
		}

		public bool IsJoystickAxisPressed(MyJoystickAxesEnum axis)
		{
			bool flag = false;
			if (m_joystickConnected && axis != 0)
			{
				flag = (GetJoystickAxisStateForGameplay(axis) > 0.5f);
			}
			if (!flag && axis == MyJoystickAxesEnum.None)
			{
				return true;
			}
			if (!IsJoystickAxisSupported(axis))
			{
				return false;
			}
			return flag;
		}

		public bool IsJoystickAxisNewPressed(MyJoystickAxesEnum axis)
		{
			bool flag = false;
			if (m_joystickConnected && axis != 0)
			{
				float joystickAxisStateForGameplay = GetJoystickAxisStateForGameplay(axis);
				float previousJoystickAxisStateForGameplay = GetPreviousJoystickAxisStateForGameplay(axis);
				flag = (joystickAxisStateForGameplay > 0.5f && previousJoystickAxisStateForGameplay <= 0.5f);
			}
			if (!flag && axis == MyJoystickAxesEnum.None)
			{
				return true;
			}
			if (!IsJoystickAxisSupported(axis))
			{
				return false;
			}
			return flag;
		}

		public bool IsNewJoystickAxisReleased(MyJoystickAxesEnum axis)
		{
			bool flag = false;
			if (m_joystickConnected && axis != 0)
			{
				flag = (GetJoystickAxisStateForGameplay(axis) <= 0.5f && GetPreviousJoystickAxisStateForGameplay(axis) > 0.5f);
			}
			if (!flag && axis == MyJoystickAxesEnum.None)
			{
				return true;
			}
			if (!IsJoystickAxisSupported(axis))
			{
				return false;
			}
			return flag;
		}

		public bool IsJoystickAxisReleased(MyJoystickAxesEnum axis)
		{
			bool flag = false;
			if (m_joystickConnected && axis != 0)
			{
				flag = (GetJoystickAxisStateForGameplay(axis) <= 0.5f);
			}
			if (!flag && axis == MyJoystickAxesEnum.None)
			{
				return true;
			}
			if (!IsJoystickAxisSupported(axis))
			{
				return false;
			}
			return flag;
		}

		public bool WasJoystickAxisPressed(MyJoystickAxesEnum axis)
		{
			bool flag = false;
			if (m_joystickConnected && axis != 0)
			{
				flag = (GetPreviousJoystickAxisStateForGameplay(axis) > 0.5f);
			}
			if (!flag && axis == MyJoystickAxesEnum.None)
			{
				return true;
			}
			if (!IsJoystickAxisSupported(axis))
			{
				return false;
			}
			return flag;
		}

		public bool WasJoystickAxisReleased(MyJoystickAxesEnum axis)
		{
			bool flag = false;
			if (m_joystickConnected && axis != 0)
			{
				flag = (GetPreviousJoystickAxisStateForGameplay(axis) <= 0.5f);
			}
			if (!flag && axis == MyJoystickAxesEnum.None)
			{
				return true;
			}
			if (!IsJoystickAxisSupported(axis))
			{
				return false;
			}
			return flag;
		}

		public float GetJoystickSensitivity()
		{
			return m_joystickSensitivity;
		}

		public void SetJoystickSensitivity(float newSensitivity)
		{
			m_joystickSensitivity = newSensitivity;
		}

		public float GetJoystickExponent()
		{
			return m_joystickExponent;
		}

		public void SetJoystickExponent(float newExponent)
		{
			m_joystickExponent = newExponent;
		}

		public float GetJoystickDeadzone()
		{
			return m_joystickDeadzone;
		}

		public void SetJoystickDeadzone(float newDeadzone)
		{
			m_joystickDeadzone = newDeadzone;
		}

		public int MouseScrollWheelValue()
		{
			return m_actualMouseState.ScrollWheelValue;
		}

		public int PreviousMouseScrollWheelValue()
		{
			return m_previousMouseState.ScrollWheelValue;
		}

		public int DeltaMouseScrollWheelValue()
		{
			return MouseScrollWheelValue() - PreviousMouseScrollWheelValue();
		}

		public int GetMouseX()
		{
			return m_actualMouseState.X;
		}

		public int GetMouseY()
		{
			return m_actualMouseState.Y;
		}

		public int GetMouseXForGamePlay()
		{
			int num = (!m_mouseXIsInverted) ? 1 : (-1);
			return (int)(m_mouseSensitivity * (float)(num * m_actualMouseState.X));
		}

		public int GetMouseYForGamePlay()
		{
			int num = (!m_mouseYIsInverted) ? 1 : (-1);
			return (int)(m_mouseSensitivity * (float)(num * m_actualMouseState.Y));
		}

		public float GetMouseXForGamePlayF()
		{
			float num = m_mouseXIsInverted ? (-1f) : 1f;
			return m_mouseSensitivity * (num * (float)m_actualMouseState.X);
		}

		public float GetMouseYForGamePlayF()
		{
			float num = m_mouseYIsInverted ? (-1f) : 1f;
			return m_mouseSensitivity * (num * (float)m_actualMouseState.Y);
		}

		public bool GetMouseXInversion()
		{
			return m_mouseXIsInverted;
		}

		public bool GetMouseYInversion()
		{
			return m_mouseYIsInverted;
		}

		public void SetMouseXInversion(bool inverted)
		{
			m_mouseXIsInverted = inverted;
		}

		public void SetMouseYInversion(bool inverted)
		{
			m_mouseYIsInverted = inverted;
		}

		public float GetMouseSensitivity()
		{
			return m_mouseSensitivity;
		}

		public void SetMouseSensitivity(float sensitivity)
		{
			m_mouseSensitivity = sensitivity;
		}

		/// <summary>
		/// Returns immediatelly current cursor position.
		/// Obtains position on every call, it can get cursor data with higher rate than 60 fps
		/// </summary>
		public Vector2 GetMousePosition()
		{
			return m_absoluteMousePosition;
		}

		public Vector2 GetMouseAreaSize()
		{
			return m_platformInput.MouseAreaSize;
		}

		public void SetMousePosition(int x, int y)
		{
			m_platformInput.MousePosition = new Vector2(x, y);
		}

		public bool IsJoystickConnected()
		{
			return m_joystickConnected;
		}

		private void SetConnectedJoystick(string joystickInstanceName)
		{
			if (joystickInstanceName != null)
			{
				m_joystickInstanceName = joystickInstanceName;
			}
			bool flag = joystickInstanceName != null;
			if (m_joystickConnected != flag)
			{
				m_joystickConnected = flag;
				this.JoystickConnected?.Invoke(m_joystickConnected);
			}
		}

		/// <summary>
		/// Get the actual and previous gamepad key directions (use the first POV controller).
		/// Returns false if this type of input is not available.
		/// </summary>
		public unsafe bool GetGamepadKeyDirections(out int actual, out int previous)
		{
			if (m_joystickConnected)
			{
				actual = *m_actualJoystickState.PointOfViewControllers;
				previous = *m_previousJoystickState.PointOfViewControllers;
				return true;
			}
			actual = -1;
			previous = -1;
			return false;
		}

		public bool IsGamepadKeyRightPressed()
		{
			if (GetGamepadKeyDirections(out int actual, out int _))
			{
				if (actual >= 4500)
				{
					return actual <= 13500;
				}
				return false;
			}
			return false;
		}

		public bool IsGamepadKeyLeftPressed()
		{
			if (GetGamepadKeyDirections(out int actual, out int _))
			{
				if (actual >= 22500)
				{
					return actual <= 31500;
				}
				return false;
			}
			return false;
		}

		public bool IsGamepadKeyDownPressed()
		{
			if (GetGamepadKeyDirections(out int actual, out int _))
			{
				if (actual >= 13500)
				{
					return actual <= 22500;
				}
				return false;
			}
			return false;
		}

		public bool IsGamepadKeyUpPressed()
		{
			if (GetGamepadKeyDirections(out int actual, out int _))
			{
				if (actual < 0 || actual > 4500)
				{
					if (actual >= 31500)
					{
						return actual <= 36000;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		public bool WasGamepadKeyRightPressed()
		{
			if (GetGamepadKeyDirections(out int _, out int previous))
			{
				if (previous >= 4500)
				{
					return previous <= 13500;
				}
				return false;
			}
			return false;
		}

		public bool WasGamepadKeyLeftPressed()
		{
			if (GetGamepadKeyDirections(out int _, out int previous))
			{
				if (previous >= 22500)
				{
					return previous <= 31500;
				}
				return false;
			}
			return false;
		}

		public bool WasGamepadKeyDownPressed()
		{
			if (GetGamepadKeyDirections(out int _, out int previous))
			{
				if (previous >= 13500)
				{
					return previous <= 22500;
				}
				return false;
			}
			return false;
		}

		public bool WasGamepadKeyUpPressed()
		{
			if (GetGamepadKeyDirections(out int _, out int previous))
			{
				if (previous < 0 || previous > 4500)
				{
					if (previous >= 31500)
					{
						return previous <= 36000;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		public bool IsNewGamepadKeyRightPressed()
		{
			if (!WasGamepadKeyRightPressed())
			{
				return IsGamepadKeyRightPressed();
			}
			return false;
		}

		public bool IsNewGamepadKeyLeftPressed()
		{
			if (!WasGamepadKeyLeftPressed())
			{
				return IsGamepadKeyLeftPressed();
			}
			return false;
		}

		public bool IsNewGamepadKeyDownPressed()
		{
			if (!WasGamepadKeyDownPressed())
			{
				return IsGamepadKeyDownPressed();
			}
			return false;
		}

		public bool IsNewGamepadKeyUpPressed()
		{
			if (!WasGamepadKeyUpPressed())
			{
				return IsGamepadKeyUpPressed();
			}
			return false;
		}

		public bool IsNewGamepadKeyRightReleased()
		{
			if (WasGamepadKeyRightPressed())
			{
				return !IsGamepadKeyRightPressed();
			}
			return false;
		}

		public bool IsNewGamepadKeyLeftReleased()
		{
			if (WasGamepadKeyLeftPressed())
			{
				return !IsGamepadKeyLeftPressed();
			}
			return false;
		}

		public bool IsNewGamepadKeyDownReleased()
		{
			if (WasGamepadKeyDownPressed())
			{
				return !IsGamepadKeyDownPressed();
			}
			return false;
		}

		public bool IsNewGamepadKeyUpReleased()
		{
			if (WasGamepadKeyUpPressed())
			{
				return !IsGamepadKeyUpPressed();
			}
			return false;
		}

		public unsafe void GetActualJoystickState(StringBuilder text)
		{
			MyJoystickState actualJoystickState = m_actualJoystickState;
			text.Append("Supported axes: ");
			if (IsJoystickAxisSupported(MyJoystickAxesEnum.Xpos))
			{
				text.Append("X ");
			}
			if (IsJoystickAxisSupported(MyJoystickAxesEnum.Ypos))
			{
				text.Append("Y ");
			}
			if (IsJoystickAxisSupported(MyJoystickAxesEnum.Zpos))
			{
				text.Append("Z ");
			}
			if (IsJoystickAxisSupported(MyJoystickAxesEnum.RotationXpos))
			{
				text.Append("Rx ");
			}
			if (IsJoystickAxisSupported(MyJoystickAxesEnum.RotationYpos))
			{
				text.Append("Ry ");
			}
			if (IsJoystickAxisSupported(MyJoystickAxesEnum.RotationZpos))
			{
				text.Append("Rz ");
			}
			if (IsJoystickAxisSupported(MyJoystickAxesEnum.Slider1pos))
			{
				text.Append("S1 ");
			}
			if (IsJoystickAxisSupported(MyJoystickAxesEnum.Slider2pos))
			{
				text.Append("S2 ");
			}
			text.AppendLine();
			text.Append("accX: ");
			text.AppendInt32(actualJoystickState.AccelerationX);
			text.AppendLine();
			text.Append("accY: ");
			text.AppendInt32(actualJoystickState.AccelerationY);
			text.AppendLine();
			text.Append("accZ: ");
			text.AppendInt32(actualJoystickState.AccelerationZ);
			text.AppendLine();
			text.Append("angAccX: ");
			text.AppendInt32(actualJoystickState.AngularAccelerationX);
			text.AppendLine();
			text.Append("angAccY: ");
			text.AppendInt32(actualJoystickState.AngularAccelerationY);
			text.AppendLine();
			text.Append("angAccZ: ");
			text.AppendInt32(actualJoystickState.AngularAccelerationZ);
			text.AppendLine();
			text.Append("angVelX: ");
			text.AppendInt32(actualJoystickState.AngularVelocityX);
			text.AppendLine();
			text.Append("angVelY: ");
			text.AppendInt32(actualJoystickState.AngularVelocityY);
			text.AppendLine();
			text.Append("angVelZ: ");
			text.AppendInt32(actualJoystickState.AngularVelocityZ);
			text.AppendLine();
			text.Append("forX: ");
			text.AppendInt32(actualJoystickState.ForceX);
			text.AppendLine();
			text.Append("forY: ");
			text.AppendInt32(actualJoystickState.ForceY);
			text.AppendLine();
			text.Append("forZ: ");
			text.AppendInt32(actualJoystickState.ForceZ);
			text.AppendLine();
			text.Append("rotX: ");
			text.AppendInt32(actualJoystickState.RotationX);
			text.AppendLine();
			text.Append("rotY: ");
			text.AppendInt32(actualJoystickState.RotationY);
			text.AppendLine();
			text.Append("rotZ: ");
			text.AppendInt32(actualJoystickState.RotationZ);
			text.AppendLine();
			text.Append("torqX: ");
			text.AppendInt32(actualJoystickState.TorqueX);
			text.AppendLine();
			text.Append("torqY: ");
			text.AppendInt32(actualJoystickState.TorqueY);
			text.AppendLine();
			text.Append("torqZ: ");
			text.AppendInt32(actualJoystickState.TorqueZ);
			text.AppendLine();
			text.Append("velX: ");
			text.AppendInt32(actualJoystickState.VelocityX);
			text.AppendLine();
			text.Append("velY: ");
			text.AppendInt32(actualJoystickState.VelocityY);
			text.AppendLine();
			text.Append("velZ: ");
			text.AppendInt32(actualJoystickState.VelocityZ);
			text.AppendLine();
			text.Append("X: ");
			text.AppendInt32(actualJoystickState.X);
			text.AppendLine();
			text.Append("Y: ");
			text.AppendInt32(actualJoystickState.Y);
			text.AppendLine();
			text.Append("Z: ");
			text.AppendInt32(actualJoystickState.Z);
			text.AppendLine();
			text.AppendLine();
			text.Append("AccSliders: ");
			for (int i = 0; i < 2; i++)
			{
				text.AppendInt32(actualJoystickState.AccelerationSliders[i]);
				text.Append(" ");
			}
			text.AppendLine();
			text.Append("Buttons: ");
			for (int j = 0; j < 128; j++)
			{
				text.Append((actualJoystickState.Buttons[j] > 0) ? "#" : "_");
				text.Append(" ");
			}
			text.AppendLine();
			text.Append("ForSliders: ");
			for (int k = 0; k < 2; k++)
			{
				text.AppendInt32(actualJoystickState.ForceSliders[k]);
				text.Append(" ");
			}
			text.AppendLine();
			text.Append("POVControllers: ");
			for (int l = 0; l < 4; l++)
			{
				text.AppendInt32(actualJoystickState.PointOfViewControllers[l]);
				text.Append(" ");
			}
			text.AppendLine();
			text.Append("Sliders: ");
			for (int m = 0; m < 2; m++)
			{
				text.AppendInt32(actualJoystickState.Sliders[m]);
				text.Append(" ");
			}
			text.AppendLine();
			text.Append("VelocitySliders: ");
			for (int n = 0; n < 2; n++)
			{
				text.AppendInt32(actualJoystickState.VelocitySliders[n]);
				text.Append(" ");
			}
			text.AppendLine();
		}

		public bool IsJoystickAxisSupported(MyJoystickAxesEnum axis)
		{
			if (!m_joystickConnected)
			{
				return false;
			}
			return MyVRage.Platform.Input2.IsJoystickAxisSupported(axis);
		}

		public bool IsNewGameControlPressed(MyStringId controlId)
		{
			if (IsControlBlocked(controlId))
			{
				return false;
			}
			if (m_gameControlsList.TryGetValue(controlId, out MyControl value))
			{
				return value.IsNewPressed();
			}
			return false;
		}

		public bool IsGameControlPressed(MyStringId controlId)
		{
			if (IsControlBlocked(controlId))
			{
				return false;
			}
			if (m_gameControlsList.TryGetValue(controlId, out MyControl value))
			{
				return value.IsPressed();
			}
			return false;
		}

		public bool IsNewGameControlReleased(MyStringId controlId)
		{
			if (IsControlBlocked(controlId))
			{
				return false;
			}
			if (m_gameControlsList.TryGetValue(controlId, out MyControl value))
			{
				return value.IsNewReleased();
			}
			return false;
		}

		public float GetGameControlAnalogState(MyStringId controlId)
		{
			if (IsControlBlocked(controlId))
			{
				return 0f;
			}
			if (m_gameControlsList.TryGetValue(controlId, out MyControl value))
			{
				return value.GetAnalogState();
			}
			return 0f;
		}

		public bool IsGameControlReleased(MyStringId controlId)
		{
			if (IsControlBlocked(controlId))
			{
				return false;
			}
			if (m_gameControlsList.TryGetValue(controlId, out MyControl value))
			{
				return value.IsNewReleased();
			}
			return false;
		}

		public bool IsKeyValid(MyKeys key)
		{
			foreach (MyKeys validKeyboardKey in m_validKeyboardKeys)
			{
				if (validKeyboardKey == key)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsKeyDigit(MyKeys key)
		{
			return m_digitKeys.Contains(key);
		}

		public bool IsMouseButtonValid(MyMouseButtonsEnum button)
		{
			foreach (MyMouseButtonsEnum validMouseButton in m_validMouseButtons)
			{
				if (validMouseButton == button)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsJoystickButtonValid(MyJoystickButtonsEnum button)
		{
			foreach (MyJoystickButtonsEnum validJoystickButton in m_validJoystickButtons)
			{
				if (validJoystickButton == button)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsJoystickAxisValid(MyJoystickAxesEnum axis)
		{
			foreach (MyJoystickAxesEnum validJoystickAxis in m_validJoystickAxes)
			{
				if (validJoystickAxis == axis)
				{
					return true;
				}
			}
			return false;
		}

		public MyControl GetControl(MyKeys key)
		{
			foreach (MyControl value in m_gameControlsList.Values)
			{
				if (value.GetKeyboardControl() == key || value.GetSecondKeyboardControl() == key)
				{
					return value;
				}
			}
			return null;
		}

		public MyControl GetControl(MyMouseButtonsEnum button)
		{
			foreach (MyControl value in m_gameControlsList.Values)
			{
				if (value.GetMouseControl() == button)
				{
					return value;
				}
			}
			return null;
		}

		public void GetListOfPressedKeys(List<MyKeys> keys)
		{
			GetPressedKeys(keys);
		}

		public void GetListOfPressedMouseButtons(List<MyMouseButtonsEnum> result)
		{
			result.Clear();
			if (IsLeftMousePressed())
			{
				result.Add(MyMouseButtonsEnum.Left);
			}
			if (IsRightMousePressed())
			{
				result.Add(MyMouseButtonsEnum.Right);
			}
			if (IsMiddleMousePressed())
			{
				result.Add(MyMouseButtonsEnum.Middle);
			}
			if (IsXButton1MousePressed())
			{
				result.Add(MyMouseButtonsEnum.XButton1);
			}
			if (IsXButton2MousePressed())
			{
				result.Add(MyMouseButtonsEnum.XButton2);
			}
		}

		public DictionaryValuesReader<MyStringId, MyControl> GetGameControlsList()
		{
			return m_gameControlsList;
		}

		public void TakeSnapshot()
		{
			m_joystickInstanceNameSnapshot = JoystickInstanceName;
			CloneControls(m_gameControlsList, m_gameControlsSnapshot);
		}

		public void RevertChanges()
		{
			JoystickInstanceName = m_joystickInstanceNameSnapshot;
			CloneControls(m_gameControlsSnapshot, m_gameControlsList);
		}

		public string GetGameControlTextEnum(MyStringId controlId)
		{
			return m_gameControlsList[controlId].GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
		}

		public MyControl GetGameControl(MyStringId controlId)
		{
			m_gameControlsList.TryGetValue(controlId, out MyControl value);
			return value;
		}

		private void CloneControls(Dictionary<MyStringId, MyControl> original, Dictionary<MyStringId, MyControl> copy)
		{
			foreach (KeyValuePair<MyStringId, MyControl> item in original)
			{
				if (copy.TryGetValue(item.Key, out MyControl value))
				{
					value.CopyFrom(item.Value);
				}
				else
				{
					copy[item.Key] = new MyControl(item.Value);
				}
			}
		}

		public void RevertToDefaultControls()
		{
			m_mouseXIsInverted = false;
			m_mouseYIsInverted = false;
			m_mouseSensitivity = 1.655f;
			m_joystickSensitivity = 2f;
			m_joystickDeadzone = 0.2f;
			m_joystickExponent = 2f;
			CloneControls(m_defaultGameControlsList, m_gameControlsList);
		}

		public void SaveControls(SerializableDictionary<string, string> controlsGeneral, SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
		{
			controlsGeneral.Dictionary.Clear();
			controlsGeneral.Dictionary.Add("mouseXIsInverted", m_mouseXIsInverted.ToString());
			controlsGeneral.Dictionary.Add("mouseYIsInverted", m_mouseYIsInverted.ToString());
			controlsGeneral.Dictionary.Add("mouseSensitivity", m_mouseSensitivity.ToString(CultureInfo.InvariantCulture));
			controlsGeneral.Dictionary.Add("joystickInstanceName", m_joystickInstanceName);
			controlsGeneral.Dictionary.Add("joystickSensitivity", m_joystickSensitivity.ToString(CultureInfo.InvariantCulture));
			controlsGeneral.Dictionary.Add("joystickExponent", m_joystickExponent.ToString(CultureInfo.InvariantCulture));
			controlsGeneral.Dictionary.Add("joystickDeadzone", m_joystickDeadzone.ToString(CultureInfo.InvariantCulture));
			controlsButtons.Dictionary.Clear();
			foreach (MyControl value in m_gameControlsList.Values)
			{
				SerializableDictionary<string, string> serializableDictionary = new SerializableDictionary<string, string>();
				controlsButtons[value.GetGameControlEnum().ToString()] = serializableDictionary;
				serializableDictionary["Keyboard"] = value.GetKeyboardControl().ToString();
				serializableDictionary["Keyboard2"] = value.GetSecondKeyboardControl().ToString();
				serializableDictionary["Mouse"] = MyEnumsToStrings.MouseButtonsEnum[(uint)value.GetMouseControl()];
			}
		}

		private bool LoadControls(SerializableDictionary<string, string> controlsGeneral, SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
		{
			if (controlsGeneral.Dictionary.Count == 0)
			{
				MyLog.Default.WriteLine("    Loading default controls");
				RevertToDefaultControls();
				return false;
			}
			try
			{
				m_mouseXIsInverted = bool.Parse(controlsGeneral["mouseXIsInverted"]);
				m_mouseYIsInverted = bool.Parse(controlsGeneral["mouseYIsInverted"]);
				m_mouseSensitivity = float.Parse(controlsGeneral["mouseSensitivity"], CultureInfo.InvariantCulture);
				JoystickInstanceName = controlsGeneral["joystickInstanceName"];
				m_joystickSensitivity = float.Parse(controlsGeneral["joystickSensitivity"], CultureInfo.InvariantCulture);
				m_joystickExponent = float.Parse(controlsGeneral["joystickExponent"], CultureInfo.InvariantCulture);
				m_joystickDeadzone = float.Parse(controlsGeneral["joystickDeadzone"], CultureInfo.InvariantCulture);
				LoadGameControls(controlsButtons);
				return true;
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("    Error loading controls from config:");
				MyLog.Default.WriteLine(ex);
				MyLog.Default.WriteLine("    Loading default controls");
				RevertToDefaultControls();
				return false;
			}
		}

		private void LoadGameControls(SerializableDictionary<string, SerializableDictionary<string, string>> controlsButtons)
		{
			if (controlsButtons.Dictionary.Count == 0)
			{
				throw new Exception("ControlsButtons config parameter is empty.");
			}
			foreach (KeyValuePair<string, SerializableDictionary<string, string>> item in controlsButtons.Dictionary)
			{
				MyStringId? myStringId = TryParseMyGameControlEnums(item.Key);
				if (myStringId.HasValue)
				{
					m_gameControlsList[myStringId.Value].SetNoControl();
					SerializableDictionary<string, string> value = item.Value;
					LoadGameControl(value["Keyboard"], myStringId.Value, ParseMyGuiInputDeviceEnum("Keyboard"));
					LoadGameControl(value["Keyboard2"], myStringId.Value, ParseMyGuiInputDeviceEnum("KeyboardSecond"));
					LoadGameControl(value["Mouse"], myStringId.Value, ParseMyGuiInputDeviceEnum("Mouse"));
				}
			}
		}

		private void LoadGameControl(string controlName, MyStringId controlType, MyGuiInputDeviceEnum device)
		{
			switch (device)
			{
			case MyGuiInputDeviceEnum.None:
			case (MyGuiInputDeviceEnum)3:
			case (MyGuiInputDeviceEnum)4:
				break;
			case MyGuiInputDeviceEnum.Keyboard:
			{
				MyKeys key2 = (MyKeys)Enum.Parse(typeof(MyKeys), controlName);
				if (!IsKeyValid(key2))
				{
					throw new Exception("Key \"" + key2.ToString() + "\" is already assigned or is not valid.");
				}
				FindNotAssignedGameControl(controlType, device).SetControl(MyGuiInputDeviceEnum.Keyboard, key2);
				break;
			}
			case MyGuiInputDeviceEnum.KeyboardSecond:
			{
				MyKeys key = (MyKeys)Enum.Parse(typeof(MyKeys), controlName);
				if (!IsKeyValid(key))
				{
					throw new Exception("Key \"" + key.ToString() + "\" is already assigned or is not valid.");
				}
				FindNotAssignedGameControl(controlType, device).SetControl(MyGuiInputDeviceEnum.KeyboardSecond, key);
				break;
			}
			case MyGuiInputDeviceEnum.Mouse:
			{
				MyMouseButtonsEnum myMouseButtonsEnum = ParseMyMouseButtonsEnum(controlName);
				if (!IsMouseButtonValid(myMouseButtonsEnum))
				{
					throw new Exception("Mouse button \"" + myMouseButtonsEnum.ToString() + "\" is already assigned or is not valid.");
				}
				FindNotAssignedGameControl(controlType, device).SetControl(myMouseButtonsEnum);
				break;
			}
			}
		}

		public MyGuiInputDeviceEnum ParseMyGuiInputDeviceEnum(string s)
		{
			for (int i = 0; i < MyEnumsToStrings.GuiInputDeviceEnum.Length; i++)
			{
				if (MyEnumsToStrings.GuiInputDeviceEnum[i] == s)
				{
					return (MyGuiInputDeviceEnum)i;
				}
			}
			throw new ArgumentException("Value \"" + s + "\" is not from GuiInputDeviceEnum.", "s");
		}

		public MyJoystickButtonsEnum ParseMyJoystickButtonsEnum(string s)
		{
			for (int i = 0; i < MyEnumsToStrings.JoystickButtonsEnum.Length; i++)
			{
				if (MyEnumsToStrings.JoystickButtonsEnum[i] == s)
				{
					return (MyJoystickButtonsEnum)i;
				}
			}
			throw new ArgumentException("Value \"" + s + "\" is not from JoystickButtonsEnum.", "s");
		}

		public MyJoystickAxesEnum ParseMyJoystickAxesEnum(string s)
		{
			for (int i = 0; i < MyEnumsToStrings.JoystickAxesEnum.Length; i++)
			{
				if (MyEnumsToStrings.JoystickAxesEnum[i] == s)
				{
					return (MyJoystickAxesEnum)i;
				}
			}
			throw new ArgumentException("Value \"" + s + "\" is not from JoystickAxesEnum.", "s");
		}

		public MyMouseButtonsEnum ParseMyMouseButtonsEnum(string s)
		{
			for (int i = 0; i < MyEnumsToStrings.MouseButtonsEnum.Length; i++)
			{
				if (MyEnumsToStrings.MouseButtonsEnum[i] == s)
				{
					return (MyMouseButtonsEnum)i;
				}
			}
			throw new ArgumentException("Value \"" + s + "\" is not from MouseButtonsEnum.", "s");
		}

		public MyStringId? TryParseMyGameControlEnums(string s)
		{
			MyStringId orCompute = MyStringId.GetOrCompute(s);
			if (m_gameControlsList.ContainsKey(orCompute))
			{
				return orCompute;
			}
			return null;
		}

		public MyGuiControlTypeEnum ParseMyGuiControlTypeEnum(string s)
		{
			for (int i = 0; i < MyEnumsToStrings.ControlTypeEnum.Length; i++)
			{
				if (MyEnumsToStrings.ControlTypeEnum[i] == s)
				{
					return (MyGuiControlTypeEnum)i;
				}
			}
			throw new ArgumentException("Value \"" + s + "\" is not from MyGuiInputTypeEnum.", "s");
		}

		private MyControl FindNotAssignedGameControl(MyStringId controlId, MyGuiInputDeviceEnum deviceType)
		{
			if (!m_gameControlsList.TryGetValue(controlId, out MyControl value))
			{
				throw new Exception("Game control \"" + controlId.ToString() + "\" not found in control list.");
			}
			if (value.IsControlAssigned(deviceType))
			{
				throw new Exception("Game control \"" + controlId.ToString() + "\" is already assigned.");
			}
			return value;
		}

		public string GetKeyName(MyStringId controlId)
		{
			return GetGameControl(controlId).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
		}

		public string GetKeyName(MyKeys key)
		{
			return m_nameLookup.GetKeyName(key);
		}

		public string GetName(MyMouseButtonsEnum mouseButton)
		{
			return m_nameLookup.GetName(mouseButton);
		}

		public string GetName(MyJoystickButtonsEnum joystickButton)
		{
			return m_nameLookup.GetName(joystickButton);
		}

		public string GetName(MyJoystickAxesEnum joystickAxis)
		{
			return m_nameLookup.GetName(joystickAxis);
		}

		public string GetUnassignedName()
		{
			return m_nameLookup.UnassignedText;
		}

		public void SetControlBlock(MyStringId controlEnum, bool block = false)
		{
			if (block)
			{
				m_gameControlsBlacklist.Add(controlEnum);
			}
			else
			{
				m_gameControlsBlacklist.Remove(controlEnum);
			}
		}

		public bool IsControlBlocked(MyStringId controlEnum)
		{
			return m_gameControlsBlacklist.Contains(controlEnum);
		}

		public void ClearBlacklist()
		{
			m_gameControlsBlacklist.Clear();
		}

		public bool IsMouseMoved()
		{
			if (m_actualMouseState.X == m_previousMouseState.X)
			{
				return m_actualMouseState.Y != m_previousMouseState.Y;
			}
			return true;
		}
	}
}
