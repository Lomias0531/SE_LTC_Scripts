using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Engine.Multiplayer
{
	[Serializable]
	public struct AllMembersDataMsg
	{
		protected class Sandbox_Engine_Multiplayer_AllMembersDataMsg_003C_003EIdentities_003C_003EAccessor : IMemberAccessor<AllMembersDataMsg, List<MyObjectBuilder_Identity>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AllMembersDataMsg owner, in List<MyObjectBuilder_Identity> value)
			{
				owner.Identities = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AllMembersDataMsg owner, out List<MyObjectBuilder_Identity> value)
			{
				value = owner.Identities;
			}
		}

		protected class Sandbox_Engine_Multiplayer_AllMembersDataMsg_003C_003EPlayers_003C_003EAccessor : IMemberAccessor<AllMembersDataMsg, List<MyPlayerCollection.AllPlayerData>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AllMembersDataMsg owner, in List<MyPlayerCollection.AllPlayerData> value)
			{
				owner.Players = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AllMembersDataMsg owner, out List<MyPlayerCollection.AllPlayerData> value)
			{
				value = owner.Players;
			}
		}

		protected class Sandbox_Engine_Multiplayer_AllMembersDataMsg_003C_003EFactions_003C_003EAccessor : IMemberAccessor<AllMembersDataMsg, List<MyObjectBuilder_Faction>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AllMembersDataMsg owner, in List<MyObjectBuilder_Faction> value)
			{
				owner.Factions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AllMembersDataMsg owner, out List<MyObjectBuilder_Faction> value)
			{
				value = owner.Factions;
			}
		}

		protected class Sandbox_Engine_Multiplayer_AllMembersDataMsg_003C_003EClients_003C_003EAccessor : IMemberAccessor<AllMembersDataMsg, List<MyObjectBuilder_Client>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref AllMembersDataMsg owner, in List<MyObjectBuilder_Client> value)
			{
				owner.Clients = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref AllMembersDataMsg owner, out List<MyObjectBuilder_Client> value)
			{
				value = owner.Clients;
			}
		}

		public List<MyObjectBuilder_Identity> Identities;

		public List<MyPlayerCollection.AllPlayerData> Players;

		public List<MyObjectBuilder_Faction> Factions;

		public List<MyObjectBuilder_Client> Clients;
	}
}
