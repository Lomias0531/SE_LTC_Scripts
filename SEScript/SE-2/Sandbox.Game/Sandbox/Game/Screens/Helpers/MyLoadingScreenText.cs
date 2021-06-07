using System.Collections.Generic;
using VRage.Collections;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyLoadingScreenText
	{
		public readonly MyStringId Text;

		protected static List<MyLoadingScreenText> m_textsShared;

		protected static List<MyLoadingScreenText> m_textsKeyboard;

		protected static List<MyLoadingScreenText> m_textsGamepad;

		public static ListReader<MyLoadingScreenText> TextsShared => m_textsShared;

		public MyLoadingScreenText(MyStringId text)
		{
			Text = text;
		}

		public override string ToString()
		{
			MyStringId text = Text;
			return text.ToString();
		}

		static MyLoadingScreenText()
		{
			if (m_textsShared == null)
			{
				m_textsShared = new List<MyLoadingScreenText>();
			}
			else
			{
				m_textsShared.Clear();
			}
			if (m_textsKeyboard == null)
			{
				m_textsKeyboard = new List<MyLoadingScreenText>();
			}
			else
			{
				m_textsKeyboard.Clear();
			}
			if (m_textsGamepad == null)
			{
				m_textsGamepad = new List<MyLoadingScreenText>();
			}
			else
			{
				m_textsGamepad.Clear();
			}
			MyLoadingScreenQuote.Init();
			MyLoadingScreenHint.Init();
		}

		public static MyLoadingScreenText GetSharedScreenText(int i)
		{
			i = MyMath.Mod(i, m_textsShared.Count);
			return m_textsShared[i];
		}

		public static MyLoadingScreenText GetGamepadScreenText(int i)
		{
			i = MyMath.Mod(i, m_textsGamepad.Count);
			return m_textsGamepad[i];
		}

		public static MyLoadingScreenText GetKeyboardScreenText(int i)
		{
			i = MyMath.Mod(i, m_textsKeyboard.Count);
			return m_textsKeyboard[i];
		}

		public static MyLoadingScreenText GetRandomText()
		{
			int count = m_textsShared.Count;
			int num = 0;
			if (MyInput.Static.IsJoystickLastUsed)
			{
				num = m_textsShared.Count + m_textsGamepad.Count;
				int num2 = MyRandom.Instance.Next(0, num);
				if (num2 < count)
				{
					return GetSharedScreenText(num2);
				}
				return GetGamepadScreenText(num2 - count);
			}
			num = m_textsShared.Count + m_textsKeyboard.Count;
			int num3 = MyRandom.Instance.Next(0, num);
			if (num3 < count)
			{
				return GetSharedScreenText(num3);
			}
			return GetKeyboardScreenText(num3 - count);
		}

		public static MyLoadingScreenText GetText(int i = 0, bool forceGamepad = false)
		{
			int count = m_textsShared.Count;
			int num = 0;
			if (MyInput.Static.IsJoystickLastUsed || forceGamepad)
			{
				num = m_textsShared.Count + m_textsGamepad.Count;
				int num2 = i % num;
				if (num2 < count)
				{
					return GetSharedScreenText(num2);
				}
				return GetGamepadScreenText(num2 - count);
			}
			num = m_textsShared.Count + m_textsKeyboard.Count;
			int num3 = i % num;
			if (num3 < count)
			{
				return GetSharedScreenText(num3);
			}
			return GetKeyboardScreenText(num3 - count);
		}
	}
}
