using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyStoreBlockViewModel : MyViewModelBase, IMyStoreBlockViewModel
	{
		private bool m_isBuyEnabled;

		private bool m_isSellEnabled;

		private bool m_isRefreshEnabled;

		private bool m_isAdministrationVisible;

		private bool m_isPreviewEnabled;

		private bool m_isBuyTabVisible = true;

		private bool m_isSellTabVisible = true;

		private long m_lastEconomyTick;

		private float m_amountToSell;

		private float m_amountToBuy;

		private float m_amountToBuyMaximum;

		private int m_selectedOfferedItemIndex;

		private int m_selectedOrderedItemIndex;

		private int m_tabSelectedIndex;

		private string m_totalPriceToBuy;

		private string m_totalPriceToSell;

		private ICommand m_refreshCommand;

		private ICommand m_sellCommand;

		private ICommand m_buyCommand;

		private ICommand m_sortingOfferedItemsCommand;

		private ICommand m_sortingDemandedItemsCommand;

		private ICommand m_cancelOfferCommand;

		private ICommand m_cancelOrderCommand;

		private ICommand m_onBuyItemDoubleClickCommand;

		private ICommand m_setAllAmountOfferCommand;

		private ICommand m_setAllAmountOrderCommand;

		private ICommand m_showPreviewCommand;

		private ObservableCollection<MyStoreItemModel> m_offeredItems = new ObservableCollection<MyStoreItemModel>();

		private ObservableCollection<MyStoreItemModel> m_orderedItems = new ObservableCollection<MyStoreItemModel>();

		private MyStoreItemModel m_selectedOfferItem;

		private MyStoreItemModel m_selectedOrderItem;

		private MyStoreBlock m_storeBlock;

		public bool IsPreviewEnabled
		{
			get
			{
				return m_isPreviewEnabled;
			}
			set
			{
				SetProperty(ref m_isPreviewEnabled, value, "IsPreviewEnabled");
			}
		}

		public bool IsAdministrationVisible
		{
			get
			{
				return m_isAdministrationVisible;
			}
			set
			{
				SetProperty(ref m_isAdministrationVisible, value, "IsAdministrationVisible");
			}
		}

		public bool IsBuyTabVisible
		{
			get
			{
				return m_isBuyTabVisible;
			}
			set
			{
				SetProperty(ref m_isBuyTabVisible, value, "IsBuyTabVisible");
			}
		}

		public bool IsSellTabVisible
		{
			get
			{
				return m_isSellTabVisible;
			}
			set
			{
				SetProperty(ref m_isSellTabVisible, value, "IsSellTabVisible");
			}
		}

		public int TabSelectedIndex
		{
			get
			{
				return m_tabSelectedIndex;
			}
			set
			{
				SetProperty(ref m_tabSelectedIndex, value, "TabSelectedIndex");
			}
		}

		public bool IsBuyEnabled
		{
			get
			{
				return m_isBuyEnabled;
			}
			set
			{
				SetProperty(ref m_isBuyEnabled, value, "IsBuyEnabled");
			}
		}

		public bool IsSellEnabled
		{
			get
			{
				return m_isSellEnabled;
			}
			set
			{
				SetProperty(ref m_isSellEnabled, value, "IsSellEnabled");
			}
		}

		public bool IsRefreshEnabled
		{
			get
			{
				return m_isRefreshEnabled;
			}
			set
			{
				SetProperty(ref m_isRefreshEnabled, value, "IsRefreshEnabled");
			}
		}

		public int SelectedOfferedItemIndex
		{
			get
			{
				return m_selectedOfferedItemIndex;
			}
			set
			{
				SetProperty(ref m_selectedOfferedItemIndex, value, "SelectedOfferedItemIndex");
				UpdateOfferedInfo(m_selectedOfferedItemIndex);
			}
		}

		public int SelectedOrderedItemIndex
		{
			get
			{
				return m_selectedOrderedItemIndex;
			}
			set
			{
				SetProperty(ref m_selectedOrderedItemIndex, value, "SelectedOrderedItemIndex");
				UpdateOrderedInfo(m_selectedOrderedItemIndex);
			}
		}

		public string TotalPriceToSell
		{
			get
			{
				return m_totalPriceToSell;
			}
			set
			{
				SetProperty(ref m_totalPriceToSell, value, "TotalPriceToSell");
			}
		}

		public string TotalPriceToBuy
		{
			get
			{
				return m_totalPriceToBuy;
			}
			set
			{
				SetProperty(ref m_totalPriceToBuy, value, "TotalPriceToBuy");
			}
		}

		public float AmountToSell
		{
			get
			{
				return m_amountToSell;
			}
			set
			{
				if (m_selectedOrderItem != null)
				{
					UpdateTotalPriceToSell(value);
				}
				SetProperty(ref m_amountToSell, value, "AmountToSell");
			}
		}

		public float AmountToBuyMaximum
		{
			get
			{
				return m_amountToBuyMaximum;
			}
			set
			{
				SetProperty(ref m_amountToBuyMaximum, value, "AmountToBuyMaximum");
			}
		}

		public float AmountToBuy
		{
			get
			{
				return m_amountToBuy;
			}
			set
			{
				if (m_selectedOfferItem != null)
				{
					UpdateTotalPriceToBuy(value);
				}
				SetProperty(ref m_amountToBuy, value, "AmountToBuy");
			}
		}

		public ICommand SellCommand
		{
			get
			{
				return m_sellCommand;
			}
			set
			{
				SetProperty(ref m_sellCommand, value, "SellCommand");
			}
		}

		public ICommand BuyCommand
		{
			get
			{
				return m_buyCommand;
			}
			set
			{
				SetProperty(ref m_buyCommand, value, "BuyCommand");
			}
		}

		public ICommand SortingOfferedItemsCommand
		{
			get
			{
				return m_sortingOfferedItemsCommand;
			}
			set
			{
				SetProperty(ref m_sortingOfferedItemsCommand, value, "SortingOfferedItemsCommand");
			}
		}

		public ICommand SortingDemandedItemsCommand
		{
			get
			{
				return m_sortingDemandedItemsCommand;
			}
			set
			{
				SetProperty(ref m_sortingDemandedItemsCommand, value, "SortingDemandedItemsCommand");
			}
		}

		public ICommand RefreshCommand
		{
			get
			{
				return m_refreshCommand;
			}
			set
			{
				SetProperty(ref m_refreshCommand, value, "RefreshCommand");
			}
		}

		public ICommand OnBuyItemDoubleClickCommand
		{
			get
			{
				return m_onBuyItemDoubleClickCommand;
			}
			set
			{
				SetProperty(ref m_onBuyItemDoubleClickCommand, value, "OnBuyItemDoubleClickCommand");
			}
		}

		public ICommand SetAllAmountOfferCommand
		{
			get
			{
				return m_setAllAmountOfferCommand;
			}
			set
			{
				SetProperty(ref m_setAllAmountOfferCommand, value, "SetAllAmountOfferCommand");
			}
		}

		public ICommand SetAllAmountOrderCommand
		{
			get
			{
				return m_setAllAmountOrderCommand;
			}
			set
			{
				SetProperty(ref m_setAllAmountOrderCommand, value, "SetAllAmountOrderCommand");
			}
		}

		public ICommand ShowPreviewCommand
		{
			get
			{
				return m_showPreviewCommand;
			}
			set
			{
				SetProperty(ref m_showPreviewCommand, value, "ShowPreviewCommand");
			}
		}

		public ObservableCollection<MyStoreItemModel> OfferedItems
		{
			get
			{
				return m_offeredItems;
			}
			set
			{
				SetProperty(ref m_offeredItems, value, "OfferedItems");
			}
		}

		public ObservableCollection<MyStoreItemModel> OrderedItems
		{
			get
			{
				return m_orderedItems;
			}
			set
			{
				SetProperty(ref m_orderedItems, value, "OrderedItems");
			}
		}

		public MyInventoryTargetViewModel InventoryTargetViewModel
		{
			get;
			private set;
		}

		public MyStoreBlockAdministrationViewModel AdministrationViewModel
		{
			get;
			private set;
		}

		public MyStoreBlockViewModel(MyStoreBlock storeBlock)
		{
			BuyCommand = new RelayCommand(OnBuy);
			SellCommand = new RelayCommand(OnSell);
			RefreshCommand = new RelayCommand(OnRefresh);
			OnBuyItemDoubleClickCommand = new RelayCommand(OnBuyItemDoubleClick);
			SetAllAmountOfferCommand = new RelayCommand(OnSetAllAmountOffer);
			SetAllAmountOrderCommand = new RelayCommand(OnSetAllAmountOrder);
			ShowPreviewCommand = new RelayCommand(OnShowPreview);
			SortingOfferedItemsCommand = new RelayCommand<DataGridSortingEventArgs>(OnSortingOfferedItems);
			SortingDemandedItemsCommand = new RelayCommand<DataGridSortingEventArgs>(OnSortingDemandedItems);
			m_storeBlock = storeBlock;
			InventoryTargetViewModel = new MyInventoryTargetViewModel(storeBlock);
			IsAdministrationVisible = (m_storeBlock.HasLocalPlayerAccess() && m_storeBlock.OwnerId == MySession.Static.LocalPlayerId);
			AdministrationViewModel = new MyStoreBlockAdministrationViewModel(m_storeBlock);
			if (IsAdministrationVisible)
			{
				AdministrationViewModel.Initialize();
				AdministrationViewModel.OfferCreated += AdministrationViewModel_Changed;
				AdministrationViewModel.OrderCreated += AdministrationViewModel_Changed;
				AdministrationViewModel.StoreItemRemoved += AdministrationViewModel_Changed;
			}
			TotalPriceToBuy = "0";
			TotalPriceToSell = "0";
			ServiceManager.Instance.AddService((IMyStoreBlockViewModel)this);
		}

		public override void InitializeData()
		{
			m_storeBlock.CreateGetConnectedGridInventoriesRequest(OnGetInventories);
			RefreshStoreItems();
			if (!IsBuyTabVisible && TabSelectedIndex == 0 && IsAdministrationVisible)
			{
				TabSelectedIndex = 2;
			}
		}

		private void OnShowPreview(object obj)
		{
			if (m_selectedOfferItem != null)
			{
				m_storeBlock.ShowPreview(m_selectedOfferItem.Id);
			}
		}

		private void OnSetAllAmountOrder(object obj)
		{
			if (m_selectedOrderItem != null)
			{
				float itemAmount = InventoryTargetViewModel.GetItemAmount(m_selectedOrderItem.ItemDefinitionId);
				if (itemAmount != 0f)
				{
					AmountToSell = Math.Min(m_selectedOrderItem.Amount, itemAmount);
				}
			}
		}

		private void OnSetAllAmountOffer(object obj)
		{
			AmountToBuy = AmountToBuyMaximum;
		}

		private void OnBuyItemDoubleClick(object obj)
		{
			if (IsBuyEnabled)
			{
				OnBuy(obj);
			}
		}

		private void OnGetInventories(List<long> inventoryEntities)
		{
			InventoryTargetViewModel.Initialize(inventoryEntities, includeCharacterInventory: true, showAllOption: false);
		}

		private void UpdateLocalPlayerAccountBalance()
		{
			InventoryTargetViewModel.UpdateLocalPlayerCurrency();
		}

		private void OnRefresh(object obj)
		{
			RefreshStoreItems();
		}

		private void RefreshStoreItems()
		{
			IsRefreshEnabled = false;
			AmountToBuy = 0f;
			AmountToSell = 0f;
			m_selectedOrderItem = null;
			m_selectedOfferItem = null;
			m_storeBlock.CreateGetStoreItemsRequest(MySession.Static.LocalPlayerId, OnGetStoreItems);
		}

		private void OnGetStoreItems(List<MyStoreItem> storeItems, long lastEconomyTick, float offersBonus, float ordersBonus)
		{
			m_lastEconomyTick = lastEconomyTick;
			IsRefreshEnabled = true;
			ObservableCollection<MyStoreItemModel> observableCollection = new ObservableCollection<MyStoreItemModel>();
			ObservableCollection<MyStoreItemModel> observableCollection2 = new ObservableCollection<MyStoreItemModel>();
			ObservableCollection<MyStoreItemModel> observableCollection3 = new ObservableCollection<MyStoreItemModel>();
			foreach (MyStoreItem storeItem in storeItems)
			{
				if (storeItem.Amount != 0)
				{
					MyStoreItemModel myStoreItemModel = new MyStoreItemModel
					{
						Id = storeItem.Id,
						Amount = storeItem.Amount,
						ItemType = storeItem.ItemType,
						StoreItemType = storeItem.StoreItemType
					};
					switch (storeItem.ItemType)
					{
					case ItemTypes.PhysicalItem:
					{
						MyPhysicalItemDefinition definition = null;
						if (!MyDefinitionManager.Static.TryGetDefinition(storeItem.Item.Value, out definition))
						{
							continue;
						}
						myStoreItemModel.Name = definition.DisplayNameText;
						myStoreItemModel.Description = definition.DescriptionText;
						string[] icons2 = definition.Icons;
						if (icons2 != null && icons2.Length != 0)
						{
							myStoreItemModel.SetIcon(definition.Icons[0]);
						}
						myStoreItemModel.IsOre = definition.IsOre;
						myStoreItemModel.ItemDefinitionId = storeItem.Item.Value;
						break;
					}
					case ItemTypes.Oxygen:
						myStoreItemModel.Name = MyTexts.GetString(MySpaceTexts.DisplayName_Item_Oxygen);
						myStoreItemModel.SetIcon("Textures\\GUI\\Icons\\OxygenIcon.dds");
						break;
					case ItemTypes.Hydrogen:
						myStoreItemModel.Name = MyTexts.GetString(MySpaceTexts.DisplayName_Item_Hydrogen);
						myStoreItemModel.SetIcon("Textures\\GUI\\Icons\\HydrogenIcon.dds");
						break;
					case ItemTypes.Grid:
					{
						MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(storeItem.PrefabName);
						if (prefabDefinition != null)
						{
							if (!string.IsNullOrEmpty(prefabDefinition.DisplayNameString))
							{
								myStoreItemModel.Name = prefabDefinition.DisplayNameString;
								string[] icons = prefabDefinition.Icons;
								string icon = (icons != null && icons.Length != 0) ? prefabDefinition.Icons[0] : string.Empty;
								myStoreItemModel.SetIcon(icon);
								myStoreItemModel.Description = prefabDefinition.DescriptionString;
								myStoreItemModel.HasTooltip = true;
								string tooltipImage = prefabDefinition.TooltipImage;
								myStoreItemModel.SetTooltipImage(tooltipImage);
							}
							else
							{
								MyObjectBuilder_CubeGrid[] cubeGrids = prefabDefinition.CubeGrids;
								myStoreItemModel.Name = ((cubeGrids != null && cubeGrids.Length != 0) ? prefabDefinition.CubeGrids[0].DisplayName : string.Empty);
							}
						}
						myStoreItemModel.PrefabTotalPcu = storeItem.PrefabTotalPcu;
						break;
					}
					}
					switch (storeItem.StoreItemType)
					{
					case StoreItemTypes.Offer:
						myStoreItemModel.PricePerUnit = (int)((float)storeItem.PricePerUnit * offersBonus);
						if (offersBonus < 1f)
						{
							float num = (float)storeItem.PricePerUnit * (1f + storeItem.PricePerUnitDiscount);
							float num2 = (num > (float)myStoreItemModel.PricePerUnit) ? (1f - (float)myStoreItemModel.PricePerUnit / num) : 0f;
							myStoreItemModel.PricePerUnitDiscount = (float)Math.Round(num2 * 100f, 1);
						}
						else
						{
							myStoreItemModel.PricePerUnitDiscount = (float)Math.Round(storeItem.PricePerUnitDiscount * 100f, 1);
						}
						observableCollection.Add(myStoreItemModel);
						break;
					case StoreItemTypes.Order:
						myStoreItemModel.PricePerUnit = (int)((float)storeItem.PricePerUnit * ordersBonus);
						observableCollection2.Add(myStoreItemModel);
						break;
					}
					myStoreItemModel.TotalPrice = storeItem.Amount * myStoreItemModel.PricePerUnit;
					observableCollection3.Add(myStoreItemModel);
				}
			}
			OfferedItems = observableCollection;
			OrderedItems = observableCollection2;
			if (IsAdministrationVisible)
			{
				AdministrationViewModel.OfferCount = observableCollection.Count;
				AdministrationViewModel.OrderCount = observableCollection2.Count;
				AdministrationViewModel.StoreItems = observableCollection3;
			}
			SelectedOfferedItemIndex = -1;
			SelectedOrderedItemIndex = -1;
		}

		private void UpdateOfferedInfo(int itemIndex)
		{
			if (itemIndex >= OfferedItems.Count || itemIndex < 0)
			{
				AmountToBuy = 0f;
				AmountToBuyMaximum = 1f;
				m_selectedOfferItem = null;
				IsBuyEnabled = false;
				IsPreviewEnabled = false;
			}
			else
			{
				m_selectedOfferItem = OfferedItems[itemIndex];
				AmountToBuyMaximum = m_selectedOfferItem.Amount;
				AmountToBuy = 1f;
				IsPreviewEnabled = (m_selectedOfferItem.ItemType == ItemTypes.Grid);
			}
		}

		private void UpdateOrderedInfo(int itemIndex)
		{
			if (itemIndex >= OrderedItems.Count || itemIndex < 0)
			{
				AmountToSell = 0f;
				IsSellEnabled = false;
				m_selectedOrderItem = null;
			}
			else
			{
				m_selectedOrderItem = OrderedItems[itemIndex];
				AmountToSell = 1f;
				IsSellEnabled = true;
			}
		}

		private void UpdateTotalPriceToBuy(float amount)
		{
			long num = 0L;
			if (!float.IsNaN(amount))
			{
				num = (int)amount * m_selectedOfferItem.PricePerUnit;
			}
			TotalPriceToBuy = MyBankingSystem.GetFormatedValue(num);
			IsBuyEnabled = (InventoryTargetViewModel.LocalPlayerAccountInfo.Balance >= num && amount != 0f && !float.IsNaN(amount));
		}

		private void UpdateTotalPriceToSell(float amount)
		{
			long valueToFormat = 0L;
			if (!float.IsNaN(amount))
			{
				valueToFormat = (int)amount * m_selectedOrderItem.PricePerUnit;
			}
			TotalPriceToSell = MyBankingSystem.GetFormatedValue(valueToFormat);
			IsSellEnabled = ((float)m_selectedOrderItem.Amount >= amount && amount != 0f && !float.IsNaN(amount));
		}

		private void OnSell(object obj)
		{
			if (m_selectedOrderItem != null)
			{
				int num = (int)AmountToSell;
				if (num <= m_selectedOrderItem.Amount && !float.IsNaN(num) && InventoryTargetViewModel.SelectedInventoryIndex >= 0 && InventoryTargetViewModel.SelectedInventoryIndex < InventoryTargetViewModel.Inventories.Count)
				{
					long entityId = InventoryTargetViewModel.Inventories[InventoryTargetViewModel.SelectedInventoryIndex].EntityId;
					m_storeBlock.CreateSellItemRequest(m_selectedOrderItem.Id, num, entityId, m_lastEconomyTick, OnSellResult);
					IsSellEnabled = false;
				}
			}
		}

		private void OnSellResult(MyStoreSellItemResult result)
		{
			switch (result.Result)
			{
			case MyStoreSellItemResults.ItemNotFound:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemNotFound), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemNotFound));
				break;
			case MyStoreSellItemResults.WrongAmount:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_WrongAmount), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_WrongAmount));
				break;
			case MyStoreSellItemResults.ItemsTimeout:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemsTimeout), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemsTimeout));
				break;
			case MyStoreSellItemResults.NotEnoughAmount:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_NotEnoughAmount), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_NotEnoughAmount));
				break;
			case MyStoreSellItemResults.NotEnoughMoney:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_NotEnoughMoney), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_NotEnoughMoney));
				break;
			case MyStoreSellItemResults.NotEnoughInventorySpace:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreSell_Error_Caption_NotEnoughInventorySpace), MyTexts.Get(MySpaceTexts.StoreSell_Error_Text_NotEnoughInventorySpace));
				break;
			}
			RefreshStoreItems();
			UpdateLocalPlayerAccountBalance();
			SelectedOrderedItemIndex = -1;
		}

		private void ShowMessageBoxError(StringBuilder caption, StringBuilder text)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, caption, null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		private void OnBuy(object obj)
		{
			if (m_selectedOfferItem != null)
			{
				int num = (int)AmountToBuy;
				if (num <= m_selectedOfferItem.Amount && !float.IsNaN(num) && num * m_selectedOfferItem.PricePerUnit <= InventoryTargetViewModel.LocalPlayerAccountInfo.Balance && InventoryTargetViewModel.SelectedInventoryIndex >= 0 && InventoryTargetViewModel.SelectedInventoryIndex < InventoryTargetViewModel.Inventories.Count)
				{
					long entityId = InventoryTargetViewModel.Inventories[InventoryTargetViewModel.SelectedInventoryIndex].EntityId;
					m_storeBlock.CreateBuyRequest(m_selectedOfferItem.Id, num, entityId, m_lastEconomyTick, OnBuyResult);
					IsBuyEnabled = false;
				}
			}
		}

		private void OnBuyResult(MyStoreBuyItemResult result)
		{
			switch (result.Result)
			{
			case MyStoreBuyItemResults.ItemNotFound:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemNotFound), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemNotFound));
				break;
			case MyStoreBuyItemResults.WrongAmount:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongAmount), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_WrongAmount));
				break;
			case MyStoreBuyItemResults.NotEnoughMoney:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughMoney), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughMoney));
				break;
			case MyStoreBuyItemResults.ItemsTimeout:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_ItemsTimeout), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_ItemsTimeout));
				break;
			case MyStoreBuyItemResults.NotEnoughInventorySpace:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughInventorySpace), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughInventorySpace));
				break;
			case MyStoreBuyItemResults.WrongInventory:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongInventory), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_WrongInventory));
				break;
			case MyStoreBuyItemResults.SpawnFailed:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_SpawnFailed), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_SpawnFailed));
				break;
			case MyStoreBuyItemResults.FreePositionNotFound:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_FreePositionNotFound), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_FreePositionNotFound));
				break;
			case MyStoreBuyItemResults.NotEnoughStoreBlockInventorySpace:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughStoreBlockInventorySpace), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughStoreBlockInventorySpace));
				break;
			case MyStoreBuyItemResults.NotEnoughAmount:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughAmount), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughAmount));
				break;
			case MyStoreBuyItemResults.NotEnoughPCU:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughPCU), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughPCU));
				break;
			case MyStoreBuyItemResults.NotEnoughSpaceInTank:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughSpaceInTank), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughSpaceInTank));
				break;
			}
			RefreshStoreItems();
			UpdateLocalPlayerAccountBalance();
			SelectedOfferedItemIndex = -1;
		}

		private void OnSortingDemandedItems(DataGridSortingEventArgs sortingArgs)
		{
			IEnumerable<MyStoreItemModel> collection = SortStoreItems(OrderedItems, sortingArgs);
			OrderedItems = new ObservableCollection<MyStoreItemModel>(collection);
		}

		private void OnSortingOfferedItems(DataGridSortingEventArgs sortingArgs)
		{
			IEnumerable<MyStoreItemModel> collection = SortStoreItems(OfferedItems, sortingArgs);
			OfferedItems = new ObservableCollection<MyStoreItemModel>(collection);
		}

		private IEnumerable<MyStoreItemModel> SortStoreItems(ObservableCollection<MyStoreItemModel> toSort, DataGridSortingEventArgs sortingArgs)
		{
			IEnumerable<MyStoreItemModel> result = null;
			DataGridColumn column = sortingArgs.Column;
			ListSortDirection? sortDirection = column.SortDirection;
			switch (column.SortMemberPath)
			{
			case "Name":
				if (!sortDirection.HasValue || sortDirection == ListSortDirection.Descending)
				{
					result = toSort.OrderBy((MyStoreItemModel u) => u.Name);
					column.SortDirection = ListSortDirection.Ascending;
				}
				else
				{
					result = toSort.OrderByDescending((MyStoreItemModel u) => u.Name);
					column.SortDirection = ListSortDirection.Descending;
				}
				break;
			case "Amount":
				if (!sortDirection.HasValue || sortDirection == ListSortDirection.Descending)
				{
					result = toSort.OrderBy((MyStoreItemModel u) => u.Amount);
					column.SortDirection = ListSortDirection.Ascending;
				}
				else
				{
					result = toSort.OrderByDescending((MyStoreItemModel u) => u.Amount);
					column.SortDirection = ListSortDirection.Descending;
				}
				break;
			case "PricePerUnit":
				if (!sortDirection.HasValue || sortDirection == ListSortDirection.Descending)
				{
					result = toSort.OrderBy((MyStoreItemModel u) => u.PricePerUnit);
					column.SortDirection = ListSortDirection.Ascending;
				}
				else
				{
					result = toSort.OrderByDescending((MyStoreItemModel u) => u.PricePerUnit);
					column.SortDirection = ListSortDirection.Descending;
				}
				break;
			case "TotalPrice":
				if (!sortDirection.HasValue || sortDirection == ListSortDirection.Descending)
				{
					result = toSort.OrderBy((MyStoreItemModel u) => u.TotalPrice);
					column.SortDirection = ListSortDirection.Ascending;
				}
				else
				{
					result = toSort.OrderByDescending((MyStoreItemModel u) => u.TotalPrice);
					column.SortDirection = ListSortDirection.Descending;
				}
				break;
			}
			return result;
		}

		private void AdministrationViewModel_Changed(object sender, EventArgs e)
		{
			RefreshStoreItems();
			UpdateLocalPlayerAccountBalance();
		}

		public override void OnScreenClosing()
		{
			AdministrationViewModel.OfferCreated -= AdministrationViewModel_Changed;
			AdministrationViewModel.OrderCreated -= AdministrationViewModel_Changed;
			AdministrationViewModel.StoreItemRemoved -= AdministrationViewModel_Changed;
			InventoryTargetViewModel.UnsubscribeEvents();
			ServiceManager.Instance.RemoveService<IMyStoreBlockViewModel>();
			base.OnScreenClosing();
		}
	}
}
