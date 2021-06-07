using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
	public class MyObjectBuilder_EnvironmentModuleBase : MyObjectBuilder_Base
	{
		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentModuleBase_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentModuleBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentModuleBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentModuleBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentModuleBase_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentModuleBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentModuleBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentModuleBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentModuleBase_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentModuleBase, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentModuleBase owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentModuleBase owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentModuleBase_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentModuleBase, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentModuleBase owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentModuleBase owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentModuleBase_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentModuleBase>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentModuleBase();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EnvironmentModuleBase CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentModuleBase();
			}

			MyObjectBuilder_EnvironmentModuleBase IActivator<MyObjectBuilder_EnvironmentModuleBase>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
