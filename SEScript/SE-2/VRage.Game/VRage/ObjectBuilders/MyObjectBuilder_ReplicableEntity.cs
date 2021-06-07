using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.Utils;

namespace VRage.ObjectBuilders
{
	/// <summary>
	/// This object builder is old and is for "MyInventoryBagEntity". Do not use it as base class or for anything. It is here only for backward compatibility.
	/// </summary>
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_ReplicableEntity : MyObjectBuilder_EntityBase
	{
		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003ELinearVelocity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ReplicableEntity, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in SerializableVector3 value)
			{
				owner.LinearVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out SerializableVector3 value)
			{
				value = owner.LinearVelocity;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003EAngularVelocity_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ReplicableEntity, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in SerializableVector3 value)
			{
				owner.AngularVelocity = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out SerializableVector3 value)
			{
				value = owner.AngularVelocity;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003EMass_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ReplicableEntity, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in float value)
			{
				owner.Mass = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out float value)
			{
				value = owner.Mass;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003EEntityId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in long value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out long value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003EPersistentFlags_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPersistentFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, MyPersistentEntityFlags2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in MyPersistentEntityFlags2 value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out MyPersistentEntityFlags2 value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003EName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in string value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out string value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003EPositionAndOrientation_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPositionAndOrientation_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in MyPositionAndOrientation? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out MyPositionAndOrientation? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003EComponentContainer_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EComponentContainer_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in MyObjectBuilder_ComponentContainer value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out MyObjectBuilder_ComponentContainer value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003EEntityDefinitionId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityDefinitionId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_EntityBase*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_EntityBase*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ReplicableEntity, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ReplicableEntity owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ReplicableEntity owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_ObjectBuilders_MyObjectBuilder_ReplicableEntity_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ReplicableEntity>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ReplicableEntity();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ReplicableEntity CreateInstance()
			{
				return new MyObjectBuilder_ReplicableEntity();
			}

			MyObjectBuilder_ReplicableEntity IActivator<MyObjectBuilder_ReplicableEntity>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SerializableVector3 LinearVelocity;

		public SerializableVector3 AngularVelocity;

		public float Mass = 5f;
	}
}
