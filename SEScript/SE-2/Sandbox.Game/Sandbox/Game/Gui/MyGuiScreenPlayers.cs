using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game.GameSystems.Trading;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.VoiceChat;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.GameServices;
using VRage.Input;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[StaticEventOwner]
	public class MyGuiScreenPlayers : MyGuiScreenBase
	{
		protected sealed class Promote_003C_003ESystem_UInt64_0023System_Boolean : ICallSite<IMyEventOwner, ulong, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in ulong playerId, in bool promote, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				Promote(playerId, promote);
			}
		}

		protected sealed class ShowPromoteMessage_003C_003EVRage_Game_ModAPI_MyPromoteLevel_0023System_Boolean : ICallSite<IMyEventOwner, MyPromoteLevel, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyPromoteLevel promoteLevel, in bool promote, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ShowPromoteMessage(promoteLevel, promote);
			}
		}

		protected sealed class RequestPingsAndRefresh_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestPingsAndRefresh();
			}
		}

		protected sealed class SendPingsAndRefresh_003C_003EVRage_Serialization_SerializableDictionary_00602_003CSystem_UInt64_0023System_Int16_003E : ICallSite<IMyEventOwner, SerializableDictionary<ulong, short>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in SerializableDictionary<ulong, short> dictionary, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SendPingsAndRefresh(dictionary);
			}
		}

		protected static readonly string OWNER_MARKER = "*****";

		protected int PlayerNameColumn;

		protected int PlayerFactionNameColumn = 1;

		protected int PlayerFactionTagColumn = 2;

		protected int PlayerMutedColumn = 3;

		protected int GameAdminColumn = 4;

		protected int GamePingColumn = 5;

		private bool m_getPingAndRefresh = true;

		protected MyGuiControlButton m_profileButton;

		protected MyGuiControlButton m_promoteButton;

		protected MyGuiControlButton m_demoteButton;

		protected MyGuiControlButton m_kickButton;

		protected MyGuiControlButton m_banButton;

		protected MyGuiControlButton m_inviteButton;

		protected MyGuiControlButton m_tradeButton;

		protected MyGuiControlLabel m_maxPlayersValueLabel;

		protected MyGuiControlTable m_playersTable;

		protected MyGuiControlCombobox m_lobbyTypeCombo;

		protected MyGuiControlSlider m_maxPlayersSlider;

		protected Dictionary<ulong, short> pings = new Dictionary<ulong, short>();

		protected ulong m_lastSelected;

		private MyGuiControlLabel m_caption;

		private bool m_waitingTradeResponse;

		public MyGuiScreenPlayers()
			: base(null, size: new Vector2(0.837f, 0.813f), backgroundColor: MyGuiConstants.SCREEN_BACKGROUND_COLOR, isTopMostScreen: false, backgroundTexture: MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture, backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			MyMultiplayer.Static.ClientJoined += Multiplayer_PlayerJoined;
			MyMultiplayer.Static.ClientLeft += Multiplayer_PlayerLeft;
			MySession.Static.Factions.FactionCreated += OnFactionCreated;
			MySession.Static.Factions.FactionEdited += OnFactionEdited;
			MySession.Static.Factions.FactionStateChanged += OnFactionStateChanged;
			MySession.Static.OnUserPromoteLevelChanged += OnUserPromoteLevelChanged;
			MyMultiplayerLobby myMultiplayerLobby = MyMultiplayer.Static as MyMultiplayerLobby;
			if (myMultiplayerLobby != null)
			{
				myMultiplayerLobby.OnLobbyDataUpdated += Matchmaking_LobbyDataUpdate;
			}
			MyMultiplayerLobbyClient myMultiplayerLobbyClient = MyMultiplayer.Static as MyMultiplayerLobbyClient;
			if (myMultiplayerLobbyClient != null)
			{
				myMultiplayerLobbyClient.OnLobbyDataUpdated += Matchmaking_LobbyDataUpdate;
			}
			if (MyPerGameSettings.EnableMutePlayer)
			{
				GameAdminColumn = 4;
			}
			RecreateControls(constructor: true);
			MyVoiceChatSessionComponent.Static.OnPlayerMutedStateChanged += OnPlayerMutedStateChanged;
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenPlayers";
		}

		protected override void OnClosed()
		{
			if (m_waitingTradeResponse)
			{
				MyTradingManager.Static.TradeCancel_Client();
				m_waitingTradeResponse = false;
			}
			base.OnClosed();
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ClientJoined -= Multiplayer_PlayerJoined;
				MyMultiplayer.Static.ClientLeft -= Multiplayer_PlayerLeft;
			}
			if (MySession.Static != null)
			{
				MySession.Static.Factions.FactionCreated -= OnFactionCreated;
				MySession.Static.Factions.FactionEdited -= OnFactionEdited;
				MySession.Static.Factions.FactionStateChanged -= OnFactionStateChanged;
				MySession.Static.OnUserPromoteLevelChanged -= OnUserPromoteLevelChanged;
			}
			MyMultiplayerLobby myMultiplayerLobby = MyMultiplayer.Static as MyMultiplayerLobby;
			if (myMultiplayerLobby != null)
			{
				myMultiplayerLobby.OnLobbyDataUpdated -= Matchmaking_LobbyDataUpdate;
			}
			MyMultiplayerLobbyClient myMultiplayerLobbyClient = MyMultiplayer.Static as MyMultiplayerLobbyClient;
			if (myMultiplayerLobbyClient != null)
			{
				myMultiplayerLobbyClient.OnLobbyDataUpdated -= Matchmaking_LobbyDataUpdate;
			}
			MyVoiceChatSessionComponent.Static.OnPlayerMutedStateChanged -= OnPlayerMutedStateChanged;
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			base.CloseButtonEnabled = true;
			Vector2 value = base.Size.Value / MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.SizeGui;
			_ = -0.5f * base.Size.Value + value * MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.PaddingSizeGui * 1.1f;
			m_caption = AddCaption(MyCommonTexts.ScreenCaptionPlayers, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.364f, -0.331f), 0.728f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.364f, 0.358f), 0.728f);
			myGuiControlSeparatorList.AddHorizontal(new Vector2(-0.36f, 0.05f), 0.17f);
			Controls.Add(myGuiControlSeparatorList);
			Vector2 vector = new Vector2(0f, 0.057f);
			Vector2 vector2 = new Vector2(-0.361f, -0.304f);
			m_profileButton = new MyGuiControlButton(vector2, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Profile));
			m_profileButton.ButtonClicked += profileButton_ButtonClicked;
			Controls.Add(m_profileButton);
			vector2 += vector;
			m_promoteButton = new MyGuiControlButton(vector2, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Promote));
			m_promoteButton.ButtonClicked += promoteButton_ButtonClicked;
			Controls.Add(m_promoteButton);
			vector2 += vector;
			m_demoteButton = new MyGuiControlButton(vector2, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Demote));
			m_demoteButton.ButtonClicked += demoteButton_ButtonClicked;
			Controls.Add(m_demoteButton);
			vector2 += vector;
			m_kickButton = new MyGuiControlButton(vector2, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Kick));
			m_kickButton.ButtonClicked += kickButton_ButtonClicked;
			Controls.Add(m_kickButton);
			vector2 += vector;
			m_banButton = new MyGuiControlButton(vector2, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Ban));
			m_banButton.ButtonClicked += banButton_ButtonClicked;
			Controls.Add(m_banButton);
			vector2 += vector;
			m_tradeButton = new MyGuiControlButton(vector2, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MySpaceTexts.PlayersScreen_TradeBtn));
			m_tradeButton.SetTooltip(MyTexts.GetString(MySpaceTexts.PlayersScreen_TradeBtn_TTP));
			m_tradeButton.ButtonClicked += tradeButton_ButtonClicked;
			Controls.Add(m_tradeButton);
			bool num = MyMultiplayer.Static != null && MyMultiplayer.Static.IsLobby;
			Vector2 vector3 = vector2 + new Vector2(-0.002f, m_tradeButton.Size.Y + 0.03f);
			MyGuiControlLabel control = new MyGuiControlLabel(vector3, null, MyTexts.GetString(MySpaceTexts.PlayersScreen_LobbyType), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			if (num)
			{
				Controls.Add(control);
			}
			vector3 += new Vector2(0f, 0.033f);
			m_lobbyTypeCombo = new MyGuiControlCombobox(vector3, null, null, null, 3);
			m_lobbyTypeCombo.Size = new Vector2(0.175f, 0.04f);
			m_lobbyTypeCombo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_lobbyTypeCombo.AddItem(0L, MyCommonTexts.ScreenPlayersLobby_Private);
			m_lobbyTypeCombo.AddItem(1L, MyCommonTexts.ScreenPlayersLobby_Friends);
			m_lobbyTypeCombo.AddItem(2L, MyCommonTexts.ScreenPlayersLobby_Public);
			m_lobbyTypeCombo.SelectItemByKey((long)MyMultiplayer.Static.GetLobbyType());
			if (num)
			{
				Controls.Add(m_lobbyTypeCombo);
			}
			Vector2 vector4 = vector3 + new Vector2(0f, 0.05f);
			MyGuiControlLabel control2 = new MyGuiControlLabel(vector4 + new Vector2(0.001f, 0f), null, MyTexts.GetString(MyCommonTexts.MaxPlayers) + ":", null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			if (num)
			{
				Controls.Add(control2);
			}
			m_maxPlayersValueLabel = new MyGuiControlLabel(vector4 + new Vector2(0.169f, 0f), null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			if (num)
			{
				Controls.Add(m_maxPlayersValueLabel);
			}
			vector4 += new Vector2(0f, 0.03f);
			m_maxPlayersSlider = new MyGuiControlSlider(vector4, 2f, MyMultiplayerLobby.MAX_PLAYERS, 0.177f, Sync.IsServer ? MySession.Static.MaxPlayers : MyMultiplayer.Static.MemberLimit, null, null, 1, 0.8f, 0f, "White", null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, intValue: true);
			m_maxPlayersValueLabel.Text = m_maxPlayersSlider.Value.ToString();
			m_maxPlayersSlider.ValueChanged = MaxPlayersSlider_Changed;
			if (num)
			{
				Controls.Add(m_maxPlayersSlider);
			}
			m_inviteButton = new MyGuiControlButton(new Vector2(-0.361f, 0.285000026f), MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, null, MyTexts.Get(MyCommonTexts.ScreenPlayers_Invite));
			m_inviteButton.ButtonClicked += inviteButton_ButtonClicked;
			Controls.Add(m_inviteButton);
			m_playersTable = new MyGuiControlTable
			{
				Position = new Vector2(0.364f, -0.307f),
				Size = new Vector2(0.54f, 0.813f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
				ColumnsCount = 6
			};
			m_playersTable.VisibleRowsCount = 18;
			float num2 = 0.3f;
			float num3 = 0.12f;
			float num4 = 0.12f;
			float num5 = 0.12f;
			float num6 = MyPerGameSettings.EnableMutePlayer ? 0.13f : 0f;
			m_playersTable.SetCustomColumnWidths(new float[6]
			{
				num2,
				1f - num2 - num3 - num4 - num6 - num5,
				num3,
				num6,
				num4,
				num5
			});
			m_playersTable.SetColumnComparison(PlayerNameColumn, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => a.Text.CompareToIgnoreCase(b.Text));
			m_playersTable.SetColumnName(PlayerNameColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_PlayerName));
			m_playersTable.SetColumnComparison(PlayerFactionNameColumn, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => a.Text.CompareToIgnoreCase(b.Text));
			m_playersTable.SetColumnName(PlayerFactionNameColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_FactionName));
			m_playersTable.SetColumnComparison(PlayerFactionTagColumn, (MyGuiControlTable.Cell a, MyGuiControlTable.Cell b) => a.Text.CompareToIgnoreCase(b.Text));
			m_playersTable.SetColumnName(PlayerFactionTagColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_FactionTag));
			if (MyPerGameSettings.EnableMutePlayer)
			{
				m_playersTable.SetColumnName(PlayerMutedColumn, new StringBuilder(MyTexts.GetString(MyCommonTexts.ScreenPlayers_Muted)));
			}
			m_playersTable.SetColumnComparison(GameAdminColumn, GameAdminCompare);
			m_playersTable.SetColumnName(GameAdminColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_Rank));
			m_playersTable.SetColumnComparison(GamePingColumn, GamePingCompare);
			m_playersTable.SetColumnName(GamePingColumn, MyTexts.Get(MyCommonTexts.ScreenPlayers_Ping));
			m_playersTable.ItemSelected += playersTable_ItemSelected;
			Controls.Add(m_playersTable);
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer.Id.SerialId == 0)
				{
					for (int i = 0; i < m_playersTable.RowsCount; i++)
					{
						MyGuiControlTable.Row row = m_playersTable.GetRow(i);
						if (row.UserData is ulong)
						{
							_ = (ulong)row.UserData;
							_ = onlinePlayer.Id.SteamId;
						}
					}
					AddPlayer(onlinePlayer.Id.SteamId);
				}
			}
			m_lobbyTypeCombo.ItemSelected += lobbyTypeCombo_OnSelect;
			if (m_lastSelected != 0L)
			{
				MyGuiControlTable.Row row2 = m_playersTable.Find((MyGuiControlTable.Row r) => (ulong)r.UserData == m_lastSelected);
				if (row2 != null)
				{
					m_playersTable.SelectedRow = row2;
				}
			}
			UpdateButtonsEnabledState();
			UpdateCaption();
		}

		private void profileButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null)
			{
				MyGameService.OpenOverlayUser((ulong)selectedRow.UserData);
			}
		}

		private void OnPlayerMutedStateChanged(ulong playerId, bool isMuted)
		{
			foreach (MyGuiControlBase control in m_playersTable.Controls)
			{
				MyGuiControlCheckbox myGuiControlCheckbox;
				object userData;
				if ((myGuiControlCheckbox = (control as MyGuiControlCheckbox)) != null && myGuiControlCheckbox.VisualStyle == MyGuiControlCheckboxStyleEnum.Muted && (userData = myGuiControlCheckbox.UserData) is ulong)
				{
					ulong num = (ulong)userData;
					if (num == playerId)
					{
						myGuiControlCheckbox.IsChecked = isMuted;
					}
				}
			}
		}

		private void tradeButton_ButtonClicked(MyGuiControlButton obj)
		{
			ulong num = (m_playersTable.SelectedRow != null) ? ((ulong)m_playersTable.SelectedRow.UserData) : 0;
			if (num != 0L)
			{
				m_waitingTradeResponse = true;
				MyTradingManager.Static.TradeRequest_Client(MyGameService.UserId, num, OnAnswerRecieved);
				if (m_waitingTradeResponse)
				{
					m_tradeButton.Enabled = false;
					m_tradeButton.Text = MyTexts.GetString(MySpaceTexts.PlayersScreen_TradeBtn_Waiting);
				}
			}
		}

		private void OnAnswerRecieved(MyTradeResponseReason reason)
		{
			m_waitingTradeResponse = false;
			UpdateTradeButton();
			m_tradeButton.Text = MyTexts.GetString(MySpaceTexts.PlayersScreen_TradeBtn);
		}

		private void TradeRequestOriginator_Response(ulong originId, ulong recieverId)
		{
			if (originId != MyGameService.UserId)
			{
				MyLog.Default.Error($"Server response for player trade request does not match the player. PlayerId: {originId} but OriginId (from server): {originId}");
				return;
			}
			MyPlayerTradeViewModel viewModel = new MyPlayerTradeViewModel(recieverId);
			ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(viewModel);
		}

		private int GameAdminCompare(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			ulong steamId = (ulong)a.Row.UserData;
			ulong steamId2 = (ulong)b.Row.UserData;
			int userPromoteLevel = (int)MySession.Static.GetUserPromoteLevel(steamId);
			int userPromoteLevel2 = (int)MySession.Static.GetUserPromoteLevel(steamId2);
			return userPromoteLevel.CompareTo(userPromoteLevel2);
		}

		private int GamePingCompare(MyGuiControlTable.Cell a, MyGuiControlTable.Cell b)
		{
			if (!int.TryParse(a.Text.ToString(), out int result))
			{
				result = -1;
			}
			if (!int.TryParse(b.Text.ToString(), out int result2))
			{
				result2 = -1;
			}
			return result.CompareTo(result2);
		}

		protected void IsMuteCheckedChanged(MyGuiControlCheckbox obj)
		{
			bool isChecked = obj.IsChecked;
			ulong playerId = (ulong)obj.UserData;
			MyVoiceChatSessionComponent.Static.SetPlayerMuted(playerId, isChecked);
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyInput.Static.IsNewKeyPressed(MyKeys.F3))
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudMouseClick);
				CloseScreen();
			}
		}

		protected void AddPlayer(ulong userId)
		{
			string memberName = MyMultiplayer.Static.GetMemberName(userId);
			if (string.IsNullOrEmpty(memberName))
			{
				return;
			}
			MyGuiControlTable.Row row = new MyGuiControlTable.Row(userId);
			row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(memberName), memberName));
			long playerId = Sync.Players.TryGetIdentityId(userId);
			MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(playerId);
			StringBuilder stringBuilder = new StringBuilder();
			if (playerFaction != null)
			{
				stringBuilder.Append(MyStatControlText.SubstituteTexts(playerFaction.Name));
				if (playerFaction.IsLeader(playerId))
				{
					stringBuilder.Append(" (").Append((object)MyTexts.Get(MyCommonTexts.Leader)).Append(")");
				}
			}
			row.AddCell(new MyGuiControlTable.Cell(stringBuilder));
			row.AddCell(new MyGuiControlTable.Cell(new StringBuilder((playerFaction != null) ? playerFaction.Tag : "")));
			MyGuiControlTable.Cell cell = new MyGuiControlTable.Cell(new StringBuilder(""));
			row.AddCell(cell);
			if (MyPerGameSettings.EnableMutePlayer && userId != Sync.MyId)
			{
				MyGuiControlCheckbox myGuiControlCheckbox = new MyGuiControlCheckbox(null, null, "", isChecked: false, MyGuiControlCheckboxStyleEnum.Muted);
				myGuiControlCheckbox.IsChecked = MySandboxGame.Config.MutedPlayers.Contains(userId);
				myGuiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(myGuiControlCheckbox.IsCheckedChanged, new Action<MyGuiControlCheckbox>(IsMuteCheckedChanged));
				myGuiControlCheckbox.UserData = userId;
				cell.Control = myGuiControlCheckbox;
				m_playersTable.Controls.Add(myGuiControlCheckbox);
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			MyPromoteLevel userPromoteLevel = MySession.Static.GetUserPromoteLevel(userId);
			for (int i = 0; i < (int)userPromoteLevel; i++)
			{
				stringBuilder2.Append("*");
			}
			row.AddCell(new MyGuiControlTable.Cell(stringBuilder2));
			if (pings.ContainsKey(userId))
			{
				row.AddCell(new MyGuiControlTable.Cell(new StringBuilder(pings[userId].ToString())));
			}
			else
			{
				row.AddCell(new MyGuiControlTable.Cell(new StringBuilder("----")));
			}
			m_playersTable.Add(row);
			UpdateCaption();
		}

		protected void RemovePlayer(ulong userId)
		{
			m_playersTable.Remove((MyGuiControlTable.Row row) => (ulong)row.UserData == userId);
			UpdateButtonsEnabledState();
			if (MySession.Static != null)
			{
				UpdateCaption();
			}
		}

		private void UpdateCaption()
		{
			string text = string.Empty;
			MyMultiplayerClient myMultiplayerClient = MyMultiplayer.Static as MyMultiplayerClient;
			if (myMultiplayerClient != null)
			{
				if (myMultiplayerClient.Server != null)
				{
					text = myMultiplayerClient.Server.Name;
				}
			}
			else
			{
				MyMultiplayerLobbyClient myMultiplayerLobbyClient = MyMultiplayer.Static as MyMultiplayerLobbyClient;
				if (myMultiplayerLobbyClient != null)
				{
					text = myMultiplayerLobbyClient.HostName;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				m_caption.Text = string.Concat(MyTexts.Get(MyCommonTexts.ScreenCaptionPlayers), " (", m_playersTable.RowsCount, " / ", MySession.Static.MaxPlayers, ")");
			}
			else
			{
				m_caption.Text = string.Concat(MyTexts.Get(MyCommonTexts.ScreenCaptionServerName), text, "  -  ", MyTexts.Get(MyCommonTexts.ScreenCaptionPlayers), " (", m_playersTable.RowsCount, " / ", MySession.Static.MaxPlayers, ")");
			}
		}

		protected void UpdateButtonsEnabledState()
		{
			if (MyMultiplayer.Static != null)
			{
				bool flag = m_playersTable.SelectedRow != null;
				ulong userId = MyGameService.UserId;
				ulong owner = MyMultiplayer.Static.GetOwner();
				ulong num = flag ? ((ulong)m_playersTable.SelectedRow.UserData) : 0;
				bool flag2 = userId == num;
				bool flag3 = MySession.Static.IsUserAdmin(userId);
				bool enabled = userId == owner;
				bool flag4 = flag && MySession.Static.CanPromoteUser(Sync.MyId, num);
				bool enabled2 = flag && MySession.Static.CanDemoteUser(Sync.MyId, num);
				MyLobbyType myLobbyType = (MyLobbyType)m_lobbyTypeCombo.GetSelectedKey();
				if (flag && !flag2)
				{
					m_promoteButton.Enabled = flag4;
					m_demoteButton.Enabled = enabled2;
					m_kickButton.Enabled = (flag4 && flag3);
					m_banButton.Enabled = (flag4 && flag3);
				}
				else
				{
					m_promoteButton.Enabled = false;
					m_demoteButton.Enabled = false;
					m_kickButton.Enabled = false;
					m_banButton.Enabled = false;
				}
				m_banButton.Enabled &= (MyMultiplayer.Static is MyMultiplayerClient);
				if (MyMultiplayer.Static.IsServer)
				{
					m_inviteButton.Enabled = (myLobbyType == MyLobbyType.Public || myLobbyType == MyLobbyType.FriendsOnly);
				}
				else
				{
					m_inviteButton.Enabled = (myLobbyType == MyLobbyType.Public);
				}
				m_lobbyTypeCombo.Enabled = enabled;
				m_maxPlayersSlider.Enabled = enabled;
				m_profileButton.Enabled = flag;
				UpdateTradeButton();
			}
		}

		private void UpdateTradeButton()
		{
			bool num = m_playersTable.SelectedRow != null;
			ulong userId = MyGameService.UserId;
			ulong num2 = num ? ((ulong)m_playersTable.SelectedRow.UserData) : 0;
			bool flag = userId == num2;
			bool flag2 = MyTradingManager.ValidateTradeProssible(userId, num2, out MyPlayer _, out MyPlayer _) == MyTradeResponseReason.Ok;
			flag2 = (!flag && flag2);
			m_tradeButton.Enabled = flag2;
		}

		protected void Multiplayer_PlayerJoined(ulong userId)
		{
			AddPlayer(userId);
		}

		protected void Multiplayer_PlayerLeft(ulong userId, MyChatMemberStateChangeEnum arg2)
		{
			RemovePlayer(userId);
		}

		protected void Matchmaking_LobbyDataUpdate(bool success, IMyLobby lobby, ulong memberOrLobby)
		{
			if (success)
			{
				ulong newOwnerId = lobby.OwnerId;
				MyGuiControlTable.Row row2 = m_playersTable.Find((MyGuiControlTable.Row row) => row.GetCell(GameAdminColumn).Text.Length == OWNER_MARKER.Length);
				MyGuiControlTable.Row row3 = m_playersTable.Find((MyGuiControlTable.Row row) => (ulong)row.UserData == newOwnerId);
				row2?.GetCell(GameAdminColumn).Text.Clear();
				row3?.GetCell(GameAdminColumn).Text.Clear().Append(OWNER_MARKER);
				MyLobbyType lobbyType = lobby.LobbyType;
				m_lobbyTypeCombo.SelectItemByKey((long)lobbyType, sendEvent: false);
				MySession.Static.Settings.OnlineMode = GetOnlineMode(lobbyType);
				UpdateButtonsEnabledState();
				if (!Sync.IsServer)
				{
					m_maxPlayersSlider.ValueChanged = null;
					MySession.Static.Settings.MaxPlayers = (short)MyMultiplayer.Static.MemberLimit;
					m_maxPlayersSlider.Value = MySession.Static.MaxPlayers;
					m_maxPlayersSlider.ValueChanged = MaxPlayersSlider_Changed;
					m_maxPlayersValueLabel.Text = m_maxPlayersSlider.Value.ToString();
					UpdateCaption();
				}
			}
		}

		protected MyOnlineModeEnum GetOnlineMode(MyLobbyType lobbyType)
		{
			switch (lobbyType)
			{
			case MyLobbyType.Private:
				return MyOnlineModeEnum.PRIVATE;
			case MyLobbyType.FriendsOnly:
				return MyOnlineModeEnum.FRIENDS;
			case MyLobbyType.Public:
				return MyOnlineModeEnum.PUBLIC;
			default:
				return MyOnlineModeEnum.PUBLIC;
			}
		}

		protected void playersTable_ItemSelected(MyGuiControlTable table, MyGuiControlTable.EventArgs args)
		{
			UpdateButtonsEnabledState();
			if (m_playersTable.SelectedRow != null)
			{
				m_lastSelected = (ulong)m_playersTable.SelectedRow.UserData;
			}
		}

		protected void inviteButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGameService.OpenInviteOverlay();
		}

		protected void promoteButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null && MySession.Static.CanPromoteUser(Sync.MyId, (ulong)selectedRow.UserData))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => Promote, (ulong)selectedRow.UserData, arg3: true);
			}
		}

		protected void demoteButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null && MySession.Static.CanDemoteUser(Sync.MyId, (ulong)selectedRow.UserData))
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => Promote, (ulong)selectedRow.UserData, arg3: false);
			}
		}

		[Event(null, 697)]
		[Reliable]
		[Server]
		public static void Promote(ulong playerId, bool promote)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && ((promote && !MySession.Static.CanPromoteUser(MyEventContext.Current.Sender.Value, playerId)) || (!promote && !MySession.Static.CanDemoteUser(MyEventContext.Current.Sender.Value, playerId))))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, kick: false);
			}
			else
			{
				PromoteImplementation(playerId, promote);
			}
		}

		public static void PromoteImplementation(ulong playerId, bool promote)
		{
			MyPromoteLevel userPromoteLevel = MySession.Static.GetUserPromoteLevel(playerId);
			if (promote)
			{
				if (userPromoteLevel >= MyPromoteLevel.Admin)
				{
					return;
				}
				userPromoteLevel++;
				if (!MySession.Static.EnableScripterRole && userPromoteLevel == MyPromoteLevel.Scripter)
				{
					userPromoteLevel++;
				}
			}
			else
			{
				if (userPromoteLevel == MyPromoteLevel.None)
				{
					return;
				}
				userPromoteLevel--;
				if (!MySession.Static.EnableScripterRole && userPromoteLevel == MyPromoteLevel.Scripter)
				{
					userPromoteLevel--;
				}
			}
			MySession.Static.SetUserPromoteLevel(playerId, userPromoteLevel);
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => ShowPromoteMessage, userPromoteLevel, promote, new EndpointId(playerId));
			RefreshPlusPings();
		}

		[Event(null, 737)]
		[Reliable]
		[Client]
		protected static void ShowPromoteMessage(MyPromoteLevel promoteLevel, bool promote)
		{
			ClearPromoteNotificaions();
			switch (promoteLevel)
			{
			case MyPromoteLevel.Owner:
				break;
			case MyPromoteLevel.None:
				MyHud.Notifications.Add(MyNotificationSingletons.PlayerDemotedNone);
				break;
			case MyPromoteLevel.Scripter:
				MyHud.Notifications.Add(promote ? MyNotificationSingletons.PlayerPromotedScripter : MyNotificationSingletons.PlayerDemotedScripter);
				break;
			case MyPromoteLevel.Moderator:
				MyHud.Notifications.Add(promote ? MyNotificationSingletons.PlayerPromotedModerator : MyNotificationSingletons.PlayerDemotedModerator);
				break;
			case MyPromoteLevel.SpaceMaster:
				MyHud.Notifications.Add(promote ? MyNotificationSingletons.PlayerPromotedSpaceMaster : MyNotificationSingletons.PlayerDemotedSpaceMaster);
				break;
			case MyPromoteLevel.Admin:
				MyHud.Notifications.Add(MyNotificationSingletons.PlayerPromotedAdmin);
				break;
			default:
				throw new ArgumentOutOfRangeException("promoteLevel", promoteLevel, null);
			}
		}

		private static void ClearPromoteNotificaions()
		{
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedNone);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedScripter);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedModerator);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerDemotedSpaceMaster);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedScripter);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedModerator);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedSpaceMaster);
			MyHud.Notifications.Remove(MyNotificationSingletons.PlayerPromotedAdmin);
		}

		protected static void Refresh()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyScreenManager.GetFirstScreenOfType<MyGuiScreenPlayers>()?.RecreateControls(constructor: false);
			}
		}

		protected void kickButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null)
			{
				MyMultiplayer.Static.KickClient((ulong)selectedRow.UserData);
			}
		}

		protected void banButton_ButtonClicked(MyGuiControlButton obj)
		{
			MyGuiControlTable.Row selectedRow = m_playersTable.SelectedRow;
			if (selectedRow != null)
			{
				MyMultiplayer.Static.BanClient((ulong)selectedRow.UserData, banned: true);
			}
		}

		protected void lobbyTypeCombo_OnSelect()
		{
			MyLobbyType lobbyType = (MyLobbyType)m_lobbyTypeCombo.GetSelectedKey();
			m_lobbyTypeCombo.SelectItemByKey((long)MyMultiplayer.Static.GetLobbyType(), sendEvent: false);
			MyMultiplayer.Static.SetLobbyType(lobbyType);
		}

		protected void MaxPlayersSlider_Changed(MyGuiControlSlider control)
		{
			MySession.Static.Settings.MaxPlayers = (short)m_maxPlayersSlider.Value;
			MyMultiplayer.Static.SetMemberLimit(MySession.Static.MaxPlayers);
			m_maxPlayersValueLabel.Text = m_maxPlayersSlider.Value.ToString();
			UpdateCaption();
		}

		private void OnFactionCreated(long insertedId)
		{
			RefreshPlusPings();
		}

		private void OnFactionEdited(long editedId)
		{
			RefreshPlusPings();
		}

		private void OnFactionStateChanged(MyFactionStateChange action, long fromFactionId, long toFactionId, long playerId, long senderId)
		{
			RefreshPlusPings();
		}

		private void OnUserPromoteLevelChanged(ulong steamId, MyPromoteLevel promotionLevel)
		{
			RefreshPlusPings();
		}

		public static void RefreshPlusPings()
		{
			if (Sync.IsServer)
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null)
				{
					MyReplicationServer myReplicationServer = MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
					if (myReplicationServer != null)
					{
						myReplicationServer.GetClientPings(out SerializableDictionary<ulong, short> dictionary);
						SendPingsAndRefresh(dictionary);
					}
				}
			}
			else
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => RequestPingsAndRefresh);
			}
		}

		[Event(null, 862)]
		[Reliable]
		[Server]
		public static void RequestPingsAndRefresh()
		{
			if (Sync.IsServer && MySession.Static != null)
			{
				MyReplicationServer myReplicationServer = MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
				if (myReplicationServer != null)
				{
					myReplicationServer.GetClientPings(out SerializableDictionary<ulong, short> arg);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => SendPingsAndRefresh, arg, new EndpointId(MyEventContext.Current.Sender.Value));
				}
			}
		}

		[Event(null, 879)]
		[Reliable]
		[Client]
		private static void SendPingsAndRefresh(SerializableDictionary<ulong, short> dictionary)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MyGuiScreenPlayers firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenPlayers>();
				if (firstScreenOfType != null)
				{
					firstScreenOfType.pings.Clear();
					foreach (KeyValuePair<ulong, short> item in dictionary.Dictionary)
					{
						firstScreenOfType.pings[item.Key] = item.Value;
					}
					firstScreenOfType.RecreateControls(constructor: false);
				}
			}
		}

		public override bool Draw()
		{
			bool result = base.Draw();
			if (m_getPingAndRefresh)
			{
				m_getPingAndRefresh = false;
				RefreshPlusPings();
			}
			return result;
		}
	}
}
