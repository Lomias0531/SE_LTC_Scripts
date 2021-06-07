using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, "Factions")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_FactionCollection : MyObjectBuilder_Base
	{
		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003EFactions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_Faction>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in List<MyObjectBuilder_Faction> value)
			{
				owner.Factions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out List<MyObjectBuilder_Faction> value)
			{
				value = owner.Factions;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003EPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, SerializableDictionary<long, long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in SerializableDictionary<long, long> value)
			{
				owner.Players = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out SerializableDictionary<long, long> value)
			{
				value = owner.Players;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003ERelations_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_FactionRelation>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in List<MyObjectBuilder_FactionRelation> value)
			{
				owner.Relations = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out List<MyObjectBuilder_FactionRelation> value)
			{
				value = owner.Relations;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003ERelationsWithPlayers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_PlayerFactionRelation>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in List<MyObjectBuilder_PlayerFactionRelation> value)
			{
				owner.RelationsWithPlayers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out List<MyObjectBuilder_PlayerFactionRelation> value)
			{
				value = owner.RelationsWithPlayers;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003ERequests_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_FactionRequests>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in List<MyObjectBuilder_FactionRequests> value)
			{
				owner.Requests = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out List<MyObjectBuilder_FactionRequests> value)
			{
				value = owner.Requests;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003EPlayerToFactionsVis_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionCollection, List<MyObjectBuilder_FactionsVisEntry>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in List<MyObjectBuilder_FactionsVisEntry> value)
			{
				owner.PlayerToFactionsVis = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out List<MyObjectBuilder_FactionsVisEntry> value)
			{
				value = owner.PlayerToFactionsVis;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionCollection, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionCollection, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionCollection, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionCollection_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_FactionCollection, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_FactionCollection owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_FactionCollection owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_FactionCollection_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FactionCollection>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_FactionCollection();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FactionCollection CreateInstance()
			{
				return new MyObjectBuilder_FactionCollection();
			}

			MyObjectBuilder_FactionCollection IActivator<MyObjectBuilder_FactionCollection>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(16)]
		public List<MyObjectBuilder_Faction> Factions;

		[ProtoMember(19)]
		public SerializableDictionary<long, long> Players;

		[ProtoMember(22)]
		public List<MyObjectBuilder_FactionRelation> Relations;

		[ProtoMember(25)]
		public List<MyObjectBuilder_PlayerFactionRelation> RelationsWithPlayers;

		[ProtoMember(28)]
		public List<MyObjectBuilder_FactionRequests> Requests;

		[ProtoMember(31)]
		public List<MyObjectBuilder_FactionsVisEntry> PlayerToFactionsVis;
	}
}
