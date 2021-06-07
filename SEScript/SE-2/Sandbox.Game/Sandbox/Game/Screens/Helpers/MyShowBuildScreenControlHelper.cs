using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyShowBuildScreenControlHelper : MyAbstractControlMenuItem
	{
		private IMyControllableEntity m_entity;

		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowBuildScreen);

		public MyShowBuildScreenControlHelper()
			: base(MyControlsSpace.BUILD_SCREEN)
		{
		}

		public void SetEntity(IMyControllableEntity entity)
		{
			m_entity = entity;
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			MyGuiScreenHudSpace.Static.HideScreen();
			MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, m_entity as MyShipController, null));
		}
	}
}
