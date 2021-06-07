using System;
using System.Runtime.CompilerServices;

namespace VRage.Network
{
	[Serializable]
	public struct JoinResultMsg
	{
		protected class VRage_Network_JoinResultMsg_003C_003EJoinResult_003C_003EAccessor : IMemberAccessor<JoinResultMsg, JoinResult>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref JoinResultMsg owner, in JoinResult value)
			{
				owner.JoinResult = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref JoinResultMsg owner, out JoinResult value)
			{
				value = owner.JoinResult;
			}
		}

		protected class VRage_Network_JoinResultMsg_003C_003EServerExperimental_003C_003EAccessor : IMemberAccessor<JoinResultMsg, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref JoinResultMsg owner, in bool value)
			{
				owner.ServerExperimental = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref JoinResultMsg owner, out bool value)
			{
				value = owner.ServerExperimental;
			}
		}

		protected class VRage_Network_JoinResultMsg_003C_003EAdmin_003C_003EAccessor : IMemberAccessor<JoinResultMsg, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref JoinResultMsg owner, in ulong value)
			{
				owner.Admin = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref JoinResultMsg owner, out ulong value)
			{
				value = owner.Admin;
			}
		}

		public JoinResult JoinResult;

		public bool ServerExperimental;

		public ulong Admin;
	}
}
