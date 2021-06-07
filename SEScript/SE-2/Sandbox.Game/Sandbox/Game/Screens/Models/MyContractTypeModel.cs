using EmptyKeys.UserInterface.Mvvm;

namespace Sandbox.Game.Screens.Models
{
	public class MyContractTypeModel : BindableBase
	{
		private string m_name;

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				SetProperty(ref m_name, value, "Name");
			}
		}
	}
}
