using Sandbox.Graphics.GUI;

namespace Sandbox.Game.SessionComponents
{
	internal class MyAnselGuiScreen : MyGuiScreenBase
	{
		public MyAnselGuiScreen()
			: base(null, null, null, isTopMostScreen: true)
		{
			base.DrawMouseCursor = false;
			m_canShareInput = false;
		}

		public override string GetFriendlyName()
		{
			return "MyAnselGuiScreen";
		}
	}
}
