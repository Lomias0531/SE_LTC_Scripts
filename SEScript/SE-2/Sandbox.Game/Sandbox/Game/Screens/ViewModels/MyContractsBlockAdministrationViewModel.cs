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
using VRage.Collections;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components.Contracts;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyContractsBlockAdministrationViewModel : ViewModelBase
	{
		public enum MyAdminSelectionDialogTypes
		{
			None,
			DeliverBlock,
			ObtainAndDeliverBlock,
			FindGrid,
			Repair
		}

		private MyContractBlock m_contractBlock;

		private int m_createdContractCount;

		private int m_createdContractCountMax;

		private MyContractModel m_selectedAdministrableContract;

		private int m_selectedContractTypeIndex;

		private float m_newContractCurrencyReward;

		private float m_newContractStartDeposit;

		private float m_newContractDurationInMin;

		private string m_newContracSelectionName;

		private long m_newContracSelectionId;

		private MyDeliverItemModel m_newContractObtainAndDeliverSelectedItemType;

		private float m_newContractObtainAndDeliverItemAmount;

		private float m_newContractFindSearchRadius;

		private bool m_isVisibleAdminSelection;

		private string m_adminSelectionCaption;

		private string m_adminSelectionText;

		private int m_adminSelectedItemIndex;

		private bool m_isNoAdministrableContractVisible;

		private long m_currentMoney;

		private bool m_isDeleteEnabled;

		private BitmapImage m_currencyIcon;

		private MyAdminSelectionDialogTypes m_selectionDialogType;

		private ICommand m_newContractDeliverBlockSelectCommand;

		private ICommand m_newContractObtainAndDeliverBlockSelectCommand;

		private ICommand m_newContractFindGridSelectCommand;

		private ICommand m_newContractRepairGridSelectCommand;

		private ICommand m_adminSelectionConfirmCommand;

		private ICommand m_adminSelectionExitCommand;

		private ICommand m_deleteCommand;

		private ICommand m_refreshCommand;

		private ICommand m_createCommand;

		private System.Collections.ObjectModel.ObservableCollection<MyContractModel> m_administrableContracts;

		private System.Collections.ObjectModel.ObservableCollection<MyContractTypeModel> m_contractTypes;

		private System.Collections.ObjectModel.ObservableCollection<MyDeliverItemModel> m_deliverableItems;

		private System.Collections.ObjectModel.ObservableCollection<MyAdminSelectionItemModel> m_adminSelectionItems;

		public Action OnNewContractCreated;

		public MyContractModel SelectedAdministrableContract
		{
			get
			{
				return m_selectedAdministrableContract;
			}
			set
			{
				SetProperty(ref m_selectedAdministrableContract, value, "SelectedAdministrableContract");
				UpdateDelete();
			}
		}

		public int SelectedContractTypeIndex
		{
			get
			{
				return m_selectedContractTypeIndex;
			}
			set
			{
				SetProperty(ref m_selectedContractTypeIndex, value, "SelectedContractTypeIndex");
				switch (value)
				{
				case 0:
					SelectionDialogType = MyAdminSelectionDialogTypes.DeliverBlock;
					break;
				case 1:
					SelectionDialogType = MyAdminSelectionDialogTypes.ObtainAndDeliverBlock;
					break;
				case 2:
					SelectionDialogType = MyAdminSelectionDialogTypes.FindGrid;
					break;
				case 3:
					SelectionDialogType = MyAdminSelectionDialogTypes.Repair;
					break;
				default:
					SelectionDialogType = MyAdminSelectionDialogTypes.None;
					break;
				}
				RaisePropertyChanged("IsContractSelected_Deliver");
				RaisePropertyChanged("IsContractSelected_ObtainAndDeliver");
				RaisePropertyChanged("IsContractSelected_Find");
				RaisePropertyChanged("IsContractSelected_Repair");
			}
		}

		public bool IsContractSelected_Deliver => SelectedContractTypeIndex == 0;

		public bool IsContractSelected_ObtainAndDeliver => SelectedContractTypeIndex == 1;

		public bool IsContractSelected_Find => SelectedContractTypeIndex == 2;

		public bool IsContractSelected_Repair => SelectedContractTypeIndex == 3;

		public int CreatedContractCount
		{
			get
			{
				return m_createdContractCount;
			}
			set
			{
				SetProperty(ref m_createdContractCount, value, "CreatedContractCount");
			}
		}

		public int CreatedContractCountMax
		{
			get
			{
				return m_createdContractCountMax;
			}
			set
			{
				SetProperty(ref m_createdContractCountMax, value, "CreatedContractCountMax");
			}
		}

		public float NewContractCurrencyReward
		{
			get
			{
				return m_newContractCurrencyReward;
			}
			set
			{
				SetProperty(ref m_newContractCurrencyReward, value, "NewContractCurrencyReward");
			}
		}

		public float NewContractStartDeposit
		{
			get
			{
				return m_newContractStartDeposit;
			}
			set
			{
				SetProperty(ref m_newContractStartDeposit, value, "NewContractStartDeposit");
			}
		}

		public float NewContractDurationInMin
		{
			get
			{
				return m_newContractDurationInMin;
			}
			set
			{
				SetProperty(ref m_newContractDurationInMin, value, "NewContractDurationInMin");
			}
		}

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

		public string NewContractSelectionName
		{
			get
			{
				return m_newContracSelectionName;
			}
			set
			{
				SetProperty(ref m_newContracSelectionName, value, "NewContractSelectionName");
			}
		}

		public long NewContractSelectionId
		{
			get
			{
				return m_newContracSelectionId;
			}
			set
			{
				SetProperty(ref m_newContracSelectionId, value, "NewContractSelectionId");
			}
		}

		public MyDeliverItemModel NewContractObtainAndDeliverSelectedItemType
		{
			get
			{
				return m_newContractObtainAndDeliverSelectedItemType;
			}
			set
			{
				SetProperty(ref m_newContractObtainAndDeliverSelectedItemType, value, "NewContractObtainAndDeliverSelectedItemType");
			}
		}

		public float NewContractObtainAndDeliverItemAmount
		{
			get
			{
				return m_newContractObtainAndDeliverItemAmount;
			}
			set
			{
				SetProperty(ref m_newContractObtainAndDeliverItemAmount, value, "NewContractObtainAndDeliverItemAmount");
			}
		}

		public float NewContractFindSearchRadius
		{
			get
			{
				return m_newContractFindSearchRadius;
			}
			set
			{
				SetProperty(ref m_newContractFindSearchRadius, value, "NewContractFindSearchRadius");
			}
		}

		public bool IsVisibleAdminSelection
		{
			get
			{
				return m_isVisibleAdminSelection;
			}
			set
			{
				SetProperty(ref m_isVisibleAdminSelection, value, "IsVisibleAdminSelection");
			}
		}

		public string AdminSelectionCaption
		{
			get
			{
				return m_adminSelectionCaption;
			}
			set
			{
				SetProperty(ref m_adminSelectionCaption, value, "AdminSelectionCaption");
			}
		}

		public string AdminSelectionText
		{
			get
			{
				return m_adminSelectionText;
			}
			set
			{
				SetProperty(ref m_adminSelectionText, value, "AdminSelectionText");
			}
		}

		public int AdminSelectedItemIndex
		{
			get
			{
				return m_adminSelectedItemIndex;
			}
			set
			{
				SetProperty(ref m_adminSelectedItemIndex, value, "AdminSelectedItemIndex");
			}
		}

		public long CurrentMoney
		{
			get
			{
				return m_currentMoney;
			}
			set
			{
				SetProperty(ref m_currentMoney, value, "CurrentMoney");
				RaisePropertyChanged("CurrentMoneyFormated");
			}
		}

		public string CurrentMoneyFormated => MyBankingSystem.GetFormatedValue(CurrentMoney);

		public bool IsDeleteEnabled
		{
			get
			{
				return m_isDeleteEnabled;
			}
			set
			{
				SetProperty(ref m_isDeleteEnabled, value, "IsDeleteEnabled");
			}
		}

		public bool IsOwner
		{
			get;
			private set;
		}

		public bool IsNoAdministrableContractVisible
		{
			get
			{
				return m_isNoAdministrableContractVisible;
			}
			set
			{
				SetProperty(ref m_isNoAdministrableContractVisible, value, "IsNoAdministrableContractVisible");
			}
		}

		public MyAdminSelectionDialogTypes SelectionDialogType
		{
			get
			{
				return m_selectionDialogType;
			}
			set
			{
				SetProperty(ref m_selectionDialogType, value, "SelectionDialogType");
				IsVisibleAdminSelection = false;
				switch (value)
				{
				case MyAdminSelectionDialogTypes.DeliverBlock:
					AdminSelectionCaption = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionCaption_DeliverBlock);
					AdminSelectionText = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionText_DeliverBlock);
					break;
				case MyAdminSelectionDialogTypes.ObtainAndDeliverBlock:
					AdminSelectionCaption = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionCaption_ObtainAndDeliverBlock);
					AdminSelectionText = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionText_ObtainAndDeliverBlock);
					break;
				case MyAdminSelectionDialogTypes.FindGrid:
					AdminSelectionCaption = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionCaption_FindGrid);
					AdminSelectionText = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionText_FindGrid);
					break;
				case MyAdminSelectionDialogTypes.Repair:
					AdminSelectionCaption = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionCaption_Repair);
					AdminSelectionText = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_SelectionText_Repair);
					break;
				default:
					AdminSelectionCaption = string.Empty;
					AdminSelectionText = string.Empty;
					break;
				}
				AdminSelectedItemIndex = -1;
				AdminSelectionItems = new System.Collections.ObjectModel.ObservableCollection<MyAdminSelectionItemModel>();
				ResetSelection();
			}
		}

		public ICommand NewContractDeliverBlockSelectCommand
		{
			get
			{
				return m_newContractDeliverBlockSelectCommand;
			}
			set
			{
				SetProperty(ref m_newContractDeliverBlockSelectCommand, value, "NewContractDeliverBlockSelectCommand");
			}
		}

		public ICommand NewContractObtainAndDeliverBlockSelectCommand
		{
			get
			{
				return m_newContractObtainAndDeliverBlockSelectCommand;
			}
			set
			{
				SetProperty(ref m_newContractObtainAndDeliverBlockSelectCommand, value, "NewContractObtainAndDeliverBlockSelectCommand");
			}
		}

		public ICommand NewContractFindGridSelectCommand
		{
			get
			{
				return m_newContractFindGridSelectCommand;
			}
			set
			{
				SetProperty(ref m_newContractFindGridSelectCommand, value, "NewContractFindGridSelectCommand");
			}
		}

		public ICommand NewContractRepairGridSelectCommand
		{
			get
			{
				return m_newContractRepairGridSelectCommand;
			}
			set
			{
				SetProperty(ref m_newContractRepairGridSelectCommand, value, "NewContractRepairGridSelectCommand");
			}
		}

		public ICommand AdminSelectionConfirmCommand
		{
			get
			{
				return m_adminSelectionConfirmCommand;
			}
			set
			{
				SetProperty(ref m_adminSelectionConfirmCommand, value, "AdminSelectionConfirmCommand");
			}
		}

		public ICommand AdminSelectionExitCommand
		{
			get
			{
				return m_adminSelectionExitCommand;
			}
			set
			{
				SetProperty(ref m_adminSelectionExitCommand, value, "AdminSelectionExitCommand");
			}
		}

		public ICommand DeleteCommand
		{
			get
			{
				return m_deleteCommand;
			}
			set
			{
				SetProperty(ref m_deleteCommand, value, "DeleteCommand");
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

		public ICommand CreateCommand
		{
			get
			{
				return m_createCommand;
			}
			set
			{
				SetProperty(ref m_createCommand, value, "CreateCommand");
			}
		}

		public System.Collections.ObjectModel.ObservableCollection<MyContractModel> AdministrableContracts
		{
			get
			{
				return m_administrableContracts;
			}
			set
			{
				SetProperty(ref m_administrableContracts, value, "AdministrableContracts");
				IsNoAdministrableContractVisible = (value == null || value.Count == 0);
			}
		}

		public System.Collections.ObjectModel.ObservableCollection<MyContractTypeModel> ContractTypes
		{
			get
			{
				return m_contractTypes;
			}
			set
			{
				SetProperty(ref m_contractTypes, value, "ContractTypes");
				if (value == null || value.Count <= 0)
				{
					SelectedContractTypeIndex = -1;
				}
				else
				{
					SelectedContractTypeIndex = 0;
				}
			}
		}

		public System.Collections.ObjectModel.ObservableCollection<MyDeliverItemModel> DeliverableItems
		{
			get
			{
				return m_deliverableItems;
			}
			set
			{
				SetProperty(ref m_deliverableItems, value, "DeliverableItems");
			}
		}

		public System.Collections.ObjectModel.ObservableCollection<MyAdminSelectionItemModel> AdminSelectionItems
		{
			get
			{
				return m_adminSelectionItems;
			}
			set
			{
				SetProperty(ref m_adminSelectionItems, value, "AdminSelectionItems");
			}
		}

		public MyContractsBlockAdministrationViewModel(MyContractBlock contractBlock)
		{
			m_contractBlock = contractBlock;
			Initialize();
			BitmapImage bitmapImage = new BitmapImage();
			string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
			bitmapImage.TextureAsset = ((icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty);
			CurrencyIcon = bitmapImage;
			NewContractDeliverBlockSelectCommand = new RelayCommand(OnDeliverBlockSelection);
			NewContractObtainAndDeliverBlockSelectCommand = new RelayCommand(OnObtainandDeliverBlockSelection);
			NewContractFindGridSelectCommand = new RelayCommand(OnFindGridSelection);
			NewContractRepairGridSelectCommand = new RelayCommand(OnRepairGridSelection);
			AdminSelectionConfirmCommand = new RelayCommand(OnAdminSelectionConfirm);
			AdminSelectionExitCommand = new RelayCommand(OnAdminSelectionExit);
			DeleteCommand = new RelayCommand(OnDelete);
			RefreshCommand = new RelayCommand(OnRefresh);
			CreateCommand = new RelayCommand(OnCreate);
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			CreatedContractCountMax = component.GetContractCreationLimitPerPlayer();
			SelectionDialogType = MyAdminSelectionDialogTypes.DeliverBlock;
			IsVisibleAdminSelection = false;
			ResetSelection();
			UpdateMoney();
		}

		internal void InitializeData()
		{
			if (IsOwner)
			{
				m_contractBlock.GetAdministrableContracts(OnGetAdministrableContracts);
			}
		}

		public void Initialize()
		{
			IsOwner = (m_contractBlock != null && m_contractBlock.OwnerId == MySession.Static.LocalPlayerId);
			System.Collections.ObjectModel.ObservableCollection<MyContractTypeModel> observableCollection = new System.Collections.ObjectModel.ObservableCollection<MyContractTypeModel>();
			observableCollection.Add(new MyContractTypeModel
			{
				Name = MyTexts.GetString(MySpaceTexts.ContractTypeNames_Deliver)
			});
			observableCollection.Add(new MyContractTypeModel
			{
				Name = MyTexts.GetString(MySpaceTexts.ContractTypeNames_ObtainAndDeliver)
			});
			observableCollection.Add(new MyContractTypeModel
			{
				Name = MyTexts.GetString(MySpaceTexts.ContractTypeNames_Find)
			});
			observableCollection.Add(new MyContractTypeModel
			{
				Name = MyTexts.GetString(MySpaceTexts.ContractTypeNames_Repair)
			});
			ContractTypes = observableCollection;
			ListReader<MyPhysicalItemDefinition> physicalItemDefinitions = MyDefinitionManager.Static.GetPhysicalItemDefinitions();
			List<MyDeliverItemModel> list = new List<MyDeliverItemModel>();
			foreach (MyPhysicalItemDefinition item in physicalItemDefinitions)
			{
				if (item != null && item.Public && item.Enabled)
				{
					list.Add(new MyDeliverItemModel(item));
				}
			}
			list.SortNoAlloc((MyDeliverItemModel x, MyDeliverItemModel y) => string.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase));
			DeliverableItems = new System.Collections.ObjectModel.ObservableCollection<MyDeliverItemModel>(list);
		}

		private void OnDeliverBlockSelection(object obj)
		{
			DisplaySelectionScreen(MyAdminSelectionDialogTypes.DeliverBlock);
		}

		private void OnObtainandDeliverBlockSelection(object obj)
		{
			DisplaySelectionScreen(MyAdminSelectionDialogTypes.ObtainAndDeliverBlock);
		}

		private void OnFindGridSelection(object obj)
		{
			DisplaySelectionScreen(MyAdminSelectionDialogTypes.FindGrid);
		}

		private void OnRepairGridSelection(object obj)
		{
			DisplaySelectionScreen(MyAdminSelectionDialogTypes.Repair);
		}

		private void OnAdminSelectionConfirm(object obj)
		{
			IsVisibleAdminSelection = false;
			if (AdminSelectionItems == null || AdminSelectedItemIndex < 0 || AdminSelectedItemIndex >= AdminSelectionItems.Count)
			{
				ResetSelection();
				return;
			}
			NewContractSelectionName = AdminSelectionItems[AdminSelectedItemIndex].NameCombinedShort;
			NewContractSelectionId = AdminSelectionItems[AdminSelectedItemIndex].Id;
		}

		private void ResetSelection()
		{
			NewContractSelectionName = MyTexts.GetString(MySpaceTexts.ContractScreen_Administration_NoSelection);
			NewContractSelectionId = -1L;
		}

		private void OnAdminSelectionExit(object obj)
		{
			IsVisibleAdminSelection = false;
			ResetSelection();
		}

		private void OnDelete(object obj)
		{
			if (SelectedAdministrableContract != null)
			{
				m_contractBlock.DeleteCustomContract(SelectedAdministrableContract.Id, OnDeleteCustomContractCallback);
			}
		}

		private void OnRefresh(object obj)
		{
			OnRefreshAdministrable(null);
		}

		private void OnCreate(object obj)
		{
			if (CreatedContractCount >= CreatedContractCountMax)
			{
				MyContractCreationResults result = MyContractCreationResults.Fail_CreationLimitHard;
				OnCreateContractCallback(result);
				return;
			}
			if (float.IsNaN(NewContractCurrencyReward))
			{
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_MoneyReward), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_MoneyReward));
				return;
			}
			if (float.IsNaN(NewContractStartDeposit))
			{
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_StartingDeposit), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_StartingDeposit));
				return;
			}
			if (float.IsNaN(NewContractDurationInMin))
			{
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_Duration), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_Duration));
				return;
			}
			int rewardMoney = (int)NewContractCurrencyReward;
			int startingDeposit = (int)NewContractStartDeposit;
			int durationInMin = (int)NewContractDurationInMin;
			switch (SelectionDialogType)
			{
			case MyAdminSelectionDialogTypes.None:
				break;
			case MyAdminSelectionDialogTypes.DeliverBlock:
				if (AdminSelectionItems != null && AdminSelectedItemIndex >= 0 && AdminSelectedItemIndex < AdminSelectionItems.Count)
				{
					long id5 = AdminSelectionItems[AdminSelectedItemIndex].Id;
					m_contractBlock.CreateCustomContractDeliver(rewardMoney, startingDeposit, durationInMin, id5, OnCreateContractCallback);
				}
				else
				{
					DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_TargetContractBlockNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_TargetContractBlockNotSelected));
				}
				break;
			case MyAdminSelectionDialogTypes.ObtainAndDeliverBlock:
				if (AdminSelectionItems != null && AdminSelectedItemIndex >= 0 && AdminSelectedItemIndex < AdminSelectionItems.Count)
				{
					long id3 = AdminSelectionItems[AdminSelectedItemIndex].Id;
					if (NewContractObtainAndDeliverSelectedItemType != null)
					{
						MyDefinitionId id4 = NewContractObtainAndDeliverSelectedItemType.ItemDefinition.Id;
						if (float.IsNaN(NewContractObtainAndDeliverItemAmount))
						{
							DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_ItemAmount), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_ItemAmount));
							break;
						}
						int itemAmount = (int)NewContractObtainAndDeliverItemAmount;
						m_contractBlock.CreateCustomContractObtainAndDeliver(rewardMoney, startingDeposit, durationInMin, id3, id4, itemAmount, OnCreateContractCallback);
					}
					else
					{
						DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_ItemTypeNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_ItemTypeNotSelected));
					}
				}
				else
				{
					DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_TargetContractBlockNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_TargetContractBlockNotSelected));
				}
				break;
			case MyAdminSelectionDialogTypes.FindGrid:
				if (AdminSelectionItems != null && AdminSelectedItemIndex >= 0 && AdminSelectedItemIndex < AdminSelectionItems.Count)
				{
					long id2 = AdminSelectionItems[AdminSelectedItemIndex].Id;
					if (float.IsNaN(NewContractFindSearchRadius))
					{
						DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_IsNaN_SearchRadius), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_IsNaN_SearchRadius));
						break;
					}
					double searchRadius = NewContractFindSearchRadius;
					m_contractBlock.CreateCustomContractFind(rewardMoney, startingDeposit, durationInMin, id2, searchRadius, OnCreateContractCallback);
				}
				else
				{
					DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_TargetGridNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_TargetGridNotSelected));
				}
				break;
			case MyAdminSelectionDialogTypes.Repair:
				if (AdminSelectionItems != null && AdminSelectedItemIndex >= 0 && AdminSelectedItemIndex < AdminSelectionItems.Count)
				{
					long id = AdminSelectionItems[AdminSelectedItemIndex].Id;
					m_contractBlock.CreateCustomContractRepair(rewardMoney, startingDeposit, durationInMin, id, OnCreateContractCallback);
				}
				else
				{
					DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailCaption_TargetGridNotSelected), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_FailText_TargetGridNotSelected));
				}
				break;
			}
		}

		private void UpdateDelete()
		{
			if (m_selectedAdministrableContract == null)
			{
				IsDeleteEnabled = false;
			}
			else
			{
				IsDeleteEnabled = true;
			}
		}

		private void UpdateMoney()
		{
			MyBankingSystem component = MySession.Static.GetComponent<MyBankingSystem>();
			if (component != null && component.TryGetAccountInfo(MySession.Static.LocalPlayerId, out MyAccountInfo account))
			{
				CurrentMoney = account.Balance;
			}
		}

		public void OnCreateContractCallback(MyContractCreationResults result)
		{
			switch (result)
			{
			case MyContractCreationResults.Success:
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_Success), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_Success));
				break;
			case MyContractCreationResults.Fail_Common:
			case MyContractCreationResults.Fail_Impossible:
			case MyContractCreationResults.Fail_NoAccess:
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_Fail), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_Fail));
				break;
			case MyContractCreationResults.Fail_GridNotFound:
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_GridNotFound), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_GridNotFound));
				break;
			case MyContractCreationResults.Fail_BlockNotFound:
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_BlockNotFound), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_BlockNotFound));
				break;
			case MyContractCreationResults.Fail_NotAnOwnerOfGrid:
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_NotAnOwnerOfGrid), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_NotAnOwnerOfGrid));
				break;
			case MyContractCreationResults.Fail_NotAnOwnerOfBlock:
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_NotAnOwnerOfBlock), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_NotAnOwnerOfBlock));
				break;
			case MyContractCreationResults.Fail_NotEnoughFunds:
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_NotEnoughFunds), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_NotEnoughFunds));
				break;
			case MyContractCreationResults.Fail_CreationLimitHard:
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_CreationLimitHard), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_CreationLimitHard));
				break;
			case MyContractCreationResults.Error:
			case MyContractCreationResults.Error_MissingKeyStructure:
				DisplayMessageBoxOk(MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultCaption_Error), MyTexts.Get(MySpaceTexts.ContractScreen_Aministration_CreatinResultText_Error));
				break;
			}
			if (result == MyContractCreationResults.Success)
			{
				if (OnNewContractCreated != null)
				{
					OnNewContractCreated();
				}
				OnRefreshAdministrable(null);
				UpdateMoney();
			}
		}

		public void OnDeleteCustomContractCallback(bool success)
		{
			OnRefreshAdministrable(null);
			UpdateMoney();
		}

		public void OnGetAdministrableContracts(List<MyObjectBuilder_Contract> contracts)
		{
			System.Collections.ObjectModel.ObservableCollection<MyContractModel> observableCollection = new System.Collections.ObjectModel.ObservableCollection<MyContractModel>();
			foreach (MyObjectBuilder_Contract contract in contracts)
			{
				MyContractModel item = MyContractModelFactory.CreateInstance(contract);
				observableCollection.Add(item);
			}
			AdministrableContracts = observableCollection;
			CreatedContractCount = observableCollection.Count;
			SetDefaultAdministrableContractIndex();
		}

		private void SetDefaultAdministrableContractIndex()
		{
			if (AdministrableContracts.Count > 0)
			{
				SelectedAdministrableContract = AdministrableContracts[0];
			}
			else
			{
				SelectedAdministrableContract = null;
			}
		}

		public void OnRefreshAdministrable(object obj)
		{
			if (IsOwner)
			{
				m_contractBlock.GetAdministrableContracts(OnGetAdministrableContracts);
			}
		}

		private void DisplaySelectionScreen(MyAdminSelectionDialogTypes type)
		{
			SelectionDialogType = type;
			PopulateSelectionScreen(type);
			IsVisibleAdminSelection = true;
		}

		private void PopulateSelectionScreen(MyAdminSelectionDialogTypes type)
		{
			switch (type)
			{
			case MyAdminSelectionDialogTypes.None:
				break;
			case MyAdminSelectionDialogTypes.DeliverBlock:
			case MyAdminSelectionDialogTypes.ObtainAndDeliverBlock:
				m_contractBlock.GetAllOwnedContractBlocks(MySession.Static.LocalPlayerId, OnGetAllOwnedContractBlocks);
				break;
			case MyAdminSelectionDialogTypes.FindGrid:
			case MyAdminSelectionDialogTypes.Repair:
				m_contractBlock.GetAllOwnedGrids(MySession.Static.LocalPlayerId, OnGetAllOwnedGrids);
				break;
			}
		}

		public void OnGetAllOwnedContractBlocks(List<MyContractBlock.MyEntityInfoWrapper> data)
		{
			System.Collections.ObjectModel.ObservableCollection<MyAdminSelectionItemModel> observableCollection = new System.Collections.ObjectModel.ObservableCollection<MyAdminSelectionItemModel>();
			foreach (MyContractBlock.MyEntityInfoWrapper datum in data)
			{
				observableCollection.Add(new MyAdminSelectionItemModel(datum.NamePrefix, datum.NameSuffix, datum.Id));
			}
			AdminSelectionItems = observableCollection;
			if (observableCollection.Count > 0)
			{
				AdminSelectedItemIndex = 0;
			}
		}

		public void OnGetAllOwnedGrids(List<MyContractBlock.MyEntityInfoWrapper> data)
		{
			System.Collections.ObjectModel.ObservableCollection<MyAdminSelectionItemModel> observableCollection = new System.Collections.ObjectModel.ObservableCollection<MyAdminSelectionItemModel>();
			foreach (MyContractBlock.MyEntityInfoWrapper datum in data)
			{
				observableCollection.Add(new MyAdminSelectionItemModel(datum.NamePrefix, datum.NameSuffix, datum.Id));
			}
			AdminSelectionItems = observableCollection;
			if (observableCollection.Count > 0)
			{
				AdminSelectedItemIndex = 0;
			}
		}

		private void DisplayMessageBoxOk(StringBuilder caption, StringBuilder text)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, caption, null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false, null, useOpacity: false));
		}

		private void ShowMessageBoxYesNo(StringBuilder caption, StringBuilder text, Action<MyGuiScreenMessageBox.ResultEnum> callback)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, text, caption, null, null, null, null, callback, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false, null, useOpacity: false));
		}
	}
}
