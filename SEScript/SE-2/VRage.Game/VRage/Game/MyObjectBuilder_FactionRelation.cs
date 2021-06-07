using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyObjectBuilder_FactionRelation
	{
		protected class VRage_Game_MyObjectBuilder_FactionRelation_003C_003EFactionId1_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRelation, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionRelation owner, in long value)
			{
				owner.FactionId1 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionRelation owner, out long value)
			{
				value = owner.FactionId1;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionRelation_003C_003EFactionId2_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRelation, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionRelation owner, in long value)
			{
				owner.FactionId2 = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionRelation owner, out long value)
			{
				value = owner.FactionId2;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionRelation_003C_003ERelation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRelation, MyRelationsBetweenFactions>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionRelation owner, in MyRelationsBetweenFactions value)
			{
				owner.Relation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionRelation owner, out MyRelationsBetweenFactions value)
			{
				value = owner.Relation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionRelation_003C_003EReputation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRelation, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionRelation owner, in int value)
			{
				owner.Reputation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionRelation owner, out int value)
			{
				value = owner.Reputation;
			}
		}

		private class VRage_Game_MyObjectBuilder_FactionRelation_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FactionRelation>
		{
			private sealed override object CreateInstance()
			{
				return default(MyObjectBuilder_FactionRelation);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FactionRelation CreateInstance()
			{
				return (MyObjectBuilder_FactionRelation)(object)default(MyObjectBuilder_FactionRelation);
			}

			MyObjectBuilder_FactionRelation IActivator<MyObjectBuilder_FactionRelation>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long FactionId1;

		[ProtoMember(4)]
		public long FactionId2;

		[ProtoMember(7)]
		public MyRelationsBetweenFactions Relation;

		[ProtoMember(10)]
		public int Reputation;
	}
}
