using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyEditFactionIconViewModelLocator : ViewModelLocatorBase<IMyEditFactionIconViewModel>
	{
		public MyEditFactionIconViewModelLocator()
			: base(isDesignMode: true)
		{
		}

		public MyEditFactionIconViewModelLocator(bool isDesignMode = true)
			: base(isDesignMode: true)
		{
		}

		public override void Initialize(bool isDesignMode)
		{
			base.IsInDesignMode = false;
		}
	}
}
