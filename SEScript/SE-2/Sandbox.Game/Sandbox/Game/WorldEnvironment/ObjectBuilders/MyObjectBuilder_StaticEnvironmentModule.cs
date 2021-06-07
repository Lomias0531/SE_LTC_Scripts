using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.ObjectBuilders
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("Sandbox.Game.XmlSerializers")]
	public class MyObjectBuilder_StaticEnvironmentModule : MyObjectBuilder_EnvironmentModuleBase
	{
		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule_003C_003EDisabledItems_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, HashSet<int>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in HashSet<int> value)
			{
				owner.DisabledItems = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out HashSet<int> value)
			{
				value = owner.DisabledItems;
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule_003C_003EBoxes_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, List<SerializableOrientedBoundingBoxD>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in List<SerializableOrientedBoundingBoxD> value)
			{
				owner.Boxes = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out List<SerializableOrientedBoundingBoxD> value)
			{
				value = owner.Boxes;
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule_003C_003EMinScanned_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in int value)
			{
				owner.MinScanned = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out int value)
			{
				value = owner.MinScanned;
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_StaticEnvironmentModule, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_StaticEnvironmentModule owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_StaticEnvironmentModule owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class Sandbox_Game_WorldEnvironment_ObjectBuilders_MyObjectBuilder_StaticEnvironmentModule_003C_003EActor : IActivator, IActivator<MyObjectBuilder_StaticEnvironmentModule>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_StaticEnvironmentModule();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_StaticEnvironmentModule CreateInstance()
			{
				return new MyObjectBuilder_StaticEnvironmentModule();
			}

			MyObjectBuilder_StaticEnvironmentModule IActivator<MyObjectBuilder_StaticEnvironmentModule>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public HashSet<int> DisabledItems;

		[Nullable]
		[ProtoMember(4)]
		public List<SerializableOrientedBoundingBoxD> Boxes;

		[ProtoMember(7)]
		public int MinScanned = 15;

		public MyObjectBuilder_StaticEnvironmentModule()
		{
			DisabledItems = new HashSet<int>();
			Boxes = new List<SerializableOrientedBoundingBoxD>();
		}
	}
}
