using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Station
	{
		protected class VRage_Game_MyObjectBuilder_Station_003C_003EId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out long value)
			{
				value = owner.Id;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EPosition_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, SerializableVector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in SerializableVector3D value)
			{
				owner.Position = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out SerializableVector3D value)
			{
				value = owner.Position;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EUp_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in SerializableVector3 value)
			{
				owner.Up = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out SerializableVector3 value)
			{
				value = owner.Up;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EForward_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in SerializableVector3 value)
			{
				owner.Forward = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out SerializableVector3 value)
			{
				value = owner.Forward;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EStationType_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, MyStationTypeEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in MyStationTypeEnum value)
			{
				owner.StationType = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out MyStationTypeEnum value)
			{
				value = owner.StationType;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EIsDeepSpaceStation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in bool value)
			{
				owner.IsDeepSpaceStation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out bool value)
			{
				value = owner.IsDeepSpaceStation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EStationEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in long value)
			{
				owner.StationEntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out long value)
			{
				value = owner.StationEntityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EFactionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in long value)
			{
				owner.FactionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out long value)
			{
				value = owner.FactionId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EPrefabName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in string value)
			{
				owner.PrefabName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out string value)
			{
				value = owner.PrefabName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003ESafeZoneEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in long value)
			{
				owner.SafeZoneEntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out long value)
			{
				value = owner.SafeZoneEntityId;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EStoreItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, List<MyObjectBuilder_StoreItem>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in List<MyObjectBuilder_StoreItem> value)
			{
				owner.StoreItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out List<MyObjectBuilder_StoreItem> value)
			{
				value = owner.StoreItems;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Station_003C_003EIsOnPlanetWithAtmosphere_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Station, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Station owner, in bool value)
			{
				owner.IsOnPlanetWithAtmosphere = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Station owner, out bool value)
			{
				value = owner.IsOnPlanetWithAtmosphere;
			}
		}

		private class VRage_Game_MyObjectBuilder_Station_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Station>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Station();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Station CreateInstance()
			{
				return new MyObjectBuilder_Station();
			}

			MyObjectBuilder_Station IActivator<MyObjectBuilder_Station>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long Id;

		[ProtoMember(2)]
		public SerializableVector3D Position;

		[ProtoMember(3)]
		public SerializableVector3 Up;

		[ProtoMember(5)]
		public SerializableVector3 Forward;

		[ProtoMember(7)]
		public MyStationTypeEnum StationType;

		[ProtoMember(9)]
		public bool IsDeepSpaceStation;

		[ProtoMember(11)]
		public long StationEntityId;

		[ProtoMember(13)]
		public long FactionId;

		[ProtoMember(15)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string PrefabName;

		[ProtoMember(19)]
		public long SafeZoneEntityId;

		[ProtoMember(21)]
		public List<MyObjectBuilder_StoreItem> StoreItems;

		[ProtoMember(23)]
		public bool IsOnPlanetWithAtmosphere;
	}
}
