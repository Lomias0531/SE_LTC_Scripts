using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyStoreBlockAdministrationViewModel : ViewModelBase
	{
		private MyStoreBlock m_storeBlock;

		private bool m_isCreateOfferEnabled;

		private bool m_isCreateOrderEnabled;

		private bool m_isTabNewOrderVisible = true;

		private float m_offerAmount;

		private float m_offerPricePerUnit;

		private float m_offerAmountMaximum;

		private float m_offerPricePerUnitMinimum;

		private float m_orderAmount;

		private float m_orderPricePerUnitMaximum;

		private float m_orderPricePerUnit;

		private string m_orderTotalPrice;

		private string m_offerTotalPrice;

		private string m_offerListingFee;

		private string m_orderListingFee;

		private string m_offerTransactionFee;

		private string m_localPlayerCurrency;

		public int m_orderCount;

		public int m_offerCount;

		public int m_orderOfferCountMax;

		private ICommand m_createOfferCommand;

		private ICommand m_createOrderCommand;

		private ICommand m_removeStoreItemCommand;

		private MyOrderItemModel m_selectedOrderItem;

		private MyStoreItemModel m_selectedStoreItem;

		private MyOrderItemModel m_selectedOfferItem;

		private ObservableCollection<MyStoreItemModel> m_storeItems = new ObservableCollection<MyStoreItemModel>();

		private ObservableCollection<MyOrderItemModel> m_orderItems = new ObservableCollection<MyOrderItemModel>();

		private MyMinimalPriceCalculator m_priceCalculator = new MyMinimalPriceCalculator();

		private MyAccountInfo m_localPlayerAccountInfo;

		private BitmapImage m_currencyIcon;

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

		public bool IsCreateOrderEnabled
		{
			get
			{
				return m_isCreateOrderEnabled;
			}
			set
			{
				SetProperty(ref m_isCreateOrderEnabled, value, "IsCreateOrderEnabled");
			}
		}

		public bool IsCreateOfferEnabled
		{
			get
			{
				return m_isCreateOfferEnabled;
			}
			set
			{
				SetProperty(ref m_isCreateOfferEnabled, value, "IsCreateOfferEnabled");
			}
		}

		public float OfferAmount
		{
			get
			{
				return m_offerAmount;
			}
			set
			{
				SetProperty(ref m_offerAmount, value, "OfferAmount");
				UpdateOffer();
			}
		}

		public float OfferPricePerUnit
		{
			get
			{
				return m_offerPricePerUnit;
			}
			set
			{
				SetProperty(ref m_offerPricePerUnit, value, "OfferPricePerUnit");
				UpdateOffer();
			}
		}

		public string OfferTotalPrice
		{
			get
			{
				return m_offerTotalPrice;
			}
			set
			{
				SetProperty(ref m_offerTotalPrice, value, "OfferTotalPrice");
			}
		}

		public float OfferAmountMaximum
		{
			get
			{
				return m_offerAmountMaximum;
			}
			set
			{
				SetProperty(ref m_offerAmountMaximum, value, "OfferAmountMaximum");
			}
		}

		public float OfferPricePerUnitMinimum
		{
			get
			{
				return m_offerPricePerUnitMinimum;
			}
			set
			{
				SetProperty(ref m_offerPricePerUnitMinimum, value, "OfferPricePerUnitMinimum");
			}
		}

		public string OfferListingFee
		{
			get
			{
				return m_offerListingFee;
			}
			set
			{
				SetProperty(ref m_offerListingFee, value, "OfferListingFee");
			}
		}

		public string OfferTransactionFee
		{
			get
			{
				return m_offerTransactionFee;
			}
			set
			{
				SetProperty(ref m_offerTransactionFee, value, "OfferTransactionFee");
			}
		}

		public float OrderAmount
		{
			get
			{
				return m_orderAmount;
			}
			set
			{
				SetProperty(ref m_orderAmount, value, "OrderAmount");
				UpdateOrder();
			}
		}

		public float OrderPricePerUnitMaximum
		{
			get
			{
				return m_orderPricePerUnitMaximum;
			}
			set
			{
				SetProperty(ref m_orderPricePerUnitMaximum, value, "OrderPricePerUnitMaximum");
			}
		}

		public float OrderPricePerUnit
		{
			get
			{
				return m_orderPricePerUnit;
			}
			set
			{
				SetProperty(ref m_orderPricePerUnit, value, "OrderPricePerUnit");
				UpdateOrder();
			}
		}

		public string OrderTotalPrice
		{
			get
			{
				return m_orderTotalPrice;
			}
			set
			{
				SetProperty(ref m_orderTotalPrice, value, "OrderTotalPrice");
			}
		}

		public string OrderListingFee
		{
			get
			{
				return m_orderListingFee;
			}
			set
			{
				SetProperty(ref m_orderListingFee, value, "OrderListingFee");
			}
		}

		public ICommand CreateOfferCommand
		{
			get
			{
				return m_createOfferCommand;
			}
			set
			{
				SetProperty(ref m_createOfferCommand, value, "CreateOfferCommand");
			}
		}

		public ICommand CreateOrderCommand
		{
			get
			{
				return m_createOrderCommand;
			}
			set
			{
				SetProperty(ref m_createOrderCommand, value, "CreateOrderCommand");
			}
		}

		public ICommand RemoveStoreItemCommand
		{
			get
			{
				return m_removeStoreItemCommand;
			}
			set
			{
				SetProperty(ref m_removeStoreItemCommand, value, "RemoveStoreItemCommand");
			}
		}

		public ObservableCollection<MyOrderItemModel> OrderItems
		{
			get
			{
				return m_orderItems;
			}
			set
			{
				SetProperty(ref m_orderItems, value, "OrderItems");
			}
		}

		public MyOrderItemModel SelectedOrderItem
		{
			get
			{
				return m_selectedOrderItem;
			}
			set
			{
				SetProperty(ref m_selectedOrderItem, value, "SelectedOrderItem");
				UpdateMaximumOrderPrice();
			}
		}

		public MyOrderItemModel SelectedOfferItem
		{
			get
			{
				return m_selectedOfferItem;
			}
			set
			{
				SetProperty(ref m_selectedOfferItem, value, "SelectedOfferItem");
				UpdateMinimumOfferPrice();
			}
		}

		public string LocalPlayerCurrency
		{
			get
			{
				return m_localPlayerCurrency;
			}
			set
			{
				SetProperty(ref m_localPlayerCurrency, value, "LocalPlayerCurrency");
			}
		}

		public int OrderCount
		{
			get
			{
				return m_orderCount;
			}
			set
			{
				SetProperty(ref m_orderCount, value, "OrderCount");
			}
		}

		public int OfferCount
		{
			get
			{
				return m_offerCount;
			}
			set
			{
				SetProperty(ref m_offerCount, value, "OfferCount");
			}
		}

		public int OrderOfferCountMax
		{
			get
			{
				return m_orderOfferCountMax;
			}
			set
			{
				SetProperty(ref m_orderOfferCountMax, value, "OrderOfferCountMax");
			}
		}

		public ObservableCollection<MyStoreItemModel> StoreItems
		{
			get
			{
				return m_storeItems;
			}
			set
			{
				SetProperty(ref m_storeItems, value, "StoreItems");
			}
		}

		public MyStoreItemModel SelectedStoreItem
		{
			get
			{
				return m_selectedStoreItem;
			}
			set
			{
				SetProperty(ref m_selectedStoreItem, value, "SelectedStoreItem");
			}
		}

		public bool IsTabNewOrderVisible
		{
			get
			{
				return m_isTabNewOrderVisible;
			}
			set
			{
				SetProperty(ref m_isTabNewOrderVisible, value, "IsTabNewOrderVisible");
			}
		}

		public event EventHandler OfferCreated;

		public event EventHandler OrderCreated;

		public event EventHandler StoreItemRemoved;

		public MyStoreBlockAdministrationViewModel(MyStoreBlock storeBlock)
		{
			m_storeBlock = storeBlock;
			CreateOfferCommand = new RelayCommand(OnCreateOffer);
			CreateOrderCommand = new RelayCommand(OnCreateOrder);
			RemoveStoreItemCommand = new RelayCommand(OnRemoveStoreItem);
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			OrderOfferCountMax = component.GetStoreCreationLimitPerPlayer();
			UpdateLocalPlayerAccountBalance();
			BitmapImage bitmapImage = new BitmapImage();
			string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
			bitmapImage.TextureAsset = ((icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty);
			CurrencyIcon = bitmapImage;
		}

		internal void Initialize()
		{
			List<MyOrderItemModel> list = new List<MyOrderItemModel>();
			foreach (MyPhysicalItemDefinition physicalItemDefinition in MyDefinitionManager.Static.GetPhysicalItemDefinitions())
			{
				if (physicalItemDefinition.Public && physicalItemDefinition.Enabled && physicalItemDefinition.CanPlayerOrder)
				{
					list.Add(new MyOrderItemModel(physicalItemDefinition));
				}
			}
			list.SortNoAlloc((MyOrderItemModel x, MyOrderItemModel y) => string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase));
			OrderItems = new ObservableCollection<MyOrderItemModel>(list);
		}

		private void OnCreateOffer(object obj)
		{
			if (SelectedOfferItem == null)
			{
				return;
			}
			if (OrderCount + OfferCount >= OrderOfferCountMax)
			{
				ShowErrorFailNotification(MyStoreCreationResult.Fail_CreationLimitHard);
				return;
			}
			int minimumOfferPrice = GetMinimumOfferPrice();
			if (float.IsNaN(OfferPricePerUnit) || (float)minimumOfferPrice > OfferPricePerUnit)
			{
				string formatedValue = MyBankingSystem.GetFormatedValue(minimumOfferPrice);
				StringBuilder text = new StringBuilder().AppendFormat(MySpaceTexts.StoreBuy_Error_Text_WrongOfferPricePerUnit, formatedValue);
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongOfferPricePerUnit), text);
			}
			else
			{
				MyDefinitionId id = SelectedOfferItem.ItemDefinition.Id;
				m_storeBlock.CreateNewOfferRequest(id, (int)OfferAmount, (int)OfferPricePerUnit, OnCreateOfferResult);
			}
		}

		private void ShowMessageBoxError(StringBuilder caption, StringBuilder text)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, caption, null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		private void OnCreateOfferResult(MyStoreCreationResult result)
		{
			if (result == MyStoreCreationResult.Success)
			{
				UpdateLocalPlayerAccountBalance();
				this.OfferCreated?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				ShowErrorFailNotification(result);
			}
		}

		private void UpdateLocalPlayerAccountBalance()
		{
			MyBankingSystem.Static.TryGetAccountInfo(MySession.Static.LocalPlayerId, out m_localPlayerAccountInfo);
			LocalPlayerCurrency = MyBankingSystem.GetFormatedValue(m_localPlayerAccountInfo.Balance);
		}

		private void UpdateMinimumOfferPrice()
		{
			int minimumOfferPrice = GetMinimumOfferPrice();
			OfferPricePerUnitMinimum = minimumOfferPrice;
			if (float.IsNaN(OfferPricePerUnit) || (float)minimumOfferPrice > OfferPricePerUnit)
			{
				OfferPricePerUnit = minimumOfferPrice;
			}
		}

		private int GetMinimumOfferPrice()
		{
			if (SelectedOfferItem == null)
			{
				return 0;
			}
			int result = 0;
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component != null)
			{
				MyDefinitionId id = SelectedOfferItem.ItemDefinition.Id;
				result = component.GetMinimumItemPrice(id);
			}
			return result;
		}

		private void UpdateOffer()
		{
			long num = 0L;
			if (!float.IsNaN(OfferAmount) && !float.IsNaN(OfferPricePerUnit))
			{
				num = (long)(OfferAmount * OfferPricePerUnit);
			}
			if (num < 0)
			{
				num = 0L;
			}
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component != null)
			{
				long valueToFormat = (long)((float)num * component.EconomyDefinition.ListingFee);
				OfferListingFee = MyBankingSystem.GetFormatedValue(valueToFormat);
				long valueToFormat2 = (long)((float)num * component.EconomyDefinition.TransactionFee);
				OfferTransactionFee = MyBankingSystem.GetFormatedValue(valueToFormat2);
			}
			IsCreateOfferEnabled = (num > 0 && SelectedOfferItem != null);
			OfferTotalPrice = MyBankingSystem.GetFormatedValue(num);
		}

		private void UpdateMaximumOrderPrice()
		{
			OrderPricePerUnitMaximum = 1E+09f;
		}

		private void UpdateOrder()
		{
			long num = 0L;
			if (!float.IsNaN(OrderAmount) && !float.IsNaN(OrderPricePerUnit))
			{
				num = (long)(OrderAmount * OrderPricePerUnit);
			}
			if (num < 0)
			{
				num = 0L;
			}
			MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
			if (component != null)
			{
				long valueToFormat = (long)((float)num * component.EconomyDefinition.ListingFee);
				OrderListingFee = MyBankingSystem.GetFormatedValue(valueToFormat);
			}
			IsCreateOrderEnabled = (num > 0 && SelectedOrderItem != null);
			OrderTotalPrice = MyBankingSystem.GetFormatedValue(num);
		}

		private void OnCreateOrder(object obj)
		{
			if (SelectedOrderItem != null)
			{
				if (OrderCount + OfferCount >= OrderOfferCountMax)
				{
					ShowErrorFailNotification(MyStoreCreationResult.Fail_CreationLimitHard);
				}
				else
				{
					m_storeBlock.CreateNewOrderRequest(SelectedOrderItem.ItemDefinition.Id, (int)OrderAmount, (int)OrderPricePerUnit, OnCreateOrderResult);
				}
			}
		}

		private void OnCreateOrderResult(MyStoreCreationResult result)
		{
			if (result == MyStoreCreationResult.Success)
			{
				UpdateLocalPlayerAccountBalance();
				this.OrderCreated?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				ShowErrorFailNotification(result);
			}
		}

		private void ShowErrorFailNotification(MyStoreCreationResult state)
		{
			switch (state)
			{
			case MyStoreCreationResult.Success:
				break;
			case MyStoreCreationResult.Fail_PricePerUnitIsLowerThanMinimum:
				break;
			case MyStoreCreationResult.Fail_CreationLimitHard:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Store_Error_Caption_OrderOfferLimitReachedHard), MyTexts.Get(MySpaceTexts.Store_Error_Text_OrderOfferLimitReachedHard));
				break;
			case MyStoreCreationResult.Error:
				MyLog.Default.Error(new StringBuilder("Contracts - error result: " + state));
				break;
			}
		}

		private void ShowMessageBoxOk(StringBuilder caption, StringBuilder text)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, caption, null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		private void ShowMessageBoxYesNo(StringBuilder caption, StringBuilder text, Action<MyGuiScreenMessageBox.ResultEnum> callback)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, text, caption, null, null, null, null, callback, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		private void OnRemoveStoreItem(object obj)
		{
			if (SelectedStoreItem != null)
			{
				m_storeBlock.CreateCancelStoreItemRequest(SelectedStoreItem.Id, OnCancelStoreItemResult);
			}
		}

		private void OnCancelStoreItemResult(bool result)
		{
			this.StoreItemRemoved?.Invoke(this, EventArgs.Empty);
		}
	}
}
