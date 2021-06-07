using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Multiplayer
{
	[Serializable]
	public struct ScriptedChatMsg
	{
		protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg_003C_003EText_003C_003EAccessor : IMemberAccessor<ScriptedChatMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScriptedChatMsg owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScriptedChatMsg owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg_003C_003EAuthor_003C_003EAccessor : IMemberAccessor<ScriptedChatMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScriptedChatMsg owner, in string value)
			{
				owner.Author = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScriptedChatMsg owner, out string value)
			{
				value = owner.Author;
			}
		}

		protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg_003C_003ETarget_003C_003EAccessor : IMemberAccessor<ScriptedChatMsg, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScriptedChatMsg owner, in long value)
			{
				owner.Target = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScriptedChatMsg owner, out long value)
			{
				value = owner.Target;
			}
		}

		protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg_003C_003EFont_003C_003EAccessor : IMemberAccessor<ScriptedChatMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScriptedChatMsg owner, in string value)
			{
				owner.Font = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScriptedChatMsg owner, out string value)
			{
				value = owner.Font;
			}
		}

		protected class Sandbox_Engine_Multiplayer_ScriptedChatMsg_003C_003EColor_003C_003EAccessor : IMemberAccessor<ScriptedChatMsg, Color>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScriptedChatMsg owner, in Color value)
			{
				owner.Color = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScriptedChatMsg owner, out Color value)
			{
				value = owner.Color;
			}
		}

		public string Text;

		public string Author;

		public long Target;

		public string Font;

		public Color Color;
	}
}
