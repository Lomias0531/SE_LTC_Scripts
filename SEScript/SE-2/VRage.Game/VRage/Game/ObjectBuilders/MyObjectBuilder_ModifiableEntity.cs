using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ModifiableEntity : MyObjectBuilder_EntityBase
	{
		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003EAssetModifiers_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ModifiableEntity, List<SerializableDefinitionId>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in List<SerializableDefinitionId> value)
			{
				owner.AssetModifiers = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out List<SerializableDefinitionId> value)
			{
				value = owner.AssetModifiers;
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003EEntityId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in long value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out long value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003EPersistentFlags_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPersistentFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, MyPersistentEntityFlags2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in MyPersistentEntityFlags2 value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out MyPersistentEntityFlags2 value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003EName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003EPositionAndOrientation_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPositionAndOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in MyPositionAndOrientation? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out MyPositionAndOrientation? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003EComponentContainer_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003EEntityDefinitionId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityDefinitionId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ModifiableEntity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ModifiableEntity owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ModifiableEntity owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_MyObjectBuilder_ModifiableEntity_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ModifiableEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ModifiableEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ModifiableEntity CreateInstance()
			{
				return new MyObjectBuilder_ModifiableEntity();
			}

			MyObjectBuilder_ModifiableEntity IActivator<MyObjectBuilder_ModifiableEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1, IsRequired = false)]
		[XmlArrayItem("AssetModifier")]
		[Serialize(MyObjectFlags.DefaultZero)]
		public List<SerializableDefinitionId> AssetModifiers;
	}
}
