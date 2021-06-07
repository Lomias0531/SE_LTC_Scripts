using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.ViewModels
{
	public abstract class MyViewModelBase : ViewModelBase
	{
		private ColorW m_backgroundOverlay;

		private ICommand m_exitCommand;

		private float m_maxWidth;

		public ColorW BackgroundOverlay
		{
			get
			{
				return m_backgroundOverlay;
			}
			set
			{
				SetProperty(ref m_backgroundOverlay, value, "BackgroundOverlay");
			}
		}

		public ICommand ExitCommand
		{
			get
			{
				return m_exitCommand;
			}
			set
			{
				SetProperty(ref m_exitCommand, value, "ExitCommand");
			}
		}

		public float MaxWidth
		{
			get
			{
				return m_maxWidth;
			}
			set
			{
				SetProperty(ref m_maxWidth, value, "MaxWidth");
			}
		}

		public MyViewModelBase()
		{
			ExitCommand = new RelayCommand(OnExit);
		}

		public virtual void InitializeData()
		{
		}

		public void OnExit(object obj)
		{
			MyScreenManager.GetScreenWithFocus().CloseScreen();
		}

		public virtual void OnScreenClosing()
		{
		}
	}
}
