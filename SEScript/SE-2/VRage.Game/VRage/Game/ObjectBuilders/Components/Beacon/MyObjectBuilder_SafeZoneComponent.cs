using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Beacon
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SafeZoneComponent : MyObjectBuilder_ComponentBase
	{
		protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent_003C_003EUpkeepTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneComponent, double>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SafeZoneComponent owner, in double value)
			{
				owner.UpkeepTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SafeZoneComponent owner, out double value)
			{
				value = owner.UpkeepTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent_003C_003EActivating_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneComponent, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SafeZoneComponent owner, in bool value)
			{
				owner.Activating = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SafeZoneComponent owner, out bool value)
			{
				value = owner.Activating;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent_003C_003EActivationTime_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneComponent, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SafeZoneComponent owner, in long value)
			{
				owner.ActivationTime = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SafeZoneComponent owner, out long value)
			{
				value = owner.ActivationTime;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent_003C_003ESafeZoneOb_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneComponent, MyObjectBuilder_EntityBase>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SafeZoneComponent owner, in MyObjectBuilder_EntityBase value)
			{
				owner.SafeZoneOb = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SafeZoneComponent owner, out MyObjectBuilder_EntityBase value)
			{
				value = owner.SafeZoneOb;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SafeZoneComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SafeZoneComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SafeZoneComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SafeZoneComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SafeZoneComponent CreateInstance()
			{
				return new MyObjectBuilder_SafeZoneComponent();
			}

			MyObjectBuilder_SafeZoneComponent IActivator<MyObjectBuilder_SafeZoneComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public double UpkeepTime;

		[ProtoMember(4)]
		public bool Activating;

		[ProtoMember(7)]
		public long ActivationTime;

		[ProtoMember(10)]
		[XmlElement(Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_EntityBase>))]
		[DynamicObjectBuilder(false)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_EntityBase SafeZoneOb;
	}
}
