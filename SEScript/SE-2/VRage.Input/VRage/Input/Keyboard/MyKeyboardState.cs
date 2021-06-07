using System.Collections.Generic;

namespace VRage.Input.Keyboard
{
	public struct MyKeyboardState
	{
		private MyKeyboardBuffer m_buffer;

		public void GetPressedKeys(List<MyKeys> keys)
		{
			keys.Clear();
			for (int i = 1; i < 255; i++)
			{
				if (m_buffer.GetBit((byte)i))
				{
					keys.Add((MyKeys)i);
				}
			}
		}

		public bool IsAnyKeyPressed()
		{
			return m_buffer.AnyBitSet();
		}

		public void SetKey(MyKeys key, bool value)
		{
			m_buffer.SetBit((byte)key, value);
		}

		public static MyKeyboardState FromBuffer(MyKeyboardBuffer buffer)
		{
			MyKeyboardState result = default(MyKeyboardState);
			result.m_buffer = buffer;
			return result;
		}

		public bool IsKeyDown(MyKeys key)
		{
			return m_buffer.GetBit((byte)key);
		}

		public bool IsKeyUp(MyKeys key)
		{
			return !IsKeyDown(key);
		}

		public void AddKey(MyKeys key, bool value)
		{
			m_buffer.SetBit((byte)key, value: true);
		}
	}
}
