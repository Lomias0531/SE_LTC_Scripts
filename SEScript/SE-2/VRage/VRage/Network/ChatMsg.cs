using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
	[Serializable]
	public struct ChatMsg
	{
		protected class VRage_Network_ChatMsg_003C_003EText_003C_003EAccessor : IMemberAccessor<ChatMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ChatMsg owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ChatMsg owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class VRage_Network_ChatMsg_003C_003EAuthor_003C_003EAccessor : IMemberAccessor<ChatMsg, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ChatMsg owner, in ulong value)
			{
				owner.Author = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ChatMsg owner, out ulong value)
			{
				value = owner.Author;
			}
		}

		protected class VRage_Network_ChatMsg_003C_003EChannel_003C_003EAccessor : IMemberAccessor<ChatMsg, byte>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ChatMsg owner, in byte value)
			{
				owner.Channel = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ChatMsg owner, out byte value)
			{
				value = owner.Channel;
			}
		}

		protected class VRage_Network_ChatMsg_003C_003ETargetId_003C_003EAccessor : IMemberAccessor<ChatMsg, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ChatMsg owner, in long value)
			{
				owner.TargetId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ChatMsg owner, out long value)
			{
				value = owner.TargetId;
			}
		}

		protected class VRage_Network_ChatMsg_003C_003ECustomAuthorName_003C_003EAccessor : IMemberAccessor<ChatMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ChatMsg owner, in string value)
			{
				owner.CustomAuthorName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ChatMsg owner, out string value)
			{
				value = owner.CustomAuthorName;
			}
		}

		public string Text;

		public ulong Author;

		public byte Channel;

		public long TargetId;

		public string CustomAuthorName;
	}
}
