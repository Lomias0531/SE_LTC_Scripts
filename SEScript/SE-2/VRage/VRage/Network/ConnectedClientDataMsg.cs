using System;
using System.Runtime.CompilerServices;
using VRage.Serialization;

namespace VRage.Network
{
	[Serializable]
	public struct ConnectedClientDataMsg
	{
		protected class VRage_Network_ConnectedClientDataMsg_003C_003ESteamID_003C_003EAccessor : IMemberAccessor<ConnectedClientDataMsg, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ConnectedClientDataMsg owner, in ulong value)
			{
				owner.SteamID = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ConnectedClientDataMsg owner, out ulong value)
			{
				value = owner.SteamID;
			}
		}

		protected class VRage_Network_ConnectedClientDataMsg_003C_003EName_003C_003EAccessor : IMemberAccessor<ConnectedClientDataMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ConnectedClientDataMsg owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ConnectedClientDataMsg owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Network_ConnectedClientDataMsg_003C_003EIsAdmin_003C_003EAccessor : IMemberAccessor<ConnectedClientDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ConnectedClientDataMsg owner, in bool value)
			{
				owner.IsAdmin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ConnectedClientDataMsg owner, out bool value)
			{
				value = owner.IsAdmin;
			}
		}

		protected class VRage_Network_ConnectedClientDataMsg_003C_003EJoin_003C_003EAccessor : IMemberAccessor<ConnectedClientDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ConnectedClientDataMsg owner, in bool value)
			{
				owner.Join = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ConnectedClientDataMsg owner, out bool value)
			{
				value = owner.Join;
			}
		}

		protected class VRage_Network_ConnectedClientDataMsg_003C_003EToken_003C_003EAccessor : IMemberAccessor<ConnectedClientDataMsg, byte[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ConnectedClientDataMsg owner, in byte[] value)
			{
				owner.Token = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ConnectedClientDataMsg owner, out byte[] value)
			{
				value = owner.Token;
			}
		}

		protected class VRage_Network_ConnectedClientDataMsg_003C_003EExperimentalMode_003C_003EAccessor : IMemberAccessor<ConnectedClientDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ConnectedClientDataMsg owner, in bool value)
			{
				owner.ExperimentalMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ConnectedClientDataMsg owner, out bool value)
			{
				value = owner.ExperimentalMode;
			}
		}

		protected class VRage_Network_ConnectedClientDataMsg_003C_003EIsProfiling_003C_003EAccessor : IMemberAccessor<ConnectedClientDataMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ConnectedClientDataMsg owner, in bool value)
			{
				owner.IsProfiling = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ConnectedClientDataMsg owner, out bool value)
			{
				value = owner.IsProfiling;
			}
		}

		public ulong SteamID;

		[Serialize(MyObjectFlags.DefaultZero)]
		public string Name;

		public bool IsAdmin;

		public bool Join;

		[Serialize(MyObjectFlags.DefaultZero)]
		public byte[] Token;

		public bool ExperimentalMode;

		public bool IsProfiling;
	}
}
