using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlGenericFunctionalBlock : MyGuiControlBase
	{
		private List<ITerminalControl> m_currentControls = new List<ITerminalControl>();

		private MyGuiControlSeparatorList m_separatorList;

		private MyGuiControlList m_terminalControlList;

		private MyGuiControlMultilineText m_blockPropertiesMultilineText;

		private MyTerminalBlock[] m_currentBlocks;

		private Dictionary<ITerminalControl, int> m_tmpControlDictionary = new Dictionary<ITerminalControl, int>(InstanceComparer<ITerminalControl>.Default);

		private bool m_recreatingControls;

		private MyGuiControlCombobox m_transferToCombobox;

		private MyGuiControlCombobox m_shareModeCombobox;

		private MyGuiControlLabel m_ownershipLabel;

		private MyGuiControlLabel m_ownerLabel;

		private MyGuiControlLabel m_transferToLabel;

		private MyGuiControlLabel m_shareWithLabel;

		private MyGuiControlButton m_npcButton;

		private bool m_isDetailedInfoBeingUpdated;

		private List<MyCubeGrid.MySingleOwnershipRequest> m_requests = new List<MyCubeGrid.MySingleOwnershipRequest>();

		private bool m_askForConfirmation = true;

		private bool m_canChangeShareMode = true;

		private MyScenarioBuildingBlock dummy = new MyScenarioBuildingBlock();

		internal MyGuiControlGenericFunctionalBlock(MyTerminalBlock block)
			: this(new MyTerminalBlock[1]
			{
				block
			})
		{
		}

		internal MyGuiControlGenericFunctionalBlock(MyTerminalBlock[] blocks)
			: base(null, null, null, null, null, isActiveControl: false, canHaveFocus: true)
		{
			m_currentBlocks = blocks;
			m_separatorList = new MyGuiControlSeparatorList();
			Elements.Add(m_separatorList);
			m_terminalControlList = new MyGuiControlList();
			m_terminalControlList.VisualStyle = MyGuiControlListStyleEnum.Simple;
			m_terminalControlList.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			m_terminalControlList.Position = new Vector2(0.1f, 0.1f);
			Elements.Add(m_terminalControlList);
			m_blockPropertiesMultilineText = new MyGuiControlMultilineText(new Vector2(0.05f, -0.195f), new Vector2(0.4f, 0.635f), null, "Blue", 0.85f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, drawScrollbarV: true, drawScrollbarH: true, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			m_blockPropertiesMultilineText.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_blockPropertiesMultilineText.Text = new StringBuilder();
			Elements.Add(m_blockPropertiesMultilineText);
			m_transferToCombobox = new MyGuiControlCombobox(Vector2.Zero, new Vector2(0.245f, 0.1f));
			m_transferToCombobox.ItemSelected += m_transferToCombobox_ItemSelected;
			m_transferToCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_TransferCombobox));
			m_transferToCombobox.ShowTooltipWhenDisabled = true;
			Elements.Add(m_transferToCombobox);
			m_shareModeCombobox = new MyGuiControlCombobox(Vector2.Zero, new Vector2(0.287f, 0.1f));
			m_shareModeCombobox.ItemSelected += m_shareModeCombobox_ItemSelected;
			m_shareModeCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_ShareCombobox));
			m_shareModeCombobox.ShowTooltipWhenDisabled = true;
			Elements.Add(m_shareModeCombobox);
			m_ownershipLabel = new MyGuiControlLabel(Vector2.Zero, null, MyTexts.GetString(MySpaceTexts.BlockOwner_Owner) + ":");
			Elements.Add(m_ownershipLabel);
			m_ownerLabel = new MyGuiControlLabel(Vector2.Zero, null, string.Empty);
			Elements.Add(m_ownerLabel);
			m_transferToLabel = new MyGuiControlLabel(Vector2.Zero, null, MyTexts.GetString(MySpaceTexts.BlockOwner_TransferTo));
			Elements.Add(m_transferToLabel);
			m_shareWithLabel = new MyGuiControlLabel(Vector2.Zero, null, MyTexts.GetString(MySpaceTexts.ControlScreen_ShareLabel));
			Elements.Add(m_shareWithLabel);
			m_npcButton = new MyGuiControlButton(new Vector2(0.27f, -0.13f), MyGuiControlButtonStyleEnum.Rectangular, new Vector2(0.04f, 0.053f), null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, MyTexts.GetString(MyCommonTexts.AddNewNPC), new StringBuilder("+"), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_ACTIVE, OnNewNpcClick, GuiSounds.MouseClick, 0.75f);
			Elements.Add(m_npcButton);
			m_npcButton.Enabled = false;
			m_npcButton.Enabled = MySession.Static.IsUserSpaceMaster(Sync.MyId);
			RecreateBlockControls();
			RecreateOwnershipControls();
			if (m_currentBlocks.Length != 0)
			{
				m_currentBlocks[0].PropertiesChanged += block_PropertiesChanged;
				m_currentBlocks[0].IsOpenedInTerminal = true;
			}
			MyTerminalBlock[] currentBlocks = m_currentBlocks;
			foreach (MyTerminalBlock obj in currentBlocks)
			{
				obj.OwnershipChanged += block_OwnershipChanged;
				obj.VisibilityChanged += block_VisibilityChanged;
			}
			Sync.Players.IdentitiesChanged += Players_IdentitiesChanged;
			UpdateDetailedInfo();
			base.Size = new Vector2(0.595f, 0.64f);
			base.CanFocusChildren = true;
		}

		private void Players_IdentitiesChanged()
		{
			UpdateOwnerGui();
		}

		private void block_OwnershipChanged(MyTerminalBlock sender)
		{
			if (m_canChangeShareMode)
			{
				RecreateOwnershipControls();
				UpdateOwnerGui();
			}
		}

		public override void OnRemoving()
		{
			m_currentControls.Clear();
			if (m_currentBlocks.Length != 0)
			{
				m_currentBlocks[0].PropertiesChanged -= block_PropertiesChanged;
				m_currentBlocks[0].IsOpenedInTerminal = false;
			}
			MyTerminalBlock[] currentBlocks = m_currentBlocks;
			foreach (MyTerminalBlock obj in currentBlocks)
			{
				obj.OwnershipChanged -= block_OwnershipChanged;
				obj.VisibilityChanged -= block_VisibilityChanged;
			}
			Sync.Players.IdentitiesChanged -= Players_IdentitiesChanged;
			base.OnRemoving();
		}

		private void block_VisibilityChanged(MyTerminalBlock obj)
		{
			foreach (ITerminalControl currentControl in m_currentControls)
			{
				if (currentControl.GetGuiControl().Visible != currentControl.IsVisible(obj))
				{
					currentControl.GetGuiControl().Visible = currentControl.IsVisible(obj);
				}
			}
		}

		private void block_PropertiesChanged(MyTerminalBlock sender)
		{
			if (m_canChangeShareMode)
			{
				MyScrollbar scrollBar = m_terminalControlList.GetScrollBar();
				float value = scrollBar.Value;
				RecreateBlockControls();
				foreach (ITerminalControl currentControl in m_currentControls)
				{
					currentControl.UpdateVisual();
				}
				scrollBar.Value = MathHelper.Min(value, scrollBar.MaxSize);
				UpdateDetailedInfo();
			}
		}

		private void UpdateDetailedInfo()
		{
			if (m_isDetailedInfoBeingUpdated)
			{
				return;
			}
			m_isDetailedInfoBeingUpdated = true;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			if (m_currentBlocks.Length == 1)
			{
				MyTerminalBlock myTerminalBlock = m_currentBlocks[0];
				stringBuilder.AppendStringBuilder(myTerminalBlock.DetailedInfo);
				if (myTerminalBlock.CustomInfo.Length > 0)
				{
					stringBuilder.TrimTrailingWhitespace().AppendLine();
					stringBuilder.AppendStringBuilder(myTerminalBlock.CustomInfo);
				}
				stringBuilder.Autowrap(0.29f, "Blue", 0.8f * MyGuiManager.LanguageTextScale);
				flag = true;
			}
			m_blockPropertiesMultilineText.Text.Clear();
			if (flag)
			{
				m_blockPropertiesMultilineText.Text = stringBuilder;
				m_blockPropertiesMultilineText.RefreshText(useEnum: false);
			}
			m_isDetailedInfoBeingUpdated = false;
		}

		private void RecreateBlockControls()
		{
			if (m_recreatingControls)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < m_terminalControlList.Controls.Count; i++)
			{
				if (m_terminalControlList.Controls[i].HasFocus)
				{
					num = i;
				}
			}
			m_currentControls.Clear();
			m_terminalControlList.Controls.Clear();
			try
			{
				m_recreatingControls = true;
				MyTerminalBlock[] currentBlocks = m_currentBlocks;
				foreach (MyTerminalBlock myTerminalBlock in currentBlocks)
				{
					myTerminalBlock.GetType();
					foreach (ITerminalControl control in MyTerminalControls.Static.GetControls(myTerminalBlock))
					{
						if (control != null)
						{
							m_tmpControlDictionary.TryGetValue(control, out int value);
							m_tmpControlDictionary[control] = value + (control.IsVisible(myTerminalBlock) ? 1 : 0);
						}
					}
				}
				if (MySession.Static.Settings.ScenarioEditMode && MyFakes.ENABLE_NEW_TRIGGERS)
				{
					foreach (ITerminalControl control2 in MyTerminalControlFactory.GetControls(typeof(MyTerminalBlock)))
					{
						m_tmpControlDictionary[control2] = m_currentBlocks.Length;
					}
				}
				int num2 = m_currentBlocks.Length;
				foreach (KeyValuePair<ITerminalControl, int> item in m_tmpControlDictionary)
				{
					bool visible = item.Value != 0;
					if ((num2 <= 1 || item.Key.SupportsMultipleBlocks) && item.Value == num2 && item.Key.GetGuiControl() != null)
					{
						item.Key.GetGuiControl().Visible = visible;
						m_terminalControlList.Controls.Add(item.Key.GetGuiControl());
						item.Key.TargetBlocks = m_currentBlocks;
						item.Key.UpdateVisual();
						m_currentControls.Add(item.Key);
					}
				}
			}
			finally
			{
				if (num != -1)
				{
					if (num >= m_terminalControlList.Controls.Count)
					{
						num = m_terminalControlList.Controls.Count - 1;
					}
					GetTopMostOwnerScreen().FocusedControl = m_terminalControlList.Controls[num];
				}
				m_tmpControlDictionary.Clear();
				m_recreatingControls = false;
			}
		}

		private void RecreateOwnershipControls()
		{
			bool flag = false;
			MyTerminalBlock[] currentBlocks = m_currentBlocks;
			for (int i = 0; i < currentBlocks.Length; i++)
			{
				if (currentBlocks[i].IDModule != null)
				{
					flag = true;
				}
			}
			if (flag && MyFakes.SHOW_FACTIONS_GUI)
			{
				m_ownershipLabel.Visible = true;
				m_ownerLabel.Visible = true;
				m_transferToLabel.Visible = true;
				m_shareWithLabel.Visible = true;
				m_transferToCombobox.Visible = true;
				m_shareModeCombobox.Visible = true;
				if (m_npcButton != null)
				{
					m_npcButton.Visible = true;
				}
				Vector2 value = Vector2.One * -0.5f;
				Vector2 vector = new Vector2(0.3f, 0.55f);
				m_ownershipLabel.Position = value + new Vector2(vector.X + 0.212f, 0.315f);
				m_ownerLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_ownerLabel.Position = m_ownershipLabel.Position + new Vector2(m_ownershipLabel.Size.X + 0.015f, 0f);
				m_transferToLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_transferToLabel.Position = value + new Vector2(vector.X + 0.212f, 0.335f);
				m_transferToCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_transferToCombobox.Position = value + new Vector2(vector.X + 0.212f, 0.368f);
				m_shareWithLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_shareWithLabel.Position = value + new Vector2(vector.X + 0.212f, 0.42f);
				m_shareModeCombobox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_shareModeCombobox.Position = value + new Vector2(vector.X + 0.212f, 0.45f);
				m_shareModeCombobox.ClearItems();
				m_shareModeCombobox.AddItem(0L, MyTexts.Get(MySpaceTexts.BlockOwner_ShareNone));
				m_shareModeCombobox.AddItem(1L, MyTexts.Get(MySpaceTexts.BlockOwner_ShareFaction));
				m_shareModeCombobox.AddItem(2L, MyTexts.Get(MySpaceTexts.BlockOwner_ShareAll));
				UpdateOwnerGui();
			}
			else
			{
				m_ownershipLabel.Visible = false;
				m_ownerLabel.Visible = false;
				m_transferToLabel.Visible = false;
				m_shareWithLabel.Visible = false;
				m_transferToCombobox.Visible = false;
				m_shareModeCombobox.Visible = false;
				if (m_npcButton != null)
				{
					m_npcButton.Visible = false;
				}
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			base.HandleInput();
			return HandleInputElements();
		}

		protected override void OnSizeChanged()
		{
			if (m_currentBlocks.Length == 0)
			{
				return;
			}
			Vector2 value = base.Size * -0.5f;
			Vector2 vector = new Vector2(0.3f, 0.55f);
			m_separatorList.Clear();
			m_separatorList.AddHorizontal(value + new Vector2(vector.X + 0.008f, 0.11f), vector.X * 0.96f);
			m_terminalControlList.Position = value + new Vector2(vector.X * 0.5f - 0.006f, -0.032f);
			m_terminalControlList.Size = new Vector2(vector.X - 0.013f, 0.675f);
			float num = 0.06f;
			if (MyFakes.SHOW_FACTIONS_GUI)
			{
				MyTerminalBlock[] currentBlocks = m_currentBlocks;
				for (int i = 0; i < currentBlocks.Length; i++)
				{
					if (currentBlocks[i].IDModule != null)
					{
						num = 0.22f;
						m_separatorList.AddHorizontal(value + new Vector2(vector.X + 0.008f, num + 0.11f), vector.X * 0.96f);
						break;
					}
				}
			}
			m_blockPropertiesMultilineText.Position = value + new Vector2(vector.X + 0.012f, num + 0.133f);
			m_blockPropertiesMultilineText.Size = 0.5f * base.Size - m_blockPropertiesMultilineText.Position;
			base.OnSizeChanged();
		}

		private void m_shareModeCombobox_ItemSelected()
		{
			if (!m_canChangeShareMode)
			{
				return;
			}
			m_canChangeShareMode = false;
			bool flag = false;
			MyOwnershipShareModeEnum myOwnershipShareModeEnum = (MyOwnershipShareModeEnum)m_shareModeCombobox.GetSelectedKey();
			if (m_currentBlocks.Length != 0)
			{
				m_requests.Clear();
				MyTerminalBlock[] currentBlocks = m_currentBlocks;
				foreach (MyTerminalBlock myTerminalBlock in currentBlocks)
				{
					if (myTerminalBlock.IDModule != null && myOwnershipShareModeEnum >= MyOwnershipShareModeEnum.None && (myTerminalBlock.OwnerId == MySession.Static.LocalPlayerId || MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals)))
					{
						m_requests.Add(new MyCubeGrid.MySingleOwnershipRequest
						{
							BlockId = myTerminalBlock.EntityId,
							Owner = myTerminalBlock.IDModule.Owner
						});
						flag = true;
					}
				}
				if (m_requests.Count > 0)
				{
					MyCubeGrid.ChangeOwnersRequest(myOwnershipShareModeEnum, m_requests, MySession.Static.LocalPlayerId);
				}
			}
			m_canChangeShareMode = true;
			if (flag)
			{
				block_PropertiesChanged(null);
			}
		}

		private void m_transferToCombobox_ItemSelected()
		{
			if (m_transferToCombobox.GetSelectedIndex() != -1)
			{
				if (m_askForConfirmation)
				{
					long ownerKey = m_transferToCombobox.GetSelectedKey();
					int selectedIndex = m_transferToCombobox.GetSelectedIndex();
					StringBuilder value = m_transferToCombobox.GetItemByIndex(selectedIndex).Value;
					MyGuiScreenMessageBox obj = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextChangeOwner), value.ToString()), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum retval)
					{
						if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							if (m_currentBlocks.Length != 0)
							{
								m_requests.Clear();
								bool flag = MySession.Static.IsUserAdmin(Sync.MyId) && MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals);
								MyTerminalBlock[] currentBlocks = m_currentBlocks;
								foreach (MyTerminalBlock myTerminalBlock in currentBlocks)
								{
									if (myTerminalBlock.IDModule != null && (myTerminalBlock.OwnerId == 0 || flag || myTerminalBlock.OwnerId == MySession.Static.LocalPlayerId))
									{
										m_requests.Add(new MyCubeGrid.MySingleOwnershipRequest
										{
											BlockId = myTerminalBlock.EntityId,
											Owner = ownerKey
										});
									}
								}
								if (m_requests.Count > 0)
								{
									if (flag && Sync.Players.IdentityIsNpc(ownerKey))
									{
										MyCubeGrid.ChangeOwnersRequest(MyOwnershipShareModeEnum.Faction, m_requests, MySession.Static.LocalPlayerId);
									}
									else if (MySession.Static.LocalPlayerId == ownerKey)
									{
										MyCubeGrid.ChangeOwnersRequest(MyOwnershipShareModeEnum.Faction, m_requests, MySession.Static.LocalPlayerId);
									}
									else
									{
										MyCubeGrid.ChangeOwnersRequest(MyOwnershipShareModeEnum.None, m_requests, MySession.Static.LocalPlayerId);
									}
								}
							}
							RecreateOwnershipControls();
							UpdateOwnerGui();
						}
						else
						{
							m_askForConfirmation = false;
							m_transferToCombobox.SelectItemByIndex(-1);
							m_askForConfirmation = true;
						}
					}, timeoutInMiliseconds: 0, focusedResult: MyGuiScreenMessageBox.ResultEnum.NO);
					obj.CanHideOthers = false;
					MyGuiSandbox.AddScreen(obj);
				}
				else
				{
					UpdateOwnerGui();
				}
			}
		}

		private void UpdateOwnerGui()
		{
			bool ownershipStatus = GetOwnershipStatus(out long? owner);
			m_transferToCombobox.ClearItems();
			if (!ownershipStatus && !owner.HasValue)
			{
				return;
			}
			if (ownershipStatus || owner.Value != 0L)
			{
				m_transferToCombobox.AddItem(0L, MyTexts.Get(MySpaceTexts.BlockOwner_Nobody));
			}
			if (ownershipStatus || owner.Value != MySession.Static.LocalPlayerId)
			{
				m_transferToCombobox.AddItem(MySession.Static.LocalPlayerId, MyTexts.Get(MySpaceTexts.BlockOwner_Me));
			}
			if (MySession.Static.IsUserAdmin(Sync.MyId))
			{
				foreach (KeyValuePair<long, MyIdentity> item in MySession.Static.Players.GetAllIdentitiesOrderByName())
				{
					if (item.Value.IdentityId != MySession.Static.LocalPlayerId && !MySession.Static.Players.IdentityIsNpc(item.Value.IdentityId) && (MySession.Static.LocalHumanPlayer.GetRelationTo(item.Value.IdentityId) != MyRelationsBetweenPlayerAndBlock.Enemies || MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
					{
						m_transferToCombobox.AddItem(item.Value.IdentityId, new StringBuilder(item.Value.DisplayName));
					}
				}
			}
			else
			{
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					MyIdentity identity = onlinePlayer.Identity;
					if (identity.IdentityId != MySession.Static.LocalPlayerId && !identity.IsDead && (MySession.Static.LocalHumanPlayer.GetRelationTo(identity.IdentityId) != MyRelationsBetweenPlayerAndBlock.Enemies || MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId)))
					{
						m_transferToCombobox.AddItem(identity.IdentityId, new StringBuilder(identity.DisplayName));
					}
				}
			}
			foreach (long nPCIdentity in Sync.Players.GetNPCIdentities())
			{
				MyIdentity myIdentity = Sync.Players.TryGetIdentity(nPCIdentity);
				if (myIdentity != null)
				{
					MySession.Static.LocalHumanPlayer.GetRelationTo(myIdentity.IdentityId);
					if (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId))
					{
						m_transferToCombobox.AddItem(myIdentity.IdentityId, new StringBuilder(myIdentity.DisplayName));
					}
				}
			}
			if (!ownershipStatus)
			{
				bool flag = MySession.Static.IsUserAdmin(Sync.MyId) && MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals);
				if (owner.Value == MySession.Static.LocalPlayerId || flag)
				{
					m_shareModeCombobox.Enabled = true;
					m_shareModeCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_ShareCombobox));
				}
				else
				{
					m_shareModeCombobox.Enabled = false;
					m_shareModeCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_ShareComboboxDisabled));
				}
				if (owner.Value == 0L)
				{
					m_transferToCombobox.Enabled = true;
					m_npcButton.Enabled = (m_transferToCombobox.Enabled && MySession.Static.IsUserSpaceMaster(Sync.MyId));
					m_ownerLabel.TextEnum = MySpaceTexts.BlockOwner_Nobody;
				}
				else
				{
					m_transferToCombobox.Enabled = (owner.Value == MySession.Static.LocalPlayerId || flag);
					m_npcButton.Enabled = (m_transferToCombobox.Enabled && MySession.Static.IsUserSpaceMaster(Sync.MyId));
					m_ownerLabel.TextEnum = MySpaceTexts.BlockOwner_Me;
					if (owner.Value != MySession.Static.LocalPlayerId)
					{
						MyIdentity myIdentity2 = Sync.Players.TryGetIdentity(owner.Value);
						if (myIdentity2 != null)
						{
							m_ownerLabel.Text = myIdentity2.DisplayName + (myIdentity2.IsDead ? (" [" + MyTexts.Get(MyCommonTexts.PlayerInfo_Dead).ToString() + "]") : "");
						}
						else
						{
							m_ownerLabel.TextEnum = MySpaceTexts.BlockOwner_Unknown;
						}
					}
				}
				ownershipStatus = GetShareMode(out MyOwnershipShareModeEnum? shareMode);
				m_canChangeShareMode = false;
				if (!ownershipStatus && shareMode.HasValue && owner.Value != 0L)
				{
					m_shareModeCombobox.SelectItemByKey((long)shareMode.Value);
				}
				else
				{
					m_shareModeCombobox.SelectItemByIndex(-1);
				}
				m_canChangeShareMode = true;
			}
			else
			{
				m_shareModeCombobox.Enabled = true;
				m_shareModeCombobox.SetToolTip(MyTexts.GetString(MySpaceTexts.ControlScreen_ShareCombobox));
				m_ownerLabel.Text = "";
				m_canChangeShareMode = false;
				m_shareModeCombobox.SelectItemByIndex(-1);
				m_canChangeShareMode = true;
			}
		}

		private bool GetOwnershipStatus(out long? owner)
		{
			bool result = false;
			owner = null;
			MyTerminalBlock[] currentBlocks = m_currentBlocks;
			foreach (MyTerminalBlock myTerminalBlock in currentBlocks)
			{
				if (myTerminalBlock.IDModule != null)
				{
					if (!owner.HasValue)
					{
						owner = myTerminalBlock.IDModule.Owner;
					}
					else if (owner.Value != myTerminalBlock.IDModule.Owner)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private bool GetShareMode(out MyOwnershipShareModeEnum? shareMode)
		{
			bool result = false;
			shareMode = null;
			MyTerminalBlock[] currentBlocks = m_currentBlocks;
			foreach (MyTerminalBlock myTerminalBlock in currentBlocks)
			{
				if (myTerminalBlock.IDModule != null)
				{
					if (!shareMode.HasValue)
					{
						shareMode = myTerminalBlock.IDModule.ShareMode;
					}
					else if (shareMode.Value != myTerminalBlock.IDModule.ShareMode)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private void OnNewNpcClick(MyGuiControlButton button)
		{
			Sync.Players.RequestNewNpcIdentity();
		}
	}
}
