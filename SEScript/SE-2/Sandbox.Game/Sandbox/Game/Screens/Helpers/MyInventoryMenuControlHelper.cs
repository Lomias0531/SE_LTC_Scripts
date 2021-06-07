using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyInventoryMenuControlHelper : MyAbstractControlMenuItem
	{
		private IMyControllableEntity m_entity;

		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_OpenInventory);

		public void SetEntity(IMyControllableEntity entity)
		{
			m_entity = entity;
		}

		public MyInventoryMenuControlHelper()
			: base(MyControlsSpace.INVENTORY)
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			MyGuiScreenHudSpace.Static.HideScreen();
			m_entity.ShowInventory();
		}
	}
}
