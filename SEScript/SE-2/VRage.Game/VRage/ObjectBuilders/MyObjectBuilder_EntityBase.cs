using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_EntityBase : MyObjectBuilder_Base
	{
		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityBase owner, in long value)
			{
				owner.EntityId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityBase owner, out long value)
			{
				value = owner.EntityId;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPersistentFlags_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, MyPersistentEntityFlags2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityBase owner, in MyPersistentEntityFlags2 value)
			{
				owner.PersistentFlags = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityBase owner, out MyPersistentEntityFlags2 value)
			{
				value = owner.PersistentFlags;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityBase owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityBase owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EPositionAndOrientation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityBase owner, in MyPositionAndOrientation? value)
			{
				owner.PositionAndOrientation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityBase owner, out MyPositionAndOrientation? value)
			{
				value = owner.PositionAndOrientation;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EComponentContainer_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, MyObjectBuilder_ComponentContainer>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityBase owner, in MyObjectBuilder_ComponentContainer value)
			{
				owner.ComponentContainer = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityBase owner, out MyObjectBuilder_ComponentContainer value)
			{
				value = owner.ComponentContainer;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EEntityDefinitionId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EntityBase, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EntityBase owner, in SerializableDefinitionId? value)
			{
				owner.EntityDefinitionId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EntityBase owner, out SerializableDefinitionId? value)
			{
				value = owner.EntityDefinitionId;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EntityBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EntityBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EntityBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_ObjectBuilders_MyObjectBuilder_EntityBase_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EntityBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EntityBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EntityBase CreateInstance()
			{
				return new MyObjectBuilder_EntityBase();
			}

			MyObjectBuilder_EntityBase IActivator<MyObjectBuilder_EntityBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public long EntityId;

		[ProtoMember(4)]
		public MyPersistentEntityFlags2 PersistentFlags;

		[ProtoMember(7)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public string Name;

		[ProtoMember(10)]
		public MyPositionAndOrientation? PositionAndOrientation;

		[ProtoMember(13)]
		[DefaultValue(null)]
		[Serialize(MyObjectFlags.DefaultZero)]
		public MyObjectBuilder_ComponentContainer ComponentContainer;

		[ProtoMember(16)]
		[DefaultValue(null)]
		[NoSerialize]
		public SerializableDefinitionId? EntityDefinitionId;

		public bool ShouldSerializePositionAndOrientation()
		{
			return PositionAndOrientation.HasValue;
		}

		public bool ShouldSerializeComponentContainer()
		{
			if (ComponentContainer != null && ComponentContainer.Components != null)
			{
				return ComponentContainer.Components.Count > 0;
			}
			return false;
		}

		public bool ShouldSerializeEntityDefinitionId()
		{
			return false;
		}

		/// <summary>
		/// Remaps this entity's entityId to a new value.
		/// If there are cross-referenced between different entities in this object builder, the remapHelper should be used to rememeber these
		/// references and retrieve them.
		/// </summary>
		public virtual void Remap(IMyRemapHelper remapHelper)
		{
			EntityId = remapHelper.RemapEntityId(EntityId);
		}
	}
}
