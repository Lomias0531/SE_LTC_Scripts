using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.ObjectBuilders
{
	[ProtoContract]
	public struct SerializableDefinitionId
	{
		protected class VRage_ObjectBuilders_SerializableDefinitionId_003C_003ETypeId_003C_003EAccessor : IMemberAccessor<SerializableDefinitionId, MyObjectBuilderType>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDefinitionId owner, in MyObjectBuilderType value)
			{
				owner.TypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDefinitionId owner, out MyObjectBuilderType value)
			{
				value = owner.TypeId;
			}
		}

		protected class VRage_ObjectBuilders_SerializableDefinitionId_003C_003ESubtypeName_003C_003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDefinitionId owner, in string value)
			{
				owner.SubtypeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDefinitionId owner, out string value)
			{
				value = owner.SubtypeName;
			}
		}

		protected class VRage_ObjectBuilders_SerializableDefinitionId_003C_003ETypeIdStringAttribute_003C_003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDefinitionId owner, in string value)
			{
				owner.TypeIdStringAttribute = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDefinitionId owner, out string value)
			{
				value = owner.TypeIdStringAttribute;
			}
		}

		protected class VRage_ObjectBuilders_SerializableDefinitionId_003C_003ETypeIdString_003C_003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDefinitionId owner, in string value)
			{
				owner.TypeIdString = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDefinitionId owner, out string value)
			{
				value = owner.TypeIdString;
			}
		}

		protected class VRage_ObjectBuilders_SerializableDefinitionId_003C_003ESubtypeIdAttribute_003C_003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDefinitionId owner, in string value)
			{
				owner.SubtypeIdAttribute = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDefinitionId owner, out string value)
			{
				value = owner.SubtypeIdAttribute;
			}
		}

		protected class VRage_ObjectBuilders_SerializableDefinitionId_003C_003ESubtypeId_003C_003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDefinitionId owner, in string value)
			{
				owner.SubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDefinitionId owner, out string value)
			{
				value = owner.SubtypeId;
			}
		}

		protected class VRage_ObjectBuilders_SerializableDefinitionId_003C_003Em_binaryTypeId_003C_003EAccessor : IMemberAccessor<SerializableDefinitionId, ushort>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDefinitionId owner, in ushort value)
			{
				owner.m_binaryTypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDefinitionId owner, out ushort value)
			{
				value = owner.m_binaryTypeId;
			}
		}

		protected class VRage_ObjectBuilders_SerializableDefinitionId_003C_003Em_binarySubtypeId_003C_003EAccessor : IMemberAccessor<SerializableDefinitionId, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SerializableDefinitionId owner, in MyStringHash value)
			{
				owner.m_binarySubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SerializableDefinitionId owner, out MyStringHash value)
			{
				value = owner.m_binarySubtypeId;
			}
		}

		private class VRage_ObjectBuilders_SerializableDefinitionId_003C_003EActor : IActivator, IActivator<SerializableDefinitionId>
		{
			private sealed override object CreateInstance()
			{
				return default(SerializableDefinitionId);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override SerializableDefinitionId CreateInstance()
			{
				return (SerializableDefinitionId)(object)default(SerializableDefinitionId);
			}

			SerializableDefinitionId IActivator<SerializableDefinitionId>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[XmlIgnore]
		[NoSerialize]
		public MyObjectBuilderType TypeId;

		[XmlIgnore]
		[NoSerialize]
		public string SubtypeName;

		[ProtoMember(1)]
		[XmlAttribute("Type")]
		[NoSerialize]
		public string TypeIdStringAttribute
		{
			get
			{
				if (TypeId.IsNull)
				{
					return "(null)";
				}
				return TypeId.ToString();
			}
			set
			{
				if (value != null)
				{
					TypeIdString = value;
				}
			}
		}

		[ProtoMember(4)]
		[XmlElement("TypeId")]
		[NoSerialize]
		public string TypeIdString
		{
			get
			{
				if (TypeId.IsNull)
				{
					return "(null)";
				}
				return TypeId.ToString();
			}
			set
			{
				TypeId = MyObjectBuilderType.ParseBackwardsCompatible(value);
			}
		}

		[ProtoMember(7)]
		[XmlAttribute("Subtype")]
		[NoSerialize]
		public string SubtypeIdAttribute
		{
			get
			{
				return SubtypeName;
			}
			set
			{
				SubtypeName = value;
			}
		}

		[ProtoMember(10)]
		[NoSerialize]
		public string SubtypeId
		{
			get
			{
				return SubtypeName;
			}
			set
			{
				SubtypeName = value;
			}
		}

		[Serialize]
		private ushort m_binaryTypeId
		{
			get
			{
				return ((MyRuntimeObjectBuilderId)TypeId).Value;
			}
			set
			{
				TypeId = (MyObjectBuilderType)new MyRuntimeObjectBuilderId(value);
			}
		}

		[Serialize]
		private MyStringHash m_binarySubtypeId
		{
			get
			{
				return MyStringHash.TryGet(SubtypeId);
			}
			set
			{
				SubtypeName = value.String;
			}
		}

		public bool ShouldSerializeTypeIdString()
		{
			return false;
		}

		public bool ShouldSerializeSubtypeId()
		{
			return false;
		}

		public SerializableDefinitionId(MyObjectBuilderType typeId, string subtypeName)
		{
			TypeId = typeId;
			SubtypeName = subtypeName;
		}

		public override string ToString()
		{
			return $"{TypeId}/{SubtypeName}";
		}

		public bool IsNull()
		{
			return TypeId.IsNull;
		}
	}
}
