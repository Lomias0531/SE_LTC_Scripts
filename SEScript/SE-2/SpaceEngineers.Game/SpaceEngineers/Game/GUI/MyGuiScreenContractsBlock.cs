using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using Sandbox;
using Sandbox.Game.Screens.ViewModels;
using VRage.Utils;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenContractsBlock : MyGuiScreenMvvmBase
	{
		public MyGuiScreenContractsBlock(MyContractsBlockViewModel viewModel)
			: base(viewModel)
		{
			MyLog.Default.WriteLine("MyGuiScreenContractsBlock OPEN");
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenContractsBlock";
		}

		public override UIRoot CreateView()
		{
			return new ContractsBlockView(MySandboxGame.ScreenSize.X, MySandboxGame.ScreenSize.Y);
		}

		public override bool CloseScreen()
		{
			MyLog.Default.WriteLine("MyGuiScreenContractsBlock CLOSE");
			return base.CloseScreen();
		}
	}
}
