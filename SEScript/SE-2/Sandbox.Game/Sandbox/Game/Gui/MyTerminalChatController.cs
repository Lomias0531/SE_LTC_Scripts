using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game.ModAPI;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyTerminalChatController : MyTerminalController
	{
		private MyGuiControlListbox m_playerList;

		private MyGuiControlListbox m_factionList;

		private MyGuiControlListbox.Item m_chatBotItem;

		private MyGuiControlListbox.Item m_broadcastItem;

		private MyGuiControlListbox.Item m_globalItem;

		private MyGuiControlMultilineText m_chatHistory;

		private MyGuiControlTextbox m_chatbox;

		private MyGuiControlButton m_sendButton;

		private StringBuilder m_chatboxText = new StringBuilder();

		private StringBuilder m_tempStringBuilder = new StringBuilder();

		private bool m_closed = true;

		private bool m_pendingUpdatePlayerList;

		private bool m_waitedOneFrameBeforeUpdating;

		private int m_frameCount;

		private bool AllowPlayerDrivenChat => MyMultiplayer.Static?.IsChatAvailable ?? true;

		public void Init(IMyGuiControlsParent controlsParent)
		{
			m_playerList = (MyGuiControlListbox)controlsParent.Controls.GetControlByName("PlayerListbox");
			m_factionList = (MyGuiControlListbox)controlsParent.Controls.GetControlByName("FactionListbox");
			m_chatHistory = (MyGuiControlMultilineText)controlsParent.Controls.GetControlByName("ChatHistory");
			m_chatbox = (MyGuiControlTextbox)controlsParent.Controls.GetControlByName("Chatbox");
			m_chatbox.SetToolTip(MyTexts.GetString(MySpaceTexts.ChatScreen_TerminaMessageBox));
			m_playerList.ItemsSelected += m_playerList_ItemsSelected;
			m_playerList.MultiSelect = false;
			m_factionList.ItemsSelected += m_factionList_ItemsSelected;
			m_factionList.MultiSelect = false;
			m_sendButton = (MyGuiControlButton)controlsParent.Controls.GetControlByName("SendButton");
			m_sendButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ChatScreen_TerminalSendMessage));
			m_sendButton.ButtonClicked += m_sendButton_ButtonClicked;
			m_sendButton.ShowTooltipWhenDisabled = true;
			m_chatbox.TextChanged += m_chatbox_TextChanged;
			m_chatbox.EnterPressed += m_chatbox_EnterPressed;
			if (MySession.Static.LocalCharacter != null)
			{
				MySession.Static.ChatSystem.PlayerMessageReceived += MyChatSystem_PlayerMessageReceived;
				MySession.Static.ChatSystem.FactionMessageReceived += MyChatSystem_FactionMessageReceived;
			}
			MySession.Static.Players.PlayersChanged += Players_PlayersChanged;
			RefreshLists();
			m_chatbox.Text = string.Empty;
			m_sendButton.Enabled = false;
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ChatMessageReceived += Multiplayer_ChatMessageReceived;
			}
			m_closed = false;
		}

		private void m_chatbox_TextChanged(MyGuiControlTextbox obj)
		{
			m_chatboxText.Clear();
			obj.GetText(m_chatboxText);
			if (m_chatboxText.Length == 0)
			{
				m_sendButton.Enabled = false;
				m_sendButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ChatScreen_TerminalSendMessageDisabled));
				return;
			}
			if (MySession.Static.LocalCharacter != null)
			{
				m_sendButton.Enabled = true;
				m_sendButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ChatScreen_TerminalSendMessage));
			}
			if (m_chatboxText.Length > 200)
			{
				m_chatboxText.Length = 200;
				m_chatbox.SetText(m_chatboxText);
			}
		}

		private void m_chatbox_EnterPressed(MyGuiControlTextbox obj)
		{
			if (m_chatboxText.Length > 0)
			{
				SendMessage();
			}
		}

		private void m_sendButton_ButtonClicked(MyGuiControlButton obj)
		{
			SendMessage();
		}

		private void m_playerList_ItemsSelected(MyGuiControlListbox obj)
		{
			if (m_playerList.SelectedItems.Count > 0)
			{
				MyGuiControlListbox.Item item = m_playerList.SelectedItems[0];
				if (item == m_globalItem)
				{
					RefreshGlobalChatHistory();
				}
				else if (item == m_chatBotItem)
				{
					RefreshChatBotHistory();
				}
				else
				{
					MyIdentity playerIdentity = (MyIdentity)item.UserData;
					RefreshPlayerChatHistory(playerIdentity);
				}
				m_chatbox.Text = string.Empty;
				m_chatbox.Enabled = (AllowPlayerDrivenChat || item == m_chatBotItem);
			}
		}

		private void m_factionList_ItemsSelected(MyGuiControlListbox obj)
		{
			if (m_factionList.SelectedItems.Count > 0)
			{
				MyFaction faction = (MyFaction)m_factionList.SelectedItems[0].UserData;
				RefreshFactionChatHistory(faction);
				m_chatbox.Enabled = true;
				m_chatbox.Text = string.Empty;
			}
		}

		private void MyChatSystem_PlayerMessageReceived(long playerId)
		{
			if (m_playerList != null && m_playerList.SelectedItems != null && m_playerList.SelectedItems.Count > 0)
			{
				_ = (MyIdentity)m_playerList.SelectedItems[0].UserData;
			}
		}

		private void MyChatSystem_FactionMessageReceived(long factionId)
		{
			if (m_factionList.SelectedItems.Count > 0)
			{
				MyFaction myFaction = (MyFaction)m_factionList.SelectedItems[0].UserData;
				if (myFaction.FactionId == factionId)
				{
					RefreshFactionChatHistory(myFaction);
				}
			}
		}

		private void OnChatBotResponse(string text)
		{
			if (MySession.Static != null)
			{
				MyUnifiedChatItem item = MyUnifiedChatItem.CreateChatbotMessage(text, DateTime.UtcNow, 0L, MySession.Static.LocalPlayerId, MyTexts.GetString(MySpaceTexts.ChatBotName));
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(ref item);
				RefreshChatBotHistory();
			}
		}

		private void Multiplayer_ChatMessageReceived(ulong steamUserId, string messageText, ChatChannel channel, long targetId, string customAuthorName = null)
		{
			if (m_playerList.SelectedItems.Count > 0)
			{
				MyGuiControlListbox.Item item = m_playerList.SelectedItems[0];
				if (item == m_globalItem)
				{
					RefreshGlobalChatHistory();
					return;
				}
				if (item == m_chatBotItem)
				{
					RefreshChatBotHistory();
					return;
				}
				MyIdentity myIdentity = (MyIdentity)item.UserData;
				if (MySession.Static.Players.TryGetPlayerId(myIdentity.IdentityId, out MyPlayer.PlayerId result) && MySession.Static.LocalPlayerId == targetId && steamUserId == result.SteamId)
				{
					RefreshPlayerChatHistory(myIdentity);
				}
			}
			else if (m_factionList.SelectedItems.Count > 0)
			{
				MyFaction myFaction = (MyFaction)m_factionList.SelectedItems[0].UserData;
				if (myFaction.FactionId == targetId)
				{
					RefreshFactionChatHistory(myFaction);
				}
			}
		}

		private void Players_PlayersChanged(bool added, MyPlayer.PlayerId playerId)
		{
			if (!m_closed)
			{
				UpdatePlayerList();
			}
		}

		private void SendMessage()
		{
			if (MySession.Static.LocalCharacter == null)
			{
				return;
			}
			m_chatboxText.Clear();
			m_chatbox.GetText(m_chatboxText);
			if (m_playerList.SelectedItems.Count > 0)
			{
				MyGuiControlListbox.Item item = m_playerList.SelectedItems[0];
				if (AllowPlayerDrivenChat || item == m_chatBotItem)
				{
					if (item == m_globalItem)
					{
						if (MyMultiplayer.Static != null)
						{
							MyMultiplayer.Static.SendChatMessage(m_chatboxText.ToString(), ChatChannel.Global, 0L);
						}
						else if (MyGameService.IsActive)
						{
							MyHud.Chat.ShowMessageColoredSP(m_chatboxText.ToString(), ChatChannel.Global, 0L);
						}
						else
						{
							MyHud.Chat.ShowMessage(MySession.Static.LocalHumanPlayer?.DisplayName ?? "Player", m_chatboxText.ToString());
						}
						RefreshGlobalChatHistory();
					}
					else if (item == m_chatBotItem)
					{
						string text = m_chatboxText.ToString();
						MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(text, ChatChannel.ChatBot, MySession.Static.LocalPlayerId, -1L, DateTime.UtcNow);
						RefreshChatBotHistory();
						if (MySession.Static.ChatBot != null && !MySession.Static.ChatBot.FilterMessage(text, OnChatBotResponse))
						{
							MySession.Static.ChatBot.FilterMessage("? " + text, OnChatBotResponse);
						}
					}
					else
					{
						MyIdentity myIdentity = (MyIdentity)item.UserData;
						MyMultiplayer.Static.SendChatMessage(m_chatboxText.ToString(), ChatChannel.Private, myIdentity.IdentityId);
						RefreshPlayerChatHistory(myIdentity);
					}
				}
			}
			else if (m_factionList.SelectedItems.Count > 0)
			{
				MyFaction myFaction = (MyFaction)m_factionList.SelectedItems[0].UserData;
				if (!myFaction.IsMember(MySession.Static.LocalPlayerId))
				{
					return;
				}
				if (MyMultiplayer.Static != null)
				{
					MyMultiplayer.Static.SendChatMessage(m_chatboxText.ToString(), ChatChannel.Faction, myFaction.FactionId);
				}
				else if (MyGameService.IsActive)
				{
					MyHud.Chat.ShowMessageColoredSP(m_chatboxText.ToString(), ChatChannel.Faction, myFaction.FactionId);
				}
				else
				{
					MyHud.Chat.ShowMessage(MySession.Static.LocalHumanPlayer?.DisplayName ?? "Player", m_chatboxText.ToString());
				}
				RefreshFactionChatHistory(myFaction);
			}
			m_chatbox.Text = string.Empty;
		}

		private void RefreshPlayerChatHistory(MyIdentity playerIdentity)
		{
			m_chatHistory.Clear();
			List<MyUnifiedChatItem> list = new List<MyUnifiedChatItem>();
			MySession.Static.ChatSystem.ChatHistory.GetPrivateHistory(ref list, playerIdentity.IdentityId);
			foreach (MyUnifiedChatItem item in list)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(item.SenderId);
				if (myIdentity != null)
				{
					Color relationColor = MyChatSystem.GetRelationColor(item.SenderId);
					Color channelColor = MyChatSystem.GetChannelColor(item.Channel);
					m_chatHistory.AppendText(myIdentity.DisplayName, "White", m_chatHistory.TextScale, relationColor);
					m_chatHistory.AppendText(": ", "White", m_chatHistory.TextScale, relationColor);
					m_chatHistory.AppendText(item.Text, "White", m_chatHistory.TextScale, channelColor);
					m_chatHistory.AppendLine();
				}
			}
			m_factionList.SelectedItems.Clear();
			m_chatHistory.ScrollbarOffsetV = 1f;
		}

		private void RefreshFactionChatHistory(MyFaction faction)
		{
			m_chatHistory.Clear();
			if (MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId) != null || MySession.Static.IsUserAdmin(Sync.MyId))
			{
				List<MyUnifiedChatItem> list = new List<MyUnifiedChatItem>();
				MySession.Static.ChatSystem.ChatHistory.GetFactionHistory(ref list, faction.FactionId);
				foreach (MyUnifiedChatItem item in list)
				{
					MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(item.SenderId);
					if (myIdentity != null)
					{
						Color relationColor = MyChatSystem.GetRelationColor(item.SenderId);
						Color channelColor = MyChatSystem.GetChannelColor(item.Channel);
						m_chatHistory.AppendText(myIdentity.DisplayName, "White", m_chatHistory.TextScale, relationColor);
						m_chatHistory.AppendText(": ", "White", m_chatHistory.TextScale, relationColor);
						m_chatHistory.AppendText(item.Text, "White", m_chatHistory.TextScale, channelColor);
						m_chatHistory.AppendLine();
					}
				}
				m_playerList.SelectedItems.Clear();
				m_chatHistory.ScrollbarOffsetV = 1f;
			}
		}

		private void RefreshGlobalChatHistory()
		{
			m_chatHistory.Clear();
			List<MyUnifiedChatItem> list = new List<MyUnifiedChatItem>();
			if (AllowPlayerDrivenChat)
			{
				MySession.Static.ChatSystem.ChatHistory.GetGeneralHistory(ref list);
			}
			else
			{
				list.Add(new MyUnifiedChatItem
				{
					Channel = ChatChannel.GlobalScripted,
					Text = MyTexts.GetString(MyCommonTexts.ChatRestricted),
					AuthorFont = "White"
				});
			}
			foreach (MyUnifiedChatItem item in list)
			{
				if (item.Channel == ChatChannel.GlobalScripted)
				{
					Color relationColor = MyChatSystem.GetRelationColor(item.SenderId);
					Color channelColor = MyChatSystem.GetChannelColor(item.Channel);
					if (item.CustomAuthor.Length > 0)
					{
						m_chatHistory.AppendText(item.CustomAuthor + ": ", item.AuthorFont, m_chatHistory.TextScale, relationColor);
					}
					else
					{
						m_chatHistory.AppendText(MyTexts.GetString(MySpaceTexts.ChatBotName) + ": ", item.AuthorFont, m_chatHistory.TextScale, relationColor);
					}
					m_chatHistory.AppendText(item.Text, "White", m_chatHistory.TextScale, channelColor);
					m_chatHistory.AppendLine();
				}
				else if (item.Channel == ChatChannel.Global)
				{
					MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(item.SenderId);
					if (myIdentity != null)
					{
						Color relationColor2 = MyChatSystem.GetRelationColor(item.SenderId);
						Color channelColor2 = MyChatSystem.GetChannelColor(item.Channel);
						m_chatHistory.AppendText(myIdentity.DisplayName, "White", m_chatHistory.TextScale, relationColor2);
						m_chatHistory.AppendText(": ", "White", m_chatHistory.TextScale, relationColor2);
						m_chatHistory.AppendText(item.Text, "White", m_chatHistory.TextScale, channelColor2);
						m_chatHistory.AppendLine();
					}
				}
			}
			m_factionList.SelectedItems.Clear();
			m_chatHistory.ScrollbarOffsetV = 1f;
		}

		private void RefreshChatBotHistory()
		{
			m_chatHistory.Clear();
			List<MyUnifiedChatItem> list = new List<MyUnifiedChatItem>();
			MySession.Static.ChatSystem.ChatHistory.GetChatbotHistory(ref list);
			foreach (MyUnifiedChatItem item in list)
			{
				MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity((item.SenderId != 0L) ? item.SenderId : item.TargetId);
				if (myIdentity != null)
				{
					Vector4 one = Vector4.One;
					Color white = Color.White;
					string text = (item.CustomAuthor.Length > 0) ? item.CustomAuthor : myIdentity.DisplayName;
					m_chatHistory.AppendText(text, "White", m_chatHistory.TextScale, one);
					m_chatHistory.AppendText(": ", "White", m_chatHistory.TextScale, one);
					m_chatHistory.Parse(item.Text, "White", m_chatHistory.TextScale, white);
					m_chatHistory.AppendLine();
				}
			}
			m_factionList.SelectedItems.Clear();
			m_chatHistory.ScrollbarOffsetV = 1f;
		}

		private void ClearChat()
		{
			m_chatHistory.Clear();
			m_chatbox.Text = string.Empty;
		}

		private void RefreshLists()
		{
			RefreshPlayerList();
			RefreshFactionList();
		}

		private void RefreshPlayerList()
		{
			m_globalItem = new MyGuiControlListbox.Item(MyTexts.Get(MySpaceTexts.TerminalTab_Chat_ChatHistory), MyTexts.GetString(MySpaceTexts.TerminalTab_Chat_ChatHistory));
			if (AllowPlayerDrivenChat)
			{
				m_playerList.Add(m_globalItem);
			}
			m_tempStringBuilder.Clear();
			m_tempStringBuilder.Append((object)MyTexts.Get(MySpaceTexts.TerminalTab_Chat_GlobalChat));
			m_tempStringBuilder.Clear();
			m_tempStringBuilder.Append("-");
			m_tempStringBuilder.Append((object)MyTexts.Get(MySpaceTexts.ChatBotName));
			m_tempStringBuilder.Append("-");
			m_chatBotItem = new MyGuiControlListbox.Item(m_tempStringBuilder, m_tempStringBuilder.ToString());
			m_playerList.Add(m_chatBotItem);
			if (AllowPlayerDrivenChat)
			{
				foreach (MyPlayer.PlayerId allPlayer in MySession.Static.Players.GetAllPlayers())
				{
					MyIdentity myIdentity = MySession.Static.Players.TryGetIdentity(MySession.Static.Players.TryGetIdentityId(allPlayer.SteamId, allPlayer.SerialId));
					if (myIdentity != null && myIdentity.IdentityId != MySession.Static.LocalPlayerId && allPlayer.SerialId == 0)
					{
						m_tempStringBuilder.Clear();
						m_tempStringBuilder.Append(myIdentity.DisplayName);
						StringBuilder tempStringBuilder = m_tempStringBuilder;
						object userData = myIdentity;
						MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(tempStringBuilder, m_tempStringBuilder.ToString(), null, userData);
						m_playerList.Add(item);
					}
				}
			}
			else
			{
				m_playerList.Add(m_globalItem);
			}
		}

		private void RefreshFactionList()
		{
			if (AllowPlayerDrivenChat)
			{
				IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId);
				if (myFaction != null)
				{
					m_tempStringBuilder.Clear();
					m_tempStringBuilder.Append(MyStatControlText.SubstituteTexts(myFaction.Name));
					StringBuilder tempStringBuilder = m_tempStringBuilder;
					object userData = myFaction;
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(tempStringBuilder, m_tempStringBuilder.ToString(), null, userData);
					m_factionList.Add(item);
					m_factionList.SetToolTip(string.Empty);
				}
				else
				{
					m_factionList.SelectedItems.Clear();
					m_factionList.Items.Clear();
					m_factionList.SetToolTip(MyTexts.GetString(MySpaceTexts.TerminalTab_Chat_NoFaction));
				}
			}
		}

		public void Update()
		{
			if (!m_closed)
			{
				UpdateLists();
			}
		}

		private void UpdatePlayerList()
		{
			long num = -1L;
			bool flag = false;
			bool flag2 = false;
			if (m_playerList.SelectedItems.Count > 0)
			{
				if (m_playerList.SelectedItems[0] == m_globalItem)
				{
					flag = true;
				}
				else if (m_playerList.SelectedItems[0] == m_chatBotItem)
				{
					flag2 = true;
				}
				else
				{
					num = ((MyIdentity)m_playerList.SelectedItems[0].UserData).IdentityId;
				}
			}
			int num2 = m_playerList.FirstVisibleRow;
			m_playerList.SelectedItems.Clear();
			m_playerList.Items.Clear();
			RefreshPlayerList();
			if (num != -1)
			{
				bool flag3 = false;
				foreach (MyGuiControlListbox.Item item in m_playerList.Items)
				{
					if (item.UserData != null && ((MyIdentity)item.UserData).IdentityId == num)
					{
						m_playerList.SelectedItems.Clear();
						m_playerList.SelectedItems.Add(item);
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					ClearChat();
				}
			}
			else if (flag)
			{
				m_playerList.SelectedItems.Clear();
				m_playerList.SelectedItems.Add(m_globalItem);
			}
			else if (flag2)
			{
				m_playerList.SelectedItems.Clear();
				m_playerList.SelectedItems.Add(m_chatBotItem);
			}
			if (num2 >= m_playerList.Items.Count)
			{
				num2 = m_playerList.Items.Count - 1;
			}
			m_playerList.FirstVisibleRow = num2;
		}

		private void UpdateLists()
		{
			UpdateFactionList(forceRefresh: false);
			if (m_frameCount > 100)
			{
				m_frameCount = 0;
				UpdatePlayerList();
			}
			m_frameCount++;
		}

		private void UpdateFactionList(bool forceRefresh)
		{
			MyFactionCollection factions = MySession.Static.Factions;
			if (factions.TryGetPlayerFaction(MySession.Static.LocalPlayerId) == null)
			{
				if (m_factionList.Items.Count != 0)
				{
					RefreshFactionList();
				}
			}
			else
			{
				if (!forceRefresh && m_factionList.Items.Count == factions.Count())
				{
					return;
				}
				long num = -1L;
				if (m_factionList.SelectedItems.Count > 0)
				{
					num = ((MyFaction)m_factionList.SelectedItems[0].UserData).FactionId;
				}
				int num2 = m_factionList.FirstVisibleRow;
				m_factionList.SelectedItems.Clear();
				m_factionList.Items.Clear();
				RefreshFactionList();
				if (num != -1)
				{
					bool flag = false;
					foreach (MyGuiControlListbox.Item item in m_factionList.Items)
					{
						if (((MyFaction)item.UserData).FactionId == num)
						{
							m_factionList.SelectedItems.Clear();
							m_factionList.SelectedItems.Add(item);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						ClearChat();
					}
				}
				if (num2 >= m_factionList.Items.Count)
				{
					num2 = m_factionList.Items.Count - 1;
				}
				m_factionList.FirstVisibleRow = num2;
			}
		}

		public void Close()
		{
			m_closed = false;
			m_playerList.ItemsSelected -= m_playerList_ItemsSelected;
			m_factionList.ItemsSelected -= m_factionList_ItemsSelected;
			m_sendButton.ButtonClicked -= m_sendButton_ButtonClicked;
			m_chatbox.TextChanged -= m_chatbox_TextChanged;
			m_chatbox.EnterPressed -= m_chatbox_EnterPressed;
			if (MyMultiplayer.Static != null)
			{
				MyMultiplayer.Static.ChatMessageReceived -= Multiplayer_ChatMessageReceived;
			}
			if (MySession.Static.LocalCharacter != null)
			{
				MySession.Static.ChatSystem.PlayerMessageReceived -= MyChatSystem_PlayerMessageReceived;
				MySession.Static.ChatSystem.FactionMessageReceived -= MyChatSystem_FactionMessageReceived;
			}
			MySession.Static.Players.PlayersChanged -= Players_PlayersChanged;
		}
	}
}
