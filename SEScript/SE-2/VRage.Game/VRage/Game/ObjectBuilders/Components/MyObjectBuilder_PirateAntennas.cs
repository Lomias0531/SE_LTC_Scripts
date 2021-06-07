using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_PirateAntennas : MyObjectBuilder_SessionComponent
	{
		[ProtoContract]
		public class MyPirateDrone
		{
			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003EMyPirateDrone_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyPirateDrone, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPirateDrone owner, in long value)
				{
					owner.EntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPirateDrone owner, out long value)
				{
					value = owner.EntityId;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003EMyPirateDrone_003C_003EAntennaEntityId_003C_003EAccessor : IMemberAccessor<MyPirateDrone, long>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPirateDrone owner, in long value)
				{
					owner.AntennaEntityId = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPirateDrone owner, out long value)
				{
					value = owner.AntennaEntityId;
				}
			}

			protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003EMyPirateDrone_003C_003EDespawnTimer_003C_003EAccessor : IMemberAccessor<MyPirateDrone, int>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyPirateDrone owner, in int value)
				{
					owner.DespawnTimer = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyPirateDrone owner, out int value)
				{
					value = owner.DespawnTimer;
				}
			}

			private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003EMyPirateDrone_003C_003EActor : IActivator, IActivator<MyPirateDrone>
			{
				private sealed override object CreateInstance()
				{
					return new MyPirateDrone();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyPirateDrone CreateInstance()
				{
					return new MyPirateDrone();
				}

				MyPirateDrone IActivator<MyPirateDrone>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			[XmlAttribute("EntityId")]
			public long EntityId;

			[ProtoMember(4)]
			[XmlAttribute("AntennaEntityId")]
			public long AntennaEntityId;

			[ProtoMember(7)]
			[XmlAttribute("DespawnTimer")]
			public int DespawnTimer;
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003EPiratesIdentity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PirateAntennas, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PirateAntennas owner, in long value)
			{
				owner.PiratesIdentity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PirateAntennas owner, out long value)
			{
				value = owner.PiratesIdentity;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003EDrones_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_PirateAntennas, MyPirateDrone[]>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_PirateAntennas owner, in MyPirateDrone[] value)
			{
				owner.Drones = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_PirateAntennas owner, out MyPirateDrone[] value)
			{
				value = owner.Drones;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PirateAntennas owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PirateAntennas owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PirateAntennas owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PirateAntennas owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PirateAntennas owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PirateAntennas owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PirateAntennas owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PirateAntennas owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_PirateAntennas, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_PirateAntennas owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_PirateAntennas owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_PirateAntennas_003C_003EActor : IActivator, IActivator<MyObjectBuilder_PirateAntennas>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_PirateAntennas();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_PirateAntennas CreateInstance()
			{
				return new MyObjectBuilder_PirateAntennas();
			}

			MyObjectBuilder_PirateAntennas IActivator<MyObjectBuilder_PirateAntennas>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(10)]
		public long PiratesIdentity;

		[ProtoMember(13)]
		public MyPirateDrone[] Drones;
	}
}
