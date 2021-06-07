using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Screens.Helpers;
using System.Text;
using VRage;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Models
{
	public class MyInventoryItemModel : BindableBase
	{
		private string m_name;

		private string m_iconSymbol;

		private float m_amount;

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

		public string IconSymbol
		{
			get
			{
				return m_iconSymbol;
			}
			set
			{
				SetProperty(ref m_iconSymbol, value, "IconSymbol");
			}
		}

		public float Amount
		{
			get
			{
				return m_amount;
			}
			set
			{
				SetProperty(ref m_amount, value, "Amount");
				MyPhysicalInventoryItem inventoryItem = InventoryItem;
				inventoryItem.Amount = (MyFixedPoint)m_amount;
				InventoryItem = inventoryItem;
				RaisePropertyChanged("AmountFormatted");
			}
		}

		public string AmountFormatted
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				MyGuiControlInventoryOwner.FormatItemAmount(InventoryItem, stringBuilder);
				return stringBuilder.ToString();
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

		public MyPhysicalInventoryItem InventoryItem
		{
			get;
			private set;
		}

		public MyInventoryBase Inventory
		{
			get;
			private set;
		}

		public MyInventoryItemModel()
		{
		}

		public MyInventoryItemModel(MyPhysicalInventoryItem inventoryItem, MyInventoryBase inventory)
		{
			MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition(inventoryItem.Content);
			Name = physicalItemDefinition.DisplayNameText;
			BitmapImage bitmapImage = new BitmapImage();
			string[] icons = physicalItemDefinition.Icons;
			if (icons != null && icons.Length != 0)
			{
				bitmapImage.TextureAsset = physicalItemDefinition.Icons[0];
			}
			Icon = bitmapImage;
			Amount = (float)inventoryItem.Amount;
			IconSymbol = (physicalItemDefinition.IconSymbol.HasValue ? MyTexts.GetString(physicalItemDefinition.IconSymbol.Value) : string.Empty);
			InventoryItem = inventoryItem;
			Inventory = inventory;
		}
	}
}
