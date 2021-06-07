using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyPlayersMenuControlHelper : MyAbstractControlMenuItem
	{
		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowPlayers);

		public MyPlayersMenuControlHelper()
			: base("F3")
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.PlayersScreen));
		}
	}
}
