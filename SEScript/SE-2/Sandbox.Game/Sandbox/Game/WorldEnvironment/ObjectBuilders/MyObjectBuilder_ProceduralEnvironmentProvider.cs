using ProtoBuf;
using System.Collections.Generic;
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
	public class MyObjectBuilder_ProceduralEnvironmentProvider : MyObjectBuilder_EnvironmentDataProvider
	{
		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider_003C_003ESectors_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, List<MyObjectBuilder_ProceduralEnvironmentSector>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, in List<MyObjectBuilder_ProceduralEnvironmentSector> value)
			{
				owner.Sectors = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, out List<MyObjectBuilder_ProceduralEnvironmentSector> value)
			{
				value = owner.Sectors;
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider_003C_003EFace_003C_003EAccessor : Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_EnvironmentDataProvider_003C_003EFace_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, Base6Directions.Direction>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, in Base6Directions.Direction value)
			{
				Set(ref *(MyObjectBuilder_EnvironmentDataProvider*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, out Base6Directions.Direction value)
			{
				Get(ref *(MyObjectBuilder_EnvironmentDataProvider*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_ProceduralEnvironmentProvider, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_ProceduralEnvironmentProvider owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_ProceduralEnvironmentProvider_003C_003EActor : IActivator, IActivator<MyObjectBuilder_ProceduralEnvironmentProvider>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_ProceduralEnvironmentProvider();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_ProceduralEnvironmentProvider CreateInstance()
			{
				return new MyObjectBuilder_ProceduralEnvironmentProvider();
			}

			MyObjectBuilder_ProceduralEnvironmentProvider IActivator<MyObjectBuilder_ProceduralEnvironmentProvider>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlElement("Sector")]
		public List<MyObjectBuilder_ProceduralEnvironmentSector> Sectors = new List<MyObjectBuilder_ProceduralEnvironmentSector>();
	}
}
