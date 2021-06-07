using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	/// <summary>
	/// Implements player trading screen
	/// </summary>
	public class MyGuiScreenTradePlayer : MyGuiScreenMvvmBase
	{
		public MyGuiScreenTradePlayer(MyPlayerTradeViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenTradePlayer OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenTradePlayer";
		}

		public override UIRoot CreateView()
		{
			return new PlayerTradeView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool CloseScreen()
		{
			MyLog.Default.WriteLine("MyGuiScreenTradePlayer CLOSE");
			return base.CloseScreen();
		}
	}
}
