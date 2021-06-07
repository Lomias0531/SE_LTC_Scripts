using System.Collections.Generic;

namespace VRage.Input.Keyboard
{
	internal class MyGuiLocalizedKeyboardState
	{
		private static HashSet<byte> m_localKeys;

		private MyKeyboardState m_previousKeyboardState;

		private MyKeyboardState m_actualKeyboardState;

		private readonly IVRageInput2 m_platformInput;

		public MyGuiLocalizedKeyboardState(IVRageInput2 platformInput)
		{
			m_platformInput = platformInput;
			m_actualKeyboardState = GetCurrentState();
			if (m_localKeys == null)
			{
				m_localKeys = new HashSet<byte>();
				AddLocalKey(MyKeys.LeftControl);
				AddLocalKey(MyKeys.LeftAlt);
				AddLocalKey(MyKeys.LeftShift);
				AddLocalKey(MyKeys.RightAlt);
				AddLocalKey(MyKeys.RightControl);
				AddLocalKey(MyKeys.RightShift);
				AddLocalKey(MyKeys.Delete);
				AddLocalKey(MyKeys.NumPad0);
				AddLocalKey(MyKeys.NumPad1);
				AddLocalKey(MyKeys.NumPad2);
				AddLocalKey(MyKeys.NumPad3);
				AddLocalKey(MyKeys.NumPad4);
				AddLocalKey(MyKeys.NumPad5);
				AddLocalKey(MyKeys.NumPad6);
				AddLocalKey(MyKeys.NumPad7);
				AddLocalKey(MyKeys.NumPad8);
				AddLocalKey(MyKeys.NumPad9);
				AddLocalKey(MyKeys.Decimal);
				AddLocalKey(MyKeys.LeftWindows);
				AddLocalKey(MyKeys.RightWindows);
				AddLocalKey(MyKeys.Apps);
				AddLocalKey(MyKeys.Pause);
				AddLocalKey(MyKeys.Divide);
			}
		}

		private unsafe MyKeyboardState GetCurrentState()
		{
			MyKeyboardBuffer buffer = default(MyKeyboardBuffer);
			m_platformInput.GetAsyncKeyStates(buffer.Data);
			if (buffer.GetBit(165))
			{
				buffer.SetBit(162, value: false);
				buffer.SetBit(17, value: false);
			}
			return MyKeyboardState.FromBuffer(buffer);
		}

		private void AddLocalKey(MyKeys key)
		{
			m_localKeys.Add((byte)key);
		}

		public void ClearStates()
		{
			m_previousKeyboardState = m_actualKeyboardState;
			m_actualKeyboardState = default(MyKeyboardState);
		}

		public void UpdateStates()
		{
			m_previousKeyboardState = m_actualKeyboardState;
			m_actualKeyboardState = GetCurrentState();
		}

		public void UpdateStatesFromSnapshot(MyKeyboardState state)
		{
			m_previousKeyboardState = m_actualKeyboardState;
			m_actualKeyboardState = state;
		}

		public void UpdateStatesFromSnapshot(MyKeyboardState currentState, MyKeyboardState previousState)
		{
			m_previousKeyboardState = previousState;
			m_actualKeyboardState = currentState;
		}

		public MyKeyboardState GetActualKeyboardState()
		{
			return m_actualKeyboardState;
		}

		public MyKeyboardState GetPreviousKeyboardState()
		{
			return m_previousKeyboardState;
		}

		public void SetKey(MyKeys key, bool value)
		{
			m_actualKeyboardState.SetKey(key, value);
		}

		public bool IsPreviousKeyDown(MyKeys key, bool isLocalKey)
		{
			if (!isLocalKey)
			{
				key = LocalToUSEnglish(key);
			}
			return m_previousKeyboardState.IsKeyDown(key);
		}

		public bool IsPreviousKeyDown(MyKeys key)
		{
			return IsPreviousKeyDown(key, IsKeyLocal(key));
		}

		public void NegateEscapePress()
		{
			m_previousKeyboardState.SetKey(MyKeys.Escape, value: true);
			m_actualKeyboardState.SetKey(MyKeys.Escape, value: true);
		}

		public bool IsPreviousKeyUp(MyKeys key, bool isLocalKey)
		{
			if (!isLocalKey)
			{
				key = LocalToUSEnglish(key);
			}
			return m_previousKeyboardState.IsKeyUp(key);
		}

		public bool IsPreviousKeyUp(MyKeys key)
		{
			return IsPreviousKeyUp(key, IsKeyLocal(key));
		}

		public bool IsKeyDown(MyKeys key, bool isLocalKey)
		{
			if (!isLocalKey)
			{
				key = LocalToUSEnglish(key);
			}
			return m_actualKeyboardState.IsKeyDown(key);
		}

		public bool IsKeyUp(MyKeys key, bool isLocalKey)
		{
			if (!isLocalKey)
			{
				key = LocalToUSEnglish(key);
			}
			return m_actualKeyboardState.IsKeyUp(key);
		}

		public bool IsKeyDown(MyKeys key)
		{
			return IsKeyDown(key, IsKeyLocal(key));
		}

		private bool IsKeyLocal(MyKeys key)
		{
			return m_localKeys.Contains((byte)key);
		}

		public bool IsKeyUp(MyKeys key)
		{
			return IsKeyUp(key, IsKeyLocal(key));
		}

		private static MyKeys USEnglishToLocal(MyKeys key)
		{
			return key;
		}

		private static MyKeys LocalToUSEnglish(MyKeys key)
		{
			return key;
		}

		public bool IsAnyKeyPressed()
		{
			return m_actualKeyboardState.IsAnyKeyPressed();
		}

		public void GetActualPressedKeys(List<MyKeys> keys)
		{
			m_actualKeyboardState.GetPressedKeys(keys);
			for (int i = 0; i < keys.Count; i++)
			{
				if (!IsKeyLocal(keys[i]))
				{
					keys[i] = USEnglishToLocal(keys[i]);
				}
			}
		}
	}
}
