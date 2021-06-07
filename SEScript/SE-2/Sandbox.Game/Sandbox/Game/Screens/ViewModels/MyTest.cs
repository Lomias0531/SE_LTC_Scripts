using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyTest : BindableBase
	{
		private string text;

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				SetProperty(ref text, value, "Text");
			}
		}
	}
}
