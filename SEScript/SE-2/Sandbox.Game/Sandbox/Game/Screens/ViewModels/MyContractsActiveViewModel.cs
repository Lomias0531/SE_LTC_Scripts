using EmptyKeys.UserInterface.Input;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Models;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VRage;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Utils;

namespace Sandbox.Game.Screens.ViewModels
{
	public class MyContractsActiveViewModel : MyViewModelBase
	{
		private int m_selectedActiveContractIndex;

		private int m_activeContractCount;

		private int m_activeContractCountMax;

		private bool m_isAbandonEnabled;

		private ICommand m_refreshActiveCommand;

		private ICommand m_abandonCommand;

		private MyContractModel m_selectedActiveContract;

		private ObservableCollection<MyContractModel> m_activeContracts;

		private bool m_isWaitingForAbandon;

		private bool m_isNoActiveContractVisible;

		public int SelectedActiveContractIndex
		{
			get
			{
				return m_selectedActiveContractIndex;
			}
			set
			{
				SetProperty(ref m_selectedActiveContractIndex, value, "SelectedActiveContractIndex");
				UpdateAbandon();
			}
		}

		private bool IsWaitingForAbandon
		{
			get
			{
				return m_isWaitingForAbandon;
			}
			set
			{
				SetProperty(ref m_isWaitingForAbandon, value, "IsWaitingForAbandon");
				UpdateAbandon();
			}
		}

		public int ActiveContractCount
		{
			get
			{
				return m_activeContractCount;
			}
			set
			{
				SetProperty(ref m_activeContractCount, value, "ActiveContractCount");
			}
		}

		public int ActiveContractCountMax
		{
			get
			{
				return m_activeContractCountMax;
			}
			set
			{
				SetProperty(ref m_activeContractCountMax, value, "ActiveContractCountMax");
			}
		}

		public bool IsAbandonEnabled
		{
			get
			{
				return m_isAbandonEnabled;
			}
			set
			{
				SetProperty(ref m_isAbandonEnabled, value, "IsAbandonEnabled");
			}
		}

		public bool IsNoActiveContractVisible
		{
			get
			{
				return m_isNoActiveContractVisible;
			}
			set
			{
				SetProperty(ref m_isNoActiveContractVisible, value, "IsNoActiveContractVisible");
			}
		}

		public ICommand RefreshActiveCommand
		{
			get
			{
				return m_refreshActiveCommand;
			}
			set
			{
				SetProperty(ref m_refreshActiveCommand, value, "RefreshActiveCommand");
			}
		}

		public ICommand AbandonCommand
		{
			get
			{
				return m_abandonCommand;
			}
			set
			{
				SetProperty(ref m_abandonCommand, value, "AbandonCommand");
			}
		}

		public MyContractModel SelectedActiveContract
		{
			get
			{
				return m_selectedActiveContract;
			}
			set
			{
				SetProperty(ref m_selectedActiveContract, value, "SelectedActiveContract");
			}
		}

		public ObservableCollection<MyContractModel> ActiveContracts
		{
			get
			{
				return m_activeContracts;
			}
			set
			{
				SetProperty(ref m_activeContracts, value, "ActiveContracts");
				IsNoActiveContractVisible = (value == null || value.Count == 0);
			}
		}

		public MyContractsActiveViewModel()
		{
			RefreshActiveCommand = new RelayCommand(OnRefreshActive);
			AbandonCommand = new RelayCommand(OnAbandon);
			MySessionComponentContractSystem component = MySession.Static.GetComponent<MySessionComponentContractSystem>();
			ActiveContractCountMax = component.GetContractLimitPerPlayer();
		}

		public override void InitializeData()
		{
			MyContractBlock.GetActiveContractsStatic(OnGetActiveContracts);
		}

		public void OnRefreshActive(object obj)
		{
			MyContractBlock.GetActiveContractsStatic(OnGetActiveContracts);
		}

		private void OnAbandon(object obj)
		{
			if (SelectedActiveContractIndex >= 0 && SelectedActiveContractIndex < ActiveContracts.Count)
			{
				MyContractModel cont = m_activeContracts[SelectedActiveContractIndex];
				ShowMessageBoxYesNo(MyTexts.Get(MySpaceTexts.Contracts_AbandonConfirmation_Caption), MyTexts.Get(MySpaceTexts.Contracts_AbandonConfirmation_Text), delegate(MyGuiScreenMessageBox.ResultEnum retval)
				{
					if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						IsWaitingForAbandon = true;
						MyContractBlock.AbandonContractStatic(cont.Id, OnAbandonCallback);
					}
				});
			}
		}

		private void SetDefaultActiveContractIndex()
		{
			SelectedActiveContractIndex = ((ActiveContracts == null || ActiveContracts.Count <= 0) ? (-1) : 0);
			if (SelectedActiveContractIndex > -1)
			{
				SelectedActiveContract = ActiveContracts[SelectedActiveContractIndex];
			}
			else
			{
				SelectedActiveContract = null;
			}
		}

		private void OnGetActiveContracts(List<MyObjectBuilder_Contract> contracts)
		{
			ObservableCollection<MyContractModel> observableCollection = new ObservableCollection<MyContractModel>();
			foreach (MyObjectBuilder_Contract contract in contracts)
			{
				MyContractModel item = MyContractModelFactory.CreateInstance(contract);
				observableCollection.Add(item);
			}
			ActiveContracts = observableCollection;
			SetDefaultActiveContractIndex();
			ActiveContractCount = observableCollection.Count;
		}

		private void OnAbandonCallback(MyContractResults result)
		{
			if (result != 0)
			{
				ShowErrorFailNotification(result);
			}
			OnRefreshActive(null);
			IsWaitingForAbandon = false;
		}

		private void ShowErrorFailNotification(MyContractResults state)
		{
			switch (state)
			{
			case MyContractResults.Success:
				MyLog.Default.WriteToLogAndAssert("Why showing error/fail message for success?");
				break;
			case MyContractResults.Fail_CannotAccess:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_NoAccess), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_NoAccess));
				break;
			case MyContractResults.Fail_ContractNotFound_Activation:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_Activation), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_Activation));
				break;
			case MyContractResults.Fail_ContractNotFound_Finish:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_Finish), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_Finish));
				break;
			case MyContractResults.Fail_ContractNotFound_Abandon:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_Abandon), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_Abandon));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationconditionNotMet));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_InsufficientFunds:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_InsufficientFunds), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_InsufficientFunds));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_InsufficientSpace:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_InsufficientSpace), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_InsufficientSpace));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_ContractLimitReachedHard:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_ContractLimitReached), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_ContractLimitReached));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_TargetOffline:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_TargetOffline), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_TargetOffline));
				break;
			case MyContractResults.Fail_ActivationConditionsNotMet_YouAreTargetOfThisHunt:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_ActivationConditionNotMet_YouAreTargetOfThisHunt), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_ActivationConditionNotMet_YouAreTargetOfThisHunt));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishingCondition), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishingCondition));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet_IncorrectTargetEntity:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_IncorrectGrid), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_IncorrectGrid));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet_MissingPackage:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_MissingPackage), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_MissingPackage));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet_NotEnoughItems:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_NotEnoughItems), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_NotEnoughItems));
				break;
			case MyContractResults.Fail_FinishConditionsNotMet_NotEnoughSpace:
				ShowMessageBoxOk(MyTexts.Get(MySpaceTexts.Contracts_Error_Caption_FinishCondition_NotEnoughSpace), MyTexts.Get(MySpaceTexts.Contracts_Error_Text_FinishCondition_NotEnoughSpace));
				break;
			case MyContractResults.Error_Unknown:
			case MyContractResults.Error_MissingKeyStructure:
			case MyContractResults.Error_InvalidData:
				MyLog.Default.WriteToLogAndAssert("Contracts - error result: " + state);
				break;
			default:
				MyLog.Default.WriteToLogAndAssert("Missing case in switch.");
				break;
			}
		}

		private void ShowMessageBoxOk(StringBuilder caption, StringBuilder text)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, caption, null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false, null, useOpacity: false));
		}

		private void ShowMessageBoxYesNo(StringBuilder caption, StringBuilder text, Action<MyGuiScreenMessageBox.ResultEnum> callback)
		{
			MyScreenManager.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, text, caption, null, null, null, null, callback, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: false, null, useOpacity: false));
		}

		private void UpdateAbandon()
		{
			if (IsWaitingForAbandon || SelectedActiveContractIndex < 0 || SelectedActiveContractIndex >= ActiveContracts.Count)
			{
				IsAbandonEnabled = false;
			}
			else
			{
				IsAbandonEnabled = true;
			}
		}
	}
}
