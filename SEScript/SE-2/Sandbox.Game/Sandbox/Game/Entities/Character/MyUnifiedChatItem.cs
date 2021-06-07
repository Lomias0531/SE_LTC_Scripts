using Sandbox.Game.Gui;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Game.Entities.Character
{
	[Serializable]
	public class MyUnifiedChatItem
	{
		protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem_003C_003EAuthorFont_003C_003EAccessor : IMemberAccessor<MyUnifiedChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUnifiedChatItem owner, in string value)
			{
				owner.AuthorFont = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUnifiedChatItem owner, out string value)
			{
				value = owner.AuthorFont;
			}
		}

		protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem_003C_003EText_003C_003EAccessor : IMemberAccessor<MyUnifiedChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUnifiedChatItem owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUnifiedChatItem owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem_003C_003ETimestamp_003C_003EAccessor : IMemberAccessor<MyUnifiedChatItem, DateTime>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUnifiedChatItem owner, in DateTime value)
			{
				owner.Timestamp = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUnifiedChatItem owner, out DateTime value)
			{
				value = owner.Timestamp;
			}
		}

		protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem_003C_003EChannel_003C_003EAccessor : IMemberAccessor<MyUnifiedChatItem, ChatChannel>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUnifiedChatItem owner, in ChatChannel value)
			{
				owner.Channel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUnifiedChatItem owner, out ChatChannel value)
			{
				value = owner.Channel;
			}
		}

		protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem_003C_003ECustomAuthor_003C_003EAccessor : IMemberAccessor<MyUnifiedChatItem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUnifiedChatItem owner, in string value)
			{
				owner.CustomAuthor = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUnifiedChatItem owner, out string value)
			{
				value = owner.CustomAuthor;
			}
		}

		protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem_003C_003ESenderId_003C_003EAccessor : IMemberAccessor<MyUnifiedChatItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUnifiedChatItem owner, in long value)
			{
				owner.SenderId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUnifiedChatItem owner, out long value)
			{
				value = owner.SenderId;
			}
		}

		protected class Sandbox_Game_Entities_Character_MyUnifiedChatItem_003C_003ETargetId_003C_003EAccessor : IMemberAccessor<MyUnifiedChatItem, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUnifiedChatItem owner, in long value)
			{
				owner.TargetId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUnifiedChatItem owner, out long value)
			{
				value = owner.TargetId;
			}
		}

		public string AuthorFont = "Blue";

		public string Text
		{
			get;
			set;
		}

		public DateTime Timestamp
		{
			get;
			set;
		}

		public ChatChannel Channel
		{
			get;
			set;
		}

		public string CustomAuthor
		{
			get;
			set;
		}

		public long SenderId
		{
			get;
			set;
		}

		public long TargetId
		{
			get;
			set;
		}

		public MyUnifiedChatItem()
		{
			Text = string.Empty;
			Timestamp = DateTime.UtcNow;
			Channel = ChatChannel.Global;
			CustomAuthor = string.Empty;
			AuthorFont = string.Empty;
			SenderId = 0L;
			TargetId = 0L;
		}

		public static MyUnifiedChatItem CreateGlobalMessage(string text, DateTime timestamp, long senderId, string authorFont = "Blue")
		{
			return new MyUnifiedChatItem
			{
				Text = text,
				Timestamp = timestamp,
				Channel = ChatChannel.Global,
				CustomAuthor = string.Empty,
				SenderId = senderId,
				TargetId = 0L,
				AuthorFont = authorFont
			};
		}

		public static MyUnifiedChatItem CreateFactionMessage(string text, DateTime timestamp, long senderId, long targetId, string authorFont = "Blue")
		{
			return new MyUnifiedChatItem
			{
				Text = text,
				Timestamp = timestamp,
				Channel = ChatChannel.Faction,
				CustomAuthor = string.Empty,
				SenderId = senderId,
				TargetId = targetId,
				AuthorFont = authorFont
			};
		}

		public static MyUnifiedChatItem CreatePrivateMessage(string text, DateTime timestamp, long senderId, long targetId, string authorFont = "Blue")
		{
			return new MyUnifiedChatItem
			{
				Text = text,
				Timestamp = timestamp,
				Channel = ChatChannel.Private,
				CustomAuthor = string.Empty,
				SenderId = senderId,
				TargetId = targetId,
				AuthorFont = authorFont
			};
		}

		public static MyUnifiedChatItem CreateScriptedMessage(string text, DateTime timestamp, string customAuthor, string authorFont = "Blue")
		{
			return new MyUnifiedChatItem
			{
				Text = text,
				Timestamp = timestamp,
				Channel = ChatChannel.GlobalScripted,
				CustomAuthor = (string.IsNullOrEmpty(customAuthor) ? string.Empty : customAuthor),
				SenderId = 0L,
				TargetId = 0L,
				AuthorFont = authorFont
			};
		}

		public static MyUnifiedChatItem CreateChatbotMessage(string text, DateTime timestamp, long senderId, long targetId = 0L, string customAuthor = null, string authorFont = "Blue")
		{
			return new MyUnifiedChatItem
			{
				Text = text,
				Timestamp = timestamp,
				Channel = ChatChannel.ChatBot,
				CustomAuthor = (string.IsNullOrEmpty(customAuthor) ? string.Empty : customAuthor),
				SenderId = senderId,
				TargetId = targetId,
				AuthorFont = authorFont
			};
		}
	}
}
