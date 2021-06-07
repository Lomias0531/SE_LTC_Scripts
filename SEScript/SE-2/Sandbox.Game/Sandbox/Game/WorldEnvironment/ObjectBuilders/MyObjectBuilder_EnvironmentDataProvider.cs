using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
	public class MyObjectBuilder_EnvironmentDataProvider : MyObjectBuilder_Base
	{
		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentDataProvider_003C_003EFace_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_EnvironmentDataProvider, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_EnvironmentDataProvider owner, in Base6Directions.Direction value)
			{
				owner.Face = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_EnvironmentDataProvider owner, out Base6Directions.Direction value)
			{
				value = owner.Face;
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentDataProvider_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDataProvider, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDataProvider owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDataProvider owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentDataProvider_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDataProvider, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDataProvider owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDataProvider owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentDataProvider_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDataProvider, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDataProvider owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDataProvider owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentDataProvider_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_EnvironmentDataProvider, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_EnvironmentDataProvider owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_EnvironmentDataProvider owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentDataProvider_003C_003EActor : IActivator, IActivator<MyObjectBuilder_EnvironmentDataProvider>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentDataProvider();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_EnvironmentDataProvider CreateInstance()
			{
				return new MyObjectBuilder_EnvironmentDataProvider();
			}

			MyObjectBuilder_EnvironmentDataProvider IActivator<MyObjectBuilder_EnvironmentDataProvider>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute("Face")]
		public Base6Directions.Direction Face;
	}
}
