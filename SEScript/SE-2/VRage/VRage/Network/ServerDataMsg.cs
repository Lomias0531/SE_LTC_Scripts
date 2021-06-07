using System;
using System.Runtime.CompilerServices;
using VRage.Library.Utils;
using VRage.Serialization;

namespace VRage.Network
{
	[Serializable]
	public struct ServerDataMsg
	{
		protected class VRage_Network_ServerDataMsg_003C_003EWorldName_003C_003EAccessor : IMemberAccessor<ServerDataMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in string value)
			{
				owner.WorldName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out string value)
			{
				value = owner.WorldName;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EGameMode_003C_003EAccessor : IMemberAccessor<ServerDataMsg, MyGameModeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in MyGameModeEnum value)
			{
				owner.GameMode = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out MyGameModeEnum value)
			{
				value = owner.GameMode;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EInventoryMultiplier_003C_003EAccessor : IMemberAccessor<ServerDataMsg, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in float value)
			{
				owner.InventoryMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out float value)
			{
				value = owner.InventoryMultiplier;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EAssemblerMultiplier_003C_003EAccessor : IMemberAccessor<ServerDataMsg, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in float value)
			{
				owner.AssemblerMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out float value)
			{
				value = owner.AssemblerMultiplier;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003ERefineryMultiplier_003C_003EAccessor : IMemberAccessor<ServerDataMsg, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in float value)
			{
				owner.RefineryMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out float value)
			{
				value = owner.RefineryMultiplier;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EHostName_003C_003EAccessor : IMemberAccessor<ServerDataMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in string value)
			{
				owner.HostName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out string value)
			{
				value = owner.HostName;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EWorldSize_003C_003EAccessor : IMemberAccessor<ServerDataMsg, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in ulong value)
			{
				owner.WorldSize = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out ulong value)
			{
				value = owner.WorldSize;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EAppVersion_003C_003EAccessor : IMemberAccessor<ServerDataMsg, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in int value)
			{
				owner.AppVersion = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out int value)
			{
				value = owner.AppVersion;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EMembersLimit_003C_003EAccessor : IMemberAccessor<ServerDataMsg, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in int value)
			{
				owner.MembersLimit = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out int value)
			{
				value = owner.MembersLimit;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EDataHash_003C_003EAccessor : IMemberAccessor<ServerDataMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in string value)
			{
				owner.DataHash = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out string value)
			{
				value = owner.DataHash;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EWelderMultiplier_003C_003EAccessor : IMemberAccessor<ServerDataMsg, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in float value)
			{
				owner.WelderMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out float value)
			{
				value = owner.WelderMultiplier;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EGrinderMultiplier_003C_003EAccessor : IMemberAccessor<ServerDataMsg, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in float value)
			{
				owner.GrinderMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out float value)
			{
				value = owner.GrinderMultiplier;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EBlocksInventoryMultiplier_003C_003EAccessor : IMemberAccessor<ServerDataMsg, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in float value)
			{
				owner.BlocksInventoryMultiplier = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out float value)
			{
				value = owner.BlocksInventoryMultiplier;
			}
		}

		protected class VRage_Network_ServerDataMsg_003C_003EServerPasswordSalt_003C_003EAccessor : IMemberAccessor<ServerDataMsg, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ServerDataMsg owner, in string value)
			{
				owner.ServerPasswordSalt = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ServerDataMsg owner, out string value)
			{
				value = owner.ServerPasswordSalt;
			}
		}

		[Serialize(MyObjectFlags.DefaultZero)]
		public string WorldName;

		public MyGameModeEnum GameMode;

		public float InventoryMultiplier;

		public float AssemblerMultiplier;

		public float RefineryMultiplier;

		[Serialize(MyObjectFlags.DefaultZero)]
		public string HostName;

		public ulong WorldSize;

		public int AppVersion;

		public int MembersLimit;

		[Serialize(MyObjectFlags.DefaultZero)]
		public string DataHash;

		public float WelderMultiplier;

		public float GrinderMultiplier;

		public float BlocksInventoryMultiplier;

		[Serialize(MyObjectFlags.DefaultZero)]
		public string ServerPasswordSalt
		{
			get;
			set;
		}
	}
}
