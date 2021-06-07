using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyHelpMenuControlHelper : MyAbstractControlMenuItem
	{
		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowHelp);

		public MyHelpMenuControlHelper()
			: base(MyControlsSpace.HELP_SCREEN)
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HelpScreen));
		}
	}
}
