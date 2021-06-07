using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyShowTerminalControlHelper : MyAbstractControlMenuItem
	{
		private IMyControllableEntity m_entity;

		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_Terminal);

		public MyShowTerminalControlHelper()
			: base(MyControlsSpace.TERMINAL)
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			MyGuiScreenHudSpace.Static.HideScreen();
			m_entity.ShowTerminal();
		}

		public void SetEntity(IMyControllableEntity entity)
		{
			m_entity = entity;
		}
	}
}
