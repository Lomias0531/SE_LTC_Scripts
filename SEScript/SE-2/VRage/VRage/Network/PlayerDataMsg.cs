using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Serialization;
using VRageMath;

namespace VRage.Network
{
	[Serializable]
	public struct PlayerDataMsg
	{
		protected class VRage_Network_PlayerDataMsg_003C_003EClientSteamId_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in ulong value)
			{
				owner.ClientSteamId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out ulong value)
			{
				value = owner.ClientSteamId;
			}
		}

		protected class VRage_Network_PlayerDataMsg_003C_003EPlayerSerialId_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in int value)
			{
				owner.PlayerSerialId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out int value)
			{
				value = owner.PlayerSerialId;
			}
		}

		protected class VRage_Network_PlayerDataMsg_003C_003EIdentityId_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in long value)
			{
				owner.IdentityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out long value)
			{
				value = owner.IdentityId;
			}
		}

		protected class VRage_Network_PlayerDataMsg_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in string value)
			{
				owner.DisplayName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out string value)
			{
				value = owner.DisplayName;
			}
		}

		protected class VRage_Network_PlayerDataMsg_003C_003EBuildColors_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, List<Vector3>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in List<Vector3> value)
			{
				owner.BuildColors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out List<Vector3> value)
			{
				value = owner.BuildColors;
			}
		}

		protected class VRage_Network_PlayerDataMsg_003C_003ERealPlayer_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in bool value)
			{
				owner.RealPlayer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out bool value)
			{
				value = owner.RealPlayer;
			}
		}

		protected class VRage_Network_PlayerDataMsg_003C_003ENewIdentity_003C_003EAccessor : IMemberAccessor<PlayerDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref PlayerDataMsg owner, in bool value)
			{
				owner.NewIdentity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref PlayerDataMsg owner, out bool value)
			{
				value = owner.NewIdentity;
			}
		}

		public ulong ClientSteamId;

		public int PlayerSerialId;

		public long IdentityId;

		[Serialize(MyObjectFlags.DefaultZero)]
		public string DisplayName;

		[Serialize(MyObjectFlags.DefaultZero)]
		public List<Vector3> BuildColors;

		public bool RealPlayer;

		public bool NewIdentity;
	}
}
