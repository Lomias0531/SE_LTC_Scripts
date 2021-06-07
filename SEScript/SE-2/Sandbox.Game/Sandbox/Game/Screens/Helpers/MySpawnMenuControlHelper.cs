using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MySpawnMenuControlHelper : MyAbstractControlMenuItem
	{
		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowSpawnMenu);

		public override bool Enabled
		{
			get
			{
				if (!MySession.Static.IsAdminMenuEnabled || MyPerGameSettings.Game == GameEnum.UNKNOWN_GAME)
				{
					return false;
				}
				return true;
			}
		}

		public MySpawnMenuControlHelper()
			: base("F10", MySupportKeysEnum.SHIFT)
		{
		}

		public override void Activate()
		{
			if (!MySession.Static.IsAdminMenuEnabled || MyPerGameSettings.Game == GameEnum.UNKNOWN_GAME)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.AdminMenuNotAvailable);
				return;
			}
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.VoxelMapEditingScreen));
		}
	}
}
