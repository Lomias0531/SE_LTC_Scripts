using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SessionComponentContractSystem : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContractSystem_003C_003EInactiveContracts_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContractSystem, MySerializableList<MyObjectBuilder_Contract>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystem owner, in MySerializableList<MyObjectBuilder_Contract> value)
			{
				owner.InactiveContracts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystem owner, out MySerializableList<MyObjectBuilder_Contract> value)
			{
				value = owner.InactiveContracts;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContractSystem_003C_003EActiveContracts_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SessionComponentContractSystem, MySerializableList<MyObjectBuilder_Contract>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystem owner, in MySerializableList<MyObjectBuilder_Contract> value)
			{
				owner.ActiveContracts = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystem owner, out MySerializableList<MyObjectBuilder_Contract> value)
			{
				value = owner.ActiveContracts;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContractSystem_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContractSystem_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContractSystem_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystem, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystem owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystem owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContractSystem_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystem, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystem owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystem owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContractSystem_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SessionComponentContractSystem, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SessionComponentContractSystem owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SessionComponentContractSystem owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SessionComponentContractSystem_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SessionComponentContractSystem>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentContractSystem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SessionComponentContractSystem CreateInstance()
			{
				return new MyObjectBuilder_SessionComponentContractSystem();
			}

			MyObjectBuilder_SessionComponentContractSystem IActivator<MyObjectBuilder_SessionComponentContractSystem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))]
		[DynamicObjectBuilder(false)]
		[XmlArrayItem(Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_Contract>))]
		[NoSerialize]
		public MySerializableList<MyObjectBuilder_Contract> InactiveContracts;

		[ProtoMember(3)]
		[Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))]
		[DynamicObjectBuilder(false)]
		[XmlArrayItem(Type = typeof(MyAbstractXmlSerializer<MyObjectBuilder_Contract>))]
		[NoSerialize]
		public MySerializableList<MyObjectBuilder_Contract> ActiveContracts;
	}
}
