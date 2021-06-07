using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media.Imaging;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyInventoryTargetViewModel : ViewModelBase
	{
		private MyStoreBlock m_storeBlock;

		private ObservableCollection<MyInventoryTargetModel> m_inventories;

		private ObservableCollection<MyInventoryItemModel> m_inventoryItems;

		private int m_selectedInventoryIndex;

		private MyInventoryItemModel m_selectedInventoryItem;

		private string m_localPlayerCurrency;

		private string m_currentInventoryVolume;

		private string m_maxInventoryVolume;

		private MyAccountInfo m_localPlayerAccountInfo;

		private BitmapImage m_currencyIcon;

		private float m_balanceChangeValue;

		private ICommand m_withdrawCommand;

		private ICommand m_depositCommand;

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

		internal MyAccountInfo LocalPlayerAccountInfo => m_localPlayerAccountInfo;

		public int SelectedInventoryIndex
		{
			get
			{
				return m_selectedInventoryIndex;
			}
			set
			{
				SetProperty(ref m_selectedInventoryIndex, value, "SelectedInventoryIndex");
				UpdateVolumeText();
				UpdateInventoryContent();
			}
		}

		public MyInventoryItemModel SelectedInventoryItem
		{
			get
			{
				return m_selectedInventoryItem;
			}
			set
			{
				SetProperty(ref m_selectedInventoryItem, value, "SelectedInventoryItem");
				OnSelectedInventoryItemChanged();
			}
		}

		public ObservableCollection<MyInventoryTargetModel> Inventories
		{
			get
			{
				return m_inventories;
			}
			set
			{
				SetProperty(ref m_inventories, value, "Inventories");
			}
		}

		public ObservableCollection<MyInventoryItemModel> InventoryItems
		{
			get
			{
				return m_inventoryItems;
			}
			set
			{
				SetProperty(ref m_inventoryItems, value, "InventoryItems");
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

		public string MaxInventoryVolume
		{
			get
			{
				return m_maxInventoryVolume;
			}
			set
			{
				SetProperty(ref m_maxInventoryVolume, value, "MaxInventoryVolume");
			}
		}

		public string CurrentInventoryVolume
		{
			get
			{
				return m_currentInventoryVolume;
			}
			set
			{
				SetProperty(ref m_currentInventoryVolume, value, "CurrentInventoryVolume");
			}
		}

		public float BalanceChangeValue
		{
			get
			{
				return m_balanceChangeValue;
			}
			set
			{
				SetProperty(ref m_balanceChangeValue, value, "BalanceChangeValue");
			}
		}

		public ICommand DepositCommand
		{
			get
			{
				return m_depositCommand;
			}
			set
			{
				SetProperty(ref m_depositCommand, value, "DepositCommand");
			}
		}

		public ICommand WithdrawCommand
		{
			get
			{
				return m_withdrawCommand;
			}
			set
			{
				SetProperty(ref m_withdrawCommand, value, "WithdrawCommand");
			}
		}

		public event EventHandler SelectedInventoryItemChanged;

		private void OnSelectedInventoryItemChanged()
		{
			if (this.SelectedInventoryItemChanged != null)
			{
				this.SelectedInventoryItemChanged(this, EventArgs.Empty);
			}
		}

		public MyInventoryTargetViewModel(MyStoreBlock storeBlock)
		{
			BitmapImage bitmapImage = new BitmapImage();
			string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
			bitmapImage.TextureAsset = ((icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty);
			CurrencyIcon = bitmapImage;
			DepositCommand = new RelayCommand(OnDeposit);
			WithdrawCommand = new RelayCommand(OnWithdraw);
			m_storeBlock = storeBlock;
		}

		private void OnWithdraw(object obj)
		{
			if (SelectedInventoryIndex >= 0 && SelectedInventoryIndex < Inventories.Count && !float.IsNaN(BalanceChangeValue))
			{
				long entityId = Inventories[SelectedInventoryIndex].EntityId;
				int num = (int)BalanceChangeValue;
				m_storeBlock.CreateChangeBalanceRequest(-num, entityId, OnWithdrawCompleted);
			}
		}

		private void OnWithdrawCompleted(MyStoreBuyItemResults result)
		{
			ProcessResult(result);
		}

		private void ProcessResult(MyStoreBuyItemResults result)
		{
			switch (result)
			{
			case MyStoreBuyItemResults.WrongAmount:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_WrongAmount), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_WrongAmount));
				break;
			case MyStoreBuyItemResults.NotEnoughMoney:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughMoney), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughMoney));
				break;
			case MyStoreBuyItemResults.NotEnoughInventorySpace:
				ShowMessageBoxError(MyTexts.Get(MySpaceTexts.StoreBuy_Error_Caption_NotEnoughInventorySpace), MyTexts.Get(MySpaceTexts.StoreBuy_Error_Text_NotEnoughInventorySpace));
				break;
			}
			UpdateLocalPlayerCurrency();
		}

		private void ShowMessageBoxError(StringBuilder caption, StringBuilder text)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, caption, null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false));
		}

		private void OnDeposit(object obj)
		{
			if (SelectedInventoryIndex >= 0 && SelectedInventoryIndex < Inventories.Count && !float.IsNaN(BalanceChangeValue))
			{
				long entityId = Inventories[SelectedInventoryIndex].EntityId;
				int amount = (int)BalanceChangeValue;
				m_storeBlock.CreateChangeBalanceRequest(amount, entityId, OnDepositCompleted);
			}
		}

		private void OnDepositCompleted(MyStoreBuyItemResults result)
		{
			ProcessResult(result);
		}

		public void Initialize(List<long> inventoryEntities, bool includeCharacterInventory, bool showAllOption)
		{
			List<MyInventoryTargetModel> list = new List<MyInventoryTargetModel>();
			foreach (long inventoryEntity in inventoryEntities)
			{
				if (MyEntities.TryGetEntityById(inventoryEntity, out MyCubeBlock entity))
				{
					string name = entity.CubeGrid.DisplayName + " - " + entity.DisplayNameText;
					for (int i = 0; i < entity.InventoryCount; i++)
					{
						MyInventory inventory = entity.GetInventory(i);
						MyInventoryTargetModel inventoryModel = new MyInventoryTargetModel(inventory)
						{
							Name = name,
							EntityId = inventoryEntity,
							Volume = (float)inventory.CurrentVolume * 1000f,
							MaxVolume = (float)inventory.MaxVolume * 1000f
						};
						MyGasTank gasTank;
						if ((gasTank = (entity as MyGasTank)) != null)
						{
							inventoryModel.GasTank = gasTank;
							UpdateGasTankVolume(inventoryModel, gasTank);
							gasTank.FilledRatioChanged = delegate
							{
								UpdateGasTankVolume(inventoryModel, gasTank);
								UpdateVolumeText();
							};
						}
						MyCubeBlockDefinition blockDefinition = entity.BlockDefinition;
						if (blockDefinition != null && blockDefinition.Icons?.Length > 0)
						{
							inventoryModel.Icon = new BitmapImage
							{
								TextureAsset = entity.BlockDefinition.Icons[0]
							};
						}
						list.Add(inventoryModel);
						inventory.InventoryContentChanged += OnInventoryContentChanged;
						inventory.ContentsChanged += Inventory_ContentsChanged;
					}
				}
			}
			list.SortNoAlloc((MyInventoryTargetModel x, MyInventoryTargetModel y) => string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase));
			if (showAllOption)
			{
				list.Insert(0, new MyInventoryTargetModel(null)
				{
					Name = MyTexts.GetString(MySpaceTexts.InventorySelection_All),
					AllInventories = true
				});
			}
			if (includeCharacterInventory)
			{
				MyInventory inventory2 = MySession.Static.LocalCharacter.GetInventory();
				if (inventory2 != null)
				{
					list.Insert(0, new MyInventoryTargetModel(inventory2)
					{
						Name = MyTexts.GetString(MySpaceTexts.InventorySelection_Character),
						EntityId = MySession.Static.LocalCharacterEntityId,
						Volume = (float)inventory2.CurrentVolume * 1000f,
						MaxVolume = (float)inventory2.MaxVolume * 1000f
					});
					inventory2.InventoryContentChanged += OnInventoryContentChanged;
					inventory2.ContentsChanged += Inventory_ContentsChanged;
				}
			}
			UnsubscribeEvents();
			Inventories = new ObservableCollection<MyInventoryTargetModel>(list);
			if (Inventories.Count > 0)
			{
				SelectedInventoryIndex = 0;
			}
			UpdateLocalPlayerCurrency();
		}

		private void UpdateGasTankVolume(MyInventoryTargetModel inventoryModel, MyGasTank gasTank)
		{
			inventoryModel.Volume = (float)Math.Round(gasTank.FilledRatio * (double)gasTank.Capacity, 0);
			inventoryModel.MaxVolume = gasTank.Capacity;
		}

		private void Inventory_ContentsChanged(MyInventoryBase inventory)
		{
			if (SelectedInventoryIndex >= 0 && SelectedInventoryIndex < Inventories.Count)
			{
				MyInventoryTargetModel myInventoryTargetModel = Inventories[SelectedInventoryIndex];
				if (myInventoryTargetModel.Inventory == inventory)
				{
					myInventoryTargetModel.Volume = (float)inventory.CurrentVolume * 1000f;
					UpdateVolumeText();
				}
			}
		}

		private void OnInventoryContentChanged(MyInventoryBase inventory, MyPhysicalInventoryItem item, MyFixedPoint amount)
		{
			if (SelectedInventoryIndex >= 0 && SelectedInventoryIndex < Inventories.Count && Inventories[SelectedInventoryIndex].Inventory == inventory)
			{
				MyInventoryItemModel myInventoryItemModel = null;
				bool flag = true;
				foreach (MyInventoryItemModel inventoryItem in InventoryItems)
				{
					if (inventoryItem.Inventory == inventory && item.ItemId == inventoryItem.InventoryItem.ItemId)
					{
						if (item.Amount == 0)
						{
							myInventoryItemModel = inventoryItem;
						}
						else
						{
							inventoryItem.Amount += (float)amount;
						}
						flag = false;
						break;
					}
				}
				if (flag)
				{
					MyInventoryItemModel item2 = new MyInventoryItemModel(item, inventory);
					InventoryItems.Add(item2);
				}
				if (myInventoryItemModel != null)
				{
					InventoryItems.Remove(myInventoryItemModel);
				}
			}
		}

		internal void UnsubscribeEvents()
		{
			ObservableCollection<MyInventoryTargetModel> inventories = Inventories;
			if (inventories != null && inventories.Count > 0)
			{
				foreach (MyInventoryTargetModel inventory in Inventories)
				{
					if (inventory.Inventory != null)
					{
						inventory.Inventory.InventoryContentChanged -= OnInventoryContentChanged;
						inventory.Inventory.ContentsChanged -= Inventory_ContentsChanged;
						if (inventory.GasTank != null)
						{
							inventory.GasTank.FilledRatioChanged = null;
						}
					}
				}
			}
		}

		private void UpdateVolumeText()
		{
			if (SelectedInventoryIndex >= 0 && SelectedInventoryIndex < Inventories.Count)
			{
				MyInventoryTargetModel myInventoryTargetModel = Inventories[SelectedInventoryIndex];
				string @string = MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_VolumeValue);
				MyInventory myInventory;
				if (MySession.Static.CreativeMode && (myInventory = (myInventoryTargetModel.Inventory as MyInventory)) != null && myInventory.IsCharacterOwner)
				{
					CurrentInventoryVolume = string.Format(@string, MyValueFormatter.GetFormatedFloat(myInventoryTargetModel.Volume, 2, ","));
					MaxInventoryVolume = MyTexts.GetString(MySpaceTexts.ScreenTerminalInventory_UnlimitedVolume);
				}
				else
				{
					CurrentInventoryVolume = string.Format(@string, MyValueFormatter.GetFormatedFloat(myInventoryTargetModel.Volume, 2, ","));
					MaxInventoryVolume = string.Format(@string, MyValueFormatter.GetFormatedFloat(myInventoryTargetModel.MaxVolume, 2, ","));
				}
			}
		}

		public void UpdateInventoryContent()
		{
			if (SelectedInventoryIndex >= 0 && SelectedInventoryIndex < Inventories.Count)
			{
				MyInventoryTargetModel myInventoryTargetModel = Inventories[SelectedInventoryIndex];
				List<MyInventoryItemModel> list = new List<MyInventoryItemModel>();
				if (myInventoryTargetModel.AllInventories)
				{
					foreach (MyInventoryTargetModel inventory in Inventories)
					{
						if (!inventory.AllInventories)
						{
							long entityId = inventory.EntityId;
							AddInventoryItems(inventory.Inventory, entityId, list);
						}
					}
				}
				else
				{
					long entityId2 = myInventoryTargetModel.EntityId;
					AddInventoryItems(myInventoryTargetModel.Inventory, entityId2, list);
				}
				InventoryItems = new ObservableCollection<MyInventoryItemModel>(list);
				UpdateLocalPlayerCurrency();
			}
		}

		internal void UpdateLocalPlayerCurrency()
		{
			MyBankingSystem.Static.TryGetAccountInfo(MySession.Static.LocalPlayerId, out m_localPlayerAccountInfo);
			LocalPlayerCurrency = MyBankingSystem.GetFormatedValue(m_localPlayerAccountInfo.Balance);
		}

		private static void AddInventoryItems(MyInventoryBase inventory, long entityId, List<MyInventoryItemModel> invItems)
		{
			MyEntity entity = null;
			if (MyEntities.TryGetEntityById(entityId, out entity))
			{
				foreach (MyPhysicalInventoryItem item2 in inventory.GetItems())
				{
					MyInventoryItemModel item = new MyInventoryItemModel(item2, inventory);
					invItems.Add(item);
				}
			}
		}

		internal float GetItemAmount(MyDefinitionId itemDefinitionId)
		{
			float num = 0f;
			foreach (MyInventoryItemModel inventoryItem in InventoryItems)
			{
				if (inventoryItem.InventoryItem.Content.GetId() == itemDefinitionId)
				{
					num += inventoryItem.Amount;
				}
			}
			return num;
		}
	}
}
