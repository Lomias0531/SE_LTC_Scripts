using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyAdminMenuControlHelper : MyAbstractControlMenuItem
	{
		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowAdminMenu);

		public MyAdminMenuControlHelper()
			: base("F10", MySupportKeysEnum.ALT)
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.AdminMenuScreen));
		}
	}
}
