using Sandbox.Engine.Multiplayer;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.GUI;
using Sandbox.Game.Gui.FactionTerminal;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Library.Utils;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	internal class MyTerminalFactionController : MyTerminalController
	{
		internal enum MyMemberComparerEnum
		{
			Founder,
			Leader,
			Member,
			Applicant
		}

		internal enum MyFactionFilter : byte
		{
			None,
			Player,
			Friend,
			Neutral,
			Enemy,
			NPC,
			Unknown,
			PlayersFactions,
			Discovered
		}

		private struct AccountBalanceAnimationInfo
		{
			internal long OldBalance;

			internal long NewBalance;
		}

		protected sealed class NewNpcClickedInternal_003C_003ESystem_Int64_0023System_String : ICallSite<IMyEventOwner, long, string, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long factionId, in string npcName, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				NewNpcClickedInternal(factionId, npcName);
			}
		}

		internal static readonly Color COLOR_CUSTOM_RED = new Color(228, 62, 62);

		internal static readonly Color COLOR_CUSTOM_GREEN = new Color(101, 178, 91);

		internal static readonly Color COLOR_CUSTOM_GREY = new Color(149, 169, 179);

		internal static readonly Color COLOR_CUSTOM_BLUE = new Color(117, 201, 241);

		private IMyGuiControlsParent m_controlsParent;

		private bool m_userIsFounder;

		private bool m_userIsLeader;

		private long m_selectedUserId;

		private string m_selectedUserName;

		private IMyFaction m_userFaction;

		private IMyFaction m_selectedFaction;

		private MyGuiControlTable m_tableFactions;

		private MyGuiControlCombobox m_tableFactionsFilter;

		private MyGuiControlButton m_buttonCreate;

		private MyGuiControlButton m_buttonJoin;

		private MyGuiControlButton m_buttonCancelJoin;

		private MyGuiControlButton m_buttonLeave;

		private MyGuiControlButton m_buttonSendPeace;

		private MyGuiControlButton m_buttonCancelPeace;

		private MyGuiControlButton m_buttonAcceptPeace;

		private MyGuiControlButton m_buttonMakeEnemy;

		private MyGuiControlLabel m_labelFactionDesc;

		private MyGuiControlLabel m_labelFactionPriv;

		private MyGuiControlLabel m_labelReputation;

		private MyGuiControlLabel m_textReputation;

		private MyGuiControlLabel m_labelMembers;

		private MyGuiControlLabel m_labelAutoAcceptMember;

		private MyGuiControlLabel m_labelAutoAcceptPeace;

		private MyGuiControlCheckbox m_checkAutoAcceptMember;

		private MyGuiControlCheckbox m_checkAutoAcceptPeace;

		private MyGuiControlMultilineText m_textFactionDesc;

		private MyGuiControlImage m_factionIcon;

		private MyGuiControlMultilineText m_textFactionPriv;

		private MyGuiControlTable m_tableMembers;

		private MyGuiControlButton m_buttonEdit;

		private MyGuiControlButton m_buttonKick;

		private MyGuiControlButton m_buttonPromote;

		private MyGuiControlButton m_buttonDemote;

		private MyGuiControlButton m_buttonAcceptJoin;

		private MyGuiControlButton m_buttonShareProgress;

		private MyGuiControlButton m_buttonAddNpc;

		private MyGuiControlButton m_btnWithdraw;

		private MyGuiControlButton m_btnDeposit;

		private MyGuiReputationProgressBar m_progressReputation;

		private IMyFaction TargetFaction
		{
			get
			{
				if (m_selectedFaction != null && MySession.Static.IsUserAdmin(Sync.MyId))
				{
					return m_selectedFaction;
				}
				return m_userFaction;
			}
		}

		public void Init(IMyGuiControlsParent controlsParent)
		{
			m_controlsParent = controlsParent;
			RefreshUserInfo();
			m_tableFactions = (MyGuiControlTable)controlsParent.Controls.GetControlByName("FactionsTable");
			m_tableFactions.SetColumnComparison(0, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => ((StringBuilder)a.UserData).CompareToIgnoreCase((StringBuilder)b.UserData));
			m_tableFactions.SetColumnComparison(1, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => ((StringBuilder)a.UserData).CompareToIgnoreCase((StringBuilder)b.UserData));
			m_tableFactions.ItemSelected += OnFactionsTableItemSelected;
			RefreshTableFactions(MyFactionFilter.None);
			m_tableFactions.SortByColumn(1);
			m_tableFactionsFilter = (MyGuiControlCombobox)controlsParent.Controls.GetControlByName("FactionFilters");
			RefreshFilterCombo();
			m_tableFactionsFilter.ItemSelected += OnFactionFilterItemSelected;
			m_buttonCreate = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonCreate");
			m_buttonJoin = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonJoin");
			m_buttonCancelJoin = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonCancelJoin");
			m_buttonLeave = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonLeave");
			m_buttonSendPeace = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonSendPeace");
			m_buttonCancelPeace = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonCancelPeace");
			m_buttonAcceptPeace = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonAcceptPeace");
			m_buttonMakeEnemy = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonEnemy");
			m_buttonMakeEnemy.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_EnemyToolTip));
			m_buttonLeave.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_LeaveToolTip));
			m_buttonCreate.ShowTooltipWhenDisabled = true;
			m_buttonCreate.TextEnum = MySpaceTexts.TerminalTab_Factions_Create;
			m_buttonJoin.TextEnum = MySpaceTexts.TerminalTab_Factions_Join;
			m_buttonCancelJoin.TextEnum = MySpaceTexts.TerminalTab_Factions_CancelJoin;
			m_buttonLeave.TextEnum = MySpaceTexts.TerminalTab_Factions_Leave;
			m_buttonSendPeace.TextEnum = MySpaceTexts.TerminalTab_Factions_Friend;
			m_buttonCancelPeace.TextEnum = MySpaceTexts.TerminalTab_Factions_CancelPeaceRequest;
			m_buttonAcceptPeace.TextEnum = MySpaceTexts.TerminalTab_Factions_AcceptPeaceRequest;
			m_buttonMakeEnemy.TextEnum = MySpaceTexts.TerminalTab_Factions_Enemy;
			m_buttonJoin.SetToolTip(MySpaceTexts.TerminalTab_Factions_JoinToolTip);
			m_buttonSendPeace.SetToolTip(MySpaceTexts.TerminalTab_Factions_FriendToolTip);
			m_buttonCreate.ButtonClicked += OnCreateClicked;
			m_buttonJoin.ButtonClicked += OnJoinClicked;
			m_buttonCancelJoin.ButtonClicked += OnCancelJoinClicked;
			m_buttonLeave.ButtonClicked += OnLeaveClicked;
			m_buttonSendPeace.ButtonClicked += OnFriendClicked;
			m_buttonCancelPeace.ButtonClicked += OnCancelPeaceRequestClicked;
			m_buttonAcceptPeace.ButtonClicked += OnAcceptFriendClicked;
			m_buttonMakeEnemy.ButtonClicked += OnEnemyClicked;
			m_labelFactionDesc = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelFactionDesc");
			m_labelFactionPriv = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelFactionPrivate");
			m_textReputation = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("textReputation");
			m_labelReputation = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelReputation");
			m_labelMembers = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelFactionMembers");
			m_labelAutoAcceptMember = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelFactionMembersAcceptEveryone");
			m_labelAutoAcceptPeace = (MyGuiControlLabel)controlsParent.Controls.GetControlByName("labelFactionMembersAcceptPeace");
			m_labelFactionDesc.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_CreateFactionDescription).ToString();
			m_labelFactionPriv.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Private).ToString();
			m_labelReputation.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Reputation).ToString();
			m_textReputation.Text = string.Empty;
			m_labelMembers.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_Members).ToString();
			m_labelAutoAcceptMember.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_AutoAccept).ToString();
			m_labelAutoAcceptPeace.Text = MyTexts.Get(MySpaceTexts.TerminalTab_Factions_AutoAcceptRequest).ToString();
			m_labelAutoAcceptMember.SetToolTip(MySpaceTexts.TerminalTab_Factions_AutoAcceptToolTip);
			m_labelAutoAcceptPeace.SetToolTip(MySpaceTexts.TerminalTab_Factions_AutoAcceptRequestToolTip);
			m_textFactionDesc = (MyGuiControlMultilineText)controlsParent.Controls.GetControlByName("textFactionDesc");
			m_textFactionPriv = (MyGuiControlMultilineText)controlsParent.Controls.GetControlByName("textFactionPrivate");
			m_factionIcon = (MyGuiControlImage)controlsParent.Controls.GetControlByName("factionIcon");
			m_progressReputation = (MyGuiReputationProgressBar)controlsParent.Controls.GetControlByName("progressReputation");
			m_textFactionDesc.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
			m_textFactionPriv.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
			m_tableMembers = (MyGuiControlTable)controlsParent.Controls.GetControlByName("tableMembers");
			m_tableMembers.SetColumnComparison(1, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => ((int)(MyMemberComparerEnum)a.UserData).CompareTo((int)(MyMemberComparerEnum)b.UserData));
			m_tableMembers.ItemSelected += OnTableItemSelected;
			m_checkAutoAcceptMember = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("checkFactionMembersAcceptEveryone");
			m_checkAutoAcceptPeace = (MyGuiControlCheckbox)controlsParent.Controls.GetControlByName("checkFactionMembersAcceptPeace");
			m_checkAutoAcceptMember.SetToolTip(MySpaceTexts.TerminalTab_Factions_AutoAcceptToolTip);
			m_checkAutoAcceptPeace.SetToolTip(MySpaceTexts.TerminalTab_Factions_AutoAcceptRequestToolTip);
			MyGuiControlCheckbox checkAutoAcceptMember = m_checkAutoAcceptMember;
			checkAutoAcceptMember.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkAutoAcceptMember.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAutoAcceptChanged));
			MyGuiControlCheckbox checkAutoAcceptPeace = m_checkAutoAcceptPeace;
			checkAutoAcceptPeace.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkAutoAcceptPeace.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAutoAcceptChanged));
			m_buttonEdit = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonEdit");
			m_buttonPromote = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonPromote");
			m_buttonKick = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonKick");
			m_buttonAcceptJoin = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonAcceptJoin");
			m_buttonDemote = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonDemote");
			m_buttonShareProgress = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonShareProgress");
			m_buttonAddNpc = (MyGuiControlButton)controlsParent.Controls.GetControlByName("buttonAddNpc");
			MyGuiControlImage obj = (MyGuiControlImage)controlsParent.Controls.GetControlByName("imageCurrency");
			string[] icons = MyBankingSystem.BankingSystemDefinition.Icons;
			obj.SetTexture((icons != null && icons.Length != 0) ? MyBankingSystem.BankingSystemDefinition.Icons[0] : string.Empty);
			m_buttonEdit.SetToolTip(MySpaceTexts.TerminalTab_Factions_FriendToolTip);
			m_buttonPromote.SetToolTip(MySpaceTexts.TerminalTab_Factions_PromoteToolTip);
			m_buttonKick.SetToolTip(MySpaceTexts.TerminalTab_Factions_KickToolTip);
			m_buttonDemote.SetToolTip(MySpaceTexts.TerminalTab_Factions_DemoteToolTip);
			m_buttonAcceptJoin.SetToolTip(MySpaceTexts.TerminalTab_Factions_JoinToolTip);
			m_buttonShareProgress.SetToolTip(MySpaceTexts.TerminalTab_Factions_ShareProgressToolTip);
			m_buttonAddNpc.SetToolTip(MySpaceTexts.AddNpcToFactionHelp);
			m_buttonEdit.TextEnum = MyCommonTexts.Edit;
			m_buttonPromote.TextEnum = MyCommonTexts.Promote;
			m_buttonKick.TextEnum = MyCommonTexts.Kick;
			m_buttonAcceptJoin.TextEnum = MyCommonTexts.Accept;
			m_buttonDemote.TextEnum = MyCommonTexts.Demote;
			m_buttonShareProgress.TextEnum = MySpaceTexts.ShareProgress;
			m_buttonAddNpc.TextEnum = MySpaceTexts.AddNpcToFaction;
			m_buttonEdit.ButtonClicked += OnEditClicked;
			m_buttonPromote.ButtonClicked += OnPromotePlayerClicked;
			m_buttonKick.ButtonClicked += OnKickPlayerClicked;
			m_buttonAcceptJoin.ButtonClicked += OnAcceptJoinClicked;
			m_buttonDemote.ButtonClicked += OnDemoteClicked;
			m_buttonShareProgress.ButtonClicked += OnShareProgressClicked;
			m_buttonAddNpc.ButtonClicked += OnNewNpcClicked;
			m_btnWithdraw = (MyGuiControlButton)controlsParent.Controls.GetControlByName("withdrawBtn");
			m_btnWithdraw.TextEnum = MySpaceTexts.FactionTerminal_Withdraw_Currency;
			m_btnWithdraw.SetToolTip(MySpaceTexts.FactionTerminal_Withdraw_Currency_TTIP);
			m_btnWithdraw.ButtonClicked += OnWithdrawDepositBntClicked;
			m_btnWithdraw.UserData = MyTransactionType.Withdraw;
			m_btnDeposit = (MyGuiControlButton)controlsParent.Controls.GetControlByName("depositBtn");
			m_btnDeposit.TextEnum = MySpaceTexts.FactionTerminal_Deposit_Currency;
			m_btnDeposit.SetToolTip(MySpaceTexts.FactionTerminal_Deposit_Currency_TTIP);
			m_btnDeposit.ButtonClicked += OnWithdrawDepositBntClicked;
			m_btnDeposit.UserData = MyTransactionType.Deposit;
			MySession.Static.Factions.FactionCreated += OnFactionCreated;
			MySession.Static.Factions.FactionEdited += OnFactionEdited;
			MySession.Static.Factions.FactionStateChanged += OnFactionsStateChanged;
			MySession.Static.Factions.FactionAutoAcceptChanged += OnAutoAcceptChanged;
			MySession.Static.OnUserPromoteLevelChanged += OnUserPromoteLevelChanged;
			MyBankingSystem.Static.OnAccountBalanceChanged += OnAccountBalanceChanged;
			Refresh();
		}

		public void Close()
		{
			UnregisterEvents();
			m_selectedFaction = null;
			m_tableFactions = null;
			m_buttonCreate = null;
			m_buttonJoin = null;
			m_buttonCancelJoin = null;
			m_buttonLeave = null;
			m_buttonSendPeace = null;
			m_buttonCancelPeace = null;
			m_buttonAcceptPeace = null;
			m_buttonMakeEnemy = null;
			m_labelFactionDesc = null;
			m_labelReputation = null;
			m_textReputation = null;
			m_labelFactionPriv = null;
			m_labelMembers = null;
			m_labelAutoAcceptMember = null;
			m_labelAutoAcceptPeace = null;
			m_checkAutoAcceptMember = null;
			m_checkAutoAcceptPeace = null;
			m_textFactionDesc = null;
			m_factionIcon = null;
			m_textFactionPriv = null;
			m_tableMembers = null;
			m_buttonKick = null;
			m_buttonAcceptJoin = null;
			m_buttonShareProgress = null;
			m_controlsParent = null;
		}

		private void UnregisterEvents()
		{
			if (m_controlsParent != null)
			{
				MyBankingSystem.Static.OnAccountBalanceChanged -= OnAccountBalanceChanged;
				MySession.Static.OnUserPromoteLevelChanged -= OnUserPromoteLevelChanged;
				MySession.Static.Factions.FactionCreated -= OnFactionCreated;
				MySession.Static.Factions.FactionEdited -= OnFactionEdited;
				MySession.Static.Factions.FactionStateChanged -= OnFactionsStateChanged;
				MySession.Static.Factions.FactionAutoAcceptChanged -= OnAutoAcceptChanged;
				m_tableFactions.ItemSelected -= OnFactionsTableItemSelected;
				m_tableFactionsFilter.ItemSelected -= OnFactionFilterItemSelected;
				m_tableMembers.ItemSelected -= OnTableItemSelected;
				MyGuiControlCheckbox checkAutoAcceptMember = m_checkAutoAcceptMember;
				checkAutoAcceptMember.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkAutoAcceptMember.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAutoAcceptChanged));
				MyGuiControlCheckbox checkAutoAcceptPeace = m_checkAutoAcceptPeace;
				checkAutoAcceptPeace.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkAutoAcceptPeace.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAutoAcceptChanged));
				m_buttonCreate.ButtonClicked -= OnCreateClicked;
				m_buttonJoin.ButtonClicked -= OnJoinClicked;
				m_buttonCancelJoin.ButtonClicked -= OnCancelJoinClicked;
				m_buttonLeave.ButtonClicked -= OnLeaveClicked;
				m_buttonSendPeace.ButtonClicked -= OnFriendClicked;
				m_buttonAcceptPeace.ButtonClicked -= OnAcceptFriendClicked;
				m_buttonMakeEnemy.ButtonClicked -= OnEnemyClicked;
				m_buttonEdit.ButtonClicked -= OnEditClicked;
				m_buttonPromote.ButtonClicked -= OnPromotePlayerClicked;
				m_buttonKick.ButtonClicked -= OnKickPlayerClicked;
				m_buttonAcceptJoin.ButtonClicked -= OnAcceptJoinClicked;
				m_buttonDemote.ButtonClicked -= OnDemoteClicked;
				m_buttonShareProgress.ButtonClicked -= OnShareProgressClicked;
				m_buttonAddNpc.ButtonClicked -= OnNewNpcClicked;
				m_btnWithdraw.ButtonClicked -= OnWithdrawDepositBntClicked;
				m_btnDeposit.ButtonClicked -= OnWithdrawDepositBntClicked;
			}
		}

		private void OnFactionsTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
		{
			if (sender.SelectedRow != null)
			{
				m_selectedFaction = (MyFaction)sender.SelectedRow.UserData;
				bool flag = IsFactionDiscovered(m_selectedFaction);
				if (flag)
				{
					m_textFactionDesc.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
					m_textFactionDesc.Text = new StringBuilder(m_selectedFaction.Description);
					m_textFactionPriv.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
					m_textFactionPriv.Text = new StringBuilder(m_selectedFaction.PrivateInfo);
					GetFactionIconTextures(flag, out MyGuiControlImage.MyDrawTexture backgroundTexture, out MyGuiControlImage.MyDrawTexture iconFaction);
					m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
					{
						backgroundTexture,
						iconFaction
					});
				}
				else
				{
					m_textFactionDesc.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
					m_textFactionDesc.Text = MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable);
					m_textFactionDesc.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
					m_textFactionPriv.Text = MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable);
					m_factionIcon.SetTexture();
					m_factionIcon.ColorMask = new Vector4(1f, 1f, 1f, 1f);
				}
				UpdateAccountControls();
				RefreshTableMembers();
			}
			else
			{
				m_selectedFaction = null;
			}
			m_tableMembers.Sort(switchSort: false);
			RefreshJoinButton();
			RefreshDiplomacyButtons();
			RefreshFactionProperties();
		}

		private void UpdateAccountControls()
		{
			if (m_selectedFaction != null && m_tableFactions.SelectedRow != null)
			{
				bool flag = m_selectedFaction == m_userFaction || MySession.Static.CreativeToolsEnabled(MySession.Static.LocalHumanPlayer.Client.SteamUserId);
				MyGuiControlLabel obj = (MyGuiControlLabel)m_controlsParent.Controls.GetControlByName("labelBalanceValue");
				obj.UserData = null;
				obj.Text = (flag ? MyBankingSystem.Static.GetBalanceShortString(m_selectedFaction.FactionId, addCurrencyShortName: false) : MyTexts.GetString(MySpaceTexts.Terminal_Factions_DataNotAvailable));
				m_controlsParent.Controls.GetControlByName("imageCurrency").Visible = flag;
				if (m_tableFactions.SelectedRow != null && flag)
				{
					if ((m_userIsLeader || MySession.Static.CreativeToolsEnabled(MySession.Static.LocalHumanPlayer.Client.SteamUserId)) && flag)
					{
						m_btnWithdraw.Enabled = true;
					}
					else
					{
						m_btnWithdraw.Enabled = false;
					}
					m_btnDeposit.Enabled = flag;
				}
				else
				{
					m_btnWithdraw.Enabled = false;
					m_btnDeposit.Enabled = false;
				}
			}
			else
			{
				m_btnWithdraw.Enabled = false;
				m_btnDeposit.Enabled = false;
			}
		}

		internal void Update()
		{
			if (m_selectedFaction == null)
			{
				return;
			}
			MyGuiControlLabel myGuiControlLabel = (MyGuiControlLabel)m_controlsParent.Controls.GetControlByName("labelBalanceValue");
			if (myGuiControlLabel.UserData != null)
			{
				AccountBalanceAnimationInfo accountBalanceAnimationInfo = (AccountBalanceAnimationInfo)myGuiControlLabel.UserData;
				long num = accountBalanceAnimationInfo.NewBalance - accountBalanceAnimationInfo.OldBalance;
				long num2 = (long)((float)num * 0.2f);
				long num3 = accountBalanceAnimationInfo.OldBalance + num2;
				if (Math.Abs(num) < 10)
				{
					num3 = accountBalanceAnimationInfo.NewBalance;
					myGuiControlLabel.UserData = null;
				}
				else
				{
					accountBalanceAnimationInfo.OldBalance = num3;
					myGuiControlLabel.UserData = accountBalanceAnimationInfo;
				}
				myGuiControlLabel.Text = MyBankingSystem.GetFormatedValue(num3);
			}
		}

		private void OnAccountBalanceChanged(MyAccountInfo accOldInfo, MyAccountInfo accInfo)
		{
			if (m_selectedFaction != null && m_selectedFaction.FactionId == accInfo.OwnerIdentifier)
			{
				((MyGuiControlLabel)m_controlsParent.Controls.GetControlByName("labelBalanceValue")).UserData = new AccountBalanceAnimationInfo
				{
					OldBalance = accOldInfo.Balance,
					NewBalance = accInfo.Balance
				};
			}
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs args)
		{
			RefreshRightSideButtons(sender.SelectedRow);
		}

		private void OnFactionFilterItemSelected()
		{
			MyFactionFilter factionFilter = (MyFactionFilter)m_tableFactionsFilter.GetSelectedKey();
			RefreshTableFactions(factionFilter);
		}

		private void OnCreateClicked(MyGuiControlButton sender)
		{
			MyGuiScreenCreateOrEditFaction obj = (MyGuiScreenCreateOrEditFaction)MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.CreateFactionScreen);
			obj.Init(ref m_userFaction);
			MyGuiSandbox.AddScreen(obj);
		}

		private void OnEditClicked(MyGuiControlButton sender)
		{
			IMyFaction editData = TargetFaction;
			if (editData != null)
			{
				MyGuiScreenCreateOrEditFaction obj = (MyGuiScreenCreateOrEditFaction)MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.CreateFactionScreen);
				obj.Init(ref editData);
				MyGuiSandbox.AddScreen(obj);
			}
		}

		private void OnJoinClicked(MyGuiControlButton sender)
		{
			if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.PER_FACTION && !MyBlockLimits.IsFactionChangePossible(MySession.Static.LocalPlayerId, m_selectedFaction.FactionId))
			{
				ShowErrorBox(new StringBuilder(MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_JoinLimitsExceeded)));
			}
			MyFactionCollection.SendJoinRequest(m_selectedFaction.FactionId, MySession.Static.LocalPlayerId);
		}

		private void OnCancelJoinClicked(MyGuiControlButton sender)
		{
			MyFactionCollection.CancelJoinRequest(m_selectedFaction.FactionId, MySession.Static.LocalPlayerId);
		}

		private void OnLeaveClicked(MyGuiControlButton sender)
		{
			if (m_selectedFaction.FactionId == m_userFaction.FactionId)
			{
				ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsLeave, MyStatControlText.SubstituteTexts(m_userFaction.Name)), LeaveFaction);
			}
		}

		private void LeaveFaction()
		{
			if (m_userFaction != null)
			{
				MyFactionCollection.MemberLeaves(m_userFaction.FactionId, MySession.Static.LocalPlayerId);
				m_userFaction = null;
				Refresh();
			}
		}

		private void OnFriendClicked(MyGuiControlButton sender)
		{
			MyFactionCollection.SendPeaceRequest(m_userFaction.FactionId, m_selectedFaction.FactionId);
		}

		private void OnAcceptFriendClicked(MyGuiControlButton sender)
		{
			MyFactionCollection.AcceptPeace(m_userFaction.FactionId, m_selectedFaction.FactionId);
		}

		private void OnCancelPeaceRequestClicked(MyGuiControlButton sender)
		{
			MyFactionCollection.CancelPeaceRequest(m_userFaction.FactionId, m_selectedFaction.FactionId);
		}

		private void OnEnemyClicked(MyGuiControlButton sender)
		{
			ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmWarDeclaration, MyStatControlText.SubstituteTexts(m_selectedFaction.Name)), delegate
			{
				MyFactionCollection.DeclareWar(m_userFaction.FactionId, m_selectedFaction.FactionId);
			});
		}

		private void OnAutoAcceptChanged(MyGuiControlCheckbox sender)
		{
			IMyFaction targetFaction = TargetFaction;
			if (targetFaction != null)
			{
				MySession.Static.Factions.ChangeAutoAccept(targetFaction.FactionId, MySession.Static.LocalPlayerId, m_checkAutoAcceptMember.IsChecked, m_checkAutoAcceptPeace.IsChecked);
			}
		}

		private void OnAutoAcceptChanged(long factionId, bool autoAcceptMember, bool autoAcceptPeace)
		{
			RefreshFactionProperties();
		}

		private void OnPromotePlayerClicked(MyGuiControlButton sender)
		{
			if (m_tableMembers.SelectedRow != null)
			{
				ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsPromote, m_selectedUserName), PromotePlayer);
			}
		}

		private void PromotePlayer()
		{
			MyFactionCollection.PromoteMember(TargetFaction.FactionId, m_selectedUserId);
		}

		private void OnKickPlayerClicked(MyGuiControlButton sender)
		{
			if (m_tableMembers.SelectedRow != null)
			{
				ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsKickPlayer, m_selectedUserName), KickPlayer);
			}
		}

		private void KickPlayer()
		{
			if (TargetFaction != null)
			{
				MyFactionCollection.KickMember(TargetFaction.FactionId, m_selectedUserId);
			}
		}

		private void OnAcceptJoinClicked(MyGuiControlButton sender)
		{
			if (m_tableMembers.SelectedRow != null)
			{
				ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsAcceptJoin, m_selectedUserName), AcceptJoin);
			}
		}

		private void AcceptJoin()
		{
			MyFactionCollection.AcceptJoin(TargetFaction.FactionId, m_selectedUserId);
		}

		private void OnDemoteClicked(MyGuiControlButton sender)
		{
			if (m_tableMembers.SelectedRow != null)
			{
				ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmFactionsDemote, m_selectedUserName), Demote);
			}
		}

		private void Demote()
		{
			MyFactionCollection.DemoteMember(TargetFaction.FactionId, m_selectedUserId);
		}

		private void OnShareProgressClicked(MyGuiControlButton sender)
		{
			if (m_tableMembers.SelectedRow != null)
			{
				ShowConfirmBox(new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxConfirmShareResearch, m_selectedUserName), ShareProgress);
			}
		}

		private void ShareProgress()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => MySessionComponentResearch.CallShareResearch, m_selectedUserId);
		}

		private void OnNewNpcClicked(MyGuiControlButton sender)
		{
			string arg = TargetFaction.Tag + " NPC" + MyRandom.Instance.Next(1000, 9999);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => NewNpcClickedInternal, TargetFaction.FactionId, arg);
		}

		[Event(null, 661)]
		[Reliable]
		[Server]
		public static void NewNpcClickedInternal(long factionId, string npcName)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyIdentity myIdentity = Sync.Players.CreateNewNpcIdentity(npcName, 0L);
			MyFactionCollection.SendJoinRequest(factionId, myIdentity.IdentityId);
		}

		private void OnWithdrawDepositBntClicked(MyGuiControlButton obj)
		{
			MyIdentity myIdentity = MySession.Static.Players.TryGetPlayerIdentity(MySession.Static.LocalHumanPlayer.Id);
			long num;
			long num2;
			if ((MyTransactionType)obj.UserData == MyTransactionType.Deposit)
			{
				num = myIdentity.IdentityId;
				num2 = m_selectedFaction.FactionId;
			}
			else
			{
				num = m_selectedFaction.FactionId;
				num2 = myIdentity.IdentityId;
			}
			MyGuiSandbox.AddScreen((MyGuiScreenTransaction)MyGuiSandbox.CreateScreen(typeof(MyGuiScreenTransaction), (MyTransactionType)obj.UserData, num, num2));
		}

		private void Refresh()
		{
			RefreshUserInfo();
			RefreshCreateButton();
			RefreshJoinButton();
			RefreshDiplomacyButtons();
			RefreshRightSideButtons(null);
			RefreshFactionProperties();
			UpdateAccountControls();
		}

		private void RefreshUserInfo()
		{
			m_userIsFounder = false;
			m_userIsLeader = false;
			m_userFaction = MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId);
			if (m_userFaction != null)
			{
				m_userIsFounder = m_userFaction.IsFounder(MySession.Static.LocalPlayerId);
				m_userIsLeader = m_userFaction.IsLeader(MySession.Static.LocalPlayerId);
			}
		}

		private void RefreshCreateButton()
		{
			if (m_buttonCreate != null)
			{
				if (m_userFaction != null)
				{
					m_buttonCreate.Enabled = false;
					m_buttonCreate.SetToolTip(MySpaceTexts.TerminalTab_Factions_BeforeCreateLeave);
				}
				else if (MySession.Static.MaxFactionsCount == 0 || (MySession.Static.MaxFactionsCount > 0 && MySession.Static.Factions.HumansCount() < MySession.Static.MaxFactionsCount))
				{
					m_buttonCreate.Enabled = true;
					m_buttonCreate.SetToolTip(MySpaceTexts.TerminalTab_Factions_CreateToolTip);
				}
				else
				{
					m_buttonCreate.Enabled = false;
					m_buttonCreate.SetToolTip(MySpaceTexts.TerminalTab_Factions_MaxCountReachedToolTip);
				}
			}
		}

		private void RefreshJoinButton()
		{
			m_buttonLeave.Visible = false;
			m_buttonJoin.Visible = false;
			m_buttonCancelJoin.Visible = false;
			m_buttonLeave.Enabled = false;
			m_buttonJoin.Enabled = false;
			m_buttonCancelJoin.Enabled = false;
			if (m_userFaction != null && MySession.Static.BlockLimitsEnabled != MyBlockLimitsEnabledEnum.PER_FACTION)
			{
				m_buttonLeave.Visible = true;
				m_buttonLeave.Enabled = ((m_tableFactions.SelectedRow != null && m_tableFactions.SelectedRow.UserData == m_userFaction) ? true : false);
			}
			else if (m_tableFactions.SelectedRow != null && IsFactionDiscovered(m_selectedFaction))
			{
				if (m_selectedFaction.JoinRequests.ContainsKey(MySession.Static.LocalPlayerId))
				{
					m_buttonCancelJoin.Visible = true;
					m_buttonCancelJoin.Enabled = true;
					m_buttonJoin.Visible = false;
				}
				else if (MySession.Static.CreativeToolsEnabled(MySession.Static.LocalHumanPlayer.Client.SteamUserId) || (m_selectedFaction.AcceptHumans && m_userFaction != m_selectedFaction))
				{
					m_buttonJoin.Visible = true;
					m_buttonJoin.Enabled = true;
				}
				else
				{
					m_buttonJoin.Visible = true;
					m_buttonJoin.Enabled = false;
				}
			}
			else
			{
				m_buttonJoin.Visible = true;
				m_buttonJoin.Enabled = false;
			}
		}

		private void RefreshDiplomacyButtons()
		{
			m_buttonSendPeace.Enabled = false;
			m_buttonCancelPeace.Enabled = false;
			m_buttonAcceptPeace.Enabled = false;
			m_buttonMakeEnemy.Enabled = false;
			m_buttonCancelPeace.Visible = false;
			m_buttonAcceptPeace.Visible = false;
			if (m_userIsLeader && m_selectedFaction != null && m_selectedFaction.FactionId != m_userFaction.FactionId)
			{
				if (MySession.Static.Factions.AreFactionsEnemies(m_userFaction.FactionId, m_selectedFaction.FactionId))
				{
					if (MySession.Static.Factions.IsPeaceRequestStateSent(m_userFaction.FactionId, m_selectedFaction.FactionId))
					{
						m_buttonSendPeace.Visible = false;
						m_buttonCancelPeace.Visible = true;
						m_buttonCancelPeace.Enabled = true;
					}
					else if (MySession.Static.Factions.IsPeaceRequestStatePending(m_userFaction.FactionId, m_selectedFaction.FactionId))
					{
						m_buttonSendPeace.Visible = false;
						m_buttonAcceptPeace.Visible = true;
						m_buttonAcceptPeace.Enabled = true;
					}
					else
					{
						m_buttonSendPeace.Visible = true;
						m_buttonSendPeace.Enabled = true;
					}
				}
				else
				{
					m_buttonMakeEnemy.Enabled = true;
				}
			}
			else
			{
				m_buttonSendPeace.Visible = true;
			}
		}

		private void RefreshRightSideButtons(MyGuiControlTable.Row selected)
		{
			bool flag = MySession.Static.IsUserAdmin(Sync.MyId);
			m_buttonPromote.Enabled = false;
			m_buttonKick.Enabled = false;
			m_buttonAcceptJoin.Enabled = false;
			m_buttonDemote.Enabled = false;
			m_buttonShareProgress.Enabled = false;
			if (selected == null || selected.UserData == null)
			{
				return;
			}
			MyFactionMember myFactionMember = (MyFactionMember)selected.UserData;
			m_selectedUserId = myFactionMember.PlayerId;
			MyIdentity myIdentity = Sync.Players.TryGetIdentity(myFactionMember.PlayerId);
			m_selectedUserName = ((myIdentity != null) ? myIdentity.DisplayName : string.Empty);
			if (m_selectedUserId != MySession.Static.LocalPlayerId || flag)
			{
				if ((m_userIsFounder || flag) && TargetFaction.IsLeader(m_selectedUserId))
				{
					m_buttonKick.Enabled = true;
					m_buttonDemote.Enabled = true;
				}
				else if ((m_userIsFounder || flag) && TargetFaction.IsMember(m_selectedUserId))
				{
					m_buttonKick.Enabled = true;
					m_buttonPromote.Enabled = true;
				}
				else if ((m_userIsLeader || flag) && TargetFaction.IsMember(m_selectedUserId) && !TargetFaction.IsLeader(m_selectedUserId) && !TargetFaction.IsFounder(m_selectedUserId))
				{
					m_buttonKick.Enabled = true;
				}
				else if ((m_userIsLeader || m_userIsFounder || flag) && TargetFaction.JoinRequests.ContainsKey(m_selectedUserId))
				{
					m_buttonAcceptJoin.Enabled = true;
				}
				if (m_userFaction != null && TargetFaction.IsMember(m_selectedUserId))
				{
					m_buttonShareProgress.Enabled = true;
				}
			}
		}

		private void OnUserPromoteLevelChanged(ulong arg1, MyPromoteLevel arg2)
		{
			if (arg1 == Sync.MyId)
			{
				RefreshFactionProperties();
			}
		}

		private void RefreshFactionProperties()
		{
			bool flag = MySession.Static.IsUserAdmin(Sync.MyId);
			MyGuiControlCheckbox checkAutoAcceptMember = m_checkAutoAcceptMember;
			checkAutoAcceptMember.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkAutoAcceptMember.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAutoAcceptChanged));
			MyGuiControlCheckbox checkAutoAcceptPeace = m_checkAutoAcceptPeace;
			checkAutoAcceptPeace.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Remove(checkAutoAcceptPeace.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAutoAcceptChanged));
			m_checkAutoAcceptMember.Enabled = false;
			m_checkAutoAcceptPeace.Enabled = false;
			m_buttonEdit.Enabled = false;
			m_buttonKick.Enabled = false;
			m_buttonPromote.Enabled = false;
			m_buttonDemote.Enabled = false;
			m_buttonAcceptJoin.Enabled = false;
			m_buttonShareProgress.Enabled = false;
			m_buttonAddNpc.Enabled = false;
			if (m_tableFactions.SelectedRow != null)
			{
				m_selectedFaction = (MyFaction)m_tableFactions.SelectedRow.UserData;
				bool flag2 = IsFactionDiscovered(m_selectedFaction);
				m_textFactionDesc.Text = (flag2 ? new StringBuilder(m_selectedFaction.Description) : MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable));
				m_textFactionDesc.TextBoxAlign = ((!flag2) ? MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER : MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				if (flag2)
				{
					GetFactionIconTextures(flag2, out MyGuiControlImage.MyDrawTexture backgroundTexture, out MyGuiControlImage.MyDrawTexture iconFaction);
					m_factionIcon.SetTextures(new MyGuiControlImage.MyDrawTexture[2]
					{
						backgroundTexture,
						iconFaction
					});
				}
				else
				{
					m_factionIcon.SetTexture();
				}
				UpdateAccountControls();
				m_checkAutoAcceptMember.IsChecked = m_selectedFaction.AutoAcceptMember;
				m_checkAutoAcceptPeace.IsChecked = m_selectedFaction.AutoAcceptPeace;
				Tuple<MyRelationsBetweenFactions, int> relationBetweenPlayerAndFaction = MySession.Static.Factions.GetRelationBetweenPlayerAndFaction(MySession.Static.LocalPlayerId, m_selectedFaction.FactionId);
				int item = relationBetweenPlayerAndFaction.Item2;
				m_progressReputation.SetCurrentValue(item);
				m_textReputation.Text = item.ToString();
				MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
				if (component != null)
				{
					if (relationBetweenPlayerAndFaction.Item1 == MyRelationsBetweenFactions.Friends)
					{
						m_progressReputation.SetBonusValues(MultiplierToPercentage(component.GetOffersFriendlyBonus(item)), MultiplierToPercentage(component.GetOrdersFriendlyBonus(item)), BonusToPercentage(component.GetOffersFriendlyBonusMax()), BonusToPercentage(component.GetOrdersFriendlyBonusMax()));
					}
					else
					{
						m_progressReputation.SetBonusValues(0, 0, BonusToPercentage(component.GetOffersFriendlyBonusMax()), BonusToPercentage(component.GetOrdersFriendlyBonusMax()));
					}
				}
				if (flag || (m_userFaction != null && m_userFaction.FactionId == m_selectedFaction.FactionId))
				{
					m_textFactionPriv.Text = (flag2 ? new StringBuilder(m_selectedFaction.PrivateInfo) : MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable));
					m_textFactionPriv.TextBoxAlign = ((!flag2) ? MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER : MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
					if (flag2 & (m_userIsLeader || flag))
					{
						m_checkAutoAcceptMember.Enabled = true;
						m_checkAutoAcceptPeace.Enabled = true;
						m_buttonEdit.Enabled = true;
					}
					if (flag2 && (MySession.Static.IsUserSpaceMaster(MySession.Static.LocalHumanPlayer.Client.SteamUserId) || flag))
					{
						m_buttonAddNpc.Enabled = true;
					}
				}
				else
				{
					m_textFactionPriv.Text = null;
				}
			}
			else
			{
				m_tableMembers.Clear();
			}
			if (m_selectedFaction == null)
			{
				m_labelReputation.Visible = false;
				m_textReputation.Visible = false;
				m_progressReputation.Visible = false;
				m_labelFactionPriv.Visible = false;
				m_textFactionPriv.Visible = false;
				m_factionIcon.SetTexture();
				m_btnWithdraw.Enabled = false;
				m_btnDeposit.Enabled = false;
				HideCurrencyIcon();
				AccountBalanceNotApplicable();
			}
			else if (m_userFaction != null && m_userFaction == m_selectedFaction)
			{
				m_labelReputation.Visible = false;
				m_textReputation.Visible = false;
				m_progressReputation.Visible = false;
				m_labelFactionPriv.Visible = true;
				m_textFactionPriv.Visible = true;
			}
			else
			{
				m_labelReputation.Visible = true;
				m_textReputation.Visible = true;
				m_progressReputation.Visible = true;
				m_labelFactionPriv.Visible = false;
				m_textFactionPriv.Visible = false;
			}
			MyGuiControlCheckbox checkAutoAcceptMember2 = m_checkAutoAcceptMember;
			checkAutoAcceptMember2.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkAutoAcceptMember2.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAutoAcceptChanged));
			MyGuiControlCheckbox checkAutoAcceptPeace2 = m_checkAutoAcceptPeace;
			checkAutoAcceptPeace2.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkAutoAcceptPeace2.IsCheckedChanged, new Action<MyGuiControlCheckbox>(OnAutoAcceptChanged));
		}

		private void HideCurrencyIcon()
		{
			((MyGuiControlImage)m_controlsParent.Controls.GetControlByName("imageCurrency")).Visible = false;
		}

		private void AccountBalanceNotApplicable()
		{
			MyGuiControlLabel obj = (MyGuiControlLabel)m_controlsParent.Controls.GetControlByName("labelBalanceValue");
			obj.UserData = null;
			obj.Text = MyTexts.GetString(MySpaceTexts.Terminal_Factions_DataNotAvailable);
		}

		private int BonusToPercentage(float bonus)
		{
			return (int)(Math.Round(bonus, 2) * 100.0);
		}

		private int MultiplierToPercentage(float bonus)
		{
			return (int)(Math.Round(bonus - 1f, 2) * 100.0);
		}

		private void GetFactionIconTextures(bool isFactionDiscovered, out MyGuiControlImage.MyDrawTexture backgroundTexture, out MyGuiControlImage.MyDrawTexture iconFaction)
		{
			Vector3 hSV = MyColorPickerConstants.HSVOffsetToHSV(m_selectedFaction.CustomColor);
			MyGuiControlImage.MyDrawTexture myDrawTexture = new MyGuiControlImage.MyDrawTexture
			{
				Texture = "Textures\\GUI\\Blank.dds",
				ColorMask = hSV.HSVtoColor().ToVector4()
			};
			backgroundTexture = myDrawTexture;
			string texture = (isFactionDiscovered && m_selectedFaction.FactionIcon.HasValue) ? m_selectedFaction.FactionIcon.Value.ToString() : null;
			Vector3 hSV2 = MyColorPickerConstants.HSVOffsetToHSV(m_selectedFaction.IconColor);
			myDrawTexture = (iconFaction = new MyGuiControlImage.MyDrawTexture
			{
				Texture = texture,
				ColorMask = hSV2.HSVtoColor().ToVector4()
			});
		}

		private bool IsFactionDiscovered(IMyFaction faction)
		{
			return (MySession.Static.Factions.IsFactionDiscovered(MySession.Static.LocalHumanPlayer.Id, faction.FactionId) || !MySession.Static.Factions.IsNpcFaction(faction.Tag) || MySession.Static.Factions.IsDiscoveredByDefault(faction.Tag)) | (faction == m_userFaction) | MySession.Static.CreativeToolsEnabled(MySession.Static.LocalHumanPlayer.Client.SteamUserId);
		}

		private void RefreshTableFactions(MyFactionFilter factionFilter)
		{
			m_tableFactions.Clear();
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				MyFaction value = faction.Value;
				bool flag = MySession.Static.Factions.IsNpcFaction(value.Tag);
				bool flag2 = (m_userFaction != null && !flag) ? MySession.Static.Factions.AreFactionsEnemies(m_userFaction.FactionId, value.FactionId) : MySession.Static.Factions.IsFactionWithPlayerEnemy(MySession.Static.LocalPlayerId, value.FactionId);
				bool flag3 = (m_userFaction != null && !flag) ? MySession.Static.Factions.AreFactionsFriends(m_userFaction.FactionId, value.FactionId) : MySession.Static.Factions.IsFactionWithPlayerFriend(MySession.Static.LocalPlayerId, value.FactionId);
				bool flag4 = IsFactionDiscovered(value);
				bool flag5 = m_userFaction != null && m_userFaction.FactionId == value.FactionId;
				if ((factionFilter != MyFactionFilter.Enemy || flag2) && (factionFilter != MyFactionFilter.Neutral || !(flag2 || flag5)) && (factionFilter != MyFactionFilter.Friend || flag3) && (factionFilter != MyFactionFilter.Player || flag5) && (factionFilter != MyFactionFilter.NPC || flag) && !(factionFilter == MyFactionFilter.Unknown && flag4) && (factionFilter != MyFactionFilter.Discovered || flag4) && !(factionFilter == MyFactionFilter.PlayersFactions && flag))
				{
					Color? color = null;
					MyGuiHighlightTexture? icon = null;
					string iconToolTip = null;
					if (m_userFaction == null)
					{
						if (value.JoinRequests.ContainsKey(MySession.Static.LocalPlayerId))
						{
							icon = MyGuiConstants.TEXTURE_ICON_SENT_JOIN_REQUEST;
							iconToolTip = MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_SentJoinToolTip);
						}
					}
					else if (MySession.Static.Factions.IsPeaceRequestStateSent(m_userFaction.FactionId, value.FactionId))
					{
						icon = MyGuiConstants.TEXTURE_ICON_SENT_WHITE_FLAG;
						iconToolTip = MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_SentPeace);
					}
					else if (MySession.Static.Factions.IsPeaceRequestStatePending(m_userFaction.FactionId, value.FactionId))
					{
						icon = MyGuiConstants.TEXTURE_ICON_WHITE_FLAG;
						iconToolTip = MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_PendingPeace);
					}
					if (!flag4 && flag)
					{
						color = Color.Yellow;
					}
					else if (flag5)
					{
						color = COLOR_CUSTOM_BLUE;
					}
					else if (flag2)
					{
						color = COLOR_CUSTOM_RED;
					}
					else if (flag3)
					{
						color = COLOR_CUSTOM_GREEN;
					}
					AddFaction(value, color, icon, iconToolTip, !flag4);
				}
			}
			m_tableFactions.Sort(switchSort: false);
		}

		private void RefreshFilterCombo()
		{
			m_tableFactionsFilter.ClearItems();
			m_tableFactionsFilter.AddItem(0L, MyTexts.Get(MySpaceTexts.Faction_Filter_None));
			m_tableFactionsFilter.AddItem(1L, MyTexts.Get(MySpaceTexts.Faction_Filter_Player));
			m_tableFactionsFilter.AddItem(5L, MyTexts.Get(MySpaceTexts.Faction_Filter_NPC));
			m_tableFactionsFilter.AddItem(2L, MyTexts.Get(MySpaceTexts.Faction_Filter_Friend));
			m_tableFactionsFilter.AddItem(4L, MyTexts.Get(MySpaceTexts.Faction_Filter_Enemy));
			m_tableFactionsFilter.AddItem(3L, MyTexts.Get(MySpaceTexts.Faction_Filter_Neutral));
			m_tableFactionsFilter.AddItem(6L, MyTexts.Get(MySpaceTexts.Faction_Filter_Unknown));
			m_tableFactionsFilter.AddItem(8L, MyTexts.Get(MySpaceTexts.Faction_Filter_Discovered));
			m_tableFactionsFilter.AddItem(7L, MyTexts.Get(MySpaceTexts.Faction_Filter_PlayersFactions));
			m_tableFactionsFilter.SelectItemByKey(0L);
		}

		private void RefreshTableMembers()
		{
			m_tableMembers.Clear();
			if (IsFactionDiscovered(m_selectedFaction))
			{
				foreach (KeyValuePair<long, MyFactionMember> member in m_selectedFaction.Members)
				{
					MyFactionMember value = member.Value;
					MyIdentity myIdentity = Sync.Players.TryGetIdentity(value.PlayerId);
					if (myIdentity != null)
					{
						MyGuiControlTable.Row row = new MyGuiControlTable.Row(value);
						MyMemberComparerEnum myMemberComparerEnum = MyMemberComparerEnum.Member;
						MyStringId id = MyCommonTexts.Member;
						Color? textColor = null;
						if (m_selectedFaction.IsFounder(value.PlayerId))
						{
							myMemberComparerEnum = MyMemberComparerEnum.Founder;
							id = MyCommonTexts.Founder;
						}
						else if (m_selectedFaction.IsLeader(value.PlayerId))
						{
							myMemberComparerEnum = MyMemberComparerEnum.Leader;
							id = MyCommonTexts.Leader;
						}
						else if (m_selectedFaction.JoinRequests.ContainsKey(value.PlayerId))
						{
							textColor = COLOR_CUSTOM_GREY;
							myMemberComparerEnum = MyMemberComparerEnum.Applicant;
							id = MyCommonTexts.Applicant;
						}
						row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(myIdentity.DisplayName), toolTip: myIdentity.DisplayName, userData: member, textColor: textColor));
						row.AddCell(new MyGuiControlTable.Cell(MyTexts.Get(id), toolTip: MyTexts.GetString(id), userData: myMemberComparerEnum, textColor: textColor));
						m_tableMembers.Add(row);
					}
				}
				foreach (KeyValuePair<long, MyFactionMember> joinRequest in m_selectedFaction.JoinRequests)
				{
					MyFactionMember value2 = joinRequest.Value;
					MyGuiControlTable.Row row2 = new MyGuiControlTable.Row(value2);
					MyIdentity myIdentity2 = Sync.Players.TryGetIdentity(value2.PlayerId);
					if (myIdentity2 != null)
					{
						row2.AddCell(new MyGuiControlTable.Cell(new StringBuilder(myIdentity2.DisplayName), toolTip: myIdentity2.DisplayName, userData: joinRequest, textColor: COLOR_CUSTOM_GREY));
						row2.AddCell(new MyGuiControlTable.Cell(MyTexts.Get(MyCommonTexts.Applicant), MyMemberComparerEnum.Applicant, MyTexts.GetString(MyCommonTexts.Applicant), COLOR_CUSTOM_GREY));
						m_tableMembers.Add(row2);
					}
				}
				return;
			}
			MyGuiControlTable.Row row3 = new MyGuiControlTable.Row();
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(MyTexts.Get(MySpaceTexts.Terminal_Factions_DataNotAvailable), null, MyTexts.GetString(MySpaceTexts.Terminal_Factions_DataNotAvailable), COLOR_CUSTOM_GREY);
			row3.AddCell(cell);
			m_tableMembers.Add(row3);
		}

		private void OnFactionCreated(long insertedId)
		{
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(insertedId);
			AddFaction(myFaction, myFaction.IsMember(MySession.Static.LocalPlayerId) ? COLOR_CUSTOM_GREEN : COLOR_CUSTOM_RED);
			Refresh();
			RefreshTableFactions((MyFactionFilter)m_tableFactionsFilter.GetSelectedKey());
			m_tableFactions.Sort(switchSort: false);
			m_tableFactions.SelectedRowIndex = m_tableFactions.FindIndex((MyGuiControlTable.Row row) => ((MyFaction)row.UserData).FactionId == insertedId);
			OnFactionsTableItemSelected(m_tableFactions, default(MyGuiControlTable.EventArgs));
		}

		private void OnFactionEdited(long editedId)
		{
			RefreshTableFactions((MyFactionFilter)m_tableFactionsFilter.GetSelectedKey());
			m_tableFactions.SelectedRowIndex = m_tableFactions.FindIndex((MyGuiControlTable.Row row) => ((MyFaction)row.UserData).FactionId == editedId);
			OnFactionsTableItemSelected(m_tableFactions, default(MyGuiControlTable.EventArgs));
			Refresh();
		}

		private void OnFactionsStateChanged(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			if (MySession.Static == null || MySession.Static.Factions == null || m_tableFactions == null)
			{
				return;
			}
			IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(fromFactionId);
			bool flag = false;
			if (myFaction != null)
			{
				flag |= MySession.Static.Factions.IsFactionDiscovered(MySession.Static.LocalHumanPlayer.Id, myFaction.FactionId);
				flag |= (!MySession.Static.Factions.IsNpcFaction(myFaction.Tag) || MySession.Static.Factions.IsDiscoveredByDefault(myFaction.Tag));
				flag |= (myFaction == m_userFaction);
			}
			IMyFaction myFaction2 = MySession.Static.Factions.TryGetFactionById(toFactionId);
			bool flag2 = false;
			if (myFaction2 != null)
			{
				flag2 |= MySession.Static.Factions.IsFactionDiscovered(MySession.Static.LocalHumanPlayer.Id, myFaction2.FactionId);
				flag2 |= (!MySession.Static.Factions.IsNpcFaction(myFaction2.Tag) || MySession.Static.Factions.IsDiscoveredByDefault(myFaction2.Tag));
				flag2 |= (myFaction2 == m_userFaction);
			}
			switch (action)
			{
			case MyFactionStateChange.SendFriendRequest:
				if (m_userFaction == null)
				{
					return;
				}
				if (m_userFaction.FactionId == fromFactionId)
				{
					RemoveFaction(toFactionId);
					AddFaction(myFaction2, null, MyGuiConstants.TEXTURE_ICON_SENT_WHITE_FLAG, MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_SentPeace), !flag2);
				}
				else if (m_userFaction.FactionId == toFactionId)
				{
					RemoveFaction(fromFactionId);
					AddFaction(myFaction, null, MyGuiConstants.TEXTURE_ICON_WHITE_FLAG, MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_PendingPeace), !flag);
				}
				break;
			case MyFactionStateChange.SendPeaceRequest:
				if (m_userFaction == null)
				{
					return;
				}
				if (m_userFaction.FactionId == fromFactionId)
				{
					RemoveFaction(toFactionId);
					AddFaction(myFaction2, COLOR_CUSTOM_RED, MyGuiConstants.TEXTURE_ICON_SENT_WHITE_FLAG, MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_SentPeace), !flag2);
				}
				else if (m_userFaction.FactionId == toFactionId)
				{
					RemoveFaction(fromFactionId);
					AddFaction(myFaction, COLOR_CUSTOM_RED, MyGuiConstants.TEXTURE_ICON_WHITE_FLAG, MyTexts.GetString(MySpaceTexts.TerminalTab_Factions_PendingPeace), !flag);
				}
				break;
			case MyFactionStateChange.AcceptFriendRequest:
				if (m_userFaction == null)
				{
					return;
				}
				if (m_userFaction.FactionId == fromFactionId)
				{
					RemoveFaction(toFactionId);
					AddFaction(myFaction2, COLOR_CUSTOM_GREEN, null, null, !flag2);
				}
				else if (m_userFaction.FactionId == toFactionId)
				{
					RemoveFaction(fromFactionId);
					AddFaction(myFaction, COLOR_CUSTOM_GREEN, null, null, !flag);
				}
				break;
			case MyFactionStateChange.AcceptPeace:
			case MyFactionStateChange.CancelFriendRequest:
				if (m_userFaction == null)
				{
					return;
				}
				if (m_userFaction.FactionId == fromFactionId)
				{
					RemoveFaction(toFactionId);
					AddFaction(myFaction2, null, null, null, !flag2);
				}
				else if (m_userFaction.FactionId == toFactionId)
				{
					RemoveFaction(fromFactionId);
					AddFaction(myFaction, null, null, null, !flag);
				}
				break;
			case MyFactionStateChange.CancelPeaceRequest:
			case MyFactionStateChange.DeclareWar:
				if (m_userFaction == null)
				{
					return;
				}
				if (m_userFaction.FactionId == fromFactionId)
				{
					RemoveFaction(toFactionId);
					AddFaction(myFaction2, COLOR_CUSTOM_RED, null, null, !flag2);
				}
				else if (m_userFaction.FactionId == toFactionId)
				{
					RemoveFaction(fromFactionId);
					AddFaction(myFaction, COLOR_CUSTOM_RED, null, null, !flag);
				}
				break;
			case MyFactionStateChange.RemoveFaction:
				RemoveFaction(toFactionId);
				break;
			default:
				OnMemberStateChanged(action, myFaction, playerId);
				break;
			}
			m_tableFactions.Sort(switchSort: false);
			m_tableFactions.SelectedRowIndex = m_tableFactions.FindIndex((MyGuiControlTable.Row row) => ((MyFaction)row.UserData).FactionId == toFactionId);
			OnFactionsTableItemSelected(m_tableFactions, default(MyGuiControlTable.EventArgs));
			Refresh();
		}

		private void RemoveFaction(long factionId)
		{
			if (m_tableFactions != null)
			{
				m_tableFactions.Remove((MyGuiControlTable.Row row) => ((MyFaction)row.UserData).FactionId == factionId);
			}
		}

		private void AddFaction(IMyFaction faction, Color? color = null, MyGuiHighlightTexture? icon = null, string iconToolTip = null, bool setAnonymous = false)
		{
			if (m_tableFactions == null)
			{
				return;
			}
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(faction);
			StringBuilder stringBuilder = setAnonymous ? MyTexts.Get(MySpaceTexts.Terminal_Factions_Unknown_Tag) : new StringBuilder(faction.Tag);
			StringBuilder obj = setAnonymous ? MyTexts.Get(MySpaceTexts.Terminal_Factions_Unknown_Label) : new StringBuilder(MyStatControlText.SubstituteTexts(faction.Name));
			Color? textColor = color;
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(stringBuilder, stringBuilder, stringBuilder.ToString(), textColor);
			row.AddCell(cell);
			string toolTip = string.Empty;
			if (setAnonymous)
			{
				toolTip = MyTexts.GetString(MySpaceTexts.Terminal_Factions_Unknown_Label_TTIP);
			}
			else if (MySession.Static.Factions.IsNpcFaction(faction.Tag))
			{
				MySessionComponentEconomy component = MySession.Static.GetComponent<MySessionComponentEconomy>();
				if (component != null)
				{
					toolTip = component.GetFactionFriendTooltip(faction.FactionId);
				}
			}
			else
			{
				toolTip = MyStatControlText.SubstituteTexts(faction.Name);
			}
			textColor = color;
			MyGuiControlTable.Cell cell2 = new MyGuiControlTable.Cell(obj, obj, toolTip, textColor);
			row.AddCell(cell2);
			StringBuilder text = new StringBuilder();
			MyGuiHighlightTexture? icon2 = icon;
			MyGuiControlTable.Cell cell3 = new MyGuiControlTable.Cell(text, null, iconToolTip, null, icon2);
			row.AddCell(cell3);
			m_tableFactions.Add(row);
		}

		private void OnMemberStateChanged(MyFactionStateChange action, IMyFaction fromFaction, long playerId)
		{
			MyIdentity myIdentity = Sync.Players.TryGetIdentity(playerId);
			if (myIdentity == null)
			{
				MyLog.Default.WriteLine("ERROR: Faction " + MyStatControlText.SubstituteTexts(fromFaction.Name) + " member " + playerId + " does not exists! ");
				return;
			}
			RemoveMember(playerId);
			switch (action)
			{
			case MyFactionStateChange.FactionMemberPromote:
				AddMember(playerId, myIdentity.DisplayName, isLeader: true, MyMemberComparerEnum.Leader, MyCommonTexts.Leader);
				break;
			case MyFactionStateChange.FactionMemberAcceptJoin:
			case MyFactionStateChange.FactionMemberDemote:
				AddMember(playerId, myIdentity.DisplayName, isLeader: false, MyMemberComparerEnum.Member, MyCommonTexts.Member);
				break;
			case MyFactionStateChange.FactionMemberSendJoin:
				AddMember(playerId, myIdentity.DisplayName, isLeader: false, MyMemberComparerEnum.Applicant, MyCommonTexts.Applicant, COLOR_CUSTOM_GREY);
				break;
			}
			RefreshUserInfo();
			RefreshTableFactions((MyFactionFilter)m_tableFactionsFilter.GetSelectedKey());
			m_tableMembers.Sort(switchSort: false);
		}

		private void RemoveMember(long playerId)
		{
			m_tableMembers.Remove(delegate(MyGuiControlTable.Row row)
			{
				if (row == null)
				{
					return false;
				}
				MyFactionMember? myFactionMember = row.UserData as MyFactionMember?;
				return myFactionMember.HasValue && myFactionMember.Value.PlayerId == playerId;
			});
		}

		private void AddMember(long playerId, string playerName, bool isLeader, MyMemberComparerEnum status, MyStringId textEnum, Color? color = null)
		{
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(new MyFactionMember(playerId, isLeader));
			StringBuilder text = new StringBuilder(playerName);
			string toolTip = playerName;
			row.AddCell(new MyGuiControlTable.Cell(text, playerId, toolTip, color));
			row.AddCell(new MyGuiControlTable.Cell(MyTexts.Get(textEnum), toolTip: MyTexts.GetString(textEnum), userData: status, textColor: color));
			m_tableMembers.Add(row);
		}

		private void ShowErrorBox(StringBuilder text)
		{
			StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
			MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, text, messageCaption);
			myGuiScreenMessageBox.SkipTransition = true;
			myGuiScreenMessageBox.CloseBeforeCallback = true;
			myGuiScreenMessageBox.CanHideOthers = false;
			MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
		}

		private void ShowConfirmBox(StringBuilder text, Action callback)
		{
			StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
			MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, text, messageCaption, null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum retval)
			{
				if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					callback();
				}
			}, 0, MyGuiScreenMessageBox.ResultEnum.NO);
			myGuiScreenMessageBox.SkipTransition = true;
			myGuiScreenMessageBox.CloseBeforeCallback = true;
			myGuiScreenMessageBox.CanHideOthers = false;
			MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
		}
	}
}
