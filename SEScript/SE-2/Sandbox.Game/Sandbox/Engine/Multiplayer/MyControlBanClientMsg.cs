using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
	[Serializable]
	public struct MyControlBanClientMsg
	{
		protected class Sandbox_Engine_Multiplayer_MyControlBanClientMsg_003C_003EBannedClient_003C_003EAccessor : IMemberAccessor<MyControlBanClientMsg, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyControlBanClientMsg owner, in ulong value)
			{
				owner.BannedClient = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyControlBanClientMsg owner, out ulong value)
			{
				value = owner.BannedClient;
			}
		}

		protected class Sandbox_Engine_Multiplayer_MyControlBanClientMsg_003C_003EBanned_003C_003EAccessor : IMemberAccessor<MyControlBanClientMsg, BoolBlit>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyControlBanClientMsg owner, in BoolBlit value)
			{
				owner.Banned = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyControlBanClientMsg owner, out BoolBlit value)
			{
				value = owner.Banned;
			}
		}

		public ulong BannedClient;

		public BoolBlit Banned;
	}
}
