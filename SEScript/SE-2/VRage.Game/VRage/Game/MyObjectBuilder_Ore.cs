using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_Ore : MyObjectBuilder_PhysicalObject
	{
		protected class VRage_Game_MyObjectBuilder_Ore_003C_003EMaterialTypeName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Ore, MyStringHash?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Ore owner, in MyStringHash? value)
			{
				owner.MaterialTypeName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Ore owner, out MyStringHash? value)
			{
				value = owner.MaterialTypeName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Ore_003C_003Em_materialName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Ore, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Ore owner, in string value)
			{
				owner.m_materialName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Ore owner, out string value)
			{
				value = owner.m_materialName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Ore_003C_003EFlags_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalObject_003C_003EFlags_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, MyItemFlags>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Ore owner, in MyItemFlags value)
			{
				Set(ref *(MyObjectBuilder_PhysicalObject*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Ore owner, out MyItemFlags value)
			{
				Get(ref *(MyObjectBuilder_PhysicalObject*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Ore_003C_003EDurabilityHP_003C_003EAccessor : VRage_Game_MyObjectBuilder_PhysicalObject_003C_003EDurabilityHP_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, float?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Ore owner, in float? value)
			{
				Set(ref *(MyObjectBuilder_PhysicalObject*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Ore owner, out float? value)
			{
				Get(ref *(MyObjectBuilder_PhysicalObject*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Ore_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Ore owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Ore owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Ore_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Ore owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Ore owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Ore_003C_003EMaterialNameString_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_Ore, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_Ore owner, in string value)
			{
				owner.MaterialNameString = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_Ore owner, out string value)
			{
				value = owner.MaterialNameString;
			}
		}

		protected class VRage_Game_MyObjectBuilder_Ore_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Ore owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Ore owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_Ore_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_Ore, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_Ore owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_Ore owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_Ore_003C_003EActor : IActivator, IActivator<MyObjectBuilder_Ore>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_Ore();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_Ore CreateInstance()
			{
				return new MyObjectBuilder_Ore();
			}

			MyObjectBuilder_Ore IActivator<MyObjectBuilder_Ore>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[Nullable]
		[XmlIgnore]
		public MyStringHash? MaterialTypeName;

		[XmlIgnore]
		[NoSerialize]
		private string m_materialName;

		[NoSerialize]
		[ProtoMember(1, IsRequired = false)]
		public string MaterialNameString
		{
			get
			{
				if (MaterialTypeName.HasValue && MaterialTypeName.Value.GetHashCode() != 0)
				{
					return MaterialTypeName.Value.String;
				}
				return m_materialName;
			}
			set
			{
				m_materialName = value;
			}
		}

		public string GetMaterialName()
		{
			if (!string.IsNullOrEmpty(m_materialName))
			{
				return m_materialName;
			}
			if (MaterialTypeName.HasValue)
			{
				return MaterialTypeName.Value.String;
			}
			return string.Empty;
		}

		public bool HasMaterialName()
		{
			if (string.IsNullOrEmpty(m_materialName))
			{
				if (MaterialTypeName.HasValue)
				{
					return MaterialTypeName.Value.GetHashCode() != 0;
				}
				return false;
			}
			return true;
		}

		public override MyObjectBuilder_Base Clone()
		{
			MyObjectBuilder_Ore obj = MyObjectBuilderSerializer.Clone(this) as MyObjectBuilder_Ore;
			obj.MaterialTypeName = MaterialTypeName;
			return obj;
		}
	}
}
