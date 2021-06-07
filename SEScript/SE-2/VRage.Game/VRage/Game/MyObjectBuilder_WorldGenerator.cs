using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
	[ProtoContract]
	[MyObjectBuilderDefinition(null, null)]
	[XmlSerializerAssembly("VRage.Game.XmlSerializers")]
	public class MyObjectBuilder_WorldGenerator : MyObjectBuilder_SessionComponent
	{
		protected class VRage_Game_MyObjectBuilder_WorldGenerator_003C_003EMarkedAreas_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGenerator, HashSet<EmptyArea>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGenerator owner, in HashSet<EmptyArea> value)
			{
				owner.MarkedAreas = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGenerator owner, out HashSet<EmptyArea> value)
			{
				value = owner.MarkedAreas;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGenerator_003C_003EDeletedAreas_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGenerator, HashSet<EmptyArea>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGenerator owner, in HashSet<EmptyArea> value)
			{
				owner.DeletedAreas = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGenerator owner, out HashSet<EmptyArea> value)
			{
				value = owner.DeletedAreas;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGenerator_003C_003EExistingObjectsSeeds_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGenerator, HashSet<MyObjectSeedParams>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_WorldGenerator owner, in HashSet<MyObjectSeedParams> value)
			{
				owner.ExistingObjectsSeeds = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_WorldGenerator owner, out HashSet<MyObjectSeedParams> value)
			{
				value = owner.ExistingObjectsSeeds;
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGenerator_003C_003Em_subtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGenerator owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGenerator owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGenerator_003C_003Em_subtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_subtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGenerator owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGenerator owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGenerator_003C_003EDefinition_003C_003EAccessor : VRage_Game_MyObjectBuilder_SessionComponent_003C_003EDefinition_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, SerializableDefinitionId?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGenerator owner, in SerializableDefinitionId? value)
			{
				Set(ref *(MyObjectBuilder_SessionComponent*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGenerator owner, out SerializableDefinitionId? value)
			{
				Get(ref *(MyObjectBuilder_SessionComponent*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGenerator_003C_003Em_serializableSubtypeId_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003Em_serializableSubtypeId_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, MyStringHash>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGenerator owner, in MyStringHash value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGenerator owner, out MyStringHash value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		protected class VRage_Game_MyObjectBuilder_WorldGenerator_003C_003ESubtypeName_003C_003EAccessor : VRage_ObjectBuilders_MyObjectBuilder_Base_003C_003ESubtypeName_003C_003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGenerator, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Set(ref MyObjectBuilder_WorldGenerator owner, in string value)
			{
				Set(ref *(MyObjectBuilder_Base*)(&owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe sealed override void Get(ref MyObjectBuilder_WorldGenerator owner, out string value)
			{
				Get(ref *(MyObjectBuilder_Base*)(&owner), out value);
			}
		}

		private class VRage_Game_MyObjectBuilder_WorldGenerator_003C_003EActor : IActivator, IActivator<MyObjectBuilder_WorldGenerator>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_WorldGenerator();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_WorldGenerator CreateInstance()
			{
				return new MyObjectBuilder_WorldGenerator();
			}

			MyObjectBuilder_WorldGenerator IActivator<MyObjectBuilder_WorldGenerator>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(25)]
		public HashSet<EmptyArea> MarkedAreas = new HashSet<EmptyArea>();

		[ProtoMember(28)]
		public HashSet<EmptyArea> DeletedAreas = new HashSet<EmptyArea>();

		[ProtoMember(31)]
		public HashSet<MyObjectSeedParams> ExistingObjectsSeeds = new HashSet<MyObjectSeedParams>();
	}
}
