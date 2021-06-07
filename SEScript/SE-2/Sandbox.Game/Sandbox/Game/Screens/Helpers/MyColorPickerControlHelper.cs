using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyColorPickerControlHelper : MyAbstractControlMenuItem
	{
		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowColorPicker);

		public MyColorPickerControlHelper()
			: base(MyControlsSpace.LANDING_GEAR, MySupportKeysEnum.SHIFT)
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = new MyGuiScreenColorPicker());
		}
	}
}
