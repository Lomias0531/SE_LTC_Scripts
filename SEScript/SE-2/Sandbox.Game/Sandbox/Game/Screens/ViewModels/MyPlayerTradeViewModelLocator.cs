using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyPlayerTradeViewModelLocator : ViewModelLocatorBase<IMyPlayerTradeViewModel>
	{
		public MyPlayerTradeViewModelLocator()
			: base(isDesignMode: true)
		{
		}

		public MyPlayerTradeViewModelLocator(bool isDesignMode = true)
			: base(isDesignMode: true)
		{
		}

		public override void Initialize(bool isDesignMode)
		{
			base.IsInDesignMode = false;
		}
	}
}
