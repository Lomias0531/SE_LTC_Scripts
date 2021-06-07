using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Medieval.ObjectBuilders
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlType("TestTrees")]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_WorldGeneratorOperation_TestTrees : MyObjectBuilder_WorldGeneratorOperation
	{
		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_TestTrees_003C_003EFactionTag_003C_003EAccessor : VRage_Game_MyObjectBuilder_WorldGeneratorOperation_003C_003EFactionTag_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_TestTrees, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, in string value)
			{
				Set(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, out string value)
			{
				Get(ref *(MyObjectBuilder_WorldGeneratorOperation*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_TestTrees_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_TestTrees, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_TestTrees_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_TestTrees, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_TestTrees_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_TestTrees, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_TestTrees_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorOperation_TestTrees, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGeneratorOperation_TestTrees owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorOperation_TestTrees_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorOperation_TestTrees>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_TestTrees();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGeneratorOperation_TestTrees CreateInstance()
			{
				return new MyObjectBuilder_WorldGeneratorOperation_TestTrees();
			}

			MyObjectBuilder_WorldGeneratorOperation_TestTrees IActivator<MyObjectBuilder_WorldGeneratorOperation_TestTrees>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
