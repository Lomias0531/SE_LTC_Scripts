using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
	/// <summary>
	/// Entry used to store player to factions visibility
	/// </summary>
	[ProtoContract]
	public struct MyObjectBuilder_FactionsVisEntry
	{
		protected class VRage_Game_MyObjectBuilder_FactionsVisEntry_003C_003EPlayerId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionsVisEntry, ulong>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionsVisEntry owner, in ulong value)
			{
				owner.PlayerId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionsVisEntry owner, out ulong value)
			{
				value = owner.PlayerId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionsVisEntry_003C_003ESerialId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionsVisEntry, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionsVisEntry owner, in int value)
			{
				owner.SerialId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionsVisEntry owner, out int value)
			{
				value = owner.SerialId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FactionsVisEntry_003C_003EDiscoveredFactions_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FactionsVisEntry, List<long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FactionsVisEntry owner, in List<long> value)
			{
				owner.DiscoveredFactions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FactionsVisEntry owner, out List<long> value)
			{
				value = owner.DiscoveredFactions;
			}
		}

		private class VRage_Game_MyObjectBuilder_FactionsVisEntry_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FactionsVisEntry>
		{
			private sealed override object CreateInstance()
			{
				return default(MyObjectBuilder_FactionsVisEntry);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FactionsVisEntry CreateInstance()
			{
				return (MyObjectBuilder_FactionsVisEntry)(object)default(MyObjectBuilder_FactionsVisEntry);
			}

			MyObjectBuilder_FactionsVisEntry IActivator<MyObjectBuilder_FactionsVisEntry>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public ulong PlayerId;

		[ProtoMember(3)]
		public int SerialId;

		[ProtoMember(5)]
		public List<long> DiscoveredFactions;
	}
}
