using EmptyKeys.UserInterface.Input;

namespace Sandbox.Game.Screens.ViewModels
{
	public interface IMyPlayerTradeViewModel
	{
		ICommand RemoveItemFromOfferCommand
		{
			get;
			set;
		}

		ICommand AddItemToOfferCommand
		{
			get;
			set;
		}

		ICommand AddStackTenToOfferCommand
		{
			get;
			set;
		}

		ICommand AddStackHundredToOfferCommand
		{
			get;
			set;
		}
	}
}
