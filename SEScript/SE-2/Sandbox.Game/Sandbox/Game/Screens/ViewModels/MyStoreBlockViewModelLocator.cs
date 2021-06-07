using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyStoreBlockViewModelLocator : ViewModelLocatorBase<IMyStoreBlockViewModel>
	{
		public MyStoreBlockViewModelLocator()
			: base(isDesignMode: true)
		{
		}

		public MyStoreBlockViewModelLocator(bool isDesignMode = true)
			: base(isDesignMode: true)
		{
		}

		public override void Initialize(bool isDesignMode)
		{
			base.IsInDesignMode = false;
		}
	}
}
