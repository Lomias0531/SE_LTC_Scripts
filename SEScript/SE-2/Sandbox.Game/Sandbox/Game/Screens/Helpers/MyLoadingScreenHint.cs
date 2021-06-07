using System.Collections.Generic;
using VRage;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyLoadingScreenHint : MyLoadingScreenText
	{
		private struct MyContextWithControl
		{
			public MyStringId Context;

			public MyStringId Control;
		}

		public readonly List<object> Control = new List<object>();

		private List<object> m_control = new List<object>();

		public MyLoadingScreenHint(MyStringId text, List<object> control)
			: base(text)
		{
			Control = control;
			for (int i = 0; i < control.Count; i++)
			{
				m_control.Add(null);
			}
			RefreshControls();
		}

		public override string ToString()
		{
			RefreshControls();
			return string.Format(MyTexts.GetString(Text), m_control.ToArray());
		}

		private void RefreshControls()
		{
			for (int i = 0; i < m_control.Count; i++)
			{
				if (Control[i] == null)
				{
					continue;
				}
				object obj;
				if ((obj = Control[i]) is MyStringId)
				{
					MyStringId controlEnum = (MyStringId)obj;
					m_control[i] = MyInput.Static.GetGameControl(controlEnum);
				}
				else if ((obj = Control[i]) is MyContextWithControl)
				{
					MyContextWithControl myContextWithControl = (MyContextWithControl)obj;
					MyControllerHelper.IControl control = MyControllerHelper.TryGetControl(myContextWithControl.Context, myContextWithControl.Control);
					if (control == null)
					{
						control = MyControllerHelper.GetNullControl();
					}
					m_control[i] = control;
					if (MyControllerHelper.IsNullControl(control))
					{
						MyLog.Default.Error("Control for hint is missing. Context - {0}, Control - {1}", myContextWithControl.Context.ToString(), myContextWithControl.Control.ToString());
					}
				}
			}
		}

		public static void Init()
		{
			GetSharedHints();
			GetKeyboardOnlyHints();
			GetGamepadOnlyHints();
		}

		private static void GetGamepadOnlyHints()
		{
			MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
			int num = 0;
			while ((nullOrEmpty = MyStringId.TryGet($"HintGamepadOnly{num:00}Text")) != MyStringId.NullOrEmpty)
			{
				int num2 = 0;
				MyStringId nullOrEmpty2 = MyStringId.NullOrEmpty;
				List<object> list = new List<object>();
				while ((nullOrEmpty2 = MyStringId.TryGet($"HintGamepadOnly{num:00}Control{num2}")) != MyStringId.NullOrEmpty)
				{
					string[] array = MyTexts.GetString(nullOrEmpty2).Split(new char[1]
					{
						':'
					});
					if (array.Length == 1)
					{
						MyStringId orCompute = MyStringId.GetOrCompute(array[0]);
						list.Add(orCompute);
					}
					else if (array.Length == 2)
					{
						list.Add(new MyContextWithControl
						{
							Context = MyStringId.GetOrCompute(array[0]),
							Control = MyStringId.GetOrCompute(array[1])
						});
					}
					num2++;
				}
				MyLoadingScreenText.m_textsGamepad.Add(new MyLoadingScreenHint(nullOrEmpty, list));
				num++;
			}
		}

		private static void GetKeyboardOnlyHints()
		{
			MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
			int num = 0;
			while ((nullOrEmpty = MyStringId.TryGet($"HintKeyboardOnly{num:00}Text")) != MyStringId.NullOrEmpty)
			{
				int num2 = 0;
				MyStringId nullOrEmpty2 = MyStringId.NullOrEmpty;
				List<object> list = new List<object>();
				while ((nullOrEmpty2 = MyStringId.TryGet($"HintKeyboardOnly{num:00}Control{num2}")) != MyStringId.NullOrEmpty)
				{
					MyStringId orCompute = MyStringId.GetOrCompute(MyTexts.GetString(nullOrEmpty2));
					list.Add(orCompute);
					num2++;
				}
				MyLoadingScreenText.m_textsKeyboard.Add(new MyLoadingScreenHint(nullOrEmpty, list));
				num++;
			}
		}

		private static void GetSharedHints()
		{
			MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
			int num = 0;
			while ((nullOrEmpty = MyStringId.TryGet($"Hint{num:00}Text")) != MyStringId.NullOrEmpty)
			{
				int num2 = 0;
				MyStringId nullOrEmpty2 = MyStringId.NullOrEmpty;
				List<object> list = new List<object>();
				while ((nullOrEmpty2 = MyStringId.TryGet($"Hint{num:00}Control{num2}")) != MyStringId.NullOrEmpty)
				{
					MyStringId orCompute = MyStringId.GetOrCompute(MyTexts.GetString(nullOrEmpty2));
					list.Add(orCompute);
					num2++;
				}
				MyLoadingScreenText.m_textsShared.Add(new MyLoadingScreenHint(nullOrEmpty, list));
				num++;
			}
		}
	}
}
