using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenActiveContracts : MyGuiScreenMvvmBase
	{
		public MyGuiScreenActiveContracts(MyContractsActiveViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenActiveContracts OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenActiveContracts";
		}

		public override UIRoot CreateView()
		{
			return new ActiveContractsView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool CloseScreen()
		{
			MyLog.Default.WriteLine("MyGuiScreenActiveContracts CLOSE");
			return base.CloseScreen();
		}
	}
}
