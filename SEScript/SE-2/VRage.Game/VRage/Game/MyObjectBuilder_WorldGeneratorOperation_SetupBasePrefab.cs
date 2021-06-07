using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("SetupBasePrefab")]
	public class MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab : MyObjectBuilder_WorldGeneratorOperation
	{
		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003EPrefabFile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, in string value)
			{
				owner.PrefabFile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, out string value)
			{
				value = owner.PrefabFile;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003EOffset_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, SerializableVector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, in SerializableVector3 value)
			{
				owner.Offset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, out SerializableVector3 value)
			{
				value = owner.Offset;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003EAsteroidName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, in string value)
			{
				owner.AsteroidName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, out string value)
			{
				value = owner.AsteroidName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003EBeaconName_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, in string value)
			{
				owner.BeaconName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, out string value)
			{
				value = owner.BeaconName;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorOperation_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab();
			}

			MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab IActivator<MyObjectBuilder_WorldGeneratorOperation_SetupBasePrefab>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(148)]
		[XmlAttribute]
		public string PrefabFile;

		[ProtoMember(151)]
		public SerializableVector3 Offset;

		[ProtoMember(154)]
		[XmlAttribute]
		public string AsteroidName;

		[ProtoMember(157)]
		[XmlAttribute]
		public string BeaconName;

		public bool ShouldSerializeOffset()
		{
			return Offset != new SerializableVector3(0f, 0f, 0f);
		}

		public bool ShouldSerializeBeaconName()
		{
			return !string.IsNullOrEmpty(BeaconName);
		}
	}
}
