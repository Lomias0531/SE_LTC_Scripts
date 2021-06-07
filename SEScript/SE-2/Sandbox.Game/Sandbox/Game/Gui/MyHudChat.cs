using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GUI.HudViewers;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyHudChat
	{
		private static readonly int MAX_MESSAGES_IN_CHAT_DEFAULT = 10;

		private static readonly int MAX_MESSAGE_TIME_DEFAULT = 15000;

		public static int MaxMessageTime = MAX_MESSAGE_TIME_DEFAULT;

		public static int MaxMessageCount = MAX_MESSAGES_IN_CHAT_DEFAULT;

		public Queue<MyChatItem> MessagesQueue = new Queue<MyChatItem>();

		public List<MyChatItem> MessageHistory = new List<MyChatItem>();

		private int m_lastUpdateTime = int.MaxValue;

		private int m_lastScreenUpdateTime = int.MaxValue;

		public MyHudControlChat ChatControl;

		private bool m_chatScreenOpen;

		public int Timestamp
		{
			get;
			private set;
		}

		public int LastUpdateTime => m_lastUpdateTime;

		public int TimeSinceLastUpdate => MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastScreenUpdateTime;

		public MyHudChat()
		{
			Timestamp = 0;
		}

		public void RegisterChat(MyMultiplayerBase multiplayer)
		{
			if (multiplayer != null)
			{
				multiplayer.ChatMessageReceived += Multiplayer_ChatMessageReceived;
				multiplayer.ScriptedChatMessageReceived += multiplayer_ScriptedChatMessageReceived;
			}
		}

		public void UnregisterChat(MyMultiplayerBase multiplayer)
		{
			if (multiplayer != null)
			{
				multiplayer.ChatMessageReceived -= Multiplayer_ChatMessageReceived;
				multiplayer.ScriptedChatMessageReceived -= multiplayer_ScriptedChatMessageReceived;
				MessagesQueue.Clear();
				UpdateTimestamp();
			}
		}

		public void ShowMessageScripted(string sender, string messageText)
		{
			Color paleGoldenrod = Color.PaleGoldenrod;
			Color white = Color.White;
			ShowMessage(sender, messageText, paleGoldenrod, white);
		}

		public void ShowMessage(string sender, string messageText, Color color, string font = "Blue")
		{
			MyChatItem item = new MyChatItem(sender, messageText, font, color);
			MessagesQueue.Enqueue(item);
			MessageHistory.Add(item);
			m_lastScreenUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (MessagesQueue.Count > MaxMessageCount)
			{
				MessagesQueue.Dequeue();
			}
			UpdateTimestamp();
		}

		public void ShowMessage(string sender, string messageText, string font = "Blue")
		{
			ShowMessage(sender, messageText, Color.White, font);
		}

		public void ShowMessageColoredSP(string text, ChatChannel channel, long targetId = 0L, string customAuthorName = null)
		{
			string empty = string.Empty;
			if (channel == ChatChannel.Private)
			{
				if (targetId == MySession.Static.LocalPlayerId)
				{
					empty = MySession.Static.LocalHumanPlayer.DisplayName;
					empty = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_From), empty);
				}
				else
				{
					empty = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_To), targetId);
				}
			}
			else
			{
				empty = MySession.Static.LocalHumanPlayer.DisplayName;
			}
			long num = MySession.Static.Players.TryGetIdentityId(Sync.MyId);
			Color relationColor = MyChatSystem.GetRelationColor(num);
			Color channelColor = MyChatSystem.GetChannelColor(channel);
			ShowMessage(string.IsNullOrEmpty(customAuthorName) ? empty : customAuthorName, text, relationColor, channelColor);
			if (channel == ChatChannel.GlobalScripted)
			{
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessageScripted(text, string.IsNullOrEmpty(customAuthorName) ? MyTexts.GetString(MySpaceTexts.ChatBotName) : customAuthorName);
			}
			else
			{
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(text, channel, num, targetId);
			}
		}

		public void ShowMessage(string sender, string message, Color senderColor)
		{
			ShowMessage(sender, message, senderColor, Color.White);
		}

		public void ShowMessage(string sender, string message, Color senderColor, Color messageColor)
		{
			MyChatItem item = new MyChatItem(sender, message, "White", senderColor, messageColor);
			MessagesQueue.Enqueue(item);
			MessageHistory.Add(item);
			m_lastScreenUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (MessagesQueue.Count > MaxMessageCount)
			{
				MessagesQueue.Dequeue();
			}
			UpdateTimestamp();
		}

		private void Multiplayer_ChatMessageReceived(ulong steamUserId, string messageText, ChatChannel channel, long targetId, string customAuthorName = null)
		{
			if (!MyGameService.IsActive)
			{
				return;
			}
			string text = string.Empty;
			if (channel == ChatChannel.Private)
			{
				if (targetId == MySession.Static.LocalPlayerId)
				{
					text = MyMultiplayer.Static.GetMemberName(steamUserId);
					text = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_From), text);
				}
				else
				{
					ulong num = MySession.Static.Players.TryGetSteamId(targetId);
					if (num != 0L)
					{
						text = string.Format(MyTexts.GetString(MyCommonTexts.Chat_NameModifier_To), MyMultiplayer.Static.GetMemberName(num));
					}
				}
			}
			else
			{
				text = MyMultiplayer.Static.GetMemberName(steamUserId);
			}
			long num2 = MySession.Static.Players.TryGetIdentityId(steamUserId);
			Color relationColor = MyChatSystem.GetRelationColor(num2);
			Color channelColor = MyChatSystem.GetChannelColor(channel);
			ShowMessage(string.IsNullOrEmpty(customAuthorName) ? text : customAuthorName, messageText, relationColor, channelColor);
			if (channel == ChatChannel.GlobalScripted)
			{
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessageScripted(messageText, string.IsNullOrEmpty(customAuthorName) ? MyTexts.GetString(MySpaceTexts.ChatBotName) : customAuthorName);
			}
			else
			{
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessage(messageText, channel, num2, targetId);
			}
		}

		public void multiplayer_ScriptedChatMessageReceived(string message, string author, string font, Color color)
		{
			if (MyGameService.IsActive)
			{
				ShowMessage(author, message, font);
				MySession.Static.ChatSystem.ChatHistory.EnqueueMessageScripted(message, author, font);
			}
		}

		private void UpdateTimestamp()
		{
			Timestamp++;
			m_lastUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		public void Update()
		{
			if (m_chatScreenOpen)
			{
				m_lastScreenUpdateTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_lastUpdateTime > MaxMessageTime && MessagesQueue.Count > 0)
			{
				MessagesQueue.Dequeue();
				UpdateTimestamp();
			}
		}

		public static void ResetChatSettings()
		{
			MaxMessageTime = MAX_MESSAGE_TIME_DEFAULT;
			MaxMessageCount = MAX_MESSAGES_IN_CHAT_DEFAULT;
		}

		public void ChatOpened()
		{
			m_chatScreenOpen = true;
		}

		public void ChatClosed()
		{
			m_chatScreenOpen = false;
		}
	}
}
