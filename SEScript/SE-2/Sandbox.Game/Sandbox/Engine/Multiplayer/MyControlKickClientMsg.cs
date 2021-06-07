using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
	[Serializable]
	public struct MyControlKickClientMsg
	{
		protected class Sandbox_Engine_Multiplayer_MyControlKickClientMsg_003C_003EKickedClient_003C_003EAccessor : IMemberAccessor<MyControlKickClientMsg, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyControlKickClientMsg owner, in ulong value)
			{
				owner.KickedClient = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyControlKickClientMsg owner, out ulong value)
			{
				value = owner.KickedClient;
			}
		}

		protected class Sandbox_Engine_Multiplayer_MyControlKickClientMsg_003C_003EKicked_003C_003EAccessor : IMemberAccessor<MyControlKickClientMsg, BoolBlit>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyControlKickClientMsg owner, in BoolBlit value)
			{
				owner.Kicked = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyControlKickClientMsg owner, out BoolBlit value)
			{
				value = owner.Kicked;
			}
		}

		protected class Sandbox_Engine_Multiplayer_MyControlKickClientMsg_003C_003EAdd_003C_003EAccessor : IMemberAccessor<MyControlKickClientMsg, BoolBlit>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyControlKickClientMsg owner, in BoolBlit value)
			{
				owner.Add = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyControlKickClientMsg owner, out BoolBlit value)
			{
				value = owner.Add;
			}
		}

		public ulong KickedClient;

		public BoolBlit Kicked;

		public BoolBlit Add;
	}
}
