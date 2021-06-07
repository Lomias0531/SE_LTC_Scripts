using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Utils;

namespace Sandbox.Game.Screens.Models
{
	public class MyStoreItemModel : BindableBase
	{
		private long m_totalPrice;

		private int m_pricePerUnit;

		private int m_amount;

		private string m_name;

		private string m_description;

		private BitmapImage m_icon;

		private bool m_hasTooltip;

		private BitmapImage m_tooltipImage;

		private BitmapImage m_currencyIcon;

		private ItemTypes m_itemTypes;

		private StoreItemTypes m_storeItemType;

		private bool m_isOre;

		private int m_prefabTotalPcu;

		private float m_perPriceDiscount;

		private bool m_hasPricePerUnitDiscount;

		private bool m_hasNormalPrice;

		public long Id
		{
			get;
			set;
		}

		public int PricePerUnit
		{
			get
			{
				return m_pricePerUnit;
			}
			set
			{
				SetProperty(ref m_pricePerUnit, value, "PricePerUnit");
				RaisePropertyChanged("PricePerUnit");
			}
		}

		public string PricePerUnitFormatted => MyBankingSystem.GetFormatedValue(PricePerUnit);

		public long TotalPrice
		{
			get
			{
				return m_totalPrice;
			}
			set
			{
				SetProperty(ref m_totalPrice, value, "TotalPrice");
				RaisePropertyChanged("TotalPriceFormatted");
			}
		}

		public string TotalPriceFormatted => MyBankingSystem.GetFormatedValue(TotalPrice);

		public BitmapImage CurrencyIcon
		{
			get
			{
				return m_currencyIcon;
			}
			set
			{
				SetProperty(ref m_currencyIcon, value, "CurrencyIcon");
			}
		}

		public int Amount
		{
			get
			{
				return m_amount;
			}
			set
			{
				SetProperty(ref m_amount, value, "Amount");
				RaisePropertyChanged("AmountFormatted");
			}
		}

		public string AmountFormatted
		{
			get
			{
				switch (ItemType)
				{
				case ItemTypes.PhysicalItem:
					if (IsOre)
					{
						return MyValueFormatter.GetFormattedOreAmount(Amount);
					}
					return MyValueFormatter.GetFormattedPiecesAmount(Amount);
				case ItemTypes.Oxygen:
					return MyValueFormatter.GetFormattedGasAmount(Amount);
				case ItemTypes.Hydrogen:
					return MyValueFormatter.GetFormattedGasAmount(Amount);
				case ItemTypes.Grid:
					return MyValueFormatter.GetFormattedPiecesAmount(Amount);
				default:
					return MyValueFormatter.GetFormatedInt(Amount);
				}
			}
		}

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

		public string Description
		{
			get
			{
				return m_description;
			}
			set
			{
				SetProperty(ref m_description, value, "Description");
			}
		}

		public bool HasTooltip
		{
			get
			{
				return m_hasTooltip;
			}
			set
			{
				SetProperty(ref m_hasTooltip, value, "HasTooltip");
			}
		}

		public BitmapImage TooltipImage
		{
			get
			{
				return m_tooltipImage;
			}
			set
			{
				SetProperty(ref m_tooltipImage, value, "TooltipImage");
			}
		}

		public bool IsOre
		{
			get
			{
				return m_isOre;
			}
			set
			{
				SetProperty(ref m_isOre, value, "IsOre");
				RaisePropertyChanged("AmountFormatted");
			}
		}

		public ItemTypes ItemType
		{
			get
			{
				return m_itemTypes;
			}
			set
			{
				SetProperty(ref m_itemTypes, value, "ItemType");
				RaisePropertyChanged("AmountFormatted");
			}
		}

		public int PrefabTotalPcu
		{
			get
			{
				return m_prefabTotalPcu;
			}
			set
			{
				SetProperty(ref m_prefabTotalPcu, value, "PrefabTotalPcu");
			}
		}

		public StoreItemTypes StoreItemType
		{
			get
			{
				return m_storeItemType;
			}
			set
			{
				SetProperty(ref m_storeItemType, value, "StoreItemType");
			}
		}

		public bool IsOffer => m_storeItemType == StoreItemTypes.Offer;

		public bool IsOrder => m_storeItemType == StoreItemTypes.Order;

		public MyDefinitionId ItemDefinitionId
		{
			get;
			set;
		}

		public float PricePerUnitDiscount
		{
			get
			{
				return m_perPriceDiscount;
			}
			set
			{
				SetProperty(ref m_perPriceDiscount, value, "PricePerUnitDiscount");
				HasPricePerUnitDiscount = (m_perPriceDiscount > 0f);
				HasNormalPrice = !HasPricePerUnitDiscount;
			}
		}

		public bool HasPricePerUnitDiscount
		{
			get
			{
				return m_hasPricePerUnitDiscount;
			}
			set
			{
				SetProperty(ref m_hasPricePerUnitDiscount, value, "HasPricePerUnitDiscount");
			}
		}

		public bool HasNormalPrice
		{
			get
			{
				return m_hasNormalPrice;
			}
			set
			{
				SetProperty(ref m_hasNormalPrice, value, "HasNormalPrice");
			}
		}

		public MyStoreItemModel()
		{
			BitmapImage bitmapImage = new BitmapImage();
			string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
			bitmapImage.TextureAsset = ((icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty);
			CurrencyIcon = bitmapImage;
		}

		public void SetIcon(string iconPath)
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.TextureAsset = iconPath;
			Icon = bitmapImage;
		}

		public void SetTooltipImage(string imagePath)
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.TextureAsset = imagePath;
			TooltipImage = bitmapImage;
		}
	}
}
