using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Graphics
{
	public interface IMyGuiScreenFactoryService
	{
		bool IsAnyScreenOpen
		{
			get;
			set;
		}

		void CreateScreen(ViewModelBase viewModel);
	}
}
