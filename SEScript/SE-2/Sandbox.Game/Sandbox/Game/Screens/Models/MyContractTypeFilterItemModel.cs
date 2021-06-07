using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.Screens.Models
{
	public class MyContractTypeFilterItemModel : BindableBase
	{
		public class MyComparator_LocalizedName : IComparer<MyContractTypeFilterItemModel>
		{
			public int Compare(MyContractTypeFilterItemModel x, MyContractTypeFilterItemModel y)
			{
				if (y == null)
				{
					return 1;
				}
				if (x == null)
				{
					return -1;
				}
				return string.Compare(x.LocalizedName, y.LocalizedName);
			}
		}

		private string m_name;

		private string m_localizedName;

		private BitmapImage m_icon;

		private MyDefinitionId? m_contractTypeId;

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

		public string LocalizedName
		{
			get
			{
				return m_localizedName;
			}
			set
			{
				SetProperty(ref m_localizedName, value, "LocalizedName");
			}
		}

		public BitmapImage Icon
		{
			get
			{
				return m_icon;
			}
			set
			{
				SetProperty(ref m_icon, value, "Icon");
			}
		}

		public MyDefinitionId? ContractTypeId
		{
			get
			{
				return m_contractTypeId;
			}
			set
			{
				SetProperty(ref m_contractTypeId, value, "ContractTypeId");
			}
		}
	}
}
