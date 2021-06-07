using System;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
	internal abstract class MyIngameHelpObjective
	{
		protected interface IHelplet
		{
			void OnActivated();
		}

		public string Id;

		public string[] RequiredIds;

		public string FollowingId;

		public Func<bool> RequiredCondition;

		public MyStringId TitleEnum;

		public MyIngameHelpDetail[] Details = new MyIngameHelpDetail[0];

		public float DelayToHide;

		public float DelayToAppear;

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

		public static object GetHighlightedControl(string text)
		{
			return "[" + text + "]";
		}

		public virtual void OnActivated()
		{
		}

		public virtual void OnBeforeActivate()
		{
		}

		public virtual bool IsCritical()
		{
			return false;
		}
	}
}
