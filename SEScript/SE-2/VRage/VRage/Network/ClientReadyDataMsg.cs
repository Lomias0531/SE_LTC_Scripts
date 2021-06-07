using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
	[Serializable]
	public struct ClientReadyDataMsg
	{
		protected class VRage_Network_ClientReadyDataMsg_003C_003EUsePlayoutDelayBufferForCharacter_003C_003EAccessor : IMemberAccessor<ClientReadyDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ClientReadyDataMsg owner, in bool value)
			{
				owner.UsePlayoutDelayBufferForCharacter = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ClientReadyDataMsg owner, out bool value)
			{
				value = owner.UsePlayoutDelayBufferForCharacter;
			}
		}

		protected class VRage_Network_ClientReadyDataMsg_003C_003EUsePlayoutDelayBufferForJetpack_003C_003EAccessor : IMemberAccessor<ClientReadyDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ClientReadyDataMsg owner, in bool value)
			{
				owner.UsePlayoutDelayBufferForJetpack = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ClientReadyDataMsg owner, out bool value)
			{
				value = owner.UsePlayoutDelayBufferForJetpack;
			}
		}

		protected class VRage_Network_ClientReadyDataMsg_003C_003EUsePlayoutDelayBufferForGrids_003C_003EAccessor : IMemberAccessor<ClientReadyDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ClientReadyDataMsg owner, in bool value)
			{
				owner.UsePlayoutDelayBufferForGrids = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ClientReadyDataMsg owner, out bool value)
			{
				value = owner.UsePlayoutDelayBufferForGrids;
			}
		}

		public bool UsePlayoutDelayBufferForCharacter;

		public bool UsePlayoutDelayBufferForJetpack;

		public bool UsePlayoutDelayBufferForGrids;
	}
}
