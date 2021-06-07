using System.Collections.Generic;
using VRage;
using VRage.Input;

namespace Sandbox.Graphics.GUI
{
	public class MyKeyThrottler
	{
		private class MyKeyThrottleState
		{
			public int LastKeyPressTime = -60000;

			public int RequiredDelay;
		}

		private static int m_WINDOWS_CharacterInitialDelay = -1;

		private static int m_WINDOWS_CharacterRepeatDelay = -1;

		private static int m_WINDOWS_CharacterInitialDelayMs = 0;

		private static int m_WINDOWS_CharacterRepeatDelayMs = 0;

		private Dictionary<MyKeys, MyKeyThrottleState> m_keyTimeControllers = new Dictionary<MyKeys, MyKeyThrottleState>();

		public static int WINDOWS_CharacterInitialDelayMs
		{
			get
			{
				if (m_WINDOWS_CharacterInitialDelay != MyVRage.Platform.Input.KeyboardDelay)
				{
					m_WINDOWS_CharacterInitialDelay = MyVRage.Platform.Input.KeyboardDelay;
					ComputeCharacterInitialDelay(m_WINDOWS_CharacterInitialDelay, out m_WINDOWS_CharacterInitialDelayMs);
				}
				return m_WINDOWS_CharacterInitialDelayMs;
			}
		}

		public static int WINDOWS_CharacterRepeatDelayMs
		{
			get
			{
				if (m_WINDOWS_CharacterRepeatDelay != MyVRage.Platform.Input.KeyboardSpeed)
				{
					m_WINDOWS_CharacterRepeatDelay = MyVRage.Platform.Input.KeyboardSpeed;
					ComputeCharacterRepeatDelay(m_WINDOWS_CharacterRepeatDelay, out m_WINDOWS_CharacterRepeatDelayMs);
				}
				return m_WINDOWS_CharacterRepeatDelayMs;
			}
		}

		private static void ComputeCharacterInitialDelay(int code, out int ms)
		{
			switch (code)
			{
			case 0:
				ms = 250;
				break;
			case 1:
				ms = 500;
				break;
			case 2:
				ms = 750;
				break;
			case 3:
				ms = 1000;
				break;
			default:
				ms = 500;
				break;
			}
		}

		private static void ComputeCharacterRepeatDelay(int code, out int ms)
		{
			if (code < 0 || code > 31)
			{
				ms = 25;
				return;
			}
			float num = 500f;
			float num2 = 25f;
			float num3 = (float)code / 31f;
			ms = (int)((1f - num3) * num + num3 * num2);
		}

		private MyKeyThrottleState GetKeyController(MyKeys key)
		{
			if (m_keyTimeControllers.TryGetValue(key, out MyKeyThrottleState value))
			{
				return value;
			}
			value = new MyKeyThrottleState();
			m_keyTimeControllers[key] = value;
			return value;
		}

		public bool IsNewPressAndThrottled(MyKeys key)
		{
			if (!MyInput.Static.IsNewKeyPressed(key))
			{
				return false;
			}
			MyKeyThrottleState keyController = GetKeyController(key);
			if (keyController == null)
			{
				return true;
			}
			if (MyGuiManager.TotalTimeInMilliseconds - keyController.LastKeyPressTime > WINDOWS_CharacterRepeatDelayMs)
			{
				keyController.LastKeyPressTime = MyGuiManager.TotalTimeInMilliseconds;
				return true;
			}
			return false;
		}

		public ThrottledKeyStatus GetKeyStatus(MyKeys key)
		{
			if (!MyInput.Static.IsKeyPress(key))
			{
				return ThrottledKeyStatus.UNPRESSED;
			}
			MyKeyThrottleState keyController = GetKeyController(key);
			if (keyController == null)
			{
				return ThrottledKeyStatus.PRESSED_AND_READY;
			}
			if (MyInput.Static.IsNewKeyPressed(key))
			{
				keyController.RequiredDelay = WINDOWS_CharacterInitialDelayMs;
				keyController.LastKeyPressTime = MyGuiManager.TotalTimeInMilliseconds;
				return ThrottledKeyStatus.PRESSED_AND_READY;
			}
			if (MyGuiManager.TotalTimeInMilliseconds - keyController.LastKeyPressTime > keyController.RequiredDelay)
			{
				keyController.RequiredDelay = WINDOWS_CharacterRepeatDelayMs;
				keyController.LastKeyPressTime = MyGuiManager.TotalTimeInMilliseconds;
				return ThrottledKeyStatus.PRESSED_AND_READY;
			}
			return ThrottledKeyStatus.PRESSED_AND_WAITING;
		}
	}
}
