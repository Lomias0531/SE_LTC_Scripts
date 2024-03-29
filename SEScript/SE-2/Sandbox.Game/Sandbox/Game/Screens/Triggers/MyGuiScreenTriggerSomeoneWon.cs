using Sandbox.Game.Localization;
using Sandbox.Game.World.Triggers;
using VRageMath;

namespace Sandbox.Game.Screens.Triggers
{
	public class MyGuiScreenTriggerSomeoneWon : MyGuiScreenTrigger
	{
		public MyGuiScreenTriggerSomeoneWon(MyTrigger trg)
			: base(trg, new Vector2(0.5f, 0.3f))
		{
			AddCaption(MySpaceTexts.GuiTriggerCaptionSomeoneWon);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTriggerSomeoneWon";
		}
	}
}
