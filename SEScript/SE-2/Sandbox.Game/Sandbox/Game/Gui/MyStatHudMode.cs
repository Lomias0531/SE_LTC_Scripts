using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage.Audio;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatHudMode : MyStatBase
	{
		public MyStatHudMode()
		{
			base.Id = MyStringHash.GetOrCompute("hud_mode");
			base.CurrentValue = MySandboxGame.Config.HudState;
		}

		public override void Update()
		{
			base.CurrentValue = MyHud.HudState;
			if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.TOGGLE_HUD) && MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay && !MyInput.Static.IsAnyAltKeyPressed())
			{
				base.CurrentValue += 1f;
				if (base.CurrentValue > 2f)
				{
					base.CurrentValue = 0f;
				}
				MyHud.HudState = (int)base.CurrentValue;
				MyHud.MinimalHud = MyHud.IsHudMinimal;
				MyGuiAudio.PlaySound(MyGuiSounds.HudClick);
			}
		}
	}
}
