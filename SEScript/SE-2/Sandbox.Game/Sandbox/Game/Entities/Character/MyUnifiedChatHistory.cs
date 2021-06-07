using Sandbox.Game.Gui;
using System;
using System.Collections.Generic;

namespace Sandbox.Game.Entities.Character
{
	public class MyUnifiedChatHistory
	{
		protected Queue<MyUnifiedChatItem> m_chat = new Queue<MyUnifiedChatItem>();

		public void EnqueueMessage(string text, ChatChannel channel, long senderId, long targetId = 0L, DateTime? timestamp = null, string authorFont = "Blue")
		{
			DateTime timestamp2 = (!timestamp.HasValue || !timestamp.HasValue) ? DateTime.UtcNow : timestamp.Value;
			MyUnifiedChatItem item;
			switch (channel)
			{
			case ChatChannel.Global:
			case ChatChannel.GlobalScripted:
				item = MyUnifiedChatItem.CreateGlobalMessage(text, timestamp2, senderId, authorFont);
				break;
			case ChatChannel.Faction:
				item = MyUnifiedChatItem.CreateFactionMessage(text, timestamp2, senderId, targetId, authorFont);
				break;
			case ChatChannel.Private:
				item = MyUnifiedChatItem.CreatePrivateMessage(text, timestamp2, senderId, targetId, authorFont);
				break;
			case ChatChannel.ChatBot:
				item = MyUnifiedChatItem.CreateChatbotMessage(text, timestamp2, senderId, targetId, null, authorFont);
				break;
			default:
				item = null;
				break;
			}
			if (item != null)
			{
				EnqueueMessage(ref item);
			}
		}

		public void EnqueueMessageScripted(string text, string customAuthor, string authorFont = "Blue")
		{
			MyUnifiedChatItem item = MyUnifiedChatItem.CreateScriptedMessage(text, DateTime.UtcNow, customAuthor, authorFont);
			EnqueueMessage(ref item);
		}

		public void EnqueueMessage(ref MyUnifiedChatItem item)
		{
			m_chat.Enqueue(item);
		}

		public void GetCompleteHistory(ref List<MyUnifiedChatItem> list)
		{
			foreach (MyUnifiedChatItem item in m_chat)
			{
				list.Add(item);
			}
		}

		public void GetGeneralHistory(ref List<MyUnifiedChatItem> list)
		{
			foreach (MyUnifiedChatItem item in m_chat)
			{
				if (item.Channel == ChatChannel.Global || item.Channel == ChatChannel.GlobalScripted)
				{
					list.Add(item);
				}
			}
		}

		public void GetFactionHistory(ref List<MyUnifiedChatItem> list, long factionId)
		{
			foreach (MyUnifiedChatItem item in m_chat)
			{
				if (item.Channel == ChatChannel.Faction && item.TargetId == factionId)
				{
					list.Add(item);
				}
			}
		}

		public void GetPrivateHistory(ref List<MyUnifiedChatItem> list, long playerId)
		{
			foreach (MyUnifiedChatItem item in m_chat)
			{
				if (item.Channel == ChatChannel.Private && (item.TargetId == playerId || item.SenderId == playerId))
				{
					list.Add(item);
				}
			}
		}

		public void GetChatbotHistory(ref List<MyUnifiedChatItem> list)
		{
			foreach (MyUnifiedChatItem item in m_chat)
			{
				if (item.Channel == ChatChannel.ChatBot)
				{
					list.Add(item);
				}
			}
		}

		public void ClearNonGlobalHistory()
		{
			Queue<MyUnifiedChatItem> queue = new Queue<MyUnifiedChatItem>();
			foreach (MyUnifiedChatItem item in m_chat)
			{
				switch (item.Channel)
				{
				case ChatChannel.Global:
				case ChatChannel.GlobalScripted:
				case ChatChannel.ChatBot:
					queue.Enqueue(item);
					break;
				}
			}
			m_chat = queue;
		}
	}
}
