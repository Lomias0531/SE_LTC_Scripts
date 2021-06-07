using Sandbox.Game.Localization;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyPauseToggleControlHelper : MyAbstractControlMenuItem
	{
		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_PauseGame);

		public MyPauseToggleControlHelper()
			: base(MyControlsSpace.PAUSE_GAME)
		{
		}

		public override void Activate()
		{
			MySandboxGame.PauseToggle();
		}
	}
}
