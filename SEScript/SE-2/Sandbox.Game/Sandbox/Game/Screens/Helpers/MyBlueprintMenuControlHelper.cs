using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyBlueprintMenuControlHelper : MyAbstractControlMenuItem
	{
		public override string Label => MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowBlueprints);

		public MyBlueprintMenuControlHelper()
			: base("F10")
		{
		}

		public override void Activate()
		{
			MyScreenManager.CloseScreen(typeof(MyGuiScreenControlMenu));
			if (MyFakes.I_AM_READY_FOR_NEW_BLUEPRINT_SCREEN)
			{
				MyGuiSandbox.AddScreen(MyGuiBlueprintScreen_Reworked.CreateBlueprintScreen(MyClipboardComponent.Static.Clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL));
			}
			else
			{
				MyGuiSandbox.AddScreen(new MyGuiBlueprintScreen(MyClipboardComponent.Static.Clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL));
			}
		}
	}
}
