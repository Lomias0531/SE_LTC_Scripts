using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.ObjectBuilders
{
	[ProtoContract]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public abstract class MyObjectBuilder_Base
	{
		protected class VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Base, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Base owner, in MyStringHash value)
			{
				owner.m_subtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Base owner, out MyStringHash value)
			{
				value = owner.m_subtypeId;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Base, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Base owner, in string value)
			{
				owner.m_subtypeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Base owner, out string value)
			{
				value = owner.m_subtypeName;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Base, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Base owner, in MyStringHash value)
			{
				owner.m_serializableSubtypeId = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Base owner, out MyStringHash value)
			{
				value = owner.m_serializableSubtypeId;
			}
		}

		protected class VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Base, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Base owner, in string value)
			{
				owner.SubtypeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Base owner, out string value)
			{
				value = owner.SubtypeName;
			}
		}

		private MyStringHash m_subtypeId;

		private string m_subtypeName;

		[DefaultValue(0)]
		public MyStringHash SubtypeId => m_subtypeId;

		[Serialize]
		private MyStringHash m_serializableSubtypeId
		{
			get
			{
				return m_subtypeId;
			}
			set
			{
				m_subtypeId = value;
				m_subtypeName = value.String;
			}
		}

		[ProtoMember(1)]
		[DefaultValue(null)]
		[NoSerialize]
		public string SubtypeName
		{
			get
			{
				return m_subtypeName;
			}
			set
			{
				m_subtypeName = value;
				m_subtypeId = MyStringHash.GetOrCompute(value);
			}
		}

		[XmlIgnore]
		public MyObjectBuilderType TypeId => GetType();

		public bool ShouldSerializeSubtypeId()
		{
			return false;
		}

		public void Save(string filepath)
		{
			MyObjectBuilderSerializer.SerializeXML(filepath, compress: false, this);
		}

		public virtual MyObjectBuilder_Base Clone()
		{
			return MyObjectBuilderSerializer.Clone(this);
		}
	}
}
