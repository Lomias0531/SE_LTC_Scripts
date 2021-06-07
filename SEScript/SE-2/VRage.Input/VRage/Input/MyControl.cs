using System.Text;
using VRage.ModAPI;
using VRage.Utils;

namespace VRage.Input
{
	public class MyControl : IMyControl
	{
		private struct Data
		{
			public MyStringId Name;

			public MyStringId ControlId;

			public MyGuiControlTypeEnum ControlType;

			public MyKeys KeyboardKey;

			public MyKeys KeyboardKey2;

			public MyMouseButtonsEnum MouseButton;

			public MyStringId? Description;
		}

		private const int DEFAULT_CAPACITY = 16;

		private static StringBuilder m_toStringCache = new StringBuilder(16);

		private Data m_data;

		private MyStringId m_name
		{
			get
			{
				return m_data.Name;
			}
			set
			{
				m_data.Name = value;
			}
		}

		private MyStringId m_controlId
		{
			get
			{
				return m_data.ControlId;
			}
			set
			{
				m_data.ControlId = value;
			}
		}

		private MyGuiControlTypeEnum m_controlType
		{
			get
			{
				return m_data.ControlType;
			}
			set
			{
				m_data.ControlType = value;
			}
		}

		private MyKeys m_keyboardKey
		{
			get
			{
				return m_data.KeyboardKey;
			}
			set
			{
				m_data.KeyboardKey = value;
			}
		}

		private MyKeys m_KeyboardKey2
		{
			get
			{
				return m_data.KeyboardKey2;
			}
			set
			{
				m_data.KeyboardKey2 = value;
			}
		}

		private MyMouseButtonsEnum m_mouseButton
		{
			get
			{
				return m_data.MouseButton;
			}
			set
			{
				m_data.MouseButton = value;
			}
		}

		public string ButtonNames
		{
			get
			{
				m_toStringCache.Clear();
				AppendBoundButtonNames(ref m_toStringCache, ", ", MyInput.Static.GetUnassignedName());
				return m_toStringCache.ToString();
			}
		}

		public string ButtonNamesIgnoreSecondary
		{
			get
			{
				m_toStringCache.Clear();
				AppendBoundButtonNames(ref m_toStringCache, ", ", null, includeSecondary: false);
				return m_toStringCache.ToString();
			}
		}

		public MyControl(MyStringId controlId, MyStringId name, MyGuiControlTypeEnum controlType, MyMouseButtonsEnum? defaultControlMouse, MyKeys? defaultControlKey, MyStringId? helpText = null, MyKeys? defaultControlKey2 = null, MyStringId? description = null)
		{
			m_controlId = controlId;
			m_name = name;
			m_controlType = controlType;
			m_mouseButton = (defaultControlMouse ?? MyMouseButtonsEnum.None);
			m_keyboardKey = (defaultControlKey ?? MyKeys.None);
			m_KeyboardKey2 = (defaultControlKey2 ?? MyKeys.None);
			m_data.Description = description;
		}

		public MyControl(MyControl other)
		{
			CopyFrom(other);
		}

		public void SetControl(MyGuiInputDeviceEnum device, MyKeys key)
		{
			switch (device)
			{
			case MyGuiInputDeviceEnum.Keyboard:
				m_keyboardKey = key;
				break;
			case MyGuiInputDeviceEnum.KeyboardSecond:
				m_KeyboardKey2 = key;
				break;
			}
		}

		public void SetControl(MyMouseButtonsEnum mouseButton)
		{
			m_mouseButton = mouseButton;
		}

		public void SetNoControl()
		{
			m_mouseButton = MyMouseButtonsEnum.None;
			m_keyboardKey = MyKeys.None;
			m_KeyboardKey2 = MyKeys.None;
		}

		public MyKeys GetKeyboardControl()
		{
			return m_keyboardKey;
		}

		public MyKeys GetSecondKeyboardControl()
		{
			return m_KeyboardKey2;
		}

		public MyMouseButtonsEnum GetMouseControl()
		{
			return m_mouseButton;
		}

		public bool IsPressed()
		{
			bool flag = false;
			if (m_keyboardKey != 0)
			{
				flag = MyInput.Static.IsKeyPress(m_keyboardKey);
			}
			if (m_KeyboardKey2 != 0 && !flag)
			{
				flag = MyInput.Static.IsKeyPress(m_KeyboardKey2);
			}
			if (m_mouseButton != 0 && !flag)
			{
				flag = MyInput.Static.IsMousePressed(m_mouseButton);
			}
			return flag;
		}

		public bool IsNewPressed()
		{
			bool flag = false;
			if (m_keyboardKey != 0)
			{
				flag = MyInput.Static.IsNewKeyPressed(m_keyboardKey);
			}
			if (m_KeyboardKey2 != 0 && !flag)
			{
				flag = MyInput.Static.IsNewKeyPressed(m_KeyboardKey2);
			}
			if (m_mouseButton != 0 && !flag)
			{
				flag = MyInput.Static.IsNewMousePressed(m_mouseButton);
			}
			return flag;
		}

		public bool IsNewReleased()
		{
			bool flag = false;
			if (m_keyboardKey != 0)
			{
				flag = MyInput.Static.IsNewKeyReleased(m_keyboardKey);
			}
			if (m_KeyboardKey2 != 0 && !flag)
			{
				flag = MyInput.Static.IsNewKeyReleased(m_KeyboardKey2);
			}
			if (m_mouseButton != 0 && !flag)
			{
				switch (m_mouseButton)
				{
				case MyMouseButtonsEnum.Left:
					flag = MyInput.Static.IsNewLeftMouseReleased();
					break;
				case MyMouseButtonsEnum.Middle:
					flag = MyInput.Static.IsNewMiddleMouseReleased();
					break;
				case MyMouseButtonsEnum.Right:
					flag = MyInput.Static.IsNewRightMouseReleased();
					break;
				case MyMouseButtonsEnum.XButton1:
					flag = MyInput.Static.IsNewXButton1MouseReleased();
					break;
				case MyMouseButtonsEnum.XButton2:
					flag = MyInput.Static.IsNewXButton2MouseReleased();
					break;
				}
			}
			return flag;
		}

		public bool IsJoystickPressed()
		{
			return false;
		}

		public bool IsNewJoystickPressed()
		{
			return false;
		}

		public bool IsNewJoystickReleased()
		{
			return false;
		}

		/// <summary>
		/// Return the analog state between 0 (not pressed at all) and 1 (fully pressed).
		/// If a digital button is mapped to an analog control, it can return only 0 or 1.
		/// </summary>
		public float GetAnalogState()
		{
			bool flag = false;
			if (m_keyboardKey != 0)
			{
				flag = MyInput.Static.IsKeyPress(m_keyboardKey);
			}
			if (m_KeyboardKey2 != 0 && !flag)
			{
				flag = MyInput.Static.IsKeyPress(m_KeyboardKey2);
			}
			if (m_mouseButton != 0 && !flag)
			{
				switch (m_mouseButton)
				{
				case MyMouseButtonsEnum.Left:
					flag = MyInput.Static.IsLeftMousePressed();
					break;
				case MyMouseButtonsEnum.Middle:
					flag = MyInput.Static.IsMiddleMousePressed();
					break;
				case MyMouseButtonsEnum.Right:
					flag = MyInput.Static.IsRightMousePressed();
					break;
				case MyMouseButtonsEnum.XButton1:
					flag = MyInput.Static.IsXButton1MousePressed();
					break;
				case MyMouseButtonsEnum.XButton2:
					flag = MyInput.Static.IsXButton2MousePressed();
					break;
				}
			}
			if (flag)
			{
				return 1f;
			}
			return 0f;
		}

		public MyStringId GetControlName()
		{
			return m_name;
		}

		public MyStringId? GetControlDescription()
		{
			return m_data.Description;
		}

		public MyGuiControlTypeEnum GetControlTypeEnum()
		{
			return m_controlType;
		}

		public MyStringId GetGameControlEnum()
		{
			return m_controlId;
		}

		public bool IsControlAssigned()
		{
			if (m_keyboardKey == MyKeys.None)
			{
				return m_mouseButton != MyMouseButtonsEnum.None;
			}
			return true;
		}

		public bool IsControlAssigned(MyGuiInputDeviceEnum deviceType)
		{
			bool result = false;
			switch (deviceType)
			{
			case MyGuiInputDeviceEnum.Keyboard:
				result = (m_keyboardKey != MyKeys.None);
				break;
			case MyGuiInputDeviceEnum.Mouse:
				result = (m_mouseButton != MyMouseButtonsEnum.None);
				break;
			}
			return result;
		}

		public void CopyFrom(MyControl other)
		{
			m_data = other.m_data;
		}

		/// <summary>
		/// Causes allocation. Creates single string with list of assigned controls.
		/// </summary>
		public override string ToString()
		{
			return ButtonNames.UpdateControlsToNotificationFriendly();
		}

		/// <summary>
		/// Causes allocation. Creates single StringBuilder with list of assigned controls. Caller
		/// takes ownership of returned StringBuilder (it is not stored internally).
		/// </summary>
		public StringBuilder ToStringBuilder(string unassignedText)
		{
			m_toStringCache.Clear();
			AppendBoundButtonNames(ref m_toStringCache, ", ", unassignedText);
			return new StringBuilder(m_toStringCache.Length).AppendStringBuilder(m_toStringCache);
		}

		public string GetControlButtonName(MyGuiInputDeviceEnum deviceType)
		{
			m_toStringCache.Clear();
			AppendBoundButtonNames(ref m_toStringCache, deviceType);
			return m_toStringCache.ToString();
		}

		public void AppendBoundKeyJustOne(ref StringBuilder output)
		{
			EnsureExists(ref output);
			if (m_keyboardKey != 0)
			{
				AppendName(ref output, m_keyboardKey);
			}
			else
			{
				AppendName(ref output, m_KeyboardKey2);
			}
		}

		public void AppendBoundButtonNames(ref StringBuilder output, MyGuiInputDeviceEnum device, string separator = null)
		{
			EnsureExists(ref output);
			switch (device)
			{
			case (MyGuiInputDeviceEnum)3:
			case (MyGuiInputDeviceEnum)4:
				break;
			case MyGuiInputDeviceEnum.Keyboard:
				if (separator == null)
				{
					AppendName(ref output, m_keyboardKey);
				}
				else
				{
					AppendName(ref output, m_keyboardKey, m_KeyboardKey2, separator);
				}
				break;
			case MyGuiInputDeviceEnum.KeyboardSecond:
				if (separator == null)
				{
					AppendName(ref output, m_KeyboardKey2);
				}
				else
				{
					AppendName(ref output, m_keyboardKey, m_KeyboardKey2, separator);
				}
				break;
			case MyGuiInputDeviceEnum.Mouse:
				AppendName(ref output, m_mouseButton);
				break;
			}
		}

		public void AppendBoundButtonNames(ref StringBuilder output, string separator = ", ", string unassignedText = null, bool includeSecondary = true)
		{
			EnsureExists(ref output);
			MyGuiInputDeviceEnum[] obj = new MyGuiInputDeviceEnum[2]
			{
				MyGuiInputDeviceEnum.Keyboard,
				MyGuiInputDeviceEnum.Mouse
			};
			int num = 0;
			MyGuiInputDeviceEnum[] array = obj;
			foreach (MyGuiInputDeviceEnum myGuiInputDeviceEnum in array)
			{
				if (IsControlAssigned(myGuiInputDeviceEnum))
				{
					if (num > 0)
					{
						output.Append(separator);
					}
					AppendBoundButtonNames(ref output, myGuiInputDeviceEnum, includeSecondary ? separator : null);
					num++;
				}
			}
			if (num == 0 && unassignedText != null)
			{
				output.Append(unassignedText);
			}
		}

		public static void AppendName(ref StringBuilder output, MyKeys key)
		{
			EnsureExists(ref output);
			if (key != 0)
			{
				output.Append(MyInput.Static.GetKeyName(key));
			}
		}

		public static void AppendName(ref StringBuilder output, MyKeys key1, MyKeys key2, string separator)
		{
			EnsureExists(ref output);
			string text = null;
			string text2 = null;
			if (key1 != 0)
			{
				text = MyInput.Static.GetKeyName(key1);
			}
			if (key2 != 0)
			{
				text2 = MyInput.Static.GetKeyName(key2);
			}
			if (text != null && text2 != null)
			{
				output.Append(text).Append(separator).Append(text2);
			}
			else if (text != null)
			{
				output.Append(text);
			}
			else if (text2 != null)
			{
				output.Append(text2);
			}
		}

		public static void AppendName(ref StringBuilder output, MyMouseButtonsEnum mouseButton)
		{
			EnsureExists(ref output);
			output.Append(MyInput.Static.GetName(mouseButton));
		}

		public static void AppendName(ref StringBuilder output, MyJoystickButtonsEnum joystickButton)
		{
			EnsureExists(ref output);
			output.Append(MyInput.Static.GetName(joystickButton));
		}

		public static void AppendName(ref StringBuilder output, MyJoystickAxesEnum joystickAxis)
		{
			EnsureExists(ref output);
			output.Append(MyInput.Static.GetName(joystickAxis));
		}

		public static void AppendUnknownTextIfNeeded(ref StringBuilder output, string unassignedText)
		{
			EnsureExists(ref output);
			if (output.Length == 0)
			{
				output.Append(unassignedText);
			}
		}

		private static void EnsureExists(ref StringBuilder output)
		{
			if (output == null)
			{
				output = new StringBuilder(16);
			}
		}
	}
}
