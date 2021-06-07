using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyObjectBuilder_FactionMember
	{
		protected class VRage_Game_MyObjectBuilder_FactionMember_003C_003EPlayerId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionMember, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionMember owner, in long value)
			{
				owner.PlayerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionMember owner, out long value)
			{
				value = owner.PlayerId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionMember_003C_003EIsLeader_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionMember, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionMember owner, in bool value)
			{
				owner.IsLeader = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionMember owner, out bool value)
			{
				value = owner.IsLeader;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionMember_003C_003EIsFounder_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionMember, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionMember owner, in bool value)
			{
				owner.IsFounder = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionMember owner, out bool value)
			{
				value = owner.IsFounder;
			}
		}

		private class VRage_Game_MyObjectBuilder_FactionMember_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FactionMember>
		{
			private sealed override object CreateInstance()
			{
				return default(MyObjectBuilder_FactionMember);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FactionMember CreateInstance()
			{
				return (MyObjectBuilder_FactionMember)(object)default(MyObjectBuilder_FactionMember);
			}

			MyObjectBuilder_FactionMember IActivator<MyObjectBuilder_FactionMember>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long PlayerId;

		[ProtoMember(4)]
		public bool IsLeader;

		[ProtoMember(7)]
		public bool IsFounder;
	}
}
