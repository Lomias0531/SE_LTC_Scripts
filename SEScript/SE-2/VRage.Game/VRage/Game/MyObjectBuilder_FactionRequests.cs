using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public struct MyObjectBuilder_FactionRequests
	{
		protected class VRage_Game_MyObjectBuilder_FactionRequests_003C_003EFactionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRequests, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionRequests owner, in long value)
			{
				owner.FactionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionRequests owner, out long value)
			{
				value = owner.FactionId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionRequests_003C_003EFactionRequests_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionRequests, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionRequests owner, in List<long> value)
			{
				owner.FactionRequests = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionRequests owner, out List<long> value)
			{
				value = owner.FactionRequests;
			}
		}

		private class VRage_Game_MyObjectBuilder_FactionRequests_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FactionRequests>
		{
			private sealed override object CreateInstance()
			{
				return default(MyObjectBuilder_FactionRequests);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FactionRequests CreateInstance()
			{
				return (MyObjectBuilder_FactionRequests)(object)default(MyObjectBuilder_FactionRequests);
			}

			MyObjectBuilder_FactionRequests IActivator<MyObjectBuilder_FactionRequests>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		public long FactionId;

		[ProtoMember(13)]
		public List<long> FactionRequests;
	}
}
