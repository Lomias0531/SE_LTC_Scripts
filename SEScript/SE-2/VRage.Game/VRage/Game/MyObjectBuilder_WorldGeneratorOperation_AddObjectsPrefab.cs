using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("AddObjectsPrefab")]
	public class MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab : MyObjectBuilder_WorldGeneratorOperation
	{
		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab_003C_003EPrefabFile_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, in string value)
			{
				owner.PrefabFile = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, out string value)
			{
				value = owner.PrefabFile;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorOperation_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab();
			}

			MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab IActivator<MyObjectBuilder_WorldGeneratorOperation_AddObjectsPrefab>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(133)]
		[XmlAttribute]
		public string PrefabFile;
	}
}
