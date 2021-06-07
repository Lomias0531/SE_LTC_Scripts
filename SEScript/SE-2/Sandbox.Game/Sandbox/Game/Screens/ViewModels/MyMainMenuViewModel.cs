using EmptyKeys.UserInterface.Mvvm;
using System.Collections.ObjectModel;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyMainMenuViewModel : ViewModelBase
	{
		private ObservableCollection<MyTest> m_tests = new ObservableCollection<MyTest>();

		public ObservableCollection<MyTest> GridData
		{
			get
			{
				return m_tests;
			}
			set
			{
				SetProperty(ref m_tests, value, "GridData");
			}
		}

		public MyMainMenuViewModel()
		{
			m_tests.Add(new MyTest
			{
				Text = "Line 1"
			});
			m_tests.Add(new MyTest
			{
				Text = "Line 2"
			});
			m_tests.Add(new MyTest
			{
				Text = "Line 3"
			});
			m_tests.Add(new MyTest
			{
				Text = "Line 4"
			});
			m_tests.Add(new MyTest
			{
				Text = "Line 5"
			});
			m_tests.Add(new MyTest
			{
				Text = "Line 6"
			});
			m_tests.Add(new MyTest
			{
				Text = "Line 7"
			});
			m_tests.Add(new MyTest
			{
				Text = "Line 8"
			});
			m_tests.Add(new MyTest
			{
				Text = "Line 9"
			});
		}
	}
}
