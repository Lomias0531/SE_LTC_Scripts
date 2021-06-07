using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyQuickLoadControlHelper : MyAbstractControlMenuItem
	{
		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_QuickLoad);

		public MyQuickLoadControlHelper()
			: base("F5")
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			if (Sync.IsServer)
			{
				if (MyAsyncSaving.InProgress)
				{
					MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextSavingInProgress), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
					myGuiScreenMessageBox.SkipTransition = true;
					myGuiScreenMessageBox.InstantClose = false;
					MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
				}
				else
				{
					MyGuiScreenGamePlay.Static.ShowLoadMessageBox(MySession.Static.CurrentPath);
				}
			}
			else
			{
				MyGuiScreenGamePlay.Static.ShowReconnectMessageBox();
			}
		}
	}
}
