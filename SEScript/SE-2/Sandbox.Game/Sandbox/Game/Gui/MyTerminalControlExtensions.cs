using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using System;
using System.Text;
using VRage;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public static class MyTerminalControlExtensions
	{
		private static StringBuilder Combine(MyStringId prefix, MyStringId title)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = MyTexts.Get(prefix);
			if (stringBuilder2.Length > 0)
			{
				stringBuilder.Append((object)stringBuilder2).Append(" ");
			}
			return stringBuilder.Append(MyTexts.GetString(title)).TrimTrailingWhitespace();
		}

		private static StringBuilder GetTitle(MyStringId title)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string @string = MyTexts.GetString(title);
			if (@string.Length > 0)
			{
				stringBuilder.Append(@string);
			}
			return stringBuilder;
		}

		private static StringBuilder CombineOnOff(MyStringId title, MyStringId? on = null, MyStringId? off = null)
		{
			return GetTitle(title).Append(" ").Append(MyTexts.GetString(on ?? MySpaceTexts.SwitchText_On)).Append("/")
				.Append(MyTexts.GetString(off ?? MySpaceTexts.SwitchText_Off));
		}

		public static void EnableActions<TBlock>(this MyTerminalControlSlider<TBlock> slider, float step = 0.05f, Func<TBlock, bool> enabled = null, Func<TBlock, bool> callable = null) where TBlock : MyTerminalBlock
		{
			slider.EnableActions(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE, step, enabled, callable);
		}

		public static void EnableActions<TBlock>(this MyTerminalControlSlider<TBlock> slider, string increaseIcon, string decreaseIcon, float step = 0.05f, Func<TBlock, bool> enabled = null, Func<TBlock, bool> callable = null) where TBlock : MyTerminalBlock
		{
			StringBuilder increaseName = Combine(MySpaceTexts.ToolbarAction_Increase, slider.Title);
			StringBuilder decreaseName = Combine(MySpaceTexts.ToolbarAction_Decrease, slider.Title);
			slider.EnableActions(increaseIcon, decreaseIcon, increaseName, decreaseName, step, null, null, enabled, callable);
		}

		public static void EnableActionsWithReset<TBlock>(this MyTerminalControlSlider<TBlock> slider, float step = 0.05f) where TBlock : MyTerminalBlock
		{
			slider.EnableActionsWithReset(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE, MyTerminalActionIcons.RESET, step);
		}

		public static void EnableActionsWithReset<TBlock>(this MyTerminalControlSlider<TBlock> slider, string increaseIcon, string decreaseIcon, string resetIcon, float step = 0.05f) where TBlock : MyTerminalBlock
		{
			StringBuilder increaseName = Combine(MySpaceTexts.ToolbarAction_Increase, slider.Title);
			StringBuilder decreaseName = Combine(MySpaceTexts.ToolbarAction_Decrease, slider.Title);
			StringBuilder resetName = Combine(MySpaceTexts.ToolbarAction_Reset, slider.Title);
			slider.EnableActions(increaseIcon, decreaseIcon, increaseName, decreaseName, step, resetIcon, resetName);
		}

		public static MyTerminalAction<TBlock> EnableAction<TBlock>(this MyTerminalControlButton<TBlock> button, string icon = null, MyStringId? title = null, MyTerminalControl<TBlock>.WriterDelegate writer = null) where TBlock : MyTerminalBlock
		{
			return button.EnableAction(icon ?? MyTerminalActionIcons.TOGGLE, MyTexts.Get(title ?? button.Title), writer);
		}

		public static MyTerminalAction<TBlock> EnableAction<TBlock>(this MyTerminalControlCheckbox<TBlock> checkbox, Func<TBlock, bool> callable = null) where TBlock : MyTerminalBlock
		{
			StringBuilder name = CombineOnOff(checkbox.Title);
			StringBuilder onText = MyTexts.Get(checkbox.OnText);
			StringBuilder offText = MyTexts.Get(checkbox.OffText);
			MyTerminalAction<TBlock> myTerminalAction = checkbox.EnableAction(MyTerminalActionIcons.TOGGLE, name, onText, offText);
			myTerminalAction.Enabled = checkbox.Enabled;
			if (callable != null)
			{
				myTerminalAction.Callable = callable;
			}
			return myTerminalAction;
		}

		public static MyTerminalAction<TBlock> EnableToggleAction<TBlock>(this MyTerminalControlOnOffSwitch<TBlock> onOff) where TBlock : MyTerminalBlock
		{
			return onOff.EnableToggleAction(MyTerminalActionIcons.TOGGLE);
		}

		public static MyTerminalAction<TBlock> EnableToggleAction<TBlock>(this MyTerminalControlOnOffSwitch<TBlock> onOff, string iconPath) where TBlock : MyTerminalBlock
		{
			StringBuilder name = CombineOnOff(onOff.Title, onOff.OnText, onOff.OffText);
			StringBuilder onText = MyTexts.Get(onOff.OnText);
			StringBuilder offText = MyTexts.Get(onOff.OffText);
			return onOff.EnableToggleAction(iconPath, name, onText, offText);
		}

		public static void EnableOnOffActions<TBlock>(this MyTerminalControlOnOffSwitch<TBlock> onOff) where TBlock : MyTerminalBlock
		{
			onOff.EnableOnOffActions(MyTerminalActionIcons.ON, MyTerminalActionIcons.OFF);
		}

		public static void EnableOnOffActions<TBlock>(this MyTerminalControlOnOffSwitch<TBlock> onOff, string onIcon, string OffIcon) where TBlock : MyTerminalBlock
		{
			StringBuilder stringBuilder = MyTexts.Get(onOff.OnText);
			StringBuilder stringBuilder2 = MyTexts.Get(onOff.OffText);
			onOff.EnableOnAction(onIcon, GetTitle(onOff.Title).Append(" ").Append((object)stringBuilder), stringBuilder, stringBuilder2);
			onOff.EnableOffAction(OffIcon, GetTitle(onOff.Title).Append(" ").Append((object)stringBuilder2), stringBuilder, stringBuilder2);
		}
	}
}
