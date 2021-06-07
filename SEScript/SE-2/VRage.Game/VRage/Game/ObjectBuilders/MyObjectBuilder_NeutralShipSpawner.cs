using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_NeutralShipSpawner : MyObjectBuilder_SessionComponent
	{
		[ProtoContract]
		public struct ShipTimePair
		{
			protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003EShipTimePair_003C_003EEntityIds_003C_003EAccessor : IMemberAccessor<ShipTimePair, List<long>>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ShipTimePair owner, in List<long> value)
				{
					owner.EntityIds = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ShipTimePair owner, out List<long> value)
				{
					value = owner.EntityIds;
				}
			}

			protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003EShipTimePair_003C_003ETimeTicks_003C_003EAccessor : IMemberAccessor<ShipTimePair, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref ShipTimePair owner, in long value)
				{
					owner.TimeTicks = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref ShipTimePair owner, out long value)
				{
					value = owner.TimeTicks;
				}
			}

			private class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003EShipTimePair_003C_003EActor : IActivator, IActivator<ShipTimePair>
			{
				private sealed override object CreateInstance()
				{
					return default(ShipTimePair);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override ShipTimePair CreateInstance()
				{
					return (ShipTimePair)(object)default(ShipTimePair);
				}

				ShipTimePair IActivator<ShipTimePair>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public List<long> EntityIds;

			public long TimeTicks;
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003EShipsInProgress_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, List<ShipTimePair>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in List<ShipTimePair> value)
			{
				owner.ShipsInProgress = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out List<ShipTimePair> value)
			{
				value = owner.ShipsInProgress;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_NeutralShipSpawner, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_NeutralShipSpawner owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_NeutralShipSpawner owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_NeutralShipSpawner_003C_003EActor : IActivator, IActivator<MyObjectBuilder_NeutralShipSpawner>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_NeutralShipSpawner();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_NeutralShipSpawner CreateInstance()
			{
				return new MyObjectBuilder_NeutralShipSpawner();
			}

			MyObjectBuilder_NeutralShipSpawner IActivator<MyObjectBuilder_NeutralShipSpawner>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public List<ShipTimePair> ShipsInProgress;
	}
}
