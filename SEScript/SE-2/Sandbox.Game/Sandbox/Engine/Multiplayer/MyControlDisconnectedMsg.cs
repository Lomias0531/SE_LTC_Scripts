using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
	[Serializable]
	public struct MyControlDisconnectedMsg
	{
		protected class Sandbox_Engine_Multiplayer_MyControlDisconnectedMsg_003C_003EClient_003C_003EAccessor : IMemberAccessor<MyControlDisconnectedMsg, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyControlDisconnectedMsg owner, in ulong value)
			{
				owner.Client = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyControlDisconnectedMsg owner, out ulong value)
			{
				value = owner.Client;
			}
		}

		public ulong Client;
	}
}
