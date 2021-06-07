using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;

namespace SpaceEngineers.Game.GUI
{
	public class MyGuiScreenFactoryService : IMyGuiScreenFactoryService
	{
		public bool IsAnyScreenOpen
		{
			get;
			set;
		}

		public void CreateScreen(ViewModelBase viewModel)
		{
			if (!IsAnyScreenOpen)
			{
				if (viewModel.GetType() == typeof(MyPlayerTradeViewModel))
				{
					MyGuiScreenTradePlayer screen = MyGuiSandbox.CreateScreen<MyGuiScreenTradePlayer>(new object[1]
					{
						viewModel
					});
					AddScreen(screen);
				}
				else if (viewModel.GetType() == typeof(MyEditFactionIconViewModel))
				{
					MyGuiScreenEditFactionIcon screen2 = MyGuiSandbox.CreateScreen<MyGuiScreenEditFactionIcon>(new object[1]
					{
						viewModel
					});
					AddScreen(screen2);
				}
				else if (viewModel.GetType() == typeof(MyContractsActiveViewModel))
				{
					MyGuiScreenActiveContracts screen3 = MyGuiSandbox.CreateScreen<MyGuiScreenActiveContracts>(new object[1]
					{
						viewModel
					});
					AddScreen(screen3);
				}
			}
		}

		private void AddScreen(MyGuiScreenBase screen)
		{
			MyGuiSandbox.AddScreen(screen);
			IsAnyScreenOpen = true;
			screen.Closed += Screen_Closed;
		}

		private void Screen_Closed(MyGuiScreenBase screen)
		{
			IsAnyScreenOpen = false;
			screen.Closed -= Screen_Closed;
		}
	}
}
