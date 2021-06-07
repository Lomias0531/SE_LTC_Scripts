using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.Entities;

namespace Sandbox.Game.Screens.Models
{
	public class MySimpleSelectableItemModel : BindableBase
	{
		private string m_name;

		private string m_displayName;

		private long m_id;

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

		public string DisplayName
		{
			get
			{
				return m_displayName;
			}
			set
			{
				SetProperty(ref m_displayName, value, "DisplayName");
			}
		}

		public long Id
		{
			get
			{
				return m_id;
			}
			set
			{
				SetProperty(ref m_id, value, "Id");
			}
		}

		public MySimpleSelectableItemModel()
		{
			Name = string.Empty;
			DisplayName = string.Empty;
			Id = 0L;
		}

		public MySimpleSelectableItemModel(MyCubeGrid grid)
		{
			Name = grid.Name;
			DisplayName = grid.DisplayName;
			Id = grid.EntityId;
		}

		public MySimpleSelectableItemModel(long id, string name, string displayName)
		{
			Name = name;
			DisplayName = displayName;
			Id = id;
		}
	}
}
