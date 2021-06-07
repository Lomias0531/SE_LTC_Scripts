using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_SharedStorageComponent : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EExistingFieldsAndStaticAttribute_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, bool>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, bool> value)
			{
				owner.ExistingFieldsAndStaticAttribute = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, bool> value)
			{
				value = owner.ExistingFieldsAndStaticAttribute;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EBoolStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, bool>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, bool> value)
			{
				owner.BoolStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, bool> value)
			{
				value = owner.BoolStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EIntStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, int> value)
			{
				owner.IntStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, int> value)
			{
				value = owner.IntStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003ELongStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, long>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, long> value)
			{
				owner.LongStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, long> value)
			{
				value = owner.LongStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EStringStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, string>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, string> value)
			{
				owner.StringStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, string> value)
			{
				value = owner.StringStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EFloatStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, float>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, float> value)
			{
				owner.FloatStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, float> value)
			{
				value = owner.FloatStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EVector3DStorage_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDictionary<string, SerializableVector3D>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDictionary<string, SerializableVector3D> value)
			{
				owner.Vector3DStorage = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDictionary<string, SerializableVector3D> value)
			{
				value = owner.Vector3DStorage;
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_SharedStorageComponent, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_SharedStorageComponent owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_SharedStorageComponent owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_ObjectBuilders_Components_MyObjectBuilder_SharedStorageComponent_003C_003EActor : IActivator, IActivator<MyObjectBuilder_SharedStorageComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_SharedStorageComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_SharedStorageComponent CreateInstance()
			{
				return new MyObjectBuilder_SharedStorageComponent();
			}

			MyObjectBuilder_SharedStorageComponent IActivator<MyObjectBuilder_SharedStorageComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public SerializableDictionary<string, bool> ExistingFieldsAndStaticAttribute = new SerializableDictionary<string, bool>();

		public SerializableDictionary<string, bool> BoolStorage = new SerializableDictionary<string, bool>();

		public SerializableDictionary<string, int> IntStorage = new SerializableDictionary<string, int>();

		public SerializableDictionary<string, long> LongStorage = new SerializableDictionary<string, long>();

		public SerializableDictionary<string, string> StringStorage = new SerializableDictionary<string, string>();

		public SerializableDictionary<string, float> FloatStorage = new SerializableDictionary<string, float>();

		public SerializableDictionary<string, SerializableVector3D> Vector3DStorage = new SerializableDictionary<string, SerializableVector3D>();
	}
}
