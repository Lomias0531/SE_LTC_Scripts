using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyObjectBuilder_PlayerFactionRelation
	{
		protected class VRage_Game_MyObjectBuilder_PlayerFactionRelation_003C_003EPlayerId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerFactionRelation, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerFactionRelation owner, in long value)
			{
				owner.PlayerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerFactionRelation owner, out long value)
			{
				value = owner.PlayerId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerFactionRelation_003C_003EFactionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerFactionRelation, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerFactionRelation owner, in long value)
			{
				owner.FactionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerFactionRelation owner, out long value)
			{
				value = owner.FactionId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerFactionRelation_003C_003ERelation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerFactionRelation, MyRelationsBetweenFactions>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerFactionRelation owner, in MyRelationsBetweenFactions value)
			{
				owner.Relation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerFactionRelation owner, out MyRelationsBetweenFactions value)
			{
				value = owner.Relation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_PlayerFactionRelation_003C_003EReputation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PlayerFactionRelation, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PlayerFactionRelation owner, in int value)
			{
				owner.Reputation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PlayerFactionRelation owner, out int value)
			{
				value = owner.Reputation;
			}
		}

		private class VRage_Game_MyObjectBuilder_PlayerFactionRelation_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PlayerFactionRelation>
		{
			private sealed override object CreateInstance()
			{
				return default(MyObjectBuilder_PlayerFactionRelation);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PlayerFactionRelation CreateInstance()
			{
				return (MyObjectBuilder_PlayerFactionRelation)(object)default(MyObjectBuilder_PlayerFactionRelation);
			}

			MyObjectBuilder_PlayerFactionRelation IActivator<MyObjectBuilder_PlayerFactionRelation>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long PlayerId;

		[ProtoMember(4)]
		public long FactionId;

		[ProtoMember(7)]
		public MyRelationsBetweenFactions Relation;

		[ProtoMember(10)]
		public int Reputation;
	}
}
