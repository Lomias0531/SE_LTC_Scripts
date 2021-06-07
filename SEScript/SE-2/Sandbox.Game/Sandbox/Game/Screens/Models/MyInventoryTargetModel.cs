using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.Entities.Blocks;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Models
{
	public class MyInventoryTargetModel : BindableBase
	{
		private string m_name;

		private float m_volume;

		private float m_maxVolume;

		private BitmapImage m_icon;

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

		public long EntityId
		{
			get;
			set;
		}

		public bool AllInventories
		{
			get;
			set;
		}

		public float MaxVolume
		{
			get
			{
				return m_maxVolume;
			}
			set
			{
				SetProperty(ref m_maxVolume, value, "MaxVolume");
			}
		}

		public float Volume
		{
			get
			{
				return m_volume;
			}
			set
			{
				SetProperty(ref m_volume, value, "Volume");
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

		public MyInventoryBase Inventory
		{
			get;
			private set;
		}

		public MyGasTank GasTank
		{
			get;
			set;
		}

		public MyInventoryTargetModel(MyInventoryBase inventoryBase)
		{
			Inventory = inventoryBase;
		}
	}
}
